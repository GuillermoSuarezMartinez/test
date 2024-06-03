//***********************************************************************
// Assembly         : Orbita.Controles.VA
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
using System.Collections;
using System.Data;
using System.Windows.Forms;
using Orbita.Controles;
using Orbita.Utiles;
using Orbita.VAComun;
using Orbita.VAControl;

namespace Orbita.Controles.VA
{
    public partial class FrmGestionVariables : FrmBase
    {
        #region Constructor(es)
        /// <summary>
        /// Constructor de la calse
        /// </summary>
        /// <param name="modoAperturaFormulario">Modo de apertura del formulario</param>
        public FrmGestionVariables() :
            base()
        {
            InitializeComponent();
        } 
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Carga y muestra datos del formulario comunes para los tres modos de funcionamiento
        /// </summary>
        protected override void CargarDatosComunes()
        {
            base.CargarDatosComunes();

            CargarVariables();
        }
        /// <summary>
        ///  Método para que sea sobreescrito en el hijo y realizar funciones si el usuario no desea guardar los cambios 
        /// </summary>
        protected override void AccionesNoGuardar()
        {
        }
        #endregion Métodos virtuales

        #region Eventos
        /// <summary>
        /// Evento que refresca el grid de varibles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridVariables_OrbBotonRefrescarClick(object sender, EventArgs e)
        {
            try
            {
                CargarVariables();
            }
            catch (Exception exception)
            {
                //LogsRuntime.Error(ModulosSistema.Comun, "DirectoryCopy", exception);
                //MensajeError.MostrarExcepcion(ex);
            }
        }
        /// <summary>
        ///  Evento que elimina una variable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridVariables_OrbBotonEliminarFilaClick(object sender, EventArgs e)
        {
            try
            {
                if (OMensajes.MostrarPreguntaSiNo("¿Desea eliminar la variable " + gridVariables.OrbFilaActiva.Cells["CodVariable"].Value.ToString() + "?") == DialogResult.Yes)
                {
                    //Indice en la tabla
                    int indice = this.gridVariables.OrbFilaActiva.VisibleIndex;

                    //LLamada a la funcion que llama al procedimiento SQL
                    Orbita.VAControl.AppBD.EliminaVariable((int)gridVariables.OrbFilaActiva.Cells["IdVariable"].Value);

                    //Refrescamos la lista
                    this.CargarVariables();

                    //Dejar seleccionada la fila con la entidad
                    this.gridVariables.OrbActivarFila(indice);
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Fatal(ModulosControl.GestionVariables, "Eliminación", exception);
            }
        }
        /// <summary>
        /// Evento que muestra el formulario para añadir variables
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridVariables_OrbBotonAñadirClick(object sender, EventArgs e)
        {
            // Creamos el formulario de nueva variable
            FrmGestionVariablesEdicion frmGestionVariablesEdicion = new FrmGestionVariablesEdicion();

            if (frmGestionVariablesEdicion.ShowDialog() == DialogResult.OK)
            {
                CargarVariables();
            }
        }
        /// <summary>
        /// Evento que muestra el formulario para modificar la variable seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridVariables_OrbBotonModificarClick(object sender, EventArgs e)
        {
            // Creamos el formaulario de edicion de la variable
            FrmGestionVariablesEdicion frmGestionVariablesEdicion = new FrmGestionVariablesEdicion(
                (int)gridVariables.OrbFilaActiva.Cells["IdVariable"].Value,
                gridVariables.OrbFilaActiva.Cells["CodVariable"].Value.ToString(),
                gridVariables.OrbFilaActiva.Cells["NombreVariable"].Value.ToString(),
                gridVariables.OrbFilaActiva.Cells["DescVariable"].Value.ToString(),
                (bool)gridVariables.OrbFilaActiva.Cells["HabilitadoVariable"].Value,
                gridVariables.OrbFilaActiva.Cells["Grupo"].Value.ToString(),
                (bool)gridVariables.OrbFilaActiva.Cells["GuardarTrazabilidad"].Value,
                (int)gridVariables.OrbFilaActiva.Cells["IdTipoVariable"].Value);

            if (frmGestionVariablesEdicion.ShowDialog() == DialogResult.OK)
            {
                //Indice en la tabla
                int indice = this.gridVariables.OrbFilaActiva.VisibleIndex;

                // Cargamos las variables
                CargarVariables();

                //Dejar seleccionada la fila con la entidad
                this.gridVariables.OrbActivarFila(indice);
            }
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        ///  Metodo para cargar las variables
        /// </summary>
        private void CargarVariables()
        {
            DataTable dt = Orbita.VAControl.AppBD.GetListaVariables();
            if (dt != null)
            {
                //Formateamos y cargamos el grid
                ArrayList cols = new ArrayList();
                cols.Add(new Estilos.CamposEstilos("CodVariable", "Código",150));
                cols.Add(new Estilos.CamposEstilos("NombreVariable", "Nombre",190));
                cols.Add(new Estilos.CamposEstilos("DescVariable", "Descripción",270));
                cols.Add(new Estilos.CamposEstilos("HabilitadoVariable", "Habilitado", Estilos.EstiloColumna.Check, Estilos.Alineacion.Centrado,80));
                cols.Add(new Estilos.CamposEstilos("Grupo", "Grupo",114));
                cols.Add(new Estilos.CamposEstilos("GuardarTrazabilidad", "Trazabilidad", Estilos.EstiloColumna.Check, Estilos.Alineacion.Centrado,80));
                cols.Add(new Estilos.CamposEstilos("NombreTipoVariable", "Tipo",80));

                gridVariables.OrbFormatear(dt, cols);
            }
        }
        #endregion

    }
}
