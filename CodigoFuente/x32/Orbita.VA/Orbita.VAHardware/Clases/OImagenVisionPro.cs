//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 12-12-2012
// Description      : Se actualiza el código y la fecha de creación de la imagen
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using Orbita.VAComun;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Imagen de tipo VisionPro de Cognex
    /// </summary>
    [Serializable]
    public class OImagenVisionPro : OImage
    {
        #region Atributo(s)
        /// <summary>
        /// Propieadad a heredar donde se accede a la imagen
        /// </summary>
        public new ICogImage Image
        {
            get { return (ICogImage)_Image; }
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
                if (this.Image is ICogImage)
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
                if (this.Image is ICogImage)
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
        public OImagenVisionPro()
            : this("VisionProImage")
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OImagenVisionPro(string codigo)
            : base(codigo)
        {
            this.TipoImagen = TipoImagen.VisionPro;
        }
        /// <summary>
        /// Constructor de la clase que además se encarga de liberar la memoria de la variable que se le pasa por parámetro
        /// </summary>
        /// <param name="imagenALiberar">Variable que se desea liberar de memoria</param>
        public OImagenVisionPro(object imagen)
            : this("VisionProImage", imagen)
        {
        }
        /// <summary>
        /// Constructor de la clase que además se encarga de liberar la memoria de la variable que se le pasa por parámetro
        /// </summary>
        /// <param name="imagenALiberar">Variable que se desea liberar de memoria</param>
        public OImagenVisionPro(string codigo, object imagen)
            : base(codigo, imagen)
        {
            this.TipoImagen = TipoImagen.VisionPro;
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Método a implementar en las clases hijas. Elimina la memoria asignada por el objeto.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (!this.Disposed)
            {
                if (this.Image != null)
                {
                    ////Marshal.ReleaseComObject(this._Image); NO FUNCIONA!!
                    //((IDisposable)this._Image).Dispose(); // Da error al salir del programa
                    this._Image = null;
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Clonado de la imagen
        /// </summary>
        public override object Clone()
        {
            OImagenVisionPro imagenResultado = new OImagenVisionPro();
            imagenResultado.Codigo = this.Codigo;
            imagenResultado.MomentoCreacion = this.MomentoCreacion;
            if (this.Image != null)
            {
                try
                {
                    if (this.Image is CogImage24PlanarColor)
                    {
                        Cognex.VisionPro.CogImage24PlanarColor nuevaImagen = new CogImage24PlanarColor((CogImage24PlanarColor)this.Image);
                        imagenResultado.Image = nuevaImagen;
                    }
                    else
                    {
                        imagenResultado.Image = this.Image.CopyBase(CogImageCopyModeConstants.CopyPixels);
                    }
                }
                catch (Exception exception)
                {
                    OVALogsManager.Error(ModulosHardware.ImagenGraficosVPro, "VisionProImage_Clone", exception);
                }
            }
            return imagenResultado;
        }

        /// <summary>
        /// Método a implementar en las clases hijas que indica que la imagen es válida
        /// </summary>
        /// <returns></returns>
        public override bool EsValida()
        {
            return (base.EsValida()) && (this.Image.Allocated);
        }

        /// <summary>
        /// Método a implementar en las clases hijas para guardar la imagen en un fichero de disco
        /// </summary>
        /// <param name="ruta">Ruta donde se ha de guardar la imagen</param>
        /// <returns>Verdadero si la ruta donde se ha de guardar el fichero es válida</returns>
        public override bool Guardar(string ruta)
        {
            try
            {
                if (base.Guardar(ruta))
                {
                    CogImageFile ImageFile = new CogImageFile();
                    ImageFile.Open(ruta, CogImageFileModeConstants.Write);
                    ImageFile.Append(this.Image);
                    ImageFile.Close();

                    // Se guarda el sistema de coordenadas raiz
                    string rutaCoord = Path.GetDirectoryName(ruta) + @"\" + Path.GetFileNameWithoutExtension(ruta) + ".xyz";
                    Stream stream = new FileStream(rutaCoord, FileMode.Create, FileAccess.Write, FileShare.None);
                    BinaryFormatter formateador = new BinaryFormatter();
                    try
                    {
                        OVisionProImageExtras extras = new OVisionProImageExtras(this.Image.PixelFromRootTransform, this.Image.CoordinateSpaceTree);
                        formateador.Serialize(stream, extras);
                    }
                    catch (Exception exception)
                    {
                        OVALogsManager.Error(ModulosHardware.ImagenGraficosVPro, "VisionProImage", exception);
                    }
                    stream.Close();

                    return File.Exists(ruta) && File.Exists(rutaCoord);
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(ModulosHardware.ImagenGraficosVPro, "VisionProImage", exception);
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
            try
            {
                if (base.Cargar(ruta))
                {
                    CogImageFile ImageFile = new CogImageFile();
                    ImageFile.Open(ruta, CogImageFileModeConstants.Read);
                    this.Image = ImageFile[0];
                    this.MomentoCreacion = DateTime.Now;
                    ImageFile.Close();

                    // Se cargan los sistemas de coordenadas
                    string rutaCoordXYZ = Path.GetDirectoryName(ruta) + @"\" + Path.GetFileNameWithoutExtension(ruta) + ".xyz";
                    if (File.Exists(rutaCoordXYZ))
                    {
                        OVisionProImageExtras extras = null;
                        Stream stream = new FileStream(rutaCoordXYZ, FileMode.Open, FileAccess.Read, FileShare.Read);
                        BinaryFormatter formateador = new BinaryFormatter();
                        try
                        {
                            extras = (OVisionProImageExtras)formateador.Deserialize(stream);
                        }
                        catch (Exception exception)
                        {
                            OVALogsManager.Error(ModulosHardware.ImagenGraficosVPro, "VisionProImage", exception);
                        }
                        stream.Close();
                        this.Image.PixelFromRootTransform = extras.PixelFromRootTransform;
                        this.Image.CoordinateSpaceTree = extras.CoordinateSpaceTree;
                    }

                    return this.EsValida();
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(ModulosHardware.ImagenGraficosVPro, "VisionProImage", exception);
            }

            return false;
        }

        /// <summary>
        /// Convierte la imagen de tipo bitmap al tipo actual
        /// </summary>
        /// <param name="imagenBitmap">Imagen de tipo bitmap desde el cual se va a importar la imagen</param>
        public override OImage ConvertFromBitmap(Bitmap imagenBitmap)
        {
            ICogImage cogImage = null;

            // Bitmap --> CogImage24PlanarColor
            if ((imagenBitmap.PixelFormat == PixelFormat.Format24bppRgb) || (imagenBitmap.PixelFormat == PixelFormat.Format32bppArgb) )
            {
                cogImage = new CogImage24PlanarColor(imagenBitmap);
            }

            // Bitmap --> CogImage8Grey
            if (imagenBitmap.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                cogImage = new CogImage8Grey(imagenBitmap);
            }

            this.Image = cogImage;
            return new OImagenVisionPro(cogImage);
        }

        /// <summary>
        /// Convierte la imagen actual al tipo genérico de imagen de tipo bitmap
        /// </summary>
        public override Bitmap ConvertToBitmap()
        {
            Bitmap bitmap = null;

            // CogImage24PlanarColor --> Bitmap
            if (this.Image is CogImage24PlanarColor)
            {
                bitmap = ((CogImage24PlanarColor)this.Image).ToBitmap();
            }

            // CogImage8Grey --> Bitmap
            if (this.Image is CogImage8Grey)
            {
                bitmap = ((CogImage8Grey)this.Image).ToBitmap();
            }

            return bitmap;
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
            OImagenVisionPro resultado = new OImagenVisionPro(this.Codigo, this.Image.ScaleImage(ancho, alto));
            resultado.MomentoCreacion = this.MomentoCreacion;

            return resultado;
        }

        /// <summary>
        /// Método que realiza el empaquetado de un objeto para ser enviado por remoting
        /// </summary>
        /// <returns></returns>
        public override byte[] ToArray(ImageFormat formato, double escalado)
        {
            byte[] resultado = new byte[0];
            if (this.Image is ICogImage)
            {
                try
                {
                    // Escalado de la imagen
                    OImagenVisionPro imgAux = (OImagenVisionPro)this.EscalarImagen(this, escalado);

                    MemoryStream stream = new MemoryStream();
                    imgAux.Image.ToBitmap().Save(stream, formato);
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
            OImagenVisionPro resultado = null;

            if (arrayValue.Length > 0)
            {
                try
                {
                    MemoryStream stream = new MemoryStream(arrayValue);
                    Bitmap bmp = new Bitmap(stream);

                    resultado = new OImagenVisionPro(this.Codigo);
                    resultado = (OImagenVisionPro)resultado.ConvertFromBitmap(bmp);
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
            return new OImagenVisionPro(this.Codigo);
        }
        #endregion
    }

    /// <summary>
    /// Grafico de tipo CogGraphicCollection de Cognex
    /// </summary>
    [Serializable]
    public class OVisionProGrafico : OGrafico
    {
        #region Atributo(s)
        /// <summary>
        /// Propieadad a heredar donde se accede a la imagen
        /// </summary>
        public new CogGraphicCollection Grafico
        {
            get { return (CogGraphicCollection)_Grafico; }
            set { _Grafico = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OVisionProGrafico(object grafico)
            : base(grafico)
        {
            this.TipoGrafico = TipoImagen.VisionPro;
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OVisionProGrafico()
            : base()
        {
            this.TipoGrafico = TipoImagen.VisionPro;
        }
        /// <summary>
        /// Constructor de la clase que además se encarga de liberar la memoria de la variable que se le pasa por parámetro
        /// </summary>
        /// <param name="imagenALiberar">Variable que se desea liberar de memoria</param>

        public OVisionProGrafico(OImage graficoALiberar)
            : base(graficoALiberar)
        {
            this.TipoGrafico = TipoImagen.VisionPro;
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Desplaza todo el conjunto de gráficos en X y en Y
        /// </summary>
        /// <param name="X">Valor de desplazamiento en X</param>
        /// <param name="Y">Valor de desplazamiento en Y</param>
        public void Desplaza(int X, int Y)
        {
            if ((this.Grafico != null) && (this.Grafico is CogGraphicCollection))
            {
                for (int i = 0; i < this.Grafico.Count; i++)
                {
                    if (this.Grafico[i] is CogPolygon)
                    {
                        CogPolygon poligono = (CogPolygon)this.Grafico[i];
                        for (int j = 0; j < poligono.NumVertices; j++)
                        {
                            poligono.SetVertexX(j, poligono.GetVertexX(j) + X);
                            poligono.SetVertexY(j, poligono.GetVertexY(j) + Y);
                        }
                    }
                    if (this.Grafico[i] is CogGraphicLabel)
                    {
                        CogGraphicLabel label = (CogGraphicLabel)this.Grafico[i];
                        label.X += X;
                        label.Y += Y;
                    }
                    if (this.Grafico[i] is CogLine)
                    {
                        CogLine linea = (CogLine)this.Grafico[i];
                        linea.X += X;
                        linea.Y += Y;
                    }
                    if (this.Grafico[i] is CogLineSegment)
                    {
                        CogLineSegment lineaSeg = (CogLineSegment)this.Grafico[i];
                        lineaSeg.StartX += X;
                        lineaSeg.StartY += Y;
                        lineaSeg.EndX += X;
                        lineaSeg.EndY += Y;
                    }
                    if (this.Grafico[i] is CogCircle)
                    {
                        CogCircle circulo = (CogCircle)this.Grafico[i];
                        circulo.CenterX += X;
                        circulo.CenterY += Y;
                    }
                    if (this.Grafico[i] is CogPointMarker)
                    {
                        CogPointMarker punto = (CogPointMarker)this.Grafico[i];
                        punto.X += X;
                        punto.Y += Y;
                    }
                    if (this.Grafico[i] is CogCoordinateAxes)
                    {
                        CogCoordinateAxes coord = (CogCoordinateAxes)this.Grafico[i];
                        coord.OriginX += X;
                        coord.OriginY += Y;
                    }
                    if (this.Grafico[i] is CogRectangle)
                    {
                        CogRectangle rect = (CogRectangle)this.Grafico[i];
                        rect.X += X;
                        rect.Y += Y;
                    }
                }
            }
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Elimina la memoria asignada por el objeto.
        /// </summary>
        public override void Dispose()
        {
            try
            {
                if (this._Grafico != null)
                {
                    this._Grafico = null;
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// Clonado del gráfico
        /// </summary>
        public override object Clone()
        {
            OVisionProGrafico graficoResultado = new OVisionProGrafico();
            if (this.Grafico != null)
            {
                graficoResultado.Grafico = new CogGraphicCollection(this.Grafico);
            }
            return graficoResultado;
        }

        /// <summary>
        /// Método a implementar en las clases hijas que indica que la imagen es válida
        /// </summary>
        /// <returns></returns>
        public override bool EsValida()
        {
            return (base.EsValida()) && (this.Grafico.Count > 0);
        }

        /// <summary>
        /// Método a implementar en las clases hijas para guardar la imagen en un fichero de disco
        /// </summary>
        /// <param name="ruta">Ruta donde se ha de guardar la imagen</param>
        /// <returns>Verdadero si la ruta donde se ha de guardar el fichero es válida</returns>
        public override bool Guardar(string ruta)
        {
            try
            {
                if (base.Guardar(ruta))
                {
                    Stream stream = new FileStream(ruta, FileMode.Create, FileAccess.Write, FileShare.None);
                    BinaryFormatter formateador = new BinaryFormatter();
                    try
                    {
                        formateador.Serialize(stream, this.Grafico);
                    }
                    catch (Exception exception)
                    {
                        OVALogsManager.Error(ModulosHardware.ImagenGraficosVPro, "VisionProGrafico", exception);
                    }
                    stream.Close();

                    return File.Exists(ruta);
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(ModulosHardware.ImagenGraficosVPro, "VisionProGrafico", exception);
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
            try
            {
                if (base.Cargar(ruta))
                {
                    CogGraphicCollection graficoVPro = null;

                    Stream stream = new FileStream(ruta, FileMode.Open, FileAccess.Read, FileShare.Read);
                    BinaryFormatter formateador = new BinaryFormatter();
                    try
                    {
                        graficoVPro = (CogGraphicCollection)formateador.Deserialize(stream);
                    }
                    catch (Exception exception)
                    {
                        OVALogsManager.Error(ModulosHardware.ImagenGraficosVPro, "VisionProGrafico", exception);
                    }
                    stream.Close();
                    this.Grafico = graficoVPro;

                    return this.EsValida();
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(ModulosHardware.ImagenGraficosVPro, "VisionProGrafico", exception);
            }

            return false;
        }

        #endregion
    }

    /// <summary>
    /// Clase que almacena información extra de la imagen, necesaria para guardar y cargar de disco
    /// </summary>
    [Serializable]
    internal class OVisionProImageExtras
    {
        #region Atributo(s)
        /// <summary>
        /// Transformación del espacio de pixel al espacio raiz
        /// </summary>
        public ICogTransform2D PixelFromRootTransform;
        /// <summary>
        /// Conjunto de sistemas de coordenadas utilizados en la imagen
        /// </summary>
        public CogCoordinateSpaceTree CoordinateSpaceTree;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// {
        public OVisionProImageExtras(ICogTransform2D pixelFromRootTransform, CogCoordinateSpaceTree coordinateSpaceTree)
        {
            this.PixelFromRootTransform = pixelFromRootTransform;
            this.CoordinateSpaceTree = coordinateSpaceTree;
        }
        #endregion
    }
}
