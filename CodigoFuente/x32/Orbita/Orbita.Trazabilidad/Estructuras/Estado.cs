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
    /// Acciones generales.
    /// </summary>
    public struct Estado
    {
        #region Constantes
        /// <summary>
        /// Excepción general.
        /// </summary>
        public const string Excepcion = "Excepción";
        /// <summary>
        /// Acción de backup.
        /// </summary>
        public const string Backup = "Backup";
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
        /// <param name="estado">Estado de tipo contenedor.</param>
        /// <param name="cadena">Cadena de comparación.</param>
        /// <returns>La igualdad de la cadena de comparación con el tipo.</returns>
        public static bool operator ==(Orbita.Trazabilidad.Estado estado, string cadena)
        {
            return estado.Equals(cadena);
        }
        /// <summary>
        /// Operador de desigualdad.
        /// </summary>
        /// <param name="estado">Estado de tipo contenedor.</param>
        /// <param name="cadena">Cadena de comparación.</param>
        /// <returns>La desigualdad de la cadena de comparación con el tipo.</returns>
        public static bool operator !=(Orbita.Trazabilidad.Estado estado, string cadena)
        {
            return !estado.Equals(cadena);
        }
        #endregion
    }
}
