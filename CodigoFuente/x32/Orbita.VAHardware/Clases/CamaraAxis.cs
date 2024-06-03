//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 05-11-2012
// Description      : Adaptada a la utilización de los nuevos controles display
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using Orbita.VAComun;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Clase que implementa las funciones de manejo de la cámara Axis
    /// </summary>
    public class CamaraAxis : CamaraBase
    {
        #region Constante(s)
        public const int TimeOutConexionMS = 10000;
        #endregion

        #region Atributo(s)
        /// <summary>
        /// Dirección IP de la cámara
        /// </summary>
        private IPAddress IP;
        /// <summary>
        /// Usuario para el acceso a la cámara
        /// </summary>
        private string Usuario;
        /// <summary>
        /// Contraseña para el acceso a la cámara
        /// </summary>
        private string Constraseña;
        /// <summary>
        /// Duración máxima de la grabación
        /// </summary>
        private TimeSpan TiempoMaxGrabacion;
        /// <summary>
        /// Timer de tiempo máximo de grabación
        /// </summary>
        private Timer TimerTiempoMaxGrabacion;
        /// <summary>
        /// Input 1
        /// </summary>
        private TerminalIOAxisBit Input1;
        /// <summary>
        /// Input 1
        /// </summary>
        private TerminalIOAxisBit Input2;
        /// <summary>
        /// Output 1
        /// </summary>
        private TerminalIOAxisBit Output1;
        /// <summary>
        /// Informa si hay adquirida una nueva imagen
        /// </summary>
        private bool HayNuevaImagen;
        /// <summary>
        /// Estado interno de la cámara axis
        /// </summary>
        public EstadoCamaraAxis EstadoCamaraAxis;
        /// <summary>
        /// Timer de comprobación del estado de la conexión
        /// </summary>
        private Timer TimerComprobacionConexion;
        /// <summary>
        /// Ping utilizado para detectar la conectividad con la cámara
        /// </summary>
        private Ping Ping;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Propieadad a heredar donde se accede a la imagen
        /// </summary>
        public new BitmapImage ImagenActual
        {
            get { return (BitmapImage)this._ImagenActual; }
            set { this._ImagenActual = value; }
        }
        /// <summary>
        /// Propieadad a heredar donde se accede a la imagen
        /// </summary>
        public new CtrlDisplayAxis Display
        {
            get { return (CtrlDisplayAxis)this._Display; }
            set { this._Display = value; }
        }
        /// <summary>
        /// La cámara está lista para adquirir
        /// </summary>
        public bool ReadyToAcquire
        {
            get
            {
                return this.SimulacionCamara.Simulacion || (this.EstadoCamaraAxis.INITIALIZED && this.EstadoCamaraAxis.PLAYING && !this.EstadoCamaraAxis.OPENING);
            }
        }
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>       
        public CamaraAxis(string codigo)
            : base(codigo)
        {
            try
            {
                // No hay ninguna imagen adquirida
                this.HayNuevaImagen = false;

                // Cargamos valores de la base de datos
                DataTable dt = AppBD.GetCamara(codigo);
                if (dt.Rows.Count == 1)
                {
                    this.IP = IPAddress.Parse(dt.Rows[0]["IPCam_IP"].ToString());
                    this.Usuario = dt.Rows[0]["IPCam_Usuario"].ToString();
                    this.Constraseña = dt.Rows[0]["IPCam_Contraseña"].ToString();

                    // Creación del timer de grabación máxima
                    this.TimerTiempoMaxGrabacion = new Timer();
                    this.TimerTiempoMaxGrabacion.Enabled = false;
                    this.TimerTiempoMaxGrabacion.Tick += this.TimerTiempoMaxGrabacion_Tick;

                    this.TiempoMaxGrabacion = TimeSpan.FromMilliseconds(0);
                    int tiempoMaxGrabacionMs = App.EvaluaNumero(dt.Rows[0]["Axis_TiempoMaxGrabacionMs"], 1, int.MaxValue, -1);
                    if (tiempoMaxGrabacionMs > 0)
                    {
                        this.TiempoMaxGrabacion = TimeSpan.FromMilliseconds(tiempoMaxGrabacionMs);
                        this.TimerTiempoMaxGrabacion.Interval = tiempoMaxGrabacionMs;
                    }

                    // Rellenamos los terminales
                    this.Input1 = new TerminalIOAxisBit(codigo, "Input1", this.IP);
                    this.Input2 = new TerminalIOAxisBit(codigo, "Input2", this.IP);
                    this.Output1 = new TerminalIOAxisBit(codigo, "Output1", this.IP);

                    // Creación del timer de comprobación de la conexión
                    this.TimerComprobacionConexion = new Timer();
                    this.TimerComprobacionConexion.Interval = TimeOutConexionMS;
                    this.TimerComprobacionConexion.Enabled = false;
                    this.TimerComprobacionConexion.Tick += this.TimerComprobacionConexion_Tick;

                    // Creación del ping para detectar la conectividad con la cámara
                    this.Ping = new Ping();
                    this.Ping.PingCompleted += this.PingCompletedEventHandler;

                    this.Existe = true;
                }
                else
                {
                    throw new Exception("No se ha podido cargar la información de la cámara " + codigo + " \r\nde la base de datos.");
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Fatal(ModulosHardware.CamaraAxis, this.Codigo, exception);
                throw new Exception("Imposible iniciar la cámara " + this.Codigo);
            }
        } 
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Se extrae la imagen actual de la cámara
        /// </summary>
        /// <returns></returns>
        private bool GetCurrentImage(out BitmapImage bitmapImage)
        {
            bool resultado = false;
            bitmapImage = null;

            if (this.HayNuevaImagen)
            {
                resultado = this.Display.GetCurrentImage(out bitmapImage);
                this.HayNuevaImagen = !resultado;
            }

            return resultado;
        }

        /// <summary>
        /// Se importa una imagen a la cámara
        /// </summary>
        /// <returns></returns>
        private bool SetCurrentImage(BitmapImage bitmapImage)
        {
            return this.Display.SetCurrentImage(bitmapImage);
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Se extrae el resultado de la entrada de la camara
        /// </summary>
        /// <returns></returns>
        public bool GetEstadoEntrada(int entrada)
        {
            bool resultado = false;
            try
            {
                // Creamos la respuesta a la dirección del parámetro 		
                string cadenaComando = "http://" + this.IP + "/axis-cgi/io/input.cgi?check=" + entrada.ToString(); ;
                WebRequest request = WebRequest.Create(cadenaComando);
                // Configuramos las credenciales
                request.Credentials = CredentialCache.DefaultCredentials;
                // Obtenemos la respuesta
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Comprobamos el estado de la petición
                MessageBox.Show(response.StatusDescription);
                // Conseguimos el dato de la respuesta
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Leemos el contenido
                string responseFromServer = reader.ReadToEnd();
                // Interpretamos la respuesta
                MessageBox.Show(responseFromServer);
                // Limpiamos las variables utilizadas
                reader.Close();
                dataStream.Close();
                response.Close();

                resultado = true;
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraAxis, this.Codigo, exception);
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

            // Nos suscribimos al cambio de estatus de la cámara
            this.Display.dispAxis.OnError += DispAxis_OnError;
            this.Display.dispAxis.OnStatusChange += DispAxis_OnStatusChange;
        }

        /// <summary>
        /// Finaliza la cámara
        /// </summary>
        public override void Finalizar()
        {
            // Nos desuscribimos al cambio de estatus de la cámara
            this.Display.dispAxis.OnError -= DispAxis_OnError;
            this.Display.dispAxis.OnStatusChange -= DispAxis_OnStatusChange;

            // Finalizamos la comprobación de la grabación máxima
            this.TimerTiempoMaxGrabacion.Tick -= this.TimerTiempoMaxGrabacion_Tick;
            this.TimerTiempoMaxGrabacion.Stop();

            // Finalizamos la comprobación de la conectividad con la cámara
            this.TimerComprobacionConexion.Tick -= this.TimerComprobacionConexion_Tick;
            this.TimerComprobacionConexion.Stop();
            this.Ping.SendAsyncCancel();

            base.Finalizar();
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
                    // Verificamos que la cámara está conectada
                    if (this.Ping.Send(this.IP).Status != IPStatus.Success)
                    {
                        LogsRuntime.Error(ModulosHardware.CamaraAxis, this.Codigo, "Problema de conexión con la cámara.");
                        this.EstadoConexion = EstadoConexion.ErrorConexion;
                        return false;
                    }

                    //Stops possible streams
                    this.Display.dispAxis.Stop();

                    // Set the PTZ properties
                    this.Display.dispAxis.PTZControlURL = "http://" + this.IP + "/axis-cgi/com/ptz.cgi";
                    this.Display.dispAxis.UIMode = "ptz-absolute";

                    // Show the status bar and the tool bar in the AXIS Media Control
                    this.Display.dispAxis.ShowStatusBar = false;
                    this.Display.dispAxis.ShowToolbar = false;
                    this.Display.dispAxis.StretchToFit = true;
                    this.Display.dispAxis.EnableAreaZoom = true;
                    this.Display.dispAxis.EnableContextMenu = true;
                    this.Display.dispAxis.ToolbarConfiguration = "default,+ptz";

                    // Set the media URL and the media type
                    this.Display.dispAxis.MediaURL = "http://" + this.IP + "/axis-cgi/mjpg/video.cgi";
                    this.Display.dispAxis.MediaType = "mjpeg";

                    // Uusario y contraseña
                    this.Display.dispAxis.MediaUsername = this.Usuario;
                    this.Display.dispAxis.MediaPassword = this.Constraseña;

                    this.Display.dispAxis.OnNewImage += new EventHandler(this.ImagenAdquirida);

                    this.EstadoConexion = EstadoConexion.Conectado;
                    //OrbitaImage inicio = new OrbitaImage();
                    //this.CargarImagenDeDisco(out inicio, Path.GetDirectoryName(Application.ExecutablePath) + "\\Inicio.bmp");

                    // Iniciamos la comprobación de la conectividad con la cámara
                    this.TimerComprobacionConexion.Start();

                    resultado = true;
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraAxis, this.Codigo, exception);
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
                if ((this.Existe) && (this.EstadoConexion == EstadoConexion.Conectado))
                {
                    //Stops possible streams
                    this.Display.dispAxis.Stop();

                    // Set the PTZ properties
                    this.Display.dispAxis.PTZControlURL = "";
                    this.Display.dispAxis.MediaURL = "";

                    this.Display.dispAxis.OnNewImage -= this.ImagenAdquirida;

                    this.EstadoConexion = EstadoConexion.Desconectado;
                    resultado = true;
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraAxis, this.Codigo, exception);
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

                    this.HayNuevaImagen = false;
                    this.Display.dispAxis.Play();
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraAxis, this.Codigo, exception);
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
                if (this.EstadoConexion == EstadoConexion.Conectado)
                {
                    //Stops possible streams
                    this.Display.dispAxis.Stop();

                    base.InternalStop();
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraAxis, this.Codigo, exception);
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
                if (this.EstadoConexion == EstadoConexion.Conectado)
                {
                    resultado = base.InternalSnap();

                    // Verificamos que la cámara está preparada para adquirir la imagen
                    if (!this.ReadyToAcquire)
                    {
                        LogsRuntime.Debug(ModulosHardware.CamaraAxis, this.Codigo, "La cámara no está preparada para adquirir imágenes");
                        return false;
                    }

                    // Se consulta la imágen de la cámara
                    BitmapImage bitmapImage;
                    resultado = this.GetCurrentImage(out bitmapImage);

                    // Se asigna el valor de la variable asociada
                    if (resultado)
                    {
                        this.EstablecerVariableAsociada(bitmapImage);
                    }
                }

                return resultado;
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraAxis, this.Codigo, exception);
            }
            return resultado;
        }

        /// <summary>
        /// Realiza una grabación de la camara que se encuentra en modo grab
        /// </summary>
        /// ruta y nombre del fichero que contendra el video
        /// <returns></returns>
        protected override bool InternalREC(string fichero)
        {
            if (this.TiempoMaxGrabacion.TotalMilliseconds > 0)
            {
                this.TimerTiempoMaxGrabacion.Stop();
                this.TimerTiempoMaxGrabacion.Start();
            }

            return this.Display.REC(fichero);
        }

        /// <summary>
        /// Termina una grabación continua de la cámara
        /// </summary>
        /// <returns></returns>
        protected override bool InternalStopREC()
        {
            bool resultado = false;

            this.TimerTiempoMaxGrabacion.Stop();

            if (this.EstadoConexion == EstadoConexion.Conectado)
            {
                //Stops possible recording 
                this.Display.StopREC();
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
            // Valores por defecto
            bool resultado = base.CargarImagenDeDisco(out imagen, ruta);

            // Se carga la imagen
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.Cargar(ruta);
            if (bitmapImage.EsValida())
            {
                // Se carga en el display
                this.SetCurrentImage(bitmapImage);

                // Devolvemos los valores
                imagen = bitmapImage;
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
            // Valores por defecto
            bool resultado = base.GuardarImagenADisco(ruta);

            BitmapImage bitmapImage = new BitmapImage();

            // Se carga en el display
            if (this.GetCurrentImage(out bitmapImage))
            {
                // Se guarda la imagen
                if (bitmapImage.EsValida())
                {
                    bitmapImage.Guardar(ruta);
                    resultado = true;
                }
            }

            return resultado;
        }

        /// <summary>
        /// Devuelve una nueva imagen del tipo adecuado al trabajo con la cámara
        /// </summary>
        /// <returns>Imagen del tipo adecuado al trabajo con la cámara</returns>
        public override OrbitaImage NuevaImagen()
        {
            BitmapImage bitmapImage = new BitmapImage(new Bitmap(this.Resolucion.X, this.Resolucion.Y));
            return bitmapImage;
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
        #endregion

        #region Eventos
        /// <summary>
        /// Evento de recepción de nueva imagen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImagenAdquirida(object sender, EventArgs e)
        {
            try
            {
                if (!ThreadRuntime.EjecucionEnTrheadPrincipal())
                {
                    ThreadRuntime.SincronizarConThreadPrincipal(new EventHandler(this.ImagenAdquirida), new object[] { sender, e });
                    return;
                }

                this.HayNuevaImagen = true;
            
                if (this.LanzarEventoAlSnap && (this.EstadoConexion == EstadoConexion.Conectado))
                {
                    BitmapImage bitmapImage = null;
                    bool resultado = this.Display.GetCurrentImage(out bitmapImage);
                    // Se asigna el valor de la variable asociada
                    if (resultado)
                    {
                        this.ImagenActual = bitmapImage;
                    }

                    // Actualizo el Frame Rate
                    this.MedidorVelocidadAdquisicion.NuevaCaptura();

                    // Visualización en vivo
                    if (this.VisualizacionEnVivo)
                    {
                        this.VisualizarUltimaImagen();
                    }

                    // Se asigna el valor de la variable asociada
                    if (resultado && this.ImagenActual.EsValida())
                    {
                        this.EstablecerVariableAsociada(this.ImagenActual);
                    }
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraAxis, this.Codigo, exception);
            }
        }

        /// <summary>
        /// Evento de error en la cámara axis
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DispAxis_OnError(object sender, AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnErrorEvent e)
        {
            LogsRuntime.Error(ModulosHardware.CamaraAxis, this.Codigo, string.Format("Código de error: {0}, Información: {1}", e.theErrorCode, e.theErrorInfo));
        }

        /// <summary>
        /// Cambio de estado en la cámara axis
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DispAxis_OnStatusChange(object sender, AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnStatusChangeEvent e)
        {
            try
            {
                LogsRuntime.Debug(ModulosHardware.CamaraAxis, this.Codigo, string.Format("Estado anterior: {0}, estado actual: {1}", e.theOldStatus, e.theNewStatus));

                EstadoCamaraAxis estadoCamaraAxisOld = new EstadoCamaraAxis(e.theOldStatus);
                this.EstadoCamaraAxis = new EstadoCamaraAxis(e.theNewStatus);

                // Se detecta un fallo de conexión cuando pasa del estado (playing+opening --> !playing+!opening)
                if (!this.SimulacionCamara.Simulacion && estadoCamaraAxisOld.OPENING && estadoCamaraAxisOld.PLAYING && !this.EstadoCamaraAxis.PLAYING && !this.EstadoCamaraAxis.OPENING)
                {
                    this.EstadoConexion = EstadoConexion.ErrorConexion;
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraAxis, this.Codigo, exception);
            }
        }

        /// <summary>
        /// Evento del timer de parada de la grabación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerTiempoMaxGrabacion_Tick(object sender, EventArgs e)
        {
            this.TimerTiempoMaxGrabacion.Stop();

            try
            {
                this.StopREC();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraAxis, this.Codigo, exception);
            }
        }

        /// <summary>
        /// Evento del timer de comprobación de la conexión
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerComprobacionConexion_Tick(object sender, EventArgs e)
        {
            this.TimerComprobacionConexion.Stop();
            try
            {
                this.Ping.SendAsync(this.IP, TimeOutConexionMS, new object());
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraAxis, this.Codigo, exception);
            }
        }

        /// <summary>
        /// Evento de ping completado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PingCompletedEventHandler(object sender, PingCompletedEventArgs e)
        {
            try
            {
                if (e.Reply.Status == IPStatus.Success)
                {
                    if (this.EstadoConexion == EstadoConexion.ErrorConexion)
                    {
                        this.Conectar();
                    }
                }
                else if (this.EstadoConexion == VAHardware.EstadoConexion.Conectado)
                {
                    this.EstadoConexion = EstadoConexion.ErrorConexion;
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraAxis, this.Codigo, exception);
            }
            this.TimerComprobacionConexion.Start();
        }
        #endregion
    }

    /// <summary>
    /// Terminal de tipo bit que simboliza un bit de un puerto
    /// </summary>
    internal class TerminalIOAxisBit : TerminalIOBase
    {
        #region Atributo(s)
        /// <summary>
        /// IP de la camara
        /// </summary>
        private IPAddress IP;
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
        public TerminalIOAxisBit(string codTarjetaIO, string codTerminalIO, IPAddress iP)
            : base(codTarjetaIO, codTerminalIO)
        {
            this.IP = iP;
        }
        #endregion

        #region Método(s) heredado(s)

        /// <summary>
        /// Método a heredar donde se inicializan los terminales
        /// </summary>
        public new void Inicializar()
        {
            base.Inicializar();

            if (this.TipoTerminalIO == TipoTerminalIO.SalidaDigital)
            {
                // Creamos la respuesta a la dirección del parámetro de salida para que se ponga en off		
                string cadenaComando = "http://" + this.IP.ToString() + "/axis-cgi/io/input.cgi?action=" + this.Numero.ToString() + @"\";
                WebRequest request = WebRequest.Create(cadenaComando);
                // Configuramos las credenciales
                request.Credentials = CredentialCache.DefaultCredentials;
                // Obtenemos la respuesta
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            }
        }

        /// <summary>
        /// Lectura de la entrada física
        /// </summary>
        public override void LeerEntrada()
        {
            base.LeerEntrada();
            bool boolValor = this.Valor;

            if (this.Habilitado && (this.TipoTerminalIO == TipoTerminalIO.EntradaDigital))
            {
                // Leo la entrada fisica
                // Creamos la respuesta a la dirección del parámetro 		
                string cadenaComando = "http://" + this.IP + "/axis-cgi/io/input.cgi?check=" + this.Numero.ToString();
                WebRequest request = WebRequest.Create(cadenaComando);
                // Configuramos las credenciales
                request.Credentials = CredentialCache.DefaultCredentials;
                // Obtenemos la respuesta
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Comprobamos el estado de la petición
                if (response.StatusDescription == "OK")
                {
                    // Conseguimos el dato de la respuesta
                    Stream dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Leemos el contenido
                    string respuestaDelServidor = reader.ReadToEnd();
                    // Interpretamos la respuesta
                    
                    if (respuestaDelServidor.Trim() == "input" + this.Numero.ToString() + "=1")
                    {
                        boolValor = true;
                    }
                    else
                    {
                        boolValor = false;
                    }
                    // Limpiamos las variables utilizadas
                    reader.Close();
                    dataStream.Close();
                }
                else
                {
                    //FALLO DE COMUNICACION
                    LogsRuntime.Error(ModulosHardware.CamaraAxis, "LeerEntrada", "Error: Comunicación IP");
                }
                response.Close();

                if (this.Valor != boolValor)
                {
                    this.Valor = boolValor;
                    this.EstablecerValorVariable();
                    //VariableRuntime.SetValue(this.CodVariable, this.Valor, "Camaras", this.Codigo);
                }
            }
        }

        /// <summary>
        /// Lectura de la salida física
        /// </summary>
        public override void LeerSalida()
        {
            base.LeerEntrada();
            bool boolValor = this.Valor;

            if (this.Habilitado && (this.TipoTerminalIO == TipoTerminalIO.SalidaDigital))
            {
                // Leo la entrada fisica
                // Creamos la respuesta a la dirección del parámetro 		
                string cadenaComando = "http://" + this.IP + "/axis-cgi/io/output.cgi?check=" + this.Numero.ToString();
                WebRequest request = WebRequest.Create(cadenaComando);
                // Configuramos las credenciales
                request.Credentials = CredentialCache.DefaultCredentials;
                // Obtenemos la respuesta
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Comprobamos el estado de la petición
                if (response.StatusDescription == "OK")
                {
                    // Conseguimos el dato de la respuesta
                    Stream dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Leemos el contenido
                    string respuestaDelServidor = reader.ReadToEnd();
                    // Interpretamos la respuesta

                    if (respuestaDelServidor == "output" + this.Numero.ToString() + "=1")
                    {
                        boolValor = true;
                    }
                    else
                    {
                        boolValor = false;
                    }
                    // Limpiamos las variables utilizadas
                    reader.Close();
                    dataStream.Close();
                }
                else
                {
                    //FALLO DE COMUNICACION
                    LogsRuntime.Error(ModulosHardware.CamaraAxis, "LeerSalida", "Error: Comunicación IP");
                }
                response.Close();

                if (this.Valor != boolValor)
                {
                    this.Valor = boolValor;
                    this.EstablecerValorVariable();
                    //VariableRuntime.SetValue(this.CodVariable, this.Valor, "Camaras", this.Codigo);
                }
            }
        }

        /// <summary>
        /// Escritura de la salida física
        /// </summary>
        public override void EscribirSalida(string codigoVariable, object valor)
        {
            base.EscribirSalida(codigoVariable, valor);

            // Se comprueba que el valor a escribir sea correcto
            bool boolValor;
            if (this.ComprobarValor(valor, out boolValor))
            {
                // Si el valor es correcto se escribe la salida física
                if (this.Habilitado && (this.TipoTerminalIO == TipoTerminalIO.SalidaDigital))
                {
                    // Creamos la respuesta a la dirección del parámetro 	
                    string cadenaComando = "http://" + this.IP + "/axis-cgi/io/output.cgi?action=" + this.Numero.ToString();
                    if (boolValor)
                    {
                        cadenaComando += @"/";
                    }
                    else
                    {
                        cadenaComando += @"\";
                    }
                    WebRequest request = WebRequest.Create(cadenaComando);
                    // Configuramos las credenciales
                    request.Credentials = CredentialCache.DefaultCredentials;
                    // Obtenemos la respuesta
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    // Comprobamos el estado de la petición
                    if (response.StatusDescription != "OK")
                    {
                        //FALLO DE COMUNICACION
                        LogsRuntime.Error(ModulosHardware.CamaraAxis, "EscribirSalida", "Error: Comunicación IP");
                    }
                    response.Close();

                }
                this.Valor = boolValor;
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
}
