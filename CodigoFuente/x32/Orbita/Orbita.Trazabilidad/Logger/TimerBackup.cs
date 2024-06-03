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
namespace Orbita.Trazabilidad
{
    /// <summary>
    /// Timer Backup.
    /// </summary>
    public class TimerBackup
    {
        #region Atributos privados
        /// <summary>
        /// System.TimeSpan que representa el período tiempo de retraso antes de que
        /// se llame al método de devolución de llamada que se especificó cuando se creó
        /// System.Threading.Timer.
        /// </summary>
        TimeSpan hora;
        /// <summary>
        /// Período de tiempo entre invocaciones del método de llamada especificado en
        /// el momento de la construcción de System.Threading.Timer.
        /// </summary>
        TimeSpan periodo;
        #endregion

        #region Constructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.TimerBackup.
        /// </summary>
        public TimerBackup()
            : this(new TimeSpan(23, 0, 0), new TimeSpan(1, 0, 0, 0)) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.TimerBackup.
        /// </summary>
        /// <param name="hora">System.TimeSpan que representa el período tiempo de retraso antes de que se
        /// llame al método de devolución de llamada que se especificó cuando se creó System.Threading.Timer.</param>
        public TimerBackup(TimeSpan hora)
            : this(hora, new TimeSpan(1, 0, 0, 0)) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.TimerBackup.
        /// </summary>
        /// <param name="hora">System.TimeSpan que representa el período tiempo de retraso antes de que se
        /// llame al método de devolución de llamada que se especificó cuando se creó System.Threading.Timer.</param>
        /// <param name="periodo">Período de tiempo entre invocaciones del método de llamada especificado en
        /// el momento de la construcción de System.Threading.Timer.</param>
        public TimerBackup(TimeSpan hora, TimeSpan periodo)
        {
            this.hora = hora;
            this.periodo = periodo;
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// System.TimeSpan que representa el período tiempo de retraso antes de que se llame al método 
        /// de devolución de llamada que se especificó cuando se creó System.Threading.Timer.
        /// </summary>
        public TimeSpan Hora
        {
            get { return this.hora; }
            set { this.hora = value; }
        }
        /// <summary>
        /// Período de tiempo entre invocaciones del método de llamada especificado en el momento de la
        /// construcción de System.Threading.Timer.
        /// </summary>
        public TimeSpan Periodo
        {
            get { return this.periodo; }
            set { this.periodo = value; }
        }
        #endregion
    }
}