//***********************************************************************
// Assembly         : Orbita.VAComun
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 16-11-2012
// Description      : Borrada la implementación de InicioSistema y ParoSistema
//                    Declarados como virtuales los métodos InicioSistema y ParoSistema
//                     Es necesario heredarlos e implemenarlos en las clases hijas para dotarlos de funcionalidad
//                    Se ha extraido el atributo OpcionesEscritorio del fichero de configuración
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using Orbita.Utiles;

namespace Orbita.VAComun
{
    /// <summary>
    /// Clase estática encargada de dar acceso a toda la aplicación al control del inicio y la detención de los módulos instalados en el sistema
    /// </summary>
    public static class OSistemaManager
    {
        #region Atributo(s)
        /// <summary>
        /// Campo del sistema que se ha arrancado
        /// </summary>
        public static OSistema Sistema;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Enumerado que describe el estado del sistema
        /// </summary>
        public static EstadoSistema EstadoSistema
        {
            get { return Sistema.EstadoSistema; }
            set { Sistema.EstadoSistema = value; }
        }

        /// <summary>
        /// Parámetros de la aplicación
        /// </summary>
        public static ConfiguracionSistema Configuracion
        {
            get { return Sistema.Configuracion; }
            set { Sistema.Configuracion = value; }
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Constructor de los campos estáticos de la clase
        /// </summary>
        public static void Constructor(OSistema sistema)
        {
            Sistema = sistema;
        }
        /// <summary>
        /// Inicia el sistema de inspección en tiempo real
        /// </summary>
        public static bool IniciarSistema()
        {
            return Sistema.IniciarSistema(true);
        }
        /// <summary>
        /// Detiene el funcionamiento del inspección en tiempo real
        /// </summary>
        public static bool PararSistema()
        {
            return Sistema.PararSistema();
        }
        /// <summary>
        /// Reinicia el sistema de control en tiempo real
        /// </summary>
        public static bool ReiniciarSistema()
        {

            bool resultado = true;

            if (resultado)
            {
                resultado = Sistema.PararSistema();
            }

            if (resultado)
            {
                resultado = Sistema.IniciarSistema(false);
            }

            return resultado;
        }
        #endregion
    }

    /// <summary>
    /// Clase que controla el inicio y la detención del resto de módulos instalados en el sistema
    /// </summary>
    public class OSistema
    {
        #region Atributo(s)
        /// <summary>
        /// Enumerado que describe el estado del sistema
        /// </summary>
        public EstadoSistema EstadoSistema;

        /// <summary>
        /// Parámetros de la aplicación
        /// </summary>
        public ConfiguracionSistema Configuracion;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OSistema()
            : this(ConfiguracionSistema.ConfFile)
        {
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OSistema(string configFile)
        {
            this.EstadoSistema = EstadoSistema.Detenido;

            try
            {
                this.Configuracion = (ConfiguracionSistema)(new ConfiguracionSistema(configFile).CargarDatos());
            }
            catch (FileNotFoundException exception)
            {
                this.Configuracion = new ConfiguracionSistema();
                this.Configuracion.Guardar();
            }
        }

        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Conslta de un parámetro de la aplicación
        /// </summary>
        /// <returns></returns>
        public bool Parametro(string textoParametro)
        {
            bool resultado = false;

            if (App.ListaParametrosEntradaAplicacion != null)
            {
                List<string> listaParametrosEntradaAplicacion = new List<string>(App.ListaParametrosEntradaAplicacion);

                resultado = listaParametrosEntradaAplicacion.Exists(delegate(string p) { return string.Equals(p, textoParametro, StringComparison.InvariantCultureIgnoreCase); });
            }

            return resultado;
        }
        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Inicia el sistema de inspección en tiempo real
        /// </summary>
        public virtual bool IniciarSistema(bool incial)
        {
            // A implementar en heredados
            return false;
        }
        /// <summary>
        /// Detiene el sistema de inspección en tiempo real
        /// </summary>
        public virtual bool PararSistema()
        {
            // A implementar en heredados
            return false;
        }
        /// <summary>
        /// Se muestra un mensaje en el splash screen de la evolución de arranque del sistema
        /// </summary>
        protected virtual void MensajeInfoArranqueSistema(string mensaje)
        {
            // A implementar en heredados
        }
        #endregion
    }

    /// <summary>
    /// Enumerado que describe el estado del sistema
    /// </summary>
    public enum EstadoSistema
    {
        /// <summary>
        /// El sistema se encuentra detenido
        /// </summary>
        Detenido,
        /// <summary>
        /// El sistema se encuentra en transición de parado a iniciado
        /// </summary>
        Arrancando,
        /// <summary>
        /// El sistema se encuentra en transición de iniciado a parado
        /// </summary>
        Deteniendo,
        /// <summary>
        /// El sistema se encuentra iniciado
        /// </summary>
        Iniciado
    }

    /// <summary>
    /// Parámetros de la aplicación
    /// </summary>
    [Serializable]
    public class ConfiguracionSistema: OAlmacenXML
    {
        #region Atributo(s) estáticos
        /// <summary>
        /// Ruta por defecto del fichero xml de configuración
        /// </summary>
        public static string ConfFile = Path.Combine(ORutaParametrizable.AppFolder, "Configuracion" + Application.ProductName + ".xml");
        #endregion

        #region Atributo(s)
        /// <summary>
        /// Tiempo entre refresco de monitorización
        /// </summary>
        [XmlIgnore]
        public TimeSpan CadenciaMonitorizacion = TimeSpan.FromMilliseconds(200);

        /// <summary>
        /// Tiempo entre refresco de monitorización
        /// </summary>
        public int CadenciaMonitorizacionMilisegundos
        {
            get { return (int)Math.Round(this.CadenciaMonitorizacion.TotalMilliseconds); }
            set { this.CadenciaMonitorizacion = TimeSpan.FromMilliseconds(value); }
        }

        /// <summary>
        /// Indica si el sistema utiliza o no base de datos SQLServer o por el contrario ataca a ficheros XML
        /// </summary>
        public bool UtilizaBaseDeDatos = true;

        /// <summary>
        /// Lista de informaciones de las bases de datos existentes en el sistema
        /// </summary>
        public List<InformacionBasesDatos> ListaInformacionBasesDatos;

        /// <summary>
        /// Opciones de configuración del registro de eventos
        /// </summary>
        public OpcionesLogs OpcionesLogs;
        #endregion

        #region Constructor
        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public ConfiguracionSistema()
            : base(ConfFile)
        {
            this.ListaInformacionBasesDatos = new List<InformacionBasesDatos>();
            this.OpcionesLogs = new OpcionesLogs();
        }

        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public ConfiguracionSistema(string rutaFichero)
            : base(rutaFichero)
        {
        }
        #endregion Constructor
    }
}
