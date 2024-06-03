using System;
using System.Collections;
using System.Data;
using System.IO.Ports;
using System.Threading;
using Orbita.Comunicaciones;
using Orbita.Utiles;

namespace Orbita.Comunicaciones
{
    /// <summary>
    /// Clase que encapsula un puerto serie RS232
    /// </summary>
    public class OPuertoRS232 : OPuertoComunicaciones
    {
        #region Atributo/s
        /// <summary>
        /// PuertoRS232.
        /// </summary>
        private SerialPort PuertoRS232;
        /// <summary>
        /// Buffer de recepción de datos
        /// </summary>
        public Queue BufferDatosRecibidos;
        #endregion Atributo/s

        #region Constructor/es
        /// <summary>
        /// Constructor del puerto con la configuración por defecto
        /// </summary>
        public OPuertoRS232()
            : base(new OConfiguracionPuertoRS232())
        {
            this.IniciarComponentes();
        }
        /// <summary>
        /// Constructor del puerto con la configuración pasada por argumento
        /// </summary>
        /// <param name="configuracionPuerto">Configuración del puerto serie RS232</param>
        public OPuertoRS232(OConfiguracionPuertoRS232 configuracionPuerto)
            : base(configuracionPuerto)
        {
            this.IniciarComponentes();
        }
        /// <summary>
        /// Constructor del puerto con la configuración e información básica pasada por argumento
        /// </summary>
        /// <param name="configuracionPuerto">Configuración del puerto serie RS232</param>
        /// <param name="info">Información básica del puerto de comunicaciones</param>
        public OPuertoRS232(OConfiguracionPuertoRS232 configuracionPuerto, OInformacionPuerto info)
            : base(configuracionPuerto, info)
        {
            this.IniciarComponentes();
        }
        /// <summary>
        /// Constructor del puerto con la configuración e información básica pasada por argumento
        /// </summary>
        /// <param name="configuracionPuerto">Configuración del puerto serie RS232</param>
        /// <param name="idNumerico">Identificador númerico del puerto de comunicaciones</param>
        /// <param name="tipoPuerto">Tipo del puerto de comunicaciones</param>
        public OPuertoRS232(OConfiguracionPuertoRS232 configuracionPuerto, int idNumerico, string tipoPuerto)
            : base(configuracionPuerto, idNumerico, tipoPuerto)
        {
            this.IniciarComponentes();
        }
        #endregion Constructor/es

        #region Propiedad/es
        /// <summary>
        /// Indica si el puerto serie RS232  está abierto
        /// </summary>
        public bool EstaAbierto
        {
            get
            {
                return this.PuertoRS232.IsOpen;
            }
        }
        #endregion Propiedad/es

        #region Metodo/s privado/s
        /// <summary>
        /// Inicia todos los componentes de este puerto de comunicaciones, a excepción del inicio del puerto serie, ya que se hace desde la clase padre al configurar el puerto
        /// </summary>
        private void IniciarComponentes()
        {
            this.BufferDatosRecibidos = new Queue();

            if (this.PuertoRS232 != null)
            {
                if (this.PuertoRS232.IsOpen)
                {
                    this.PuertoRS232.Close();
                }
                this.PuertoRS232.Dispose();
                this.PuertoRS232 = null;
            }
            this.PuertoRS232 = new SerialPort();
            this.PuertoRS232.DataReceived += new SerialDataReceivedEventHandler(PuertoRS232_DataReceived);
            this.ConfigurarPuerto(this.ConfiguracionPuerto);
        }
        /// <summary>
        /// Establece una equivaencia unidireccional entre el tipo Paridades y el tipo Parity
        /// </summary>
        /// <param name="paridad">Paridad a convertir</param>
        /// <returns>Paridad convertida</returns>
        private Parity ConvertirParidad(OParidades paridad)
        {
            Parity convParidad = Parity.None;
            switch (paridad)
            {
                case OParidades.Ninguna:
                    convParidad = Parity.None;
                    break;
                case OParidades.Par:
                    convParidad = Parity.Even;
                    break;
                case OParidades.Impar:
                    convParidad = Parity.Odd;
                    break;
                case OParidades.Marca:
                    convParidad = Parity.Mark;
                    break;
                case OParidades.Espacio:
                    convParidad = Parity.Space;
                    break;
            }
            return convParidad;
        }
        /// <summary>
        /// Establece una equivaencia unidireccional entre el tipo BitsStop y el tipo StopBits
        /// </summary>
        /// <param name="bitsStop">Bits de parada a convertir</param>
        /// <returns>Bits de parada convertidos</returns>
        private StopBits ConvertirBitsStop(OBitsStop bitsStop)
        {
            StopBits convBitsStop = StopBits.Two;

            switch (bitsStop)
            {
                case OBitsStop.Uno:
                    convBitsStop = StopBits.One;
                    break;
                case OBitsStop.UnoYMedio:
                    convBitsStop = StopBits.OnePointFive;
                    break;
                case OBitsStop.Dos:
                    convBitsStop = StopBits.Two;
                    break;
            }
            return convBitsStop;
        }
        /// <summary>
        /// Establece una equivaencia unidireccional entre el tipo HandShakes y el tipo Handshake
        /// </summary>
        /// <param name="handshake">Control de flujo a convertir</param>
        /// <returns>Control de flujo convertidos</returns>
        private Handshake ConvertirHandshake(OHandShakes handshake)
        {
            Handshake convHandshake = Handshake.None;
            switch (handshake)
            {
                case OHandShakes.CtsRts:
                    convHandshake = Handshake.RequestToSend;
                    break;
                case OHandShakes.XonXoff:
                    convHandshake = Handshake.XOnXOff;
                    break;
                case OHandShakes.DsrDtr:
                    throw new Exception("Handshake DsrDtr no disponible con el control de Orbita.Comunicaciones serie de VS2005.");
            }
            return convHandshake;
        }
        #endregion Metodo/s privado/s

