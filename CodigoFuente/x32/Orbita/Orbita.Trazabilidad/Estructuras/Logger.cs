//***********************************************************************
// Assembly         : OrbitaTrazabilidad
// Author           : crodriguez
// Created          : 02-17-2011
//
// Last Modified By : crodriguez
// Last Modified On : 02-21-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
namespace Orbita.Trazabilidad
{
    /// <summary>
    /// Constantes generales de logger.
    /// </summary>
    public struct Logger
    {
        #region Constantes
        /// <summary>
        /// Puerto por defecto.
        /// </summary>
        public const int Puerto = 1440;
        /// <summary>
        /// Separador de datos para cada item de log.
        /// </summary>
        public const string Separador = "-";
        /// <summary>
        /// Logger.
        /// </summary>
        public const string Alias = "logger";
        /// <summary>
        /// DebugLogger.
        /// </summary>
        public const string Debug = "debug";
        /// <summary>
        /// RemotingLogger.
        /// </summary>
        public const string Remoting = "remoting";
        /// <summary>
        /// TrazaLogger.
        /// </summary>
        public const string Traza = "traza";
        #endregion

        #region Métodos públicos
        /// <summary>
        /// Sobreescritura del método Equals.
        /// </summary>
        /// <param name="obj">Objeto de comparación.</param>
        /// <returns>Si la instancia y el objeto son iguales.</returns>
        public override bool Equals(Object obj)
        {
            return false;
        }
        /// <summary>
        /// Sobreescritura del método GetHashCode.
        /// </summary>
        /// <returns>El código Hash de esta instancia.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// Operador de igualdad.
        /// </summary>
        /// <param name="logger">Logger de tipo contenedor.</param>
        /// <param name="cadena">Cadena de comparación.</param>
        /// <returns>La igualdad de la cadena de comparación con el tipo.</returns>
        public static bool operator ==(Orbita.Trazabilidad.Logger logger, string cadena)
        {
            return logger.Equals(cadena);
        }
        /// <summary>
        /// Operador de desigualdad.
        /// </summary>
        /// <param name="logger">Logger de tipo contenedor.</param>
        /// <param name="cadena">Cadena de comparación.</param>
        /// <returns>La desigualdad de la cadena de comparación con el tipo.</returns>
        public static bool operator !=(Orbita.Trazabilidad.Logger logger, string cadena)
        {
            return !logger.Equals(cadena);
        }
        #endregion
    }
}
