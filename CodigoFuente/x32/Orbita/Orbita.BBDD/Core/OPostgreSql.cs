//***********************************************************************
// Assembly         : Orbita.BBDD
// Author           : crodriguez
// Created          : 08-18-2011
//
// Last Modified By : vnicolau
// Last Modified On : 08-18-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Npgsql;
namespace Orbita.BBDD
{
    /// <summary>
    /// Clase tipo para instanciar objetos de base de datos OPostgreSQL.
    /// </summary>
    public class OPostgreSql : OBBDDCore
    {
        #region Constructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase MySQL.
        /// </summary>
        /// <param name="infoConexion">Información de la conexión actual.</param>
        public OPostgreSql(OInfoConexion infoConexion)
            : base(infoConexion)
        {
            if (infoConexion != null)
            {
                this.CadenaConexion = string.Format(CultureInfo.CurrentCulture, @"Server={0};Port={1};User Id={2};Password={3};Database={4};",
                      infoConexion.Instancia, infoConexion.Port, infoConexion.Usuario, infoConexion.Password, infoConexion.BaseDatos);
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
            using (NpgsqlConnection conexion = new NpgsqlConnection(this.CadenaConexion))
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
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaConexion))
                using (NpgsqlCommand command = conexion.CreateCommand())
                {
                    // Abrir conexión.
                    conexion.Open();
                    // Establecer timeout de ejecución.
                    command.CommandTimeout = timeout;
                    command.CommandText = sql;

                    using (NpgsqlDataReader reader = command.ExecuteReader())
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
            using (NpgsqlConnection conexion = new NpgsqlConnection(this.CadenaConexion))
            {
                // Abrir conexión.
                conexion.Open();
                using (NpgsqlTransaction transaccion = conexion.BeginTransaction())
                using (NpgsqlCommand command = new NpgsqlCommand(sql, conexion, transaccion))
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
                    catch (NpgsqlException)
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

