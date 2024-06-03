//***********************************************************************
// Assembly         : Orbita.VAComun
// Author           : aibañez
// Created          : 06-09-2012
//
// Pendiente        : Los valores decimales deben soportar tambien strings
//
// Last Modified By : aibañez
// Last Modified On : 12-12-2012
// Description      : Modificados validaciones de enteros y enumerados 
//                    para soportar valores string
//                    Cambiado el log de Info por Debug
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Globalization;

namespace Orbita.VAComun
{
    #region ORobusto
    /// <summary>
    /// Clase estática destinada a alojar métodos genéricos para el manejo de objetos de forma segura
    /// </summary>
    public static class ORobusto
    {
        #region Método(s) estático(s)
        /// <summary>
        /// Función que devuelve si el objeto pertenece a alguno de los tipos listados
        /// </summary>
        /// <param name="o">Objeto que se quiere conocer el tipo</param>
        /// <param name="types">Vector de tipos con lo que se ha de comparar el tipo del objeto</param>
        /// <returns>Verdadero si el tipo del objeto está dentro de la lista de tipos pasados como parámetros</returns>
        public static bool IsTypeOf(object o, params Type[] types)
        {
            foreach (Type t in types)
            {
                if (o.GetType() == t)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Indica si el objeto pasado es de tipo numérico
        /// </summary>
        /// <param name="o">Objeto que se quiere conocer si es de tipo numérico</param>
        /// <returns>Verdadero si el tipo del objeto es numérico</returns>
        public static bool IsNumeric(object o)
        {
            return (o != null) && IsTypeOf(o, typeof(int), typeof(short), typeof(long), typeof(uint), typeof(ushort), typeof(ulong), typeof(byte), typeof(float), typeof(double), typeof(decimal));
        }

        /// <summary>
        /// Convierte un objeto a string
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static string ToString(object valor)
        {
            return valor != null ? valor.ToString() : string.Empty;
        }

        /// <summary>
        /// Realiza una comparación entre dos objetos
        /// </summary>
        /// <param name="valor1">Primer objeto a comparar</param>
        /// <param name="valor2">Segundo objeto a comparar</param>
        /// <returns></returns>
        public static bool CompararObjetos(object valor1, object valor2)
        {
            bool resultado = false;

            if ((valor1 == null) && (valor2 == null))
            {
                // Ambos son null
                return true;
            }

            if ((valor1 != null) && (valor2 != null))
            {
                if (valor1.GetType() != valor2.GetType())
                {
                    // No son del mismo tipo
                    return false;
                }

                if (valor1 is byte[])
                {
                    byte[] valorByte1 = (byte[])valor1;
                    byte[] valorByte2 = (byte[])valor2;

                    if (valorByte1.Length != valorByte2.Length)
                    {
                        // No tienen la misma longitud
                        return false;
                    }

                    for (int i = 0; i < valorByte1.Length; i++)
                    {
                        if (valorByte1[i] != valorByte2[i])
                        {
                            // Tienen algun valor distinto
                            return false;
                        }
                    }

                    // Los arrays son iguales
                    return true;
                }

                return valor1.Equals(valor2);
            }

            return resultado;
        }
        #endregion
    }
    #endregion

    #region Objeto Robusto
    /// <summary>
    /// Clase base para el trabajo de forma robusta con variables
    /// </summary>
    public class OObjetoRobusto<ClaseTipo>
    {
        #region Atributo(s)
        /// <summary>
        /// Indica que se ha de lanzar una excepción de tipo InvalidValueException cuando el valor a establecer no sea el correcto
        /// </summary>
        public bool LanzarExcepcionSiValorNoValido;
        /// <summary>
        /// Valor por defecto del objeto
        /// </summary>
        public ClaseTipo ValorPorDefecto;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Valor del objeto
        /// </summary>
        protected ClaseTipo _Valor;
        /// <summary>
        /// Valor del objeto
        /// </summary>
        public ClaseTipo Valor
        {
            get
            {
                return _Valor;
            }
            set
            {
                this.ValorGenerico = value;
            }
        }

        /// <summary>
        /// Código identificativo de la clase.
        /// </summary>
        private string _Codigo;
        /// <summary>
        /// Código identificativo de la clase.
        /// </summary>
        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }

        /// <summary>
        /// Indica que el valor asignado es válido
        /// </summary>
        public bool Valido
        {
            get { return this.Estado == EnumEstadoObjetoRobusto.ResultadoCorrecto; }
        }

        /// <summary>
        /// Estado del valor actual
        /// </summary>
        private EnumEstadoRobusto _Estado;
        /// <summary>
        /// Estado del valor actual
        /// </summary>
        public EnumEstadoRobusto Estado
        {
            get { return _Estado; }
            set { _Estado = value; }
        }

        /// <summary>
        /// Valor del objeto
        /// </summary>
        public object ValorGenerico
        {
            set
            {
                // Validación
                object refObj = value;
                this.Estado = this.Validar(ref refObj);

                // Excepción
                if (this.LanzarExcepcionSiValorNoValido && !this.Valido)
                {
                    this.LanzarExcepcion();
                }

                // Asignación
                this._Valor = (ClaseTipo)refObj;

                if (!this.Valido)
                {
                    // Registro
                    OVALogsManager.Debug(ModulosSistema.Comun, "Obtener Valor de forma segura", "No admite el valor " + ORobusto.ToString(value));
                }
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OObjetoRobusto(string codigo, ClaseTipo valorDefecto, bool lanzarExcepcionSiValorNoValido)
        {
            this.Codigo = codigo;
            this.LanzarExcepcionSiValorNoValido = lanzarExcepcionSiValorNoValido;
            this.ValorPorDefecto = valorDefecto;
            this._Valor = valorDefecto;
            this.Estado = EnumEstadoObjetoRobusto.ResultadoCorrecto;
        }
        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="valor">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public virtual EnumEstadoRobusto Validar(ref object valor)
        {
            // Inicialización de resultados
            EnumEstadoRobusto validacion = EnumEstadoTextoRobusto.ValorTipoIncorrecto;
            bool correcto = false;
            ClaseTipo outValor = this.ValorPorDefecto;

            // Validación
            if (correcto)
            {
                if (valor is ClaseTipo)
                {
                    outValor = (ClaseTipo)valor;
                    validacion = EnumEstadoTextoRobusto.ResultadoCorrecto;
                    correcto = true;
                }
            }

            // Devolución de resultados
            valor = outValor;
            return validacion;
        }

        /// <summary>
        /// Lanza una exepción por no estár permitido el valor especificado
        /// </summary>
        /// <param name="valor">valor no permitido</param>
        public virtual void LanzarExcepcion()
        {
            // Implementado en heredados
        }
        #endregion
    }

    /// <summary>
    /// Define el conjunto de módulos del sistema
    /// </summary>
    public class EnumEstadoObjetoRobusto : OEnumeradosHeredable
    {
        #region Atributo(s)
        /// <summary>
        /// Módulo de funciones comunes del sistema. 
        /// </summary>
        public static EnumEstadoRobusto ResultadoCorrecto = new EnumEstadoRobusto("ResultadoCorrecto", "ResultadoCorrecto", 1);
        /// <summary>
        /// El valor a asignar no es del tipo correcto
        /// </summary>
        public static EnumEstadoRobusto ValorTipoIncorrecto = new EnumEstadoRobusto("ValorTipoIncorrecto", "ValorTipoIncorrecto", 2);
        #endregion
    }

    /// <summary>
    /// Clase que implementa el enumerado de los módulos del sistema
    /// </summary>
    public class EnumEstadoRobusto : OEnumeradoHeredable
    {
        #region Constructor
        /// <summary>
        /// Constuctor de la clase
        /// </summary>
        public EnumEstadoRobusto(string nombre, string descripcion, int valor) :
            base(nombre, descripcion, valor)
        { }
        #endregion
    }
    #endregion

    #region Texto Robusto
    /// <summary>
    /// Asignación de una variable a un campo de tipo texto
    /// </summary>
    public class OTextoRobusto : OObjetoRobusto<string>
    {
        #region Atributo(s)
        /// <summary>
        /// Máxima longitud del string
        /// </summary>
        public int MaxLength;
        /// <summary>
        /// Indica que es admitido como válido la cadena vacia
        /// </summary>
        public bool AdmiteVacio;
        /// <summary>
        /// Corta el texto en caso de sobreparsar la longitud máxima
        /// </summary>
        public bool LimitarLongitud;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OTextoRobusto(string codigo, int maxLength, bool admiteVacio, bool limitarLongitud, string valorDefecto, bool lanzarExcepcionSiValorNoValido)
            : base(codigo, valorDefecto, lanzarExcepcionSiValorNoValido)
        {
            this.MaxLength = maxLength;
            this.AdmiteVacio = admiteVacio;
            this.LimitarLongitud = limitarLongitud;
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="valor">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override EnumEstadoRobusto Validar(ref object valor)
        {
            // Valores por defecto
            EnumEstadoRobusto validacion = EnumEstadoTextoRobusto.ResultadoCorrecto;
            bool correcto = true;
            string outValor = this.ValorPorDefecto;
            string auxValor = this.ValorPorDefecto;

            // Validación nº 1
            if (correcto)
            {
                if (!(valor is string))
                {
                    validacion = EnumEstadoTextoRobusto.ValorTipoIncorrecto;
                    correcto = false;
                }
            }

            // Validación nº 2
            if (correcto)
            {
                auxValor = (string)valor;
                if (auxValor.Length > this.MaxLength)
                {
                    validacion = EnumEstadoTextoRobusto.LongitudSobrepasada;
                    correcto = false;
                }
            }

            // Validación nº 3
            if (correcto)
            {
                if (!this.AdmiteVacio && (auxValor == string.Empty))
                {
                    validacion = EnumEstadoTextoRobusto.CadenaVacia;
                    correcto = false;
                }
            }

            // Composición de resultado
            if (correcto)
            {
                outValor = auxValor;
            }
            else if ((validacion == EnumEstadoTextoRobusto.LongitudSobrepasada) && this.LimitarLongitud)
            {
                outValor = auxValor.Substring(0, this.MaxLength);
            }
            else
            {
                outValor = this.ValorPorDefecto;
            }

            valor = outValor;
            return validacion;
        }

        /// <summary>
        /// Lanza una exepción por no estár permitido el valor especificado
        /// </summary>
        /// <param name="resultado">valor no permitido</param>
        public override void LanzarExcepcion()
        {
            if (this.Estado == EnumEstadoTextoRobusto.ValorTipoIncorrecto)
                throw new Exception("El campo " + this.Codigo + " no es válido.");
            if (this.Estado == EnumEstadoTextoRobusto.LongitudSobrepasada)
                throw new Exception("El campo " + this.Codigo + " es demasiado largo.");
            if (this.Estado == EnumEstadoTextoRobusto.CadenaVacia)
                throw new Exception("El campo " + this.Codigo + " no puede estar en blanco.");
        }
        #endregion

        #region Método(s) estático(s)
        /// <summary>
        /// Evalua si el parámetro es texto
        /// </summary>
        public static string Validar(object valor, out EnumEstadoRobusto validacion, int maxLength, bool admiteVacio, bool limitarLongitud, string defecto)
        {
            OTextoRobusto robusto = new OTextoRobusto(string.Empty, maxLength, admiteVacio, limitarLongitud, defecto, false);
            robusto.ValorGenerico = valor;
            validacion = robusto.Estado;
            return robusto.Valor;
        }

        /// <summary>
        /// Evalua si el parámetro es texto
        /// </summary>
        public static string Validar(object valor, int maxLength, bool admiteVacio, bool limitarLongitud, string defecto)
        {
            OTextoRobusto robusto = new OTextoRobusto(string.Empty, maxLength, admiteVacio, limitarLongitud, defecto, false);
            robusto.ValorGenerico = valor;
            return robusto.Valor;
        }

        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="valor">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public static EnumEstadoRobusto Validar(object valor, string codigo, int maxLength, bool admiteVacio, bool limitarLongitud, string defecto, bool lanzarExcepcionSiValorNoValido)
        {
            OTextoRobusto robusto = new OTextoRobusto(codigo, maxLength, admiteVacio, limitarLongitud, defecto, lanzarExcepcionSiValorNoValido);
            robusto.ValorGenerico = valor;
            return robusto.Estado;
        }
        #endregion
    }

    /// <summary>
    /// Resultado de la validación del SafeBool
    /// </summary>
    public class EnumEstadoTextoRobusto : EnumEstadoObjetoRobusto
    {
        #region Atributo(s)
        /// <summary>
        /// La longitud del texto es demasiado larga
        /// </summary>
        public static EnumEstadoRobusto LongitudSobrepasada = new EnumEstadoRobusto("LongitudSobrepasada", "LongitudSobrepasada", 12);
        /// <summary>
        /// El texto no contiene ningun caracter
        /// </summary>
        public static EnumEstadoRobusto CadenaVacia = new EnumEstadoRobusto("CadenaVacia", "CadenaVacia", 13);
        #endregion
    }
    #endregion

    #region Enumerado Robusto
    /// <summary>
    /// Asignación de una variable a un campo de tipo enumerado (aunque internamente trabaja como un string)
    /// </summary>
    public class OStringEnumRobusto : OObjetoRobusto<string>
    {
        #region Atributo(s)
        /// <summary>
        /// Valores permitidos para el texto
        /// </summary>
        public string[] ValoresPermitidos;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OStringEnumRobusto(string codigo, string[] valoresPermitidos, string valorDefecto, bool lanzarExcepcionSiValorNoValido)
            : base(codigo, valorDefecto, lanzarExcepcionSiValorNoValido)
        {
            this.ValoresPermitidos = valoresPermitidos;
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="valor">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override EnumEstadoRobusto Validar(ref object valor)
        {
            // Valores por defecto
            EnumEstadoRobusto validacion = EnumEstadoEnumRobusto.ResultadoCorrecto;
            bool correcto = true;
            string outValor = this.ValorPorDefecto;
            string auxValor = this.ValorPorDefecto;

            // Validación nº 1
            if (correcto)
            {
                if (!(valor is string))
                {
                    validacion = EnumEstadoEnumRobusto.ValorTipoIncorrecto;
                    correcto = false;
                }
            }

            // Validación nº 2
            if (correcto)
            {
                auxValor = (string)valor;
                bool found = false;
                foreach (string loopValor in this.ValoresPermitidos)
                {
                    found = (loopValor == auxValor);
                    if (found)
                    {
                        break;
                    }
                }
                if (!found)
                {
                    validacion = EnumEstadoEnumRobusto.ValorNoPermitido;
                    correcto = false;
                }
            }

            // Composición de resultado
            if (correcto)
            {
                outValor = auxValor;
            }
            else
            {
                outValor = this.ValorPorDefecto;
            }

            // Devolución de resultados
            valor = outValor;
            return validacion;
        }

        /// <summary>
        /// Lanza una exepción por no estár permitido el valor especificado
        /// </summary>
        /// <param name="resultado">valor no permitido</param>
        public override void LanzarExcepcion()
        {
            if (this.Estado == EnumEstadoEnumRobusto.ValorTipoIncorrecto)
                throw new Exception("El campo " + this.Codigo + " no es válido.");
            if (this.Estado == EnumEstadoEnumRobusto.ValorNoPermitido)
                throw new Exception("El campo " + this.Codigo + " no está permitido.");
        }
        #endregion

        #region Método(s) estático(s)
        /// <summary>
        /// Evalua si el parámetro es texto
        /// </summary>
        public static string Validar(object valor, out EnumEstadoRobusto validacion, string[] valoresPermitidos, string defecto)
        {
            OStringEnumRobusto robusto = new OStringEnumRobusto(string.Empty, valoresPermitidos, defecto, false);
            robusto.ValorGenerico = valor;
            validacion = robusto.Estado;
            return robusto.Valor;
        }

        /// <summary>
        /// Evalua si el parámetro es texto
        /// </summary>
        public static string Validar(object valor, string[] valoresPermitidos, string defecto)
        {
            OStringEnumRobusto robusto = new OStringEnumRobusto(string.Empty, valoresPermitidos, defecto, false);
            robusto.ValorGenerico = valor;
            return robusto.Valor;
        }

        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="valor">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public static EnumEstadoRobusto Validar(object valor, string codigo, string[] valoresPermitidos, string defecto, bool lanzarExcepcionSiValorNoValido)
        {
            OStringEnumRobusto robusto = new OStringEnumRobusto(codigo, valoresPermitidos, defecto, lanzarExcepcionSiValorNoValido);
            robusto.ValorGenerico = valor;
            valor = robusto.Valor;
            return robusto.Estado;
        }
        #endregion
    }

    /// <summary>
    /// Asignación de una variable a un campo de tipo enumerado (aunque internamente trabaja como un string)
    /// </summary>
    public class OEnumRobusto<T> : OObjetoRobusto<T>
    {
        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OEnumRobusto(string codigo, T valorDefecto, bool lanzarExcepcionSiValorNoValido)
            : base(codigo, valorDefecto, lanzarExcepcionSiValorNoValido)
        {
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="valor">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override EnumEstadoRobusto Validar(ref object valor)
        {
            // Valores por defecto
            EnumEstadoRobusto validacion = EnumEstadoEnumRobusto.ValorTipoIncorrecto;
            bool correcto = false;
            T outValor = this.ValorPorDefecto;
            T auxValor = this.ValorPorDefecto;

            // Validación nº 1
            if (!correcto)
            {
                if ((valor is T) && (typeof(T).IsEnum))
                {
                    auxValor = (T)valor;
                    validacion = EnumEstadoEnumRobusto.ResultadoCorrecto;
                    correcto = true;
                }
            }

            // Validación nº 2
            if (!correcto)
            {
                if (valor is string)
                {
                    string strValor = (string)valor;
                    try
                    {
                        auxValor = (T)Enum.Parse(typeof(T), strValor, true);
                        validacion = EnumEstadoEnumRobusto.ResultadoCorrecto;
                        correcto = true;
                    }
                    catch { }
                }
            }

            // Validación nº 3
            if (!correcto)
            {
                if (OEnteroRobusto.IsNumericInt(valor))
                {
                    try
                    {
                        auxValor = (T)valor;
                        validacion = EnumEstadoEnumRobusto.ResultadoCorrecto;
                        correcto = true;
                    }
                    catch { }
                }
            }

            // Composición de resultado
            if (correcto)
            {
                outValor = auxValor;
            }
            else
            {
                outValor = this.ValorPorDefecto;
            }

            // Devolución de resultados
            valor = outValor;
            return validacion;
        }

        /// <summary>
        /// Lanza una exepción por no estár permitido el valor especificado
        /// </summary>
        /// <param name="resultado">valor no permitido</param>
        public override void LanzarExcepcion()
        {
            if (this.Estado == EnumEstadoEnumRobusto.ValorTipoIncorrecto)
                throw new Exception("El campo " + this.Codigo + " no es válido.");
            if (this.Estado == EnumEstadoEnumRobusto.ValorTipoIncorrecto)
                throw new Exception("El campo " + this.Codigo + " no está permitido.");
        }
        #endregion

        #region Método(s) estático(s)
        /// <summary>
        /// Evalua si el parámetro es texto
        /// </summary>
        public static T Validar(object valor, out EnumEstadoRobusto validacion, T defecto)
        {
            OEnumRobusto<T> robusto = new OEnumRobusto<T>(string.Empty, defecto, false);
            robusto.ValorGenerico = valor;
            validacion = robusto.Estado;
            return robusto.Valor;
        }

        /// <summary>
        /// Evalua si el parámetro es texto
        /// </summary>
        public static T Validar(object valor, T defecto)
        {
            OEnumRobusto<T> robusto = new OEnumRobusto<T>(string.Empty, defecto, false);
            robusto.ValorGenerico = valor;
            return robusto.Valor;
        }

        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="valor">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public static EnumEstadoRobusto Validar(object valor, string codigo, T defecto, bool lanzarExcepcionSiValorNoValido)
        {
            OEnumRobusto<T> robusto = new OEnumRobusto<T>(codigo, defecto, lanzarExcepcionSiValorNoValido);
            robusto.ValorGenerico = valor;
            valor = robusto.Valor;
            return robusto.Estado;
        }

        /// <summary>
        /// Se utiliza con enumerados y devuelve verdadero si el enumerado está contenido en el valor
        /// </summary>
        /// <param name="valor">Valor del cual se quiere saber si contiene cieto enumerado</param>
        /// <param name="enumerate">Enumerado que deseamos comparar con el valor</param>
        /// <returns>Devuelve verdadero si el enumerado está contenido en el valor</returns>
        public static bool EnumContains(int valor, int[] enumerate)
        {
            foreach (int i in enumerate)
            {
                if ((valor & i) != 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Se utiliza con enumerados para convertir un valor de texto en un enumerado del tipo indicado
        /// </summary>
        /// <param name="enumType">Tipo del enumerado al que deseamos convertir</param>
        /// <param name="valor">Texto que queremos convertir a enumerado</param>
        /// <param name="defecto">Valor por defecto en el caso que el texto no coincida con ningun elemento del enumerado</param>
        /// <returns>Devuelve el enumerado correspondiente con el texto</returns>
        public static object EnumParse(Type enumType, string valor, object defecto)
        {
            object resultado;

            try
            {
                resultado = Enum.Parse(enumType, valor);
            }
            catch
            {
                resultado = defecto;
            }

            return resultado;
        }
        #endregion
    }

    /// <summary>
    /// Resultado de la validación del SafeBool
    /// </summary>
    public class EnumEstadoEnumRobusto : EnumEstadoObjetoRobusto
    {
        #region Atributo(s)
        /// <summary>
        /// El valor a asignar no está permitido
        /// </summary>
        public static EnumEstadoRobusto ValorNoPermitido = new EnumEstadoRobusto("ValorNoPermitido", "ValorNoPermitido", 22);
        #endregion
    }
    #endregion

    #region Entero Robusto
    /// <summary>
    /// Asignación de una variable a un campo de tipo entero
    /// </summary>
    public class OEnteroRobusto : OObjetoRobusto<int>
    {
        #region Atributo(s)
        /// <summary>
        /// Valor mínimo
        /// </summary>
        public int MinValor;
        /// <summary>
        /// Valor máximo
        /// </summary>
        public int MaxValor;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OEnteroRobusto(string codigo, int minValor, int maxValor, int valorDefecto, bool lanzarExcepcionSiValorNoValido)
            : base(codigo, valorDefecto, lanzarExcepcionSiValorNoValido)
        {
            this.MinValor = minValor;
            this.MaxValor = maxValor;
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="valor">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override EnumEstadoRobusto Validar(ref object valor)
        {
            // Valores por defecto
            EnumEstadoRobusto validacion = EnumEstadoEnteroRobusto.ValorTipoIncorrecto;
            bool correcto = false;
            int outValor = this.ValorPorDefecto;
            int auxValor = this.ValorPorDefecto;

            // Validación nº 1
            if (!correcto)
            {
                if (IsNumericInt(valor))
                {
                    auxValor = Convert.ToInt32(valor);
                    validacion = EnumEstadoEnteroRobusto.ResultadoCorrecto;
                    correcto = true;
                }
            }

            // Validación nº 2
            if (!correcto)
            {
                if (valor is string)
                {
                    int intValor;
                    if (int.TryParse((string)valor, NumberStyles.Any, CultureInfo.InvariantCulture, out intValor))
                    {
                        auxValor = intValor;
                        validacion = EnumEstadoEnteroRobusto.ResultadoCorrecto;
                        correcto = true;
                    }
                }
            }

            // Validación nº 3
            if (!correcto)
            {
                if (valor != null)
                {
                    try
                    {
                        auxValor = Convert.ToInt32(valor);
                        validacion = EnumEstadoEnteroRobusto.ResultadoCorrecto;
                        correcto = true;
                    }
                    catch { }
                }
            }

            // Validación nº 4
            if (correcto)
            {
                if (auxValor < this.MinValor)
                {
                    validacion = EnumEstadoEnteroRobusto.ValorInferiorMinimo;
                    correcto = false;
                }

                if (auxValor > this.MaxValor)
                {
                    validacion = EnumEstadoEnteroRobusto.ValorSuperiorMaximo;
                    correcto = false;
                }
            }

            // Composición de resultado
            if (correcto)
            {
                outValor = auxValor;
            }
            else
            {
                outValor = this.ValorPorDefecto;
            }

            // Devolución de resultados
            valor = outValor;
            return validacion;
        }

        /// <summary>
        /// Lanza una exepción por no estár permitido el valor especificado
        /// </summary>
        public override void LanzarExcepcion()
        {
            if (this.Estado == EnumEstadoEnteroRobusto.ValorTipoIncorrecto)
                throw new Exception("El campo " + this.Codigo + " no es un número entero.");
            if (this.Estado == EnumEstadoEnteroRobusto.ValorInferiorMinimo)
                throw new Exception("El campo " + this.Codigo + " es inferior al mínimo " + this.MinValor.ToString() + ".");
            if (this.Estado == EnumEstadoEnteroRobusto.ValorSuperiorMaximo)
                throw new Exception("El campo " + this.Codigo + " es superior al máximo " + this.MaxValor.ToString() + ".");
        }
        #endregion

        #region Métodos estático(s)
        /// <summary>
        /// Evalúa si el parámetro está dentro del rango determinado
        /// </summary>
        /// <returns>Devuelve verdadero si el parámetro está dentro del rango determinado</returns>
        public static int Validar(object valor, out EnumEstadoRobusto validacion, int min, int max, int defecto)
        {
            OEnteroRobusto robusto = new OEnteroRobusto(string.Empty, min, max, defecto, false);
            robusto.ValorGenerico = valor;
            validacion = robusto.Estado;
            return robusto.Valor;
        }

        /// <summary>
        /// Evalúa si el parámetro está dentro del rango determinado
        /// </summary>
        /// <returns>Devuelve verdadero si el parámetro está dentro del rango determinado</returns>
        public static int Validar(object valor, int min, int max, int defecto)
        {
            OEnteroRobusto robusto = new OEnteroRobusto(string.Empty, min, max, defecto, false);
            robusto.ValorGenerico = valor;
            return robusto.Valor;
        }

        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="value">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public static EnumEstadoRobusto Validar(object valor, string codigo, int min, int max, int defecto, bool lanzarExcepcionSiValorNoValido)
        {
            OEnteroRobusto robusto = new OEnteroRobusto(codigo, min, max, defecto, lanzarExcepcionSiValorNoValido);
            robusto.ValorGenerico = valor;
            valor = robusto.Valor;
            return robusto.Estado;
        }

        /// <summary>
        /// Indica si el objeto pasado es de tipo entero
        /// </summary>
        /// <param name="o">Objeto que se quiere conocer si es de tipo entero</param>
        /// <returns>Verdadero si el tipo del objeto es entero</returns>
        public static bool IsNumericInt(object o)
        {
            return (o != null) && ORobusto.IsTypeOf(o, typeof(int), typeof(short), typeof(long), typeof(uint), typeof(ushort), typeof(ulong), typeof(byte));
        }

        /// <summary>
        /// Evalúa si el parámetro está dentro del rango determinado
        /// </summary>
        /// <returns>Devuelve verdadero si el parámetro está dentro del rango determinado</returns>
        public static bool InRange(int valor, int min, int max)
        {
            return (valor >= min) && (valor <= max);
        }

        /// <summary>
        /// Evalúa si el parámetro está dentro del rango determinado y en caso contrario lo modifica para que cumpla la condición
        /// </summary>
        /// <returns>Devuelve el número obligando a que esté dentro del rango determinado</returns>
        public static int EnsureRange(int valor, int min, int max)
        {
            int resultado = valor;

            if (valor < min)
            {
                resultado = min;
            }
            if (valor > max)
            {
                resultado = max;
            }

            return resultado;
        }
        #endregion
    }

    /// <summary>
    /// Resultado de la validación del SafeBool
    /// </summary>
    public class EnumEstadoEnteroRobusto : EnumEstadoObjetoRobusto
    {
        #region Atributo(s)
        /// <summary>
        /// El valor a asignar es sueprior al máximo permitido
        /// </summary>
        public static EnumEstadoRobusto ValorSuperiorMaximo = new EnumEstadoRobusto("ValorSuperiorMaximo", "ValorSuperiorMaximo", 32);
        /// <summary>
        /// El valor a asignar es inferior al mínimo permitido
        /// </summary>
        public static EnumEstadoRobusto ValorInferiorMinimo = new EnumEstadoRobusto("ValorInferiorMinimo", "ValorInferiorMinimo", 33);
        #endregion
    }
    #endregion

    #region Decimal Robusto
    /// <summary>
    /// Asignación de una variable a un campo de tipo decimal
    /// </summary>
    public class ODecimalRobusto : OObjetoRobusto<double>
    {
        #region Atributo(s)
        /// <summary>
        /// Valor mínimo
        /// </summary>
        public double MinValor;
        /// <summary>
        /// Valor máximo
        /// </summary>
        public double MaxValor;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ODecimalRobusto(string codigo, double minValor, double maxValor, double valorDefecto, bool lanzarExcepcionSiValorNoValido)
            : base(codigo, valorDefecto, lanzarExcepcionSiValorNoValido)
        {
            this.MinValor = minValor;
            this.MaxValor = maxValor;
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="valor">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override EnumEstadoRobusto Validar(ref object valor)
        {
            // Valores por defecto
            EnumEstadoRobusto validacion = EnumEstadoDecimalRobusto.ValorTipoIncorrecto;
            bool correcto = false;
            double outValor = this.ValorPorDefecto;
            double auxValor = this.ValorPorDefecto;

            // Validación nº 1
            if (!correcto)
            {
                if (IsNumericFloat(valor))
                {
                    auxValor = Convert.ToDouble(valor);
                    validacion = EnumEstadoDecimalRobusto.ResultadoCorrecto;
                    correcto = true;
                }
            }

            // Validación nº 2
            if (!correcto)
            {
                if (valor is string)
                {
                    int intValor;
                    if (int.TryParse((string)valor, NumberStyles.Any, CultureInfo.InvariantCulture, out intValor))
                    {
                        auxValor = intValor;
                        validacion = EnumEstadoDecimalRobusto.ResultadoCorrecto;
                        correcto = true;
                    }
                }
            }

            // Validación nº 3
            if (!correcto)
            {
                if (valor != null)
                {
                    try
                    {
                        auxValor = Convert.ToDouble(valor);
                        validacion = EnumEstadoDecimalRobusto.ResultadoCorrecto;
                        correcto = true;
                    }
                    catch { }
                }
            }

            // Validación nº 4
            if (correcto)
            {
                if (auxValor < this.MinValor)
                {
                    validacion = EnumEstadoDecimalRobusto.ValorInferiorMinimo;
                    correcto = false;
                }

                if (auxValor > this.MaxValor)
                {
                    validacion = EnumEstadoDecimalRobusto.ValorSuperiorMaximo;
                    correcto = false;
                }
            }

            // Composición de resultado
            if (correcto)
            {
                outValor = auxValor;
            }
            else
            {
                outValor = this.ValorPorDefecto;
            }

            valor = outValor;
            return validacion;
        }

        /// <summary>
        /// Lanza una exepción por no estár permitido el valor especificado
        /// </summary>
        /// <param name="value">valor no permitido</param>
        public override void LanzarExcepcion()
        {
            if (this.Estado == EnumEstadoDecimalRobusto.ValorTipoIncorrecto)
                throw new Exception("El campo " + this.Codigo + " no es un número decimal.");
            if (this.Estado == EnumEstadoDecimalRobusto.ValorInferiorMinimo)
                throw new Exception("El campo " + this.Codigo + " es inferior al mínimo " + this.MinValor.ToString() + ".");
            if (this.Estado == EnumEstadoDecimalRobusto.ValorSuperiorMaximo)
                throw new Exception("El campo " + this.Codigo + " es superior al máximo " + this.MaxValor.ToString() + ".");
        }
        #endregion

        #region Método(s) estático(s)
        /// <summary>
        /// Evalúa si el parámetro está dentro del rango determinado
        /// </summary>
        /// <returns>Devuelve verdadero si el parámetro está dentro del rango determinado</returns>
        public static double Validar(object valor, out EnumEstadoRobusto validacion, double min, double max, double defecto)
        {
            ODecimalRobusto robusto = new ODecimalRobusto(string.Empty, min, max, defecto, false);
            robusto.ValorGenerico = valor;
            validacion = robusto.Estado;
            return robusto.Valor;
        }

        /// <summary>
        /// Evalúa si el parámetro está dentro del rango determinado
        /// </summary>
        /// <returns>Devuelve verdadero si el parámetro está dentro del rango determinado</returns>
        public static double Validar(object valor, double min, double max, double defecto)
        {
            ODecimalRobusto robusto = new ODecimalRobusto(string.Empty, min, max, defecto, false);
            robusto.ValorGenerico = valor;
            return robusto.Valor;
        }

        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="value">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public static EnumEstadoRobusto Validar(object valor, string codigo, double min, double max, double defecto, bool lanzarExcepcionSiValorNoValido)
        {
            ODecimalRobusto robusto = new ODecimalRobusto(codigo, min, max, defecto, lanzarExcepcionSiValorNoValido);
            robusto.ValorGenerico = valor;
            valor = robusto.Valor;
            return robusto.Estado;
        }

        /// <summary>
        /// Indica si el objeto pasado es de tipo decimal
        /// </summary>
        /// <param name="o">Objeto que se quiere conocer si es de tipo decimal</param>
        /// <returns>Verdadero si el tipo del objeto es decimal</returns>
        public static bool IsNumericFloat(object o)
        {
            return (o != null) && ORobusto.IsTypeOf(o, typeof(float), typeof(double), typeof(decimal));
        }

        /// <summary>
        /// Evalúa si el parámetro está dentro del rango determinado
        /// </summary>
        /// <returns>Devuelve verdadero si el parámetro está dentro del rango determinado</returns>
        public static bool InRange(double valor, double min, double max)
        {
            return (valor >= min) && (valor <= max);
        }

        /// <summary>
        /// Evalúa si el parámetro está dentro del rango determinado y en caso contrario lo modifica para que cumpla la condición
        /// </summary>
        /// <returns>Devuelve el número obligando a que esté dentro del rango determinado</returns>
        public static double EnsureRange(double valor, double min, double max)
        {
            double resultado = valor;

            if (valor < min)
            {
                resultado = min;
            }
            if (valor > max)
            {
                resultado = max;
            }

            return resultado;
        }
        #endregion
    }

    /// <summary>
    /// Resultado de la validación del SafeBool
    /// </summary>
    public class EnumEstadoDecimalRobusto : EnumEstadoObjetoRobusto
    {
        #region Atributo(s)
        /// <summary>
        /// El valor a asignar es sueprior al máximo permitido
        /// </summary>
        public static EnumEstadoRobusto ValorSuperiorMaximo = new EnumEstadoRobusto("ValorSuperiorMaximo", "ValorSuperiorMaximo", 42);
        /// <summary>
        /// El valor a asignar es inferior al mínimo permitido
        /// </summary>
        public static EnumEstadoRobusto ValorInferiorMinimo = new EnumEstadoRobusto("ValorInferiorMinimo", "ValorInferiorMinimo", 43);
        #endregion
    }
    #endregion

    #region Booleano Robusto
    /// <summary>
    /// Asignación de una variable a un campo de tipo booleano
    /// </summary>
    public class OBoolRobusto : OObjetoRobusto<bool>
    {
        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OBoolRobusto(string codigo, bool valorDefecto, bool lanzarExcepcionSiValorNoValido)
            : base(codigo, valorDefecto, lanzarExcepcionSiValorNoValido)
        {
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="valor">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override EnumEstadoRobusto Validar(ref object valor)
        {
            // Valores por defecto
            EnumEstadoRobusto validacion = EnumEstadoBoolRobusto.ValorTipoIncorrecto;
            bool correcto = false;
            bool outValor = this.ValorPorDefecto;
            bool auxValor = this.ValorPorDefecto;

            // Validación nº 1
            if (!correcto)
            {
                if (valor is bool)
                {
                    auxValor = (bool)valor;
                    validacion = EnumEstadoBoolRobusto.ResultadoCorrecto;
                    correcto = true;
                }
            }

            // Validación nº 2
            if (!correcto)
            {
                if (valor is string)
                {
                    string strValor = (string)valor;
                    if (string.Compare(strValor, "0") == 0)
                    {
                        auxValor = false;
                        validacion = EnumEstadoBoolRobusto.ResultadoCorrecto;
                        correcto = true;
                    }
                    else if (string.Compare(strValor, "1") == 0)
                    {
                        auxValor = true;
                        validacion = EnumEstadoBoolRobusto.ResultadoCorrecto;
                        correcto = true;
                    }
                }
            }

            // Validación nº 3
            if (!correcto)
            {
                if (valor != null)
                {
                    try
                    {
                        auxValor = Convert.ToBoolean(valor);
                        validacion = EnumEstadoBoolRobusto.ResultadoCorrecto;
                        correcto = true;
                    }
                    catch { }
                }
            }

            // Composición de resultado
            if (correcto)
            {
                outValor = auxValor;
            }
            else
            {
                outValor = this.ValorPorDefecto;
            }

            valor = outValor;
            return validacion;
        }

        /// <summary>
        /// Lanza una exepción por no estár permitido el valor especificado
        /// </summary>
        public override void LanzarExcepcion()
        {
            if (this.Estado == EnumEstadoBoolRobusto.ValorTipoIncorrecto)
                throw new Exception("El campo " + this.Codigo + " no es booleano.");
        }
        #endregion

        #region Método(s) estático(s)
        /// <summary>
        /// Evalúa si el parámetro es booleano
        /// </summary>
        public static bool Validar(object valor, out EnumEstadoRobusto validacion, bool defecto)
        {
            OBoolRobusto robusto = new OBoolRobusto(string.Empty, defecto, false);
            robusto.ValorGenerico = valor;
            validacion = robusto.Estado;
            return robusto.Valor;
        }

        /// <summary>
        /// Evalúa si el parámetro está dentro del rango determinado
        /// </summary>
        /// <returns>Devuelve verdadero si el parámetro está dentro del rango determinado</returns>
        public static bool Validar(object valor, bool defecto)
        {
            OBoolRobusto robusto = new OBoolRobusto(string.Empty, defecto, false);
            robusto.ValorGenerico = valor;
            return robusto.Valor;
        }

        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="valor">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public static EnumEstadoRobusto Validar(object valor, string codigo, bool defecto, bool lanzarExcepcionSiValorNoValido)
        {
            OBoolRobusto robusto = new OBoolRobusto(codigo, defecto, lanzarExcepcionSiValorNoValido);
            robusto.ValorGenerico = valor;
            valor = robusto.Valor;
            return robusto.Estado;
        }
        #endregion
    }

    /// <summary>
    /// Resultado de la validación del SafeBool
    /// </summary>
    public class EnumEstadoBoolRobusto : EnumEstadoObjetoRobusto
    {
    }
    #endregion

    #region TimeSpan Robusto
    /// <summary>
    /// Asignación de una variable a un campo de tipo intervalo de tiempo
    /// </summary>
    public class OIntervaloTiempoRobusto : OObjetoRobusto<TimeSpan>
    {
        #region Atributo(s)
        /// <summary>
        /// Valor mínimo
        /// </summary>
        public TimeSpan MinValor;
        /// <summary>
        /// Valor máximo
        /// </summary>
        public TimeSpan MaxValor;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OIntervaloTiempoRobusto(string codigo, TimeSpan minValor, TimeSpan maxValor, TimeSpan valorDefecto, bool lanzarExcepcionSiValorNoValido)
            : base(codigo, valorDefecto, lanzarExcepcionSiValorNoValido)
        {
            this.MinValor = minValor;
            this.MaxValor = maxValor;
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="valor">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override EnumEstadoRobusto Validar(ref object valor)
        {
            // Valores por defecto
            EnumEstadoRobusto validacion = EnumEstadoIntervaloTiempoRobusto.ValorTipoIncorrecto;
            bool correcto = false;
            TimeSpan outValor = this.ValorPorDefecto;
            TimeSpan auxValor = this.ValorPorDefecto;

            // Validación nº 1
            if (!correcto)
            {
                if (valor is TimeSpan)
                {
                    auxValor = (TimeSpan)valor;
                    validacion = EnumEstadoIntervaloTiempoRobusto.ResultadoCorrecto;
                    correcto = true;
                }
            }

            // Validación nº 2
            if (!correcto)
            {
                if (ORobusto.IsNumeric(valor))
                {
                    EnumEstadoRobusto resultConversion;
                    double decimalValue = ODecimalRobusto.Validar(valor, out resultConversion, TimeSpan.MinValue.TotalMilliseconds, TimeSpan.MaxValue.TotalMilliseconds, this.ValorPorDefecto.TotalMilliseconds);
                    if (resultConversion == EnumEstadoObjetoRobusto.ResultadoCorrecto)
                    {
                        auxValor = TimeSpan.FromDays(decimalValue);
                        validacion = EnumEstadoIntervaloTiempoRobusto.ResultadoCorrecto;
                        correcto = true;
                    }
                }
            }

            // Validación nº 3
            if (!correcto)
            {
                if (valor is string)
                {
                    string strValue = (string)valor;
                    bool ok = TimeSpan.TryParse(strValue, out auxValor);
                    if (ok)
                    {
                        auxValor = auxValor;
                        validacion = EnumEstadoIntervaloTiempoRobusto.ResultadoCorrecto;
                        correcto = true;
                    }
                }
            }

            // Validación nº 4
            if (correcto)
            {
                if (auxValor < this.MinValor)
                {
                    validacion = EnumEstadoIntervaloTiempoRobusto.ValorInferiorMinimo;
                    correcto = false;
                }

                if (auxValor > this.MaxValor)
                {
                    validacion = EnumEstadoIntervaloTiempoRobusto.ValorSuperiorMaximo;
                    correcto = false;
                }
            }

            // Composición de resultado
            if (correcto)
            {
                outValor = auxValor;
            }
            else
            {
                outValor = this.ValorPorDefecto;
            }

            valor = outValor;
            return validacion;
        }

        /// <summary>
        /// Lanza una exepción por no estár permitido el valor especificado
        /// </summary>
        /// <param name="value">valor no permitido</param>
        public override void LanzarExcepcion()
        {
            if (this.Estado == EnumEstadoIntervaloTiempoRobusto.ValorTipoIncorrecto)
                throw new Exception("El campo " + this.Codigo + " no es un periodo de tiempo válido.");
            if (this.Estado == EnumEstadoIntervaloTiempoRobusto.ValorInferiorMinimo)
                throw new Exception("El campo " + this.Codigo + " es inferior al mínimo " + this.MinValor.ToString() + ".");
            if (this.Estado == EnumEstadoIntervaloTiempoRobusto.ValorSuperiorMaximo)
                throw new Exception("El campo " + this.Codigo + " es superior al máximo " + this.MaxValor.ToString() + ".");
        }
        #endregion

        #region Método(s) estático(s)
        /// <summary>
        /// Evalúa si el parámetro está dentro del rango determinado
        /// </summary>
        /// <returns>Devuelve verdadero si el parámetro está dentro del rango determinado</returns>
        public static TimeSpan Validar(object valor, out EnumEstadoRobusto validacion, TimeSpan min, TimeSpan max, TimeSpan defecto)
        {
            OIntervaloTiempoRobusto robusto = new OIntervaloTiempoRobusto(string.Empty, min, max, defecto, false);
            robusto.ValorGenerico = valor;
            validacion = robusto.Estado;
            return robusto.Valor;
        }

        /// <summary>
        /// Evalúa si el parámetro está dentro del rango determinado
        /// </summary>
        /// <returns>Devuelve verdadero si el parámetro está dentro del rango determinado</returns>
        public static TimeSpan Validar(object valor, TimeSpan min, TimeSpan max, TimeSpan defecto)
        {
            OIntervaloTiempoRobusto robusto = new OIntervaloTiempoRobusto(string.Empty, min, max, defecto, false);
            robusto.ValorGenerico = valor;
            return robusto.Valor;
        }

        /// <summary>
        /// Evalúa si el parámetro está dentro del rango determinado
        /// </summary>
        /// <returns>Devuelve verdadero si el parámetro está dentro del rango determinado</returns>
        public static TimeSpan Validar(object valor, TimeSpan defecto)
        {
            OIntervaloTiempoRobusto robusto = new OIntervaloTiempoRobusto(string.Empty, TimeSpan.MinValue, TimeSpan.MaxValue, defecto, false);
            robusto.ValorGenerico = valor;
            return robusto.Valor;
        }

        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="valor">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public static EnumEstadoRobusto Validar(object valor, string codigo, TimeSpan min, TimeSpan max, TimeSpan defecto, bool lanzarExcepcionSiValorNoValido)
        {
            OIntervaloTiempoRobusto robusto = new OIntervaloTiempoRobusto(codigo, min, max, defecto, lanzarExcepcionSiValorNoValido);
            robusto.ValorGenerico = valor;
            valor = robusto.Valor;
            return robusto.Estado;
        }
        #endregion
    }

    /// <summary>
    /// Resultado de la validación del SafeBool
    /// </summary>
    public class EnumEstadoIntervaloTiempoRobusto : EnumEstadoObjetoRobusto
    {
        #region Atributo(s)
        /// <summary>
        /// El valor a asignar es sueprior al máximo permitido
        /// </summary>
        public static EnumEstadoRobusto ValorSuperiorMaximo = new EnumEstadoRobusto("ValorSuperiorMaximo", "ValorSuperiorMaximo", 51);
        /// <summary>
        /// El valor a asignar es inferior al mínimo permitido
        /// </summary>
        public static EnumEstadoRobusto ValorInferiorMinimo = new EnumEstadoRobusto("ValorInferiorMinimo", "ValorInferiorMinimo", 51);
        #endregion
    }
    #endregion

    #region DateTime Robusto
    /// <summary>
    /// Asignación de una variable a un campo de tipo fecha
    /// </summary>
    public class OFechaHoraRobusta : OObjetoRobusto<DateTime>
    {
        #region Atributo(s)
        /// <summary>
        /// Valor mínimo
        /// </summary>
        public DateTime MinValor;
        /// <summary>
        /// Valor máximo
        /// </summary>
        public DateTime MaxValor;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OFechaHoraRobusta(string codigo, DateTime minValor, DateTime maxValor, DateTime valorDefecto, bool lanzarExcepcionSiValorNoValido)
            : base(codigo, valorDefecto, lanzarExcepcionSiValorNoValido)
        {
            this.MinValor = minValor;
            this.MaxValor = maxValor;
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="valor">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override EnumEstadoRobusto Validar(ref object valor)
        {
            // Valores por defecto
            EnumEstadoRobusto validacion = EnumEstadoFechaHoraRobusta.ValorTipoIncorrecto;
            bool correcto = false;
            DateTime outValor = this.ValorPorDefecto;
            DateTime auxValor = this.ValorPorDefecto;

            // Validación nº 1
            if (!correcto)
            {
                if (valor is DateTime)
                {
                    auxValor = (DateTime)valor;
                    validacion = EnumEstadoFechaHoraRobusta.ResultadoCorrecto;
                    correcto = true;
                }
            }

            // Validación nº 2
            if (!correcto)
            {
                if (valor is string)
                {
                    string strValue = (string)valor;
                    bool ok = DateTime.TryParse(strValue, out auxValor);
                    if (ok)
                    {
                        validacion = EnumEstadoFechaHoraRobusta.ResultadoCorrecto;
                        correcto = true;
                    }
                }
            }

            // Validación nº 3
            if (correcto)
            {
                if (auxValor < this.MinValor)
                {
                    validacion = EnumEstadoFechaHoraRobusta.ValorInferiorMinimo;
                    correcto = false;
                }

                if (auxValor > this.MaxValor)
                {
                    validacion = EnumEstadoFechaHoraRobusta.ValorSuperiorMaximo;
                    correcto = false;
                }
            }

            // Composición de resultado
            if (correcto)
            {
                outValor = auxValor;
            }
            else
            {
                outValor = this.ValorPorDefecto;
            }

            valor = outValor;
            return validacion;
        }

        /// <summary>
        /// Lanza una exepción por no estár permitido el valor especificado
        /// </summary>
        public override void LanzarExcepcion()
        {
            if (this.Estado == EnumEstadoFechaHoraRobusta.ValorTipoIncorrecto)
                throw new Exception("El campo " + this.Codigo + " no es una fecha válida.");
            if (this.Estado == EnumEstadoFechaHoraRobusta.ValorInferiorMinimo)
                throw new Exception("El campo " + this.Codigo + " es inferior al mínimo " + this.MinValor.ToString() + ".");
            if (this.Estado == EnumEstadoFechaHoraRobusta.ValorSuperiorMaximo)
                throw new Exception("El campo " + this.Codigo + " es superior al máximo " + this.MaxValor.ToString() + ".");
        }
        #endregion

        #region Método(s) estático(s)
        /// <summary>
        /// Evalúa si el parámetro es booleano
        /// </summary>
        public static DateTime Validar(object valor, out EnumEstadoRobusto validacion, DateTime min, DateTime max, DateTime defecto)
        {
            OFechaHoraRobusta robusto = new OFechaHoraRobusta(string.Empty, min, max, defecto, false);
            robusto.ValorGenerico = valor;
            validacion = robusto.Estado;
            return robusto.Valor;
        }

        /// <summary>
        /// Evalúa si el parámetro está dentro del rango determinado
        /// </summary>
        /// <returns>Devuelve verdadero si el parámetro está dentro del rango determinado</returns>
        public static DateTime Validar(object valor, DateTime min, DateTime max, DateTime defecto)
        {
            OFechaHoraRobusta robusto = new OFechaHoraRobusta(string.Empty, min, max, defecto, false);
            robusto.ValorGenerico = valor;
            return robusto.Valor;
        }

        /// <summary>
        /// Evalúa si el parámetro está dentro del rango determinado
        /// </summary>
        /// <returns>Devuelve verdadero si el parámetro está dentro del rango determinado</returns>
        public static DateTime Validar(object valor, DateTime defecto)
        {
            OFechaHoraRobusta robusto = new OFechaHoraRobusta(string.Empty, DateTime.MinValue, DateTime.MaxValue, defecto, false);
            robusto.ValorGenerico = valor;
            return robusto.Valor;
        }

        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="valor">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public static EnumEstadoRobusto Validar(object valor, string codigo, DateTime min, DateTime max, DateTime defecto, bool lanzarExcepcionSiValorNoValido)
        {
            OFechaHoraRobusta robusto = new OFechaHoraRobusta(codigo, min, max, defecto, lanzarExcepcionSiValorNoValido);
            robusto.ValorGenerico = valor;
            valor = robusto.Valor;
            return robusto.Estado;
        }
        #endregion
    }

    /// <summary>
    /// Resultado de la validación del SafeBool
    /// </summary>
    public class EnumEstadoFechaHoraRobusta : EnumEstadoObjetoRobusto
    {
        #region Atributo(s)
        /// <summary>
        /// El valor a asignar es sueprior al máximo permitido
        /// </summary>
        public static EnumEstadoRobusto ValorSuperiorMaximo = new EnumEstadoRobusto("ValorSuperiorMaximo", "ValorSuperiorMaximo", 62);
        /// <summary>
        /// El valor a asignar es inferior al mínimo permitido
        /// </summary>
        public static EnumEstadoRobusto ValorInferiorMinimo = new EnumEstadoRobusto("ValorInferiorMinimo", "ValorInferiorMinimo", 63);
        #endregion
    }
    #endregion
}
