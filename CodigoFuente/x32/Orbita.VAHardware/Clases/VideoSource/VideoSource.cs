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
using Orbita.VAComun;

namespace Orbita.VAHardware
{
    /// <summary>
    /// IVideoSource interface
    /// </summary>
    public interface IVideoSource
    {
        #region Declaración de eventos
        /// <summary>
        /// New frame event - notify client about the new frame
        /// </summary>
        event CameraEventHandler NewFrame;

        /// <summary>
        /// Error en la adquisición
        /// </summary>
        event CameraErrorEventHandler OnCameraError;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Video source property
        /// </summary>
        string VideoSource { get; set; }

        /// <summary>
        /// Login property
        /// </summary>
        string Login { get; set; }

        /// <summary>
        /// Password property
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// FramesReceived property
        /// get number of frames the video source received from the last
        /// access to the property
        /// </summary>
        int FramesReceived { get; }

        /// <summary>
        /// BytesReceived property
        /// get number of bytes the video source received from the last
        /// access to the property
        /// </summary>
        int BytesReceived { get; }

        /// <summary>
        /// UserData property
        /// allows to associate user data with an object
        /// </summary>
        object UserData { get; set; }

        /// <summary>
        /// Get state of video source
        /// </summary>
        bool Running { get; } 

        /// <summary>
        /// Timeout en milisegundos de la lectura
        /// </summary>
        int ReadTimeOutMs { get; set; }
        #endregion

        #region Métodos
        /// <summary>
        /// Start receiving video frames
        /// </summary>
        void Start();

        /// <summary>
        /// Stop receiving video frames
        /// </summary>
        void SignalToStop();

        /// <summary>
        /// Wait for stop
        /// </summary>
        void WaitForStop();

        /// <summary>
        /// Stop work
        /// </summary>
        void Stop(); 
        #endregion
    }

    /// <summary>
    /// Enumerado con la lista de origenes de video
    /// </summary>
    public enum TipoOrigenVideo
    {
        /// <summary>
        /// Video con formato Motion JPEG
        /// </summary>
        [StringValue("Motion JPEG")]
        MJPG = 1,

        /// <summary>
        /// Secuencia de JPEG
        /// </summary>
        [StringValue("JPEG")]
        JPG = 2,
    }
}
