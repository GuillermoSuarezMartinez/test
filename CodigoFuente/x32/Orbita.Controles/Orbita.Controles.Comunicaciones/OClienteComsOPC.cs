using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Orbita.Controles.Comunicaciones
{
    public partial class OClienteComsOPC : OClienteComs
    {
        #region Variables
        /// <summary>
        /// Enlace de OPC
        /// </summary>
        string _cadenaOPC = "LOCALSERVER";
        /// <summary>
        /// Enlace de PLC
        /// </summary>
        string _cadenaPLC = "S7 connection_1";
        #endregion

        public OClienteComsOPC()
        {
            InitializeComponent();
        }

        #region Métodos
        /// <summary>
        /// Arranca las comunicaciones con el dispositivo
        /// </summary>
        public override void Iniciar()
        {
            this._cadenaOPC = this.txtCadenaOPC.Text;
            this._cadenaPLC = this.txtCadenaPLC.Text;
            base.Iniciar();
        }
        /// <summary>
        /// Procesa la información para el cambio de estado por pantalla
        /// </summary>
        /// <param name="estado"></param>
        public override void cambiarEstado(Orbita.Comunicaciones.OEstadoComms estado)
        {
            if (estado.Estado == "OK")
            {
                if (estado.Id == this._idDispositivo)
                {
                    if (estado.Enlace.Contains(this._cadenaPLC))
                    {
                        this.txtPLC.BackColor = System.Drawing.Color.Green;
                    }
                    else if (estado.Enlace.Contains(this._cadenaOPC))
                    {
                        this.txtCom.BackColor = System.Drawing.Color.Green;
                    }
                }
            }
            else
            {
                if (estado.Id == this._idDispositivo)
                {
                    if (estado.Enlace.Contains("S7 connection_1"))
                    {
                        this.txtPLC.BackColor = System.Drawing.Color.Red;
                    }
                    else if (estado.Enlace.Contains("LOCALSERVER"))
                    {
                        this.txtCom.BackColor = System.Drawing.Color.Red;
                    }
                }
            }
        }
        #endregion
    }
}
