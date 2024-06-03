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
using System.Threading;
using System.Windows.Forms;

namespace Orbita.VAComun
{
    /// <summary>
    /// Clase estática para el control de la ejecución de threads
    /// </summary>
    public static class ThreadRuntime
    {
        #region Atributo(s)
        /// <summary>
        /// Listado de los threads del sistema
        /// </summary>
        public static List<ThreadOrbita> ListaThreads;
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Constructor de los campos de la clase
        /// </summary>
        public static void Constructor()
        {
            ListaThreads = new List<ThreadOrbita>();
        }

        /// <summary>
        /// Destruye los campos de la clase
        /// </summary>
        public static void Destructor()
        {
            ListaThreads = null;
        }

        /// <summary>
        /// Inicializa las variables necesarias para el funcionamiento de la clase
        /// </summary>
        public static void Inicializar()
        {
        }

        /// <summary>
        /// Finaliza la ejecución
        /// </summary>
        public static void Finalizar()
        {
            foreach (ThreadOrbita thread in ListaThreads)
            {
                thread.Stop();
            }
        }

        /// <summary>
        /// Detiene la ejecución hasta que el semaforo se pone en verde
        /// Debe de ser llamada desde el propio thread!!
        /// </summary>
        /// <returns>Falso si el semaforo no existe</returns>
        public static bool EsperarSemaforo()
        {
            bool resultado = false;
            ThreadOrbita thread = ListaThreads.Find(delegate(ThreadOrbita t) { return t.ThreadEjecucion.Equals(Thread.CurrentThread); });
            if (thread != null)
            {
                resultado = thread.Wait();
            }

            return resultado;
        }

        /// <summary>
        /// Pone el semaforo en rojo
        /// Debe de ser llamaeda desde fuera del thread!!!
        /// </summary>
        /// <param name="codigo">Identificador del objeto</param>
        /// <returns>Falso si el semaforo no existe</returns>
        public static bool SemaforoRojo(string codigo)
        {
            bool resultado = false;

            ThreadOrbita thread = ListaThreads.Find(delegate(ThreadOrbita t) { return string.Equals(t.Codigo, codigo, StringComparison.CurrentCultureIgnoreCase); });
            if (thread != null)
            {
                resultado = thread.Pause();
            }

            return resultado;
        }

        /// <summary>
        /// Pone el semaforo en verde
        /// Debe de ser llamaeda desde fuera del thread!!!
        /// </summary>
        /// <param name="codigo">Identificador del objeto</param>
        /// <returns>Falso si el semaforo no existe</returns>
        public static bool SemaforoVerde(string codigo)
        {
            bool resultado = false;

            ThreadOrbita thread = ListaThreads.Find(delegate(ThreadOrbita t) { return string.Equals(t.Codigo, codigo, StringComparison.CurrentCultureIgnoreCase); });
            if (thread != null)
            {
                resultado = thread.Resume();
            }

            return resultado;
        }

        /// <summary>
        /// Informa si se está ejecutando desde el mismo hilo de ejecución que la aplicación principal o desde otro distinto
        /// </summary>
        /// <returns>Verdadero si se está ejecutando desde el mismo hilo de ejecución que la aplicación principal</returns>
        public static bool EjecucionEnTrheadPrincipal()
        {
            return !App.FormularioPrincipalMDI.InvokeRequired;
        }

        /// <summary>
        /// Ejecuta el método pasado como parámetro en el hilo principal de la aplicación
        /// </summary>
        /// <param name="metodo">Método que se desea ejecutar en el hilo principal</param>
        /// <param name="parametros">Parámetros del métodos que se desea ejecutar en el hilo principal</param>
        public static void SincronizarConThreadPrincipal(Delegate metodo, object[] parametros)
        {
            try
            {
                App.FormularioPrincipalMDI.Invoke(metodo, parametros);
            }
            catch (Exception exception)
            {
                // ¿No puede lanzarse el error al estar dentro de un thread?
                LogsRuntime.Error(ModulosSistema.Threads, "SincronizarConThreadPrincipal", exception);
            }
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Añade un nuevo Thread al sistema
        /// </summary>
        /// <param name="threadOrbita">Thread a añadir</param>
        internal static void NuevoThread(ThreadOrbita threadOrbita)
        {
            // Añadimos a la lista
            if (ListaThreads != null)
            {
                ListaThreads.Add(threadOrbita);
            }
        }

        /// <summary>
        /// Añade un nuevo Thread al sistema
        /// </summary>
        /// <param name="threadOrbita">Thread a eliminar</param>
        internal static void EliminarThread(ThreadOrbita threadOrbita)
        {
            // Añadimos a la lista
            if (ListaThreads != null)
            {
                ListaThreads.Remove(threadOrbita);
            }
        }
        #endregion
    }

