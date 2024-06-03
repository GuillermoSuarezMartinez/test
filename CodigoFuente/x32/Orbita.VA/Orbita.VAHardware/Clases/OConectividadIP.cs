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
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using Orbita.VAComun;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Clase encargada de comprobar la conectividad con un dispositivo TCP/IP
    /// </summary>
    public class OConectividadIP: OConectividad
    {
        #region Atributo(s)
        /// <summary>
        /// Timer de comprobación del estado de la conexión
        /// </summary>
        private Timer TimerComprobacionConexion;
        /// <summary>
        /// Ping utilizado para detectar la conectividad con la cámara
        /// </summary>
        private Ping Ping;
        /// <summary>
        /// Cronómetro del tiempo sin respuesta de la cámara
        /// </summary>
        private Stopwatch CronometroTiempoSinRespuestaCamara;
        /// <summary>
        /// Indica si hay activo un envío de ping
        /// </summary>
        private bool PingEnProceso;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Dirección IP de la cámara
        /// </summary>
        private IPAddress _IP;
        /// <summary>
        /// Dirección IP de la cámara
        /// </summary>
        public IPAddress IP
        {
            get { return _IP; }
            set { _IP = value; }
        }

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
            get { return base.EstadoConexion; }
            set
            {
                if (this._EstadoConexion != value)
                {
                    base.EstadoConexion = value;
                }

                switch (value)
                {
                    case OEstadoConexion.Reconectado:
                    case OEstadoConexion.Conectado:
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
        public OConectividadIP(IPAddress ip, int intervaloComprobacionMs) :
            base(ip.ToString())
        {
            this._IP = ip;
            this._IntervaloComprabacion = TimeSpan.FromMilliseconds(intervaloComprobacionMs);

            // Creación del timer de comprobación de la conexión
            this.TimerComprobacionConexion = new Timer();
            this.TimerComprobacionConexion.Interval = intervaloComprobacionMs;
            this.TimerComprobacionConexion.Enabled = false;

            // Creación del ping para detectar la conectividad con la cámara
            this.Ping = new Ping();

            // Creación del cronómetro de tiempo de espera sin respuesta de la cámara
            this.CronometroTiempoSinRespuestaCamara = new Stopwatch();

            // No hay ningun ping en proceso actualmente
            this.PingEnProceso = false;
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
            this.Ping.PingCompleted += this.PingCompletedEventHandler;
            this.TimerComprobacionConexion.Start();
        }

        /// <summary>
        /// Finaliza de la comprobación
        /// </summary>
        public override void Stop()
        {
            base.Stop();

            this.TimerComprobacionConexion.Tick -= this.TimerComprobacionConexion_Tick;
            this.Ping.PingCompleted -= this.PingCompletedEventHandler;
            this.TimerComprobacionConexion.Stop();
            this.Ping.SendAsyncCancel();
        }

        /// <summary>
        /// Fuerza una consulta de verificación de la conectividad con el dispositivo TCP/IP
        /// </summary>
        public override bool ForzarVerificacionConectividad()
        {
            // Verificamos que la cámara está conectada
            bool resultado = this.Ping.Send(this.IP).Status == IPStatus.Success;

            // Lanzamos el evento de error de conexión
            if (!resultado)
            {
                this.EstadoConexion = OEstadoConexion.ErrorConexion;
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
                if (this.Habilitado && !this.PingEnProceso && (this.CronometroTiempoSinRespuestaCamara.Elapsed > this.IntervaloComprabacion))
                {
                    this.PingEnProceso = true;
                    this.Ping.SendAsync(this.IP, (int)this.IntervaloComprabacion.TotalMilliseconds, new object());
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.Camaras, "Conectividad " + this.IP.ToString(), exception);
            }
            this.TimerComprobacionConexion.Start();
        }

        /// <summary>
        /// Evento de ping completado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PingCompletedEventHandler(object sender, PingCompletedEventArgs e)
        {
            try
            {
                if (this.Habilitado)
                {
                    if (e.Reply.Status == IPStatus.Success)
                    {
                        if (this.EstadoConexion == OEstadoConexion.Reconectando)
                        {
                            this.EstadoConexion = OEstadoConexion.Reconectado;
                        }
                        else
                        {
                            this.EstadoConexion = OEstadoConexion.Conectado;
                        }
                    }
                    else
                    {
                        if (this.EstadoConexion == OEstadoConexion.Conectado)
                        {
                            this.EstadoConexion = OEstadoConexion.ErrorConexion;
                        }
                    }

                    this.PingEnProceso = false;
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.Camaras, "Conectividad " + this.IP.ToString(), exception);
            }
        }
        #endregion
    }
}
