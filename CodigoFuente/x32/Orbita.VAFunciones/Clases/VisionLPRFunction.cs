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
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Orbita.Utiles;
using Orbita.VAComun;
using VPARMTWrapper;

namespace Orbita.VAFunciones
{
    /// <summary>
    /// Clase que ejecuta continuamente el
    /// </summary>
    internal static class LPRRuntime
    {
        #region Atributo(s)
        /// <summary>
        /// Thread de ejecución continua del módulo LPR
        /// </summary>
        public static ThreadEjecucionLPR ThreadEjecucionLPR;

        /// <summary>
        /// Indica si alguna función de visión ha demandado el uso del LPR
        /// </summary>
        private static bool UsoDemandado = false;

        /// <summary>
        /// Parámetros de configuración del LPR
        /// </summary>
        private static ConfiguracionLPR Configuracion;

        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Construye los objetos
        /// </summary>
        private static void Constructor()
        {
            ThreadEjecucionLPR = new ThreadEjecucionLPR("LPR", ThreadPriority.Normal);
        }

        /// <summary>
        /// Destruye los objetos
        /// </summary>
        private static void Destructor()
        {
            ThreadEjecucionLPR.Stop();
            ThreadEjecucionLPR = null;
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
                    LPRRuntime.Configuracion = (ConfiguracionLPR)(new ConfiguracionLPR().CargarDatos());
                }
                catch (FileNotFoundException exception)
                {
                    LogsRuntime.Error(ModulosFunciones.LPR, "LPR", exception);
                    LPRRuntime.Configuracion = new ConfiguracionLPR();
                    LPRRuntime.Configuracion.Guardar();
                }

                // Inicializamos el motor de búsqueda de LPR             
                int id = MTInterface.Init(LPRRuntime.Configuracion.CountryCode, LPRRuntime.Configuracion.AvCharHeight, LPRRuntime.Configuracion.DuplicateLines, LPRRuntime.Configuracion.Reordenar, LPRRuntime.Configuracion.filterColor, LPRRuntime.Configuracion.TraceVpar, LPRRuntime.Configuracion.TraceWrapper);
                // Almacenamos el valor de incio
                if (id == 1)
                {
                    LogsRuntime.Debug(ModulosFunciones.LPR, "LPR", "Iniciado correctamente");
                }
                else
                {
                    LogsRuntime.Error(ModulosFunciones.LPR, "LPR", "Error de inicialización");
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosFunciones.LPR, "LPR", exception);
            }

