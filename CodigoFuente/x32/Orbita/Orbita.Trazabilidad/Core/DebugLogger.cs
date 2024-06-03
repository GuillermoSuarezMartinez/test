//***********************************************************************
// Assembly         : OrbitaTrazabilidad
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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
namespace Orbita.Trazabilidad
{
    /// <summary>
    /// Debug Logger.
    /// </summary>
    public class DebugLogger : TraceLogger
    {
        #region Atributos privados
        /// <summary>
        /// Ruta de almacenamiento de ficheros de backup de logger.
        /// </summary>
        PathBackup pathBackup;
        /// <summary>
        /// Tiempo de inicio y periodo de ejecución de backups de logger.
        /// </summary>
        TimerLogger timerLogger;
        /// <summary>
        /// Ruta de almacenamiento de ficheros de error de logger.
        /// </summary>
        PathLogger pathError;
        /// <summary>
        /// Párametro de tamaño máximo de comparación de backups de logger.
        /// </summary>
        long sizeBackup;
        /// <summary>
        /// Escucha las notificaciones de cambio del sistema de archivos y provoca eventos
        /// cuando cambia un directorio o un archivo de un directorio. Útil para backups
        /// de logger por tamaño.
        /// </summary>
        System.IO.FileSystemWatcher watcher;
        #endregion

        #region Delegados públicos
        /// <summary>
        /// Evento que se ejecuta tras ejecutar la Callback.
        /// </summary>
        public event EventHandler OnDespuesEjecutar;
        #endregion

