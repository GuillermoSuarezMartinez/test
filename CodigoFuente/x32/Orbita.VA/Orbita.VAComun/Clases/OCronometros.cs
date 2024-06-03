//***********************************************************************
// Assembly         : Orbita.VAComun
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 16-11-2012
// Description      : Añadido nuevo método ExisteCronometro
//                    Utilización interna de este nuevo método
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Orbita.VAComun
{
    /// <summary>
    /// Clase estática para el acceso a los cronómetros que se están ejecutandose en el sistema
    /// </summary>
    public static class OCronometrosManager
    {
        #region Atributo(s)
        /// <summary>
        /// Listado de todos los cronómetros necesarios para el cómputo de tiempos de proceso
        /// </summary>
        public static List<OCronometro> ListaCronometros;

        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Constructor de los campos de la clase
        /// </summary>
        public static void Constructor()
        {
            ListaCronometros = new List<OCronometro>();
        }

        /// <summary>
        /// Destruye los campos de la clase
        /// </summary>
        public static void Destructor()
        {
            ListaCronometros = null;
        }

        /// <summary>
        /// Inicializa las variables necesarias para el funcionamiento de los cronómetros
        /// </summary>
        public static void Inicializar()
        {
        }

        /// <summary>
        /// Finaliza la ejecución
        /// </summary>
        public static void Finalizar()
        {
        }

        /// <summary>
        /// Añade un nuevo cronómetro al sistema
        /// </summary>
        /// <param name="codigo">Código identificador del cronómetro</param>
        /// <param name="nombre">Nombre del cronómetro</param>
        /// <param name="descripcion">Texto descriptivo del cronómetro</param>
        public static void NuevoCronometro(string codigo, string nombre, string descripcion)
        {
            // Si el cronómetro no existe se crea
            if (!ExisteCronometro(codigo))
            {
                // Creación del nuevo cronómetro
                OCronometro cronometro = new OCronometro(codigo, nombre, descripcion);

                // Añadimos a la lista de máquinas de estado
                if (ListaCronometros != null)
                {
                    ListaCronometros.Add(cronometro);
                }
            }
        }

        public static bool ExisteCronometro(string codigo)
        {
            return ListaCronometros.Exists(delegate(OCronometro c) { return c.Codigo == codigo;});
        }

        public static OCronometro BuscaCronometro(string codigo)
        {
            return ListaCronometros.Find(delegate(OCronometro c) { return c.Codigo == codigo; });
        }

        /// <summary>
        /// Inicia la cuenta de determinado cronometro
        /// </summary>
        public static void Start(string codigo)
        {
            OCronometro cronometro = BuscaCronometro(codigo);
            if (cronometro != null)
            {
                cronometro.Start();
            }
        }

        /// <summary>
        /// Inicia la cuenta de forma pausada de determinado cronometro
        /// </summary>
        public static void StartPaused(string codigo)
        {
            OCronometro cronometro = BuscaCronometro(codigo);
            if (cronometro != null)
            {
                cronometro.StartPaused();
            }
        }

        /// <summary>
        /// Detiene la cuenta de determinado cronometro
        /// </summary>
        public static void Stop(string codigo)
        {
            OCronometro cronometro = BuscaCronometro(codigo);
            if (cronometro != null)
            {
                cronometro.Stop();
            }
        }

        /// <summary>
        /// Pausa el cronometro
        /// </summary>
        public static void Pause(string codigo)
        {
            OCronometro cronometro = BuscaCronometro(codigo);
            if (cronometro != null)
            {
                cronometro.Pause();
            }
        }

        /// <summary>
        /// Reanuda el cronometro
        /// </summary>
        public static void Resume(string codigo)
        {
            OCronometro cronometro = BuscaCronometro(codigo);
            if (cronometro != null)
            {
                cronometro.Resume();
            }
        }

        /// <summary>
        /// Duración de la última ejecución
        /// </summary>
        public static TimeSpan DuracionUltimaEjecucion(string codigo)
        {
            TimeSpan resultado = new TimeSpan();
            OCronometro cronometro = BuscaCronometro(codigo);
            if (cronometro != null)
            {
                resultado = cronometro.DuracionUltimaEjecucion;
            }
            return resultado;
        }
        #endregion
    }

    /// <summary>
    /// Clase que implementa un cronómetro para la medición de tiempos
    /// </summary>
    public class OCronometro
    {
        #region Atributo(s)
        /// <summary>
        /// Cronómetro de la ejecución del estado
        /// </summary>
        private Stopwatch CronometroEjecucion;
        /// <summary>
        /// Estado del cronómetro
        /// </summary>
        public OEstadoCronometro EstadoCronometro;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Código identificativo del cronómetro.
        /// </summary>
        private string _Codigo;
        /// <summary>
        /// Código identificativo del cronómetro.
        /// </summary>
        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }

        /// <summary>
        /// Nombre del cronómetro.
        /// </summary>
        private string _Nombre;
        /// <summary>
        /// Nombre del cronómetro.
        /// </summary>
        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        /// <summary>
        /// Texto descriptivo.
        /// </summary>
        private string _Descripcion;
        /// <summary>
        /// Texto descriptivo.
        /// </summary>
        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }

        /// <summary>
        /// Contador del número de veces que se ha realizado una medición de tiempos
        /// </summary>
        private long _ContadorEjecuciones;
        /// <summary>
        /// Contador del número de veces que se ha realizado una medición de tiempos
        /// </summary>
        public long ContadorEjecuciones
        {
            get { return _ContadorEjecuciones; }
            set { _ContadorEjecuciones = value; }
        }

        /// <summary>
        /// Duración promedio del tiempo que ha contabilizado el cronómetro
        /// </summary>
        public TimeSpan DuracionPromedioEjecucion
        {
            get
            {
                if (this.ContadorEjecuciones > 0)
                {
                    return TimeSpan.FromMilliseconds(this.DuracionTotalEjecucion.TotalMilliseconds / (double)this.ContadorEjecuciones);
                }
                return new TimeSpan();
            }
        }
        /// <summary>
        /// Duración promedio del tiempo que ha contabilizado el cronómetro
        /// </summary>
        public string StrDuracionPromedioEjecucion
        {
            get
            {
                return this.DuracionPromedioEjecucion.TotalMilliseconds.ToString("#0.000");
            }
        }

        /// <summary>
        /// Duración del tiempo acumulado que ha contabilizado el cronómetro
        /// </summary>
        private TimeSpan _DuracionTotalEjecucion;
        /// <summary>
        /// Duración del tiempo acumulado que ha contabilizado el cronómetro
        /// </summary>
        public TimeSpan DuracionTotalEjecucion
        {
            get
            {
                if (this.CronometroEjecucion.IsRunning)
                {
                    return this._DuracionTotalEjecucion + this.DuracionUltimaEjecucion;
                }
                else
                {
                    return this._DuracionTotalEjecucion;
                }
            }
        }
        /// <summary>
        /// Duración del tiempo acumulado que ha contabilizado el cronómetro
        /// </summary>
        public string StrDuracionTotalEjecucion
        {
            get
            {
                return this.DuracionTotalEjecucion.TotalMilliseconds.ToString("#0.000");
            }
        }

        /// <summary>
        /// Momento en el que se ejecutó por última vez
        /// </summary>
        private DateTime _MomentoUltimaEjecucion;
        /// <summary>
        /// Momento en el que se ejecutó por última vez
        /// </summary>
        public DateTime MomentoUltimaEjecucion
        {
            get { return _MomentoUltimaEjecucion; }
            set { _MomentoUltimaEjecucion = value; }
        }

        /// <summary>
        /// Duración de la última ejecución
        /// </summary>
        public TimeSpan DuracionUltimaEjecucion
        {
            get
            {
                return this.CronometroEjecucion.Elapsed;
            }
        }
        /// <summary>
        /// Duración del tiempo acumulado que ha contabilizado el cronómetro
        /// </summary>
        public string StrDuracionUltimaEjecucion
        {
            get
            {
                return this.DuracionUltimaEjecucion.TotalMilliseconds.ToString("#0.000");
            }
        }

        /// <summary>
        /// Indica si el contador está ejecutandose
        /// </summary>
        public bool Ejecutando
        {
            get { return this.CronometroEjecucion.IsRunning; }
        }
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OCronometro(string codigo, string nombre, string descripcion)
        {
            this._Codigo = codigo;
            this._Nombre = nombre;
            this._Descripcion = descripcion;
            this._ContadorEjecuciones = 0;
            this.EstadoCronometro = OEstadoCronometro.Detenido;
            this.MomentoUltimaEjecucion = DateTime.Now;
            this.CronometroEjecucion = new Stopwatch();
            this._DuracionTotalEjecucion = new TimeSpan();
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OCronometro()
        {
            this._Codigo = string.Empty;
            this._Nombre = string.Empty;
            this._Descripcion = string.Empty;
            this._ContadorEjecuciones = 0;
            this.EstadoCronometro = OEstadoCronometro.Detenido;
            this.MomentoUltimaEjecucion = DateTime.Now;
            this.CronometroEjecucion = new Stopwatch();
            this._DuracionTotalEjecucion = new TimeSpan();
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Inicia el cronometro
        /// </summary>
        public void Start()
        {
            if (this.EstadoCronometro == OEstadoCronometro.Detenido)
            {
                this.ContadorEjecuciones++;
                this._MomentoUltimaEjecucion = DateTime.Now;
                this.CronometroEjecucion.Reset();
                this.CronometroEjecucion.Start();
                this.EstadoCronometro = OEstadoCronometro.Iniciado;
            }
        }
        /// <summary>
        /// Inicia el cronometro de forma pausada
        /// </summary>
        public void StartPaused()
        {
            if (this.EstadoCronometro == OEstadoCronometro.Detenido)
            {
                this.ContadorEjecuciones++;
                this._MomentoUltimaEjecucion = DateTime.Now;
                this.CronometroEjecucion.Reset();
                this.EstadoCronometro = OEstadoCronometro.Pausado;
            }
        }
        /// <summary>
        /// Detiene el cronometro
        /// </summary>
        public void Stop()
        {
            if ((this.EstadoCronometro == OEstadoCronometro.Iniciado) || (this.EstadoCronometro == OEstadoCronometro.Pausado))
            {
                this.CronometroEjecucion.Stop();
                this._DuracionTotalEjecucion += this.DuracionUltimaEjecucion;
                this.EstadoCronometro = OEstadoCronometro.Detenido;
            }
        }
        /// <summary>
        /// Pausa el cronometro
        /// </summary>
        public void Pause()
        {
            if (this.EstadoCronometro == OEstadoCronometro.Iniciado)
            {
                this.CronometroEjecucion.Stop();
                this.EstadoCronometro = OEstadoCronometro.Pausado;
            }
        }
        /// <summary>
        /// Reanuda el cronometro
        /// </summary>
        public void Resume()
        {
            if (this.EstadoCronometro == OEstadoCronometro.Pausado)
            {
                this.CronometroEjecucion.Start();
                this.EstadoCronometro = OEstadoCronometro.Iniciado;
            }
        }

        #endregion
    }

    /// <summary>
    /// Enumerado que describe el esatado de un cronómetro
    /// </summary>
    public enum OEstadoCronometro
    {
        /// <summary>
        /// Detenido
        /// </summary>
        [OStringValue("Detenido")]
        Detenido,
        /// <summary>
        /// Pausado
        /// </summary>
        [OStringValue("Pausado")]
        Pausado,
        /// <summary>
        /// Iniciado
        /// </summary>
        [OStringValue("Iniciado")]
        Iniciado
    }
}
