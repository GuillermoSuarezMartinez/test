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
using System.Collections;
namespace Orbita.Utiles
{
    /// <summary>
    /// Queue .NET.
    /// </summary>
    public class OCola : ICola
    {
        #region Atributo(s)
        /// <summary>
        /// Colección definida, cola.
        /// </summary>
        Queue _cola;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Inicializar una nueva instancia de la clase OCola.
        /// </summary>
        public OCola()
        {
            this._cola = new Queue();
        }
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Colección definida, cola.
        /// </summary>
        protected Queue Cola
        {
            get { return this._cola; }
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Método que encola un objeto.
        /// </summary>
        /// <param name="sender">Objecto a encolar.</param>
        public virtual void Encolar(object sender)
        {
            // Bloquear la cola..
            lock (this._cola.SyncRoot)
            {
                // Encolar el objeto.
                this._cola.Enqueue(sender);
            }
        }
        /// <summary>
        /// Método que desencola un objeto.
        /// </summary>
        /// <returns>Objeto encolado.</returns>
        public virtual object Desencolar()
        {
            // Bloquear la cola.
            lock (this._cola.SyncRoot)
            {
                object objetoEncolado = null;
                if (this._cola.Count > 0)
                {
                    // Desencolar el objeto.
                    objetoEncolado = this._cola.Dequeue();
                }
                return objetoEncolado;
            }
        }
        /// <summary>
        /// Método de borrado total de la cola.
        /// </summary>
        public void Limpiar()
        {
            // Borrar el contenido de la cola.
            this._cola.Clear();
        }
        /// <summary>
        /// Método de comprobación de aparición del objeto en la cola.
        /// </summary>
        /// <param name="identificador">Idenficador del objeto encolado.</param>
        /// <returns>La existencia o no del objeto.</returns>
        public bool Existe(object identificador)
        {
            // Comprobar  si  el  objeto  existe  en la cola.
            // Devolver verdadero o falso, según sea el caso.
            return this._cola.Contains(identificador);
        }
        /// <summary>
        /// Método contador de elementos de la cola.
        /// </summary>
        /// <returns>Número de elementos de la cola.</returns>
        public int Contar()
        {
            // Método 'Count' de las colección.
            return this._cola.Count;
        }
        /// <summary>
        /// Método que obtiene sin borrarlo el primer
        /// elemento de la cola.
        /// </summary>
        /// <returns>El primer elemento de la cola.</returns>
        public object Primero()
        {
            // Método 'Peek()' de la cola.
            return this._cola.Peek();
        }
        #endregion
    }
}
