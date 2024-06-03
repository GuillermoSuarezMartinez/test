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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Orbita.VAComun
{
    /// <summary>
    /// Imagen de tipo Bitmap de windows
    /// </summary>
    [Serializable]
    public class BitmapImage : OrbitaImage, IDisposable, ICloneable
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
                int resultado = base.Width;
                if (this.Image is Bitmap)
                {
                    resultado = this.Image.Height;
                }
                return resultado;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public BitmapImage()
            : base()
        {
            this.TipoImagen = TipoImagen.Bitmap;
        }
        /// <summary>
        /// Constructor de la clase que además se encarga de liberar la memoria de la variable que se le pasa por parámetro
        /// </summary>
        /// <param name="imagenALiberar">Variable que se desea liberar de memoria</param>
        public BitmapImage(object imagen)
            : base(imagen)
        {
            this.TipoImagen = TipoImagen.Bitmap;
        }
        /// <summary>
        /// Constructor de la clase que además se encarga de liberar la memoria de la variable que se le pasa por parámetro
        /// </summary>
        /// <param name="imagenALiberar">Variable que se desea liberar de memoria</param>
        public BitmapImage(OrbitaImage imagenALiberar)
            : base(imagenALiberar)
        {
            this.TipoImagen = TipoImagen.Bitmap;
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Método a implementar en las clases hijas. Elimina la memoria asignada por el objeto.
        /// </summary>
        public override void Dispose()
        {
            try
            {
                if (this.Image != null)
                {
                    this.Image.Dispose();
                    this.Image = null;
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// Clonado de la imagen
        /// </summary>
        public override object Clone()
        {
            BitmapImage imagenResultado = new BitmapImage();
            if (this._Image != null)
            {
                imagenResultado._Image = this.Image.Clone();
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

                return this.EsValida();
            }
            return false;
        }

        /// <summary>
        /// Convierte la imagen de tipo bitmap al tipo actual
        /// </summary>
        /// <param name="imagenBitmap">Imagen de tipo bitmap desde el cual se va a importar la imagen</param>
        public override OrbitaImage ConvertFromBitmap(Bitmap imagenBitmap)
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
        public override OrbitaImage EscalarImagen(OrbitaImage img, int alto, int ancho)
        {
            Bitmap b = new Bitmap(alto, ancho);
            OrbitaImage reducida = new OrbitaImage();
            Graphics g = Graphics.FromImage((Image)b);

            g.DrawImage((Image)img.Image, 0, 0, alto, ancho);
            g.Dispose();

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
                    BitmapImage imgAux = (BitmapImage)this.EscalarImagen(this, escalado);

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
        public override OrbitaImage FromArray(byte[] arrayValue)
        {
            BitmapImage resultado = null;

            if (arrayValue.Length > 0)
            {
                try
                {
                    MemoryStream stream = new MemoryStream(arrayValue);
                    Bitmap bmp = new Bitmap(stream);

                    resultado = new BitmapImage(bmp);
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
        public override OrbitaImage Nueva()
        {
            return new BitmapImage();
        }
        #endregion
    }
}
