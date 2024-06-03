//***********************************************************************
// Assembly         : Orbita.VAFunciones
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using CIDARMTWrapper;
using Orbita.Utiles;
using Orbita.VAComun;


namespace Orbita.VAFunciones
{
    /// <summary>
    /// Clase que ejecuta continuamente el
    /// </summary>
    internal static class OCRRuntime
    {
        #region Atributo(s)
        /// <summary>
        /// Thread de ejecución continua del módulo OCR
        /// </summary>
        public static ThreadEjecucionOCR ThreadEjecucionOCR;

        /// <summary>
        /// Indica si alguna función de visión ha demandado el uso del OCR
        /// </summary>
        private static bool UsoDemandado = false;

        /// <summary>
        /// Parámetros de configuración del OCR
        /// </summary>
        private static ConfiguracionOCR Configuracion;
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Construye los objetos
        /// </summary>
        private static void Constructor()
        {
            ThreadEjecucionOCR = new ThreadEjecucionOCR("OCR", ThreadPriority.Normal);
        }

        /// <summary>
        /// Destruye los objetos
        /// </summary>
        private static void Destructor()
        {
            ThreadEjecucionOCR.Stop();
            ThreadEjecucionOCR = null;
        }

        /// <summary>
        /// Carga las propiedades de la base de datos
        /// </summary>
        private static void Inicializar()
        {
            try
            {
                try
                {
                    OCRRuntime.Configuracion = (ConfiguracionOCR)(new ConfiguracionOCR().CargarDatos());
                }
                catch (FileNotFoundException exception)
                {
                    LogsRuntime.Error(ModulosFunciones.OCRContainer, "OCR", exception);
                    OCRRuntime.Configuracion = new ConfiguracionOCR();
                    OCRRuntime.Configuracion.Guardar();
                }

                // Inicializamos el motor de búsqueda de OCR
                int id = MTInterface.Init(OCRRuntime.Configuracion.AvCharHeight, OCRRuntime.Configuracion.DuplicateLines, OCRRuntime.Configuracion.Trace, OCRRuntime.Configuracion.TraceWrapper);
                // Almacenamos el valor de incio
                if (id == 1)
                {
                    LogsRuntime.Debug(ModulosFunciones.OCRContainer, "OCR", "Iniciado correctamente");
                }
                else
                {
                    LogsRuntime.Error(ModulosFunciones.OCRContainer, "OCR", "Error de inicialización");
                }
                // Si hemos de limitar los núcleos los limitamos
                if (OCRRuntime.Configuracion.LimitCores)
                {
                    id = MTInterface.LimitCores(OCRRuntime.Configuracion.Cores);
                    // Almacenamos el valor de incio
                    if (id == 0)
                    {
                        LogsRuntime.Error(ModulosFunciones.OCRContainer, "OCR", "Error limitando Cores");
                    }
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosFunciones.OCRContainer, "OCR", exception);
            }

            ThreadEjecucionOCR.Start();
        }

        /// <summary>
        /// Finaliza la ejecución
        /// </summary>
        private static void Finalizar()
        {
            ThreadEjecucionOCR.Stop(1000);

            // Liberamos memoria reservada para la libreria de OCR, cuando termina de procesar las imagenes
            MTInterface.QueryEnd();
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Resetea la función de visión OCR
        /// </summary>
        public static void Reset()
        {
            // Detenemos el Hilo
            ThreadEjecucionOCR.Pause();
            
            int id = MTInterface.Reset();
            // Almacenamos el valor de incio
            if (id == 0)
            {
                LogsRuntime.Debug(ModulosFunciones.OCRContainer, "OCR", "Error reseteando el wrapper");
                // Lo realizamos a lo bestia como antes
                // Liberamos memoria reservada para la libreria de OCR, cuando termina de procesar las imagenes
                MTInterface.QueryEnd();
                // Inicializamos el motor de búsqueda de OCR
                MTInterface.Init(OCRRuntime.Configuracion.AvCharHeight, OCRRuntime.Configuracion.DuplicateLines, OCRRuntime.Configuracion.Trace, OCRRuntime.Configuracion.TraceWrapper);
            }
            // Reiniciamos el hilo
            ThreadEjecucionOCR.Resume();
        }

        /// <summary>
        /// Se demanda el uso del OCR, por lo que se necesita iniciar las librerías correspondientes
        /// </summary>
        public static void DemandaUso()
        {
            if (!UsoDemandado)
            {
                UsoDemandado = true;

                // Constructor de las funciones OCR
                OCRRuntime.Constructor();

                // Inicialización de las funciones OCR
                OCRRuntime.Inicializar();
            }
        }

        /// <summary>
        /// Se elimina la demanda el uso del OCR, por lo que se necesita finalizar las librerías correspondientes
        /// </summary>
        public static void FinDemandaUso()
        {
            if (UsoDemandado)
            {
                UsoDemandado = false;

                // Finalización de las funciones OCR
                OCRRuntime.Finalizar();

                // Destructor de las funciones OCR
                OCRRuntime.Destructor();
            }
        }
        #endregion
    }