        #region Constructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>, <c>Hora de ejecución=23:00:00</c>, <c>Periodo de ejecución=24h</c>, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        public DebugLogger(string identificador)
            : base(identificador) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>, <c>Hora de ejecución=23:00:00</c>, <c>Periodo de ejecución=24h</c>, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        public DebugLogger(string identificador, NivelLog nivelLog)
            : base(identificador, nivelLog) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>Hora de ejecución=23:00:00</c>, <c>Periodo de ejecución=24h</c>, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        public DebugLogger(string identificador, PathLogger pathLogger)
            : base(identificador, pathLogger) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>Hora de ejecución=23:00:00</c>, <c>Periodo de ejecución=24h</c>, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        public DebugLogger(string identificador, NivelLog nivelLog, PathLogger pathLogger)
            : base(identificador, nivelLog, pathLogger) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="timerBackup">Objeto que representa el periodo de tiempo de inicio de backup logger y tiempo entre invocaciones.</param>
        public DebugLogger(string identificador, TimerBackup timerBackup)
            : this(identificador, NivelLog.Debug, timerBackup) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="sizeBackup">Tamaño máximo de logger hasta generar backup en bytes.</param>
        public DebugLogger(string identificador, long sizeBackup)
            : this(identificador, NivelLog.Debug, sizeBackup) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="timerBackup">Objeto que representa el periodo de tiempo de inicio de backup logger y tiempo entre invocaciones.</param>
        public DebugLogger(string identificador, NivelLog nivelLog, TimerBackup timerBackup)
            : this(identificador, nivelLog, new PathLogger(), timerBackup) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="sizeBackup">Tamaño máximo de logger hasta generar backup en bytes.</param>
        public DebugLogger(string identificador, NivelLog nivelLog, long sizeBackup)
            : this(identificador, nivelLog, new PathLogger(), sizeBackup) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        /// <param name="timerBackup">Objeto que representa el periodo de tiempo de inicio de backup logger y tiempo entre invocaciones.</param>
        public DebugLogger(string identificador, PathLogger pathLogger, TimerBackup timerBackup)
            : this(identificador, NivelLog.Debug, pathLogger, timerBackup) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        /// <param name="sizeBackup">Tamaño máximo de logger hasta generar backup en bytes.</param>
        public DebugLogger(string identificador, PathLogger pathLogger, long sizeBackup)
            : this(identificador, NivelLog.Debug, pathLogger, sizeBackup) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        /// <param name="timerBackup">Objeto que representa el periodo de tiempo de inicio de backup logger y tiempo entre invocaciones.</param>
        public DebugLogger(string identificador, NivelLog nivelLog, PathLogger pathLogger, TimerBackup timerBackup)
            : this(identificador, nivelLog, pathLogger, timerBackup, new PathBackup()) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>PathBackup=Application.StartupPath\BLog</c>, <c>Máscara=aaaaMM</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        /// <param name="sizeBackup">Tamaño máximo de logger hasta generar backup en bytes.</param>
        public DebugLogger(string identificador, NivelLog nivelLog, PathLogger pathLogger, long sizeBackup)
            : this(identificador, nivelLog, pathLogger, sizeBackup, new PathBackup()) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>, <c>Hora de ejecución=23:00:00</c>, <c>Periodo de ejecución=24h</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        public DebugLogger(string identificador, PathBackup pathBackup)
            : this(identificador, NivelLog.Debug, new TimerBackup(), pathBackup) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>, <c>Hora de ejecución=23:00:00</c>, <c>Periodo de ejecución=24h</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        public DebugLogger(string identificador, NivelLog nivelLog, PathBackup pathBackup)
            : this(identificador, nivelLog, new PathLogger(), new TimerBackup(), pathBackup) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>Hora de ejecución=23:00:00</c>, <c>Periodo de ejecución=24h</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        public DebugLogger(string identificador, PathLogger pathLogger, PathBackup pathBackup)
            : this(identificador, NivelLog.Debug, pathLogger, new TimerBackup(), pathBackup) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>Hora de ejecución=23:00:00</c>, <c>Periodo de ejecución=24h</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        public DebugLogger(string identificador, NivelLog nivelLog, PathLogger pathLogger, PathBackup pathBackup)
            : this(identificador, nivelLog, pathLogger, new TimerBackup(), pathBackup) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="timerBackup">Objeto que representa el periodo de tiempo de inicio de backup logger y tiempo entre invocaciones.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        public DebugLogger(string identificador, TimerBackup timerBackup, PathBackup pathBackup)
            : this(identificador, NivelLog.Debug, timerBackup, pathBackup) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="sizeBackup">Tamaño máximo de logger hasta generar backup en bytes.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        public DebugLogger(string identificador, long sizeBackup, PathBackup pathBackup)
            : this(identificador, NivelLog.Debug, sizeBackup, pathBackup) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="timerBackup">Objeto que representa el periodo de tiempo de inicio de backup logger y tiempo entre invocaciones.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        public DebugLogger(string identificador, NivelLog nivelLog, TimerBackup timerBackup, PathBackup pathBackup)
            : this(identificador, nivelLog, new PathLogger(), timerBackup, pathBackup) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>PathLogger=Application.StartupPath\Log</c>, <c>Fichero=debug</c>, <c>Extensión=log</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="sizeBackup">Tamaño máximo de logger hasta generar backup en bytes.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        public DebugLogger(string identificador, NivelLog nivelLog, long sizeBackup, PathBackup pathBackup)
            : this(identificador, nivelLog, new PathLogger(), sizeBackup, pathBackup) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        /// <param name="timerBackup">Objeto que representa el periodo de tiempo de inicio de backup logger y tiempo entre invocaciones.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        public DebugLogger(string identificador, PathLogger pathLogger, TimerBackup timerBackup, PathBackup pathBackup)
            : this(identificador, NivelLog.Debug, pathLogger, timerBackup, pathBackup) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// Por defecto, <c>NivelLog=Debug</c>.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        /// <param name="sizeBackup">Tamaño máximo de logger hasta generar backup en bytes.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        public DebugLogger(string identificador, PathLogger pathLogger, long sizeBackup, PathBackup pathBackup)
            : this(identificador, NivelLog.Debug, pathLogger, sizeBackup, pathBackup) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        /// <param name="timerBackup">Objeto que representa el periodo de tiempo de inicio de backup logger y tiempo entre invocaciones.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        public DebugLogger(string identificador, NivelLog nivelLog, PathLogger pathLogger, TimerBackup timerBackup, PathBackup pathBackup)
            : base(identificador, nivelLog, pathLogger)
        {
            if (pathLogger != null)
            {
                // Componer y asignar el listeners de trazas generadas en excepciones de logger.
                this.PathError = pathLogger;
            }
            if (pathBackup != null)
            {
                // Asignar path de backup de logger.
                this.pathBackup = pathBackup;
            }
            if (timerBackup != null)
            {
                // Asignar el timer adecuado de copias backup, en tiempo y periodo.
                // Crear el objeto público, para permitir desde fuera destruir el timer.
                this.timerLogger = new TimerLogger();
                this.timerLogger.CallBack = new System.Threading.TimerCallback(ProcesoBackup);
                this.timerLogger.Timer = new System.Threading.Timer(this.timerLogger.CallBack);
                this.timerLogger.Change(timerBackup.Hora, timerBackup.Periodo);
            }
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.DebugLogger.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="nivelLog">Nivel de logger. Por defecto, <c>NivelLog.Debug</c>.</param>
        /// <param name="pathLogger">Ruta de almacenamiento del fichero de logger.</param>
        /// <param name="sizeBackup">Tamaño máximo de logger hasta generar backup en bytes.</param>
        /// <param name="pathBackup">Ruta de almacenamiento del fichero de backup de logger.</param>
        public DebugLogger(string identificador, NivelLog nivelLog, PathLogger pathLogger, long sizeBackup, PathBackup pathBackup)
            : this(identificador, nivelLog, pathLogger, null, pathBackup)
        {
            // Tamaño máximo de logger hasta hacer backup.
            this.sizeBackup = sizeBackup;
            // Crear un vigilante de archivos del sistema.
            this.watcher = new System.IO.FileSystemWatcher();
            // Asignar la ruta que se quiere monitorizar.
            this.watcher.Path = System.IO.Path.GetFullPath(pathLogger.Path);
            this.watcher.Filter = "*." + pathLogger.Extension;
            this.watcher.NotifyFilter = System.IO.NotifyFilters.Size;
            // No incluir subdirectorios en la ruta de monitorización.
            this.watcher.IncludeSubdirectories = false;
            // Asignar el evento de cambio.
            this.watcher.Changed += new System.IO.FileSystemEventHandler(Watcher_Changed);
            //
            // Comenzar la exploración de tamaño de logger.
            this.watcher.EnableRaisingEvents = true;
        }
        #endregion

        #region Propiedades públicas
        /// <summary>
        /// Ruta de almacenamiento del fichero de logger.
        /// </summary>
        public override PathLogger PathLogger
        {
            get { return this.pathLogger; }
            set { this.pathLogger = value; }
        }
        /// <summary>
        /// Ruta de almacenamiento del fichero de backup de logger.
        /// </summary>
        public PathBackup PathBackup
        {
            get { return this.pathBackup; }
            set { this.pathBackup = value; }
        }
        /// <summary>
        /// Ruta de almacenamiento del fichero de error de logger.
        /// </summary>
        public PathLogger PathError
        {
            get { return this.pathError; }
            set
            {
                if (value != null)
                {
                    this.pathError = new PathLogger(value.Path, string.Concat(value.Fichero, "-", Orbita.Trazabilidad.Fichero.Error), value.Extension);
                    this.listener = new TextWriterTraceListener(this.pathError.ToString());
                }
            }
        }
        /// <summary>
        /// Tiempo de inicio y periodo de ejecución de backups de logger. Propiedad de solo lectura.
        /// </summary>
        public TimerLogger TimerLogger
        {
            get { return this.timerLogger; }
        }
        /// <summary>
        /// Separador para fichero logger.
        /// </summary>
        public override string Separador
        {
            get { return string.Format(System.Globalization.CultureInfo.CurrentCulture, " {0} ", this.separador); }
            set { this.separador = value; }
        }
        /// <summary>
        /// Permitir incluir retornos de carro en los mensajes de logger. Por defecto, false.
        /// </summary>
        public override bool RetornoCarro
        {
            get { return this.retornoCarro; }
            set { this.retornoCarro = value; }
        }
        /// <summary>
        /// Ruta completa de almacenamiento del fichero de logger.
        /// </summary>
        public override string Fichero
        {
            get { return this.pathLogger.ToString(); }
        }
        #endregion

        #region Propiedades privadas
        /// <summary>
        /// Nombre del fichero del backup de logger.
        /// </summary>
        string FicheroBackup
        {
            get
            {
                // Fecha actual, necesaria para la generación de la máscara de path de backup logger.
                DateTime fecha = DateTime.Now;
                // Crear el nuevo objeto de path de backup logger completo.
                // Asignar el path (pathlogger), nombre del fichero destino y extensión.
                PathLogger pathBackupFull = new PathLogger(this.PathBackup.Path, string.Format(System.Globalization.CultureInfo.CurrentCulture, @"{0}-{1}-{2}", this.PathLogger.Fichero, fecha.ToString(Orbita.Trazabilidad.Formato.AñoMesDia, System.Globalization.CultureInfo.CurrentCulture), fecha.ToString(Orbita.Trazabilidad.Formato.HoraMinutoSegundoCentesima, System.Globalization.CultureInfo.CurrentCulture)), this.PathLogger.Extension);
                // Formato del subpath de backup.
                string formato = this.PathBackup.Mascara;
                if (!string.IsNullOrEmpty(formato))
                {
                    pathBackupFull.Path = string.Format(System.Globalization.CultureInfo.CurrentCulture, @"{0}\{1}", this.PathBackup.Path, fecha.ToString(formato, System.Globalization.CultureInfo.CurrentCulture));
                }
                return pathBackupFull.ToString();
            }
        }
        #endregion

        #region Métodos privados
        /// <summary>
        /// Método de proceso de Callback.
        /// </summary>
        /// <param name="estado">Estado de la Callback.</param>
        [SuppressMessageAttribute("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        void ProcesoBackup(object estado)
        {
            try
            {
                lock (Bloqueo)
                {
                    // Si existe el fichero origen (por defecto, debug.log).
                    if (this.PathLogger.ExisteFichero())
                    {
                        // Leer mediante un StreamReader el fichero de información origen (por defecto, debug.log).
                        using (System.IO.StreamReader origen = new System.IO.StreamReader(this.Fichero))
                        using (System.IO.StreamWriter destino = new System.IO.StreamWriter(this.FicheroBackup))
                        {
                            // Escribir en el destino mediante un StreamWriter.
                            // Leer la secuencia desde la posición actual hasta el final de la secuencia.
                            destino.Write(origen.ReadToEnd());
                        }
                        // Eliminar el fichero origen (por defecto, debug.log).
                        if (!this.PathLogger.BorrarFichero())
                        {
                            throw new Exception(string.Format(System.Globalization.CultureInfo.CurrentCulture, "No se ha podido eliminar el archivo {0}", this.Fichero));
                        }
                        // En C# debemos comprobar que el evento no sea null.
                        if (this.OnDespuesEjecutar != null)
                        {
                            // El evento se lanza como cualquier delegado.
                            this.OnDespuesEjecutar(this, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Escribir(ex);
            }
        }
        #endregion

        #region Manejadores de eventos
        /// <summary>
        /// Watcher_Changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Watcher_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            try
            {
                bool backup;
                lock (Bloqueo)
                {
                    backup = false;
                    // Si existe el fichero origen (por defecto, debug.log).
                    System.IO.FileInfo logger = new System.IO.FileInfo(e.FullPath);
                    if (logger.Exists)
                    {
                        // Si el tamaño del fichero en bytes es igual o superior al establecido
                        // procesar el backup correspondiente.
                        backup = logger.Length.CompareTo(this.sizeBackup) >= 0;
                    }
                }
                // Control de backup fuera del bloqueo.
                if (backup)
                {
                    this.ProcesoBackup(null);
                }
            }
            catch (Exception ex)
            {
                Escribir(ex);
            }
        }
        #endregion
    }
}
