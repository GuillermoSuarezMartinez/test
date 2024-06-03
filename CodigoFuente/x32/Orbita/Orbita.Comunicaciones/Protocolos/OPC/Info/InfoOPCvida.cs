//***********************************************************************
// Assembly         : OrbitaComunicaciones
// Author           : crodriguez
// Created          : 03-11-2011
//
// Last Modified By : crodriguez
// Last Modified On : 03-14-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
namespace Orbita.Comunicaciones
{
    /// <summary>
    /// Clase para la generación de eventos de bit de vida
    /// </summary>
    public class OInfoOPCvida
    {
        #region Atributo(s)

        /// <summary>
        /// Identificador de variables.
        /// </summary>
        int[] _variables;
        /// <summary>
        /// Valor de las variables.
        /// </summary>
        string[] _valores;


        /// <summary>
        /// Enlace de conexión.
        /// </summary>
        string[] _enlaces;

        #endregion

        #region Constructor(es)

        /// <summary>
        /// Inicializar una nueva instancia de la clase InfoVida.
        /// </summary>
        /// <param name="contador">Número de elementos.</param>
        /// <param name="enlaces">Descripción del enlace.</param>
        public OInfoOPCvida(int contador, string[] enlaces)
        {
            this._variables = new int[contador];
            this._valores = new string[contador];
            this._enlaces = enlaces;
        }

        #endregion

        #region Propiedad(es)

        /// <summary>
        /// Enlace de conexión.
        /// </summary>
        public string[] Enlaces
        {
            get { return this._enlaces; }
            set { this._enlaces = value; }
        }
        /// <summary>
        /// Variables
        /// </summary>
        public int[] Variables
        {
            get { return _variables; }
            set { _variables = value; }
        }
        /// <summary>
        /// Valores
        /// </summary>
        public string[] Valores
        {
            get { return _valores; }
            set { _valores = value; }
        }
        #endregion

        #region Método(s) público(s)

        /// <summary>
        /// Obtener la colección de variables.
        /// </summary>
        /// <returns></returns>
        public int[] GetVariables()
        {
            return this._variables;
        }
        /// <summary>
        /// Obtener el elemento i-esimo de la
        /// colección de variables.
        /// </summary>
        /// <param name="indice">Indice de la
        /// colección de variables.</param>
        /// <returns>El objeto de la colección.</returns>
        public object GetVariables(int indice)
        {
            return this._variables[indice];
        }

        /// <summary>
        /// Asignar la colección de variables.
        /// </summary>
        /// <param name="indice">Indice de la colección.</param>
        /// <param name="variable">Variable a almacenar.</param>
        public void SetVariables(int indice, int variable)
        {
            this._variables[indice] = variable;
        }

        /// <summary>
        /// Obtener la colección de valores.
        /// </summary>
        /// <returns></returns>
        public string[] GetValores()
        {
            return this._valores;
        }

        /// <summary>
        /// Asignar la colección de valores.
        /// </summary>
        public void SetValores(string[] valores)
        {
            this._valores = valores;
        }

        #endregion
    }
}
