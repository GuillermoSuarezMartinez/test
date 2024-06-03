//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 05-11-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
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
using System.Diagnostics;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Clase que implementa las funciones de manejo de la cámara IP
    /// </summary>
    public class CamaraIP : CamaraBase
    {
        #region Constante(s)
        public const int TimeOutConexionMS = 1000;
        #endregion

        #region Atributo(s)
        /// <summary>
        /// Dirección URL original
        /// </summary>
        private string URLOriginal;
        /// <summary>
        /// Dirección URL del video
        /// </summary>
        private string URL;
        /// <summary>
        /// Dirección IP de la cámara
        /// </summary>
        private IPAddress IP;
        /// <summary>
        /// Puerto de conexión con la cámara
        /// </summary>
        private int Puerto;
        /// <summary>
        /// Usuario para el acceso a la cámara
        /// </summary>
        private string Usuario;
        /// <summary>
        /// Contraseña para el acceso a la cámara
        /// </summary>
        private string Constraseña;
        /// <summary>
        /// Fuente de video
        /// </summary>
        private IVideoSource VideoSource;
        /// <summary>
        /// Informa si hay adquirida una nueva imagen
        /// </summary>
        private bool HayNuevaImagen;
        /// <summary>
        /// Timer de comprobación del estado de la conexión
        /// </summary>
        private Timer TimerComprobacionConexion;
        /// <summary>
        /// Ping utilizado para detectar la conectividad con la cámara
        /// </summary>
        private Ping Ping;
        /// <summary>
        /// Cronómetro del tiempo sin respuesta de la cámara
        /// </summary>
        private Stopwatch CronometroTiempoSinRespuestaCamara;
        /// <summary>
        /// Indica si la cámara estaba en estado de reproducción continua antes del error de desconexión.
        /// En caso de volver a conectarse, automáticamente se dispondrá en modo reproducción continua.
        /// </summary>
        private bool EnReproduccionAntesErrorConexion;
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
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>       
        public CamaraIP(string codigo)
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
                    this.Puerto = App.EvaluaNumero(dt.Rows[0]["IPCam_Puerto"], 0, int.MaxValue, 80);
                    this.Usuario = dt.Rows[0]["IPCam_Usuario"].ToString();
                    this.Constraseña = dt.Rows[0]["IPCam_Contraseña"].ToString();
                    this.URLOriginal = dt.Rows[0]["IPCam_URL"].ToString();

                    // Creación del vido source
                    string strVideoSource = dt.Rows[0]["IPCam_OrigenVideo"].ToString();
                    TipoOrigenVideo tipoOrigenVideo = (TipoOrigenVideo)App.EnumParse(typeof(TipoOrigenVideo), strVideoSource, TipoOrigenVideo.JPG);
                    switch (tipoOrigenVideo)
                    {
                        case TipoOrigenVideo.MJPG:
                            this.VideoSource = new MJPEGSource();
                            break;
                        case TipoOrigenVideo.JPG:
                        default:
                            this.VideoSource = new JPEGSource();
                            break;
                    }

                    // Creación del timer de comprobación de la conexión
                    this.TimerComprobacionConexion = new Timer();
                    this.TimerComprobacionConexion.Interval = TimeOutConexionMS;
                    this.TimerComprobacionConexion.Enabled = false;
                    this.TimerComprobacionConexion.Tick += this.TimerComprobacionConexion_Tick;

                    // Creación del ping para detectar la conectividad con la cámara
                    this.Ping = new Ping();
                    this.Ping.PingCompleted += this.PingCompletedEventHandler;

                    // Creación del cronómetro de tiempo de espera sin respuesta de la cámara
                    this.CronometroTiempoSinRespuestaCamara = new Stopwatch();

                    this.Existe = true;
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Fatal(ModulosHardware.Camaras, this.Codigo, exception);
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
                resultado = true;
                bitmapImage = this.ImagenActual;
                this.HayNuevaImagen = false;
            }

            return resultado;
        }

        /// <summary>
        /// Se importa una imagen a la cámara
        /// </summary>
        /// <returns></returns>
        private bool SetCurrentImage(BitmapImage bitmapImage)
        {
            this.ImagenActual = bitmapImage;
            return true;
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
                    // Construcción de la url
                    string url = this.URLOriginal;
                    url = App.StringReplace(url, @"%IPCam_IP%", this.IP.ToString(), StringComparison.OrdinalIgnoreCase);
                    url = App.StringReplace(url, @"%IPCam_Puerto%", this.Puerto.ToString(), StringComparison.OrdinalIgnoreCase);
                    url = App.StringReplace(url, @"%IPCam_Usuario%", this.Usuario, StringComparison.OrdinalIgnoreCase);
                    url = App.StringReplace(url, @"%IPCam_Contraseña%", this.Constraseña, StringComparison.OrdinalIgnoreCase);
                    url = App.StringReplace(url, @"%ResolucionX%", this.Resolucion.X.ToString(), StringComparison.OrdinalIgnoreCase);
                    url = App.StringReplace(url, @"%ResolucionY%", this.Resolucion.Y.ToString(), StringComparison.OrdinalIgnoreCase);
                    int frameInterval = (int)Math.Truncate(this.FrameInterval);
                    if (frameInterval > 0)
                    {
                        int fps = (int)(1000 / Math.Min(frameInterval, 1000));
                        url = App.StringReplace(url, @"%FrameIntervalMs%", frameInterval.ToString(), StringComparison.OrdinalIgnoreCase);
                        url = App.StringReplace(url, @"%fps%", fps.ToString(), StringComparison.OrdinalIgnoreCase);
                    }
                    this.URL = url;

                    // Parametrización del videosource
                    this.VideoSource.Login = this.Usuario;
                    this.VideoSource.Password = this.Constraseña;
                    this.VideoSource.VideoSource = url;

                    // Detengo videosource actual
                    this.VideoSource.SignalToStop();
                    App.Espera(delegate() { return !this.VideoSource.Running; }, 1000);
                    this.VideoSource.Stop();

                    // Nos suscribimos a la recepción de imágenes de la cámara
                    this.VideoSource.NewFrame += this.ImagenAdquirida;
                    this.VideoSource.OnCameraError += ErrorAdquisicion;
                    
                    this.EstadoConexion = EstadoConexion.Conectado;

                    // Iniciamos la comprobación de la conectividad con la cámara
                    this.TimerComprobacionConexion.Start();

                    // Verificamos que la cámara está conectada
                    if (this.Ping.Send(this.IP).Status != IPStatus.Success)
                    {
                        LogsRuntime.Error(ModulosHardware.Camaras, this.Codigo, "Problema de conexión con la cámara.");
                        this.EstadoConexion = EstadoConexion.ErrorConexion;
                        return false;
                    }

                    resultado = true;
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.Camaras, this.Codigo, exception);
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
                    // Nos dessuscribimos a la recepción de imágenes de la cámara
                    this.VideoSource.NewFrame -= this.ImagenAdquirida;
                    this.VideoSource.OnCameraError -= ErrorAdquisicion;

                    // Detengo videosource actual
                    this.VideoSource.SignalToStop();
                    App.Espera(delegate() { return !this.VideoSource.Running; }, 1000);
                    this.VideoSource.Stop();

                    this.EstadoConexion = EstadoConexion.Desconectado;
                    resultado = true;
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.Camaras, this.Codigo, exception);
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
                    this.VideoSource.Start();
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.Camaras, this.Codigo, exception);
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
                    // Detengo videosource actual
                    this.VideoSource.SignalToStop();
                    App.Espera(delegate() { return !this.VideoSource.Running; }, 1000);
                    this.VideoSource.Stop();

                    base.InternalStop();
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.Camaras, this.Codigo, exception);
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
                    if (!this.VideoSource.Running)
                    {
                        LogsRuntime.Debug(ModulosHardware.Camaras, this.Codigo, "La cámara no está preparada para adquirir imágenes");
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
                LogsRuntime.Error(ModulosHardware.Camaras, this.Codigo, exception);
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
        #endregion

        #region Eventos
        /// <summary>
        /// Evento de recepción de nueva imagen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImagenAdquirida(object sender, CameraEventArgs e)
        {
            try
            {
                if (!ThreadRuntime.EjecucionEnTrheadPrincipal())
                {
                    ThreadRuntime.SincronizarConThreadPrincipal(new CameraEventHandler(this.ImagenAdquirida), new object[] { sender, e });
                    return;
                }

                this.HayNuevaImagen = true;
            
                if (this.EstadoConexion == EstadoConexion.Conectado)
                {
                    this.ImagenActual = new BitmapImage();
                    this.ImagenActual.Image = (Bitmap)e.Bitmap.Clone();

                    // Actualizo el tiempo sin respuesta de la cámara
                    this.CronometroTiempoSinRespuestaCamara.Stop();
                    this.CronometroTiempoSinRespuestaCamara.Reset();
                    this.CronometroTiempoSinRespuestaCamara.Start();

                    // Actualizo el Frame Rate
                    this.MedidorVelocidadAdquisicion.NuevaCaptura();

                    // Lanamos el evento de adquisición
                    this.AdquisicionCompletada(this.ImagenActual);

                    // Se asigna el valor de la variable asociada
                    if (this.LanzarEventoAlSnap && this.ImagenActual.EsValida())
                    {
                        this.EstablecerVariableAsociada(this.ImagenActual);
                    }
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.Camaras, this.Codigo, exception);
            }
        }

        /// <summary>
        /// Evento de recepción de nueva imagen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ErrorAdquisicion(object sender, CameraErrorEventArgs e)
        {
            try
            {
                if (!ThreadRuntime.EjecucionEnTrheadPrincipal())
                {
                    ThreadRuntime.SincronizarConThreadPrincipal(new CameraErrorEventHandler(this.ErrorAdquisicion), new object[] { sender, e });
                    return;
                }

                this.EstadoConexion = VAHardware.EstadoConexion.ErrorConexion;
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.Camaras, this.Codigo, exception);
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
                // Tiempo máximo entre snaps
                if (this.CronometroTiempoSinRespuestaCamara.ElapsedMilliseconds > TimeOutConexionMS)
                {
                    this.Ping.SendAsync(this.IP, TimeOutConexionMS, new object());
                    //bool ok = App.HttpPing(string.Format("www.{0}:{1}", this.IP, this.Puerto));
                    //if (ok)
                    //    {
                    //        if (this.EstadoConexion == EstadoConexion.ErrorConexion)
                    //        {
                    //            this.Conectar();
                    //            if (this.EnReproduccionAntesErrorConexion)
                    //            {
                    //                this.Start();
                    //            }
                    //        }
                    //    }
                    //else if (this.EstadoConexion == VAHardware.EstadoConexion.Conectado)
                    //{
                    //    this.EnReproduccionAntesErrorConexion = this.Grab;
                    //    this.EstadoConexion = EstadoConexion.ErrorConexion;
                    //}
                    //this.TimerComprobacionConexion.Start();
                }
                else
                {
                    this.TimerComprobacionConexion.Start();
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.Camaras, this.Codigo, exception);
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
                        if (this.EnReproduccionAntesErrorConexion)
                        {
                            this.Start();
                        }
                    }
                }
                else if (this.EstadoConexion == VAHardware.EstadoConexion.Conectado)
                {
                    this.EnReproduccionAntesErrorConexion = this.Grab;
                    this.EstadoConexion = EstadoConexion.ErrorConexion;
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.Camaras, this.Codigo, exception);
            }
            this.TimerComprobacionConexion.Start();
        }
        #endregion
    }
}
