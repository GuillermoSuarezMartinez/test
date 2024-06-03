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
using System.Drawing.Imaging;

namespace Orbita.VAHardware
{
    /// <summary>
    /// NewFrame delegate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    internal delegate void CameraEventHandler(object sender, CameraEventArgs e);

    /// <summary>
    /// Camera event arguments
    /// </summary>
    internal class CameraEventArgs : EventArgs
    {
        #region Atributo(s)
        /// <summary>
        /// Bitmap
        /// </summary>
        private System.Drawing.Bitmap bmp;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Bitmap property
        /// </summary>
        public System.Drawing.Bitmap Bitmap
        {
            get { return bmp; }
        }
        #endregion

        #region Contructor(es)
        /// <summary>
        /// // Constructor de la clase
        /// </summary>
        /// <param name="bmp"></param>
        public CameraEventArgs(System.Drawing.Bitmap bmp)
        {
            this.bmp = bmp;
        }
        #endregion
    }

    /// <summary>
    /// Error delegate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    internal delegate void CameraErrorEventHandler(object sender, CameraErrorEventArgs e);

    /// <summary>
    /// Camera event arguments
    /// </summary>
    internal class CameraErrorEventArgs : EventArgs
    {
        #region Atributo(s)
        /// <summary>
        /// Bitmap
        /// </summary>
        private Exception _Exception;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Bitmap property
        /// </summary>
        public Exception Exception
        {
            get { return _Exception; }
        }
        #endregion

        #region Contructor(es)
        /// <summary>
        /// // Constructor de la clase
        /// </summary>
        /// <param name="bmp"></param>
        public CameraErrorEventArgs(Exception exception)
        {
            this._Exception = exception;
        }
        #endregion
    }
}
