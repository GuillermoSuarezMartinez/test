//***********************************************************************
// Assembly         : Orbita.Controles.VA
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 12-12-2012
// Description      : Se muestra información sobre los nuevos estado de conexión
//                    Conectando, Desconectando y Reconectando
//
// Last Modified By : aibañez
// Last Modified On : 16-11-2012
// Description      : Movido al proyecto Orbita.Controles.VA
//
// Last Modified By : aibañez
// Last Modified On : 29/10/2012
// Description      : Cambiado a ensamblado de VAComun
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Orbita.VAComun;
using Orbita.VAHardware;

namespace Orbita.Controles.VA
{
    /// <summary>
    /// Control base para todos los tipos de displays
    /// </summary>
    public partial class OrbitaVisorBase : UserControl
    {
        #region Atributos
        /// <summary>
        /// Estado de visualización del control (maximizado o normal)
        /// </summary>
        protected FormWindowState EstadoVentana;

        /// <summary>
        /// Indica si la velocidad de visualización de imagenes está limitada y por lo tanto se ha de visualizar de forma retrasada con un timer
        /// </summary>
        protected bool FrameRateVisualizacionLimitado;

        /// <summary>
        /// Cámara Asociada al Visor
        /// </summary>
        protected OCamaraBase CamaraAsociada;

        /// <summary>
        /// Indica que se ha de visualizar en vivo la imagen de la cámara
        /// </summary>
        protected bool VisualizarEnVivo;

        /// <summary>
        /// Indica que se trata de la primera foto después de la conexión de la cámara;
        /// </summary>
        protected bool PrimerFoto;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Código identificativo del visor o de la cámara asociada al display
        /// </summary>
        private string _Codigo;
        /// <summary>
        /// Código identificativo del visor o de la cámara asociada al display
        /// </summary>
        [Browsable(true),
        Category("Orbita"),
        Description("Código identificativo del visor o de la cámara asociada en el caso de que exista"),
        DefaultValue("")]
        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }
        #endregion

        #region Propiedad(es) Virtuales
        /// <summary>
        /// Muestra la barra superior de botones
        /// </summary>
        protected bool _MostrarToolStrip;
        [Browsable(true),
        Category("Orbita"),
        Description("Indica si se ha de mostrar la barra superior de botones"),
        DefaultValue(true)]
        /// <summary>
        /// Muestra la barra superior de botones
        /// </summary>
        public virtual bool MostrarToolStrip
        {
            get { return _MostrarToolStrip; }
            set { _MostrarToolStrip = value; }
        }

        /// <summary>
        /// Muestra la barra inferior de estado
        /// </summary>
        protected bool _MostrarStatusBar;
        [Browsable(true),
        Category("Orbita"),
        Description("Indica si se ha de mostrar la barra inferior de estado"),
        DefaultValue(true)]
        /// <summary>
        /// Muestra la barra inferior de estado
        /// </summary>
        public virtual bool MostrarStatusBar
        {
            get { return _MostrarStatusBar; }
            set { _MostrarStatusBar = value; }
        }

        /// <summary>
        /// Muestra el botón de abrir fotografía
        /// </summary>
        protected bool _MostrarBtnAbrir;
        /// <summary>
        /// Muestra el botón de abrir fotografía
        /// </summary>
        [Browsable(true),
        Category("Orbita"),
        Description("Indica si se ha de mostrar el botón de cargar imagen"),
        DefaultValue(false)]
        public virtual bool MostrarBtnAbrir
        {
            get { return _MostrarBtnAbrir; }
            set
            {
                this._MostrarBtnAbrir = value;
            }
        }

        /// <summary>
        /// Muestra el botón de guardar fotografía
        /// </summary>
        protected bool _MostrarBtnGuardar;
        /// <summary>
        /// Muestra el botón de guardar fotografía
        /// </summary>
        [Browsable(true),
        Category("Orbita"),
        Description("Indica si se ha de mostrar el botón de guardar imagen"),
        DefaultValue(true)]
        public virtual bool MostrarBtnGuardar
        {
            get { return _MostrarBtnGuardar; }
            set
            {
                this._MostrarBtnGuardar = value;
            }
        }

