//***********************************************************************
// Assembly         : OrbitaTrazabilidad
// Author           : crodriguez
// Created          : 02-17-2011
//
// Last Modified By : crodriguez
// Last Modified On : 02-22-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System.Collections.Generic;
namespace Orbita.Trazabilidad
{
    /// <summary>
    /// Wrapper Logger.
    /// </summary>
    public class WrapperLogger : BaseLogger
    {
        #region Atributos privados
        /// <summary>
        /// Colección de loggers.
        /// </summary>
        List<ILogger> loggers;
        #endregion

        #region Constructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.WrapperLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>Puerto=1440</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="loggers">Colección de loggers.</param>
        public WrapperLogger(string identificador, params ILogger[] loggers)
            : this(identificador, NivelLog.Debug, loggers) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.WrapperLogger.
        /// Por defecto, <c>Puerto=1440</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger.</param>
        /// <param name="loggers">Colección de loggers.</param>
        public WrapperLogger(string identificador, NivelLog nivelLog, params ILogger[] loggers)
            : base(identificador, nivelLog)
        {
            // Crear la colección de loggers.
            this.loggers = new List<ILogger>(loggers);
        }
        #endregion

        #region Métodos públicos
        /// <summary>
        /// Registra un elemento determinado.
        /// </summary>
        /// <param name="item">Entrada de registro.</param>
        public override void Log(ItemLog item)
        {
            // Recorrer la colección de loggers.
            foreach (BaseLogger logger in this.loggers)
            {
                // Registrar cada item de la colección.
                logger.Log(item);
            }
        }
        /// <summary>
        /// Registra un elemento determinado con parámetros adicionales.
        /// </summary>
        /// <param name="item">Entrada de registro.</param>
        /// <param name="args">Parámetros adicionales.</param>
        public override void Log(ItemLog item, object[] args)
        {
            // Recorrer la colección de loggers.
            foreach (BaseLogger logger in this.loggers)
            {
                // Registrar cada item de la colección.
                logger.Log(item, args);
            }
        }
        #endregion
    }
}
