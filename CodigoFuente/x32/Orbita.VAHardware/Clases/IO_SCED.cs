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
    public class IO_SCED : TarjetaIOBase
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
        public IO_SCED(string codHardware)
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
                        TerminalIOSCED terminal;

                        EnumTipoDispositivoSCED tipoDispositivo = (EnumTipoDispositivoSCED)StringValueAttribute.FindStringValue(typeof(EnumTipoDispositivoSCED), dr["SCED_TipoDispositivo"].ToString(), EnumTipoDispositivoSCED.OPC_SimaticNET);
                        int intTipoTerminalIO = App.EvaluaNumero(dr["IdTipoTerminalIO"], 1, 4, 1);

                        TipoTerminalIO tipoTerminalIO = (TipoTerminalIO)intTipoTerminalIO;

                        switch (tipoTerminalIO)
                        {
                            case TipoTerminalIO.EntradaDigital:
                            case TipoTerminalIO.SalidaDigital:
                            default:
                                switch (tipoDispositivo)
                                {
                                    case EnumTipoDispositivoSCED.OPC_SimaticNET:
                                    case EnumTipoDispositivoSCED.MCC_EPDISO16:
                                    default:
                                        terminal = new TerminalIOSCED_String(codHardware, dr["CodTerminalIO"].ToString(), tipoDispositivo);
                                        break;
                                    case EnumTipoDispositivoSCED.Orbita:
                                        terminal = new TerminalIOSCED_Object(codHardware, dr["CodTerminalIO"].ToString(), tipoDispositivo);
                                        break;
                                }
                                break;
                            case TipoTerminalIO.SalidaComando:
                                terminal = new TerminalIOSCED_WriteCommand(codHardware, dr["CodTerminalIO"].ToString(), tipoDispositivo);
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
                    foreach (TerminalIOSCED terminal in this.ListaTerminales)
                    {
                        terminal.Inicializar(this.Servidor);
                    }
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.ES_SCED, this.Servidor.ToString(), exception);
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
                foreach (TerminalIOSCED terminal in this.ListaTerminales)
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
                LogsRuntime.Error(ModulosHardware.ES_SCED, this.Servidor.ToString(), exception);
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
            LogsRuntime.Info(ModulosHardware.ES_SCED, this.Servidor.ToString(), "Desconectado del Servicio");
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
                eventWrapper.OrbitaCambioDato += new ManejadorEventoComm(eventWrapper_OrbitaCambioDato);
                // ...alarma
                eventWrapper.OrbitaAlarma += new ManejadorEventoComm(eventWrapper_OrbitaAlarma);
                // ...comunicaciones.
                eventWrapper.OrbitaComm += new ManejadorEventoComm(eventWrapper_OrbitaComm);

                // Eventos del servidor.
                // ...cambio de dato.
                this.Servidor.OrbitaCambioDato += new ManejadorEventoComm(eventWrapper.OnCambioDato);
                // ...alarma.
                this.Servidor.OrbitaAlarma += new ManejadorEventoComm(eventWrapper.OnAlarma);
                // ...comunicaciones.
                this.Servidor.OrbitaComm += new ManejadorEventoComm(eventWrapper.OnComm);

                // Establecer conexión con el servidor.
                Conectar(true);

                LogsRuntime.Info(ModulosHardware.ES_SCED, this.Servidor.ToString(), "Conectado al servicio");
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.ES_SCED, this.Servidor.ToString(), exception);
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

        #region Manejadores eventos
        /// <summary>
        /// Evento de cambio de dato.
        /// </summary>
        /// <param name="e"></param>
        void eventWrapper_OrbitaCambioDato(OEventArgs e)
        {
            try
            {
                // Recogemos el código del terminal
                string codigoCambioEstado = ((InfoOPCdato)e.Argumento).Texto;

                // Recogemos el dispositivo al que pertenece el terminal
                int dispositivo = ((InfoOPCdato)e.Argumento).Dispositivo;

                // Búsqueda del terminal
                TerminalIOSCED terminal = (this.ListaTerminales.Find(delegate(TerminalIOBase t) { return (((TerminalIOSCED)t).CodigoVariableSCED == codigoCambioEstado) && (((TerminalIOSCED)t).IdDispositivo == dispositivo); }) as TerminalIOSCED);

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
                LogsRuntime.Error(ModulosHardware.ES_SCED, this.Servidor.ToString(), exception);
            }
        }

        /// <summary>
        /// Evento de alarma.
        /// </summary>
        /// <param name="e"></param>
        void eventWrapper_OrbitaAlarma(OEventArgs e)
        {
            // No implementado
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
    internal class TerminalIOSCED : TerminalIOBase
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
        protected EnumTipoDispositivoSCED TipoDispositivo;

        /// <summary>
        /// Indica que la escritura del valor se ha de producir inmediatamente al cambio del valor de la variable
        /// </summary>
        protected bool EscrituraInmediata;

        /// <summary>
        /// Nos indica si el cambio de valor procede del evento al cambio para en ese caso no realizar escrituras en los delegados
        /// </summary>
        public bool ProcedeEventoCambio;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Valor del terminal
        /// </summary>
        public new object Valor
        {
            get
            {
                return base.Valor;
            }
            set
            {
                base.Valor = value;
            }
        }        
        #endregion

        #region Constructor
        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public TerminalIOSCED(string codTarjetaIO, string codTerminalIO, EnumTipoDispositivoSCED tipoDispositivoSCED)
            : base(codTarjetaIO, codTerminalIO)
        {
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
                LogsRuntime.Error(ModulosHardware.ES_SCED, this.Servidor.ToString(), exception);
            }
        }

        /// <summary>
        /// Simulación de la escritura de la entrada física
        /// </summary>
        public override void EscribirEntrada(string codigoVariable, object valor)
        {
            this.Valor = valor;
            // Implementado en hijos
        }

        /// <summary>
        /// Escritura de la salida
        /// </summary>
        public override void EscribirSalida(string codigoVariable, object valor)
        {
            this.Valor = valor;
            // Implementado en hijos
        }
        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Trata el cambio de valor
        /// </summary>
        /// <returns></returns>
        internal virtual void LeerEntrada(OEventArgs eventArgs)
        {
            base.LeerEntrada();
            // Implementado en hijos
        }

        /// <summary>
        /// Devuelve el valor listo para ser enviado al SCED
        /// </summary>
        /// <param name="valorObj"></param>
        /// <returns></returns>
        internal virtual object ValorFormateado()
        {
            return null;
        }
        #endregion
    }

    /// <summary>
    /// Terminal del SCED para trabajo con texto
    /// </summary>
    internal class TerminalIOSCED_String : TerminalIOSCED
    {
        #region Constructor
        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public TerminalIOSCED_String(string codTarjetaIO, string codTerminalIO, EnumTipoDispositivoSCED tipoDispositivoSCED)
            : base(codTarjetaIO, codTerminalIO, tipoDispositivoSCED)
        {
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Convierte el string devuelto por el servidor de sced al formato correcto de trabajo
        /// </summary>
        /// <param name="valorSCED"></param>
        /// <returns></returns>
        private object SCEDAFormatoCorrecto(string valorSCED)
        {
            object resultado = null;

            switch (this.TipoDato)
            {
                case EnumTipoDato.SinDefinir:
                case EnumTipoDato.Imagen:
                case EnumTipoDato.Grafico:
                default:
                    resultado = valorSCED;
                    break;
                case EnumTipoDato.Bit:
                    resultado = false;
                    if (valorSCED == "1" || valorSCED.ToUpper() == "TRUE")
                    {
                        resultado = true;
                    }
                    break;
                case EnumTipoDato.Entero:
                    resultado = 0;
                    int valorInt;
                    if (int.TryParse(valorSCED, out valorInt))
                    {
                        resultado = valorInt;
                    }
                    break;
                case EnumTipoDato.Texto:
                    resultado = valorSCED;
                    break;
                case EnumTipoDato.Decimal:
                    resultado = 0D;
                    double valorFloat;
                    if (double.TryParse(valorSCED, out valorFloat))
                    {
                        resultado = valorFloat;
                    }
                    break;
                case EnumTipoDato.Fecha:
                    resultado = DateTime.MinValue;
                    DateTime valorDateTime;
                    if (DateTime.TryParse(valorSCED, out valorDateTime))
                    {
                        resultado = valorDateTime;
                    }
                    break;
                case EnumTipoDato.Flag:
                    resultado = null;
                    break;
            }

            return resultado;
        }

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
                case EnumTipoDato.SinDefinir:
                case EnumTipoDato.Imagen:
                case EnumTipoDato.Grafico:
                default:
                    resultado = valorObj.ToString();
                    break;
                case EnumTipoDato.Bit:
                    resultado = "0";
                    if ((valorObj is bool) && ((bool)valorObj))
                    {
                        resultado = "1";
                    }
                    if ((valorObj is string) && (((string)valorObj).ToUpper() == "TRUE"))
                    {
                        resultado = "1";
                    }
                    break;
                case EnumTipoDato.Entero:
                    resultado = App.EvaluaNumero(valorObj, int.MinValue, int.MaxValue, 0).ToString();
                    break;
                case EnumTipoDato.Texto:
                    resultado = valorObj.ToString();
                    break;
                case EnumTipoDato.Decimal:
                    resultado = App.EvaluaNumero(valorObj, double.MinValue, double.MaxValue, 0D).ToString();
                    break;
                case EnumTipoDato.Fecha:
                    if (valorObj is DateTime)
                    {
                        resultado = valorObj.ToString();
                    }
                    break;
                case EnumTipoDato.Flag:
                    resultado = string.Empty;
                    break;
            }

            return resultado;
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Devuelve el valor listo para ser enviado al SCED
        /// </summary>
        /// <param name="valorObj"></param>
        /// <returns></returns>
        internal override object ValorFormateado()
        {
            return this.FormatoCorrectoASCED(this.Valor);
        }

        /// <summary>
        /// Simulación de la escritura de la entrada física
        /// </summary>
        public override void EscribirEntrada(string codigoVariable, object valor)
        {
            return; //TODO: No válido para terminales de tipo string . Haría falta definir un nuevo tipo de terminal

            try
            {
                base.EscribirEntrada(codigoVariable, valor);

                if (this.EscrituraInmediata)
                {
                    // Conversión a string
                    string valorSCED = this.FormatoCorrectoASCED(valor);

                    // Escritura en el sced
                    this.Servidor.OrbitaEscribir(this.IdDispositivo, new string[1] { this.CodigoVariableSCED }, new object[1] { valorSCED });
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.ES_SCED, this.Servidor.ToString(), exception);
            }
        }

        /// <summary>
        /// Escritura de la salida
        /// </summary>
        public override void EscribirSalida(string codigoVariable, object valor)
        {
            // Conversión a string
            string valorSCED = this.FormatoCorrectoASCED(valor);
            try
            {
                base.EscribirSalida(codigoVariable, valor);

                if (this.EscrituraInmediata)
                {
                    if (((TerminalIOBase)this).TipoTerminalIO == Orbita.VAHardware.TipoTerminalIO.SalidaDigital)
                    {
                        this.Servidor.OrbitaEscribir(this.IdDispositivo, new string[1] { this.CodigoVariableSCED }, new object[1] { valorSCED });
                        LogsRuntime.Debug(ModulosHardware.ES_SCED, this.Servidor.ToString() + "- Escritura variable:" + this.CodigoVariableSCED + "-" + valorSCED.ToString(), "");
                    }
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.ES_SCED, this.Servidor.ToString() + "- Fallo escritura variable:" + this.CodigoVariableSCED + "-" + valorSCED.ToString(), exception);
            }
        }

        /// <summary>
        /// lee del OPC.Servidor el valor de la variable
        /// </summary>       
        /// <param name="Variable">variable en la que se escribe el valor</param>
        public override void LeerEntrada()
        {
            try
            {
                if (this.TipoTerminalIO == Orbita.VAHardware.TipoTerminalIO.EntradaDigital) // TODO: Sólo se lee si se trata de entrada
                {
                    base.LeerEntrada();

                    // Lectura del sced
                    string[] resultadoSCED = this.Servidor.OrbitaLeer(this.IdDispositivo, new string[1] { this.CodigoVariableSCED }, true);
                    if ((resultadoSCED != null) && (resultadoSCED.Length == 1))
                    {
                        object valor = this.SCEDAFormatoCorrecto(resultadoSCED[0]);

                        //if (!valor.Equals(this.Valor))
                        if (!App.CompararObjetos(this.Valor, valor))
                        {
                            this.Valor = valor;

                            // Se lanza desde un thread distino.
                            SimpleMethod setVariableEnThread = new SimpleMethod(this.EstablecerValorVariable);
                            setVariableEnThread.BeginInvoke(null, null);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.ES_SCED, this.Servidor.ToString(), exception);
            }
        }

        /// <summary>
        /// Trata el cambio de valor
        /// </summary>
        /// <returns></returns>
        internal override void LeerEntrada(OEventArgs eventArgs)
        {
            try
            {
                base.LeerEntrada(eventArgs);

                string strCambioEstado = ((InfoOPCdato)eventArgs.Argumento).Valor;
                object valor = this.SCEDAFormatoCorrecto(strCambioEstado);

                //if (!valor.Equals(this.Valor))
                if (!App.CompararObjetos(this.Valor, valor))
                {
                    this.Valor = valor;

                    if (!this.ProcedeEventoCambio)
                    {
                        // Se lanza desde un thread distino.
                        SimpleMethod setVariableEnThread = new SimpleMethod(this.EstablecerValorVariable);
                        setVariableEnThread.BeginInvoke(null, null);
                    }
                    else
                    {
                        if ((((TerminalIOBase)this).TipoTerminalIO == TipoTerminalIO.EntradaDigital) || (((TerminalIOBase)this).TipoTerminalIO == TipoTerminalIO.EntradaComando))
                        {
                            // Se lanza desde un thread distino.
                            SimpleMethod setVariableEnThread = new SimpleMethod(this.EstablecerValorVariable);
                            setVariableEnThread.BeginInvoke(null, null);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.ES_SCED, this.Servidor.ToString(), exception);
            }
        }
        #endregion
    }

    /// <summary>
    /// Terminal del SCED para el trabajo con objetos
    /// </summary>
    internal class TerminalIOSCED_Object : TerminalIOSCED
    {
        #region Atributo(s)
        /// <summary>
        /// Valor que se envía al SCED
        /// </summary>
        private object ValorSCED;
        #endregion

        #region Constructor
        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public TerminalIOSCED_Object(string codTarjetaIO, string codTerminalIO, EnumTipoDispositivoSCED tipoDispositivoSCED)
            : base(codTarjetaIO, codTerminalIO, tipoDispositivoSCED)
        {
            this.ValorSCED = null;
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
                case EnumTipoDato.Bit:
                case EnumTipoDato.Entero:
                case EnumTipoDato.Texto:
                case EnumTipoDato.Decimal:
                case EnumTipoDato.Fecha:
                case EnumTipoDato.Flag:
                    resultado = valorSCED;
                    break;
                case EnumTipoDato.SinDefinir:
                case EnumTipoDato.Grafico: // Todavía no implementado
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
                case EnumTipoDato.Imagen:
                    resultado = null;
                    try
                    {
                        byte[] valorByte = (byte[])valorSCED;
                        BitmapImage bmp = new BitmapImage();

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
                case EnumTipoDato.Bit:
                case EnumTipoDato.Entero:
                case EnumTipoDato.Texto:
                case EnumTipoDato.Decimal:
                case EnumTipoDato.Fecha:
                    resultado = valorObj;
                    break;
                case EnumTipoDato.Flag:
                    resultado = null;
                    break;
                case EnumTipoDato.SinDefinir:
                case EnumTipoDato.Grafico: // Todavía no implementado
                default:
                    IFormatter formatter = new BinaryFormatter();
                    MemoryStream stream = new MemoryStream();
                    formatter.Serialize(stream, valorObj);
                    stream.Close();
                    resultado = stream.ToArray();
                    break;
                case EnumTipoDato.Imagen:
                    if (valorObj is OrbitaImage)
                    {
                        OrbitaImage imagen = (OrbitaImage)valorObj;

                        resultado = imagen.ToArray(ImageFormat.Jpeg, 1);
                    }
                    break;
            }

            return resultado;
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Devuelve el valor listo para ser enviado al SCED
        /// </summary>
        /// <param name="valorObj"></param>
        /// <returns></returns>
        internal override object ValorFormateado()
        {
            return this.FormatoCorrectoASCED(this.Valor);
        }

        /// <summary>
        /// Escritura de la salida
        /// </summary>
        public override void EscribirEntrada(string codigoVariable, object valor)
        {
            try
            {
                base.EscribirEntrada(codigoVariable, valor);

                if (this.EscrituraInmediata)
                {
                    // Conversión
                    this.ValorSCED = this.FormatoCorrectoASCED(valor);

                    // Escritura en el sced
                    this.Servidor.OrbitaEscribir(this.IdDispositivo, new string[1] { this.CodigoVariableSCED }, new object[1] { this.ValorSCED });
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.ES_SCED, this.Servidor.ToString(), exception);
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
                    this.ValorSCED = this.FormatoCorrectoASCED(valor);

                    // Escritura en el sced
                    this.Servidor.OrbitaEscribir(this.IdDispositivo, new string[1] { this.CodigoVariableSCED }, new object[1] { this.ValorSCED });
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.ES_SCED, this.Servidor.ToString(), exception);
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
                InfoOPCdato[] infoDato = this.Servidor.OrbitaLeerInfo(this.IdDispositivo, new string[1] { this.CodigoVariableSCED });
                if (infoDato.Length == 1)
                {
                    if ((infoDato[0] != null) && (infoDato[0].Ovalor != null))
                    {
                        object valorSCED = infoDato[0].Ovalor;

                        if (!App.CompararObjetos(this.ValorSCED, valorSCED))
                        {
                            this.ValorSCED = valorSCED;

                            // Conversión
                            this.Valor = this.SCEDAFormatoCorrecto(this.ValorSCED);

                            // Se lanza desde un thread distino.
                            SimpleMethod setVariableEnThread = new SimpleMethod(this.EstablecerValorVariable);
                            setVariableEnThread.BeginInvoke(null, null);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.ES_SCED, this.Servidor.ToString(), exception);
            }
        }

        /// <summary>
        /// Trata el cambio de valor
        /// </summary>
        /// <returns></returns>
        internal override void LeerEntrada(OEventArgs eventArgs)
        {
            try
            {
                base.LeerEntrada(eventArgs);

                // Se devuelve el objeto tal cual
                object valorSCED = ((InfoOPCdato)eventArgs.Argumento).Ovalor;

                if (!App.CompararObjetos(this.ValorSCED, valorSCED))
                {
                    this.ValorSCED = valorSCED;

                    // Conversión
                    this.Valor = this.SCEDAFormatoCorrecto(this.ValorSCED);

                    // Se lanza desde un thread distino.
                    SimpleMethod setVariableEnThread = new SimpleMethod(this.EstablecerValorVariable);
                    setVariableEnThread.BeginInvoke(null, null);
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.ES_SCED, this.Servidor.ToString(), exception);
            }
        }
        #endregion
    }

    /// <summary>
    /// Terminal del SCED para trabajo con texto
    /// </summary>
    internal class TerminalIOSCED_WriteCommand : TerminalIOSCED
    {
        #region Atributo(s)
        /// <summary>
        /// Lista de códigos de terminales asociados
        /// </summary>
        private List<string> ListaCodigosTerminalesAsociados;

        /// <summary>
        /// Lista de terminales asociados
        /// </summary>
        private List<TerminalIOSCED> ListaTerminalesAsociados;
        #endregion

        #region Constructor
        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public TerminalIOSCED_WriteCommand(string codTarjetaIO, string codTerminalIO, EnumTipoDispositivoSCED tipoDispositivoSCED)
            : base(codTarjetaIO, codTerminalIO, tipoDispositivoSCED)
        {
            this.ListaCodigosTerminalesAsociados = new List<string>();
            this.ListaTerminalesAsociados = new List<TerminalIOSCED>();

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
                case EnumTipoDato.SinDefinir:
                case EnumTipoDato.Imagen:
                case EnumTipoDato.Grafico:
                default:
                    resultado = valorObj.ToString();
                    break;
                case EnumTipoDato.Bit:
                    resultado = "0";
                    if ((valorObj is bool) && ((bool)valorObj))
                    {
                        resultado = "1";
                    }
                    break;
                case EnumTipoDato.Entero:
                    resultado = App.EvaluaNumero(valorObj, int.MinValue, int.MaxValue, 0).ToString();
                    break;
                case EnumTipoDato.Texto:
                    resultado = valorObj.ToString();
                    break;
                case EnumTipoDato.Decimal:
                    resultado = App.EvaluaNumero(valorObj, double.MinValue, double.MaxValue, 0D).ToString();
                    break;
                case EnumTipoDato.Fecha:
                    if (valorObj is DateTime)
                    {
                        resultado = valorObj.ToString();
                    }
                    break;
                case EnumTipoDato.Flag:
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

            TarjetaIOBase tarjetaTerminal = IORuntime.ListaTarjetasIO.Find(delegate(TarjetaIOBase t) { return t.Codigo == this.CodTarjeta; });
            foreach (string codTerminalIOAsociado in this.ListaCodigosTerminalesAsociados)
            {
                TerminalIOSCED terminalAsociado = (TerminalIOSCED)tarjetaTerminal.ListaTerminales.Find(delegate(TerminalIOBase t) { return t.Codigo == codTerminalIOAsociado; });
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

                    foreach (TerminalIOSCED terminalAsociado in this.ListaTerminalesAsociados)
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
                LogsRuntime.Error(ModulosHardware.ES_SCED, this.Servidor.ToString(), exception);
            }
        }
        #endregion
    }

    /// <summary>
    /// Tipo de dispositivo controlado por el SCED
    /// </summary>
    internal enum EnumTipoDispositivoSCED
    {
        /// <summary>
        /// Servidor OPC de Siemens
        /// </summary>
        [StringValue("OPC.SimaticNET")]
        OPC_SimaticNET = 1,

        /// <summary>
        /// Tarjeta Ethernet E/S
        /// </summary>
        [StringValue("MCC.EPDISO16")]
        MCC_EPDISO16 = 2,

        /// <summary>
        /// Dispositivo para variables internas
        /// </summary>
        [StringValue("Orbita")]
        Orbita = 3
    }
}

