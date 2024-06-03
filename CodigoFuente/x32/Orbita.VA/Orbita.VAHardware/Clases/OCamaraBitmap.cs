//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 10-12-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System.Drawing;
using Orbita.VAComun;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Clase base que implementa las funciones de trabajo sobre bitmaps
    /// </summary>
    public class OCamaraBitmap : OCamaraBase
    {
        #region Atributo(s)
        /// <summary>
        /// Informa si hay adquirida una nueva imagen
        /// </summary>
        protected bool HayNuevaImagen;
        #endregion

        #region Propiedad(es) heredadas
        /// <summary>
        /// Propieadad a heredar donde se accede a la imagen
        /// </summary>
        public new OImagenBitmap ImagenActual
        {
            get { return (OImagenBitmap)this._ImagenActual; }
            set { this._ImagenActual = value; }
        }
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>       
        public OCamaraBitmap(string codigo)
            : base(codigo)
        {
            // No hay ninguna imagen adquirida
            this.HayNuevaImagen = false;

            // Implementado en heredados
        } 
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Se extrae la imagen actual de la cámara
        /// </summary>
        /// <returns></returns>
        protected bool GetCurrentImage(out OImagenBitmap bitmapImage)
        {
            bool resultado = false;
            bitmapImage = null;

            if (this.HayNuevaImagen)
            {
                resultado = true;
                bitmapImage = this.ImagenActual;
                this.HayNuevaImagen = false;
            }

            return resultado;
        }

        /// <summary>
        /// Se importa una imagen a la cámara
        /// </summary>
        /// <returns></returns>
        protected bool SetCurrentImage(OImagenBitmap bitmapImage)
        {
            this.ImagenActual = bitmapImage;
            return true;
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Carga los valores de la cámara
        /// </summary>
        public override void Inicializar()
        {
            base.Inicializar();
        }

        /// <summary>
        /// Finaliza la cámara
        /// </summary>
        public override void Finalizar()
        {          
            base.Finalizar();
        }

        /// <summary>
        /// Carga una imagen de disco
        /// </summary>
        /// <param name="ruta">Indica la ruta donde se encuentra la fotografía</param>
        /// <returns>Devuelve verdadero si la operación se ha realizado con éxito</returns>
        public override bool CargarImagenDeDisco(out OImage imagen, string ruta)
        {
            // Valores por defecto
            bool resultado = base.CargarImagenDeDisco(out imagen, ruta);

            // Se carga la imagen
            OImagenBitmap bitmapImage = new OImagenBitmap(this.Codigo);
            bitmapImage.Cargar(ruta);
            if (bitmapImage.EsValida())
            {
                // Se carga en el display
                this.SetCurrentImage(bitmapImage);

                // Devolvemos los valores
                imagen = bitmapImage;
                resultado = true;
            }
            return resultado;
        }

        /// <summary>
        /// Guarda una imagen en disco
        /// </summary>
        /// <param name="ruta">Indica la ruta donde se ha de guardar la fotografía</param>
        /// <returns>Devuelve verdadero si la operación se ha realizado con éxito</returns>
        public override bool GuardarImagenADisco(string ruta)
        {
            // Valores por defecto
            bool resultado = base.GuardarImagenADisco(ruta);

            OImagenBitmap bitmapImage = new OImagenBitmap();

            // Se carga en el display
            if (this.GetCurrentImage(out bitmapImage))
            {
                // Se guarda la imagen
                if (bitmapImage.EsValida())
                {
                    bitmapImage.Guardar(ruta);
                    resultado = true;
                }
            }

            return resultado;
        }

        /// <summary>
        /// Devuelve una nueva imagen del tipo adecuado al trabajo con la cámara
        /// </summary>
        /// <returns>Imagen del tipo adecuado al trabajo con la cámara</returns>
        public override OImage NuevaImagen()
        {
            OImagenBitmap bitmapImage = new OImagenBitmap(new Bitmap(this.Resolucion.Width, this.Resolucion.Height));
            return bitmapImage;
        }
        #endregion
    }
}
