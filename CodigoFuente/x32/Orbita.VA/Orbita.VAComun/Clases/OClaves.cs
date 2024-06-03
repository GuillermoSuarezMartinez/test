//***********************************************************************
// Assembly         : Orbita.VAComun
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections.Generic;
using System.Data;

namespace Orbita.VAComun
{
    /// <summary>
    /// Clase que agrupa un conjunto de claves
    /// </summary>
    public class OClaves: Dictionary<string, OClave>
    {
        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OClaves()
            : base()
        { }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="claves"></param>
        public OClaves(OClaves claves): base(claves)
        {}
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Añade una clave
        /// </summary>
        /// <param name="clave"></param>
        public void Add(OClave clave)
        {
            this.Add(clave.Codigo, clave);
        }

        /// <summary>
        /// Modifica el valor de una clave
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="valor"></param>
        public bool ModificaValorClave(string codigo, object valor)
        {
            bool resultado = false;

            OClave clave;
            if (this.TryGetValue(codigo, out clave))
            {
                clave.Valor = valor;
                resultado = true;
            }

            return resultado;
        }

        /// <summary>
        /// Compara el valor de una clave
        /// </summary>
        /// <param name="codigo"></param>
        /// <param name="valor"></param>
        public bool CompararValorClave(string codigo, object valor)
        {
            bool resultado = false;

            OClave clave;
            if (this.TryGetValue(codigo, out clave))
            {
                resultado = clave.Valor == valor;
            }

            return resultado;
        }

        /// <summary>
        /// Realiza una comparación de las claves actuales con las que se pasan por parametro
        /// </summary>
        /// <param name="claves"></param>
        /// <returns></returns>
        public bool CompararClaves(OClaves claves)
        {
            bool resultado = true;

            foreach (KeyValuePair<string, OClave> clavePair in this)
            {
                OClave clavePropia = clavePair.Value;
                if (clavePropia.ValorDefinido)
                {
                    OClave claveAjena;
                    if (!claves.TryGetValue(clavePair.Key, out claveAjena))
                    {
                        resultado = false;
                        break;
                    }

                    if (!clavePropia.Valor.Equals(claveAjena.Valor))
                    {
                        resultado = false;
                        break;
                    }
                }
            }

            return resultado;
        }

        /// <summary>
        /// Devuelve las claves en un diccionario de string a objetos
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,object> ToDictionary()
        {
            Dictionary<string, object> resultado = new Dictionary<string, object>();

            foreach (KeyValuePair<string, OClave> pair in this)
            {
                resultado.Add(pair.Key, pair.Value.Valor);
            }

            return resultado;
        }
        #endregion
    }

    /// <summary>
    /// Clase que contiene un valor clave
    /// </summary>
    public class OClave
    {
        #region Propiedad(es)
        /// <summary>
        /// Código identificativo de la clave.
        /// </summary>
        private string _Codigo;
        /// <summary>
        /// Código identificativo de la clave.
        /// </summary>
        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }

        /// <summary>
        /// Nombre de la clave.
        /// </summary>
        private string _Nombre;
        /// <summary>
        /// Nombre de la clave.
        /// </summary>
        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        /// <summary>
        /// Texto descriptivo.
        /// </summary>
        private string _Descripcion;
        /// <summary>
        /// Texto descriptivo.
        /// </summary>
        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }

        /// <summary>
        /// Tipo de dato
        /// </summary>
        private OEnumTipoDato _Tipo;
        /// <summary>
        /// Tipo de dato
        /// </summary>
        public OEnumTipoDato Tipo
        {
            get { return _Tipo; }
            set { _Tipo = value; }
        }


        /// <summary>
        /// Valor de la clave
        /// </summary>
        private object _Valor;
        /// <summary>
        /// Valor de la clave
        /// </summary>
        public object Valor
        {
            get { return _Valor; }
            set { _Valor = value; }
        }

        /// <summary>
        /// Indica si la clave tiene un valor definido
        /// </summary>
        private bool _ValorDefinido;
        /// <summary>
        /// Indica si la clave tiene un valor definido
        /// </summary>
        public bool ValorDefinido
        {
            get { return _ValorDefinido; }
            set { _ValorDefinido = value; }
        }

        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codigo">Código identificativo de la clave.</param>
        /// <param name="nombre">Nombre de la clave</param>
        /// <param name="descripcion">Texto descriptivo</param>
        /// <param name="valor">Valor de la clave</param>
        /// <param name="valorDefinido">Indica si la clave tiene un valor definido</param>
        public OClave(string codigo, string nombre, string descripcion, OEnumTipoDato tipo, object valor, bool valorDefinido)
        {
            this._Codigo = codigo;
            this._Nombre = nombre;
            this._Descripcion = descripcion;
            this.Tipo = tipo;
            this._Valor = valor;
            this._ValorDefinido = valorDefinido;
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codigo">Código identificativo de la clave.</param>
        /// <param name="nombre">Nombre de la clave</param>
        /// <param name="descripcion">Texto descriptivo</param>
        /// <param name="valor">Valor de la clave</param>
        /// <param name="valorDefinido">Indica si la clave tiene un valor definido</param>
        public OClave(DataRow dr)
        {
            this._Codigo = dr["CodClave"].ToString();
            this._Nombre = dr["NombreClave"].ToString();
            this._Descripcion = dr["DescClave"].ToString();
            this.Tipo = (OEnumTipoDato)App.EvaluaNumero(dr["IdTipoValorClave"], 0, 99, 0);
            this._Valor = null;
            switch (this.Tipo)
            {
                case OEnumTipoDato.Bit:
                    this._Valor = App.EvaluaBooleano(dr["ValorBit"], false);
                    break;
                case OEnumTipoDato.Entero:
                    this._Valor = App.EvaluaNumero(dr["ValorEntero"], int.MinValue, int.MaxValue, 0);
                    break;
                case OEnumTipoDato.Texto:
                    this._Valor = dr["ValorTexto"].ToString();
                    break;
                case OEnumTipoDato.Decimal:
                    this._Valor = App.EvaluaNumero(dr["ValorDecimal"], double.MinValue, double.MaxValue, 0);
                    break;
                case OEnumTipoDato.Fecha:
                    this._Valor = App.EvaluaFecha(dr["ValorFecha"], DateTime.Now);
                    break;
            }
            this._ValorDefinido = App.EvaluaBooleano(dr["ValorFijo"], false);
        }
        #endregion
    }
}
