//***********************************************************************
// Assembly         : OrbitaTrazabilidad
// Author           : crodriguez
// Created          : 02-17-2011
//
// Last Modified By : crodriguez
// Last Modified On : 02-21-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System.Globalization;
using System.Windows.Forms;
namespace Orbita.Trazabilidad
{
    /// <summary>
    /// Path Logger.
    /// </summary>
    public class PathLogger : FullPath
    {
        #region Atributos privados
        /// <summary>
        /// Nombre del fichero de logger.
        /// </summary>
        string fichero;
        /// <summary>
        /// Extensión del fichero de logger.
        /// </summary>
        string extension;
        #endregion

        #region Constructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.PathLogger.
        /// </summary>
        public PathLogger()
            : this(string.Format(CultureInfo.CurrentCulture, @"{0}\{1}", Application.StartupPath, Orbita.Trazabilidad.Ruta.Debug)) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.PathLogger.
        /// </summary>
        /// <param name="path">Ruta de almacenamiento de ficheros de logger.</param>
        public PathLogger(string path)
            : this(path, Orbita.Trazabilidad.Fichero.Debug) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.PathLogger.
        /// </summary>
        /// <param name="path">Ruta de almacenamiento de ficheros de logger.</param>
        /// <param name="fichero">Nombre del fichero de logger.</param>
        public PathLogger(string path, string fichero)
            : this(path, fichero, Orbita.Trazabilidad.Extension.Log) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.PathLogger.
        /// </summary>
        /// <param name="path">Ruta de almacenamiento de ficheros de logger.</param>
        /// <param name="fichero">Nombre del fichero de logger.</param>
        /// <param name="extension">Extensión del fichero de logger.</param>
        public PathLogger(string path, string fichero, string extension)
            : base(path)
        {
            this.fichero = fichero;
            this.extension = extension;
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Nombre del fichero de logger.
        /// </summary>
        public string Fichero
        {
            get { return this.fichero; }
            set { this.fichero = value; }
        }
        /// <summary>
        /// Extensión del fichero de logger.
        /// </summary>
        public string Extension
        {
            get { return this.extension; }
            set { this.extension = value; }
        }
        #endregion

        #region Métodos públicos
        /// <summary>
        /// Sobreescritura del método ToString().
        /// </summary>
        /// <returns>String.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, @"{0}\{1}.{2}", this.Path, this.Fichero, this.Extension);
        }
        /// <summary>
        /// Comprobar la existencia del fichero.
        /// </summary>
        /// <returns>True/False.</returns>
        public bool ExisteFichero()
        {
            return System.IO.File.Exists(this.ToString());
        }
        /// <summary>
        /// Borrar fichero.
        /// </summary>
        /// <returns>True/False.</returns>
        public bool BorrarFichero()
        {
            string archivo = this.ToString();
            if (System.IO.File.Exists(archivo))
            {
                // Elimina el archivo especificado. Si el archivo no existe, no produce una excepción.
                System.IO.File.Delete(archivo);
                return true;
            }
            return false;
        }
        #endregion
    }
}