        /// <summary>
        /// Muestra el botón de play/stop
        /// </summary>
        protected bool _MostrarBtnReproduccion;
        /// <summary>
        /// Muestra el botón de play/stop
        /// </summary>
        [Browsable(true),
        Category("Orbita"),
        Description("Indica si se ha de mostrar el botón de play/stop"),
        DefaultValue(false)]
        public virtual bool MostrarBtnReproduccion
        {
            set
            {
                this._MostrarBtnReproduccion = value;
            }
        }

        /// <summary>
        /// Muestra el botón de play/stop
        /// </summary>
        protected bool _MostrarBtnSnap;
        /// <summary>
        /// Muestra el botón de snap
        /// </summary>
        [Browsable(true),
        Category("Orbita"),
        Description("Indica si se ha de mostrar el botón de snap"),
        DefaultValue(false)]
        public virtual bool MostrarBtnSnap
        {
            set
            {
                this._MostrarBtnSnap = value;
            }
        }

        /// <summary>
        /// Muestra la etiqueta del título
        /// </summary>
        protected bool _MostrarLblTitulo;
        /// <summary>
        /// Muestra la etiqueta del título
        /// </summary>
        [Browsable(true),
        Category("Orbita"),
        Description("Indica si se ha de mostrar la etiqueta con el título del visor"),
        DefaultValue(true)]
        public virtual bool MostrarLblTitulo
        {
            get { return _MostrarLblTitulo; }
            set
            {
                this._MostrarLblTitulo = value;
            }
        }

        /// <summary>
        /// Muestra el botón de información del visor
        /// </summary>
        protected bool _MostrarBtnInfo;
        /// <summary>
        /// Muestra el botón de información del visor
        /// </summary>
        [Browsable(true),
        Category("Orbita"),
        Description("Indica si se ha de mostrar el botón de información del visor"),
        DefaultValue(true)]
        public virtual bool MostrarBtnInfo
        {
            get { return _MostrarBtnInfo; }
            set
            {
                this._MostrarBtnInfo = value;
            }
        }

        /// <summary>
        /// Muestra el botón de maximizar/minimizar
        /// </summary>
        protected bool _MostrarBtnMaximinzar;
        /// <summary>
        /// Muestra el botón de maximizar/minimizar
        /// </summary>
        [Browsable(true),
        Category("Orbita"),
        Description("Indica si se ha de mostrar el botón de maximizar/minimizar"),
        DefaultValue(true)]
        public virtual bool MostrarBtnMaximinzar
        {
            get { return _MostrarBtnMaximinzar; }
            set
            {
                this._MostrarBtnMaximinzar = value;
            }
        }

        /// <summary>
        /// Muestra los botones de siguiente/anterior dispositivo
        /// </summary>
        protected bool _MostrarBtnSiguienteAnterior;
        /// <summary>
        /// Muestra los botones de siguiente/anterior dispositivo
        /// </summary>
        [Browsable(true),
        Category("Orbita"),
        Description("Indica si se ha de mostrar los botones de siguiente/anterior dispositivo"),
        DefaultValue(true)]
        public virtual bool MostrarBtnSiguienteAnterior
        {
            get { return _MostrarBtnSiguienteAnterior; }
            set
            {
                this._MostrarBtnSiguienteAnterior = value;
            }
        }

        /// <summary>
        /// Muestra el la información adicional de la imagen visualizada
        /// </summary>
        protected bool _MostrarStatusMensaje;
        /// <summary>
        /// Muestra el la información adicional de la imagen visualizada
        /// </summary>
        [Browsable(true),
        Category("Orbita"),
        Description("Indica si se ha de mostrar información adicional sobre la imagen visualizada"),
        DefaultValue(true)]
        public virtual bool MostrarStatusMensaje
        {
            get { return _MostrarStatusMensaje; }
            set
            {
                this._MostrarStatusMensaje = value;
            }
        }

