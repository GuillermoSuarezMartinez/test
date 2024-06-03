using System;
using System.Collections.Generic;
using System.Text;

namespace Orbita.Comunicaciones
{
    /// <summary>
    /// Protocolo de comunicación para modbus TCP
    /// </summary>
    public class OModbusTCP
    {
        #region Variables

        /// <summary>
        /// cabecera del mensaje de lectura F3
        /// </summary>
        private byte[] _cabeceraMensajeLecturaF3;
        /// <summary>
        /// cabecera del mensaje de escritura F16
        /// </summary>
        private byte[] _cabeceraMensajeEscrituraF16; 

        #endregion
        /// <summary>
        /// Cosntructor de clase para el protocolo modbus TCP
        /// </summary>
        public OModbusTCP()
        { 
            this._cabeceraMensajeLecturaF3 = new byte[8];
            this._cabeceraMensajeEscrituraF16 = new byte[8];
        }

        #region Metodos
 
        /// <summary>
        /// Configura el mensaje para la lectura de variables F3
        /// </summary>
        /// <param name="registro">direccion inicial de lectura</param>
        /// <param name="lecturas">numero de lecturas</param>
        /// <returns></returns>
        public byte[] configurarMensajeLecturaF3(int registro, int lecturas)
        {
            byte[] mensaje = null;

            try
            {
                mensaje = new byte[12];

                this.CabeceraMensajeLecturaF3.CopyTo(mensaje, 0);

                byte[] registroDisp = this.convertirArrayLectura(registro);
                byte[] valorLecturas = this.convertirArrayLectura(lecturas);

                registroDisp.CopyTo(mensaje,8);
                valorLecturas.CopyTo(mensaje,10);
            }
            catch (Exception ex)
            {
                string error = "Error en configurarMensajeLecturaF3: " + ex;
                throw ex;
            }
            

            return mensaje;
        }
        /// <summary>
        /// Configura el mensaje para las escrituras de variables F6
        /// </summary>
        /// <param name="registro">direccion inicial para las escrituras</param>
        /// <param name="escrituras">valor de las escrituras</param>
        /// <returns></returns>
        public byte[] configurarMensajeEscrituraF16(int registro, byte[] escrituras)
        {
            byte[] mensaje = null;

            try
            {
                mensaje = new byte[13+escrituras.Length];

                //agregamos la cabecera
                this.CabeceraMensajeEscrituraF16.CopyTo(mensaje, 0);
                //agregamos el registro
                byte[] registroDisp = this.convertirArrayLectura(registro);
                registroDisp.CopyTo(mensaje, 8);               
                //agregamos el número de valores
                byte[] wordCount = new byte[2];
                wordCount = this.convertirArrayLectura(escrituras.Length/2);
                wordCount.CopyTo(mensaje, 10);
                //agregamos el número de bytes
                byte[] numBytes = new byte[1];
                numBytes = BitConverter.GetBytes(escrituras.Length);
                Array.Resize(ref numBytes, 1);
                numBytes.CopyTo(mensaje, 12);
                //agregamos las escrituras
                escrituras.CopyTo(mensaje,13);
            }
            catch (Exception ex)
            {
                string error = "Error en configurarMensajeLecturaF3: " + ex;
                throw ex;
            }


            return mensaje;
        }
        
        private byte[] convertirArrayLectura(int valor)
        {
            byte[] retorno = null;
            try
            {
                retorno = BitConverter.GetBytes(valor);
                Array.Resize(ref retorno, 2);

                byte valor1 = retorno[1];
                retorno[1] = retorno[0];
                retorno[0] = valor1;
               
            }
            catch (Exception ex)
            {
                string error = "Error en convertirArrayLectura: " + ex;
                throw ex;
            }            

            return retorno;
        }

        #endregion

        #region Propiedades
        /// <summary>
        /// cabecera del mensaje de lectura F3
        /// </summary>
        public byte[] CabeceraMensajeLecturaF3
        {
            get {

                this._cabeceraMensajeLecturaF3[0] = 0;
                this._cabeceraMensajeLecturaF3[1] = 0;
                this._cabeceraMensajeLecturaF3[2] = 0;
                this._cabeceraMensajeLecturaF3[3] = 0;
                this._cabeceraMensajeLecturaF3[4] = 0;
                this._cabeceraMensajeLecturaF3[5] = 6;
                this._cabeceraMensajeLecturaF3[6] = 255;
                this._cabeceraMensajeLecturaF3[7] = 3;

                return _cabeceraMensajeLecturaF3; }
        }
        /// <summary>
        /// cabecera del mensaje de escritura F16
        /// </summary>
        public byte[] CabeceraMensajeEscrituraF16
        {
            get {
                this._cabeceraMensajeEscrituraF16[0] = 0;
                this._cabeceraMensajeEscrituraF16[1] = 0;
                this._cabeceraMensajeEscrituraF16[2] = 0;
                this._cabeceraMensajeEscrituraF16[3] = 0;
                this._cabeceraMensajeEscrituraF16[4] = 0;
                this._cabeceraMensajeEscrituraF16[5] = 9;
                this._cabeceraMensajeEscrituraF16[6] = 1;
                this._cabeceraMensajeEscrituraF16[7] = 16;
                
                return _cabeceraMensajeEscrituraF16; }           
        }

        #endregion
    }
}
