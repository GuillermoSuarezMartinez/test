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
using IBM.Data.DB2;
namespace Orbita.BBDD
{
    /// <summary>
    /// Clase tipo para instanciar objetos de base de datos ODB2.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "El origen del nombre abreviado.")]
    public class ODB2 : OBBDDCore
    {
        #region Constructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase DB2.
        /// </summary>
        /// <param name="infoConexion">Información de la conexión actual</param>
        public ODB2(OInfoConexion infoConexion)
            : base(infoConexion)
        {
            if (infoConexion != null)
            {
                this.CadenaConexion = string.Format(CultureInfo.CurrentCulture, @"Server={0}; Database={1}; UID={2}; PWD={3}",
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
            using (DB2Connection conexion = new DB2Connection(this.CadenaConexion))
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
        /// <param name="sql">Consulta T-SQL.</param>
        /// <returns>Datatable con el resultado.</returns>
        public DataTable SeleccionSql(string sql)
        {
            return this.SeleccionSql(sql, this.Timeout);
        }
        /// <summary>
        /// Ejecutar una consulta Select.
        /// </summary>
        /// <param name="sql">Consulta T-SQL.</param>
        /// <param name="timeout">Timeout de SqlCommand.</param>
        /// <returns>Datatable con el resultado.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "SQL proviene de un archivo de recursos y parámetros.")]
        public DataTable SeleccionSql(string sql, int timeout)
        {
            // Inicializar resultado.
            DataTable resultado = null;
            try
            {
                using (DB2Connection conexion = new DB2Connection(this.CadenaConexion))
                using (DB2Command command = new DB2Command(sql, conexion))
                {
                    // Abrir conexión.
                    conexion.Open();
                    // Establecer timeout de ejecución.
                    command.CommandTimeout = timeout;
                    using (DB2DataAdapter adapter = new DB2DataAdapter(command))
                    {
                        // Asignar resultados al DataTable.
                        adapter.Fill(resultado = new DataTable());
                        // Establecer una configuración regional actual.
                        resultado.Locale = CultureInfo.CurrentCulture;
                    }
                }
                // La conexión se cierra automáticamente en este punto, es decir,
                // en el final del bloque using (DB2Connection conexion = new ...).
                // http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.close.aspx
                return resultado;
            }
            finally { if (resultado != null) { resultado.Dispose(); } }
        }
        #endregion

        #region EjecutarSql
        /// <summary>
        /// Ejecutar una sentencia T-SQL con timeout por defecto.
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
        /// Ejecutar una sentencia T-SQL.
        /// Para las instrucciones Update, Insert y Delete, 
        /// el valor devuelto corresponde al número de filas afectadas por 
        /// el comando. Para  los demás  tipos de  instrucciones, el valor 
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
            using (DB2Connection conexion = new DB2Connection(this.CadenaConexion))
            using (DB2Transaction transaccion = conexion.BeginTransaction())
            using (DB2Command command = new DB2Command(sql, conexion, transaccion))
            {
                try
                {
                    // Abrir conexión.
                    conexion.Open();
                    // Establecer timeout de ejecución.
                    command.CommandTimeout = timeout;
                    // Ejecutar query.
                    resultado = command.ExecuteNonQuery();
                    // Confirmar transacción.
                    transaccion.Commit();
                }
                catch (DB2Exception)
                {
                    // Liberar transacción.
                    transaccion.Rollback();
                }
            }
            return resultado;
        }
        /// <summary>
        /// Ejecutar una sentencia T-SQL.
        /// </summary>
        /// <param name="dt">Datatabla con los datos a insertar.</param>
        /// <param name="tablaDestino">Tabla de la base de datos sobre la que
        /// se va a producir la inserción masiva de registros.</param>
        /// <returns>Para la instrucción Insert, el valor devuelto corresponde 
        /// al número de filas afectadas por  el comando. Para los demás tipos 
        /// de instrucciones, el valor devuelto es -1.</returns>
        public int EjecutarSql(DataTable dt, string tablaDestino)
        {
            // Inicializar resultado.
            int resultado = -1;
            using (DB2Connection conexion = new DB2Connection(this.CadenaConexion))
            using (DB2BulkCopy bulk = new DB2BulkCopy(conexion, DB2BulkCopyOptions.Default))
            {
                    // Abrir conexión.
                    conexion.Open();
                    // Asignar la tabla destino.
                    bulk.DestinationTableName = tablaDestino;
                    if (dt != null)
                    {
                        // Ejecutar query.
                        bulk.WriteToServer(dt);
                        // Asignar resultado correpondiente
                        // al número de filas contenidas en
                        // el Datatable.
                        resultado = dt.Rows.Count;
                    }
            }
            return resultado;
        }
        #endregion

        #endregion
    }
}
