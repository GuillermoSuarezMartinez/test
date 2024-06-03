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
    public class ModulosFunciones: ModulosSistema
    {
        #region Atributo(s)
        /// <summary>
        /// Módulo de las funciones de visión
        /// </summary>
        public static EnumModulosSistema Vision = new EnumModulosSistema("Vision", "Módulo de las funciones de visión", 301);
        /// <summary>
        /// Módulo de Vision Pro
        /// </summary>
        public static EnumModulosSistema VisionPro = new EnumModulosSistema("VisionPro", "Módulo de Vision Pro", 302);
        /// <summary>
        /// Módulo de CCR. Lectura de mátriculas de contenedores
        /// </summary>
        public static EnumModulosSistema CCRContainer = new EnumModulosSistema("CCRContainer", "Módulo de lectura de las matrículas de los contenedoress", 303);
        /// <summary>
        /// Módulo de LPR de vehículos
        /// </summary>
        public static EnumModulosSistema LPR = new EnumModulosSistema("LPR", "Módulo de lectura de matrículas de vehículos", 304);
        /// <summary>
        /// Módulo de los mantenimientos de las funciones de visión
        /// </summary>
        public static EnumModulosSistema Mantenimientos = new EnumModulosSistema("Mantenimientos", "Módulo de los mantenimientos de las funciones de visión", 305);
        /// <summary>
        /// Módulo de las inspecciones del sistema
        /// </summary>
        public static EnumModulosSistema Inspeccion = new EnumModulosSistema("Inspeccion", "Módulo de las inspecciones del sistema", 306);
        #endregion
    }
}
