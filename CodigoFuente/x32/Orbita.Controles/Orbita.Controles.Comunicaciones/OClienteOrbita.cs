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
    public partial class OClienteOrbita : UserControl
    {
        #region Delegados
        /// <summary>
        /// Delegado para mostar los datos en el formulario
        /// </summary>
        /// <param name="Elemento"></param>
        internal delegate void Delegado(string Elemento);
        /// <summary>
        /// Delegado para el cambio de estado
        /// </summary>
        /// <param name="Elemento"></param>
        internal delegate void DelegadoCambioEstado(OEstadoComms estado);
        #endregion

        #region Variables
        /// <summary>
        /// Puerto de comunicación de remoting
        /// </summary>
        private int _remotingPuerto = 1852;
        /// <summary>
        /// Servidor de comunicaciones.
        /// </summary>
        IOCommRemoting _servidor;
        /// <summary>
        /// Identificador de dispositivo
        /// </summary>
        int _idDispositivo = 3;
        /// <summary>
        /// Servidor remoting
        /// </summary>
        string _servidorRemoting = "localhost";
        #endregion

        public OClienteOrbita()
        {
            InitializeComponent();
        }

        #region Métodos
        /// <summary>
        /// Arranca las comunicaciones con el dispositivo
        /// </summary>
        private void Iniciar()
        {
            try
            {
                this._remotingPuerto = Convert.ToInt32(this.txtPuertoRemoting.Text);
                this._idDispositivo = Convert.ToInt32(this.txtIdDispositivo.Text);
                this._servidorRemoting = this.txtServidorRemoting.Text;
            }
            catch (Exception)
            {
                MessageBox.Show("Error al convertir los valores de configuración.");
            }

            try
            {
                // Establecer la configuración Remoting entre procesos.
                ORemoting.InicConfiguracionCliente(this._remotingPuerto, this._servidorRemoting);
                this._servidor = (Orbita.Comunicaciones.IOCommRemoting)ORemoting.GetObject(typeof(Orbita.Comunicaciones.IOCommRemoting));

                // Eventwrapper de comunicaciones.
                Orbita.Comunicaciones.OBroadcastEventWrapper eventWrapper = new Orbita.Comunicaciones.OBroadcastEventWrapper();

                //Eventos locales.
                //...cambio de dato.
                eventWrapper.OrbitaCambioDato += new OManejadorEventoComm(eventWrapper_OrbitaCambioDato);
                // ...alarma
                eventWrapper.OrbitaAlarma += new OManejadorEventoComm(eventWrapper_OrbitaAlarma);
                // ...comunicaciones.
                eventWrapper.OrbitaComm += new OManejadorEventoComm(eventWrapper_OrbitaComm);

                // Eventos del servidor.
                // ...cambio de dato.
                this._servidor.OrbitaCambioDato += new OManejadorEventoComm(eventWrapper.OnCambioDato);
                // ...alarma.
                this._servidor.OrbitaAlarma += new OManejadorEventoComm(eventWrapper.OnAlarma);
                // ...comunicaciones.
                this._servidor.OrbitaComm += new OManejadorEventoComm(eventWrapper.OnComm);

                // Establecer conexión con el servidor.
                Conectar(true);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Conectar al servidor vía Remoting.
        /// </summary>
        /// <param name="estado">Estado de conexión.</param>
        void Conectar(bool estado)
        {
            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();

            string canal = "canal" + strHostName + ":" + this._remotingPuerto.ToString();
            this._servidor.OrbitaConectar(canal, estado);
        }
        /// <summary>
        /// Desconectar del servidor vía Remoting.
        /// </summary>
        public void Desconectar()
        {
            Conectar(false);
        }
        /// <summary>
        /// Procesa la información del evento de comunicaciones
        /// </summary>
        /// <param name="e"></param>
        private void procesarComunicacionesServidor(OEventArgs e)
        {
            try
            {
                OEstadoComms estado = (OEstadoComms)e.Argumento;
                DelegadoCambioEstado MyDelegado = new DelegadoCambioEstado(cambiarEstado);
                this.Invoke(MyDelegado, new object[] { estado });

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        /// <summary>
        /// Procesa la información para el cambio de estado por pantalla
        /// </summary>
        /// <param name="estado"></param>
        private void cambiarEstado(OEstadoComms estado)
        {
            if (estado.Estado == "OK")
            {
                if (estado.Id == this._idDispositivo)
                {
                    this.txtOrbita.BackColor = System.Drawing.Color.Green;
                }
            }
            else
            {
                if (estado.Id == this._idDispositivo)
                {
                    this.txtOrbita.BackColor = System.Drawing.Color.Red;
                }
            }
        }
        /// <summary>
        /// Agrega los items a la lista
        /// </summary>
        /// <param name="texto"></param>
        private void agregarItemOrbita(string texto)
        {
            if (this.listViewOrbita.InvokeRequired)
            {
                Delegado MyDelegado = new Delegado(agregarItemOrbita);
                this.Invoke(MyDelegado, new object[] { texto });
            }
            else
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = texto;
                //lvi.ImageIndex = 0;
                lvi.Tag = texto;
                this.listViewOrbita.Items.Add(lvi);
            }
        }        
        #endregion

        #region Eventos

        /// <summary>
        /// Evento de cambio de dato.
        /// </summary>
        /// <param name="e"></param>
        void eventWrapper_OrbitaCambioDato(OEventArgs e)
        {
            try
            {
                OInfoDato info = (OInfoDato)e.Argumento;
                string texto = "Cambio de dato de la variable " + info.Texto.ToString() + " a " + info.Valor.ToString();

                if (info.Dispositivo == this._idDispositivo)
                {
                    this.agregarItemOrbita(texto);

                }
            }
            catch (System.Exception ex)
            {
            }

        }
        /// <summary>
        /// Evento de alarma.
        /// </summary>
        /// <param name="e"></param>
        void eventWrapper_OrbitaAlarma(OEventArgs e)
        {
            try
            {
                OInfoDato info = (OInfoDato)e.Argumento;
                string texto = "Alarma " + info.Texto.ToString() + " pasa a valer " + info.Valor.ToString();

                if (info.Dispositivo == this._idDispositivo)
                {
                    this.agregarItemOrbita(texto);

                }
            }
            catch (System.Exception ex)
            {

            }

        }
        /// <summary>
        /// Evento de comunicaciones.
        /// </summary>
        /// <param name="e"></param>
        void eventWrapper_OrbitaComm(OEventArgs e)
        {
            try
            {
                this.procesarComunicacionesServidor(e);
            }
            catch (System.Exception ex)
            {

            }

        }
        /// <summary>
        /// Inicia el cliente de comunicaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConectar_Click(object sender, EventArgs e)
        {
            this.Iniciar();
        }
        /// <summary>
        /// Lee una variable por OPC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLecturaOPC_Click(object sender, EventArgs e)
        {
            try
            {

                string[] lectura = new string[1];
                lectura[0] = this.txtVarLeerOrbita.Text;
                object[] valor = this._servidor.OrbitaLeer(this._idDispositivo, lectura, true);

                string svalor = (string)valor[0];
                this.txtVaLeerOrbita.Text = svalor;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        /// <summary>
        /// Escribe una variable por OPC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEscrituraOPC_Click(object sender, EventArgs e)
        {
            try
            {
                string[] variable = new string[1];
                variable[0] = this.txtVarEscribirOrbita.Text;
                object[] valor = new object[1];
                valor[0] = this.txtValEscribirOrbita.Text;
                //valor[0] = dt;
                bool resp = this._servidor.OrbitaEscribir(this._idDispositivo, variable, valor);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        /// <summary>
        /// Lee el valor de todas las variables del dispositivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLeerVariablesOPC_Click(object sender, EventArgs e)
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

            DataTable dt = new DataTable();

            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Variable";
            dt.Columns.Add(column);

            DataColumn column2 = new DataColumn();
            column2.DataType = System.Type.GetType("System.String");
            column2.ColumnName = "Valor";
            dt.Columns.Add(column2);

            for (int j = 0; j < variables.Length; j++)
            {
                DataRow dr = dt.NewRow();
                dr["Variable"] = variables[j];
                try
                {
                    dr["Valor"] = resultado[j];
                }
                catch (Exception)
                {
                    dr["Valor"] = "";
                }

                dt.Rows.Add(dr);
            }

            this.dataGridViewOrbita.DataSource = dt;
        }
        /// <summary>
        /// Lee las alarmas activas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLeerAlarmasOPC_Click(object sender, EventArgs e)
        {

            ArrayList alarmas = this._servidor.OrbitaGetAlarmasActivas(this._idDispositivo);
            string[] variables = new string[alarmas.Count];
            for (int i = 0; i < alarmas.Count; i++)
            {
                variables[i] = alarmas[i].ToString();
            }



            object[] resultado = this._servidor.OrbitaLeer(this._idDispositivo, variables, true);

            DataTable dt = new DataTable();

            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Variable";
            dt.Columns.Add(column);

            DataColumn column2 = new DataColumn();
            column2.DataType = System.Type.GetType("System.String");
            column2.ColumnName = "Valor";
            dt.Columns.Add(column2);

            for (int j = 0; j < variables.Length; j++)
            {
                DataRow dr = dt.NewRow();
                dr["Variable"] = variables[j];
                try
                {
                    dr["Valor"] = resultado[j];
                }
                catch (Exception)
                {
                    dr["Valor"] = "";
                }

                dt.Rows.Add(dr);
            }

            this.dataGridViewOrbita.DataSource = dt;
        }

        #endregion   
    }
}