    /// <summary>
    /// Clase que ejecuta el reconocimiento de los codigos en un thread
    /// </summary>
    internal class ThreadEjecucionOCR : ThreadOrbita
    {
        #region Atributo(s)
        /// <summary>
        /// Indica si se ha de finalizar la ejecución del thread
        /// </summary>
        public bool Finalizar;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ThreadEjecucionOCR(string codigo)
            : base(codigo)
        {
            this.Finalizar = false;
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ThreadEjecucionOCR(string codigo, ThreadPriority threadPriority)
            : base(codigo, 1, threadPriority)
        {
            this.Finalizar = false;
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Ejecuta el callback en el mismo thread que la Applicación principal
        /// </summary>
        private void CallBack(CallBackResultadoParcial callBack, EventArgsResultadoParcial argumentoEvento)
        {
            if (!ThreadRuntime.EjecucionEnTrheadPrincipal())
            {
                ThreadRuntime.SincronizarConThreadPrincipal(new DelegadoCallBack(this.CallBack), new object[] { callBack, argumentoEvento });
                return;
            }

            // Llamada al callback
            if (callBack != null)
            {
                callBack(this, argumentoEvento);
            }
        }
        #endregion

        #region Definición de delegado(s)
        /// <summary>
        /// Delegado de ejecución del callback
        /// </summary>
        /// <param name="callBack"></param>
        /// <param name="argumentoEvento"></param>
        private delegate void DelegadoCallBack(CallBackResultadoParcial callBack, EventArgsResultadoParcial argumentoEvento);
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Método a heredar para implementar la ejecución del thread.
        /// Este método se está ejecutando en un bucle. Para salir del bucle hay que devolver finalize a true.
        /// </summary>
        protected override void Ejecucion(out bool finalize)
        {
            finalize = this.Finalizar;

            try
            {
                if (!this.Finalizar)
                {
                    NLInfo element = MTInterface.GetFirstElement;

                    if (element != null)
                    {
                        // Guardamos la traza
                        LogsRuntime.Debug(ModulosFunciones.Vision, this.Codigo, "Ejecución de la función LPR " + this.Codigo);

                        CodeInfo info = element.GetFirstItem;
                        InfoInspeccionOCR infoInspeccionOCR = (InfoInspeccionOCR)element.ImageInformation.GetObject; // Obtengo la información de entrada de la inspección
                        ResultadoOCR resultadoParcial = new ResultadoOCR(info, element.ImageInformation.GetTimestamp); // Obtengo el resultado parcial
                        infoInspeccionOCR.Resultados = resultadoParcial; // Añado el resultado a la información de la inspección

                        EventArgsResultadoParcial argumentoEvento = new EventArgsResultadoParcial(infoInspeccionOCR); // Creación del argumento del evento
                        CallBackResultadoParcial callBack = infoInspeccionOCR.Info.CallBackResultadoParcial; // Obtención del callback
                        this.CallBack(callBack, argumentoEvento); // Llamada al callback

                        // Guardamos en la traza el resultado
                        String codeNumber = info.GetCodeNumber;
                        String fcode = info.ToString();
                        fcode += "(FC:" + Math.Round(info.GetGlobalConfidence, 1) + ")";
                        fcode += "(FI:" + Math.Round(info.GetExtraInfoConfidence, 1) + ")";
                        fcode += "(AC:" + info.GetAverageCharacterHeigth + ")";
                        fcode += "(T:" + info.GetProcessingTime + ")";
                        LogsRuntime.Debug(ModulosFunciones.OCRContainer, this.Codigo, "Resultado recibido:" + fcode);

                        if (info != null)
                        {
                            info.Dispose();
                        }
                        element.ImageInformation.ClearImage();
                        element.ImageInformation.Dispose();
                        element.Dispose();
                    }
                }
            }
            catch (ThreadAbortException)
            {
                // TODO Do nothing
            }
            catch (ApplicationException)
            {
                // TODO Do nothing
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosFunciones.OCRContainer, this.Codigo, exception);
            }
        }
        #endregion
    }

