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
using System.Data;
using Orbita.VAComun;

namespace Orbita.VAControl
{
    /// <summary>
    /// Escenario Base.
    /// Permite la utilización de vistas.
    /// </summary>
    public class Escenario
    {
        #region Atributo(s)
        /// <summary>
        /// Código del escenario
        /// </summary>
        public string Codigo;

        /// <summary>
        /// Claves del escenario
        /// </summary>
        public Claves Claves;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codigo"></param>
        public Escenario(string codigo)
        {
            this.Codigo = codigo;
            this.Claves = new Claves();

            // Consulta las claves de una determinada vista
            DataTable dt = AppBD.GetClavesDeVista(this.Codigo);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    this.Claves.Add(new Clave(dr));
                }
            }
        }
        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Inicializa la clase
        /// </summary>
        public virtual void Inicializar()
        {
            // Implementado en heredados
        }

        /// <summary>
        /// Finaliza la clase
        /// </summary>
        public virtual void Finalizar()
        {
            // Implementado en heredados
        }
        #endregion
    }
}
