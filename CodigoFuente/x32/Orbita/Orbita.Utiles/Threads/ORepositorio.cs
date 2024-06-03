//***********************************************************************
// Assembly         : OrbitaUtiles
// Author           : crodriguez
// Created          : 03-03-2011
//
// Last Modified By : crodriguez
// Last Modified On : 03-03-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
namespace Orbita.Utiles
{
    /// <summary>
    /// Repositorio de hilos.
    /// </summary>
    public sealed class ORepositorio
    {
        #region Atributos
        /// <summary>
        /// Repositorio de hilos.
        /// </summary>
        static OHilos shilos;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor estático de la clase Repositorio.
        /// </summary>
        ORepositorio() { }
        #endregion

        #region Método(s) público(s)

        #region Método(s) Estático(s)
        /// <summary>
        /// Obtener la colección de hilos.
        /// </summary>
        /// <returns></returns>
        public static OHilos GetHilos()
        {
            return shilos;
        }
        /// <summary>
        /// Asignar la colección de hilos.
        /// </summary>
        /// <param name="hilos">Colección de hilos.</param>
        public static void SetHilos(OHilos hilos)
        {
            shilos = hilos;
        }
        #endregion

        #endregion
    }
}
