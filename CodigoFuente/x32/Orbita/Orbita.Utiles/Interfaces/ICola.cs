//***********************************************************************
// Assembly         : OrbitaUtiles
// Author           : crodriguez
// Created          : 03-03-2011
//
// Last Modified By : crodriguez
// Last Modified On : 03-03-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
namespace Orbita.Utiles
{
    /// <summary>
    /// Interfaz para colas
    /// </summary>
    public interface ICola
    {
        #region Método(s)
        /// <summary>
        /// Método que encola un objeto.
        /// </summary>
        /// <param name="sender">Objeto a encolar.</param>
        void Encolar(object sender);
        /// <summary>
        /// Método que desencola un objeto.
        /// </summary>
        /// <returns>Objeto encolado.</returns>
        object Desencolar();
        #endregion
    }
}
