using System;
using System.Collections.Generic;
using System.Text;

namespace Orbita.VAComun
{
    /// <summary>
    /// Trabajo con números binarios
    /// </summary>
    public static class OBinario
    {
        /// <summary>
        /// Extrae un bit en la posición indicada
        /// </summary>
        /// <param name="numero">Valor al cual queremos extraer el bit</param>
        /// <param name="posicion">Posición del bit a extraer</param>
        /// <returns>Booleano con el valor del bit extraido</returns>
        public static bool GetBit(byte numero, int posicion)
        {
            byte mascara = Convert.ToByte(Math.Pow(2, posicion));
            return (numero & mascara) != 0;
        }

        /// <summary>
        /// Extrae un bit en la posición indicada
        /// </summary>
        /// <param name="numero">Valor al cual queremos extraer el bit</param>
        /// <param name="posicion">Posición del bit a extraer</param>
        /// <returns>Booleano con el valor del bit extraido</returns>
        public static bool GetBit(ushort numero, int posicion)
        {
            UInt16 mascara = Convert.ToUInt16(Math.Pow(2, posicion));
            return (numero & mascara) != 0;
        }

        /// <summary>
        /// Extrae un bit en la posición indicada
        /// </summary>
        /// <param name="numero">Valor al cual queremos extraer el bit</param>
        /// <param name="posicion">Posición del bit a extraer</param>
        /// <returns>Booleano con el valor del bit extraido</returns>
        public static bool GetBit(uint numero, int posicion)
        {
            uint mascara = Convert.ToUInt32(Math.Pow(2, posicion));
            return (numero & mascara) != 0;
        }

        /// <summary>
        /// Establece un bit en la posición indicada
        /// </summary>
        /// <param name="numero">Valor al cual queremos establecer el bit</param>
        /// <param name="posicion">Posición del bit a establecer</param>
        /// <param name="valor">Booleano con el valor del bit a establecer</param>
        public static void SetBit(ref byte numero, int posicion, bool valor)
        {
            byte mascara = Convert.ToByte(Math.Pow(2, posicion));
            if (valor)
            {
                numero = (byte)(numero | mascara);
            }
            else
            {
                numero = (byte)(numero & ~mascara);
            }
        }

        /// <summary>
        /// Establece un bit en la posición indicada
        /// </summary>
        /// <param name="numero">Valor al cual queremos establecer el bit</param>
        /// <param name="posicion">Posición del bit a establecer</param>
        /// <param name="valor">Booleano con el valor del bit a establecer</param>
        public static void SetBit(ref ushort numero, int posicion, bool valor)
        {
            ushort mascara = Convert.ToUInt16(Math.Pow(2, posicion));
            if (valor)
            {
                numero = (ushort)(numero | mascara);
            }
            else
            {
                numero = (ushort)(numero & ~mascara);
            }
        }

        /// <summary>
        /// Establece un bit en la posición indicada
        /// </summary>
        /// <param name="numero">Valor al cual queremos establecer el bit</param>
        /// <param name="posicion">Posición del bit a establecer</param>
        /// <param name="valor">Booleano con el valor del bit a establecer</param>
        public static void SetBit(ref uint numero, int posicion, bool valor)
        {
            uint mascara = Convert.ToUInt32(Math.Pow(2, posicion));
            if (valor)
            {
                numero = (uint)(numero | mascara);
            }
            else
            {
                numero = (uint)(numero & ~mascara);
            }
        }
    }
}
