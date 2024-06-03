//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 05-11-2012
//
// Last Modified By : aibañez
// Last Modified On : 12-12-2012
// Description      : Heredado de la clase CamaraBitmap
//                    Modificada la forma de conexión desconexión
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Data;
using System.Drawing;
using System.Net;
using Orbita.VAComun;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Clase que implementa las funciones de manejo de la cámara IP
    /// </summary>
    public class OCamaraIP : OCamaraBitmap
    {
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
        private string Contraseña;
        /// <summary>
        /// Fuente de video
        /// </summary>
        private IVideoSource VideoSource;
        /// <summary>
        /// Inervalo entre comprobaciones de conectividad con la cámara IP
        /// </summary>
        private int IntervaloComprobacionConectividadMS;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>       
        public OCamaraIP(string codigo)
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
                    this.Puerto = OEnteroRobusto.Validar(dt.Rows[0]["IPCam_Puerto"], 0, int.MaxValue, 80);
                    this.Usuario = dt.Rows[0]["IPCam_Usuario"].ToString();
                    this.Contraseña = dt.Rows[0]["IPCam_Contraseña"].ToString();
                    this.URLOriginal = dt.Rows[0]["IPCam_URL"].ToString();
                    this.IntervaloComprobacionConectividadMS = OEnteroRobusto.Validar(dt.Rows[0]["IPCam_IntervaloComprobacionConectividadMS"], 1, int.MaxValue, 100);

                    // Construcción de la url
                    string url = this.URLOriginal;
                    url = App.StringReplace(url, @"%IPCam_IP%", this.IP.ToString(), StringComparison.OrdinalIgnoreCase);
                    url = App.StringReplace(url, @"%IPCam_Puerto%", this.Puerto.ToString(), StringComparison.OrdinalIgnoreCase);
                    url = App.StringReplace(url, @"%IPCam_Usuario%", this.Usuario, StringComparison.OrdinalIgnoreCase);
                    url = App.StringReplace(url, @"%IPCam_Contraseña%", this.Contraseña, StringComparison.OrdinalIgnoreCase);
                    url = App.StringReplace(url, @"%ResolucionX%", this.Resolucion.Width.ToString(), StringComparison.OrdinalIgnoreCase);
                    url = App.StringReplace(url, @"%ResolucionY%", this.Resolucion.Height.ToString(), StringComparison.OrdinalIgnoreCase);
                    int fps = (int)Math.Ceiling(this.ExpectedFrameRate);
                    url = App.StringReplace(url, @"%FrameIntervalMs%", fps.ToString(), StringComparison.OrdinalIgnoreCase);
                    url = App.StringReplace(url, @"%fps%", fps.ToString(), StringComparison.OrdinalIgnoreCase);
                    this.URL = url;

                    // Creación del vido source
                    string strVideoSource = dt.Rows[0]["IPCam_OrigenVideo"].ToString();
                    TipoOrigenVideo tipoOrigenVideo = OEnumRobusto<TipoOrigenVideo>.Validar(strVideoSource, TipoOrigenVideo.JPG);
                    //TipoOrigenVideo tipoOrigenVideo = (TipoOrigenVideo)App.EnumParse(typeof(TipoOrigenVideo), strVideoSource, TipoOrigenVideo.JPG);
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

                    this.Existe = true;
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Fatal(ModulosHardware.Camaras, this.Codigo, exception);
                throw new Exception("Imposible iniciar la cámara " + this.Codigo);
            }
        } 
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Se toma el control de la cámara
        /// </summary>
        /// <returns>Verdadero si la operación ha funcionado correctamente</returns>
        protected override bool ConectarInterno(bool reconexion)
        {
            bool resultado = base.ConectarInterno(reconexion);
            try
            {
                // Parametrización del videosource
                this.VideoSource.Login = this.Usuario;
                this.VideoSource.Password = this.Contraseña;
                this.VideoSource.VideoSource = this.URL;

                // Detengo videosource actual
                this.VideoSource.SignalToStop();
                OThread.Espera(delegate() { return !this.VideoSource.Running; }, 1000);
                this.VideoSource.Stop();

                // Nos suscribimos a la recepción de imágenes de la cámara
                this.VideoSource.NewFrame += this.ImagenAdquirida;
                this.VideoSource.OnCameraError += ErrorAdquisicion;

                resultado = true;
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(ModulosHardware.Camaras, this.Codigo, exception);
            }

            return resultado;
        }

        /// <summary>
        /// Se deja el control de la cámara
        /// </summary>
        /// <returns>Verdadero si la operación ha funcionado correctamente</returns>
        protected override bool DesconectarInterno(bool errorConexion)
        {
            bool resultado = base.DesconectarInterno(errorConexion);

            try
            {
                // Nos dessuscribimos a la recepción de imágenes de la cámara
                this.VideoSource.NewFrame -= this.ImagenAdquirida;
                this.VideoSource.OnCameraError -= ErrorAdquisicion;

                // Detengo videosource actual
                this.VideoSource.SignalToStop();
                OThread.Espera(delegate() { return !this.VideoSource.Running; }, 1000);
                this.VideoSource.Stop();

                resultado = true;
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(ModulosHardware.Camaras, this.Codigo, exception);
            }

            return resultado;
        }

        /// <summary>
        /// Comienza una reproducción continua de la cámara
        /// </summary>
        /// <returns></returns>
        protected override bool StartInterno()
        {
            bool resultado = false;

            try
            {
                if (this.EstadoConexion == EstadoConexion.Conectado)
                {
                    base.StartInterno();

                    this.HayNuevaImagen = false;
                    this.VideoSource.Start();

                    resultado = true;
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(ModulosHardware.Camaras, this.Codigo, exception);
            }

            return resultado;
        }

        /// <summary>
        /// Termina una reproducción continua de la cámara
        /// </summary>
        /// <returns></returns>
        protected override bool StopInterno()
        {
            bool resultado = false;

            try
            {
                if (this.EstadoConexion == EstadoConexion.Conectado)
                {
                    // Detengo videosource actual
                    this.VideoSource.SignalToStop();
                    OThread.Espera(delegate() { return !this.VideoSource.Running; }, 1000);
                    this.VideoSource.Stop();

                    base.StopInterno();

                    resultado = true;
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(ModulosHardware.Camaras, this.Codigo, exception);
            }

            return resultado;
        }

        /// <summary>
        /// Realiza una fotografía de forma sincrona
        /// </summary>
        /// <returns></returns>
        protected override bool SnapInterno()
        {
            bool resultado = false;
            try
            {
                if (this.EstadoConexion == EstadoConexion.Conectado)
                {
                    resultado = base.SnapInterno();

                    // Verificamos que la cámara está preparada para adquirir la imagen
                    if (!this.VideoSource.Running)
                    {
                        OVALogsManager.Debug(ModulosHardware.Camaras, this.Codigo, "La cámara no está preparada para adquirir imágenes");
                        return false;
                    }

                    // Se consulta la imágen de la cámara
                    OImagenBitmap bitmapImage;
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
                OVALogsManager.Error(ModulosHardware.Camaras, this.Codigo, exception);
            }
            return resultado;
        }

        /// <summary>
        /// Crea el objeto de conectividad adecuado para la cámara
        /// </summary>
        protected override void CrearConectividad()
        {
            // Creación de la comprobación de la conexión con la cámara IP
            this.Conectividad = new OConectividadIP(this.IP, this.IntervaloComprobacionConectividadMS);
        }
        #endregion

        #region Evento(s)
        /// <summary>
        /// Evento de recepción de nueva imagen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImagenAdquirida(object sender, CameraEventArgs e)
        {
            try
            {
                if (!OThreadManager.EjecucionEnTrheadPrincipal())
                {
                    OThreadManager.SincronizarConThreadPrincipal(new CameraEventHandler(this.ImagenAdquirida), new object[] { sender, e });
                    return;
                }

                this.HayNuevaImagen = true;
            
                if (this.EstadoConexion == EstadoConexion.Conectado)
                {
                    this.ImagenActual = new OImagenBitmap(this.Codigo);
                    this.ImagenActual.Image = (Bitmap)e.Bitmap.Clone();

                    // Actualizo la conectividad
                    this.Conectividad.EstadoConexion = EstadoConexion.Conectado;

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
                OVALogsManager.Error(ModulosHardware.Camaras, this.Codigo, exception);
            }
        }

        /// <summary>
        /// Evento de recepción de nueva imagen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ErrorAdquisicion(object sender, CameraErrorEventArgs e)
        {
            this.OnCambioEstadoConexionCamara(EstadoConexion.ErrorConexion);
        }
        #endregion

        #region Evento(s) heredado(s)
        /// <summary>
        /// Evento de error en la conexión con la cámara
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnCambioEstadoConectividadCamara(string codigo, EstadoConexion estadoConexionActal, EstadoConexion estadoConexionAnterior)
        {
            try
            {
                if (!OThreadManager.EjecucionEnTrheadPrincipal())
                {
                    OThreadManager.SincronizarConThreadPrincipal(new DelegadoCambioEstadoConexionCamaraAdv(this.OnCambioEstadoConectividadCamara), new object[] {codigo, estadoConexionActal, estadoConexionAnterior});
                    return;
                }

                base.OnCambioEstadoConectividadCamara(codigo, estadoConexionActal, estadoConexionAnterior);

                if ((estadoConexionActal == EstadoConexion.Reconectado) && (estadoConexionAnterior == EstadoConexion.Reconectando))
                {
                    this.Conectar(true);
                }
                else 
                if ((estadoConexionActal == EstadoConexion.ErrorConexion) && (estadoConexionAnterior == EstadoConexion.Conectado))
                {
                    this.Stop();
                    this.Desconectar(true);
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(ModulosHardware.Camaras, this.Codigo, exception);
            }
        }
        #endregion
    }
}
