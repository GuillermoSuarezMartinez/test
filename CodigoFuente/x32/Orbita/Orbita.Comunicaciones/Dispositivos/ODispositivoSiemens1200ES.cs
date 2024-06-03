using System;
using System.Collections;
using System.Text;
using System.Threading;

using Orbita.Winsock;
using Orbita.Utiles;
using Orbita.Trazabilidad;

namespace Orbita.Comunicaciones
{
    /// <summary>
    /// Dispositivo entradas salidas Siemens1200
    /// </summary>
    public class ODispositivoSiemens1200ES : ODispositivoES
    {
        #region Atributos

        /// <summary>
        /// Cola de recepción de tramas de datos.
        /// </summary>
        private Queue _qEntradaSalida;
        /// <summary>
        /// Evento reset de recepción de tramas KeepAlive, 
        /// Entrada/Salida.
        /// </summary>
        private OResetManual _eReset;  //0-keep alive;1-lecturas;2-escrituras
        /// <summary>
        /// Colección para la búsqueda de lecturas. La clave es la dupla "dirección-bit"
        /// </summary>
        private OHashtable _almacenLecturas;
        /// <summary>
        /// Colección para la búsqueda de escrituras. La clave es la dupla "dirección-bit"
        /// </summary>
        private OHashtable _almacenEscrituras;       
        /// <summary>
        /// Número de lecturas a realizar
        /// </summary>
        private int _numLecturas;
        /// <summary>
        /// Número de bytes de entradas
        /// </summary>
        private int _numeroBytesEntradas;
        /// <summary>
        /// Número de bytes de salidas
        /// </summary>
        private int _numeroBytesSalidas;
        /// <summary>
        /// Valor de las lecturas
        /// </summary>
        private byte[] _lecturas;
        /// <summary>
        /// Valor inicial del registro de lecturas
        /// </summary>
        private int _registroInicialEntradas;
        /// <summary>
        /// Valor inicial del registro de escrituras
        /// </summary>
        private int _registroInicialSalidas;

        /// <summary>
        /// identificador del mensaje
        /// </summary>
        byte idMensaje = 0;
        /// <summary>
        /// Protocolo comunicación usado para el hilo vida
        /// </summary>
        private ProtocoloTCPSiemens protocoloHiloVida;
        /// <summary>
        /// Protocolo comunicación usado para la escritura
        /// </summary>
        private ProtocoloTCPSiemens protocoloEscritura;
        /// <summary>
        /// Protocolo comunicación usado para el proceso del mensaje
        /// </summary>
        private ProtocoloTCPSiemens protocoloProcesoMensaje;
        /// <summary>
        /// Protocolo de comunicación usado para el proceso del hilo
        /// </summary>
        private ProtocoloTCPSiemens protocoloProcesoHilo;
        /// <summary>
        /// Valor devuelto tras la escritura del PLC
        /// </summary>
        private byte[] _valorEscritura;

