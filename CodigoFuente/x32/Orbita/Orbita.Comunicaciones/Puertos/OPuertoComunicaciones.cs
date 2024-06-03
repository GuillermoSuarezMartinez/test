using System;
using System.Collections;
using System.Data;
using System.Text;
namespace Orbita.Comunicaciones
{
    /// <summary>
    /// Implementa una clase abstacta de un puerto de comunicaciones
    /// </summary>
    public abstract class OPuertoComunicaciones : IDisposable
    {
        #region Atributos protegidos
        /// <summary>
        /// Contiene la información básica del puerto de comunicaciones
        /// </summary>
        protected OInformacionPuerto _InformacionPuerto;
        /// <summary>
        /// Contiene la configuración del puerto de comunicaciones
        /// </summary>
        protected OConfiguracionPuerto _ConfiguracionPuerto;
        #endregion

        #region Atributos públicos
        /// <summary>
        /// Contiene los dispositivos asociados a este puerto
        /// </summary>
        public Hashtable Dispositivos = new Hashtable();
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="configuracionPuerto">Configuración del puerto de comunicaciones</param>
        public OPuertoComunicaciones(OConfiguracionPuerto configuracionPuerto)
        {
            this._ConfiguracionPuerto = configuracionPuerto;
            this._InformacionPuerto = new OInformacionPuerto(-1, "No especificado");
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="configuracionPuerto">Configuración del puerto de comunicaciones</param>
        /// <param name="info">Información básica del puerto de comunicaciones</param>
        public OPuertoComunicaciones(OConfiguracionPuerto configuracionPuerto, OInformacionPuerto info)
        {
            this._ConfiguracionPuerto = configuracionPuerto;
            this._InformacionPuerto = info;
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="configuracionPuerto">Configuración del puerto de comunicaciones</param>
        /// <param name="idNumerico">Identificador númerico del puerto de comunicaciones</param>
        /// <param name="tipoPuerto">Tipo del puerto de comunicaciones</param>
        public OPuertoComunicaciones(OConfiguracionPuerto configuracionPuerto, int idNumerico, string tipoPuerto)
        {
            this._ConfiguracionPuerto = configuracionPuerto;
            this._InformacionPuerto = new OInformacionPuerto(idNumerico, tipoPuerto);
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Obtiene la información básica del puerto de comunicaciones
        /// </summary>
        public OInformacionPuerto InformacionPuerto
        {
            get
            {
                return this._InformacionPuerto;
            }
        }
        /// <summary>
        /// Obtiene la configuración del puerto de comunicaciones
        /// </summary>
        public OConfiguracionPuerto ConfiguracionPuerto
        {
            get
            {
                return this._ConfiguracionPuerto;
            }
        }
        #endregion

        #region Métodos públicos abstractos - Funciones a implementar en función del tipo de puerto.
        /// <summary>
        /// Configura el puerto de comunicaciones
        /// </summary>
        /// <param name="configuracion"></param>
        public abstract void ConfigurarPuerto(OConfiguracionPuerto configuracion);
        /// <summary>
        /// Abre el puerto de comunicaciones
        /// </summary>
        public abstract void Abrir();
        /// <summary>
        /// Cierra el puerto de comunicaciones
        /// </summary>
        public abstract void Cerrar();
        /// <summary>
        /// Envía por el puerto de comunicaciones el vector de bytes tramaTx
        /// </summary>
        /// <param name="tramaTx">Vector de bytes a enviar por el puerto de comunicaciones</param>
        public abstract void Enviar(byte[] tramaTx);
        /// <summary>
        /// Copia en el vector de bytes tramaRx la información recibida por el puerto de comunicaciones
        /// </summary>
        /// <returns>Vector de bytes donde que contendrá la información recibida hasta el momento</returns>
        public abstract byte[] RecibirBytes();
        /// <summary>
        ///  Devuleve el número de bytes recibidos hasta el momento por el puerto de comunicaciones
        /// </summary>
        /// <returns>El número de bytes recibidos hasta el momento</returns>
        public abstract int BytesRecibidos();
        /// <summary>
        /// Resetea el buffer de recepcion de datos del puerto de comunicaciones
        /// </summary>
        public abstract void ResetBuffer();
        /// <summary>
        /// Busca un carácter en el buffer de recepcion de datos del puerto de comunicaciones
        /// </summary>
        /// <param name="caracter">Carácter a buscar</param>
        /// <returns>True encuentra el valor buscado; false en caso contrario</returns>
        public abstract bool BuscarCaracter(byte caracter);
        /// <summary>
        /// Lista los puertos COM RS-232 disponbles en el sistema en un DataTable
        /// </summary>
        /// <returns>Un DataTable con los puertos COM RS-232 disponbles en el sistema</returns>
        public abstract DataTable ObtenerPuertosDisponibles();
        #endregion

        #region Métodos públicos
        /// <summary>
        /// Envia una cadena de texto por el puerto utilizando una codificación ASCII
        /// </summary>
        /// <param name="tramaTx">Cadena de texto a enviar</param>
        public void Enviar(string tramaTx)
        {
            byte[] b = ASCIIEncoding.ASCII.GetBytes(tramaTx);
            this.Enviar(b);
        }
        /// <summary>
        /// Envia una cadena de texto por el puerto utilizando la codificación enc
        /// </summary>
        /// <param name="tramaTx"></param>
        /// <param name="enc">Cadena de texto a enviar</param>
        public void Enviar(string tramaTx, Encoding enc)
        {
            byte[] b = enc.GetBytes(tramaTx);
            this.Enviar(b);
        }
        /// <summary>
        /// Recibe una cadena de texto por el puerto utilizando la codificación ASCII
        /// </summary>
        public string RecibirCadena()
        {
            string tramaRx;
            byte[] b = this.RecibirBytes();
            if (b != null && b.Length > 0)
            {
                tramaRx = ASCIIEncoding.ASCII.GetString(b);
            }
            else
            {
                tramaRx = string.Empty;
            }
            return tramaRx;
        }
        /// <summary>
        /// Recibe una cadena de texto por el puerto utilizando la codificación enc
        /// </summary>
        /// <param name="enc">Cadena que contendrá el texto recibido</param>
        public string RecibirCadena(Encoding enc)
        {
            string tramaRx;
            byte[] b = this.RecibirBytes();
            tramaRx = enc.GetString(b);
            return tramaRx;
        }
        #endregion

        #region Miembros de IDisposable
        /// <summary>
        /// Libera los recursos utilizados
        /// </summary>
        public virtual void Dispose()
        {
            this._InformacionPuerto = null;
            this._ConfiguracionPuerto = null;
            this.Dispositivos.Clear();
            this.Dispositivos = null;
        }
        #endregion
    }
}
