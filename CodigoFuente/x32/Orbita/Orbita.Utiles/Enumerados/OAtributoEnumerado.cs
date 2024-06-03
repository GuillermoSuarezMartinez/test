//***********************************************************************
// Assembly         : OrbitaUtiles
// Author           : vnicolau
// Created          : 03-03-2011
//
// Last Modified By : crodriguez
// Last Modified On : 05-04-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
namespace Orbita.Utiles
{
    /// <summary>
    /// Clases para dotar de atributos a los tipos enumerados.
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.All, Inherited = true)]
    public class OAtributoEnumerado : Attribute
    {
        #region Atributo(s)
        /// <summary>
        /// Valor del enumerado.
        /// </summary>
        string _valor;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Inicializar una nueva instancia de la clase OAtributoEnumerado.
        /// </summary>
        public OAtributoEnumerado() { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase OAtributoEnumerado.
        /// </summary>
        /// <param name="valor">valor.</param>
        public OAtributoEnumerado(string valor)
        {
            this._valor = valor;
        }
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Obtiene el valor.
        /// </summary>
        /// <value></value>
        public string Valor
        {
            get { return this._valor; }
        }
        #endregion
    }
}
