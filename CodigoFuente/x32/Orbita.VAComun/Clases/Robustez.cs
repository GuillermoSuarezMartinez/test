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

namespace Orbita.VAComun
{
    /// <summary>
    /// Clase base para el trabajo de forma robusta con variables
    /// </summary>
    public class ObjetoRobusto<ClaseTipo, EstadoTipo>
        where EstadoTipo : EstadoRobusto, new()
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
        /// <summary>
        /// Indica que el valor del objeto es válido
        /// </summary>
        public EstadoTipo Estado;
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
        /// Valor del objeto
        /// </summary>
        public virtual object ValorGenerico
        {
            set
            {
                this.Estado = this.Validar(value, this.LanzarExcepcionSiValorNoValido);
                if (this.Estado.Valido)
                {
                    ClaseTipo tValor = (ClaseTipo)value;
                    this._Valor = tValor;
                }
                else
                {
                    this._Valor = ValorPorDefecto;
                    LogsRuntime.Info(ModulosSistema.Comun, "Obtener Valor de forma segura", "No admite el valor " + value.ToString());
                }
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

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ObjetoRobusto(string codigo, ClaseTipo valorDefecto, bool lanzarExcepcionSiValorNoValido)
        {
            this.Codigo = codigo;
            this.LanzarExcepcionSiValorNoValido = lanzarExcepcionSiValorNoValido;
            this.ValorPorDefecto = valorDefecto;
            this.Estado = new EstadoTipo();
            this._Valor = valorDefecto;
        }
        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="value">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public virtual EstadoTipo Validar(object value, bool lanzarExcepcionSiValorNoValido)
        {
            EstadoTipo estado = new EstadoTipo();

            estado.Valido = value is ClaseTipo;

            if (lanzarExcepcionSiValorNoValido && !estado.Valido)
            {
                this.LanzarExcepcion(estado);
            }

            return estado;
        }
        /// <summary>
        /// Lanza una exepción por no estár permitido el valor especificado
        /// </summary>
        /// <param name="value">valor no permitido</param>
        protected virtual void LanzarExcepcion(EstadoTipo resultado)
        {
            // Implementado en heredados
        }
        #endregion

        #region Miembros de ILoggableClass

        #endregion
    }

    /// <summary>
    /// Clase que informa del resultado de la asignación de un valor
    /// </summary>
    public class EstadoRobusto
    {
        #region Atributo(s)
        /// <summary>
        /// Indica que el valor es válido
        /// </summary>
        public bool Valido;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public EstadoRobusto()
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public EstadoRobusto(bool valido)
        {
            this.Valido = valido;
        }
        #endregion
    }

    /// <summary>
    /// Asignación de una variable a un campo de tipo texto
    /// </summary>
    public class TextoRobusto : ObjetoRobusto<string, EstadoTextoRobusto>
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
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public TextoRobusto(string codigo, int maxLength, bool admiteVacio, string valorDefecto, bool lanzarExcepcionSiValorNoValido)
            : base(codigo, valorDefecto, lanzarExcepcionSiValorNoValido)
        {
            this.MaxLength = maxLength;
            this.AdmiteVacio = admiteVacio;
            this.Estado = new EstadoTextoRobusto(EstadoTextoRobusto.EnumEstadoTextoSeguro.ResultadoCorrecto);
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="value">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override EstadoTextoRobusto Validar(object valor, bool lanzarExcepcionSiValorNoValido)
        {
            EstadoTextoRobusto estado = new EstadoTextoRobusto(EstadoTextoRobusto.EnumEstadoTextoSeguro.ResultadoCorrecto);

            if (estado.Valido)
            {
                if (!(valor is string))
                {
                    estado.Valor = EstadoTextoRobusto.EnumEstadoTextoSeguro.ValorNoString;
                }
            }

            if (estado.Valido)
            {
                string strValor = (string)valor;
                if (strValor.Length > this.MaxLength)
                {
                    estado.Valor = EstadoTextoRobusto.EnumEstadoTextoSeguro.LongitudSobrepasada;
                }
            }

            if (estado.Valido)
            {
                string strValor = (string)valor;
                if (!this.AdmiteVacio && (strValor == string.Empty))
                {
                    estado.Valor = EstadoTextoRobusto.EnumEstadoTextoSeguro.CadenaVacia;
                }
            }

            if (lanzarExcepcionSiValorNoValido && !estado.Valido)
            {
                this.LanzarExcepcion(estado);
            }
            return estado;
        }
        /// <summary>
        /// Lanza una exepción por no estár permitido el valor especificado
        /// </summary>
        /// <param name="resultado">valor no permitido</param>
        protected override void LanzarExcepcion(EstadoTextoRobusto resultado)
        {
            switch (resultado.Valor)
            {
                case EstadoTextoRobusto.EnumEstadoTextoSeguro.ValorNoString:
                    throw new Exception("El campo " + this.Codigo + " no es válido.");
                    break;
                case EstadoTextoRobusto.EnumEstadoTextoSeguro.LongitudSobrepasada:
                    throw new Exception("El campo " + this.Codigo + " es demasiado largo.");
                    break;
                case EstadoTextoRobusto.EnumEstadoTextoSeguro.CadenaVacia:
                    throw new Exception("El campo " + this.Codigo + " no puede estar en blanco.");
                    break;
            }
        }
        #endregion
    }

    /// <summary>
    /// Clase que informa del resultado de la asignación de un valor
    /// </summary>
    public class EstadoTextoRobusto : EstadoRobusto
    {
        #region Propiedad(es)
        /// <summary>
        /// Valor del estado
        /// </summary>
        private EnumEstadoTextoSeguro _Valor;
        /// <summary>
        /// Valor del estado
        /// </summary>
        public EnumEstadoTextoSeguro Valor
        {
            get
            {
                return this._Valor;
            }
            set
            {
                this._Valor = value;
                this.Valido = value == EnumEstadoTextoSeguro.ResultadoCorrecto;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public EstadoTextoRobusto()
            : base()
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public EstadoTextoRobusto(EnumEstadoTextoSeguro tipoError)
            : base(tipoError == EnumEstadoTextoSeguro.ResultadoCorrecto)
        {
            this.Valor = tipoError;
        }
        #endregion

        #region Enumerados
        /// <summary>
        /// Resultado de la validación del SafeBool
        /// </summary>
        public enum EnumEstadoTextoSeguro
        {
            /// <summary>
            /// Resultado correcto
            /// </summary>
            ResultadoCorrecto = 0,
            /// <summary>
            /// El valor a asignar no es string
            /// </summary>
            ValorNoString = 1,
            /// <summary>
            /// La longitud del texto es demasiado larga
            /// </summary>
            LongitudSobrepasada = 2,
            /// <summary>
            /// El texto no contiene ningun caracter
            /// </summary>
            CadenaVacia = 3
        }
        #endregion
    }

    /// <summary>
    /// Asignación de una variable a un campo de tipo enumerado (aunque internamente trabaja como un string)
    /// </summary>
    public class StringEnumRobusto : ObjetoRobusto<string, EstadoEnumRobusto>
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
        public StringEnumRobusto(string codigo, string[] valoresPermitidos, string valorDefecto, bool lanzarExcepcionSiValorNoValido)
            : base(codigo, valorDefecto, lanzarExcepcionSiValorNoValido)
        {
            this.ValoresPermitidos = valoresPermitidos;
            this.Estado = new EstadoEnumRobusto(EstadoEnumRobusto.EnumEstadoEnumRobusto.ResultadoCorrecto);
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="value">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override EstadoEnumRobusto Validar(object valor, bool lanzarExcepcionSiValorNoValido)
        {
            EstadoEnumRobusto estado = new EstadoEnumRobusto(EstadoEnumRobusto.EnumEstadoEnumRobusto.ResultadoCorrecto);

            if (estado.Valido)
            {
                if (!(valor is string))
                {
                    estado.Valor = EstadoEnumRobusto.EnumEstadoEnumRobusto.ValorNoEnumerado;
                }
            }

            if (estado.Valido)
            {
                string strValor = (string)valor;
                bool found = false;
                foreach (string loopValor in this.ValoresPermitidos)
                {
                    found = (loopValor == strValor);
                    if (found)
                    {
                        break;
                    }
                }
                if (!found)
                {
                    estado.Valor = EstadoEnumRobusto.EnumEstadoEnumRobusto.ValorNoPermitido;
                }
            }

            if (lanzarExcepcionSiValorNoValido && !estado.Valido)
            {
                this.LanzarExcepcion(estado);
            }
            return estado;
        }
        /// <summary>
        /// Lanza una exepción por no estár permitido el valor especificado
        /// </summary>
        /// <param name="value">valor no permitido</param>
        protected override void LanzarExcepcion(EstadoEnumRobusto resultado)
        {
            switch (resultado.Valor)
            {
                case EstadoEnumRobusto.EnumEstadoEnumRobusto.ValorNoEnumerado:
                    throw new Exception("El campo " + this.Codigo + " no es válido.");
                    break;
                case EstadoEnumRobusto.EnumEstadoEnumRobusto.ValorNoPermitido:
                    throw new Exception("El campo " + this.Codigo + " no está permitido.");
                    break;
            }
        }
        #endregion
    }

    /// <summary>
    /// Asignación de una variable a un campo de tipo enumerado (aunque internamente trabaja como un string)
    /// </summary>
    public class EnumRobusto<T> : ObjetoRobusto<T, EstadoEnumRobusto>
    {
        #region Propiedad(es)
        /// <summary>
        /// Valor del objeto
        /// </summary>
        public override object ValorGenerico
        {
            set
            {
                this.Estado = this.Validar(value, this.LanzarExcepcionSiValorNoValido);
                if (this.Estado.Valido)
                {
                    if (value is T)
                    {
                        this._Valor = (T)value;
                    }
                    else if (value is string)
                    {
                        this._Valor = (T)Enum.Parse(typeof(T), (string)value);
                    }
                    else if (App.IsNumericInt(value))
                    {
                        T tValor = (T)value;
                    }
                }
                else
                {
                    this._Valor = ValorPorDefecto;
                    LogsRuntime.Info(ModulosSistema.Comun, "ValorGenerico", "No admite el valor " + value.ToString());
                }
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public EnumRobusto(string codigo, T valorDefecto, bool lanzarExcepcionSiValorNoValido)
            : base(codigo, valorDefecto, lanzarExcepcionSiValorNoValido)
        {
            this.Estado = new EstadoEnumRobusto(EstadoEnumRobusto.EnumEstadoEnumRobusto.ResultadoCorrecto);
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="value">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override EstadoEnumRobusto Validar(object valor, bool lanzarExcepcionSiValorNoValido)
        {
            EstadoEnumRobusto estado = new EstadoEnumRobusto(EstadoEnumRobusto.EnumEstadoEnumRobusto.ValorNoEnumerado);

            if (!estado.Valido)
            {
                if ((valor is T) && (typeof(T).IsEnum))
                {
                    estado.Valor = EstadoEnumRobusto.EnumEstadoEnumRobusto.ResultadoCorrecto;
                }
            }

            if (!estado.Valido)
            {
                if (valor is string)
                {
                    string strValor = (string)valor;
                    try
                    {
                        T tValor = (T)Enum.Parse(typeof(T), strValor);
                        estado.Valor = EstadoEnumRobusto.EnumEstadoEnumRobusto.ResultadoCorrecto;
                    }
                    catch
                    {
                        estado.Valor = EstadoEnumRobusto.EnumEstadoEnumRobusto.ValorNoPermitido;
                    }
                }
            }

            if (!estado.Valido)
            {
                if (App.IsNumericInt(valor))
                {
                    try
                    {
                        T tValor = (T)valor;
                        estado.Valor = EstadoEnumRobusto.EnumEstadoEnumRobusto.ResultadoCorrecto;
                    }
                    catch
                    {
                        estado.Valor = EstadoEnumRobusto.EnumEstadoEnumRobusto.ValorNoPermitido;
                    }
                }
            }

            if (lanzarExcepcionSiValorNoValido && !estado.Valido)
            {
                this.LanzarExcepcion(estado);
            }
            return estado;
        }
        /// <summary>
        /// Lanza una exepción por no estár permitido el valor especificado
        /// </summary>
        /// <param name="value">valor no permitido</param>
        protected override void LanzarExcepcion(EstadoEnumRobusto resultado)
        {
            switch (resultado.Valor)
            {
                case EstadoEnumRobusto.EnumEstadoEnumRobusto.ValorNoEnumerado:
                    throw new Exception("El campo " + this.Codigo + " no es válido.");
                    break;
                case EstadoEnumRobusto.EnumEstadoEnumRobusto.ValorNoPermitido:
                    throw new Exception("El campo " + this.Codigo + " no está permitido.");
                    break;
            }
        }
        #endregion
    }

    /// <summary>
    /// Clase que informa del resultado de la asignación de un valor
    /// </summary>
    public class EstadoEnumRobusto : EstadoRobusto
    {
        #region Propiedad(es)
        /// <summary>
        /// Valor del estado
        /// </summary>
        private EnumEstadoEnumRobusto _Valor;
        /// <summary>
        /// Valor del estado
        /// </summary>
        public EnumEstadoEnumRobusto Valor
        {
            get
            {
                return this._Valor;
            }
            set
            {
                this._Valor = value;
                this.Valido = value == EnumEstadoEnumRobusto.ResultadoCorrecto;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public EstadoEnumRobusto()
            : base()
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public EstadoEnumRobusto(EnumEstadoEnumRobusto valor)
            : base(valor == EnumEstadoEnumRobusto.ResultadoCorrecto)
        {
            this.Valor = valor;
        }
        #endregion

        #region Enumerados
        /// <summary>
        /// Resultado de la validación del SafeBool
        /// </summary>
        public enum EnumEstadoEnumRobusto
        {
            /// <summary>
            /// Resultado correcto
            /// </summary>
            ResultadoCorrecto = 0,
            /// <summary>
            /// El valor a asignar no es enumerado
            /// </summary>
            ValorNoEnumerado = 1,
            /// <summary>
            /// El valor a asignar no está permitido
            /// </summary>
            ValorNoPermitido = 2
        }
        #endregion
    }

    /// <summary>
    /// Asignación de una variable a un campo de tipo entero
    /// </summary>
    public class EnteroRobusto : ObjetoRobusto<int, EstadoEnteroRobusto>
    {
        #region Atributo(s)
        /// <summary>
        /// Valor mínimo
        /// </summary>
        public int MinValue;
        /// <summary>
        /// Valor máximo
        /// </summary>
        public int MaxValue;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public EnteroRobusto(string codigo, int minValue, int maxValue, int valorDefecto, bool lanzarExcepcionSiValorNoValido)
            : base(codigo, valorDefecto, lanzarExcepcionSiValorNoValido)
        {
            Nullable<int> a = 1;

            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.Estado = new EstadoEnteroRobusto(EstadoEnteroRobusto.EnumEstadoEnteroRobusto.ResultadoCorrecto);
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="value">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override EstadoEnteroRobusto Validar(object valor, bool lanzarExcepcionSiValorNoValido)
        {
            EstadoEnteroRobusto estado = new EstadoEnteroRobusto(EstadoEnteroRobusto.EnumEstadoEnteroRobusto.ResultadoCorrecto);

            if (estado.Valido)
            {
                if (!App.IsNumericInt(valor))
                {
                    estado.Valor = EstadoEnteroRobusto.EnumEstadoEnteroRobusto.ValorNoEntero;
                }
            }

            if (estado.Valido)
            {
                int intValor = Convert.ToInt32(valor);
                if (intValor < this.MinValue)
                {
                    estado.Valor = EstadoEnteroRobusto.EnumEstadoEnteroRobusto.ValorInferiorMinimo;
                }
            }

            if (estado.Valido)
            {
                int intValor = Convert.ToInt32(valor);
                if (intValor > this.MaxValue)
                {
                    estado.Valor = EstadoEnteroRobusto.EnumEstadoEnteroRobusto.ValorSuperiorMaximo;
                }
            }

            if (lanzarExcepcionSiValorNoValido && !estado.Valido)
            {
                this.LanzarExcepcion(estado);
            }

            return estado;
        }
        /// <summary>
        /// Lanza una exepción por no estár permitido el valor especificado
        /// </summary>
        /// <param name="value">valor no permitido</param>
        protected override void LanzarExcepcion(EstadoEnteroRobusto resultado)
        {
            switch (resultado.Valor)
            {
                case EstadoEnteroRobusto.EnumEstadoEnteroRobusto.ValorNoEntero:
                    throw new Exception("El campo " + this.Codigo + " no es un número entero.");
                    break;
                case EstadoEnteroRobusto.EnumEstadoEnteroRobusto.ValorInferiorMinimo:
                    throw new Exception("El campo " + this.Codigo + " es inferior al mínimo " + this.MinValue.ToString() + ".");
                    break;
                case EstadoEnteroRobusto.EnumEstadoEnteroRobusto.ValorSuperiorMaximo:
                    throw new Exception("El campo " + this.Codigo + " es superior al máximo " + this.MaxValue.ToString() + ".");
                    break;
            }
        }
        #endregion
    }

    /// <summary>
    /// Clase que informa del resultado de la asignación de un valor
    /// </summary>
    public class EstadoEnteroRobusto : EstadoRobusto
    {
        #region Propiedad(es)
        /// <summary>
        /// Valor del estado
        /// </summary>
        private EnumEstadoEnteroRobusto _Valor;
        /// <summary>
        /// Valor del estado
        /// </summary>
        public EnumEstadoEnteroRobusto Valor
        {
            get
            {
                return this._Valor;
            }
            set
            {
                this._Valor = value;
                this.Valido = value == EnumEstadoEnteroRobusto.ResultadoCorrecto;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public EstadoEnteroRobusto()
            : base()
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public EstadoEnteroRobusto(EnumEstadoEnteroRobusto valor)
            : base(valor == EnumEstadoEnteroRobusto.ResultadoCorrecto)
        {
            this.Valor = valor;
        }
        #endregion

        #region Enumerados
        /// <summary>
        /// Resultado de la validación del SafeBool
        /// </summary>
        public enum EnumEstadoEnteroRobusto
        {
            /// <summary>
            /// Resultado correcto
            /// </summary>
            ResultadoCorrecto = 0,
            /// <summary>
            /// El valor a asignar no es entero
            /// </summary>
            ValorNoEntero = 1,
            /// <summary>
            /// El valor a asignar es sueprior al máximo permitido
            /// </summary>
            ValorSuperiorMaximo = 2,
            /// <summary>
            /// El valor a asignar es inferior al mínimo permitido
            /// </summary>
            ValorInferiorMinimo = 3
        }
        #endregion
    }

    /// <summary>
    /// Asignación de una variable a un campo de tipo decimal
    /// </summary>
    public class DecimalRobusto : ObjetoRobusto<double, EstadoDecimalRobusto>
    {
        #region Atributo(s)
        /// <summary>
        /// Valor mínimo
        /// </summary>
        public double MinValue;
        /// <summary>
        /// Valor máximo
        /// </summary>
        public double MaxValue;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public DecimalRobusto(string codigo, double minValue, double maxValue, double valorDefecto, bool lanzarExcepcionSiValorNoValido)
            : base(codigo, valorDefecto, lanzarExcepcionSiValorNoValido)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.Estado = new EstadoDecimalRobusto(EstadoDecimalRobusto.EnumEstadoDecimalRobusto.ResultadoCorrecto);
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="value">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override EstadoDecimalRobusto Validar(object valor, bool lanzarExcepcionSiValorNoValido)
        {
            EstadoDecimalRobusto estado = new EstadoDecimalRobusto(EstadoDecimalRobusto.EnumEstadoDecimalRobusto.ResultadoCorrecto);

            if (estado.Valido)
            {
                if (!App.IsNumericFloat(valor))
                {
                    estado.Valor = EstadoDecimalRobusto.EnumEstadoDecimalRobusto.ValorNoDoble;
                }
            }

            if (estado.Valido)
            {
                double intValor = Convert.ToDouble(valor);
                if (intValor < this.MinValue)
                {
                    estado.Valor = EstadoDecimalRobusto.EnumEstadoDecimalRobusto.ValorInferiorMinimo;
                }
            }

            if (estado.Valido)
            {
                double intValor = Convert.ToDouble(valor);
                if (intValor > this.MaxValue)
                {
                    estado.Valor = EstadoDecimalRobusto.EnumEstadoDecimalRobusto.ValorSuperiorMaximo;
                }
            }

            if (lanzarExcepcionSiValorNoValido && !estado.Valido)
            {
                this.LanzarExcepcion(estado);
            }

            return estado;
        }
        /// <summary>
        /// Lanza una exepción por no estár permitido el valor especificado
        /// </summary>
        /// <param name="value">valor no permitido</param>
        protected override void LanzarExcepcion(EstadoDecimalRobusto resultado)
        {
            switch (resultado.Valor)
            {
                case EstadoDecimalRobusto.EnumEstadoDecimalRobusto.ValorNoDoble:
                    throw new Exception("El campo " + this.Codigo + " no es un número decimal.");
                    break;
                case EstadoDecimalRobusto.EnumEstadoDecimalRobusto.ValorInferiorMinimo:
                    throw new Exception("El campo " + this.Codigo + " es inferior al mínimo " + this.MinValue.ToString() + ".");
                    break;
                case EstadoDecimalRobusto.EnumEstadoDecimalRobusto.ValorSuperiorMaximo:
                    throw new Exception("El campo " + this.Codigo + " es superior al máximo " + this.MaxValue.ToString() + ".");
                    break;
            }
        }
        #endregion
    }

    /// <summary>
    /// Clase que informa del resultado de la asignación de un valor
    /// </summary>
    public class EstadoDecimalRobusto : EstadoRobusto
    {
        #region Propiedad(es)
        /// <summary>
        /// Valor del estado
        /// </summary>
        private EnumEstadoDecimalRobusto _Valor;
        /// <summary>
        /// Valor del estado
        /// </summary>
        public EnumEstadoDecimalRobusto Valor
        {
            get
            {
                return this._Valor;
            }
            set
            {
                this._Valor = value;
                this.Valido = value == EnumEstadoDecimalRobusto.ResultadoCorrecto;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public EstadoDecimalRobusto()
            : base()
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public EstadoDecimalRobusto(EnumEstadoDecimalRobusto valor)
            : base(valor == EnumEstadoDecimalRobusto.ResultadoCorrecto)
        {
            this.Valor = valor;
        }
        #endregion

        #region Enumerados
        /// <summary>
        /// Resultado de la validación del SafeBool
        /// </summary>
        public enum EnumEstadoDecimalRobusto
        {
            /// <summary>
            /// Resultado correcto
            /// </summary>
            ResultadoCorrecto = 0,
            /// <summary>
            /// El valor a asignar no es entero
            /// </summary>
            ValorNoDoble = 1,
            /// <summary>
            /// El valor a asignar es sueprior al máximo permitido
            /// </summary>
            ValorSuperiorMaximo = 2,
            /// <summary>
            /// El valor a asignar es inferior al mínimo permitido
            /// </summary>
            ValorInferiorMinimo = 3
        }
        #endregion
    }

    /// <summary>
    /// Asignación de una variable a un campo de tipo booleano
    /// </summary>
    public class BoolRobusto : ObjetoRobusto<bool, EstadoBoolRobusto>
    {
        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public BoolRobusto(string codigo, bool valorDefecto, bool lanzarExcepcionSiValorNoValido)
            : base(codigo, valorDefecto, lanzarExcepcionSiValorNoValido)
        {
            this.Estado = new EstadoBoolRobusto(EstadoBoolRobusto.EnumEstadoBoolRobusto.ResultadoCorrecto);
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="value">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override EstadoBoolRobusto Validar(object valor, bool lanzarExcepcionSiValorNoValido)
        {
            EstadoBoolRobusto estado = new EstadoBoolRobusto(EstadoBoolRobusto.EnumEstadoBoolRobusto.ResultadoCorrecto);

            if (estado.Valido)
            {
                if (!(valor is bool))
                {
                    estado.Valor = EstadoBoolRobusto.EnumEstadoBoolRobusto.ValorNoBooleano;
                }
            }

            if (lanzarExcepcionSiValorNoValido && !estado.Valido)
            {
                this.LanzarExcepcion(estado);
            }

            return estado;
        }
        /// <summary>
        /// Lanza una exepción por no estár permitido el valor especificado
        /// </summary>
        /// <param name="value">valor no permitido</param>
        protected override void LanzarExcepcion(EstadoBoolRobusto resultado)
        {
            if (resultado.Valor == EstadoBoolRobusto.EnumEstadoBoolRobusto.ValorNoBooleano)
            {
                string OMensajes = "El campo " + this.Codigo + " no es booleano.";
                throw new Exception(OMensajes);
            }
        }
        #endregion
    }

    /// <summary>
    /// Clase que informa del resultado de la asignación de un valor
    /// </summary>
    public class EstadoBoolRobusto : EstadoRobusto
    {
        #region Propiedad(es)
        /// <summary>
        /// Valor del estado
        /// </summary>
        private EnumEstadoBoolRobusto _Valor;
        /// <summary>
        /// Valor del estado
        /// </summary>
        public EnumEstadoBoolRobusto Valor
        {
            get
            {
                return this._Valor;
            }
            set
            {
                this._Valor = value;
                this.Valido = value == EnumEstadoBoolRobusto.ResultadoCorrecto;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public EstadoBoolRobusto()
            : base()
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public EstadoBoolRobusto(EnumEstadoBoolRobusto valor)
            : base(valor == EnumEstadoBoolRobusto.ResultadoCorrecto)
        {
            this.Valor = valor;
        }
        #endregion

        #region Enumerados
        /// <summary>
        /// Resultado de la validación del SafeBool
        /// </summary>
        public enum EnumEstadoBoolRobusto
        {
            /// <summary>
            /// Resultado correcto
            /// </summary>
            ResultadoCorrecto = 0,
            /// <summary>
            /// El valor a asignar no es booleano
            /// </summary>
            ValorNoBooleano = 1
        }
        #endregion
    }

    /// <summary>
    /// Asignación de una variable a un campo de tipo intervalo de tiempo
    /// </summary>
    public class IntervaloTiempoRobusto : ObjetoRobusto<TimeSpan, EstadoIntervaloTiempoRobusto>
    {
        #region Atributo(s)
        /// <summary>
        /// Valor mínimo
        /// </summary>
        public TimeSpan MinValue;
        /// <summary>
        /// Valor máximo
        /// </summary>
        public TimeSpan MaxValue;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public IntervaloTiempoRobusto(string codigo, TimeSpan minValue, TimeSpan maxValue, TimeSpan valorDefecto, bool lanzarExcepcionSiValorNoValido)
            : base(codigo, valorDefecto, lanzarExcepcionSiValorNoValido)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.Estado = new EstadoIntervaloTiempoRobusto(EstadoIntervaloTiempoRobusto.EnumEstadoIntervaloTiempoRobusto.ResultadoCorrecto);
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="value">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override EstadoIntervaloTiempoRobusto Validar(object valor, bool lanzarExcepcionSiValorNoValido)
        {
            EstadoIntervaloTiempoRobusto estado = new EstadoIntervaloTiempoRobusto(EstadoIntervaloTiempoRobusto.EnumEstadoIntervaloTiempoRobusto.ResultadoCorrecto);

            if (estado.Valido)
            {
                if (!(valor is TimeSpan))
                {
                    estado.Valor = EstadoIntervaloTiempoRobusto.EnumEstadoIntervaloTiempoRobusto.ValorNoIntervaloTiempo;
                }
            }

            if (estado.Valido)
            {
                TimeSpan timeSpanValor = (TimeSpan)valor;
                if (timeSpanValor < this.MinValue)
                {
                    estado.Valor = EstadoIntervaloTiempoRobusto.EnumEstadoIntervaloTiempoRobusto.ValorInferiorMinimo;
                }
            }

            if (estado.Valido)
            {
                TimeSpan timeSpanValor = (TimeSpan)valor;
                if (timeSpanValor > this.MaxValue)
                {
                    estado.Valor = EstadoIntervaloTiempoRobusto.EnumEstadoIntervaloTiempoRobusto.ValorSuperiorMaximo;
                }
            }

            if (lanzarExcepcionSiValorNoValido && !estado.Valido)
            {
                this.LanzarExcepcion(estado);
            }

            return estado;
        }
        /// <summary>
        /// Lanza una exepción por no estár permitido el valor especificado
        /// </summary>
        /// <param name="value">valor no permitido</param>
        protected override void LanzarExcepcion(EstadoIntervaloTiempoRobusto resultado)
        {
            switch (resultado.Valor)
            {
                case EstadoIntervaloTiempoRobusto.EnumEstadoIntervaloTiempoRobusto.ValorNoIntervaloTiempo:
                    throw new Exception("El campo " + this.Codigo + " no es un periodo de tiempo válido.");
                    break;
                case EstadoIntervaloTiempoRobusto.EnumEstadoIntervaloTiempoRobusto.ValorInferiorMinimo:
                    throw new Exception("El campo " + this.Codigo + " es inferior al mínimo " + this.MinValue.ToString() + ".");
                    break;
                case EstadoIntervaloTiempoRobusto.EnumEstadoIntervaloTiempoRobusto.ValorSuperiorMaximo:
                    throw new Exception("El campo " + this.Codigo + " es superior al máximo " + this.MaxValue.ToString() + ".");
                    break;
            }
        }
        #endregion
    }

    /// <summary>
    /// Clase que informa del resultado de la asignación de un valor
    /// </summary>
    public class EstadoIntervaloTiempoRobusto : EstadoRobusto
    {
        #region Propiedad(es)
        /// <summary>
        /// Valor del estado
        /// </summary>
        private EnumEstadoIntervaloTiempoRobusto _Valor;
        /// <summary>
        /// Valor del estado
        /// </summary>
        public EnumEstadoIntervaloTiempoRobusto Valor
        {
            get
            {
                return this._Valor;
            }
            set
            {
                this._Valor = value;
                this.Valido = value == EnumEstadoIntervaloTiempoRobusto.ResultadoCorrecto;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public EstadoIntervaloTiempoRobusto()
            : base()
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public EstadoIntervaloTiempoRobusto(EnumEstadoIntervaloTiempoRobusto valor)
            : base(valor == EnumEstadoIntervaloTiempoRobusto.ResultadoCorrecto)
        {
            this.Valor = valor;
        }
        #endregion

        #region Enumerados
        /// <summary>
        /// Resultado de la validación del SafeBool
        /// </summary>
        public enum EnumEstadoIntervaloTiempoRobusto
        {
            /// <summary>
            /// Resultado correcto
            /// </summary>
            ResultadoCorrecto = 0,
            /// <summary>
            /// El valor a asignar no es entero
            /// </summary>
            ValorNoIntervaloTiempo = 1,
            /// <summary>
            /// El valor a asignar es sueprior al máximo permitido
            /// </summary>
            ValorSuperiorMaximo = 2,
            /// <summary>
            /// El valor a asignar es inferior al mínimo permitido
            /// </summary>
            ValorInferiorMinimo = 3
        }
        #endregion
    }

    /// <summary>
    /// Asignación de una variable a un campo de tipo fecha
    /// </summary>
    public class FechaHoraRobusta : ObjetoRobusto<DateTime, EstadoFechaHoraRobusta>
    {
        #region Atributo(s)
        /// <summary>
        /// Valor mínimo
        /// </summary>
        public DateTime MinValue;
        /// <summary>
        /// Valor máximo
        /// </summary>
        public DateTime MaxValue;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public FechaHoraRobusta(string codigo, DateTime minValue, DateTime maxValue, DateTime valorDefecto, bool lanzarExcepcionSiValorNoValido)
            : base(codigo, valorDefecto, lanzarExcepcionSiValorNoValido)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.Estado = new EstadoFechaHoraRobusta(EstadoFechaHoraRobusta.EnumEstadoFechaHoraRobusta.ResultadoCorrecto);
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="value">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override EstadoFechaHoraRobusta Validar(object valor, bool lanzarExcepcionSiValorNoValido)
        {
            EstadoFechaHoraRobusta estado = new EstadoFechaHoraRobusta(EstadoFechaHoraRobusta.EnumEstadoFechaHoraRobusta.ResultadoCorrecto);

            if (estado.Valido)
            {
                if (!(valor is DateTime))
                {
                    estado.Valor = EstadoFechaHoraRobusta.EnumEstadoFechaHoraRobusta.ValorNoFecha;
                }
            }

            if (estado.Valido)
            {
                DateTime timeSpanValor = (DateTime)valor;
                if (timeSpanValor < this.MinValue)
                {
                    estado.Valor = EstadoFechaHoraRobusta.EnumEstadoFechaHoraRobusta.ValorInferiorMinimo;
                }
            }

            if (estado.Valido)
            {
                DateTime timeSpanValor = (DateTime)valor;
                if (timeSpanValor > this.MaxValue)
                {
                    estado.Valor = EstadoFechaHoraRobusta.EnumEstadoFechaHoraRobusta.ValorSuperiorMaximo;
                }
            }

            if (lanzarExcepcionSiValorNoValido && !estado.Valido)
            {
                this.LanzarExcepcion(estado);
            }

            return estado;
        }
        /// <summary>
        /// Lanza una exepción por no estár permitido el valor especificado
        /// </summary>
        /// <param name="value">valor no permitido</param>
        protected override void LanzarExcepcion(EstadoFechaHoraRobusta resultado)
        {
            switch (resultado.Valor)
            {
                case EstadoFechaHoraRobusta.EnumEstadoFechaHoraRobusta.ValorNoFecha:
                    throw new Exception("El campo " + this.Codigo + " no es una fecha válida.");
                    break;
                case EstadoFechaHoraRobusta.EnumEstadoFechaHoraRobusta.ValorInferiorMinimo:
                    throw new Exception("El campo " + this.Codigo + " es inferior al mínimo " + this.MinValue.ToString() + ".");
                    break;
                case EstadoFechaHoraRobusta.EnumEstadoFechaHoraRobusta.ValorSuperiorMaximo:
                    throw new Exception("El campo " + this.Codigo + " es superior al máximo " + this.MaxValue.ToString() + ".");
                    break;
            }
        }
        #endregion
    }

    /// <summary>
    /// Clase que informa del resultado de la asignación de un valor
    /// </summary>
    public class EstadoFechaHoraRobusta : EstadoRobusto
    {
        #region Propiedad(es)
        /// <summary>
        /// Valor del estado
        /// </summary>
        private EnumEstadoFechaHoraRobusta _Valor;
        /// <summary>
        /// Valor del estado
        /// </summary>
        public EnumEstadoFechaHoraRobusta Valor
        {
            get
            {
                return this._Valor;
            }
            set
            {
                this._Valor = value;
                this.Valido = value == EnumEstadoFechaHoraRobusta.ResultadoCorrecto;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public EstadoFechaHoraRobusta()
            : base()
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public EstadoFechaHoraRobusta(EnumEstadoFechaHoraRobusta valor)
            : base(valor == EnumEstadoFechaHoraRobusta.ResultadoCorrecto)
        {
            this.Valor = valor;
        }
        #endregion

        #region Enumerados
        /// <summary>
        /// Resultado de la validación del SafeBool
        /// </summary>
        public enum EnumEstadoFechaHoraRobusta
        {
            /// <summary>
            /// Resultado correcto
            /// </summary>
            ResultadoCorrecto = 0,
            /// <summary>
            /// El valor a asignar no es entero
            /// </summary>
            ValorNoFecha = 1,
            /// <summary>
            /// El valor a asignar es sueprior al máximo permitido
            /// </summary>
            ValorSuperiorMaximo = 2,
            /// <summary>
            /// El valor a asignar es inferior al mínimo permitido
            /// </summary>
            ValorInferiorMinimo = 3
        }
        #endregion
    }
}
