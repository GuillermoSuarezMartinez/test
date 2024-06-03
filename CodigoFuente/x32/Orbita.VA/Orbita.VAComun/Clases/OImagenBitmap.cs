//***********************************************************************
// Assembly         : Orbita.VAComun
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 12-12-2012
// Description      : Corregido problema con With y Height
//                    Se completa informaciónd el código y la fecha de creación
//
// Last Modified By : aibañez
// Last Modified On : 16-11-2012
// Description      : Modificados métodos constructores y destructores para heredar de LargeObject
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Orbita.VAComun
{
    /// <summary>
    /// Imagen de tipo Bitmap de windows
    /// </summary>
    [Serializable]
    public class OImagenBitmap : OImage
    {
        #region Atributo(s)
        /// <summary>
        /// Propieadad a heredar donde se accede a la imagen
        /// </summary>
        public new Bitmap Image
        {
            get { return (Bitmap)_Image; }
            set { _Image = value; }
        }
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Tamaño en X de la imagen
        /// </summary>
        public override int Width
        {
            get 
            {
                int resultado = base.Width;
                if (this.Image is Bitmap)
                {
                    resultado = this.Image.Width;
                }
                return resultado; 
            }
        }

        /// <summary>
        /// Tamaño en Y de la imagen
        /// </summary>
        public override int Height
        {
            get
            {
                int resultado = base.Height;
                if (this.Image is Bitmap)
                {
                    resultado = this.Image.Height;
                }
                return resultado;
            }
        }
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OImagenBitmap()
            : this("BitmapImage")
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OImagenBitmap(string codigo)
            : base(codigo)
        {
            this.TipoImagen = OTipoImagen.Bitmap;
        }
        /// <summary>
        /// Constructor de la clase que además se encarga de liberar la memoria de la variable que se le pasa por parámetro
        /// </summary>
        /// <param name="imagenALiberar">Variable que se desea liberar de memoria</param>
        public OImagenBitmap(object imagen)
            : this("BitmapImage", imagen)
        {
        }
        /// <summary>
        /// Constructor de la clase que además se encarga de liberar la memoria de la variable que se le pasa por parámetro
        /// </summary>
        /// <param name="imagenALiberar">Variable que se desea liberar de memoria</param>
        public OImagenBitmap(string codigo, object imagen)
            : base(codigo, imagen)
        {
            this.TipoImagen = OTipoImagen.Bitmap;
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Método a implementar en las clases hijas. Elimina la memoria asignada por el objeto.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (this.Image != null)
                {
                    //if (disposing)
                    //{
                        //this.Image.Dispose(); // No se elimina porque otro objeto puede hacer referencia a ella
                    //}
                    this.Image = null;
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Clonado de la imagen
        /// </summary>
        public override object Clone()
        {
            OImagenBitmap imagenResultado = new OImagenBitmap();
            if (this._Image != null)
            {
                imagenResultado._Image = this.Image.Clone();
                imagenResultado.Codigo = this.Codigo;
                imagenResultado.MomentoCreacion = this.MomentoCreacion;
            }
            return imagenResultado;
        }

        /// <summary>
        /// Método a implementar en las clases hijas que indica que la imagen es válida
        /// </summary>
        /// <returns></returns>
        public override bool EsValida()
        {
            return base.EsValida();
        }

        /// <summary>
        /// Método a implementar en las clases hijas para guardar la imagen en un fichero de disco
        /// </summary>
        /// <param name="ruta">Ruta donde se ha de guardar la imagen</param>
        /// <returns>Verdadero si la ruta donde se ha de guardar el fichero es válida</returns>
        public override bool Guardar(string ruta)
        {
            if (base.Guardar(ruta))
            {
                this.Image.Save(ruta);

                return File.Exists(ruta);
            }
            return false;
        }

        /// <summary>
        /// Método a implementar en las clases hijas para cargar la imagen de un fichero de disco
        /// </summary>
        /// <param name="ruta">Ruta de donde se ha de cargar la imagen</param>
        /// <returns>Verdadero si el fichero que se ha de cargar existe</returns>
        public override bool Cargar(string ruta)
        {
            if (base.Cargar(ruta))
            {
                this.Image = (Bitmap)Bitmap.FromFile(ruta);
                this.MomentoCreacion = DateTime.Now;

                return this.EsValida();
            }
            return false;
        }

        /// <summary>
        /// Convierte la imagen de tipo bitmap al tipo actual
        /// </summary>
        /// <param name="imagenBitmap">Imagen de tipo bitmap desde el cual se va a importar la imagen</param>
        public override OImage ConvertFromBitmap(Bitmap imagenBitmap)
        {
            this.Image = imagenBitmap;
            return this;
        }

        /// <summary>
        /// Convierte la imagen actual al tipo genérico de imagen de tipo bitmap
        /// </summary>
        public override Bitmap ConvertToBitmap()
        {
            return this.Image;
        }

        /// <summary>
        /// Sirve para reducir la imagen 
        /// </summary>
        /// <param name="img">Imagen a reducir</param>
        /// <param name="width">Ancho de la imagen resultado</param>
        /// <param name="height">Alto de la imagen resultado</param>
        /// <returns>Imagen reducida</returns>
        public override OImage EscalarImagen(OImage img, int ancho, int alto)
        {
            Bitmap b = new Bitmap(ancho, alto);
            
            OImage reducida = new OImagenBitmap(this.Codigo);
            reducida.MomentoCreacion = this.MomentoCreacion;
            Graphics g = Graphics.FromImage((Image)b);

            g.DrawImage((Image)img.Image, 0, 0, ancho, alto);
            g.Dispose();
            g = null;

            reducida.Image = (Image)b;
            Image prueba = (Image)b;
            return reducida;
        }

        /// <summary>
        /// Método que realiza el empaquetado de un objeto para ser enviado por remoting
        /// </summary>
        /// <returns></returns>
        public override byte[] ToArray(ImageFormat formato, double escalado)
        {
            byte[] resultado = new byte[0];
            if (this.Image is Bitmap)
            {
                try
                {
                    // Escalado de la imagen
                    OImagenBitmap imgAux = (OImagenBitmap)this.EscalarImagen(this, escalado);

                    MemoryStream stream = new MemoryStream();
                    imgAux.Image.Save(stream, formato);
                    resultado = stream.ToArray();
                }
                catch
                {
                    resultado = new byte[0];
                }
            }

            return resultado;
        }

        /// <summary>
        /// Método que realiza el desempaquetado de un objeto recibido por remoting
        /// </summary>
        /// <returns></returns>
        public override OImage FromArray(byte[] arrayValue)
        {
            OImagenBitmap resultado = null;

            if (arrayValue.Length > 0)
            {
                try
                {
                    MemoryStream stream = new MemoryStream(arrayValue);
                    Bitmap bmp = new Bitmap(stream);

                    resultado = new OImagenBitmap(this.Codigo, bmp);
                }
                catch
                {
                    resultado = null;
                }
            }          
            
            return resultado;
        }

        /// <summary>
        /// Crea una nueva imagen de la misma clase
        /// </summary>
        /// <returns>Nueva instancia de la misma clase de imagen</returns>
        public override OImage Nueva()
        {
            return new OImagenBitmap(this.Codigo);
        }
        #endregion
    }
}
