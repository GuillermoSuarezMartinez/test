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
using System.Collections;
using System.Net;
namespace Orbita.Trazabilidad
{
    /// <summary>
    /// LogManager.
    /// </summary>
    public static class LogManager
    {
        #region Atributos privados estáticos
        /// <summary>
        /// Repositorio de loggers.
        /// </summary>
        static Hashtable Repositorio = new Hashtable();
        #endregion

        #region Métodos públicos estáticos

        /// <summary>
        /// Obtener logger por nombre.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <returns>ILogger.</returns>
        public static ILogger GetLogger(string identificador)
        {
            ILogger logger = null;
            if (Repositorio.Contains(identificador))
            {
                logger = (Repositorio[identificador] as SimpleLogger).Logger;
            }
            return logger;
        }
        /// <summary>
        /// Asignar logger al repositorio.
        /// </summary>
        /// <param name="logger">Interface de logger.</param>
        public static void SetLogger(ILogger logger)
        {
            if (logger != null)
            {
                string clave = logger.Identificador ?? Orbita.Trazabilidad.Logger.Debug;
                if (!Repositorio.Contains(clave))
                {
                    Repositorio.Add(clave, new SimpleLogger(logger));
                }
            }
        }
        /// <summary>
        /// Eliminar logger por nombre del repositorio.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <returns>Si existe en la colección el identificador de traza.</returns>
        public static bool DelLogger(string identificador)
        {
            bool res = Repositorio.Contains(identificador);
            if (res)
            {
                ILogger logger = (Repositorio[identificador] as SimpleLogger).Logger;
                if (logger.GetType() == typeof(DebugLogger))
                {
                    // Detener el proceso de backup del objeto System.Threading.Timer.
                    (logger as DebugLogger).TimerLogger.Timer.Dispose();
                }
                Repositorio.Remove(identificador);

            }
            return res;
        }

