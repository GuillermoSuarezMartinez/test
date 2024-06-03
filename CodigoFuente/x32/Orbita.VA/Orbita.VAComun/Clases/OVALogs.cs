//***********************************************************************
// Assembly         : Orbita.VAComun
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 16-11-2012
// Description      : Gestión de la visualización de formularios desde llamadas en threads
//                    
// Last Modified By : aibañez
// Last Modified On : 16-11-2012
// Description      : Desvinculado de forularios de diálogo
//                    Nuevos eventos de ShowMessageDelegate y ShowExceptionDelegate
//                    Nuevo módulo de log: ManejadorObjetosGrandes
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Orbita.Trazabilidad;

namespace Orbita.VAComun
{
    /// <summary>
    /// Clase estática para acceder a las funciones de los registros
    /// </summary>
    public static class OVALogsManager
    {
        #region Atributo(s)
        /// <summary>
        /// Log por defecto
        /// </summary>
        public static ILogger LogDefecto;

        /// <summary>
        /// Log de excepciones
        /// </summary>
        public static ILogger LogExcepcion;

        /// <summary>
        /// Se está iniciando el sistema
        /// </summary>
        public static bool IniciandoSistema;

        /// <summary>
        /// Logger de tipo backup
        /// </summary>
        public static Dictionary<string, ILogger> Loggers;

        /// <summary>
        /// Delegado utilizado para lanzar mensajes 
        /// </summary>
        public static OMessageDelegate ShowMessageDelegate;

        /// <summary>
        /// Delegado utilizado para lanzar mensajes 
        /// </summary>
        public static OExceptionDelegate ShowExceptionDelegate;
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Construye los objetos
        /// </summary>
        public static void Constructor(OMessageDelegate showMessageEvent, OExceptionDelegate showExceptionDelegate)
        {
            Loggers = new Dictionary<string, ILogger>();
            IniciandoSistema = true;
            ShowMessageDelegate = showMessageEvent;
            ShowExceptionDelegate = showExceptionDelegate;
        }

        /// <summary>
        /// Destruye los campos de la clase
        /// </summary>
        public static void Destructor()
        {
        }

        /// <summary>
        /// Se cargan los valores de la clase
        /// </summary>
        public static void Inicializar()
        {
            // Consulta de la información
            OOpcionesLogs opcionesLogs = OSistemaManager.Configuracion.OpcionesLogs;

            // Creamos el logger por defecto
            LogDefecto = LogManager.SetDebugLogger("Defecto", 
                NivelLog.Info,
                new PathLogger(Path.Combine(ORutaParametrizable.AppFolder, opcionesLogs.Path), opcionesLogs.Fichero, opcionesLogs.Extension),
                new TimerBackup(opcionesLogs.Inicio, opcionesLogs.Retardo),
                new PathBackup(Path.Combine(ORutaParametrizable.AppFolder, opcionesLogs.PathBackup), opcionesLogs.MascaraSubPathBackup));
            (LogDefecto as DebugLogger).Separador = opcionesLogs.Separador;

            // Creamos el logger de errores
            LogExcepcion = LogManager.SetDebugLogger("Excepcion",
                NivelLog.Error,
                new PathLogger(ORutaParametrizable.AppFolder, "Error", opcionesLogs.Extension));
            (LogExcepcion as DebugLogger).Separador = opcionesLogs.Separador;
            
            // Creamos los loggers para el ensamblado comun
            foreach (OOpcionesLog opcionesLog in opcionesLogs.OpcionesLog)
            {
                ILogger logger = LogManager.SetDebugLogger(opcionesLog.Identificador,
                opcionesLog.NivelLog,
                new PathLogger(Path.Combine(ORutaParametrizable.AppFolder, opcionesLogs.Path), opcionesLogs.Fichero, opcionesLogs.Extension),
                new TimerBackup(opcionesLogs.Inicio, opcionesLogs.Retardo),
                new PathBackup(Path.Combine(ORutaParametrizable.AppFolder, opcionesLogs.PathBackup), opcionesLogs.MascaraSubPathBackup));
                (logger as DebugLogger).Separador = opcionesLogs.Separador;

                Loggers.Add(opcionesLog.Identificador, logger);
            }
        }

