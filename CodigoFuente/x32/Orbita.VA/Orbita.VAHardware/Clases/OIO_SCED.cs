//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 13-12-2012
// Description      : Adaptado a la versión del servidor de comunicaciones.
//                    Un solo tipo de terminal (de objeto)
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Orbita.Comunicaciones;
using Orbita.Utiles;
using Orbita.VAComun;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Clase que implementa las funciones para el control de Entradas/Salidas del SCED
    /// </summary>
    public class OIO_SCED : OTarjetaIOBase
    {
        #region Atributo(s)
        /// <summary>
        /// Si están o no configurados los terminales
        /// </summary>
        private bool _Valido;
        /// <summary>
        /// Servidor de comunicaciones.
        /// </summary>
        private IOCommRemoting Servidor;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Dirección del servidor SCED
        /// </summary>
        private string _ServidorSced;
        /// <summary>
        /// Dirección del servidor SCED
        /// </summary>
        public string ServidorSced
        {
            get { return this._ServidorSced; }
            set { this._ServidorSced = value; }
        }

        /// <summary>
        /// Número de puerto remoto
        /// </summary>
        private int _PuertoRemoto;
        /// <summary>
        /// Número de puerto remoto
        /// </summary>
        public int PuertoRemoto
        {
            get { return this._PuertoRemoto; }
            set { this._PuertoRemoto = value; }
        }
        #endregion

        #region Contructores
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codHardware">Código del hardware</param>
        public OIO_SCED(string codHardware)
            : base(codHardware)
        {
            // Cargamos valores de la base de datos
            DataTable dtTarjetaIO = AppBD.GetTarjetaIO(codHardware);
            if (dtTarjetaIO.Rows.Count == 1)
            {
                this._ServidorSced = dtTarjetaIO.Rows[0]["SCED_Server"].ToString();
                this._PuertoRemoto = App.EvaluaNumero(dtTarjetaIO.Rows[0]["SCED_Puerto"], 0, int.MaxValue, 1851);
            }

            // Cargamos valores de la base de datos buscamos terminales IO
            using (DataTable dtTerminalesIO = AppBD.GetTerminalesIO(codHardware))
            {
                if (dtTerminalesIO.Rows.Count != 0)
                {
                    this._Valido = true;
                    // Crear los terminales leidas por BBDD  
                    foreach (DataRow dr in dtTerminalesIO.Rows)
                    {
                        OTerminalIOSCED terminal;

                        OEnumTipoDispositivoSCED tipoDispositivo = (OEnumTipoDispositivoSCED)OStringValueAttribute.FindStringValue(typeof(OEnumTipoDispositivoSCED), dr["SCED_TipoDispositivo"].ToString(), OEnumTipoDispositivoSCED.OPC_SimaticNET);
                        int intTipoTerminalIO = App.EvaluaNumero(dr["IdTipoTerminalIO"], 1, 4, 1);

                        OTipoTerminalIO tipoTerminalIO = (OTipoTerminalIO)intTipoTerminalIO;

                        switch (tipoTerminalIO)
                        {
                            case OTipoTerminalIO.EntradaDigital:
                            case OTipoTerminalIO.SalidaDigital:
                            default:
                                terminal = new OTerminalIOSCED(codHardware, dr["CodTerminalIO"].ToString(), tipoDispositivo);
                                break;
                            case OTipoTerminalIO.SalidaComando:
                                terminal = new OTerminalIOSCED_WriteCommand(codHardware, dr["CodTerminalIO"].ToString(), tipoDispositivo);
                                break;
                        }

                        this.ListaTerminales.Add(terminal);
                    }
                }
                else
                {
                    this._Valido = false;
                }
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
                // Base inicializar
                base.Inicializar();

                if ((this.Habilitado) && (this._Valido))
                {
                    //Conectamos con el servicio SCEDServidor
                    this.Conectar();

                    // Se inicializan los terminales
                    foreach (OTerminalIOSCED terminal in this.ListaTerminales)
                    {
                        terminal.Inicializar(this.Servidor);
                    }
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.ES_SCED, this.Servidor.ToString(), exception);
            }

        }
        /// <summary>
        /// Método a heredar donde se conecta y se configura la tarjeta de IO
        /// </summary>
        public override void Finalizar()
        {
            try
            {
                // Base finalizar
                base.Finalizar();

                // Se finalizan los terminales
                foreach (OTerminalIOSCED terminal in this.ListaTerminales)
                {
                    terminal.Finalizar();
                }

                if ((this.Habilitado) && (this._Valido))
                {
                    // Desconectamos del servicio SCEDServidor
                    this.Desconectar();
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.ES_SCED, this.Servidor.ToString(), exception);
            }
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Desconectar del servidor vía Remoting.
        /// </summary>
        private void Desconectar()
        {
            Conectar(false);
            OVALogsManager.Info(OModulosHardware.ES_SCED, this.Servidor.ToString(), "Desconectado del Servicio");
        }
        /// <summary>
        /// Conectar al servidor vía Remoting.
        /// </summary>
        private void Conectar()
        {
            try
            {
                // Establecer la configuración Remoting entre procesos.
                try
                {
                    ORemoting.InicConfiguracionCliente(this.PuertoRemoto, this.ServidorSced);
                }
                catch (Exception)
                {
                    // No hacer nada, canal ya registrado
                }
                this.Servidor = (IOCommRemoting)ORemoting.GetObject(typeof(IOCommRemoting));

                // Eventwrapper de comunicaciones.
                OBroadcastEventWrapper eventWrapper = new OBroadcastEventWrapper();

                // Eventos locales.
                // ...cambio de dato.
                eventWrapper.OrbitaCambioDato += new OManejadorEventoComm(eventWrapper_OrbitaCambioDato);
                // ...comunicaciones.
                eventWrapper.OrbitaComm += new OManejadorEventoComm(eventWrapper_OrbitaComm);

                // Eventos del servidor.
                // ...cambio de dato.
                this.Servidor.OrbitaCambioDato += new OManejadorEventoComm(eventWrapper.OnCambioDato);
                // ...comunicaciones.
                this.Servidor.OrbitaComm += new OManejadorEventoComm(eventWrapper.OnComm);

                // Establecer conexión con el servidor.
                Conectar(true);

                OVALogsManager.Info(OModulosHardware.ES_SCED, this.Servidor.ToString(), "Conectado al servicio");
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.ES_SCED, this.Servidor.ToString(), exception);
            }
        }
        /// <summary>
        /// Conectar al servidor vía Remoting.
        /// </summary>
        /// <param name="estado">Estado de conexión.</param>
        void Conectar(bool estado)
        {
            this.Servidor.OrbitaConectar("canal" + this.PuertoRemoto, estado);
        }
        #endregion

        #region Manejador(es) evento(s)
        /// <summary>
        /// Evento de cambio de dato.
        /// </summary>
        /// <param name="e"></param>
        void eventWrapper_OrbitaCambioDato(OEventArgs e)
        {
            try
            {
                // Recogemos el código del terminal
                string codigoCambioEstado = ((OInfoDato)e.Argumento).Texto;

                // Recogemos el dispositivo al que pertenece el terminal
                int dispositivo = ((OInfoDato)e.Argumento).Dispositivo;

                // Búsqueda del terminal
                OTerminalIOSCED terminal = (this.ListaTerminales.Find(delegate(OTerminalIOBase t) { return (((OTerminalIOSCED)t).CodigoVariableSCED == codigoCambioEstado) && (((OTerminalIOSCED)t).IdDispositivo == dispositivo); }) as OTerminalIOSCED);

                if (terminal != null)
                {
                    // Extraemos el valor
                    terminal.ProcedeEventoCambio = true;                    
                    terminal.LeerEntrada(e);                    
                    terminal.ProcedeEventoCambio = false;
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.ES_SCED, this.Servidor.ToString(), exception);
            }
        }

        /// <summary>
        /// Evento de comunicaciones.
        /// </summary>
        /// <param name="e"></param>
        void eventWrapper_OrbitaComm(OEventArgs e)
        {
            // No implementado
        }
        #endregion
    }

    /// <summary>
    /// Terminal del SCED
    /// </summary>
    internal class OTerminalIOSCED : OTerminalIOBase
    {
        #region Atributo(s)
        /// <summary>
        /// Código de la variable del SCED contra la que trabaja el terminal
        /// </summary>
        internal string CodigoVariableSCED;

        /// <summary>
        /// Servidor de comunicación
        /// </summary>
        protected IOCommRemoting Servidor;

        /// <summary>
        /// Información del Dispositivo
        /// </summary>
        internal int IdDispositivo;

        /// <summary>
        /// Tipo de dispositivo controlado por el SCED
        /// </summary>
        protected OEnumTipoDispositivoSCED TipoDispositivo;

        /// <summary>
        /// Indica que la escritura del valor se ha de producir inmediatamente al cambio del valor de la variable
        /// </summary>
        protected bool EscrituraInmediata;

        /// <summary>
        /// Nos indica si el cambio de valor procede del evento al cambio para en ese caso no realizar escrituras en los delegados
        /// </summary>
        public bool ProcedeEventoCambio;

        /// <summary>
        /// Valor que se envía al SCED
        /// </summary>
        private object Valor;
        #endregion

        #region Constructor
        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public OTerminalIOSCED(string codTarjetaIO, string codTerminalIO, OEnumTipoDispositivoSCED tipoDispositivoSCED)
            : base(codTarjetaIO, codTerminalIO)
        {
            this.Valor = null;

            // Cargamos valores de la base de datos
            DataTable dtTerminalIO = AppBD.GetTerminalIO(codTarjetaIO, codTerminalIO);
            if (dtTerminalIO.Rows.Count == 1)
            {
                this.IdDispositivo = App.EvaluaNumero(dtTerminalIO.Rows[0]["SCED_ID"], 0, 999, 0);
                this.EscrituraInmediata = App.EvaluaBooleano(dtTerminalIO.Rows[0]["SCED_EscrituraInmediata"], true);
                this.CodigoVariableSCED = dtTerminalIO.Rows[0]["SCED_CodVariable"].ToString();
            }
            this.TipoDispositivo = tipoDispositivoSCED;
            this.ProcedeEventoCambio = false;
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Convierte el string devuelto por el servidor de sced al formato correcto de trabajo
        /// </summary>
        /// <param name="valorSCED"></param>
        /// <returns></returns>
        private object SCEDAFormatoCorrecto(object valorSCED)
        {
            object resultado = null;

            switch (this.TipoDato)
            {
                case OEnumTipoDato.Bit:
                case OEnumTipoDato.Entero:
                case OEnumTipoDato.Texto:
                case OEnumTipoDato.Decimal:
                case OEnumTipoDato.Fecha:
                case OEnumTipoDato.Flag:
                    resultado = valorSCED;
                    break;
                case OEnumTipoDato.SinDefinir:
                case OEnumTipoDato.Grafico: // Todavía no implementado
                default:
                    resultado = null;
                    try
                    {
                        byte[] valorByte = (byte[])valorSCED;

                        IFormatter formatter = new BinaryFormatter();
                        MemoryStream stream = new MemoryStream(valorByte);
                        resultado = formatter.Deserialize(stream);
                    }
                    catch
                    {
                        resultado = null;
                    }
                    break;
                case OEnumTipoDato.Imagen:
                    resultado = null;
                    try
                    {
                        byte[] valorByte = (byte[])valorSCED;
                        OBitmapImage bmp = new OBitmapImage();

                        resultado = bmp.FromArray(valorByte);
                    }
                    catch
                    {
                        resultado = null;
                    }
                    break;
            }

            return resultado;
        }

        /// <summary>
        /// Convierte el string devuelto por el servidor de sced al formato correcto de trabajo
        /// </summary>
        /// <param name="valorObj"></param>
        /// <returns></returns>
        private object FormatoCorrectoASCED(object valorObj)
        {
            object resultado = null;

            switch (this.TipoDato)
            {
                case OEnumTipoDato.Bit:
                case OEnumTipoDato.Entero:
                case OEnumTipoDato.Texto:
                case OEnumTipoDato.Decimal:
                case OEnumTipoDato.Fecha:
                    resultado = valorObj;
                    break;
                case OEnumTipoDato.Flag:
                    resultado = null;
                    break;
                case OEnumTipoDato.SinDefinir:
                case OEnumTipoDato.Grafico: // Todavía no implementado
                default:
                    IFormatter formatter = new BinaryFormatter();
                    MemoryStream stream = new MemoryStream();
                    formatter.Serialize(stream, valorObj);
                    stream.Close();
                    resultado = stream.ToArray();
                    break;
                case OEnumTipoDato.Imagen:
                    if (valorObj is OImage)
                    {
                        OImage imagen = (OImage)valorObj;

                        resultado = imagen.ToArray(ImageFormat.Jpeg, 1);
                    }
                    break;
            }

            return resultado;
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Trata el cambio de valor
        /// </summary>
        /// <returns></returns>
        internal void LeerEntrada(OEventArgs eventArgs)
        {
            try
            {
                // Se devuelve el objeto tal cual
                object valorSCED = ((OInfoDato)eventArgs.Argumento).Valor;

                if (!App.CompararObjetos(this.Valor, valorSCED))
                {
                    // Conversión
                    this.Valor = this.SCEDAFormatoCorrecto(valorSCED);

                    // Se lanza desde un thread distino.
                    OSimpleMethod setVariableEnThread = new OSimpleMethod(this.EstablecerValorVariable);
                    setVariableEnThread.BeginInvoke(null, null);
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.ES_SCED, this.Servidor.ToString(), exception);
            }
        }

        /// <summary>
        /// Devuelve el valor listo para ser enviado al SCED
        /// </summary>
        /// <param name="valorObj"></param>
        /// <returns></returns>
        internal object ValorFormateado()
        {
            return this.FormatoCorrectoASCED(this.Valor);
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Método a heredar donde se inicializan los terminales
        /// </summary>
        public void Inicializar(IOCommRemoting servidor)
        {
            try
            {
                this.Inicializar();
                this.Servidor = servidor;

                //Leo de cada terminal su valor y lo actualizo.
                this.LeerEntrada();
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.ES_SCED, this.Servidor.ToString(), exception);
            }
        }

        /// <summary>
        /// Simulación de la escritura de la entrada física
        /// </summary>
        public override void EscribirEntrada(string codigoVariable, object valor)
        {
            try
            {
                base.EscribirEntrada(codigoVariable, valor);

                if (this.EscrituraInmediata)
                {
                    // Conversión
                    this.Valor = this.FormatoCorrectoASCED(valor);

                    // Escritura en el sced
                    this.Servidor.OrbitaEscribir(this.IdDispositivo, new string[1] { this.CodigoVariableSCED }, new object[1] { this.Valor });
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.ES_SCED, this.Servidor.ToString(), exception);
            }
        }

        /// <summary>
        /// Escritura de la salida
        /// </summary>
        public override void EscribirSalida(string codigoVariable, object valor)
        {
            try
            {
                base.EscribirSalida(codigoVariable, valor);

                if (this.EscrituraInmediata)
                {
                    // Conversión
                    this.Valor = this.FormatoCorrectoASCED(valor);

                    // Escritura en el sced
                    this.Servidor.OrbitaEscribir(this.IdDispositivo, new string[1] { this.CodigoVariableSCED }, new object[1] { this.Valor });
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.ES_SCED, this.Servidor.ToString(), exception);
            }
        }
        
        /// <summary>
        /// lee del OPC.Servidor el valor de la variable
        /// </summary>       
        /// <param name="variable">variable en la que se escribe el valor</param>
        public override void LeerEntrada()
        {
            try
            {
                base.LeerEntrada();

                // Lectura del sced
                object[] infoDato = this.Servidor.OrbitaLeer(this.IdDispositivo, new string[1] { this.CodigoVariableSCED }, true);
                if (infoDato.Length == 1)
                {
                    if (infoDato[0] != null)
                    {
                        object valorSCED = infoDato[0];

                        if (!App.CompararObjetos(this.Valor, valorSCED))
                        {
                            // Conversión
                            this.Valor = this.SCEDAFormatoCorrecto(valorSCED);

                            // Se lanza desde un thread distino.
                            OSimpleMethod setVariableEnThread = new OSimpleMethod(this.EstablecerValorVariable);
                            setVariableEnThread.BeginInvoke(null, null);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.ES_SCED, this.Servidor.ToString(), exception);
            }
        }
        #endregion
    }

    /// <summary>
    /// Terminal del SCED para trabajo con texto
    /// </summary>
    internal class OTerminalIOSCED_WriteCommand : OTerminalIOSCED
    {
        #region Atributo(s)
        /// <summary>
        /// Lista de códigos de terminales asociados
        /// </summary>
        private List<string> ListaCodigosTerminalesAsociados;

        /// <summary>
        /// Lista de terminales asociados
        /// </summary>
        private List<OTerminalIOSCED> ListaTerminalesAsociados;
        #endregion

        #region Constructor
        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public OTerminalIOSCED_WriteCommand(string codTarjetaIO, string codTerminalIO, OEnumTipoDispositivoSCED tipoDispositivoSCED)
            : base(codTarjetaIO, codTerminalIO, tipoDispositivoSCED)
        {
            this.ListaCodigosTerminalesAsociados = new List<string>();
            this.ListaTerminalesAsociados = new List<OTerminalIOSCED>();

            // Cargamos valores de la base de datos
            DataTable dtTerminalIO = AppBD.GetTerminalesIO_EscrituraSCED(codTarjetaIO, codTerminalIO);
            if (dtTerminalIO.Rows.Count > 0)
            {
                foreach (DataRow drTerminalIOAsociado in dtTerminalIO.Rows)
                {
                    string codigoTerminalAsociado = drTerminalIOAsociado["CodTerminalIO"].ToString();
                    this.ListaCodigosTerminalesAsociados.Add(codigoTerminalAsociado);
                }
            }
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Convierte el string devuelto por el servidor de sced al formato correcto de trabajo
        /// </summary>
        /// <param name="valorObj"></param>
        /// <returns></returns>
        private string FormatoCorrectoASCED(object valorObj)
        {
            string resultado = string.Empty;

            switch (this.TipoDato)
            {
                case OEnumTipoDato.SinDefinir:
                case OEnumTipoDato.Imagen:
                case OEnumTipoDato.Grafico:
                default:
                    resultado = valorObj.ToString();
                    break;
                case OEnumTipoDato.Bit:
                    resultado = "0";
                    if ((valorObj is bool) && ((bool)valorObj))
                    {
                        resultado = "1";
                    }
                    break;
                case OEnumTipoDato.Entero:
                    resultado = App.EvaluaNumero(valorObj, int.MinValue, int.MaxValue, 0).ToString();
                    break;
                case OEnumTipoDato.Texto:
                    resultado = valorObj.ToString();
                    break;
                case OEnumTipoDato.Decimal:
                    resultado = App.EvaluaNumero(valorObj, double.MinValue, double.MaxValue, 0D).ToString();
                    break;
                case OEnumTipoDato.Fecha:
                    if (valorObj is DateTime)
                    {
                        resultado = valorObj.ToString();
                    }
                    break;
                case OEnumTipoDato.Flag:
                    resultado = string.Empty;
                    break;
            }

            return resultado;
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Inicialización de los terminales
        /// </summary>
        public override void Inicializar()
        {
            base.Inicializar();

            OTarjetaIOBase tarjetaTerminal = OIOManager.ListaTarjetasIO.Find(delegate(OTarjetaIOBase t) { return t.Codigo == this.CodTarjeta; });
            foreach (string codTerminalIOAsociado in this.ListaCodigosTerminalesAsociados)
            {
                OTerminalIOSCED terminalAsociado = (OTerminalIOSCED)tarjetaTerminal.ListaTerminales.Find(delegate(OTerminalIOBase t) { return t.Codigo == codTerminalIOAsociado; });
                this.ListaTerminalesAsociados.Add(terminalAsociado);
            }
        }

        /// <summary>
        /// Escritura de la salida
        /// </summary>
        public override void EscribirSalida(string codigoVariable, object valor)
        {
            try
            {
                base.EscribirSalida(codigoVariable, null); // No guardamos el valor

                if (this.ListaTerminalesAsociados.Count > 0)
                {
                    // Inicializamos las variables de envio
                    List<string> variables = new List<string>();
                    List<object> valores = new List<object>();

                    foreach (OTerminalIOSCED terminalAsociado in this.ListaTerminalesAsociados)
                    {
                        variables.Add(terminalAsociado.CodigoVariableSCED);
                        valores.Add(terminalAsociado.ValorFormateado());
                    }

                    // Escritura en el sced de las variables asocidas a este terminal tipo Comando salida
                    bool resultado = this.Servidor.OrbitaEscribir(this.IdDispositivo, variables.ToArray(), valores.ToArray());

                    // Conversión a string
                    string valorSCED = this.FormatoCorrectoASCED(valor);
                    
                    // TODO : REVISAR :
                    //Thread.Sleep(2000);

                    // Escritura en el sced de la variable tipo Comando Salida, para que sea escrita en ultimo lugar
                    resultado = this.Servidor.OrbitaEscribir(this.IdDispositivo, new string[1] { this.CodigoVariableSCED }, new object[1] { valorSCED });
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.ES_SCED, this.Servidor.ToString(), exception);
            }
        }
        #endregion
    }

    /// <summary>
    /// Tipo de dispositivo controlado por el SCED
    /// </summary>
    internal enum OEnumTipoDispositivoSCED
    {
        /// <summary>
        /// Servidor OPC de Siemens
        /// </summary>
        [OStringValue("OPC.SimaticNET")]
        OPC_SimaticNET = 1,

        /// <summary>
        /// Tarjeta Ethernet E/S
        /// </summary>
        [OStringValue("MCC.EPDISO16")]
        MCC_EPDISO16 = 2,

        /// <summary>
        /// Dispositivo para variables internas
        /// </summary>
        [OStringValue("Orbita")]
        Orbita = 3
    }
}