        #region Metodo/s publico/s
        /// <summary>
        /// Crea y configura el puerto serie RS232
        /// </summary>
        /// <param name="configuracionPuerto">Configuración del puerto RS232 a establecer</param>
        public override void ConfigurarPuerto(OConfiguracionPuerto configuracionPuerto)
        {
            this._ConfiguracionPuerto = configuracionPuerto;
            if (this.PuertoRS232 != null)
            {
                if (this.PuertoRS232.IsOpen)
                {
                    this.PuertoRS232.Close();
                }
            }

            OConfiguracionPuertoRS232 configRS232 = (OConfiguracionPuertoRS232)configuracionPuerto;
            this.PuertoRS232.PortName = "COM" + configRS232.NumeroPuerto.ToString();
            this.PuertoRS232.BaudRate = configRS232.Velocidad;
            this.PuertoRS232.DataBits = configRS232.BitsDatos;
            this.PuertoRS232.Parity = this.ConvertirParidad(configRS232.Paridad);
            this.PuertoRS232.StopBits = this.ConvertirBitsStop(configRS232.BitsStop);
            this.PuertoRS232.Handshake = this.ConvertirHandshake(configRS232.Handshake);
        }
        /// <summary>
        /// Abre el puerto serie RS232
        /// </summary>
        public override void Abrir()
        {
            if (this.PuertoRS232 != null)
            {
                if (this.PuertoRS232.IsOpen)
                {
                    return;
                }

                this.PuertoRS232.Open();
            }
        }
        /// <summary>
        /// Cierra el puerto serie RS232
        /// </summary>
        public override void Cerrar()
        {
            if (this.PuertoRS232 != null)
            {
                if (this.PuertoRS232.IsOpen)
                {
                    this.PuertoRS232.Close();
                    Thread.Sleep(200);
                }
            }
        }
        /// <summary>
        /// Envía por el puerto serie RS232 el vector de bytes tramaTx
        /// </summary>
        /// <param name="tramaTx">Vector de bytes a enviar por el puerto serie RS232</param>
        public override void Enviar(byte[] tramaTx)
        {
            if (this.PuertoRS232 != null)
            {
                if (this.PuertoRS232.IsOpen)
                {
                    if (tramaTx.Length > 0)
                    {
                        this.PuertoRS232.Write(tramaTx, 0, tramaTx.Length);
                    }
                }
            }
        }
        /// <summary>
        /// Copia en el vector de bytes tramaRx la información recibida por el puerto de comunicaciones
        /// </summary>
        /// <returns>Vector de bytes donde que contendrá la información recibida hasta el momento</returns>
        public override byte[] RecibirBytes()
        {
            lock (this)
            {
                byte[] tramaRx;

                if (this.BufferDatosRecibidos.Count == 0)
                {
                    tramaRx = null;
                }
                else
                {
                    tramaRx = new byte[this.BufferDatosRecibidos.Count];
                    int i = 0;

                    while (this.BufferDatosRecibidos.Count > 0)
                    {
                        tramaRx[i] = (byte)this.BufferDatosRecibidos.Dequeue();
                        i++;
                    }

                    this.BufferDatosRecibidos.TrimToSize();
                }
                return tramaRx;
            }
        }
        /// <summary>
        /// Devuleve el número de bytes recibidos hasta el momento
        /// </summary>
        /// <returns>El número de bytes recibidos hasta el momento</returns>
        public override int BytesRecibidos()
        {
            lock (this)
            {
                return this.BufferDatosRecibidos.Count;
            }
        }
        /// <summary>
        /// Resetea el buffer de recepcion de datos. Realmente resetea la variable que lleva la posición del último byte recibido, por lo que los datos permanencen en el buffer hasta que son sobreescritos.
        /// </summary>
        public override void ResetBuffer()
        {
            lock (this)
            {
                this.BufferDatosRecibidos.Clear();
                this.BufferDatosRecibidos.TrimToSize();
            }
        }
        /// <summary>
        /// Busca un carácter en el buffer de recepcion de datos
        /// </summary>
        /// <param name="caracter">Carácter a buscar</param>
        /// <returns>True encuentra el valor buscado; false en caso contrario</returns>
        public override bool BuscarCaracter(byte caracter)
        {
            lock (this)
            {
                return this.BufferDatosRecibidos.Contains(caracter);
            }
        }
        /// <summary>
        /// Elimina los recursos que está utilizando
        /// </summary>
        public override void Dispose()
        {
            if (this.PuertoRS232 != null)
            {
                this.Cerrar();
                this.PuertoRS232.DataReceived -= new SerialDataReceivedEventHandler(PuertoRS232_DataReceived);
                this.PuertoRS232.Dispose();
                this.PuertoRS232 = null;
                this.BufferDatosRecibidos.Clear();
                this.BufferDatosRecibidos.TrimToSize();
                this.BufferDatosRecibidos = null;
            }
        }
        /// <summary>
        /// Lista los puertos COM RS-232 disponbles en el sistema en un DataTable con 3 columnas:
        ///  - Identificador: identificador numérico dado por el orden de la lista de puertos COM devuelto por el sistema
        ///  - Nombre: nombre del puerto serie RS-232
        ///  - NumeroCOM: número del puerto serie RS-232
        /// </summary>
        /// <returns>Un DataTable con los puertos COM RS-232 disponbles en el sistema con 3 columnas:
        ///  - Identificador: identificador numérico dado por el orden de la lista de puertos COM devuelto por el sistema
        ///  - Nombre: nombre del puerto serie RS-232
        ///  - NumeroCOM: número del puerto serie RS-232</returns>
        public override DataTable ObtenerPuertosDisponibles()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Identificador", typeof(System.Int32));
            dt.Columns.Add("Nombre", typeof(System.String));
            dt.Columns.Add("NumeroCOM", typeof(System.String));
            DataRow dr;
            string[] puertos = SerialPort.GetPortNames();