        /// <summary>
        /// Muestra el la información de la velocidad de adquisición de la cámara asociada
        /// </summary>
        protected bool _MostrarStatusFps;
        /// <summary>
        /// Muestra el la información de la velocidad de adquisición de la cámara asociada
        /// </summary>
        [Browsable(true),
        Category("Orbita"),
        Description("Indica si se ha de mostrar información de la velocidad de adquisición de la cámara asociada"),
        DefaultValue(true)]
        public virtual bool MostrarStatusFps
        {
            get { return _MostrarStatusFps; }
            set
            {
                this._MostrarStatusFps = value;
            }
        }

        /// <summary>
        /// Imagen actual visualizada
        /// </summary>
        protected OImage _ImagenActual;
        /// <summary>
        /// Imagen actual visualizada
        /// </summary>
        [Browsable(false)]
        public virtual OImage ImagenActual
        {
            get { return this._ImagenActual; }
            set { this._ImagenActual = value; }
        }

        /// <summary>
        /// Gráfico actual visualizado
        /// </summary>
        protected OGrafico _GraficoActual;
        /// <summary>
        /// Gráfico actual visualizado
        /// </summary>
        [Browsable(false)]
        public virtual OGrafico GraficoActual
        {
            get { return this._GraficoActual; }
            set { this._GraficoActual = value; }
        }
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OrbitaVisorBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="titulo">Titulo a visualizar en el display</param>
        public OrbitaVisorBase(string titulo, string codCamara, double maxFrameIntervalVisualizacion, bool asociarCamara, bool visualizarEnVivo)
        {
            InitializeComponent();
            
            // Recogemos los parametros
            this.Codigo = codCamara;
            this.CamaraAsociada = asociarCamara? OCamaraManager.GetCamara(this.Codigo): null;
            this.VisualizarEnVivo = visualizarEnVivo && (this.CamaraAsociada is OCamaraBase);

            // Temas de visualización
            this.EstadoVentana = FormWindowState.Normal;

            // Creamos el timer de retraso de visualización
            this.FrameRateVisualizacionLimitado = maxFrameIntervalVisualizacion > 0;
            this.TimerRetrasoVisualizacionUltimaImagen.Interval = OEnteroRobusto.Validar((int)Math.Truncate(maxFrameIntervalVisualizacion), 1, 1000000, 100);
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Carga la imagen acutal mediante una ventana de diálogo
        /// </summary>
        protected void CargarImagenDialogo()
        {
            string rutaArchivo = ORutaParametrizable.AppFolder;
            bool archivoSeleccionadoOK = App.FormularioSeleccionArchivo(this.openFileDialog, ref rutaArchivo);
            if (archivoSeleccionadoOK)
            {
                this.CargarImagenDeDisco(rutaArchivo);
            }
        }

        /// <summary>
        /// Guarda la imagen actual mediante una ventana de diálogo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GuardarImagenDialogo()
        {
            string rutaArchivo = ORutaParametrizable.AppFolder;
            bool archivoSeleccionadoOK = App.FormularioGuardarArchivo(this.saveFileDialog, ref rutaArchivo);
            if (archivoSeleccionadoOK)
            {
                this.GuardarImagenADisco(rutaArchivo);
            }
        }

        /// <summary>
        /// Muestra un formuario con información sobre el dispositivo origen de la imagen
        /// </summary>
        protected void MostrarInfoDispositivo()
        {
            if (this.OnInfoDemandada != null)
            {
                this.OnInfoDemandada(this, new EventVisorClickButtonArgs(this.Codigo));
            }
        }

        /// <summary>
        /// Lanza el evento de botón de play/stop pulsado
        /// </summary>
        protected void BotonPlayStopPulsado()
        {
            if (this.CamaraAsociada is OCamaraBase)
	        {
                if (this.CamaraAsociada.EstadoConexion == OEstadoConexion.Conectado)
                {
                    this.CamaraAsociada.Grab = !this.CamaraAsociada.Grab;
                }
	        }
        }

        /// <summary>
        /// Lanza el evento de botón de play/stop pulsado
        /// </summary>
        protected void BotonSnapPulsado()
        {
            if (this.CamaraAsociada.EstadoConexion == OEstadoConexion.Conectado)
            {
                this.CamaraAsociada.Snap();
            }
        }

        /// <summary>
        /// Método que lanza el evento de cambio de visor
        /// </summary>
        /// <param name="ordenCambioVisor"></param>
        protected void CambioVisor(EventDeviceViewerChangedArgs.DeviceViewerChangeOrder ordenCambioVisor)
        {
            if (this.OnVisorDispositivoCambiado != null)
            {
                this.OnVisorDispositivoCambiado(this, new EventDeviceViewerChangedArgs(this._Codigo, ordenCambioVisor));
            }
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Visualiza una imagen en el display
        /// </summary>
        /// <param name="imagen">Imagen a visualizar</param>
        public void Visualizar(OImage imagen)
        {
            this.ImagenActual = imagen;
            if (this.FrameRateVisualizacionLimitado)
            {
                this.TimerRetrasoVisualizacionUltimaImagen.Start();
            }
            else
            {
                this.VisualizarInterno();
            }
        }

        /// <summary>
        /// Visualiza una imagen en el display
        /// </summary>
        /// <param name="imagen">Imagen a visualizar</param>
        /// <param name="graficos">Objeto que contiene los gráficos a visualizar (letras, rectas, circulos, etc)</param>
        public void Visualizar(OImage imagen, OGrafico graficos)
        {
            this.ImagenActual = imagen;
            this.GraficoActual = graficos;
            if (FrameRateVisualizacionLimitado)
            {
                this.TimerRetrasoVisualizacionUltimaImagen.Start();
            }
            else
            {
                this.VisualizarInterno();
            }
        }

        /// <summary>
        /// Visualiza una imagen en el display
        /// </summary>
        /// <param name="graficos">Objeto que contiene los gráficos a visualizar (letras, rectas, circulos, etc)</param>
        public void Visualizar(OGrafico graficos)
        {
            this.GraficoActual = graficos;
            if (FrameRateVisualizacionLimitado)
            {
                this.TimerRetrasoVisualizacionUltimaImagen.Start();
            }
            else
            {
                this.VisualizarInterno();
            }
        }
        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Carga los valores de la cámara
        /// </summary>
        public virtual void Inicializar()
        {
            //this.ImagenActual = new OImage();
            //this.GraficoActual = new OGrafico();

            if (this.CamaraAsociada is OCamaraBase)
            {
                this.CamaraAsociada.CrearSuscripcionMensajes(this.MostrarMensaje);
                this.CamaraAsociada.CrearSuscripcionCambioEstado(this.OnCambioEstadoConexionCamara);
                this.CamaraAsociada.OnCambioReproduccionCamara += this.CambioModoReproduccionCamara;
                if (this.VisualizarEnVivo)
                {
                    this.CamaraAsociada.CrearSuscripcionNuevaFotografia(this.OnNuevaFotografiaCamara);
                }

                // Obtener las propiedades actuales de la cámara
                this.OnCambioEstadoConexionCamara(this.CamaraAsociada.EstadoConexion);
                this.CambioModoReproduccionCamara(this.CamaraAsociada.Grab);
            }
        }

        /// <summary>
        /// Finaliza la cámara
        /// </summary>
        public virtual void Finalizar()
        {
            if (this.CamaraAsociada is OCamaraBase)
            {
                this.CamaraAsociada.EliminarSuscripcionMensajes(this.MostrarMensaje);
                this.CamaraAsociada.EliminarSuscripcionCambioEstado(this.OnCambioEstadoConexionCamara);
                this.CamaraAsociada.OnCambioReproduccionCamara -= this.CambioModoReproduccionCamara;
                if (this.VisualizarEnVivo)
                {
                    this.CamaraAsociada.EliminarSuscripcionNuevaFotografia(this.OnNuevaFotografiaCamara);
                }
            }

            if (this.ImagenActual != null)
            {
                this.ImagenActual = null;
            }
            if (this.GraficoActual != null)
            {
                this.GraficoActual = null;
            }
        }

        /// <summary>
        /// Carga una imagen de disco
        /// </summary>
        /// <param name="ruta">Indica la ruta donde se encuentra la fotografía</param>
        /// <returns>Devuelve verdadero si la operación se ha realizado con éxito</returns>
        public virtual bool CargarImagenDeDisco(string ruta)
        {
            // Método implementado en las clases hijas
            return false;
        }

        /// <summary>
        /// Guarda una imagen en disco
        /// </summary>
        /// <param name="ruta">Indica la ruta donde se ha de guardar la fotografía</param>
        /// <returns>Devuelve verdadero si la operación se ha realizado con éxito</returns>
        public virtual bool GuardarImagenADisco(string ruta)
        {
            // Método implementado en las clases hijas
            return false;
        }

        /// <summary>
        /// Carga un grafico de disco
        /// </summary>
        /// <param name="ruta">Indica la ruta donde se encuentra el grafico</param>
        /// <returns>Devuelve verdadero si la operación se ha realizado con éxito</returns>
        public virtual bool CargarGraficoDeDisco(string ruta)
        {
            // Método implementado en las clases hijas
            return false;
        }

        /// <summary>
        /// Guarda un objeto gráfico en disco
        /// </summary>
        /// <param name="ruta">Indica la ruta donde se ha de guardar el objeto gráfico</param>
        /// <returns>Devuelve verdadero si la operación se ha realizado con éxito</returns>
        public virtual bool GuardarGraficoADisco(string ruta)
        {
            // Método implementado en las clases hijas
            return false;
        }

        /// <summary>
        /// Devuelve una nueva imagen del tipo adecuado al trabajo con el display
        /// </summary>
        /// <returns>Imagen del tipo adecuado al trabajo con el display</returns>
        public virtual OImage NuevaImagen()
        {
            return null;
        }

        /// <summary>
        /// Devuelve un nuevo gráfico del tipo adecuado al trabajo con el display
        /// </summary>
        /// <returns>Grafico del tipo adecuado al trabajo con el display</returns>
        public virtual OGrafico NuevoGrafico()
        {
            return null;
        }

        /// <summary>
        /// Visualiza una imagen en el display
        /// </summary>
        /// <param name="imagen">Imagen a visualizar</param>
        protected virtual void VisualizarInterno()
        {
            // Método a implementar en heredados
        }

        /// <summary>
        /// La imagen se encaja en pantalla
        /// </summary>
        public virtual void ZoomFit()
        {
            // Método a implementar en heredados
        }

        /// <summary>
        /// La imagen se encaja en pantalla
        /// </summary>
        public virtual void ZoomFull()
        {
            // Método a implementar en heredados
        }

        /// <summary>
        /// Se aplica un factor de escalado
        /// </summary>
        public virtual void ZoomValue(double value)
        {
            // Método a implementar en heredados
        }

        /// <summary>
        /// Muestra un mensaje de información sobre el dispositivo origen
        /// </summary>
        public virtual void MostrarMensaje(string mensaje)
        {
            // Método a implementar en heredados
        }

        /// <summary>
        /// Muestra la velocidad de adquisición de la cámara asociada
        /// </summary>
        public virtual void MostrarFps(double framePerSecond)
        {
            // Método a implementar en heredados
        }

        /// <summary>
        /// Maximizamos o restauramos la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void AccionMaximizarMinimizar(FormWindowState estadoVentana)
        {
            if (this.EstadoVentana != estadoVentana)
            {
                this.EstadoVentana = estadoVentana;

                if (this.OnEstadoVentanaCambiado != null)
                {
                    this.OnEstadoVentanaCambiado(this, new EventStateMaximizedArgs(this._Codigo, estadoVentana));
                }

                this.ZoomFit();
            }
        }

        /// <summary>
        /// Método que indica que la cámara ha cambiado su modo de reproducción
        /// </summary>
        /// <param name="modoReproduccionContinua"></param>
        protected virtual void CambioModoReproduccionCamara(bool modoReproduccionContinua)
        {
            // Implementado en heredados
        }

        #endregion

        #region Evento(s)
        /// <summary>
        /// Evento del timer de visualización de la última imágen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimerVisualizacionUltimaImagen(object sender, EventArgs e)
        {
            try
            {
                this.TimerRetrasoVisualizacionUltimaImagen.Stop();
                this.VisualizarInterno();
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosSistema.ImagenGraficos, this.Codigo, exception);
            }
        }

        /// <summary>
        /// Delegado de nueva fotografía
        /// </summary>
        /// <param name="OEstadoConexion"></param>
        public void OnNuevaFotografiaCamara(string codigo, OImage imagen, DateTime momentoAdquisicion, double velocidadAdquisicion)
        {
            try
            {
                this.Visualizar(imagen);
                this.MostrarFps(velocidadAdquisicion);
                if (!this.PrimerFoto)
                {
                    this.PrimerFoto = true;
                    this.ZoomFit();
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosSistema.ImagenGraficos, this.Codigo, exception);
            }
        }

        #endregion

        #region Evento(s) virtual(es)
        /// <summary>
        /// Delegado de cambio de estaco de conexión de la cámara
        /// </summary>
        /// <param name="estadoConexion"></param>
        public virtual void OnCambioEstadoConexionCamara(OEstadoConexion estadoConexion)
        {
            try
            {
                switch (estadoConexion)
                {
                    case OEstadoConexion.Desconectado:
                    default:
                        this.ImagenActual = this.NuevaImagen();
                        this.ImagenActual.ConvertFromBitmap(global::Orbita.Controles.VA.Properties.Resources.CamaraDesConectada);
                        this.Visualizar(this.ImagenActual);
                        this.ZoomFit();
                        this.MostrarMensaje("Cámara desconectada");
                        break;
                    case OEstadoConexion.Desconectando:
                        this.MostrarMensaje("Cámara en proceso de desconexión");
                        break;
                    case OEstadoConexion.Conectado:
                        this.ImagenActual = this.NuevaImagen();
                        this.ImagenActual.ConvertFromBitmap(global::Orbita.Controles.VA.Properties.Resources.CamaraConectada);
                        this.Visualizar(this.ImagenActual);
                        this.ZoomFit();
                        this.MostrarMensaje("Cámara conectada");
                        this.PrimerFoto = false;
                        break;
                    case OEstadoConexion.Conectando:
                        this.MostrarMensaje("Cámara en proceso de conexión");
                        break;
                    case OEstadoConexion.ErrorConexion:
                        this.MostrarMensaje("Error de conexión");
                        break;
                    case OEstadoConexion.Reconectando:
                        this.ImagenActual = this.NuevaImagen();
                        this.ImagenActual.ConvertFromBitmap(global::Orbita.Controles.VA.Properties.Resources.CamaraDesConectada);
                        this.Visualizar(this.ImagenActual);
                        this.ZoomFit();
                        this.MostrarMensaje("Intentando reconexión");
                        break;
                    case OEstadoConexion.Reconectado:
                        this.MostrarMensaje("Reconexión realizada");
                        break;
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosSistema.ImagenGraficos, this.Codigo, exception);
            }
        }
        #endregion

        #region Definición de eventos(s)
        /// <summary>
        /// Evento que indica el cambio de estado de visualización de la ventana (maximizado o normal)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [Category("Orbita")]
        [Description("Se dispara cuando se produce una maximización o restauración del control.")]
        public event EstadoVentanaCambiado OnEstadoVentanaCambiado;

        /// <summary>
        /// Evento que indica el cambio de visualización al siguiente/anterior dispositivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [Category("Orbita")]
        [Description("Se dispara cuando el usuario desea visualizar imagenes del siguiente o anterior dispositivo.")]
        public event VisorDispositivoCambiado OnVisorDispositivoCambiado;

        /// <summary>
        /// Evento que indica el cambio de estado de visualización de la ventana (maximizado o normal)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [Category("Orbita")]
        [Description("Se dispara cuando el usuario desea ver más información sobre el dispositivo origen de la imagen.")]
        public event VisorClicBoton OnInfoDemandada;
        #endregion
    }

