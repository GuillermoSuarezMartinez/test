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
    /// <summary>
    /// Clase base para el trabajo de forma robusta con variables
    /// </summary>
    public class OObjetoRobusto<ClaseTipo, EstadoTipo>
        where EstadoTipo : OEstadoObjetoRobusto, new()
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
                    OVALogsManager.Debug(OModulosSistema.Comun, "Obtener Valor de forma segura", "No admite el valor " + value.ToString());
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
        public OObjetoRobusto(string codigo, ClaseTipo valorDefecto, bool lanzarExcepcionSiValorNoValido)
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
    public class OEstadoObjetoRobusto
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
        public OEstadoObjetoRobusto()
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OEstadoObjetoRobusto(bool valido)
        {
            this.Valido = valido;
        }
        #endregion
    }

    /// <summary>
    /// Asignación de una variable a un campo de tipo texto
    /// </summary>
    public class OTextoRobusto : OObjetoRobusto<string, OEstadoTextoRobusto>
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
        public OTextoRobusto(string codigo, int maxLength, bool admiteVacio, string valorDefecto, bool lanzarExcepcionSiValorNoValido)
            : base(codigo, valorDefecto, lanzarExcepcionSiValorNoValido)
        {
            this.MaxLength = maxLength;
            this.AdmiteVacio = admiteVacio;
            this.Estado = new OEstadoTextoRobusto(OEstadoTextoRobusto.EnumEstadoTextoSeguro.ResultadoCorrecto);
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="value">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override OEstadoTextoRobusto Validar(object valor, bool lanzarExcepcionSiValorNoValido)
        {
            OEstadoTextoRobusto estado = new OEstadoTextoRobusto(OEstadoTextoRobusto.EnumEstadoTextoSeguro.ResultadoCorrecto);

            if (estado.Valido)
            {
                if (!(valor is string))
                {
                    estado.Valor = OEstadoTextoRobusto.EnumEstadoTextoSeguro.ValorNoString;
                }
            }

            if (estado.Valido)
            {
                string strValor = (string)valor;
                if (strValor.Length > this.MaxLength)
                {
                    estado.Valor = OEstadoTextoRobusto.EnumEstadoTextoSeguro.LongitudSobrepasada;
                }
            }

            if (estado.Valido)
            {
                string strValor = (string)valor;
                if (!this.AdmiteVacio && (strValor == string.Empty))
                {
                    estado.Valor = OEstadoTextoRobusto.EnumEstadoTextoSeguro.CadenaVacia;
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
        protected override void LanzarExcepcion(OEstadoTextoRobusto resultado)
        {
            switch (resultado.Valor)
            {
                case OEstadoTextoRobusto.EnumEstadoTextoSeguro.ValorNoString:
                    throw new Exception("El campo " + this.Codigo + " no es válido.");
                    break;
                case OEstadoTextoRobusto.EnumEstadoTextoSeguro.LongitudSobrepasada:
                    throw new Exception("El campo " + this.Codigo + " es demasiado largo.");
                    break;
                case OEstadoTextoRobusto.EnumEstadoTextoSeguro.CadenaVacia:
                    throw new Exception("El campo " + this.Codigo + " no puede estar en blanco.");
                    break;
            }
        }
        #endregion
    }

    /// <summary>
    /// Clase que informa del resultado de la asignación de un valor
    /// </summary>
    public class OEstadoTextoRobusto : OEstadoObjetoRobusto
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
        public OEstadoTextoRobusto()
            : base()
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OEstadoTextoRobusto(EnumEstadoTextoSeguro tipoError)
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
    public class OStringEnumRobusto : OObjetoRobusto<string, OEstadoEnumRobusto>
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
            this.Estado = new OEstadoEnumRobusto(OEstadoEnumRobusto.EnumEstadoEnumRobusto.ResultadoCorrecto);
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="value">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override OEstadoEnumRobusto Validar(object valor, bool lanzarExcepcionSiValorNoValido)
        {
            OEstadoEnumRobusto estado = new OEstadoEnumRobusto(OEstadoEnumRobusto.EnumEstadoEnumRobusto.ResultadoCorrecto);

            if (estado.Valido)
            {
                if (!(valor is string))
                {
                    estado.Valor = OEstadoEnumRobusto.EnumEstadoEnumRobusto.ValorNoEnumerado;
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
                    estado.Valor = OEstadoEnumRobusto.EnumEstadoEnumRobusto.ValorNoPermitido;
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
        protected override void LanzarExcepcion(OEstadoEnumRobusto resultado)
        {
            switch (resultado.Valor)
            {
                case OEstadoEnumRobusto.EnumEstadoEnumRobusto.ValorNoEnumerado:
                    throw new Exception("El campo " + this.Codigo + " no es válido.");
                    break;
                case OEstadoEnumRobusto.EnumEstadoEnumRobusto.ValorNoPermitido:
                    throw new Exception("El campo " + this.Codigo + " no está permitido.");
                    break;
            }
        }
        #endregion
    }

    /// <summary>
    /// Asignación de una variable a un campo de tipo enumerado (aunque internamente trabaja como un string)
    /// </summary>
    public class OEnumRobusto<T> : OObjetoRobusto<T, OEstadoEnumRobusto>
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
                        this._Valor = (T)Enum.Parse(typeof(T), (string)value, true);
                    }
                    else if (App.IsNumericInt(value))
                    {
                        T tValor = (T)value;
                    }
                }
                else
                {
                    this._Valor = ValorPorDefecto;
                    OVALogsManager.Debug(OModulosSistema.Comun, "ValorGenerico", "No admite el valor " + value.ToString());
                }
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OEnumRobusto(string codigo, T valorDefecto, bool lanzarExcepcionSiValorNoValido)
            : base(codigo, valorDefecto, lanzarExcepcionSiValorNoValido)
        {
            this.Estado = new OEstadoEnumRobusto(OEstadoEnumRobusto.EnumEstadoEnumRobusto.ResultadoCorrecto);
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="value">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override OEstadoEnumRobusto Validar(object valor, bool lanzarExcepcionSiValorNoValido)
        {
            OEstadoEnumRobusto estado = new OEstadoEnumRobusto(OEstadoEnumRobusto.EnumEstadoEnumRobusto.ValorNoEnumerado);

            if (!estado.Valido)
            {
                if ((valor is T) && (typeof(T).IsEnum))
                {
                    estado.Valor = OEstadoEnumRobusto.EnumEstadoEnumRobusto.ResultadoCorrecto;
                }
            }

            if (!estado.Valido)
            {
                if (valor is string)
                {
                    string strValor = (string)valor;
                    try
                    {
                        T tValor = (T)Enum.Parse(typeof(T), strValor, true);
                        estado.Valor = OEstadoEnumRobusto.EnumEstadoEnumRobusto.ResultadoCorrecto;
                    }
                    catch
                    {
                        estado.Valor = OEstadoEnumRobusto.EnumEstadoEnumRobusto.ValorNoPermitido;
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
                        estado.Valor = OEstadoEnumRobusto.EnumEstadoEnumRobusto.ResultadoCorrecto;
                    }
                    catch
                    {
                        estado.Valor = OEstadoEnumRobusto.EnumEstadoEnumRobusto.ValorNoPermitido;
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
        protected override void LanzarExcepcion(OEstadoEnumRobusto resultado)
        {
            switch (resultado.Valor)
            {
                case OEstadoEnumRobusto.EnumEstadoEnumRobusto.ValorNoEnumerado:
                    throw new Exception("El campo " + this.Codigo + " no es válido.");
                    break;
                case OEstadoEnumRobusto.EnumEstadoEnumRobusto.ValorNoPermitido:
                    throw new Exception("El campo " + this.Codigo + " no está permitido.");
                    break;
            }
        }
        #endregion
    }

    /// <summary>
    /// Clase que informa del resultado de la asignación de un valor
    /// </summary>
    public class OEstadoEnumRobusto : OEstadoObjetoRobusto
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
        public OEstadoEnumRobusto()
            : base()
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OEstadoEnumRobusto(EnumEstadoEnumRobusto valor)
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
    public class OEnteroRobusto : OObjetoRobusto<int, OEstadoEnteroRobusto>
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
                    int intValor = 0;
                    if (App.IsNumericInt(value))
                    {
                        intValor = Convert.ToInt32(value);
                    }
                    else
                    {
                        if (value is string)
                        {
                            int.TryParse((string)value, NumberStyles.Any, CultureInfo.InvariantCulture, out intValor);
                        }
                    }

                    this._Valor = intValor;
                }
                else
                {
                    this._Valor = ValorPorDefecto;
                    OVALogsManager.Debug(OModulosSistema.Comun, "Obtener Valor de forma segura", "No admite el valor " + value.ToString());
                }
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OEnteroRobusto(string codigo, int minValue, int maxValue, int valorDefecto, bool lanzarExcepcionSiValorNoValido)
            : base(codigo, valorDefecto, lanzarExcepcionSiValorNoValido)
        {
            Nullable<int> a = 1;

            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.Estado = new OEstadoEnteroRobusto(OEstadoEnteroRobusto.EnumEstadoEnteroRobusto.ResultadoCorrecto);
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="value">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override OEstadoEnteroRobusto Validar(object valor, bool lanzarExcepcionSiValorNoValido)
        {
            OEstadoEnteroRobusto estado = new OEstadoEnteroRobusto(OEstadoEnteroRobusto.EnumEstadoEnteroRobusto.ResultadoCorrecto);

            int intValor = 0;
            if (estado.Valido)
            {
                if (App.IsNumericInt(valor))
                {
                    intValor = Convert.ToInt32(valor);
                }
                else
                {
                    if (!(valor is string) || !int.TryParse((string)valor, NumberStyles.Any, CultureInfo.InvariantCulture, out intValor))
                    {
                        estado.Valor = OEstadoEnteroRobusto.EnumEstadoEnteroRobusto.ValorNoEntero;
                    }
                }
            }

            if (estado.Valido)
            {
                if (intValor < this.MinValue)
                {
                    estado.Valor = OEstadoEnteroRobusto.EnumEstadoEnteroRobusto.ValorInferiorMinimo;
                }

                if (intValor > this.MaxValue)
                {
                    estado.Valor = OEstadoEnteroRobusto.EnumEstadoEnteroRobusto.ValorSuperiorMaximo;
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
        protected override void LanzarExcepcion(OEstadoEnteroRobusto resultado)
        {
            switch (resultado.Valor)
            {
                case OEstadoEnteroRobusto.EnumEstadoEnteroRobusto.ValorNoEntero:
                    throw new Exception("El campo " + this.Codigo + " no es un número entero.");
                    break;
                case OEstadoEnteroRobusto.EnumEstadoEnteroRobusto.ValorInferiorMinimo:
                    throw new Exception("El campo " + this.Codigo + " es inferior al mínimo " + this.MinValue.ToString() + ".");
                    break;
                case OEstadoEnteroRobusto.EnumEstadoEnteroRobusto.ValorSuperiorMaximo:
                    throw new Exception("El campo " + this.Codigo + " es superior al máximo " + this.MaxValue.ToString() + ".");
                    break;
            }
        }
        #endregion
    }

    /// <summary>
    /// Clase que informa del resultado de la asignación de un valor
    /// </summary>
    public class OEstadoEnteroRobusto : OEstadoObjetoRobusto
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
        public OEstadoEnteroRobusto()
            : base()
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OEstadoEnteroRobusto(EnumEstadoEnteroRobusto valor)
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
    public class ODecimalRobusto : OObjetoRobusto<double, OEstadoDecimalRobusto>
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
                    double doubleValor = 0;
                    if (App.IsNumericFloat(value))
                    {
                        doubleValor = Convert.ToDouble(value);
                    }
                    else
                    {
                        if (value is string)
                        {
                            double.TryParse((string)value, NumberStyles.Any, CultureInfo.InvariantCulture, out doubleValor);
                        }
                    }

                    this._Valor = doubleValor;
                }
                else
                {
                    this._Valor = ValorPorDefecto;
                    OVALogsManager.Debug(OModulosSistema.Comun, "Obtener Valor de forma segura", "No admite el valor " + value.ToString());
                }
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ODecimalRobusto(string codigo, double minValue, double maxValue, double valorDefecto, bool lanzarExcepcionSiValorNoValido)
            : base(codigo, valorDefecto, lanzarExcepcionSiValorNoValido)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.Estado = new OEstadoDecimalRobusto(OEstadoDecimalRobusto.EnumEstadoDecimalRobusto.ResultadoCorrecto);
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="value">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override OEstadoDecimalRobusto Validar(object valor, bool lanzarExcepcionSiValorNoValido)
        {
            OEstadoDecimalRobusto estado = new OEstadoDecimalRobusto(OEstadoDecimalRobusto.EnumEstadoDecimalRobusto.ResultadoCorrecto);

            double doubleValor = 0;
            if (estado.Valido)
            {
                if (App.IsNumericFloat(valor))
                {
                    doubleValor = Convert.ToDouble(valor);
                }
                else
                {
                    if (!(valor is string) || !double.TryParse((string)valor, NumberStyles.Any, CultureInfo.InvariantCulture, out doubleValor))
                    {
                        estado.Valor = OEstadoDecimalRobusto.EnumEstadoDecimalRobusto.ValorNoDoble;
                    }
                }
            }

            if (estado.Valido)
            {
                if (doubleValor < this.MinValue)
                {
                    estado.Valor = OEstadoDecimalRobusto.EnumEstadoDecimalRobusto.ValorInferiorMinimo;
                }

                if (doubleValor > this.MaxValue)
                {
                    estado.Valor = OEstadoDecimalRobusto.EnumEstadoDecimalRobusto.ValorSuperiorMaximo;
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
        protected override void LanzarExcepcion(OEstadoDecimalRobusto resultado)
        {
            switch (resultado.Valor)
            {
                case OEstadoDecimalRobusto.EnumEstadoDecimalRobusto.ValorNoDoble:
                    throw new Exception("El campo " + this.Codigo + " no es un número decimal.");
                    break;
                case OEstadoDecimalRobusto.EnumEstadoDecimalRobusto.ValorInferiorMinimo:
                    throw new Exception("El campo " + this.Codigo + " es inferior al mínimo " + this.MinValue.ToString() + ".");
                    break;
                case OEstadoDecimalRobusto.EnumEstadoDecimalRobusto.ValorSuperiorMaximo:
                    throw new Exception("El campo " + this.Codigo + " es superior al máximo " + this.MaxValue.ToString() + ".");
                    break;
            }
        }
        #endregion
    }

    /// <summary>
    /// Clase que informa del resultado de la asignación de un valor
    /// </summary>
    public class OEstadoDecimalRobusto : OEstadoObjetoRobusto
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
        public OEstadoDecimalRobusto()
            : base()
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OEstadoDecimalRobusto(EnumEstadoDecimalRobusto valor)
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
            /// El valor a asignar no es decimal
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
    public class OBoolRobusto : OObjetoRobusto<bool, OEstadoBoolRobusto>
    {
        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OBoolRobusto(string codigo, bool valorDefecto, bool lanzarExcepcionSiValorNoValido)
            : base(codigo, valorDefecto, lanzarExcepcionSiValorNoValido)
        {
            this.Estado = new OEstadoBoolRobusto(OEstadoBoolRobusto.EnumEstadoBoolRobusto.ResultadoCorrecto);
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="value">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override OEstadoBoolRobusto Validar(object valor, bool lanzarExcepcionSiValorNoValido)
        {
            OEstadoBoolRobusto estado = new OEstadoBoolRobusto(OEstadoBoolRobusto.EnumEstadoBoolRobusto.ResultadoCorrecto);

            if (estado.Valido)
            {
                if (!(valor is bool))
                {
                    estado.Valor = OEstadoBoolRobusto.EnumEstadoBoolRobusto.ValorNoBooleano;
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
        protected override void LanzarExcepcion(OEstadoBoolRobusto resultado)
        {
            if (resultado.Valor == OEstadoBoolRobusto.EnumEstadoBoolRobusto.ValorNoBooleano)
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
    public class OEstadoBoolRobusto : OEstadoObjetoRobusto
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
        public OEstadoBoolRobusto()
            : base()
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OEstadoBoolRobusto(EnumEstadoBoolRobusto valor)
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
    public class OIntervaloTiempoRobusto : OObjetoRobusto<TimeSpan, OEstadoIntervaloTiempoRobusto>
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
        public OIntervaloTiempoRobusto(string codigo, TimeSpan minValue, TimeSpan maxValue, TimeSpan valorDefecto, bool lanzarExcepcionSiValorNoValido)
            : base(codigo, valorDefecto, lanzarExcepcionSiValorNoValido)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.Estado = new OEstadoIntervaloTiempoRobusto(OEstadoIntervaloTiempoRobusto.EnumEstadoIntervaloTiempoRobusto.ResultadoCorrecto);
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="value">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override OEstadoIntervaloTiempoRobusto Validar(object valor, bool lanzarExcepcionSiValorNoValido)
        {
            OEstadoIntervaloTiempoRobusto estado = new OEstadoIntervaloTiempoRobusto(OEstadoIntervaloTiempoRobusto.EnumEstadoIntervaloTiempoRobusto.ResultadoCorrecto);

            if (estado.Valido)
            {
                if (!(valor is TimeSpan))
                {
                    estado.Valor = OEstadoIntervaloTiempoRobusto.EnumEstadoIntervaloTiempoRobusto.ValorNoIntervaloTiempo;
                }
            }

            if (estado.Valido)
            {
                TimeSpan timeSpanValor = (TimeSpan)valor;
                if (timeSpanValor < this.MinValue)
                {
                    estado.Valor = OEstadoIntervaloTiempoRobusto.EnumEstadoIntervaloTiempoRobusto.ValorInferiorMinimo;
                }
            }

            if (estado.Valido)
            {
                TimeSpan timeSpanValor = (TimeSpan)valor;
                if (timeSpanValor > this.MaxValue)
                {
                    estado.Valor = OEstadoIntervaloTiempoRobusto.EnumEstadoIntervaloTiempoRobusto.ValorSuperiorMaximo;
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
        protected override void LanzarExcepcion(OEstadoIntervaloTiempoRobusto resultado)
        {
            switch (resultado.Valor)
            {
                case OEstadoIntervaloTiempoRobusto.EnumEstadoIntervaloTiempoRobusto.ValorNoIntervaloTiempo:
                    throw new Exception("El campo " + this.Codigo + " no es un periodo de tiempo válido.");
                    break;
                case OEstadoIntervaloTiempoRobusto.EnumEstadoIntervaloTiempoRobusto.ValorInferiorMinimo:
                    throw new Exception("El campo " + this.Codigo + " es inferior al mínimo " + this.MinValue.ToString() + ".");
                    break;
                case OEstadoIntervaloTiempoRobusto.EnumEstadoIntervaloTiempoRobusto.ValorSuperiorMaximo:
                    throw new Exception("El campo " + this.Codigo + " es superior al máximo " + this.MaxValue.ToString() + ".");
                    break;
            }
        }
        #endregion
    }

    /// <summary>
    /// Clase que informa del resultado de la asignación de un valor
    /// </summary>
    public class OEstadoIntervaloTiempoRobusto : OEstadoObjetoRobusto
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
        public OEstadoIntervaloTiempoRobusto()
            : base()
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OEstadoIntervaloTiempoRobusto(EnumEstadoIntervaloTiempoRobusto valor)
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
    public class OFechaHoraRobusta : OObjetoRobusto<DateTime, OEstadoFechaHoraRobusta>
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
        public OFechaHoraRobusta(string codigo, DateTime minValue, DateTime maxValue, DateTime valorDefecto, bool lanzarExcepcionSiValorNoValido)
            : base(codigo, valorDefecto, lanzarExcepcionSiValorNoValido)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.Estado = new OEstadoFechaHoraRobusta(OEstadoFechaHoraRobusta.EnumEstadoFechaHoraRobusta.ResultadoCorrecto);
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Comprueba que el valor del objeto es correcto
        /// </summary>
        /// <param name="value">Valor del objeto a comprobar</param>
        /// <returns>Verdadero si el valor es correcto</returns>
        public override OEstadoFechaHoraRobusta Validar(object valor, bool lanzarExcepcionSiValorNoValido)
        {
            OEstadoFechaHoraRobusta estado = new OEstadoFechaHoraRobusta(OEstadoFechaHoraRobusta.EnumEstadoFechaHoraRobusta.ResultadoCorrecto);

            if (estado.Valido)
            {
                if (!(valor is DateTime))
                {
                    estado.Valor = OEstadoFechaHoraRobusta.EnumEstadoFechaHoraRobusta.ValorNoFecha;
                }
            }

            if (estado.Valido)
            {
                DateTime timeSpanValor = (DateTime)valor;
                if (timeSpanValor < this.MinValue)
                {
                    estado.Valor = OEstadoFechaHoraRobusta.EnumEstadoFechaHoraRobusta.ValorInferiorMinimo;
                }
            }

            if (estado.Valido)
            {
                DateTime timeSpanValor = (DateTime)valor;
                if (timeSpanValor > this.MaxValue)
                {
                    estado.Valor = OEstadoFechaHoraRobusta.EnumEstadoFechaHoraRobusta.ValorSuperiorMaximo;
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
        protected override void LanzarExcepcion(OEstadoFechaHoraRobusta resultado)
        {
            switch (resultado.Valor)
            {
                case OEstadoFechaHoraRobusta.EnumEstadoFechaHoraRobusta.ValorNoFecha:
                    throw new Exception("El campo " + this.Codigo + " no es una fecha válida.");
                    break;
                case OEstadoFechaHoraRobusta.EnumEstadoFechaHoraRobusta.ValorInferiorMinimo:
                    throw new Exception("El campo " + this.Codigo + " es inferior al mínimo " + this.MinValue.ToString() + ".");
                    break;
                case OEstadoFechaHoraRobusta.EnumEstadoFechaHoraRobusta.ValorSuperiorMaximo:
                    throw new Exception("El campo " + this.Codigo + " es superior al máximo " + this.MaxValue.ToString() + ".");
                    break;
            }
        }
        #endregion
    }

    /// <summary>
    /// Clase que informa del resultado de la asignación de un valor
    /// </summary>
    public class OEstadoFechaHoraRobusta : OEstadoObjetoRobusto
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
        public OEstadoFechaHoraRobusta()
            : base()
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OEstadoFechaHoraRobusta(EnumEstadoFechaHoraRobusta valor)
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
