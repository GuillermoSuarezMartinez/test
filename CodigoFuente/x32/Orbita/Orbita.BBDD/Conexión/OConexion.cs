//***********************************************************************
// Assembly         : Orbita.BBDD
// Author           : crodriguez
// Created          : 02-17-2011
//
// Last Modified By : crodriguez
// Last Modified On : 02-18-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Data.SqlClient;
namespace Orbita.BBDD
{
    /// <summary>
    /// Clase que almacena los datos necesarios útiles para el uso concatenado de transacciones.
    /// </summary>
    public class OConexion : IDisposable
    {
        #region Atributos privados
        /// <summary>
        /// Identificador de transacción.
        /// </summary>
        Guid identificador;
        /// <summary>
        /// Conexión de base de datos.
        /// </summary>
        SqlConnection conexion;
        /// <summary>
        /// Transacción de base de datos.
        /// </summary>
        SqlTransaction transaccion;
        #endregion

        #region Constructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase OConexion.
        /// </summary>
        /// <param name="infoConexion">Información de la conexión.</param>
        public OConexion(string infoConexion)
        {
            // Crear la conexión.
            this.conexion = new SqlConnection(infoConexion);
        }
        #endregion

        #region Destructores
        /// <summary>
        /// Indica si ya se llamo al método Dispose. (Default = false)
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
                if (disposing)
                {
                    // Llamar a dispose de todos los recursos manejados.
                    this.conexion.Dispose();
                    this.transaccion.Dispose();
                }

                // Finalizar correctamente los recursos no manejados.
                this.identificador = Guid.Empty;

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
        ~OConexion()
        {
            // Llamar a Dispose(false) es óptimo en terminos de legibilidad y mantenimiento.
            Dispose(false);
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Identificador de transacción.
        /// </summary>
        public Guid Identificador
        {
            get { return this.identificador; }
            set { this.identificador = value; }
        }
        /// <summary>
        /// Conexión de base de datos.
        /// </summary>
        public SqlConnection Conexion
        {
            get { return this.conexion; }
            set { this.conexion = value; }
        }
        /// <summary>
        /// Transacción de base de datos.
        /// </summary>
        public SqlTransaction Transaccion
        {
            get { return this.transaccion; }
            set { this.transaccion = value; }
        }
        #endregion

        #region Métodos públicos
        /// <summary>
        /// Iniciar transacción.
        /// </summary>
        public string Iniciar()
        {
            if (this.conexion.State == System.Data.ConnectionState.Closed)
            {
                // Abrir la conexión.
                this.conexion.Open();
                // Iniciar y asignar la transacción.
                this.transaccion = this.conexion.BeginTransaction();
            }
            return this.identificador.ToString();
        }
        /// <summary>
        /// Iniciar transacción con un nivel de bloqueo específico.
        /// </summary>
        public string Iniciar(System.Data.IsolationLevel iso)
        {
            if (this.conexion.State == System.Data.ConnectionState.Closed)
            {
                // Abrir la conexión.
                this.conexion.Open();
                // Iniciar y asignar la transacción con un nivel de bloque específico.
                this.transaccion = this.conexion.BeginTransaction(iso);
            }
            return this.identificador.ToString();
        }
        #endregion
    }
}