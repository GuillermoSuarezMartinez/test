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

namespace Orbita.VAComun
{
    /// <summary>
    /// Clase que guarda imagenes en un thread separado
    /// </summary>
    public static class AlmacenVisualRuntime
    {
        #region Atributo(s)
        /// <summary>
        ///  Clase que guarda imagenes en un thread separado
        /// </summary>
        private static AlmacenObjetosVisuales AlmacenObjetosVisuales;
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Construye los objetos
        /// </summary>
        public static void Constructor()
        {
            AlmacenObjetosVisuales = new AlmacenObjetosVisuales();
        }

        /// <summary>
        /// Destruye los campos de la clase
        /// </summary>
        public static void Destructor()
        {
            AlmacenObjetosVisuales.Dispose();
            AlmacenObjetosVisuales = null;
        }

        /// <summary>
        /// Se cargan los valores de la clase
        /// </summary>
        public static void Inicializar()
        {
            AlmacenObjetosVisuales.Inicializar();
        }

        /// <summary>
        /// Se realizan las acciones de finalización
        /// </summary>
        public static void Finalizar()
        {
            AlmacenObjetosVisuales.Finalizar();
        }

        /// <summary>
        /// Método que añade una determinada imagen a la cola de imagenes a ser guardadas
        /// </summary>
        /// <param name="ruta">Ruta donde se ha de guardar la imagen</param>
        /// <param name="imagen">Imagen a guardar</param>
        public static void GuardarImagen(string ruta, OrbitaImage imagen)
        {
            AlmacenObjetosVisuales.GuardarImagen(ruta, imagen);
        }

        /// <summary>
        /// Método que añade un determinado gráfico a la cola de graficos a ser guardados
        /// </summary>
        /// <param name="ruta">Ruta donde se ha de guardar el gráfico</param>
        /// <param name="grafico">Grafico a guardar</param>
        public static void GuardarGrafico(string ruta, OrbitaGrafico grafico)
        {
            AlmacenObjetosVisuales.GuardarGrafico(ruta, grafico);
        }

        #endregion
    }

    /// <summary>
    /// Clase que guarda imagenes en un thread separado
    /// </summary>
    public class AlmacenObjetosVisuales : IDisposable
    {
        #region Constante(s)
        /// <summary>
        /// Capacidad máxima de la cola
        /// </summary>
        private const int MaxCapacidadCola = 50;
        #endregion

        #region Atributo(s)
        /// <summary>
        /// Campo donde se guarda la cola de imágenes a guardar
        /// </summary>
        private Queue<KeyValuePair<string, OrbitaImage>> ColaImagenes;

        /// <summary>
        /// Campo donde se guarda la cola de gráficos a guardar
        /// </summary>
        private Queue<KeyValuePair<string, OrbitaGrafico>> ColaGraficos;

        /// <summary>
        /// Thread donde se ejecutará el guardado de imágenes o gráficos
        /// </summary>
        private ThreadOrbita Thread;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public AlmacenObjetosVisuales()
        {
            this.ColaImagenes = new Queue<KeyValuePair<string, OrbitaImage>>();
            this.ColaGraficos = new Queue<KeyValuePair<string, OrbitaGrafico>>();
            this.Thread = new ThreadOrbita("AlmacenObjetosVisuales", 50, ThreadPriority.Lowest);
            this.Thread.OnEjecucion += Ejecutar;
        }
        #endregion

