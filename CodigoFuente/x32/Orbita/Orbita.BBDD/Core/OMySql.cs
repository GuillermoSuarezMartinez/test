//***********************************************************************
// Assembly         : Orbita.BBDD
// Author           : crodriguez
// Created          : 02-22-2011
//
// Last Modified By : crodriguez
// Last Modified On : 02-22-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using MySql.Data.MySqlClient;
namespace Orbita.BBDD
{
    /// <summary>
    /// Clase tipo para instanciar objetos de base de datos OMySQL.
    /// </summary>
    public class OMySql : OBBDDCore
    {
        #region Constructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase MySQL.
        /// </summary>
        /// <param name="infoConexion">Información de la conexión actual.</param>
        public OMySql(OInfoConexion infoConexion)
            : base(infoConexion)
        {
            if (infoConexion != null)
            {
                this.CadenaConexion = string.Format(CultureInfo.CurrentCulture, @"SERVER={0}; DATABASE={1}; UID={2}; PASSWORD={3}", 
                    infoConexion.Instancia, infoConexion.BaseDatos, infoConexion.Usuario, infoConexion.Password);
            }
        }
        #endregion

        #region Métodos públicos

        #region Test
        /// <summary>
        /// Comprueba el estado una conexión existente.
        /// </summary>
        /// <returns>El resultado del test de conexión.</returns>
        public bool TestConexion()
        {
            // Inicializar resultado.
            bool resultado = false;
            using (MySqlConnection conexion = new MySqlConnection(this.CadenaConexion))
            {
                // Abrir conexión.
                conexion.Open(); resultado = (conexion.State == ConnectionState.Open);
            }
            // La conexión se cierra automáticamente en este punto, es decir,
            // en el final del bloque using (SqlConnection conexion = new ...).
            // http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.close.aspx
            return resultado;
        }
        #endregion

        #region SeleccionSql
        /// <summary>
        /// Ejecutar una consulta Select con timeout por defecto.
        /// </summary>
        /// <param name="sql">Consulta SQL.</param>
        /// <returns>Datatable con el resultado.</returns>
        public DataTable SeleccionSql(string sql)
        {
            return this.SeleccionSql(sql, this.Timeout);
        }
        /// <summary>
        /// Ejecutar una consulta Select.
        /// </summary>
        /// <param name="sql">Consulta SQL.</param>
        /// <param name="timeout">Timeout de SqlCommand.</param>
        /// <returns>Datatable con el resultado.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "SQL proviene de un archivo de recursos y parámetros.")]
        public DataTable SeleccionSql(string sql, int timeout)
        {
            // Inicializar resultado.
            DataTable resultado = null;
            try
            {
                using (MySqlConnection conexion = new MySqlConnection(this.CadenaConexion))
                using (MySqlCommand command = conexion.CreateCommand())
                {
                    // Abrir conexión.
                    conexion.Open();
                    // Establecer timeout de ejecución.
                    command.CommandTimeout = timeout;
                    command.CommandText = sql;
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        // Asignar resultados al DataTable.
                        resultado = new DataTable();
                        // Establecer una configuración regional actual.
                        resultado.Locale = CultureInfo.CurrentCulture;
                        // Cargar el resultado del reader.
                        resultado.Load(reader);
                    }
                }
                // La conexión se cierra automáticamente en este punto, es decir,
                // en el final del bloque using (MySqlConnection conexion = new ...).
                // http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.close.aspx
                return resultado;
            }
            finally { if (resultado != null) { resultado.Dispose(); } }
        }
        #endregion

        #region EjecutarSql
        /// <summary>
        /// Ejecutar una sentencia SQL con timeout por defecto.
        /// </summary>
        /// <param name="sql">Consulta SQL.</param>
        /// <returns>Para las instrucciones Update, Insert y Delete, el 
        /// valor devuelto corresponde al número de filas afectadas por
        /// el comando. Para los demás tipos de instrucciones, el valor
        /// devuelto es -1.</returns>
        public int EjecutarSql(string sql)
        {
            return this.EjecutarSql(sql, this.Timeout);
        }
        /// <summary>
        /// Ejecutar una sentencia SQL.
        /// Para las instrucciones Update, Insert y Delete, 
        /// el valor devuelto corresponde al número de filas afectadas por
        /// el comando. Para los demás tipos de instrucciones, el valor 
        /// devuelto es -1.
        /// </summary>
        /// <param name="sql">Consulta SQL.</param>
        /// <param name="timeout">Timeout de SqlCommand.</param>
        /// <returns>Para las instrucciones Update, Insert y Delete, el 
        /// valor devuelto corresponde al número de filas afectadas por
        /// el comando. Para los demás tipos de instrucciones, el valor
        /// devuelto es -1.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "SQL proviene de un archivo de recursos y parámetros.")]
        public int EjecutarSql(string sql, int timeout)
        {
            // Inicializar resultado.
            int resultado = -1;
            using (MySqlConnection conexion = new MySqlConnection(this.CadenaConexion))
            {
                // Abrir conexión.
                conexion.Open();
                using (MySqlTransaction transaccion = conexion.BeginTransaction())
                using (MySqlCommand command = new MySqlCommand(sql, conexion, transaccion))
                {
                    // Establecer timeout de ejecución.
                    command.CommandTimeout = timeout;
                    try
                    {
                        // Ejecutar query.
                        resultado = command.ExecuteNonQuery();
                        // Confirmar transacción.
                        transaccion.Commit();
                    }
                    catch (MySqlException)
                    {
                        // Liberar transacción.
                        transaccion.Rollback();
                    }
                }
            }
            return resultado;
        }
        #endregion

        #endregion
    }
}
