//***********************************************************************
// Assembly         : Orbita.Controles.VA
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 12-12-2012
// Description      : No se muestran los visores de las cámaras no habilitadas
//
// Last Modified By : aibañez
// Last Modified On : 16-11-2012
// Description      : Movido al proyecto Orbita.Controles.VA
//
// Last Modified By : aibañez
// Last Modified On : 30-10-2012
// Description      : Movido a VAComun
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Orbita.VAComun;
using System.Data;
using Orbita.VAHardware;
using System.ComponentModel;

namespace Orbita.Controles.VA
{
    /// <summary>
    /// Formulario de agrupación de displays
    /// </summary>
    public partial class FrmDisplays : FrmBase
    {
        #region Atributo(s)
        /// <summary>
        /// Contador del número de cámaras
        /// </summary>
        private int NumeroCamaras = 0;

        /// <summary>
        /// Lista de visores
        /// </summary>
        public List<OrbitaVisorBase> ListaDisplays;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Enlazamos con las cámaras
        /// </summary>
        private bool _EnlazarCamaras;
        /// <summary>
        /// Enlazamos con las cámaras
        /// </summary>
        [Browsable(true),
        Category("Orbita"),
        Description("Se enlazan las cámaras dadas de alta en el sistema automáticamente"),
        DefaultValue(true)]
        public bool EnlazarCamaras
        {
            get { return _EnlazarCamaras; }
            set { _EnlazarCamaras = value; }
        }
        
        /// <summary>
        /// Se requiere visualización en vivo
        /// </summary>
        private bool _VisualizacionEnVivo;
        /// <summary>
        /// Se requiere visualización en vivo
        /// </summary>
        [Browsable(true),
        Category("Orbita"),
        Description("Se visualizan las imagenes adquiridas por las cámaras automáticamente"),
        DefaultValue(true)]
        public bool VisualizacionEnVivo
        {
            get { return _VisualizacionEnVivo; }
            set { _VisualizacionEnVivo = value; }
        }

