using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Net;
using Orbita.VAComun;
using System.Windows.Forms;

namespace Orbita.VAHardware
{
    /// <summary>
	/// JPEGSource - JPEG downloader
	/// </summary>
	public class JPEGSource : IVideoSource
	{
        #region Constante(s)
        /// <summary>
        /// Tamaño del buffer
        /// </summary>
        private const int bufSize = 512 * 1024;	// buffer size

        /// <summary>
        /// Tamaño de la lectura
        /// </summary>
        private const int readSize = 1024;		// portion size to read
        #endregion

        #region Atributo(s)
        /// <summary>
        /// Cadena de conexión con el origen del video
        /// </summary>
        private string source;

        /// <summary>
        /// Usuario
        /// </summary>
        private string login = null;

        /// <summary>
        /// Constraseña
        /// </summary>
        private string password = null;

        /// <summary>
        /// Datos del usuario
        /// </summary>
        private object userData = null;

        /// <summary>
        /// Frames
        /// </summary>
        private int framesReceived;

        /// <summary>
        /// Bytes recibidos
        /// </summary>
        private int bytesReceived;

        /// <summary>
        /// Uso de grupo de conexión separada
        /// </summary>
        private bool useSeparateConnectionGroup = false;

        /// <summary>
        /// Prevenir caché
        /// </summary>
		private bool	preventCaching = false;

        /// <summary>
        /// frame interval in miliseconds
        /// </summary>
		private int		frameInterval = 0;		

        /// <summary>
        /// Thread de captura de datos
        /// </summary>
        private Thread thread = null;

        /// <summary>
        /// Evento de stop del thread de captura de datos
        /// </summary>
        private ManualResetEvent stopEvent = null;

        /// <summary>
        /// Timeout en milisegundos de la lectura
        /// </summary>
        private int readTimeOutMs = 1000;
        #endregion

        #region Definicion(es) de evento(s)
        /// <summary>
        /// Evento de nuevo frame
        /// </summary>
        public event CameraEventHandler NewFrame;

        /// <summary>
        /// Error de adquisición
        /// </summary>
        public event CameraErrorEventHandler OnCameraError;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// SeparateConnectioGroup property indicates to open WebRequest in separate connection group
        /// </summary>
        public bool SeparateConnectionGroup
        {
            get { return useSeparateConnectionGroup; }
            set { useSeparateConnectionGroup = value; }
        }
        /// <summary>
        /// PreventCaching property
        /// If the property is set to true, we are trying to prevent caching
        /// appneding fake parameter to URL. It's needed is client is behind
        /// proxy server.
        /// </summary>
        public bool PreventCaching
        {
            get { return preventCaching; }
            set { preventCaching = value; }
        }
        /// <summary>
        /// FrameInterval property - interval between frames
        /// If the property is set 100, than the source will produce 10 frames
        /// per second if it possible
        /// </summary>
        public int FrameInterval
        {
            get { return frameInterval; }
            set { frameInterval = value; }
        }
        /// <summary>
        /// VideoSource property
        /// </summary>
        public virtual string VideoSource
        {
            get { return source; }
            set { source = value; }
        }
        /// <summary>
        /// Login property
        /// </summary>
        public string Login
        {
            get { return login; }
            set { login = value; }
        }
        /// <summary>
        /// Password property
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        /// <summary>
        /// FramesReceived property
        /// </summary>
        public int FramesReceived
        {
            get
            {
                int frames = framesReceived;
                framesReceived = 0;
                return frames;
            }
        }
        /// <summary>
        /// BytesReceived property
        /// </summary>
        public int BytesReceived
        {
            get
            {
                int bytes = bytesReceived;
                bytesReceived = 0;
                return bytes;
            }
        }
        /// <summary>
        /// UserData property
        /// </summary>
        public object UserData
        {
            get { return userData; }
            set { userData = value; }
        }
        /// <summary>
        /// Get state of the video source thread
        /// </summary>
        public bool Running
        {
            get
            {
                if (thread != null)
                {
                    if (thread.Join(0) == false)
                        return true;

                    // the thread is not running, so free resources
                    Free();
                }
                return false;
            }
        }
        /// <summary>
        /// Timeout en milisegundos de la lectura
        /// </summary>
        public int ReadTimeOutMs
        {
            get { return readTimeOutMs; }
            set { readTimeOutMs = value; }
        }
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public JPEGSource()
        {
        } 
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Start work
        /// </summary>
        public void Start()
        {
            if (thread == null)
            {
                framesReceived = 0;
                bytesReceived = 0;

                // create events
                stopEvent = new ManualResetEvent(false);

                // create and start new thread
                thread = new Thread(new ThreadStart(WorkerThread));
                thread.Name = source;
                thread.Start();
            }
        }

        /// <summary>
        /// Signal thread to stop work
        /// </summary>
        public void SignalToStop()
        {
            // stop thread
            if (thread != null)
            {
                // signal to stop
                stopEvent.Set();
            }
        }

        /// <summary>
        /// Wait for thread stop
        /// </summary>
        public void WaitForStop()
        {
            if (thread != null)
            {
                // wait for thread stop
                //thread.Join();
                bool cerrado = false;
                while (!cerrado)
                {
                    cerrado = this.thread.Join(10);
                    Application.DoEvents();
                }

                Free();
            }
        }

