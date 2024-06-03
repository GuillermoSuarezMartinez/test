//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Orbita.VAComun;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Clase estática que contiene llamadas a los procedimiento almacenados en la base de datos
    /// </summary>
    public static class AppBD
    {
        #region TAB Select: funciones Get -> GetX(...)
        /// <summary>
        /// Consulta las cámaras del sistema
        /// </summary>
        /// <returns>DataTable con los códigos de las cámaras existentes en el sistema</returns>
        public static DataTable GetCamaras()
        {
            return OBaseDatosParam.SQLServer.SeleccionProcedimientoAlmacenado("CAM_GET_CAMARAS");
        }
        /// <summary>
        /// Consulta la información de una determinada cámara
        /// </summary>
        /// <returns>DataTable con toda la información de una determinada cámara</returns>
        public static DataTable GetCamara(string codCamara)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@CodCamara", codCamara));

            return OBaseDatosParam.SQLServer.SeleccionProcedimientoAlmacenado("CAM_GET_CAMARA", list);
        }

        /// <summary>
        /// Consulta las tarjetas IO del sistema
        /// </summary>
        /// <returns>DataTable con los códigos de las tarjetas IO existentes en el sistema</returns>
        public static DataTable GetTarjetasIO()
        {
            return OBaseDatosParam.SQLServer.SeleccionProcedimientoAlmacenado("IO_GET_TARJETAS");
        }
        /// <summary>
        /// Consulta los terminales IO de una determinada tarjeta IO
        /// </summary>
        /// <returns>DataTable con los terminales IO  de una determinada tarjeta IO</returns>
        public static DataTable GetTerminalesIO(string codTarjetaIO)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@CodTarjetaIO", codTarjetaIO));

            return OBaseDatosParam.SQLServer.SeleccionProcedimientoAlmacenado("IO_GET_TERMINALES", list);
        }
        /// <summary>
        /// Consulta la información de una determinada tarjeta IO
        /// </summary>
        /// <returns>DataTable con toda la información de una determinada tarjeta IO</returns>
        public static DataTable GetTarjetaIO(string codTarjetaIO)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@CodTarjetaIO", codTarjetaIO));

            return OBaseDatosParam.SQLServer.SeleccionProcedimientoAlmacenado("IO_GET_TARJETA", list);
        }

        /// <summary>
        /// Consulta la información de un determinado terminal de una tarjeta IO
        /// </summary>
        /// <returns>DataTable con toda la información de un determinado terminal de una tarjeta IO</returns>
        public static DataTable GetTerminalIO(string codTarjetaIO, string codTerminalIO)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@CodTarjetaIO", codTarjetaIO));
            list.Add(new SqlParameter("@CodTerminalIO", codTerminalIO));

            return OBaseDatosParam.SQLServer.SeleccionProcedimientoAlmacenado("IO_GET_TERMINAL", list);
        }

        /// <summary>
        /// Consulta el alias de una vista de hardware
        /// </summary>
        /// <param name="codVista"></param>
        /// <returns></returns>
        public static DataTable GetAliasVistaHardware(string codVista)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@CodVista", codVista));

            return OBaseDatosParam.SQLServer.SeleccionProcedimientoAlmacenado("HWR_GET_ALIAS_VISTA", list);
        }

        /// <summary>
        /// Consulta la información de todos los terminales que están vinculados a cierto terminal de escritura
        /// </summary>
        /// <returns>DataTable con los terminales IO  de una determinada tarjeta IO</returns>
        public static DataTable GetTerminalesIO_EscrituraSCED(string codTarjetaIO, string codTerminalIOEscritura)
        {
            ArrayList list = new ArrayList();
            list.Add(new SqlParameter("@CodTarjetaIO", codTarjetaIO));
            list.Add(new SqlParameter("@CodTerminalIO", codTerminalIOEscritura));

            return OBaseDatosParam.SQLServer.SeleccionProcedimientoAlmacenado("IO_GET_SCED_TERMINALES_ESCRITURA", list);
        }
        #endregion

        #region TAB Insert: funciones Add -> AddX(...)

        #endregion

        #region TAB Update: funciones Modify -> MdfX(...)

        #endregion

        #region TAB Delete: funciones Delete -> DelX(...)

        #endregion
    }
}

