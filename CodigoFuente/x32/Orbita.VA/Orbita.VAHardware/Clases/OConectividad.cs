//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 13-12-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using Orbita.VAComun;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Clase encargada de comprobar la conectividad con un dispositivo TCP/IP
    /// </summary>
    public class OConectividad
    {
        #region Propiedad(es)
        /// <summary>
        /// Código identificativo del dispositivo
        /// </summary>
        private string _Codigo;
        /// <summary>
        /// Código identificativo del dispositivo
        /// </summary>
        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }

        /// <summary>
        /// Habilita o deshabilita el proceso de comprobación de la conexión
        /// </summary>
        private bool _Habilitado;
        /// <summary>
        /// Habilita o deshabilita el proceso de comprobación de la conexión
        /// </summary>
        public bool Habilitado
        {
            get { return _Habilitado; }
            set 
            {
                if (this._Habilitado != value)
                {
                    this._Habilitado = value;
                    if (value)
                    {
                        this.Start();
                    }
                    else 
                    {
                        this.Stop();
                    }
                }
            }
        }
        #endregion

        #region Propiedad(es) virtual(es)
        /// <summary>
        /// Estado de la conexión
        /// </summary>
        protected EstadoConexion _EstadoConexion;
        /// <summary>
        /// Estado de la conexión
        /// </summary>
        public virtual EstadoConexion EstadoConexion
        {
            get { return _EstadoConexion; }
            set
            {
                if (this._EstadoConexion != value)
                {
                    EstadoConexion estadoConexionAnterior = this._EstadoConexion;

                    this._EstadoConexion = value;

                    // Guardamos el log
                    switch (value)
                    {
                        case EstadoConexion.Desconectado:
                        default:
                            OVALogsManager.Info(ModulosHardware.Camaras, "Conectividad" + this.Codigo, "Dispositivo desconectado");
                            break;
                        case EstadoConexion.Desconectando:
                            OVALogsManager.Info(ModulosHardware.Camaras, "Conectividad" + this.Codigo, "Dispositivo en proceso de desconexión");
                            break;
                        case EstadoConexion.Conectado:
                            OVALogsManager.Info(ModulosHardware.Camaras, "Conectividad" + this.Codigo, "Dispositivo conectado");
                            break;
                        case EstadoConexion.Conectando:
                            OVALogsManager.Info(ModulosHardware.Camaras, "Conectividad" + this.Codigo, "Dispositivo en proceso de conexión");
                            break;
                        case EstadoConexion.ErrorConexion:
                            OVALogsManager.Error(ModulosHardware.Camaras, "Conectividad" + this.Codigo, "Problema de conexión con el dispositivo.");
                            break;
                        case EstadoConexion.Reconectando:
                            OVALogsManager.Error(ModulosHardware.Camaras, "Conectividad" + this.Codigo, "Dispositivo en proceso de reconexión.");
                            break;
                        case EstadoConexion.Reconectado:
                            OVALogsManager.Error(ModulosHardware.Camaras, "Conectividad" + this.Codigo, "Dispositivo reconectado.");
                            break;
                    }

                    if (this.OnCambioEstadoConexion != null)
                    {
                        this.OnCambioEstadoConexion(this.Codigo, value, estadoConexionAnterior);
                    }
                }
            }
        }
        #endregion

        #region Declaracion(es) de evento(s)
        /// <summary>
        /// Delegado de error de conexión con la cámara
        /// </summary>
        /// <param name="estadoConexion"></param>
        public DelegadoCambioEstadoConexionCamaraAdv OnCambioEstadoConexion;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OConectividad(string codigo)
        {
            this.Codigo = codigo;
            this.EstadoConexion = EstadoConexion.Desconectado;
            this._Habilitado = false;
        }
        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Inicio de la comprobación
        /// </summary>
        public virtual void Start()
        {
            this._Habilitado = true;

            // Implementado en heredados
        }

        /// <summary>
        /// Finaliza de la comprobación
        /// </summary>
        public virtual void Stop()
        {
            this._Habilitado = false;

            // Implementado en heredados
        }

        /// <summary>
        /// Fuerza una consulta de verificación de la conectividad con el dispositivo TCP/IP
        /// </summary>
        public virtual bool ForzarVerificacionConectividad()
        {
            return true;

            // Implementado en heredados
        }
        #endregion
    }
}
