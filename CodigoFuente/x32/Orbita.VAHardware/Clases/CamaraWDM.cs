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
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using Orbita.VAComun;
using System.Diagnostics;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Clase que implementa las funciones de manejo de la dispositivo capturador genérico
    /// </summary>
    public class CamaraWDM : CamaraBase
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
        /// <summary>
        /// Informa si hay adquirida una nueva imagen
        /// </summary>
        private bool HayNuevaImagen;
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
        public CamaraWDM(string codigo)
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
                    this.VideoSource.VideoSource = this.Dispostivo;

                    // Detengo videosource actual
                    this.VideoSource.SignalToStop();
                    App.Espera(delegate() { return !this.VideoSource.Running; }, 1000);
                    this.VideoSource.Stop();

                    // Nos suscribimos a la recepción de imágenes de la cámara
                    this.VideoSource.NewFrame += this.ImagenAdquirida;
                    this.VideoSource.OnCameraError += ErrorAdquisicion;
                    
                    this.EstadoConexion = EstadoConexion.Conectado;

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
        #endregion
    }
}
