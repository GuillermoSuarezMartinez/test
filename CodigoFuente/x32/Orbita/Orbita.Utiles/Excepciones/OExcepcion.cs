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
using System.Runtime.Serialization;
namespace Orbita.Utiles
{
    /// <summary>
    /// Clase OExcepcion.
    /// </summary>
    [Serializable]
    public class OExcepcion : Exception
    {
        #region Constructor(es)
        /// <summary>
        /// Inicializar una nueva instancia de la clase OExcepcion.
        /// </summary>
        public OExcepcion()
            : base() { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase OExcepcion.
        /// </summary>
        /// <param name="mensaje">Mensaje que describe el error.</param>
        public OExcepcion(string mensaje)
            : base(mensaje) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase OExcepcion.
        /// </summary>
        /// <param name="mensaje">Mensaje de error que explica la razón
        /// de la excepción.</param>
        /// <param name="inner">La excepción que es la causa de la excepción
        /// actual o una referencia nula si no se especifica ninguna excepción
        /// interna.</param>
        public OExcepcion(string mensaje, Exception inner)
            : base(mensaje, inner) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info">Clase System.Runtime.Serialization.SerializationInfo
        /// que contiene los datos serializados del objeto que hacen referencia a la
        /// excepción que se va a producir.</param>
        /// <param name="context">Enumeración System.Runtime.Serialization.StreamingContext
        /// que contiene información contextual sobre el origen o el destino.</param>
        protected OExcepcion(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
        #endregion
    }
}
