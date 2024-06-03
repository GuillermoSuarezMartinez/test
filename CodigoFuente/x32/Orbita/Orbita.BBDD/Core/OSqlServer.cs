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
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
namespace Orbita.BBDD
{
    /// <summary>
    /// Clase tipo para instanciar objetos de base de datos OSQLServer.
    /// </summary>
    public class OSqlServer : OBBDDCore
    {
        #region Clases internas
        /// <summary>
        /// Clase tipo para instanciar objetos de base de datos SQL Server,
        /// útil cuando se llaman procedimientos almacenados concatenados,
        /// de forma que la transacción es iniciada y finalizada desde los
        /// propios métodos transaccionales.
        /// </summary>
        public class Transaccional : OSqlServer
        {
            #region Atributo privados estáticos
            /// <summary>
            /// Colección estática de conexiones.
            /// </summary>
            static OConexiones Conexiones = new OConexiones();
            #endregion

            #region Constructores
            /// <summary>
            /// Inicializar una nueva instancia de la clase Transaccional de SQL Server.
            /// </summary>
            /// <param name="infoConexion">Información de la conexión actual.</param>
            public Transaccional(OInfoConexion infoConexion)
                : base(infoConexion) { }
            /// <summary>
            /// Inicializar una nueva instancia de la clase Transaccional de SQL Server.
            /// </summary>
            /// <param name="infoConexion">Información de la conexión actual con mirroring.</param>
            public Transaccional(OInfoMirroring infoConexion)
                : base(infoConexion) { }
            #endregion

            #region Métodos públicos

            /// <summary>
            /// Obtener la conexión de la colección de conexiones existente.
            /// </summary>
            /// <param name="identificadorTransaccion">Identificador de transacción.</param>
            /// <returns></returns>
            public OConexion Conexion(string identificadorTransaccion)
            {
                // Obtener la conexión relativa al identificador de transacción.
                return Conexiones[new Guid(identificadorTransaccion)];
            }
            /// <summary>
            /// Crear la conexión e iniciar la transacción.
            /// </summary>
            /// <returns>Identificador de transacción.</returns>
            public string AbrirTransaccion()
            {
                return Conexiones[this.CadenaConexion].Iniciar();
            }
            /// <summary>
            /// Crear la conexión e iniciar la transacción con un nivel de bloqueo específico.
            /// </summary>
            /// <param name="iso">Especifica el comportamiento de bloqueo de la transacción para la conexión.</param>
            /// <returns>Identificador de transacción.</returns>
            public string AbrirTransaccion(IsolationLevel iso)
            {
                return Conexiones[this.CadenaConexion].Iniciar(iso);
            }
            /// <summary>
            /// Confirmar la transacción y cerrar la conexión.
            /// </summary>
            /// <param name="identificadorTransaccion">Identificador de la transacción. 
            /// Utilizar el método AbrirTransaccion() para generar un identificador de 
            /// transacción válido.</param>
            public void CerrarTransaccion(string identificadorTransaccion)
            {
                // Obtener la conexión relativa al identificador de transacción.
                OConexion conexion = Conexiones[new Guid(identificadorTransaccion)];
                if (conexion != null)
                {
                    try
                    {
                        // Confirmar transacción.
                        conexion.Transaccion.Commit();
                        // Liberar la transacción.
                        conexion.Transaccion.Dispose();

                        // Cerrar la conexión.
                        conexion.Conexion.Close();
                        // Liberar la conexión.
                        conexion.Conexion.Dispose();
                    }
                    catch (SqlException)
                    {
                        // Control en la excepción de la transacción.
                        if (conexion.Transaccion != null)
                        {
                            // Liberar transacción.
                            conexion.Transaccion.Rollback();
                        }

                        // Control en la excepción de la conexión.
                        if (conexion.Conexion != null)
                        {
                            if (conexion.Conexion.State != ConnectionState.Closed)
                            {
                                // Cerrar la conexión.
                                conexion.Conexion.Close();
                            }
                            // Liberar conexión.
                            conexion.Conexion.Dispose();
                        }
                    }
                }
            }
            /// <summary>
            /// Deshacer la transacción y cerrar la conexión.
            /// </summary>
            /// <param name="identificadorTransaccion">Identificador de la transacción. 
            /// Utilizar el método AbrirTransaccion() para generar un identificador de 
            /// transacción válido.</param>
            public void DeshacerTransaccion(string identificadorTransaccion)
            {
                // Obtener la conexión relativa al identificador de transacción.
                OConexion conexion = Conexiones[new Guid(identificadorTransaccion)];
                if (conexion != null)
                {
                    // Control en la excepción de la transacción.
                    if (conexion.Transaccion != null)
                    {
                        // Liberar transacción.
                        conexion.Transaccion.Rollback();
                    }

                    // Control en la excepción de la conexión.
                    if (conexion.Conexion != null)
                    {
                        if (conexion.Conexion.State != ConnectionState.Closed)
                        {
                            // Cerrar la conexión.
                            conexion.Conexion.Close();
                        }
                        // Liberar conexión.
                        conexion.Conexion.Dispose();
                    }
                }
            }