        #region Método(s) privado(s)
        private void Ejecutar(out bool finalize)
        {
            try
            {
                if (this.ColaImagenes.Count > 0)
                {
                    KeyValuePair<string, OrbitaImage> pareja = new KeyValuePair<string, OrbitaImage>();

                    lock (this.ColaImagenes)
                    {
                        pareja = this.ColaImagenes.Dequeue();
                    }
                    OrbitaImage imagen = pareja.Value;
                    string ruta = pareja.Key;

                    imagen.Guardar(ruta);
                    LogsRuntime.Info(ModulosSistema.ImagenGraficos, "Thread Guardado", "Se procede a guardar la imagen (" + this.ColaImagenes.Count.ToString() + " elementos en la cola)");
                }

                if (this.ColaGraficos.Count > 0)
                {
                    KeyValuePair<string, OrbitaGrafico> pareja = new KeyValuePair<string, OrbitaGrafico>();

                    lock (this.ColaGraficos)
                    {
                        pareja = this.ColaGraficos.Dequeue();
                    }

                    OrbitaGrafico grafico = pareja.Value;
                    string ruta = pareja.Key;

                    grafico.Guardar(ruta);
                    LogsRuntime.Info(ModulosSistema.ImagenGraficos, "Thread Guardado", "Se procede a guardar el gráfico (" + this.ColaGraficos.Count.ToString() + " elementos en la cola)");
                }

                if ((this.ColaImagenes.Count > 10) || (this.ColaGraficos.Count > 10))
                {
                    this.Thread.ChangePriority(ThreadPriority.Normal);
                }
                else
                {
                    this.Thread.ChangePriority(ThreadPriority.Lowest);
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosSistema.ImagenGraficos, "Almacen", exception);
            }

            finalize = false;
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Inicialización de la clase
        /// </summary>
        public void Inicializar()
        {
            this.Thread.Start();
        }

        /// <summary>
        /// Finalización de la clase
        /// </summary>
        public void Finalizar()
        {
            this.Thread.Dispose(500);
        }

        /// <summary>
        /// Elimina la memoria asignada por el objeto.
        /// </summary>
        public void Dispose()
        {
            this.Thread = null;
        }

        /// <summary>
        /// Método que añade una determinada imagen a la cola de imagenes a ser guardadas
        /// </summary>
        /// <param name="ruta">Ruta donde se ha de guardar la imagen</param>
        /// <param name="imagen">Imagen a guardar</param>
        public void GuardarImagen(string ruta, OrbitaImage imagen)
        {
            // Se clona la imagen
            OrbitaImage imagenClonada = (OrbitaImage)imagen.Clone();

            // Se crea la pareja de ruta + imagen
            KeyValuePair<string, OrbitaImage> pareja = new KeyValuePair<string, OrbitaImage>(ruta, imagenClonada);

            // Se añade a la cola de imagenes a guardar
            lock (this.ColaImagenes)
            {
                if (this.ColaImagenes.Count < MaxCapacidadCola) // Si hay más de 50 no añadimos el elemento a guardar
                {
                    this.ColaImagenes.Enqueue(pareja);
                    LogsRuntime.Info(ModulosSistema.ImagenGraficos, "Guardado", "Se apila la imagen en la cola de guardado (" + this.ColaImagenes.Count.ToString() + " elementos en la cola)");
                }
                else
                {
                    LogsRuntime.Warning(ModulosSistema.ImagenGraficos, "Guardado", "Capacidad de la cola de guardado de imágenes sobrepasada (" + this.ColaImagenes.Count.ToString() + " elementos en la cola)");
                }
            }
        }

        /// <summary>
        /// Método que añade un determinado gráfico a la cola de graficos a ser guardados
        /// </summary>
        /// <param name="ruta">Ruta donde se ha de guardar el gráfico</param>
        /// <param name="grafico">Grafico a guardar</param>
        public void GuardarGrafico(string ruta, OrbitaGrafico grafico)
        {
            // Se clona el gráfico
            OrbitaGrafico graficoClonado = (OrbitaGrafico)grafico.Clone();

            // Se crea la pareja de ruta + gráfico
            KeyValuePair<string, OrbitaGrafico> pareja = new KeyValuePair<string, OrbitaGrafico>(ruta, graficoClonado);

            // Se añade a la cola de gráficos a guardar
            lock (this.ColaGraficos)
            {
                if (this.ColaGraficos.Count < MaxCapacidadCola) // Si hay más de 50 no añadimos el elemento a guardar
                {
                    this.ColaGraficos.Enqueue(pareja);
                    LogsRuntime.Info(ModulosSistema.ImagenGraficos, "Guardado", "Se apila el gráfico en la cola de guardado (" + this.ColaGraficos.Count.ToString() + " elementos en la cola)");
                }
                else
                {
                    LogsRuntime.Warning(ModulosSistema.ImagenGraficos, "Guardado", "Capacidad de la cola de guardado de gráficos sobrepasada (" + this.ColaImagenes.Count.ToString() + " elementos en la cola)");
                }
            }
        }
        #endregion
    }
}
