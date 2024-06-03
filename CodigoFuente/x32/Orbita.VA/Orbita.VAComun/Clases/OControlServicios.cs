//***********************************************************************
// Assembly         : Orbita.VAComun
// Author           : aibañez
// Created          : 13-12-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceProcess;

namespace Orbita.VAComun
{
    /// <summary>
    /// Clase utilizada para la instalación/desinstalación de servicios y su inicio y detención
    /// </summary>
    public class OControlServicios
    {
        #region Atributo(s)
        /// <summary>
        /// Nombre del servicio
        /// </summary>
        string Nombre;
        /// <summary>
        /// Ruta del ejecutable
        /// </summary>
        string Ruta;
        /// <summary>
        /// Controlador del servicio instalado
        /// </summary>
        private ServiceController Controlador;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Informa si existe algún servicio con el nombre especificado, en caso contrario se supone que el servicio no está instalado
        /// </summary>
        public bool Instalado
        {
            get
            {
                return this.IsInstalled();
            }
            set
            {
                this.Instalar(value);
            }
        }

        /// <summary>
        /// Estado del servicio
        /// </summary>
        public ServiceControllerStatus Estado
        {
            get { return this.Controlador.Status; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="nombre">Nombre del servicio</param>
        public OControlServicios(string nombre, string ruta)
        {
            this.Nombre = nombre;
            this.Ruta = ruta;

            // Se averigua si está instalado
            this.Controlador = null;
            ServiceController[] listaServicios = ServiceController.GetServices();
            foreach (ServiceController servicio in listaServicios)
            {
                if ((servicio.ServiceName == "OrbitaJobService") && (servicio.MachineName == "."))
                {
                    this.Controlador = servicio;
                }
            }

            this.Controlador = new ServiceController(this.Nombre);
        }
        #endregion

        #region Método(s) público
        /// <summary>
        /// Instala el servicio
        /// </summary>
        /// <returns>Verdadero si el proceso ha finalizado con éxito</returns>
        public bool Instalar()
        {
            return this.Instalar(true);
        }
        /// <summary>
        /// Desinstala el servicio
        /// </summary>
        /// <returns>Verdadero si el proceso ha finalizado con éxito</returns>
        public bool Desinstalar()
        {
            return this.Instalar(false);
        }
        /// <summary>
        /// Inicia el servicio
        /// </summary>
        /// <returns>Verdadero si se ha iniciado con éxtio</returns>
        public bool Iniciar()
        {
            if (this.Controlador is ServiceController)
            {
                if ((this.Controlador.Status == ServiceControllerStatus.Stopped) || (this.Controlador.Status == ServiceControllerStatus.Paused))
                {
                    this.Controlador.Start();
                    this.Controlador.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMilliseconds(10000));
                }

            }
            return this.Controlador.Status == ServiceControllerStatus.Running;
        }
        /// <summary>
        /// Inicia el servicio
        /// </summary>
        /// <returns>Verdadero si se ha iniciado con éxtio</returns>
        public bool Detener()
        {
            if (this.Controlador is ServiceController)
            {
                if ((this.Controlador.Status == ServiceControllerStatus.Running) || (this.Controlador.Status == ServiceControllerStatus.Paused))
                {
                    this.Controlador.Stop();
                    this.Controlador.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds(10000));
                }
            }
            return this.Controlador.Status == ServiceControllerStatus.Stopped;
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Instala o desinstala el servicio
        /// </summary>
        /// <returns>Verdadero si el proceso ha finalizado con éxito</returns>
        private bool Instalar(bool valor)
        {
            Process proceso = new Process();
            string netFolder = RuntimeEnvironment.GetRuntimeDirectory();
            string rutaInstalador = Path.Combine(netFolder, "installutil.exe");
            if (File.Exists(rutaInstalador))
            {
                proceso.StartInfo.FileName = Path.Combine(netFolder, "installutil.exe");
                proceso.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                if (valor)
                {
                    proceso.StartInfo.Arguments = this.Ruta;
                }
                else
                {
                    proceso.StartInfo.Arguments = "-u " + this.Ruta;
                }
                proceso.Start();
                proceso.WaitForExit();
                return this.IsInstalled();
            }
            return false;
        }

        private bool IsInstalled()
        {
            this.Controlador = null;
            ServiceController[] listaServicios = ServiceController.GetServices();
            foreach (ServiceController servicio in listaServicios)
            {
                if ((servicio.ServiceName == this.Nombre) && (servicio.MachineName == "."))
                {
                    this.Controlador = servicio;
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
