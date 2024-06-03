//***********************************************************************
// Assembly         : Orbita.VAComun
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 27-09-2012
// Description      : Añadidas funciones de almacenamiento
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Orbita.VAComun
{
    /// <summary>
    /// Clase estática que contiene llamadas a los procedimiento almacenados en la base de datos
    /// </summary>
    public static class AppBD
    {
        #region Constante(s)
        /// <summary>
        /// Ruta por defecto del fichero xml de configuración
        /// </summary>
        public static string DataFile = Path.Combine(ORutaParametrizable.AppFolder, "Configuracion_Datos.xml");
        #endregion

        #region Conversión SQLServer a XML
        /// <summary>
        /// Exporta la base de datos SQL a ficheros XML
        /// </summary>
        public static void CreateXMLDataBase(Dictionary<string, string> tablas)
        {
            DataSet ds = new DataSet();
            foreach (KeyValuePair<string, string> tabla in tablas)
            {
                ArrayList list = new ArrayList();
                list.Add(new SqlParameter("@TextoSQL", tabla.Value));
                DataTable dt = OBaseDatosParam.SQLServer.SeleccionProcedimientoAlmacenado("APP_GET_SELECT_A_MEDIDA", list);
                dt.TableName = tabla.Key;
                ds.Tables.Add(dt);
            }

            ds.WriteXml(DataFile, XmlWriteMode.WriteSchema);
        }
        #endregion

        #region TAB Select: funciones Get -> GetX(...)

        /// <summary>
        /// Consulta los parámetros básicos de la aplicación
        /// </summary>
        /// <returns>DataTable con los parámetros básicos de la aplicación</returns>
        public static DataTable GetParametrosAplicacion()
        {
            return OBaseDatosParam.SQLServer.SeleccionProcedimientoAlmacenado("APP_GET_PARAMETROS");
        }

        /// <summary>
        /// Consulta todas vistas existentes en el sistema
        /// </summary>
        /// <returns>DataTable con los códigos de las variables existentes en el sistema</returns>
        public static DataTable GetVistas()
        {
            return OBaseDatosParam.SQLServer.SeleccionProcedimientoAlmacenado("VST_GET_VISTAS");
        }
        /// <summary>
        /// Consulta una vista existente en el sistema
        /// </summary>
        /// <returns>DataTable con los códigos de las vistas existentes en el sistema</returns>
        public static DataTable GetVista(string codVista)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@CodVista", codVista));

            return OBaseDatosParam.SQLServer.SeleccionProcedimientoAlmacenado("VST_GET_VISTA", list);
        }

        /// <summary>
        /// Consulta un usuario existente en el sistema
        /// </summary>
        /// <returns>DataTable con los la información del usuario</returns>
        public static DataTable GetUsuarios()
        {
            return OBaseDatosParam.SQLServer.SeleccionProcedimientoAlmacenado("USU_GET_USUARIOS");
        }
        /// <summary>
        /// Consulta un usuario existente en el sistema
        /// </summary>
        /// <returns>DataTable con los la información del usuario</returns>
        public static DataTable GetUsuario(string codigo)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@CodUsuario", codigo));

            return OBaseDatosParam.SQLServer.SeleccionProcedimientoAlmacenado("USU_GET_USUARIO", list);
        }
        #endregion

        #region TAB Insert: funciones Add -> AddX(...)
        /// <summary>
        /// Añade el historico de la inspección de la pieza
        /// </summary>
        /// <returns></returns>        
        public static int AddHistoricoSerie(bool resultado, DateTime fecha, bool inspeccionado, string excepcion, bool rechazado, TimeSpan tiempoProceso, int IdTipoHistorico, OCXML xMLDetalle, OCXML xMLClaves)
        {
            ArrayList list = new ArrayList();

            list.Add(new SqlParameter("@Resultado", resultado));
            list.Add(new SqlParameter("@Fecha", fecha));
            list.Add(new SqlParameter("@Inspeccionado", inspeccionado));
            list.Add(new SqlParameter("@Excepcion", excepcion));
            list.Add(new SqlParameter("@Rechazado", rechazado));
            list.Add(new SqlParameter("@TiempoProceso", (int)Math.Round(tiempoProceso.TotalMilliseconds)));
            list.Add(new SqlParameter("@IdTipoHistorico", IdTipoHistorico));

            list.Add(new SqlParameter("@DETALLE_HISTORICO_ADD", (xMLDetalle == null) ? null : xMLDetalle.SW_XML.ToString()));
            list.Add(new SqlParameter("@CLAVES_HISTORICO_ADD", (xMLClaves == null) ? null : xMLClaves.SW_XML.ToString()));

            return OBaseDatosAlmacen.SQLServer.EjecutarProcedimientoAlmacenado("HIS_ADD_HISTORICO_SERIE_XML", list);
        }

        /// <summary>
        /// Añade el historico de la inspección de la pieza
        /// </summary>
        /// <returns></returns>        
        public static int AddHistoricoSubInspeccionSerie(int idHistorico, bool resultado, bool inspeccionado, string excepcion, TimeSpan tiempoProceso, OCXML xMLDetalle)
        {
            ArrayList list = new ArrayList();

            list.Add(new SqlParameter("@IdHistorico", idHistorico));
            list.Add(new SqlParameter("@Resultado", resultado));
            list.Add(new SqlParameter("@Inspeccionado", inspeccionado));
            list.Add(new SqlParameter("@Excepcion", excepcion));
            list.Add(new SqlParameter("@TiempoProceso", (int)Math.Round(tiempoProceso.TotalMilliseconds)));

            list.Add(new SqlParameter("@DETALLE_HISTORICO_ADD", (xMLDetalle == null) ? null : xMLDetalle.SW_XML.ToString()));

            return OBaseDatosAlmacen.SQLServer.EjecutarProcedimientoAlmacenado("HIS_ADD_HISTORICO_SUBINSPECCION_SERIE_XML", list);
        }

        /// <summary>
        /// Añade el historico de la inspección de la pieza
        /// </summary>
        /// <returns></returns>        
        public static int AddHistoricoParalelo(int IdTipoHistorico, OCXML xMLDetalle, OCXML xMLClaves, OCXML xMLSubDetalle)
        {
            ArrayList list = new ArrayList();

            list.Add(new SqlParameter("@IdTipoHistorico", IdTipoHistorico));
            list.Add(new SqlParameter("@DETALLE_HISTORICO_ADD", (xMLDetalle == null) ? null : xMLDetalle.SW_XML.ToString()));
            list.Add(new SqlParameter("@CLAVES_HISTORICO_ADD", (xMLClaves == null) ? null : xMLClaves.SW_XML.ToString()));
            list.Add(new SqlParameter("@SUB_DETALLE_HISTORICO_ADD", (xMLSubDetalle == null) ? null : xMLSubDetalle.SW_XML.ToString()));

            return OBaseDatosAlmacen.SQLServer.EjecutarProcedimientoAlmacenado("HIS_ADD_HISTORICO_PARALELO_XML", list);
        }
        #endregion

        #region TAB Update: funciones Modify -> MdfX(...)
        #endregion

        #region TAB Delete: funciones Delete -> DelX(...)
        #endregion
    }
}
