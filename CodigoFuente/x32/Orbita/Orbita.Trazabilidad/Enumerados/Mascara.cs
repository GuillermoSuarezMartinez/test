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
namespace Orbita.Trazabilidad
{
    ///<summary>
    /// Define las máscaras de subpath para el backup de logger.
    ///</summary>
    public enum Mascara
    {
        /// <summary>
        /// Sin máscara de subpath de backup logger.
        /// </summary>
        None = 0,
        /// <summary>
        /// Máscara de subpath de backup logger con formato 'aaaa'.
        /// </summary>
        Año = 1,
        /// <summary>
        /// Máscara de subpath de backup logger con formato 'MM'.
        /// </summary>
        Mes = 2,
        /// <summary>
        /// Máscara de subpath de backup logger con formato 'MMdd'.
        /// </summary>
        MesDia = 3,
        /// <summary>
        /// Máscara de subpath de backup logger con formato 'aaaaMM'.
        /// </summary>
        AñoMes = 4,
        /// <summary>
        /// Máscara de subpath de backup logger con formato 'aaaaMMdd'.
        /// </summary>
        AñoMesDia = 5
    }
}
