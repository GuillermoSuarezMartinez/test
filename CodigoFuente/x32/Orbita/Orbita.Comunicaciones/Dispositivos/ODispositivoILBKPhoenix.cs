using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Security.Permissions;
using System.Threading;
using System.Data;
using Orbita.Trazabilidad;
using Orbita.Utiles;
using Orbita.Winsock;

namespace Orbita.Comunicaciones
{
    /// <summary>
    /// Dispositivo entradas salidas ILBKPhoenix
    /// </summary>
    public class ODispositivoILBKPhoenix : ODispositivoES
    {
        #region Atributos
        /// <summary>
        /// Valor inicial del registro de lecturas
        /// </summary>
        private int _registroInicialLecturas;
        /// <summary>
        /// Valor inicial del registro de escrituras
        /// </summary>
        private int _registroInicialEscrituras;
        /// <summary>
        /// Número de lecturas a realizar
        /// </summary>
        private int _numLecturas;
        /// <summary>
        /// Número de escrituras a realizar
        /// </summary>
        private int _numEscrituras;
        /// <summary>
        /// Registros modbus
        /// </summary>
        private ArrayList _registrosModbus;
        /// <summary>
        /// Valores de la lectura modbus
        /// </summary>
        private int[] _valorLectura;
        /// <summary>
        /// Contiene OHashtable con los registros de lectura y escritura
        /// </summary>
        private OHashtable _datosRegistros;
        /// <summary>
        /// Tipo de objeto instanciado
        /// </summary>
        private TipoBKPhoenix _tipoBKPhoenix;
        /// <summary>
        /// Tipos de BK
        /// </summary>
        enum TipoBKPhoenix
        {
            ILBKPhoenix48
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de clase de ILBKPhoenix
        /// </summary>
        public ODispositivoILBKPhoenix(OTags tags, OHilos hilos, ODispositivo dispositivo)
            : base(tags, hilos, dispositivo)
        {
            switch (dispositivo.Tipo)
            {
                case "IL.BK.Phoenix48":
                    this._tipoBKPhoenix = TipoBKPhoenix.ILBKPhoenix48;
                    break;
                default:
                    break;
            }
            this.CrearParametrosConexionTCP();
            this.GetRegister();
        }

        #endregion

        #region Metodos publicos

