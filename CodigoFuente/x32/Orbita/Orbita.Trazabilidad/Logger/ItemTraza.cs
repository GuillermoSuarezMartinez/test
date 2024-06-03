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
using System;
using System.Collections;
namespace Orbita.Trazabilidad
{
    /// <summary>
    /// Item Traza.
    /// </summary>
    [Serializable]
    public class ItemTraza : IDisposable
    {
        #region Atributos privados
        /// <summary>
        /// El nivel de registro de traza.
        /// </summary>
        NivelTraza nivelTraza;
        /// <summary>
        /// Dia y hora de entrada en el registro. Si no  explicitamente esta propiedad ofrece la marca de tiempo de la creación del objeto.
        /// </summary>
        DateTime fecha;
        /// <summary>
        /// El procedimiento almacenado. Por defecto, <c>string.Empty</c>.
        /// </summary>
        string procedimiento;
        /// <summary>
        /// Identificador de evento. Por defecto, <c>null</c>.
        /// </summary>
        int? identificador;
        /// <summary>
        /// Argumentos adicionales que puede contener el item, además del mensaje propiamente dicho.
        /// </summary>
        ArrayList args;
        #endregion

        #region Constructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.ItemTraza.
        /// </summary>
        public ItemTraza()
        {
            this.fecha = DateTime.Now;
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.ItemTraza.
        /// </summary>
        /// <param name="nivelTraza">El nivel de registro de traza.</param>
        public ItemTraza(NivelTraza nivelTraza)
            : this()
        {
            this.nivelTraza = nivelTraza;
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.ItemTraza.
        /// </summary>
        /// <param name="nivelTraza">El nivel de registro de traza.</param>
        /// <param name="procedimiento">El procedimiento almacenado.</param>
        public ItemTraza(NivelTraza nivelTraza, string procedimiento)
            : this(nivelTraza)
        {
            this.procedimiento = procedimiento;
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.ItemTraza.
        /// </summary>
        /// <param name="nivelTraza">El nivel de registro de traza.</param>
        /// <param name="procedimiento">El procedimiento almacenado.</param>
        /// <param name="args">Argumentos del procedimiento almacenado.</param>
        public ItemTraza(NivelTraza nivelTraza, string procedimiento, ArrayList args)
            : this(nivelTraza, procedimiento)
        {
            this.args = args;
        }
        #endregion

        #region Destructores
        /// <summary>
        /// Indica si ya se llamo al método Dispose. (default = false)
        /// </summary>
        bool disposed = false;
        /// <summary>
        /// Implementa IDisposable.
        /// No  hacer  este  método  virtual.
        /// Una clase derivada no debería ser capaz de  reemplazar este método.
        /// </summary>
        public void Dispose()
        {
            // Llamo al método que  contiene la lógica para liberar los recursos de esta clase.
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
                // Finalizar correctamente los recursos no manejados.
                this.procedimiento = null;
                this.identificador = null;
                this.args = null;
                // Marcar como desechada ó desechandose, de forma que no se puede ejecutar el código dos veces.
                disposed = true;
            }
        }
        /// <summary>
        /// Destructor(es) de clase.
        /// En caso de que se nos olvide “desechar” la clase, el GC llamará al destructor, que tambén ejecuta la lógica anterior para liberar los recursos.
        /// </summary>
        ~ItemTraza()
        {
            // Llamar a Dispose(false) es óptimo en terminos de legibilidad y mantenimiento.
            Dispose(false);
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// El nivel de registro de traza.
        /// </summary>
        public NivelTraza NivelTraza
        {
            get { return this.nivelTraza; }
            set { this.nivelTraza = value; }
        }
        /// <summary>
        /// Día y hora de entrada en el registro. Si no  explicitamente
        /// esta propiedad ofrece la marca de tiempo de la creación del
        /// objeto.
        /// </summary>
        public DateTime Fecha
        {
            get { return this.fecha; }
            set { this.fecha = value; }
        }
        /// <summary>
        /// El procedimiento almacenado. Por defecto <c>string.Empty</c>.
        /// </summary>
        public string Procedimiento
        {
            get { return this.procedimiento; }
            set { this.procedimiento = value; }
        }
        /// <summary>
        /// Identificador de evento. Por defecto <c>null</c>.
        /// </summary>
        public int? Identificador
        {
            get { return this.identificador; }
            set { this.identificador = value; }
        }
        /// <summary>
        /// Argumentos adicionales que puede contener el item, además del mensaje propiamente dicho.
        /// </summary>
        public ArrayList Argumentos
        {
            get { return this.args; }
        }
        #endregion

        #region Métodos públicos
        /// <summary>
        /// Argumentos adicionales que puede contener el item, además del mensaje propiamente dicho.
        /// </summary>
        public void SetArgumentos(ArrayList args)
        {
            this.args = args;
        }
        #endregion
    }
}