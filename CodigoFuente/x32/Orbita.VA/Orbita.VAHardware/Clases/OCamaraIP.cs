﻿//***********************************************************************
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
                    this.Puerto = App.EvaluaNumero(dt.Rows[0]["IPCam_Puerto"], 0, int.MaxValue, 80);
                    this.Usuario = dt.Rows[0]["IPCam_Usuario"].ToString();
                    this.Contraseña = dt.Rows[0]["IPCam_Contraseña"].ToString();
                    this.URLOriginal = dt.Rows[0]["IPCam_URL"].ToString();

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

                    // Creación de la comprobación de la conexión con la cámara IP
                    this.IntervaloComprobacionConectividadMS = App.EvaluaNumero(dt.Rows[0]["IPCam_IntervaloComprobacionConectividadMS"], 1, int.MaxValue, 100);
                    this.Conectividad = new OConectividadIP(this.IP, this.IntervaloComprobacionConectividadMS);

                    this.Existe = true;
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Fatal(OModulosHardware.Camaras, this.Codigo, exception);
                throw new Exception("Imposible iniciar la cámara " + this.Codigo);
            }
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
        }

        /// <summary>
        /// Se toma el control de la cámara
        /// </summary>
        /// <returns>Verdadero si la operación ha funcionado correctamente</returns>
        protected override bool Conectar(bool reconexion)
        {
            bool resultado = base.Conectar(reconexion);
            try
            {
                if ((this.Existe) && (
                        (this.EstadoConexion == OEstadoConexion.Desconectado) ||
                        ((this.EstadoConexion != OEstadoConexion.Reconectando) && reconexion)))
                {
                    this.EstadoConexion = OEstadoConexion.Conectando;
                    
                    // Parametrización del videosource
                    this.VideoSource.Login = this.Usuario;
                    this.VideoSource.Password = this.Contraseña;
                    this.VideoSource.VideoSource = this.URL;

                    // Detengo videosource actual
                    this.VideoSource.SignalToStop();
                    App.Espera(delegate() { return !this.VideoSource.Running; }, 1000);
                    this.VideoSource.Stop();

                    // Nos suscribimos a la recepción de imágenes de la cámara
                    this.VideoSource.NewFrame += this.ImagenAdquirida;
                    this.VideoSource.OnCameraError += ErrorAdquisicion;

                    if (!reconexion)
                    {
                        this.EstadoConexion = OEstadoConexion.Conectado;

                        // Verificamos que la cámara está conectada
                        this.Conectividad.OnCambioEstadoConexion += this.OnCambioEstadoConectividadCamara;
                        if (!this.Conectividad.ForzarVerificacionConectividad())
                        {
                            this.EstadoConexion = OEstadoConexion.ErrorConexion;
                            resultado = false;
                        }

                        // Iniciamos la comprobación de la conectividad con la cámara
                        this.Conectividad.Start();
                    }

                    // Iniciamos el PTZ
                    this.PTZ.Inicializar();

                    resultado = true;
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.Camaras, this.Codigo, exception);
            }

            return resultado;
        }

        /// <summary>
        /// Se deja el control de la cámara
        /// </summary>
        /// <returns>Verdadero si la operación ha funcionado correctamente</returns>
        protected override bool Desconectar(bool errorConexion)
        {
            bool resultado = false;

            try
            {
                if ((this.Existe) && (
                        (this.EstadoConexion == OEstadoConexion.Conectado) ||
                        ((this.EstadoConexion != OEstadoConexion.ErrorConexion) && errorConexion)))
                {
                    this.EstadoConexion = OEstadoConexion.Desconectando;

                    // Nos dessuscribimos a la recepción de imágenes de la cámara
                    this.VideoSource.NewFrame -= this.ImagenAdquirida;
                    this.VideoSource.OnCameraError -= ErrorAdquisicion;

                    // Detengo videosource actual
                    this.VideoSource.SignalToStop();
                    App.Espera(delegate() { return !this.VideoSource.Running; }, 1000);
                    this.VideoSource.Stop();

                    if (!errorConexion)
                    {
                        // Finalizamos la comprobación de la conectividad con la cámara
                        this.Conectividad.OnCambioEstadoConexion -= this.OnCambioEstadoConectividadCamara;
                        this.Conectividad.Stop();
                    }

                    // Finalizamos el PTZ
                    this.PTZ.Finalizar();

                    this.EstadoConexion = OEstadoConexion.Desconectado;
                    resultado = true;
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.Camaras, this.Codigo, exception);
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
                if (this.EstadoConexion == OEstadoConexion.Conectado)
                {
                    base.InternalStart();

                    this.HayNuevaImagen = false;
                    this.VideoSource.Start();
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.Camaras, this.Codigo, exception);
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
                if (this.EstadoConexion == OEstadoConexion.Conectado)
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
                OVALogsManager.Error(OModulosHardware.Camaras, this.Codigo, exception);
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
                if (this.EstadoConexion == OEstadoConexion.Conectado)
                {
                    resultado = base.InternalSnap();

                    // Verificamos que la cámara está preparada para adquirir la imagen
                    if (!this.VideoSource.Running)
                    {
                        OVALogsManager.Debug(OModulosHardware.Camaras, this.Codigo, "La cámara no está preparada para adquirir imágenes");
                        return false;
                    }

                    // Se consulta la imágen de la cámara
                    OBitmapImage bitmapImage;
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
                OVALogsManager.Error(OModulosHardware.Camaras, this.Codigo, exception);
            }
            return resultado;
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
            
                if (this.EstadoConexion == OEstadoConexion.Conectado)
                {
                    this.ImagenActual = new OBitmapImage(this.Codigo);
                    this.ImagenActual.Image = (Bitmap)e.Bitmap.Clone();

                    // Actualizo la conectividad
                    this.Conectividad.EstadoConexion = OEstadoConexion.Conectado;

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
                OVALogsManager.Error(OModulosHardware.Camaras, this.Codigo, exception);
            }
        }

        /// <summary>
        /// Evento de recepción de nueva imagen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ErrorAdquisicion(object sender, CameraErrorEventArgs e)
        {
            this.OnCambioEstadoConexionCamara(OEstadoConexion.ErrorConexion);
        }
        #endregion

        #region Evento(s) heredado(s)
        /// <summary>
        /// Evento de error en la conexión con la cámara
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnCambioEstadoConectividadCamara(string codigo, OEstadoConexion estadoConexionActal, OEstadoConexion estadoConexionAnterior)
        {
            try
            {
                if (!OThreadManager.EjecucionEnTrheadPrincipal())
                {
                    OThreadManager.SincronizarConThreadPrincipal(new ODelegadoCambioEstadoConexionCamaraAdv(this.OnCambioEstadoConectividadCamara), new object[] {codigo, estadoConexionActal, estadoConexionAnterior});
                    return;
                }

                base.OnCambioEstadoConectividadCamara(codigo, estadoConexionActal, estadoConexionAnterior);

                if ((estadoConexionActal == OEstadoConexion.Conectado) && (estadoConexionAnterior == OEstadoConexion.ErrorConexion))
                {
                    this.Conectar(true);
                }

                if ((estadoConexionActal == OEstadoConexion.ErrorConexion) && (estadoConexionAnterior == OEstadoConexion.Conectado))
                {
                    this.Stop();
                    this.Desconectar(true);
                    this.EstadoConexion = OEstadoConexion.Reconectando;
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.Camaras, this.Codigo, exception);
            }
        }
        #endregion
    }
}