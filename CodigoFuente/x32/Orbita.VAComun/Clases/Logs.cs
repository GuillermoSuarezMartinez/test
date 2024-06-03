//***********************************************************************
// Assembly         : Orbita.VAComun
// Author           : aibañez
// Created          : 06-09-2012
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
    public static class LogsRuntime
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
        public static MessageDelegate ShowMessageDelegate;

        /// <summary>
        /// Delegado utilizado para lanzar mensajes 
        /// </summary>
        public static ExceptionDelegate ShowExceptionDelegate;
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Construye los objetos
        /// </summary>
        public static void Constructor(MessageDelegate showMessageEvent, ExceptionDelegate showExceptionDelegate)
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
            OpcionesLogs opcionesLogs = SystemRuntime.Configuracion.OpcionesLogs;

            // Creamos el logger por defecto
            LogDefecto = LogManager.SetDebugLogger("Defecto", 
                NivelLog.Info,
                new PathLogger(Path.Combine(RutaParametrizable.AppFolder, opcionesLogs.Path), opcionesLogs.Fichero, opcionesLogs.Extension),
                new TimerBackup(opcionesLogs.Inicio, opcionesLogs.Retardo),
                new PathBackup(Path.Combine(RutaParametrizable.AppFolder, opcionesLogs.PathBackup), opcionesLogs.MascaraSubPathBackup));
            (LogDefecto as DebugLogger).Separador = opcionesLogs.Separador;

            // Creamos el logger de errores
            LogExcepcion = LogManager.SetDebugLogger("Excepcion",
                NivelLog.Error,
                new PathLogger(RutaParametrizable.AppFolder, "Error", opcionesLogs.Extension));
            (LogExcepcion as DebugLogger).Separador = opcionesLogs.Separador;
            
            // Creamos los loggers para el ensamblado comun
            foreach (OpcionesLog opcionesLog in opcionesLogs.OpcionesLog)
            {
                ILogger logger = LogManager.SetDebugLogger(opcionesLog.Identificador,
                opcionesLog.NivelLog,
                new PathLogger(Path.Combine(RutaParametrizable.AppFolder, opcionesLogs.Path), opcionesLogs.Fichero, opcionesLogs.Extension),
                new TimerBackup(opcionesLogs.Inicio, opcionesLogs.Retardo),
                new PathBackup(Path.Combine(RutaParametrizable.AppFolder, opcionesLogs.PathBackup), opcionesLogs.MascaraSubPathBackup));
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
        public static void Debug(EnumModulosSistema modulo, string remitente, string mensaje)
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
        public static void Debug(EnumModulosSistema modulo, string remitente, Exception excepcion)
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
        public static void Debug(EnumModulosSistema modulo, string remitente, object objeto)
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
        public static void Debug(EnumModulosSistema modulo, string remitente, Exception excepcion, string mensaje)
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
        public static void Debug(EnumModulosSistema modulo, string remitente, string mensaje, params object[] args)
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
        public static void Debug(EnumModulosSistema modulo, string remitente, Exception excepcion, string mensaje, params object[] args)
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
        public static void Info(EnumModulosSistema modulo, string remitente, string mensaje)
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
        public static void Info(EnumModulosSistema modulo, string remitente, Exception excepcion)
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
        public static void Info(EnumModulosSistema modulo, string remitente, object objeto)
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
        public static void Info(EnumModulosSistema modulo, string remitente, Exception excepcion, string mensaje)
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
        public static void Info(EnumModulosSistema modulo, string remitente, string mensaje, params object[] args)
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
        public static void Info(EnumModulosSistema modulo, string remitente, Exception excepcion, string mensaje, params object[] args)
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
        public static void Warning(EnumModulosSistema modulo, string remitente, string mensaje)
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
        public static void Warning(EnumModulosSistema modulo, string remitente, Exception excepcion)
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
        public static void Warning(EnumModulosSistema modulo, string remitente, object objeto)
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
        public static void Warning(EnumModulosSistema modulo, string remitente, Exception excepcion, string mensaje)
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
        public static void Warning(EnumModulosSistema modulo, string remitente, string mensaje, params object[] args)
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
        public static void Warning(EnumModulosSistema modulo, string remitente, Exception excepcion, string mensaje, params object[] args)
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
        public static void Error(EnumModulosSistema modulo, string remitente, string mensaje)
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
        public static void Error(EnumModulosSistema modulo, string remitente, Exception excepcion)
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
        public static void Error(EnumModulosSistema modulo, string remitente, object objeto)
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
        public static void Error(EnumModulosSistema modulo, string remitente, Exception excepcion, string mensaje)
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
        public static void Error(EnumModulosSistema modulo, string remitente, string mensaje, params object[] args)
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
        public static void Error(EnumModulosSistema modulo, string remitente, Exception excepcion, string mensaje, params object[] args)
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
        public static void Fatal(EnumModulosSistema modulo, string remitente, string mensaje)
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
        public static void Fatal(EnumModulosSistema modulo, string remitente, Exception excepcion)
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
        public static void Fatal(EnumModulosSistema modulo, string remitente, object objeto)
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
        public static void Fatal(EnumModulosSistema modulo, string remitente, Exception excepcion, string mensaje)
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
        public static void Fatal(EnumModulosSistema modulo, string remitente, string mensaje, params object[] args)
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
        public static void Fatal(EnumModulosSistema modulo, string remitente, Exception excepcion, string mensaje, params object[] args)
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
                    ShowExceptionDelegate(excepcion);
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
                    ShowMessageDelegate(informacion);
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
                ShowExceptionDelegate(excepcion);
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
                ShowMessageDelegate(informacion);
            }
        }
        #endregion
    }

    /// <summary>
    /// Define el conjunto de módulos del sistema
    /// </summary>
    public class ModulosSistema : Enumerados
    {
        #region Atributo(s)
        /// <summary>
        /// Módulo de funciones comunes del sistema. 
        /// </summary>
        public static EnumModulosSistema Comun = new EnumModulosSistema("Común", "Módulo de funciones comunes", 1);
        /// <summary>
        /// Módulo de funciones propias del inicio y la finalización del sistema. 
        /// </summary>
        public static EnumModulosSistema Sistema = new EnumModulosSistema("Sistema", "Módulo de funciones propias del inicio y la finalización del sistema", 2);
        /// <summary>
        /// Módulo de funciones accesos a bases de datos. 
        /// </summary>
        public static EnumModulosSistema BBDD = new EnumModulosSistema("BBDD", "Módulo de funciones de acceso a bases de datos", 3);
        /// <summary>
        /// Módulo de cronómetros del sistema. 
        /// </summary>
        public static EnumModulosSistema Cronometros = new EnumModulosSistema("Cronómetros", "Módulo de cronómetros del sistema", 4);
        /// <summary>
        /// Módulo de Threads del sistema. 
        /// </summary>
        public static EnumModulosSistema Threads = new EnumModulosSistema("Threads", "Módulo de control de hilos del sistema", 5);
        /// <summary>
        /// Módulo de monitorización del sistema. 
        /// </summary>
        public static EnumModulosSistema Monitorizacion = new EnumModulosSistema("Monitorización", "Módulo de monitorización de hilos del sistema", 6);
        /// <summary>
        /// Módulo de protección del código. 
        /// </summary>
        public static EnumModulosSistema Proteccion = new EnumModulosSistema("Protección", "Módulo de protección del código", 7);
        /// <summary>
        /// Módulo de gestión de formularios. 
        /// </summary>
        public static EnumModulosSistema Escritorios = new EnumModulosSistema("Escritorios", "Módulo de gestión de formularios", 8);
        /// <summary>
        /// Módulo de funciones base para las imagenes y gráficos
        /// </summary>
        public static EnumModulosSistema ImagenGraficos = new EnumModulosSistema("ImagenGraficos", "Módulo de funciones base para las imagenes y gráficos", 9);
        /// <summary>
        /// Módulo de funciones base para el almacenamientod de información
        /// </summary>
        public static EnumModulosSistema AlmacenInformacion = new EnumModulosSistema("AlmacenInformacion", "Módulo de funciones base para el almacenamientod de información", 10);
        /// <summary>
        /// Módulo de funciones encargadas de monitorizar la creación y destrucción de objetos de origen propio que ocupan gran cantidad de memoria y de creación frecuente.
        /// </summary>
        public static EnumModulosSistema ManejadorObjetosGrandes = new EnumModulosSistema("ManejadorObjetosGrandes", "Módulo de funciones encargadas de monitorizar la creación y destrucción de objetos de origen propio que ocupan gran cantidad de memoria y de creación frecuente.", 11);
        #endregion
    }

    /// <summary>
    /// Clase que implementa el enumerado de los módulos del sistema
    /// </summary>
    public class EnumModulosSistema : Enumerado
    {
        #region Constructor
        /// <summary>
        /// Constuctor de la clase
        /// </summary>
        public EnumModulosSistema(string nombre, string descripcion, int valor) :
            base(nombre, descripcion, valor)
        { }
        #endregion
    }

    /// <summary>
    /// Opciones de configuración del registro de eventos
    /// </summary>
    [Serializable]
    public class OpcionesLogs
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
        public List<OpcionesLog> OpcionesLog;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OpcionesLogs()
        {
            this.OpcionesLog = new List<OpcionesLog>();
        }
        #endregion
    }

    /// <summary>
    /// Opciones de configuración del registro de eventos
    /// </summary>
    [Serializable]
    public class OpcionesLog
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