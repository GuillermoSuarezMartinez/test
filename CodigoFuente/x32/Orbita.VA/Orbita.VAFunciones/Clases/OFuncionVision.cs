//***********************************************************************
// Assembly         : Orbita.VAFunciones
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
using Orbita.VAComun;
using Orbita.VAControl;

namespace Orbita.VAFunciones
{
    /// <summary>
    /// Clase de acceso estático que contiene la lista de funciones de visión
    /// </summary>
    public static class OFuncionesVisionManager
    {
        #region Atributo(s)
        /// <summary>
        /// Lista de funciones de visión funcionando en el sistema
        /// </summary>
        private static List<OFuncionVisionBase> ListaFuncionesVision;

        #endregion

        #region Método(s) público(s)

        /// <summary>
        /// Construye los objetos
        /// </summary>
        public static void Constructor()
        {
            // Construyo la lista de funciones de visión
            ListaFuncionesVision = new List<OFuncionVisionBase>();

            // Consulta a la base de datos
            DataTable dt = AppBD.GetFuncionesVision();
            if (dt.Rows.Count > 0)
            {
                // Cargamos todas las funciones de visión existentes en el sistema
                foreach (DataRow dr in dt.Rows)
                {
                    string codFuncionVision = dr["CodFuncionVision"].ToString();
                    string ensambladoClaseImplementadora = dr["EnsambladoClaseImplementadora"].ToString();
                    string claseImplementadora = string.Format("{0}.{1}", ensambladoClaseImplementadora, dr["ClaseImplementadora"].ToString());

                    object objetoImplementado;
                    if (App.ConstruirClase(ensambladoClaseImplementadora, claseImplementadora, out objetoImplementado, codFuncionVision))
                    {
                        OFuncionVisionBase funcionVision = (OFuncionVisionBase)objetoImplementado;
                        ListaFuncionesVision.Add(funcionVision);
                    }
                }
            }
        }

        /// <summary>
        /// Destruye los objetos
        /// </summary>
        public static void Destructor()
        {
            // Destruyo la lista de máquinas de estados
            ListaFuncionesVision = null;

            // Se elimina la demanda del uso de LPR
            LPRManager.FinDemandaUso();

            // Se elimina la demanda del uso de OCR
            CCRManager.FinDemandaUso();
        }

        /// <summary>
        /// Carga las propiedades de la base de datos
        /// </summary>
        public static void Inicializar()
        {
            foreach (OFuncionVisionBase visionFunction in ListaFuncionesVision)
            {
                visionFunction.Inicializar();
            }
        }

        /// <summary>
        /// Finaliza la ejecución
        /// </summary>
        public static void Finalizar()
        {
            foreach (OFuncionVisionBase visionFunction in ListaFuncionesVision)
            {
                visionFunction.Finalizar();
            }
        }

        /// <summary>
        /// Inicia la ejecución de determinada función de visión.
        /// Se debe ejecutar a esta función desde el hilo principal de la aplicación, de lo contrario ocurrirán excepciones.
        /// </summary>
        public static void IniciarEjecucion(string codFuncionVision)
        {
            foreach (OFuncionVisionBase visionFunction in ListaFuncionesVision)
            {
                if (visionFunction.Codigo == codFuncionVision)
                {
                    visionFunction.IniciarEjecucion();
                }
            }
        }

        /// <summary>
        /// Inicia la ejecución de determinada función de visión de forma síncrona
        /// Se debe ejecutar a esta función desde el hilo principal de la aplicación, de lo contrario ocurrirán excepciones.
        /// </summary>
        public static void IniciarEjecucionSincrona(string codFuncionVision)
        {
            foreach (OFuncionVisionBase visionFunction in ListaFuncionesVision)
            {
                if (visionFunction.Codigo == codFuncionVision)
                {
                    visionFunction.IniciarEjecucionSincrona();
                }
            }
        }

        /// <summary>
        /// Inicia la ejecución de determinada función de visión de forma asíncrona
        /// Se debe ejecutar a esta función desde el hilo principal de la aplicación, de lo contrario ocurrirán excepciones.
        /// </summary>
        /// 
        public static void IniciarEjecucionAsincrona(string codFuncionVision)
        {
            foreach (OFuncionVisionBase visionFunction in ListaFuncionesVision)
            {
                if (visionFunction.Codigo == codFuncionVision)
                {
                    visionFunction.IniciarEjecucionAsincrona();
                }
            }
        }

