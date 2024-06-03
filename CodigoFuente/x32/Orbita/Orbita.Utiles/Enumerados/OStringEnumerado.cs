//***********************************************************************
// Assembly         : OrbitaUtiles
// Author           : vnicolau
// Created          : 03-03-2011
//
// Last Modified By : crodriguez
// Last Modified On : 03-03-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
namespace Orbita.Utiles
{
    /// <summary>
    /// Clases para dotar de atributos a los tipos enumerados.
    /// </summary>
    public class OStringEnumerado
    {
        #region Atributo(s)
        /// <summary>
        /// Tipo.
        /// </summary>
        Type _tipo;
        /// <summary>
        /// Valores.
        /// </summary>
        static Hashtable _valores = new Hashtable();
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Creates a new <see cref="OStringEnumerado"/> instance.
        /// </summary>
        /// <param name="tipo">Enum type.</param>
        public OStringEnumerado(Type tipo)
        {
            if (!tipo.IsEnum)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "El tipo proporcionado debe ser un enumerado.  Tipo es: {0}", tipo));
            }
            this._tipo = tipo;
        }
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Gets the underlying enum type for this instance.
        /// </summary>
        /// <value></value>
        public Type TipoEnumerado
        {
            get { return this._tipo; }
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Gets the string value associated with the given enum value.
        /// </summary>
        /// <param name="valor">Name of the enum value.</param>
        /// <returns>String Value</returns>
        public string GetValorString(string valor)
        {
            Enum enumType;
            string stringValue = null;
            enumType = (Enum)Enum.Parse(this._tipo, valor);
            stringValue = GetValorString(enumType);

            return stringValue;
        }
        /// <summary>
        /// Gets the string values associated with the enum.
        /// </summary>
        /// <returns>String value array</returns>
        public Array GetValoresString()
        {
            ArrayList values = new ArrayList();
            //Look for our string value associated with fields in this enum
            foreach (FieldInfo fi in this._tipo.GetFields())
            {
                //Check for our custom attribute
                OAtributoEnumerado[] attrs = fi.GetCustomAttributes(typeof(OAtributoEnumerado), false) as OAtributoEnumerado[];
                if (attrs.Length > 0)
                {
                    values.Add(attrs[0].Valor);
                }
            }
            return values.ToArray();
        }
        /// <summary>
        /// Gets the values as a 'bindable' list datasource.
        /// </summary>
        /// <returns>IList for data binding</returns>
        public IList GetListaValores()
        {
            Type underlyingType = Enum.GetUnderlyingType(this._tipo);
            ArrayList values = new ArrayList();
            //Look for our string value associated with fields in this enum
            foreach (FieldInfo fi in this._tipo.GetFields())
            {
                //Check for our custom attribute
                OAtributoEnumerado[] attrs = fi.GetCustomAttributes(typeof(OAtributoEnumerado), false) as OAtributoEnumerado[];
                if (attrs.Length > 0)
                {
                    values.Add(new DictionaryEntry(Convert.ChangeType(Enum.Parse(this._tipo, fi.Name), underlyingType, CultureInfo.CurrentCulture), attrs[0].Valor));
                }
            }
            return values;
        }
        /// <summary>
        /// Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="valor">String value.</param>
        /// <returns>Existence of the string value</returns>
        public bool StringDefinido(string valor)
        {
            return Parse(this._tipo, valor) != null;
        }
        /// <summary>
        /// Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="valor">String value.</param>
        /// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
        /// <returns>Existence of the string value</returns>
        public bool StringDefinido(string valor, bool ignoreCase)
        {
            return Parse(this._tipo, valor, ignoreCase) != null;
        }
        #region Método(s) estático(s)
        /// <summary>
        /// Gets a string value for a particular enum value.
        /// </summary>
        /// <param name="valor">Value.</param>
        /// <returns>String Value associated via a <see cref="OAtributoEnumerado"/> attribute, or null if not found.</returns>
        public static string GetValorString(Enum valor)
        {
            string output = null;
            Type type = valor.GetType();

            if (_valores.ContainsKey(valor))
                output = (_valores[valor] as OAtributoEnumerado).Valor;
            else
            {
                //Look for our 'StringValueAttribute' in the field's custom attributes
                FieldInfo fi = type.GetField(valor.ToString());
                OAtributoEnumerado[] attrs = fi.GetCustomAttributes(typeof(OAtributoEnumerado), false) as OAtributoEnumerado[];
                if (attrs.Length > 0)
                {
                    _valores.Add(valor, attrs[0]);
                    output = attrs[0].Valor;
                }
            }
            return output;
        }
        /// <summary>
        /// Parses the supplied enum and string value to find an associated enum value (case sensitive).
        /// </summary>
        /// <param name="tipo">Type.</param>
        /// <param name="valor">String value.</param>
        /// <returns>Enum value associated with the string value, or null if not found.</returns>
        public static object Parse(Type tipo, string valor)
        {
            return Parse(tipo, valor, false);
        }
        /// <summary>
        /// Parses the supplied enum and string value to find an associated enum value.
        /// </summary>
        /// <param name="tipo">Type.</param>
        /// <param name="valor">String value.</param>
        /// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
        /// <returns>Enum value associated with the string value, or null if not found.</returns>
        public static object Parse(Type tipo, string valor, bool ignoreCase)
        {
            object output = null;
            string enumStringValue = null;

            if (!tipo.IsEnum)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "El tipo proporcionado debe ser un enumerado.  Tipo es: {0}", tipo));
            }
            //Look for our string value associated with fields in this enum
            foreach (FieldInfo fi in tipo.GetFields())
            {
                //Check for our custom attribute
                OAtributoEnumerado[] attrs = fi.GetCustomAttributes(typeof(OAtributoEnumerado), false) as OAtributoEnumerado[];
                if (attrs.Length > 0)
                {
                    enumStringValue = attrs[0].Valor;
                }
                //Check for equality then select actual enum value.
                if (string.Compare(enumStringValue, valor, ignoreCase, CultureInfo.CurrentCulture) == 0)
                {
                    output = Enum.Parse(tipo, fi.Name);
                    break;
                }
            }
            return output;
        }
        /// <summary>
        /// Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="valor">String value.</param>
        /// <param name="tipo">Type of enum</param>
        /// <returns>Existence of the string value</returns>
        public static bool StringDefinido(Type tipo, string valor)
        {
            return Parse(tipo, valor) != null;
        }
        /// <summary>
        /// Return the existence of the given string value within the enum.
        /// </summary>
        /// <param name="valor">String value.</param>
        /// <param name="tipo">Type of enum</param>
        /// <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
        /// <returns>Existence of the string value</returns>
        public static bool StringDefinido(Type tipo, string valor, bool ignoreCase)
        {
            return Parse(tipo, valor, ignoreCase) != null;
        }
        #endregion
        #endregion
    }
}