            for (int i = 0; i < puertos.Length; i++)
            {
                dr = dt.NewRow();
                dr["Identificador"] = i;
                dr["Nombre"] = puertos[i].ToString();
                dr["NumeroCOM"] = puertos[i].ToString().Substring(3);
                dt.Rows.Add(dr);
            }
            return dt;
        }
        #endregion Metodo/s publico/s

        #region Manejador/es de evento/s
        /// <summary>
        /// Se ejecuta cada vez que se reciben datos por this.PuertoRS232
        /// </summary>
        /// <param name="sender">Objeto que lanza el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void PuertoRS232_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                //TODO: Habría que dar la posibilidad de abrir el puertos serie en dos modos, a saber:
                // - Tal y como está, en el que se van escribiendo los datos en su buffer interno
                // - En un modo en el que no se escriben los datos, pero se propaga el evento lazando por el puerto serie del framework. Esta segunda opción es la que habría que implementar.
                lock (this)
                {
                    int numeroBytes = this.PuertoRS232.BytesToRead;

                    if (numeroBytes > 0)
                    {
                        byte[] vb = new byte[numeroBytes];
                        this.PuertoRS232.Read(vb, 0, numeroBytes);
                        foreach (byte b in vb)
                        {
                            this.BufferDatosRecibidos.Enqueue(b);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OMensajes.MostrarError(ex.ToString());
            }
        }
        #endregion Manejador/es de evento/s
    }
}


