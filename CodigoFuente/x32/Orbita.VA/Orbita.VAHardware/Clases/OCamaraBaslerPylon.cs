//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 18-12-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Orbita.Trazabilidad;
using System.Windows.Forms;
using Orbita.Utiles;
using Orbita.VAComun;
using PylonC.NET;
using PylonC.NETSupportLibrary;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Clase que implementa las funciones de manejo de la cámara BaslerVPro
    /// </summary>
    public class OCamaraBaslerPylon : OCamaraBitmap
    {
        #region Atributo(s) estáticos
        private static List<DeviceEnumerator.Device> ListaDispositivos;
        /// <summary>
        /// Booleano que evita que se construya varias veces el listado de cámaras de tipo GigE
        /// </summary>
        public static bool PrimeraInstancia = true;
        #endregion

        #region Atributo(s)
        /// <summary>
        /// Proveedor de imagenes
        /// </summary>
        private ImageProvider ImageProvider;
        /// <summary>
        /// Bitmap temporal de guardado de imágenes
        /// </summary>
        private Bitmap TempBitmap;
        /// <summary>
        /// The friendly name of the device.
        /// </summary>
        public string Name;
        /// <summary>
        /// The index of the device.
        /// </summary>
        public uint Index;
        /// <summary>
        /// The serial of the device.
        /// </summary>
        public string Serial; 
        /// <summary>
        /// The type of device of the device.
        /// </summary>
        public string DeviceClass;
        /// <summary>
        /// The factory number of the device.
        /// </summary>
        public string DeviceFactory;
        /// <summary>
        /// The version number of the device.
        /// </summary>
        public string DeviceVersion;
        /// <summary>
        /// The full name of the device.
        /// </summary>
        public string FullName;
        /// <summary>
        /// The model name of the device.
        /// </summary>
        public string ModelName;
        /// <summary>
        /// The user defined name of the device.
        /// </summary>
        public string UserDefinedName;
        /// <summary>
        /// The vendor name of the device.
        /// </summary>
        public string VendorName;

        /// <summary>
        /// Timer de escaneo de las entradas
        /// </summary>
        private Timer TimerScan;
        /// <summary>
        /// Inervalo entre comprobaciones de conectividad con la cámara IP
        /// </summary>
        private int IntervaloComprobacionConectividadMS;
        /// <summary>
        /// Indica que la adquisición está siendo procesada en el momento actual
        /// </summary>
        private bool AdquisicionEnProceso;
        #endregion

        #region Propiedad(es)
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
        public OCamaraBaslerPylon(string codigo)
            : base(codigo)
        {
            try
            {
                // Inicialización de variables
                this.AdquisicionEnProceso = false;

                // Create one image provider.
                this.ImageProvider = new ImageProvider();

                Pylon.Initialize();

                /* Register for the events of the image provider needed for proper operation. */
                this.ImageProvider.GrabErrorEvent += new ImageProvider.GrabErrorEventHandler(OnGrabErrorEventCallback);
                this.ImageProvider.DeviceRemovedEvent += new ImageProvider.DeviceRemovedEventHandler(OnDeviceRemovedEventCallback);
                this.ImageProvider.DeviceOpenedEvent += new ImageProvider.DeviceOpenedEventHandler(OnDeviceOpenedEventCallback);
                this.ImageProvider.DeviceClosedEvent += new ImageProvider.DeviceClosedEventHandler(OnDeviceClosedEventCallback);
                this.ImageProvider.GrabbingStartedEvent += new ImageProvider.GrabbingStartedEventHandler(OnGrabbingStartedEventCallback);
                this.ImageProvider.ImageReadyEvent += new ImageProvider.ImageReadyEventHandler(OnImageReadyEventCallback);
                this.ImageProvider.GrabbingStoppedEvent += new ImageProvider.GrabbingStoppedEventHandler(OnGrabbingStoppedEventCallback);


                // Cargamos valores de la base de datos
                DataTable dt = AppBD.GetCamara(codigo);
                if (dt.Rows.Count == 1)
                {
                    // Rellenamos la información propia de la cámara
                    this._DeviceId = dt.Rows[0]["Basler_Pilot_DeviceID"].ToString();

                    // Se construye la lista de cámaras GigE
                    if (PrimeraInstancia)
                    {
                        // Se crea una lista de dispositivos
                        ListaDispositivos = DeviceEnumerator.EnumerateDevices();
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
                OVALogsManager.Fatal(ModulosHardware.CamaraBalserPylon, this.Codigo, exception);
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

            /* Fore each device to the list. */
            foreach (DeviceEnumerator.Device dispositivo in ListaDispositivos)
            {
                if (this._DeviceId == dispositivo.Serial)
                {
                    this.Name = dispositivo.Name;
                    this.Index = dispositivo.Index;
                    this.Serial = dispositivo.Serial;
                    this.DeviceClass = dispositivo.DeviceClass; 
                    this.DeviceFactory = dispositivo.DeviceFactory; 
                    this.DeviceVersion = dispositivo.DeviceVersion; 
                    this.FullName = dispositivo.FullName;
                    this.ModelName = dispositivo.ModelName; 
                    this.UserDefinedName = dispositivo.UserDefinedName;
                    this.VendorName = dispositivo.VendorName;

                    resultado = true;
                    break;
                }
            }

            return resultado;
        }
        #endregion

        #region Método(s) heredado(s)
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
        protected override bool ConectarInterno(bool reconexion)
        {
            bool resultado = base.ConectarInterno(reconexion);
            try
            {
                /* Open the image provider using the index from the device data. */
                this.ImageProvider.Open(this.Index);

                OImage inicio = new OImage();

                resultado = true;
            }
            catch (OCameraConectionException exception)
            {
                OVALogsManager.Error(ModulosHardware.CamaraBalserPylon, this.Codigo, "Problema de conexión con la cámara " + this.Codigo + ": " + exception.ToString());
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(ModulosHardware.CamaraBalserPylon, this.Codigo, "Error al conectarse a la cámara " + this.Codigo + ": " + exception.ToString());
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
                this.ImageProvider.Stop();
                this.ImageProvider.Close();

                resultado = true;
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(ModulosHardware.CamaraBalserPylon, this.Codigo, exception);
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

                    // Indicamos que no existe ninguna adquisición ejecutandose en estos momentos
                    this.AdquisicionEnProceso = false;

                    /* Start the grabbing of images until grabbing is stopped. */
                    this.ImageProvider.ContinuousShot(); 

                    resultado = true;
                }
            }
            catch (OCameraConectionException exception)
            {
                OVALogsManager.Error(ModulosHardware.CamaraBalserPylon, this.Codigo, "Problema de conexión con la cámara " + this.Codigo + ": " + exception.ToString());
                this.EstadoConexion = EstadoConexion.ErrorConexion;
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(ModulosHardware.CamaraBalserPylon, this.Codigo, exception);
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

                if (this.EstadoConexion != EstadoConexion.Desconectado)
                {
                    //this.TimerScan.Stop();

                    this.ImageProvider.Close(); 

                    // Indicamos que no existe ninguna adquisición ejecutandose en estos momentos
                    this.AdquisicionEnProceso = false;

                    base.StopInterno();
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(ModulosHardware.CamaraBalserPylon, this.Codigo, exception);
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
                    base.SnapInterno();

                    this.ImageProvider.OneShot(); /* Starts the grabbing of one image. */
                }
            }
            catch (OCameraConectionException exception)
            {
                OVALogsManager.Error(ModulosHardware.CamaraBalserPylon, this.Codigo, "Problema de conexión con la cámara " + this.Codigo + ": " + exception.ToString());
                this.EstadoConexion = EstadoConexion.ErrorConexion;
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(ModulosHardware.CamaraBalserPylon, this.Codigo, exception);
            }
            return resultado;
        }

        /// <summary>
        /// Crea el objeto de conectividad adecuado para la cámara
        /// </summary>
        protected override void CrearConectividad()
        {
            // Creación de la comprobación de la conexión con la cámara Basler
            this.Conectividad = new OConectividad(this.Codigo);
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Handles the event related to the occurrence of an error while grabbing proceeds.
        /// </summary>
        /// <param name="grabException"></param>
        /// <param name="additionalErrorMessage"></param>
        private void OnGrabErrorEventCallback(Exception grabException, string additionalErrorMessage)
        {
            if (!OThreadManager.EjecucionEnTrheadPrincipal())
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                OThreadManager.SincronizarConThreadPrincipal(new ImageProvider.GrabErrorEventHandler(OnGrabErrorEventCallback), new object[] {grabException, additionalErrorMessage});
                return;
            }

            OVALogsManager.Error(ModulosHardware.CamaraBalserPylon, "Error de adquisición", grabException, additionalErrorMessage);
        }

        /// <summary>
        /// Handles the event related to the removal of a currently open device.
        /// </summary>
        private void OnDeviceRemovedEventCallback()
        {
            if (!OThreadManager.EjecucionEnTrheadPrincipal())
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                OThreadManager.SincronizarConThreadPrincipal(new ImageProvider.DeviceRemovedEventHandler(OnDeviceRemovedEventCallback), null);
                return;
            }
            ///* Disable the buttons. */
            //EnableButtons(false, false);
            ///* Stops the grabbing of images. */
            //Stop();
            ///* Close the image provider. */
            //CloseTheImageProvider();
            ///* Since one device is gone, the list needs to be updated. */
            //UpdateDeviceList();
        }

        /// <summary>
        /// Handles the event related to a device being open.
        /// </summary>
        private void OnDeviceOpenedEventCallback()
        {
            if (!OThreadManager.EjecucionEnTrheadPrincipal())
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                OThreadManager.SincronizarConThreadPrincipal(new ImageProvider.DeviceOpenedEventHandler(OnDeviceOpenedEventCallback), null);
                return;
            }
            /* The image provider is ready to grab. Enable the grab buttons. */
            //EnableButtons(true, false);
        }

        /// <summary>
        /// Handles the event related to a device being closed.
        /// </summary>
        private void OnDeviceClosedEventCallback()
        {
            if (!OThreadManager.EjecucionEnTrheadPrincipal())
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                OThreadManager.SincronizarConThreadPrincipal(new ImageProvider.DeviceClosedEventHandler(OnDeviceClosedEventCallback), null);
                return;
            }
            /* The image provider is closed. Disable all buttons. */
            //EnableButtons(false, false);
        }

        /// <summary>
        /// Handles the event related to the image provider executing grabbing.
        /// </summary>
        private void OnGrabbingStartedEventCallback()
        {
            if (!OThreadManager.EjecucionEnTrheadPrincipal())
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                OThreadManager.SincronizarConThreadPrincipal(new ImageProvider.GrabbingStartedEventHandler(OnGrabbingStartedEventCallback), null);
                return;
            }
            /* The image provider is grabbing. Disable the grab buttons. Enable the stop button. */
            //EnableButtons(false, true);
        }

        /// <summary>
        /// Handles the event related to an image having been taken and waiting for processing.
        /// </summary>
        private void OnImageReadyEventCallback()
        {
            if (!OThreadManager.EjecucionEnTrheadPrincipal())
            {
                /* Suspend the grab thread for a while to avoid blocking the computer by using up all processor resources. */
                //System.Threading.Thread.Sleep(20); /* This is only required for this sample. */
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                OThreadManager.SincronizarConThreadPrincipal(new ImageProvider.ImageReadyEventHandler(OnImageReadyEventCallback), null);
                return;
            }

            // indicamos que se está procesando una adquisición
            this.AdquisicionEnProceso = true;

            try
            {
                if (this.EstadoConexion == EstadoConexion.Conectado)
                {
                    /* Acquire the image from the image provider. */
                    ImagePylon image = this.ImageProvider.GetCurrentImage();

                    /* Check if the image has been removed in the meantime. */
                    if (image != null)
                    {
                        /* Check if the image is compatible with the currently used bitmap. */
                        if (BitmapFactory.IsCompatible(this.TempBitmap, image.Width, image.Height, image.Color))
                        {
                            /* Update the bitmap with the image data. */
                            BitmapFactory.UpdateBitmap(this.TempBitmap, image.Buffer, image.Width, image.Height, image.Color);
                        }
                        else /* A new bitmap is required. */
                        {
                            BitmapFactory.CreateBitmap(ref this.TempBitmap, image.Width, image.Height, image.Color);
                            BitmapFactory.UpdateBitmap(this.TempBitmap, image.Buffer, image.Width, image.Height, image.Color);
                        }
                        /* The processing of the image is done. Release the image buffer. */
                        this.ImageProvider.ReleaseImage();
                        /* The buffer can be used for the next image grabs. 
                         If another image is in the output queue it can be acquired now using GetCurrentImage(). */
                    }

                    this.ImagenActual = new OImagenBitmap(this.Codigo);
                    this.ImagenActual.Image = (Bitmap)this.TempBitmap.Clone();

                    // Comprobación de que la imagen recibida de la cámara es correcta
                    if ((this.ImagenActual.Image == null) || (this.ImagenActual.Image.Width <= 0) || (this.ImagenActual.Image.Height <= 0))
                    {
                        throw new Exception(string.Format("La imagen recibida de la cámara {0} está corrupta.", this.Codigo));
                    }

                    // Actualizo la conectividad
                    this.Conectividad.EstadoConexion = EstadoConexion.Conectado;

                    // Actualizo el Frame Rate
                    this.MedidorVelocidadAdquisicion.NuevaCaptura();

                    // Lanamos el evento de adquisición
                    this.AdquisicionCompletada(this.ImagenActual);

                    // Se asigna el valor de la variable asociada
                    if (this.LanzarEventoAlSnap && (ImagenActual.EsValida()))
                    {
                        this.EstablecerVariableAsociada(ImagenActual);
                    }
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(ModulosHardware.CamaraBalserPylon, this.Codigo, exception, this.ImageProvider.GetLastErrorMessage());
            }

            // indicamos se ha finalizado la adquisición
            this.AdquisicionEnProceso = false;
        }

        /// <summary>
        /// Handles the event related to the image provider having stopped grabbing.
        /// </summary>
        private void OnGrabbingStoppedEventCallback()
        {
            if (!OThreadManager.EjecucionEnTrheadPrincipal())
            {
                /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
                OThreadManager.SincronizarConThreadPrincipal(new ImageProvider.GrabbingStoppedEventHandler(OnGrabbingStoppedEventCallback), null);
                return;
            }
            /* The image provider stopped grabbing. Enable the grab buttons. Disable the stop button. */
            //EnableButtons(m_imageProvider.IsOpen, false);
        }
        #endregion
    }
}