    #region Clase VisionOCRContainerFunction
    /// <summary>
    /// Clase que realizara la función de reconocimiento de OCR
    /// </summary>
    public class VisionOCRContainerFunction : VisionFunctionQueue
    {
        #region Constante(s)
        /// <summary>
        /// Número máximo de inspecciones en la cola de ejecución
        /// </summary>
        private const int MaxInspeccionesEnCola = 100;
        #endregion

        #region Atributo(s)
        /// <summary>
        /// Siguiente parametros de entrada a procesar en el OCR
        /// </summary>
        private ParametrosOCR ParametrosOCR;
        /// <summary>
        /// Lista de imagenes pendientes de ejecutar
        /// </summary>
        private List<Bitmap> ListaImagenesPendientes;
        /// <summary>
        /// Siguiente imágen a procesar en el OCR
        /// </summary>
        private BitmapImage Imagen;
        /// <summary>
        /// Siguiente ruta de imágen a procesar en el OCR
        /// </summary>
        private string RutaImagen;
        ///// <summary>
        ///// Hilo para ir procesando las imagenes pendientes
        ///// </summary>
        //private OHilo HiloProcesarPendientes;
        #endregion

        #region Constructor
        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public VisionOCRContainerFunction(string codFuncionVision)
            : base(codFuncionVision)
        {
            try
            {
                // Demanda del uso de OCR
                OCRRuntime.DemandaUso();

                this.Valido = true;
                this.ParametrosOCR = new ParametrosOCR();

                // Cargamos valores de la base de datos
                DataTable dtFuncionVision = AppBD.GetFuncionVision(this.Codigo);
                if (dtFuncionVision.Rows.Count == 1)
                {
                    this.ParametrosOCR.Altura = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_Altura"], -1, 10000, -1);
                    this.ParametrosOCR.ActivadoRangoAlturas = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_ActivadoRangoAlturas"], 0, 1, 0);
                    this.ParametrosOCR.AlturaMin = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_AlturaMin"], 1, 10000, 30);
                    this.ParametrosOCR.AlturaMax = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_AlturaMax"], 1, 10000, 60);
                    this.ParametrosOCR.VectorAlturas = new int[2] { this.ParametrosOCR.AlturaMin, this.ParametrosOCR.AlturaMax };
                    this.ParametrosOCR.TimeOut = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_TimeOut"], 0, 1000000, 0);
                    this.ParametrosOCR.ActivadaAjusteCorreccion = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_ActivadaAjusteCorrecion"], 0, 1, 0);
                    this.ParametrosOCR.CoeficienteHorizontal = (float)App.EvaluaDecimal(dtFuncionVision.Rows[0]["NL_CoeficienteHorizontal"], -100000, +100000, 0);
                    this.ParametrosOCR.CoeficienteVertical = (float)App.EvaluaDecimal(dtFuncionVision.Rows[0]["NL_CoeficienteVertical"], -100000, +100000, 0);
                    this.ParametrosOCR.CoeficienteRadial = (float)App.EvaluaDecimal(dtFuncionVision.Rows[0]["NL_CoeficienteRadial"], -100000, +100000, 0);
                    this.ParametrosOCR.AnguloRotacion = (float)App.EvaluaDecimal(dtFuncionVision.Rows[0]["NL_AnguloRotacion"], -100000, +100000, 0);
                    this.ParametrosOCR.Distancia = (float)App.EvaluaDecimal(dtFuncionVision.Rows[0]["NL_Distancia"], 0, +100000, 0);
                    this.ParametrosOCR.CoordIzq = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_CoordIzq"], 0, 10000, 0);
                    this.ParametrosOCR.CoordArriba = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_CoordArriba"], 0, 10000, 0);
                    this.ParametrosOCR.AlturaVentanaBusqueda = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_AlturaVentanaBusqueda"], 0, 10000, 0);
                    this.ParametrosOCR.AnchuraVentanaBusqueda = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_AnchuraVentanaBusqueda"], 0, 10000, 0);
                    this.ParametrosOCR.ActivadaMasInformacion = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_ActivadaMasInformacion"], 0, 10000, 1);
                    // Columnas añadidas posteriormente
                    try
                    {
                        this.ParametrosOCR.Escala = (float)App.EvaluaDecimal(dtFuncionVision.Rows[0]["NL_Escala"], 0, +100000, 0);
                        this.ParametrosOCR.Param1 = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_Param1"], 0, 10000, 1);
                        this.ParametrosOCR.Param2 = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_Param2"], 0, 10000, 1);
                    }
                    catch (Exception ex)
                    {
                        LogsRuntime.Error(ModulosFunciones.OCRContainer, "FuncionOCR", ex);
                    }
                }
                this.ListaImagenesPendientes = new List<Bitmap>();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosFunciones.OCRContainer, "FuncionOCR", exception);
            }
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Cargamos la configuracion de los parametros pasados por parámetro en caso de necesidad
        /// </summary>
        private void EstablecerConfiguracion(ParametrosOCR parametros)
        {
            // Ejecutamos la configuración
            MTInterface.SetConfiguration(parametros.TimeOut, parametros.ActivadaAjusteCorreccion,
                    parametros.Distancia, parametros.CoeficienteVertical, parametros.CoeficienteHorizontal, parametros.CoeficienteRadial,
                    parametros.AnguloRotacion, parametros.CoordIzq, parametros.CoordArriba,
                    parametros.AnchuraVentanaBusqueda, parametros.AlturaVentanaBusqueda, parametros.ActivadoRangoAlturas,
                    parametros.VectorAlturas, parametros.ActivadaMasInformacion,parametros.Escala,parametros.Param1,parametros.Param2);
        }
        /// <summary>
        /// Comprueba que el archivo no este en uso
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private  bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        /// <summary>
        /// Ejecuta lo que ha quedado pendiente
        /// </summary>
        //private void ProcesarImagenesPendientes()
        //{
        //    // Guardamos la traza
        //    LogsRuntime.Debug(ModulosFunciones.OCRContainer, this.Codigo, "Se procede a ejecutar las imagenes pendientes" + this.Codigo);

