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
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
namespace Orbita.Trazabilidad
{
    /// <summary>
    /// Remoting Logger.
    /// </summary>
    public class RemotingLogger : BaseLogger
    {
        #region Atributos privados
        /// <summary>
        /// Crea y controla un subproceso, establece su prioridad y obtiene su estado.
        /// </summary>
        Thread hilo;
        /// <summary>
        /// Cliente remoting.
        /// </summary>
        IRemotingLogger cliente;
        /// <summary>
        /// Colección de items.
        /// </summary>
        List<ItemLog> lista;
        #endregion

        #region Constructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>Alias=logger</c>, <c>Puerto=1440</c>, <c>Máquina=localhost</c>.
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        public RemotingLogger(string identificador)
            : this(identificador, NivelLog.Debug) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// Por defecto, <c>Alias=logger</c>, <c>Puerto=1440</c>, <c>Máquina=localhost</c>.
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        /// <param name="nivelLog">Nivel de logger.</param>
        public RemotingLogger(string identificador, NivelLog nivelLog)
            : this(identificador, nivelLog, Orbita.Trazabilidad.Logger.Alias) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>Alias=logger</c>, <c>Máquina=localhost</c>.
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        /// <param name="puerto">Puerto del URI de conexión .NET remoting.</param>
        public RemotingLogger(string identificador, int puerto)
            : this(identificador, NivelLog.Debug, puerto) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// Por defecto, <c>Alias=logger</c>, <c>Máquina=localhost</c>.
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        /// <param name="nivelLog">Nivel de logger.</param>
        /// <param name="puerto">Puerto del URI de conexión .NET remoting.</param>
        public RemotingLogger(string identificador, NivelLog nivelLog, int puerto)
            : this(identificador, nivelLog, Orbita.Trazabilidad.Logger.Alias, puerto) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>Alias=logger</c>.
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        /// <param name="puerto">Puerto del URI de conexión .NET remoting.</param>
        /// <param name="maquina">Host de la máquina cliente de conexión .NET remoting.</param>
        public RemotingLogger(string identificador, int puerto, string maquina)
            : this(identificador, NivelLog.Debug, puerto, maquina) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// Por defecto, <c>Alias=logger</c>.
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        /// <param name="nivelLog">Nivel de logger.</param>
        /// <param name="puerto">Puerto del URI de conexión .NET remoting.</param>
        /// <param name="maquina">Host de la máquina cliente de conexión .NET remoting.</param>
        public RemotingLogger(string identificador, NivelLog nivelLog, int puerto, string maquina)
            : this(identificador, nivelLog, Orbita.Trazabilidad.Logger.Alias, puerto, maquina) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>Puerto=1440</c>, <c>Máquina=localhost</c>.
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        /// <param name="alias">Alias del URI de conexión .NET remoting.</param>
        public RemotingLogger(string identificador, string alias)
            : this(identificador, NivelLog.Debug, alias) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// Por defecto, <c>Puerto=1440</c>, <c>Máquina=localhost</c>.
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        /// <param name="nivelLog">Nivel de logger.</param>
        /// <param name="alias">Alias del URI de conexión .NET remoting.</param>
        public RemotingLogger(string identificador, NivelLog nivelLog, string alias)
            : this(identificador, nivelLog, alias, Orbita.Trazabilidad.Logger.Puerto) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// Por defecto, <c>NivelLog=Debug</c>, <c>Máquina=localhost</c>.
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        /// <param name="alias">Alias del URI de conexión .NET remoting.</param>
        /// <param name="puerto">Puerto del URI de conexión .NET remoting.</param>
        public RemotingLogger(string identificador, string alias, int puerto)
            : this(identificador, NivelLog.Debug, alias, puerto) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// Por defecto, <c>Máquina=localhost</c>.
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        /// <param name="nivelLog">Nivel de logger.</param>
        /// <param name="alias">Alias del URI de conexión .NET remoting.</param>
        /// <param name="puerto">Puerto del URI de conexión .NET remoting.</param>
        public RemotingLogger(string identificador, NivelLog nivelLog, string alias, int puerto)
            : this(identificador, nivelLog, alias, puerto, Dns.GetHostName()) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// Por defecto, <c>NivelLog=Debug</c>. 
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        /// <param name="alias">Alias del URI de conexión .NET remoting.</param>
        /// <param name="puerto">Puerto del URI de conexión .NET remoting.</param>
        /// <param name="maquina">Host de la máquina cliente de conexión .NET remoting.</param>
        public RemotingLogger(string identificador, string alias, int puerto, string maquina)
            : this(identificador, NivelLog.Debug, alias, puerto, maquina) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.RemotingLogger.
        /// </summary>
        /// <param name="identificador">Identificador del logger.</param>
        /// <param name="nivelLog">Nivel de logger.</param>
        /// <param name="alias">Alias del URI de conexión .NET remoting.</param>
        /// <param name="puerto">Puerto del URI de conexión .NET remoting.</param>
        /// <param name="maquina">Host de la máquina cliente de conexión .NET remoting.</param>
        public RemotingLogger(string identificador, NivelLog nivelLog, string alias, int puerto, string maquina)
            : base(identificador, nivelLog)
        {
            // Crear la cadena URL de conexión.
            string url = string.Format(CultureInfo.CurrentCulture, @"tcp://{0}:{1}/{2}", maquina, puerto, alias);
            // Registrar el canal de conexión con los servicios de canal.
            ChannelServices.RegisterChannel(new TcpClientChannel(alias, null), false);
            // Crea un proxy para el objeto conocido indicado por el tipo y dirección URL especificados.
            this.cliente = (IRemotingLogger)Activator.GetObject(typeof(IRemotingLogger), url);
            // Crear la colección de items.
            this.lista = new List<ItemLog>();
            // Inicializar e iniciar el hilo de proceso de copias backup.
            this.hilo = new Thread(new ThreadStart(Procesar));
            this.hilo.IsBackground = true;
            this.hilo.Start();
        }
        #endregion

        #region Métodos públicos
        /// <summary>
        /// Registra un elemento determinado.
        /// </summary>
        /// <param name="item">El elemento que va a ser registrado.</param>
        public override void Log(ItemLog item)
        {
            lock (this)
            {
                // Añadir el item a la colección.
                this.lista.Add(item);
                // Activar la sincronización mediante un monitor.
                Monitor.Pulse(this);
            }
        }
        /// <summary>
        /// Registra un elemento determinado con parámetros adicionales.
        /// </summary>
        /// <param name="item">El elemento que va a ser registrado.</param>
        /// <param name="args">Parámetros adicionales.</param>
        public override void Log(ItemLog item, object[] args)
        {
            Log(item);
        }
        #endregion

        #region Métodos privados
        /// <summary>
        /// Añade a la colección de remoting el item (mensaje).
        /// </summary>
        void Procesar()
        {
            while (true)
            {
                ItemLog[] mensajes;
                lock (this)
                {
                    while (this.lista.Count == 0)
                    {
                        Monitor.Wait(this);
                    }
                    mensajes = this.lista.ToArray();
                    this.lista.Clear();
                }
                this.cliente.Log(mensajes);
            }
        }
        #endregion
    }
}
