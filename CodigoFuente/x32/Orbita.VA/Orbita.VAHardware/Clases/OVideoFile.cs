//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 03-02-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System.Threading;
using Orbita.VAComun;
using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using AForge.Video.FFMPEG;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Clase utilizada para cargar y guardar videos de disco
    /// </summary>
	public class OVideoFile
	{
        #region Atributo(s)
        /// <summary>
        /// Thread de consumición de imagenes para guardado en disco
        /// </summary>
        private OProductorConsumidorThread<OImage> ThreadConsumidor;

        /// <summary>
        /// Escritor de ficheros AVI
        /// </summary>
        //AVIWriter AVIWriter;
        private VideoFileWriter AVIWriter;

        /// <summary>
        /// Crónometro de la duración de la grabación
        /// </summary>
        private Stopwatch CronometroDuracion;

        /// <summary>
        /// Momento exacto de la realización de la primera captura del video.
        /// Se utiliza la diferencia de tiempo de las capturas con respecto a la primera para espaciar el video.
        /// </summary>
        private DateTime MomentoPrimeraCaptura;

        /// <summary>
        /// Momento exacto de la última captura realizada.
        /// Se utiliza la diferencia de tiempo con la última captura para verificar si se cumple el FrameRate.
        /// Si está por debajo del FrameRate especificado la imagen se desprecia
        /// </summary>
        private DateTime MomentoAnteriorCaptura;
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
        /// Ruta del archivo a guardar
        /// </summary>
        private string _Ruta;
        /// <summary>
        /// Ruta del archivo a guardar
        /// </summary>
        public string Ruta
        {
            get { return _Ruta; }
            set 
            { 
                this._Ruta = value;

                try
                {
                    string dir = Path.GetDirectoryName(value);
                    this._Valido = Path.IsPathRooted(dir);
                }
                catch (Exception exception)
                {
                    OVALogsManager.Error(ModulosHardware.Camaras, "Consultar la validez de la ruta: " + this.Codigo, exception);
                }
            }
        }

        /// <summary>
        /// Duración máxima de la grabación
        /// </summary>
        private TimeSpan _TiempoMaxGrabacion;
        /// <summary>
        /// Duración máxima de la grabación
        /// </summary>
        public TimeSpan TiempoMaxGrabacion
        {
            get { return _TiempoMaxGrabacion; }
            set { _TiempoMaxGrabacion = value; }
        }

        /// <summary>
        /// Indica el tamaño de la imagen a guardar
        /// </summary>
        private Size _Resolucion;
        /// <summary>
        /// Indica el tamaño de la imagen a guardar
        /// </summary>
        public Size Resolucion
        {
            get { return _Resolucion; }
            set { _Resolucion = value; }
        }

        /// <summary>
        /// Informa si la grabación está preparda para realizarse
        /// </summary>
        private bool _Valido;
        /// <summary>
        /// Informa si la grabación está preparda para realizarse
        /// </summary>
        public bool Valido
        {
            get { return _Valido; }
        }

        /// <summary>
        /// Intervalo entre capturas
        /// </summary>
        private TimeSpan _FrameInterval;
        /// <summary>
        /// Intervalo entre capturas
        /// </summary>
        public TimeSpan FrameInterval
        {
            get { return _FrameInterval; }
            set { _FrameInterval = value; }
        }

        /// <summary>
        /// Intervalo entre capturas
        /// </summary>
        public int FrameRate
        {
            get { return (int)Math.Ceiling(1000 / FrameInterval.TotalMilliseconds); }
        }

        /// <summary>
        /// Tasa de transferencia
        /// </summary>
        private int _BitRate;
        /// <summary>
        /// Tasa de transferencia
        /// </summary>
        public int BitRate
        {
            get { return _BitRate; }
            set { _BitRate = value; }
        }
        
        /// <summary>
        /// Codec utilizado para la codificación del video
        /// </summary>
        private VideoCodec _Codec;
        /// <summary>
        /// Codec utilizado para la codificación del video
        /// </summary>
        public VideoCodec Codec
        {
            get { return _Codec; }
            set { _Codec = value; }
        }
        
        /// <summary>
        /// Estado de la ejecución de la grabación de video
        /// </summary>
        public EstadoProductorConsumidor Estado
        {
            get { return this.ThreadConsumidor.Estado; }
        }
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OVideoFile(string codigo, Size resolucion, TimeSpan tiempoMaxGrabacion, double frameIntervalMs, int bitRate, VideoCodec codec)
        {
            this.Codigo = codigo;
            this._TiempoMaxGrabacion = tiempoMaxGrabacion;
            this.Resolucion = resolucion;
            frameIntervalMs = frameIntervalMs == 0 ? 1 : frameIntervalMs;
            this.FrameInterval = TimeSpan.FromMilliseconds(frameIntervalMs);
            this.Codec = codec;
            this.BitRate = bitRate;
            this.MomentoPrimeraCaptura = DateTime.Now;
            this.MomentoAnteriorCaptura = DateTime.Now;

            this.CronometroDuracion = new Stopwatch();
            this.AVIWriter = new VideoFileWriter();

            int tiempoSleep = (int)Math.Ceiling(frameIntervalMs / 2);
            this.ThreadConsumidor = new OProductorConsumidorThread<OImage>(codigo, tiempoSleep, ThreadPriority.Lowest, 20);
            this.ThreadConsumidor.CrearSuscripcionConsumidor(this.ConsumidorRun, true);
            this.ThreadConsumidor.CrearSuscripcionMonitorizacion(this.Monitorizacion, true);
            this.ThreadConsumidor.CrearSuscripcionFin(this.FinEjecucion, true);
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Añade una imagen a la cola de guardado del video
        /// </summary>
        /// <param name="imagen">Imagen a añadir al video</param>
        /// <returns>Verdadero si se ha podido añadir la imagen al video</returns>
        public bool Add(OImage imagen)
        {
            bool resultado = false;

            if ((this.Estado == EstadoProductorConsumidor.EnEjecucion) && (imagen is OImage))
            {
                DateTime momentoCapturaActual = imagen.MomentoCreacion;
                bool primeraCaptura = this.ThreadConsumidor.Total == 0;

                // Si se cumple el tiempo del frame rate especificado
                if (primeraCaptura)
                {
                    // Guardo el momento actual de captura
                    this.MomentoPrimeraCaptura = momentoCapturaActual;
                    resultado = true;
                }
                else
                {
                    TimeSpan difAnteriorCaptura = momentoCapturaActual - this.MomentoAnteriorCaptura;
                    if (difAnteriorCaptura > this.FrameInterval)
                    {
                        resultado = true;
                    }
                }

                if (resultado)
                {
                    // Guardo el momento actual de captura
                    this.MomentoAnteriorCaptura = momentoCapturaActual;

                    // Se procede al encolamiento de la imagen
                    OImage imagenAux = imagen.EscalarImagen(imagen, this.Resolucion.Width, this.Resolucion.Height);
                    resultado = this.ThreadConsumidor.Encolar(imagenAux);
                }
            }

            return resultado;
        }

        /// <summary>
        /// Inicia el guardado del video
        /// </summary>
        public bool Start()
        {
            bool resultado = false;

            try
            {
                if ((this.Estado == EstadoProductorConsumidor.Detenido) &&
                    this.Valido)
                {
                    this.AVIWriter.Open(this._Ruta, this.Resolucion.Width, this.Resolucion.Height, this.FrameRate, this.Codec, this.BitRate);

                    this.CronometroDuracion.Reset();
                    this.CronometroDuracion.Start();
                    this.ThreadConsumidor.Start();

                    resultado = true;
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(ModulosHardware.Camaras, "Inicio de la grabación de video: " + this.Codigo, exception);
            }

            return resultado;
        }

        /// <summary>
        /// Finaliza el guardado del video
        /// </summary>
        public void Stop()
        {
            try
            {
                if (this.Estado == EstadoProductorConsumidor.EnEjecucion)
                {
                    this.ThreadConsumidor.Stop();
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(ModulosHardware.Camaras, "Fin de la grabación de video: " + this.Codigo, exception);
            }
        }
        #endregion

        #region Evento(s)
        /// <summary>
        /// Ejecución del consumidor
        /// </summary>
        private void ConsumidorRun(OImage valor)
        {
            try
            {
                // Diferencia de tiempo entre la primera captura y la actual
                TimeSpan difTiempo = valor.MomentoCreacion - this.MomentoPrimeraCaptura;

                // Adición del nuevo frame
                this.AVIWriter.WriteVideoFrame(valor.ConvertToBitmap(), difTiempo);
            }
            catch (Exception exception)
            {
                OVALogsManager.Info(ModulosHardware.Camaras, "Añadir Frame al video: " + this.Codigo, exception);
            }
        }

        /// <summary>
        /// Monitorización de la ejecución
        /// </summary>
        /// <param name="finalize"></param>
        private void Monitorizacion(ref bool finalize)
        {
            finalize = false;

            if (((this.Estado == EstadoProductorConsumidor.Deteniendo) && (this.ThreadConsumidor.Count == 0)) || 
                (this.CronometroDuracion.Elapsed > this._TiempoMaxGrabacion))
            {
                finalize = true;
            }
        }

        /// <summary>
        /// Fin de la ejecución del consumidor
        /// </summary>
        private void FinEjecucion()
        {
            try
            {
                this.AVIWriter.Close();
                this.ThreadConsumidor.Clear();
                this.CronometroDuracion.Stop();
            }
            catch (Exception exception)
            {
                OVALogsManager.Info(ModulosHardware.Camaras, "Guardado en disco del video: " + this.Codigo, exception);
            }
        }
        #endregion
	}
}
