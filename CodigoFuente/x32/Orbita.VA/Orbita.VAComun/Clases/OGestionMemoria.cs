//***********************************************************************
// Assembly         : Orbita.VAComun
// Author           : aibañez
// Created          : 16-11-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections.Generic;

namespace Orbita.VAComun
{
    /// <summary>
    /// Clase estática encargada de monitorizar la creación y destrucción de objetos de origen propio que ocupan gran cantidad de memoria y de creación frecuente.
    /// </summary>
    public static class OGestionMemoriaManager
    {
        #region Atributo(s)
        /// <summary>
        /// Diccionario de objetos grandes. Clave: Código Hash del objeto. Valor: Triplete de Tipo y código inerno
        /// </summary>
        private static Dictionary<int, OPair<Type, string>> ListaObjetos;

        /// <summary>
        /// Objeto de bloqueo. Utilizado para el bloqueo multihilo
        /// </summary>
        private static object BlockObject = new object();
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Devuelve el número de objetos residentes en memoria
        /// </summary>
        /// <returns></returns>
        public static int Count
        {
            get
            {
                int resultado = 0;
                if (ListaObjetos != null)
                {
                    lock (BlockObject)
                    {
                        resultado = ListaObjetos.Count;
                    }
                }
                return resultado;
            }
        }
        #endregion

        #region Método(s) publico(s)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
		public static void Constructor()
        {
            ListaObjetos = new Dictionary<int, OPair<Type, string>>();
            BlockObject = new object();
        }

        /// <summary>
        /// Destructor de la clase
        /// </summary>
        public static void Destructor()
        {
            ListaObjetos.Clear();
            ListaObjetos = null;
            BlockObject = null;
        }

        /// <summary>
        /// Inicializa las variables necesarias para el funcionamiento de los cronómetros
        /// </summary>
        public static void Inicializar()
        {
            // Creamos el cronómetro
            OCronometrosManager.NuevoCronometro("GarbageCollector", "LiberadorMemoria", "Liberación de memoria");
        }

        /// <summary>
        /// Finaliza la ejecución
        /// </summary>
        public static void Finalizar()
        {
        }

        /// <summary>
        /// Consulta la información de la lista
        /// </summary>
        public static Dictionary<int, OPair<Type, string>> GetListaObjetos()
        {
            Dictionary<int, OPair<Type, string>> resultado = new Dictionary<int, OPair<Type, string>>();

            if (ListaObjetos != null)
            {
                lock (BlockObject)
                {
                    foreach (KeyValuePair<int, OPair<Type, string>> keyValuePair in ListaObjetos)
                    {
                        resultado.Add(keyValuePair.Key, new OPair<Type, string>(keyValuePair.Value.First, keyValuePair.Value.Second));
                    }
                }
            }
            return resultado;
        }

        /// <summary>
        /// Devuelve el estado de la lista de objetos
        /// </summary>
        /// <param name="textoFormateado">Texto válido para utilizar en la función string.Format.
        /// Debe contener {0}, {1}, {2} para ser válido</param>
        public static List<string> Resumen(string textoFormateado)
        {
            List<string> resultado = new List<string>();
            if (ListaObjetos != null)
            {
                lock (BlockObject)
                {
                    foreach (KeyValuePair<int, OPair<Type, string>> keyValuePair in ListaObjetos)
                    {
                        string registro = string.Format(textoFormateado, keyValuePair.Key, keyValuePair.Value.First, keyValuePair.Value.Second);
                        resultado.Add(registro);
                    }
                }
            }
            return resultado;
        }

        /// <summary>
        /// Registra en el log el estado de la lista de objetos
        /// </summary>
        /// <param name="textoFormateado">Texto válido para utilizar en la función string.Format.
        /// Debe contener {0}, {1}, {2} para ser válido</param>
        public static void RegistraEnLog(string textoFormateado)
        {
            if (ListaObjetos != null)
            {
                List<string> listaResumen = Resumen(textoFormateado);
                foreach (string textoResumen in listaResumen)
                {
                    OVALogsManager.Info(ModulosSistema.GestionMemoria, "LargeObjectsRuntime", textoResumen);
                }
            }
        }

        /// <summary>
        /// Añade al registro de objetos
        /// </summary>
        /// <param name="baseObject"></param>
        public static void Add(OLargeObjectBase baseObject)
        {
            if (ListaObjetos != null)
	        {
                lock (BlockObject)
                {
                    ListaObjetos.Add(baseObject.GetHashCode(), new OPair<Type, string>(baseObject.GetType(), baseObject.Codigo));
                }
	        }
        }

