//***********************************************************************
// Assembly         : Orbita.VAComun
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Orbita.Controles;
using Orbita.Utiles;

namespace Orbita.VAComun
{
    /// <summary>
    /// Formulario utilizado para la gestión de escritorios
    /// </summary>
    public partial class FrmGestionEscritorios : FrmDialogoBase
    {
        #region Atributo(s)
        /// <summary>
        /// Parametros que se están editando
        /// </summary>
        public OpcionesEscritorios OpcionesEscritorios;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public FrmGestionEscritorios()
            : base()
        {
            InitializeComponent();

            this.ModoAperturaFormulario = ModoAperturaFormulario.Modificacion;
            this.OpcionesEscritorios = SystemRuntime.Configuracion.OpcionesEscritorio;
        } 
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public FrmGestionEscritorios(OpcionesEscritorios opcionesEscritorio)
            : base(ModoAperturaFormulario.Modificacion)
        {
            this.InitializeComponent();

            this.OpcionesEscritorios = opcionesEscritorio;
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Carga y muestra datos del formulario comunes para los tres modos de funcionamiento
        /// </summary>
        protected override void CargarDatosComunes()
        {
            base.CargarDatosComunes();

            this.SuspendLayout();

            // Escritorios
            this.CargarGridEscritorios();

            // General
            this.checkHabilitado.Checked = this.OpcionesEscritorios.ManejoAvanzadoEscritorio;
            this.checkAnclaje.Checked = this.OpcionesEscritorios.PermiteAnclajes;
            this.checkHabilitado_CheckedChanged(this.checkHabilitado, new EventArgs());
            this.checkAutoAbrirFoms.Checked = this.OpcionesEscritorios.AutoAbrirFoms;
            this.checkPreferenciaMaximizado.Checked = this.OpcionesEscritorios.PreferenciaMaximizado;
            this.CargarComboEscritorios();

            this.ResumeLayout();
        }
        /// <summary>
        /// Se añade el evento de monitorización a los controles del formulario
        /// </summary>
        protected override void IniciarMonitorizarModificaciones()
        {
            base.IniciarMonitorizarModificaciones();

            this.GridEscritorios.OrbBotonAñadirClick += this.EventoCambioValor;
            this.GridEscritorios.OrbBotonEliminarFilaClick += this.EventoCambioValor;
        }
        /// <summary>
        /// Se elimina el evento de monitorización a los controles del formulario
        /// </summary>
        protected override void FinalizarMonitorizarModificaciones()
        {
            base.FinalizarMonitorizarModificaciones();

            this.GridEscritorios.OrbBotonAñadirClick -= this.EventoCambioValor;
            this.GridEscritorios.OrbBotonEliminarFilaClick -= this.EventoCambioValor;
        }
        /// <summary>
        /// Realiza las comprobaciones pertinentes antes de realizar un guardado de los datos. Se usa para el caso en que hayan restricciones en el momento de guardar los datos
        /// </summary>
        /// <returns>True si todo está correcto para ser guardado; false en caso contrario</returns>
        protected override bool ComprobacionesDeCampos()
        {
            bool resultado = base.ComprobacionesDeCampos();
            try
            {
                foreach (UltraGridRow row in this.GridEscritorios.OrbGrid.Rows)
                {
                    if (!(row.Cells["Valor"].Value is OpcionesEscritorio))
                    {    
                        resultado = false;
                        throw new Exception("La lista de escritorios no es válida");
                    }

                    OpcionesEscritorio opcionesEscritorio = (OpcionesEscritorio)row.Cells["Valor"].Value;
                    resultado &= App.ValidarTexto(opcionesEscritorio.Nombre, "Lista Escritorios", 50, false, true);
                }

                if (resultado && this.checkHabilitado.Checked)
                {
                    if (!App.ComprobarFila(this.cboEscritorios.OrbCombo.ActiveRow))
                    {
                        resultado = false;
                        throw new Exception("El campo Escritorio Predefinido no es válido");
                    }
                    resultado &= App.ValidarTexto(this.cboEscritorios.OrbCombo.ActiveRow.Cells["Indice"].Value, "Escritorio Predefinido", 50, false, true);
                    string nombreEscritorioPredefinido = (string)this.cboEscritorios.OrbCombo.ActiveRow.Cells["Indice"].Value;

                    // Escritorios
                    List<OpcionesEscritorio> listaOpcionesEscritorio = this.ObtenerListaOpcionesEscritoriosDeGrid();
                    bool existeEscritorio = listaOpcionesEscritorio.Exists(delegate(OpcionesEscritorio opcionesEscritorio) { return opcionesEscritorio.Nombre == nombreEscritorioPredefinido; });
                    if (!existeEscritorio)
                    {
                        resultado = false;
                        throw new Exception("No existe el Escritorio Predefinido seleccionado");
                    }
                }
            }
            catch (Exception exception)
            {
                //LogsRuntime.Error(ModulosSistema.Escritorios, "ComprobacionesDeCampos", exception);
                OMensajes.MostrarAviso(exception.Message);
                resultado = false;
            }

            return resultado;
        }
        /// <summary>
        /// Guarda los datos cuando el formulario está abierto en modo Modificación
        /// </summary>
        /// <returns>True si la operación de guardado de datos ha tenido éxito; false en caso contrario</returns>
        protected override bool GuardarDatosModoModificacion()
        {
            bool resultado = false;
            base.GuardarDatosModoModificacion();
            resultado = this.VolcarDatosOpcionesEscritorios();
            SystemRuntime.Configuracion.Guardar();
            return resultado;
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Carga y aplica un formato al Combo con los datos existentes
        /// </summary>
        private void CargarComboEscritorios()
        {
            this.cboEscritorios.OrbLimpiarValor();

            List<OpcionesEscritorio> listaOpcionesEscritorio = this.ObtenerListaOpcionesEscritoriosDeGrid();
            if (listaOpcionesEscritorio.Count > 0)
            {
                Dictionary<object, string> valores = new Dictionary<object, string>();
                foreach (OpcionesEscritorio opcionesEscritorio in listaOpcionesEscritorio)
                {
                    valores.Add(opcionesEscritorio.Nombre, opcionesEscritorio.Nombre);
                }
                App.CargarCombo(this.cboEscritorios, valores, typeof(string), this.OpcionesEscritorios.NombreEscritorioActual);
            }
        }

        /// <summary>
        /// Carga y aplica un formato al Combo con los datos existentes
        /// </summary>
        private void RefrescarComboEscritorios()
        {
            //Obtenemos información sobre el registro está actualmente seleccionado
            string nombreEscritorioSeleccionado = this.cboEscritorios.OrbTexto; // Nunca puede ser null

            // Limpiamos los valores del combo
            this.cboEscritorios.OrbLimpiarValor();

            List<OpcionesEscritorio> listaOpcionesEscritorio = this.ObtenerListaOpcionesEscritoriosDeGrid();
            if (listaOpcionesEscritorio.Count > 0)
            {
                Dictionary<object, string> valores = new Dictionary<object, string>();
                foreach (OpcionesEscritorio opcionesEscritorio in listaOpcionesEscritorio)
                {
                    valores.Add(opcionesEscritorio.Nombre, opcionesEscritorio.Nombre);
                }
                App.CargarCombo(this.cboEscritorios, valores, typeof(string), nombreEscritorioSeleccionado);
            }
        }

        /// <summary>
        /// Carga y aplica un formato al Grid con los datos existentes
        /// </summary>
        private void CargarGridEscritorios()
        {
            List<object> valores = new List<object>();
            foreach (OpcionesEscritorio opcionesEscritorio in this.OpcionesEscritorios.ListaOpcionesEscritorio)
            {
                valores.Add(opcionesEscritorio);
            }
            App.CargarGridSimple(this.GridEscritorios, valores, typeof(OpcionesEscritorio), Estilos.EstiloColumna.Texto, Estilos.Alineacion.Izquierda, null, 150, null);

            this.GridEscritorios.OrbToolbarAgregarBoton("Establecer", global::Orbita.VAComun.Properties.Resources.btnEstablecerEscritorio16, "Establecer", 2, true);
        }

        /// <summary>
        /// Método que guarda los valores de los componentes de edición del formulario a la clase XMLTarea
        /// </summary>
        private bool VolcarDatosOpcionesEscritorios()
        {
            bool resultado = true;
            try
            {
                // General
                this.OpcionesEscritorios.AutoAbrirFoms = this.checkAutoAbrirFoms.Checked;
                this.OpcionesEscritorios.ManejoAvanzadoEscritorio = this.checkHabilitado.Checked;
                this.OpcionesEscritorios.PermiteAnclajes = this.checkAnclaje.Checked;
                this.OpcionesEscritorios.PreferenciaMaximizado = this.checkPreferenciaMaximizado.Checked;
                this.OpcionesEscritorios.NombreEscritorioActual = string.Empty;
                if (this.cboEscritorios.OrbCombo.ActiveRow != null)
                {
                    this.OpcionesEscritorios.NombreEscritorioActual = (string)this.cboEscritorios.OrbCombo.ActiveRow.Cells["Indice"].Value;
                }

                // Escritorios
                this.OpcionesEscritorios.ListaOpcionesEscritorio = this.ObtenerListaOpcionesEscritoriosDeGrid();
            }
            catch (System.Exception exception)
            {
                LogsRuntime.Error(ModulosSistema.Escritorios, "Guardar datos", exception);
                resultado = false;
            }
            return resultado;
        }

        /// <summary>
        /// añade una nueva fila a un grid simple (Una sóla columna)
        /// </summary>
        private void NuevaFila(OrbitaGridPro grid, object valor)
        {
            grid.OrbFilaActiva = grid.OrbGrid.DisplayLayout.Bands[0].AddNew();
            grid.OrbFilaActiva.Cells[0].Value = valor;
        }

        /// <summary>
        /// Extrae la lista de escritorios del grid
        /// </summary>
        private List<OpcionesEscritorio> ObtenerListaOpcionesEscritoriosDeGrid()
        {
            List<OpcionesEscritorio> listaOpcionesEscritorio = new List<OpcionesEscritorio>();
            foreach (UltraGridRow row in this.GridEscritorios.OrbGrid.Rows)
            {
                if (row.Cells["Valor"].Value is OpcionesEscritorio)
                {
                    listaOpcionesEscritorio.Add((OpcionesEscritorio)row.Cells["Valor"].Value);
                }
            }

            return listaOpcionesEscritorio;
        }

        /// <summary>
        /// Valida que el nuevo nombre no exista previamente entre los nombres dados de alta en el grid
        /// </summary>
        /// <param name="anteriorNombre">Nombre anterior</param>
        /// <param name="nuevoNombre">Nuevo nombre</param>
        /// <returns></returns>
        private bool ValidarNuevoNombreEscritorioEnGrid(string anteriorNombre, string nuevoNombre)
        {
            bool resultado = true;

            if (anteriorNombre != nuevoNombre)
            {
                List<OpcionesEscritorio> listaOpcionesEscritorio = this.ObtenerListaOpcionesEscritoriosDeGrid();
                foreach (OpcionesEscritorio opcionesEscriorio in listaOpcionesEscritorio)
                {
                    if (nuevoNombre == opcionesEscriorio.Nombre)
                    {
                        resultado = false;
                        break;
                    }
                }
            }

            return resultado;
        }
        #endregion

        #region Evento(s)
        /// <summary>
        /// Cambio de habilitar/deshabilitar manejo de escritorios
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="celda">Argumentos del evento</param>
        private void checkHabilitado_CheckedChanged(object sender, EventArgs e)
        {
            this.PnlOpcionesEscritorios.Visible = this.checkHabilitado.Checked;
        }

        /// <summary>
        /// Nuevo escritorio
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void GridEscritorios_OrbBotonAñadirClick(object sender, EventArgs e)
        {
            try
            {
                string nombreNuevoEscritorio = "Nuevo";
                string nombreDefinitivoNuevoEscritorio = nombreNuevoEscritorio;
                int i = 1;
                while (!this.ValidarNuevoNombreEscritorioEnGrid(string.Empty, nombreDefinitivoNuevoEscritorio))
                {
                    nombreDefinitivoNuevoEscritorio = nombreNuevoEscritorio + i.ToString();
                    i++;
                }

                Escritorio escritorio = new Escritorio(nombreDefinitivoNuevoEscritorio);
                escritorio.ObtenerEscritorioAplicacion();

                OpcionesEscritorio opcionesEscritorio = new OpcionesEscritorio();
                opcionesEscritorio.Nombre = escritorio.Nombre;
                opcionesEscritorio.ListaInfoPosForms = escritorio.ListaInfoPosForms;

                this.NuevaFila(this.GridEscritorios, opcionesEscritorio);

                this.RefrescarComboEscritorios();
            }
            catch (System.Exception exception)
            {
                LogsRuntime.Error(ModulosSistema.Escritorios, "Nuevo escritorio", exception);
            }
        }

        /// <summary>
        /// Eliminar escritorio
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void GridEscritorios_OrbBotonEliminarFilaClick(object sender, EventArgs e)
        {
            try
            {
                if (App.ComprobarGrid(this.GridEscritorios)) // Si hay alguna selección en el grid
                {
                    if (OMensajes.MostrarPreguntaSiNo("¿Desea eliminar el escritorio " + this.GridEscritorios.OrbFilaActiva.Cells["Valor"].Text + "?") == DialogResult.Yes)
                    {
                        this.GridEscritorios.OrbEliminarFilaActiva();

                        this.RefrescarComboEscritorios();
                    }
                }
            }
            catch (System.Exception exception)
            {
                LogsRuntime.Error(ModulosSistema.Escritorios, "Eliminar escritorio", exception);
            }
        }

        /// <summary>
        /// Cambios en las celdas del grid de la lista de escritorios
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void GridEscritorios_OrbCeldaFinEdicion(object sender, UltraGridCell celda)
        {
            try
            {
                if ((celda.Value is string) && (celda.OriginalValue is OpcionesEscritorio))
                {
                    string anteriorNombre = ((OpcionesEscritorio)celda.OriginalValue).Nombre;
                    string nuevoNombre = (string)celda.Value;
                    if (!this.ValidarNuevoNombreEscritorioEnGrid(anteriorNombre, nuevoNombre))
                    {
                        OMensajes.MostrarAviso("Ya existe un escritorio con el nombre " + nuevoNombre);
                        nuevoNombre = anteriorNombre;
                    }

                    OpcionesEscritorio opcionesEscritorio = (OpcionesEscritorio)celda.OriginalValue;
                    opcionesEscritorio.Nombre = nuevoNombre;
                    celda.Value = opcionesEscritorio;

                    this.RefrescarComboEscritorios();
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosSistema.Escritorios, "FinEdicionGridEscritorios", exception);
            }
        }

        /// <summary>
        /// Click en los botones personalizados de la barra del grid de escritorios
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void GridEscritorios_OrbToolbarClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            try
            {
                if (e.Tool.Key == "Establecer")
                {
                    if (App.ComprobarGrid(this.GridEscritorios)) // Si hay alguna selección en el grid
                    {
                        //Obtenemos información sobre el registro está actualmente seleccionado
                        object valorCeldaActiva = this.GridEscritorios.OrbFilaActiva.Cells["Valor"].Value;
                        if (valorCeldaActiva is OpcionesEscritorio)
                        {
                            OpcionesEscritorio opcionesEscritorio = (OpcionesEscritorio)valorCeldaActiva;
                            Escritorio escritorio = new Escritorio(opcionesEscritorio.Nombre, opcionesEscritorio.ListaInfoPosForms);
                            escritorio.EstablecerEscritorioAplicacion();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosSistema.Escritorios, "ClickEnBarraGridEscritorios", exception);
            }
        }
        #endregion
    }
}                                        