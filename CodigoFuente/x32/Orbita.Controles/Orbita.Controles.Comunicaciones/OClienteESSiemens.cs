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
                OInfoDato dato = (OInfoDato)e.Argumento;

                switch (dato.Texto)
                {
                    case "E01":
                        if ((int)dato.Valor == 1)
                        {
                            this.EB00.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.EB00.BackColor = Color.Beige;
                        }
                        break;
                    case "E02":
                        if ((int)dato.Valor == 1)
                        {
                            this.EB01.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.EB01.BackColor = Color.Beige;
                        }
                        break;
                    case "E03":
                        if ((int)dato.Valor == 1)
                        {
                            this.EB02.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.EB02.BackColor = Color.Beige;
                        }
                        break;
                    case "E04":
                        if ((int)dato.Valor == 1)
                        {
                            this.EB03.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.EB03.BackColor = Color.Beige;
                        }
                        break;
                    case "E05":
                        if ((int)dato.Valor == 1)
                        {
                            this.EB04.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.EB04.BackColor = Color.Beige;
                        }
                        break;
                    case "E06":
                        if ((int)dato.Valor == 1)
                        {
                            this.EB05.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.EB05.BackColor = Color.Beige;
                        }
                        break;
                    case "E07":
                        if ((int)dato.Valor == 1)
                        {
                            this.EB06.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.EB06.BackColor = Color.Beige;
                        }
                        break;
                    case "E11":
                        if ((int)dato.Valor == 1)
                        {
                            this.EB10.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.EB10.BackColor = Color.Beige;
                        }
                        break;
                    case "E12":
                        if ((int)dato.Valor == 1)
                        {
                            this.EB11.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.EB11.BackColor = Color.Beige;
                        }
                        break;
                    case "E13":
                        if ((int)dato.Valor == 1)
                        {
                            this.EB12.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.EB12.BackColor = Color.Beige;
                        }
                        break;
                    case "E14":
                        if ((int)dato.Valor == 1)
                        {
                            this.EB13.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.EB13.BackColor = Color.Beige;
                        }
                        break;
                    case "E15":
                        if ((int)dato.Valor == 1)
                        {
                            this.EB14.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.EB14.BackColor = Color.Beige;
                        }
                        break;
                    case "E16":
                        if ((int)dato.Valor == 1)
                        {
                            this.EB15.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.EB15.BackColor = Color.Beige;
                        }
                        break;
                    case "E17":
                        if ((int)dato.Valor == 1)
                        {
                            this.EB16.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.EB16.BackColor = Color.Beige;
                        }
                        break;
                    case "S01":
                        if ((int)dato.Valor == 1)
                        {
                            this.SB00.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.SB00.BackColor = Color.Beige;
                        }
                        break;
                    case "S02":
                        if ((int)dato.Valor == 1)
                        {
                            this.SB01.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.SB01.BackColor = Color.Beige;
                        }
                        break;
                    case "S03":
                        if ((int)dato.Valor == 1)
                        {
                            this.SB02.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.SB02.BackColor = Color.Beige;
                        }
                        break;
                    case "S04":
                        if ((int)dato.Valor == 1)
                        {
                            this.SB03.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.SB03.BackColor = Color.Beige;
                        }
                        break;
                    case "S05":
                        if ((int)dato.Valor == 1)
                        {
                            this.SB04.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.SB04.BackColor = Color.Beige;
                        }
                        break;
                    case "S06":
                        if ((int)dato.Valor == 1)
                        {
                            this.SB05.BackColor = Color.Orange;
                        }
                        else
                        {
                            this.SB05.BackColor = Color.Beige;
                        }
                        break;
                    case "E2":
                        this.EB20.Text = dato.Valor.ToString();
                        break;
                    case "E3":
                        this.EB30.Text = dato.Valor.ToString();
                        break;
                }
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
