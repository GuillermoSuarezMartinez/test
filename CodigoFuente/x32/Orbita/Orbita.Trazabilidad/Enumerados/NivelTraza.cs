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
    /// Define los niveles disponibles de interactuación.
    ///</summary>
    public enum NivelTraza
    {
        /// <summary>
        /// Llama a procedimientos de inserción.
        /// </summary>
        Add = 0,
        /// <summary>
        /// Llama a procedimientos de eliminación.
        /// </summary>
        Del = 1,
        /// <summary>
        /// Llama a procedimientos de modificación.
        /// </summary>
        Mdf = 2
    }
}