        /// <summary>
        /// Función para la actualización de parámetros de entrada
        /// </summary>
        /// <param name="codFuncionVision">Código identificador de la función de vision</param>
        /// <param name="codPieza">Código identificador de la pieza</param>
        /// <param name="codParametro">Código identificador del parámetro</param>
        /// <param name="valor">Nuevo valor del parámetro</param>
        /// <param name="tipoVariable">Tipo del parámetro</param>
        /// <returns>Verdadero si la función se ha ejecutado correctamente</returns>
        public static bool SetEntrada(string codFuncionVision, string codParametro, object valor, OEnumTipoDato tipoVariable)
        {
            foreach (OFuncionVisionBase visionFunction in ListaFuncionesVision)
            {
                if (visionFunction.Codigo == codFuncionVision)
                {
                    return visionFunction.SetEntrada(codParametro, valor, tipoVariable);
                }
            }
            return false;
        }

        /// <summary>
        /// Función para la consulta de parámetros de salida
        /// </summary>
        /// <param name="codFuncionVision">Código identificador de la función de vision</param>
        /// <param name="codPieza">Código identificador de la pieza</param>
        /// <param name="codParametro">Código identificador del parámetro</param>
        /// <param name="valor">Valor del parámetro devuelto por la función</param>
        /// <param name="tipoVariable">Tipo del parámetro</param>
        /// <returns>Verdadero si la función se ha ejecutado correctamente</returns>
        public static bool GetSalida(string codFuncionVision, string codParametro, out object valor, OEnumTipoDato tipoVariable)
        {
            valor = null;
            foreach (OFuncionVisionBase visionFunction in ListaFuncionesVision)
            {
                if (visionFunction.Codigo == codFuncionVision)
                {
                    return visionFunction.GetSalida(codParametro, out valor, tipoVariable);
                }
            }
            return false;
        }

        /// <summary>
        /// Función para la consulta de parámetros de salida
        /// </summary>
        /// <param name="listaSalidas">Lista de objetos que respresentan el conjunto de salidas de la función de visión</param>
        /// <returns></returns>
        public static bool GetSalidas(string codFuncionVision, out Dictionary<string, object> listaSalidas)
        {
            listaSalidas = null;
            foreach (OFuncionVisionBase visionFunction in ListaFuncionesVision)
            {
                if (visionFunction.Codigo == codFuncionVision)
                {
                    return visionFunction.GetSalidas(out listaSalidas);
                }
            }
            return false;
        }

        /// <summary>
        /// Obtiene la función de visión mediante las claves
        /// </summary>
        /// <returns></returns>
        public static OFuncionVisionBase GetFuncionVision(OClaves claves)
        {
            OFuncionVisionBase resultado = null;

            foreach (OFuncionVisionBase funcionVision in ListaFuncionesVision)
            {
                if (funcionVision.Claves.CompararClaves(claves))
                {
                    resultado = funcionVision;
                    break;
                }
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene el código de la función de visión mediante las claves
        /// </summary>
        /// <returns></returns>
        public static string GetCodigoFuncionVision(OClaves claves)
        {
            string resultado = string.Empty;

            foreach (OFuncionVisionBase funcionVision in ListaFuncionesVision)
            {
                if (funcionVision.Claves.CompararClaves(claves))
                {
                    resultado = funcionVision.Codigo;
                    break;
                }
            }

            return resultado;
        }
        #endregion
    }

    /// <summary>
    /// Clase base para todas las funciones de visión
    /// </summary>
    public class OFuncionVisionBase
    {
        #region Atributo(s)
        /// <summary>
        /// Lista de entradas de la funcion
        /// </summary>
        public List<OParametroFuncionVision> ListaParametrosEntrada;

        /// <summary>
        /// Lista de salidas de la función
        /// </summary>
        public List<OParametroFuncionVision> ListaParametrosSalida;

        /// <summary>
        /// Informa del estado de la ejecución
        /// </summary>
        public bool EjecucionTerminada;

        /// <summary>
        /// Claves de la función de visión
        /// </summary>
        public OClaves Claves;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Id de la ejecucion Actual
        /// </summary>
        private long _IdEjecucionActual;
        /// <summary>
        /// d de la ejecucion Actual
        /// </summary>
        public long IdEjecucionActual
        {
            get { return _IdEjecucionActual; }
            set { _IdEjecucionActual = value; }
        }

        /// <summary>
        /// Ruta del fichero a cargar
        /// </summary>
        private string _RutaFichero;
        /// <summary>
        /// Ruta del fichero a cargar
        /// </summary>
        public string RutaFichero
        {
            get { return _RutaFichero; }
            set { _RutaFichero = value; }
        }

        /// <summary>
        /// Código identificativo de la función
        /// </summary>
        private string _Codigo;
        /// <summary>
        /// Código identificativo de la función
        /// </summary>
        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }

        /// <summary>
        /// Nombre identificativo de la función
        /// </summary>
        private string _Nombre;
        /// <summary>
        /// Nombre identificativo de la función
        /// </summary>
        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        /// <summary>
        /// Descripción de la función
        /// </summary>
        private string _Descripcion;
        /// <summary>
        /// Descripción de la función
        /// </summary>
        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }

