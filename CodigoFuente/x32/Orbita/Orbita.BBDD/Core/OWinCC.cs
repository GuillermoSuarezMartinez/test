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
using System.Data.Common;
using System.Data.OleDb;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
namespace Orbita.BBDD
{
    /// <summary>
    /// Clase tipo para instanciar objetos de base de datos SQLServer,
    /// asignadas a WinCC. Acceso a través de Connectivity Pack.
    /// </summary>
    public class OWinCC : OBBDDCore
    {
        #region Enumerados
        /// <summary>
        /// Tipo de alarmas.
        /// El parámetro FlagsAttribute considera
        /// que el  enumerado  puede considerarse
        /// como un conjunto de enumerados.
        /// </summary>
        [FlagsAttribute]
        enum Alarmas
        {
            /// <summary>
            /// Indefinido.
            /// </summary>
            None = 0,
            /// <summary>
            /// Alarma activa.
            /// </summary>
            Activa = 1,
            /// <summary>
            /// Alarma inactiva.
            /// </summary>
            Inactiva = 2,
            /// <summary>
            /// Alarma acusada.
            /// </summary>
            Acusada = 3,
            /// <summary>
            /// Alarma reactivada.
            /// </summary>
            Reactivada = 16
        }
        /// <summary>
        /// Tipo agregado útil para interpolación.
        /// </summary>
        public enum AgregationType
        {
            /// <summary>
            /// Indefinido.
            /// </summary>
            None = 0,
            /// <summary>
            /// Sin interpolación primero.
            /// </summary>
            WithoutInterpolationFirst = 1,
            /// <summary>
            /// Sin interpolación último.
            /// </summary>
            WithoutInterpolationLast = 2,
            /// <summary>
            /// Sin interpolación mínimo.
            /// </summary>
            WithoutInterpolationMin = 3,
            /// <summary>
            /// Sin interpolación máximo.
            /// </summary>
            WithoutInterpolationMax = 4,
            /// <summary>
            /// Sin interpolación media.
            /// </summary>
            WithoutInterpolationAvg = 5,
            /// <summary>
            /// Sin interpolación suma.
            /// </summary>
            WithoutInterpolationSum = 6,
            /// <summary>
            /// Sin interpolación contador.
            /// </summary>
            WithoutInterpolationCount = 7,
            /// <summary>
            /// Con interpolación primero.
            /// </summary>
            WithInterpolationFirstInterpolated = 257,
            /// <summary>
            /// Con interpolación último.
            /// </summary>
            WithInterpolationLastInterpolated = 258,
            /// <summary>
            /// Con interpolación mínimo.
            /// </summary>
            WithInterpolationMinInterpolated = 259,
            /// <summary>
            /// Con interpolación máximo.
            /// </summary>
            WithInterpolationMaxInterpolated = 260,
            /// <summary>
            /// Con interpolación media.
            /// </summary>
            WithInterpolationAvgInterpolated = 261,
            /// <summary>
            /// Con interpolación suma.
            /// </summary>
            WithInterpolationSumInterpolated = 262,
            /// <summary>
            /// Con interpolación contador.
            /// </summary>
            WithInterpolationCountInterpolated = 263
        }
        #endregion

        #region Constantes privadas
        const string Fecha = "yyyy-MM-dd HH:mm:ss";
        #endregion

