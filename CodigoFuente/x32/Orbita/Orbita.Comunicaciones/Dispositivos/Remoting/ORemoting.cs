//***********************************************************************
// Assembly         : Orbita.Comunicaciones
// Author           : crodriguez
// Created          : 03-11-2011
//
// Last Modified By : crodriguez
// Last Modified On : 03-12-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.Remoting;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;

namespace Orbita.Comunicaciones
{
    /// <summary>
    /// Clase estática de métodos remoting.
    /// </summary>
    [Serializable]
    public static class ORemoting
    {
        #region Atributo(s)

        /// <summary>
        /// Valor de inicialización.
        /// </summary>
        readonly static bool Inic = false;
        /// <summary>
        /// Colección de  tipos  conocidos
        /// en el establecimiento remoting.
        /// </summary>
        static IDictionary WellKnownTypes;

        #region Constante(s)

        /// <summary>
        /// Ruta de configuración general.
        /// </summary>
        const string CONFIG = @"\Config";
        /// <summary>
        /// Ruta de configuración de servidor
        /// y cliente para .NET Remoting.
        /// </summary>
        const string REMOTING = @"\Config\remoting";

        #endregion

        #endregion

        #region Método(s) público(s)

        /// <summary>
        /// Método de inicialización del configurador de servidor para .NET Remoting.
        /// </summary>
        /// <param name="espacioDEnombres">Espacio de nombres del ensamblado.</param>
        /// <param name="clase">Nombre de la clase.</param>
        /// <param name="puerto">Puerto de salidad de datos.</param>
        /// <param name="canal"></param>
        public static string InicConfiguracionServidor(string espacioDEnombres, string clase, int puerto, string canal)
        {
            // Obtener el nombre del ensamblado.
            AssemblyName ensamblado = Assembly.GetExecutingAssembly().GetName();

            // Crear el documento Xml.
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(string.Format(CultureInfo.CurrentCulture,
                @"<?xml version='1.0' encoding='utf-8' ?>" +
                 "<configuration>" +
                    "<system.runtime.remoting>" +
                        "<application>" +
                            "<channels>" +
                                "<channel name=\"{4}\" ref=\"tcp\" port=\"{0}\">" +
                                    "<serverProviders><provider ref=\"wsdl\" /><formatter ref=\"soap\" typeFilterLevel=\"Full\" /><formatter ref=\"binary\" typeFilterLevel=\"Full\" />" +
                                    "</serverProviders>" +
                                "</channel>" +
                            "</channels>" +
                            "<service><wellknown mode=\"Singleton\" type=\"{1}.{2}, {1}\" objectUri=\"{3}.soap\" />" +
                            "</service>" +
                        "</application>" +
                    "</system.runtime.remoting>" +
                 "</configuration>", puerto, espacioDEnombres, clase, ensamblado.Name,canal));
            // Ruta de salida.
            string directorio = string.Concat(Application.StartupPath, CONFIG);

            string fichero = string.Concat(Application.StartupPath, REMOTING + puerto + ".config.xml");

            // Control en la creación del directorio de salida.
            if (!Directory.Exists(directorio))
            {
                Directory.CreateDirectory(directorio);
            }
            // Guardar el documento en la salida.
            doc.Save(fichero);

            // Configurar .NET Remoting.
            Configurar(fichero);

            return fichero;
        }

        /// <summary>
        /// Método de inicialización del configurador de cliente para .NET Remoting.
        /// </summary>
        /// <param name="puerto">Puerto de salidad de datos.</param>
        public static string InicConfiguracionCliente(int puerto)
        {
            return InicConfiguracionCliente(puerto,"localhost");
        }

        /// <summary>
        /// Método de inicialización del configurador de cliente para .NET Remoting.
        /// </summary>
        /// <param name="puerto">Puerto de salidad de datos.</param>
        /// <param name="maquina">Nombre de la maquina servidor de remotings.</param>
        public static string InicConfiguracionCliente(int puerto, string maquina)
        {
            // Obtener el nombre del ensamblado.
            AssemblyName ensamblado = Assembly.GetExecutingAssembly().GetName();

            // Crear el documento Xml.
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(string.Format(CultureInfo.CurrentCulture,
                @"<?xml version='1.0' encoding='utf-8' ?>" +
                 "<configuration>" +
                    "<system.runtime.remoting>" +
                        "<application>" +
                            "<channels>" +
                                "<channel ref=\"tcp\" port='0' />" +
                            "</channels>" +
                            "<client><wellknown type=\"Orbita.Comunicaciones.{1}, {0}\" url=\"tcp://{3}:{2}/{0}.soap\" />" +
                            "</client>" +
                        "</application>" +
                    "</system.runtime.remoting>" +
                 "</configuration>", ensamblado.Name, typeof(IOCommRemoting).Name, puerto, maquina));
           
            string directorio = string.Concat(Application.StartupPath, CONFIG);
            string fichero = string.Concat(Application.StartupPath, REMOTING);

            // Control en la creación del directorio de salida.
            if (!Directory.Exists(directorio))
            {
                Directory.CreateDirectory(directorio);
            }
            // Guardar el documento en la salida.
            doc.Save(fichero);

            // Configurar .NET Remoting.
            Configurar(fichero);

            return fichero;
        }

        /// <summary>
        /// Obtener objeto de tipo remoting por el cliente
        /// de conexión.
        /// </summary>
        /// <param name="tipo">Interface de eventos y métodos remoting.</param>
        /// <returns>Objeto con el establecimiento de conexión.</returns>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public static object GetObject(Type tipo)
        {
            if (!Inic) InicTipoCache();
            WellKnownClientTypeEntry entrada = (WellKnownClientTypeEntry)WellKnownTypes[tipo];
            if (entrada == null)
            {
                throw new RemotingException("Tipo (WellKnownClientTypeEntry) no encontrado.");
            }
            return Activator.GetObject(entrada.ObjectType, entrada.ObjectUrl);
        }

        #endregion

        #region Método(s) privado(s)

        /// <summary>
        /// Configurar Remoting a partir de la información
        /// del  fichero creado  en  los  inicializadores.
        /// </summary>
        /// <param name="fichero">Fichero de configuración</param>
        static void Configurar(string fichero)
        {
            // Configurar .NET Remoting.
            RemotingConfiguration.Configure(fichero, false);
        }

        /// <summary>
        /// Inicializar el tipo caché.
        /// </summary>
        static void InicTipoCache()
        {
            WellKnownTypes = new Hashtable();
            foreach (WellKnownClientTypeEntry entrada in RemotingConfiguration.GetRegisteredWellKnownClientTypes())
            {
                WellKnownTypes.Add(entrada.ObjectType, entrada);
            }
        }

        #endregion
    }
}
