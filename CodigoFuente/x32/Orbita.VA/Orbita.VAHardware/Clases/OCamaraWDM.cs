//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 15-11-2012
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
using Orbita.VAComun;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Clase que implementa las funciones de manejo de la dispositivo capturador genérico
    /// </summary>
    public class OCamaraWDM : OCamaraBitmap
    {
        #region Constante(s)
        public const int TimeOutConexionMS = 1000;
        #endregion

        #region Atributo(s)
        /// <summary>
        /// Dirección URL original
        /// </summary>
        private string Dispostivo;
        /// <summary>
        /// Fuente de video
        /// </summary>
        private CaptureDevice VideoSource;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>       
        public OCamaraWDM(string codigo)
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
                    this.Dispostivo = dt.Rows[0]["WDM_NombreDispositivo"].ToString();

                    // Creación del vido source
                    this.VideoSource = new CaptureDevice();

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
            bool resultado = base.Conectar();
            try
            {
                if ((this.Existe) && (
                        (this.EstadoConexion == OEstadoConexion.Desconectado) ||
                        ((this.EstadoConexion != OEstadoConexion.Reconectando) && reconexion)))
                {
                    this.EstadoConexion = OEstadoConexion.Conectando;

                    this.VideoSource.VideoSource = this.Dispostivo;

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
                    }

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
                if (!OThreadManager.EjecucionEnTrheadPrincipal())
                {
                    OThreadManager.SincronizarConThreadPrincipal(new CameraEventHandler(this.ImagenAdquirida), new object[] { sender, e });
                    return;
                }

                this.HayNuevaImagen = true;
            
                if (this.EstadoConexion == OEstadoConexion.Conectado)
                {
                    this.ImagenActual = new OBitmapImage();
                    this.ImagenActual.Image = (Bitmap)e.Bitmap.Clone();

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
            try
            {
                if (!OThreadManager.EjecucionEnTrheadPrincipal())
                {
                    OThreadManager.SincronizarConThreadPrincipal(new CameraErrorEventHandler(this.ErrorAdquisicion), new object[] { sender, e });
                    return;
                }

                this.EstadoConexion = VAHardware.OEstadoConexion.ErrorConexion;
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.Camaras, this.Codigo, exception);
            }
        }
        #endregion
    }
}