            #region EjecutarProcedimientoAlmacenado
            /// <summary>
            /// Ejecutar un procedimiento almacenado con timeout por defecto.
            /// </summary>
            /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
            /// <param name="identificadorTransaccion">Identificador de la transacción. 
            /// Utilizar el método AbrirTransaccion() para generar un identificador de 
            /// transacción válido.</param>
            /// <returns>Resultado del procedimiento almacenado.</returns>
            public int EjecutarProcedimientoAlmacenado(string procedimiento, string identificadorTransaccion)
            {
                return EjecutarProcedimientoAlmacenado(procedimiento, this.Timeout, identificadorTransaccion);
            }
            /// <summary>
            /// Ejecutar un procedimiento almacenado.
            /// </summary>
            /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
            /// <param name="timeout">Timeout de SqlCommand.</param>
            /// <param name="identificadorTransaccion">Identificador de la transacción. 
            /// Utilizar el método AbrirTransaccion() para generar un identificador de 
            /// transacción válido.</param>
            /// <returns>Resultado del procedimiento almacenado.</returns>
            [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "StoredProc proviene de un archivo de recursos y parámetros.")]
            public static int EjecutarProcedimientoAlmacenado(string procedimiento, int timeout, string identificadorTransaccion)
            {
                // Inicializar resultado.
                int resultado = -1;
                // Obtener la conexión relativa al identificador de transacción.
                OConexion conexion = Conexiones[new Guid(identificadorTransaccion)];
                if (conexion != null)
                {
                    using (SqlCommand command = new SqlCommand(procedimiento, conexion.Conexion, conexion.Transaccion))
                    {
                        // Establecer timeout de ejecución.
                        command.CommandTimeout = timeout;
                        // Asignar el tipo de comando procedimiento almacenado.
                        command.CommandType = CommandType.StoredProcedure;
                        // Asignar procedimiento almacenado a ejecutar.
                        command.CommandText = procedimiento;
                        // Inicializar parámetro de retorno del procedimiento almacenado.
                        SqlParameter parametroDeRetorno = new SqlParameter("RETURN", SqlDbType.Int);
                        // Establecer dirección de salida del parámetro.
                        parametroDeRetorno.Direction = ParameterDirection.ReturnValue;
                        // Añadir el parámetro de retorno al SqlCommand.
                        command.Parameters.Add(parametroDeRetorno);
                        // Ejecutar query.
                        command.ExecuteNonQuery();
                        // Asignar resultado del parámetro de retorno.
                        resultado = (int)parametroDeRetorno.Value;
                    }
                }
                return resultado;
            }
            /// <summary>
            /// Ejecutar un procedimiento almacenado con timeout por defecto.
            /// </summary>
            /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
            /// <param name="parametros">Parámetros de entrada del procedimiento almacenado.</param>
            /// <param name="identificadorTransaccion">Identificador de la transacción. 
            /// Utilizar el método AbrirTransaccion() para generar un identificador de 
            /// transacción válido.</param>
            /// <returns>Resultado del procedimiento almacenado.</returns>
            public int EjecutarProcedimientoAlmacenado(string procedimiento, ArrayList parametros, string identificadorTransaccion)
            {
                return EjecutarProcedimientoAlmacenado(procedimiento, parametros, this.Timeout, identificadorTransaccion);
            }
            /// <summary>
            /// Ejecutar un procedimiento almacenado.
            /// </summary>
            /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
            /// <param name="parametros">Parámetros de entrada del procedimiento almacenado.</param>
            /// <param name="timeout">Timeout de SqlCommand.</param>
            /// <param name="identificadorTransaccion">Identificador de la transacción. 
            /// Utilizar el método AbrirTransaccion() para generar un identificador de 
            /// transacción válido.</param>
            /// <returns>Resultado del procedimiento almacenado.</returns>
            [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "StoredProc proviene de un archivo de recursos y parámetros.")]
            public static int EjecutarProcedimientoAlmacenado(string procedimiento, ArrayList parametros, int timeout, string identificadorTransaccion)
            {
                // Inicializar resultado.
                int resultado = -1;
                // Obtener la conexión relativa al identificador de transacción.
                OConexion conexion = Conexiones[new Guid(identificadorTransaccion)];
                if (conexion != null)
                {
                    using (SqlCommand command = new SqlCommand(procedimiento, conexion.Conexion, conexion.Transaccion))
                    {
                        // Establecer timeout de ejecución.
                        command.CommandTimeout = timeout;
                        // Asignar el tipo de comando procedimiento almacenado.
                        command.CommandType = CommandType.StoredProcedure;
                        // Asignar procedimiento almacenado a ejecutar.
                        command.CommandText = procedimiento;
                        // Parámetros de entrada al procedimiento almacenado.
                        if (parametros != null)
                        {
                            // Asignar parámetros a SqlCommand.
                            foreach (SqlParameter parametro in parametros)
                            {
                                command.Parameters.Add(parametro);
                            }
                        }
                        // Inicializar parámetro de retorno del procedimiento almacenado.
                        SqlParameter parametroDeRetorno = new SqlParameter("RETURN", SqlDbType.Int);
                        // Establecer dirección de salida del parámetro.
                        parametroDeRetorno.Direction = ParameterDirection.ReturnValue;
                        // Añadir el parámetro de retorno al SqlCommand.
                        command.Parameters.Add(parametroDeRetorno);
                        // Ejecutar query.
                        command.ExecuteNonQuery();
                        // Asignar resultado del parámetro de retorno.
                        resultado = (int)parametroDeRetorno.Value;
                    }
                }
                return resultado;
            }
            #endregion

