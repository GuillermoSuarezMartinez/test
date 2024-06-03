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
using System.Collections;
namespace Orbita.Trazabilidad
{
    /// <summary>
    /// TrazaManager.
    /// </summary>
    public static class TrazaManager
    {
        #region Atributos privados estáticos
        /// <summary>
        /// Repositorio de trazas.
        /// </summary>
        static Hashtable Repositorio = new Hashtable();
        #endregion

        #region Métodos públicos

        /// <summary>
        /// Obtener la traza por nombre.
        /// </summary>
        /// <param name="identificador">Identificador de la traza.</param>
        /// <returns>Interface de traza.</returns>
        public static ITraza GetTraza(string identificador)
        {
            ITraza traza = null;
            if (Repositorio.Contains(identificador))
            {
                traza = (Repositorio[identificador] as SimpleTraza).Traza;
            }
            return traza;
        }
        /// <summary>
        /// Asignar al repositorio de trazas la traza del parámetro de entrada del método actual.
        /// </summary>
        /// <param name="traza">Interface de traza.</param>
        public static void SetTraza(ITraza traza)
        {
            if (traza != null)
            {
                string clave = traza.Identificador ?? Orbita.Trazabilidad.Logger.Traza;
                if (!Repositorio.Contains(clave))
                {
                    Repositorio.Add(clave, new SimpleTraza(traza));
                }
            }
        }
        /// <summary>
        /// Eliminar traza por nombre del repositorio.
        /// </summary>
        /// <param name="identificador">Identificador de traza.</param>
        /// <returns>Si existe en la colección el identificador de traza.</returns>
        public static bool DelTraza(string identificador)
        {
            bool res = Repositorio.Contains(identificador);
            if (res)
            {
                Repositorio.Remove(identificador);
            }
            return res;
        }

        #region TrazaLogger
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.TrazaLogger.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="instancia">Nombre de la instancia de base de datos.</param>
        /// <param name="baseDatos">Nombre de la base de datos.</param>
        /// <param name="usuario">Usuario de las credenciales de base de datos.</param>
        /// <param name="password">Password de las credenciales de base de datos.</param>
        /// <param name="logger">DebugLogger de registro que chequea errores de inserción en base de datos.</param>
        /// <returns>ITraza.</returns>
        public static ITraza SetTrazaLogger(string identificador, string instancia, string baseDatos, string usuario, string password, ILogger logger)
        {
            return SetTrazaLogger(identificador, new Orbita.BBDD.OInfoConexion(instancia, baseDatos, usuario, password), logger);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.TrazaLogger.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="instancia">Nombre de la instancia de base de datos.</param>
        /// <param name="baseDatos">Nombre de la base de datos.</param>
        /// <param name="usuario">Usuario de las credenciales de base de datos.</param>
        /// <param name="password">Password de las credenciales de base de datos.</param>
        /// <returns>ITraza.</returns>
        public static ITraza SetTrazaLogger(string identificador, string instancia, string baseDatos, string usuario, string password)
        {
            return SetTrazaLogger(identificador, instancia, baseDatos, usuario, password, null);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.TrazaLogger.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="instancia">Nombre de la instancia de base de datos.</param>
        /// <param name="baseDatos">Nombre de la base de datos.</param>
        /// <param name="usuario">Usuario de las credenciales de base de datos.</param>
        /// <param name="password">Password de las credenciales de base de datos.</param>
        /// <param name="timeout">Timeout de conexión.</param>
        /// <returns>ITraza.</returns>
        public static ITraza SetTrazaLogger(string identificador, string instancia, string baseDatos, string usuario, string password, int timeout)
        {
            return SetTrazaLogger(identificador, instancia, baseDatos, usuario, password, timeout, null);
        }
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
        /// <returns>ITraza.</returns>
        public static ITraza SetTrazaLogger(string identificador, string instancia, string baseDatos, string usuario, string password, int timeout, ILogger logger)
        {
            return SetTrazaLogger(identificador, new Orbita.BBDD.OInfoConexion(instancia, baseDatos, usuario, password, timeout), logger);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.TrazaLogger.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="infoConexion">Información de la conexión actual.</param>
        /// <returns>ITraza.</returns>
        public static ITraza SetTrazaLogger(string identificador, Orbita.BBDD.OInfoConexion infoConexion)
        {
            return SetTrazaLogger(identificador, infoConexion, null);
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.TrazaLogger.
        /// </summary>
        /// <param name="identificador">Identificador de logger.</param>
        /// <param name="infoConexion">Información de la conexión actual.</param>
        /// <param name="logger">DebugLogger de registro que chequea errores de inserción en base de datos.</param>
        /// <returns>ITraza.</returns>
        public static ITraza SetTrazaLogger(string identificador, Orbita.BBDD.OInfoConexion infoConexion, ILogger logger)
        {
            // Construir la traza.
            ITraza traza = new TrazaLogger(identificador, infoConexion, logger);
            // Asignar traza al repositorio.
            SetTraza(traza);
            // Retornar la traza.
            return GetTraza(identificador);
        }
        #endregion

        #endregion
    }
}
