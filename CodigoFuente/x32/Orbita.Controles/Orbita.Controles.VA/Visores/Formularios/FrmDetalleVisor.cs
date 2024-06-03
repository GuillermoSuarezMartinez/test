//***********************************************************************
// Assembly         : Orbita.Controles.VA
// Author           : aibañez
// Created          : 31-10-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Data;
using Orbita.Controles;

namespace Orbita.Controles.VA
{
    /// <summary>
    /// Formulario base para la visualización de información sobre el origen de la imagen
    /// </summary>
    public partial class FrmDetalleVisor : OrbitaDialog
    {
        #region Atributo(s)
        /// <summary>
        /// Codigo identificativo de la camara
        /// </summary>
        protected string Codigo;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codigoCamara"></param>
        public FrmDetalleVisor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codigoCamara"></param>
        public FrmDetalleVisor(string codigoCamara)
        {
            InitializeComponent();
            this.Codigo = codigoCamara;
        }
        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Cargamos la información de la cámara
        /// </summary>
        protected virtual void CargarInformacion()
        {
        }
        #endregion

        #region Evento(s)
        /// <summary>
        /// Cerramos la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CerrarVentana(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Cerramos la ventana auromáticamente despues de 10 segundos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerCierre_Tick(object sender, EventArgs e)
        {
            this.timerCierre.Enabled = false;
            this.Close();
        }

        /// <summary>
        /// Carga de información del formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmDetalleVisor_Load(object sender, EventArgs e)
        {
            this.CargarInformacion();
            this.timerCierre.Enabled = true;
        }
        #endregion
    }
}