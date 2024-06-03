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
using System.Data;
using Orbita.Controles;
using System.IO;
using Orbita.VAHardware;
using Orbita.VAComun;

namespace Orbita.Controles.VA
{
    /// <summary>
    /// Ventana de detalle de la cámara Axis
    /// </summary>
    public partial class FrmDetalleCamara : FrmDetalleVisor
    {
        #region Constructor
        /// <summary>
        /// Constructor de la cámara
        /// </summary>
        /// <param name="codigoCamara"></param>
        public FrmDetalleCamara(string codigoCamara):
            base(codigoCamara)
        {
            InitializeComponent();
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Cargamos la información de la cámara
        /// </summary>
        protected override void CargarInformacion()
        {
            try
            {
                DataTable dt = Orbita.VAHardware.AppBD.GetCamara(this.Codigo);
                this.lblCodigoModelo.Text = dt.Rows[0]["CodTipoHardware"].ToString();
                this.lblFabricante.Text = "Fabricante: " + dt.Rows[0]["Fabricante"].ToString();
                this.lblModelo.Text = "Modelo: " + dt.Rows[0]["Modelo"].ToString();
                this.lblResolucion.Text = "Resolución: " + dt.Rows[0]["ResolucionX"].ToString() + " x " + dt.Rows[0]["ResolucionY"].ToString();
                if ((int)dt.Rows[0]["Color"] == 1)
                {
                    this.lblColor.Text = "Cámara RGB";
                }
                else
                {
                    this.lblColor.Text = "Cámara Monocromo";
                }
                this.lblIP.Text = "IP: " + dt.Rows[0]["IPCam_IP"].ToString();
                this.lblFirmware.Text = "Firmware " + dt.Rows[0]["Firmware"].ToString();
                this.lblSerial.Text = "Número de serie: " + dt.Rows[0]["Basler_Pilot_DeviceID"].ToString();

                string fileName = dt.Rows[0]["FotoIlustrativa"].ToString();
                if (File.Exists(fileName))
                {
                    this.pbCamara.Load(fileName);
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.Camaras, this.Codigo, exception);
            }
        }
        #endregion
  }
}