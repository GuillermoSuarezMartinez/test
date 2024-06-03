//***********************************************************************
// Assembly         : OrbitaUtiles
// Author           : crodriguez
// Created          : 03-03-2011
//
// Last Modified By : crodriguez
// Last Modified On : 03-03-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
namespace Orbita.Utiles
{
    /// <summary>
    ///  Proporciona un mecanismo para ejecutar métodos en intervalos especificados.
    /// </summary>
    public class OTimer
    {
        #region Atributo(s)
        /// <summary>
        /// Timer.
        /// </summary>
        System.Threading.Timer _timer;
        /// <summary>
        /// TimerCallback.
        /// </summary>
        System.Threading.TimerCallback _callBack;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Inicializar una nueva instancia de la clase OTimer.
        /// </summary>
        public OTimer() { }
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Timer.
        /// </summary>
        public System.Threading.Timer Timer
        {
            get { return this._timer; }
            set { this._timer = value; }
        }
        /// <summary>
        /// TimerCallback.
        /// </summary>
        public System.Threading.TimerCallback CallBack
        {
            get { return this._callBack; }
            set { this._callBack = value; }
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Método Change correspondiente, en el primer caso a la llamada a AddHoras, 
        /// AddMinutos, AddSegundos, en el segundo y tercer caso, correspondiente a la 
        /// llamada en un día concreto.
        /// </summary>
        /// <param name="unidadTiempo">Unidad de tiempo.</param>
        /// <param name="tiempo">Tiempo correspondiente a horas o minutos o segundos o 
        /// milisegundos.</param>
        /// <param name="hora">Hora del timespan.</param>
        /// <param name="minuto">Minuto del timespan.</param>
        /// <param name="segundo">Segundo del timespan.</param>
        /// <param name="milisegundo">Milisegundo del timespan.</param>
        public void Change(Tiempo unidadTiempo, int tiempo, int hora, int minuto, int segundo, int milisegundo)
        {
            switch (unidadTiempo)
            {
                case Tiempo.Hora:
                    this._timer.Change(AddHoras(tiempo, new TimeSpan(hora, minuto, segundo, milisegundo)), new TimeSpan(tiempo, 0, 0));
                    break;
                case Tiempo.Minuto:
                    this._timer.Change(AddMinutos(tiempo, new TimeSpan(hora, minuto, segundo, milisegundo)), new TimeSpan(0, tiempo, 0));
                    break;
                case Tiempo.Segundo:
                    this._timer.Change(AddSegundos(tiempo, new TimeSpan(hora, minuto, segundo, milisegundo)), new TimeSpan(0, 0, tiempo));
                    break;
                case Tiempo.Milisegundo:
                    this._timer.Change(AddSegundos(tiempo, new TimeSpan(hora, minuto, segundo, milisegundo)), new TimeSpan(0, 0, tiempo));
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Método Change correspondiente, en el primer caso a la llamada a AddHoras, 
        /// AddMinutos, AddSegundos, en el segundo y tercer caso, correspondiente a la 
        /// llamada en un día concreto.
        /// </summary>
        /// <param name="unidadTiempo"></param>
        /// <param name="tiempo"></param>
        /// <param name="hora"></param>
        /// <param name="minuto"></param>
        /// <param name="segundo"></param>
        public void Change(Tiempo unidadTiempo, int tiempo, int hora, int minuto, int segundo)
        {
            switch (unidadTiempo)
            {
                case Tiempo.Hora:
                    this._timer.Change(AddHoras(tiempo, new TimeSpan(hora, minuto, segundo)), new TimeSpan(tiempo, 0, 0));
                    break;
                case Tiempo.Minuto:
                    this._timer.Change(AddMinutos(tiempo, new TimeSpan(hora, minuto, segundo)), new TimeSpan(0, tiempo, 0));
                    break;
                case Tiempo.Segundo:
                    this._timer.Change(AddSegundos(tiempo, new TimeSpan(hora, minuto, segundo)), new TimeSpan(0, 0, tiempo));
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dia"></param>
        /// <param name="hora"></param>
        /// <param name="minuto"></param>
        /// <param name="segundo"></param>
        public void Change(int dia, int hora, int minuto, int segundo)
        {
            this._timer.Change(AddDias(dia, new TimeSpan(hora, minuto, segundo)), TimeSpan.Zero);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dia"></param>
        /// <param name="hora"></param>
        /// <param name="minuto"></param>
        /// <param name="segundo"></param>
        /// <param name="inversa"></param>
        public void Change(int dia, int hora, int minuto, int segundo, bool inversa)
        {
            this._timer.Change(AddDias(dia, new TimeSpan(hora, minuto, segundo), inversa), TimeSpan.Zero);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ts"></param>
        public void Change(TimeSpan ts)
        {
            this._timer.Change(ts, new TimeSpan(0, 0, 0, 0, -1));
        }
        #region Método(s) estático(s)
        /// <summary>
        /// Ejecutar a una hora concreta de un dia concreto y cada mes.
        /// La función se divide en 2 partes, de forma que no se utiliza el
        /// segundo parámetro del change, ya que, en cada ejecución del método
        /// se hace de nuevo una llamada al cálculo del tiempo.
        /// </summary>
        /// <param name="dia">Día de ejecución.</param>
        /// <param name="ts"></param>
        /// <param name="inverso"></param>
        /// <returns></returns>
        public static TimeSpan AddDias(int dia, TimeSpan ts, bool inverso)
        {
            // La función Change de Timer se lee: ...en prosa: 
            // ejecutate dentro de (tiempo) y luego cada (tiempo).

            // Esta función es equivalente a la segunda parte
            // de la prosa... ejecutate cada (tiempo).

            // Por tanto, primero hay que calcular a partir de la 
            // hora actual, el tiempo que falta para ejecutarse.

            // Primero definir la fecha actual con la hora de ejecución.
            DateTime fechaHora = DateTime.Now;
            // Concatenar a la fecha la hora de ejecución.
            DateTime fechaHoraEjecucion = new DateTime(fechaHora.Year, fechaHora.Month, dia, ts.Hours, ts.Minutes, ts.Seconds);
            
            return fechaHoraEjecucion.AddMonths(1).Subtract(fechaHora);
        }
        /// <summary>
        /// Ejecutar a una hora concreta de un dia concreto y cada mes.
        /// La función se  divide en 2 partes, de  forma que no se  utiliza el
        /// segundo parámetro del change, ya que, en cada ejecución del método
        /// se hace de nuevo una llamada al cálculo del tiempo.
        /// </summary>
        /// <param name="dia">Día de ejecución.</param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static TimeSpan AddDias(int dia, TimeSpan ts)
        {
            TimeSpan resultado = TimeSpan.MinValue;

            // La función Change de Timer se lee: ...en prosa: 
            // ejecutate dentro de (tiempo) y luego cada (tiempo).

            // Por tanto, primero hay que calcular a partir de la hora 
            // actual, el tiempo que falta para ejecutarse.

            // Primero definir la fecha actual con la hora de ejecución.
            DateTime fechaHora = DateTime.Now;
            // Concatenar a la fecha la hora de ejecución.
            DateTime fechaHoraEjecucion = new DateTime(fechaHora.Year, fechaHora.Month, dia, ts.Hours, ts.Minutes, ts.Seconds);

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
                do
                {
                    DateTime fechaHoraAdicion = fechaHoraEjecucion.AddMonths(1);
                    resultado = fechaHoraAdicion.Subtract(fechaHora);
                    fechaHoraEjecucion = fechaHoraAdicion;
                }
                while (resultado.Milliseconds < 0);
            }
            return resultado;
        }
        /// <summary>
        /// Ejecutar a una hora concreta y posteriormente cada n horas.
        /// </summary>
        /// <param name="tiempo"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static TimeSpan AddHoras(int tiempo, TimeSpan ts)
        {
            TimeSpan resultado = TimeSpan.MinValue;

            // La función Change de Timer se lee: ...en prosa: 
            // ejecutate dentro de (tiempo) y luego cada (tiempo).

            // Por tanto, primero hay que calcular a partir de la hora 
            // actual, el tiempo que falta para ejecutarse.

            // Primero definir la fecha actual con la hora de ejecución.
            DateTime fechaHora = DateTime.Now;
            // Concatenar a la fecha la hora de ejecución.
            DateTime fechaHoraEjecucion = new DateTime(fechaHora.Year, fechaHora.Month, fechaHora.Day, ts.Hours, ts.Minutes, ts.Seconds);

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
                do
                {
                    DateTime fechaHoraAdicion = fechaHoraEjecucion.AddHours(tiempo);
                    resultado = fechaHoraAdicion.Subtract(fechaHora);
                    fechaHoraEjecucion = fechaHoraAdicion;
                }
                while (resultado.Milliseconds < 0);
            }
            return resultado;
        }
        /// <summary>
        /// Ejecutar a una hora concreta y posteriormente cada n minutos.
        /// </summary>
        /// <param name="tiempo"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static TimeSpan AddMinutos(int tiempo, TimeSpan ts)
        {
            TimeSpan resultado = TimeSpan.MinValue;

            // La función Change de Timer se lee: ...en prosa: 
            // ejecutate dentro de (tiempo) y luego cada (tiempo).

            // Por tanto, primero hay que calcular a partir de la hora 
            // actual, el tiempo que falta para ejecutarse.

            // Primero definir la fecha actual con la hora de ejecución.
            DateTime fechaHora = DateTime.Now;
            // Concatenar a la fecha la hora de ejecución.
            DateTime fechaHoraEjecucion = new DateTime(fechaHora.Year, fechaHora.Month, fechaHora.Day, ts.Hours, ts.Minutes, ts.Seconds);

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
                do
                {
                    DateTime fechaHoraAdicion = fechaHoraEjecucion.AddMinutes(tiempo);
                    resultado = fechaHoraAdicion.Subtract(fechaHora);
                    fechaHoraEjecucion = fechaHoraAdicion;
                }
                while (resultado.Milliseconds < 0);
            }
            return resultado;
        }
        /// <summary>
        /// Ejecutar a una hora concreta y posteriormente cada n segundos.
        /// </summary>
        /// <param name="tiempo"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static TimeSpan AddSegundos(int tiempo, TimeSpan ts)
        {
            TimeSpan resultado = TimeSpan.MinValue;

            // La función Change de Timer se lee: ...en prosa: 
            // ejecutate dentro de (tiempo) y luego cada (tiempo).

            // Por tanto, primero hay que calcular a partir de la hora 
            // actual, el tiempo que falta para ejecutarse.

            // Primero definir la fecha actual con la hora de ejecución.
            DateTime fechaHora = DateTime.Now;
            // Concatenar a la fecha la hora de ejecución.
            DateTime fechaHoraEjecucion = new DateTime(fechaHora.Year, fechaHora.Month, fechaHora.Day, ts.Hours, ts.Minutes, ts.Seconds);

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
                do
                {
                    DateTime fechaHoraAdicion = fechaHoraEjecucion.AddSeconds(tiempo);
                    resultado = fechaHoraAdicion.Subtract(fechaHora);
                    fechaHoraEjecucion = fechaHoraAdicion;
                }
                while (resultado.Milliseconds < 0);
            }
            return resultado;
        }
        #endregion
        #endregion
    }
}