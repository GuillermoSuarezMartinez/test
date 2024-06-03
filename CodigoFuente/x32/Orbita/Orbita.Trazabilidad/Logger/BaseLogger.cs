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
    /// Base Logger.
    /// </summary>
    public abstract class BaseLogger : ILogger
    {
        #region Atributos privados
        /// <summary>
        /// Identificador de logger.
        /// </summary>
        string identificador;
        /// <summary>
        /// Nivel de logger.
        /// </summary>
        NivelLog nivelLog;
        #endregion

        #region Constructores protegidos
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.BaseLogger.
        /// </summary>
        protected BaseLogger() { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.BaseLogger.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger.</param>
        protected BaseLogger(string identificador, NivelLog nivelLog)
        {
            this.identificador = identificador;
            this.nivelLog = nivelLog;
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Identificador de logger.
        /// </summary>
        public string Identificador
        {
            get { return this.identificador; }
            set { this.identificador = value; }
        }
        /// <summary>
        /// Nivel de logger.
        /// </summary>
        public NivelLog NivelLog
        {
            get { return this.nivelLog; }
            set { this.nivelLog = value; }
        }
        #endregion

        #region Métodos públicos

        #region Debug

        /// <summary>
        /// Logger de tipo debug.
        /// </summary>
        /// <param name="objeto">Objeto que encapsula un mensaje.</param>
        public void Debug(object objeto)
        {
            if (ESNivelLogEnabled(NivelLog.Debug))
            {
                Log(NivelLog.Debug, objeto != null ? objeto.ToString() : Orbita.Trazabilidad.E.Nulo);
            }
        }
        /// <summary>
        /// Logger de tipo debug.
        /// </summary>
        /// <param name="mensaje">Mensaje de logger.</param>
        public void Debug(string mensaje)
        {
            if (ESNivelLogEnabled(NivelLog.Debug))
            {
                Log(NivelLog.Debug, mensaje);
            }
        }
        /// <summary>
        /// Logger de tipo debug.
        /// </summary>
        /// <param name="mensaje">Mensaje de logger.</param>
        /// <param name="args">Argumentos de inclusión.</param>
        public void Debug(string mensaje, params object[] args)
        {
            if (ESNivelLogEnabled(NivelLog.Debug))
            {
                Log(NivelLog.Debug, mensaje, args);
            }
        }
        /// <summary>
        /// Logger de tipo debug.
        /// </summary>
        /// <param name="excepcion">Excepción.</param>
        public void Debug(Exception excepcion)
        {
            if (ESNivelLogEnabled(NivelLog.Debug))
            {
                Log(NivelLog.Debug, excepcion);
            }
        }
        /// <summary>
        /// Logger de tipo debug.
        /// </summary>
        /// <param name="excepcion">Excepción.</param>
        /// <param name="mensaje">Mensaje de logger.</param>
        public void Debug(Exception excepcion, string mensaje)
        {
            if (ESNivelLogEnabled(NivelLog.Debug))
            {
                Log(NivelLog.Debug, excepcion, mensaje);
            }
        }
        /// <summary>
        /// Logger de tipo debug.
        /// </summary>
        /// <param name="excepcion">Excepción.</param>
        /// <param name="mensaje">Mensaje de logger.</param>
        /// <param name="args">Argumentos de inclusión.</param>
        public void Debug(Exception excepcion, string mensaje, params object[] args)
        {
            if (ESNivelLogEnabled(NivelLog.Debug))
            {
                Log(NivelLog.Debug, excepcion, mensaje, args);
            }
        }

        #endregion

        #region Info

        /// <summary>
        /// Logger de tipo info.
        /// </summary>
        /// <param name="objeto">Objeto que encapsula un mensaje.</param>
        public void Info(object objeto)
        {
            if (ESNivelLogEnabled(NivelLog.Info))
            {
                Log(NivelLog.Info, objeto != null ? objeto.ToString() : Orbita.Trazabilidad.E.Nulo);
            }
        }
        /// <summary>
        /// Logger de tipo info.
        /// </summary>
        /// <param name="mensaje">Mensaje de logger.</param>
        public void Info(string mensaje)
        {
            if (ESNivelLogEnabled(NivelLog.Info))
            {
                Log(NivelLog.Info, mensaje);
            }
        }
        /// <summary>
        /// Logger de tipo info.
        /// </summary>
        /// <param name="mensaje">Mensaje de logger.</param>
        /// <param name="args">Argumentos de inclusión.</param>
        public void Info(string mensaje, params object[] args)
        {
            if (ESNivelLogEnabled(NivelLog.Info))
            {
                Log(NivelLog.Info, mensaje, args);
            }
        }
        /// <summary>
        /// Logger de tipo info.
        /// </summary>
        /// <param name="excepcion">Excepción de inclusión.</param>
        public void Info(Exception excepcion)
        {
            if (ESNivelLogEnabled(NivelLog.Info))
            {
                Log(NivelLog.Info, excepcion);
            }
        }
        /// <summary>
        /// Logger de tipo info.
        /// </summary>
        /// <param name="excepcion">Excepción de inclusión.</param>
        /// <param name="mensaje">Mensaje de logger.</param>
        public void Info(Exception excepcion, string mensaje)
        {
            if (ESNivelLogEnabled(NivelLog.Info))
            {
                Log(NivelLog.Info, excepcion, mensaje);
            }
        }
        /// <summary>
        /// Logger de tipo info.
        /// </summary>
        /// <param name="excepcion"></param>
        /// <param name="mensaje">Mensaje de logger.</param>
        /// <param name="args">Argumentos de inclusión.</param>
        public void Info(Exception excepcion, string mensaje, params object[] args)
        {
            if (ESNivelLogEnabled(NivelLog.Info))
            {
                Log(NivelLog.Info, excepcion, mensaje, args);
            }
        }

        #endregion

        #region Warn

        /// <summary>
        /// Logger de tipo Warn.
        /// </summary>
        /// <param name="objeto">Objeto que encapsula un mensaje.</param>
        public void Warn(object objeto)
        {
            if (ESNivelLogEnabled(NivelLog.Warn))
            {
                Log(NivelLog.Warn, objeto != null ? objeto.ToString() : Orbita.Trazabilidad.E.Nulo);
            }
        }
        /// <summary>
        /// Logger de tipo Warn.
        /// </summary>
        /// <param name="mensaje">Mensaje de logger.</param>
        public void Warn(string mensaje)
        {
            if (ESNivelLogEnabled(NivelLog.Warn))
            {
                Log(NivelLog.Warn, mensaje);
            }
        }
        /// <summary>
        /// Logger de tipo Warn.
        /// </summary>
        /// <param name="mensaje">Mensaje de logger.</param>
        /// <param name="args">Argumentos de inclusión.</param>
        public void Warn(string mensaje, params object[] args)
        {
            if (ESNivelLogEnabled(NivelLog.Warn))
            {
                Log(NivelLog.Warn, mensaje, args);
            }
        }
        /// <summary>
        /// Logger de tipo Warn.
        /// </summary>
        /// <param name="excepcion">Excepción de inclusión.</param>
        public void Warn(Exception excepcion)
        {
            if (ESNivelLogEnabled(NivelLog.Warn))
            {
                Log(NivelLog.Warn, excepcion);
            }
        }
        /// <summary>
        /// Logger de tipo Warn.
        /// </summary>
        /// <param name="excepcion">Excepción de inclusión.</param>
        /// <param name="mensaje">Mensaje de logger.</param>
        public void Warn(Exception excepcion, string mensaje)
        {
            if (ESNivelLogEnabled(NivelLog.Warn))
            {
                Log(NivelLog.Warn, excepcion, mensaje);
            }
        }
        /// <summary>
        /// Logger de tipo Warn.
        /// </summary>
        /// <param name="excepcion">Excepción de inclusión.</param>
        /// <param name="mensaje">Mensaje de logger.</param>
        /// <param name="args">Argumentos de inclusión.</param>
        public void Warn(Exception excepcion, string mensaje, params object[] args)
        {
            if (ESNivelLogEnabled(NivelLog.Warn))
            {
                Log(NivelLog.Warn, excepcion, mensaje, args);
            }
        }

        #endregion

        #region Error

        /// <summary>
        /// Logger de tipo error.
        /// </summary>
        /// <param name="objeto">Objeto que encapsula un mensaje.</param>
        public void Error(object objeto)
        {
            if (ESNivelLogEnabled(NivelLog.Error))
            {
                Log(NivelLog.Error, objeto != null ? objeto.ToString() : Orbita.Trazabilidad.E.Nulo);
            }
        }
        /// <summary>
        /// Logger de tipo error.
        /// </summary>
        /// <param name="mensaje">Mensaje de logger.</param>
        public void Error(string mensaje)
        {
            if (ESNivelLogEnabled(NivelLog.Error))
            {
                Log(NivelLog.Error, mensaje);
            }
        }
        /// <summary>
        /// Logger de tipo error.
        /// </summary>
        /// <param name="mensaje">Mensaje de logger.</param>
        /// <param name="args">Argumentos de inclusión.</param>
        public void Error(string mensaje, params object[] args)
        {
            if (ESNivelLogEnabled(NivelLog.Error))
            {
                Log(NivelLog.Error, mensaje, args);
            }
        }
        /// <summary>
        /// Logger de tipo error.
        /// </summary>
        /// <param name="excepcion">Excepción de inclusión.</param>
        public void Error(Exception excepcion)
        {
            if (ESNivelLogEnabled(NivelLog.Error))
            {
                Log(NivelLog.Error, excepcion);
            }
        }
        /// <summary>
        /// Logger de tipo error.
        /// </summary>
        /// <param name="excepcion">Excepción de inclusión.</param>
        /// <param name="mensaje">Mensaje de logger.</param>
        public void Error(Exception excepcion, string mensaje)
        {
            if (ESNivelLogEnabled(NivelLog.Error))
            {
                Log(NivelLog.Error, excepcion, mensaje);
            }
        }
        /// <summary>
        /// Logger de tipo error.
        /// </summary>
        /// <param name="excepcion">Excepción de inclusión.</param>
        /// <param name="mensaje">Mensaje de logger.</param>
        /// <param name="args">Argumentos de inclusión.</param>
        public void Error(Exception excepcion, string mensaje, params object[] args)
        {
            if (ESNivelLogEnabled(NivelLog.Error))
            {
                Log(NivelLog.Error, excepcion, mensaje, args);
            }
        }

        #endregion

        #region Fatal

        /// <summary>
        /// Logger de tipo error fatal.
        /// </summary>
        /// <param name="objeto">Objeto que encapsula un mensaje.</param>
        public void Fatal(object objeto)
        {
            if (ESNivelLogEnabled(NivelLog.Fatal))
            {
                Log(NivelLog.Fatal, objeto != null ? objeto.ToString() : Orbita.Trazabilidad.E.Nulo);
            }
        }
        /// <summary>
        /// Logger de tipo error fatal.
        /// </summary>
        /// <param name="mensaje">Mensaje de logger.</param>
        public void Fatal(string mensaje)
        {
            if (ESNivelLogEnabled(NivelLog.Fatal))
            {
                Log(NivelLog.Fatal, mensaje);
            }
        }
        /// <summary>
        /// Logger de tipo error fatal.
        /// </summary>
        /// <param name="mensaje">Mensaje de logger.</param>
        /// <param name="args">Argumentos de inclusión.</param>
        public void Fatal(string mensaje, params object[] args)
        {
            if (ESNivelLogEnabled(NivelLog.Fatal))
            {
                Log(NivelLog.Fatal, mensaje, args);
            }
        }
        /// <summary>
        /// Logger de tipo error fatal.
        /// </summary>
        /// <param name="excepcion">Excepción de inclusión.</param>
        public void Fatal(Exception excepcion)
        {
            if (ESNivelLogEnabled(NivelLog.Fatal))
            {
                Log(NivelLog.Fatal, excepcion);
            }
        }
        /// <summary>
        /// Logger de tipo error fatal.
        /// </summary>
        /// <param name="excepcion">Excepción de inclusión.</param>
        /// <param name="mensaje">Mensaje de logger.</param>
        public void Fatal(Exception excepcion, string mensaje)
        {
            if (ESNivelLogEnabled(NivelLog.Fatal))
            {
                Log(NivelLog.Fatal, excepcion, mensaje);
            }
        }
        /// <summary>
        /// Logger de tipo error fatal.
        /// </summary>
        /// <param name="excepcion">Excepción de inclusión.</param>
        /// <param name="mensaje">Mensaje de logger.</param>
        /// <param name="args">Argumentos de inclusión.</param>
        public void Fatal(Exception excepcion, string mensaje, params object[] args)
        {
            if (ESNivelLogEnabled(NivelLog.Fatal))
            {
                Log(NivelLog.Fatal, excepcion, mensaje, args);
            }
        }

        #endregion

        #region Log

        /// <summary>
        /// Crea una entrada de registro nuevo basado en un elemento de inicio de sesión dado.
        /// </summary>
        /// <param name="item">Encapsula la información de registro.</param>
        public abstract void Log(ItemLog item);
        /// <summary>
        /// Crea una entrada de registro nuevo basado en un elemento de inicio de sesión dado.
        /// </summary>
        /// <param name="item">Encapsula la información de registro.</param>
        /// <param name="args">Argumentos adicionales.</param>
        public abstract void Log(ItemLog item, object[] args);
        /// <summary>
        /// Crea una entrada de registro nuevo basado en un elemento de inicio de sesión dado.
        /// </summary>
        /// <param name="nivel">Nivel de logger.</param>
        /// <param name="mensaje">Mensaje de logger.</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        public virtual void Log(NivelLog nivel, string mensaje)
        {
            ItemLog item = new ItemLog(nivel, mensaje);
            Log(item);
        }
        /// <summary>
        /// Crea una entrada de registro nuevo basado en un elemento de inicio de sesión dado.
        /// </summary>
        /// <param name="nivel">Nivel de logger.</param>
        /// <param name="mensaje">Mensaje de logger.</param>
        /// <param name="args">Argumentos de inclusión.</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        public virtual void Log(NivelLog nivel, string mensaje, params object[] args)
        {
            ItemLog item = new ItemLog(nivel, mensaje);
            item.SetArgumentos(args);
            Log(item, args);
        }
        /// <summary>
        /// Crea una entrada de registro nuevo basado en un elemento de inicio de sesión dado.
        /// </summary>
        /// <param name="nivel">Nivel de logger.</param>
        /// <param name="excepcion">Excepción de inclusión.</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        public virtual void Log(NivelLog nivel, Exception excepcion)
        {
            ItemLog item = new ItemLog(nivel, excepcion);
            Log(item);
        }
        /// <summary>
        /// Crea una entrada de registro nuevo basado en un elemento de inicio de sesión dado.
        /// </summary>
        /// <param name="nivel">Nivel de logger.</param>
        /// <param name="excepcion">Excepción de inclusión.</param>
        /// <param name="mensaje">Mensaje de logger.</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        public virtual void Log(NivelLog nivel, Exception excepcion, string mensaje)
        {
            ItemLog item = new ItemLog(nivel, excepcion, mensaje);
            Log(item);
        }
        /// <summary>
        /// Crea una entrada de registro nuevo basado en un elemento de inicio de sesión dado.
        /// </summary>
        /// <param name="nivel">Nivel de logger.</param>
        /// <param name="excepcion">Excepción de inclusión.</param>
        /// <param name="mensaje">Mensaje de logger.</param>
        /// <param name="args">Argumentos de inclusión.</param>
        [SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        public virtual void Log(NivelLog nivel, Exception excepcion, string mensaje, params object[] args)
        {
            ItemLog item = new ItemLog(nivel, excepcion, mensaje);
            item.SetArgumentos(args);
            Log(item, args);
        }
        
        #endregion

        #endregion
        
        #region Métodos protegidos
        /// <summary>
        /// Método escalable de inclusión de logger.
        /// </summary>
        /// <param name="nivel">Nivel de logger.</param>
        /// <returns>Indica si el nivel de logger se incluye en el tipo de definido.</returns>
        protected virtual bool ESNivelLogEnabled(NivelLog nivel)
        {
            return nivel >= this.NivelLog;
        }
        #endregion
    }
}