        //DateTime dtinilec;
        //DateTime dtfinlec;
        //DateTime dtiniproc;
        //DateTime dtfinproc;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de clase de Siemens1200
        /// </summary>
        public ODispositivoSiemens1200ES(OTags tags, OHilos hilos, ODispositivo dispositivo)
            : base(tags, hilos, dispositivo)
        {
            //Inicialización de objetos
            this.IniciarObjetos();
            LogManager.GetLogger("wrapper").Debug("Objetos del dispositivo de ES Siemens creados.");
            //Inicio de los parámetros TCP
            try
            {
                this.CrearParametrosConexionTCP();
                Hilos.Add(new ThreadStart(ESProcesarHilo), true);
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("wrapper").Error("No se pudo crear la conexión TCP con el dispositivo de ES Siemens." + ex.ToString());
            }

            LogManager.GetLogger("wrapper").Debug("Comunicaciones TCP del dispositivo de ES Siemens arrancadas correctamente.");
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
            estado.Enlace = this.Tags.HtVida.Enlaces[0].ToString();
            Boolean responde = true;

            int reintento = 0;
            int maxReintentos = 3;

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
                        LogManager.GetLogger("wrapper").Error("Error al conectar con dispositivo de ES Siemens para keep alive: ", ex);
                    }
                }
                else
                {
                    try
                    {
                        using (protocoloHiloVida)
                        {
                            // Enviar un mensaje de KeepAlive. 
                            //this.dtinilec = DateTime.Now;
                            this.Winsock.Send(protocoloHiloVida.KeepAliveEnviar());

                            // Letargo del hilo hasta t-tiempo, o bien, hasta recibir respuesta, el cual realiza el Set sobre
                            // el eventReset del evento 'wsk_DataArrival'.
                            if (!this._eReset.Dormir(0, TimeSpan.FromSeconds(this.Config.TiempoVida / 1000)))
                            {
                                if (reintento<maxReintentos)
                                {
                                    reintento++;
                                }
                                else
                                {
                                    responde = false;
                                    estado.Estado = "NOK";
                                }                                
                                // Trazar recepción errónea.
                                LogManager.GetLogger("wrapper").Warn("Timeout en el keep alive del dispositivo de ES Siemens.");
                            }

                            // Resetear el evento.
                            this._eReset.Resetear(0);

                            // Letargo del hilo en función de la respuesta.
                            if (responde)
                            {
                                //this.dtfinlec = DateTime.Now;
                                //TimeSpan ts = this.dtfinlec.Subtract(dtinilec);
                                //LogManager.GetLogger("wrapper").Info("tiempo de lectura " + ts.TotalMilliseconds.ToString());
                                estado.Estado = "OK";
                                reintento = 0;
                                Thread.Sleep(this.Config.TiempoEsperaLectura);
                            }
                            else
                            {
                                responde = true;
                                // Establecer un micro tiempo de letargo, necesario en este tipo de hilos.
                                // Realizar un envio inmediato tras la no respuesta anterior.
                                Thread.Sleep(10);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        estado.Estado = "NOK";
                        LogManager.GetLogger("wrapper").Error("Error en keep alive del dispositivo de ES Siemens: ", ex);
                    }
                }

                try
                {
                    this.OEventargs.Argumento = estado;
                    this.OnComm(this.OEventargs);
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger("wrapper").Error("Error en envío de evento de comunicaciones en dispositivo de ES Siemens: ", ex);
                }
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
                    for (int i = 0; i < contador; i++)
                    {
                        object res = this.Tags.GetDB(variables[i]).Valor;
                        resultado[i] = res;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("wrapper").Fatal("Error al leer en el dispositivo de ES Siemens. ", ex);
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
            bool resultado = false;

            byte[] salidas = new byte[_numeroBytesSalidas];
            for (int i = 0; i < this.Salidas.Length; i++)
            {
                salidas[i] = this.Salidas[i];
            }
            try
            {
                for (int i = 0; i < variables.Length; i++)
                {
                    OInfoDato infodato = this.Tags.GetDB(variables[i]);

                    salidas[infodato.Direccion - this._registroInicialSalidas] = this.ProcesarByte(salidas[infodato.Direccion - this._registroInicialSalidas], infodato.Bit, Convert.ToInt32(valores[i]));

                }
                if (this.Winsock.State != WinsockStates.Connected)
                {
                    try
                    {
                        //Se reconectará en el keep alive
                        Thread.Sleep(Convert.ToInt32(this.SegReconexion * 1000));
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger("wrapper").Error("Error al conectar con dispositivo de ES Siemens para escribir: ", ex);
                    }
                }
                else
                {
                    using (protocoloEscritura)
                    {
                        try
                        {
                            //Configuramos las salidas en dos bytes
                            byte[] byteSalidas = protocoloEscritura.SalidasEnviar(salidas[0], this.IdMensaje);

                            if (byteSalidas != null)
                            {
                                this.Winsock.Send(byteSalidas);

                                if (!this._eReset.Dormir(2, TimeSpan.FromSeconds(this.Config.TiempoEsperaEscritura / 1000)))
                                {
                                    // Trazar recepción errónea.
                                    LogManager.GetLogger("wrapper").Warn("Timeout en la escritura de variables en el dispositivo de ES Siemens");
                                }
                                else
                                {
                                    if (protocoloEscritura.SalidasProcesar(this._valorEscritura, this.idMensaje))
                                    {
                                        resultado = true;
                                    }
                                }
                                // Resetear el evento.
                                this._eReset.Resetear(2); 
                            }                            
                        }
                        catch (Exception ex)
                        {
                            LogManager.GetLogger("wrapper").Error("Error en la escritura de variables en el dispositivo de ES Siemens " + ex.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("wrapper").Fatal("Error en la escritura de variables en el dispositivo de ES Siemens " + ex.ToString());
            }

            return resultado;
        }

        #endregion

        #region Metodos Privados

        #region Comunes

        /// <summary>
        /// Publica los eventos de socket
        /// </summary>
        private void CrearParametrosConexionTCP()
        {
            try
            {
                this.Winsock = new OWinsockBase();
                this.Winsock.LegacySupport = true;
                this.Winsock.DataArrival += new IWinsock.DataArrivalEventHandler(_winsock_DataArrival);
                this.Winsock.StateChanged += new IWinsock.StateChangedEventHandler(_winsock_StateChanged);
                this.Winsock.SendComplete += new IWinsock.SendCompleteEventHandler(_winsock_SendComplete);
                this.Winsock.ErrorReceived += new IWinsock.ErrorReceivedEventHandler(_winsock_ErrorReceived);
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("wrapper").Fatal("Error al crear la conexión TCP con el dispositivo de ES Siemens. " + ex.ToString());
                throw ex;
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
        /// Establece el valor inicial de los objetos
        /// </summary>
        private void IniciarObjetos()
        {
            // Cola de envío/recepción de tramas.
            this._qEntradaSalida = new Queue();
            // Evento reset de envío/recepción de tramas KeepAlive y Entrada/Salida.
            this._eReset = new OResetManual(3);
            // Iniciamos los datos 
            ArrayList listEntradas = new ArrayList();
            ArrayList listSalidas = new ArrayList();

            this._almacenLecturas = new OHashtable();
            this._almacenEscrituras = new OHashtable();

            if (this.Protocolo == "OCR")
            {
                this.protocoloHiloVida = new ProtocoloTCPSiemensGateOCRES();
                this.protocoloEscritura = new ProtocoloTCPSiemensGateOCRES();
                this.protocoloProcesoMensaje = new ProtocoloTCPSiemensGateOCRES();
                this.protocoloProcesoHilo = new ProtocoloTCPSiemensGateOCRES();
            }
            else if(this.Protocolo == "OS")
            {
                this.protocoloHiloVida = new ProtocoloTCPSiemensGateOSES();
                this.protocoloEscritura = new ProtocoloTCPSiemensGateOSES();
                this.protocoloProcesoMensaje = new ProtocoloTCPSiemensGateOSES();
                this.protocoloProcesoHilo = new ProtocoloTCPSiemensGateOSES();                
            }

            foreach (DictionaryEntry item in this.Tags.GetDatos())
            {
                OInfoDato infodato = (OInfoDato)item.Value;
                infodato.Valor = 0;
                string key = infodato.Direccion.ToString() + "-" + infodato.Bit.ToString();
                if (infodato.EsEntrada)
                {
                    this._almacenLecturas.Add(key, infodato);
                    if (!listEntradas.Contains(infodato.Direccion))
                    {
                        listEntradas.Add(infodato.Direccion);
                    }
                }
                else
                {
                    this._almacenEscrituras.Add(key, infodato);
                    if (!listSalidas.Contains(infodato.Direccion))
                    {
                        listSalidas.Add(infodato.Direccion);
                    }
                }
            }

            listEntradas.Sort();
            listSalidas.Sort();

            this._numLecturas = listEntradas.Count + listSalidas.Count;//OCR: 4E/1S
            this._numeroBytesEntradas = listEntradas.Count;//OCR: 4E
            this._numeroBytesSalidas = listSalidas.Count;//OCR: 1S
            this._registroInicialEntradas = Convert.ToInt32(listEntradas[0]);//OCR: 0
            this._registroInicialSalidas = Convert.ToInt32(listSalidas[0]);//OCR: 0

            this.Entradas = new byte[this._numeroBytesEntradas];
            this.Salidas = new byte[this._numeroBytesSalidas];

            this._lecturas = new byte[_numLecturas];
        }
        /// <summary>
        /// Procesa los mensajes recibidos en el data arrival
        /// </summary>
        /// <param name="mensaje"></param>
        private void ProcesarMensajeRecibido(byte[] mensaje)
        {
            try
            {
                byte[] bmensaje = new byte[13];
                Array.Copy(mensaje,1,bmensaje,0,13);
                string smensaje = ASCIIEncoding.ASCII.GetString(bmensaje);
                using (protocoloProcesoMensaje)
                {
                    if (smensaje.Contains("OCRDATA") || smensaje.Contains("OS_DATA"))//respuesta para la lectura
                    {
                        if (mensaje[15]==0)
                        {
                            byte[] lecturas;
                            if (protocoloProcesoMensaje.KeepAliveProcesar(mensaje, out lecturas))
                            {
                                for (int i = 0; i < this._numLecturas; i++)
                                {
                                    if (this._lecturas[i] != lecturas[i])
                                    {
                                        this.ESEncolar(lecturas);
                                        break;
                                    }
                                }
                                this._lecturas = lecturas;
                                // Despertar el hilo en la línea:
                                // this._eReset.Dormir de ProcesarHiloKeepAlive.                        
                                this._eReset.Despertar(0);
                            }
                        }
                        else//respuesta para la escritura
                        {
                            this._valorEscritura = mensaje;
                            this._eReset.Despertar(2);
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                string error = "Error en ProcesarMensajeRecibido en el dispositivo de ES Siemens: " + ex.ToString();
                LogManager.GetLogger("wrapper").Error(error);
            }
            
        }
        /// <summary>
        /// Procesa los bits poniendo a 1 o 0 el bit correspondiente
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="bit"></param>
        /// <param name="valorBit"></param>
        /// <returns></returns>
        private byte ProcesarByte(byte valor, int bit, int valorBit)
        {
            byte ret = 0;

            try
            {
                if (valorBit == 1)
                {
                    ret = (byte)((valor) | (byte)(Math.Pow(2, bit)));
                }
                else
                {
                    ret = (byte)(valor & ~(byte)(Math.Pow(2, bit)));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }

        #endregion

        #region ES

        /// <summary>
        /// Método que encola trama GateData.
        /// </summary>
        /// <param name="trama"></param>
        private void ESEncolar(byte[] trama)
        {
            // Bloquear la cola sincronizada.
            lock (this._qEntradaSalida.SyncRoot)
            {
                // Encolar la trama de ES recibida.
                this._qEntradaSalida.Enqueue(trama);

                // Despertar el hilo 'ESProcesarHilo' aletargado
                // en la línea: 'this._eReset.Dormir(1)'
                this._eReset.Despertar(1);
            }
        }
        /// <summary>
        /// Método que desencola trama GateData.
        /// </summary>
        /// <returns>Objeto GateData</returns>
        private byte[] ESDesencolar()
        {
            // Bloquear la cola sincronizada.
            lock (this._qEntradaSalida.SyncRoot)
            {
                byte[] mensaje = null;
                if (this._qEntradaSalida.Count > 0)
                {
                    // Desencolar el objeto Trama encolado en wsk_DataArrival.
                    mensaje = ((byte[])this._qEntradaSalida.Dequeue());
                }
                else
                {
                    this._eReset.Resetear(1);
                }
                return mensaje;
            }
        }
        /// <summary>
        /// Hilo de proceso de ES
        /// </summary>
        private void ESProcesarHilo()
        {
            while (true)
            {
                byte[] mensaje = this.ESDesencolar();
                //this.dtiniproc = DateTime.Now;

                if (mensaje != null)
                {
                    try
                    {
                        byte[] entradas = new byte[4];
                        byte[] salidas = new byte[1];

                        Array.Copy(mensaje, 0, entradas, 0, 4);
                        Array.Copy(mensaje, 4, salidas, 0, 1);

                        this.ESProcesar(entradas, salidas);
                        //this.dtfinproc = DateTime.Now;
                        //TimeSpan ts = dtfinproc.Subtract(dtiniproc);
                        //LogManager.GetLogger("wrapper").Info("tiempo de proceso " + ts.TotalMilliseconds.ToString());
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger("wrapper").Fatal("Error al procesar las ES en el dispositivo de ES Siemens. " + ex.ToString());
                    }
                    Thread.Sleep(10);
                }
                else
                {
                    this._eReset.Dormir(1);
                }
            }
        }
        /// <summary>
        /// Procesa los bytes de entradas y salidas para actualizar los valores de las variables
        /// </summary>
        /// <param name="entradas">byte de entradas recibido</param>
        /// <param name="salidas">byte de salidas recibido</param>
        private void ESProcesar(byte[] entradas, byte[] salidas)
        {
            try
            {
                for (int i = 0; i < entradas.Length; i++)
                {
                    if (entradas[i] != this.Entradas[i])
                    {
                        this.ESActualizarVariablesEntradas(entradas[i], i + this._registroInicialEntradas);
                    }
                    if (i==0)
                    {
                        if (salidas[i] != this.Salidas[i])
                        {
                            this.ESActualizarVariablesSalidas(salidas[i], i + this._registroInicialSalidas);
                        }
                    }                    
                }
                this.Entradas = entradas;
                this.Salidas = salidas;
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("wrapper").Fatal("Error al procesar las ES en el dispositivo de ES Siemens en ESProcesar. " + ex.ToString());
                throw ex;
            }
        }
        /// <summary>
        /// Actualiza los valores de las entradas y genera los eventos de cambio de dato y alarma
        /// </summary>
        /// <param name="valor">valor del byte</param>
        /// <param name="posicion">posición del byte</param>
        private void ESActualizarVariablesEntradas(byte valor, int posicion)
        {
            OInfoDato infodato = null;
            OEventArgs ev = new OEventArgs();
            try
            {
                if (posicion<2)//datos de bits
                {
                    for (int i = 0; i < 8; i++)
                    {
                    
                            infodato = (OInfoDato)this._almacenLecturas[posicion.ToString() + "-" + i.ToString()];
                            //Comprobamos el valor nuevo 
                            if (infodato != null)
                            {
                                int resultado = 0;
                                if ((valor & (1 << i)) != 0)
                                {
                                    resultado = 1;
                                }

                                if (resultado != Convert.ToInt32(infodato.Valor))
                                {
                                    infodato.Valor = resultado;
                                    ev.Argumento = infodato;
                                    this.OnCambioDato(ev);

                                    if (this.Tags.GetAlarmas(infodato.Identificador) != null)
                                    {
                                        if (Convert.ToInt32(infodato.Valor) == 1)
                                        {
                                            if (!AlarmasActivas.Contains(infodato.Texto))
                                            {
                                                this.AlarmasActivas.Add(infodato.Texto);
                                            }
                                        }
                                        else
                                        {
                                            if (AlarmasActivas.Contains(infodato.Texto))
                                            {
                                                this.AlarmasActivas.Remove(infodato.Texto);
                                            }
                                        }

                                        this.OnAlarma(ev);
                                    }
                                }
                            }
                            else
                            {
                                LogManager.GetLogger("wrapper").Warn("No se puede encontrar la dupla " + posicion.ToString() + "-" + i.ToString() +
                                    " al actualizar las variables de entrada en el dispositivo de ES Siemens.");
                            }
                    
                    }
                }
                else//posiciones de bytes
                {
                    infodato = (OInfoDato)this._almacenLecturas[posicion.ToString() + "-0"];
                    //Comprobamos el valor nuevo 
                    if (infodato != null)
                    {
                        if (valor != Convert.ToInt32(infodato.Valor))
                        {
                            infodato.Valor = valor;
                            ev.Argumento = infodato;
                            this.OnCambioDato(ev);                            
                        }
                    }
                    else
                    {
                        LogManager.GetLogger("wrapper").Warn("No se puede encontrar la dupla " + posicion.ToString() + "-0" +
                            " al actualizar las variables de entrada en el dispositivo de ES Siemens.");
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("wrapper").Error("Error no controlado al procesar las entradas en el dispositivo de ES Siemens" + ex.ToString());
            }
        }
        /// <summary>
        /// Actualiza los valores de las salidas y genera los eventos de cambio de dato y alarma
        /// </summary>
        /// <param name="valor">valor del byte</param>
        /// <param name="posicion">posición del byte</param>
        private void ESActualizarVariablesSalidas(byte valor, int posicion)
        {
            OInfoDato infodato = null;
            OEventArgs ev = new OEventArgs();

            for (int i = 0; i < 8; i++)
            {
                try
                {
                    infodato = (OInfoDato)this._almacenEscrituras[posicion.ToString() + "-" + i.ToString()];
                    //Comprobamos el valor nuevo 
                    if (infodato != null)
                    {
                        int resultado = 0;
                        if ((valor & (1 << i)) != 0)
                        {
                            resultado = 1;
                        }

                        if (resultado != Convert.ToInt32(infodato.Valor))
                        {
                            infodato.Valor = resultado;
                            ev.Argumento = infodato;
                            this.OnCambioDato(ev);
                        }

                        if (this.Tags.GetAlarmas(infodato.Identificador) != null)
                        {
                            if (Convert.ToInt32(infodato.Valor) == 1)
                            {
                                if (!AlarmasActivas.Contains(infodato.Texto))
                                {
                                    this.AlarmasActivas.Add(infodato.Texto);
                                }
                            }
                            else
                            {
                                if (AlarmasActivas.Contains(infodato.Texto))
                                {
                                    this.AlarmasActivas.Remove(infodato.Texto);
                                }
                            }

                            this.OnAlarma(ev);
                        }
                    }
                    else
                    {
                        LogManager.GetLogger("wrapper").Warn("No se puede encontrar la dupla " + posicion.ToString() + "-" + i.ToString() +
                            " al actualizar las variables de salida en el dispositivo de ES Siemens.");
                    }

                }
                catch (Exception ex)
                {
                    LogManager.GetLogger("wrapper").Error("Error no controlado al procesar las salidas en el dispositivo de ES Siemens " + ex.ToString());
                }

            }
        }

        #endregion

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

                this.ProcesarMensajeRecibido(recibido);
                LogManager.GetLogger("wrapper").Info("Data Arrival en el dispositivo de ES Siemens: " + ret);
            }
            catch (Exception ex)
            {
                string error = "Error Data Arrival en el dispositivo de ES Siemens: " + ex.ToString();
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

                LogManager.GetLogger("wrapper").Info("Send Complete en el dispositivo de ES Siemens: " + enviado);

            }
            catch (Exception ex)
            {
                string error = "Error Send Complete en el dispositivo de ES Siemens: " + ex.ToString();
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
                string estado = "State Changed en el dispositivo de ES Siemens. Cambia de " + e.Old_State.ToString() + " a " + e.New_State.ToString();
                LogManager.GetLogger("wrapper").Info(estado);
            }
            catch (Exception ex)
            {
                string error = "Error State Changed en el dispositivo de ES Siemens: " + ex.ToString();
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
                string error = "Error Received en el dispositivo de ES Siemens: " + e.Message;
                LogManager.GetLogger("wrapper").Info(error);
            }
            catch (Exception ex)
            {
                string error = "Error Received en el dispositivo de ES Siemens: " + ex.ToString();
                LogManager.GetLogger("wrapper").Error(error);
            }
        }

        #endregion

        #region Propiedades
        /// <summary>
        /// Identificador del mensaje
        /// </summary>
        public byte IdMensaje
        {
            get {
                    if (this.idMensaje>255)
                    {
                        idMensaje = 0;
                    }
                    else
                    {
                        idMensaje++;
                    }
                    return idMensaje; 
                }
        }

        #endregion
    }
}
