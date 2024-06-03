//***********************************************************************
// Assembly         : OrbitaTrazabilidad
// Author           : crodriguez
// Created          : 02-17-2011
//
// Last Modified By : crodriguez
// Last Modified On : 02-22-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections;
using System.Data.SqlClient;
using System.Globalization;
using Orbita.BBDD;
namespace Orbita.Trazabilidad
{
    /// <summary>
    /// Traza Logger.
    /// </summary>
    public class TrazaLogger : BaseTraza
    {
        #region Atributos privados
        /// <summary>
        /// Información de la conexión.
        /// La inclusión del atributo es necesaria para poder obtener las propiedades de  la
        /// clase InfoConexion a través del objeto de base de datos que se cree en cada caso.
        /// </summary>
        OInfoConexion infoConexion;
        /// <summary>
        /// Logger de trazas correctas.
        /// </summary>
        ILogger logger;
        #endregion

        #region Constructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.TrazaLogger.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        public TrazaLogger(string identificador)
            : base(identificador) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.TrazaLogger.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="instancia">Nombre de la instancia de base de datos.</param>
        /// <param name="baseDatos">Nombre de la base de datos.</param>
        /// <param name="usuario">Usuario de las credenciales de base de datos.</param>
        /// <param name="password">Password de las credenciales de base de datos.</param>
        /// <param name="logger">DebugLogger de registro que chequea errores de inserción en base de datos.</param>
        public TrazaLogger(string identificador, string instancia, string baseDatos, string usuario, string password, ILogger logger)
            : this(identificador, new OInfoConexion(instancia, baseDatos, usuario, password), logger) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.TrazaLogger.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="instancia">Nombre de la instancia de base de datos.</param>
        /// <param name="baseDatos">Nombre de la base de datos.</param>
        /// <param name="usuario">Usuario de las credenciales de base de datos.</param>
        /// <param name="password">Password de las credenciales de base de datos.</param>
        public TrazaLogger(string identificador, string instancia, string baseDatos, string usuario, string password)
            : this(identificador, instancia, baseDatos, usuario, password, null) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.TrazaLogger.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="instancia">Nombre de la instancia de base de datos.</param>
        /// <param name="baseDatos">Nombre de la base de datos.</param>
        /// <param name="usuario">Usuario de las credenciales de base de datos.</param>
        /// <param name="password">Password de las credenciales de base de datos.</param>
        /// <param name="timeout">Timeout de conexión.</param>
        public TrazaLogger(string identificador, string instancia, string baseDatos, string usuario, string password, int timeout)
            : this(identificador, instancia, baseDatos, usuario, password, timeout, null) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.TrazaLogger.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="instancia">Nombre de la instancia de base de datos.</param>
        /// <param name="baseDatos">Nombre de la base de datos.</param>
        /// <param name="usuario">Usuario de las credenciales de base de datos.</param>
        /// <param name="password">Password de las credenciales de base de datos.</param>
        /// <param name="timeout">Timeout de conexión.</param>
        /// <param name="logger">DebugLogger de registro que chequea errores de inserción en base de datos.</param>
        public TrazaLogger(string identificador, string instancia, string baseDatos, string usuario, string password, int timeout, ILogger logger)
            : this(identificador, new OInfoConexion(instancia, baseDatos, usuario, password, timeout), logger) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.TrazaLogger.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="infoConexion">Información de la conexión actual.</param>
        public TrazaLogger(string identificador, OInfoConexion infoConexion)
            : this(identificador, infoConexion, null) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.TrazaLogger.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="infoConexion">Información de la conexión actual.</param>
        /// <param name="logger">DebugLogger de registro que chequea errores de inserción en base de datos.</param>
        public TrazaLogger(string identificador, OInfoConexion infoConexion, ILogger logger)
            : this(identificador)
        {
            this.infoConexion = infoConexion;
            this.logger = logger;
        }
        #endregion

        #region Propiedades públicas
        /// <summary>
        /// InfoConexion de base de datos.
        /// </summary>
        public OInfoConexion InfoConexion
        {
            get { return this.infoConexion; }
            set { this.infoConexion = value; }
        }
        #endregion

        #region Propiedades privadas
        /// <summary>
        /// Base de datos.
        /// </summary>
        OSqlServer BD
        {
            get { return new OSqlServer(this.infoConexion); }
        }
        /// <summary>
        /// Logger de control de excepciones.
        /// </summary>
        ILogger Logger
        {
            get { return this.logger; }
        }
        #endregion

        #region Métodos públicos
        /// <summary>
        /// Crea una entrada de registro nuevo basado en un elemento de inicio de sesión dado.
        /// </summary>
        /// <param name="item">Entrada de registro de traza.</param>
        /// <returns>Resultado de ejecución del procedimiento almacenado.</returns>
        public override int Log(ItemTraza item)
        {
            return (item == null) ? -1 : TrazarDato(item);
        }
        #endregion

        #region Métodos privados
        /// <summary>
        /// Escribir en base de datos.
        /// </summary>
        /// <param name="item">Entrada de registro de traza.</param>
        /// <returns>El estado del procedimiento almacenado.</returns>
        int TrazarDato(ItemTraza item)
        {
            int resultado = -1;
            try
            {
                resultado = this.BD.EjecutarProcedimientoAlmacenado(item.Procedimiento, item.Argumentos);
            }
            catch (SqlException ex)
            {
                if (this.logger != null)
                {
                    if (item.Argumentos != null)
                    {
                        // Convertir el array de parámetros en un array de valores.
                        ArrayList argsValores = new ArrayList();
                        for (int i = 0; i < item.Argumentos.Count; i++)
                        {
                            argsValores.Add(((SqlParameter)item.Argumentos[i]).SqlValue.ToString());
                        }
                        // Logger del registro que no se ha podido insertar, con argumentos.
                        this.Logger.Error(ex, item.Procedimiento, (string[])argsValores.ToArray(typeof(string)));
                    }
                    else
                    {
                        // Logger del registro que no se ha podido insertar, sin argumentos.
                        this.Logger.Error(ex, item.Procedimiento);
                    }
                }
            }
            return resultado;
        }
        #endregion
    }
}
