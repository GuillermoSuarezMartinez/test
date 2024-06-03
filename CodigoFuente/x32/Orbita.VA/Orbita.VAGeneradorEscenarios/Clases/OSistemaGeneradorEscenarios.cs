using System;
using Orbita.VAComun;

namespace CodeGenerator
{
    /// <summary>
    /// Clase que controla el inicio y la detención del resto de módulos instalados en el sistema
    /// </summary>
    public class OSistemaGeneradorEscenarios : OSistema
    {
        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="panelEstadoImg">Panel de imagen en la barra de estado del formulario principal donde se muestra el estado del sistema</param>
        /// <param name="panelEstadoTexto">Panel de texto en la barra de estado del formulario principal donde se muestra el estado del sistema</param>
        /// <param name="menu">Menú del formulario principal</param>
        public OSistemaGeneradorEscenarios(string configFile)
            : base(configFile)
        {
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Inicia el sistema de inspección en tiempo real
        /// </summary>
        protected bool InicioSistema()
        {
            bool resultado = false;

            if (this.EstadoSistema == OEstadoSistema.Detenido)
            {
                this.EstadoSistema = OEstadoSistema.Arrancando;
                resultado = true;

                // Fase 1: Se obtiene la conexión con la BBDD y se inicia el registro de eventos
                if (resultado)
                {
                    try
                    {
                        // Creación del log de eventos
                        MensajeInfoArranqueSistema("Creando log de eventos");
                        OVALogsManager.Constructor(this.OnShowLogMessage, this.OnShowLogException);
                        OVALogsManager.Inicializar();
                        OVALogsManager.Info(OModulosSistema.Sistema, "IniciarSistema", "Inicio del sistema");

                        // Conexión con la base de datos
                        if (this.Configuracion.UtilizaBaseDeDatos)
                        {
                            MensajeInfoArranqueSistema("Conectando con las bases de datos");
                            OBaseDatosManager.Constructor();
                            OBaseDatosManager.Inicializar();
                            OBaseDatosParam.Conectar();
                            OBaseDatosAlmacen.Conectar();
                        }
                    }
                    catch (Exception exception)
                    {
                        OVALogsManager.Error(OModulosSistema.Sistema, "IniciarSistema", exception);
                        resultado = false;
                    }
                }
            }

            return resultado;
        }
        /// <summary>
        /// Se ejecuta al finalizar el inicio del sistema
        /// </summary>
        protected void FinInicioSistema(ref bool resultado)
        {
            if (resultado)
            {
                try
                {
                    // Se finaliza el modo inicialización de los logs (tienen un comportamiento dinstinto)
                    OVALogsManager.FinInicializacion();

                    // Ocultamos el formulario Splash
                    MensajeInfoArranqueSistema("Inicio finalizado con éxito");
                    OVALogsManager.Info(OModulosSistema.Sistema, "IniciarSistema", "Inicio finalizado con éxito");
                }
                catch (Exception exception)
                {
                    OVALogsManager.Error(OModulosSistema.Sistema, "IniciarSistema", "Error: " + exception.ToString());
                    resultado = false;
                }
            }

            // Cambiamos el estado del sistema según el resultado del arranque
            if (resultado)
            {
                this.EstadoSistema = OEstadoSistema.Iniciado;
            }
            else
            {
                this.EstadoSistema = OEstadoSistema.Detenido;
            }
        }
        /// <summary>
        /// Detiene el funcionamiento del inspección en tiempo real
        /// </summary>
        protected bool ParoSistema()
        {
            bool resultado = false;

            if (this.EstadoSistema == OEstadoSistema.Iniciado)
            {
                this.EstadoSistema = OEstadoSistema.Deteniendo;
                resultado = true;

                OVALogsManager.Info(OModulosSistema.Sistema, "PararSistema", "Paro del sistema");
            }

            return resultado;
        }
        /// <summary>
        /// Se ejecuta al finalizar la detención del sistema
        /// </summary>
        protected void FinParoSistema(ref bool resultado)
        {
            if (resultado)
            {
                try
                {
                    // Finalizaación de las bases de datos
                    OBaseDatosManager.Finalizar();
                    OBaseDatosManager.Destructor();

                    // Finalización del log de eventos
                    OVALogsManager.Info(OModulosSistema.Sistema, "PararSistema", "Paro del sistema finalizado con éxito");
                    OVALogsManager.Finalizar();
                    OVALogsManager.Destructor();
                }
                catch (Exception exception)
                {
                    resultado = false;
                }

                // Cambiamos el estado del sistema según el resultado del arranque
                if (resultado)
                {
                    this.EstadoSistema = OEstadoSistema.Detenido;
                }
                else
                {
                    this.EstadoSistema = OEstadoSistema.Iniciado;
                }
            }
        }
	    #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Inicia el sistema de inspección en tiempo real
        /// </summary>
        public override bool IniciarSistema(bool incial)
        {
            bool resultado;

            resultado = this.InicioSistema();
            this.FinInicioSistema(ref resultado);

            return resultado;
        }
        /// <summary>
        /// Detiene el sistema de inspección en tiempo real
        /// </summary>
        public override bool PararSistema()
        {
            bool resultado;

            resultado = this.ParoSistema();
            this.FinParoSistema(ref resultado);

            return resultado;
        }
        /// <summary>
        /// Se muestra un mensaje en el splash screen de la evolución de arranque del sistema
        /// </summary>
        protected override void MensajeInfoArranqueSistema(string mensaje)
        {
            Console.WriteLine(mensaje);
        }
        #endregion

        #region Eventos
        private void OnShowLogMessage(string message)
        {
            Console.WriteLine(message);
        }

        private void OnShowLogException(Exception excepcion)
        {
            Console.WriteLine(excepcion.ToString());
        }
        #endregion
    }
}
