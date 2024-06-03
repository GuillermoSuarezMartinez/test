using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Diagnostics;

namespace Orbita.VAComun
{
    internal static class OColectorBasura
    {
        #region Atributo(s)
		/// <summary>
        /// Variable for continual checking in the 
        /// While loop in the WaitForFullGCProc method.
        /// </summary>
        private static bool checkForNotify = false;

        /// <summary>
        /// Thread de chequeo de notificación del garbage collector
        /// </summary>
        private static ThreadOrbita ThreadWaitForFullGC;
	    #endregion

        #region Definicion(es) de evento(s)
        /// <summary>
        /// Notificación de aproximación de recolección completa de memoria
        /// </summary>
        public static SimpleMethod OnFullGCApproachNotify;

        /// <summary>
        /// Notificación de recolección completa de memoria finalizada
        /// </summary>
        public static SimpleMethod OnFullGCCompleteEndNotify;
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public static void Constructror()
        {
            try
            {
                // Create a thread using WaitForFullGCProc.
                ThreadWaitForFullGC = new ThreadOrbita("ThreadWaitForFullGC", 10, ThreadPriority.BelowNormal);
                ThreadWaitForFullGC.OnEjecucion += WaitForFullGCProc;
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosSistema.GestionMemoria, "Constructor OColectorBasura", exception);
                Console.WriteLine("GC Notifications are not supported while concurrent GC is enabled.\n" + exception.Message);
            }
        }

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        public static void Destructor()
        {
            try
            {
                // Destroy a thread using WaitForFullGCProc.
                ThreadWaitForFullGC.OnEjecucion -= WaitForFullGCProc;
                ThreadWaitForFullGC = null;
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosSistema.GestionMemoria, "Destructor OColectorBasura", exception);
                Console.WriteLine("GC Notifications are not supported while concurrent GC is enabled.\n" + exception.Message);
            }
        }

        /// <summary>
        /// Inicialización de la clase
        /// </summary>
		public static void Inicializar()
        {             
            try
            {
                // Register for a notification. 
                GC.RegisterForFullGCNotification(10, 10);
                Console.WriteLine("Registered for GC notification.");

                // Iniciamos el thread
                checkForNotify = true;
                ThreadWaitForFullGC.Start();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosSistema.GestionMemoria, "Inicializar OColectorBasura", exception);
                Console.WriteLine("GC Notifications are not supported while concurrent GC is enabled.\n" + exception.Message);
            }
        }

        /// <summary>
        /// Inicialización de la clase
        /// </summary>
        public static void Finalizar()
        {
            try
            {
                checkForNotify = false;
                GC.CancelFullGCNotification();
                ThreadWaitForFullGC.Stop(1000);
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosSistema.GestionMemoria, "Finalizar OColectorBasura", exception);
                Console.WriteLine("GC Notifications are not supported while concurrent GC is enabled.\n" + exception.Message);
            }
        }


	    #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Método ejeutado en un thread que consulta el estado del recolector de basura
        /// </summary>
        /// <param name="finalize"></param>
        private static void WaitForFullGCProc(out bool finalize)
        {
            finalize = !checkForNotify;

            // Check for a notification of an approaching collection.
            GCNotificationStatus s = GC.WaitForFullGCApproach();
            if (s == GCNotificationStatus.Succeeded)
            {
                Console.WriteLine("GC Notification raised.");
                FullGCApproachNotify();
            }
            else if (s == GCNotificationStatus.Canceled)
            {
                //Console.WriteLine("GC Notification cancelled.");
                return;
            }
            else
            {
                // This can occur if a timeout period
                // is specified for WaitForFullGCApproach(Timeout) 
                // or WaitForFullGCComplete(Timeout)  
                // and the time out period has elapsed. 
                //Console.WriteLine("GC Notification not applicable.");
                return;
            }

            // Check for a notification of a completed collection.
            s = GC.WaitForFullGCComplete();
            if (s == GCNotificationStatus.Succeeded)
            {
                Console.WriteLine("GC Notifiction raised.");
                FullGCCompleteEndNotify();
            }
            else if (s == GCNotificationStatus.Canceled)
            {
                //Console.WriteLine("GC Notification cancelled.");
            }
            else
            {
                // Could be a time out.
                //Console.WriteLine("GC Notification not applicable.");
                return;
            }
        }

        private static void FullGCApproachNotify()
        {

            Console.WriteLine("Redirecting requests.");

            if (OnFullGCApproachNotify != null)
            {
                OnFullGCApproachNotify();
            }
        }

        private static void FullGCCompleteEndNotify()
        {

            if (OnFullGCCompleteEndNotify != null)
            {
                OnFullGCCompleteEndNotify();
            }
        }
	    #endregion
    }
}
