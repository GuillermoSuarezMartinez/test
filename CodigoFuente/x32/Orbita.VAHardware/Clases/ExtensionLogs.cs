//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using Orbita.VAComun;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Define el conjunto de módulos del sistema
    /// </summary>
    public class ModulosHardware: ModulosSistema
    {
        #region Atributo(s)
        /// <summary>
        /// Módulo de funcionamiento base de las cámaras
        /// </summary>
        public static EnumModulosSistema Camaras = new EnumModulosSistema("Camaras", "Módulo de funcionamiento base de las cámaras", 201);
        /// <summary>
        /// Módulo de funcionamiento base de las Entradas/Salidas digitales
        /// </summary>
        public static EnumModulosSistema EntradasSalidas = new EnumModulosSistema("EntradasSalidas", "Módulo de funcionamiento base de las Entradas/Salidas digitales", 202);
        /// <summary>
        /// Módulo de funcionamiento de las cámaras Basler Pilot mediante el driver de VisionPro
        /// </summary>
        public static EnumModulosSistema CamaraBaslerVPro = new EnumModulosSistema("CamaraBaslerVPro", "Módulo de funcionamiento de las cámaras Basler Pilot mediante el driver de VisionPro", 203);
        /// <summary>
        /// Módulo de funcionamiento de las cámaras Axis
        /// </summary>
        public static EnumModulosSistema CamaraAxis = new EnumModulosSistema("CamaraAxis", "Módulo de funcionamiento de las cámaras Axis", 204);
        /// <summary>
        /// Módulo de funcionamiento del dispositivo de Entradas/Salidas digitales de las cámaras Axis
        /// </summary>
        public static EnumModulosSistema ES_Axis = new EnumModulosSistema("ES_Axis", "Módulo de funcionamiento del dispositivo de Entradas/Salidas digitales de las cámaras Axis", 205);
        /// <summary>
        /// Módulo de funcionamiento del dispositivo de Entradas/Salidas digitales USB1024HLS de Measurement Computing
        /// </summary>                         
        public static EnumModulosSistema ES_MeasurementComputing = new EnumModulosSistema("ES_USB1024HLS", "Módulo de funcionamiento del dispositivo de Entradas/Salidas digitales USB1024HLS de Measurement Computing", 206);
        /// <summary>
        /// Módulo de funcionamiento del dispositivo de Entradas/Salidas digitales mediante SCED
        /// </summary>
        public static EnumModulosSistema ES_SCED = new EnumModulosSistema("ES_SCED", "Módulo de funcionamiento del dispositivo de Entradas/Salidas digitales mediante SCED", 207);
        /// <summary>
        /// Módulo de funciones base para las imagenes y gráficos de tipo Bitmap
        /// </summary>
        public static EnumModulosSistema ImagenGraficosBitmap = new EnumModulosSistema("ImagenGraficosBitmap", "Módulo de funciones base para las imagenes y gráficos de tipo Bitmap", 208);
        /// <summary>
        /// Módulo de funciones base para las imagenes y gráficos de tipo VisionPro
        /// </summary>
        public static EnumModulosSistema ImagenGraficosVPro = new EnumModulosSistema("ImagenGraficosVPro", "Módulo de funciones base para las imagenes y gráficos de tipo VisionPro", 209);
        #endregion
    }
}
