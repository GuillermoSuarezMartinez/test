using System;
using System.Collections.Generic;
using System.Text;

namespace Orbita.Comunicaciones
{
    /// <summary>
    /// Configuración del dispositivo
    /// </summary>
    [Serializable]
    public class OConfigDispositivo
    {
        #region Atributos privados
        /// <summary>
        /// Tiempos de espera de lectura (segundos).
        /// </summary>
        int _tiempoEsperaLectura;
        /// <summary>
        /// Tiempos de espera de escritura (segundos).
        /// </summary>
        int _tiempoEsperaEscritura;
        /// <summary>
        /// Tiempos de espera de vida (segundos).
        /// </summary>
        int _tiempoVida;
        #endregion

        #region Atributos públicos
        /// <summary>
        /// Tiempo de espera lectura (segundos).
        /// </summary>
        public int TiempoEsperaLectura
        {
            get { return this._tiempoEsperaLectura; }
            set { this._tiempoEsperaLectura = value; }
        }
        /// <summary>
        /// Tiempo de espera escritura (segundos).
        /// </summary>
        public int TiempoEsperaEscritura
        {
            get { return this._tiempoEsperaEscritura; }
            set { this._tiempoEsperaEscritura = value; }
        }
        /// <summary>
        /// Tiempo vida (segundos).
        /// </summary>
        public int TiempoVida
        {
            get { return this._tiempoVida; }
            set { this._tiempoVida = value; }
        }
        #endregion
    }
}
