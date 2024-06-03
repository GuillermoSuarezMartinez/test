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
    public class OModulosHardware: OModulosSistema
    {
        #region Atributo(s)
        /// <summary>
        /// Módulo de funcionamiento base de las cámaras
        /// </summary>
        public static OEnumModulosSistema Camaras = new OEnumModulosSistema("Camaras", "Módulo de funcionamiento base de las cámaras", 201);
        /// <summary>
        /// Módulo de funcionamiento base de las Entradas/Salidas digitales
        /// </summary>
        public static OEnumModulosSistema EntradasSalidas = new OEnumModulosSistema("EntradasSalidas", "Módulo de funcionamiento base de las Entradas/Salidas digitales", 202);
        /// <summary>
        /// Módulo de funcionamiento de las cámaras Basler Pilot mediante el driver de VisionPro
        /// </summary>
        public static OEnumModulosSistema CamaraBaslerVPro = new OEnumModulosSistema("CamaraBaslerVPro", "Módulo de funcionamiento de las cámaras Basler Pilot mediante el driver de VisionPro", 203);
        /// <summary>
        /// Módulo de funcionamiento de las cámaras Axis
        /// </summary>
        public static OEnumModulosSistema CamaraAxis = new OEnumModulosSistema("CamaraAxis", "Módulo de funcionamiento de las cámaras Axis", 204);
        /// <summary>
        /// Módulo de funcionamiento del dispositivo de Entradas/Salidas digitales de las cámaras Axis
        /// </summary>
        public static OEnumModulosSistema ES_Axis = new OEnumModulosSistema("ES_Axis", "Módulo de funcionamiento del dispositivo de Entradas/Salidas digitales de las cámaras Axis", 205);
        /// <summary>
        /// Módulo de funcionamiento del dispositivo de Entradas/Salidas digitales USB1024HLS de Measurement Computing
        /// </summary>                         
        public static OEnumModulosSistema ES_MeasurementComputing = new OEnumModulosSistema("ES_USB1024HLS", "Módulo de funcionamiento del dispositivo de Entradas/Salidas digitales USB1024HLS de Measurement Computing", 206);
        /// <summary>
        /// Módulo de funcionamiento del dispositivo de Entradas/Salidas digitales mediante SCED
        /// </summary>
        public static OEnumModulosSistema ES_SCED = new OEnumModulosSistema("ES_SCED", "Módulo de funcionamiento del dispositivo de Entradas/Salidas digitales mediante SCED", 207);
        /// <summary>
        /// Módulo de funciones base para las imagenes y gráficos de tipo Bitmap
        /// </summary>
        public static OEnumModulosSistema ImagenGraficosBitmap = new OEnumModulosSistema("ImagenGraficosBitmap", "Módulo de funciones base para las imagenes y gráficos de tipo Bitmap", 208);
        /// <summary>
        /// Módulo de funciones base para las imagenes y gráficos de tipo VisionPro
        /// </summary>
        public static OEnumModulosSistema ImagenGraficosVPro = new OEnumModulosSistema("ImagenGraficosVPro", "Módulo de funciones base para las imagenes y gráficos de tipo VisionPro", 209);
        /// <summary>
        /// Módulo de funcionamiento de los PTZs
        /// </summary>
        public static OEnumModulosSistema PTZ = new OEnumModulosSistema("PTZ", "Módulo de funcionamiento de los controladores PTZ", 210);
        /// <summary>
        /// Módulo de funcionamiento de las cámaras Basler mediante el driver propio de Pylon
        /// </summary>
        public static OEnumModulosSistema CamaraBalserPylon = new OEnumModulosSistema("CamaraBalserPylon", "Módulo de funcionamiento de las cámaras Basler mediante el driver de Pylon", 211);
        #endregion
    }
}