        /// <summary>
        /// Se desactiva el modo inicialización (trabaja de un modo distinto)
        /// </summary>
        public static void FinInicializacion()
        {
            IniciandoSistema = false;
        }

        /// <summary>
        /// Se realizan las acciones de finalización
        /// </summary>
        public static void Finalizar()
        {
            IniciandoSistema = true;
        }

        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="mensaje">Mensaje a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Debug(OEnumModulosSistema modulo, string remitente, string mensaje)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Debug(mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="excepcion">Excepción a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Debug(OEnumModulosSistema modulo, string remitente, Exception excepcion)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Debug(excepcion, "Error", (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="objeto">Objeto a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Debug(OEnumModulosSistema modulo, string remitente, object objeto)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Debug("Dato", objeto, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="excepcion">Excepción a guardar</param>
        /// <param name="mensaje">Mensaje a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Debug(OEnumModulosSistema modulo, string remitente, Exception excepcion, string mensaje)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Debug(excepcion, mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="mensaje">Mensaje a guardar</param>
        /// <param name="args">Argumento(s) a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Debug(OEnumModulosSistema modulo, string remitente, string mensaje, params object[] args)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Debug(mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente, args);
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="excepcion">Excepción a guardar</param>
        /// <param name="mensaje">Mensaje a guardar</param>
        /// <param name="args">Argumento(s) a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Debug(OEnumModulosSistema modulo, string remitente, Exception excepcion, string mensaje, params object[] args)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Debug(excepcion, mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente, args);
        }

        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="mensaje">Mensaje a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Info(OEnumModulosSistema modulo, string remitente, string mensaje)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Info(mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="excepcion">Excepción a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Info(OEnumModulosSistema modulo, string remitente, Exception excepcion)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Info(excepcion, "Error", (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="objeto">Objeto a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Info(OEnumModulosSistema modulo, string remitente, object objeto)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Info("Dato", objeto, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="excepcion">Excepción a guardar</param>
        /// <param name="mensaje">Mensaje a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Info(OEnumModulosSistema modulo, string remitente, Exception excepcion, string mensaje)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Info(excepcion, mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="mensaje">Mensaje a guardar</param>
        /// <param name="args">Argumento(s) a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Info(OEnumModulosSistema modulo, string remitente, string mensaje, params object[] args)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Info(mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente, args);
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="excepcion">Excepción a guardar</param>
        /// <param name="mensaje">Mensaje a guardar</param>
        /// <param name="args">Argumento(s) a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Info(OEnumModulosSistema modulo, string remitente, Exception excepcion, string mensaje, params object[] args)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Info(excepcion, mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente, args);
        }

        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="mensaje">Mensaje a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Warning(OEnumModulosSistema modulo, string remitente, string mensaje)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Warn(mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="excepcion">Excepción a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Warning(OEnumModulosSistema modulo, string remitente, Exception excepcion)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Warn(excepcion, "Error", (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="objeto">Objeto a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Warning(OEnumModulosSistema modulo, string remitente, object objeto)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Warn("Dato", objeto, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="excepcion">Excepción a guardar</param>
        /// <param name="mensaje">Mensaje a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Warning(OEnumModulosSistema modulo, string remitente, Exception excepcion, string mensaje)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Warn(excepcion, mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="mensaje">Mensaje a guardar</param>
        /// <param name="args">Argumento(s) a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Warning(OEnumModulosSistema modulo, string remitente, string mensaje, params object[] args)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Warn(mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente, args);
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="excepcion">Excepción a guardar</param>
        /// <param name="mensaje">Mensaje a guardar</param>
        /// <param name="args">Argumento(s) a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Warning(OEnumModulosSistema modulo, string remitente, Exception excepcion, string mensaje, params object[] args)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Warn(excepcion, mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente, args);
        }

        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="mensaje">Mensaje a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Error(OEnumModulosSistema modulo, string remitente, string mensaje)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Error(mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
            LogExcepcion.Error(mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
            MensajeError(mensaje);
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="excepcion">Excepción a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Error(OEnumModulosSistema modulo, string remitente, Exception excepcion)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Error(excepcion, "Error", (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
            LogExcepcion.Error(excepcion, "Error", (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
            MensajeError(excepcion);
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="objeto">Objeto a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Error(OEnumModulosSistema modulo, string remitente, object objeto)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Error("Dato", objeto, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
            LogExcepcion.Error("Dato", objeto, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
            MensajeError(App.ToString(objeto));
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="excepcion">Excepción a guardar</param>
        /// <param name="mensaje">Mensaje a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Error(OEnumModulosSistema modulo, string remitente, Exception excepcion, string mensaje)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Error(excepcion, mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
            LogExcepcion.Error(excepcion, mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
            MensajeError(excepcion);
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="mensaje">Mensaje a guardar</param>
        /// <param name="args">Argumento(s) a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Error(OEnumModulosSistema modulo, string remitente, string mensaje, params object[] args)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Error(mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente, args);
            LogExcepcion.Error(mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente, args);
            MensajeError(mensaje);
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="excepcion">Excepción a guardar</param>
        /// <param name="mensaje">Mensaje a guardar</param>
        /// <param name="args">Argumento(s) a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Error(OEnumModulosSistema modulo, string remitente, Exception excepcion, string mensaje, params object[] args)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Error(excepcion, mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente, args);
            LogExcepcion.Error(excepcion, mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente, args);
            MensajeError(excepcion);
        }

        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="mensaje">Mensaje a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Fatal(OEnumModulosSistema modulo, string remitente, string mensaje)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Fatal(mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
            LogExcepcion.Fatal(mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
            MensajeFatal(mensaje);
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="excepcion">Excepción a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Fatal(OEnumModulosSistema modulo, string remitente, Exception excepcion)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Fatal(excepcion, "Error", (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
            LogExcepcion.Fatal(excepcion, "Error", (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
            MensajeFatal(excepcion);
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="objeto">Objeto a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Fatal(OEnumModulosSistema modulo, string remitente, object objeto)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Fatal("Dato", objeto, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
            LogExcepcion.Fatal("Dato", objeto, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
            MensajeFatal(App.ToString(objeto));
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="excepcion">Excepción a guardar</param>
        /// <param name="mensaje">Mensaje a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Fatal(OEnumModulosSistema modulo, string remitente, Exception excepcion, string mensaje)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Fatal(excepcion, mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
            LogExcepcion.Fatal(excepcion, mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente);
            MensajeFatal(excepcion);
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="mensaje">Mensaje a guardar</param>
        /// <param name="args">Argumento(s) a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Fatal(OEnumModulosSistema modulo, string remitente, string mensaje, params object[] args)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Fatal(mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente, args);
            LogExcepcion.Fatal(mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente, args);
            MensajeFatal(mensaje);
        }
        /// <summary>
        /// Añade el registro log
        /// </summary>
        /// <param name="modulo">Código del módulo desde donde se realiza la llamada</param>
        /// <param name="remitente">Código del objeto que realiza la llamada</param>
        /// <param name="excepcion">Excepción a guardar</param>
        /// <param name="mensaje">Mensaje a guardar</param>
        /// <param name="args">Argumento(s) a guardar</param>
        /// <param name="NivelLog">Nivel del registro a guardar</param>
        public static void Fatal(OEnumModulosSistema modulo, string remitente, Exception excepcion, string mensaje, params object[] args)
        {
            ILogger logger = GetLog(modulo.Nombre);
            logger.Fatal(excepcion, mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente, args);
            LogExcepcion.Fatal(excepcion, mensaje, (modulo.Nombre == logger.Identificador ? modulo.Nombre : string.Format("{0}({1})", logger.Identificador, modulo.Nombre)), remitente, args);
            MensajeFatal(excepcion);
        }

        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Busca el log con determinado nombre
        /// </summary>
        /// <param name="nombre">Nombre del log</param>
        /// <returns>ILogger seleccionado</returns>
        private static ILogger GetLog(string nombre) 
        {
            ILogger logger = LogDefecto;
            if (Loggers.ContainsKey(nombre))
            {
                logger = Loggers[nombre];
            }
            return logger;
        }

        /// <summary>
        /// Muestra un mensaje de información
        /// </summary>
        /// <param name="excepcion">Información a mostrar</param>
        private static void MensajeError(Exception excepcion)
        {
            // Se mostrará un mensaje únicamente cuando se está iniciando el sistema o cuando se produce un error fatal. Ambos si se está en modo debug
            bool mostrarMensaje = IniciandoSistema;
            #if DEBUG
            mostrarMensaje = true;
            #endif
            if (mostrarMensaje)
            {
                if (ShowExceptionDelegate != null)
                {
                    if (!OThreadManager.EjecucionEnTrheadPrincipal())
                    {
                        OThreadManager.SincronizarConThreadPrincipal(new OExceptionDelegate(ShowExceptionDelegate), new object[] { excepcion });
                    }
                    else
                    {
                        ShowExceptionDelegate(excepcion);
                    }
                }
            }
        }
        /// <summary>
        /// Muestra un mensaje de información
        /// </summary>
        /// <param name="informacion">Información a mostrar</param>
        private static void MensajeError(string informacion)
        {
            // Se mostrará un mensaje únicamente cuando se está iniciando el sistema o cuando se produce un error fatal. Ambos si se está en modo debug
            bool mostrarMensaje = IniciandoSistema;
            #if DEBUG
            mostrarMensaje = true;
            #endif
            if (mostrarMensaje)
            {
                if (ShowMessageDelegate != null)
                {
                    if (!OThreadManager.EjecucionEnTrheadPrincipal())
                    {
                        OThreadManager.SincronizarConThreadPrincipal(new OMessageDelegate(ShowMessageDelegate), new object[] { informacion });
                    }
                    else
                    {
                        ShowMessageDelegate(informacion);
                    }
                }
            }
        }

        /// <summary>
        /// Muestra un mensaje de información
        /// </summary>
        /// <param name="excepcion">Información a mostrar</param>
        private static void MensajeFatal(Exception excepcion)
        {
            if (ShowExceptionDelegate != null)
            {
                if (!OThreadManager.EjecucionEnTrheadPrincipal())
                {
                    OThreadManager.SincronizarConThreadPrincipal(new OExceptionDelegate(ShowExceptionDelegate), new object[] { excepcion });
                }
                else
                {
                    ShowExceptionDelegate(excepcion);
                }

            }
        }
        /// <summary>
        /// Muestra un mensaje de información
        /// </summary>
        /// <param name="informacion">Información a mostrar</param>
        private static void MensajeFatal(string informacion)
        {
            if (ShowMessageDelegate != null)
            {
                if (!OThreadManager.EjecucionEnTrheadPrincipal())
                {
                    OThreadManager.SincronizarConThreadPrincipal(new OMessageDelegate(ShowMessageDelegate), new object[] { informacion });
                }
                else
                {
                    ShowMessageDelegate(informacion);
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// Define el conjunto de módulos del sistema
    /// </summary>
    public class OModulosSistema : OEnumeradosHeredable
    {
        #region Atributo(s)
        /// <summary>
        /// Módulo de funciones comunes del sistema. 
        /// </summary>
        public static OEnumModulosSistema Comun = new OEnumModulosSistema("Común", "Módulo de funciones comunes", 1);
        /// <summary>
        /// Módulo de funciones propias del inicio y la finalización del sistema. 
        /// </summary>
        public static OEnumModulosSistema Sistema = new OEnumModulosSistema("Sistema", "Módulo de funciones propias del inicio y la finalización del sistema", 2);
        /// <summary>
        /// Módulo de funciones accesos a bases de datos. 
        /// </summary>
        public static OEnumModulosSistema BBDD = new OEnumModulosSistema("BBDD", "Módulo de funciones de acceso a bases de datos", 3);
        /// <summary>
        /// Módulo de cronómetros del sistema. 
        /// </summary>
        public static OEnumModulosSistema Cronometros = new OEnumModulosSistema("Cronómetros", "Módulo de cronómetros del sistema", 4);
        /// <summary>
        /// Módulo de Threads del sistema. 
        /// </summary>
        public static OEnumModulosSistema Threads = new OEnumModulosSistema("Threads", "Módulo de control de hilos del sistema", 5);
        /// <summary>
        /// Módulo de monitorización del sistema. 
        /// </summary>
        public static OEnumModulosSistema Monitorizacion = new OEnumModulosSistema("Monitorización", "Módulo de monitorización de hilos del sistema", 6);
        /// <summary>
        /// Módulo de protección del código. 
        /// </summary>
        public static OEnumModulosSistema Proteccion = new OEnumModulosSistema("Protección", "Módulo de protección del código", 7);
        /// <summary>
        /// Módulo de gestión de formularios. 
        /// </summary>
        public static OEnumModulosSistema Escritorios = new OEnumModulosSistema("Escritorios", "Módulo de gestión de formularios", 8);
        /// <summary>
        /// Módulo de funciones base para las imagenes y gráficos
        /// </summary>
        public static OEnumModulosSistema ImagenGraficos = new OEnumModulosSistema("ImagenGraficos", "Módulo de funciones base para las imagenes y gráficos", 9);
        /// <summary>
        /// Módulo de funciones base para el almacenamientod de información
        /// </summary>
        public static OEnumModulosSistema AlmacenInformacion = new OEnumModulosSistema("AlmacenInformacion", "Módulo de funciones base para el almacenamientod de información", 10);
        /// <summary>
        /// Módulo de funciones encargadas de monitorizar la creación y destrucción de objetos de origen propio que ocupan gran cantidad de memoria y de creación frecuente, así como la gestión del colector de basura de .net
        /// </summary>
        public static OEnumModulosSistema GestionMemoria = new OEnumModulosSistema("GestionMemoria", "Módulo de funciones encargadas de monitorizar la creación y destrucción de objetos de origen propio que ocupan gran cantidad de memoria y de creación frecuente, así como la gestión del colector de basura de .net", 11);
        #endregion
    }

    /// <summary>
    /// Clase que implementa el enumerado de los módulos del sistema
    /// </summary>
    public class OEnumModulosSistema : OEnumeradoHeredable
    {
        #region Constructor
        /// <summary>
        /// Constuctor de la clase
        /// </summary>
        public OEnumModulosSistema(string nombre, string descripcion, int valor) :
            base(nombre, descripcion, valor)
        { }
        #endregion
    }

    /// <summary>
    /// Opciones de configuración del registro de eventos
    /// </summary>
    [Serializable]
    internal class OOpcionesLogs
    {
        #region Atributo(s)
        /// <summary>
        /// Ruta de registro de eventos
        /// </summary>
        public string Path = "Registro de eventos";

        /// <summary>
        /// Fichero de registro de eventos
        /// </summary>
        public string Fichero = "Eventos";

        /// <summary>
        /// Extensión de registro de eventos
        /// </summary>
        public string Extension = "Log";

        /// <summary>
        /// Separador de los campos del registro de eventos
        /// </summary>
        public string Separador = ";";

        /// <summary>
        /// Cadencia de backup
        /// </summary>
        [XmlIgnore]
        public TimeSpan Inicio = TimeSpan.FromDays(1);

        /// <summary>
        /// Cadencia de backup
        /// </summary>
        public double InicioEnMinutos
        {
            get { return Inicio.TotalMinutes; }
            set { Inicio = TimeSpan.FromMinutes(value); }
        }

        /// <summary>
        /// Cadencia de backup
        /// </summary>
        [XmlIgnore]
        public TimeSpan Retardo = TimeSpan.FromDays(1);

        /// <summary>
        /// Cadencia de backup
        /// </summary>
        public double RetardoEnMinutos
        {
            get { return Retardo.TotalMinutes; }
            set { Retardo = TimeSpan.FromMinutes(value); }
        }

        /// <summary>
        /// Ruta de las copias de seguridad del registro de eventos
        /// </summary>
        public string PathBackup = "Backup de eventos";

        /// <summary>
        /// Subruta de las copias de seguridad del registro de eventos
        /// </summary>
        public Mascara MascaraSubPathBackup = Mascara.AñoMesDia;

        /// <summary>
        /// Lista de los logs del sistema
        /// </summary>
        public List<OOpcionesLog> OpcionesLog;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OOpcionesLogs()
        {
            this.OpcionesLog = new List<OOpcionesLog>();
        }
        #endregion
    }

    /// <summary>
    /// Opciones de configuración del registro de eventos
    /// </summary>
    [Serializable]
    internal class OOpcionesLog
    {
        #region Atributo(s)
        /// <summary>
        /// Identificador del log
        /// </summary>
        public string Identificador = string.Empty;

        /// <summary>
        /// Nivel a partir del cual se guardan los logs
        /// </summary>        
        public NivelLog NivelLog = NivelLog.Fatal; 
        #endregion
    }
}