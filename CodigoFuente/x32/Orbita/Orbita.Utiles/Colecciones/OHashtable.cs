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
using System.Collections;
using System.Runtime.Serialization;
namespace Orbita.Utiles
{
    /// <summary>
    /// Colección de que representa una tabla de dispersión (Hash).
    /// </summary>
    [Serializable]
    public class OHashtable : Hashtable, IDisposable
    {
        #region Atributo(s)
        #region Evento(s)
        /// <summary>
        /// Evento que se ejecuta tras añadir un objeto a la colección.
        /// </summary>
        public event ManejadorEvento OnDespuesAdicionar;
        /// <summary>
        /// Evento que se ejecuta tras eliminar un objeto de la colección.
        /// </summary>
        public event ManejadorEvento OnDespuesEliminar;
        #endregion
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Inicializar una nueva instancia de la clase OHashtable.
        /// </summary>
        public OHashtable()
            :base() { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase OHashtable.
        /// </summary>
        /// <param name="info">Objeto System.Runtime.Serialization.SerializationInfo 
        /// que contiene la información que se requiere para serializar el objeto
        /// System.Collections.Hashtable.</param>
        /// <param name="context">Objeto System.Runtime.Serialization.StreamingContext
        /// que contiene el origen y el destino de la secuencia serializada asociada a 
        /// System.Collections.Hashtable.</param>
        protected OHashtable(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase OHashtable.
        /// </summary>
        /// <param name="table">Colección hashtable.</param>
        OHashtable(Hashtable table)
            :base(table) { }
        #endregion

        #region Destructor(es)
        /// <summary>
        /// Indica si ya se llamo al método Dispose. (default = false)
        /// </summary>
        bool disposed = false;
        /// <summary>
        /// Implementa IDisposable.
        /// No  hacer  este  método  virtual.
        /// Una clase derivada no debería ser
        /// capaz de  reemplazar este método.
        /// </summary>
        public void Dispose()
        {
            // Llamo al método que  contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
            // Este objeto será limpiado por el método Dispose.
            // Llama al método del recolector de basura, GC.SuppressFinalize.
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Método  sobrecargado de  Dispose que será  el que
        /// libera los recursos. Controla que solo se ejecute
        /// dicha lógica una  vez y evita que el GC tenga que
        /// llamar al destructor de clase.
        /// </summary>
        /// <param name="disposing">Indica si llama al método Dispose.</param>
        protected virtual void Dispose(bool disposing)
        {
            // Preguntar si Dispose ya fue llamado.
            if (!this.disposed)
            {
                // Marcar como desechada ó desechandose,
                // de forma que no se puede ejecutar el
                // código dos veces.
                disposed = true;
            }
        }
        /// <summary>
        /// Destructor(es) de clase.
        /// En caso de que se nos olvide “desechar” la clase,
        /// el GC llamará al destructor, que tambén ejecuta 
        /// la lógica anterior para liberar los recursos.
        /// </summary>
        ~OHashtable()
        {
            // Llamar a Dispose(false) es óptimo en terminos
            // de legibilidad y mantenimiento.
            Dispose(false);
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Agrega un elemento con  la clave y el  valor 
        /// especificados a System.Collections.Hashtable.
        /// </summary>
        /// <param name="key">Clave del elemento que se va a agregar.</param>
        /// <param name="value">Valor del elemento que se va a agregar. 
        /// El valor puede ser null.</param>
        public override void Add(object key, object value)
        {
            // Añadir un nuevo elemento a la colección.
            //
            // Bloquear este método por si se usan Threads.
            lock (this)
            {
                if (!Existe(key))
                {
                    base.Add(key, value);

                    // En C# debemos comprobar que el evento no sea null.
                    if (OnDespuesAdicionar != null)
                    {
                        // El evento se lanza como cualquier delegado.
                        OnDespuesAdicionar(this, new OEventArgs(key));
                    }
                }
            }
        }
        /// <summary>
        /// Método de borrado del elemento en la colección.
        /// </summary>
        /// <param name="clave">Clave.</param>
        public virtual void Eliminar(object clave)
        {
            // Añadir un nuevo elemento a la colección.
            //
            // Bloquear este método por si se usan Threads.
            lock (this)
            {
                if (Existe(clave))
                {
                    base.Remove(clave);

                    // En C# debemos comprobar que el evento no sea null.
                    if (OnDespuesEliminar != null)
                    {
                        // El evento se lanza como cualquier delegado.
                        OnDespuesEliminar(this, new OEventArgs(clave));
                    }
                }
            }
        }
        /// <summary>
        /// Método contador de elementos de la colección.
        /// </summary>
        /// <returns>Número de elementos de la colección.</returns>
        public int Contar()
        {
            // Método 'Count' de la colección.
            return base.Count;
        }
        /// <summary>
        /// Método de comprobación de aparición del objeto en la colección.
        /// </summary>
        /// <param name="clave">Identificar de key en object.</param>
        /// <returns>La existencia o no del objeto.</returns>
        public bool Existe(object clave)
        {
            // Comprobar si el ID indicado existe en la colección.
            // Devolver verdadero o falso, según sea el caso.
            return base.ContainsKey(clave);
        }
        /// <summary>
        /// Método de borrado total de la colección.
        /// </summary>
        public void Limpiar()
        {
            // Borrar el contenido de la colección.
            base.Clear();
        }
        /// <summary>
        /// Clonar una copia superficial del objeto.
        /// </summary>
        /// <returns>Una nueva colección clonada.</returns>
        public virtual object Clonar()
        {
            return new OHashtable(this);
        }
        /// <summary>
        /// Copia los elementos de la interfaz System.Collections.ICollection en un objeto
        ///  System.Array, a partir de un índice determinado de la clase System.Array.
        /// </summary>
        /// <param name="array">Objeto System.Array unidimensional que constituye el destino 
        /// de los elementos copiados de la interfaz System.Collections.ICollection. 
        /// System.Array debe tener una indización de base cero.</param>
        /// <param name="indice">Índice de base cero de array donde comienza la copia.</param>
        public void CopiarA(Exception[] array, int indice)
        {
            this.CopyTo(array, indice);
        }
        /// <summary>
        /// Implementa la interfaz de System.Runtime.Serialization.ISerializable y devuelve
        ///  los datos necesarios para serializar System.Collections.Hashtable.
        /// </summary>
        /// <param name="info">Objeto System.Runtime.Serialization.SerializationInfo que contiene 
        /// la información que se requiere para serializar System.Collections.Hashtable.</param>
        /// <param name="context">Objeto System.Runtime.Serialization.StreamingContext que contiene 
        /// el origeny el destino de la secuencia serializada asociada a System.Collections.Hashtable.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
        #endregion

        #region Método(s) privado(es)
        /// <summary>
        /// Copia los elementos de la interfaz System.Collections.ICollection en un objeto
        ///  System.Array, a partir de un índice determinado de la clase System.Array.
        /// </summary>
        /// <param name="array">Objeto System.Array unidimensional que constituye el destino 
        /// de los elementos copiados de la interfaz System.Collections.ICollection. 
        /// System.Array debe tener una indización de base cero.</param>
        /// <param name="indice">Índice de base cero de array donde comienza la copia.</param>
        void CopyTo(Exception[] array, int indice)
        {
            base.CopyTo(array, indice);
        }
        #endregion
    }
}
