using Orbita.Comunicaciones;
using Orbita.Utiles;

namespace Orbita.Comunicaciones
{
    /// <summary>
    /// Clase que encapsula la configuraciñon del puerto serie RS232.
    /// </summary>
    public class OConfiguracionPuertoRS232 : OConfiguracionPuerto
    {
        #region Atributos/s
        /// <summary>
        /// Identificador de puerto
        /// </summary>
        public int NumeroPuerto;
        /// <summary>
        /// Velocidad del puerto en baudios
        /// </summary>
        public int Velocidad;
        /// <summary>
        /// Bits de datos para la comunicación serie
        /// </summary>
        public int BitsDatos;
        /// <summary>
        /// Paridad para la comunicación serie
        /// </summary>
        public OParidades Paridad;
        /// <summary>
        /// Bits de stop para la comunicación serie
        /// </summary>
        public OBitsStop BitsStop;
        /// <summary>
        /// Handshake para la comunicación serie
        /// </summary>
        public OHandShakes Handshake;

        #endregion Atributos/s

        #region Constructor/es
        /// <summary>
        /// Constructor con la configuración por defecto
        /// </summary>
        public OConfiguracionPuertoRS232()
        {
            //Configuración del puerto serie por defecto
            this.NumeroPuerto = 1;
            this.Velocidad = 9600;
            this.BitsDatos = 8;
            this.Paridad = OParidades.Ninguna;
            this.BitsStop = OBitsStop.Uno;
            this.Handshake = OHandShakes.Ninguno;
        }
        /// <summary>
        /// Constructor con la configuración pasada por argumento
        /// </summary>
        /// <param name="numeroPuerto">Número de puertto</param>
        /// <param name="velocidad">Velocidad del puerto</param>
        /// <param name="bitsDatos">Número de bits de datos del puerto</param>
        /// <param name="paridad">Paridad del puerto</param>
        /// <param name="bitsStop">Número de bits de parada del puerto</param>
        /// <param name="handShakes">Control de flujo del puerto</param>
        public OConfiguracionPuertoRS232(int numeroPuerto, int velocidad, int bitsDatos, OParidades paridad, OBitsStop bitsStop, OHandShakes handShakes)
        {
            this.NumeroPuerto = numeroPuerto;
            this.Velocidad = velocidad;
            this.BitsDatos = bitsDatos;
            this.Paridad = paridad;
            this.BitsStop = bitsStop;
            this.Handshake = handShakes;
        }
        #endregion Constructor/es
    }
    /// <summary>
    /// Enumera las posibles velocidades para el puerto serie RS232.
    /// </summary>
    public enum OVelocidad
    {
        /// <summary>
        /// 110 baudios
        /// </summary>
        [OAtributoEnumerado("110 bds")]
        bds110 = 110,
        /// <summary>
        /// 300 baudios
        /// </summary>
        [OAtributoEnumerado("300 bds")]
        bds300 = 300,
        /// <summary>
        /// 1200 baudios
        /// </summary>
        [OAtributoEnumerado("1200 bds")]
        bds1200 = 1200,
        /// <summary>
        /// 2400 baudios
        /// </summary>
        [OAtributoEnumerado("2400 bds")]
        bds2400 = 2400,
        /// <summary>
        /// 4800 baudios
        /// </summary>
        [OAtributoEnumerado("4800 bds")]
        bds4800 = 4800,
        /// <summary>
        /// 9600 baudios
        /// </summary>
        [OAtributoEnumerado("9600 bds")]
        bds9600 = 9600,
        /// <summary>
        /// 19200 baudios
        /// </summary>
        [OAtributoEnumerado("19200 bds")]
        bds19200 = 19200,
        /// <summary>
        /// 38400 baudios
        /// </summary>
        [OAtributoEnumerado("38400 bds")]
        bds38400 = 38400,
        /// <summary>
        /// 57600 baudios
        /// </summary>
        [OAtributoEnumerado("57600 bds")]
        bds57600 = 57600,
        /// <summary>
        /// 115200 baudios
        /// </summary>
        [OAtributoEnumerado("115200 bds")]
        bds115200 = 115200,
        /// <summary>
        /// 230400 baudios
        /// </summary>
        [OAtributoEnumerado("230400 bds")]
        bds230400 = 230400,
        /// <summary>
        /// 460800 baudios
        /// </summary>
        [OAtributoEnumerado("460800 bds")]
        bds460800 = 460800,
        /// <summary>
        /// 921600 baudios
        /// </summary>
        [OAtributoEnumerado("921600 bds")]
        bds921600 = 921600
    }
    /// <summary>
    /// Enumera las posibles opciones de bits de datos para el puerto serie RS232.
    /// </summary>
    public enum OBitsDatos
    {
        /// <summary>
        /// 5 bits
        /// </summary>
        [OAtributoEnumerado("5 bits")]
        b5 = 5,
        /// <summary>
        /// 6 bits
        /// </summary>
        [OAtributoEnumerado("6 bits")]
        b6 = 6,
        /// <summary>
        /// 7 bits
        /// </summary>
        [OAtributoEnumerado("7 bits")]
        b7 = 7,
        /// <summary>
        /// 8 bits
        /// </summary>
        [OAtributoEnumerado("8 bits")]
        b8 = 8
    }
    /// <summary>
    /// Enumera las posibles paridades para el puerto serie RS232.
    /// </summary>
    public enum OParidades
    {
        /// <summary>
        /// Ninguna
        /// </summary>
        [OAtributoEnumerado("Ninguna")]
        Ninguna,
        /// <summary>
        /// Par
        /// </summary>
        [OAtributoEnumerado("Par")]
        Par,
        /// <summary>
        /// Impar
        /// </summary>
        [OAtributoEnumerado("Impar")]
        Impar,
        /// <summary>
        /// Marca
        /// </summary>
        [OAtributoEnumerado("Marca")]
        Marca,
        /// <summary>
        /// Espacio
        /// </summary>
        [OAtributoEnumerado("Espacio")]
        Espacio
    }
    /// <summary>
    /// Enumera las posibles opciones de bits de parada para el puerto serie RS232.
    /// </summary>
    public enum OBitsStop
    {

        /// <summary>
        /// Uno
        /// </summary>
        [OAtributoEnumerado("Uno")]
        Uno,
        /// <summary>
        /// Uno y medio
        /// </summary>
        [OAtributoEnumerado("Uno y medio")]
        UnoYMedio,
        /// <summary>
        /// Dos
        /// </summary>
        [OAtributoEnumerado("Dos")]
        Dos
    }
    /// <summary>
    /// Enumera las posibles opciones del control de flujo de parada para el puerto serie RS232.
    /// </summary>
    public enum OHandShakes
    {
        /// <summary>
        /// Ninguno
        /// </summary>
        [OAtributoEnumerado("Ninguno")]
        Ninguno,
        /// <summary>
        /// Ninguno
        /// </summary>
        [OAtributoEnumerado("XonXoff")]
        XonXoff,
        /// <summary>
        /// CtsRts
        /// </summary>
        [OAtributoEnumerado("CtsRts")]
        CtsRts,
        /// <summary>
        /// DsrDtr
        /// </summary>
        [OAtributoEnumerado("DsrDtr")]
        DsrDtr
    }
}
