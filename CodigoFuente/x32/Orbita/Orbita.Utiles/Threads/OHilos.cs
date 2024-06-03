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
using System.Collections.Generic;
using System.Threading;
using System;
namespace Orbita.Utiles
{
    /// <summary>
    /// Colección de hilos.
    /// </summary>
    public class OHilos
    {
        #region Atributo(s)
        /// <summary>
        /// Colección de hilos.
        /// </summary>
        List<OHilo> _lista;
        #region Delegado(s)
        /// <summary>
        /// Evento del método adición.
        /// </summary>
        public event ManejadorEvento OnDespuesAdicionar;
        #endregion
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Inicializar una nueva instancia de la clase OHilos.
        /// </summary>
        public OHilos()
        {
            // Crear la colección 'List<>'.
            this._lista = new List<OHilo>();
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Adicionar el hilo a la colección.
        /// </summary>
        /// <param name="metodo">Proceso de ejecución del hilo.</param>
        /// <param name="iniciar">Indica si se inicia el hilo tras su creación.</param>
        /// <returns>Hilo creado.</returns>
        public virtual OHilo Add(ThreadStart metodo, bool iniciar)
        {
            // Bloquear esta propiedad por si se usan Threads (17/Jul/10)
            lock (this._lista)
            {
                // Crear un nuevo objeto hilo.
                OHilo hilo = new OHilo(metodo, iniciar);

                // Añadir a la colección.
                this._lista.Add(hilo);

                // Ejecutar evento de adición a la colección de hilos.
                if (OnDespuesAdicionar != null)
                {
                    OnDespuesAdicionar(hilo, new OEventArgs(Enum.GetName(typeof(Accion), Accion.Inicializado)));
                }
                return hilo;
            }
        }
        /// <summary>
        /// Adicionar el hilo a la colección.
        /// </summary>
        /// <param name="metodo">Proceso de ejecución del hilo.</param>
        /// <param name="descripcion">Descripción del hilo.</param>
        /// <param name="iniciar">Indica si se inicia el hilo tras su creación.</param>
        /// <returns>Hilo creado.</returns>
        public virtual OHilo Add(ThreadStart metodo, string descripcion, bool iniciar)
        {
            // Bloquear esta propiedad por si se usan Threads (17/Jul/10)
            lock (this._lista)
            {
                // Crear un nuevo objeto hilo.
                OHilo hilo = new OHilo(metodo, descripcion, iniciar);

                // Añadir a la colección.
                this._lista.Add(hilo);

                // Ejecutar evento de adición a la colección de hilos.
                if (OnDespuesAdicionar != null)
                {
                    OnDespuesAdicionar(hilo, new OEventArgs(Enum.GetName(typeof(Accion), Accion.Inicializado)));
                }
                return hilo;
            }
        }
        /// <summary>
        /// Adicionar el hilo a la colección.
        /// </summary>
        /// <param name="metodo">Proceso de ejecución del hilo.</param>
        /// <param name="prioridad">Prioridad de hilo.</param>
        /// <param name="segundoPlano">Indica si se ejecuta el hilo en segundo plano.</param>
        /// <param name="iniciar">Indica si se inicia el hilo tras su creación.</param>
        /// <returns>Hilo creado.</returns>
        public virtual OHilo Add(ThreadStart metodo, ThreadPriority prioridad, bool segundoPlano, bool iniciar)
        {
            // Bloquear esta propiedad por si se usan Threads (17/Jul/10)
            lock (this._lista)
            {
                // Crear un nuevo objeto hilo.
                OHilo hilo = new OHilo(metodo, prioridad, segundoPlano, iniciar);

                // Añadir a la colección.
                this._lista.Add(hilo);

                // Ejecutar evento de adición a la colección de hilos.
                if (OnDespuesAdicionar != null)
                {
                    OnDespuesAdicionar(hilo, new OEventArgs(Enum.GetName(typeof(Accion), Accion.Inicializado)));
                }
                return hilo;
            }
        }
        /// <summary>
        /// Adicionar el hilo a la colección.
        /// </summary>
        /// <param name="metodo">Proceso de ejecución del hilo.</param>
        /// <param name="nombre">Nombre del hilo.</param>
        /// <param name="prioridad">Prioridad de hilo.</param>
        /// <param name="segundoPlano">Indica si se ejecuta el hilo en segundo plano.</param>
        /// <param name="iniciar">Indica si se inicia el hilo tras su creación.</param>
        /// <returns>Hilo creado.</returns>
        public virtual OHilo Add(ThreadStart metodo, string nombre, ThreadPriority prioridad, bool segundoPlano, bool iniciar)
        {
            // Bloquear esta propiedad por si se usan Threads (17/Jul/10)
            lock (this._lista)
            {
                // Crear un nuevo objeto hilo.
                OHilo hilo = new OHilo(metodo, nombre, prioridad, segundoPlano, iniciar);

                // Añadir a la colección.
                this._lista.Add(hilo);

                // Ejecutar evento de adición a la colección de hilos.
                if (OnDespuesAdicionar != null)
                {
                    OnDespuesAdicionar(hilo, new OEventArgs(Enum.GetName(typeof(Accion), Accion.Inicializado)));
                }
                return hilo;
            }
        }
        /// <summary>
        /// Adicionar el hilo a la colección.
        /// </summary>
        /// <param name="metodo">Proceso de ejecución del hilo.</param>
        /// <param name="nombre">Nombre del hilo.</param>
        /// <param name="descripcion">Descripción del hilo.</param>
        /// <param name="prioridad">Prioridad de hilo.</param>
        /// <param name="segundoPlano">Indica si se ejecuta el hilo en segundo plano.</param>
        /// <param name="iniciar">Indica si se inicia el hilo tras su creación.</param>
        /// <returns>Hilo creado.</returns>
        public virtual OHilo Add(ThreadStart metodo, string nombre, string descripcion, ThreadPriority prioridad, bool segundoPlano, bool iniciar)
        {
            // Bloquear esta propiedad por si se usan Threads (17/Jul/10)
            lock (this._lista)
            {
                // Crear un nuevo objeto hilo.
                OHilo hilo = new OHilo(metodo, nombre, descripcion, prioridad, segundoPlano, iniciar);

                // Añadir a la colección.
                this._lista.Add(hilo);

                // Ejecutar evento de adición a la colección de hilos.
                if (OnDespuesAdicionar != null)
                {
                    OnDespuesAdicionar(hilo, new OEventArgs(Enum.GetName(typeof(Accion), Accion.Inicializado)));
                }
                return hilo;
            }
        }
        /// <summary>
        /// Recorrer la colección e iniciar todos los hilos que contenga.
        /// </summary>
        public void Iniciar()
        {
            if (this._lista != null)
            {
                for (int i = 0; i < this._lista.Count; i++)
                {
                    this._lista[i].Iniciar();
                }
            }
        }
        /// <summary>
        /// Recorrer la colección y suspender todos los hilos que contenga.
        /// </summary>
        public void Suspender()
        {
            if (this._lista != null)
            {
                for (int i = 0; i < this._lista.Count; i++)
                {
                    this._lista[i].Suspender();
                }
            }
        }
        /// <summary>
        /// Recorrer la colección y reanudar todos los hilos que contenga.
        /// </summary>
        public void Reanudar()
        {
            if (this._lista != null)
            {
                for (int i = 0; i < this._lista.Count; i++)
                {
                    this._lista[i].Reanudar();
                }
            }
        }
        /// <summary>
        /// Recorrer la colección y abortar todos los hilos que contenga.
        /// </summary>
        public void Destruir()
        {
            if (this._lista != null)
            {
                for (int i = 0; i < this._lista.Count; i++)
                {
                    this._lista[i].Terminar();
                }
            }
        }
        /// <summary>
        /// Contar el número de hilos de la colección.
        /// </summary>
        public int Contar()
        {
            int resultado = 0;
            if (this._lista != null)
            {
                resultado = this._lista.Count;
            }
            return resultado;
        }
        #endregion
    }
}
