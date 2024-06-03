//***********************************************************************
// Assembly         : Orbita.BBDD
// Author           : crodriguez
// Created          : 02-21-2011
//
// Last Modified By : crodriguez
// Last Modified On : 02-21-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
namespace Orbita.BBDD
{
    /// <summary>
    /// Información de la conexión para SQL Server con mirroring.
    /// </summary>
    public class OInfoMirroring : OInfoConexion
    {
        #region Atributos privados
        /// <summary>
        /// Nombre del servidor en mirror.
        /// </summary>
        string mirror;
        #endregion

        #region Constructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase InfoMirroring.
        /// </summary>
        /// <param name="mirror">Nombre del servidor en mirror.</param>
        public OInfoMirroring(string mirror)
            : base()
        {
            this.mirror = mirror;
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase InfoMirroring.
        /// </summary>
        /// <param name="timeout">Timeout de conexión.</param>
        /// <param name="mirror">Nombre del servidor en mirror.</param>
        public OInfoMirroring(int timeout, string mirror)
            : base(timeout)
        {
            this.mirror = mirror;
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase InfoMirroring.
        /// </summary>
        /// <param name="servidor">Nombre del servidor.</param>
        /// <param name="baseDatos">Nombre de la base de datos.</param>
        /// <param name="usuario">Usuario de las credenciales de base de datos.</param>
        /// <param name="password">Password de las credenciales de base de datos.</param>
        /// <param name="timeout">Timeout de conexión.</param>
        /// <param name="mirror">Nombre del servidor en mirror.</param>
        public OInfoMirroring(string servidor, string baseDatos, string usuario, string password, int timeout, string mirror)
            : base(servidor, baseDatos, usuario, password, timeout)
        {
            this.mirror = mirror;
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Nombre del servidor en mirror.
        /// </summary>
        public string Mirror
        {
            get { return this.mirror; }
            set { this.mirror = value; }
        }
        #endregion
    }
}