        #region Constructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase WinCC.
        /// </summary>
        /// <param name="infoConexion">Información de la conexión actual.</param>
        public OWinCC(OInfoConexion infoConexion)
            : base(infoConexion)
        {
            if (infoConexion != null)
            {
                // Obtenermos el nombre de la base de datos consultando la tabla master. Necesitamos usuario y password.
                OSqlServer osql = new OSqlServer(new OInfoConexion(infoConexion.Instancia, "master", infoConexion.Usuario, infoConexion.Password, 180));
                DataTable tablaAux = osql.SeleccionSql("SELECT NAME AS BDD FROM SYSDATABASES WHERE (NAME LIKE 'CC_%') AND (NAME LIKE '%R')");
                if (tablaAux != null || tablaAux.Rows.Count > 0)
                {
                    // Extramos el nombre de la base de datos que le pasaremos a la cadena de conexión.
                    this.InfoConexion.BaseDatos = tablaAux.Rows[0]["BDD"].ToString();
                }
                else
                {
                    throw new Exception("No se encuentra la base de datos de WinCC.");
                }
                this.CadenaConexion = string.Format(CultureInfo.CurrentCulture, @"Provider=WinCCOLEDBProvider.1; Catalog={0}; Data Source={1}",
                    infoConexion.BaseDatos, infoConexion.Instancia);
            }
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Tabla con los Tags de WinCC
        /// </summary>
        /// <returns></returns>
        public DataTable TablaTagLogging
        {
            get
            {
                // Creamos la conexión sql para la consulta.
                OSqlServer oSqlServer = new OSqlServer(new OInfoConexion(this.Instancia, this.BaseDatos, this.Usuario, this.InfoConexion.Password));

                string sql =
                    "SELECT	ARCHIVE.ValueID Id,  " +
                    "	LTRIM(RTRIM(ARCHIVE.ValueName)) Nombre,  " +
                    "	CONVERT(NVARCHAR(30),lastmodification,120) FechaModificacion, " +
                    "	TIMES.[TIMEBASE]*TAGS.[ARCFACTOR] TiempoAdquisicion, " +
                    "	LTRIM(RTRIM(TAGS.[PROCVARNAME])) VariableProceso, " +
                    "	LTRIM(RTRIM(TAGS.[ARCNAME])) Grupo, " +
                    "	VARIABLES.[ADDRESSPARAMETER] Direccion, " +
                    "	TIPO.[VARIABLETYPELENGTH] Tamanyo, " +
                    "	LTRIM(RTRIM(TIPO.[VARIABLETYPENAME])) Tipo " +
                    "FROM	[" + this.BaseDatos + "].[dbo].[Archive] ARCHIVE,  " +
                    "		[" + this.BaseDatos.Remove(this.BaseDatos.Length - 1) + "].[dbo].[PDE#TAGs] TAGS, " +
                    "		[" + this.BaseDatos.Remove(this.BaseDatos.Length - 1) + "].[dbo].[PDE#TIMES] TIMES, " +
                    "		[" + this.BaseDatos.Remove(this.BaseDatos.Length - 1) + "].[dbo].[MCPTVARIABLEDESC] VARIABLES, " +
                    "		[" + this.BaseDatos.Remove(this.BaseDatos.Length - 1) + "].[dbo].[MCPTVARIABLETYPE] TIPO " +
                    "WHERE 	ARCHIVE.[ValueID] = TAGS.[TLGTAGID] AND " +
                    "		TAGS.[SCANTIME] = TIMES.[TIMENAME] AND " +
                    "		TAGS.[PROCVARNAME] = VARIABLES.[VARIABLENAME] AND " +
                    "		VARIABLES.[VARIABLETYPEID] = TIPO.[VARIABLETYPEID]";
                // Devolvemos los datos.
                DataTable resultado = oSqlServer.SeleccionSql(sql);
                resultado.PrimaryKey = new DataColumn[] { resultado.Columns["Id"] };
                return resultado;
            }
        }
        #endregion

        #region Métodos públicos

        #region TagLogging
        /// <summary>
        /// Obtener registros de TagLogging.
        /// </summary>
        /// <param name="identificador">Identificador de Tagname
        /// en TagLogging.</param>
        /// <param name="fechaInicial">Fecha inicial.</param>
        /// <returns></returns>
        public DataTable LeerTagLogging(object identificador, DateTime fechaInicial)
        {
            return this.LeerTagLogging(string.Format(CultureInfo.CurrentCulture, @"TAG:R,('{0}'),'{1}','0000-00-00 00:00:00.000'",
                identificador, fechaInicial.ToUniversalTime().ToString(Fecha, CultureInfo.CurrentCulture)));
        }
        /// <summary>
        /// Obtener registros de TagLogging.
        /// </summary>
        /// <param name="identificador">Identificador de Tagname
        /// en TagLogging</param>
        /// <param name="fechaInicial">Fecha inicial.</param>
        /// <param name="tiempo">Tiempo de paso.</param>
        /// <param name="agregado">Tipo de agregado para interpolación.</param>
        /// <returns></returns>
        public DataTable LeerTagLogging(object identificador, DateTime fechaInicial, int tiempo, AgregationType agregado)
        {
            return this.LeerTagLogging(string.Format(CultureInfo.CurrentCulture, @"TAG:R,('{0}'),'{1}','0000-00-00 00:00:00.000','TIMESTEP={2},{3}'",
                identificador, fechaInicial.ToUniversalTime().ToString(Fecha, CultureInfo.CurrentCulture), tiempo, (int)agregado));
        }
        /// <summary>
        /// Recupera los datos de las variables que le pasamos como parámetro en un intervalo de tiempo.
        /// </summary>
        /// <param name="variables">Array con los ID's de las variables</param>
        /// <param name="fechaInicial">Fecha de inicio</param>
        /// <param name="fechaFinal">Fecha de fin</param>
        /// <returns>Cada tabla es una variable. El nombre de la tabla es el ID de la variable</returns>
        public DataSet LeerTagLogging(ArrayList variables, DateTime fechaInicial, DateTime fechaFinal)
        {
            // El agregado no se usa en esta funcion.
            return LeerTagLogging(variables, fechaInicial, fechaFinal, -1, AgregationType.None);
        }
        /// <summary>
        /// Recupera los datos de las variables que le pasamos como parámetro en un intervalo de tiempo. Agregamos por tiempo.
        /// </summary>
        /// <param name="variables">Array con los ID's de las variables</param>
        /// <param name="fechaInicial">Fecha de inicio</param>
        /// <param name="fechaFinal">Fecha de fin</param>
        /// <param name="timeStep">Intervalo de captura</param>
        /// <param name="agregacion">Tipo de agregado</param>
        /// <returns>Cada tabla es una variable. El nombre de la tabla es el ID de la variable</returns>
        public DataSet LeerTagLogging(ArrayList variables, DateTime fechaInicial, DateTime fechaFinal, int timeStep, AgregationType agregacion)
        {
            // DataTable temporal de todos los datos.
            DataTable dt = new DataTable();
            // DataSet de retorno.
            DataSet ds = new DataSet();
            // Variable auxiliar para el array de variables.
            string auxStr = string.Empty;
            // Variable auxiliar donde almacenamos el string de la SQL.
            string sql = string.Empty;

            // Extraer las variables del array.
            foreach (string varID in variables)
            {
                auxStr = auxStr + "'" + varID + "';";
            }
            auxStr = auxStr.Substring(0, auxStr.Length - 1);

            // Creamos la consulta SQL.
            sql = @"TAG:R,(" + auxStr + "),'" + fechaInicial.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture) + "','" + fechaFinal.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture) + "'";
            if (timeStep != -1) sql = sql + ", 'TIMESTEP=" + timeStep + "," + (int)agregacion + "'";