        /// <summary>
        /// Indica que la función ha sido cargada correctamente
        /// </summary>
        private bool _Valido;
        /// <summary>
        /// Indica que la función ha sido cargada correctamente
        /// </summary>
        public bool Valido
        {
            get { return _Valido; }
            set { _Valido = value; }
        }

        /// <summary>
        /// Tipo de función de visión
        /// </summary>
        private OTipoFuncionVision _TipoFuncionVision;
        /// <summary>
        /// Tipo de función de visión
        /// </summary>
        public OTipoFuncionVision TipoFuncionVision
        {
            get { return _TipoFuncionVision; }
            set { _TipoFuncionVision = value; }
        }

        /// <summary>
        /// Indica si la ejecución se realiza por defecto en el mismo hilo de ejecución que la
        /// applicación principal o en un hijo de ejecución distinto
        /// </summary>
        private OTipoEjecucionFuncionVision _TipoEjecucionFuncionVision;
        /// <summary>
        /// Indica si la ejecución se realiza por defecto en el mismo hilo de ejecución que la
        /// applicación principal o en un hijo de ejecución distinto
        /// </summary>
        public OTipoEjecucionFuncionVision TipoEjecucionFuncionVision
        {
            get { return _TipoEjecucionFuncionVision; }
            set { _TipoEjecucionFuncionVision = value; }
        }

        /// <summary>
        /// Código identificativo de la variable que se activa
        /// </summary>
        private string _CodVariableEnEjecucion;
        /// <summary>
        /// Código identificativo de la función
        /// </summary>
        public string CodVariableEnEjecucion
        {
            get { return _CodVariableEnEjecucion; }
            set { _CodVariableEnEjecucion = value; }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public OFuncionVisionBase(string codFuncionVision)
        {
            // Inicialiamos los valores
            this._Codigo = codFuncionVision;
            this.ListaParametrosEntrada = new List<OParametroFuncionVision>();
            this.ListaParametrosSalida = new List<OParametroFuncionVision>();
            this._Valido = false;
            this._IdEjecucionActual = 0;

            // Cargamos valores de la base de datos
            DataTable dtFuncionVision = AppBD.GetFuncionVision(this._Codigo);
            if (dtFuncionVision.Rows.Count == 1)
            {
                this._Nombre = dtFuncionVision.Rows[0]["NombreFuncionVision"].ToString();
                this._Descripcion = dtFuncionVision.Rows[0]["DescFuncionVision"].ToString();
                this._RutaFichero = dtFuncionVision.Rows[0]["RutaFichero"].ToString();

                int intTipoFuncion = App.EvaluaNumero(dtFuncionVision.Rows[0]["IdTipoFuncionVision"], 1, 3, 1);
                this._TipoFuncionVision = (OTipoFuncionVision)intTipoFuncion;

                this._TipoEjecucionFuncionVision = (OTipoEjecucionFuncionVision)App.EnumParse(typeof(OTipoEjecucionFuncionVision), dtFuncionVision.Rows[0]["TipoEjecucion"].ToString(), OTipoEjecucionFuncionVision.EjecucionSincrona);

                this._CodVariableEnEjecucion = dtFuncionVision.Rows[0]["CodVariable"].ToString();

                // Consulta las claves de una determinada vista
                this.Claves = new OClaves();
                DataTable dt = AppBD.GetClavesDeFuncionVision(this.Codigo);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Claves.Add(new OClave(dr));
                    }
                }

                // Cargamos el código de las variables de entrada que se utilizan en la funcion
                DataTable dtParametrosEntrada = AppBD.GetParametrosFuncionesVision(this._Codigo, true);
                if (dtParametrosEntrada.Rows.Count > 0)
                {
                    object codParametroEntradaAux;
                    foreach (DataRow dr in dtParametrosEntrada.Rows)
                    {
                        codParametroEntradaAux = dr["CodParametroFuncionVision"];
                        if ((codParametroEntradaAux is string) && ((string)codParametroEntradaAux != string.Empty))
                        {
                            OParametroFuncionVision parametro = new OParametroFuncionVision(this._Codigo, (string)codParametroEntradaAux);
                            this.ListaParametrosEntrada.Add(parametro);
                        }
                    }
                }

                // Cargamos el código de las variables de salida que se utilizan en la funcion
                DataTable dtParametrosSalida = AppBD.GetParametrosFuncionesVision(this._Codigo, false);
                if (dtParametrosSalida.Rows.Count > 0)
                {
                    object codParametroSalidaAux;
                    foreach (DataRow dr in dtParametrosSalida.Rows)
                    {
                        codParametroSalidaAux = dr["CodParametroFuncionVision"];
                        if ((codParametroSalidaAux is string) && ((string)codParametroSalidaAux != string.Empty))
                        {
                            OParametroFuncionVision parametro = new OParametroFuncionVision(this._Codigo, (string)codParametroSalidaAux);
                            this.ListaParametrosSalida.Add(parametro);
                        }
                    }
                }

                // Creación del cronómetro
                OCronometrosManager.NuevoCronometro(this._Codigo, "Ejecución función " + this.Nombre, "Ejecución de la función de visión " + this.Nombre);
            }
        }

        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Llamada a la ejecución de la función
        /// </summary>
        public virtual void IniciarEjecucion()
        {
            switch (this._TipoEjecucionFuncionVision)
            {
                case OTipoEjecucionFuncionVision.EjecucionSincrona:
                default:
                    this.IniciarEjecucionSincrona();
                    break;
                case OTipoEjecucionFuncionVision.EjecucionAsincrona:
                    this.IniciarEjecucionAsincrona();
                    break;
            }
        }

