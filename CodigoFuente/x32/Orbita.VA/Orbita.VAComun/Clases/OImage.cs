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
// Last Modified On : 10-12-2012
// Description      : Cambiado el orden de los parámetros alto y ancho del método escalar imagen
//
// Last Modified By : aibañez
// Last Modified On : 03-11-2012
// Description      : Añadido campo de momento de creación
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
    /// Clase genérica de imagen. Independiente del formato de almacenamiento y del formato de adquisición
    /// </summary>
    [Serializable]
    public class OImage : OLargeObjectBase, ICloneable
    {
        #region Propiedad(es)
        /// <summary>
        /// Campo donde se guarda la imagen
        /// </summary>
        protected object _Image;
        /// <summary>
        /// Propieadad a heredar donde se accede a la imagen
        /// </summary>
        public virtual object Image
        {
            get { return _Image; }
            set { _Image = value; }
        }

        /// <summary>
        /// Tipo de imagen que almacena
        /// </summary>
        private TipoImagen _TipoImagen;
        /// <summary>
        /// Tipo de imagen que almacena
        /// </summary>
        public TipoImagen TipoImagen
        {
            get { return _TipoImagen; }
            set { _TipoImagen = value; }
        }

        /// <summary>
        /// Tamaño en X de la imagen
        /// </summary>
        public virtual int Width
        {
            get { return 0; }
        }

        /// <summary>
        /// Tamaño en Y de la imagen
        /// </summary>
        public virtual int Height
        {
            get { return 0; }
        }

        /// <summary>
        /// Informa del momento de la creación de la imagen
        /// </summary>
        private DateTime _MomentoCreacion;
        /// <summary>
        /// Informa del momento de la creación de la imagen
        /// </summary>
        public DateTime MomentoCreacion
        {
            get { return _MomentoCreacion; }
            set { _MomentoCreacion = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OImage():
            this("OrbitaImage")
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OImage(string codigo) :
            base(codigo)
        {
            this._Image = null;
            this._TipoImagen = TipoImagen.Bitmap; // Valor por defecto
            this.MomentoCreacion = DateTime.Now;
        }
        /// <summary>
        /// Constructor de la clase que además se encarga de liberar la memoria de la variable que se le pasa por parámetro
        /// </summary>
        /// <param name="imagenALiberar">Variable que se desea liberar de memoria</param>
        public OImage(object imagen):
            this("OrbitaImage", imagen)
        {
        }
        /// <summary>
        /// Constructor de la clase que además se encarga de liberar la memoria de la variable que se le pasa por parámetro
        /// </summary>
        /// <param name="imagenALiberar">Variable que se desea liberar de memoria</param>
        public OImage(string codigo, object imagen):
            base(codigo)
        {
            this._Image = imagen;
            this._TipoImagen = TipoImagen.Bitmap; // Valor por defecto
            this.MomentoCreacion = DateTime.Now;
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Guarda la imagen asincronamente en un fichero de disco
        /// </summary>
        /// <param name="ruta">Ruta donde se ha de guardar la imagen</param>
        public void GuardarAsincrono(string ruta)
        {
            if (this.EsValida())
            {
                OAlmacenImagenesManager.GuardarImagen(ruta, this);
            }
        }
        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Clonado de la imagen
        /// </summary>
        public virtual object Clone()
        {
            return null;
        }

        /// <summary>
        /// Método a implementar en las clases hijas que indica que la imagen es válida
        /// </summary>
        /// <returns></returns>
        public virtual bool EsValida()
        {
            return (this.Image != null) && (this.Width > 0) && (this.Height > 0);
        }

        /// <summary>
        /// Método a implementar en las clases hijas para guardar la imagen en un fichero de disco
        /// </summary>
        /// <param name="ruta">Ruta donde se ha de guardar la imagen</param>
        /// <returns>Verdadero si la ruta donde se ha de guardar el fichero es válida</returns>
        public virtual bool Guardar(string ruta)
        {
            if (this.EsValida())
            {
                if (App.CreacionDirectorio(Path.GetDirectoryName(ruta)))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Método a implementar en las clases hijas para cargar la imagen de un fichero de disco
        /// </summary>
        /// <param name="ruta">Ruta de donde se ha de cargar la imagen</param>
        /// <returns>Verdadero si el fichero que se ha de cargar existe</returns>
        public virtual bool Cargar(string ruta)
        {
            if (File.Exists(ruta))
            {
                this.MomentoCreacion = DateTime.Now;

                return true;
            }
            return false;
        }

        /// <summary>
        /// Convierte la imagen de tipo bitmap al tipo actual
        /// </summary>
        /// <param name="imagenBitmap">Imagen de tipo bitmap desde el cual se va a importar la imagen</param>
        public virtual OImage ConvertFromBitmap(Bitmap imagenBitmap)
        {
            return null;
        }

        /// <summary>
        /// Convierte la imagen actual al tipo genérico de imagen de tipo bitmap
        /// </summary>
        public virtual Bitmap ConvertToBitmap()
        {
            return null;
        }

        /// <summary>
        /// Conversión del tipo de imagen 
        /// </summary>
        /// <typeparam name="ClaseImagen">Tipo de imagen a devolver</typeparam>
        /// <param name="imagen"></param>
        /// <returns></returns>
        public virtual bool Convert<ClaseImagen>(out ClaseImagen imagen)
            where ClaseImagen : OImage, new()
        {
            imagen = null;
            Bitmap bitmapImage = this.ConvertToBitmap();
            if ((bitmapImage != null) && (bitmapImage.Width > 0) && (bitmapImage.Height > 0))
            {
                imagen = new ClaseImagen();
                imagen = (ClaseImagen)imagen.ConvertFromBitmap(bitmapImage);
                if (imagen.EsValida())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Sirve para reducir o ampliar la imagen 
        /// </summary>
        /// <param name="img">Imagen a reducir o ampliar</param>
        /// <param name="width">Ancho de la imagen resultado</param>
        /// <param name="height">Alto de la imagen resultado</param>
        /// <returns>Imagen reducida</returns>
        public virtual OImage EscalarImagen(OImage img, int ancho, int alto)
        {
            // Implementado en hijos
            return null;
        }

        /// <summary>
        /// Método que realiza el empaquetado de un objeto para ser enviado por remoting
        /// </summary>
        /// <returns></returns>
        public virtual byte[] ToArray(ImageFormat formato, double escalado)
        {
            // Implementado en hijos
            return null;
        }

        /// <summary>
        /// Método que realiza el desempaquetado de un objeto recibido por remoting
        /// </summary>
        /// <returns></returns>
        public virtual OImage FromArray(byte[] arrayValue)
        {
            // Implementado en hijos
            return null;
        }

        /// <summary>
        /// Crea una nueva imagen de la misma clase
        /// </summary>
        /// <returns>Nueva instancia de la misma clase de imagen</returns>
        public virtual OImage Nueva()
        {
            // Implementado en hijos
            return null;
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
                this._Image = null;
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Sirve para reducir o ampliar la imagen 
        /// </summary>
        /// <param name="img">Imagen a reduciro ampliar </param>
        /// <param name="width">Ancho de la imagen resultado</param>
        /// <param name="height">Alto de la imagen resultado</param>
        /// <returns>Imagen reducida</returns>
        public OImage EscalarImagen(OImage img, ResolucionesImagenEstandar resolucion)
        {
            int width;
            int height;
            switch (resolucion)
            {
                case ResolucionesImagenEstandar.VGA:
                    width = 640;
                    height = 480;
                    break;
                case ResolucionesImagenEstandar.SVGA:
                    width = 800;
                    height = 600;
                    break;
                case ResolucionesImagenEstandar.Mpx1:
                    width = 1280;
                    height = 800;
                    break;
                case ResolucionesImagenEstandar.Mpx1_3:
                    width = 1440;
                    height = 900;
                    break;
                case ResolucionesImagenEstandar.Mpx2:
                    width = 1600;
                    height = 1200;
                    break;
                case ResolucionesImagenEstandar.Mpx3:
                    width = 2048;
                    height = 1536;
                    break;
                case ResolucionesImagenEstandar.Mpx4:
                    width = 640;
                    height = 480;
                    break;
                case ResolucionesImagenEstandar.Mpx5:
                    width = 2560;
                    height = 1920;
                    break;
                default:
                    width = ((Image)img.Image).Size.Width;
                    height = ((Image)img.Image).Size.Height;
                    break;
            }

            return EscalarImagen(img, width, height);
        }

        /// <summary>
        /// Sirve para reducir o ampliar la imagen 
        /// </summary>
        /// <param name="img">Imagen a reduciro ampliar </param>
        /// <param name="escalado">Escalado</param>
        /// <returns>Imagen reducida</returns>
        public OImage EscalarImagen(OImage img, double escalado)
        {
            if (escalado == 1)
            {
                return img;    
            }

            int width = (int)(this.Width * escalado);
            int height = (int)(this.Height * escalado);

            return EscalarImagen(img, width, height);
        }
        #endregion
    }

    /// <summary>
    /// Clase genérica de gráfico asociado a imagenes. Independiente del formato de almacenamiento y del formato de adquisición
    /// </summary>
    [Serializable]
    public class OGrafico : IDisposable, ICloneable
    {
        #region Atributo(s)
        /// <summary>
        /// Campo donde se guarda la imagen
        /// </summary>
        protected object _Grafico;
        /// <summary>
        /// Propieadad a heredar donde se accede a la imagen
        /// </summary>
        public virtual object Grafico
        {
            get { return _Grafico; }
            set { _Grafico = value; }
        }
        /// <summary>
        /// Tipo de imagen que almacena
        /// </summary>
        private TipoImagen _TipoGrafico;
        /// <summary>
        /// Tipo de imagen que almacena
        /// </summary>
        public TipoImagen TipoGrafico
        {
            get { return _TipoGrafico; }
            set { _TipoGrafico = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OGrafico(object grafico)
        {
            this._Grafico = grafico;
            this._TipoGrafico = TipoImagen.Bitmap; // Valor por defecto
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OGrafico()
        {
            this._Grafico = null;
            this._TipoGrafico = TipoImagen.Bitmap; // Valor por defecto
        }
        /// <summary>
        /// Constructor de la clase que además se encarga de liberar la memoria de la variable que se le pasa por parámetro
        /// </summary>
        /// <param name="imagenALiberar">Variable que se desea liberar de memoria</param>
        public OGrafico(OImage graficoALiberar)
        {
            // Limpieza de memoria
            if (graficoALiberar != null)
            {
                graficoALiberar = null;
            }

            this._Grafico = null;
            this._TipoGrafico = TipoImagen.Bitmap; // Valor por defecto
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Guarda el gráfico asincronamente en un fichero de disco
        /// </summary>
        /// <param name="ruta">Ruta donde se ha de guardar la imagen</param>
        public void GuardarAsincrono(string ruta)
        {
            if (this.EsValida())
            {
                OAlmacenImagenesManager.GuardarGrafico(ruta, this);
            }
        }
        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Método a implementar en las clases hijas. Elimina la memoria asignada por el objeto.
        /// </summary>
        public virtual void Dispose()
        {
            if (this._Grafico != null)
            {
                this._Grafico = null;
            }
        }

        /// <summary>
        /// Clonado de la imagen
        /// </summary>
        public virtual object Clone()
        {
            return null;
        }

        /// <summary>
        /// Método a implementar en las clases hijas que indica que la imagen es válida
        /// </summary>
        /// <returns></returns>
        public virtual bool EsValida()
        {
            return (this.Grafico != null);
        }

        /// <summary>
        /// Método a implementar en las clases hijas para guardar la imagen en un fichero de disco
        /// </summary>
        /// <param name="ruta">Ruta donde se ha de guardar la imagen</param>
        /// <returns>Verdadero si la ruta donde se ha de guardar el fichero es válida</returns>
        public virtual bool Guardar(string ruta)
        {
            if (this.EsValida())
            {
                if (App.CreacionDirectorio(Path.GetDirectoryName(ruta)))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Método a implementar en las clases hijas para cargar la imagen de un fichero de disco
        /// </summary>
        /// <param name="ruta">Ruta de donde se ha de cargar la imagen</param>
        /// <returns>Verdadero si el fichero que se ha de cargar existe</returns>
        public virtual bool Cargar(string ruta)
        {
            if (File.Exists(ruta))
            {
                return true;
            }
            return false;
        }
        #endregion
    }

    /// <summary>
    /// Agrupación de imágen con su gráfico asociado
    /// </summary>
    public class OConjuntoImagenGrafico
    {
        #region Atributo(s)
        /// <summary>
        /// Código identificativo
        /// </summary>
        public string Codigo; 
        
        /// <summary>
        /// Imagen
        /// </summary>
        public OImage Imagen;

        /// <summary>
        /// Gráfico
        /// </summary>
        public OGrafico Grafico;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codigo">Código identificativo</param>
        /// <param name="imagen">Imagen</param>
        /// <param name="grafico">Gráfico asociado a la imagen</param>
        public OConjuntoImagenGrafico(string codigo, OImage imagen, OGrafico grafico)
        {
            this.Codigo = codigo;
            this.Imagen = imagen;
            this.Grafico = grafico;
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codigo">Código identificativo</param>
        /// <param name="imagen">Imagen</param>
        /// <param name="grafico">Gráfico asociado a la imagen</param>
        public OConjuntoImagenGrafico(string codigo, OImage imagen)
            : this(codigo, imagen, null)
        {
        }
        #endregion
    }

    /// <summary>
    /// Enumerado de los formatos de imagen soportados por el sistema
    /// </summary>
    public enum TipoImagen
    {
        /// <summary>
        /// Bitmap de Windows
        /// </summary>
        Bitmap,
        /// <summary>
        /// CogImage de VisionPro
        /// </summary>
        VisionPro,
    }
    /// <summary>
    /// Resoluciones frecuentes de imagenes
    /// </summary>
    public enum ResolucionesImagenEstandar
    {
        /// <summary>
        /// VGA
        /// </summary>
        VGA,
        /// <summary>
        /// SVGA
        /// </summary>
        SVGA,
        /// <summary>
        /// 1 Mpx
        /// </summary>
        Mpx1,
        /// <summary>
        /// 1,3 Mpx
        /// </summary>
        Mpx1_3,
        /// <summary>
        /// 2 Mpx
        /// </summary>
        Mpx2,
        /// <summary>
        /// 3 Mpx
        /// </summary>
        Mpx3,
        /// <summary>
        /// 4 Mpx
        /// </summary>
        Mpx4,
        /// <summary>
        /// 5 Mpx
        /// </summary>
        Mpx5
    }
}
