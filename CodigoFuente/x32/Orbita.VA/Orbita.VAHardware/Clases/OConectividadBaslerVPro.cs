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
//***********************************************************************using Orbita.VAComun;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Orbita.VAComun;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Clase encargada de comprobar la conectividad con un dispositivo TCP/IP
    /// </summary>
    public class OConectividadGigE: OConectividad
    {
        #region Atributo(s)
        /// <summary>
        /// Timer de comprobación del estado de la conexión
        /// </summary>
        private Timer TimerComprobacionConexion;
        /// <summary>
        /// Cronómetro del tiempo sin respuesta de la cámara
        /// </summary>
        private Stopwatch CronometroTiempoSinRespuestaCamara;
        /// <summary>
        /// Característica a consultar de la cámara
        /// </summary>
        private OGigEIntFeature FeatureGigeConexion;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Inervalo entre comprobaciones
        /// </summary>
        private TimeSpan _IntervaloComprabacion;
        /// <summary>
        /// Inervalo entre comprobaciones
        /// </summary>
        public TimeSpan IntervaloComprabacion
        {
            get { return _IntervaloComprabacion; }
            set { _IntervaloComprabacion = value; }
        }
        #endregion

        #region Propiedad(es) heredada(s)
        /// <summary>
        /// Estado de la conexión
        /// </summary>
        public override OEstadoConexion EstadoConexion
        {
            set
            {
                if (this._EstadoConexion != value)
                {
                    base.EstadoConexion = value;
                }

                switch (value)
                {
                    case OEstadoConexion.Desconectado:
                    default:
                        break;
                    case OEstadoConexion.Conectado:
                    case OEstadoConexion.ErrorConexion:
                        // Actualizo el tiempo sin respuesta de la cámara
                        this.CronometroTiempoSinRespuestaCamara.Stop();
                        this.CronometroTiempoSinRespuestaCamara.Reset();
                        this.CronometroTiempoSinRespuestaCamara.Start();
                        break;
                }
            }
        }
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OConectividadGigE(string codigo, OGigEIntFeature featureGigeConexion, int intervaloComprobacionMs) :
            base(codigo)
        {
            this._IntervaloComprabacion = TimeSpan.FromMilliseconds(intervaloComprobacionMs);
            this.FeatureGigeConexion = featureGigeConexion;

            // Creación del timer de comprobación de la conexión
            this.TimerComprobacionConexion = new Timer();
            this.TimerComprobacionConexion.Interval = intervaloComprobacionMs;
            this.TimerComprobacionConexion.Enabled = false;

            // Creación del cronómetro de tiempo de espera sin respuesta de la cámara
            this.CronometroTiempoSinRespuestaCamara = new Stopwatch();
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Inicio de la comprobación
        /// </summary>
        public override void Start()
        {
            base.Start();

            this.TimerComprobacionConexion.Tick += this.TimerComprobacionConexion_Tick;
            this.TimerComprobacionConexion.Start();
        }

        /// <summary>
        /// Finaliza de la comprobación
        /// </summary>
        public override void Stop()
        {
            base.Stop();

            this.TimerComprobacionConexion.Tick -= this.TimerComprobacionConexion_Tick;
            this.TimerComprobacionConexion.Stop();
        }

        /// <summary>
        /// Fuerza una consulta de verificación de la conectividad con el dispositivo TCP/IP
        /// </summary>
        public override bool ForzarVerificacionConectividad()
        {
            bool resultado = false;
            try
            {
                // Verificamos que la cámara está conectada
                resultado = this.FeatureGigeConexion.Receive();
            }
            finally
            {

                // Lanzamos el evento de error de conexión
                if (resultado)
                {
                    this.EstadoConexion = OEstadoConexion.Conectado;
                }
                else
                {
                    this.EstadoConexion = OEstadoConexion.ErrorConexion;
                }
            }

            return resultado;
        }
        #endregion

        #region Evento(s)
        /// <summary>
        /// Evento del timer de comprobación de la conexión
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerComprobacionConexion_Tick(object sender, EventArgs e)
        {
            this.TimerComprobacionConexion.Stop();
            try
            {
                // TimeOut de conectividad
                if (this.Habilitado && (this.CronometroTiempoSinRespuestaCamara.Elapsed > this.IntervaloComprabacion))
                {
                    this.ForzarVerificacionConectividad();
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.Camaras, "Conectividad " + this.Codigo, exception);
            }
            this.TimerComprobacionConexion.Start();
        }
        #endregion
    }
}
