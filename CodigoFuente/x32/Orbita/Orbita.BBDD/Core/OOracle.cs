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
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Oracle.DataAccess.Client;
namespace Orbita.BBDD
{
    /// <summary>
    /// Clase tipo para instanciar objetos de base de datos OOracle.
    /// </summary>
    public class OOracle : OBBDDCore
    {
        #region Constructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase OOracle.
        /// </summary>
        /// <param name="infoConexion">Información de la conexión actual.</param>
        public OOracle(OInfoConexionOracle infoConexion)
            : base(infoConexion)
        {
            if (infoConexion != null)
            {
                base.CadenaConexion = string.Format(CultureInfo.CurrentCulture, @"Data Source = (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1}))(CONNECT_DATA = (SERVICE_NAME = {2}))); User Id = {3}; Password = {4}; Connection Timeout = {5};", 
                    infoConexion.Instancia, infoConexion.Puerto, infoConexion.BaseDatos, infoConexion.Usuario, infoConexion.Password, infoConexion.Timeout);
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
            using (OracleConnection conexion = new OracleConnection(base.CadenaConexion))
            {
                // Abrir conexión.
                conexion.Open(); 
                resultado = (conexion.State == ConnectionState.Open);
                conexion.Close();
            }
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
            return this.SeleccionSql(sql, base.Timeout);
        }
        /// <summary>
        /// Ejecutar una consulta Select.
        /// </summary>
        /// <param name="sql">Consulta T-SQL.</param>
        /// <param name="timeout">Timeout de SqlCommand.</param>
        /// <returns>Datatable con el resultado.</returns>
        public DataTable SeleccionSql(string sql, int timeout)
        {
            // Inicializar resultado.
            DataTable resultado = null;
            try
            {
                using (OracleConnection conexion = new OracleConnection(base.CadenaConexion))
                using (OracleCommand command = new OracleCommand(sql, conexion))
                {
                    // Abrir conexión.
                    conexion.Open();

                    // Establecer timeout de ejecución.
                    command.CommandTimeout = timeout;
            
                    //Establecer el tipo de comando
                    command.CommandType = CommandType.Text;

                    using (OracleDataAdapter adapter = new OracleDataAdapter(command))
                    {
                        // Asignar resultados al DataTable.
                        adapter.Fill(resultado = new DataTable());
                        // Establecer una configuración regional actual.
                        resultado.Locale = CultureInfo.CurrentCulture;
                    }

                    conexion.Close();
                }

                return resultado;
            }
            finally  
            { 
                if (resultado != null) 
                { 
                    resultado.Dispose();
                }
            }
        }
        #endregion

        #region SeleccionProcedimientoAlmacenado
        /// <summary>
        /// Ejecutar una sentencia Select en un procedimiento almacenado con timeout por defecto.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <returns>Datatable con el resultado.</returns>
        public DataTable SeleccionProcedimientoAlmacenado(string procedimiento)
        {
            return this.SeleccionProcedimientoAlmacenado(procedimiento, this.Timeout);
        }
        /// <summary>
        /// Ejecutar una sentencia Select en un procedimiento almacenado.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <param name="timeout">Timeout de SqlCommand.</param>
        /// <returns>Datatable con el resultado.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "StoredProc proviene de un archivo de recursos y parámetros.")]
        public DataTable SeleccionProcedimientoAlmacenado(string procedimiento, int timeout)
        {
            throw new NotImplementedException("Este método no está todavía implementado. Es cuestión de tiempo que lo esté.");
        }
        /// <summary>
        /// Ejecutar una sentencia Select en un procedimiento almacenado con parámetros de entrada y con timeout por defecto.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Parámetros de entrada del procedimiento almacenado.</param>
        /// <returns>Datatable con el resultado.</returns>
        public DataTable SeleccionProcedimientoAlmacenado(string procedimiento, ArrayList parametros)
        {
            return this.SeleccionProcedimientoAlmacenado(procedimiento, parametros, this.Timeout);
        }
        /// <summary>
        /// Ejecutar una sentencia Select en un procedimiento almacenado con parámetros de entrada.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Parámetros de entrada del procedimiento almacenado.</param>
        /// <param name="timeout">Timeout de SqlCommand.</param>
        /// <returns>Datatable con el resultado.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "StoredProc proviene de un archivo de recursos y parámetros.")]
        public DataTable SeleccionProcedimientoAlmacenado(string procedimiento, ArrayList parametros, int timeout)
        {
            throw new NotImplementedException("Este método no está todavía implementado. Es cuestión de tiempo que lo esté.");
        }
        #endregion

        #region EjecutarProcedimientoAlmacenado
        /// <summary>
        /// Ejecutar un procedimiento almacenado con timeout por defecto.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <returns>Resultado del procedimiento almacenado.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "StoredProc proviene de un archivo de recursos y parámetros.")]
        public int EjecutarProcedimientoAlmacenado(string procedimiento)
        {
            return this.EjecutarProcedimientoAlmacenado(procedimiento, this.Timeout);
        }
        /// <summary>
        /// Ejecutar un procedimiento almacenado.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <param name="timeout">Timeout de SqlCommand.</param>
        /// <returns>Resultado del procedimiento almacenado.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "StoredProc proviene de un archivo de recursos y parámetros.")]
        public int EjecutarProcedimientoAlmacenado(string procedimiento, int timeout)
        {
            throw new NotImplementedException("Este método no está todavía implementado. Es cuestión de tiempo que lo esté.");
        }
        /// <summary>
        /// Ejecutar un procedimiento almacenado con parámetros de entrada y timeout por defecto.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Parámetros de entrada del procedimiento almacenado.</param>
        /// <returns>Resultado del procedimiento almacenado.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "StoredProc proviene de un archivo de recursos y parámetros.")]
        public int EjecutarProcedimientoAlmacenado(string procedimiento, ArrayList parametros)
        {
            return this.EjecutarProcedimientoAlmacenado(procedimiento, parametros, this.Timeout);
        }
        /// <summary>
        /// Ejecutar un procedimiento almacenado con parámetros de entrada.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Parámetros de entrada del procedimiento almacenado.</param>
        /// <param name="timeout">Timeout de SqlCommand.</param>
        /// <returns>Resultado del procedimiento almacenado.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "StoredProc proviene de un archivo de recursos y parámetros.")]
        public int EjecutarProcedimientoAlmacenado(string procedimiento, ArrayList parametros, int timeout)
        {
            throw new NotImplementedException("Este método no está todavía implementado. Es cuestión de tiempo que lo esté.");
        }
        #endregion

        #region EjecutarProcedimientoAlmacenadoSinRetorno
        /// <summary>
        /// Ejecutar un procedimiento almacenado con timeout por defecto, prescindiendo 
        /// del parámetro "return" del procedimiento almacenado.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <returns>Número de registros afectados en el procedimiento almacenado.</returns>
        public int EjecutarProcedimientoAlmacenadoSinRetorno(string procedimiento)
        {
            return this.EjecutarProcedimientoAlmacenadoSinRetorno(procedimiento, this.Timeout);
        }
        /// <summary>
        /// Ejecutar un procedimiento almacenado, prescindiendo del parámetro "return" 
        /// del procedimiento almacenado.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <param name="timeout">Timeout de SqlCommand.</param>
        /// <returns>Número de registros afectados en el procedimiento almacenado.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "StoredProc proviene de un archivo de recursos y parámetros.")]
        public int EjecutarProcedimientoAlmacenadoSinRetorno(string procedimiento, int timeout)
        {
            throw new NotImplementedException("Este método no está todavía implementado. Es cuestión de tiempo que lo esté.");
        }
        /// <summary>
        /// Ejecutar un procedimiento almacenado con parámetros de entrada y timeout por defecto, 
        /// prescindiendo del parámetro "return" del procedimiento almacenado.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Parámetros de entrada del procedimiento almacenado.</param>
        /// <returns>Número de registros afectados en el procedimiento almacenado.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "StoredProc proviene de un archivo de recursos y parámetros.")]
        public int EjecutarProcedimientoAlmacenadoSinRetorno(string procedimiento, ArrayList parametros)
        {
            return this.EjecutarProcedimientoAlmacenadoSinRetorno(procedimiento, parametros, this.Timeout);
        }
        /// <summary>
        /// Ejecutar un procedimiento almacenado con parámetros de entrada y timeout por 
        /// defecto, prescindiendo del parámetro "return" del procedimiento almacenado.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Parámetros de entrada del procedimiento almacenado.</param>
        /// <param name="timeout">Timeout de SqlCommand.</param>
        /// <returns>Número de registros afectados en el procedimiento almacenado.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "StoredProc proviene de un archivo de recursos y parámetros.")]
        public int EjecutarProcedimientoAlmacenadoSinRetorno(string procedimiento, ArrayList parametros, int timeout)
        {
            throw new NotImplementedException("Este método no está todavía implementado. Es cuestión de tiempo que lo esté.");
        }
        #endregion

        #region EjecutarProcedimientoAlmacenadoTransaccional
        /// <summary>
        /// Ejecutar un procedimiento almacenado transaccional con timeout por defecto.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <returns>Resultado del procedimiento almacenado.</returns>
        public int EjecutarProcedimientoAlmacenadoTransaccional(string procedimiento)
        {
            return this.EjecutarProcedimientoAlmacenadoTransaccional(procedimiento, this.Timeout);
        }
        /// <summary>
        /// Ejecutar un procedimiento almacenado transaccional.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <param name="timeout">Timeout de SqlCommand.</param>
        /// <returns>Resultado del procedimiento almacenado.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "StoredProc proviene de un archivo de recursos y parámetros.")]
        public int EjecutarProcedimientoAlmacenadoTransaccional(string procedimiento, int timeout)
        {
            throw new NotImplementedException("Este método no está todavía implementado. Es cuestión de tiempo que lo esté.");
        }
        /// <summary>
        /// Ejecutar un procedimiento almacenado transaccional con parámetros de entrada y timeout por defecto.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Parámetros de entrada del procedimiento almacenado.</param>
        /// <returns>Resultado del procedimiento almacenado.</returns>
        public int EjecutarProcedimientoAlmacenadoTransaccional(string procedimiento, ArrayList parametros)
        {
            return this.EjecutarProcedimientoAlmacenadoTransaccional(procedimiento, parametros, this.Timeout);
        }
        /// <summary>
        /// Ejecutar un procedimiento almacenado transaccional con parámetros de entrada.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Parámetros de entrada del procedimiento almacenado.</param>
        /// <param name="timeout">Timeout de SqlCommand.</param>
        /// <returns>Resultado del procedimiento almacenado.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "StoredProc proviene de un archivo de recursos y parámetros.")]
        public int EjecutarProcedimientoAlmacenadoTransaccional(string procedimiento, ArrayList parametros, int timeout)
        {
            throw new NotImplementedException("Este método no está todavía implementado. Es cuestión de tiempo que lo esté.");
        }
        #endregion

        #region EjecutarProcedimientoAlmacenadoTransaccionalSinRetorno
        /// <summary>
        /// Ejecutar un procedimiento almacenado transaccional con timeout por defecto, 
        /// prescindiendo del parámetro "return" del procedimiento almacenado.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <returns>Número de registros afectados en el procedimiento almacenado.</returns>
        public int EjecutarProcedimientoAlmacenadoTransaccionalSinRetorno(string procedimiento)
        {
            return this.EjecutarProcedimientoAlmacenadoTransaccionalSinRetorno(procedimiento, this.Timeout);
        }
        /// <summary>
        /// Ejecutar un procedimiento almacenado transaccional, prescindiendo del parámetro
        /// "return" del procedimiento almacenado.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <param name="timeout">Timeout de SqlCommand.</param>
        /// <returns>Número de registros afectados en el procedimiento almacenado.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "StoredProc proviene de un archivo de recursos y parámetros.")]
        public int EjecutarProcedimientoAlmacenadoTransaccionalSinRetorno(string procedimiento, int timeout)
        {
            throw new NotImplementedException("Este método no está todavía implementado. Es cuestión de tiempo que lo esté.");
        }
        /// <summary>
        /// Ejecutar un procedimiento almacenado transaccional con parámetros de entrada y timeout 
        /// por defecto, prescindiendo del parámetro "return" del procedimiento almacenado.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Parámetros de entrada del procedimiento almacenado.</param>
        /// <returns>Número de registros afectados en el procedimiento almacenado.</returns>
        public int EjecutarProcedimientoAlmacenadoTransaccionalSinRetorno(string procedimiento, ArrayList parametros)
        {
            return this.EjecutarProcedimientoAlmacenadoTransaccionalSinRetorno(procedimiento, parametros, this.Timeout);
        }
        /// <summary>
        /// Ejecutar un procedimiento almacenado transaccional con parámetros de entrada y timeout 
        /// por defecto, prescindiendo del parámetro "return" del procedimiento almacenado.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Parámetros de entrada del procedimiento almacenado.</param>
        /// <param name="timeout">Timeout de SqlCommand.</param>
        /// <returns>Número de registros afectados en el procedimiento almacenado.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "StoredProc proviene de un archivo de recursos y parámetros.")]
        public int EjecutarProcedimientoAlmacenadoTransaccionalSinRetorno(string procedimiento, ArrayList parametros, int timeout)
        {
            throw new NotImplementedException("Este método no está todavía implementado. Es cuestión de tiempo que lo esté.");
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
            return this.EjecutarSql(sql, base.Timeout);
        }
        /// <summary>
        /// Ejecutar una sentencia T-SQL.
        /// </summary>
        /// <param name="sql">Consulta SQL.</param>
        /// <param name="timeout">Timeout de SqlCommand.</param>
        /// <returns>Para las instrucciones Update, Insert y Delete, el 
        /// valor devuelto corresponde al número de filas afectadas por
        /// el comando. Para los demás tipos de instrucciones, el valor
        /// devuelto es -1.</returns>
        public int EjecutarSql(string sql, int timeout)
        {
            // Inicializar resultado.
            int resultado = -1;
            OracleTransaction transaccion = null;
            using (OracleConnection conexion = new OracleConnection(base.CadenaConexion))
            using (OracleCommand command = new OracleCommand(sql, conexion))
            {
                try
                {
                    // Abrir conexión.
                    conexion.Open();

                    //Comenzamos la transacción (ésta debe hacerse después de abrir la conexión)
                    transaccion = conexion.BeginTransaction();

                    //Indicamos al comando en qué transacción se ´va a ejecutar
                    command.Transaction = transaccion;

                    // Establecer timeout de ejecución.
                    command.CommandTimeout = timeout;

                    // Ejecutar query.
                    resultado = command.ExecuteNonQuery();

                    // Confirmar transacción.
                    transaccion.Commit();

                    // Cerrar conexión
                    conexion.Close();
                }
                catch (OracleException)
                {
                    if (transaccion != null)
                    {
                        // Liberar transacción.
                        transaccion.Rollback();
                    }
                }
                finally
                {
                    transaccion.Dispose();
                }
            }
            return resultado;
        }
        /// <summary>
        /// ¡¡ATENCIÓN!!
        /// Este método no está desarrollado ya que la versión del ODP.NET (Oracle Data Provider for .NET) no contiene el objecto OracleBulkCopy necesario para poder ejecutar este método.
        /// La versión del ODP.NET utilizado en Orbita.BBDD.dll es la 10.2.0.2.20 (siendo la versión del fichero Oracle.DataAccess.dll la 1.102.5.0).
        /// Según parece, a partir de la versión 11.1.6.20 la clase OracleBulkCopy ya se encuentra incluida en el ODP.NET. En agosto del 2011 la última versión disponible del ODP.NET era la 11.2.0.2
        /// </summary>
        /// <param name="dt">Datatabla con los datos a insertar.</param>
        /// <param name="tablaDestino">Tabla de la base de datos sobre la que
        /// se va a producir la inserción masiva de registros.</param>
        /// <returns>Para la instrucción Insert, el valor devuelto corresponde 
        /// al número de filas afectadas por  el comando. Para los demás tipos 
        /// de instrucciones, el valor devuelto es -1.</returns>
        public int EjecutarSql(DataTable dt, string tablaDestino)
        {
            throw new NotImplementedException("Este método no está desarrollado ya que la versión del ODP.NET (Oracle Data Provider for .NET) no contiene el objecto OracleBulkCopy necesario para poder ejecutar este método." + Environment.NewLine + "La versión del ODP.NET utilizado en Orbita.BBDD.dll es la 10.2.0.2.20 (siendo la versión del fichero Oracle.DataAccess.dll la 1.102.5.0)." + Environment.NewLine + "Según parece, a partir de la versión 11.1.6.20 la clase OracleBulkCopy ya se encuentra incluida en el ODP.NET. En agosto del 2011 la última versión disponible del ODP.NET era la 11.2.0.2" + Environment.NewLine);
        }
        #endregion

        #region EjecutarEscalar
        /// <summary>
        /// Ejecutar una sentencia T-SQL con timeout por defecto.
        /// </summary>
        /// <param name="sql">Consulta SQL.</param>
        /// <returns>La primera columna de la primera fila retornada por
        /// la query, ignora el resto de columnas retornadas.</returns>
        public object EjecutarEscalar(string sql)
        {
            return this.EjecutarEscalar(sql, this.Timeout);
        }
        /// <summary>
        /// Ejecutar una sentencia T-SQL.
        /// </summary>
        /// <param name="sql">Consulta SQL.</param>
        /// <param name="timeout">Timeout de SqlCommand.</param>
        /// <returns>La primera columna de la primera fila retornada por
        /// la query, ignora el resto de columnas retornadas.</returns>
        public object EjecutarEscalar(string sql, int timeout)
        {
            // Inicializar resultado.
            object resultado = null;
            try
            {
                using (OracleConnection conexion = new OracleConnection(this.CadenaConexion))
                using (OracleCommand command = new OracleCommand(sql, conexion))
                {
                    // Abrir conexión.
                    conexion.Open();
                    // Establecer timeout de ejecución.
                    command.CommandTimeout = timeout;
                    // Asignar el tipo de comando text.
                    command.CommandType = CommandType.Text;
                    // Asignar T-SQL a ejecutar.
                    command.CommandText = sql;
                    // Ejecutar query.
                    resultado = command.ExecuteScalar();
                    // Cerrar conexión
                    conexion.Close();
                }

                return resultado;
            }
            finally 
            { 
                if (resultado != null) 
                { 
                    resultado = null; 
                } 
            }
        }
        #endregion

        #endregion       
    }
}