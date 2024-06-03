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
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net;

namespace Orbita.VAHardware
{
	/// <summary>
	/// CaptureDevice - capture video from local device
	/// </summary>
    internal class CaptureDevice : IVideoSource
	{
        #region Atributo(s)
        /// <summary>
        /// Cadena de conexión con el origen del video
        /// </summary>
        private string source;

        /// <summary>
        /// Datos del usuario
        /// </summary>
        private object userData = null;

        /// <summary>
        /// Frames
        /// </summary>
        private int framesReceived;

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
        private int readTimeOutMs = 1000; // NUEVO

        /// <summary>
        /// Timeout en milisegundos de la lectura
        /// </summary>
        public int ReadTimeOutMs // NUEVO
        {
            get { return readTimeOutMs; }
            set { readTimeOutMs = value; }
        }

        #endregion

        #region Definicion(es) de evento(s)
        /// <summary>
        /// Evento de nuevo frame
        /// </summary>
        public event CameraEventHandler NewFrame;

        /// <summary>
        /// Error de adquisición
        /// </summary>
        public event CameraErrorEventHandler OnCameraError; // NUEVO 
        #endregion

        #region Propiedad(es)
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
            get { return null; }
            set { }
        }
        /// <summary>
        /// Password property
        /// </summary>
        public string Password
        {
            get { return null; }
            set { }
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
            get { return 0; }
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
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor
        /// </summary>
        public CaptureDevice()
        {
        } 
        #endregion

        #region Métodos públicos
        /// <summary>
        /// Start work
        /// </summary>
        public void Start()
        {
            if (thread == null)
            {
                framesReceived = 0;

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
                thread.Join();

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
                // WaitForStop();
            }
        } 
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Thread entry point
        /// </summary>
        private void WorkerThread()
        {
            // grabber
            Grabber grabber = new Grabber(this);

            // objects
            object graphObj = null;
            object sourceObj = null;
            object grabberObj = null;

            // interfaces
            IGraphBuilder graph = null;
            IBaseFilter sourceBase = null;
            IBaseFilter grabberBase = null;
            ISampleGrabber sg = null;
            IMediaControl mc = null;

            try
            {
                // Get type for filter graph
                Type srvType = Type.GetTypeFromCLSID(Clsid.FilterGraph);
                if (srvType == null)
                    throw new ApplicationException("Failed creating filter graph");

                // create filter graph
                graphObj = Activator.CreateInstance(srvType);
                graph = (IGraphBuilder)graphObj;

                // ----
                UCOMIBindCtx bindCtx = null;
                UCOMIMoniker moniker = null;
                int n = 0;

                // create bind context
                if (Win32Utils.CreateBindCtx(0, out bindCtx) == 0)
                {
                    // convert moniker`s string to a moniker
                    if (Win32Utils.MkParseDisplayName(bindCtx, source, ref n, out moniker) == 0)
                    {
                        // get device base filter
                        Guid filterId = typeof(IBaseFilter).GUID;
                        moniker.BindToObject(null, null, ref filterId, out sourceObj);

                        Marshal.ReleaseComObject(moniker);
                        moniker = null;
                    }
                    Marshal.ReleaseComObject(bindCtx);
                    bindCtx = null;
                }
                // ----

                if (sourceObj == null)
                    throw new ApplicationException("Failed creating device object for moniker");

                sourceBase = (IBaseFilter)sourceObj;

                // Get type for sample grabber
                srvType = Type.GetTypeFromCLSID(Clsid.SampleGrabber);
                if (srvType == null)
                    throw new ApplicationException("Failed creating sample grabber");

                // create sample grabber
                grabberObj = Activator.CreateInstance(srvType);
                sg = (ISampleGrabber)grabberObj;
                grabberBase = (IBaseFilter)grabberObj;

                // add source filter to graph
                graph.AddFilter(sourceBase, "source");
                graph.AddFilter(grabberBase, "grabber");

                // set media type
                AMMediaType mt = new AMMediaType();
                mt.majorType = MediaType.Video;
                mt.subType = MediaSubType.RGB24;
                sg.SetMediaType(mt);

                // connect pins
                if (graph.Connect(DSTools.GetOutPin(sourceBase, 0), DSTools.GetInPin(grabberBase, 0)) < 0)
                    throw new ApplicationException("Failed connecting filters");

                // get media type
                if (sg.GetConnectedMediaType(mt) == 0)
                {
                    VideoInfoHeader vih = (VideoInfoHeader)Marshal.PtrToStructure(mt.formatPtr, typeof(VideoInfoHeader));

                    grabber.Width = vih.BmiHeader.Width;
                    grabber.Height = vih.BmiHeader.Height;
                    mt.Dispose();
                }

                // render
                graph.Render(DSTools.GetOutPin(grabberBase, 0));

                //
                sg.SetBufferSamples(false);
                sg.SetOneShot(false);
                sg.SetCallback(grabber, 1);

                // window
                IVideoWindow win = (IVideoWindow)graphObj;
                win.put_AutoShow(false);
                win = null;


                // get media control
                mc = (IMediaControl)graphObj;

                // run
                mc.Run();

                while (!stopEvent.WaitOne(0, true))
                {
                    Thread.Sleep(100);
                }
                mc.StopWhenReady();
            }
            // catch any exceptions
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("----: " + e.Message);
            }
            // finalization block
            finally
            {
                // release all objects
                mc = null;
                graph = null;
                sourceBase = null;
                grabberBase = null;
                sg = null;

                if (graphObj != null)
                {
                    Marshal.ReleaseComObject(graphObj);
                    graphObj = null;
                }
                if (sourceObj != null)
                {
                    Marshal.ReleaseComObject(sourceObj);
                    sourceObj = null;
                }
                if (grabberObj != null)
                {
                    Marshal.ReleaseComObject(grabberObj);
                    grabberObj = null;
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
        }

        // new frame
        protected void OnNewFrame(Bitmap image)
        {
            framesReceived++;
            if ((!stopEvent.WaitOne(0, true)) && (NewFrame != null))
                NewFrame(this, new CameraEventArgs(image));
        } 
        #endregion

        #region Clase(s) internas
        /// <summary>
        /// Grabber
        /// </summary>
        private class Grabber : ISampleGrabberCB
        {
            #region Atributo(s)
            /// <summary>
            /// Dispositivo contenedor del grabber
            /// </summary>
            private CaptureDevice parent;

            /// <summary>
            /// Ancho, Alto de la imagen
            /// </summary>
            private int width, height; 
            #endregion

            #region Propiedad(es)
            /// <summary>
            /// Width property
            /// </summary>
            public int Width
            {
                get { return width; }
                set { width = value; }
            }
            /// <summary>
            /// Height property
            /// </summary>
            public int Height
            {
                get { return height; }
                set { height = value; }
            } 
            #endregion

            #region Constructor(es)
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="parent"></param>
            public Grabber(CaptureDevice parent)
            {
                this.parent = parent;
            } 
            #endregion

            #region Métodos públicos
            /// <summary>
            /// Ejemplo de CallBack
            /// </summary>
            /// <param name="SampleTime"></param>
            /// <param name="pSample"></param>
            /// <returns></returns>
            public int SampleCB(double SampleTime, IntPtr pSample)
            {
                return 0;
            }

            /// <summary>
            /// Callback method that receives a pointer to the sample buffer
            /// </summary>
            /// <param name="SampleTime"></param>
            /// <param name="pBuffer"></param>
            /// <param name="BufferLen"></param>
            /// <returns></returns>
            public int BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen)
            {
                // create new image
                System.Drawing.Bitmap img = new Bitmap(width, height, PixelFormat.Format24bppRgb);

                // lock bitmap data
                BitmapData bmData = img.LockBits(
                    new Rectangle(0, 0, width, height),
                    ImageLockMode.ReadWrite,
                    PixelFormat.Format24bppRgb);

                // copy image data
                int srcStride = bmData.Stride;
                int dstStride = bmData.Stride;

                int dst = bmData.Scan0.ToInt32() + dstStride * (height - 1);
                int src = pBuffer.ToInt32();

                for (int y = 0; y < height; y++)
                {
                    Win32Utils.memcpy(dst, src, srcStride);
                    dst -= dstStride;
                    src += srcStride;
                }

                // unlock bitmap data
                img.UnlockBits(bmData);

                // notify parent
                parent.OnNewFrame(img);

                // release the image
                img.Dispose();

                return 0;
            } 
            #endregion
        } 
        #endregion
	}
}
