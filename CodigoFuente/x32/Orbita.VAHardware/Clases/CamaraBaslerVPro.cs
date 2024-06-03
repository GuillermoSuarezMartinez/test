//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 05-11-2012
// Description      : Adaptada a la utilización de los nuevos controles display
//
// Last Modified By : aibañez
// Last Modified On : 27-09-2012
// Description      : Número de entradas y salidas configurable por base de datos
//                    Bug solucionado: Timer de escaneo de entradas se paraba al detener el modo adquisición continuo
//
// Last Modified By : aibañez
// Last Modified On : 08-10-2012
// Description      : Por compatibilidad con las cámaras axis, también se emplea el parámetro configurado en  
//                    BBDD LanzarEventoAlSnap para lanzar el cambio de variable.
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Cognex.VisionPro;
using Cognex.VisionPro.FGGigE;
using Cognex.VisionPro.FGGigE.Implementation.Internal;
using Orbita.Trazabilidad;
using Orbita.Utiles;
using Orbita.VAComun;
using Cognex.VisionPro.Exceptions;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Clase que implementa las funciones de manejo de la cámara BaslerVPro
    /// </summary>
    public class CamaraBaslerVPro : CamaraBase
    {
        #region Atributo(s) estáticos
        /// <summary>
        /// Listado de todas las cámaras de tipo GigE
        /// </summary>
        private static CogFrameGrabberGigEs FrameGrabbersGigEs;
        /// <summary>
        /// Booleano que evita que se construya varias veces el listado de cámaras de tipo GigE
        /// </summary>
        public static bool PrimeraInstancia = true;
        #endregion

        #region Atributo(s)
        /// <summary>
        /// Objeto de cognex para el acceso a la cámara
        /// </summary>
        private CogFrameGrabberGigE FrameGrabber;
        /// <summary>
        /// Acceso a los parámetros internos de la cámara
        /// </summary>
        public GigEFeatures Ajustes;
        /// <summary>
        /// Buffer fifo de adquisición
        /// </summary>
        private CogAcqFifoGigE AcqFifo;
        /// <summary>
        /// Timer de escaneo de las entradas
        /// </summary>
        //private System.Timers.Timer timerScan;
        private Timer TimerScan;
        /// <summary>
        /// Timer de escaneo de las entradas
        /// </summary>
        //private System.Timers.Timer timerScan;
        private Timer TimerReconexion;

        ///// <summary>
        ///// Line1
        ///// </summary>
        //private TerminalIOBaslerVproBit Line1;
        ///// <summary>
        ///// Line2
        ///// </summary>
        //private TerminalIOBaslerVproBit Line2;
        ///// <summary>
        ///// Out1
        ///// </summary>
        //private TerminalIOBaslerVproBit Out1;
        ///// <summary>
        ///// Out2
        ///// </summary>
        //private TerminalIOBaslerVproBit Out2;
        ///// <summary>
        ///// Out3
        ///// </summary>
        //private TerminalIOBaslerVproBit Out3;
        ///// <summary>
        ///// Out4
        ///// </summary>
        //private TerminalIOBaslerVproBit Out4;

        /// <summary>
        /// Indica que la adquisición está siendo procesada en el momento actual
        /// </summary>
        private bool AdquisicionEnProceso;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Propieadad a heredar donde se accede a la imagen
        /// </summary>
        public new VisionProImage ImagenActual
        {
            get { return (VisionProImage)this._ImagenActual; }
            set { this._ImagenActual = value; }
        }

        /// <summary>
        /// Propieadad a heredar donde se accede a la imagen
        /// </summary>
        public new CtrlDisplayVPro Display
        {
            get { return (CtrlDisplayVPro)this._Display; }
            set { this._Display = value; }
        }

        /// <summary>
        /// Identificador interno de la cámara
        /// </summary>
        private string _DeviceId;
        /// <summary>
        /// Identificador interno de la cámara
        /// </summary>
        public string DeviceId
        {
            get { return _DeviceId; }
            set { _DeviceId = value; }
        }

        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public CamaraBaslerVPro(string codigo)
            : base(codigo)
        {
            try
            {
                // Inicialización de variables
                this.AdquisicionEnProceso = false;

                // Creación de los parámetros internos de las cámaras
                this.Ajustes = new GigEFeatures(this.Codigo);

                // Cargamos valores de la base de datos
                DataTable dt = AppBD.GetCamara(codigo);
                if (dt.Rows.Count == 1)
                {
                    // Rellenamos la información propia de la cámara
                    this._DeviceId = dt.Rows[0]["Basler_Pilot_DeviceID"].ToString();

                    // Rellenamos los terminales dinámicamente
                    this._ListaTerminales = new List<TerminalIOBase>();
                    DataTable dtTerminales = AppBD.GetTerminalesIO(codigo);
                    if (dtTerminales.Rows.Count > 0)
                    {
                        foreach (DataRow drTerminales in dtTerminales.Rows)
                        {
                            string codigoTerminalIO = drTerminales["CodTerminalIO"].ToString();
                            this._ListaTerminales.Add(new TerminalIOBaslerVproBit(this, codigo, codigoTerminalIO));
                        }
                    }


                    //this.Line1 = new TerminalIOBaslerVproBit(this, codigo, "Line1");
                    //this.Line2 = new TerminalIOBaslerVproBit(this, codigo, "Line2");
                    //this.Out1 = new TerminalIOBaslerVproBit(this, codigo, "Out1");
                    //this.Out2 = new TerminalIOBaslerVproBit(this, codigo, "Out2");
                    //this.Out3 = new TerminalIOBaslerVproBit(this, codigo, "Out3");
                    //this.Out4 = new TerminalIOBaslerVproBit(this, codigo, "Out4");
                    //this._ListaTerminales = new List<TerminalIOBase>() { this.Line1, this.Line2, this.Out1, this.Out2, this.Out3, this.Out4 };

                    // Creamos el timer de escaneo de las entradas
                    this.TimerScan = new Timer();
                    this.TimerScan.Interval = 1;
                    this.TimerScan.Enabled = false;
                    this.TimerScan.Tick += new EventHandler(EventoScan);

                    // Creamos el timer de reconexión
                    this.TimerReconexion = new Timer();
                    this.TimerReconexion.Interval = 1000;
                    this.TimerReconexion.Enabled = false;
                    this.TimerReconexion.Tick += new EventHandler(EventoReconexion);

                    // Se construye la lista de cámaras GigE
                    if (PrimeraInstancia)
                    {
                        // Get a reference to a collection of all the GigE Vision Camaras found by this system.
                        FrameGrabbersGigEs = new CogFrameGrabberGigEs();
                        PrimeraInstancia = false;
                    }

                    // Se busca la cámara con su número de serie
                    this.Existe = this.BuscarCamaraPorNumeroSerie();
                }
                else
                {
                    throw new Exception("No se ha podido cargar la información de la cámara " + codigo + " \r\nde la base de datos.");
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Fatal(ModulosHardware.CamaraBaslerVPro, this.Codigo, exception);
                throw new Exception("Imposible iniciar la cámara " + this.Codigo);
            }
        }

        #endregion

        #region Método(s) privado(s)

        /// <summary>
        /// Busca la frame grabber cuyo número de serie coincide con el indicado
        /// </summary>
        /// <param name="numeroDeSerie">Número de serie de la cámara a buscar</param>
        /// <param name="frameGrabber">FrameGrabber encontrado</param>
        /// <returns>Veradero si se ha encontrado el número de serie</returns>
        private bool BuscarCamaraPorNumeroSerie()
        {
            bool resultado = false;

            try
            {
                // Get a reference to a JAI CM-*GE series Camara's Frame Grabber 
                // object if one is attached to this PC.
                foreach (CogFrameGrabberGigE frameGrabberLoop in FrameGrabbersGigEs)
                {
                    string deviceId = frameGrabberLoop.SerialNumber;

                    if (this._DeviceId == deviceId)
                    {
                        this.FrameGrabber = frameGrabberLoop;
                        resultado = true;
                        break;
                    }
                }
            }
            catch (COMException exception)
            {
                OMensajes.MostrarError("La cámara con número de serie " + this._DeviceId + "\n no se encuetra o está actualmente en uso.");
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, this.DeviceId, exception);
            }

            return resultado;
        }

        /// <summary>
        /// Vacia el buffer de adquisición
        /// </summary>
        /// <returns></returns>
        private bool VaciarBuffer()
        {
            bool resultado = false;

            try
            {
                int numPending = 0;
                int numReady = 0;
                bool busy = false;
                Stopwatch cronometro = new Stopwatch();

                cronometro.Start();
                do
                {
                    Application.DoEvents();
                    this.AcqFifo.GetFifoState(out numPending, out numReady, out busy);
                }
                while ((busy || (numPending > 0) || this.AdquisicionEnProceso) && (cronometro.Elapsed.TotalMilliseconds < 2000));

                this.AcqFifo.Flush();
                resultado = true;
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, this.Codigo, exception);
            }

            return resultado;
        }

        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Carga los valores de la cámara
        /// </summary>
        public override void Inicializar()
        {
            base.Inicializar();
        }

        /// <summary>
        /// Finaliza la cámara
        /// </summary>
        public override void Finalizar()
        {
            base.Finalizar();

            this.Display.Finalizar();

            this.Ajustes = null;
            this.AcqFifo = null;
            if (this.EstadoConexion == Orbita.VAHardware.EstadoConexion.Conectado)
            {
                this.FrameGrabber.Disconnect(true);                
            }
            this.FrameGrabber = null;
        }

        /// <summary>
        /// Se toma el control de la cámara
        /// </summary>
        /// <returns>Verdadero si la operación ha funcionado correctamente</returns>
        public override bool Conectar()
        {
            bool resultado = base.Conectar();
            try
            {
                if ((this.Existe) && (this.EstadoConexion != EstadoConexion.Conectado))
                {
                    if (this.FrameGrabber.OwnedGigEAccess == null)  // Check for GigE Access support.
                        resultado = false;  // Exit if no GigE Access support on this Frame-Grabber.

                    this.Ajustes.Inicializar(this.FrameGrabber.OwnedGigEAccess);

                    // Create a CogAcqFifo object for this Camara.
                    this.AcqFifo = (CogAcqFifoGigE)this.FrameGrabber.CreateAcqFifo(this.Ajustes.FeatureAcquisitionFormat.Valor, this.Ajustes.ImageFormat, 0, false);

                    //this.AcqFifo.Timeout = 1000;
                    //this.AcqFifo.TimeoutEnabled = false; // Opcional                    
                    this.AcqFifo.TimeoutEnabled = true; // Opcional

                    // Inicializamos las características de la cámara
                    this.Ajustes.Configurar(this.FrameGrabber, this.AcqFifo, this.Color);

                    // Se configuran los terminales dinamicamente
                    foreach (TerminalIOBaslerVproBit terminalIO in this._ListaTerminales)
                    {
                        terminalIO.Inicializar(this.Ajustes.FeatureLineSelector, this.Ajustes.FeatureLineSource, this.Ajustes.FeatureLineStatusAll, this.Ajustes.FeatureUserOutputSelector, this.Ajustes.FeatureUserOutputValue);
                    }
                    //this.Line1.Inicializar(this.Ajustes.FeatureLineSelector, this.Ajustes.FeatureLineSource, this.Ajustes.FeatureLineStatusAll, this.Ajustes.FeatureUserOutputSelector, this.Ajustes.FeatureUserOutputValue);
                    //this.Line2.Inicializar(this.Ajustes.FeatureLineSelector, this.Ajustes.FeatureLineSource, this.Ajustes.FeatureLineStatusAll, this.Ajustes.FeatureUserOutputSelector, this.Ajustes.FeatureUserOutputValue);
                    //this.Out1.Inicializar(this.Ajustes.FeatureLineSelector, this.Ajustes.FeatureLineSource, this.Ajustes.FeatureLineStatusAll, this.Ajustes.FeatureUserOutputSelector, this.Ajustes.FeatureUserOutputValue);
                    //this.Out2.Inicializar(this.Ajustes.FeatureLineSelector, this.Ajustes.FeatureLineSource, this.Ajustes.FeatureLineStatusAll, this.Ajustes.FeatureUserOutputSelector, this.Ajustes.FeatureUserOutputValue);
                    //this.Out3.Inicializar(this.Ajustes.FeatureLineSelector, this.Ajustes.FeatureLineSource, this.Ajustes.FeatureLineStatusAll, this.Ajustes.FeatureUserOutputSelector, this.Ajustes.FeatureUserOutputValue);
                    //this.Out4.Inicializar(this.Ajustes.FeatureLineSelector, this.Ajustes.FeatureLineSource, this.Ajustes.FeatureLineStatusAll, this.Ajustes.FeatureUserOutputSelector, this.Ajustes.FeatureUserOutputValue);

                    OrbitaImage inicio = new OrbitaImage();

                    this.EstadoConexion = EstadoConexion.Conectado;

                    // Ponemos en marcha el timer de escaneo
                    this.TimerScan.Enabled = true;

                    resultado = true;
                }
            }
            catch (CameraConectionException exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, this.Codigo, "Problema de conexión con la cámara " + this.Codigo + ": " + exception.ToString());
                this.EstadoConexion = EstadoConexion.ErrorConexion;
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, this.Codigo, "Error al conectarse a la cámara " + this.Codigo + ": " + exception.ToString());
            }

            return resultado;
        }

        /// <summary>
        /// Se deja el control de la cámara
        /// </summary>
        /// <returns>Verdadero si la operación ha funcionado correctamente</returns>
        public override bool Desconectar()
        {
            bool resultado = false;

            try
            {
                if ((this.Existe) && (this.EstadoConexion != EstadoConexion.Desconectado))
                {
                    // Paramos el timer de escaneo
                    this.TimerScan.Enabled = false;

                    this.Ajustes.Stop();

                    this.VaciarBuffer();

                    this.EstadoConexion = EstadoConexion.Desconectado;
                    resultado = true;
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, this.Codigo, exception);
            }

            return resultado;
        }

        /// <summary>
        /// Comienza una reproducción continua de la cámara
        /// </summary>
        /// <returns></returns>
        protected override bool InternalStart()
        {
            bool resultado = false;

            try
            {
                if (this.EstadoConexion == EstadoConexion.Conectado)
                {
                    base.InternalStart();

                    // Indicamos que no existe ninguna adquisición ejecutandose en estos momentos
                    this.AdquisicionEnProceso = false;

                    // Nos suscribimos al evento de adquisición de imagen
                    this.AcqFifo.Complete += this.CompleteAcquisition;

                    // Acquisition configuration
                    this.Ajustes.Start();
                }
            }
            catch (CameraConectionException exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, this.Codigo, "Problema de conexión con la cámara " + this.Codigo + ": " + exception.ToString());
                this.EstadoConexion = EstadoConexion.ErrorConexion;
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, this.Codigo, exception);
            }

            return resultado;
        }

        /// <summary>
        /// Termina una reproducción continua de la cámara
        /// </summary>
        /// <returns></returns>
        protected override bool InternalStop()
        {
            bool resultado = false;

            try
            {

                if (this.EstadoConexion != EstadoConexion.Desconectado)
                {
                    //this.TimerScan.Stop();

                    // Nos desuscribimos al evento de adquisición de imagen
                    this.AcqFifo.Complete -= this.CompleteAcquisition;

                    this.Ajustes.Stop();
                    this.VaciarBuffer();

                    // Indicamos que no existe ninguna adquisición ejecutandose en estos momentos
                    this.AdquisicionEnProceso = false;

                    base.InternalStop();
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, this.Codigo, exception);
            }

            return resultado;
        }

        /// <summary>
        /// Realiza una fotografía de forma sincrona
        /// </summary>
        /// <returns></returns>
        protected override bool InternalSnap()
        {
            bool resultado = false;
            try
            {
                if ((this.EstadoConexion == EstadoConexion.Conectado) && (this.Ajustes.AcquisitionMode == ModoAdquisicion.DisparoSoftware))
                {
                    base.InternalSnap();

                    this.AcqFifo.StartAcquire();
                }
            }
            catch (CameraConectionException exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, this.Codigo, "Problema de conexión con la cámara " + this.Codigo + ": " + exception.ToString());
                this.EstadoConexion = EstadoConexion.ErrorConexion;
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, this.Codigo, exception);
            }
            return resultado;
        }

        /// <summary>
        /// Carga una imagen de disco
        /// </summary>
        /// <param name="ruta">Indica la ruta donde se encuentra la fotografía</param>
        /// <returns>Devuelve verdadero si la operación se ha realizado con éxito</returns>
        public override bool CargarImagenDeDisco(out OrbitaImage imagen, string ruta)
        {
            bool resultado = false;

            if (this.ImagenActual != null)
            {
                this.ImagenActual = null;
            }

            imagen = new VisionProImage();
            bool imagenok = imagen.Cargar(ruta);
            if (imagenok)
            {
                this.ImagenActual = (VisionProImage)imagen;
                resultado = true;
            }

            return resultado;
        }

        /// <summary>
        /// Guarda una imagen en disco
        /// </summary>
        /// <param name="ruta">Indica la ruta donde se ha de guardar la fotografía</param>
        /// <returns>Devuelve verdadero si la operación se ha realizado con éxito</returns>
        public override bool GuardarImagenADisco(string ruta)
        {
            bool resultado = false;

            if (this.ImagenActual is VisionProImage)
            {
                resultado = this.ImagenActual.Guardar(ruta);
            }

            return resultado;
        }

        /// <summary>
        /// Guarda un objeto gráfico en disco
        /// </summary>
        /// <param name="ruta">Indica la ruta donde se ha de guardar el objeto gráfico</param>
        /// <returns>Devuelve verdadero si la operación se ha realizado con éxito</returns>
        public override bool GuardarGraficoADisco(OrbitaGrafico graficos, string ruta)
        {
            bool resultado = false;

            if (graficos is VisionProGrafico)
            {
                resultado = graficos.Guardar(ruta);
            }

            return resultado;
        }

        /// <summary>
        /// Visualiza una imagen en el display
        /// </summary>
        /// <param name="imagen">Imagen a visualizar</param>
        /// <param name="graficos">Objeto que contiene los gráficos a visualizar (letras, rectas, circulos, etc)</param>
        public override void VisualizarImagen(OrbitaImage imagen, OrbitaGrafico graficos)
        {
            this.Display.Visualizar(imagen, graficos);
        }

        /// <summary>
        /// Devuelve una nueva imagen del tipo adecuado al trabajo con la cámara
        /// </summary>
        /// <returns>Imagen del tipo adecuado al trabajo con la cámara</returns>
        public override OrbitaImage NuevaImagen()
        {
            return new VisionProImage();
        }

        /// <summary>
        /// Se inicia el protocolo de reconexión
        /// </summary>
        public override void IniciarProtocoloReconexion()
        {
            base.IniciarProtocoloReconexion();

            this.TimerReconexion.Enabled = true;
        }

        #endregion

        #region Eventos
        /// <summary>
        /// Evento de recepción de nueva imagen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CompleteAcquisition(object sender, CogCompleteEventArgs e)
        {
            if (!ThreadRuntime.EjecucionEnTrheadPrincipal())
            {
                ThreadRuntime.SincronizarConThreadPrincipal(new CogCompleteEventHandler(this.CompleteAcquisition), new object[] { sender, e });
                return;
            }

            // indicamos que se está procesando una adquisición
            this.AdquisicionEnProceso = true;

            // Este evento se realiza desde un subproceso por lo que es necesario llamar al proceso padre
            try
            {
                if (this.Ajustes.AcquisitionMode != ModoAdquisicion.Continuo)
                {
                    LogsRuntime.Debug(ModulosHardware.CamaraBaslerVPro, this.Codigo, "Imagen adquirida", NivelLog.Info);
                }

                if (this.LanzarEventoAlSnap && (this.EstadoConexion == EstadoConexion.Conectado))
                {
                    int triggerNumber;
                    int ticket;

                    // Adquisición de la imagen
                    if (this.ImagenActual != null)
                    {
                        this.ImagenActual = null;
                    }
                    this.ImagenActual = new VisionProImage();
                    this.ImagenActual.Image = this.AcqFifo.CompleteAcquire(-1, out ticket, out triggerNumber);
                    
                    // Comprobación de que la imagen recibida de la cámara es correcta
                    if ((this.ImagenActual.Image == null) || (!this.ImagenActual.Image.Allocated) || (this.ImagenActual.Image.Width <= 0) || (this.ImagenActual.Image.Height <= 0))
                    {
                        throw new Exception(string.Format("La imagen recibida de la cámara {0} está corrupta.", this.Codigo ));
                    }

                    // Actualizo el Frame Rate
                    this.MedidorVelocidadAdquisicion.NuevaCaptura();

                    // Visualización en vivo
                    if (this.VisualizacionEnVivo)
                    {
                        this.VisualizarUltimaImagen();
                    }

                    // Se asigna el valor de la variable asociada
                    if (this.LanzarEventoAlSnap && (ImagenActual.EsValida()))
                    {
                        this.EstablecerVariableAsociada(ImagenActual);
                    }
                }
            }
            catch (CameraConectionException exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, this.Codigo, "Problema de conexión con la cámara " + this.Codigo + ": " + exception.ToString());
                this.EstadoConexion = EstadoConexion.ErrorConexion;
            }
            catch (CogAcqException exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, this.Codigo, "Problema de conexión con la cámara " + this.Codigo + ": " + exception.ToString());
                this.EstadoConexion = EstadoConexion.ErrorConexion;
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, this.Codigo, exception);
            }

            // indicamos se ha finalizado la adquisición
            this.AdquisicionEnProceso = false;
        }

        /// <summary>
        /// Evento del timer de ejecución
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void EventoScan(object sender, EventArgs e)
        {
            this.TimerScan.Stop();
            try
            {
                // Lectura dinámica
                foreach (TerminalIOBaslerVproBit terminalIO in this._ListaTerminales)
                {
                    terminalIO.LeerEntrada();
                }

                ////Lectura del puerto A
                //this.Line1.LeerEntrada();
                ////Lectura del puerto B
                //this.Line2.LeerEntrada();
            }
            catch (CameraConectionException exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, this.Codigo, "Problema de conexión con la cámara " + this.Codigo + ": " + exception.ToString());
                this.EstadoConexion = EstadoConexion.ErrorConexion;
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, this.Codigo, exception);
            }
            this.TimerScan.Start();
        }

        /// <summary>
        /// Evento del timer de reconexión
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void EventoReconexion(object sender, EventArgs e)
        {
            this.TimerReconexion.Stop();

            bool reconectado = false;

            try
            {
                // Ejecutamos una consulta a la cámara
                bool ok = this.Ajustes.FeatureLineStatusAll.Receive();
                if (ok)
                {
                    // Se ha producido la reconexión
                    this.Conectar();
                    this.Start();
                    reconectado = true;
                }
            }
            catch (Exception exception)
            {
                //Sin acciones puesto que se trata de una excepción habitual cuando se produce un problema de conexión con la cámara
            }

            if (!reconectado) // si no hemos sido capaces de reconectarnos, continuamos con el protocolo de reconexión
            {
                this.TimerReconexion.Start();
            }
        }
        #endregion
    }

    #region  Clases para el acceso a los parámeteros internos de las cámaras Basler VisionPro
    /// <summary>
    /// Listado de todas las caracteríticas de la cámara
    /// </summary>
    public class GigEFeatures
    {
        #region Atributo(s)
        //public GigEDoubleFeature FeatureExposureTimeAbs = new GigEDoubleFeature("ExposureTimeAbs", 80, 81900, 20000, 20000); //uS // Cambio Shutter
        public DecimalRobusto FeatureExposureTimeAbs = new DecimalRobusto("ExposureTimeAbs", 0.04, 81.9, 0.1, false); //ms // Cambio Shutter
        public GigEEnumFeature FeatureGainAuto = new GigEEnumFeature("GainAuto", new string[] { "Off", "Once", "Continuous" }, "Off", 100);
        //public GigEIntFeature FeatureGainRaw = new GigEIntFeature("GainRaw", 0, 500, 116, 100); // Cambio Gain
        public DecimalRobusto FeatureGainRaw = new DecimalRobusto("GainRaw", 0, 500, 0, false); // Cambio Gain
        public GigEIntFeature FeatureBlackLevelRaw = new GigEIntFeature("BlackLevelRaw", 0, 600, 32, 100);
        public GigEEnumFeature FeatureBalanceRatioSelector = new GigEEnumFeature("BalanceRatioSelector", new string[] { "Red", "Green", "Blue" }, "Red", 100);
        public GigEIntFeature FeatureBalanceRatioRed = new GigEIntFeature("BalanceRatioRaw", 0, 255, 50, 100);
        public GigEIntFeature FeatureBalanceRatioGreen = new GigEIntFeature("BalanceRatioRaw", 0, 255, 50, 100);
        public GigEIntFeature FeatureBalanceRatioBlue = new GigEIntFeature("BalanceRatioRaw", 0, 255, 50, 100);
        public GigEBoolFeature FeatureGammaEnable = new GigEBoolFeature("GammaEnable", false, 100);
        public GigEDoubleFeature FeatureGamma = new GigEDoubleFeature("Gamma", 0, 3.999023, 1, 100);
        public StringEnumRobusto FeatureAcquisitionFormat = new StringEnumRobusto("AcquisitionFormat", new string[] { "Generic GigEVision (Bayer Color)", "Generic GigEVision (Mono)" }, "Generic GigEVision (Mono)", false);
        public GigEEnumFeature FeatureTransferFormat = new GigEEnumFeature("PixelFormat", new string[] { "Mono8", "BayerBG8", "YUV422Packed", "YUV422_YUYV_Packed", "BayerBG12Packed", "BayerBG16" }, "Mono8", 100);
        public GigEEnumFeature FeatureTriggerSource = new GigEEnumFeature("TriggerSource", new string[] { "Line1", "Line2", "Software" }, "Line1", 100);
        public GigEEnumFeature FeatureTriggerMode = new GigEEnumFeature("TriggerMode", new string[] { "Off", "On" }, "Off", 100);
        public GigEIntFeature FeaturePacketSize = new GigEIntFeature("GevSCPSPacketSize", 220, 16404, 1024, 100);
        public GigEIntFeature FeaturePacketDelay = new GigEIntFeature("GevSCPD", 0, 950, 0, 100);
        public EnumRobusto<ModoAdquisicion> FeatureAcquisitionMode = new EnumRobusto<ModoAdquisicion>("AcquisitionMode", ModoAdquisicion.DisparoSoftware, false);
        public EnumRobusto<CogAcqFifoPixelFormatConstants> FeatureImageFormat = new EnumRobusto<CogAcqFifoPixelFormatConstants>("ImageFormat", CogAcqFifoPixelFormatConstants.Format8Grey, false);
        public BoolRobusto FeatureTriggerOnRisingEdge = new BoolRobusto("TriggerOnRisingEdge", true, false);
        public EnteroRobusto FeatureAOIX = new EnteroRobusto("AOIX", 0, 10000, 2454, false);
        public EnteroRobusto FeatureAOIY = new EnteroRobusto("AOIY", 0, 10000, 2454, false);
        public EnteroRobusto FeatureAOIWidth = new EnteroRobusto("AOIWidth", 0, 10000, 2454, false);
        public EnteroRobusto FeatureAOIHeight = new EnteroRobusto("AOIHeight", 0, 10000, 2056, false);
        public DecimalRobusto FeatureBandwidth = new DecimalRobusto("Bandwidth", 1, 100, 90, false);
        public GigEEnumFeature FeatureLineSelector = new GigEEnumFeature("LineSelector", new string[] { "Line1", "Line2", "Out1", "Out2", "Out3", "Out4", "Out1", "Out2" }, "Line1", 100);
        public GigEEnumFeature FeatureLineSource = new GigEEnumFeature("LineSource", new string[] { "UserOutput" }, "UserOutput", 100);
        public GigEIntFeature FeatureLineStatusAll = new GigEIntFeature("LineStatusAll", 0, int.MaxValue, 0, 100);
        public GigEEnumFeature FeatureUserOutputSelector = new GigEEnumFeature("UserOutputSelector", new string[] { "UserOutput1", "UserOutput2", "UserOutput3", "UserOutput4" }, "UserOutput1", 100);
        public GigEEnumFeature FeatureUserOutputValue = new GigEEnumFeature("UserOutputValue", new string[] { "0", "1" }, "0", 100);
        public GigEDoubleFeature FeatureLineDebouncerTimeAbs = new GigEDoubleFeature("LineDebouncerTimeAbs", 0, double.MaxValue, 0, 100);
        public GigEDoubleFeature FeatureTemperatureAbs = new GigEDoubleFeature("TemperatureAbs", 0, double.MaxValue, 0, 100);

        /// <summary>
        /// Código identificativo de la cámara
        /// </summary>
        private string CodCamara;
        /// <summary>
        /// Frame grabber asociado a la cámara
        /// </summary>
        private CogFrameGrabberGigE FrameGrabberGigE;
        /// <summary>
        /// Clase cognex que permite el acceso a los parámetros internos de la cámara
        /// </summary>
        public ICogGigEAccess GigEFeatureAccess;
        /// <summary>
        /// Buffer fifo de adquisición
        /// </summary>
        private CogAcqFifoGigE AcqFifo;
        /// <summary>
        /// Indica si la cámara es a color o monocromática
        /// </summary>
        private TipoColorPixel Color;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public GigEFeatures(string codigo)
        {
            this.CodCamara = codigo;

            try
            {
                // Cargamos valores de la base de datos
                DataTable dt = AppBD.GetCamara(this.CodCamara);
                if (dt.Rows.Count == 1)
                {
                    this.FeatureExposureTimeAbs.ValorGenerico = dt.Rows[0]["Basler_Pilot_Shutter"];
                    this.FeatureGainAuto.ValorGenerico = dt.Rows[0]["Basler_Pilot_GainAuto"];
                    this.FeatureGainRaw.ValorGenerico = dt.Rows[0]["Basler_Pilot_Gain"];
                    this.FeatureBlackLevelRaw.ValorGenerico = dt.Rows[0]["Basler_Pilot_BlackLevel"];
                    this.FeatureBalanceRatioRed.ValorGenerico = dt.Rows[0]["Basler_Pilot_WhiteBalanceRed"];
                    this.FeatureBalanceRatioGreen.ValorGenerico = dt.Rows[0]["Basler_Pilot_WhiteBalanceGreen"];
                    this.FeatureBalanceRatioBlue.ValorGenerico = dt.Rows[0]["Basler_Pilot_WhiteBalanceBlue"];
                    this.FeatureGammaEnable.ValorGenerico = dt.Rows[0]["Basler_Pilot_GammaEnable"];
                    this.FeatureGamma.ValorGenerico = dt.Rows[0]["Basler_Pilot_Gamma"];
                    this.FeatureAcquisitionFormat.ValorGenerico = dt.Rows[0]["Basler_Pilot_AcquisitionFormat"];
                    this.FeatureTransferFormat.ValorGenerico = dt.Rows[0]["Basler_Pilot_TransferFormat"];
                    this.FeatureImageFormat.ValorGenerico = dt.Rows[0]["Basler_Pilot_ImageFormat"];
                    this.FeatureAcquisitionMode.ValorGenerico = dt.Rows[0]["Basler_Pilot_AcquisitionMode"];
                    this.FeatureTriggerSource.ValorGenerico = dt.Rows[0]["Basler_Pilot_TriggerSource"];
                    this.FeaturePacketSize.ValorGenerico = dt.Rows[0]["Basler_Pilot_PacketSize"];
                    this.FeatureTriggerOnRisingEdge.ValorGenerico = dt.Rows[0]["Basler_Pilot_TriggerOnRisingEdge"];
                    this.FeatureAOIX.ValorGenerico = dt.Rows[0]["Basler_Pilot_AOI_X"];
                    this.FeatureAOIY.ValorGenerico = dt.Rows[0]["Basler_Pilot_AOI_Y"];
                    this.FeatureAOIWidth.ValorGenerico = dt.Rows[0]["Basler_Pilot_AOI_Width"];
                    this.FeatureAOIHeight.ValorGenerico = dt.Rows[0]["Basler_Pilot_AOI_Height"];
                    this.FeatureBandwidth.ValorGenerico = dt.Rows[0]["Basler_Pilot_Bandwidth"];
                    this.FeatureLineDebouncerTimeAbs.ValorGenerico = dt.Rows[0]["Basler_Pilot_DebouncerTime"];
                }
                else
                {
                    throw new Exception("No se ha podido cargar la información de los parámetros de la cámara " + codigo + " \r\nen la base de datos.");
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, this.CodCamara, exception);
            }
        }
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Tiempo de exposición
        /// </summary>
        public double Shutter 
        {
            get
            {
                this.FeatureExposureTimeAbs.Valor = this.AcqFifo.OwnedExposureParams.Exposure; // Cambio Shutter
                return this.FeatureExposureTimeAbs.Valor; // Cambio Shutter

                //this.FeatureExposureTimeAbs.Receive(); // Cambio Shutter
                //return this.FeatureExposureTimeAbs.Valor; // Cambio Shutter
            }
            set
            {
                this.FeatureExposureTimeAbs.Valor = value; // Cambio Shutter
                this.AcqFifo.OwnedExposureParams.Exposure = this.FeatureExposureTimeAbs.Valor; // Cambio Shutter

                //this.FeatureExposureTimeAbs.Valor = value; // Cambio Shutter
                //this.FeatureExposureTimeAbs.Send(); // Cambio Shutter
            }
        }// OwnerGigE
        /// <summary>
        /// Autoganancia
        /// </summary>
        public string GainAuto
        {
            get
            {
                this.FeatureGainAuto.Receive();
                return this.FeatureGainAuto.Valor;
            }
            set
            {
                this.FeatureGainAuto.Valor = value;
                this.FeatureGainAuto.Send();
            }
        }// GigE
        /// <summary>
        /// Ganancia
        /// </summary>
        //public int Gain // Cambio Gain
        public double Gain // Cambio Gain
        {
            get
            {
                this.FeatureGainRaw.Valor = this.AcqFifo.OwnedContrastParams.Contrast; // Cambio Gain
                return this.FeatureGainRaw.Valor; // Cambio Gain

                //this.FeatureGainRaw.Receive(); // Cambio Gain
                //return this.FeatureGainRaw.Valor; // Cambio Gain
            }
            set
            {
                this.FeatureGainRaw.Valor = value; // Cambio Gain
                this.AcqFifo.OwnedContrastParams.Contrast = this.FeatureGainRaw.Valor; // Cambio Gain

                //this.FeatureGainRaw.Valor = value; // Cambio Gain
                //this.FeatureGainRaw.Send(); // Cambio Gain
            }
        }// GigE
        /// <summary>
        /// Nivel de oscuridad del negro
        /// </summary>
        public int BlackLevel
        {
            get
            {
                this.FeatureBlackLevelRaw.Receive();
                return this.FeatureBlackLevelRaw.Valor;
            }
            set
            {
                this.FeatureBlackLevelRaw.Valor = value;
                this.FeatureBlackLevelRaw.Send();
            }
        }// GigE
        /// <summary>
        /// Componente de rojo del balance de blancos
        /// </summary>
        public int WhiteBalanceRed
        {
            get
            {
                this.FeatureBalanceRatioSelector.Valor = "Red";
                this.FeatureBalanceRatioSelector.Send();

                this.FeatureBalanceRatioRed.Receive();
                return this.FeatureBalanceRatioRed.Valor;
            }
            set
            {
                this.FeatureBalanceRatioSelector.Valor = "Red";
                this.FeatureBalanceRatioSelector.Send();

                this.FeatureBalanceRatioRed.Valor = value;
                this.FeatureBalanceRatioRed.Send();
            }
        }// GigE
        /// <summary>
        /// Componente de verde del balance de blancos
        /// </summary>
        public int WhiteBalanceGreen
        {
            get
            {
                this.FeatureBalanceRatioSelector.Valor = "Green";
                this.FeatureBalanceRatioSelector.Send();

                this.FeatureBalanceRatioGreen.Receive();
                return this.FeatureBalanceRatioGreen.Valor;
            }
            set
            {
                this.FeatureBalanceRatioSelector.Valor = "Green";
                this.FeatureBalanceRatioSelector.Send();

                this.FeatureBalanceRatioGreen.Valor = value;
                this.FeatureBalanceRatioGreen.Send();
            }
        }// GigE
        /// <summary>
        /// Componente de azul del balance de blancos
        /// </summary>
        public int WhiteBalanceBlue
        {
            get
            {
                this.FeatureBalanceRatioSelector.Valor = "Green";
                this.FeatureBalanceRatioSelector.Send();

                this.FeatureBalanceRatioBlue.Receive();
                return this.FeatureBalanceRatioBlue.Valor;
            }
            set
            {
                this.FeatureBalanceRatioSelector.Valor = "Green";
                this.FeatureBalanceRatioSelector.Send();

                this.FeatureBalanceRatioBlue.Valor = value;
                this.FeatureBalanceRatioBlue.Send();
            }
        }// GigE
        /// <summary>
        /// Habilita la corrección gamma
        /// </summary>
        public bool GammaEnable
        {
            get
            {
                this.FeatureGammaEnable.Receive();
                return this.FeatureGammaEnable.Valor;
            }
            set
            {
                this.FeatureGammaEnable.Valor = value;
                this.FeatureGammaEnable.Send();
            }
        }// GigE
        /// <summary>
        /// Corrección Gamma
        /// </summary>
        public double Gamma
        {
            get
            {
                this.FeatureGamma.Receive();
                return this.FeatureGamma.Valor;
            }
            set
            {
                this.FeatureGamma.Valor = value;
                this.FeatureGamma.Send();
            }
        }// GigE
        /// <summary>
        /// Texto indicativo del formato de adquisición de la cámara
        /// </summary>
        public string AcquisitionFormat
        {
            get
            {
                return this.FeatureAcquisitionFormat.Valor;
            }
            set
            {
                this.FeatureAcquisitionFormat.Valor = value;
            }
        }
        /// <summary>
        /// Texto indicativo del formato de transferencia de la cámara al PC
        /// </summary>
        public string TransferFormat
        {
            get
            {
                this.FeatureTransferFormat.Receive();
                return this.FeatureTransferFormat.Valor;
            }
            set
            {
                this.FeatureTransferFormat.Valor = value;
                this.FeatureTransferFormat.Send();
            }
        }// GigE
        /// <summary>
        /// formato de imagen de trabajo en el PC
        /// </summary>
        public CogAcqFifoPixelFormatConstants ImageFormat
        {
            get
            {
                return this.FeatureImageFormat.Valor;
            }
            set
            {
                this.FeatureImageFormat.Valor = value;
            }
        }
        /// <summary>
        /// Modo de adquisición (continuo, hardwaretrigger o softwaretrigger)
        /// </summary>
        public ModoAdquisicion AcquisitionMode
        {
            get
            {
                return this.FeatureAcquisitionMode.Valor;
            }
            set
            {
                this.FeatureAcquisitionMode.Valor = value;
            }
        }
        /// <summary>
        /// Texto indicativo del origen del trigger de la cámara (Line1, Line2, Software)
        /// </summary>
        public string TriggerSource
        {
            get
            {
                this.FeatureTriggerSource.Receive();
                return this.FeatureTriggerSource.Valor;
            }
            set
            {
                this.FeatureTriggerSource.Valor = value;
                this.FeatureTriggerSource.Send();
            }
        }// GigE
        /// <summary>
        /// Indica si el trigger se disparará en el flanco de subida o de bajada
        /// </summary>
        public bool TriggerOnRisingEdge
        {
            get
            {
                return this.FeatureTriggerOnRisingEdge.Valor;
            }
            set
            {
                this.FeatureTriggerOnRisingEdge.Valor = value;
            }
        }
        /// <summary>
        /// Area de la imagen con la que se trabajará
        /// </summary>
        public Rectangle AOI
        {
            get
            {
                // Extrae la información del AOI

                int AOIX = this.FeatureAOIX.Valor;
                int AOIY = this.FeatureAOIY.Valor;
                int AOIWidth = this.FeatureAOIWidth.Valor;
                int AOIHeight = this.FeatureAOIHeight.Valor;

                return new Rectangle(AOIX, AOIY, AOIWidth, AOIHeight);
            }
            set
            {
                this.FeatureAOIX.Valor = value.Left;
                this.FeatureAOIY.Valor = value.Top;
                this.FeatureAOIWidth.Valor = value.Width;
                this.FeatureAOIHeight.Valor = value.Height;

                // Guarda la información del AOI
            }
        }
        /// <summary>
        /// Tiempo antirebote
        /// </summary>
        public double LineDebouncerTimeAbs
        {
            get
            {
                this.FeatureLineDebouncerTimeAbs.Receive();
                return this.FeatureLineDebouncerTimeAbs.Valor;
            }
            set
            {
                this.FeatureLineDebouncerTimeAbs.Valor = value;
                this.FeatureLineDebouncerTimeAbs.Send();
            }
        }// GigE
        /// <summary>
        /// Tiempo antirebote
        /// </summary>
        public double Temperature
        {
            get
            {
                this.FeatureTemperatureAbs.Receive();
                return this.FeatureTemperatureAbs.Valor;
            }
            set
            {
                this.FeatureTemperatureAbs.Valor = value;
                this.FeatureTemperatureAbs.Send();
            }
        }// GigE
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// One of the default features of a JAI CM-*GE series Camara needs
        /// to be changed before VisionPro can interact with it.
        /// This function needs to be called before a CogAcqFifo is created.
        /// </summary>
        /// <param name="gigEAccess"></param>
        private void InitializeForCompatibility()
        {
            try
            {
                // Loading a user set, which VisionPro does at start up, takes
                // longer than the VisionPro default Camara write timeout for
                // JAI CM-*GE series Camaras.  Increase the VisionPro Camara write
                // timeout before creating an acqFifo.
                this.GigEFeatureAccess.SetFeatureWriteTimeout(500.0);
                this.GigEFeatureAccess.SetFeatureReadTimeout(500.0);
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, "InitializeForCompatibility", exception);
            }
        }
        /// <summary>
        /// Configures a GigE area of interesting
        /// </summary>
        private void ConfigureROI()
        {
            try
            {
                // Get a reference to the ROIParams interface of the AcqFifo.
                ICogAcqROI mROIParams = this.AcqFifo.OwnedROIParams;
                // Always check to see an "Owned" property is supported before using it.
                if (mROIParams != null)  // Check for ROI support.
                {
                    // sets the ROI
                    mROIParams.SetROIXYWidthHeight(this.AOI.Left, this.AOI.Top, this.AOI.Width, this.AOI.Height);
                    this.AcqFifo.Prepare();  // writes the properties to the Camara.
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, "ConfigureROI", exception);
            }
        }
        /// <summary>
        /// When using a VisionPro hardware trigger model with JAI CM-*GE 
        /// series Camaras, you must set the input line to be used 
        /// for the trigger signal and set the "ExposureMode" 
        /// ("Trigger Mode" in current JAI Camara documentation) to 
        /// "EdgePreSelect".
        /// </summary>
        /// <param name="gigEAccess">The ICogGigEAccess on which to configure
        /// the trigger.</param>
        private void ConfigureTrigger()
        {
            try
            {
                // Trigger Configuration
                ICogAcqTrigger triggerOperator = this.AcqFifo.OwnedTriggerParams;
                switch (this.AcquisitionMode)
                {
                    case ModoAdquisicion.Continuo:
                        triggerOperator.TriggerEnabled = false;
                        triggerOperator.TriggerModel = CogAcqTriggerModelConstants.FreeRun;
                        break;
                    case ModoAdquisicion.DisparoSoftware:
                        triggerOperator.TriggerEnabled = false;
                        triggerOperator.TriggerModel = CogAcqTriggerModelConstants.Manual;
                        break;
                    case ModoAdquisicion.DisparoHardware:
                        triggerOperator.TriggerEnabled = false;
                        triggerOperator.TriggerModel = CogAcqTriggerModelConstants.Auto;
                        break;
                }

                // Setup the trigger edge.
                triggerOperator.TriggerLowToHigh = this.TriggerOnRisingEdge;

                // Setup the trigger source.
                this.FeatureTriggerSource.Send();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, "ConfigureTrigger", exception);
            }
        }
        /// <summary>
        /// When using a VisionPro hardware trigger model with JAI CM-*GE 
        /// series Camaras, you must set the input line to be used 
        /// for the trigger signal and set the "ExposureMode" 
        /// ("Trigger Mode" in current JAI Camara documentation) to 
        /// "EdgePreSelect".
        /// </summary>
        /// <param name="gigEAccess">The ICogGigEAccess on which to configure
        /// the trigger.</param>
        private void DesConfigureTrigger()
        {
            try
            {
                // Trigger Configuration
                ICogAcqTrigger triggerOperator = this.AcqFifo.OwnedTriggerParams;
                triggerOperator.TriggerModel = CogAcqTriggerModelConstants.Manual;
                this.StopTrigger();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, "DesConfigureTrigger", exception);
            }
        }

        /// <summary>
        /// Detiene el trigger
        /// </summary>
        private void StopTrigger()
        {
            ICogAcqTrigger triggerOperator = this.AcqFifo.OwnedTriggerParams;
            triggerOperator.TriggerEnabled = false;
        }

        /// <summary>
        /// When using multiple Camaras, it is possible that the
        /// total data rate of the Camaras will exceed the bandwidth of the 
        /// GigE network.  Acquisition can still be successfully performed
        /// if the data rate of the Camaras is reduced to fit within the 
        /// available bandwidth.
        /// The general idea is that if you have an n Camara
        /// application, you set each Camara's bandwidth to 1/n and the data
        /// rate will be reduced as needed to allow all Camaras to work
        /// simultaneously.  See additional comments on the SetBandwidth
        /// function.
        /// Note that this code will only work if the Camara supports
        /// the required GigE Vision registers.  You can discover if these
        /// are supported by looking over the XML description file for
        /// the Camara. 
        /// </summary>
        /// <param name="gigEAccess"> The ICogGigEAccess on which to set the Bandwidth</param>
        /// <param name="percentageOfBandwidth"> Percentage of total bandwidth
        /// which is to be dedicated to this Camara.</param>
        private void SetBandwidth()
        {
            try
            {
                // 1000 MBytes / sec overall throughput
                Double maxRate = 1000 * 1024 * 1024;
                this.FeaturePacketSize.Send();
                int packetSize = this.FeaturePacketSize.Valor;
                double packetTime = packetSize / maxRate;

                // Use the bandwidth setting to calculate the time it should require to
                // transmit each packet to achieve the desired bandwidth.  For example, a
                // bandwidth setting of 0.25 means we want each packet to take 4 times
                // longer than it would at full speed.
                double desiredTime = packetTime / this.FeatureBandwidth.Valor;

                // The difference between the desired and actual times is the delay we want
                // between each packet.  Note that until the delay becomes larger than the
                // intrinsic delay between each packet sent by the Camara, changes in
                // bandwidth won't have any effect on the data rate.
                double delaySec = desiredTime - packetTime;

                ulong timeStampFreq = this.GigEFeatureAccess.TimeStampFrequency;
                int delay = (int)(delaySec * timeStampFreq);
                this.FeaturePacketDelay.Valor = delay;
                this.FeaturePacketDelay.Send();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, "SetBandwidth", exception);
            }
        }
        /// <summary>
        /// Configura el balance de blancos de la cámara
        /// </summary>
        private void ConfigureWhiteBalance()
        {
            if (this.Color == TipoColorPixel.RGB)
            {
                this.FeatureBalanceRatioSelector.Valor = "Red";
                this.FeatureBalanceRatioSelector.Send();
                this.FeatureBalanceRatioRed.Send();

                this.FeatureBalanceRatioSelector.Valor = "Green";
                this.FeatureBalanceRatioSelector.Send();
                this.FeatureBalanceRatioGreen.Send();

                this.FeatureBalanceRatioSelector.Valor = "Blue";
                this.FeatureBalanceRatioSelector.Send();
                this.FeatureBalanceRatioBlue.Send();
            }
        }
        /// <summary>
        /// Configures a GigE Camara's exposure using the standard
        /// VisionPro interface.
        /// </summary>
        private void ConfigureExposure()
        {
            // Get a reference to the ExposureParams interface of the AcqFifo.
            ICogAcqExposure exposureParams = this.AcqFifo.OwnedExposureParams;
            // Always check to see an "Owned" property is supported
            // before using it.
            if (exposureParams != null)  // Check for exposure support.
            {
                exposureParams.Exposure = this.Shutter;  // sets ExposureTimeAbs
                this.AcqFifo.Prepare();  // writes the properties to the Camara.
            }
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Inicialización del acceso a los parámetros de la cámara
        /// </summary>
        /// <param name="gigEFeatureAccess"></param>
        /// <param name="frameGrabberGigE"></param>
        /// <param name="acqFifo"></param>
        public void Inicializar(ICogGigEAccess gigEFeatureAccess)
        {
            // Asignación de los campos
            this.GigEFeatureAccess = gigEFeatureAccess;

            // Inicializamos los valores
            //this.FeatureExposureTimeAbs.Inicializar(gigEFeatureAccess); // Cambio Shutter
            this.FeatureGainAuto.Inicializar(gigEFeatureAccess);
            //this.FeatureGainRaw.Inicializar(gigEFeatureAccess); // Cambio Gain
            this.FeatureBlackLevelRaw.Inicializar(gigEFeatureAccess);
            this.FeatureBalanceRatioSelector.Inicializar(gigEFeatureAccess);
            this.FeatureBalanceRatioRed.Inicializar(gigEFeatureAccess);
            this.FeatureBalanceRatioGreen.Inicializar(gigEFeatureAccess);
            this.FeatureBalanceRatioBlue.Inicializar(gigEFeatureAccess);
            this.FeatureGammaEnable.Inicializar(gigEFeatureAccess);
            this.FeatureGamma.Inicializar(gigEFeatureAccess);
            this.FeatureTransferFormat.Inicializar(gigEFeatureAccess);
            this.FeatureTriggerSource.Inicializar(gigEFeatureAccess);
            this.FeaturePacketSize.Inicializar(gigEFeatureAccess);
            this.FeaturePacketDelay.Inicializar(gigEFeatureAccess);
            this.FeatureLineSelector.Inicializar(gigEFeatureAccess);
            this.FeatureLineSource.Inicializar(gigEFeatureAccess);
            this.FeatureLineStatusAll.Inicializar(gigEFeatureAccess);
            this.FeatureUserOutputSelector.Inicializar(gigEFeatureAccess);
            this.FeatureUserOutputValue.Inicializar(gigEFeatureAccess);
            this.FeatureLineDebouncerTimeAbs.Inicializar(gigEFeatureAccess);
            this.FeatureTemperatureAbs.Inicializar(gigEFeatureAccess);
        }

        /// <summary>
        /// Inicialización del acceso a los parámetros de la cámara
        /// </summary>
        /// <param name="gigEFeatureAccess"></param>
        /// <param name="frameGrabberGigE"></param>
        /// <param name="acqFifo"></param>
        public void Configurar(CogFrameGrabberGigE frameGrabberGigE, CogAcqFifoGigE acqFifo, TipoColorPixel color)
        {
            // Asignación de los campos
            this.FrameGrabberGigE = frameGrabberGigE;
            this.AcqFifo = acqFifo;
            this.Color = color;

            //ICogAcqROI mROIParams = this.AcqFifo.OwnedROIParams;
            //// Always check to see an "Owned" property is supported before using it.
            //if (mROIParams != null)  // Check for ROI support.
            //{
            //    // sets the ROI
            //    mROIParams.SetROIXYWidthHeight(this.FeatureAOIX.Valor, this.FeatureAOIY.Valor, this.FeatureAOIWidth.Valor, this.FeatureAOIHeight.Valor);
            //}
            //this.AcqFifo.Prepare();

            //// Trigger Configuration
            //ICogAcqTrigger triggerOperator = this.AcqFifo.OwnedTriggerParams;
            //triggerOperator.TriggerEnabled = false;
            //triggerOperator.TriggerModel = CogAcqTriggerModelConstants.Auto;
            //triggerOperator.TriggerEnabled = true;
            //// Setup the trigger edge.
            //triggerOperator.TriggerLowToHigh = true;
            //this.AcqFifo.Prepare();

            //this.GigEFeatureAccess.SetFeature("TriggerSource", "Line1");

            //this.GigEFeatureAccess.SetDoubleFeature("ExposureTimeAbs", this.FeatureExposureTimeAbs.Valor);
            //this.GigEFeatureAccess.SetIntegerFeature("GainRaw", (uint)this.FeatureGainRaw.Valor);

            //this.GigEFeatureAccess.SetFeature("BalanceRatioSelector", "Red");
            //this.GigEFeatureAccess.SetIntegerFeature("BalanceRatioRaw", (uint)this.FeatureBalanceRatioRed.Valor);

            //this.GigEFeatureAccess.SetFeature("BalanceRatioSelector", "Green");
            //this.GigEFeatureAccess.SetIntegerFeature("BalanceRatioRaw", (uint)this.FeatureBalanceRatioGreen.Valor);

            //this.GigEFeatureAccess.SetFeature("BalanceRatioSelector", "Blue");
            //this.GigEFeatureAccess.SetIntegerFeature("BalanceRatioRaw", (uint)this.FeatureBalanceRatioBlue.Valor);

            //this.GigEFeatureAccess.SetFeature("PixelFormat", "BayerBG8");

            //this.GigEFeatureAccess.SetFeature("LineSelector", "Out1");
            //this.GigEFeatureAccess.SetFeature("LineSource", "UserOutput");
            //this.GigEFeatureAccess.SetFeature("UserOutputSelector", "UserOutput1");
            //this.GigEFeatureAccess.SetFeature("UserOutputValue", "1");

            //// Save the current settings to the Camara's first user set
            //this.GigEFeatureAccess.SetFeature("UserSetSelector", "UserSet1");
            //this.GigEFeatureAccess.ExecuteCommand("UserSetSave");
            //// Choose the user set to use as the power-on default
            //this.GigEFeatureAccess.SetFeature("UserSetDefaultSelector", "UserSet1");


            //return; //////////////////////

            // Change the Region Of Interenst (ROI) of the Camara.
            this.ConfigureROI();

            // Acquisition configuration
            this.ConfigureTrigger();
            //App.Espera(10000); // Tiempo necesario para que la cámara se configure

            // Strobe Configuration
            //JAIConfig.ConfigureStrobe(gigEAccess);

            // User Set Management... Uncomment next line to enable.
            //JAIConfig.SaveUserSet(gigEAccess);

            // Data Rate Control... 
            this.SetBandwidth();

            // Expoure Configuration - Set exposure
            //this.FeatureExposureTimeAbs.Send(); //uS // Cambio Shutter
            this.Shutter = this.FeatureExposureTimeAbs.Valor; // Cambio Shutter
            //this.ConfigureExposure(); // Actualmente no necesario

            // Establecemos los valores por defecto de ganancia, nivel de negro, balance de blancos y gamma
            //this.FeatureGainRaw.Send(); // Cambio Gain
            this.Gain = this.FeatureGainRaw.Valor;  // Cambio Gain
            this.FeatureBlackLevelRaw.Send();
            this.FeatureGammaEnable.Send();
            this.FeatureGamma.Send();

            this.ConfigureWhiteBalance();

            // Formato de transferencia entre la cámara y el PC
            this.FeatureTransferFormat.Send();

            // Tiempo antirebote
            this.FeatureLineSelector.Valor = "Line1";
            this.FeatureLineSelector.Send();
            this.FeatureLineDebouncerTimeAbs.Send();
        }

        /// <summary>
        /// Prepara la cámara para la adquisición
        /// </summary>
        public void Start()
        {
            switch (this.AcquisitionMode)
            {
                case ModoAdquisicion.Continuo:
                    break;
                case ModoAdquisicion.DisparoSoftware:
                    this.FeatureTriggerMode.Valor = "Off";
                    break;
                case ModoAdquisicion.DisparoHardware:
                    this.FeatureTriggerMode.Valor = "On";
                    break;
            }
            this.FeatureTriggerMode.Send();

            ICogAcqTrigger triggerOperator = this.AcqFifo.OwnedTriggerParams;
            triggerOperator.TriggerEnabled = true;
            this.AcqFifo.Prepare();
        }

        /// <summary>
        /// Desprepara la cámara para la adquisición
        /// </summary>
        public void Stop()
        {
            switch (this.AcquisitionMode)
            {
                case ModoAdquisicion.Continuo:
                    break;
                case ModoAdquisicion.DisparoSoftware:
                case ModoAdquisicion.DisparoHardware:
                    this.FeatureTriggerMode.Valor = "Off";
                    this.FeatureTriggerMode.Send();
                    break;
            }

            //this.DesConfigureTrigger();
            this.StopTrigger();
        }
        #endregion
    }

    /// <summary>
    /// Acceso a una característica de la cámara de tipo string
    /// </summary>
    public class GigEStringFeature : TextoRobusto, iCamFeature
    {
        #region Atributo(s)
        /// <summary>
        /// Clase cognex para el acceso a los parámetros de la cámara
        /// </summary>
        private ICogGigEAccess GigEFeatureAccess;
        /// <summary>
        /// TimeOut de envio o recepción
        /// </summary>
        TimeSpan TimeOut;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public GigEStringFeature(string codigo, int maxLength, bool admiteVacio, string defaultValue, int timeOutMilis)
            : base(codigo, maxLength, admiteVacio, defaultValue, false)
        {
            this.TimeOut = TimeSpan.FromMilliseconds(timeOutMilis);
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Inicialización del objeto
        /// </summary>
        /// <param name="gigEFeatureAccess"></param>
        public void Inicializar(ICogGigEAccess gigEFeatureAccess)
        {
            this.GigEFeatureAccess = gigEFeatureAccess;
        }
        /// <summary>
        /// Aplica el valor de memoria a la cámara
        /// </summary>
        public bool Send()
        {
            bool resultado = false;

            try
            {
                if (this.GigEFeatureAccess != null)
                {
                    string strValue = (string)this.Valor;
                    this.GigEFeatureAccess.SetFeature(this.Codigo, strValue);
                    resultado = true;
                }
            }
            catch (COMException exception)
            {
                throw new CameraConectionException();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, "SetFeature:" + this.Codigo, exception);
            }

            return resultado;
        }
        /// <summary>
        /// Consulta el valor de la cámara y lo guarda en memoria
        /// </summary>
        public bool Receive()
        {
            bool resultado = false;

            try
            {
                {
                    this.Valor = this.GigEFeatureAccess.GetFeature(this.Codigo);
                    resultado = true;
                }
            }
            catch (COMException exception)
            {
                throw new CameraConectionException();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, "GetFeature:" + this.Codigo, exception);
            }

            return resultado;
        }
        #endregion
    }

    /// <summary>
    /// Acceso a una característica de la cámara de tipo enumerado (aunque internamente trabaja como un string)
    /// </summary>
    public class GigEEnumFeature : StringEnumRobusto, iCamFeature
    {
        #region Atributo(s)
        /// <summary>
        /// Clase cognex para el acceso a los parámetros de la cámara
        /// </summary>
        private ICogGigEAccess GigEFeatureAccess;
        /// <summary>
        /// TimeOut de envio o recepción
        /// </summary>
        TimeSpan TimeOut;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public GigEEnumFeature(string codigo, string[] valoresPermitidos, string defaultValue, int timeOutMilis)
            : base(codigo, valoresPermitidos, defaultValue, false)
        {
            this.TimeOut = TimeSpan.FromMilliseconds(timeOutMilis);
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Inicialización del objeto
        /// </summary>
        /// <param name="gigEFeatureAccess"></param>
        public void Inicializar(ICogGigEAccess gigEFeatureAccess)
        {
            this.GigEFeatureAccess = gigEFeatureAccess;
        }
        /// <summary>
        /// Aplica el valor de memoria a la cámara
        /// </summary>
        public bool Send()
        {
            bool resultado = false;

            try
            {
                if (this.GigEFeatureAccess != null)
                {
                    string strValue = (string)this.Valor;
                    string strOutValue = string.Empty;

                    // Momento en el que finalizará la espera
                    DateTime momentoTimeOut = DateTime.Now + this.TimeOut;
                    bool ok = false;
                    while ((!ok) && (DateTime.Now < momentoTimeOut))
                    {
                        this.GigEFeatureAccess.SetFeature(this.Codigo, strValue);
                        strOutValue = this.GigEFeatureAccess.GetFeature(this.Codigo);
                        ok = (strValue == strOutValue);
                        if (!ok)
                        {
                            App.Espera(10);
                        }
                    }

                    resultado = ok;
                }
            }
            catch (COMException exception)
            {
                throw new CameraConectionException();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, "SetFeature:" + this.Codigo, exception);
            }

            return resultado;
        }
        /// <summary>
        /// Consulta el valor de la cámara y lo guarda en memoria
        /// </summary>
        public bool Receive()
        {
            bool resultado = false;

            try
            {
                if (this.GigEFeatureAccess != null)
                {
                    this.Valor = this.GigEFeatureAccess.GetFeature(this.Codigo);
                    resultado = true;
                }
            }
            catch (COMException exception)
            {
                throw new CameraConectionException();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, "GetFeature:" + this.Codigo, exception);
            }

            return resultado;
        }
        #endregion
    }

    /// <summary>
    /// Acceso a una característica de la cámara de tipo entero
    /// </summary>
    public class GigEIntFeature : EnteroRobusto, iCamFeature
    {
        #region Atributo(s)
        /// <summary>
        /// Clase cognex para el acceso a los parámetros de la cámara
        /// </summary>
        private ICogGigEAccess GigEFeatureAccess;
        /// <summary>
        /// TimeOut de envio o recepción
        /// </summary>
        TimeSpan TimeOut;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public GigEIntFeature(string codigo, int minValue, int maxValue, int defaultValue, int timeOutMilis)
            : base(codigo, minValue, maxValue, defaultValue, false)
        {
            this.TimeOut = TimeSpan.FromMilliseconds(timeOutMilis);
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Inicialización del objeto
        /// </summary>
        /// <param name="gigEFeatureAccess"></param>
        public void Inicializar(ICogGigEAccess gigEFeatureAccess)
        {
            this.GigEFeatureAccess = gigEFeatureAccess;
        }
        /// <summary>
        /// Aplica el parámetro a la cámara
        /// </summary>
        public bool Send()
        {
            bool resultado = false;

            try
            {
                if (this.GigEFeatureAccess != null)
                {
                    uint intValue = (uint)this.Valor;
                    uint intOutValue = uint.MaxValue;

                    // Momento en el que finalizará la espera
                    DateTime momentoTimeOut = DateTime.Now + this.TimeOut;
                    bool ok = false;
                    while ((!ok) && (DateTime.Now < momentoTimeOut))
                    {
                        this.GigEFeatureAccess.SetIntegerFeature(this.Codigo, intValue);
                        intOutValue = this.GigEFeatureAccess.GetIntegerFeature(this.Codigo);
                        ok = (intValue == intOutValue);
                        if (!ok)
                        {
                            App.Espera(10);
                        }
                    }

                    resultado = ok;
                }
            }
            catch (COMException exception)
            {
                throw new CameraConectionException();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, "SetFeature:" + this.Codigo, exception);
            }

            return resultado;
        }
        /// <summary>
        /// Consulta el valor del parámetro a la cámara
        /// </summary>
        public bool Receive()
        {
            bool resultado = false;

            try
            {
                if (this.GigEFeatureAccess != null)
                {
                    this.Valor = (int)this.GigEFeatureAccess.GetIntegerFeature(this.Codigo);
                    resultado = true;
                }
            }
            catch (COMException exception)
            {
                throw new CameraConectionException();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, "GetFeature:" + this.Codigo, exception);
            }

            return resultado;
        }
        #endregion
    }

    /// <summary>
    /// Acceso a una característica de la cámara de tipo entero
    /// </summary>
    public class GigEDoubleFeature : DecimalRobusto, iCamFeature
    {
        #region Atributo(s)
        /// <summary>
        /// Clase cognex para el acceso a los parámetros de la cámara
        /// </summary>
        private ICogGigEAccess GigEFeatureAccess;
        /// <summary>
        /// TimeOut de envio o recepción
        /// </summary>
        TimeSpan TimeOut;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public GigEDoubleFeature(string codigo, double minValue, double maxValue, double defaultValue, int timeOutMilis)
            : base(codigo, minValue, maxValue, defaultValue, false)
        {
            this.TimeOut = TimeSpan.FromMilliseconds(timeOutMilis);
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Inicialización del objeto
        /// </summary>
        /// <param name="gigEFeatureAccess"></param>
        public void Inicializar(ICogGigEAccess gigEFeatureAccess)
        {
            this.GigEFeatureAccess = gigEFeatureAccess;
        }
        /// <summary>
        /// Aplica el valor de memoria a la cámara
        /// </summary>
        public bool Send()
        {
            bool resultado = false;

            try
            {
                if (this.GigEFeatureAccess != null)
                {
                    double doubleValue = (double)this.Valor;
                    double doubleOutValue = double.MaxValue;

                    // Momento en el que finalizará la espera
                    DateTime momentoTimeOut = DateTime.Now + this.TimeOut;
                    bool ok = false;
                    while ((!ok) && (DateTime.Now < momentoTimeOut))
                    {
                        this.GigEFeatureAccess.SetDoubleFeature(this.Codigo, doubleValue);
                        doubleOutValue = this.GigEFeatureAccess.GetDoubleFeature(this.Codigo);
                        ok = (Math.Round(doubleValue, 2) == Math.Round(doubleOutValue, 2));
                        if (!ok)
                        {
                            App.Espera(10);
                        }
                    }

                    resultado = ok;
                }
            }
            catch (COMException exception)
            {
                throw new CameraConectionException();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, "SetFeature:" + this.Codigo, exception);
            }

            return resultado;
        }
        /// <summary>
        /// Consulta el valor de la cámara y lo guarda en memoria
        /// </summary>
        public bool Receive()
        {
            bool resultado = false;

            try
            {
                if (this.GigEFeatureAccess != null)
                {
                    this.Valor = this.GigEFeatureAccess.GetDoubleFeature(this.Codigo);
                    resultado = true;
                }
            }
            catch (COMException exception)
            {
                throw new CameraConectionException();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, "GetFeature:" + this.Codigo, exception);
            }

            return resultado;
        }
        #endregion
    }

    /// <summary>
    /// Acceso a una característica de la cámara de tipo entero
    /// </summary>
    public class GigEBoolFeature : BoolRobusto, iCamFeature
    {
        #region Atributo(s)
        /// <summary>
        /// Clase cognex para el acceso a los parámetros de la cámara
        /// </summary>
        private ICogGigEAccess GigEFeatureAccess;
        /// <summary>
        /// TimeOut de envio o recepción
        /// </summary>
        TimeSpan TimeOut;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public GigEBoolFeature(string codigo, bool defaultValue, int timeOutMilis)
            : base(codigo, defaultValue, false)
        {
            this.TimeOut = TimeSpan.FromMilliseconds(timeOutMilis);
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Inicialización del objeto
        /// </summary>
        /// <param name="gigEFeatureAccess"></param>
        public void Inicializar(ICogGigEAccess gigEFeatureAccess)
        {
            this.GigEFeatureAccess = gigEFeatureAccess;
        }
        /// <summary>
        /// Aplica el valor de memoria a la cámara
        /// </summary>
        public bool Send()
        {
            bool resultado = false;

            try
            {
                if (this.GigEFeatureAccess != null)
                {
                    bool boolValue = (bool)this.Valor;
                    bool boolOutValue = false;

                    // Momento en el que finalizará la espera
                    DateTime momentoTimeOut = DateTime.Now + this.TimeOut;
                    bool ok = false;
                    while ((!ok) && (DateTime.Now < momentoTimeOut))
                    {
                        if (boolValue)
                        {
                            this.GigEFeatureAccess.SetFeature(this.Codigo, "true");
                        }
                        else
                        {
                            this.GigEFeatureAccess.SetFeature(this.Codigo, "false");
                        }

                        string strValue = this.GigEFeatureAccess.GetFeature(this.Codigo);
                        boolOutValue = (strValue == "true");

                        ok = (boolValue == boolOutValue);
                        if (!ok)
                        {
                            App.Espera(10);
                        }
                    }

                    resultado = ok;
                }
            }
            catch (COMException exception)
            {
                throw new CameraConectionException();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, "SetFeature:" + this.Codigo, exception);
            }

            return resultado;
        }
        /// <summary>
        /// Consulta el valor de la cámara y lo guarda en memoria
        /// </summary>
        public bool Receive()
        {
            bool resultado = false;

            try
            {
                if (this.GigEFeatureAccess != null)
                {
                    string strValue = this.GigEFeatureAccess.GetFeature(this.Codigo);
                    this.Valor = (strValue == "true");
                    resultado = true;
                }
            }
            catch (COMException exception)
            {
                throw new CameraConectionException();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, "GetFeature:" + this.Codigo, exception);
            }

            return resultado;
        }
        #endregion
    }

    #endregion

    /// <summary>
    /// Terminal de tipo bit que simboliza un bit de un puerto
    /// </summary>
    internal class TerminalIOBaslerVproBit : TerminalIOBase
    {
        #region Atributo(s)
        private CamaraBaslerVPro Camara;
        /// <summary>
        /// Selector del termial
        /// </summary>
        private GigEEnumFeature FeatureLineSelector;
        /// <summary>
        /// Tipo de terminal
        /// </summary>
        private GigEEnumFeature FeatureLineSource;
        /// <summary>
        /// Estado de los terminales (activados o desactivados)
        /// </summary>
        private GigEIntFeature FeatureLineStatusAll;
        /// <summary>
        /// Selector de salida de usuario
        /// </summary>
        private GigEEnumFeature FeatureUserOutputSelector;
        /// <summary>
        /// Valor de salida
        /// </summary>
        private GigEEnumFeature FeatureUserOutputValue;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Valor del terminal
        /// </summary>
        public new bool Valor
        {
            get
            {
                bool boolValor;
                if (this.ComprobarValor(base.Valor, out boolValor))
                {
                    return boolValor;
                }
                return false;
            }
            set
            {
                base.Valor = value;
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public TerminalIOBaslerVproBit(CamaraBaslerVPro camara, string codTarjetaIO, string codTerminalIO)
            : base(codTarjetaIO, codTerminalIO)
        {
            this.Camara = camara;
        }
        #endregion

        #region Método(s) heredado(s)

        /// <summary>
        /// Método a heredar donde se inicializan los terminales
        /// </summary>
        public new void Inicializar(GigEEnumFeature lineSelector, GigEEnumFeature lineSource, GigEIntFeature lineStatusAll, GigEEnumFeature userOutputSelector, GigEEnumFeature userOutputValue)
        {
            try
            {
                base.Inicializar();

                this.FeatureLineSelector = lineSelector;
                this.FeatureLineSource = lineSource;
                this.FeatureLineStatusAll = lineStatusAll;
                this.FeatureUserOutputSelector = userOutputSelector;
                this.FeatureUserOutputValue = userOutputValue;

                if (this.TipoTerminalIO == TipoTerminalIO.SalidaDigital)
                {
                    this.FeatureLineSelector.Valor = this.Codigo;
                    this.FeatureLineSelector.Send();

                    this.FeatureLineSource.Valor = "UserOutput";
                    this.FeatureLineSource.Send();
                }
            }
            catch (CameraConectionException exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, this.Codigo, "Problema de conexión con la cámara " + this.Codigo + ": " + exception.ToString());
                this.Camara.EstadoConexion = EstadoConexion.ErrorConexion;
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, this.Camara.Codigo, exception);
            }
        }

        /// <summary>
        /// Lectura de la entrada física
        /// </summary>
        public override void LeerEntrada()
        {
            try
            {
                if (this.Camara.EstadoConexion == EstadoConexion.Conectado)
                {
                    base.LeerEntrada();

                    if (this.Habilitado && (this.TipoTerminalIO == TipoTerminalIO.EntradaDigital))
                    {
                        // Leo la entrada fisica
                        this.FeatureLineStatusAll.Receive();
                        uint intValor = (uint)this.FeatureLineStatusAll.Valor;
                        bool boolValor = App.GetBit(intValor, this.Numero);

                        if (this.Valor != boolValor)
                        {
                            this.Valor = boolValor;
                            this.EstablecerValorVariable();
                            //VariableRuntime.SetValue(this.CodVariable, this.Valor, "Camaras", this.Codigo);
                        }
                    }
                }
            }
            catch (CameraConectionException exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, this.Codigo, "Problema de conexión con la cámara " + this.Codigo + ": " + exception.ToString());
                this.Camara.EstadoConexion = EstadoConexion.ErrorConexion;
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, this.Camara.Codigo, exception);
            }
        }

        /// <summary>
        /// Escritura de la salida física
        /// </summary>
        public override void EscribirSalida(string codigoVariable, object valor)
        {
            try
            {
                if (this.Camara.EstadoConexion == EstadoConexion.Conectado)
                {
                    base.EscribirSalida(codigoVariable, valor);

                    // Se comprueba que el valor a escribir sea correcto
                    bool boolValor;
                    if (this.ComprobarValor(valor, out boolValor))
                    {
                        // Si el valor es correcto se escribe la salida física
                        if (this.Habilitado && (this.TipoTerminalIO == TipoTerminalIO.SalidaDigital))
                        {
                            // Se escribe la entrada física
                            this.FeatureUserOutputSelector.Valor = "UserOutput" + (this.Numero + 1).ToString();
                            this.FeatureUserOutputSelector.Send();

                            if (boolValor)
                            {
                                this.FeatureUserOutputValue.Valor = "1";
                                this.FeatureUserOutputValue.Send();
                            }
                            else
                            {
                                this.FeatureUserOutputValue.Valor = "0";
                                this.FeatureUserOutputValue.Send();
                            }
                        }
                        this.Valor = boolValor;
                    }
                }
            }
            catch (CameraConectionException exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, this.Codigo, "Problema de conexión con la cámara " + this.Codigo + ": " + exception.ToString());
                this.Camara.EstadoConexion = EstadoConexion.ErrorConexion;
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraBaslerVPro, this.Camara.Codigo, exception);
            }

        }

        /// <summary>
        /// Se comprueba que el valor a escribir sea del tipo correcto
        /// </summary>
        /// <param name="valor">Valor a escribir</param>
        /// <param name="byteValor">Valor a escribir del tipo correcto</param>
        /// <returns>Devuelve verdadero si el valor a escribir es válido</returns>
        private bool ComprobarValor(object valor, out bool boolValor)
        {
            boolValor = false;

            // Se comprueba que el valor sea correcto
            bool valorOK = false;
            if (valor is bool)
            {
                boolValor = (bool)valor;
                valorOK = true;
            }
            return valorOK;
        }
        #endregion
    }

    /// <summary>
    /// Enumerado de los modos de adquisición posibles
    /// </summary>
    public enum ModoAdquisicion
    {
        /// <summary>
        /// Modo de adquisición continua de multiples imágenes
        /// </summary>
        Continuo = 0,
        /// <summary>
        /// Modo de adquisición de una única imagen mediante un comando software
        /// </summary>
        DisparoSoftware = 1,
        /// <summary>
        /// Modo de adquisición de una única imagen mediante una entrada digital
        /// </summary>
        DisparoHardware = 2
    }
}
