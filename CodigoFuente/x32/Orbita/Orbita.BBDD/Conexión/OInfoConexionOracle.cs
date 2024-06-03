//***********************************************************************
// Assembly         : Orbita.BBDD
// Author           : vnicolau
// Created          : 08-18-2011
//
// Last Modified By : vnicolau
// Last Modified On : 08-18-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
namespace Orbita.BBDD
{
    /// <summary>
    /// Información de la conexión de Oracle.
    /// </summary>
    public class OInfoConexionOracle : OInfoConexion
    {
        #region Atributos protegidos
        /// <summary>
        /// Puerto de conexión por defecto
        /// </summary>
        protected const int DefaultPort = 1521;
        /// <summary>
        /// Número de puerto al que se conectará.
        /// </summary>
        protected int puerto;
        #endregion

        #region Constructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase InfoConexion.
        /// </summary>
        public OInfoConexionOracle()
            :base() 
        {
            this.puerto = DefaultPort;
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase InfoConexion.
        /// </summary>
        /// <param name="timeout">Timeout de conexión.</param>
        public OInfoConexionOracle(int timeout)
            : base (timeout) 
        {
            this.puerto = DefaultPort;
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase InfoConexion.
        /// </summary>
        /// <param name="instancia">Nombre de la instancia de base de datos.</param>
        /// <param name="baseDatos">Nombre de la base de datos.</param>
        /// <param name="usuario">Usuario de las credenciales de base de datos.</param>
        /// <param name="password">Password de las credenciales de base de datos.</param>
        public OInfoConexionOracle(string instancia, string baseDatos, string usuario, string password)
            : base(instancia, baseDatos, usuario, password) 
        {
            this.puerto = DefaultPort;
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase InfoConexion.
        /// </summary>
        /// <param name="instancia">Nombre de la instancia de base de datos.</param>
        /// <param name="puerto">Número de puerto al que se conectará.</param>
        /// <param name="baseDatos">Nombre de la base de datos.</param>
        /// <param name="usuario">Usuario de las credenciales de base de datos.</param>
        /// <param name="password">Password de las credenciales de base de datos.</param>
        public OInfoConexionOracle(string instancia, int puerto, string baseDatos, string usuario, string password)
            : base(instancia, baseDatos, usuario, password) 
        {
            this.puerto = puerto;
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase InfoConexion.
        /// </summary>
        /// <param name="instancia">Nombre de la instancia de base de datos.</param>
        /// <param name="baseDatos">Nombre de la base de datos.</param>
        /// <param name="usuario">Usuario de las credenciales de base de datos.</param>
        /// <param name="password">Password de las credenciales de base de datos.</param>
        /// <param name="timeout">Timeout de conexión.</param>
        public OInfoConexionOracle(string instancia, string baseDatos, string usuario, string password, int timeout)
            : base(instancia, baseDatos, usuario, password, timeout)
        {
            this.puerto = DefaultPort;
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase InfoConexion.
        /// </summary>
        /// <param name="instancia">Nombre de la instancia de base de datos.</param>
        /// <param name="puerto">Número de puerto al que se conectará.</param>
        /// <param name="baseDatos">Nombre de la base de datos.</param>
        /// <param name="usuario">Usuario de las credenciales de base de datos.</param>
        /// <param name="password">Password de las credenciales de base de datos.</param>
        /// <param name="timeout">Timeout de conexión.</param>
        public OInfoConexionOracle(string instancia, int puerto, string baseDatos, string usuario, string password, int timeout)
            : base(instancia, baseDatos, usuario, password, timeout) 
        {
            this.puerto = puerto;
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Número de puerto al que se conectará.
        /// </summary>
        public int Puerto
        {
            get { return this.puerto; }
            set { this.puerto = value; }
        }
        #endregion
    }
}