        //    // Si tenemos imagenes pendientes las iremos procesando
        //    if (this.ListaImagenesPendientes.Count > 0)
        //    {
        //        VariableRuntime.SetValue(this.CodVariableEnEjecucion, true, this.Codigo, "Ejecución pendientes");
        //        bool sinIncluir = true;
        //        if (MTInterface.IsRunning())
        //        {
        //            if (this.ListaImagenesPendientes.Count != 0)
        //            {
        //                foreach (Bitmap imagen in this.ListaImagenesPendientes)
        //                {
        //                    sinIncluir = true;
        //                    while (sinIncluir)
        //                    {
        //                        if (MTInterface.GetUsedCores < MTInterface.GetLicensedCores)
        //                        //if (MTInterface.GetQueueSize < MaxInspeccionesEnCola)
        //                        {
        //                            // Se aumenta el número de inspecciones en cola
        //                            this.ContInspeccionesEnCola++;
        //                            this.IndiceFotografia++;

        //                            InfoInspeccionOCR infoInspeccionOCR = new InfoInspeccionOCR(
        //                                    //this.Imagen,
        //                                    null,
        //                                    new InfoImagenOCR(this.IdEjecucionActual, this.Codigo, this.IndiceFotografia, DateTime.Now, AñadirResultadoParcial),
        //                                    this.ParametrosOCR,
        //                                    new ResultadoOCR());

        //                            // Se carga la configuración
        //                            this.EstablecerConfiguracion((ParametrosOCR)this.ParametrosOCR);

        //                            // Se carga la imagen
        //                            object info = infoInspeccionOCR;
        //                            MTInterface.Add(imagen, info);
        //                            sinIncluir = false;
        //                        }
        //                        Thread.Sleep(1);
        //                    }
        //                }
        //            }
        //        }
        //        this.ListaImagenesPendientes.Clear();
        //        VariableRuntime.SetValue(this.CodVariableEnEjecucion, false, this.Codigo, "Ejecución pendientes");
        //    }
        //    // Guardamos la traza
        //    LogsRuntime.Debug(ModulosFunciones.OCRContainer, this.Codigo, "Finaliza la ejecución de las imagenes pendientes " + this.Codigo + " realizado con éxito");
        //}
        #endregion

