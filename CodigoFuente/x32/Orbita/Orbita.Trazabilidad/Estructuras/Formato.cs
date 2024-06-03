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
    /// Formatos generales.
    /// </summary>
    public struct Formato
    {
        #region Constantes
        /// <summary>
        /// Anyo.
        /// </summary>
        public const string Año = "yyyy";
        /// <summary>
        /// Mes.
        /// </summary>
        public const string Mes = "MM";
        /// <summary>
        /// Dia.
        /// </summary>
        public const string MesDia = "MMdd";
        /// <summary>
        /// AnyoMes.
        /// </summary>
        public const string AñoMes = "yyyyMM";
        /// <summary>
        /// AnyoMesDia.
        /// </summary>
        public const string AñoMesDia = "yyyyMMdd";
        /// <summary>
        /// Fecha larga.
        /// </summary>
        public const string FechaLarga = "yyyy-MM-dd HH:mm:ss.fff";
        /// <summary>
        /// HoraMinutoSegundoCentesima.
        /// </summary>
        public const string HoraMinutoSegundoCentesima = "HHmmssfff";
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
        /// <param name="formato">Formato de tipo contenedor.</param>
        /// <param name="cadena">Cadena de comparación.</param>
        /// <returns>La igualdad de la cadena de comparación con el tipo.</returns>
        public static bool operator ==(Orbita.Trazabilidad.Formato formato, string cadena)
        {
            return formato.Equals(cadena);
        }
        /// <summary>
        /// Operador de desigualdad.
        /// </summary>
        /// <param name="formato">Formato de tipo contenedor.</param>
        /// <param name="cadena">Cadena de comparación.</param>
        /// <returns>La desigualdad de la cadena de comparación con el tipo.</returns>
        public static bool operator !=(Orbita.Trazabilidad.Formato formato, string cadena)
        {
            return !formato.Equals(cadena);
        }
        #endregion
    }
}
