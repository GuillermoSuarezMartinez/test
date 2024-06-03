using System;
using System.Collections.Generic;
using System.Text;

namespace Orbita.Comunicaciones
{
        /// <summary>
    /// oOPCComms.
    /// </summary>
    [Serializable]
    public class OEstadoComms
    {
        #region Atributos privados
        /// <summary>
        /// Estado.
        /// </summary>
        string _estado;
        /// <summary>
        /// Enlace.
        /// </summary>
        string _enlace;
        /// <summary>
        /// OPC.
        /// </summary>
        string _nombre;
        /// <summary>
        /// Identificador de OPC.
        /// </summary>
        int _id;
        #endregion

        #region Propiedades
        /// <summary>
        /// Estado.
        /// </summary>
        public string Estado
        {
            get { return _estado; }
            set { _estado = value; }
        }
        /// <summary>
        /// Enlace.
        /// </summary>
        public string Enlace
        {
            get { return _enlace; }
            set { _enlace = value; }
        }
        /// <summary>
        /// OPC.
        /// </summary>
        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }
        /// <summary>
        /// Identificador OPC.
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        #endregion
    }
}