        #region Método publicos
        /// <summary>
        /// Realiza un reset de la función de visión de OCR que esta en ejecución
        /// </summary>
        public void Reset()
        {
            // Guardamos la traza
            LogsRuntime.Debug(ModulosFunciones.OCRContainer, this.Codigo, "Se procede a resetear la función de visión " + this.Codigo);

            OCRRuntime.Reset();

            // Ya no existen inspecciones pendientes
            this.ContInspeccionesEnCola = 0;
            this.IndiceFotografia = 0;

            // Se finaliza la ejecución de la función de visión
            this.FuncionEjecutada();

            // Guardamos la traza
            LogsRuntime.Debug(ModulosFunciones.OCRContainer, this.Codigo, "Reset de la función de visión " + this.Codigo + " realizado con éxito");
        }
        /// <summary>
        /// Ejecuta lo que ha quedado pendiente en un hilo
        /// </summary>
        //public void EjecutarPendientes()
        //{
        //    // Creamos el hilo para procesar las imagenes pendientes
        //    HiloProcesarPendientes = new OHilo(new ThreadStart(this.ProcesarImagenesPendientes), "Procesa las imagenes que han quedado pendientes", ThreadPriority.Normal, true, true);
        //    // iniciamos el hilo
        //    HiloProcesarPendientes.Iniciar();
        //}
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Ejecución de la función de OCR de forma encolada
        /// </summary>
        /// <returns></returns>
        protected override bool EjecucionEncolada()
        {
            bool resultado = false;
            resultado = base.EjecucionEncolada();

            // Se añade una nueva inspección a la cola
            try
            {
                // Añadimos inspecciones de forma bloqueante
                if (MTInterface.IsRunning())
                {
                    if (MTInterface.GetQueueSize < MaxInspeccionesEnCola)
                    {
                        // Se aumenta el número de inspecciones en cola
                        this.ContInspeccionesEnCola++;
                        this.IndiceFotografia++;

                        // Creamos el objeto con la información que nos interesa, no le pasamos la imagen para que no crezca la memoria
                        InfoInspeccionOCR infoInspeccionOCR = new InfoInspeccionOCR(
                                null,
                                new InfoImagenOCR(this.IdEjecucionActual, this.Codigo, this.IndiceFotografia, DateTime.Now, AñadirResultadoParcial),
                                this.ParametrosOCR,
                                new ResultadoOCR());

                        // Se carga la configuración
                        this.EstablecerConfiguracion((ParametrosOCR)this.ParametrosOCR);

                        // Se carga la imagen
                        object info = infoInspeccionOCR;

                        //  Si tenemos ruta y imagenes es null, pasamos la ruta y sino viceversa
                        if (this.Imagen == null)
                        {
                            if (File.Exists(this.RutaImagen))
                            {
                                FileInfo fileInfo = new FileInfo(this.RutaImagen);
                                while (this.IsFileLocked(fileInfo))
                                {
                                    // Esperamos a poder utilizar el archivo
                                    Application.DoEvents();
                                    Thread.Sleep(1);
                                }
                                MTInterface.Add(this.RutaImagen, info);
                            }
                        }
                        else
                        {
                            MTInterface.Add(this.Imagen.Image, info);
                        }
                    }
                    else
                    {
                        // Temporal hasta que lo soluccionen
                        //this.ListaImagenesPendientes.Add(this.Imagen.Image);
                        LogsRuntime.Info(ModulosFunciones.OCRContainer, "FuncionOCR: Sobrepasado el limite de imagenes en cola", "");
                    }
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosFunciones.OCRContainer, "FuncionOCR", exception);
            }

            return resultado;
        }

        /// <summary>
        /// Indica que hay inspecciones pendientes de ejecución
        /// </summary>
        /// <returns></returns>
        public override bool HayInspeccionesPendientes()
        {
            bool resultado = base.HayInspeccionesPendientes();

            resultado |= (MTInterface.GetNumberOfElements > 0);
            resultado |= this.ContInspeccionesEnCola > 0;

            return resultado;
        }

        /// <summary>
        /// Función para la actualización de parámetros de entrada
        /// </summary>
        /// <param name="ParamName">Nombre identificador del parámetro</param>
        /// <param name="ParamValue">Nuevo valor del parámetro</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override bool SetEntrada(string codigo, object valor, EnumTipoDato tipoVariable)
        {
            bool resultado = false;

            try
            {
                if (tipoVariable == EnumTipoDato.Imagen)
                {
                    this.Imagen = (BitmapImage)valor;
                }
                else if (codigo == "Parametros")
                {
                    this.ParametrosOCR = (ParametrosOCR)valor;
                }
                else if (codigo == "RutaImagen")
                {
                    this.RutaImagen = (string)valor;
                }
                else
                {
                    throw new Exception("Error en la asignación del parámetro '" + codigo + "' a la función '" + this.Codigo + "'. No se admite este tipo de parámetros.");
                }
                resultado = true;
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosFunciones.OCRContainer, this.Codigo, exception);
            }
            return resultado;
        }       
        #endregion
    }
    #endregion

