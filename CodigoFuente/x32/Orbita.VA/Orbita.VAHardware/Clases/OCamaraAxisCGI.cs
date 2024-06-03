//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : sfenollosa
// Created          : 31-10-2012
//
// Last Modified By : aibañez
// Last Modified On : 26-11-2012
// Description      : Herencia de Camara IP
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using Orbita.VAComun;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Clase que implementa las funciones de manejo de la cámara IP
    /// </summary>
    public class OCamaraAxisCGI : OCamaraIP
    {
        #region Atributo(s)
        #endregion

        #region Propiedad(es) heredadas
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>       
        public OCamaraAxisCGI(string codigo)
            : base(codigo)
        {
            try
            {
            }
            catch (Exception exception)
            {
                OVALogsManager.Fatal(OModulosHardware.Camaras, this.Codigo, exception);
                throw new Exception("Imposible iniciar la cámara " + this.Codigo);
            }
        } 
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comienza una reproducción continua de la cámara
        /// </summary>
        /// <returns></returns>
        protected override bool StartInterno()
        {
            bool resultado = false;

            try
            {
                if (this.EstadoConexion == OEstadoConexion.Conectado)
                {
                    base.StartInterno();
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.Camaras, this.Codigo, exception);
            }

            return resultado;
        }

        /// <summary>
        /// Termina una reproducción continua de la cámara
        /// </summary>
        /// <returns></returns>
        protected override bool StopInterno()
        {
            bool resultado = false;

            try
            {
                if (this.EstadoConexion == OEstadoConexion.Conectado)
                {
                    base.StopInterno();
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.Camaras, this.Codigo, exception);
            }

            return resultado;
        }

        /// <summary>
        /// Realiza una fotografía de forma sincrona
        /// </summary>
        /// <returns></returns>
        protected override bool SnapInterno()
        {
            bool resultado = false;
            try
            {
                if (this.EstadoConexion == OEstadoConexion.Conectado)
                {
                    resultado = base.SnapInterno();
                }

                return resultado;
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.Camaras, this.Codigo, exception);
            }
            return resultado;
        }
        #endregion
    }
}