    /// <summary>
    /// Thread donde se ha implementado el evento OnFinalize
    /// </summary>
    public class ThreadOrbita : IDisposable
    {
        #region Atributo(s)
        /// <summary>
        /// Thread de ejecución de la tarea
        /// </summary>
        internal Thread ThreadEjecucion;
        /// <summary>
        /// Prioridad del thread
        /// </summary>
        protected ThreadPriority Priority;
        /// <summary>
        /// Estado de funcionamiento del thread
        /// </summary>
        public EstadoThread Estado;
        /// <summary>
        /// Evento de reanudación de la suspensión
        /// </summary>
        private ManualResetEvent FinSuspensionEvent;
        /// <summary>
        /// Indica que se ha de salir del bucle de ejecución
        /// </summary>
        private bool FlagFinalizacion;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Estado del thread
        /// </summary>
        protected System.Threading.ThreadState ThreadState
        {
            get { return this.ThreadEjecucion.ThreadState; }
        }

        /// <summary>
        /// Código identificativo de la clase.
        /// </summary>
        private string _Codigo;
        /// <summary>
        /// Código identificativo de la clase.
        /// </summary>
        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }

        /// <summary>
        /// Tiempo de descanso entre bucles del thread
        /// </summary>
        private int _TiempoSleep;