            using (OleDbConnection conexion = new OleDbConnection(this.CadenaConexion))
            using (OleDbDataAdapter oleDataAdapter = new OleDbDataAdapter(sql, conexion))
            {
                // Abrimos la conexión.
                conexion.Open();
                // Usamos el DataAdapter.
                oleDataAdapter.Fill(dt);
                // Cerramos la conexión.
                conexion.Close();
                // Conformamos el DataSet de vuelta.
                foreach (string varID in variables)
                {
                    ds.Tables.Add(varID);
                    ds.Tables[varID].Columns.Add("ValueID", Type.GetType("System.Int32"));
                    ds.Tables[varID].Columns.Add("Timestamp", Type.GetType("System.DateTime"));
                    ds.Tables[varID].Columns.Add("RealValue", Type.GetType("System.Decimal"));
                    ds.Tables[varID].Columns.Add("Quality", Type.GetType("System.Int32"));
                    ds.Tables[varID].Columns.Add("Flags", Type.GetType("System.Int32"));
                    DataRow[] drs = dt.Select("ValueID = " + varID);
                    foreach (DataRow dr in drs)
                    {
                        try
                        {
                            object[] nuevaFila = { dr["ValueID"], ((DateTime)dr["Timestamp"]).ToLocalTime(), dr["RealValue"], dr["Quality"], dr["Flags"] };
                            ds.Tables[varID].Rows.Add(nuevaFila);
                        }
                        catch (Exception)  { /* No insertamos ningun valor si es erróneo. */ }
                    }
                }
            }
            return ds;
        }
        #endregion

        #region AlarmLogging
        /// <summary>
        /// Obtener registros de AlarmLogging.
        /// </summary>
        /// <param name="identificador">Identificador de tagname.</param>
        /// <param name="fechaInicial">Fecha inicial.</param>
        /// <returns></returns>
        public DataTable LeerAlarmLogging(object identificador, DateTime fechaInicial)
        {
            return this.LeerAlarmlogging(string.Format(CultureInfo.CurrentCulture, "ALARMVIEW:SELECT * FROM ALGVIEWENU WHERE MsgNr={0} AND DateTime>'{1}'",
                identificador, fechaInicial.ToUniversalTime().ToString(Fecha, CultureInfo.CurrentCulture)));
        }
        #endregion

        #endregion

