//***********************************************************************
// Assembly         : Orbita.VAComun
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
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
    public static class SystemRuntime
    {
        #region Atributo(s)
        /// <summary>
        /// Campo del sistema que se ha arrancado
        /// </summary>
        public static Sistema Sistema;
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
        public static Configuracion Configuracion
        {
            get { return Sistema.Configuracion; }
            set { Sistema.Configuracion = value; }
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Constructor de los campos estáticos de la clase
        /// </summary>
        public static void Constructor(Sistema sistema)
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
    public class Sistema
    {
        #region Atributo(s)
        /// <summary>
        /// Enumerado que describe el estado del sistema
        /// </summary>
        public EstadoSistema EstadoSistema;

        /// <summary>
        /// Parámetros de la aplicación
        /// </summary>
        public Configuracion Configuracion;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public Sistema()
            : this(Configuracion.ConfFile)
        {
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public Sistema(string configFile)
        {
            this.EstadoSistema = EstadoSistema.Detenido;

            try
            {
                this.Configuracion = (Configuracion)(new Configuracion(configFile).CargarDatos());
            }
            catch (FileNotFoundException exception)
            {
                LogsRuntime.Error(ModulosSistema.Sistema, "Constructor", exception);

                this.Configuracion = new Configuracion();
                this.Configuracion.Guardar();
            }
        }

        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Inicia el sistema de inspección en tiempo real
        /// </summary>
        internal bool IniciarSistema(bool incial)
        {
            bool resultado;

            // Mostramos el formulario Splash
            SplashRuntime.Contructor();

            resultado = this.InicioSistema();
            this.FinInicioSistema(ref resultado);

            // Ocultamos el formulario splash
            SplashRuntime.Destructor();

            if (!resultado)
            {
                OMensajes.MostrarAviso("El sistema no se ha podido iniciar con éxito.");
            }

            return resultado;
        }
        /// <summary>
        /// Detiene el sistema de inspección en tiempo real
        /// </summary>
        internal bool PararSistema()
        {
            bool resultado;

            resultado = this.ParoSistema();
            this.FinParoSistema(ref resultado);

            // Se limpian todos los objetos eliminados
            GC.Collect();

            return resultado;
        }
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
        protected virtual bool InicioSistema()
        {
            bool resultado = false;

            if (this.EstadoSistema == EstadoSistema.Detenido)
            {
                this.EstadoSistema = EstadoSistema.Arrancando;
                resultado = true;

                // Fase 1: Se obtiene la conexión con la BBDD y se inicia el registro de eventos
                if (resultado)
                {
                    try
                    {
                        // Conexión con la base de datos
                        if (this.Configuracion.UtilizaBaseDeDatos)
                        {
                            SplashRuntime.Mensaje("Conectando con las bases de datos");
                            BaseDatosRuntime.Constructor();
                            BaseDatosRuntime.Inicializar();
                            BaseDatosParam.Conectar();
                            BaseDatosAlmacen.Conectar();
                        }

                        // Creación de los threads del sistema
                        SplashRuntime.Mensaje("Creando log los hilos de ejecución");
                        ThreadRuntime.Constructor();
                        ThreadRuntime.Inicializar();

                        // Creación del log de eventos
                        SplashRuntime.Mensaje("Creando log de eventos");
                        LogsRuntime.Constructor();
                        LogsRuntime.Inicializar();
                        LogsRuntime.Info(ModulosSistema.Sistema, "IniciarSistema", "Inicio del sistema");

                        // Creación de la monitorización del sistema
                        MonitorizacionSistemaRuntime.Constructor();
                        MonitorizacionSistemaRuntime.NuevaMonitorizacion(TipoMonitorizacion.MonitorizacionCpuRam);
                        MonitorizacionSistemaRuntime.NuevaMonitorizacion(TipoMonitorizacion.MonitorizacionDiscos);
                        MonitorizacionSistemaRuntime.NuevaMonitorizacion(TipoMonitorizacion.MonitorizacionProcesos);
                        MonitorizacionSistemaRuntime.NuevaMonitorizacion(TipoMonitorizacion.MonitorizacionConexiones);
                        MonitorizacionSistemaRuntime.Inicializar();
                    }
                    catch (Exception exception)
                    {
                        LogsRuntime.Error(ModulosSistema.Sistema, "IniciarSistema", exception);
                        resultado = false;
                    }
                }

                // Fase 2: Se inician, uno a uno, los módulos básicos del sistema
                if (resultado)
                {
                    try
                    {
                        // Creación del manejo avanzado de ventanas
                        SplashRuntime.Mensaje("Construyendo el escritorio");
                        EscritoriosRuntime.Constructor();

                        // Creación de los cronómetros del sistema
                        SplashRuntime.Mensaje("Construyendo el control de tiempos");
                        CronometroRuntime.Constructor();
                        CronometroRuntime.Inicializar();

                        // Creación del almacen de objetos visuales
                        AlmacenVisualRuntime.Constructor();
                        AlmacenVisualRuntime.Inicializar();
                    }
                    catch (Exception exception)
                    {
                        LogsRuntime.Error(ModulosSistema.Sistema, "IniciarSistema", "Error: " + exception.ToString());
                        resultado = false;
                    }
                }

            }
            else
            {
                OMensajes.MostrarError("No se puede iniciar el sistema porque este no se encuentra parado.");
            }

            return resultado;
        }
        /// <summary>
        /// Se ejecuta al finalizar el inicio del sistema
        /// </summary>
        protected virtual void FinInicioSistema(ref bool resultado)
        {
            if (resultado)
            {
                try
                {
                    // Inicialización del escritorio actual
                    SplashRuntime.Mensaje("Inicializado el escritorio");
                    EscritoriosRuntime.Inicializar();

                    // Monitorización inicial para ver el estado del sistema en el momento del arranque
                    MonitorizacionSistemaRuntime.SiguienteMonitorizacion();
                    MonitorizacionSistemaRuntime.Log();

                    // Se finaliza el modo inicialización de los logs (tienen un comportamiento dinstinto)
                    LogsRuntime.FinInicializacion();

                    // Ocultamos el formulario Splash
                    SplashRuntime.Mensaje("Inicio finalizado con éxito");
                    LogsRuntime.Info(ModulosSistema.Sistema, "IniciarSistema", "Inicio finalizado con éxito");
                }
                catch (Exception exception)
                {
                    LogsRuntime.Error(ModulosSistema.Sistema, "IniciarSistema", "Error: " + exception.ToString());
                    resultado = false;
                }
            }

            // Cambiamos el estado del sistema según el resultado del arranque
            if (resultado)
            {
                this.EstadoSistema = EstadoSistema.Iniciado;
            }
            else
            {
                this.EstadoSistema = EstadoSistema.Detenido;
            }
        }
        /// <summary>
        /// Detiene el funcionamiento del inspección en tiempo real
        /// </summary>
        protected virtual bool ParoSistema()
        {
            bool resultado = false;

            if (this.EstadoSistema == EstadoSistema.Iniciado)
            {
                this.EstadoSistema = EstadoSistema.Deteniendo;
                resultado = true;

                try
                {
                    LogsRuntime.Info(ModulosSistema.Sistema, "PararSistema", "Paro del sistema");

                    // Finalización manejo avanzado de ventanas                    
                    EscritoriosRuntime.Finalizar();
                    EscritoriosRuntime.Destructor();
                }
                catch (Exception exception)
                {
                    LogsRuntime.Error(ModulosSistema.Sistema, "PararSistema", "Error: " + exception.ToString());
                    resultado = false;
                }
            }
            else
            {
                OMensajes.MostrarError("No se puede detener el sistema porque este no se encuentra iniciado.");
            }

            return resultado;
        }
        /// <summary>
        /// Se ejecuta al finalizar la detención del sistema
        /// </summary>
        protected virtual void FinParoSistema(ref bool resultado)
        {
            if (resultado)
            {
                try
                {
                    // Creación del almacen de objetos visuales
                    AlmacenVisualRuntime.Finalizar();
                    AlmacenVisualRuntime.Destructor();

                    // Finalización de los cronómetros del sistema
                    CronometroRuntime.Finalizar();
                    CronometroRuntime.Destructor();
                }
                catch (Exception exception)
                {
                    LogsRuntime.Error(ModulosSistema.Sistema, "PararSistema", "Error: " + exception.ToString());
                    resultado = false;
                }

                try
                {
                    // Finalizaación de las bases de datos
                    BaseDatosRuntime.Finalizar();
                    BaseDatosRuntime.Destructor();

                    // Finalización del log de eventos
                    LogsRuntime.Info(ModulosSistema.Sistema, "PararSistema", "Paro del sistema finalizado con éxito");
                    LogsRuntime.Finalizar();
                    LogsRuntime.Destructor();

                    // Finalización de los threads del sistema
                    SplashRuntime.Mensaje("Creando log los hilos de ejecución");
                    ThreadRuntime.Finalizar();
                    ThreadRuntime.Destructor();

                    // Liberación de memoria
                    GC.Collect();
                }
                catch (Exception exception)
                {
                    OMensajes.MostrarError(exception.ToString());
                    resultado = false;
                }

                // Cambiamos el estado del sistema según el resultado del arranque
                if (resultado)
                {
                    this.EstadoSistema = EstadoSistema.Detenido;
                }
                else
                {
                    this.EstadoSistema = EstadoSistema.Iniciado;
                }
            }
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
    public class Configuracion: OAlmacenXML
    {
        #region Atributo(s) estáticos
        /// <summary>
        /// Ruta por defecto del fichero xml de configuración
        /// </summary>
        public static string ConfFile = Path.Combine(RutaParametrizable.AppFolder, "Configuracion" + Application.ProductName + ".xml");
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

        /// <summary>
        /// Opciones de configuración del gestor de escritorios
        /// </summary>
        public OpcionesEscritorios OpcionesEscritorio;
        #endregion

        #region Constructor
        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public Configuracion()
            : base(ConfFile)
        {
            this.ListaInformacionBasesDatos = new List<InformacionBasesDatos>();
            this.OpcionesEscritorio = new OpcionesEscritorios();
            this.OpcionesLogs = new OpcionesLogs();
        }

        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public Configuracion(string rutaFichero)
            : base(rutaFichero)
        {
        }
        #endregion Constructor
    }
}
