//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 30-10-2012
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Orbita.VAComun;
using System.Windows.Forms;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Control encargado de visualizar las imágenes y gráficos de las cámaras Axis
    /// </summary>
    public partial class CtrlDisplayAxis : CtrlDisplay
    {
        #region Propiedad(es) heredada(s)
        /// <summary>
        /// Propieadad a heredar donde se accede a la imagen
        /// </summary>
        public override OrbitaImage ImagenActual
        {
            get { return (BitmapImage)this._ImagenActual; }
            set
            {
                if (value is BitmapImage)
                {
                    this._ImagenActual = value;
                }
                else if (value is OrbitaImage)
                {
                    BitmapImage valueConverted;
                    if (value.Convert<BitmapImage>(out valueConverted))
                    {
                        this._ImagenActual = valueConverted;
                    }
                }
            }
        }

        /// <summary>
        /// Muestra la barra superior de botones
        /// </summary>
        public override bool MostrarToolStrip
        {
            get { return _MostrarToolStrip; }
            set
            {
                this._MostrarToolStrip = value;
                this.ToolStripTop.Visible = value;
            }
        }

        /// <summary>
        /// Muestra la barra inferior de estado
        /// </summary>
        public override bool MostrarStatusBar
        {
            get { return _MostrarStatusBar; }
            set
            {
                this._MostrarStatusBar = value;
                this.StatusStripBottom.Visible = value;
            }
        }

        /// <summary>
        /// Muestra el botón de abrir fotografía
        /// </summary>
        public override bool MostrarBtnAbrir
        {
            get { return _MostrarBtnAbrir; }
            set
            {
                this._MostrarBtnAbrir = value;
                this.btnOpen.Visible = value;
                this.separadorArchivos.Visible = this._MostrarBtnGuardar || this._MostrarBtnAbrir;
            }
        }

        /// <summary>
        /// Muestra el botón de guardar fotografía
        /// </summary>
        public override bool MostrarBtnGuardar
        {
            set
            {
                this._MostrarBtnGuardar = value;
                this.btnSave.Visible = value;
                this.separadorArchivos.Visible = this._MostrarBtnGuardar || this._MostrarBtnAbrir;
            }
        }

        /// <summary>
        /// Muestra la etiqueta del título
        /// </summary>
        public override bool MostrarLblTitulo
        {
            set
            {
                this._MostrarLblTitulo = value;
                this.lblTituloDisplay.Visible = value;
            }
        }

        /// <summary>
        /// Muestra el botón de información del visor
        /// </summary>
        public override bool MostrarBtnInfo
        {
            set
            {
                this._MostrarBtnInfo = value;
                this.btnInfo.Visible = value;
            }
        }

        /// <summary>
        /// Muestra el botón de maximizar/minimizar
        /// </summary>
        public override bool MostrarBtnMaximinzar
        {
            set
            {
                this._MostrarBtnMaximinzar = value;
                this.btnMaximize.Visible = value;
            }
        }

        /// <summary>
        /// Muestra el la información adicional de la imagen visualizada
        /// </summary>
        public override bool MostrarStatusMensaje
        {
            get { return _MostrarStatusMensaje; }
            set
            {
                this._MostrarStatusMensaje = value;
                this.lblMensaje.Visible = value;
            }
        }
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public CtrlDisplayAxis()
            : base()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="titulo">Titulo a visualizar en el display</param>
        public CtrlDisplayAxis(string titulo, string codCamara, double maxFrameIntervalVisualizacion, string codVariableImagen, string codVariableGrafico) :
            base(titulo, codCamara, maxFrameIntervalVisualizacion, codVariableImagen, codVariableGrafico)
        {
            this.InitializeComponent();

            // Título del display
            this.lblTituloDisplay.Text = titulo;
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Se extrae la imagen actual de la cámara
        /// </summary>
        /// <returns></returns>
        public bool GetCurrentImage(out BitmapImage bitmapImage)
        {
            bool resultado = false;
            bitmapImage = null;
            try
            {
                // Se consulta la imágen de la cámara
                object imagenActual;
                int tamañoImagen;
                
                this.dispAxis.GetCurrentImage(0, out imagenActual, out tamañoImagen);

                // Se convierte a un byte array normal
                byte[] myByte = new byte[tamañoImagen];
                Array.Copy((Array)imagenActual, (Array)myByte, tamañoImagen);

                //Se carga en el bitmap
                bitmapImage = new BitmapImage();
                try
                {
                    if (myByte != null)
                    {
                        MemoryStream ms = new MemoryStream(myByte);
                        if (ms != null)
                        {
                            bitmapImage.Image = (Bitmap)Bitmap.FromStream(ms);
                        }
                        else
                        {
                            LogsRuntime.Error(ModulosHardware.CamaraAxis, "AxisGetCurrentImagsMs nulo:" + this.Codigo,new Exception());
                        }
                    }
                    else
                    {
                        bitmapImage = null;
                        LogsRuntime.Error(ModulosHardware.CamaraAxis, "AxisGetCurrentImageByte nulo:" + this.Codigo, new Exception());
                    }
                    //bitmapImage.Image = new Bitmap((Bitmap)Bitmap.FromStream(ms));
                }
                catch (Exception exception)
                {
                    bitmapImage = null;
                    LogsRuntime.Error(ModulosHardware.CamaraAxis, "AxisGetCurrentImageCreando:" + this.Codigo, exception);
                }

                resultado = bitmapImage.EsValida();
            }
            catch (Exception exception)
            {
                bitmapImage = null;
                LogsRuntime.Error(ModulosHardware.CamaraAxis, "AxisGetCurrentImage:" + this.Codigo, exception);
                //Force garbage collection.
                GC.Collect();
            }

            return resultado;
        }

        /// <summary>
        /// Se importa una imagen a la cámara
        /// </summary>
        /// <returns></returns>
        public bool SetCurrentImage(BitmapImage bitmapImage)
        {
            bool resultado = false;
            try
            {
                //Se descarga el bitmap
                MemoryStream ms = new MemoryStream();
                bitmapImage.Image.Save(ms, ImageFormat.Jpeg);

                byte[] myByte = ms.ToArray();

                int tamaño = myByte.Length;
                object objBitmap = myByte;
                this.dispAxis.SetImage(ref objBitmap, ref tamaño);

                resultado = true;
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraAxis, "AxisSetCurrentImage", exception);
            }

            return resultado;
        }

        /// <summary>
        /// Realiza una grabación de la camara que se encuentra en modo grab
        /// </summary>
        /// ruta y nombre del fichero que contendra el video
        /// <returns></returns>
        public bool REC(string fichero)
        {
            bool resultado = false;
            try
            {
                // Se inicia la grabación
                this.dispAxis.StartRecord(fichero);

                return resultado;
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.CamaraAxis, this.Codigo, exception);
            }
            return resultado;
        }

        /// <summary>
        /// Termina una grabación continua de la cámara
        /// </summary>
        /// <returns></returns>
        public bool StopREC()
        {
            bool resultado = false;
            try
            {
                //Stops possible recording 
                this.dispAxis.StopRecord();
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
        }

        /// <summary>
        /// Finaliza la cámara
        /// </summary>
        public override void Finalizar()
        {
            base.Finalizar();
        }

        /// <summary>
        /// Carga una imagen de disco
        /// </summary>
        /// <param name="ruta">Indica la ruta donde se encuentra la fotografía</param>
        /// <returns>Devuelve verdadero si la operación se ha realizado con éxito</returns>
        public override bool CargarImagenDeDisco(string ruta)
        {
            // Valores por defecto
            bool resultado = base.CargarImagenDeDisco(ruta);

            // Se carga la imagen
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.Cargar(ruta);
            if (bitmapImage.EsValida())
            {
                // Se carga en el display
                this.SetCurrentImage(bitmapImage);

                // Devolvemos los valores
                this.ImagenActual = bitmapImage;
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
        /// Devuelve una nueva imagen del tipo adecuado al trabajo con el display
        /// </summary>
        /// <returns>Imagen del tipo adecuado al trabajo con el display</returns>
        public override OrbitaImage NuevaImagen()
        {
            BitmapImage bitmapImage = new BitmapImage();
            return bitmapImage;
        }

        /// <summary>
        /// Visualiza una imagen en el display
        /// </summary>
        /// <param name="imagen">Imagen a visualizar</param>
        protected override void VisualizarInterno()
        {
            base.VisualizarInterno();

            if (this.ImagenActual is BitmapImage)
            {
                this.SetCurrentImage((BitmapImage)this.ImagenActual);
            }
        }

        /// <summary>
        /// Muestra un mensaje de información sobre el dispositivo origen
        /// </summary>
        public override void MostrarMensaje(string mensaje)
        {
            this.lblMensaje.Text = mensaje;
        }

        /// <summary>
        /// Maximizamos o restauramos la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void AccionMaximizarMinimizar(FormWindowState estadoVentana)
        {
            base.AccionMaximizarMinimizar(estadoVentana);
            switch (this.EstadoVentana)
            {
                case FormWindowState.Normal:
                    this.btnMaximize.Image = Properties.Resources.imgFullScreen16;
                    this.btnNext.Visible = false;
                    this.btnPrev.Visible = false;
                    break;
                case FormWindowState.Maximized:
                default:
                    this.btnMaximize.Image = Properties.Resources.imgNoFullScreen16;
                    this.btnNext.Visible = this._MostrarBtnSiguienteAnterior;
                    this.btnPrev.Visible = this._MostrarBtnSiguienteAnterior;
                    break;
            }
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Carga la imagen actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                this.CargarImagenDialogo();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosSistema.ImagenGraficos, this.Codigo, exception);
            }
        }

        /// <summary>
        /// Guarda la imagen actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.GuardarImagenDialogo();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosSistema.ImagenGraficos, this.Codigo, exception);
            }
        }

        /// <summary>
        /// Mostramos el formulario de información de la cámara
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInfo_Click(object sender, EventArgs e)
        {
            try
            {
                this.MostrarInfoDispositivo();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosSistema.ImagenGraficos, this.Codigo, exception);
            }
        }

        /// <summary>
        /// Maximizamos o restauramos la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMaximize_Click(object sender, EventArgs e)
        {
            try
            {
                FormWindowState estadoVentana = this.EstadoVentana == FormWindowState.Maximized ? FormWindowState.Normal : FormWindowState.Maximized;
                this.AccionMaximizarMinimizar(estadoVentana);
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosSistema.ImagenGraficos, this.Codigo, exception);
            }
        }

        /// <summary>
        /// Avanzamos al siguiente visor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.EstadoVentana == FormWindowState.Maximized)
                {
                    this.CambioVisor(EventDeviceViewerChanged.DeviceViewerChangeOrder.next);
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosSistema.ImagenGraficos, this.Codigo, exception);
            }
        }

        /// <summary>
        /// Retrocedemos al visor anterior
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.EstadoVentana == FormWindowState.Maximized)
                {
                    this.CambioVisor(EventDeviceViewerChanged.DeviceViewerChangeOrder.previous);
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosSistema.ImagenGraficos, this.Codigo, exception);
            }
        }
        #endregion
    }

    /// <summary>
    /// Estructura que describe los disintos estados de las cámara Axis
    /// </summary>
    public struct EstadoCamaraAxis
    {
        #region Constante(s)
        private const int AMC_STATUS_INITIALIZED = 1; //	AMC is initialized and ready.
        private const int AMC_STATUS_FLAG_PLAYING = 2; //	A media stream/file is playing.
        private const int AMC_STATUS_FLAG_PAUSED = 4; //	Playing of a media stream/file is paused.
        private const int AMC_STATUS_FLAG_RECORDING = 8; //	A media stream/file is being recorded.
        private const int AMC_STATUS_FLAG_OPENING = 16; //	AMC is opening a media stream/file.
        private const int AMC_STATUS_FLAG_RECONNECTING = 32; //	AMC is performing reconnection attempts.
        private const int AMC_STATUS_FLAG_ISSUING_PTZ_COMMAND = 512; //	Issuing a Pan, Tilt, Zoom command to the device.
        private const int AMC_STATUS_FLAG_EXTENDED_TEXT = 1024; //	An extended text/message is displayed in the status bar.
        private const int AMC_STATUS_FLAG_PTZ_UIMODE_ABS = 2048; //	UIMode is set to ptz-absolute.
        private const int AMC_STATUS_FLAG_PTZ_UIMODE_REL = 4096; //	UIMode is set to ptz-relative or ptz-relative-no-cross.
        private const int AMC_STATUS_FLAG_OPENING_RECEIVE_AUDIO = 65536; //	The stream for receiveing audio is being opened.
        private const int AMC_STATUS_FLAG_OPENING_TRANSMIT_AUDIO = 131072; //	AMC is opening the stream for transmitting audio.
        private const int AMC_STATUS_FLAG_RECEIVE_AUDIO = 262144; //	Receiving audio.
        private const int AMC_STATUS_FLAG_TRANSMIT_AUDIO = 524288; //	Transmitting audio.
        private const int AMC_STATUS_FLAG_TRANSMIT_AUDIO_FILE = 1048576; //	An audio file is being transmitted.
        private const int AMC_STATUS_FLAG_RECORDING_AUDIO = 2097152; //	Recording audio.
        #endregion

        #region Atributo(s)
        public bool INITIALIZED; //	AMC is initialized and ready.
        public bool PLAYING; //	A media stream/file is playing.
        public bool PAUSED; //	Playing of a media stream/file is paused.
        public bool RECORDING; //	A media stream/file is being recorded.
        public bool OPENING; //	AMC is opening a media stream/file.
        public bool RECONNECTING; //	AMC is performing reconnection attempts.
        public bool ISSUINGPTZCOMMAND; //	Issuing a Pan, Tilt, Zoom command to the device.
        public bool EXTENDEDTEXT; //	An extended text/message is displayed in the status bar.
        public bool PTZUIMODEABS; //	UIMode is set to ptz-absolute.
        public bool PTZUIMODEREL; //	UIMode is set to ptz-relative or ptz-relative-no-cross.
        public bool OPENINGRECEIVEAUDIO; //	The stream for receiveing audio is being opened.
        public bool OPENINGTRANSMITAUDIO; //	AMC is opening the stream for transmitting audio.
        public bool RECEIVEAUDIO; //	Receiving audio.
        public bool TRANSMITAUDIO; //	Transmitting audio.
        public bool TRANSMITAUDIOFILE; //	An audio file is being transmitted.
        public bool RECORDINGAUDIO; //	Recording audio.
        #endregion       

        #region Contructor
        /// <summary>
        /// Constructor de la estructura
        /// </summary>
        /// <param name="status"></param>
        public EstadoCamaraAxis(int status)
        {
            this.INITIALIZED = (status & AMC_STATUS_INITIALIZED) > 0;
            this.PLAYING = (status & AMC_STATUS_FLAG_PLAYING) > 0;
            this.PAUSED = (status & AMC_STATUS_FLAG_PAUSED) > 0;
            this.RECORDING = (status & AMC_STATUS_FLAG_RECORDING) > 0;
            this.OPENING = (status & AMC_STATUS_FLAG_OPENING) > 0;
            this.RECONNECTING = (status & AMC_STATUS_FLAG_RECONNECTING) > 0;
            this.ISSUINGPTZCOMMAND = (status & AMC_STATUS_FLAG_ISSUING_PTZ_COMMAND) > 0;
            this.EXTENDEDTEXT = (status & AMC_STATUS_FLAG_EXTENDED_TEXT) > 0;
            this.PTZUIMODEABS = (status & AMC_STATUS_FLAG_PTZ_UIMODE_ABS) > 0;
            this.PTZUIMODEREL = (status & AMC_STATUS_FLAG_PTZ_UIMODE_REL) > 0;
            this.OPENINGRECEIVEAUDIO = (status & AMC_STATUS_FLAG_OPENING_RECEIVE_AUDIO) > 0;
            this.OPENINGTRANSMITAUDIO = (status & AMC_STATUS_FLAG_OPENING_TRANSMIT_AUDIO) > 0;
            this.RECEIVEAUDIO = (status & AMC_STATUS_FLAG_RECEIVE_AUDIO) > 0;
            this.TRANSMITAUDIO = (status & AMC_STATUS_FLAG_TRANSMIT_AUDIO) > 0;
            this.TRANSMITAUDIOFILE = (status & AMC_STATUS_FLAG_TRANSMIT_AUDIO_FILE) > 0;
            this.RECORDINGAUDIO = (status & AMC_STATUS_FLAG_RECORDING_AUDIO) > 0;
        }
        #endregion
    }
}
