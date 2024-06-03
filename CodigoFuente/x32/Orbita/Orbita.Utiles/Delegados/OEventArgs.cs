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
using System;
namespace Orbita.Utiles
{
    /// <summary>
    /// Clase que representa eventos con argumentos
    /// adicionales tipados (EventArgs).
    /// </summary>
    [Serializable]
    public class OEventArgs : EventArgs
    {
        #region Atributo(s)
        /// <summary>
        /// Argumento adicional desarrollado en el evento.
        /// </summary>
        object _arg;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Inicializar una nueva instancia de la clase OEventArgs.
        /// </summary>
        public OEventArgs() { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase OEventArgs.
        /// </summary>
        /// <param name="arg">Argumento adicional.</param>
        public OEventArgs(object arg)
        {
            this._arg = arg;
        }
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Argumento adicional desarrollado en el evento.
        /// </summary>
        public object Argumento
        {
            get { return this._arg; }
            set { this._arg = value; }
        }
        #endregion
    }
}