    #region Definición de eventos(s)
    /// <summary>
    /// Delegado que indica el cambio de estado de visualización de la ventana (maximizado o normal)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void EstadoVentanaCambiado(object sender, EventStateMaximizedArgs e);

    /// <summary>
    /// Parametros de retorno del evento de cambio de maximizado o restaurado de la ventana
    /// </summary>
    public class EventStateMaximizedArgs : EventArgs
    {
        #region Atributo(s)
        /// <summary>
        /// Código de la cámara
        /// </summary>
        public string CodCamara;
        /// <summary>
        /// Estado actual de la ventana
        /// </summary>
        public FormWindowState EstadoVentana;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codEstado">Código del estado visual</param>
        /// <param name="posicion">Posición del estado visual</param>
        public EventStateMaximizedArgs(string codCamara, FormWindowState estadoVentana)
        {
            this.CodCamara = codCamara;
            this.EstadoVentana = estadoVentana;
        }
        #endregion
    }

    /// <summary>
    /// Delegado que indica el cambio de visualización al siguiente o anterior dispositivo
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void VisorDispositivoCambiado(object sender, EventDeviceViewerChangedArgs e);

    /// <summary>
    /// Parametros de retorno del evento de cambio de visualización al siguiente o anterior dispositivo
    /// </summary>
    public class EventDeviceViewerChangedArgs : EventArgs
    {
        #region Atributo(s)
        /// <summary>
        /// Código de la cámara
        /// </summary>
        public string CodCamara;
        /// <summary>
        /// Estado actual de la ventana
        /// </summary>
        public DeviceViewerChangeOrder OrdenSiguienteDispositivoAVisualizar;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codEstado">Código del estado visual</param>
        /// <param name="posicion">Posición del estado visual</param>
        public EventDeviceViewerChangedArgs(string codCamara, DeviceViewerChangeOrder ordenSiguienteDispositivoAVisualizar)
        {
            this.CodCamara = codCamara;
            this.OrdenSiguienteDispositivoAVisualizar = ordenSiguienteDispositivoAVisualizar;
        }
        #endregion

        #region Enumerado(s)
        /// <summary>
        /// Enumerado que indica el orden del siguiente dispositivo a visualizar 
        /// </summary>
		public enum DeviceViewerChangeOrder
        {
            next,
            previous
        }
	    #endregion
    }

    /// <summary>
    /// Delegado que indica el cambio de estado de visualización de la ventana (maximizado o normal)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void VisorClicBoton(object sender, EventVisorClickButtonArgs e);

    /// <summary>
    /// Parametros de retorno del evento de cambio de maximizado o restaurado de la ventana
    /// </summary>
    public class EventVisorClickButtonArgs : EventArgs
    {
        #region Atributo(s)
        /// <summary>
        /// Código de la cámara
        /// </summary>
        public string CodCamara;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codEstado">Código del estado visual</param>
        /// <param name="posicion">Posición del estado visual</param>
        public EventVisorClickButtonArgs(string codCamara)
        {
            this.CodCamara = codCamara;
        }
        #endregion
    }
    #endregion
}
