//***********************************************************************
// Assembly         : Orbita.BBDD
// Author           : crodriguez
// Created          : 02-21-2011
//
// Last Modified By : crodriguez
// Last Modified On : 02-21-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections;
namespace Orbita.BBDD
{
    /// <summary>
    /// Colección de OConexion.
    /// </summary>
    public class OConexiones
    {
        #region Atributos privados
        /// <summary>
        /// Colección de OConexiones.
        /// </summary>
        Hashtable conexiones;
        #endregion

        #region Constructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase OConexiones.
        /// </summary>
        public OConexiones()
        {
            // Crear la colección 'Hashtable'.
            this.conexiones = new Hashtable();
        }
        #endregion

        #region Métodos públicos
        /// <summary>
        /// Adicionar la conexión a la colección.
        /// </summary>
        /// <param name="info">Información de la conexión.</param>
        /// <returns> Nueva conexión creada.</returns>
        public virtual OConexion this[string info]
        {
            get
            {
                // Bloquear esta propiedad por si se usan Threads (17/Jul/10).
                lock (this.conexiones)
                {
                    // Crear una nueva conexión.
                    OConexion conexion = new OConexion(info);

                    // Asignar identificador de transacción.
                    conexion.Identificador = System.Guid.NewGuid();

                    // Añadir a la colección.
                    this.conexiones.Add(conexion.Identificador, conexion);
                    return conexion;
                }
            }
        }
        /// <summary>
        /// Obtener el objeto oconexion de la colección.
        /// </summary>
        /// <param name="identificador">Identificador de conexión.</param>
        /// <returns>Conexión de la colección.</returns>
        public virtual OConexion this[Guid identificador]
        {
            get
            {
                // Bloquear esta propiedad por si se usan Threads (17/Jul/10).
                lock (this.conexiones)
                {
                    OConexion resultado = null;
                    if (this.conexiones.ContainsKey(identificador))
                    {
                        resultado = (OConexion)this.conexiones[identificador];
                    }
                    return resultado;
                }
            }
        }
        #endregion
    }
}