            #region EjecutarProcedimientoAlmacenadoSinRetorno
            /// <summary>
            /// Ejecutar un procedimiento almacenado con timeout por defecto, prescindiendo 
            /// del parámetro "return" del procedimiento almacenado.
            /// </summary>
            /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
            /// <param name="identificadorTransaccion">Identificador de la transacción. 
            /// Utilizar el método AbrirTransaccion() para generar un identificador de 
            /// transacción válido.</param>
            /// <returns>Número de registros afectados en el procedimiento almacenado.</returns>
            public int EjecutarProcedimientoAlmacenadoSinRetorno(string procedimiento, string identificadorTransaccion)
            {
                return EjecutarProcedimientoAlmacenadoSinRetorno(procedimiento, this.Timeout, identificadorTransaccion);
            }
            /// <summary>
            /// Ejecutar un procedimiento almacenado, prescindiendo del parámetro "return" 
            /// del procedimiento almacenado.
            /// </summary>
            /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
            /// <param name="timeout">Timeout de SqlCommand.</param>
            /// <param name="identificadorTransaccion">Identificador de la transacción. 
            /// Utilizar el método AbrirTransaccion() para generar un identificador de 
            /// transacción válido.</param>
            /// <returns>Número de registros afectados en el procedimiento almacenado.</returns>
            [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "StoredProc proviene de un archivo de recursos y parámetros.")]
            public static int EjecutarProcedimientoAlmacenadoSinRetorno(string procedimiento, int timeout, string identificadorTransaccion)
            {
                // Inicializar resultado.
                int resultado = -1;
                // Obtener la conexión relativa al identificador de transacción.
                OConexion conexion = Conexiones[new Guid(identificadorTransaccion)];
                if (conexion != null)
                {
                    using (SqlCommand command = new SqlCommand(procedimiento, conexion.Conexion, conexion.Transaccion))
                    {
                        // Establecer timeout de ejecución.
                        command.CommandTimeout = timeout;
                        // Asignar el tipo de comando procedimiento almacenado.
                        command.CommandType = CommandType.StoredProcedure;
                        // Asignar procedimiento almacenado a ejecutar.
                        command.CommandText = procedimiento;
                        // Ejecutar query.
                        resultado = command.ExecuteNonQuery();
                    }
                }
                return resultado;
            }
            /// <summary>
            /// Ejecutar un procedimiento almacenado con parámetros de entrada y timeout por defecto, 
            /// prescindiendo del parámetro "return" del procedimiento almacenado.
            /// </summary>
            /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
            /// <param name="parametros">Parámetros de entrada del procedimiento almacenado.</param>
            /// <param name="identificadorTransaccion">Identificador de la transacción. 
            /// Utilizar el método AbrirTransaccion() para generar un identificador de 
            /// transacción válido.</param>
            /// <returns>Número de registros afectados en el procedimiento almacenado.</returns>
            public int EjecutarProcedimientoAlmacenadoSinRetorno(string procedimiento, ArrayList parametros, string identificadorTransaccion)
            {
                return EjecutarProcedimientoAlmacenadoSinRetorno(procedimiento, parametros, this.Timeout, identificadorTransaccion);
            }
            /// <summary>
            /// Ejecutar un procedimiento almacenado con parámetros de entrada y timeout por 
            /// defecto, prescindiendo del parámetro "return" del procedimiento almacenado.
            /// </summary>
            /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
            /// <param name="parametros">Parámetros de entrada del procedimiento almacenado.</param>
            /// <param name="timeout">Timeout de SqlCommand.</param>
            /// <param name="identificadorTransaccion">Identificador de la transacción. 
            /// Utilizar el método AbrirTransaccion() para generar un identificador de 
            /// transacción válido.</param>
            /// <returns>Número de registros afectados en el procedimiento almacenado.</returns>
            [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "StoredProc proviene de un archivo de recursos y parámetros.")]
            public static int EjecutarProcedimientoAlmacenadoSinRetorno(string procedimiento, ArrayList parametros, int timeout, string identificadorTransaccion)
            {
                // Inicializar resultado.
                int resultado = -1;
                // Obtener la conexión relativa al identificador de transacción.
                OConexion conexion = Conexiones[new Guid(identificadorTransaccion)];
                if (conexion != null)
                {
                    using (SqlCommand command = new SqlCommand(procedimiento, conexion.Conexion, conexion.Transaccion))
                    {
                        // Establecer timeout de ejecución.
                        command.CommandTimeout = timeout;
                        // Asignar el tipo de comando procedimiento almacenado.
                        command.CommandType = CommandType.StoredProcedure;
                        // Asignar procedimiento almacenado a ejecutar.
                        command.CommandText = procedimiento;
                        // Parámetros de entrada al procedimiento almacenado.
                        if (parametros != null)
                        {
                            // Asignar parámetros a SqlCommand.
                            foreach (SqlParameter parametro in parametros)
                            {
                                command.Parameters.Add(parametro);
                            }
                        }
                        // Ejecutar query.
                        resultado = command.ExecuteNonQuery();
                    }
                }
                return resultado;
            }
            #endregion

            #endregion
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase SQLServer.
        /// Connection strings for SQL Server 2005.
        /// .NET Framework Data Provider for SQL Server.
        /// Type: .NET Framework Class Library.
        /// Usage: System.Data.SqlClient.SqlConnection.
        /// Manufacturer: Microsoft.
        /// Standard Security.
        /// </summary>
        /// <param name="infoConexion">Información de la conexión actual.</param>
        public OSqlServer(OInfoConexion infoConexion)
            : base(infoConexion)
        {
            if (infoConexion != null)
            {
                this.CadenaConexion = string.Format(CultureInfo.CurrentCulture, @"Data Source={0}; Initial Catalog={1}; User Id={2}; Password={3}; Connect Timeout={4}", 
                    infoConexion.Instancia, infoConexion.BaseDatos, infoConexion.Usuario, infoConexion.Password, infoConexion.Timeout);
            }
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase SQLServer.
        /// Connection strings for SQL Server 2005.
        /// .NET Framework Data Provider for SQL Server.
        /// Type: .NET Framework Class Library.
        /// Usage: System.Data.SqlClient.SqlConnection.
        /// Manufacturer: Microsoft.
        /// Mirroring Security.
        /// </summary>
        /// <param name="infoConexion">Información de la conexión actual con mirroring.</param>
        public OSqlServer(OInfoMirroring infoConexion)
            : base(infoConexion)
        {
            if (infoConexion != null)
            {
                this.CadenaConexion = string.Format(CultureInfo.CurrentCulture, @"Data Source={0}; Failover Partner={1}; Initial Catalog={2}; User Id={3}; Password={4}; Connect Timeout={5}", 
                    infoConexion.Instancia, infoConexion.Mirror, infoConexion.BaseDatos, infoConexion.Usuario, infoConexion.Password, infoConexion.Timeout);
            }
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Nombre del servidor en mirror.
        /// </summary>
        public string Mirror
        {
            get { return ((OInfoMirroring)this.InfoConexion).Mirror; }
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
            using (SqlConnection conexion = new SqlConnection(this.CadenaConexion))
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
        /// <returns>DataTable con el resultado.</returns>
        public DataTable SeleccionSql(string sql)
        {
            return this.SeleccionSql(sql, this.Timeout);
        }
        /// <summary>
        /// Ejecutar una consulta Select.
        /// </summary>
        /// <param name="sql">Consulta T-SQL.</param>
        /// <param name="timeout">Timeout de SqlCommand.</param>
        /// <returns>DataTable con el resultado.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "SQL proviene de un archivo de recursos y parámetros.")]
        public DataTable SeleccionSql(string sql, int timeout)
        {
            // Inicializar resultado.
            DataTable resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(this.CadenaConexion))
                using (SqlCommand command = new SqlCommand(sql, conexion))
                {
                    // Abrir conexión.
                    conexion.Open();
                    // Establecer timeout de ejecución.
                    command.CommandTimeout = timeout;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        // Asignar resultados al DataTable.
                        adapter.Fill(resultado = new DataTable());
                        // Establecer una configuración regional actual.
                        resultado.Locale = CultureInfo.CurrentCulture;
                    }
                }
                // La conexión se cierra automáticamente en este punto, es decir,
                // en el final del bloque using (SqlConnection conexion = new ...).
                // http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.close.aspx
                return resultado;
            }
            finally  { if (resultado != null) { resultado.Dispose(); } }
        }
        #endregion

        #region SeleccionProcedimientoAlmacenado
        /// <summary>
        /// Ejecutar una sentencia Select en un procedimiento almacenado con timeout por defecto.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <returns>DataTable con el resultado.</returns>
        public DataTable SeleccionProcedimientoAlmacenado(string procedimiento)
        {
            return this.SeleccionProcedimientoAlmacenado(procedimiento, this.Timeout);
        }
        /// <summary>
        /// Ejecutar una sentencia Select en un procedimiento almacenado con timeout por defecto.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <param name="nombreTabla">Nombre de System.Data.DataTable.</param>
        /// <returns>DataSet con el resultado.</returns>
        public DataSet SeleccionProcedimientoAlmacenado(string procedimiento, string nombreTabla)
        {
            return this.SeleccionProcedimientoAlmacenado(procedimiento, nombreTabla, this.Timeout);
        }
        /// <summary>
        /// Ejecutar una sentencia Select en un procedimiento almacenado.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <param name="timeout">Timeout de SqlCommand.</param>
        /// <returns>DataTable con el resultado.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "StoredProc proviene de un archivo de recursos y parámetros.")]
        public DataTable SeleccionProcedimientoAlmacenado(string procedimiento, int timeout)
        {
            // Inicializar resultado.
            DataTable resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(this.CadenaConexion))
                using (SqlCommand command = new SqlCommand(procedimiento, conexion))
                {
                    // Abrir conexión.
                    conexion.Open();
                    // Establecer timeout de ejecución.
                    command.CommandTimeout = timeout;
                    // Asignar el tipo de comando procedimiento almacenado.
                    command.CommandType = CommandType.StoredProcedure;
                    // Asignar procedimiento almacenado a ejecutar.
                    command.CommandText = procedimiento;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        // Asignar resultados al DataTable.
                        adapter.Fill(resultado = new DataTable());
                        // Establecer una configuración regional actual.
                        resultado.Locale = CultureInfo.CurrentCulture;
                    }
                }
                // La conexión se cierra automáticamente en este punto, es decir,
                // en el final del bloque using (SqlConnection conexion = new ...).
                // http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.close.aspx
                return resultado;
            }
            finally { if (resultado != null) { resultado.Dispose(); } }
        }
        /// <summary>
        /// Ejecutar una sentencia Select en un procedimiento almacenado.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <param name="nombreTabla">Nombre de System.Data.DataTable.</param>
        /// <param name="timeout">Timeout de SqlCommand.</param>
        /// <returns>DataSet con el resultado.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "StoredProc proviene de un archivo de recursos y parámetros.")]
        public DataSet SeleccionProcedimientoAlmacenado(string procedimiento, string nombreTabla, int timeout)
        {
            // Inicializar resultado.
            DataSet resultado = null;
            try
            {
                // Obtener el resultado en DataTable del método sobrecargado.
                DataTable dt = SeleccionProcedimientoAlmacenado(procedimiento, timeout);
                // Establecer el nombre de la tabla del DataSet.
                dt.TableName = nombreTabla;
                // Crear el DataSet adecuado en el retorno.
                resultado = new DataSet();
                // Establecer una configuración regional actual.
                resultado.Locale = CultureInfo.CurrentCulture;
                // Agregar el objeto DataTable a la colección.
                resultado.Tables.Add(dt);
                return resultado;
            }
            finally { if (resultado != null) { resultado.Dispose(); } }
        }
        /// <summary>
        /// Ejecutar una sentencia Select en un procedimiento almacenado con parámetros de entrada y con timeout por defecto.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Parámetros de entrada del procedimiento almacenado.</param>
        /// <returns>DataTable con el resultado.</returns>
        public DataTable SeleccionProcedimientoAlmacenado(string procedimiento, ArrayList parametros)
        {
            return this.SeleccionProcedimientoAlmacenado(procedimiento, parametros, this.Timeout);
        }
        /// <summary>
        /// Ejecutar una sentencia Select en un procedimiento almacenado con parámetros de entrada y con timeout por defecto.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <param name="nombreTabla">Nombre de System.Data.DataTable.</param>
        /// <param name="parametros">Parámetros de entrada del procedimiento almacenado.</param>
        /// <returns>DataSet con el resultado.</returns>
        public DataSet SeleccionProcedimientoAlmacenado(string procedimiento, string nombreTabla, ArrayList parametros)
        {
            return this.SeleccionProcedimientoAlmacenado(procedimiento, nombreTabla, parametros, this.Timeout);
        }
        /// <summary>
        /// Ejecutar una sentencia Select en un procedimiento almacenado con parámetros de entrada.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <param name="parametros">Parámetros de entrada del procedimiento almacenado.</param>
        /// <param name="timeout">Timeout de SqlCommand.</param>
        /// <returns>DataTable con el resultado.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "StoredProc proviene de un archivo de recursos y parámetros.")]
        public DataTable SeleccionProcedimientoAlmacenado(string procedimiento, ArrayList parametros, int timeout)
        {
            // Inicializar resultado.
            DataTable resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(this.CadenaConexion))
                using (SqlCommand command = new SqlCommand(procedimiento, conexion))
                {
                    // Abrir conexión.
                    conexion.Open();
                    // Establecer timeout de ejecución.
                    command.CommandTimeout = timeout;
                    // Asignar el tipo de comando procedimiento almacenado.
                    command.CommandType = CommandType.StoredProcedure;
                    // Asignar procedimiento almacenado a ejecutar.
                    command.CommandText = procedimiento;
                    // Parámetros de entrada al procedimiento almacenado.
                    if (parametros != null)
                    {
                        // Asignar parámetros a SqlCommand.
                        foreach (SqlParameter parametro in parametros)
                        {
                            command.Parameters.Add(parametro);
                        }
                    }
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        // Asignar resultados al DataTable.
                        adapter.Fill(resultado = new DataTable());
                        // Establecer una configuración regional actual.
                        resultado.Locale = CultureInfo.CurrentCulture;
                        resultado.TableName = "Orbita";
                    }
                }
                // La conexión se cierra automáticamente en este punto, es decir,
                // en el final del bloque using (SqlConnection conexion = new ...).
                // http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.close.aspx
                return resultado;
            }
            finally { if (resultado != null) { resultado.Dispose(); } }
        }
        /// <summary>
        /// Ejecutar una sentencia Select en un procedimiento almacenado con parámetros de entrada.
        /// </summary>
        /// <param name="procedimiento">Nombre del procedimiento almacenado.</param>
        /// <param name="nombreTabla">Nombre de System.Data.DataTable.</param>
        /// <param name="parametros">Parámetros de entrada del procedimiento almacenado.</param>
        /// <param name="timeout">Timeout de SqlCommand.</param>
        /// <returns>DataTable con el resultado.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "StoredProc proviene de un archivo de recursos y parámetros.")]
        public DataSet SeleccionProcedimientoAlmacenado(string procedimiento, string nombreTabla, ArrayList parametros, int timeout)
        {
            // Inicializar resultado.
            DataSet resultado = null;
            try
            {
                // Obtener el resultado en DataTable del método sobrecargado.
                DataTable dt = SeleccionProcedimientoAlmacenado(procedimiento, parametros, timeout);
                // Establecer el nombre de la tabla del DataSet.
                dt.TableName = nombreTabla;
                // Crear el DataSet adecuado en el retorno.
                resultado = new DataSet();
                // Establecer una configuración regional actual.
                resultado.Locale = CultureInfo.CurrentCulture;
                // Agregar el objeto DataTable a la colección.
                resultado.Tables.Add(dt);
                return resultado;
            }
            finally { if (resultado != null) { resultado.Dispose(); } }
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
            // Inicializar resultado.
            int resultado = -1;
            using (SqlConnection conexion = new SqlConnection(this.CadenaConexion))
            using (SqlCommand command = new SqlCommand(procedimiento, conexion))
            {
                // Abrir conexión.
                conexion.Open();
                // Establecer timeout de ejecución.
                command.CommandTimeout = timeout;
                // Asignar el tipo de comando procedimiento almacenado.
                command.CommandType = CommandType.StoredProcedure;
                // Asignar procedimiento almacenado a ejecutar.
                command.CommandText = procedimiento;
                // Inicializar parámetro de retorno del procedimiento almacenado.
                SqlParameter parametroDeRetorno = new SqlParameter("RETURN", SqlDbType.Int);
                // Establecer dirección de salida del parámetro.
                parametroDeRetorno.Direction = ParameterDirection.ReturnValue;
                // Añadir el parámetro de retorno al SqlCommand.
                command.Parameters.Add(parametroDeRetorno);
                // Ejecutar query.
                command.ExecuteNonQuery();
                // Asignar resultado del parámetro de retorno.
                resultado = (int)parametroDeRetorno.Value;
            }
            // La conexión se cierra automáticamente en este punto, es decir,
            // en el final del bloque using (SqlConnection conexion = new ...).
            // http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.close.aspx
            return resultado;
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
            // Inicializar resultado.
            int resultado = -1;
            using (SqlConnection conexion = new SqlConnection(this.CadenaConexion))
            using (SqlCommand command = new SqlCommand(procedimiento, conexion))
            {
                // Abrir conexión.
                conexion.Open();
                // Establecer timeout de ejecución.
                command.CommandTimeout = timeout;
                // Asignar el tipo de comando procedimiento almacenado.
                command.CommandType = CommandType.StoredProcedure;
                // Asignar procedimiento almacenado a ejecutar.
                command.CommandText = procedimiento;
                // Parámetros de entrada al procedimiento almacenado.
                if (parametros != null)
                {
                    // Asignar parámetros a SqlCommand.
                    foreach (SqlParameter parametro in parametros)
                    {
                        command.Parameters.Add(parametro);
                    }
                }
                // Inicializar parámetro de retorno del procedimiento almacenado.
                SqlParameter parametroDeRetorno = new SqlParameter("RETURN", SqlDbType.Int);
                // Establecer dirección de salida del parámetro.
                parametroDeRetorno.Direction = ParameterDirection.ReturnValue;
                // Añadir el parámetro de retorno al SqlCommand.
                command.Parameters.Add(parametroDeRetorno);
                // Ejecutar query.
                command.ExecuteNonQuery();
                // Asignar resultado del parámetro de retorno.
                resultado = (int)parametroDeRetorno.Value;
            }
            // La conexión se cierra automáticamente en este punto, es decir,
            // en el final del bloque using (SqlConnection conexion = new ...).
            // http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.close.aspx
            return resultado;
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
            // Inicializar resultado.
            int resultado = -1;
            using (SqlConnection conexion = new SqlConnection(this.CadenaConexion))
            using (SqlCommand command = new SqlCommand(procedimiento, conexion))
            {
                // Abrir conexión.
                conexion.Open();
                // Establecer timeout de ejecución.
                command.CommandTimeout = timeout;
                // Asignar el tipo de comando procedimiento almacenado.
                command.CommandType = CommandType.StoredProcedure;
                // Asignar procedimiento almacenado a ejecutar.
                command.CommandText = procedimiento;
                // Ejecutar query.
                resultado = command.ExecuteNonQuery();
            }
            // La conexión se cierra automáticamente en este punto, es decir,
            // en el final del bloque using (SqlConnection conexion = new ...).
            // http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.close.aspx
            return resultado;
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
            // Inicializar resultado.
            int resultado = -1;
            using (SqlConnection conexion = new SqlConnection(this.CadenaConexion))
            using (SqlCommand command = new SqlCommand(procedimiento, conexion))
            {
                // Abrir conexión.
                conexion.Open();
                // Establecer timeout de ejecución.
                command.CommandTimeout = timeout;
                // Asignar el tipo de comando procedimiento almacenado.
                command.CommandType = CommandType.StoredProcedure;
                // Asignar procedimiento almacenado a ejecutar.
                command.CommandText = procedimiento;
                // Parámetros de entrada al procedimiento almacenado.
                if (parametros != null)
                {
                    // Asignar parámetros a SqlCommand.
                    foreach (SqlParameter parametro in parametros)
                    {
                        command.Parameters.Add(parametro);
                    }
                }
                // Ejecutar query.
                resultado = command.ExecuteNonQuery();
            }
            // La conexión se cierra automáticamente en este punto, es decir,
            // en el final del bloque using (SqlConnection conexion = new ...).
            // http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.close.aspx
            return resultado;
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
            // Inicializar resultado.
            int resultado = -1;
            using (SqlConnection conexion = new SqlConnection(this.CadenaConexion))
            using (SqlTransaction transaccion = conexion.BeginTransaction())
            using (SqlCommand command = new SqlCommand(procedimiento, conexion, transaccion))
            {
                try
                {
                    // Abrir conexión.
                    conexion.Open();
                    // Establecer timeout de ejecución en segundos.
                    command.CommandTimeout = timeout;
                    // Asignar el tipo de comando procedimiento almacenado.
                    command.CommandType = CommandType.StoredProcedure;
                    // Asignar procedimiento almacenado a ejecutar.
                    command.CommandText = procedimiento;
                    // Inicializar parámetro de retorno del procedimiento almacenado.
                    SqlParameter parametroDeRetorno = new SqlParameter("RETURN", SqlDbType.Int);
                    // Establecer dirección de salida del parámetro.
                    parametroDeRetorno.Direction = ParameterDirection.ReturnValue;
                    // Añadir el parámetro de retorno al SqlCommand.
                    command.Parameters.Add(parametroDeRetorno);
                    // Ejecutar query.
                    command.ExecuteNonQuery();
                    // Asignar resultado del parámetro de retorno.
                    resultado = (int)parametroDeRetorno.Value;
                    // Confirmar transacción.
                    transaccion.Commit();
                }
                catch (SqlException)
                {
                    // Liberar transacción.
                    transaccion.Rollback();
                }
            }
            // La conexión se cierra automáticamente en este punto, es decir,
            // en el final del bloque using (SqlConnection conexion = new ...).
            // http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.close.aspx
            return resultado;
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
            // Inicializar resultado.
            int resultado = -1;
            using (SqlConnection conexion = new SqlConnection(this.CadenaConexion))
            using (SqlTransaction transaccion = conexion.BeginTransaction())
            using (SqlCommand command = new SqlCommand(procedimiento, conexion, transaccion))
            {
                try
                {
                    // Abrir conexión.
                    conexion.Open();
                    // Establecer timeout de ejecución en segundos.
                    command.CommandTimeout = timeout;
                    // Asignar el tipo de comando procedimiento almacenado.
                    command.CommandType = CommandType.StoredProcedure;
                    // Asignar procedimiento almacenado a ejecutar.
                    command.CommandText = procedimiento;
                    // Parámetros de entrada al procedimiento almacenado.
                    if (parametros != null)
                    {
                        // Asignar parámetros a SqlCommand.
                        foreach (SqlParameter parametro in parametros)
                        {
                            command.Parameters.Add(parametro);
                        }
                    }
                    // Inicializar parámetro de retorno del procedimiento almacenado.
                    SqlParameter parametroDeRetorno = new SqlParameter("RETURN", SqlDbType.Int);
                    // Establecer dirección de salida del parámetro.
                    parametroDeRetorno.Direction = ParameterDirection.ReturnValue;
                    // Añadir el parámetro de retorno al SqlCommand.
                    command.Parameters.Add(parametroDeRetorno);
                    // Ejecutar query.
                    command.ExecuteNonQuery();
                    // Asignar resultado del parámetro de retorno.
                    resultado = (int)parametroDeRetorno.Value;
                    // Confirmar transacción.
                    transaccion.Commit();
                }
                catch (SqlException)
                {
                    // Liberar transacción.
                    transaccion.Rollback();
                }
            }
            // La conexión se cierra automáticamente en este punto, es decir,
            // en el final del bloque using (SqlConnection conexion = new ...).
            // http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.close.aspx
            return resultado;
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
            // Inicializar resultado.
            int resultado = -1;
            using (SqlConnection conexion = new SqlConnection(this.CadenaConexion))
            using (SqlTransaction transaccion = conexion.BeginTransaction())
            using (SqlCommand command = new SqlCommand(procedimiento, conexion, transaccion))
            {
                try
                {
                    // Abrir conexión.
                    conexion.Open();
                    // Establecer timeout de ejecución.
                    command.CommandTimeout = timeout;
                    // Asignar el tipo de comando procedimiento almacenado.
                    command.CommandType = CommandType.StoredProcedure;
                    // Asignar procedimiento almacenado a ejecutar.
                    command.CommandText = procedimiento;
                    // Ejecutar query.
                    resultado = command.ExecuteNonQuery();
                    // Confirmar transacción.
                    transaccion.Commit();
                }
                catch (SqlException)
                {
                    // Liberar transacción.
                    transaccion.Rollback();
                }
            }
            // La conexión se cierra automáticamente en este punto, es decir,
            // en el final del bloque using (SqlConnection conexion = new ...).
            // http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.close.aspx
            return resultado;
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
            // Inicializar resultado.
            int resultado = -1;
            using (SqlConnection conexion = new SqlConnection(this.CadenaConexion))
            using (SqlTransaction transaccion = conexion.BeginTransaction())
            using (SqlCommand command = new SqlCommand(procedimiento, conexion, transaccion))
            {
                try
                {
                    // Abrir conexión.
                    conexion.Open();
                    // Establecer timeout de ejecución.
                    command.CommandTimeout = timeout;
                    // Asignar el tipo de comando procedimiento almacenado.
                    command.CommandType = CommandType.StoredProcedure;
                    // Asignar procedimiento almacenado a ejecutar.
                    command.CommandText = procedimiento;
                    // Parámetros de entrada al procedimiento almacenado.
                    if (parametros != null)
                    {
                        // Asignar parámetros a SqlCommand.
                        foreach (SqlParameter parametro in parametros)
                        {
                            command.Parameters.Add(parametro);
                        }
                    }
                    // Ejecutar query.
                    resultado = command.ExecuteNonQuery();
                    // Confirmar transacción.
                    transaccion.Commit();
                }
                catch (SqlException)
                {
                    // Liberar transacción.
                    transaccion.Rollback();
                }
            }
            // La conexión se cierra automáticamente en este punto, es decir,
            // en el final del bloque using (SqlConnection conexion = new ...).
            // http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.close.aspx
            return resultado;
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
            using (SqlConnection conexion = new SqlConnection(this.CadenaConexion))
            using (SqlTransaction transaccion = conexion.BeginTransaction())
            using (SqlCommand command = new SqlCommand(sql, conexion, transaccion))
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
                catch (SqlException)
                {
                    // Liberar transacción.
                    transaccion.Rollback();
                }
            }
            // La conexión se cierra automáticamente en este punto, es decir,
            // en el final del bloque using (SqlConnection conexion = new ...).
            // http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.close.aspx
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
            using (SqlConnection conexion = new SqlConnection(this.CadenaConexion))
            using (SqlTransaction transaccion = conexion.BeginTransaction())
            using (SqlBulkCopy bulk = new SqlBulkCopy(conexion, SqlBulkCopyOptions.Default, transaccion))
            {
                try
                {
                    // Abrir conexión.
                    conexion.Open();
                    // Asignar la tabla destino.
                    bulk.DestinationTableName = tablaDestino;
                    if (dt != null)
                    {
                        // Ejecutar query.
                        bulk.WriteToServer(dt);
                        // Confirmar transacción.
                        transaccion.Commit();
                        // Asignar resultado correpondiente
                        // al número de filas contenidas en
                        // el DataTable.
                        resultado = dt.Rows.Count;
                    }
                }
                catch (SqlException)
                {
                    // Liberar transacción.
                    transaccion.Rollback();
                }
            }
            // La conexión se cierra automáticamente en este punto, es decir,
            // en el final del bloque using (SqlConnection conexion = new ...).
            // http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.close.aspx
            return resultado;
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
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "SQL proviene de un archivo de recursos y parámetros.")]
        public object EjecutarEscalar(string sql, int timeout)
        {
            // Inicializar resultado.
            object resultado = null;
            try
            {
                using (SqlConnection conexion = new SqlConnection(this.CadenaConexion))
                using (SqlCommand command = new SqlCommand(sql, conexion))
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
                }
                // La conexión se cierra automáticamente en este punto, es decir,
                // en el final del bloque using (SqlConnection conexion = new ...).
                // http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlconnection.close.aspx
                return resultado;
            }
            finally { if (resultado != null) { resultado = null; } }
        }
        #endregion

        #endregion       
    }
}