        /// <summary>
        /// Llamada a la ejecución de la función de forma síncrona
        /// </summary>
        internal void IniciarEjecucionSincrona()
        {
            if (this._Valido)
            {
                // Cronometramos la duración de la ejecución
                OCronometrosManager.Start(this._Codigo);

                // Guardamos la traza
                OVALogsManager.Info(OModulosFunciones.Vision, this._Codigo, "Inicio de la ejecución síncrona de la función " + this._Codigo);

                this.EjecucionTerminada = false;

                // Llamada a la variable que indica el estado de ejecución de la función
                OVariablesManager.SetValue(this._CodVariableEnEjecucion, true, "Vision", this.Codigo);

                this.SetEntradasVariables();
                this.EjecucionSincrona();
            }
        }

        /// <summary>
        /// Llamada a la ejecución de la función de forma asíncrona
        /// </summary>
        internal void IniciarEjecucionAsincrona()
        {
            if (this._Valido)
            {
                // Cronometramos la duración de la ejecución
                OCronometrosManager.Start(this._Codigo);

                // Guardamos la traza
                OVALogsManager.Info(OModulosFunciones.Vision, this._Codigo, "Inicio de la ejecución asíncrona de la función " + this._Codigo);

                this.EjecucionTerminada = false;

                // Llamada a la variable que indica el estado de ejecución de la función
                OVariablesManager.SetValue(this._CodVariableEnEjecucion, true, "Vision", this.Codigo);

                this.SetEntradasVariables();
                this.EjecucionAsincrona();
            }
        }

