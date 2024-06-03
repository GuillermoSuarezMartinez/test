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
using System.Collections.Generic;
using System.Data;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Clase estática para el acceso al hardware
    /// </summary>
    public static class OHardwareManager
    {
        #region Atributo(s)

        /// <summary>
        /// Lista de las cámaras del sistema
        /// </summary>
        public static Dictionary<string, IHardware> ListaHardware;

        /// <summary>
        /// Lista de todas las vistas de hardware del sistema
        /// </summary>
        public static Dictionary<string, OVistaHardware> Vistas;
        
        #endregion

        #region Método(s) público(s)

        /// <summary>
        /// Construye los campos de la clase
        /// </summary>
        public static void Constructor()
        {
            ListaHardware = new Dictionary<string, IHardware>();
            Vistas = new Dictionary<string, OVistaHardware>();

            OIOManager.Constructor();
            OCamaraManager.Constructor();

            // Consulta de todas las vistas existentes en el sistema
            DataTable dtVista = Orbita.VAComun.AppBD.GetVistas();
            if (dtVista.Rows.Count > 0)
            {
                // Cargamos todas las vistas existentes en el sistema
                OVistaHardware vista;
                foreach (DataRow drVista in dtVista.Rows)
                {
                    // Creamos cada una de las vistas del sistema
                    string codVista = drVista["CodVista"].ToString();
                    vista = new OVistaHardware(codVista);
                    Vistas.Add(codVista, vista);
                }
            }
        }

        /// <summary>
        /// Destruye los campos de la clase
        /// </summary>
        public static void Destructor()
        {
            OCamaraManager.Destructor();
            OIOManager.Destructor();

            ListaHardware = null;
        }

        /// <summary>
        /// Inicializa las propieades de la clase
        /// </summary>
        public static void Inicializar()
        {
            OIOManager.Inicializar();
            foreach (IHardware hardware in OIOManager.ListaTarjetasIO)
            {
                ListaHardware.Add(hardware.Codigo, hardware);
            }

            OCamaraManager.Inicializar();
            foreach (IHardware hardware in OCamaraManager.ListaCamaras)
            {
                ListaHardware.Add(hardware.Codigo, hardware);
            }
        }

        /// <summary>
        /// Finaliza las propiedades de la clase
        /// </summary>
        public static void Finalizar()
        {
            OCamaraManager.Finalizar();
            OIOManager.Finalizar();
        }

        /// <summary>
        /// Busca la cámara con el código indicado
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <returns>Hardware encontrado</returns>
        public static IHardware GetHardware(string codigo)
        {
            return GetHardware(string.Empty, codigo);
        }

        /// <summary>
        /// Busca la cámara con el código indicado
        /// </summary>
        /// <param name="vista">Vista utilizada</param>
        /// <param name="codigo">Código de la cámara</param>
        /// <returns>Hardware encontrado</returns>
        public static IHardware GetHardware(string vista, string codigo)
        {
            IHardware hardware;
            if (TryGetHardware(vista, codigo, out hardware))
            {
                return hardware;
            }
            return null;
        }

        /// <summary>
        /// Comienza una reproducción continua de la cámara
        /// </summary>
        /// <returns></returns>
        public static bool Start(string codigo)
        {
            return OCamaraManager.Start(codigo);
        }

        /// <summary>
        /// Termina una reproducción continua de la cámara
        /// </summary>
        /// <returns></returns>
        public static bool Stop(string codigo)
        {
            return OCamaraManager.Stop(codigo);

        }

        /// <summary>
        /// Comienza una reproducción de todas las cámaras
        /// </summary>
        /// <returns></returns>
        public static bool StartAll()
        {
            return OCamaraManager.StartAll();
        }

        /// <summary>
        /// Termina la reproducción de todas las cámaras
        /// </summary>
        /// <returns></returns>
        public static bool StopAll()
        {
            return OCamaraManager.StopAll();
        }

        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Método para acceder a un hardware
        /// </summary>
        /// <param name="codVista">Vista</param>
        /// <param name="codAlias">Código o alias del hardware</param>
        /// <param name="hardware">Hardware devuelto</param>
        /// <returns>Devuelve la variable correspondientes</returns>
        private static bool TryGetHardware(string codVista, string codAlias, out IHardware hardware)
        {
            // Inicialización de resultados
            string alias = codAlias;

            // Cambio el alias al de la vista
            if (codVista != string.Empty)
            {
                // Cambio el alias
                OVistaHardware vistaHardware;
                if (Vistas.TryGetValue(codVista, out vistaHardware))
                {
                    string codHardware;
                    if (vistaHardware.ListaAlias.TryGetValue(codAlias, out codHardware))
                    {
                        alias = codHardware;
                    }
                }
            }

            return ListaHardware.TryGetValue(alias, out hardware);
        }
        #endregion
    }

    /// <summary>
    /// Interfaz para el acceso a las caracerísticas comunes de todos los dispositivos hardware
    /// </summary>
    public interface IHardware
    {
        #region Propiedad(es)
        /// <summary>
        /// Código identificador de la cámara
        /// </summary>
        string Codigo { get; }

        /// <summary>
        /// Nombre identificativo de la cámara
        /// </summary>
        string Nombre { get; }

        /// <summary>
        /// Descripción de la cámara
        /// </summary>
        string Descripcion { get; }

        /// <summary>
        /// Habilita o deshabilita el funcionamiento
        /// </summary>
        bool Habilitado { get; }

        /// <summary>
        /// Fabricante del tipo de cámara
        /// </summary>
        string Fabricante { get; }

        /// <summary>
        /// Modelo del tipo de cámara
        /// </summary>
        string Modelo { get; }

        /// <summary>
        /// Tipo de hardware
        /// </summary>
        OTipoHardware TipoHardware { get; }

        /// <summary>
        /// Lista de todos los terminales de la tarjeta de IO
        /// </summary>
        List<OTerminalIOBase> ListaTerminales { get; }
        #endregion

        #region Método(s) público(s)

        /// <summary>
        /// Carga los valores del hardware
        /// </summary>
        void Inicializar();

        /// <summary>
        /// Finaliza el hardware
        /// </summary>
        void Finalizar();

        /// <summary>
        /// Comienza el hardware
        /// </summary>
        /// <returns></returns>
        bool Start();

        /// <summary>
        /// Para el hardware
        /// </summary>
        /// <returns></returns>
        bool Stop();

        #endregion
    }

        /// <summary>
    /// Clase que implementa las vistas del hardware
    /// </summary>
    public class OVistaHardware
    {
        #region Atributo(s)
        /// <summary>
        /// Código de la vista
        /// </summary>
        public string Codigo;

        /// <summary>
        /// Lista de todos los alias de la vista
        /// </summary>
        public Dictionary<string, string> ListaAlias;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codVista">Código de la vista</param>
        public OVistaHardware(string codVista)
        {
            this.Codigo = codVista;

            this.ListaAlias = new Dictionary<string, string>();
            this.RellenarAlias(codVista);
        }

        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Rellena la lista de alias de la vista
        /// </summary>
        private void RellenarAlias(string codVista)
        {
            // Consulta de todos los alias existentes en el sistema
            DataTable dtVar = AppBD.GetAliasVistaHardware(codVista);
            if (dtVar.Rows.Count > 0)
            {
                // Cargamos todos los alias existentes en el sistema
                foreach (DataRow drVar in dtVar.Rows)
                {
                    // Creamos cada una de las variables del sistema
                    string codAlias = drVar["CodAlias"].ToString();
                    string codHardware = drVar["CodHardware"].ToString();
                    ListaAlias.Add(codAlias, codHardware);
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// Tipo de hardware dispoinble
    /// </summary>
    public enum OTipoHardware
    {
        /// <summary>
        /// Cámara: Dispositivo de adquisición de imágenes
        /// </summary>
        HwCamara = 1,
        /// <summary>
        /// Dispoisitivo de entradas / salidas
        /// </summary>
        HwModuloIO = 2
    }
}
