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
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
namespace Orbita.BBDD
{
    /// <summary>
    /// Clase tipo para instanciar objetos de base de datos OAccess.
    /// </summary>
    public class OAccess
    {
        #region Clases internas
        /// <summary>
        /// Tipo de consulta.
        /// </summary>
        public enum TipoConsulta
        {
            /// <summary>
            /// Indefinida.
            /// </summary>
            Nonde = 0,
            /// <summary>
            /// Select.
            /// </summary>
            ConsultaDESeleccion = 1,
            /// <summary>
            /// Insert.
            /// </summary>
            ConsultaDEInsercion = 2,
            /// <summary>
            /// Update.
            /// </summary>
            ConsultaDEActualizacion = 3,
            /// <summary>
            /// Delete.
            /// </summary>
            ConsultaDEEliminacion = 4,
        }
        /// <summary>
        /// Información de la conexión para Access.
        /// </summary>
        public class InfoConexion
        {
            #region Atributos privados
            /// <summary>
            /// Nombre de la ruta de base de datos.
            /// </summary>
            string ruta;
            /// <summary>
            /// Nombre del grupo de trabajo.
            /// </summary>
            string grupoTrabajo;
            /// <summary>
            /// Usuario de autenticación de base de datos.
            /// </summary>
            string usuario;
            /// <summary>
            /// Password de autenticación de usuario.
            /// </summary>
            string passwordUsuario;
            /// <summary>
            /// Password de autenticación de base de datos.
            /// </summary>
            string passwordBBDD;
            #endregion

            #region Constructores
            /// <summary>
            /// Inicializar una nueva instancia de la clase InfoConexion.
            /// </summary>
            public InfoConexion() { }
            #endregion

            #region Propiedades
            /// <summary>
            /// Nombre de la ruta de base de datos.
            /// </summary>
            public string Ruta
            {
                get { return this.ruta; }
                set { this.ruta = value; }
            }
            /// <summary>
            /// Nombre del grupo de trabajo.
            /// </summary>
            public string GrupoTrabajo
            {
                get { return this.grupoTrabajo; }
                set { this.grupoTrabajo = value; }
            }
            /// <summary>
            /// Usuario de autenticación de base de datos.
            /// </summary>
            public string Usuario
            {
                get { return this.usuario; }
                set { this.usuario = value; }
            }
            /// <summary>
            /// Password de autenticación de usuario.
            /// </summary>
            public string PasswordUsuario
            {
                get { return this.passwordUsuario; }
                set { this.passwordUsuario = value; }
            }
            /// <summary>
            /// Password de autenticación de base de datos.
            /// </summary>
            public string PasswordBbdd
            {
                get { return this.passwordBBDD; }
                set { this.passwordBBDD = value; }
            }
            #endregion
        }
        /// <summary>
        /// Contiene la información necesaria para realizar un comando NonQuery en Access.
        /// </summary>
        public class ComandoSql
        {
            #region Atributos privados
            /// <summary>
            /// T-SQL.
            /// </summary>
            string sentenciaSQL;
            /// <summary>
            /// Parámetros de entrada de la sentencia.
            /// </summary>
            ParametrosSql parametros;
            /// <summary>
            /// Excepción.
            /// </summary>
            Exception excepcion;
            /// <summary>
            /// Resultado de la query.
            /// </summary>
            int resultado;
            /// <summary>
            /// Tipo de consulta definida en el enumerado.
            /// </summary>
            TipoConsulta tipoConsulta;
            #endregion

            #region Constructores
            /// <summary>
            /// Inicializar una nueva instancia de la clase ComandoSQL.
            /// </summary>
            public ComandoSql(string sql, ParametrosSql parametros, TipoConsulta tipoConsulta)
            {
                this.SentenciaSql = sql;
                this.Parameters = parametros;
                this.TipoConsulta = tipoConsulta;
            }
            #endregion