        /// <summary>
        /// Elimina del registro de objetos
        /// </summary>
        /// <param name="baseObject"></param>
        public static void Remove(OLargeObjectBase baseObject)
        {
            if (ListaObjetos != null)
            {
                lock (BlockObject)
                {
                    ListaObjetos.Remove(baseObject.GetHashCode());
                }
            }
        }

        /// <summary>
        /// Llamada al colector de basura para que llame a destructores y libere la memoria no usada.
        /// Cuando realizar la llamada:
        ///   - Es recomendable hacer una llamada a este método una vez por ciclo de la máquina de estados con el parámetro esperaFinalizacion a true (.
        ///   - También se debe llamar cuando terminamos el proceso de parada de la clase sistema con el parámetro esperaFinalizacion a true.
        ///   - Si 
        ///   - En caso de necesidad de llamarse más veces debería de ser después de una asignación a null con el parámetro esepraFinalización a false.
        /// </summary>
        /// <param name="esperaFinalizacion">Si verdadero espera a que finalice el proceso de liberación de memoria.
        /// Este proceso puede ser costoso, por lo que se recomienda utilizarlo en momentos de latencia</param>
        public static void ColectorBasura(bool esperaFinalizacion)
        {
            OCronometrosManager.Start("GarbageCollector");

            //Force garbage collection.
            GC.Collect();

            // Wait for all finalizers to complete before continuing.
            // Without this call to GC.WaitForPendingFinalizers, 
            // the worker loop below might execute at the same time 
            // as the finalizers.
            // With this call, the worker loop executes only after
            // all finalizers have been called.
            if (esperaFinalizacion)
            {
                GC.WaitForPendingFinalizers();
            }

            OCronometrosManager.Stop("GarbageCollector");
            OVALogsManager.Info(ModulosSistema.Comun, "Colector de basura", "Duración: " + OCronometrosManager.DuracionUltimaEjecucion("GarbageCollector").TotalMilliseconds.ToString());
        }

	    #endregion    
    }

    /// <summary>
    /// Patrón de diseño para una clase propia de creación frecuente y gran tamaño
    /// </summary>
    public abstract class OLargeObjectBase : IDisposable
    {
        #region Propiedades(s)
        /// <summary>
        /// Indica que se ha llamado al método dispose
        /// </summary>
        protected bool _Disposed = false;
        /// <summary>
        /// Indica que se ha llamado al método dispose
        /// </summary>
        public bool Disposed
        {
            get { return _Disposed; }
            set { _Disposed = value; }
        }

        /// <summary>
        /// Texto identificativo del objeto
        /// </summary>
        private string _Codigo;
        /// <summary>
        /// Texto identificativo del objeto
        /// </summary>
        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codigo"></param>
        public OLargeObjectBase(string codigo)
        {
            this.Codigo = codigo;
            OGestionMemoriaManager.Add(this);
        } 
        #endregion

        #region Destructor(s)
        /// <summary>
        /// Destructor de la clase
        /// </summary>
        ~OLargeObjectBase()
        {
            // Simply call Dispose(false).
            if (!this.Disposed)
            {
                Dispose(false);
            }
            OGestionMemoriaManager.Remove(this);
        }

        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Implementa IDisposable
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Método a implementar por las clases heredadas.
        /// Se utiliza para liberar recursos cuando el objeto va a ser destruido.
        /// This.Disposed es utilizado para saber si los recursos ya han sido liberados, 
        ///  por lo tanto ha de consultarse antes de realizar las instrucciones de liberación de recursos y
        ///  ha de actualizarse tras la finalización de la liberación de recursos o bien llamar al método base para que lo actualize él.
        ///  Ejemplo:
        ///     if (!Disposed)
        ///     {
        ///         if (disposing) // Llamada desde Dispose() --> Llamada explicita o mediante la utilización de la instrucción using
        ///         {
        ///             // Poner aquí llamadas a Dispose() de objetos propios
        ///         }
        ///         else // Llamada desde destructor
        ///         {
        ///         }
        ///         // Poner referencias a null
        ///         base.Dispose(disposing);
        ///     }
        /// </summary>
        /// <param name="disposing">Indica si la llamada proviene del méotod dispose (true) o del destructor de la clase(false)</param>
		protected virtual void Dispose(bool disposing)
        {
            if (!this.Disposed)
            {
                if (disposing)
                {
                }
                else
                {
                }
                Disposed = true;
            }
        }
	    #endregion    
    }
}
