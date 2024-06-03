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
    public partial class OClienteESSiemens : OClienteComs
    {
        #region Constructores

        public OClienteESSiemens()
        {
            InitializeComponent();
        }

        #endregion
        

        #region Métodos
        /// <summary>
        /// Inicia la comunicación con el servidor
        /// </summary>
        public override void Iniciar()
        {
            base.Iniciar();
            try
            {
                this.InciarVisualizacion();
            }
            catch (Exception ex)
            {
                OMensajes.MostrarError("Error al iniciar la visualización. ", ex);
            }
        }
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
                //OInfoDato dato = (OInfoDato)e.Argumento;

                //switch (dato.Texto)
                //{
                //    case "E01":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.EB00.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.EB00.BackColor = Color.Beige;
                //        }
                //        break;
                //    case "E02":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.A2.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.A2.BackColor = Color.Beige;
                //        }
                //        break;
                //    case "E03":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.A3.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.A3.BackColor = Color.Beige;
                //        }
                //        break;
                //    case "E04":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.A4.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.A4.BackColor = Color.Beige;
                //        }
                //        break;
                //    case "E05":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.A5.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.A5.BackColor = Color.Beige;
                //        }
                //        break;
                //    case "E06":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.A6.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.A6.BackColor = Color.Beige;
                //        }
                //        break;
                //    case "A7":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.A7.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.A7.BackColor = Color.Beige;
                //        }
                //        break;
                //    case "A8":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.A8.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.A8.BackColor = Color.Beige;
                //        }
                //        break;
                //    case "B1":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.B1.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.B1.BackColor = Color.Beige;
                //        }
                //        break;
                //    case "B2":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.B2.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.B2.BackColor = Color.Beige;
                //        }
                //        break;
                //    case "B3":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.B3.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.B3.BackColor = Color.Beige;
                //        }
                //        break;
                //    case "B4":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.B4.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.B4.BackColor = Color.Beige;
                //        }
                //        break;
                //    case "B5":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.B5.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.B5.BackColor = Color.Beige;
                //        }
                //        break;
                //    case "B6":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.B6.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.B6.BackColor = Color.Beige;
                //        }
                //        break;
                //    case "B7":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.B7.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.B7.BackColor = Color.Beige;
                //        }
                //        break;
                //    case "B8":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.B8.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.B8.BackColor = Color.Beige;
                //        }
                //        break;
                //    case "CL1":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.CL1.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.CL1.BackColor = Color.Beige;
                //        }
                //        break;
                //    case "CL2":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.CL2.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.CL2.BackColor = Color.Beige;
                //        }
                //        break;
                //    case "CL3":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.CL3.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.CL3.BackColor = Color.Beige;
                //        }
                //        break;
                //    case "CL4":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.CL4.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.CL4.BackColor = Color.Beige;
                //        }
                //        break;
                //    case "CH1":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.CH1.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.CH1.BackColor = Color.Beige;
                //        }
                //        break;
                //    case "CH2":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.CH2.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.CH2.BackColor = Color.Beige;
                //        }
                //        break;
                //    case "CH3":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.CH3.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.CH3.BackColor = Color.Beige;
                //        }
                //        break;
                //    case "CH4":
                //        if ((int)dato.Valor == 1)
                //        {
                //            this.CH4.BackColor = Color.Orange;
                //        }
                //        else
                //        {
                //            this.CH4.BackColor = Color.Beige;
                //        }
                //        break;
                //}
            }
        }
        /// <summary>
        /// Actualiza la visualización al arrancar la aplicación
        /// </summary>
        private void InciarVisualizacion()
        {
            string[] variables = new string[this._servidor.OrbitaGetDatos(this._idDispositivo).Count];
            int i = 0;
            foreach (DictionaryEntry item in this._servidor.OrbitaGetDatos(this._idDispositivo))
            {
                OInfoDato dato = (OInfoDato)item.Value;

                variables[i] = dato.Texto;
                i++;
            }

            object[] resultado = this._servidor.OrbitaLeer(this._idDispositivo, variables, true);

            for (int j = 0; j < resultado.Length; j++)
            {
                OEventArgs e = new OEventArgs();
                OInfoDato dato = new OInfoDato();
                dato.Texto = variables[j];
                dato.Valor = resultado[j];
                e.Argumento = dato;
                this.actualizarES(e);
            }
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Actualizamos los valores del control
        /// </summary>
        /// <param name="e"></param>
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
