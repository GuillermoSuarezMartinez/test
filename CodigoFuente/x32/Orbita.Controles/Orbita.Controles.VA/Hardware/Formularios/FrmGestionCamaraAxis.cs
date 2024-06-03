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

namespace Orbita.Controles.VA
{
    public partial class FrmGestionCamaraAxis : FrmBase
    {
        #region Atributo(s)
        /// <summary>
        /// Dirección IP de la cámara
        /// </summary>
        private string IP = "";
        #endregion

        #region Constructor
        public FrmGestionCamaraAxis(string direccionIP)
            : base(ModoAperturaFormulario.Visualizacion)
        {
            InitializeComponent();
            this.IP = direccionIP;
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Carga y muestra datos del formulario comunes para los tres modos de funcionamiento
        /// </summary>
        protected override void CargarDatosComunes()
        {
            base.CargarDatosComunes();

            Uri uri1 = new Uri(" http://" + this.IP);
            this.webBrowser.Navigate(uri1);
            return;
        }
        
        #endregion Métodos virtuales

    }
}