            #region Propiedades
            /// <summary>
            /// T-SQL.
            /// </summary>
            public string SentenciaSql
            {
                get { return this.sentenciaSQL; }
                set { this.sentenciaSQL = value; }
            }
            /// <summary>
            /// Parámetros de entrada de la sentencia.
            /// </summary>
            public ParametrosSql Parameters
            {
                get { return this.parametros; }
                set { this.parametros = value; }
            }
            /// <summary>
            /// Excepción.
            /// </summary>
            public Exception Excepcion
            {
                get { return this.excepcion; }
                set { this.excepcion = value; }
            }
            /// <summary>
            /// Resultado de la query.
            /// </summary>
            public int Resultado
            {
                get { return this.resultado; }
                set { this.resultado = value; }
            }
            /// <summary>
            /// Tipo de consulta definida en el enumerado.
            /// </summary>
            public TipoConsulta TipoConsulta
            {
                get { return this.tipoConsulta; }
                set { this.tipoConsulta = value; }
            }
            #endregion
        }
        /// <summary>
        /// Creación de parámetros de los comandos SQL.
        /// </summary>
        [Serializable]
        public class ParametrosSql : Dictionary<string, object>
        {
            #region Métodos públicos
            /// <summary>
            /// Sobreescritura del método ToString().
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                string resultado = string.Empty;
                foreach (KeyValuePair<string, object> parameter in this)
                {
                    resultado += string.Format(CultureInfo.CurrentCulture, "Nombre: {0}", parameter.Key);
                    if ((parameter.Value == null) || (parameter.Value is DBNull))
                    {
                        resultado += "; Valor es NULL";
                    }
                    else
                    {
                        resultado += string.Format(CultureInfo.CurrentCulture, "; Valor: {0}", parameter.Value);
                    }
                    resultado += "\r\n";
                }
                return resultado;
            }
            #endregion
        }
        #endregion

        #region Atributos privados
        /// <summary>
        /// Contiene la información de conexión a la base de datos Access.
        /// </summary>
        readonly OAccess.InfoConexion infoConexion;
        #endregion

