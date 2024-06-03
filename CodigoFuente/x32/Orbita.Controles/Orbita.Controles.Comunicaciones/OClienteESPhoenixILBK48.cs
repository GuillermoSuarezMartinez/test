using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Orbita.Comunicaciones;
using Orbita.Utiles;

namespace Orbita.Controles.Comunicaciones
{
    public partial class OClienteESPhoenixILBK48 : OClienteComs
    {

        public OClienteESPhoenixILBK48()
        {
            InitializeComponent();
        }

        #region Métodos

        /// <summary>
        /// Actualiza las ES del control
        /// </summary>
        /// <param name="e"></param>
        private void actualizarES(OEventArgs e)
        {
            if (InvokeRequired)
            {
                DelegadoES delegado = new DelegadoES(actualizarES);
                this.Invoke(delegado, new object[] { e });
            }
            else
            {
                OInfoDato dato = (OInfoDato)e.Argumento;

                switch (dato.Texto)
                {
                    case "E11":
                        if ((int)dato.Valor == 1)
                        {
                            this.E11.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.E11.BackColor = Color.Beige;
                        }
                        break;
                    case "E12":
                        if ((int)dato.Valor == 1)
                        {
                            this.E12.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.E12.BackColor = Color.Beige;
                        }
                        break;
                    case "E13":
                        if ((int)dato.Valor == 1)
                        {
                            this.E13.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.E13.BackColor = Color.Beige;
                        }
                        break;
                    case "E14":
                        if ((int)dato.Valor == 1)
                        {
                            this.E14.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.E14.BackColor = Color.Beige;
                        }
                        break;
                    case "S11":
                        if ((int)dato.Valor == 1)
                        {
                            this.S11.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.S11.BackColor = Color.Beige;
                        }
                        break;
                    case "S12":
                        if ((int)dato.Valor == 1)
                        {
                            this.S12.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.S12.BackColor = Color.Beige;
                        }
                        break;
                    case "S13":
                        if ((int)dato.Valor == 1)
                        {
                            this.S13.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.S13.BackColor = Color.Beige;
                        }
                        break;
                    case "S14":
                        if ((int)dato.Valor == 1)
                        {
                            this.S14.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.S14.BackColor = Color.Beige;
                        }
                        break;
                    case "E21":
                        if ((int)dato.Valor == 1)
                        {
                            this.E21.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.E21.BackColor = Color.Beige;
                        }
                        break;
                    case "E22":
                        if ((int)dato.Valor == 1)
                        {
                            this.E22.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.E22.BackColor = Color.Beige;
                        }
                        break;
                    case "E23":
                        if ((int)dato.Valor == 1)
                        {
                            this.E23.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.E23.BackColor = Color.Beige;
                        }
                        break;
                    case "E24":
                        if ((int)dato.Valor == 1)
                        {
                            this.E24.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.E24.BackColor = Color.Beige;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public override void eventWrapper_OrbitaCambioDato(OEventArgs e)
        {
            try
            {
                base.eventWrapper_OrbitaCambioDato(e);
                OInfoDato info = (OInfoDato)e.Argumento;
                if (info.Dispositivo == this._idDispositivo)
                {
                    this.actualizarES(e);
                }
            }
            catch (System.Exception ex)
            {
                OMensajes.MostrarError(ex);
            }

        }

        #endregion
    }
}
