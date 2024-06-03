//***********************************************************************
// Assembly         : Orbita.VAComun
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 12-12-2012
// Description      : Nueva clase ProductorConsumidor
//                    Posibilidad de lanzar eventos en el propio thread o 
//                    en el thread principal
//                    Se evita tener más de un delegado suscrito al evento run
//
// Last Modified By : aibañez
// Last Modified On : 16-11-2012
// Description      : Cabiados métodos de finalización
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
    public static class OThreadManager
    {
        #region Atributo(s)
        /// <summary>
        /// Listado de los threads del sistema
        /// </summary>
        public static List<OThread> ListaThreads;
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Constructor de los campos de la clase
        /// </summary>
        public static void Constructor()
        {
            ListaThreads = new List<OThread>();
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
            foreach (OThread thread in ListaThreads)
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
            OThread thread = ListaThreads.Find(delegate(OThread t)
                {
                    bool result = false;
                    if (t.ThreadEjecucion != null)
                    {
                        result = t.ThreadEjecucion.Equals(Thread.CurrentThread);
                    }
                    return result;
                });
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

            OThread thread = ListaThreads.Find(delegate(OThread t) { return string.Equals(t.Codigo, codigo, StringComparison.CurrentCultureIgnoreCase); });
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

            OThread thread = ListaThreads.Find(delegate(OThread t) { return string.Equals(t.Codigo, codigo, StringComparison.CurrentCultureIgnoreCase); });
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
            return !App.FormularioPrincipal.InvokeRequired;
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
                App.FormularioPrincipal.Invoke(metodo, parametros);
            }
            catch (Exception exception)
            {
                OVALogsManager.Info(ModulosSistema.Threads, "SincronizarConThreadPrincipal", exception);
            }
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Añade un nuevo Thread al sistema
        /// </summary>
        /// <param name="threadOrbita">Thread a añadir</param>
        internal static void NuevoThread(OThread threadOrbita)
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
        internal static void EliminarThread(OThread threadOrbita)
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
    public class OThread : IDisposable
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
        /// <summary>
        /// Indica si la ejecución del evento Run se ha de realizar en el thread o fuera de él
        /// </summary>
        private bool EjecutarRunEnThread;
        /// <summary>
        /// Indica si la ejecución del evento Fin se ha de realizar en el thread o fuera de él
        /// </summary>
        private bool EjecutarFinEnThread;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Estado del thread
        /// </summary>
        protected ThreadState ThreadState
        {
            get
            {
                ThreadState resultado = ThreadState.Unstarted;

                if (this.ThreadEjecucion != null)
                {
                    resultado = this.ThreadEjecucion.ThreadState;
                }

                return resultado;
            }
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
        public OThread(string codigo) :
            this(codigo, 1, ThreadPriority.Normal)
        {
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OThread(string codigo, int tiempoSleep, ThreadPriority threadPriority)
        {
            this.Codigo = codigo;
            this.TiempoSleep = tiempoSleep;
            this.Priority = threadPriority;
            this.FinSuspensionEvent = new ManualResetEvent(false);

            this.CrearSuscripcionRun(this.Ejecucion, true); // Delegado único
            this.CrearSuscripcionFin(this.FinEjecucion, true); // Delegado múltiple
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Lanza la ejecución del thread
        /// </summary>
        /// <param name="finalize">Indica la finalización del thread</param>
        private void LanzarEventoEjecucion(ref bool finalize)
        {
            try
            {
                if (this.OnEjecucion != null)
                {
                    if (this.EjecutarRunEnThread)
                    {
                        this.OnEjecucion(ref finalize);
                    }
                    else
                    {
                        object[] obj = new object[] { finalize };
                        App.FormularioPrincipal.Invoke(this.OnEjecucion, obj);
                        finalize = (bool)obj[0];
                    }
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Info(ModulosSistema.Threads, "LanzarEventoEjecucion", exception);
            }
        }

        /// <summary>
        /// Lanza la el final de ejecución del thread
        /// </summary>
        private void LanzarEventoFinEjecucion()
        {
            try
            {
                if (this.OnFinEjecucion != null)
                {
                    if (this.EjecutarFinEnThread)
                    {
                        this.OnFinEjecucion();
                    }
                    else
                    {
                        App.FormularioPrincipal.Invoke(this.OnFinEjecucion);
                    }
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Info(ModulosSistema.Threads, "LanzarEventoFinEjecucion", exception);
            }
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Inicio del thread
        /// </summary>
        public void Start()
        {
            if ((this.Estado == EstadoThread.ThreadInitial) || (this.Estado == EstadoThread.ThreadEnded))
            {
                this.FlagFinalizacion = false;
                this.CrearThread();
                this.Estado = EstadoThread.ThreadRunning;
                this.ThreadEjecucion.Start();
            }
        }
        /// <summary>
        /// Inicio del thread
        /// </summary>
        public void StartPaused()
        {
            if ((this.Estado == EstadoThread.ThreadInitial) || (this.Estado == EstadoThread.ThreadEnded))
            {
                this.FlagFinalizacion = false;
                this.CrearThread();
                this.Estado = EstadoThread.ThreadSuspending;
                this.ThreadEjecucion.Start();
            }
        }
        /// <summary>
        /// Fin del trehad
        /// </summary>
        public void Stop()
        {
            if (((this.ThreadEjecucion != null)) && ((this.Estado == EstadoThread.ThreadRunning) || (this.Estado == EstadoThread.ThreadSuspended) || (this.Estado == EstadoThread.ThreadSuspending)))
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
                    OVALogsManager.Error(ModulosSistema.Threads, "Stop", exception);
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
                    OThread.Espera(10);
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
                    OVALogsManager.Info(ModulosSistema.Threads, "Wait", exception);
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
            this.DestruirThread();
        }
        /// <summary>
        /// Elimina la memoria asignada por el objeto.
        /// </summary>
        public void Dispose(int milisecondsTimeout)
        {
            this.Stop(milisecondsTimeout);
            this.DestruirThread();
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
        /// <summary>
        /// Creación de la suscripción al evento de ejecución
        /// </summary>
        public void CrearSuscripcionRun(ThreadRun threadRun, bool ejecutarEnThread)
        {
            this.OnEjecucion = threadRun; // Este método no puede ser nunca múltiple
            this.EjecutarRunEnThread = ejecutarEnThread;
        }
        /// <summary>
        /// Creación de la suscripción al evento de finalización de la ejecución
        /// </summary>
        public void CrearSuscripcionFin(OSimpleMethod threadEnd, bool ejecutarEnThread)
        {
            this.OnFinEjecucion += threadEnd; // Este método puede ser múltiple
            this.EjecutarFinEnThread = ejecutarEnThread;
        }
        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Inicio del thread
        /// </summary>
        protected virtual void CrearThread()
        {
            this.Estado = EstadoThread.ThreadInitial;

            this.ThreadEjecucion = new Thread(this.EjecucionInterna);
            this.ThreadEjecucion.Name = this.Codigo;
            this.ThreadEjecucion.Priority = this.Priority;
            this.ThreadEjecucion.IsBackground = true;

            OThreadManager.NuevoThread(this);
        }

        /// <summary>
        /// Fin del thread
        /// </summary>
        protected virtual void DestruirThread()
        {
            //this.OnEjecucion = null; // Delegado único
            //this.OnFinEjecucion -= FinEjecucion;

            this.ThreadEjecucion = null;
            OThreadManager.EliminarThread(this);
        }

        /// <summary>
        /// Método a heredar para implementar la ejecución del thread.
        /// Este método se está ejecutando en un bucle. Para salir del bucle hay que devolver finalize a true.
        /// </summary>
        protected virtual void Ejecucion(ref bool finalize)
        {
            // Implementado en hijos
        }

        /// <summary>
        /// Método a heredar para implementar la ejecución del thread.
        /// Este método se está ejecutando en un bucle. Para salir del bucle hay que devolver finalize a true.
        /// </summary>
        protected virtual void FinEjecucion()
        {
            // Implementado en hijos
        }
        #endregion

        #region Método(s) estático(s)
        /// <summary>
        /// Método que realiza una espera (sin parar la ejecución del sistema) de cierto tiempo en milisegundos
        /// </summary>
        /// <param name="timeOut">Tiempo de espera en milisegundos</param>
        public static void Espera(int timeOut)
        {
            // Momento en el que finalizará la espera
            DateTime momentoTimeOut = DateTime.Now.AddMilliseconds(timeOut);

            while (DateTime.Now < momentoTimeOut)
            {
                Application.DoEvents();
            }
        }

        /// <summary>
        /// Método que realiza una espera hasta que cierto valor sea verdadero durante un máximo de tiempo
        /// </summary>
        /// <param name="valor">Valor al cual se espera que su estado sea verdadero o falso</param>
        /// <param name="valorEsperado">Valor de comparación</param>
        /// <param name="timeOut">Tiempo máximo de la espera en milisegundos</param>
        /// <returns>Verdadero si el valor a cambiado a verdadero antes de que finalizase el TimeOut</returns>
        public static bool Espera(DelegadoEspera delegadoEspera, int timeOut)
        {
            // Momento en el que finalizará la espera
            DateTime momentoTimeOut = DateTime.Now.AddMilliseconds(timeOut);

            bool valor = delegadoEspera();
            while (!valor && (DateTime.Now < momentoTimeOut))
            {
                Application.DoEvents();
                valor = delegadoEspera();
            }

            return valor;
        }

        /// <summary>
        /// Delegado de la espera
        /// </summary>
        /// <returns></returns>
        public delegate bool DelegadoEspera();

        /// <summary>
        /// Método que realiza una espera hasta que cierto valor sea verdadero durante un máximo de tiempo
        /// </summary>
        /// <param name="valor">Valor al cual se espera que su estado sea verdadero o falso</param>
        /// <param name="valorEsperado">Valor de comparación</param>
        /// <param name="timeOut">Tiempo máximo de la espera en milisegundos</param>
        /// <returns>Verdadero si el valor a cambiado a verdadero antes de que finalizase el TimeOut</returns>
        public static bool Espera(ref bool valor, bool valorEsperado, int timeOut)
        {
            // Momento en el que finalizará la espera
            DateTime momentoTimeOut = DateTime.Now.AddMilliseconds(timeOut);

            while ((valor != valorEsperado) && (DateTime.Now < momentoTimeOut))
            {
                Application.DoEvents();
            }

            return valor == valorEsperado;
        }

        /// <summary>
        /// Método que realiza una espera hasta que cierto valor sea verdadero durante un máximo de tiempo
        /// </summary>
        /// <param name="valor">Valor al cual se espera que su estado sea verdadero o falso</param>
        /// <param name="valorEsperado">Valor de comparación</param>
        /// <param name="timeOut">Tiempo máximo de la espera en milisegundos</param>
        /// <returns>Verdadero si el valor a cambiado a verdadero antes de que finalizase el TimeOut</returns>
        public static bool Espera(ref object valor, object valorEsperado, int timeOut)
        {
            // Momento en el que finalizará la espera
            DateTime momentoTimeOut = DateTime.Now.AddMilliseconds(timeOut);

            while ((valor != valorEsperado) && (DateTime.Now < momentoTimeOut))
            {
                Application.DoEvents();
            }

            return valor == valorEsperado;
        }
        #endregion

        #region Evento(s)
        /// <summary>
        /// Método donde realmente se ejecuta el thread.
        /// </summary>
        protected void EjecucionInterna()
        {
            if (this.OnEjecucion != null)
            {
                this.FlagFinalizacion = false;
                while ((!this.FlagFinalizacion) && (this.ThreadState != System.Threading.ThreadState.AbortRequested))
                {
                    try
                    {
                        OThreadManager.EsperarSemaforo();
                        bool flagFinalizacionInterno = false;

                        // Evento de ejecución del thread
                        this.LanzarEventoEjecucion(ref flagFinalizacionInterno);

                        // Se activará el flag de finalización bien cuando se diga de forma interna (desde el método OnEjecucion)
                        // o cuando se active desde el exterior (atributo FlagFinalización)
                        this.FlagFinalizacion |= flagFinalizacionInterno;
                    }
                    catch (ThreadAbortException)
                    {
                        this.Estado = EstadoThread.ThreadEnded;
                        return;
                    }
                    catch (Exception exception)
                    {
                        OVALogsManager.Info(ModulosSistema.Threads, "EjecucionInterna: " + this._Codigo, exception);
                    }
                    Thread.Sleep(this._TiempoSleep);
                }
            }

            // Evento de finalización del thread
            if (this.ThreadState != System.Threading.ThreadState.AbortRequested)
            {
                this.LanzarEventoFinEjecucion();
            }

            this.Estado = EstadoThread.ThreadEnded;
        }
        #endregion

        #region Definición de delegado(s)
        /// <summary>
        /// Delegado que de la ejecución del thread
        /// </summary>
        protected ThreadRun OnEjecucion;

        /// <summary>
        /// Delegado que de la ejecución del fin del thread
        /// </summary>
        protected OSimpleMethod OnFinEjecucion;
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

    /// <summary>
    /// Clase que implementa un consumidor en thread
    /// </summary>
    public class OProductorConsumidorThread<T> : IDisposable
    {
        #region Atributo(s)
        /// <summary>
        /// Cola del consumidor
        /// </summary>
        private Queue<T> Cola;

        /// <summary>
        /// Capacidad máxima de la cola
        /// </summary>
        private int CapacidadMaxima;

        /// <summary>
        /// Objeto bloqueante
        /// </summary>
        private object LockObject;

        /// <summary>
        /// Thread de ejecución
        /// </summary>
        OThread Thread;

        /// <summary>
        /// Indica si la ejecución del consumidor se ha de realizar en el thread o fuera de él
        /// </summary>
        private bool EjecutarConsumidorEnThread;

        /// <summary>
        /// Indica si la ejecución del productor se ha de realizar en el thread o fuera de él
        /// </summary>
        private bool EjecutarProductorEnThread;

        /// <summary>
        /// Indica si la ejecución de la monitorización se ha de realizar en el thread o fuera de él
        /// </summary>
        private bool EjecutarMonitorizacionEnThread;
        #endregion

        #region Propiedad(es)
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
        /// Posibles estados del productor / consumidor
        /// </summary>
        private EstadoProductorConsumidor _Estado;
        /// <summary>
        /// Posibles estados del productor / consumidor
        /// </summary>
	    public EstadoProductorConsumidor Estado
	    {
		    get { return _Estado;}
		    set { _Estado = value;}
	    }

        /// <summary>
        /// Número de elementos en la cola
        /// </summary>
	    public int Count
	    {
		    get 
            { 
                int resultado = 0;
                lock (this.LockObject)
                {
                    resultado = this.Cola.Count;
                }
                return resultado;
            }
	    }

        /// <summary>
        /// Número de instancias totales añadidas a la cola
        /// </summary>
        private long _Total;
        /// <summary>
        /// Número de instancias totales añadidas a la cola
        /// </summary>
        public long Total
        {
            get { return _Total; }
            set { _Total = value; }
        }
	    #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OProductorConsumidorThread(string codigo) :
            this(codigo, 1, ThreadPriority.Normal, 100)
        {
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OProductorConsumidorThread(string codigo, int tiempoSleep, ThreadPriority threadPriority, int capacidadMaxima)
        {
            this.Codigo = codigo;
            this.Cola = new Queue<T>();
            this._Total = 0;
            this.CapacidadMaxima = capacidadMaxima;
            this.LockObject = new object();
            this.Estado = EstadoProductorConsumidor.Detenido;
            this.Thread = new OThread(codigo, tiempoSleep, threadPriority);

            this.CrearSuscripcionConsumidor(EjecucionConsumidor, true);
            this.CrearSuscripcionProductor(EjecucionProductor, true);
            this.CrearSuscripcionMonitorizacion(Monitorizacion, true);

            this.Thread.CrearSuscripcionRun(Ejecucion, true);
            this.Thread.CrearSuscripcionFin(FinEjecucion, true);
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Lanza la ejecución del thread de consumición
        /// </summary>
        /// <param name="valor">Valor a consumir</param>
        private void LanzarEventoConsumidor(T valor)
        {
            try
            {
                if (this.OnEjecucionConsumidor != null)
                {
                    if (this.EjecutarConsumidorEnThread)
                    {
                        this.OnEjecucionConsumidor(valor);
                    }
                    else
                    {
                        App.FormularioPrincipal.Invoke(this.OnEjecucionConsumidor, valor);
                    }
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Info(ModulosSistema.Threads, "LanzarEventoConsumidor", exception);
            }
        }

        /// <summary>
        /// Lanza la ejecución del thread de producción
        /// </summary>
        /// <param name="encolar">Indica que se ha de encolar un elemento</param>
        /// <param name="valor">Valor producido</param>
        private void LanzarEventoProductor(ref bool encolar, ref T valor)
        {
            try
            {
                if (this.OnEjecucionProductor != null)
                {
                    if (this.EjecutarProductorEnThread)
                    {
                        this.OnEjecucionProductor(ref encolar, ref valor);
                    }
                    else
                    {
                        App.FormularioPrincipal.Invoke(this.OnEjecucionProductor, valor);

                        object[] obj = new object[] { encolar, valor };
                        App.FormularioPrincipal.Invoke(this.OnEjecucionProductor, obj);
                        encolar = (bool)obj[0];
                        valor = (T)obj[1];
                    }
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Info(ModulosSistema.Threads, "LanzarEventoProductor", exception);
            }
        }

        /// <summary>
        /// Lanza la ejecución del thread de monitorizacion
        /// </summary>
        /// <param name="finalize">Indica la finalización del thread</param>
        /// <param name="valor">Valor producido</param>
        private void LanzarEventoMonitorizacion(ref bool finalize)
        {
            try
            {
                if (this.OnMonitorizacion != null)
                {
                    if (this.EjecutarMonitorizacionEnThread)
                    {
                        this.OnMonitorizacion(ref finalize);
                    }
                    else
                    {
                        object[] obj = new object[] { finalize };
                        App.FormularioPrincipal.Invoke(this.OnMonitorizacion, obj);
                        finalize = (bool)obj[0];
                    }
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Info(ModulosSistema.Threads, "LanzarEventoMonitorizacion", exception);
            }
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Añade el valor a encolar
        /// </summary>
        /// <param name="valor"></param>
        public bool Encolar(T valor)
        {
            bool resultado = false;
            if ((this.Estado == EstadoProductorConsumidor.EnEjecucion) && (valor is T))
            {
                lock (this.LockObject)
                {
                    if (this.Cola.Count < this.CapacidadMaxima)
                    {
                        this.Cola.Enqueue(valor);
                        this._Total++;
                        resultado = true;
                    }
                }
            }
            return resultado;
        }

        /// <summary>
        /// Devuelve el valor desencolado
        /// </summary>
        /// <param name="valor"></param>
        public bool Desencolar(out T valor)
        {
            bool resultado = false;
            valor = default(T);

            if (this.Estado == EstadoProductorConsumidor.EnEjecucion)
            {
                lock (this.LockObject)
                {
                    if (this.Cola.Count > 0)
                    {
                        valor = this.Cola.Dequeue();
                        resultado = true;
                    }
                }
            }

            return resultado;
        }

        /// <summary>
        /// Inicio del thread
        /// </summary>
        public void Start()
        {
            if (this.Estado == EstadoProductorConsumidor.Detenido)
            {
                this.Estado = EstadoProductorConsumidor.EnEjecucion;
                this._Total = 0;
                this.Thread.Start();
            }
        }

        /// <summary>
        /// Fin del thread
        /// </summary>
        public void Stop()
        {
            if (this.Estado == EstadoProductorConsumidor.EnEjecucion)
            {
                this.Estado = EstadoProductorConsumidor.Deteniendo;
            }        
        }

        /// <summary>
        /// Elimina la memoria asignada por el objeto.
        /// </summary>
        public void Dispose()
        {
            this.Stop();
            this.Thread.Stop();
            this.Thread = null;
            this.OnEjecucionConsumidor = null;
            this.OnEjecucionProductor = null;
            this.OnMonitorizacion = null;
        }

        /// <summary>
        /// Borra la cola
        /// </summary>
        public void Clear()
        {
            lock (this.LockObject)
            {
                this.Cola.Clear();
            }
        }

        /// <summary>
        /// Elimina la memoria asignada por el objeto.
        /// </summary>
        public void Dispose(int milisecondsTimeout)
        {
            this.Stop();
            this.Thread.Stop(milisecondsTimeout);
            this.Thread = null;
            this.OnEjecucionConsumidor = null;
            this.OnEjecucionProductor = null;
            this.OnMonitorizacion = null;
        }

        /// <summary>
        /// Creación de la suscripción al evento de ejecución del consumidor
        /// </summary>
        public void CrearSuscripcionConsumidor(ConsumidorRun consumidorRun, bool ejecutarEnThread)
        {
            this.OnEjecucionConsumidor = consumidorRun; // Este método no puede ser nunca múltiple
            this.EjecutarConsumidorEnThread = ejecutarEnThread;
        }

        /// <summary>
        /// Creación de la suscripción al evento de ejecución del productor
        /// </summary>
        public void CrearSuscripcionProductor(ProductorRun productorRun, bool ejecutarEnThread)
        {
            this.OnEjecucionProductor = productorRun; // Este método no puede ser nunca múltiple
            this.EjecutarProductorEnThread = ejecutarEnThread;
        }

        /// <summary>
        /// Creación de la suscripción al evento de monitorización del productor / consumidor
        /// </summary>
        public void CrearSuscripcionMonitorizacion(ThreadRun monitorizacionRun, bool ejecutarEnThread)
        {
            this.OnMonitorizacion = monitorizacionRun; // Este método no puede ser nunca múltiple
            this.EjecutarMonitorizacionEnThread = ejecutarEnThread;
        }

        /// <summary>
        /// Creación de la suscripción al evento de finalización del productor / consumidor
        /// </summary>
        public void CrearSuscripcionFin(OSimpleMethod monitorizacionRun, bool ejecutarEnThread)
        {
            this.Thread.CrearSuscripcionFin(monitorizacionRun, ejecutarEnThread);
        }
        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Método que ejecuta la acción del consumidor.
        /// </summary>
        /// <param name="valor"></param>
        protected virtual void EjecucionConsumidor(T valor)
        {
            // A implementar en heredados
        }

        /// <summary>
        /// Método que ejecuta la acción del productor.
        /// </summary>
        /// <param name="valor"></param>
        protected virtual void EjecucionProductor(ref bool encolar, ref T valor)
        {
            // A implementar en heredados
        }

        /// <summary>
        /// Ejecución de la monitorización del productor / consumidor
        /// </summary>
        protected virtual void Monitorizacion(ref bool finalize)
        {
            // Implementado en heredados
        }

        /// <summary>
        /// Fin de la ejecución del thread
        /// </summary>
        protected virtual void FinEjecucion()
        {
            // A implementar en heredados
        }
        #endregion

        #region Evento(s)
        /// <summary>
        /// Ejecución del thread
        /// </summary>
        /// <param name="finalize"></param>
        protected void Ejecucion(ref bool finalize)
        {
            finalize = false;

            // Comprobación del estado de la cola
            if ((this.Estado == EstadoProductorConsumidor.Deteniendo) && (this.Count == 0))
	        {
                finalize = true;
	        }

            // Monitorización de la ejecución del productor / consumidor
            if (!finalize)
            {
                // Evento de monitorización del thread
                this.LanzarEventoMonitorizacion(ref finalize);
            }

            if (!finalize)
            {
                // Llamada al productor
                try
                {
                    if (this.OnEjecucionProductor != null)
                    {
                        T valorEncolar = default(T);
                        bool encolar = false;
                        
                        // Evento del productor
                        this.LanzarEventoProductor(ref encolar, ref valorEncolar);

                        if (encolar && (valorEncolar is T))
                        {
                            this.Encolar(valorEncolar);
                        }
                    }
                }
                catch (Exception exception)
                {
                    OVALogsManager.Info(ModulosSistema.Threads, "EjecucionProductor", exception);
                }

                // Llamada al consumidor
                try
                {
                    if (this.OnEjecucionConsumidor != null)
                    {
                        T valorDesencolar;
                        bool hayDato = this.Desencolar(out valorDesencolar);

                        if (hayDato)
                        {
                            // Evento del consumidor
                            this.LanzarEventoConsumidor(valorDesencolar);
                        }
                    }
                }
                catch (Exception exception)
                {
                    OVALogsManager.Info(ModulosSistema.Threads, "EjecucionConsumidor", exception);
                }
            }

            // Actualicamos el estado del productor / consumidor
            if (finalize)
            {
                this.Estado = EstadoProductorConsumidor.Detenido;
            }
        }
        #endregion

        #region Definición de delegado(s)
        /// <summary>
        /// Delegado que de la ejecución del consumidor
        /// </summary>
        public delegate void ConsumidorRun(T valor);
        /// <summary>
        /// Delegado que de la ejecución del consumidor
        /// </summary>
        protected ConsumidorRun OnEjecucionConsumidor;

        /// <summary>
        /// Delegado que de la ejecución del consumidor
        /// </summary>
        public delegate void ProductorRun(ref bool encolar, ref T valor);
        /// <summary>
        /// Delegado que de la ejecución del consumidor
        /// </summary>
        protected ProductorRun OnEjecucionProductor;

        /// <summary>
        /// Delegado que de la ejecución del monitor del productor / consumidor
        /// </summary>
        protected ThreadRun OnMonitorizacion;
        #endregion
    }

    /// <summary>
    /// Delegado que de la ejecución del thread
    /// </summary>
    public delegate void ThreadRun(ref bool finalize);

    /// <summary>
    /// Enumerado de los posibles estados del productor / consumidor
    /// </summary>
    public enum EstadoProductorConsumidor
    {
        /// <summary>
        /// Estado inicial del thread
        /// </summary>
        Detenido = 0,

        /// <summary>
        /// En ejecución
        /// </summary>
        EnEjecucion = 1,

        /// <summary>
        /// En proceso de parada. La parada definitiva se realizará cuando se vacie la cola
        /// </summary>
        Deteniendo = 2
    }
}