        /// <summary>
        /// Función para la consulta de parámetros de salida
        /// </summary>
        /// <param name="listaSalidas">Lista de objetos que respresentan el conjunto de salidas de la función de visión</param>
        /// <returns></returns>
        public bool GetSalidas(out Dictionary<string, object> listaSalidas)
        {
            listaSalidas = new Dictionary<string, object>();

            foreach (OParametroFuncionVision parametro in this.ListaParametrosSalida)
            {
                switch (parametro.OrigenValorParametro)
                {
                    case OOrigenValorParametro.ValorPorCodigo:
                        object valorSalida;

                        if (this.GetSalida(parametro.Codigo, out valorSalida, parametro.Tipo))
                        {
                            listaSalidas.Add(parametro.Codigo, valorSalida);
                        }
                        break;
                }
            }

            return listaSalidas.Count > 0;
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Método donde se cargan las entradas a la función de visión
        /// </summary>
        protected void SetEntradasVariables()
        {
            object valorEntrada;
            foreach (OParametroFuncionVision parametro in this.ListaParametrosEntrada)
            {
                switch (parametro.OrigenValorParametro)
                {
                    case OOrigenValorParametro.ValorVariable:
                        valorEntrada = OVariablesManager.GetValue(parametro.CodVariable);
                        this.SetEntrada(parametro.Codigo, valorEntrada, parametro.Tipo);
                        break;
                }
            }
        }

        /// <summary>
        /// Método donde se cargan las entradas a la función de visión
        /// </summary>
        protected void SetEntradasFijas()
        {
            object valorEntrada;
            foreach (OParametroFuncionVision parametro in this.ListaParametrosEntrada)
            {
                switch (parametro.OrigenValorParametro)
                {
                    case OOrigenValorParametro.ValorFijo:
                        valorEntrada = parametro.Valor;
                        this.SetEntrada(parametro.Codigo, valorEntrada, parametro.Tipo);
                        break;
                }
            }
        }

        /// <summary>
        /// Método donde se leen las salidas de la función de visión
        /// </summary>
        protected void GetSalidasVariables()
        {
            object valorSalida = null;
            foreach (OParametroFuncionVision parametro in this.ListaParametrosSalida)
            {
                switch (parametro.OrigenValorParametro)
                {
                    case OOrigenValorParametro.ValorVariable:
                        if (this.GetSalida(parametro.Codigo, out valorSalida, parametro.Tipo))
                        {
                            OVariablesManager.SetValue(parametro.CodVariable, valorSalida, "VisionFunction", this.Codigo);
                        }
                        break;
                }
            }
        }
        #endregion

        #region Método(s) virtual(es)

        /// <summary>
        /// Método a heredar donde se carga el fichero de la función de visión
        /// </summary>
        public virtual void Inicializar()
        {
            if (this.Valido)
            {
                this.SetEntradasFijas();
            }
        }

        /// <summary>
        /// Método a heredar donde se descarga el fichero de la función de visión
        /// </summary>
        public virtual void Finalizar()
        {
            this._Valido = false;
        }

        /// <summary>
        /// Metodo virtual donde se ejecuta la función de visión de forma síncrona
        /// </summary>
        /// <returns></returns>
        protected virtual bool EjecucionSincrona()
        {
            return false;
        }

        /// <summary>
        /// Metodo virtual donde se ejecuta la función de visión de forma asíncrona
        /// </summary>
        /// <returns></returns>
        protected virtual bool EjecucionAsincrona()
        {
            return false;
        }

        /// <summary>
        /// Función para la actualización de parámetros de entrada
        /// </summary>
        /// <param name="ParamName">Nombre identificador del parámetro</param>
        /// <param name="ParamValue">Nuevo valor del parámetro</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual bool SetEntrada(string codigo, object valor, OEnumTipoDato tipoVariable)
        {
            return false;
        }

        /// <summary>
        /// Función para la consulta de parámetros de salida
        /// </summary>
        /// <param name="ParamName">Nombre identificador del parámetro</param>
        /// <param name="ParamValue">Nuevo valor del parámetro</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual bool GetSalida(string codigo, out object valor, OEnumTipoDato tipoVariable)
        {
            valor = null;
            return false;
        }

        #endregion

        #region Eventos
        /// <summary>
        /// Evento que se ejecuta cuando finaliza la ejecución de la función
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evento</param>
        protected void FuncionEjecutada()
        {
            try
            {
                this.GetSalidasVariables();

                // Paramos el cronometro de la duración de la ejecución
                OCronometrosManager.Stop(this._Codigo);

                // Guardamos la traza
                OVALogsManager.Info(OModulosFunciones.Vision, this._Codigo, "Fin de la ejecución de la función " + this._Codigo + ". Duración: " + OCronometrosManager.DuracionUltimaEjecucion(this._Codigo).ToString());

                // Llamada a la variable que indica el estado de ejecución de la función
                OVariablesManager.SetValue(this._CodVariableEnEjecucion, false, "Vision", this.Codigo);

                this.EjecucionTerminada = true;
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosFunciones.Vision, this._Codigo, exception);
            }
        }
        #endregion
    }

    /// <summary>
    /// Clase base para todas las funciones de visión
    /// </summary>
    public class OFuncionVisionEncolada : OFuncionVisionBase
    {
        #region Atributo(s)
        /// <summary>
        /// Cola donde iremos almacenando los resultados parciales
        /// </summary>
        public Queue<object> ResultadosParciales;

        /// <summary>
        /// Contador de imágenes
        /// </summary>
        public int ContInspeccionesEnCola;

        /// <summary>
        /// Indice de la última fotografía añadida a la cola de proceso
        /// </summary>
        public int IndiceFotografia;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Indica que la función ha de dejar de ejecutarse si se encuentra en ejecución continúa
        /// </summary>
        private bool _Abortar;
        /// <summary>
        /// Indica que la función ha de dejar de ejecutarse si se encuentra en ejecución continúa
        /// </summary>
        public bool Abortar
        {
            get { return _Abortar; }
            set { _Abortar = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public OFuncionVisionEncolada(string codFuncionVision)
            : base(codFuncionVision)
        {
            // Inicialiamos los valores
            this._Abortar = false;
            this.ResultadosParciales = new Queue<object>();
            this.OnResultadoParcial = null;
        }

        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Inicializa la configuración del LPR
        /// </summary>
        public override void Inicializar()
        {
            base.Inicializar();

            this.ContInspeccionesEnCola = 0;
            this.IndiceFotografia = 0;
        }

        /// <summary>
        /// Método a heredar donde se descarga el fichero de la función de visión
        /// </summary>
        public override void Finalizar()
        {
            base.Finalizar();
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Llamada a la ejecución de la función
        /// </summary>
        public new void IniciarEjecucion()
        {
            this.IniciarEjecucionEncolada();
        }

        /// <summary>
        /// Llamada a la ejecución de la función de forma encolada
        /// </summary>
        internal void IniciarEjecucionEncolada()
        {
            if (this.Valido)
            {
                // Cronometramos la duración de la ejecución
                OCronometrosManager.Start(this.Codigo);

                // Guardamos la traza
                OVALogsManager.Info(OModulosFunciones.Vision, this.Codigo, "Inicio de la ejecución encolada de la función " + this.Codigo);

                this.EjecucionTerminada = false;

                // Llamada a la variable que indica el estado de ejecución de la función
                OVariablesManager.SetValue(this.CodVariableEnEjecucion, true, "Vision", this.Codigo);

                this.EjecucionEncolada();
            }
        }

        /// <summary>
        /// Llamada para saber si se dispone de resultados parciales
        /// </summary>
        /// <returns></returns>
        public bool HayResultadosParciales()
        {
            return (this.ResultadosParciales.Count > 0);
        }

        /// <summary>
        /// Devuelve un resultado parcial
        /// </summary>
        /// <returns></returns>
        public object ExtraerResultadoParcial()
        {
            object resultado = new object();
            if (this.ResultadosParciales.Count > 0)
            {
                resultado = this.ResultadosParciales.Dequeue();
            }
            return resultado;
        }
        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Indica que hay inspecciones pendientes de ejecución
        /// </summary>
        /// <returns></returns>
        public virtual bool HayInspeccionesPendientes()
        {
            return false;
        }

        /// <summary>
        /// Metodo virtual asociado al delegado donde se recibe un resultado parcial
        /// </summary>
        /// <returns></returns>
        internal virtual void AñadirResultadoParcial(object sender, OEventArgsResultadoParcial e)
        {
            try
            {
                this.ContInspeccionesEnCola--;

                // Evento de llegada de resultado parcial
                if (this.OnResultadoParcial != null)
                {
                    this.OnResultadoParcial(sender, e);
                }

                // Se realizan las acciones de fin de inspección
                this.ResultadosParciales.Enqueue(e.InfoInspeccion);
                if (!this.HayInspeccionesPendientes())
                {
                    this.FuncionEjecutada();
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosFunciones.Vision, this.Codigo, exception);
            }
        }

        /// <summary>
        /// Resetea la cola de ejecución
        /// </summary>
        public virtual void ResetearColaEjecucion()
        {
            this.ResultadosParciales.Clear();
            this.ContInspeccionesEnCola = 0;
            this.IndiceFotografia = 0;
        }

        /// <summary>
        /// Metodo virtual donde se ejecuta la función de visión de forma encolada
        /// </summary>
        /// <returns></returns>
        protected virtual bool EjecucionEncolada()
        {
            return false;
            // Ejecutado en heredados
        }        
        #endregion

        #region Definición de delegado(s)
        /// <summary>
        /// CallBack donde mandar el resultado parcial
        /// </summary>
        public OCallBackResultadoParcial OnResultadoParcial;

        #endregion
    }

    /// <summary>
    /// Clase que representa a un parámetro de entrada o de salida de una función de visión
    /// </summary>
    public class OParametroFuncionVision
    {
        #region Propiedad(es)
        /// <summary>
        /// Código identificativo del parámetro
        /// </summary>
        private string _Codigo;
        /// <summary>
        /// Código identificativo del parámetro
        /// </summary>
        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }

        /// <summary>
        /// Nombre identificativo del parámetro
        /// </summary>
        private string _Nombre;
        /// <summary>
        /// Nombre identificativo del parámetro
        /// </summary>
        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        /// <summary>
        /// Descripción del parámetro
        /// </summary>
        private string _Descripcion;
        /// <summary>
        /// Descripción del parámetro
        /// </summary>
        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }

        /// <summary>
        /// Indica si el parámetro de la función de visión es una entrada o una salida
        /// </summary>
        private bool _EsEntrada;
        /// <summary>
        /// Indica si el parámetro de la función de visión es una entrada o una salida
        /// </summary>
        public bool EsEntrada
        {
            get { return _EsEntrada; }
            set { _EsEntrada = value; }
        }

        /// <summary>
        /// Indica si se trata de un parámetro fijo o es una variable
        /// </summary>
        private OOrigenValorParametro _OrigenValorParametro;
        /// <summary>
        /// Indica si se trata de un parámetro fijo o es una variable
        /// </summary>
        public OOrigenValorParametro OrigenValorParametro
        {
            get { return _OrigenValorParametro; }
            set { _OrigenValorParametro = value; }
        }

        /// <summary>
        /// Tipo del parámetro de la función
        /// </summary>
        private OEnumTipoDato _Tipo;
        /// <summary>
        /// Tipo del parámetro de la función
        /// </summary>
        public OEnumTipoDato Tipo
        {
            get { return _Tipo; }
            set { _Tipo = value; }
        }

        /// <summary>
        /// Propiedad opcional que indica el código de la variable asociada al parámetro
        /// </summary>
        private string _CodVariable;
        /// <summary>
        /// Propiedad opcional que indica el código de la variable asociada al parámetro
        /// </summary>
        public string CodVariable
        {
            get { return _CodVariable; }
            set { _CodVariable = value; }
        }

        /// <summary>
        /// Propiedad opcional que indica el valor fijo del parámetro
        /// </summary>
        private object _Valor;
        /// <summary>
        /// Propiedad opcional que indica el valor fijo del parámetro
        /// </summary>
        public object Valor
        {
            get { return _Valor; }
            set { _Valor = value; }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OParametroFuncionVision(string codFuncionVision, string codParametroFuncionVision)
        {
            // Inicialiamos los valores
            this._Codigo = codParametroFuncionVision;

            // Cargamos valores de la base de datos
            DataTable dtParametro = AppBD.GetParametroFuncionVision(codFuncionVision, this._Codigo);
            if (dtParametro.Rows.Count == 1)
            {
                this._Nombre = dtParametro.Rows[0]["NombreParametroFuncionVision"].ToString();
                this._Descripcion = dtParametro.Rows[0]["DescParametroFuncionVision"].ToString();
                this._CodVariable = dtParametro.Rows[0]["CodVariable"].ToString();
                this._EsEntrada = (bool)dtParametro.Rows[0]["EsEntrada"];
                this._OrigenValorParametro = (OOrigenValorParametro)App.EnumParse(typeof(OOrigenValorParametro), dtParametro.Rows[0]["OrigenValorParametro"].ToString(), OOrigenValorParametro.ValorPorCodigo);
                this._Tipo = (OEnumTipoDato)App.EvaluaNumero(dtParametro.Rows[0]["IdTipoParametroFuncionVision"], 0, 99, 0);

                if (this._OrigenValorParametro == OOrigenValorParametro.ValorFijo)
                {
                    switch (this._Tipo)
                    {
                        case OEnumTipoDato.Bit:
                            this._Valor = App.EvaluaBooleano(dtParametro.Rows[0]["ValorBit"], false);
                            break;
                        case OEnumTipoDato.Entero:
                            this._Valor = App.EvaluaNumero(dtParametro.Rows[0]["ValorEntero"], int.MinValue, int.MaxValue, 0);
                            break;
                        case OEnumTipoDato.Texto:
                            this._Valor = dtParametro.Rows[0]["ValorTexto"].ToString();
                            break;
                        case OEnumTipoDato.Decimal:
                            this._Valor = App.EvaluaNumero(dtParametro.Rows[0]["ValorDecimal"], double.MinValue, double.MaxValue, 0);
                            break;
                    }
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// Enumerado que identifica a los tipos de funciones de visión
    /// </summary>
    public enum OTipoFuncionVision
    {
        /// <summary>
        /// Función propia
        /// </summary>
        Orbita = 1,
        /// <summary>
        /// Función de las librerías VisionPro de Cognex
        /// </summary>
        VisionPro = 2,
        /// <summary>
        /// Función de las librerías de reconocimiento de la matricula de los contenedores
        /// </summary>
        OCRContainer = 3,
        /// <summary>
        /// Función de las librerías de lectura de matrículas de vehículos
        /// </summary>
        LPR = 4
    }

    /// <summary>
    /// Indica el tipo de ejecución que se realizará en la función de visión
    /// </summary>
    public enum OTipoEjecucionFuncionVision
    {
        /// <summary>
        /// Ejecución sincrona de la función de visión
        /// </summary>
        EjecucionSincrona = 1,
        /// <summary>
        /// Ejecución de la función de visión en un hilo separado
        /// </summary>
        EjecucionAsincrona = 2,
        /// <summary>
        /// Ejecución multiple de la función de visión
        /// </summary>
        EjecucionEncolada = 3
    }

    /// <summary>
    /// Enumerado que informa sobre el origen del valor de un parámetro
    /// </summary>
    public enum OOrigenValorParametro
    {
        /// <summary>
        /// Valor fijado en la base de datos (únicamente válido para las entradas)
        /// </summary>
        ValorFijo = 1,
        /// <summary>
        /// Valor asignado por/a variable (válido tanto para entradas com para salidas)
        /// </summary>
        ValorVariable = 2,
        /// <summary>
        /// Valor asignado por/a código (válido tanto para entradas com para salidas)
        /// </summary>
        ValorPorCodigo = 3
    }

    /// <summary>
    /// Clase que contiene la información referente a la inspección
    /// </summary>
    /// <typeparam name="TInfo"></typeparam>
    /// <typeparam name="TParametros"></typeparam>
    /// <typeparam name="TResultados"></typeparam>
    public class OInfoInspeccion<TImagen, TInfo, TParametros, TResultados>
        where TImagen: OImage, new()
        where TInfo : new()
        where TParametros : new()
        where TResultados : new()
    {
        #region Atributo(s)
        /// <summary>
        /// Imagen inspeccionada
        /// </summary>
        public TImagen Imagen;
        /// <summary>
        /// Información sobre la inspección
        /// </summary>
        public TInfo Info;
        /// <summary>
        /// Parametros de entarda de la inspección
        /// </summary>
        public TParametros Parametros;
        /// <summary>
        /// Resultados de la inspección
        /// </summary>
        public TResultados Resultados;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OInfoInspeccion()
        {
            this.Imagen = new TImagen();
            this.Info = new TInfo();
            this.Parametros = new TParametros();
            this.Resultados = new TResultados();
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OInfoInspeccion(TImagen imagen, TInfo info, TParametros parametros, TResultados resultados)
        {
            this.Imagen = imagen;
            this.Info = info;
            this.Parametros = parametros;
            this.Resultados = resultados;
        }
        #endregion
    }

    #region Definición de delegado(s)
    /// <summary>
    /// Parametros de retorno del evento que indica de la llegada de un mensaje actualizacion
    /// </summary>
    public class OEventArgsResultadoParcial : EventArgs
    {
        #region Atributo(s)
        /// <summary>
        /// Resultado parcial
        /// </summary>
        public object InfoInspeccion;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OEventArgsResultadoParcial()
        {
            this.InfoInspeccion = new object();
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OEventArgsResultadoParcial(object infoInspeccion)
        {
            this.InfoInspeccion = infoInspeccion;
        }
        #endregion
    }

    /// <summary>
    /// Delegado que indica de la llegada de un nuevo resultado parcial
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void OCallBackResultadoParcial(object sender, OEventArgsResultadoParcial e);
    #endregion
}
