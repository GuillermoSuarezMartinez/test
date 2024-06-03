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
                this.VideoSource.VideoSource = this.Dispostivo;

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
            
                if (this.EstadoConexion == EstadoConexion.Conectado)
                {
                    this.ImagenActual = new OImagenBitmap();
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
            try
            {
                if (!OThreadManager.EjecucionEnTrheadPrincipal())
                {
                    OThreadManager.SincronizarConThreadPrincipal(new CameraErrorEventHandler(this.ErrorAdquisicion), new object[] { sender, e });
                    return;
                }

                this.EstadoConexion = VAHardware.EstadoConexion.ErrorConexion;
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(ModulosHardware.Camaras, this.Codigo, exception);
            }
        }
        #endregion
    }
}
