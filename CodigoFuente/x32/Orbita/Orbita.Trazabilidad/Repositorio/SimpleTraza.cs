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
    /// <summary>
    /// Simple Traza.
    /// </summary>
    class SimpleTraza
    {
        #region Atributos privados
        /// <summary>
        /// Interface de traza.
        /// </summary>
        ITraza traza;
        #endregion

        #region Constructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.SimpleTraza.
        /// </summary>
        public SimpleTraza() { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.SimpleTraza.
        /// </summary>
        /// <param name="traza">Interface de traza.</param>
        public SimpleTraza(ITraza traza)
        {
            this.traza = traza;
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Interface de traza.
        /// </summary>
        public ITraza Traza
        {
            get { return this.traza; }
        }
        #endregion
    }
}
