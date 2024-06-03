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
using System.Diagnostics.CodeAnalysis;
namespace Orbita.Trazabilidad
{
    /// <summary>
    /// ILogger.
    /// </summary>
    public interface ILogger
    {
        #region Propiedades privadas
        /// <summary>
        /// Identificador de logger.
        /// </summary>
        string Identificador { get; }
        #endregion

        #region Métodos privados

        #region Debug
        /// <summary>
        /// Registra un objeto arbitrario con el <see cref="NivelLog.Debug"/>. 
        /// </summary>
        /// <param name="objeto">El objeto mensaje a registrar, el cual será convertido en un string durante el registro.</param>
        void Debug(object objeto);
        /// <summary>
        /// Registra un mensaje de depuración (<see cref="NivelLog.Debug"/>).
        /// </summary>
        /// <param name="mensaje">El mensaje a registrar.</param>
        void Debug(string mensaje);
        /// <summary>
        /// Registra un mensaje formateado en string con el nivel <see cref="NivelLog.Debug"/>. 
        /// </summary>
        /// <param name="mensaje">Una cadena con mensaje que contiene marcadores de posición.</param>
        /// <param name="args">Un <see cref="object"/> array que contiene cero o mas objetos.</param>
        void Debug(string mensaje, params object[] args);
        /// <summary>
        /// Registra una excepción con el <see cref="NivelLog.Debug"/>.
        /// </summary>
        /// <param name="excepcion">La excepción a registrar.</param>
        void Debug(Exception excepcion);
        /// <summary>
        /// Registra una excepción y un mensaje adicional con el <see cref="NivelLog.Debug"/>.
        /// </summary>
        /// <param name="excepcion"> La excepción a registrar.</param>
        /// <param name="mensaje">Información adicional con respecto a la excepción a registrar.</param>
        void Debug(Exception excepcion, string mensaje);
        /// <summary>
        /// Registra un mensaje formateado en string con el nivel <see cref="NivelLog.Debug"/>. 
        /// </summary>
        /// <param name="excepcion">La excepción a registrar. </param>
        /// <param name="mensaje">Una cadena con mensaje que contiene marcadores de posición.</param>
        /// <param name="args">Un <see cref="object"/> array que contiene cero o mas objetos.</param>
        void Debug(Exception excepcion, string mensaje, params object[] args);
        #endregion

        #region Info
        /// <summary>
        /// Registra un objeto arbitrario con el <see cref="NivelLog.Info"/>. 
        /// </summary>
        /// <param name="objeto">El objeto mensaje a registrar, el cual será convertido en un string durante el registro.</param>
        void Info(object objeto);
        /// <summary>
        /// Registra un mensaje de información (<see cref="NivelLog.Info"/>).
        /// </summary>
        /// <param name="mensaje">El mensaje a registrar.</param>
        void Info(string mensaje);
        /// <summary>
        /// Registra un mensaje formateado en string con el nivel <see cref="NivelLog.Info"/>. 
        /// </summary>
        /// <param name="mensaje">Una cadena con mensaje que contiene marcadores de posición.</param>
        /// <param name="args">Un <see cref="object"/> array que contiene cero o mas objetos.</param>
        void Info(string mensaje, params object[] args);
        /// <summary>
        /// Registra una excepción con el <see cref="NivelLog.Info"/>.
        /// </summary>
        /// <param name="excepcion">La excepción a registrar.</param>
        void Info(Exception excepcion);
        /// <summary>
        /// Registra una excepción y un mensaje adicional con el <see cref="NivelLog.Info"/>.
        /// </summary>
        /// <param name="excepcion"> La excepción a registrar.</param>
        /// <param name="mensaje">Información adicional con respecto a la excepción a registrar.</param>
        void Info(Exception excepcion, string mensaje);
        /// <summary>
        /// Registra un mensaje formateado en string con el nivel <see cref="NivelLog.Info"/>. 
        /// </summary>
        /// <param name="excepcion">La excepción a registrar.</param>
        /// <param name="mensaje">Una cadena con mensaje que contiene marcadores de posición.</param>
        /// <param name="args">Un <see cref="object"/> array que contiene cero o mas objetos.</param>
        void Info(Exception excepcion, string mensaje, params object[] args);
        #endregion

        #region Warn
        /// <summary>
        /// Registra un objeto arbitrario con el <see cref="NivelLog.Warn"/>. 
        /// </summary>
        /// <param name="objeto">El objeto mensaje a registrar, el cual será convertido en un string durante el registro.</param>
        void Warn(object objeto);
        /// <summary>
        /// Registra un mensaje Warn (<see cref="NivelLog.Warn"/>).
        /// </summary>
        /// <param name="mensaje">El mensaje a registrar.</param>
        void Warn(string mensaje);
        /// <summary>
        /// Registra un mensaje formateado en string con el nivel <see cref="NivelLog.Warn"/>. 
        /// </summary>
        /// <param name="mensaje">Una cadena con mensaje que contiene marcadores de posición.</param>
        /// <param name="args">Un <see cref="object"/> array que contiene cero o mas objetos.</param>
        void Warn(string mensaje, params object[] args);
        /// <summary>
        /// Registra una excepción con el <see cref="NivelLog.Warn"/>.
        /// </summary>
        /// <param name="excepcion">La excepción a registrar.</param>
        void Warn(Exception excepcion);
        /// <summary>
        /// Registra una excepción y un mensaje adicional con el <see cref="NivelLog.Warn"/>.
        /// </summary>
        /// <param name="excepcion"> La excepción a registrar.</param>
        /// <param name="mensaje">Información adicional con respecto a la excepción a registrar.</param>
        void Warn(Exception excepcion, string mensaje);
        /// <summary>
        /// Registra un mensaje formateado en string con el nivel <see cref="NivelLog.Warn"/>. 
        /// </summary>
        /// <param name="excepcion">La excepción a registrar. </param>
        /// <param name="mensaje">Una cadena con mensaje que contiene marcadores de posición.</param>
        /// <param name="args">Un <see cref="object"/> array que contiene cero o mas objetos.</param>
        void Warn(Exception excepcion, string mensaje, params object[] args);
        #endregion