        #region Contructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase Access.
        /// Proveedor OLEDB.4.0.
        /// </summary>
        /// <param name="infoConexion">Información de la conexión actual.</param>
        public OAccess(OAccess.InfoConexion infoConexion)
        {
            this.infoConexion = infoConexion;
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Cadena de conexión.
        /// </summary>
        public string CadenaConexion
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture, @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}; Jet OLEDB:System Database={1}; 
                    User ID={2}; Password={3}; Jet OLEDB:Database Password={4}", this.infoConexion.Ruta, this.infoConexion.GrupoTrabajo,
                        this.infoConexion.Usuario, this.infoConexion.PasswordUsuario, this.infoConexion.PasswordBbdd);
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
            using (OleDbConnection conexion = new OleDbConnection(this.CadenaConexion))
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
        /// Ejecutar una consulta Select.
        /// </summary>
        /// <param name="sql">Consulta T-SQL.</param>
        /// <returns>Datatable con el resultado.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "SQL proviene de un archivo de recursos y parámetros.")]
        public DataTable SeleccionSql(string sql)
        {
            // Inicializar resultado.
            DataTable resultado = new DataTable("Resultado de la consulta a Access");
            using (OleDbConnection conexion = new OleDbConnection(this.CadenaConexion))
            using (OleDbCommand command = new OleDbCommand(sql, conexion))
            {
                OleDbDataReader aReader = null;
                // Abrir conexión.
                conexion.Open();

                // Ejecutar el comando SQL, asignando el resultado a un lector 
                // del cual leeremos los datos devueltos por la consulta.
                aReader = command.ExecuteReader();

                // Crear la estructura del DataTable reaultado de la consulta.
                DataTable estructura = aReader.GetSchemaTable();
                foreach (DataRow fila in estructura.Rows)
                {
                    resultado.Columns.Add(fila["ColumnName"].ToString(), System.Type.GetType(fila["DataType"].ToString()));
                }
                string nombreColumna;
                string tipoColumna;
                int posicion;
                // Leer los datos devueltos en la consulta y los pasamos al DataTable.
                if (aReader != null)
                {
                    DataRow dr = null;
                    while (aReader.Read())
                    {
                        dr = resultado.NewRow();
                        foreach (DataRow fila in estructura.Rows)
                        {
                            nombreColumna = fila["ColumnName"].ToString();
                            tipoColumna = fila["DataType"].ToString();
                            posicion = Convert.ToInt32(fila["ColumnOrdinal"], CultureInfo.CurrentCulture);
                            if (aReader.IsDBNull(posicion))
                            {
                                dr[nombreColumna] = DBNull.Value;
                            }
                            else
                            {
                                switch (tipoColumna)
                                {
                                    case "System.Int32":
                                        dr[nombreColumna] = aReader.GetInt32(posicion);
                                        break;
                                    case "System.String":
                                        dr[nombreColumna] = aReader.GetString(posicion);
                                        break;
                                    case "System.DateTime":
                                        dr[nombreColumna] = aReader.GetDateTime(posicion);
                                        break;
                                    case "System.Decimal":
                                        dr[nombreColumna] = aReader.GetDecimal(posicion);
                                        break;
                                    case "System.Boolean":
                                        dr[nombreColumna] = aReader.GetBoolean(posicion);
                                        break;
                                    case "System.Byte":
                                        dr[nombreColumna] = aReader.GetByte(posicion);
                                        break;
                                    case "System.Single":
                                        dr[nombreColumna] = aReader.GetFloat(posicion);
                                        break;
                                    case "System.Double":
                                        dr[nombreColumna] = aReader.GetDouble(posicion);
                                        break;
                                    case "System.Byte[]":
                                    default:
                                        break;
                                }
                            }
                        }
                        resultado.Rows.Add(dr);
                    }
                }
            }
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
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "SQL proviene de un archivo de recursos y parámetros.")]
        public int EjecutarSql(string sql)
        {
            // Inicializar resultado.
            int resultado = 0;
            using (OleDbConnection conexion = new OleDbConnection(this.CadenaConexion))
            using (OleDbCommand command = new OleDbCommand(sql, conexion))
            {
                // Abrir conexión.
                conexion.Open();
                // Ejecutar query.
                resultado = command.ExecuteNonQuery();
            }
            return resultado;
        }
        #endregion

        #region EjecutarSqlBatch
        /// <summary>
        /// Ejecutar una sentencia T-SQL con timeout por defecto.
        /// </summary>
        /// <param name="listaComandos">Lista de comandos.</param>
        /// <returns>Para las instrucciones Update, Insert y Delete, el 
        /// valor devuelto corresponde al número de filas afectadas por
        /// el comando. Para los demás tipos de instrucciones, el valor
        /// devuelto es -1.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities",
         Justification = "SQL proviene de un archivo de recursos y parámetros.")]
        public int EjecutarSqlBatch(ref List<ComandoSql> listaComandos)
        {
            // Inicializar resultado.
            int resultado = 0;
            using (OleDbConnection conexion = new OleDbConnection(this.CadenaConexion))
            {
                // Abrir conexión.
                conexion.Open();
                foreach (ComandoSql sql in listaComandos)
                {
                    using (OleDbCommand command = new OleDbCommand(sql.SentenciaSql, conexion))
                    {
                        if (sql.Parameters != null)
                        {
                            foreach (KeyValuePair<string, object> parameter in sql.Parameters)
                            {
                                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                            }
                        }
                        try
                        {
                            // Ejecutar query.
                            sql.Resultado = command.ExecuteNonQuery();
                            resultado++;
                        }
                        catch (OleDbException ex)
                        {
                            sql.Excepcion = ex;
                        }
                    }
                }
            }
            return resultado;
        }
        #endregion

        #endregion
    }
}