    /// <summary>
    /// Clase que contiene la información referente a la inspección
    /// </summary>
    /// <typeparam name="TInfo"></typeparam>
    /// <typeparam name="TParametros"></typeparam>
    /// <typeparam name="TResultados"></typeparam>
    public class InfoInspeccionOCR : InfoInspeccion<BitmapImage, InfoImagenOCR, ParametrosOCR, ResultadoOCR>
    {
        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public InfoInspeccionOCR()
            : base()
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="info"></param>
        /// <param name="parametros"></param>
        /// <param name="resultados"></param>
        public InfoInspeccionOCR(BitmapImage imagen, InfoImagenOCR info, ParametrosOCR parametros, ResultadoOCR resultados)
            : base(imagen, info, parametros, resultados)
        {
        }
        #endregion
    }

    /// <summary>
    /// Esta clase contendra la información que queremos pasar con la imagen al OCR para recogerla cuando tengamos el resultado
    /// </summary>
    public class InfoImagenOCR
    {
        #region Atributo(s)
        /// <summary>
        /// Contiene la información de la identificacion a la que pertenece
        /// </summary>
        public long IdEjecucionActual;
        /// <summary>
        /// Contiene la información de la cámara que se le pasa en la imagen
        /// </summary>
        public string CodigoCamara;
        /// <summary>
        /// Contiene el indice de la imagen añadida
        /// </summary>
        public int IndiceImagen;
        /// <summary>
        /// Contiene la fecha exacta en la que se adquirio la imagen
        /// </summary>
        public DateTime MomentoImagen;
        /// <summary>
        /// CallBack donde mandar el resultado parcial
        /// </summary>
        internal CallBackResultadoParcial CallBackResultadoParcial;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor sin parametros
        /// </summary>
        public InfoImagenOCR()
        {
            this.IdEjecucionActual = 0;
            this.CodigoCamara = "";
            this.IndiceImagen = 0;
            this.MomentoImagen = DateTime.Now;
        }
        /// <summary>
        /// Constructor con parametros
        /// </summary>
        /// <param name="camara">camara que adquiere la imagen</param>
        public InfoImagenOCR(long idEjecucionActual,string codCamara, int indice, DateTime momentoImagen, CallBackResultadoParcial callBackResultadoParcial)
        {
            this.IdEjecucionActual = idEjecucionActual;
            this.CodigoCamara = codCamara;
            this.IndiceImagen = indice;
            this.MomentoImagen = momentoImagen;
            this.CallBackResultadoParcial = callBackResultadoParcial;
        }
        #endregion

    }

