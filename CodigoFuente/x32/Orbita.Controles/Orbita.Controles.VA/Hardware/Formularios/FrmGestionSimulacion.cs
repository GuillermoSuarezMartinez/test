//***********************************************************************
// Assembly         : Orbita.Controles.VA
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 16-11-2012
// Description      : Movido al proyecto Orbita.Controles.VA
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using Orbita.Utiles;
using Orbita.VAHardware;
using Orbita.VAComun;

//TODO! Que no modifique los objetos simulación hasta que no se acepte el formulario.
namespace Orbita.Controles.VA
{
    /// <summary>
    /// Formulario para la configuración del modo simulación de cámaras
    /// </summary>
    public partial class FrmGestionSimulacion : FrmBase
    {
        #region Atributo(s)

        /// <summary>
        /// Código de la máquina de estados
        /// </summary>
        private string Codigo;

        /// <summary>
        /// Indica si únicamente se ha de mostrar una cámara
        /// </summary>
        private bool Filtrado;

        /// <summary>
        /// Lista de las simulaciones que se están editando
        /// </summary>
        private SimulacionCamara SimulacionCamara;

        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la calse
        /// </summary>
        /// <param name="modoAperturaFormulario">Modo de apertura del formulario</param>
        public FrmGestionSimulacion(string codigo)
            : base()
        {
            InitializeComponent();

            this.Codigo = codigo;
            this.Filtrado = true;
            this.Text = "Opciones de simulación de la cámara [" + codigo + "]";
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Carga y muestra datos del formulario comunes para los tres modos de funcionamiento
        /// </summary>
        protected override void CargarDatosComunes()
        {
            base.CargarDatosComunes();

            // Rellenamos el binding de los tipos de simulaciones
            List<EnumeracionCombo> listaTiposSimulaciones = new List<EnumeracionCombo>();
            listaTiposSimulaciones.Add(new EnumeracionCombo(TipoSimulacionCamara.FotografiaSimple, "Fotografía de disco"));
            listaTiposSimulaciones.Add(new EnumeracionCombo(TipoSimulacionCamara.DirectorioFotografias, "Carpeta de fotografías de disco"));
            this.bindingTipoSimulacion.DataSource = listaTiposSimulaciones;

            // Rellenamos el binding de las simulaciones
            this.SimulacionCamara = new SimulacionCamara(this.Codigo);
            foreach (CamaraBase camara in CamaraRuntime.ListaCamaras)
            {
                if (this.Codigo == camara.Codigo)
                {
                    this.SimulacionCamara = camara.SimulacionCamara;
                }
            }
            this.BindingSimulacionCamara.DataSource = this.SimulacionCamara;
        }
        /// <summary>
        /// Comprueba si se ha modificado algún dato en algún campo del formulario
        /// </summary>
        /// <returns>True si se han modificado datos; false en caso contrario</returns>
        protected override bool ComprobarDatosModificados()
        {
            return true;
        }
        /// <summary>
        /// Realiza las comprobaciones pertinentes antes de realizar un guardado de los datos. Se usa para el caso en que hayan restricciones en el momento de guardar los datos
        /// </summary>
        /// <returns>True si todo está correcto para ser guardado; false en caso contrario</returns>
        protected override bool ComprobacionesDeCampos()
        {
            bool resultado = base.ComprobacionesDeCampos();

            if (this.checkSimulacion.Checked)
            {
                // Comprobación del filtro
                switch ((TipoSimulacionCamara)this.cboTipoSimulacion.SelectedValue)
                {
                    case TipoSimulacionCamara.DirectorioFotografias:
                        string valor = this.txtFiltro.Text;
                        resultado &= valor != string.Empty;
                        if (!resultado)
                        {
                            OMensajes.MostrarAviso("No se ha especificado ningún filtro.");
                            return false;
                        }
                        break;
                }

                // Comprobación de la ruta
                switch ((TipoSimulacionCamara)this.cboTipoSimulacion.SelectedValue)
                {
                    case TipoSimulacionCamara.FotografiaSimple:
                        if (!File.Exists(this.txtRutaFotografias.Text))
                        {
                            OMensajes.MostrarAviso("La imagen seleccionada no existe o es incorrecta.");
                            return false;
                        }
                        break;
                    case TipoSimulacionCamara.DirectorioFotografias:
                        if (!Directory.Exists(this.txtRutaFotografias.Text))
                        {
                            OMensajes.MostrarAviso("La carpeta seleccionada no existe o es incorrecta.");
                            return false;
                        }

                        if (Directory.GetFiles(this.txtRutaFotografias.Text, this.txtFiltro.Text, SearchOption.TopDirectoryOnly).Length == 0)
                        {
                            OMensajes.MostrarAviso("La carpeta seleccionada no contiene ningún archivo que cumpla los criterios del filtro.");
                            return false;
                        }
                        break;
                }

                // Comprobación del intervalo
                switch ((TipoSimulacionCamara)this.cboTipoSimulacion.SelectedValue)
                {
                    case TipoSimulacionCamara.DirectorioFotografias:
                        int valor;
                        resultado &= int.TryParse(this.txtIntervaloEntreSnaps.Text, out valor);
                        resultado &= App.InRange(valor, 1, 100000);
                        if (!resultado)
                        {
                            OMensajes.MostrarAviso("El intervalo de tiempo entre fotografías no es correcto.");
                            return false;
                        }
                        break;
                }
            }

            return resultado;
        }
        /// <summary>
        /// Guarda los datos cuando el formulario está abierto en modo modifiación
        /// </summary>
        /// <returns>True si la operación de guardado de datos ha tenido éxito; false en caso contrario</returns>
        protected override bool GuardarDatosModoModificacion()
        {
            base.GuardarDatosModoModificacion();

            this.BindingSimulacionCamara.EndEdit();

            return this.SimulacionCamara.ConfigurarModoSimulacion(); ;
        }
        /// <summary>
        ///  Método para que sea sobreescrito en el hijo y realizar funciones si el usuario no desea guardar los cambios 
        /// </summary>
        protected override void AccionesNoGuardar()
        {
            this.BindingSimulacionCamara.CancelEdit();
        }
        #endregion Métodos virtuales

        #region Eventos
        /// <summary>
        /// Evento de seleccionar un fichero de imagen o una carpeta donde existan imágenes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDialogoRuta_Click(object sender, EventArgs e)
        {
            try
            {
                switch ((TipoSimulacionCamara)this.cboTipoSimulacion.SelectedValue)
                {
                    case TipoSimulacionCamara.FotografiaSimple:
                        string rutaArchivo = this.txtRutaFotografias.Text;
                        bool archivoSeleccionadoOK = App.FormularioSeleccionArchivo(this.openFileDialog, ref rutaArchivo);
                        if (archivoSeleccionadoOK)
                        {
                            this.txtRutaFotografias.Text = rutaArchivo;
                            this.txtRutaFotografias.Focus();
                            this.btnDialogoRuta.Focus();
                        }						
                        break;
                    case TipoSimulacionCamara.DirectorioFotografias:
                        string rutaCarpeta = this.txtRutaFotografias.Text;
                        bool carpetaSeleccionadaOK = App.FormularioSeleccionCarpeta(this.folderBrowserDialog, ref rutaCarpeta);
                        if (carpetaSeleccionadaOK)
                        {
                            this.txtRutaFotografias.Text = rutaCarpeta;
                            this.txtRutaFotografias.Focus();
                            this.btnDialogoRuta.Focus();
                        }
                        break;
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.Camaras, this.Name, exception);
            }
        }

        /// <summary>
        /// Evento de modificación de los datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Simulacion_StateChanged(object sender, EventArgs e)
        {
            try
            {
                this.lblTipoSimulacion.Visible = this.checkSimulacion.Checked;
                this.cboTipoSimulacion.Visible = this.checkSimulacion.Checked;
                this.lblRutaFotografias.Visible = this.checkSimulacion.Checked;
                this.txtRutaFotografias.Visible = this.checkSimulacion.Checked;
                this.btnDialogoRuta.Visible = this.checkSimulacion.Checked;
                if (this.cboTipoSimulacion.SelectedValue is TipoSimulacionCamara)
                {
                    this.lblIntervaloEntreSnaps.Visible = this.checkSimulacion.Checked && ((TipoSimulacionCamara)this.cboTipoSimulacion.SelectedValue == TipoSimulacionCamara.DirectorioFotografias);
                    this.txtIntervaloEntreSnaps.Visible = this.checkSimulacion.Checked && ((TipoSimulacionCamara)this.cboTipoSimulacion.SelectedValue == TipoSimulacionCamara.DirectorioFotografias);
                    this.lblMsIntervaloEntreSnaps.Visible = this.checkSimulacion.Checked && ((TipoSimulacionCamara)this.cboTipoSimulacion.SelectedValue == TipoSimulacionCamara.DirectorioFotografias);
                    this.txtFiltro.Visible = this.checkSimulacion.Checked && ((TipoSimulacionCamara)this.cboTipoSimulacion.SelectedValue == TipoSimulacionCamara.DirectorioFotografias);
                    this.lblFiltro.Visible = this.checkSimulacion.Checked && ((TipoSimulacionCamara)this.cboTipoSimulacion.SelectedValue == TipoSimulacionCamara.DirectorioFotografias);
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.Camaras, this.Name, exception);
            }
        }
        #endregion

    }
}
