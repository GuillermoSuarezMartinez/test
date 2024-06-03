using System;
using System.Collections.Generic;
using System.Data;
using Orbita.VAComun;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Clase base para todos los controladores ptz
    /// </summary>
    public class OPTZBase
    {
        #region Propiedad(es)
        /// <summary>
        /// Código identificador de la cámara
        /// </summary>
        private string _Codigo;
        /// <summary>
        /// Código identificador de la cámara
        /// </summary>
        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }

        /// <summary>
        /// Listado de valores actuales
        /// </summary>
        protected OPosicionesPTZ _Posicion;
        /// <summary>
        /// Listado de valores actuales
        /// </summary>
        public OPosicionesPTZ Posicion
        {
            get { return _Posicion; }
            set { _Posicion = value; }
        }

        /// <summary>
        /// Dispitivo habilitado
        /// </summary>
        private bool _Habilitado;
        /// <summary>
        /// Dispitivo habilitado
        /// </summary>
        public bool Habilitado
        {
            get { return _Habilitado; }
            set { _Habilitado = value; }
        }
        #endregion

        #region Propiedad(es) Virtual(es)
        /// <summary>
        /// Obtiene la información de los comandos permitidos por el dispositivo PTZ
        /// </summary>
        public virtual OMovimientosPTZ ComandosPermitidos
        {
            get { return new OMovimientosPTZ() { }; }
        }

        /// <summary>
        /// Obtiene la información de los movimientos permitidos por el dispositivo PTZ
        /// </summary>
        public virtual OTiposMovimientosPTZ MovimientosPermitidos
        {
            get { return new OTiposMovimientosPTZ() { }; }
        }
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OPTZBase(string codigo)
        {
			try
			{
                this.Codigo = codigo;

			    DataTable dt = AppBD.GetCamara(codigo);
                if (dt.Rows.Count == 1)
                {
                    this.Habilitado = OBoolRobusto.Validar(dt.Rows[0]["PTZ_Habilitado"], false);
                }

                this._Posicion = new OPosicionesPTZ();
            }
            catch (Exception exception)
            {
                OVALogsManager.Fatal(ModulosHardware.Camaras, this.Codigo, exception);
                throw new Exception("Imposible iniciar el PTZ " + this.Codigo);
            }
        }
        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Carga los valores de la cámara
        /// </summary>
        public virtual void Inicializar()
        {
        }

        /// <summary>
        /// Finaliza la cámara
        /// </summary>
        public virtual void Finalizar()
        {
        }

        /// <summary>
        /// Ejecuta un movimiento simple de ptz
        /// </summary>
        /// <param name="tipo">Tipo de movimiento ptz a ejecutar</param>
        /// <param name="modo">Modo de movimiento: Absoluto o relativo</param>
        /// <param name="valor">valor a establecer</param>
        /// <returns>Verdadero si se ha ejecutado correctamente</returns>
        public virtual bool EjecutaMovimiento(OEnumTipoMovimientoPTZ tipo, OEnumModoMovimientoPTZ modo, double valor)
        {
            OComandosPTZ comandos = new OComandosPTZ();
            comandos.Add(tipo, modo, valor);

            return this.EjecutaMovimiento(comandos);
        }

        /// <summary>
        /// Ejecuta un movimiento simple de ptz
        /// </summary>
        /// <param name="movimiento">Tipo de movimiento ptz a ejecutar</param>
        /// <param name="valor">valor a establecer</param>
        /// <returns>Verdadero si se ha ejecutado correctamente</returns>
        public virtual bool EjecutaMovimiento(OMovimientoPTZ movimiento, double valor)
        {
            OComandosPTZ comandos = new OComandosPTZ();
            comandos.Add(movimiento, valor);

            return this.EjecutaMovimiento(comandos);
        }

        /// <summary>
        /// Ejecuta un movimiento simple de ptz
        /// </summary>
        /// <param name="comando">Comando del movimiento ptz a ejecutar</param>
        /// <returns>Verdadero si se ha ejecutado correctamente</returns>
        public virtual bool EjecutaMovimiento(OComandoPTZ comando)
        {
            OComandosPTZ valores = new OComandosPTZ();
            valores.Add(comando);

            return this.EjecutaMovimiento(valores);
        }

        /// <summary>
        /// Ejecuta un movimiento compuesto de ptz
        /// </summary>
        /// <param name="valores">Tipos de movimientos y valores</param>
        /// <returns>Verdadero si se ha ejecutado correctamente</returns>
        public virtual bool EjecutaMovimiento(OComandosPTZ valores)
        {
            bool resultado = false;

            foreach (OComandoPTZ comando in valores)
            {
                resultado = this.ComandosPermitidos.Exists(comando.Movimiento);
                if (!resultado)
                {
                    break;
                }
            }

            return resultado;
        }

        /// <summary>
        /// Actualiza la posición actual del PTZ
        /// </summary>
        /// <returns></returns>
        public virtual OPosicionesPTZ ConsultaPosicion()
        {
            foreach (OEnumTipoMovimientoPTZ tipoMovimiento in this.MovimientosPermitidos)
            {
                this.ConsultaPosicion(tipoMovimiento);
            }

            return this._Posicion;
        }

        /// <summary>
        /// Actualiza la posición actual de un determinado movimiento PTZ
        /// </summary>
        /// <returns>Listado de posiciones actuales</returns>
        public virtual OPosicionPTZ ConsultaPosicion(OEnumTipoMovimientoPTZ movimiento)
        {
            return this._Posicion[movimiento];
        }
        #endregion
    }

    /// <summary>
    /// Listado de parámetros PTZ
    /// </summary>
    public class OComandosPTZ : List<OComandoPTZ>
    {
        #region Propiedad(es)
        /// <summary>
        /// Propiedad para acceder a un item de la lista
        /// </summary>
        /// <param name="tipo">Tipo de movimiento</param>
        /// <param name="modo">Modo de movimiento</param>
        /// <returns>Comando</returns>
        public OComandoPTZ this[OEnumTipoMovimientoPTZ tipo, OEnumModoMovimientoPTZ modo]
        {
            get
            {
                OMovimientoPTZ movimiento = new OMovimientoPTZ(tipo, modo);
                return this.Find(movimiento);
            }
        }

        /// <summary>
        /// Propiedad para acceder a un item de la lista
        /// </summary>
        /// <param name="tipo">Tipo de movimiento</param>
        /// <param name="modo">Modo de movimiento</param>
        /// <returns>Comando</returns>
        public OComandoPTZ this[OMovimientoPTZ movimiento]
        {
            get
            {
                return this.Find(movimiento);
            }
        }
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OComandosPTZ()
        {
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OComandosPTZ(params OComandoPTZ[] valores)
        {
            this.AddRange(valores);
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Añade un comando de movimiento
        /// </summary>
        /// <param name="tipo">Tipo de movimiento</param>
        /// <param name="modo">Indica si el movimiento es de tipo absoluto o relativo</param>
        /// <param name="valor">Valor a establecer</param>
        public void Add(OEnumTipoMovimientoPTZ tipo, OEnumModoMovimientoPTZ modo, double valor)
        {
            OComandoPTZ comandoAux = new OComandoPTZ(tipo, modo, valor);
            this.Add(comandoAux);
        }

        /// <summary>
        /// Añade un comando de movimiento
        /// </summary>
        /// <param name="movimiento">Tipo de movimiento</param>
        /// <param name="valor">Valor a establecer</param>
        public void Add(OMovimientoPTZ movimiento, double valor)
        {
            OComandoPTZ comandoAux = new OComandoPTZ(movimiento, valor);
            this.Add(comandoAux);
        }

        /// <summary>
        /// Añade un comando de movimiento
        /// </summary>
        /// <param name="comando">Comando a añadir</param>
        public new void Add(OComandoPTZ comando)
        {
            if (this.Exists(comando.Movimiento))
            {
                OComandoPTZ comandoAux = this.Find(comando.Movimiento);
                comandoAux.Valor = comando.Valor;
            }
            else
            {
                base.Add(comando);
            }
        }

        /// <summary>
        /// Indica que el tipo de movimiento ya existe en la lista de comandos
        /// </summary>
        /// <param name="movimiento">Tipo de movimiento a buscar</param>
        /// <returns>Verdadero si el tipo de movimiento ya existe en la lista de comandos</returns>
        public bool Exists(OMovimientoPTZ movimiento)
        {
            return base.Exists(delegate(OComandoPTZ comando) { return (comando.Movimiento.Tipo == movimiento.Tipo) && (comando.Movimiento.Modo == movimiento.Modo); });
        }

        /// <summary>
        /// Busca el tipo de movimiento en la lista de comandos
        /// </summary>
        /// <param name="movimiento">Tipo de movimiento a buscar</param>
        /// <returns>Información del movimiento</returns>
        public OComandoPTZ Find(OMovimientoPTZ movimiento)
        {
            return base.Find(delegate(OComandoPTZ comando) { return (comando.Movimiento.Tipo == movimiento.Tipo) && (comando.Movimiento.Modo == movimiento.Modo); });
        }
        #endregion
    }

    /// <summary>
    /// Listado de parámetros PTZ
    /// </summary>
    public class OPosicionesPTZ : List<OPosicionPTZ>
    {
        #region Propiedad(es)
        /// <summary>
        /// Propiedad para acceder a un item de la lista
        /// </summary>
        /// <param name="tipo">Tipo de movimiento</param>
        /// <returns>Posicion</returns>
        public OPosicionPTZ this[OEnumTipoMovimientoPTZ tipo]
        {
            get
            {
                return this.Find(tipo);
            }
        }
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OPosicionesPTZ()
        {
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OPosicionesPTZ(params OPosicionPTZ[] valores)
        {
            this.AddRange(valores);
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Añade un comando de movimiento
        /// </summary>
        /// <param name="movimiento">Tipo de movimiento</param>
        /// <param name="valor">Valor a establecer</param>
        public void Add(OEnumTipoMovimientoPTZ tipo, double valor)
        {
            OPosicionPTZ posicion = new OPosicionPTZ(tipo, valor);
            this.Add(posicion);
        }

        /// <summary>
        /// Añade un comando de movimiento
        /// </summary>
        /// <param name="posicion">Comando a añadir</param>
        public new void Add(OPosicionPTZ posicion)
        {
            if (this.Exists(posicion.Tipo))
            {
                OPosicionPTZ posicionAux = this.Find(posicion.Tipo);
                posicionAux.Valor = posicion.Valor;
            }
            else
            {
                base.Add(posicion);
            }
        }

        /// <summary>
        /// Indica que el tipo de movimiento ya existe en la lista de comandos
        /// </summary>
        /// <param name="movimiento">Tipo de movimiento a buscar</param>
        /// <returns>Verdadero si el tipo de movimiento ya existe en la lista de comandos</returns>
        public bool Exists(OEnumTipoMovimientoPTZ tipo)
        {
            return base.Exists(delegate(OPosicionPTZ posicionAux) { return (posicionAux.Tipo == tipo); });
        }

        /// <summary>
        /// Busca el tipo de movimiento en la lista de comandos
        /// </summary>
        /// <param name="movimiento">Tipo de movimiento a buscar</param>
        /// <returns>Información del movimiento</returns>
        public OPosicionPTZ Find(OEnumTipoMovimientoPTZ tipo)
        {
            return base.Find(delegate(OPosicionPTZ posicionAux) { return (posicionAux.Tipo == tipo); });
        }

        /// <summary>
        /// Lista los tipos de movimientos PTZs del comando
        /// </summary>
        /// <returns>Lista los tipos de movimientos PTZs del comando</returns>
        public List<OEnumTipoMovimientoPTZ> GetListaTiposMovimientosPTZ()
        {
            List<OEnumTipoMovimientoPTZ> resultado = new List<OEnumTipoMovimientoPTZ>();

            foreach (OPosicionPTZ posicion in this)
            {
                resultado.Add(posicion.Tipo);
            }

            return resultado;
        }
        #endregion
    }

    /// <summary>
    /// Listado de parámetros PTZ
    /// </summary>
    public class OMovimientosPTZ : List<OMovimientoPTZ>
    {
        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OMovimientosPTZ()
        {
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OMovimientosPTZ(params OMovimientoPTZ[] valores)
        {
            this.AddRange(valores);
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Añade un movimiento
        /// </summary>
        /// <param name="tipo">Tipo de movimiento</param>
        /// <param name="modo">Modo de movimiento</param>
        public void Add(OEnumTipoMovimientoPTZ tipo, OEnumModoMovimientoPTZ modo)
        {
            OMovimientoPTZ movimiento = new OMovimientoPTZ(tipo, modo);
            this.Add(movimiento);
        }

        /// <summary>
        /// Añade un movimiento
        /// </summary>
        /// <param name="tipo">Tipo de movimiento</param>
        /// <param name="modo">Modo de movimiento</param>
        public new void Add(OMovimientoPTZ movimiento)
        {
            if (!this.Exists(movimiento))
            {
                base.Add(movimiento);
            }
        }

        /// <summary>
        /// Indica que el tipo de movimiento ya existe en la lista de comandos
        /// </summary>
        /// <param name="movimiento">Tipo de movimiento a buscar</param>
        /// <returns>Verdadero si el tipo de movimiento ya existe en la lista de comandos</returns>
        public bool Exists(OMovimientoPTZ movimiento)
        {
            return base.Exists(delegate(OMovimientoPTZ mov) { return (mov.Tipo == movimiento.Tipo) && (mov.Modo == movimiento.Modo); });
        }

        /// <summary>
        /// Busca el tipo de movimiento en la lista de comandos
        /// </summary>
        /// <param name="movimiento">Tipo de movimiento a buscar</param>
        /// <returns>Información del movimiento</returns>
        public OMovimientoPTZ Find(OMovimientoPTZ movimiento)
        {
            return base.Find(delegate(OMovimientoPTZ mov) { return (mov.Tipo == movimiento.Tipo) && (mov.Modo == movimiento.Modo); });
        }
        #endregion
    }

    /// <summary>
    /// Listado de parámetros PTZ
    /// </summary>
    public class OTiposMovimientosPTZ : List<OEnumTipoMovimientoPTZ>
    {
        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OTiposMovimientosPTZ()
        {
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OTiposMovimientosPTZ(params OEnumTipoMovimientoPTZ[] valores)
        {
            this.AddRange(valores);
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Añade un movimiento
        /// </summary>
        /// <param name="tipo">Tipo de movimiento</param>
        /// <param name="modo">Modo de movimiento</param>
        public new void Add(OEnumTipoMovimientoPTZ tipo)
        {
            if (!this.Exists(tipo))
	        {
                base.Add(tipo);
	        }
        }

        /// <summary>
        /// Indica que el tipo de movimiento ya existe en la lista de comandos
        /// </summary>
        /// <param name="movimiento">Tipo de movimiento a buscar</param>
        /// <returns>Verdadero si el tipo de movimiento ya existe en la lista de comandos</returns>
        public bool Exists(OEnumTipoMovimientoPTZ tipo)
        {
            return base.Exists(delegate(OEnumTipoMovimientoPTZ tipoAux) { return (tipoAux == tipo); });
        }
        #endregion
    }

    /// <summary>
    /// Estructura que contiene un comando de movimiento PTZ
    /// </summary>
    public struct OComandoPTZ
    {
        #region Atributo(s)
        /// <summary>
        /// Movimiento PTZ
        /// </summary>
        public OMovimientoPTZ Movimiento;
        
        /// <summary>
        /// Valor del movimiento PTZ
        /// </summary>
        public double Valor;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OComandoPTZ(OMovimientoPTZ movimiento, double valor)
        {
            this.Movimiento = movimiento;
            this.Valor = valor;
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OComandoPTZ(OEnumTipoMovimientoPTZ tipo, OEnumModoMovimientoPTZ modo, double valor)
        {
            this.Movimiento = new OMovimientoPTZ(tipo, modo);
            this.Valor = valor;
        }
        #endregion
    }

    /// <summary>
    /// Estructura que contiene una posicion PTZ
    /// </summary>
    public struct OPosicionPTZ
    {
        #region Atributo(s)
        /// <summary>
        /// Tipo de movimiento PTZ
        /// </summary>
        public OEnumTipoMovimientoPTZ Tipo;
        /// <summary>
        /// Valor del movimiento PTZ
        /// </summary>
        public double Valor;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OPosicionPTZ(OEnumTipoMovimientoPTZ tipo, double valor)
        {
            this.Tipo = tipo;
            this.Valor = valor;
        }
        #endregion
    }

    /// <summary>
    /// Estructura que contiene un movimiento PTZ
    /// </summary>
    public struct OMovimientoPTZ
    {
        #region Atributo(s)
        /// <summary>
        /// Tipo de movimiento PTZ
        /// </summary>
        public OEnumTipoMovimientoPTZ Tipo;
        /// <summary>
        /// Modo de movimiento PTZ
        /// </summary>
        public OEnumModoMovimientoPTZ Modo;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OMovimientoPTZ(OEnumTipoMovimientoPTZ tipo, OEnumModoMovimientoPTZ modo)
        {
            this.Tipo = tipo;
            this.Modo = modo;
        }
        #endregion
    }

    /// <summary>
    /// Enumerado de todos los movimientos del ptz
    /// </summary>
    public enum OEnumTipoMovimientoPTZ
    {
        /// <summary>
        /// Movimiento de giro
        /// </summary>
        Pan = 1,
        /// <summary>
        /// Movimiento de cabeceo
        /// </summary>
        Tilt = 2,
        /// <summary>
        /// Zoom
        /// </summary>
        Zoom = 4,
        /// <summary>
        /// Control de iris
        /// </summary>
        Iris = 8,
        /// <summary>
        /// Control del enfoque
        /// </summary>
        Focus = 16
    }

    /// <summary>
    /// Indica si el valor a establecer en el movimiento es aboluto o relativo
    /// </summary>
    public enum OEnumModoMovimientoPTZ
    {
        /// <summary>
        /// Movimiento absoluto
        /// </summary>
        Absoluto = 0,
        /// <summary>
        /// Movimiento relativo
        /// </summary>
        Relativo = 1
    }
}
