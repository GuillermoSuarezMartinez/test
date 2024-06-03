//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Orbita.VAComun;
using Orbita.VAControl;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Clase de acceso estático que contiene la lista de hardware de Entradas Salidas
    /// </summary>
    public static class OIOManager
    {
        #region Atributo(s)
        /// <summary>
        /// Lista del hardware de visión funcionando en el sistema
        /// </summary>
        public static List<OTarjetaIOBase> ListaTarjetasIO;
        #endregion

        #region Método(s) público(s)

        /// <summary>
        /// Construye los objetos
        /// </summary>
        public static void Constructor()
        {
            // Construyo la lista de tarjetas de IO
            ListaTarjetasIO = new List<OTarjetaIOBase>();

            // Consulta a la base de datos
            DataTable dt = AppBD.GetTarjetasIO();
            if (dt.Rows.Count > 0)
            {
                // Cargamos todas las funciones de visión existentes en el sistema
                foreach (DataRow dr in dt.Rows)
                {
                    string codMaquinaEstados = dr["CodHardware"].ToString();
                    string claseImplementadora = string.Format("{0}.{1}", Assembly.GetExecutingAssembly().GetName().Name, dr["ClaseImplementadora"].ToString());

                    object objetoImplementado;
                    if (App.ConstruirClase(Assembly.GetExecutingAssembly().GetName().Name, claseImplementadora, out objetoImplementado, codMaquinaEstados))
                    {
                        OTarjetaIOBase tarjetaIO = (OTarjetaIOBase)objetoImplementado;
                        ListaTarjetasIO.Add(tarjetaIO);
                    }
                }
            }
        }

        /// <summary>
        /// Destruye los objetos
        /// </summary>
        public static void Destructor()
        {
            // Destruyo la lista de tarjetas de IO
            ListaTarjetasIO = null;
        }

        /// <summary>
        /// Carga las propiedades de la base de datos
        /// </summary>
        public static void Inicializar()
        {
            foreach (OTarjetaIOBase tarjetaIO in ListaTarjetasIO)
            {
                tarjetaIO.Inicializar();
            }
        }

        /// <summary>
        /// Finaliza la ejecución
        /// </summary>
        public static void Finalizar()
        {
            foreach (OTarjetaIOBase tarjetaIO in ListaTarjetasIO)
            {
                tarjetaIO.Finalizar();
            }
        }

        /// <summary>
        /// Comienza una reproducción continua de la cámara
        /// </summary>
        /// <returns></returns>
        public static bool Start(string codigo)
        {
            foreach (OTarjetaIOBase tarjetaIO in ListaTarjetasIO)
            {
                if (tarjetaIO.Codigo == codigo)
                {
                    return tarjetaIO.Start();
                }
            }
            return false;
        }

        /// <summary>
        /// Termina una reproducción continua de la cámara
        /// </summary>
        /// <returns></returns>
        public static bool Stop(string codigo)
        {
            foreach (OTarjetaIOBase tarjetaIO in ListaTarjetasIO)
            {
                if (tarjetaIO.Codigo == codigo)
                {
                    return tarjetaIO.Stop();
                }
            }
            return false;
        }

        /// <summary>
        /// Comienza una reproducción de todas las cámaras
        /// </summary>
        /// <returns></returns>
        public static bool StartAll()
        {
            bool resultado = true;
            foreach (OTarjetaIOBase tarjetaIO in ListaTarjetasIO)
            {
                resultado &= tarjetaIO.Start();
            }
            return resultado;
        }

        /// <summary>
        /// Termina la reproducción de todas las cámaras
        /// </summary>
        /// <returns></returns>
        public static bool StopAll()
        {
            foreach (OTarjetaIOBase tarjetaIO in ListaTarjetasIO)
            {
                return tarjetaIO.Stop();
            }
            return false;
        }

        #endregion
    }

    /// <summary>
    /// Clase base para todos los módulos de EntradaSalida
    /// </summary>
    public class OTarjetaIOBase : IHardware
    {
        #region Propiedad(es)
        /// <summary>
        /// Código identificativo de la tarjeta de IO
        /// </summary>
        private string _Codigo;
        /// <summary>
        /// Código identificativo de la tarjeta de IO
        /// </summary>
        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }

        /// <summary>
        /// Nombre identificativo de la tarjeta de IO
        /// </summary>
        private string _Nombre;
        /// <summary>
        /// Nombre identificativo de la tarjeta de IO
        /// </summary>
        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        /// <summary>
        /// Descripción de la tarjeta de IO
        /// </summary>
        private string _Descripcion;
        /// <summary>
        /// Descripción de la tarjeta de IO
        /// </summary>
        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }

        /// <summary>
        /// Habilita o deshabilita el funcionamiento
        /// </summary>
        private bool _Habilitado;
        /// <summary>
        /// Habilita o deshabilita el funcionamiento
        /// </summary>
        public bool Habilitado
        {
            get { return _Habilitado; }
            set { _Habilitado = value; }

        }

        /// <summary>
        /// Tipo de hardware
        /// </summary>
        public OTipoHardware TipoHardware
        {
            get { return OTipoHardware.HwModuloIO; }
        }

        /// <summary>
        /// Tipo de tarjeta de IO
        /// </summary>
        private OTipoTarjetaIO _TipoTarjetaIO;
        /// <summary>
        /// Tipo de tarjeta de IO
        /// </summary>
        public OTipoTarjetaIO TipoTarjetaIO
        {
            get { return _TipoTarjetaIO; }
            set { _TipoTarjetaIO = value; }
        }

        /// <summary>
        /// Descripción del fabricante de la tarjeta de IO
        /// </summary>
        private string _Fabricante;
        /// <summary>
        /// Descripción del fabricante de la tarjeta de IO
        /// </summary>
        public string Fabricante
        {
            get { return _Fabricante; }
            set { _Fabricante = value; }
        }

        /// <summary>
        /// Descripción del modelo de la tarjeta de IO
        /// </summary>
        private string _Modelo;
        /// <summary>
        /// Descripción del modelo de la tarjeta de IO
        /// </summary>
        public string Modelo
        {
            get { return _Modelo; }
            set { _Modelo = value; }
        }

        /// <summary>
        /// Lista de todos los terminales de la tarjeta de IO
        /// </summary>              
        protected List<OTerminalIOBase> _ListaTerminales;
        /// <summary>
        /// Lista de todos los terminales de la tarjeta de IO
        /// </summary>              
        public List<OTerminalIOBase> ListaTerminales
        {
            get { return _ListaTerminales; }
            set { _ListaTerminales = value; }
        }

        /// <summary>
        /// Indica que se ha podido acceder al módulo con éxito
        /// </summary>
        private bool _Existe;
        /// <summary>
        /// Indica que se ha podido acceder al módulo con éxito
        /// </summary>
        public bool Existe
        {
            get { return _Existe; }
            set { _Existe = value; }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public OTarjetaIOBase(string codTarjetaIO)
        {
            // Inicialiamos los valores
            this._Codigo = codTarjetaIO;
            this._ListaTerminales = new List<OTerminalIOBase>();
            this._Existe = false;
        }
        #endregion

        #region Método(s) virtual(es)

        /// <summary>
        /// Método a heredar donde se conecta y se configura la tarjeta de IO
        /// </summary>
        public virtual void Inicializar()
        {
            // Cargamos valores de la base de datos
            DataTable dtTarjetaIO = AppBD.GetTarjetaIO(this._Codigo);
            if (dtTarjetaIO.Rows.Count == 1)
            {
                this._Nombre = dtTarjetaIO.Rows[0]["NombreHardware"].ToString();
                this._Descripcion = dtTarjetaIO.Rows[0]["DescHardware"].ToString();
                this._Habilitado = (bool)dtTarjetaIO.Rows[0]["HabilitadoHardware"];
                this._Fabricante = dtTarjetaIO.Rows[0]["Fabricante"].ToString();
                this._Modelo = dtTarjetaIO.Rows[0]["Modelo"].ToString();

                this.TipoTarjetaIO = OEnumRobusto<OTipoTarjetaIO>.Validar(dtTarjetaIO.Rows[0]["CodTipoHardware"].ToString(), OTipoTarjetaIO.USB_1024HLS);
                //this.TipoTarjetaIO = (OTipoTarjetaIO)App.EnumParse(typeof(OTipoTarjetaIO), dtTarjetaIO.Rows[0]["CodTipoHardware"].ToString(), OTipoTarjetaIO.USB_1024HLS);
            }
        }

        /// <summary>
        /// Método a heredar donde se desconecta y se libera la tarjeta de IO
        /// </summary>
        public virtual void Finalizar()
        {
        }

        /// <summary>
        /// Comienza el hardware
        /// </summary>
        /// <returns></returns>
        public virtual bool Start()
        {
            return true;
        }

        /// <summary>
        /// Para el hardware
        /// </summary>
        /// <returns></returns>
        public virtual bool Stop()
        {
            return true;
        }
        #endregion
    }

    /// <summary>
    /// Clase que simboliza los terminales de Entrada y/o Salida de la tarjeta
    /// </summary>
    public class OTerminalIOBase
    {
        #region Propiedad(es)
        /// <summary>
        /// Código identificativo del terminal de IO
        /// </summary>
        private string _Codigo;
        /// <summary>
        /// Código identificativo del terminal de IO
        /// </summary>
        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }

        /// <summary>
        /// Código identificativo de la tarjeta a la que pertenece el terminal de IO
        /// </summary>
        private string _CodTarjeta;
        /// <summary>
        /// Código identificativo de la tarjeta a la que pertenece el terminal de IO
        /// </summary>
        public string CodTarjeta
        {
            get { return _CodTarjeta; }
            set { _CodTarjeta = value; }
        }

        /// <summary>
        /// Nombre identificativo del terminal de IO
        /// </summary>
        private string _Nombre;
        /// <summary>
        /// Nombre identificativo del terminal de IO
        /// </summary>
        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        /// <summary>
        /// Descripción del terminal de IO
        /// </summary>
        private string _Descripcion;
        /// <summary>
        /// Descripción del terminal de IO
        /// </summary>
        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }

        /// <summary>
        /// Habilita o deshabilita el funcionamiento
        /// </summary>
        private bool _Habilitado;
        /// <summary>
        /// Habilita o deshabilita el funcionamiento
        /// </summary>
        public bool Habilitado
        {
            get { return _Habilitado; }
            set { _Habilitado = value; }
        }

        /// <summary>
        /// Tipo del terminal de IO
        /// </summary>
        private OTipoTerminalIO _TipoTerminalIO;
        /// <summary>
        /// Tipo de tarjeta de IO
        /// </summary>
        public OTipoTerminalIO TipoTerminalIO
        {
            get { return _TipoTerminalIO; }
            set { _TipoTerminalIO = value; }
        }

        /// <summary>
        /// Código de la variable asociada
        /// </summary>
        private string _CodVariable;
        /// <summary>
        /// Código de la variable asociada
        /// </summary>
        public string CodVariable
        {
            get { return _CodVariable; }
            set { _CodVariable = value; }
        }

        /// <summary>
        /// Número interno del terminal
        /// </summary>
        private int _Numero;
        /// <summary>
        /// Número interno del terminal
        /// </summary>
        public int Numero
        {
            get { return _Numero; }
            set { _Numero = value; }
        }

        /// <summary>
        /// Valor del terminal
        /// </summary>
        private object _Valor;
        /// <summary>
        /// Valor del terminal
        /// </summary>
        public virtual object Valor
        {
            get { return this._Valor; }
            set { this._Valor = value; }
        }
        /// <summary>
        /// Tipo de datos del valor del terminal
        /// </summary>
        private OEnumTipoDato _TipoDato;
        /// <summary>
        /// Tipo de datos del valor del terminal
        /// </summary>
        public OEnumTipoDato TipoDato
        {
            get { return _TipoDato; }
            set { _TipoDato = value; }
        }

        /// <summary>
        /// Indica que se ha podido acceder al módulo con éxito
        /// </summary>
        private bool _Existe;
        /// <summary>
        /// Indica que se ha podido acceder al módulo con éxito
        /// </summary>
        public bool Existe
        {
            get { return _Existe; }
            set { _Existe = value; }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public OTerminalIOBase(string codTarjetaIO, string codTerminalIO)
        {
            // Inicialiamos los valores
            this._CodTarjeta = codTarjetaIO;
            this._Codigo = codTerminalIO;
            this._Valor = null;
            this._Existe = false;

            // Cargamos valores de la base de datos
            DataTable dtTerminalIO = AppBD.GetTerminalIO(codTarjetaIO, this._Codigo);
            if (dtTerminalIO.Rows.Count == 1)
            {
                this._Nombre = dtTerminalIO.Rows[0]["NombreTerminalIO"].ToString();
                this._Descripcion = dtTerminalIO.Rows[0]["DescTerminalIO"].ToString();
                this._Habilitado = (bool)dtTerminalIO.Rows[0]["HabilitadoTerminalIO"];

                int intTipoTerminalIO = OEnteroRobusto.Validar(dtTerminalIO.Rows[0]["IdTipoTerminalIO"], 1, 4, 1);
                this._TipoTerminalIO = (OTipoTerminalIO)intTipoTerminalIO;

                this._CodVariable = dtTerminalIO.Rows[0]["CodVariable"].ToString();
                this._Numero = (int)dtTerminalIO.Rows[0]["Numero"];

                this._TipoDato = (OEnumTipoDato)OEnteroRobusto.Validar(dtTerminalIO.Rows[0]["IdTipoVariable"], 0, 99, 0);

                this._Valor = Orbita.VAComun.OTipoDato.DevaultValue(this._TipoDato);
            }
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Crea la suscripción a la variable
        /// </summary>
        protected void SuscribirAVariable()
        {
            // Suscripción a la variable
            switch (this._TipoTerminalIO)
            {
                case OTipoTerminalIO.EntradaDigital:
                    OVariablesManager.CrearSuscripcion(this._CodVariable, "IO", this._Codigo, this.EscribirEntrada);
                    break;
                case OTipoTerminalIO.SalidaDigital:
                case OTipoTerminalIO.SalidaComando:
                    OVariablesManager.CrearSuscripcion(this._CodVariable, "IO", this._Codigo, this.EscribirSalida);
                    break;
            }
        }

        /// <summary>
        /// Elimina la suscripción a la variable
        /// </summary>
        protected void EliminarSuscripcionAVariable()
        {
            // Suscripción a la variable
            switch (this._TipoTerminalIO)
            {
                case OTipoTerminalIO.EntradaDigital:
                    OVariablesManager.EliminarSuscripcion(this._CodVariable, "IO", this._Codigo, this.EscribirEntrada);
                    break;
                case OTipoTerminalIO.SalidaDigital:
                case OTipoTerminalIO.SalidaComando:
                    OVariablesManager.EliminarSuscripcion(this._CodVariable, "IO", this._Codigo, this.EscribirSalida);
                    break;
            }
        }

        /// <summary>
        /// Establece el valor de la variable
        /// </summary>
        protected void EstablecerValorVariable()
        {
            //this.EliminarSuscripcionAVariable();
            OVariablesManager.SetValue(this.CodVariable, this.Valor, "IO", this.Codigo);
            //this.SuscribirAVariable();
        }
        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Método a heredar donde se inicializan los terminales
        /// </summary>
        public virtual void Inicializar()
        {
            this.SuscribirAVariable();
        }

        /// <summary>
        /// Método a heredar donde se inicializan los terminales
        /// </summary>
        public virtual void Finalizar()
        {
            this.EliminarSuscripcionAVariable();
        }

        /// <summary>
        /// Lectura de la entrada física
        /// </summary>
        public virtual void LeerEntrada()
        {
            // Información extra
            OVALogsManager.Debug(OModulosHardware.Camaras, this.Codigo, string.Format("Lectura de entrada del terminal: {0} de la tarjeta {1}. Valor: {2}", this.Codigo, this.CodTarjeta, ORobusto.ToString(this.Valor)));
        }

        /// <summary>
        /// Simulación de la escritura de la entrada física
        /// </summary>
        public virtual void EscribirEntrada(string codigoVariable, object valor)
        {
            // Información extra
            OVALogsManager.Debug(OModulosHardware.Camaras, this.Codigo, string.Format("Escritura de entrada del terminal: {0} de la tarjeta {1}. Valor: {2}", this.Codigo, this.CodTarjeta, ORobusto.ToString(this.Valor)));
        }

        /// <summary>
        /// Escritura de la salida física
        /// </summary>
        public virtual void EscribirSalida(string codigoVariable, object valor)
        {
            // Información extra
            OVALogsManager.Debug(OModulosHardware.Camaras, this.Codigo, string.Format("Escritura de salida del terminal: {0} de la tarjeta {1}. Valor: {2}", this.Codigo, this.CodTarjeta, ORobusto.ToString(this.Valor)));
        }

        /// <summary>
        /// Lectura de la salida física
        /// </summary>
        public virtual void LeerSalida()
        {
            // Información extra
            OVALogsManager.Debug(OModulosHardware.Camaras, this.Codigo, string.Format("Lectura de salida del terminal: {0} de la tarjeta {1}. Valor: {2}", this.Codigo, this.CodTarjeta, ORobusto.ToString(this.Valor)));
        }
        #endregion
    }

    /// <summary>
    /// Enumerado que identifica a los tipos de tarjetas de IO
    /// </summary>
    public enum OTipoTarjetaIO
    {
        /// <summary>
        /// Unidad USB Externa de Measurement Computing
        /// </summary>
        USB_1024HLS = 1,
        /// <summary>
        /// Unidad USB Externa de Measurement Computing
        /// </summary>
        Axis223M = 2,
        /// <summary>
        /// Cliente SCED
        /// </summary>
        SCED = 3,
    }

    /// <summary>
    /// Enumerado que identifica a los tipos de terminales de IO
    /// </summary>
    public enum OTipoTerminalIO
    {
        /// <summary>
        /// Entrada digital
        /// </summary>
        [OStringValue("Entrada digital")]
        EntradaDigital = 1,

        /// <summary>
        /// Salida digital
        /// </summary>
        [OStringValue("Salida digital")]
        SalidaDigital = 2,

        /// <summary>
        /// Comando de Entrada
        /// </summary>
        [OStringValue("Comando de entrada")]
        EntradaComando = 3,

        /// <summary>
        /// Comando de Entrada
        /// </summary>
        [OStringValue("Comando de salida")]
        SalidaComando = 4
    }
}

