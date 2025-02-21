//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aiba�ez
// Created          : 06-09-2012
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
using Orbita.VAComun;
using System.IO;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Ventana de detalle de la c�mara Axis
    /// </summary>
    public partial class FrmDetalleCamara : FrmDetalleVisor
    {
        #region Constructor
        /// <summary>
        /// Constructor de la c�mara
        /// </summary>
        /// <param name="codigoCamara"></param>
        public FrmDetalleCamara(string codigoCamara):
            base(codigoCamara)
        {
            InitializeComponent();
        }
        #endregion

        #region M�todo(s) heredado(s)
        /// <summary>
        /// Cargamos la informaci�n de la c�mara
        /// </summary>
        protected override void CargarInformacion()
        {
            try
            {
                DataTable dt = AppBD.GetCamara(this.Codigo);
                this.lblCodigoModelo.Text = dt.Rows[0]["CodTipoHardware"].ToString();
                this.lblFabricante.Text = "Fabricante: " + dt.Rows[0]["Fabricante"].ToString();
                this.lblModelo.Text = "Modelo: " + dt.Rows[0]["Modelo"].ToString();
                this.lblResolucion.Text = "Resoluci�n: " + dt.Rows[0]["ResolucionX"].ToString() + " x " + dt.Rows[0]["ResolucionY"].ToString();
                if ((int)dt.Rows[0]["Color"] == 1)
                {
                    this.lblColor.Text = "C�mara RGB";
                }
                else
                {
                    this.lblColor.Text = "C�mara Monocromo";
                }
                this.lblIP.Text = "IP: " + dt.Rows[0]["IPCam_IP"].ToString();
                this.lblFirmware.Text = "Firmware " + dt.Rows[0]["Firmware"].ToString();
                this.lblSerial.Text = "N�mero de serie: " + dt.Rows[0]["Basler_Pilot_DeviceID"].ToString();

                string fileName = dt.Rows[0]["FotoIlustrativa"].ToString();
                if (File.Exists(fileName))
                {
                    this.pbCamara.Load(fileName);
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.Camaras, this.Codigo, exception);
            }
        }
        #endregion
  }
}