        #region Métodos privados

        #region TagLogging
        /// <summary>
        /// Obtener registros de TagLogging.
        /// </summary>
        /// <param name="sql">Consulta SQL.</param>
        /// <returns>Registros de resultados.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "SQL proviene de un archivo de recursos y parámetros.")]
        DataTable LeerTagLogging(string sql)
        {
            // Inicializar resultado.
            DataTable resultado = null;
            try
            {
                using (OleDbConnection conexion = new OleDbConnection(this.CadenaConexion))
                using (OleDbCommand command = new OleDbCommand(sql, conexion))
                {
                    // Abrir conexión.
                    conexion.Open();
                    // Establecer timeout de ejecución en segundos.
                    command.CommandTimeout = this.Timeout;
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            if (reader.HasRows)
                            {
                                resultado = LeerTagLogging();
                                foreach (DbDataRecord rec in reader)
                                {
                                    // Añadir columnas al DataTable cuya calidad sea 0,
                                    // descartar el resto de registros que no cumplan 
                                    // este criterio.
                                    if (rec.GetValue(3).Equals(0))
                                    {
                                        resultado.Rows.Add(rec.GetValue(1), rec.GetValue(2));
                                    }
                                }
                            }
                        }
                    }
                }
                return resultado;
            }
            finally { if (resultado != null) { resultado.Dispose(); } }
        }
        #endregion

        #region AlarmLogging
        /// <summary>
        /// Obtener registros de AlarmLogging.
        /// </summary>
        /// <param name="sql">Consulta SQL.</param>
        /// <returns>Registros de resultados.</returns>
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "SQL proviene de un archivo de recursos y parámetros.")]
        DataTable LeerAlarmlogging(string sql)
        {
            // Inicializar resultado.
            DataTable resultado = null;
            try
            {
                using (OleDbConnection conexion = new OleDbConnection(this.CadenaConexion))
                using (OleDbCommand command = new OleDbCommand(sql, conexion))
                {
                    // Abrir conexión.
                    conexion.Open();
                    // Establecer timeout de ejecución en segundos.
                    command.CommandTimeout = this.Timeout;
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            if (reader.HasRows)
                            {
                                resultado = LeerAlarmLogging();
                                foreach (DbDataRecord rec in reader)
                                {
                                    // Añadir  columnas  al DataTable cuyo valor este
                                    // definido en el enumerado de Alarmas, descartar
                                    // el resto de registros que no cumplan este criterio.
                                    if (Enum.IsDefined(typeof(Alarmas), rec.GetValue(1)))
                                    {
                                        resultado.Rows.Add(rec.GetValue(2), rec.GetValue(1));
                                    }
                                }
                            }
                        }
                    }
                }
                return resultado;
            }
            finally { if (resultado != null) { resultado.Dispose(); } }
        }
        #endregion

        #region Métodos estáticos

        #region TagLogging
        /// <summary>
        /// Crear las columnas TagLogging.
        /// </summary>
        static DataTable LeerTagLogging()
        {
            DataTable dt = new DataTable();
            try
            {
                // Establecer una configuración regional invariable.
                dt.Locale = CultureInfo.CurrentCulture;
                using (DataColumn dc1 = new DataColumn(), dc2 = new DataColumn())
                {
                    dc1.DataType = Type.GetType("System.String");
                    dc1.ColumnName = "Fecha";
                    dt.Columns.Add(dc1);
                    dc2.DataType = Type.GetType("System.String");
                    dc2.ColumnName = "Valor";
                    dt.Columns.Add(dc2);
                }
                return dt;
            }
            finally { if (dt != null) { dt.Dispose(); } }
        }
        #endregion

        #region AlarmLogging
        /// <summary>
        /// Crear las columnas AlarmLogging.
        /// </summary>
        static DataTable LeerAlarmLogging()
        {
            DataTable dt = new DataTable();
            try
            {
                // Establecer una configuración regional invariable.
                dt.Locale = CultureInfo.CurrentCulture;
                using (DataColumn dc1 = new DataColumn(), dc2 = new DataColumn())
                {
                    dc1.DataType = Type.GetType("System.String");
                    dc1.ColumnName = "Fecha";
                    dt.Columns.Add(dc1);
                    dc2.DataType = Type.GetType("System.String");
                    dc2.ColumnName = "Valor";
                    dt.Columns.Add(dc2);
                }
                return dt;
            }
            finally { if (dt != null) { dt.Dispose(); } }
        }
        #endregion

        #endregion

        #endregion
    }
}
