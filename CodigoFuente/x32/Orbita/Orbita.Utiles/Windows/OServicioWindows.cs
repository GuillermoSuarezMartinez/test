using System;
using System.ServiceProcess;
namespace Orbita.Utiles
{
    /// <summary>
    /// OServicioWindows.
    /// </summary>
    public class OServicioWindows : IDisposable
    {
        #region Atributo(s)
        /// <summary>
        /// Nombre del servicio Windows.
        /// </summary>
        string _nombre;
        /// <summary>
        /// Máquina donde se encuentra el servicio Windows.
        /// </summary>
        string _maquina;
        /// <summary>
        /// TimeOut de espera hasta el inicio/paro
        /// del servicio Windows.
        /// </summary>
        int _timeOutMilisegundos;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Inicializar una nueva instancia de la clase OServicioWindows.
        /// </summary>
        public OServicioWindows() { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase OServicioWindows.
        /// </summary>
        /// <param name="nombre">Nombre del servicio Windows.</param>
        public OServicioWindows(string nombre)
        {
            this._nombre = nombre;
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase OServicioWindows.
        /// </summary>
        /// <param name="nombre">Nombre del servicio Windows.</param>
        /// <param name="maquina">Máquina donde se encuentra el 
        /// servicio Windows.</param>
        public OServicioWindows(string nombre, string maquina)
        {
            this._nombre = nombre;
            this._maquina = maquina;
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase OServicioWindows.
        /// </summary>
        /// <param name="nombre">Nombre del servicio Windows.</param>
        /// <param name="timeOutMilisegundos">Tiempo de espera hasta que el
        /// servicio alcanza el estado especificado.</param>
        public OServicioWindows(string nombre, int timeOutMilisegundos)
        {
            this._nombre = nombre;
            this._timeOutMilisegundos = timeOutMilisegundos;
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase OServicioWindows.
        /// </summary>
        /// <param name="nombre">Nombre del servicio Windows.</param>
        /// <param name="maquina">Máquina donde se encuentra el 
        /// servicio Windows.</param>
        /// <param name="timeOutMilisegundos">Tiempo de espera hasta que el
        /// servicio alcanza el estado especificado.</param>
        public OServicioWindows(string nombre, string maquina, int timeOutMilisegundos)
        {
            this._nombre = nombre;
            this._maquina = maquina;
            this._timeOutMilisegundos = timeOutMilisegundos;
        }
        #endregion

        #region Destructor(es)
        /// <summary>
        /// Indica si ya se llamo al método Dispose. (default = false)
        /// </summary>
        bool disposed = false;
        /// <summary>
        /// Implementa IDisposable.
        /// No  hacer  este  método  virtual.
        /// Una clase derivada no debería ser
        /// capaz de  reemplazar este método.
        /// </summary>
        public void Dispose()
        {
            // Llamo al método que  contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
            // Este objeto será limpiado por el método Dispose.
            // Llama al método del recolector de basura, GC.SuppressFinalize.
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Método  sobrecargado de  Dispose que será  el que
        /// libera los recursos. Controla que solo se ejecute
        /// dicha lógica una  vez y evita que el GC tenga que
        /// llamar al destructor de clase.
        /// </summary>
        /// <param name="disposing">Indica si llama al método Dispose.</param>
        protected virtual void Dispose(bool disposing)
        {
            // Preguntar si Dispose ya fue llamado.
            if (!this.disposed)
            {
                // Finalizar correctamente los recursos no manejados.
                this._nombre = null;
                this._maquina = null;

                // Marcar como desechada ó desechandose,
                // de forma que no se puede ejecutar el
                // código dos veces.
                disposed = true;
            }
        }
        /// <summary>
        /// Destructor(es) de clase.
        /// En caso de que se nos olvide “desechar” la clase,
        /// el GC llamará al destructor, que tambén ejecuta 
        /// la lógica anterior para liberar los recursos.
        /// </summary>
        ~OServicioWindows()
        {
            // Llamar a Dispose(false) es óptimo en terminos
            // de legibilidad y mantenimiento.
            Dispose(false);
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Método que inicia el servicio Windows.
        /// </summary>
        public void Iniciar()
        {
            using (ServiceController servicio = new ServiceController(this._nombre))
            {
                TimeSpan timeOut = TimeSpan.FromMilliseconds(this._timeOutMilisegundos);

                servicio.Start();
                servicio.WaitForStatus(ServiceControllerStatus.Running, timeOut);
            }
        }
        #endregion
    }

}
