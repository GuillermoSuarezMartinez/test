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
        /// Carga los valores de la cámara
        /// </summary>
        public override void Inicializar()
        {
            base.Inicializar();
        }

        /// <summary>
        /// Finaliza la cámara
        /// </summary>
        public override void Finalizar()
        {          
            base.Finalizar();
        }

        /// <summary>
        /// Se toma el control de la cámara
        /// </summary>
        /// <returns>Verdadero si la operación ha funcionado correctamente</returns>
        protected override bool Conectar(bool reconexion)
        {
            bool resultado = base.Conectar(reconexion);
            return resultado;
        }

        /// <summary>
        /// Se deja el control de la cámara
        /// </summary>
        /// <returns>Verdadero si la operación ha funcionado correctamente</returns>
        protected override bool Desconectar(bool errorConexion)
        {
            bool resultado = false;
            return resultado;
        }

        /// <summary>
        /// Comienza una reproducción continua de la cámara
        /// </summary>
        /// <returns></returns>
        protected override bool InternalStart()
        {
            bool resultado = false;

            try
            {
                if (this.EstadoConexion == OEstadoConexion.Conectado)
                {
                    base.InternalStart();
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
        protected override bool InternalStop()
        {
            bool resultado = false;

            try
            {
                if (this.EstadoConexion == OEstadoConexion.Conectado)
                {
                    base.InternalStop();
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
        protected override bool InternalSnap()
        {
            bool resultado = false;
            try
            {
                if (this.EstadoConexion == OEstadoConexion.Conectado)
                {
                    resultado = base.InternalSnap();
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