        /// <summary>
        /// Procesa las lecturas del dispositivo
        /// </summary>
        public override void ProcesarHiloVida()
        {
            OEstadoComms estado = new OEstadoComms();
            estado.Nombre = this.Nombre;
            estado.Id = this.Identificador;
            while (true)
            {
                if (this.Winsock.State != WinsockStates.Connected)
                {
                    try
                    {
                        this.Conectar();
                        int segReconexion = Convert.ToInt32(this.SegReconexion * 1000);
                        Thread.Sleep(segReconexion);
                        estado.Estado = "NOK";
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger("wrapper").Error("Error al conectar con ODispositivoILBKPhoenix: ", ex);
                    }
                }
                else
                {
                    try
                    {
                        this.Winsock.Send(this.ConfigurarMensajeLectura());
                        estado.Estado = "OK";
                    }
                    catch (Exception ex)
                    {
                        estado.Estado = "NOK";
                        LogManager.GetLogger("wrapper").Error("Error de lectura continua con ODispositivoILBKPhoenix: ", ex);
                    }
                }

                try
                {
                    this.OEventargs.Argumento = estado;
                    this.OnComm(this.OEventargs);
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger("wrapper").Error("Error en hilo vida de ODispositivoILBKPhoenix: ", ex);
                }

                Thread.Sleep(this.Config.TiempoVida);
            }
        }
        /// <summary>
        /// Leer el valor de las descripciones de variables de la colección
        /// a partir del valor de la colección de datos DB actualiza  en el
        /// proceso del hilo vida.
        /// </summary>
        /// <param name="variables">Colección de variables.</param>
        /// <param name="demanda">Indica si la lectura se ejecuta sobre el dispositivo</param>
        /// <returns>Colección de resultados.</returns>
        public override object[] Leer(string[] variables, bool demanda)
        {
            object[] resultado = null;

            try
            {
                if (variables != null)
                {
                    // Inicializar contador de variables.
                    int contador = variables.Length;
                    // Asignar a la colección resultado el número
                    // de variables de la colección de variables.
                    resultado = new object[contador];

                    if (demanda)
                    {
                        for (int i = 0; i < contador; i++)
                        {
                            object res = this.Tags.GetDB(variables[i]).Valor;
                            resultado[i] = res;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < contador; i++)
                        {
                            object res = this.Tags.GetDB(variables[i]).Valor;
                            resultado[i] = res;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("wrapper").Fatal("OrbitaTCP Error al leer. ", ex);
                throw ex;
            }

            return resultado;
        }
        /// <summary>
        /// Escribir el valor de los identificadores de variables de la colección.
        /// </summary>
        /// <param name="variables">Colección de variables.</param>
        /// <param name="valores">Colección de valores.</param>
        /// <returns></returns>
        public override bool Escribir(string[] variables, object[] valores)
        {
            bool resultado = true;
            char[] salidas = this.getSalidas();

            try
            {
                if (variables != null)
                {
                    for (int i = 0; i < variables.Length; i++)
                    {
                        switch (this._tipoBKPhoenix)
                        {
                            case TipoBKPhoenix.ILBKPhoenix48:
                                OInfoDato infoDBdato = this.Tags.GetDB(variables[i]);
                                if (!infoDBdato.EsEntrada)
                                {
                                    salidas[infoDBdato.Bit] = Convert.ToChar(valores[i].ToString());
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                this.Escritura(salidas);
            }
            catch (Exception ex)
            {
                resultado = false;
                LogManager.GetLogger("wrapper").Fatal("OrbitaTCP Error al escribir: ", ex);
            }

            return resultado;
        }

        #endregion

        #region Metodos Privados

        /// <summary>
        /// Publica los eventos de socket
        /// </summary>
        private void CrearParametrosConexionTCP()
        {
            this.Winsock = new OWinsockBase();
            this.Winsock.LegacySupport = true;
            this.Winsock.DataArrival += new IWinsock.DataArrivalEventHandler(_winsock_DataArrival);
            this.Winsock.StateChanged += new IWinsock.StateChangedEventHandler(_winsock_StateChanged);
            this.Winsock.SendComplete += new IWinsock.SendCompleteEventHandler(_winsock_SendComplete);
            this.Winsock.ErrorReceived += new IWinsock.ErrorReceivedEventHandler(_winsock_ErrorReceived);
        }
        /// <summary>
        /// Calcula el registro inicial para la lectura modbus y el número de registros
        /// </summary>
        private void GetRegister()
        {
            ArrayList list = new ArrayList();
            ArrayList listEscrituras = new ArrayList();

            foreach (DictionaryEntry var in this.Tags.GetDatos())
            {
                OInfoDato item = (OInfoDato)var.Value;

                if (!list.Contains(item.Direccion))
                {
                    list.Add(item.Direccion);
                }

                if (!item.EsEntrada)
                {
                    if (!listEscrituras.Contains(item.Direccion))
                    {
                        listEscrituras.Add(item.Direccion);
                    }
                }
            }

            list.Sort();
            listEscrituras.Sort();

            this._registrosModbus = list;//Para el ILBKPhoenix48 tiene 2 registros el 8000 y el 80001
            this._numLecturas = list.Count;//Para el ILBKPhoenix48 tiene 2 lecturas
            this._numEscrituras = listEscrituras.Count;//Para el ILBKPhoenix48 tiene 1 escritura
            this._registroInicialLecturas = Convert.ToInt32(list[0]);//Para el ILBKPhoenix48 es el 8000 
            this._registroInicialEscrituras = Convert.ToInt32(listEscrituras[0]);//Para el ILBKPhoenix48 es el 8001

            this._datosRegistros = new OHashtable();
            //Para el ILBKPhoenix48 crea dos nuevos Ohastable con key 8000 y 8001
            for (int i = 0; i < this._numLecturas; i++)
            {
                this._datosRegistros.Add(Convert.ToInt32(list[i]), new OHashtable());
            }

            foreach (DictionaryEntry var in this.Tags.GetDatos())
            {
                OInfoDato item = (OInfoDato)var.Value;

                OHashtable hashReg = (OHashtable)this._datosRegistros[item.Direccion];
                hashReg.Add(item.Bit, item);
            }

        }
        /// <summary>
        /// Conecta con el dispositivo TCP
        /// </summary>
        private void Conectar()
        {
            this.Winsock.Connect(this.Direccion.ToString(), this.Puerto);
        }
        /// <summary>
        /// Proceso de lectura
        /// </summary>
        private void Leer()
        {
            if (this.Winsock.State != WinsockStates.Connected)
            {
                try
                {
                    this.Conectar();
                    int segReconexion = Convert.ToInt32(this.SegReconexion * 1000);
                    Thread.Sleep(segReconexion);
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger("wrapper").Error("Error al conectar con ODispositivoILBKPhoenix: ", ex);
                }
            }
            else
            {
                try
                {
                    this.Winsock.Send(this.ConfigurarMensajeLectura());
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger("wrapper").Error("Error de lectura continua con ODispositivoILBKPhoenix: ", ex);
                }
            }
        }
        /// <summary>
        /// Procesa la información recibida
        /// </summary>
        /// <param name="recibido"></param>
        private int[] ProcesarDataLectura(byte[] data)
        {
            int[] retorno = null;

            if (data[7] == 3)
            {
                byte numResp = data[8];
                int registros = numResp / 2;

                int[] valores = new int[registros];
                byte[] con = new byte[2];
                for (int i = 0; i < registros; i++)
                {
                    int j = 8 + (2 * i + 1);
                    con[0] = data[j];
                    con[1] = data[j + 1];
                    Array.Reverse(con);
                    valores[i] = BitConverter.ToInt16(con, 0);
                }

                retorno = valores;
            }

            return retorno;
        }
        /// <summary>
        /// Construye el mensaje de lectura del dipositivo
        /// </summary>
        /// <returns></returns>
        private byte[] ConfigurarMensajeLectura()
        {
            OModbusTCP mensaje = new OModbusTCP();
            byte[] respuesta = null;
            try
            {
                respuesta = mensaje.configurarMensajeLecturaF3(this._registroInicialLecturas, this._numLecturas);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return respuesta;
        }
        /// <summary>
        /// Proceso de escritura
        /// </summary>
        private void Escritura(char[] salidas)
        {
            if (this.Winsock.State != WinsockStates.Connected)
            {
                try
                {
                    this.Conectar();
                    int segReconexion = Convert.ToInt32(this.SegReconexion * 1000);
                    Thread.Sleep(segReconexion);
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger("wrapper").Error("Error al conectar con ODispositivoILBKPhoenix: ", ex);
                }
            }
            else
            {
                try
                {
                    this.Winsock.Send(this.configurarMensajeEscritura(salidas));
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger("wrapper").Error("Error de lectura continua con ODispositivoILBKPhoenix: ", ex);
                }
            }
        }
        /// <summary>
        /// Construye el mensaje de escritura del dipositivo
        /// </summary>
        /// <returns></returns>
        private byte[] configurarMensajeEscritura(char[] salidas)
        {
            OModbusTCP mensaje = new OModbusTCP();
            byte[] respuesta = null;
            try
            {
                switch (this._tipoBKPhoenix)
                {
                    case TipoBKPhoenix.ILBKPhoenix48:
                        byte[] escrituras = new byte[2];
                        Array.Reverse(salidas);
                        int valor = Convert.ToInt32(new string(salidas), 2);
                        escrituras = BitConverter.GetBytes(valor);

                        escrituras[1] = escrituras[0];
                        escrituras[0] = 0;

                        Array.Resize(ref escrituras, 2);

                        respuesta = mensaje.configurarMensajeEscrituraF16(this._registroInicialEscrituras, escrituras);

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return respuesta;
        }
        /// <summary>
        /// Actualiza los valores de las entradas y salidas. Genera los eventos de cambio de dato y alarma
        /// </summary>
        /// <param name="valores">valor del registro</param>
        private void ActualizarDatos(int[] valores)
        {
            Orbita.Utiles.OEventArgs ev = new Orbita.Utiles.OEventArgs();

            for (int i = 0; i < this._registrosModbus.Count; i++)
            {
                int registro = Convert.ToInt32(this._registrosModbus[i].ToString());

                if (this._valorLectura == null)
                {
                    this._valorLectura = new int[this._registrosModbus.Count];
                }

                if (valores[i] != this._valorLectura[i])
                {
                    OHashtable hashReg = (OHashtable)this._datosRegistros[registro];

                    foreach (DictionaryEntry item in hashReg)
                    {
                        OInfoDato dato = (OInfoDato)item.Value;

                        if (IsBitSet((byte)valores[i], (dato.Bit)))
                        {
                            if (dato.Valor == null || (int)dato.Valor == 0)
                            {
                                dato.UltimoValor = 0;
                                dato.Valor = 1;

                                if (this.Tags.GetLecturas(dato.Identificador) != null)
                                {
                                    ev.Argumento = dato;
                                    this.OnCambioDato(ev);
                                }

                                if (this.Tags.GetAlarmas(dato.Identificador) != null)
                                {
                                    if (!AlarmasActivas.Contains(dato.Texto))
                                    {
                                        this.AlarmasActivas.Add(dato.Texto);
                                    }

                                    this.OnAlarma(ev);
                                }
                            }
                        }
                        else
                        {
                            if (dato.Valor == null || (int)dato.Valor == 1)
                            {
                                dato.UltimoValor = 1;
                                dato.Valor = 0;

                                if (this.Tags.GetLecturas(dato.Identificador) != null)
                                {
                                    ev.Argumento = dato;
                                    this.OnCambioDato(ev);
                                }

                                if (this.Tags.GetAlarmas(dato.Identificador) != null)
                                {
                                    if (AlarmasActivas.Contains(dato.Texto))
                                    {
                                        this.AlarmasActivas.Remove(dato.Texto);
                                    }

                                    this.OnAlarma(ev);
                                }
                            }
                        }
                    }
                    this._valorLectura[i] = valores[i];
                }
            }
        }
        /// <summary>
        /// Establece si un bit está a uno
        /// </summary>
        /// <param name="b">valor de byte</param>
        /// <param name="pos">posición del bit</param>
        /// <returns></returns>
        private bool IsBitSet(byte b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }
        /// <summary>
        /// Obtiene el valor de las salidas
        /// </summary>
        /// <returns>array de char con las salidas a 0 o 1</returns>
        private char[] getSalidas()
        {
            char[] salidas = null;

            switch (this._tipoBKPhoenix)
            {
                case TipoBKPhoenix.ILBKPhoenix48:

                    OHashtable hashReg = (OHashtable)this._datosRegistros[_registroInicialEscrituras];
                    salidas = new char[4];
                    for (int i = 0; i < 4; i++)
                    {
                        try
                        {
                            OInfoDato info = (OInfoDato)hashReg[i];
                            if ((int)info.Valor == 1)
                            {
                                salidas[i] = '1';
                            }
                            else
                            {
                                salidas[i] = '0';
                            }
                        }
                        catch (Exception ex)
                        {
                            salidas[i] = '0';
                        }

                    }

                    break;
                default:
                    break;
            }

            return salidas;
        }
        #endregion

        #region Eventos Socket

        /// <summary>
        /// Evento de recepción de datos
        /// </summary>
        /// <param name="sender">Objeto que lanza el evento</param>
        /// <param name="e">Argumentos del evento</param>
        void _winsock_DataArrival(object sender, WinsockDataArrivalEventArgs e)
        {
            try
            {
                String data = "";   // data es la variable donde guardaremos el mensaje recibido
                Object dat = (object)data;  // dat es una variable tipo objeto
                // el metodo Get de winsock solo devuelve datos de tipo objeto.

                dat = Winsock.Get<object>();

                string ret = "";
                byte[] recibido = (byte[])dat;
                if (recibido != null)
                {
                    for (int i = 0; i < recibido.Length; i++)
                    {
                        ret += "[" + recibido[i].ToString() + "]";
                    }
                }

                this.ActualizarDatos(this.ProcesarDataLectura(recibido));
                LogManager.GetLogger("wrapper").Info("Data Arrival: " + ret);
            }
            catch (Exception ex)
            {
                string error = "Error Data Arrival: " + ex.ToString();
                LogManager.GetLogger("wrapper").Error(error);
            }
        }
        /// <summary>
        /// Indica que el objeto winsock ha enviado toda la información
        /// </summary>
        /// <param name="sender">Objeto que lanza el evento</param>
        /// <param name="e">Argumentos del evento</param>
        void _winsock_SendComplete(object sender, WinsockSendEventArgs e)
        {
            //verificamos la llegada
            try
            {
                string enviado = "";
                if (e.DataSent != null)
                {
                    for (int i = 0; i < e.DataSent.Length; i++)
                    {
                        enviado += "[" + e.DataSent[i].ToString() + "]";
                    }
                }

                LogManager.GetLogger("wrapper").Info("Send Complete: " + enviado);

            }
            catch (Exception ex)
            {
                string error = "Error Send Complete: " + ex.ToString();
                LogManager.GetLogger("wrapper").Error(error);
            }
        }
        /// <summary>
        /// Indica que el objeto winsock ha cambiado de estado. Trazabilidad del canal.
        /// </summary>
        /// <param name="sender">Objeto que lanza el evento</param>
        /// <param name="e">Argumentos del evento</param>
        void _winsock_StateChanged(object sender, WinsockStateChangedEventArgs e)
        {
            try
            {
                string estado = "State Changed. Cambia de " + e.Old_State.ToString() + " a " + e.New_State.ToString();
                LogManager.GetLogger("wrapper").Info(estado);
            }
            catch (Exception ex)
            {
                string error = "Error State Changed: " + ex.ToString();
                LogManager.GetLogger("wrapper").Error(error);
            }
        }
        /// <summary>
        /// Evento de errores en la comunicación TCP
        /// </summary>
        /// <param name="sender">Objeto que lanza el evento</param>
        /// <param name="e">Argumentos del evento</param>
        void _winsock_ErrorReceived(object sender, WinsockErrorReceivedEventArgs e)
        {
            try
            {
                string error = "Error Received: " + e.Message;
                LogManager.GetLogger("wrapper").Error(error);
            }
            catch (Exception ex)
            {
                string error = "Error Received: " + ex.ToString();
                LogManager.GetLogger("wrapper").Error(error);
            }
        }

        #endregion
    }
}