        /// <summary>
        /// Tiempo de descanso entre bucles del thread
        /// </summary>
        public int TiempoSleep
        {
            get { return _TiempoSleep; }
            set { _TiempoSleep = value; }
        }

        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ThreadOrbita(string codigo)
        {
            this.Codigo = codigo;
            this.Priority = ThreadPriority.Normal;
            this.FinSuspensionEvent = new ManualResetEvent(false);

            this.CrearThread();
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ThreadOrbita(string codigo, int tiempoSleep, ThreadPriority threadPriority)
        {
            this.Codigo = codigo;
            this.TiempoSleep = tiempoSleep;
            this.Priority = threadPriority;
            this.FinSuspensionEvent = new ManualResetEvent(false);

            this.CrearThread();
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Inicio del thread
        /// </summary>
        public void Start()
        {
            if (this.Estado == EstadoThread.ThreadInitial)
            {
                this.FlagFinalizacion = false;
                this.ThreadEjecucion.Start();
            }
        }
        /// <summary>
        /// Fin del trehad
        /// </summary>
        public void Stop()
        {
            if ((this.Estado == EstadoThread.ThreadRunning) || (this.Estado == EstadoThread.ThreadSuspended) || (this.Estado == EstadoThread.ThreadSuspending))
            {
                try
                {
                    this.ThreadEjecucion.Abort();
                    bool cerrado = false;
                    while (!cerrado)
                    {
                        cerrado = this.ThreadEjecucion.Join(10);
                        Application.DoEvents();
                    }
                    this.Estado = EstadoThread.ThreadEnded;
                }
                catch (Exception exception)
                {
                    LogsRuntime.Error(ModulosSistema.Threads, "Stop", exception);
                }
            }
        }
        /// <summary>
        /// Fin del trehad
        /// </summary>
        public void Stop(int milisecondsTimeout)
        {
            if (this.Estado == EstadoThread.ThreadRunning)
            {
                this.FlagFinalizacion = true;

                DateTime momentoTimeOut = DateTime.Now.AddMilliseconds(milisecondsTimeout);
                while ((this.Estado != EstadoThread.ThreadEnded) && (DateTime.Now < momentoTimeOut))
                {
                    App.Espera(10);
                }
            }

            this.Stop();
        }
        /// <summary>
        /// Pasua de la ejecución del thread. Este método no debe ser llamado desde el propio thread
        /// </summary>
        /// <returns>Verdadero si la acción ha resultado exitosa</returns>
        public bool Pause()
        {
            bool resultado = false;

            if (this.Estado == EstadoThread.ThreadRunning)
            {
                this.Estado = EstadoThread.ThreadSuspending;
                resultado = true;
            }

            return resultado;
        }
        /// <summary>
        /// Continua la ejecución del thread. Este método no debe ser llamado desde el propio thread
        /// </summary>
        /// <returns>Verdadero si la acción ha resultado exitosa</returns>
        public bool Resume()
        {
            bool resultado = false;

            if ((this.Estado == EstadoThread.ThreadSuspended) || (this.Estado == EstadoThread.ThreadSuspending))
            {
                this.Estado = EstadoThread.ThreadRunning;
                this.FinSuspensionEvent.Set();
                resultado = true;
            }

            return resultado;
        }
        /// <summary>
        /// Detiene la ejecución hasta que el semaforo se pone en verde
        /// </summary>
        /// <returns>Verdadero si la acción ha resultado exitosa</returns>
        public bool Wait()
        {
            bool resultado = false;

            if (this.Estado == EstadoThread.ThreadSuspending)
            {
                try
                {
                    this.Estado = EstadoThread.ThreadSuspended;

                    this.FinSuspensionEvent.WaitOne();
                    this.FinSuspensionEvent.Reset();
                    resultado = true;
                }
                catch (Exception exception)
                {
                    LogsRuntime.Error(ModulosSistema.Threads, "Wait", exception);
                }
            }

            return resultado;
        }
        /// <summary>
        /// Elimina la memoria asignada por el objeto.
        /// </summary>
        public void Dispose()
        {
            this.Stop();
            ThreadRuntime.EliminarThread(this);
        }
        /// <summary>
        /// Elimina la memoria asignada por el objeto.
        /// </summary>
        public void Dispose(int milisecondsTimeout)
        {
            this.Stop(milisecondsTimeout);
            ThreadRuntime.EliminarThread(this);
        }
        /// <summary>
        /// Cambia la prioridad al thread
        /// </summary>
        /// <param name="threadPriority">Prioridad a establecer</param>
        public void ChangePriority(ThreadPriority threadPriority)
        {
            if (this.ThreadEjecucion != null)
            {
                if (this.Priority != threadPriority)
                {
                    this.Priority = threadPriority;
                    this.ThreadEjecucion.Priority = threadPriority;
                }
            }
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Inicio del thread
        /// </summary>
        private void CrearThread()
        {
            this.Estado = EstadoThread.ThreadInitial;

            this.ThreadEjecucion = new Thread(this.EjecucionInterna);
            this.ThreadEjecucion.Name = this.Codigo;
            this.ThreadEjecucion.Priority = this.Priority;
            this.ThreadEjecucion.IsBackground = true;

            this.OnEjecucion += Ejecucion;

            ThreadRuntime.NuevoThread(this);
        }
        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Método a heredar para implementar la ejecución del thread.
        /// Este método se está ejecutando en un bucle. Para salir del bucle hay que devolver finalize a true.
        /// </summary>
        protected virtual void Ejecucion(out bool finalize)
        {
            // Implementado en hijos
            finalize = false;
        }
        #endregion

        #region Evento(s)
        /// <summary>
        /// Método donde realmente se ejecuta el thread.
        /// </summary>
        protected void EjecucionInterna()
        {
            this.Estado = EstadoThread.ThreadRunning;

            if (OnEjecucion != null)
            {
                this.FlagFinalizacion = false;
                while ((!this.FlagFinalizacion) && (this.ThreadState != System.Threading.ThreadState.AbortRequested))
                {
                    try
                    {
                        ThreadRuntime.EsperarSemaforo();
                        bool flagFinalizacionInterno;
                        this.OnEjecucion(out flagFinalizacionInterno);

                        // Se activará el flag de finalización bien cuando se diga de forma interna (desde el método OnEjecucion)
                        // o cuando se active desde el exterior (atributo FlagFinalización)
                        this.FlagFinalizacion |= flagFinalizacionInterno;
                    }
                    catch (ThreadAbortException)
                    {
                        //LogsRuntime.Error(ModulosSistema.Threads, "EjecucionInterna", "Abort", abortException);
                        this.Estado = EstadoThread.ThreadEnded;
                        return;
                    }
                    catch (Exception exception)
                    {
                        LogsRuntime.Error(ModulosSistema.Threads, "EjecucionInterna: " + this._Codigo, exception);
                    }
                    Thread.Sleep(this._TiempoSleep);
                }
            }

            // Evento de finalización del thread
            if (OnFinEjecucion != null)
            {
                ThreadRuntime.SincronizarConThreadPrincipal(OnFinEjecucion, new object[] { this, null });
            }

            this.Estado = EstadoThread.ThreadEnded;
        }
        #endregion

        #region Definición de delegado(s)
        /// <summary>
        /// Delegado que de la ejecución del thread
        /// </summary>
        public delegate void ThreadRun(out bool finalize);
        /// <summary>
        /// Delegado que de la ejecución del thread
        /// </summary>
        public ThreadRun OnEjecucion;

        /// <summary>
        /// Delegado que de la ejecución del fin del thread
        /// </summary>
        public delegate void ThreadCompleted();
        /// <summary>
        /// Delegado que de la ejecución del fin del thread
        /// </summary>
        public ThreadCompleted OnFinEjecucion;
        #endregion

        #region Enumerados
        /// <summary>
        /// Estado del thread
        /// </summary>
        public enum EstadoThread
        {
            /// <summary>
            /// El thread ha sido creado pero no está en funcionamiento
            /// </summary>
            ThreadInitial = 0,
            /// <summary>
            /// El thread está ejecutandose
            /// </summary>
            ThreadRunning = 1,
            /// <summary>
            /// El thread se está suspendiendo
            /// </summary>
            ThreadSuspending = 2,
            /// <summary>
            /// El thread está suspendido
            /// </summary>
            ThreadSuspended = 3,
            /// <summary>
            /// El thread se ha detenido
            /// </summary>
            ThreadEnded = 4
        }
        #endregion
    }
}