        #region Error
        /// <summary>
        /// Registra un objeto arbitrario con el <see cref="NivelLog.Error"/>. 
        /// </summary>
        /// <param name="objeto">El objeto mensaje a registrar, el cual será convertido en un string durante el registro.</param>
        [SuppressMessageAttribute("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords",
         Justification = "Error necesario para identificar este nivel de log.")]
        void Error(object objeto);
        /// <summary>
        /// Registra un mensaje de error (<see cref="NivelLog.Error"/>).
        /// </summary>
        /// <param name="mensaje">El mensaje a registrar.</param>
        [SuppressMessageAttribute("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords",
         Justification = "Error necesario para identificar este nivel de log.")]
        void Error(string mensaje);
        /// <summary>
        /// Registra un mensaje formateado en string con el nivel <see cref="NivelLog.Error"/>. 
        /// </summary>
        /// <param name="mensaje">Una cadena con mensaje que contiene marcadores de posición.</param>
        /// <param name="args">Un <see cref="object"/> array que contiene cero o mas objetos.</param>
        [SuppressMessageAttribute("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords",
         Justification = "Error necesario para identificar este nivel de log.")]
        void Error(string mensaje, params object[] args);
        /// <summary>
        /// Registra una excepción con el <see cref="NivelLog.Error"/>.
        /// </summary>
        /// <param name="excepcion">La excepción a registrar.</param>
        [SuppressMessageAttribute("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords",
         Justification = "Error necesario para identificar este nivel de log.")]
        void Error(Exception excepcion);
        /// <summary>
        /// Registra una excepción y un mensaje adicional con el <see cref="NivelLog.Error"/>.
        /// </summary>
        /// <param name="excepcion"> La excepción a registrar.</param>
        /// <param name="mensaje">Información adicional con respecto a la excepción a registrar.</param>
        [SuppressMessageAttribute("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords",
         Justification = "Error necesario para identificar este nivel de log.")]
        void Error(Exception excepcion, string mensaje);
        /// <summary>
        /// Registra un mensaje formateado en string con el nivel <see cref="NivelLog.Error"/>. 
        /// </summary>
        /// <param name="excepcion">La excepción a registrar.</param>
        /// <param name="mensaje">Una cadena con mensaje que contiene marcadores de posición.</param>
        /// <param name="args">Un <see cref="object"/> array que contiene cero o mas objetos.</param>
        [SuppressMessageAttribute("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords",
         Justification = "Error necesario para identificar este nivel de log.")]
        void Error(Exception excepcion, string mensaje, params object[] args);
        #endregion

        #region Fatal
        /// <summary>
        /// Registra un objeto arbitrario con el <see cref="NivelLog.Fatal"/>. 
        /// </summary>
        /// <param name="objeto">El objeto mensaje a registrar, el cual será convertido en un string durante el registro.</param>
        void Fatal(object objeto);
        /// <summary>
        /// Registra un mensaje de error fatal (<see cref="NivelLog.Fatal"/>).
        /// </summary>
        /// <param name="mensaje">El mensaje a registrar.</param>
        void Fatal(string mensaje);
        /// <summary>
        /// Registra un mensaje formateado en string con el nivel <see cref="NivelLog.Fatal"/>. 
        /// </summary>
        /// <param name="mensaje">Una cadena con mensaje que contiene marcadores de posición.</param>
        /// <param name="args">Un <see cref="object"/> array que contiene cero o mas objetos.</param>
        void Fatal(string mensaje, params object[] args);
        /// <summary>
        /// Registra una excepción con el <see cref="NivelLog.Fatal"/>.
        /// </summary>
        /// <param name="excepcion">La excepción a registrar.</param>
        void Fatal(Exception excepcion);
        /// <summary>
        /// Registra una excepción y un mensaje adicional con el <see cref="NivelLog.Fatal"/>.
        /// </summary>
        /// <param name="excepcion"> La excepción a registrar.</param>
        /// <param name="mensaje">Información adicional con respecto a la excepción a registrar.</param>
        void Fatal(Exception excepcion, string mensaje);
        /// <summary>
        /// Registra un mensaje formateado en string con el nivel <see cref="NivelLog.Fatal"/>. 
        /// </summary>
        /// <param name="excepcion">La excepción a registrar.</param>
        /// <param name="mensaje">Una cadena con mensaje que contiene marcadores de posición.</param>
        /// <param name="args">Un <see cref="object"/> array que contiene cero o mas objetos.</param>
        void Fatal(Exception excepcion, string mensaje, params object[] args);
        #endregion

        #region Log
        /// <summary>
        /// Crea una nueva entrada de registro basada en un elemento item.
        /// </summary>
        /// <param name="item">Encapsula información de registro.</param>
        void Log(ItemLog item);
        /// <summary>
        /// Crea una nueva entrada de registro basada en un elemento item.
        /// </summary>
        /// <param name="item">Encapsula información de registro.</param>
        /// <param name="args">Argumentos adicionales.</param>
        void Log(ItemLog item, object[] args);
        #endregion

        #endregion
    }
}