        /// <summary>
        /// Abort thread
        /// </summary>
        public void Stop()
        {
            if (this.Running)
            {
                thread.Abort();
                WaitForStop();
            }
        }

        /// <summary>
        /// Thread entry point
        /// </summary>
        public void WorkerThread()
        {
            byte[] buffer = new byte[bufSize];	// buffer to read stream
            HttpWebRequest req = null;
            WebResponse resp = null;
            Stream stream = null;
            Random rnd = new Random((int)DateTime.Now.Ticks);
            DateTime start;
            TimeSpan span;

            while (true)
            {
                try
                {
                    int read, total = 0;

                    try
                    {
                        start = DateTime.Now;

                        // create request
                        if (!preventCaching)
                        {
                            req = (HttpWebRequest)WebRequest.Create(source);
                        }
                        else
                        {
                            req = (HttpWebRequest)WebRequest.Create(source + ((source.IndexOf('?') == -1) ? '?' : '&') + "fake=" + rnd.Next().ToString());
                        }
                        // set login and password
                        if ((login != null) && (password != null) && (login != ""))
                            req.Credentials = new NetworkCredential(login, password);
                        // set connection group name
                        if (useSeparateConnectionGroup)
                            req.ConnectionGroupName = GetHashCode().ToString();
                        // get response
                        resp = req.GetResponse();

                        // get response stream
                        stream = resp.GetResponseStream();
                        stream.ReadTimeout = this.readTimeOutMs;

                        // loop
                        while (!stopEvent.WaitOne(0, true))
                        {
                            // check total read
                            if (total > bufSize - readSize)
                            {
                                total = 0;
                            }

                            // read next portion from stream
                            if ((read = stream.Read(buffer, total, readSize)) == 0)
                                break;

                            total += read;

                            // increment received bytes counter
                            bytesReceived += read;
                        }

                        if (!stopEvent.WaitOne(0, true))
                        {
                            // increment frames counter
                            framesReceived++;

                            // image at stop
                            if (NewFrame != null)
                            {
                                Bitmap bmp = (Bitmap)Bitmap.FromStream(new MemoryStream(buffer, 0, total));
                                // notify client
                                NewFrame(this, new CameraEventArgs(bmp));
                                // release the image
                                bmp.Dispose();
                                bmp = null;
                            }
                        }

                        // wait for a while ?
                        if (frameInterval > 0)
                        {
                            // times span
                            span = DateTime.Now.Subtract(start);
                            // miliseconds to sleep
                            int msec = frameInterval - (int)span.TotalMilliseconds;

                            while ((msec > 0) && (stopEvent.WaitOne(0, true) == false))
                            {
                                // sleeping ...
                                Thread.Sleep((msec < 100) ? msec : 100);
                                msec -= 100;
                            }
                        }
                    }
                    catch (WebException ex)
                    {
                        //this.LanzarError(ex);
                        LogsRuntime.Error(ModulosHardware.Camaras, "Thread de JPG", ex);
                        //System.Diagnostics.Debug.WriteLine("=============: " + ex.Message);
                        // wait for a while before the next try
                        Thread.Sleep(250);
                    }
                    catch (ApplicationException ex)
                    {
                        //this.LanzarError(ex);
                        LogsRuntime.Error(ModulosHardware.Camaras, "Thread de JPG", ex);
                        //System.Diagnostics.Debug.WriteLine("=============: " + ex.Message);
                        // wait for a while before the next try
                        Thread.Sleep(250);
                    }
                    catch (ThreadAbortException ex)
                    {
                        //this.LanzarError(ex);
                        //LogsRuntime.Error(ModulosHardware.Camaras, "Thread de MJPG", ex);
                        //System.Diagnostics.Debug.WriteLine("=============: " + ex.Message);
                    }
                    finally
                    {
                        // abort request
                        if (req != null)
                        {
                            req.Abort();
                            req = null;
                        }
                        // close response stream
                        if (stream != null)
                        {
                            stream.Close();
                            stream = null;
                        }
                        // close response
                        if (resp != null)
                        {
                            resp.Close();
                            resp = null;
                        }
                    }

                    // need to stop ?
                    if (stopEvent.WaitOne(0, true))
                        break;
                }
                catch (Exception ex)
                {
                    //this.LanzarError(ex);
                    //LogsRuntime.Error(ModulosHardware.Camaras, "Thread de JPG", ex);
                    //System.Diagnostics.Debug.WriteLine("=============: " + ex.Message);
                }
            }
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Free resources
        /// </summary>
        private void Free()
        {
            thread = null;

            // release events
            stopEvent.Close();
            stopEvent = null;
        }

        /// <summary>
        /// Lanza el evetno de error
        /// </summary>
        /// <param name="exception"></param>
        private void LanzarError(Exception exception)
        {
            if (this.OnCameraError != null)
            {
                CameraErrorEventArgs cameraErrorEventArgs = new CameraErrorEventArgs(exception);
                this.OnCameraError(this, cameraErrorEventArgs);
            }
        }
        #endregion
	}
}

