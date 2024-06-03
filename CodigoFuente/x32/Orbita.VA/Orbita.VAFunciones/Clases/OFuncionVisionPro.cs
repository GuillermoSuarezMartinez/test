//***********************************************************************
// Assembly         : Orbita.VAFunciones
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 16-11-2012
// Description      : Modificados métodos de finalización
//                    Deshabiliada la gestión de memoria por parte de VisionPro
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.ComponentModel;
using System.IO;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;
using Orbita.VAComun;
using Orbita.VAHardware;

namespace Orbita.VAFunciones
{
    /// <summary>
    /// Función de visión de VisionPro
    /// </summary>
    public class OFuncionVisionPro : OFuncionVisionBase
    {
        #region Constante(s)
        /// <summary>
        /// Se optimizará la velocidad de la primera ejecución, realizando una ejecución al cargarse el archivo vpp
        /// </summary>
        private const bool OptimizarVelocidad = true;
        #endregion

        #region Atributo(s)
        /// <summary>
        /// Variable CogToolBlock que carga la aplicación
        /// </summary>
        public CogToolBlock ToolBlock;

        /// <summary>
        /// Trabajo en segundo plano
        /// </summary>
        private BackgroundWorker ThreadEjecucion;

        #endregion

        #region Constructor(es)
        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public OFuncionVisionPro(string codFuncionVision)
            : base(codFuncionVision)
        {
            try
            {
                this.TipoFuncionVision = OTipoFuncionVision.VisionPro;

                // Creación de los campos
                this.ThreadEjecucion = new BackgroundWorker();
                this.ThreadEjecucion.WorkerSupportsCancellation = true;
                this.ThreadEjecucion.DoWork += new DoWorkEventHandler(this.EjecucionEnThread);
                this.ThreadEjecucion.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.FinEjecucionThread);

                // Carga del fichero vpp
                if (Path.IsPathRooted(this.RutaFichero) && File.Exists(this.RutaFichero))
                {
                    // Se carga el job
                    object vppFile;
                    //Encriptado con extensión vpe
                    if (this.RutaFichero.Contains(".vpe"))
                    {
                        Stream streamVPE = new MemoryStream(App.DecryptFile(this.RutaFichero, "OrbitaIng06"));
                        vppFile = CogSerializer.LoadObjectFromStream(streamVPE);
                    }
                    else
                    {
                        // Sin Encriptar
                        vppFile = CogSerializer.LoadObjectFromFile(this.RutaFichero);
                    }

                    if (vppFile is CogToolBlock)
                    {
                        this.ToolBlock = (CogToolBlock)vppFile;

                        this.Valido = true;
                    }
                }

                if (!this.Valido)
                {
                    throw new Exception("Ha sido imposible cargar la función de visión pro '" + codFuncionVision + "'. El archivo '" + this.RutaFichero + "' no existe o está dañado.");
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosFunciones.VisionPro, this.Codigo, exception);
            }
        }

        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Ejecución de la función a través de otro hilo de ejecución distinto al de la aplicación principal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EjecucionEnThread(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Run the toolblock
                this.ToolBlock.Run();
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosFunciones.VisionPro, this.Codigo, exception);
            }
        }

        /// <summary>
        /// Final de la ejecución de la función
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FinEjecucionThread(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!OThreadManager.EjecucionEnTrheadPrincipal())
            {
                OThreadManager.SincronizarConThreadPrincipal(new RunWorkerCompletedEventHandler(this.FinEjecucionThread), new object[] { sender, e });
                return;
            }

            this.FuncionEjecutada();
        }

        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Carga el fichero de QuickBuild Application
        /// </summary>
        public override void Inicializar()
        {
            base.Inicializar();

            try
            {
                if (this.Valido)
                {
                    //this.ToolBlock.GarbageCollectionEnabled = true;
                    this.ToolBlock.GarbageCollectionEnabled = false;
                    if (OptimizarVelocidad)
                    {
                        // La primera ejecución es muy lenta por lo que se realiza una ejecución de prueba al cargar el toolblock
                        this.ToolBlock.Run();
                    }

                    this.Valido = true;
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosFunciones.VisionPro, this.Codigo, exception);
            }
        }

        /// <summary>
        /// Método a heredar donde se descarga el fichero de la función de visión
        /// </summary>
        public override void Finalizar()
        {
            base.Finalizar();

            // Disconnect the event handlers before disposing the object
            if (this.ThreadEjecucion != null)
            {
                this.ThreadEjecucion.CancelAsync();
                this.ThreadEjecucion.Dispose();
                this.ThreadEjecucion = null;
            }

            if (this.ToolBlock != null)
            {
                this.ToolBlock.Dispose();
                this.ToolBlock = null;
            }
        }

        /// <summary>
        /// Ejecución de la función de Vision Pro de forma síncrona
        /// </summary>
        /// <returns></returns>
        protected override bool EjecucionSincrona()
        {
            bool resultado = false;
            resultado = base.EjecucionSincrona();

            try
            {
                // Run the toolblock
                this.ToolBlock.Run();
                this.FuncionEjecutada();
                resultado = true;
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosFunciones.VisionPro, this.Codigo, exception);
            }

            return resultado;
        }

        /// <summary>
        /// Ejecución de la función de Vision Pro de forma asíncrona
        /// </summary>
        /// <returns></returns>
        protected override bool EjecucionAsincrona()
        {
            bool resultado = false;
            resultado = base.EjecucionAsincrona();

            try
            {
                // Ejecutamos el thread
                this.ThreadEjecucion.RunWorkerAsync();
                resultado = true;
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosFunciones.VisionPro, this.Codigo, exception);
            }

            return resultado;
        }

        /// <summary>
        /// Función para la actualización de parámetros de entrada
        /// </summary>
        /// <param name="ParamName">Nombre identificador del parámetro</param>
        /// <param name="ParamValue">Nuevo valor del parámetro</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override bool SetEntrada(string codigo, object valor, OEnumTipoDato tipoVariable)
        {
            bool functionReturnValue = false;
            try
            {
                if (this.ToolBlock.Inputs[codigo].ValueType == valor.GetType())
                {
                    this.ToolBlock.Inputs[codigo].Value = valor;
                    functionReturnValue = true;
                }
                else
                {
                    if (tipoVariable == OEnumTipoDato.Imagen)
                    {
                        if (valor is OImagenVisionPro)
                        {
                            OImagenVisionPro visionProImage = (OImagenVisionPro)valor;
                            this.ToolBlock.Inputs[codigo].Value = visionProImage.Image;
                            functionReturnValue = true;
                        }
                        else
                        {
                            OImage orbitaImage = (OImage)valor;
                            OImagenVisionPro visionProImage;
                            if (orbitaImage.Convert<OImagenVisionPro>(out visionProImage))
                            {
                                this.ToolBlock.Inputs[codigo].Value = visionProImage.Image;
                                functionReturnValue = true;
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Error en la asignación del parámetro '" + codigo + "' a la función '" + this.Codigo + "'. No coinciden los tipos.");
                    }
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosFunciones.VisionPro, this.Codigo, exception);
            }
            return functionReturnValue;
        }

        /// <summary>
        /// Función para la consulta de parámetros de salida
        /// </summary>
        /// <param name="ParamName">Nombre identificador del parámetro</param>
        /// <param name="ParamValue">Nuevo valor del parámetro</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public override bool GetSalida(string codigo, out object valor, OEnumTipoDato tipoVariable)
        {
            valor = null;
            bool functionReturnValue = false;
            try
            {
                object objValor = this.ToolBlock.Outputs[codigo].Value;

                switch (tipoVariable)
                {
                    case OEnumTipoDato.Imagen:
                        {
                            OImagenVisionPro visionProImage = new OImagenVisionPro(objValor);
                            valor = visionProImage;
                            break;
                        }
                    case OEnumTipoDato.Grafico:
                        {
                            OVisionProGrafico visionProGrafico = new OVisionProGrafico(objValor);
                            valor = visionProGrafico;
                            break;
                        }
                    default:
                        {
                            valor = objValor;
                            break;
                        }
                }

                functionReturnValue = true;
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosFunciones.VisionPro, this.Codigo, exception);
            }
            return functionReturnValue;
        }

        #endregion
    }
}
