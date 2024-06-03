//***********************************************************************
// Assembly         : OrbitaUtiles
// Author           : crodriguez
// Created          : 03-03-2011
//
// Last Modified By : crodriguez
// Last Modified On : 03-03-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Threading;
namespace Orbita.Utiles
{
    /// <summary>
    /// Clase que representa hilos de ejecución (Threads).
    /// </summary>
    public class OHilo
    {
        #region Atributo(s)
        /// <summary>
        /// Hilo (Thread).
        /// </summary>
        Thread _thread;
        /// <summary>
        /// Tiempo de letargo del hilo.
        /// </summary>
        int _duermeThread;
        /// <summary>
        /// Descripción del hilo.
        /// </summary>
        string _descripcion;
        /// <summary>
        /// Iniciar hilo en el evento
        /// de inicialización.
        /// </summary>
        bool _iniciar;
        #region Evento(s)
        /// <summary>
        /// Evento de los métodos: 
        /// Iniciar(), Suspender(), Reanudar(), Terminar().
        /// </summary>
        public event ManejadorEvento OnDespuesAccion;
        #endregion
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Inicializar una nueva instancia de la clase OHilo.
        /// </summary>
        public OHilo() { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase OHilo.
        /// </summary>
        /// <param name="metodo">Proceso de ejecución del hilo.</param>
        /// <param name="iniciar">Indica si se inicia el hilo en el evento de inicialización.</param>
        public OHilo(ThreadStart metodo, bool iniciar)
        {
            // Crear un nuevo Thread.
            this._thread = new Thread(metodo);

            // Asignar el nombre.
            this._thread.Name = System.Guid.NewGuid().ToString();

            // Establecer la prioridad de todos
            // los Threads en ThreadPriority.
            this._thread.Priority = ThreadPriority.Normal;

            // Trabajar con los Threads en Background para así 
            // poder finalizar la ejecución de la aplicación.
            this._thread.IsBackground = true;

            // Indica si se inicia el hilo en el evento de
            // inicialización que se lanza en el método Add
            // de la colección de hilos.
            this._iniciar = iniciar;
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase OHilo.
        /// </summary>
        /// <param name="metodo">Proceso de ejecución del hilo.</param>
        /// <param name="descripcion">Descripción del hilo.</param>
        /// <param name="iniciar">Indica si se inicia el hilo en el evento de inicialización.</param>
        public OHilo(ThreadStart metodo, string descripcion, bool iniciar)
            : this(metodo, iniciar)
        {
            this._descripcion = descripcion;
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase OHilo.
        /// </summary>
        /// <param name="metodo">Proceso de ejecución del hilo.</param>
        /// <param name="prioridad">Prioridad de hilo.</param>
        /// <param name="segundoPlano">Indica si se ejecuta el hilo en segundo plano.</param>
        /// <param name="iniciar">Indica si se inicia el hilo en el evento de inicialización.</param>
        public OHilo(ThreadStart metodo, ThreadPriority prioridad, bool segundoPlano, bool iniciar)
        {
            // Crear un nuevo Thread.
            this._thread = new Thread(metodo);

            // Establecer la prioridad de todos
            // los Threads en ThreadPriority.
            this._thread.Priority = prioridad;

            // Trabajar con los Threads en Background para así 
            // poder finalizar la ejecución de la aplicación.
            this._thread.IsBackground = segundoPlano;

            // Indica si se inicia el hilo en el evento de
            // inicialización que se lanza en el método Add
            // de la colección de hilos.
            this._iniciar = iniciar;
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase OHilo.
        /// </summary>
        /// <param name="metodo">Proceso de ejecución del hilo.</param>
        /// <param name="nombre">Nombre del hilo.</param>
        /// <param name="prioridad">Prioridad de hilo.</param>
        /// <param name="segundoPlano">Indica si se ejecuta el hilo en segundo plano.</param>
        /// <param name="iniciar">Indica si se inicia el hilo en el evento de inicialización.</param>
        public OHilo(ThreadStart metodo, string nombre, ThreadPriority prioridad, bool segundoPlano, bool iniciar)
            : this(metodo, prioridad, segundoPlano, iniciar)
        {
            this._thread.Name = nombre;
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase OHilo.
        /// </summary>
        /// <param name="metodo">Proceso de ejecución del hilo.</param>
        /// <param name="nombre">Nombre del hilo.</param>
        /// <param name="descripcion">Descripción del hilo.</param>
        /// <param name="prioridad">Prioridad de hilo.</param>
        /// <param name="segundoPlano">Indica si se ejecuta el hilo en segundo plano.</param>
        /// <param name="iniciar">Indica si se inicia el hilo en el evento de inicialización.</param>
        public OHilo(ThreadStart metodo, string nombre, string descripcion, ThreadPriority prioridad, bool segundoPlano, bool iniciar)
            : this(metodo, nombre, prioridad, segundoPlano, iniciar)
        {
            this._descripcion = descripcion;
        }
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Hilo (Thread).
        /// </summary>
        public Thread Thread
        {
            get { return this._thread; }
            set { this._thread = value; }
        }
        /// <summary>
        /// Tiempo de letargo del hilo.
        /// </summary>
        public int Letargo
        {
            get { return this._duermeThread; }
            set { this._duermeThread = value; }
        }
        /// <summary>
        /// Nombre del hilo.
        /// </summary>
        public string Nombre
        {
            get { return this._thread.Name; }
        }
        /// <summary>
        /// Descripción del hilo.
        /// </summary>
        public string Descripcion
        {
            get { return this._descripcion; }
        }
        /// <summary>
        /// Iniciación del hilo.
        /// </summary>
        public bool Iniciarlo
        {
            get { return this._iniciar; }
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Iniciar el hilo.
        /// </summary>
        public void Iniciar()
        {
            if (!EstaVivo())
            {
                this._thread.Start();
                // En C# debemos comprobar que el evento no sea null.
                if (OnDespuesAccion != null)
                {
                    // El evento se lanza como cualquier delegado.
                    OnDespuesAccion(this, new OEventArgs(Enum.GetName(typeof(Accion), Accion.Iniciado)));
                }
            }
        }
        /// <summary>
        /// Suspender el hilo activo.
        /// </summary>
        public void Suspender()
        {
            if (EstaVivo())
            {
                this._thread.Suspend();
                // En C# debemos comprobar que el evento no sea null.
                if (OnDespuesAccion != null)
                {
                    // El evento se lanza como cualquier delegado.
                    OnDespuesAccion(this, new OEventArgs(Enum.GetName(typeof(Accion), Accion.Suspendido)));
                }
            }
        }
        /// <summary>
        /// Reanudar el hilo suspendido.
        /// </summary>
        public void Reanudar()
        {
            if (EstaVivo())
            {
                this._thread.Resume();
                // En C# debemos comprobar que el evento no sea null.
                if (OnDespuesAccion != null)
                {
                    // El evento se lanza como cualquier delegado.
                    OnDespuesAccion(this, new OEventArgs(Enum.GetName(typeof(Accion), Accion.Reanudado)));
                }
            }
            else
            {
                // No es posible reanudar un hilo que no esté vivo,
                // por tanto, iniciar un hilo sin vitalidad.
                Iniciar();
            }
        }
        /// <summary>
        /// Terminar el hilo activo.
        /// </summary>
        public bool Terminar()
        {
            if (EstaVivo())
            {
                this._thread.Abort();
                // En C# debemos comprobar que el evento no sea null.
                if (OnDespuesAccion != null)
                {
                    // El evento se lanza como cualquier delegado.
                    OnDespuesAccion(this, new OEventArgs(Enum.GetName(typeof(Accion), Accion.Terminado)));
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Bloquea el subproceso de la llamada hasta que un subproceso finaliza, pero
        /// continúa realizando suministro de SendMessage y COM estándar.
        /// </summary>
        public bool Join()
        {
            if (EstaVivo())
            {
                this._thread.Join();
                // En C# debemos comprobar que el evento no sea null.
                if (OnDespuesAccion != null)
                {
                    // El evento se lanza como cualquier delegado.
                    OnDespuesAccion(this, new OEventArgs(Enum.GetName(typeof(Accion), Accion.EnEspera)));
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Bloquea el subproceso de la llamada hasta que un subproceso finaliza, pero
        /// continúa realizando suministro de SendMessage y COM estándar.
        /// </summary>
        /// <param name="timeout">Número de milisegundos en los que se va a esperar a que el subproceso finalice.</param>
        /// <returns></returns>
        public bool Join(int timeout)
        {
            if (EstaVivo())
            {
                this._thread.Join(timeout);
                // En C# debemos comprobar que el evento no sea null.
                if (OnDespuesAccion != null)
                {
                    // El evento se lanza como cualquier delegado.
                    OnDespuesAccion(this, new OEventArgs(Enum.GetName(typeof(Accion), Accion.EnEspera)));
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Bloquea el subproceso de la llamada hasta que un subproceso finaliza, pero
        /// continúa realizando suministro de SendMessage y COM estándar.
        /// </summary>
        /// <param name="timeout">Número de milisegundos en los que se va a esperar a que el subproceso finalice.</param>
        /// <returns></returns>
        public bool Join(TimeSpan timeout)
        {
            if (EstaVivo())
            {
                this._thread.Join(timeout);
                // En C# debemos comprobar que el evento no sea null.
                if (OnDespuesAccion != null)
                {
                    // El evento se lanza como cualquier delegado.
                    OnDespuesAccion(this, new OEventArgs(Enum.GetName(typeof(Accion), Accion.EnEspera)));
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// Comprobar la vitalidad de un hilo.
        /// </summary>
        public bool EstaVivo()
        {
            return this._thread.IsAlive;
        }
        #endregion
    }
}