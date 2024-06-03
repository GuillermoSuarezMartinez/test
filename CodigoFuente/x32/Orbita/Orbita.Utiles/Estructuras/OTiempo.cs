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
using System.Diagnostics;
namespace Orbita.Utiles
{
    /// <summary>
    /// OTiempo.
    /// </summary>
    public class OTiempo : IDisposable
    {
        #region Atributo(s)
        /// <summary>
        /// Contador del tiempo de proceso.
        /// </summary>
        Stopwatch _contador;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Inicializar una nueva instancia de la clase OTiempo.
        /// </summary>
        public OTiempo()
        {
            this._contador = new Stopwatch();
        }
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
                // Finalizar correctamente los recursos no manejados.
                this._contador = null;

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
        ~OTiempo()
        {
            // Llamar a Dispose(false) es óptimo en terminos
            // de legibilidad y mantenimiento.
            Dispose(false);
        }
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Obtener el tiempo de ejecución en milisegundos.
        /// </summary>
        public long ContadorENMilisegundos
        {
            get
            {
                long resultado = 0;
                if (this._contador != null)
                {
                    resultado = this._contador.ElapsedMilliseconds;
                }
                return resultado;
            }
        }
        /// <summary>
        /// Obtener el tiempo de ejecución en segundos.
        /// </summary>
        public double ContadorENSegundos
        {
            get
            {
                double resultado = 0;
                if (this._contador != null)
                {
                    resultado = (this._contador.ElapsedMilliseconds != 0) ?
                        (this._contador.ElapsedMilliseconds / 1000.0) : 0;
                }
                return resultado;
            }
        }
        /// <summary>
        /// Obtener el tiempo de ejecución en minutos.
        /// </summary>
        public double ContadorENMinutos
        {
            get
            {
                double resultado = 0;
                if (this._contador != null)
                {
                    resultado = (this._contador.ElapsedMilliseconds != 0) ?
                        ((this._contador.ElapsedMilliseconds / 1000.0) / 60) : 0;
                }
                return resultado;
            }
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Iniciar el contador de respuesta.
        /// </summary>
        public void Iniciar()
        {
            this._contador.Start();
        }
        /// <summary>
        /// // Parar el contador de respuesta.
        /// </summary>
        public void Parar()
        {
            this._contador.Stop();
        }
        /// <summary>
        /// Resetear el contador de respuesta.
        /// </summary>
        public void Resetear()
        {
            this._contador.Reset();
        }
        /// <summary>
        /// Resetear el contador de respuesta.
        /// Iniciar el contador de respuesta.
        /// </summary>
        public void ResetearEIniciar()
        {
            Resetear();
            Iniciar();
        }
        #endregion
    }
}