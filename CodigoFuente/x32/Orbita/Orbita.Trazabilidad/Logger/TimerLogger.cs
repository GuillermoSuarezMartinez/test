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
    /// Timer Logger.
    /// </summary>
    public class TimerLogger
    {
        #region Atributos privados
        /// <summary>
        /// Proporciona un mecanismo para ejecutar métodos en intervalos especificados.
        /// </summary>
        System.Threading.Timer timer;
        /// <summary>
        /// Representa el método que controla las llamadas de un System.Threading.Timer.
        /// </summary>
        System.Threading.TimerCallback callBack;
        #endregion

        #region Constructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.TimerLogger.
        /// </summary>
        public TimerLogger() { }
        #endregion

        #region Propiedades
        /// <summary>
        /// Proporciona un mecanismo para ejecutar métodos en intervalos especificados.
        /// </summary>
        public System.Threading.Timer Timer
        {
            get { return this.timer; }
            set { this.timer = value; }
        }
        /// <summary>
        /// Representa el método que controla las llamadas de un System.Threading.Timer.
        /// </summary>
        public System.Threading.TimerCallback CallBack
        {
            get { return this.callBack; }
            set { this.callBack = value; }
        }
        #endregion

        #region Métodos públicos
        /// <summary>
        /// Sobrecarga del método change correspondiente.
        /// </summary>
        /// <param name="hora">System.TimeSpan que representa el período tiempo de retraso antes de que
        /// se llame al método de devolución de llamada que se especificó cuando se creó
        /// System.Threading.Timer.</param>
        /// <param name="periodo">Período de tiempo entre invocaciones del método de llamada especificado en
        /// el momento de la construcción de System.Threading.Timer.</param>
        public void Change(TimeSpan hora, TimeSpan periodo)
        {
            this.timer.Change(Add(hora), periodo);
        }
        #endregion

        #region Métodos privados estáticos
        /// <summary>
        /// Configuración del período de tiempo de retraso y entre invocaciones.
        /// </summary>
        /// <param name="hora">System.TimeSpan que representa el período tiempo de retraso antes de que
        /// se llame al método de devolución de llamada que se especificó cuando se creó
        /// System.Threading.Timer.</param>
        /// <returns>TimeSpan.</returns>
        static TimeSpan Add(TimeSpan hora)
        {
            TimeSpan resultado = TimeSpan.MinValue;
            // La función Change de Timer se lee: ...en prosa: 
            // ejecutate dentro de (tiempo) y luego cada (tiempo).

            // Por tanto, primero hay que calcular a partir de la hora 
            // actual, el tiempo que falta para ejecutarse.

            // Primero definir la fecha actual con la hora de ejecución.
            DateTime fechaHora = DateTime.Now;
            // Concatenar a la fecha la hora de ejecución.
            DateTime fechaHoraEjecucion = new DateTime(fechaHora.Year, fechaHora.Month, fechaHora.Day, hora.Hours, hora.Minutes, hora.Seconds);
            if (fechaHoraEjecucion > fechaHora)
            {
                // Si la fecha de ejecución calculada es posterior a la actual.
                // Hacer la resta de ambas y el resultado es el tiempo que falta
                // para ejecutarse.
                resultado = fechaHoraEjecucion.Subtract(fechaHora);
            }
            else
            {
                // Si la fecha de ejecución calculada es negativa.
                // Se debe hacer una adición a la fecha hasta que
                // esta sea positiva.
                resultado = fechaHoraEjecucion.AddDays(1).Subtract(fechaHora);
            }
            return resultado;
        }
        #endregion
    }
}