        #region DebugLogger
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>, <c>Hora de ejecución=23:00:00</c>, <c>Periodo de ejecución=24h</c>, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador)
        { 
            return SetDebugLogger(identificador, NivelLog.Debug);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>, <c>Hora de ejecución=23:00:00</c>, <c>Periodo de ejecución=24h</c>, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador, NivelLog nivelLog)
        {
            return SetDebugLogger(identificador, nivelLog, new PathLogger());
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>Hora de ejecución=23:00:00</c>, <c>Periodo de ejecución=24h</c>, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador, PathLogger pathLogger)
        {
            return SetDebugLogger(identificador, NivelLog.Debug, pathLogger);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>Hora de ejecución=23:00:00</c>, <c>Periodo de ejecución=24h</c>, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador, NivelLog nivelLog, PathLogger pathLogger)
        {
            // Construir el logger.
            ILogger logger = new DebugLogger(identificador, nivelLog, pathLogger);
            // Asignar logger al repositorio.
            SetLogger(logger);
            // Retornar el logger.
            return GetLogger(identificador);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="timerBackup">Objeto que representa el periodo de tiempo de inicio de backup logger y tiempo entre invocaciones.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador, TimerBackup timerBackup)
        {
            return SetDebugLogger(identificador, NivelLog.Debug, timerBackup);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="sizeBackup">Tamaño máximo de logger hasta generar backup en bytes.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador, long sizeBackup)
        {
            return SetDebugLogger(identificador, NivelLog.Debug, sizeBackup);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="timerBackup">Objeto que representa el periodo de tiempo de inicio de backup logger y tiempo entre invocaciones.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador, NivelLog nivelLog, TimerBackup timerBackup)
        {
            return SetDebugLogger(identificador, nivelLog, new PathLogger(), timerBackup);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="sizeBackup">Tamaño máximo de logger hasta generar backup en bytes.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador, NivelLog nivelLog, long sizeBackup)
        {
            return SetDebugLogger(identificador, nivelLog, new PathLogger(), sizeBackup);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        /// <param name="timerBackup">Objeto que representa el periodo de tiempo de inicio de backup logger y tiempo entre invocaciones.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador, PathLogger pathLogger, TimerBackup timerBackup)
        {
            return SetDebugLogger(identificador, NivelLog.Debug, pathLogger, timerBackup);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        /// <param name="sizeBackup">Tamaño máximo de logger hasta generar backup en bytes.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador, PathLogger pathLogger, long sizeBackup)
        {
            return SetDebugLogger(identificador, NivelLog.Debug, pathLogger, sizeBackup);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        /// <param name="timerBackup">Objeto que representa el periodo de tiempo de inicio de backup logger y tiempo entre invocaciones.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador, NivelLog nivelLog, PathLogger pathLogger, TimerBackup timerBackup)
        {
            return SetDebugLogger(identificador, nivelLog, pathLogger, timerBackup, new PathBackup());
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        /// <param name="sizeBackup">Tamaño máximo de logger hasta generar backup en bytes.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador, NivelLog nivelLog, PathLogger pathLogger, long sizeBackup)
        {
            return SetDebugLogger(identificador, nivelLog, pathLogger, sizeBackup, new PathBackup());
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>, <c>Hora de ejecución=23:00:00</c>, <c>Periodo de ejecución=24h</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador, PathBackup pathBackup)
        {
            return SetDebugLogger(identificador, NivelLog.Debug, new TimerBackup(), pathBackup);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>, <c>Hora de ejecución=23:00:00</c>, <c>Periodo de ejecución=24h</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador, NivelLog nivelLog, PathBackup pathBackup)
        {
            return SetDebugLogger(identificador, nivelLog, new PathLogger(), new TimerBackup(), pathBackup);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>Hora de ejecución=23:00:00</c>, <c>Periodo de ejecución=24h</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador, PathLogger pathLogger, PathBackup pathBackup)
        {
            return SetDebugLogger(identificador, NivelLog.Debug, pathLogger, new TimerBackup(), pathBackup);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>Hora de ejecución=23:00:00</c>, <c>Periodo de ejecución=24h</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador, NivelLog nivelLog, PathLogger pathLogger, PathBackup pathBackup)
        {
            return SetDebugLogger(identificador, nivelLog, pathLogger, new TimerBackup(), pathBackup);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="timerBackup">Objeto que representa el periodo de tiempo de inicio de backup logger y tiempo entre invocaciones.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador, TimerBackup timerBackup, PathBackup pathBackup)
        {
            return SetDebugLogger(identificador, NivelLog.Debug, timerBackup, pathBackup);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="sizeBackup">Tamaño máximo de logger hasta generar backup en bytes.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador, long sizeBackup, PathBackup pathBackup)
        {
            return SetDebugLogger(identificador, NivelLog.Debug, sizeBackup, pathBackup);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="timerBackup">Objeto que representa el periodo de tiempo de inicio de backup logger y tiempo entre invocaciones.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador, NivelLog nivelLog, TimerBackup timerBackup, PathBackup pathBackup)
        {
            return SetDebugLogger(identificador, nivelLog, new PathLogger(), timerBackup, pathBackup);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="sizeBackup">Tamaño máximo de logger hasta generar backup en bytes.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador, NivelLog nivelLog, long sizeBackup, PathBackup pathBackup)
        {
            return SetDebugLogger(identificador, nivelLog, new PathLogger(), sizeBackup, pathBackup);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        /// <param name="timerBackup">Objeto que representa el periodo de tiempo de inicio de backup logger y tiempo entre invocaciones.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador, PathLogger pathLogger, TimerBackup timerBackup, PathBackup pathBackup)
        {
            return SetDebugLogger(identificador, NivelLog.Debug, pathLogger, timerBackup, pathBackup);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        /// <param name="sizeBackup">Tamaño máximo de logger hasta generar backup en bytes.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador, PathLogger pathLogger, long sizeBackup, PathBackup pathBackup)
        {
            return SetDebugLogger(identificador, NivelLog.Debug, pathLogger, sizeBackup, pathBackup);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        /// <param name="timerBackup">Objeto que representa el periodo de tiempo de inicio de backup logger y tiempo entre invocaciones.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador, NivelLog nivelLog, PathLogger pathLogger, TimerBackup timerBackup, PathBackup pathBackup)
        {
            // Construir el logger.
            ILogger logger = new DebugLogger(identificador, nivelLog, pathLogger, timerBackup, pathBackup);
            // Asignar logger al repositorio.
            SetLogger(logger);
            // Retornar el logger.
            return GetLogger(identificador);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        /// <param name="sizeBackup">Tamaño máximo de logger hasta generar backup en bytes.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetDebugLogger(string identificador, NivelLog nivelLog, PathLogger pathLogger, long sizeBackup, PathBackup pathBackup)
        {
            // Construir el logger.
            ILogger logger = new DebugLogger(identificador, nivelLog, pathLogger, sizeBackup, pathBackup);
            // Asignar logger al repositorio.
            SetLogger(logger);
            // Retornar el logger.
            return GetLogger(identificador);
        }
        #endregion
       
        #region RemotingLogger
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>Alias=logger</c>, <c>Puerto=1440</c>, <c>Máquina=localhost</c>.
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetRemotingLogger(string identificador)
        {
            return SetRemotingLogger(identificador, NivelLog.Debug);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// Por defecto, <c>Alias=logger</c>, <c>Puerto=1440</c>, <c>Máquina=localhost</c>.
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetRemotingLogger(string identificador, NivelLog nivelLog)
        {
            return SetRemotingLogger(identificador, nivelLog, Orbita.Trazabilidad.Logger.Alias);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>Alias=logger</c>, <c>Máquina=localhost</c>.
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        /// <param name="puerto">Puerto del URI de conexión .NET remoting.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetRemotingLogger(string identificador, int puerto)
        {
            return SetRemotingLogger(identificador, Orbita.Trazabilidad.Logger.Alias, puerto);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// Por defecto, <c>Alias=logger</c>, <c>Máquina=localhost</c>.
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="puerto">Puerto del URI de conexión .NET remoting.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetRemotingLogger(string identificador, NivelLog nivelLog, int puerto)
        {
            return SetRemotingLogger(identificador, nivelLog, Orbita.Trazabilidad.Logger.Alias, puerto);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>Alias=logger</c>.
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        /// <param name="puerto">Puerto del URI de conexión .NET remoting.</param>
        /// <param name="maquina">Host de la máquina cliente de conexión .NET remoting.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetRemotingLogger(string identificador, int puerto, string maquina)
        {
            return SetRemotingLogger(identificador, NivelLog.Debug, puerto, maquina);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// Por defecto, <c>Alias=logger</c>.
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="puerto">Puerto del URI de conexión .NET remoting.</param>
        /// <param name="maquina">Host de la máquina cliente de conexión .NET remoting.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetRemotingLogger(string identificador, NivelLog nivelLog, int puerto, string maquina)
        {
            return SetRemotingLogger(identificador, nivelLog, Orbita.Trazabilidad.Logger.Alias, puerto, maquina);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>Puerto=1440</c>, <c>Máquina=localhost</c>.
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        /// <param name="alias">Alias del URI de conexión .NET remoting.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetRemotingLogger(string identificador, string alias)
        {
            return SetRemotingLogger(identificador, NivelLog.Debug, alias);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// Por defecto, <c>Puerto=1440</c>, <c>Máquina=localhost</c>.
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="alias">Alias del URI de conexión .NET remoting.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetRemotingLogger(string identificador, NivelLog nivelLog, string alias)
        {
            return SetRemotingLogger(identificador, nivelLog, alias, Orbita.Trazabilidad.Logger.Puerto);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>Máquina=localhost</c>.
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        /// <param name="alias">Alias del URI de conexión .NET remoting.</param>
        /// <param name="puerto">Puerto del URI de conexión .NET remoting.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetRemotingLogger(string identificador, string alias, int puerto)
        {
            return SetRemotingLogger(identificador, NivelLog.Debug, alias, puerto);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// Por defecto, <c>Máquina=localhost</c>.
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="alias">Alias del URI de conexión .NET remoting.</param>
        /// <param name="puerto">Puerto del URI de conexión .NET remoting.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetRemotingLogger(string identificador, NivelLog nivelLog, string alias, int puerto)
        {
            return SetRemotingLogger(identificador, nivelLog, alias, puerto, Dns.GetHostName());
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// Por defecto, <c>NivelLog=Debug</c>. 
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        /// <param name="alias">Alias del URI de conexión .NET remoting.</param>
        /// <param name="puerto">Puerto del URI de conexión .NET remoting.</param>
        /// <param name="maquina">Host de la máquina cliente de conexión .NET remoting.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetRemotingLogger(string identificador, string alias, int puerto, string maquina)
        {
            return SetRemotingLogger(identificador, NivelLog.Debug, alias, puerto, maquina);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="alias">Alias del URI de conexión .NET remoting.</param>
        /// <param name="puerto">Puerto del URI de conexión .NET remoting.</param>
        /// <param name="maquina">Host de la máquina cliente de conexión .NET remoting.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetRemotingLogger(string identificador, NivelLog nivelLog, string alias, int puerto, string maquina)
        {
            // Construir el logger.
            ILogger logger = new RemotingLogger(identificador, nivelLog, alias, puerto, maquina);
            // Asignar logger al repositorio.
            SetLogger(logger);
            // Retornar el logger.
            return GetLogger(identificador);
        }
        #endregion

        #region WrapperLogger
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.WrapperLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>Puerto=1440</c>, <c>Loggers=DebugLogger (debug), RemotingLogger (remoting)</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetWrapperLogger(string identificador)
        {
            return SetWrapperLogger(identificador, NivelLog.Debug);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.WrapperLogger.
        /// Por defecto, <c>Puerto=1440</c>, <c>Loggers=DebugLogger (debug), RemotingLogger (remoting)</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetWrapperLogger(string identificador, NivelLog nivelLog)
        {
            return SetWrapperLogger(identificador, nivelLog, Orbita.Trazabilidad.Logger.Puerto);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.WrapperLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>Loggers=DebugLogger (debug), RemotingLogger (remoting)</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="puerto">Puerto del URI de conexión .NET remoting.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetWrapperLogger(string identificador, int puerto)
        {
            return SetWrapperLogger(identificador, NivelLog.Debug, puerto);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.WrapperLogger.
        /// Por defecto, <c>Loggers=DebugLogger (debug), RemotingLogger (remoting)</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger.</param>
        /// <param name="puerto">Puerto del URI de conexión .NET remoting.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetWrapperLogger(string identificador, NivelLog nivelLog, int puerto)
        {
            // Construir el debuglogger.
            ILogger debugLogger = new DebugLogger(Orbita.Trazabilidad.Logger.Debug, nivelLog);
            // Construir el remotinglogger.
            ILogger remotingLogger = new RemotingLogger(Orbita.Trazabilidad.Logger.Remoting, nivelLog, puerto);
            // Retornar el logger.
            return SetWrapperLogger(identificador, nivelLog, debugLogger, remotingLogger);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.WrapperLogger.
        /// Por defecto, <c>Puerto=1440</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger.</param>
        /// <param name="loggers">Colección de loggers.</param>
        /// <returns>ILogger.</returns>
        public static ILogger SetWrapperLogger(string identificador, NivelLog nivelLog, params ILogger[] loggers)
        {
            // Construir el logger.
            ILogger logger = new WrapperLogger(identificador, nivelLog, loggers);
            // Asignar logger al repositorio.
            SetLogger(logger);
            // Retornar el logger.
            return GetLogger(identificador);
        }
        #endregion

        #endregion
    }
}
