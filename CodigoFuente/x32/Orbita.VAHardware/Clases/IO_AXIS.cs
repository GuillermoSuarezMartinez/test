//***********************************************************************
// Assembly         : Orbita.VAHardware
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
using System.Data;
using System.Net;
using System.Windows.Forms;
using Orbita.VAComun;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Clase que implementa las funciones para el control del módulo de Entradas/Salidas integrado en la cámara AXIS 223M
    /// </summary>
    class IO_AXIS : TarjetaIOBase
    {
        #region Atributo(s)
        /// <summary>
        /// Timer de escaneo de las entradas
        /// </summary>
        private Timer timerScan;
        /// <summary>
        /// IP de la tarjeta
        /// </summary>
        private IPAddress IP;
        /// <summary>
        /// Input 1
        /// </summary>
        private TerminalIOAxisBit Input1;
        /// <summary>
        /// Input 1
        /// </summary>
        private TerminalIOAxisBit Input2;
        /// <summary>
        /// Output 1
        /// </summary>
        private TerminalIOAxisBit Output1;
        /// <summary>
        /// Si estan configurados los terminales
        /// </summary>
        private bool _Valido;
        #endregion

        #region Contructores
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codHardware">Código del hardware</param>
        public IO_AXIS(string codHardware)
            : base(codHardware)
        {
            // Creamos los campos
            this.timerScan = new Timer();
            this.timerScan.Interval = 1;
            this.timerScan.Enabled = false;
            this.timerScan.Tick += new EventHandler(EventoScan);

            // Cargamos valores de la base de datos
            DataTable dtTarjetaIO = AppBD.GetTarjetaIO(this.Codigo);
            if (dtTarjetaIO.Rows.Count == 1)
            {
                this.IP = IPAddress.Parse(dtTarjetaIO.Rows[0]["AXIS223M_IP"].ToString());
                // Rellenamos los terminales
                this.Input1 = new TerminalIOAxisBit(this.Codigo, "Input1", this.IP);
                this.Input2 = new TerminalIOAxisBit(this.Codigo, "Input2", this.IP);
                this.Output1 = new TerminalIOAxisBit(this.Codigo, "Output1", this.IP);
                this._Valido = true;
            }
            else
            {
                this._Valido = false;
            }
        }
        #endregion

        #region Método(s) heredado(s)

        /// <summary>
        /// Método a heredar donde se conecta y se configura la tarjeta de IO
        /// </summary>
        public override void Inicializar()
        {
            try
            {
                base.Inicializar();

                // Se inicializan los terminales de salida
                this.Output1.Inicializar();

                // Ponemos en marcha el timer de escaneo
                this.timerScan.Enabled = true;
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.ES_Axis, this.Codigo, exception);
            }
        }
        /// <summary>
        /// Método a heredar donde se conecta y se configura la tarjeta de IO
        /// </summary>
        public override void Finalizar()
        {
            try
            {
                base.Finalizar();
                // Ponemos en marcha el timer de escaneo
                this.timerScan.Enabled = false;

            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.ES_Axis, this.Codigo, exception);
            }
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Evento del timer de ejecución
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void EventoScan(object sender, EventArgs e)
        {
            this.timerScan.Stop();
            try
            {
                // Lectura de las entradas
                if (this._Valido)
                {
                    this.Input1.LeerEntrada();
                    this.Input2.LeerEntrada();
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.ES_Axis, this.Codigo, exception);
            }
            this.timerScan.Start();
        }
        #endregion
    }

}
