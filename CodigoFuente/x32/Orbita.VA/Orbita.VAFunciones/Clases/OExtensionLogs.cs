//***********************************************************************
// Assembly         : Orbita.VAFunciones
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

namespace Orbita.VAFunciones
{
    /// <summary>
    /// Define el conjunto de módulos del sistema
    /// </summary>
    public class OModulosFunciones: OModulosSistema
    {
        #region Atributo(s)
        /// <summary>
        /// Módulo de las funciones de visión
        /// </summary>
        public static OEnumModulosSistema Vision = new OEnumModulosSistema("Vision", "Módulo de las funciones de visión", 301);
        /// <summary>
        /// Módulo de Vision Pro
        /// </summary>
        public static OEnumModulosSistema VisionPro = new OEnumModulosSistema("VisionPro", "Módulo de Vision Pro", 302);
        /// <summary>
        /// Módulo de CCR. Lectura de mátriculas de contenedores
        /// </summary>
        public static OEnumModulosSistema CCRContainer = new OEnumModulosSistema("CCRContainer", "Módulo de lectura de las matrículas de los contenedoress", 303);
        /// <summary>
        /// Módulo de LPR de vehículos
        /// </summary>
        public static OEnumModulosSistema LPR = new OEnumModulosSistema("LPR", "Módulo de lectura de matrículas de vehículos", 304);
        /// <summary>
        /// Módulo de los mantenimientos de las funciones de visión
        /// </summary>
        public static OEnumModulosSistema Mantenimientos = new OEnumModulosSistema("Mantenimientos", "Módulo de los mantenimientos de las funciones de visión", 305);
        /// <summary>
        /// Módulo de las inspecciones del sistema
        /// </summary>
        public static OEnumModulosSistema Inspeccion = new OEnumModulosSistema("Inspeccion", "Módulo de las inspecciones del sistema", 306);
        #endregion
    }
}