    /// <summary>
    /// Clase que contiene los parametros de configuracion del OCR
    /// </summary>
    public class ParametrosOCR
    {
        #region Atributo(s)
        /// <summary>
        /// Altura del caracter
        /// </summary>
        public int Altura;
        /// <summary>
        /// Para tener en cuenta el rango de alturas
        /// </summary>
        public int ActivadoRangoAlturas;
        /// <summary>
        /// Altura Minima
        /// </summary>
        public int AlturaMin;
        /// <summary>
        /// Altura Máxima
        /// </summary>
        public int AlturaMax;
        /// <summary>
        /// Vector que contendra las alturas
        /// </summary>
        public Int32[] VectorAlturas;
        /// <summary>
        /// timeout de ejecución
        /// </summary>
        public int TimeOut;
        /// <summary>
        /// activa el ajuste de la correcion
        /// </summary>
        public int ActivadaAjusteCorreccion;
        /// <summary>
        /// Coeficiente de Correccion de la perspectiva Horizontal
        /// </summary>
        public float CoeficienteHorizontal;
        /// <summary>
        /// Coeficiente de Correccion de la perspectiva Vertical
        /// </summary>
        public float CoeficienteVertical;
        /// <summary>
        /// Coeficiente para el ojo de pez
        /// </summary>
        public float CoeficienteRadial;
        /// <summary>
        /// Coeficiente de la corrección del ángulo de rotación
        /// </summary>
        public float AnguloRotacion;
        /// <summary>
        /// Distancia al código desde la camara en metros
        /// </summary>
        public float Distancia;
        /// <summary>
        /// Coordenada izquierda ventana
        /// </summary>
        public int CoordIzq;
        /// <summary>
        /// Coordenada superior ventana
        /// </summary>
        public int CoordArriba;
        /// <summary>
        /// Altura de la ventana de búsqueda
        /// </summary>
        public int AlturaVentanaBusqueda;
        /// <summary>
        /// Anchura de la ventana de búsqueda
        /// </summary>
        public int AnchuraVentanaBusqueda;
        /// <summary>
        /// Se le indica si se desea buscar más información extra en la imagén
        /// </summary>
        public int ActivadaMasInformacion;
        /// <summary>
        /// Escala 
        /// </summary>
        public float Escala;
        /// <summary>
        /// Parámetro1
        /// </summary>
        public int Param1;
        /// <summary>
        /// Parámetro2
        /// </summary>
        public int Param2;
        #endregion Campos

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase sin parámetros
        /// </summary>
        public ParametrosOCR()
        {
            this.Altura = -1;
            this.ActivadoRangoAlturas = 0;
            this.AlturaMin = 30;
            this.AlturaMax = 60;
            this.VectorAlturas = new Int32[2];
            this.VectorAlturas[0] = AlturaMin;
            this.VectorAlturas[1] = AlturaMax;
            this.TimeOut = 5000;
            this.ActivadaAjusteCorreccion = 0;
            this.CoeficienteHorizontal = 0;
            this.CoeficienteVertical = 0;
            this.CoeficienteRadial = 0;
            this.AnguloRotacion = 0;
            this.Distancia = 0;
            this.CoordArriba = 0;
            this.CoordIzq = 0;
            this.AlturaVentanaBusqueda = 0;
            this.AnchuraVentanaBusqueda = 0;
            this.ActivadaMasInformacion = 1;
            this.Escala = 0;
            this.Param1 = 0;
            this.Param2 = 0;
        }
        #endregion Constructores
    }

