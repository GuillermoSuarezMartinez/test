//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 05-11-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
	using System.Drawing;
	using System.IO;
	using System.Text;
	using System.Threading;
	using System.Net;
using System.Windows.Forms;
using Orbita.VAComun;

namespace Orbita.VAHardware
{
    /// <summary>
    /// MJPEGSource - MJPEG stream support
    /// </summary>
    internal class MJPEGSource : IVideoSource
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
        private bool useSeparateConnectionGroup = true;

        /// <summary>
        /// Thread de captura de datos
        /// </summary>
        private Thread thread = null;

        /// <summary>
        /// Evento de stop del thread de captura de datos
        /// </summary>
        private ManualResetEvent stopEvent = null;

        /// <summary>
        /// Evento de recarga del thread de captura de datos
        /// </summary>
        private ManualResetEvent reloadEvent = null;

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
        /// VideoSource property
        /// </summary>
        public string VideoSource
        {
            get { return source; }
            set
            {
                source = value;
                // signal to reload
                if (thread != null)
                    reloadEvent.Set();
            }
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
        /// Constructor
        /// </summary>
        public MJPEGSource()
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
                reloadEvent = new ManualResetEvent(false);

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
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Thread entry point
        /// </summary>
        private void WorkerThread()
        {
            byte[] buffer = new byte[bufSize];	// buffer to read stream

            while (true)
            {
                try
                {
                    // reset reload event
                    reloadEvent.Reset();

                    //HttpWebRequest req = null;
                    WebRequest req = null;
                    WebResponse resp = null;
                    Stream stream = null;
                    byte[] delimiter = null;
                    byte[] delimiter2 = null;
                    byte[] boundary = null;
                    int boundaryLen, delimiterLen = 0, delimiter2Len = 0;
                    int read, todo = 0, total = 0, pos = 0, align = 1;
                    int start = 0, stop = 0;

                    // align
                    //  1 = searching for image start
                    //  2 = searching for image end
                    try
                    {
                        // create request
                        //req = (HttpWebRequest)WebRequest.Create(source);
                        req = WebRequest.Create(source);
                        req.Method = "GET";
                        req.Timeout = 15000;
                        // set login and password
                        if ((login != null) && (password != null) && (login != ""))
                            req.Credentials = new NetworkCredential(login, password);
                        // set connection group name
                        if (useSeparateConnectionGroup)
                            req.ConnectionGroupName = GetHashCode().ToString();
                        // get response
                        resp = req.GetResponse();

                        // check content type
                        string ct = resp.ContentType;
                        if (ct.IndexOf("multipart/x-mixed-replace") == -1)
                            throw new ApplicationException("Invalid URL");

                        // get boundary
                        ASCIIEncoding encoding = new ASCIIEncoding();
                        boundary = encoding.GetBytes(ct.Substring(ct.IndexOf("boundary=", 0) + 9));
                        boundaryLen = boundary.Length;

                        // get response stream
                        stream = resp.GetResponseStream();
                        stream.ReadTimeout = this.readTimeOutMs;

                        // loop
                        while ((!stopEvent.WaitOne(0, true)) && (!reloadEvent.WaitOne(0, true)))
                        {
                            // check total read
                            if (total > bufSize - readSize)
                            {
                                total = pos = todo = 0;
                            }

                            // read next portion from stream
                            if ((read = stream.Read(buffer, total, readSize)) == 0)
                                throw new ApplicationException();

                            total += read;
                            todo += read;

                            // increment received bytes counter
                            bytesReceived += read;

                            // does we know the delimiter ?
                            if (delimiter == null)
                            {
                                // find boundary
                                pos = ByteArrayUtils.Find(buffer, boundary, pos, todo);

                                if (pos == -1)
                                {
                                    // was not found
                                    todo = boundaryLen - 1;
                                    pos = total - todo;
                                    continue;
                                }

                                todo = total - pos;

                                if (todo < 2)
                                    continue;

                                // check new line delimiter type
                                if (buffer[pos + boundaryLen] == 10)
                                {
                                    delimiterLen = 2;
                                    delimiter = new byte[2] { 10, 10 };
                                    delimiter2Len = 1;
                                    delimiter2 = new byte[1] { 10 };
                                }
                                else
                                {
                                    delimiterLen = 4;
                                    delimiter = new byte[4] { 13, 10, 13, 10 };
                                    delimiter2Len = 2;
                                    delimiter2 = new byte[2] { 13, 10 };
                                }

                                pos += boundaryLen + delimiter2Len;
                                todo = total - pos;
                            }

                            // search for image
                            if (align == 1)
                            {
                                start = ByteArrayUtils.Find(buffer, delimiter, pos, todo);
                                if (start != -1)
                                {
                                    // found delimiter
                                    start += delimiterLen;
                                    pos = start;
                                    todo = total - pos;
                                    align = 2;
                                }
                                else
                                {
                                    // delimiter not found
                                    todo = delimiterLen - 1;
                                    pos = total - todo;
                                }
                            }

                            // search for image end
                            while ((align == 2) && (todo >= boundaryLen))
                            {
                                stop = ByteArrayUtils.Find(buffer, boundary, pos, todo);
                                if (stop != -1)
                                {
                                    pos = stop;
                                    todo = total - pos;

                                    // increment frames counter
                                    framesReceived++;

                                    // image at stop
                                    if (NewFrame != null)
                                    {
                                        Bitmap bmp = (Bitmap)Bitmap.FromStream(new MemoryStream(buffer, start, stop - start));
                                        // notify client
                                        NewFrame(this, new CameraEventArgs(bmp));
                                        // release the image
                                        bmp.Dispose();
                                        bmp = null;
                                    }

                                    // shift array
                                    pos = stop + boundaryLen;
                                    todo = total - pos;
                                    Array.Copy(buffer, pos, buffer, 0, todo);

                                    total = todo;
                                    pos = 0;
                                    align = 1;
                                }
                                else
                                {
                                    // delimiter not found
                                    todo = boundaryLen - 1;
                                    pos = total - todo;
                                }
                            }
                        }
                    }
                    catch (WebException ex)
                    {
                        //this.LanzarError(ex);
                        LogsRuntime.Error(ModulosHardware.Camaras, "Thread de MJPG", ex);
                        //System.Diagnostics.Debug.WriteLine("=============: " + ex.Message);
                        // wait for a while before the next try
                        Thread.Sleep(250);
                    }
                    catch (ApplicationException ex)
                    {
                        //this.LanzarError(ex);
                        LogsRuntime.Error(ModulosHardware.Camaras, "Thread de MJPG", ex);
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
                    catch (Exception ex)
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
                    //LogsRuntime.Error(ModulosHardware.Camaras, "Thread de MJPG", ex);
                    //System.Diagnostics.Debug.WriteLine("=============: " + ex.Message);
                }
            }
        } 

        /// <summary>
        /// Free resources
        /// </summary>
        private void Free()
        {
            thread = null;

            // release events
            stopEvent.Close();
            stopEvent = null;
            reloadEvent.Close();
            reloadEvent = null;
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