        /// <summary>
        /// Indica que los visores tienen control sobre la adquisición de imágenes de la cámara
        /// </summary>
        private bool _ControlCamara;
        /// <summary>
        /// Indica que los visores tienen control sobre la adquisición de imágenes de la cámara
        /// </summary>
        [Browsable(true),
        Category("Orbita"),
        Description("Indica que los visores tienen control sobre la adquisición de imágenes de la cámar"),
        DefaultValue(true)]
        public bool ControlCamara
        {
            get { return _ControlCamara; }
            set { _ControlCamara = value; }
        }
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public FrmDisplays()
            : this("Monitorización de cámaras", true, true, true)
        {
            InitializeComponent();

            this.ListaDisplays = new List<OrbitaVisorBase>();
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public FrmDisplays(string titulo, bool enlazarCamaras, bool visualiacionEnVivo, bool controlCamara)
            : this(ModoAperturaFormulario.Modificacion, titulo, enlazarCamaras, visualiacionEnVivo, controlCamara)
        {
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public FrmDisplays(ModoAperturaFormulario modoAperturaFormulario, string titulo, bool enlazarCamaras, bool visualiacionEnVivo, bool controlCamara)
            : base(modoAperturaFormulario)
        {
            InitializeComponent();

            this.Text = titulo;
            this._EnlazarCamaras = enlazarCamaras;
            this._VisualizacionEnVivo = visualiacionEnVivo;
            this._ControlCamara = controlCamara;

            this.ListaDisplays = new List<OrbitaVisorBase>();
        }  
        #endregion

        #region Métodos privados
        /// <summary>
        /// Cambio del estado de la ventana
        /// </summary>
        /// <param name="codCamar"></param>
        /// <param name="formWindowState"></param>
        /// <returns></returns>
        private void CambiarEstadoVentana(string codCamara, FormWindowState estadoVentana)
        {
            bool controlEncontrado = false;
            int actualRow = -1;
            int actualCol = -1;

            for (int row = 0; row < this.layFondoVisores.RowCount; row++)
            {
                for (int col = 0; col < this.layFondoVisores.ColumnCount; col++)
                {
                    Control control = this.layFondoVisores.GetControlFromPosition(col, row);
                    if (control is OrbitaVisorBase)
                    {
                        string codCamaraAux = ((OrbitaVisorBase)control).Codigo;
                        if (codCamaraAux == codCamara)
                        {
                            controlEncontrado = true;
                            actualRow = row;
                            actualCol = col;
                        }
                    }
                }
            }

            if (controlEncontrado)
            {
                switch (estadoVentana)
                {
                    case FormWindowState.Maximized:
                        for (int row = 0; row < this.layFondoVisores.RowStyles.Count; row++)
                        {
                            if (row == actualRow)
                            {
                                this.layFondoVisores.RowStyles[row].Height = 100;
                            }
                            else
                            {
                                this.layFondoVisores.RowStyles[row].Height = 0;
                            }
                        }

                        for (int col = 0; col < this.layFondoVisores.ColumnStyles.Count; col++)
                        {
                            if (col == actualCol)
                            {
                                this.layFondoVisores.ColumnStyles[col].Width = 100;
                            }
                            else
                            {
                                this.layFondoVisores.ColumnStyles[col].Width = 0;
                            }
                        }
                        break;
                    case FormWindowState.Normal:
                    case FormWindowState.Minimized:
                        foreach (RowStyle rowStyle in this.layFondoVisores.RowStyles)
                        {
                            rowStyle.Height = 100 / this.layFondoVisores.RowStyles.Count;
                        }
                        foreach (ColumnStyle colStyle in this.layFondoVisores.ColumnStyles)
                        {
                            colStyle.Width = 100 / this.layFondoVisores.ColumnStyles.Count;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Método que cambia la ventana maximizada
        /// </summary>
        /// <param name="ordenCambioVisor"></param>
        private void CambioVentana(string codCamara, EventDeviceViewerChangedArgs.DeviceViewerChangeOrder ordenCambioVisor)
        {
            if (this.ListaDisplays.Count > 0)
            {
                int indiceActual = -1;
                bool encontrado = false;
                for (int i = 0; i < this.ListaDisplays.Count; i++)
                {
                    if (this.ListaDisplays[i].Codigo == codCamara)
                    {
                        indiceActual = i;
                        encontrado = true;
                        break;
                    }
                }

                if (encontrado)
                {
                    int indiceSiguiente = indiceActual;
                    switch (ordenCambioVisor)
                    {
                        case EventDeviceViewerChangedArgs.DeviceViewerChangeOrder.next:
                        default:
                            indiceSiguiente = indiceActual + 1;
                            if (!OEnteroRobusto.InRange(indiceSiguiente, 0, this.ListaDisplays.Count - 1))
                            {
                                indiceSiguiente = 0;
                            }
                            break;
                        case EventDeviceViewerChangedArgs.DeviceViewerChangeOrder.previous:
                            indiceSiguiente = indiceActual - 1;
                            if (!OEnteroRobusto.InRange(indiceSiguiente, 0, this.ListaDisplays.Count - 1))
                            {
                                indiceSiguiente = this.ListaDisplays.Count - 1;
                            }
                            break;
                    }
                    string codSiguienteCam = this.ListaDisplays[indiceSiguiente].Codigo;


                    this.ListaDisplays[indiceActual].AccionMaximizarMinimizar(FormWindowState.Normal);
                    //this.CambiarEstadoVentana(codCamara, FormWindowState.Normal);

                    this.ListaDisplays[indiceSiguiente].AccionMaximizarMinimizar(FormWindowState.Maximized);
                    //this.CambiarEstadoVentana(codSiguienteCam, FormWindowState.Maximized);
                }
            }
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Carga y muestra datos del formulario comunes para los tres modos de funcionamiento
        /// </summary>
        protected override void CargarDatosComunes()
        {
            base.CargarDatosComunes();

            if (this._EnlazarCamaras)
            {
                foreach (OCamaraBase camara in OCamaraManager.ListaCamaras)
                {
                    if (camara.Habilitado)
                    {
                        string titulo = camara.Nombre + " [" + camara.Fabricante + "]";
                        object objetoImplementado;
                        if (App.ConstruirClase(camara.EnsambladoClaseImplementadoraDisplay, camara.ClaseImplementadoraDisplay, out objetoImplementado, titulo, camara.Codigo, camara.MaxFrameIntervalVisualizacion, this._ControlCamara, this._VisualizacionEnVivo))
                        {
                            OrbitaVisorBase display = (OrbitaVisorBase)objetoImplementado;
                            this.AddDisplay(display);

                            // Añado propiedades especificas a los displays para su visualización
                            display.MostrarBtnAbrir = false;
                            display.MostrarBtnGuardar = true;
                            display.MostrarBtnReproduccion = this._ControlCamara;
                            display.MostrarBtnSnap = this._ControlCamara;
                            display.MostrarBtnInfo = true;
                            display.MostrarBtnMaximinzar = true;
                            display.MostrarBtnSiguienteAnterior = OCamaraManager.ListaCamaras.Count > 0;
                            display.OnInfoDemandada += this.OnInfoDemandada;
                            display.MostrarLblTitulo = true;
                            display.MostrarStatusBar = true;
                            display.MostrarStatusFps = this._VisualizacionEnVivo;
                            display.MostrarStatusMensaje = true;
                            display.MostrarToolStrip = true;
                            display.Inicializar();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Funciones al salir del formulario
        /// </summary>
        protected virtual void AccionesSalir()
        {
            foreach (OrbitaVisorBase display in this.ListaDisplays)
            {
                if (this._EnlazarCamaras)
                {
                    display.OnInfoDemandada -= this.OnInfoDemandada;
                }

                display.Finalizar();
            }
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Acción de añadir una cámara al formulario
        /// </summary>
        public void AddDisplay(OrbitaVisorBase display)
        {
            this.NumeroCamaras++;

            this.ListaDisplays.Add(display);

            int numColsActual = this.layFondoVisores.ColumnCount;
            int numRowsActual = this.layFondoVisores.RowCount;

            int numColsTotal = Convert.ToInt32(Math.Ceiling(Math.Sqrt(this.NumeroCamaras)));
            int numRowsTotal = Convert.ToInt32(Math.Ceiling(((double)this.NumeroCamaras / (double)numColsTotal)));

            int numColsControl = ((this.NumeroCamaras-1) % numColsTotal) + 1;
            int numRowsControl = numRowsTotal;

            // Visualización de la cámara
            display.Location = new Point(0, 0);
            this.layFondoVisores.Controls.Add(display);
            display.Visible = true;
            display.Dock = DockStyle.Fill;

            this.layFondoVisores.ColumnCount = numColsTotal;
            this.layFondoVisores.RowCount = numRowsTotal;

            if (numRowsTotal > numRowsActual)
            {
                this.layFondoVisores.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / numRowsTotal));
            }
            foreach (RowStyle rowStyle in this.layFondoVisores.RowStyles)
            {
                rowStyle.SizeType = SizeType.Percent;
                rowStyle.Height = 100 / numRowsTotal;
            }

            if (numColsTotal > numColsActual)
            {
                this.layFondoVisores.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / numColsTotal));
            }
            foreach (ColumnStyle colStyle in this.layFondoVisores.ColumnStyles)
            {
                colStyle.SizeType = SizeType.Percent;
                colStyle.Width = 100 / numColsTotal;
            }

            // Eventos
            display.OnEstadoVentanaCambiado += this.OnEstadoVentanaCambiado;
            display.OnVisorDispositivoCambiado += this.OnVisorDispositivoCambiado;
        } 
        #endregion

        #region Evento(s)
        /// <summary>
        /// Evento que se dispara cuando se realiza un cambio de estado de visualización de los displays (maximizado o normal)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnEstadoVentanaCambiado(object sender, EventStateMaximizedArgs e)
        {
            try 
            {
                this.CambiarEstadoVentana(e.CodCamara, e.EstadoVentana);
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosSistema.ImagenGraficos, e.CodCamara, exception);
            }
        }

        /// <summary>
        /// Cambio de visualización al siguiente o anterior dispositivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnVisorDispositivoCambiado(object sender, EventDeviceViewerChangedArgs e)
        {
            try 
            {
                this.CambioVentana(e.CodCamara, e.OrdenSiguienteDispositivoAVisualizar);
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosSistema.ImagenGraficos, e.CodCamara, exception);
            }
        }

        /// <summary>
        /// Visualización de ventana de información
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnInfoDemandada(object sender, EventVisorClickButtonArgs e)
        {
            try
            {
                FrmDetalleCamara frmDetalleCamara = new FrmDetalleCamara(e.CodCamara);
                frmDetalleCamara.Show();
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosSistema.ImagenGraficos, e.CodCamara, exception);
            }
        }
        #endregion
    }
}