    /// <summary>
    /// Clase que contiene los resultados que se reciben del OCR para una imagen pasada anteriormente
    /// </summary>
    public class ResultadoOCR
    {
        #region Atributo(s)
        /// <summary>
        /// Contiene la cadena del codigo del contenedor
        /// </summary>
        public string CodigoContenedor;
        /// <summary>
        /// Contiene la información extra del código del contenedor
        /// </summary>
        public string ExtaInfoCodigo;
        /// <summary>
        /// Si esta verificado el digito de control
        /// </summary>
        public bool CodigoVerificado;
        /// <summary>
        /// Fiabilidad Código
        /// </summary>
        public int FiabilidadCodigo;
        /// <summary>
        /// Fiabilidad ExtraInfo
        /// </summary>
        public int FiabilidadExtraInfo;
        /// <summary>
        /// Altura letras
        /// </summary>
        public int AlturaLetrasCodigo;       
        /// <summary>
        /// Fecha en la que se encolo a la cola de OCR (dada por él)
        /// </summary>
        public DateTime FechaEncolamiento;
        /// <summary>
        /// Tiempo de proceso
        /// </summary>
        public int TiempoDeProceso;
        /// <summary>
        /// Contiene las fiabilidades de cada letra individual
        /// </summary>
        public int[] FiabilidadesLetras;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor sin parametros
        /// </summary>
        public ResultadoOCR()
        {
            this.CodigoContenedor = "";
            this.ExtaInfoCodigo = "";
            this.CodigoVerificado = false;
            this.FiabilidadCodigo = 0;
            this.FiabilidadExtraInfo = 0;
            this.AlturaLetrasCodigo = 0;
            this.TiempoDeProceso = 0;
            this.FechaEncolamiento = new DateTime();
            this.FiabilidadesLetras = new int[11];
            for (int i = 0; i < this.FiabilidadesLetras.Length; i++)
            {
                this.FiabilidadesLetras[i] = 0;
            }
        }
        /// <summary>
        /// Constructor con parametros
        /// </summary>
        public ResultadoOCR(CIDARMTWrapper.CodeInfo resultadoImagen, DateTime fechaEncola)
        {
            try
            {
                // Inicializamos el vector de fiabilidades de las letras
                this.FiabilidadesLetras = new int[11];
                for (int i = 0; i < this.FiabilidadesLetras.Length; i++)
                {
                    this.FiabilidadesLetras[i] = 0;
                }
                this.CodigoContenedor = (string)resultadoImagen.GetCodeNumber.ToString().Clone();
                // Si tenemos código identificado , obtenemos las fiabilidades de cada una de las letras
                if (this.CodigoContenedor != "")
                {
                    float[] fiabilidades = resultadoImagen.GetCharConfidence;
                    for (int i = 0; i < fiabilidades.Length; i++)
                    {
                        this.FiabilidadesLetras[i] = Convert.ToInt32(fiabilidades[i]);
                    }
                }
                // Si tenemos información extra la obtenemos
                if (resultadoImagen.GetExtraInfoCodeNumber != null)
                {
                    this.ExtaInfoCodigo = (string)resultadoImagen.GetExtraInfoCodeNumber.ToString().Clone();
                    this.FiabilidadExtraInfo = Convert.ToInt32((Convert.ToInt32(resultadoImagen.GetExtraInfoConfidence)).ToString().Clone());
                }
                else
                {
                    this.ExtaInfoCodigo = "";
                }
                // Obtenemos el resto de resultados independientes
                this.CodigoVerificado = Convert.ToBoolean((Convert.ToBoolean(resultadoImagen.IsCodeVerified)).ToString().Clone());
                this.FiabilidadCodigo = Convert.ToInt32((Convert.ToInt32(resultadoImagen.GetGlobalConfidence)).ToString().Clone());
                this.AlturaLetrasCodigo = Convert.ToInt32((Convert.ToInt32(resultadoImagen.GetAverageCharacterHeigth)).ToString().Clone());
                this.TiempoDeProceso = Convert.ToInt32((Convert.ToInt32(resultadoImagen.GetProcessingTime)).ToString().Clone());
                this.FechaEncolamiento = fechaEncola;
            }
            catch (Exception exception)
            {
                // En caso de que se produzca cualquier error no contemplado, descartaremos el resultado recibido permitiendo continuar la ejecución
                this.CodigoContenedor = "";
                this.ExtaInfoCodigo = "";
                this.CodigoVerificado = false;
                this.FiabilidadCodigo = 0;
                this.FiabilidadExtraInfo = 0;
                this.AlturaLetrasCodigo = 0;
                this.TiempoDeProceso = 0;
                this.FechaEncolamiento = new DateTime();
                this.FechaEncolamiento = DateTime.Now;
                for (int i = 0; i < this.FiabilidadesLetras.Length; i++)
                {
                    this.FiabilidadesLetras[i] = 0;
                }
                LogsRuntime.Info(ModulosFunciones.OCRContainer, "FuncionOCR:" + resultadoImagen.GetAverageCharacterHeigth.ToString()
                     + " " + resultadoImagen.GetGlobalConfidence.ToString()
                        + " " + resultadoImagen.GetExtraInfoConfidence.ToString(), exception);
            }
        }
        #endregion
    }

    /// <summary>
    /// Parámetros de la aplicación
    /// </summary>
    [Serializable]
    public class ConfiguracionOCR : OAlmacenXML
    {
        #region Atributo(s) estáticos
        /// <summary>
        /// Ruta por defecto del fichero xml de configuración
        /// </summary>
        public static string ConfFile = Path.Combine(RutaParametrizable.AppFolder, "ConfiguracionOCR.xml");
        #endregion

        #region Atributo(s)
        /// <summary>
        /// Constante que indica si se ha de realizar una traza del OCR
        /// </summary>
        public bool TraceWrapper = true;
        /// <summary>
        /// Constante que indica si se limitan los núcleos a utilizar
        /// </summary>
        public bool LimitCores = false;
        /// <summary>
        /// Constante que indica el número máximo de núcleos a utilizar
        /// </summary>
        public int Cores = 12;
        /// <summary>
        /// Constante de altura del caracter
        /// </summary>
        public int AvCharHeight = -1;
        /// <summary>
        /// Parámetro de cidar
        /// </summary>
        public int DuplicateLines = 0;
        /// <summary>
        /// Trazabilidad
        /// </summary>
        public int Trace = 1;
        #endregion

        #region Constructor
        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public ConfiguracionOCR()
            : base(ConfFile)
        {
        }

        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public ConfiguracionOCR(string rutaFichero)
            : base(rutaFichero)
        {
        }
        #endregion Constructor
    }
}