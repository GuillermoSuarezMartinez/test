﻿//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 31/10/2012
// Description      : Botones adicionales
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using Cognex.VisionPro;
using Orbita.VAComun;
using System.Windows.Forms;
using System.ComponentModel;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Display de Vision Pro
    /// </summary>
    public partial class CtrlDisplayVPro : CtrlDisplay
    {
        #region Propiedad(es) heredada(s)
        /// <summary>
        /// Propieadad a heredar donde se accede a la imagen
        /// </summary>
        public override OrbitaImage ImagenActual
        {
            get { return (VisionProImage)this._ImagenActual; }
            set
            {
                if (value is VisionProImage)
                {
                    this._ImagenActual = value;
                }
                else if (value is OrbitaImage)
                {
                    VisionProImage valueConverted;
                    if (value.Convert<VisionProImage>(out valueConverted))
                    {
                        this._ImagenActual = valueConverted;
                    }
                }
            }
        }

        /// <summary>
        /// Propiedad a heredar donde se accede al gráfico
        /// </summary>
        public new VisionProGrafico GraficoActual
        {
            get { return (VisionProGrafico)this._GraficoActual; }
            set { this._GraficoActual = value; }
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
                this.PnlButtonsTop.Visible = value;
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
                this.PnlStatusBottom.Visible = value;
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
        public CtrlDisplayVPro()
            : base()
        {
            InitializeComponent();

            // Se asigna el statusbar y el toolbar
            this.DisplayStatusBar.Display = Display;
            this.DisplayToolbar.Display = Display;
        } 
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="titulo">Titulo a visualizar en el display</param>
        public CtrlDisplayVPro(string titulo, string codCamara, double maxFrameIntervalVisualizacion, string codVariableImagen, string codVariableGrafico) :
            base(titulo, codCamara, maxFrameIntervalVisualizacion, codVariableImagen, codVariableGrafico)
        {
            InitializeComponent();

            // Título del display
            this.lblTituloDisplay.Text = titulo;

            // Se asigna el statusbar y el toolbar
            this.DisplayStatusBar.Display = Display;
            this.DisplayToolbar.Display = Display;
        } 
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Inicia la visualización en vivo
        /// </summary>
        public void StartLiveDisplay(object acqFifo, bool own)
        {
            this.Display.StartLiveDisplay(acqFifo, own);
        }
        /// <summary>
        /// Detiene la visualización en vivo
        /// </summary>
        public void StopLiveDisplay()
        {
            this.Display.StopLiveDisplay();
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
            bool resultado = false;

            if (this.ImagenActual != null)
            {
                this.ImagenActual = null;
            }

            this.ImagenActual = new VisionProImage();
            resultado = this.ImagenActual.Cargar(ruta);

            //this.VisualizarImagen(this.ImagenActual, this.GraficoActual);

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
        /// Carga un grafico de disco
        /// </summary>
        /// <param name="ruta">Indica la ruta donde se encuentra el grafico</param>
        /// <returns>Devuelve verdadero si la operación se ha realizado con éxito</returns>
        public override bool CargarGraficoDeDisco(string ruta)
        {
            bool resultado = false;

            if (this.GraficoActual != null)
            {
                //this.GraficoActual.Dispose();
                this.GraficoActual = null;
            }

            this.GraficoActual = new VisionProGrafico();
            resultado = this.GraficoActual.Cargar(ruta);

            //this.VisualizarImagen(this.ImagenActual, this.GraficoActual);

            return resultado;
        }

        /// <summary>
        /// Guarda un objeto gráfico en disco
        /// </summary>
        /// <param name="ruta">Indica la ruta donde se ha de guardar el objeto gráfico</param>
        /// <returns>Devuelve verdadero si la operación se ha realizado con éxito</returns>
        public override bool GuardarGraficoADisco(string ruta)
        {
            bool resultado = false;

            if (this.GraficoActual is VisionProGrafico)
            {
                resultado = this.GraficoActual.Guardar(ruta);
            }

            return resultado;
        }

        /// <summary>
        /// Devuelve una nueva imagen del tipo adecuado al trabajo con el display
        /// </summary>
        /// <returns>Imagen del tipo adecuado al trabajo con el display</returns>
        public override OrbitaImage NuevaImagen()
        {
            return new VisionProImage();
        }

        /// <summary>
        /// Devuelve un nuevo gráfico del tipo adecuado al trabajo con el display
        /// </summary>
        /// <returns>Grafico del tipo adecuado al trabajo con el display</returns>
        public override OrbitaGrafico NuevoGrafico()
        {
            return new VisionProGrafico();            
        }

        /// <summary>
        /// Visualiza una imagen en el display
        /// </summary>
        /// <param name="imagen">Imagen a visualizar</param>
        /// <param name="graficos">Objeto que contiene los gráficos a visualizar (letras, rectas, circulos, etc)</param>
        protected override void VisualizarInterno()
        {
            base.VisualizarInterno();

            // Se carga la imagen y los gráficos siguiendo las recomendaciones de cognex para optimizar el rendimiento
            if (this.ImagenActual is VisionProImage)
            {
                // Disable display updates 
                this.Display.DrawingEnabled = false;

                // Remove all graphics from the display
                this.Display.StaticGraphics.Clear();

                // Update the display with the new image
                bool debeAjustar = (this.Display.Image == null);
                if (!debeAjustar)
                {
                    if ((this.ImagenActual is VisionProImage) && (((VisionProImage)this.ImagenActual).Image != null))
                    {
                        debeAjustar |= this.Display.Image.Width != ((VisionProImage)this.ImagenActual).Image.Width;
                        debeAjustar |= this.Display.Image.Height != ((VisionProImage)this.ImagenActual).Image.Height;
                    }
                }

                this.Display.Image = ((VisionProImage)this.ImagenActual).Image;
                if (debeAjustar)
                {
                    // Fit the image to the display 
                    this.ZoomFit();
                }

                if (this.GraficoActual is VisionProGrafico)
                {
                    VisionProGrafico visionProGrafico = (VisionProGrafico)this.GraficoActual;
                    if (visionProGrafico.Grafico is CogGraphicCollection)
                    {
                        this.Display.StaticGraphics.AddList(visionProGrafico.Grafico, "features");
                    }
                }

                // Enable display updates 
                this.Display.DrawingEnabled = true;
            }
        }

        /// <summary>
        /// La imagen se encaja en pantalla
        /// </summary>
        public override void ZoomFit()
        {
            this.Display.Fit(false);
        }

        /// <summary>
        /// La imagen se encaja en pantalla
        /// </summary>
        public override void ZoomFull()
        {
            this.Display.Zoom = 1;
        }

        /// <summary>
        /// Se aplica un factor de escalado
        /// </summary>
        public override void ZoomValue(double value)
        {
            this.Display.Zoom = value;
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
}