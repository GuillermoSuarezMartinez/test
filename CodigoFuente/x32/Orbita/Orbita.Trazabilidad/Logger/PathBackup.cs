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
    /// Path Backup.
    /// </summary>
    public class PathBackup : FullPath
    {
        #region Atributos privados
        /// <summary>
        /// Subruta de almacenamiento de ficheros de backup de logger.
        /// </summary>
        Orbita.Trazabilidad.Mascara subPath;
        #endregion

        #region Constructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.PathBackup.
        /// </summary>
        public PathBackup()
            : this(string.Format(CultureInfo.CurrentCulture, @"{0}\{1}", Application.StartupPath, Orbita.Trazabilidad.Ruta.Backup)) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.PathBackup.
        /// </summary>
        /// <param name="path">Ruta de almacenamiento de ficheros de backup de logger.</param>
        public PathBackup(string path)
            : this(path, Orbita.Trazabilidad.Mascara.AñoMes) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.PathBackup.
        /// </summary>
        /// <param name="subPath">Subruta de almacenamiento de ficheros de backup de logger.</param>
        public PathBackup(Orbita.Trazabilidad.Mascara subPath)
            : this(string.Format(CultureInfo.CurrentCulture, @"{0}\{1}", Application.StartupPath, Orbita.Trazabilidad.Ruta.Backup), subPath) { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase Orbita.Trazabilidad.PathBackup.
        /// </summary>
        /// <param name="path">Ruta de almacenamiento de ficheros de backup de logger.</param>
        /// <param name="subPath">Subruta de almacenamiento de ficheros de backup de logger.</param>
        public PathBackup(string path, Orbita.Trazabilidad.Mascara subPath)
            : base(path)
        {
            this.subPath = subPath;
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Subruta de almacenamiento de ficheros de backup de logger.
        /// </summary>
        public Orbita.Trazabilidad.Mascara SubPath
        {
            get { return this.subPath; }
            set { this.subPath = value; }
        }
        /// <summary>
        /// Subruta string de almacenamiento de ficheros de backup de logger. Propiedad de solo lectura.
        /// </summary>
        public string Mascara
        {
            get
            {
                switch (this.subPath)
                {
                    case Orbita.Trazabilidad.Mascara.Año:
                        return Orbita.Trazabilidad.Formato.Año;
                    case Orbita.Trazabilidad.Mascara.Mes:
                        return Orbita.Trazabilidad.Formato.Mes;
                    case Orbita.Trazabilidad.Mascara.MesDia:
                        return Orbita.Trazabilidad.Formato.MesDia;
                    case Orbita.Trazabilidad.Mascara.AñoMes:
                        return Orbita.Trazabilidad.Formato.AñoMes;
                    case Orbita.Trazabilidad.Mascara.AñoMesDia:
                        return Orbita.Trazabilidad.Formato.AñoMesDia;
                    case Orbita.Trazabilidad.Mascara.None:
                    default:
                        return null;
                }
            }
        }
        #endregion
    }
}