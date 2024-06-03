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
    /// Simple Logger.
    /// </summary>
    class SimpleLogger
    {
        #region Atributos privados
        /// <summary>
        /// Interface de logger.
        /// </summary>
        ILogger logger;
        #endregion

        #region Constructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.SimpleLogger.
        /// </summary>
        public SimpleLogger() { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.SimpleLogger.
        /// </summary>
        /// <param name="logger">Interface de logger.</param>
        public SimpleLogger(ILogger logger)
        {
            this.logger = logger;
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Interface de logger.
        /// </summary>
        public ILogger Logger
        {
            get { return this.logger; }
        }
        #endregion
    }
}
