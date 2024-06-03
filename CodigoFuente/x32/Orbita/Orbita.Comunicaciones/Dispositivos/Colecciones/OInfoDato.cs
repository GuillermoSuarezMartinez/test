//***********************************************************************
// Assembly         : OrbitaComunicaciones
// Author           : crodriguez
// Created          : 03-11-2011
//
// Last Modified By : crodriguez
// Last Modified On : 03-15-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;

namespace Orbita.Comunicaciones
{
    /// <summary>
    /// Clase para todos las variables del servidor de comunicaciones
    /// </summary>
    [Serializable]    
    public class OInfoDato
    {
        #region Atributo(s)

        /// <summary>
        /// Identificador de dato.
        /// </summary>
        int _identificador;
        /// <summary>
        /// Tipo.
        /// </summary>
        string _tipo;
        /// <summary>
        /// Texto.
        /// </summary>
        string _texto;
        /// <summary>
        /// String de conexión.
        /// </summary>
        string _conexion;
        /// <summary>
        /// DB.
        /// </summary>
        int _db;
        /// <summary>
        /// Dirección de DB.
        /// </summary>
        int _direccion;
        /// <summary>
        /// Número de bit.
        /// </summary>
        int _bit;
        /// <summary>
        /// Valor de tipo String.
        /// </summary>
        object _valor;
        /// <summary>
        /// Descripción.
        /// </summary>
        string _descripcion;
        /// <summary>
        /// Descripción del enlace.
        /// </summary>
        string _enlace;
        /// <summary>
        /// Último valor.
        /// </summary>
        object _ultimoValor;
        /// <summary>
        /// Calidad.
        /// </summary>
        string _calidad;
        /// <summary>
        /// Indica si es alarma.
        /// </summary>
        bool _esAlarma;
        /// <summary>
        /// Indica si es visualización.
        /// </summary>
        bool _esVisualizacion;
        /// <summary>
        /// Indica si es lectura.
        /// </summary>
        bool _esLectura;
        /// <summary>
        /// Identificador de lectura.
        /// </summary>
        int _idLectura;
        /// <summary>
        /// El tipo de error.
        /// </summary>
        int _error;
        /// <summary>
        /// El dispositivo.
        /// </summary>
        int _dispositivo;
        /// <summary>
        /// Indica si es una entrada
        /// </summary>
        bool _esEntrada;       
        
        #endregion

        #region Constructor(es)

        /// <summary>
        /// Inicializar una nueva instancia de la clase DatosOPC.
        /// </summary>
        public OInfoDato()
        {
            this._esAlarma = false;
            this._esVisualizacion = false;
            this._esLectura = false;
        }

        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Identificador.
        /// </summary>
        public int Identificador
        {
            get { return this._identificador; }
            set { this._identificador = value; }
        }
        /// <summary>
        /// Identificador de lectura.
        /// </summary>
        public int IdLectura
        {
            get { return this._idLectura; }
            set { this._idLectura = value; }
        }
        /// <summary>
        /// Conexión.
        /// </summary>
        public string Conexion
        {
            get { return this._conexion; }
            set { this._conexion = value; }
        }
        /// <summary>
        /// DB.
        /// </summary>
        public int DB
        {
            get { return this._db; }
            set { this._db = value; }
        }
        /// <summary>
        /// Direccion.
        /// </summary>
        public int Direccion
        {
            get { return this._direccion; }
            set { this._direccion = value; }
        }
        /// <summary>
        /// Bit.
        /// </summary>
        public int Bit
        {
            get { return this._bit; }
            set { this._bit = value; }
        }
        /// <summary>
        /// Texto.
        /// </summary>
        public string Texto
        {
            get { return this._texto; }
            set { this._texto = value; }
        }
        /// <summary>
        /// Descripción.
        /// </summary>
        public string Descripcion
        {
            get { return this._descripcion; }
            set { this._descripcion = value; }
        }
        /// <summary>
        /// Enlace.
        /// </summary>
        public string Enlace
        {
            get { return this._enlace; }
            set { this._enlace = value; }
        }
        /// <summary>
        /// Tipo.
        /// </summary>
        public string Tipo
        {
            get { return this._tipo; }
            set { this._tipo = value; }
        }
        /// <summary>
        /// Valor.
        /// </summary>
        public object Valor
        {
            get { return this._valor; }
            set { this._valor = value; }
        }
        /// <summary>
        /// Último valor.
        /// </summary>
        public object UltimoValor
        {
            get { return this._ultimoValor; }
            set { this._ultimoValor = value; }
        }
        /// <summary>
        /// Error.
        /// </summary>
        public int Error
        {
            get { return this._error; }
            set { this._error = value; }
        }
        /// <summary>
        /// Calidad.
        /// </summary>
        public string Calidad
        {
            get { return this._calidad; }
            set { this._calidad = value; }
        }
        /// <summary>
        /// Alarma.
        /// </summary>
        public bool ESAlarma
        {
            get { return this._esAlarma; }
            set { this._esAlarma = value; }
        }
        /// <summary>
        /// Lectura.
        /// </summary>
        public bool ESLectura
        {
            get { return this._esLectura; }
            set { this._esLectura = value; }
        }
        /// <summary>
        /// Visualización.
        /// </summary>
        public bool ESVisualizacion
        {
            get { return this._esVisualizacion; }
            set { this._esVisualizacion = value; }
        }
        /// <summary>
        /// Dispositivo.
        /// </summary>
        public int Dispositivo
        {
            get { return this._dispositivo; }
            set { this._dispositivo = value; }
        }
        /// <summary>
        /// Indica si la variable es una entrada
        /// </summary>
        public bool EsEntrada
        {
            get { return _esEntrada; }
            set { _esEntrada = value; }
        }
        #endregion
    }
}
