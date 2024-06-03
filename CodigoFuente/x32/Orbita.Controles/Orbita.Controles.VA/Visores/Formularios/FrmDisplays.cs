//***********************************************************************
// Assembly         : Orbita.Controles.VA
// Author           : aibañez
// Created          : 06-09-2012
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

namespace Orbita.Controles.VA
{
    /// <summary>
    /// Formulario de agrupación de displays
    /// </summary>
    public partial class FrmDisplays : FrmBase
    {
        #region Atributo(s)
        /// <summary>
        /// Enlazamos con las cámaras
        /// </summary>
        private bool EnlazarCamaras;

        /// <summary>
        /// Se requiere visualización en vivo
        /// </summary>
        private bool VisualiacionEnVivo;

        /// <summary>
        /// Contador del número de cámaras
        /// </summary>
        private int NumeroCamaras = 0;

        /// <summary>
        /// Lista de visores
        /// </summary>
        public List<CtrlDisplay> ListaDisplays;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public FrmDisplays(string titulo, bool enlazarCamaras, bool visualiacionEnVivo)
            :base()
        {
            InitializeComponent();

            this.Text = titulo;
            this.EnlazarCamaras = enlazarCamaras;
            this.VisualiacionEnVivo = visualiacionEnVivo;
            this.ListaDisplays = new List<CtrlDisplay>();
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
                    if (control is CtrlDisplay)
                    {
                        string codCamaraAux = ((CtrlDisplay)control).Codigo;
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
                            if (!App.InRange(indiceSiguiente, 0, this.ListaDisplays.Count - 1))
                            {
                                indiceSiguiente = 0;
                            }
                            break;
                        case EventDeviceViewerChangedArgs.DeviceViewerChangeOrder.previous:
                            indiceSiguiente = indiceActual - 1;
                            if (!App.InRange(indiceSiguiente, 0, this.ListaDisplays.Count - 1))
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

            if (this.EnlazarCamaras)
            {
                foreach (CamaraBase camara in CamaraRuntime.ListaCamaras)
                {
                    DataTable dt = Orbita.VAHardware.AppBD.GetCamara(camara.Codigo);
                    if (dt.Rows.Count == 1)
                    {
                        string titulo = camara.Nombre + " [" + camara.Fabricante + "]";
                        object objetoImplementado;
                        if (App.ConstruirClase(camara.EnsambladoClaseImplementadora, camara.ClaseImplementadoraDisplay, out objetoImplementado, titulo, camara.Codigo, camara.MaxFrameIntervalVisualizacion, string.Empty, string.Empty))
                        {
                            CtrlDisplay display = (CtrlDisplay)objetoImplementado;
                            this.AddDisplay(display);

                            // Añado propiedades especificas a los displays para su visualización
                            display.MostrarBtnAbrir = false;
                            display.MostrarBtnGuardar = true;
                            display.MostrarBtnInfo = true;
                            display.OnInfoDemandada += this.OnInfoDemandada;
                            display.MostrarStatusMensaje = true;
                            camara.CrearSuscripcionMensajes(display.MostrarMensaje);
                            camara.CrearSuscripcionCambioEstado(display.OnCambioEstadoConexionCamara);
                            if (this.VisualiacionEnVivo)
	                        {
                                camara.CrearSuscripcionNuevaFotografia(display.OnNuevaFotografiaCamara);
	                        }
                        }
                    }
                }
            }
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Acción de añadir una cámara al formulario
        /// </summary>
        public void AddDisplay(CtrlDisplay display)
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
                LogsRuntime.Error(ModulosSistema.ImagenGraficos, e.CodCamara, exception);
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
                LogsRuntime.Error(ModulosSistema.ImagenGraficos, e.CodCamara, exception);
            }
        }

        /// <summary>
        /// Visualización de ventana de información
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnInfoDemandada(object sender, EventInfoRequiredArgs e)
        {
            try
            {
                FrmDetalleCamara frmDetalleCamara = new FrmDetalleCamara(e.CodCamara);
                frmDetalleCamara.Show();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosSistema.ImagenGraficos, e.CodCamara, exception);
            }
        }
        #endregion
    }
}