            ThreadEjecucionLPR.Start();
        }

        /// <summary>
        /// Finaliza la ejecución
        /// </summary>
        private static void Finalizar()
        {
            ThreadEjecucionLPR.Dispose(1000);

            // Liberamos memoria reservada para la libreria de LPR, cuando termina de procesar las imagenes
            MTInterface.QueryEnd();
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Resetea la función de visión LPR
        /// </summary>
        public static void Reset()
        {
            // Detenemos el hilo 
            ThreadEjecucionLPR.Pause();
            // Liberamos memoria reservada para la libreria de LPR, cuando termina de procesar las imagenes
            MTInterface.QueryEnd();
            // Inicializamos el motor de búsqueda de LPR
            MTInterface.Init(LPRRuntime.Configuracion.CountryCode, LPRRuntime.Configuracion.AvCharHeight, LPRRuntime.Configuracion.DuplicateLines, LPRRuntime.Configuracion.Reordenar, LPRRuntime.Configuracion.filterColor, LPRRuntime.Configuracion.TraceVpar, LPRRuntime.Configuracion.TraceWrapper);
            // Reiniciamos el hilo
            ThreadEjecucionLPR.Resume();
        }

        /// <summary>
        /// Se demanda el uso del LPR, por lo que se necesita iniciar las librerías correspondientes
        /// </summary>
        public static void DemandaUso()
        {
            if (!UsoDemandado)
            {
                UsoDemandado = true;

                // Constructor de las funciones LPR
                LPRRuntime.Constructor();

                // Inicialización de las funciones LPR
                LPRRuntime.Inicializar();
            }
        }

        /// <summary>
        /// Se elimina la demanda el uso del LPR, por lo que se necesita finalizar las librerías correspondientes
        /// </summary>
        public static void FinDemandaUso()
        {
            if (UsoDemandado)
            {
                UsoDemandado = false;

                // Finalización de las funciones LPR
                LPRRuntime.Finalizar();

                // Destructor de las funciones LPR
                LPRRuntime.Destructor();
            }
        }
        #endregion
    }

    /// <summary>
    /// Clase que ejecuta el reconocimiento de las matrículas en un thread
    /// </summary>
    internal class ThreadEjecucionLPR : ThreadOrbita
    {
        #region Atributo(s)
        /// <summary>
        /// Indica si se ha de finalizar la ejecución del thread
        /// </summary>
        public bool Finalizar;

        /// <summary>
        /// Cola de elementos a procesar en MTInterface
        /// </summary>
        private int QueueSize = -1;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ThreadEjecucionLPR(string codigo)
            : base(codigo)
        {
            this.Finalizar = false;
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ThreadEjecucionLPR(string codigo, ThreadPriority threadPriority)
            : base(codigo, 1, threadPriority)
        {
            this.Finalizar = false;
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Ejecuta el callback en el mismo thread que la Applicación principal
        /// </summary>
        private void CallBack(NLInfo element, PlateInfo plateInfo)
        {
            try
            {
                if ((element != null))
                {
                    InfoInspeccionLPR infoInspeccionLPR = (InfoInspeccionLPR)element.ImageInformation.GetObject; // Obtengo la información de entrada de la inspección
                    ResultadoLPR resultadoParcial = new ResultadoLPR(plateInfo, element.ImageInformation.GetTimestamp); // Obtengo el resultado parcial
                    infoInspeccionLPR.Resultados = resultadoParcial; // Añado el resultado a la información de la inspección

                    EventArgsResultadoParcial argumentoEvento = new EventArgsResultadoParcial(infoInspeccionLPR); // Creación del argumento del evento
                    CallBackResultadoParcial callBack = infoInspeccionLPR.Info.CallBackResultadoParcial; // Obtención del callback

                    // Llamada al callback
                    if (callBack != null)
                    {
                        // Llamada desde el thread principal
                        ThreadRuntime.SincronizarConThreadPrincipal(new CallBackResultadoParcial(callBack), new object[] { this, argumentoEvento });
                    }
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosFunciones.LPR, this.Codigo, exception);
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

        protected override void Ejecucion(out bool finalize)
        {
            finalize = this.Finalizar;

            if (!this.Finalizar)
            {
                // Guardamos la traza únicamente cuando hay un cambio en el tamaño de la cola de elementos a procesar en MTInterface
                if (this.QueueSize != MTInterface.GetQueueSize)
                {
                    LogsRuntime.Info(ModulosFunciones.LPR, this.Codigo, "Eliminada imagen de la cola VPAR. Total imágenes: " + MTInterface.GetQueueSize.ToString());
                    this.QueueSize = MTInterface.GetQueueSize;
                }

                // Consulta de resultados
                NLInfo element = null;
                do
                {
                    //Get firs element where all results are ready
                    element = MTInterface.GetFirstElement();
                    if (element != null)
                    {
                        PlateInfo info = element.GetFirstPlate;

                        this.CallBack(element, info); // Llamada al callback
                        if (info != null)
                        {
                            info.Dispose();
                        }
                        element.Dispose();

                        // Guardamos la traza
                        LogsRuntime.Debug(ModulosFunciones.LPR, this.Codigo, "Fin de ejecución de la función " + this.Codigo);
                    }
                }
                while (element != null);

            }
        }
        #endregion
    }

    /// <summary>
    /// Clase que realizara la función de reconocimiento de LPR
    /// </summary>
    public class VisionLPRFunction : VisionFunctionQueue
    {
        #region Constante(s)
        /// <summary>
        /// Número máximo de inspecciones en la cola de ejecución
        /// </summary>
        private const int MaxInspeccionesEnCola = 8;
        #endregion

        #region Atributo(s)
        /// <summary>
        /// Siguiente parametros de entrada a procesar en el LPR
        /// </summary>
        private ParametrosLPR ParametrosLPR;

        /// <summary>
        /// Siguiente imágen a procesar en el LPR
        /// </summary>
        private BitmapImage Imagen;

        /// <summary>
        /// Para ir alternando la configuración, si tiene distorisión tambien es útil combinarla con la configuración estandar
        /// </summary>
        private bool AlternarConfiguracion;

        /// <summary>
        /// Siguiente ruta de imágen a procesar en el LPR
        /// </summary>
        private string RutaImagen;
        #endregion

        #region Constructor
        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public VisionLPRFunction(string codFuncionVision)
            : base(codFuncionVision)
        {
            try
            {
                // Demanda del uso de LPR
                LPRRuntime.DemandaUso();

                this.Valido = true;
                this.ParametrosLPR = new ParametrosLPR();
                this.AlternarConfiguracion = false;

                // Cargamos valores de la base de datos
                DataTable dtFuncionVision = AppBD.GetFuncionVision(this.Codigo);
                if (dtFuncionVision.Rows.Count == 1)
                {
                    this.ParametrosLPR.Altura = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_Altura"], -1, 10000, -1);
                    this.ParametrosLPR.ActivadoRangoAlturas = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_ActivadoRangoAlturas"], 0, 1, 0);
                    this.ParametrosLPR.AlturaMin = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_AlturaMin"], 1, 10000, 30);
                    this.ParametrosLPR.AlturaMax = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_AlturaMax"], 1, 10000, 60);
                    this.ParametrosLPR.VectorAlturas = new int[2] { this.ParametrosLPR.AlturaMin, this.ParametrosLPR.AlturaMax };
                    this.ParametrosLPR.TimeOut = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_TimeOut"], 0, 1000000, 0);
                    this.ParametrosLPR.ActivadaAjusteCorreccion = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_ActivadaAjusteCorrecion"], 0, 1, 0);
                    this.ParametrosLPR.CoeficienteHorizontal = (float)App.EvaluaDecimal(dtFuncionVision.Rows[0]["NL_CoeficienteHorizontal"], -100000, +100000, 0);
                    this.ParametrosLPR.CoeficienteVertical = (float)App.EvaluaDecimal(dtFuncionVision.Rows[0]["NL_CoeficienteVertical"], -100000, +100000, 0);
                    this.ParametrosLPR.CoeficienteRadial = (float)App.EvaluaDecimal(dtFuncionVision.Rows[0]["NL_CoeficienteRadial"], -100000, +100000, 0);
                    this.ParametrosLPR.AnguloRotacion = (float)App.EvaluaDecimal(dtFuncionVision.Rows[0]["NL_AnguloRotacion"], -100000, +100000, 0);
                    this.ParametrosLPR.Distancia = (float)App.EvaluaDecimal(dtFuncionVision.Rows[0]["NL_Distancia"], 0, +100000, 0);
                    this.ParametrosLPR.CoordIzq = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_CoordIzq"], 0, 10000, 0);
                    this.ParametrosLPR.CoordArriba = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_CoordArriba"], 0, 10000, 0);
                    this.ParametrosLPR.AlturaVentanaBusqueda = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_AlturaVentanaBusqueda"], 0, 10000, 0);
                    this.ParametrosLPR.AnchuraVentanaBusqueda = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_AnchuraVentanaBusqueda"], 0, 10000, 0);
                    this.ParametrosLPR.ActivadaMasInformacion = App.EvaluaNumero(dtFuncionVision.Rows[0]["NL_ActivadaMasInformacion"], 0, 10000, 1);
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosFunciones.LPR, "FuncionLPR", exception);
            }
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Cargamos la configuracion de los parametros definidos como entrada, es más eficiente alternar con los de por defecto, para tener mejor tasa de acierto
        /// </summary>
        private void EstablecerConfiguracion(ParametrosLPR parametros,bool usarPorDefecto)
        {
            bool resultCode;
            if (usarPorDefecto)
            {
                // Ejecutamos la configuración por defecto
                resultCode = MTInterface.SetConfiguration(0,0,10,0,0,0,0,0,0,0,0,new int[3]);
            }
            else
            {
                // Ejecutamos la configuración
                resultCode = MTInterface.SetConfiguration(parametros.TimeOut, parametros.ActivadaAjusteCorreccion,
                    parametros.Distancia, parametros.CoeficienteVertical, parametros.CoeficienteHorizontal,
                    parametros.AnguloRotacion, parametros.CoordIzq, parametros.CoordArriba,
                    parametros.AnchuraVentanaBusqueda, parametros.AlturaVentanaBusqueda, parametros.ActivadoRangoAlturas,
                    parametros.VectorAlturas);
            }
            if (!resultCode)
            {
                LogsRuntime.Error(ModulosFunciones.LPR, "FuncionLPR", "Error al establecer la configuración del VPAR");
            }
        }
        /// <summary>
        /// Comprueba que el archivo no este en uso
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool IsFileLocked(FileInfo file)
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
        #endregion

        #region Método publicos
        /// <summary>
        /// Realiza un reset de la función de visión de LPR que esta en ejecución
        /// </summary>
        public void Reset()
        {
            // Guardamos la traza
            LogsRuntime.Debug(ModulosFunciones.LPR, this.Codigo, "Se procede a resetear la función de visión " + this.Codigo);

            LPRRuntime.Reset();

            // Ya no existen inspecciones pendientes
            this.ContInspeccionesEnCola = 0;
            this.IndiceFotografia = 0;

            // Se finaliza la ejecución de la función de visión
            this.FuncionEjecutada();

            // Guardamos la traza
            LogsRuntime.Debug(ModulosFunciones.LPR, this.Codigo, "Reset de la función de visión " + this.Codigo + " realizado con éxito");
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Ejecución de la función de Vision Pro de forma síncrona
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

                        InfoInspeccionLPR infoInspeccionLPR = new InfoInspeccionLPR(
                                this.Imagen,
                                new InfoImagenLPR(this.IdEjecucionActual,this.Codigo, this.IndiceFotografia, DateTime.Now, AñadirResultadoParcial),
                                this.ParametrosLPR,
                                new ResultadoLPR());

                        // Se carga la configuración
                        this.EstablecerConfiguracion((ParametrosLPR)this.ParametrosLPR, this.AlternarConfiguracion);
                        // Variamos la configuración para la siguiente 
                        this.AlternarConfiguracion = !this.AlternarConfiguracion;
                        // Se carga la imagen
                        object info = infoInspeccionLPR;
                        int resultCode = 0;
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
                                resultCode = MTInterface.Add(this.RutaImagen,ref info);
                            }
                        }
                        else
                        {
                            resultCode = MTInterface.Add(this.Imagen.Image, ref info);
                        }
                        

                        if (resultCode == -1)
                        {
                            LogsRuntime.Error(ModulosFunciones.LPR, "FuncionLPR", "Error al añadir imagen a la cola VPAR. Total imágenes: " + MTInterface.GetQueueSize.ToString());
                        }

                        LogsRuntime.Info(ModulosFunciones.LPR, "FuncionLPR", "Añadida imagen a la cola VPAR. Total imágenes: " + MTInterface.GetQueueSize.ToString());
                    }
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosFunciones.LPR, "FuncionLPR", exception);
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

            resultado |= (MTInterface.GetNumberOfElements() > 0);
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
                    this.ParametrosLPR = (ParametrosLPR)valor;
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
                LogsRuntime.Error(ModulosFunciones.LPR, this.Codigo, exception);
            }
            return resultado;
        }
        #endregion
    }

    /// <summary>
    /// Clase que contiene la información referente a la inspección
    /// </summary>
    /// <typeparam name="TInfo"></typeparam>
    /// <typeparam name="TParametros"></typeparam>
    /// <typeparam name="TResultados"></typeparam>
    public class InfoInspeccionLPR : InfoInspeccion<BitmapImage, InfoImagenLPR, ParametrosLPR, ResultadoLPR>
    {
        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public InfoInspeccionLPR()
            : base()
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="info"></param>
        /// <param name="parametros"></param>
        /// <param name="resultados"></param>
        public InfoInspeccionLPR(BitmapImage imagen, InfoImagenLPR info, ParametrosLPR parametros, ResultadoLPR resultados)
            : base(imagen, info, parametros, resultados)
        {
        }
        #endregion
    }

    /// <summary>
    /// Esta clase contendra la información que queremos pasar con la imagen a la función LPR para recogerla cuando tengamos el resultado
    /// </summary>
    public class InfoImagenLPR
    {
        #region Atributo(s)
        /// <summary>
        /// Contiene la información de la identificacion a la que pertenece
        /// </summary>
        public long IdEjecucionActual;
        /// <summary>
        /// Contiene la información de la cámara que se le pasa en la imagen
        /// </summary>
        public string Codigo;
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
        public InfoImagenLPR()
        {
            this.IdEjecucionActual = 0;
            this.Codigo = "";
            this.IndiceImagen = 0;
            this.MomentoImagen = DateTime.Now;
            this.CallBackResultadoParcial = null;
        }
        /// <summary>
        /// Constructor con parametros
        /// </summary>
        /// <param name="codigo">camara que adquiere la imagen</param>
        public InfoImagenLPR(long idEjecucionActual,string codigo, int indice, DateTime momentoImagen, CallBackResultadoParcial callBackResultadoParcial)
        {
            this.IdEjecucionActual = idEjecucionActual;            
            this.Codigo = codigo;
            this.IndiceImagen = indice;
            this.MomentoImagen = momentoImagen;
            this.CallBackResultadoParcial = callBackResultadoParcial;
        }
        #endregion
    }

    /// <summary>
    /// Clase que contiene los parametros de configuracion del LPR
    /// </summary>
    public class ParametrosLPR
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
        /// Para saber si han habido modificaciones
        /// </summary>
        public bool Modificado;

        #endregion Campos

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase sin parámetros
        /// </summary>
        public ParametrosLPR()
        {
            this.Altura = -1;
            this.ActivadoRangoAlturas = 0;
            this.AlturaMin = 30;
            this.AlturaMax = 60;
            this.VectorAlturas = new Int32[2];
            this.VectorAlturas[0] = AlturaMin;
            this.VectorAlturas[1] = AlturaMax;
            this.TimeOut = 0;
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
            this.Modificado = true;
        }
        #endregion Constructores
    }

    /// <summary>
    /// Clase que contiene los resultados que se reciben de la funcion LPR para una imagen pasada 
    /// </summary>
    public class ResultadoLPR
    {
        #region Atributo(s)
        /// <summary>
        /// Contiene la cadena la matrícula del vehículo
        /// </summary>
        public string Matricula;
        /// <summary>
        /// Fiabilidad de la matrícula
        /// </summary>
        public int FiabilidadMatricula;
        /// <summary>
        /// Altura letras
        /// </summary>
        public int AlturaLetrasMatricula;
        /// <summary>
        /// Fecha en la que se encolo a la cola de LPR (dada por él)
        /// </summary>
        public DateTime FechaEncolamiento;
        /// <summary>
        /// Tiempo de proceso
        /// </summary>
        public int TiempoDeProceso;
        /// <summary>
        /// Posicion Top matricula
        /// </summary>
        public int TopPosicionMatricula;
        /// <summary>
        /// Posicion Bottom matricula
        /// </summary>
        public int BottomPosicionMatricula;
        /// <summary>
        /// Posicion Right matricula
        /// </summary>
        public int RightPosicionMatricula;
        /// <summary>
        /// Posicion Left matricula
        /// </summary>
        public int LeftPosicionMatricula;
        /// <summary>
        /// <summary>Codigo del pais de la matrículaPosicion Left matricula
        /// </summary>
        public int CodigoPais;
        /// <summary>
        /// <summary>Devuelve el pais que cumple la sintáxis
        /// </summary>
        public int PaisSintaxis;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor sin parametros
        /// </summary>
        public ResultadoLPR()
        {
            this.Matricula = string.Empty;
            this.FiabilidadMatricula = 0;
            this.AlturaLetrasMatricula = 0;
            this.TiempoDeProceso = 0;
            this.TopPosicionMatricula = 0;
            this.BottomPosicionMatricula = 0;
            this.LeftPosicionMatricula = 0;
            this.RightPosicionMatricula = 0;
            this.CodigoPais = 0;
            this.FechaEncolamiento = DateTime.Now;
            this.PaisSintaxis = 0;
        }
        /// <summary>
        /// Constructor con parametros
        /// </summary>
        public ResultadoLPR(PlateInfo resultadoImagen, DateTime fechaEncola)
        {
            try
            {
                this.Matricula = (string)resultadoImagen.GetPlateNumber.ToString().Clone();
                this.FiabilidadMatricula = Convert.ToInt16((Convert.ToInt16(resultadoImagen.GetGlobalConfidence)).ToString().Clone());
                this.AlturaLetrasMatricula = Convert.ToInt16((Convert.ToInt16(resultadoImagen.GetAverageCharacterHeigth)).ToString().Clone());
                this.TiempoDeProceso = Convert.ToInt16((Convert.ToInt16(resultadoImagen.GetProcessingTime)).ToString().Clone());
                this.TopPosicionMatricula = Convert.ToInt16((Convert.ToInt16(resultadoImagen.GetTopPlatePosition)).ToString().Clone());
                this.BottomPosicionMatricula = Convert.ToInt16((Convert.ToInt16(resultadoImagen.GetBottomPlatePosition)).ToString().Clone());
                this.LeftPosicionMatricula = Convert.ToInt16((Convert.ToInt16(resultadoImagen.GetLeftPlatePosition)).ToString().Clone());
                this.RightPosicionMatricula = Convert.ToInt16((Convert.ToInt16(resultadoImagen.GetRightPlatePosition)).ToString().Clone());
                this.CodigoPais = Convert.ToInt16((Convert.ToInt16(resultadoImagen.GetPlateFormat)).ToString().Clone());
                this.FechaEncolamiento = fechaEncola;
                this.PaisSintaxis = Convert.ToInt16((Convert.ToInt16(resultadoImagen.GetPlateFormat)).ToString().Clone());
            }
            catch (Exception exception)
            {
                // En caso de que se produzca cualquier error no contemplado, descartaremos el resultado recibido permitiendo continuar la ejecución
                this.Matricula = string.Empty;
                this.FiabilidadMatricula = 0;
                this.AlturaLetrasMatricula = 0;
                this.TiempoDeProceso = 0;
                this.TopPosicionMatricula = 0;
                this.BottomPosicionMatricula = 0;
                this.LeftPosicionMatricula = 0;
                this.RightPosicionMatricula = 0;
                this.CodigoPais = 0;
                this.FechaEncolamiento = DateTime.Now;
                LogsRuntime.Info(ModulosFunciones.LPR, "FuncionLPR", exception);
            }
        }
        #endregion
    }

    /// <summary>
    /// Parámetros de la aplicación
    /// </summary>
    [Serializable]
    public class ConfiguracionLPR : OAlmacenXML
    {
        #region Atributo(s) estáticos
        /// <summary>
        /// Ruta por defecto del fichero xml de configuración
        /// </summary>
        public static string ConfFile = Path.Combine(RutaParametrizable.AppFolder, "ConfiguracionLPR.xml");
        #endregion

        #region Atributo(s)
        /// <summary>
        /// Constante que indica el pais o paises a identificar la matricula
        /// </summary>
        public int CountryCode = 101;
        /// <summary>
        /// Constante de altura del caracter
        /// </summary>
        public int AvCharHeight = -1;
        /// <summary>
        /// Parámetro de Vpar
        /// </summary>
        public int DuplicateLines = 0;
        /// <summary>
        /// Parametro de Vpar
        /// </summary>
        public int Reordenar = 0;
        /// <summary>
        /// Parametro de Vpar
        /// </summary>
        public int filterColor = 0;
        /// <summary>
        /// Trazabilidad
        /// </summary>
        public int TraceVpar = 1;
        /// <summary>
        /// Indica si se ha de realizar una traza del LPR
        /// </summary>
        public bool TraceWrapper = true;
        #endregion

        #region Constructor
        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public ConfiguracionLPR()
            : base(ConfFile)
        {
        }

        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public ConfiguracionLPR(string rutaFichero)
            : base(rutaFichero)
        {
        }
        #endregion Constructor
    }
}
