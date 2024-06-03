//***********************************************************************
// Assembly         : Orbita.VAControl
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

namespace Orbita.VAControl
{
    /// <summary>
    /// Define el conjunto de módulos del sistema
    /// </summary>
    public class OModulosControl: OModulosSistema
    {
        #region Atributo(s)
        /// <summary>
        /// Módulo de las máquinas de estado
        /// </summary>
        public static OEnumModulosSistema MaquinasEstado = new OEnumModulosSistema("MaquinasEstado", "Módulo de funcionamiento de las máquinas de estado", 101);
        /// <summary>
        /// Módulo de las máquinas de estado
        /// </summary>
        public static OEnumModulosSistema MonitorizacionMaquinasEstado = new OEnumModulosSistema("MonitorizacionMaquinasEstado", "Módulo de monitorización de las máquinas de estado", 102);
        /// <summary>
        /// Módulo de edición y gestión de las máquinas de estado
        /// </summary>
        public static OEnumModulosSistema GestionMaquinasEstado = new OEnumModulosSistema("GestionMaquinasEstado", "Módulo de gestión de las máquinas de estado", 103);
        /// <summary>
        /// Módulo de las variables del sistema
        /// </summary>
        public static OEnumModulosSistema Variables = new OEnumModulosSistema("Variables", "Módulo de funcionamiento de las variables del sistema", 104);
        /// <summary>
        /// Módulo de las variables del sistema
        /// </summary>
        public static OEnumModulosSistema MonitorizacionVariables = new OEnumModulosSistema("MonitorizacionVariables", "Módulo de monitorización de las variables del sistema", 105);
        /// <summary>
        /// Módulo de edición y gestión de las variables del sistema
        /// </summary>
        public static OEnumModulosSistema GestionVariables = new OEnumModulosSistema("GestionVariables", "Módulo de gestión de las variables del sistema", 106);
        #endregion
    }
}
