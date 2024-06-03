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
    /// Estructura de detalle del mensaje.
    /// </summary>
    public struct OMensajesDetalle
    {
        #region Atributo(s)
        /// <summary>
        /// Nombre de la excepción.
        /// </summary>
        public string Excepcion { get; set; }
        /// <summary>
        /// Nombre de la excepción.
        /// </summary>
        public string Mensaje { get; set; }
        /// <summary>
        /// Nombre del fichero.
        /// </summary>
        public string Fichero { get; set; }
        /// <summary>
        /// Nombre de la clase.
        /// </summary>
        public string Clase { get; set; }
        /// <summary>
        /// Nombre del método
        /// </summary>
        public string Metodo { get; set; }
        /// <summary>
        /// Línea de la excepción.
        /// </summary>
        public int Linea { get; set; }
        /// <summary>
        /// Pila de llamadas.
        /// </summary>
        public string PilaLlamadas { get; set; }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Devuelve el string formateado del error.
        /// </summary>
        /// <returns>=ToString()</returns>
        public override string ToString()
        {
            return "  - Mensaje: " + this.Mensaje +
                Environment.NewLine + "  - Excepción: " + this.Excepcion +
                Environment.NewLine + "  - Fichero: " + this.Fichero +
                Environment.NewLine + "  - Clase: " + this.Clase +
                Environment.NewLine + "  - Método: " + this.Metodo +
                Environment.NewLine + "  - Línea: " + this.Linea +
                Environment.NewLine + "  - Pila de llamadas: " + this.PilaLlamadas;
        }
        #endregion
    }
}
