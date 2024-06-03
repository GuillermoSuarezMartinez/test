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
using System.Collections;
namespace Orbita.Trazabilidad
{
    /// <summary>
    /// ITraza.
    /// </summary>
    public interface ITraza
    {
        #region Propiedades privadas
        /// <summary>
        /// Identificador de traza.
        /// </summary>
        string Identificador { get; }
        #endregion

        #region Métodos privados

        #region Add
        /// <summary>
        /// Llama a un procedimiento de inserción (<see cref="NivelTraza.Add"/>).
        /// </summary>
        /// <param name="procedimiento">El procedimiento almacenado.</param>
        int Add(string procedimiento);
        /// <summary>
        /// Llama a un procedimiento de inserción (<see cref="NivelTraza.Add"/>).
        /// </summary>
        /// <param name="procedimiento">El procedimiento almacenado.</param>
        /// <param name="args">Una colección de parámetros.</param>
        int Add(string procedimiento, ArrayList args);
        #endregion

        #region Del
        /// <summary>
        /// Llama a un procedimiento de eliminación (<see cref="NivelTraza.Del"/>).
        /// </summary>
        /// <param name="procedimiento">El procedimiento almacenado.</param>
        int Del(string procedimiento);
        /// <summary>
        /// Llama a un procedimiento de eliminación (<see cref="NivelTraza.Del"/>).
        /// </summary>
        /// <param name="procedimiento">El procedimiento almacenado.</param>
        /// <param name="args">Una colección de parámetros.</param>
        int Del(string procedimiento, ArrayList args);
        #endregion

        #region Mdf
        /// <summary>
        /// Llama a un procedimiento de modificación (<see cref="NivelTraza.Mdf"/>).
        /// </summary>
        /// <param name="procedimiento">El procedimiento almacenado.</param>
        int Mdf(string procedimiento);
        /// <summary>
        /// Llama a un procedimiento de modificación (<see cref="NivelTraza.Mdf"/>).
        /// </summary>
        /// <param name="procedimiento">El procedimiento almacenado.</param>
        /// <param name="args">Una colección de parámetros.</param>
        int Mdf(string procedimiento, ArrayList args);
        #endregion

        #region Log
        /// <summary>
        /// Crea una nueva entrada de registro basada en un elemento item.
        /// </summary>
        /// <param name="item">Encapsula información de registro.</param>
        int Log(ItemTraza item);
        #endregion

        #endregion
    }
}