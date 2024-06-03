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
using System.Diagnostics;
using System.Windows.Forms;
namespace Orbita.Utiles
{
    /// <summary>
    /// Clase OMensajes.
    /// </summary>
    public sealed class OMensajes
    {
        #region Constructor(es)
        /// <summary>
        /// Inicializar una nueva instancia de la clase OMensajes.
        /// </summary>
        OMensajes() { }
        #endregion

        #region Método(s) público(s)
        #region Método(s) estático(s)
        /// <summary>
        /// Obtiene el detalle del error en la clase OMensajesDetalle.
        /// </summary>
        /// <param name="ex">Excepción.</param>
        /// <returns></returns>
        public static OMensajesDetalle ObtenerDetalle(Exception ex)
        {
            OMensajesDetalle omd = new OMensajesDetalle();
            StackTrace trace = new StackTrace(ex, true);
            omd.Excepcion = ex.GetType().FullName;
            omd.Mensaje = ex.Message;
            omd.Fichero = trace.GetFrame(0).GetFileName();
            omd.Clase = trace.GetFrame(0).GetMethod().ReflectedType.FullName;
            omd.Metodo = trace.GetFrame(0).GetMethod().Name;
            omd.Linea = trace.GetFrame(0).GetFileLineNumber();
            omd.PilaLlamadas = ex.StackTrace;
            return omd;
        }
        /// <summary>
        /// Muestra mensaje de error.
        /// </summary>
        /// <param name="mensaje">Texto del mensaje.</param>
        public static void MostrarError(string mensaje)
        {
            MessageBox.Show(mensaje, "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// Muestra mensaje de error.
        /// </summary>
        /// <param name="ex">Excepción.</param>
        public static void MostrarError(Exception ex)
        {
            MessageBox.Show(ObtenerDetalle(ex).ToString(), "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// Muestra mensaje de error.
        /// </summary>
        /// <param name="mensaje">Texto del mensaje.</param>
        /// <param name="ex">Excepción</param>
        public static void MostrarError(string mensaje, Exception ex)
        {
            MessageBox.Show(mensaje +
                Environment.NewLine +
                Environment.NewLine +
                ObtenerDetalle(ex).ToString(), "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// Muestra mensaje de aviso.
        /// </summary>
        /// <param name="mensaje">Texto del mensaje.</param>
        public static void MostrarAviso(string mensaje)
        {
            MessageBox.Show(mensaje, "¡Aviso!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        /// <summary>
        /// Mostrar mensaje informativo.
        /// </summary>
        /// <param name="mensaje">Texto del mensaje.</param>
        public static void MostrarInfo(string mensaje)
        {
            MessageBox.Show(mensaje, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// Mostrar pregunta.
        /// </summary>
        /// <param name="mensaje">Texto del mensaje.</param>
        /// <returns></returns>
        public static DialogResult MostrarPreguntaSiNo(string mensaje)
        {
            return MessageBox.Show(mensaje, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        }
        /// <summary>
        /// Mostrar pregunta.
        /// </summary>
        /// <param name="mensaje">Texto del mensaje.</param>
        /// <param name="botonDefecto">Botón por defecto.</param>
        /// <returns></returns>
        public static DialogResult MostrarPreguntaSiNo(string mensaje, MessageBoxDefaultButton botonDefecto)
        {
            return MessageBox.Show(mensaje, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question, botonDefecto);
        }
        /// <summary>
        /// Mostrar pregunta con cancelar.
        /// </summary>
        /// <param name="mensaje">Texto del mensaje.</param>
        /// <returns></returns>
        public static DialogResult MostrarPreguntaSiNoCancelar(string mensaje)
        {
            return MessageBox.Show(mensaje, "Confirmar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
        }
        /// <summary>
        /// Mostrar pregunta con cancelar.
        /// </summary>
        /// <param name="mensaje">Texto del mensaje.</param>
        /// <param name="botonDefecto">Botón por defecto.</param>
        /// <returns></returns>
        public static DialogResult MostrarPreguntaSiNoCancelar(string mensaje, MessageBoxDefaultButton botonDefecto)
        {
            return MessageBox.Show(mensaje, "Confirmar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, botonDefecto);
        }
        /// <summary>
        /// Mostrar aviso.
        /// </summary>
        /// <param name="mensaje">Texto del mensaje.</param>
        /// <returns></returns>
        public static DialogResult MostrarAvisoSiNo(string mensaje)
        {
            return MessageBox.Show(mensaje, "¡Aviso!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
        }
        /// <summary>
        /// Mostrar aviso.
        /// </summary>
        /// <param name="mensaje">Texto del mensaje.</param>
        /// <param name="botonDefecto">Botón por defecto.</param>
        /// <returns></returns>
        public static DialogResult MostrarAvisoSiNo(string mensaje, MessageBoxDefaultButton botonDefecto)
        {
            return MessageBox.Show(mensaje, "¡Aviso!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, botonDefecto);
        }
        #endregion
        #endregion
    }
}
