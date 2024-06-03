﻿//***********************************************************************
// Assembly         : Orbita.VAControl
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
using System.ComponentModel;
using System.Data;
using Orbita.VAComun;

namespace Orbita.VAControl
{
    /// <summary>
    /// Clase de acceso estático que contiene la lista de máquina de estados
    /// </summary>
    public static class StateMachineRuntime
    {
        #region Atributo(s)
        /// <summary>
        /// Lista de máquinas de estado funcionando en el sistema
        /// </summary>
        public static List<MaquinaEstadosBase> ListaMaquinasEstados;
        #endregion

        #region Método(s) público(s)

        /// <summary>
        /// Construye los objetos
        /// </summary>
        public static void Constructor()
        {
            // Construyo la lista de máquinas de estados
            ListaMaquinasEstados = new List<MaquinaEstadosBase>();

            DataTable dt = AppBD.GetMaquinasEstados();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string codMaquinaEstados = dr["CodMaquinaEstados"].ToString();
                    string ensambladoClaseImplementadora = dr["EnsambladoClaseImplementadora"].ToString();
                    string claseImplementadora = string.Format("{0}.{1}", ensambladoClaseImplementadora, dr["ClaseImplementadora"].ToString());

                    object objetoImplementado;
                    if (App.ConstruirClase(ensambladoClaseImplementadora, claseImplementadora, out objetoImplementado, codMaquinaEstados))
                    {
                        MaquinaEstadosBase maquinaEstados = (MaquinaEstadosBase)objetoImplementado;
                        ListaMaquinasEstados.Add(maquinaEstados);
                    }
                }
            }
        }

        /// <summary>
        /// Construye los objetos
        /// </summary>
        public static void Destructor()
        {
            // Construyo la lista de máquinas de estados
            ListaMaquinasEstados = null;
        }

        /// <summary>
        /// Carga las propiedades de la base de datos
        /// </summary>
        public static void Inicializar()
        {
            foreach (MaquinaEstadosBase maquinaEstados in ListaMaquinasEstados)
            {
                maquinaEstados.Inicializar();
            }
        }

        /// <summary>
        /// Espera a la finalización de las máquinas de estados
        /// </summary>
        public static void Finalizar()
        {
            foreach (MaquinaEstadosBase maquinaEstados in ListaMaquinasEstados)
            {
                maquinaEstados.Finalizar();
            }
        }

        /// <summary>
        /// Inicia la ejecución de todas las máquinas de estados
        /// </summary>
        public static void IniciarEjecucion()
        {
            foreach (MaquinaEstadosBase maquinasEstado in ListaMaquinasEstados)
            {
                maquinasEstado.IniciarEjecucion();
            }
        }

        /// <summary>
        /// Finaliza la ejecución de todas las máquinas de estados
        /// </summary>
        public static void PararEjecucion()
        {
            foreach (MaquinaEstadosBase maquinasEstado in ListaMaquinasEstados)
            {
                maquinasEstado.PararEjecucion();
            }
        }

        /// <summary>
        /// Creamos la suscripción para los mensajes de la máquina de estados
        /// </summary>
        /// <param name="codigo">Código de la máquina de estados</param>
        /// <param name="funcionProcesadoMensajes">Función que manejara el evento</param>
        public static void CrearSuscripcionMensajes(string codigo, MensajeMaquinaEstadosLanzado funcionProcesadoMensajes)
        {
            foreach (MaquinaEstadosBase maquinaEstados in ListaMaquinasEstados)
            {
                if (maquinaEstados.Codigo == codigo)
                {
                    maquinaEstados.OnMensajeMaquinaEstados += funcionProcesadoMensajes;
                    break;
                }
            }
        }

        /// <summary>
        /// Eliminamos la suscripción para los mensajes de la máquina de estados
        /// </summary>
        /// <param name="codigo">Código de la máquina de estados</param>
        /// <param name="funcionProcesadoMensajes">Función que manejara el evento</param>
        public static void EliminarSuscripcionMensajes(string codigo, MensajeMaquinaEstadosLanzado funcionProcesadoMensajes)
        {
            foreach (MaquinaEstadosBase maquinaEstados in ListaMaquinasEstados)
            {
                if (maquinaEstados.Codigo == codigo)
                {
                    maquinaEstados.OnMensajeMaquinaEstados -= funcionProcesadoMensajes;
                    break;
                }
            }
        }

        /// <summary>
        /// Creamos la suscripción para recibir un evento cuando se produzca un cambio de estados
        /// </summary>
        /// <param name="codigo">Código de la máquina de estados</param>
        /// <param name="funcionProcesadoMensajes">Función que manejara el evento</param>
        public static void CrearSuscripcionCambioEstado(string codigo, EstadoCambiado funcionEstadoCambiado)
        {
            foreach (MaquinaEstadosBase maquinaEstados in ListaMaquinasEstados)
            {
                if (maquinaEstados.Codigo == codigo)
                {
                    maquinaEstados.OnEstadoCambiado += funcionEstadoCambiado;
                    break;
                }
            }
        }

        /// <summary>
        /// Eliminamos la suscripción para recibir un evento cuando se produzca un cambio de estados
        /// </summary>
        /// <param name="codigo">Código de la máquina de estados</param>
        /// <param name="funcionProcesadoMensajes">Función que manejara el evento</param>
        public static void EliminarSuscripcionCambioEstado(string codigo, EstadoCambiado funcionEstadoCambiado)
        {
            foreach (MaquinaEstadosBase maquinaEstados in ListaMaquinasEstados)
            {
                if (maquinaEstados.Codigo == codigo)
                {
                    maquinaEstados.OnEstadoCambiado -= funcionEstadoCambiado;
                    break;
                }
            }
        }

        /// <summary>
        /// Devuelve el código del estado actual de la máquina de estados
        /// </summary>
        /// <param name="codigo">Código de la máquina de estados</param>
        /// <returns>Código del estado actual</returns>
        public static string GetEstadoActual(string codigo)
        {
            foreach (MaquinaEstadosBase maquinaEstados in ListaMaquinasEstados)
            {
                if (maquinaEstados.Codigo == codigo)
                {
                    return maquinaEstados.GetEstadoActual();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Devuelve la máquina de estados con el código correspondiente
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public static MaquinaEstadosBase GetMaquinaEstados(string codigo)
        {
            foreach (MaquinaEstadosBase maquinaEstados in ListaMaquinasEstados)
            {
                if (maquinaEstados.Codigo == codigo)
                {
                    return maquinaEstados;
                }
            }
            return null;
        }

        #endregion
    }

    /// <summary>
    /// Clase base de todas las máquinas de estados
    /// </summary>
    public partial class MaquinaEstadosBase
    {
        #region Atributo(s)

        /// <summary>
        /// Listado del conjunto de estados de la máquina de estados
        /// </summary>
        public List<EstadoBase> ListaEstados;

        /// <summary>
        /// Listado del conjunto de transiciones de la máquina de estados
        /// </summary>
        public List<TransicionBase> ListaTransiciones;

        /// <summary>
        /// Listado del conjunto de transiciones no universales de la máquina de estados
        /// </summary>
        public List<TransicionBase> ListaTransicionesSimples;

        /// <summary>
        /// Listado del conjunto de transiciones de la máquina de estados de tipo universal (se ejecutan independiemente del estado actual)
        /// </summary>
        public List<TransicionBase> ListaTransicionesUniversales;

        /// <summary>
        /// Estado actual de la máquina de estados
        /// </summary>
        public EstadoBase EstadoActual = null;

        /// <summary>
        /// Estado incial de la máquina de estados
        /// </summary>
        private EstadoBase EstadoInicial = null;

        /// <summary>
        /// Timer de refresco de la máquina de estados
        /// </summary>
        private System.Windows.Forms.Timer TimerEjecucion;

        /// <summary>
        /// Variable que informa del estado actual de la aplicación
        /// </summary>
        public string VariableEstadoActual = string.Empty;

        /// <summary>
        /// Código de la vista utilizada
        /// </summary>
        public string CodVista = string.Empty;

        #endregion

        #region Propiedad(es)

        /// <summary>
        /// Código del estado. Texto que lo identifica inequívocamente.
        /// </summary>
        private string _Codigo;
        /// <summary>
        /// Código del estado. Texto que lo identifica inequívocamente.
        /// </summary>
        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }

        /// <summary>
        /// Nombre del estado. Texto descriptivo de la funcionalidad del estado.
        /// </summary>
        private string _Nombre;
        /// <summary>
        /// Nombre del estado. Texto descriptivo de la funcionalidad del estado.
        /// </summary>
        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        /// <summary>
        /// Texto explicativo de la funcionalidad del estado
        /// </summary>
        private string _Descripcion;
        /// <summary>
        /// Texto explicativo de la funcionalidad del estado
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
        /// Tiempo entre comprobaciones de condiciones
        /// </summary>
        private TimeSpan _Cadencia;
        /// <summary>
        /// Tiempo entre comprobaciones de condiciones
        /// </summary>
        public TimeSpan Cadencia
        {
            get { return _Cadencia; }
            set { _Cadencia = value; }
        }

        /// <summary>
        /// Almacena la validez de la máquina de estados
        /// </summary>
        private bool _Valido;
        /// <summary>
        /// Almacena la validez de la máquina de estados
        /// </summary>
        public bool Valido
        {
            get { return _Valido; }
        }

        /// <summary>
        /// Habilita a la maquina de estados para llamar al colector de basura automáticamente al inicio de cada ciclo
        /// </summary>
        private bool _ColectorBasuraManual;
        /// <summary>
        /// Habilita a la maquina de estados para llamar al colector de basura automáticamente al inicio de cada ciclo
        /// </summary>
        public bool ColectorBasuraManual
        {
            get { return _ColectorBasuraManual; }
            set { _ColectorBasuraManual = value; }
        }

        /// <summary>
        /// Escenario de la clase
        /// </summary>
        protected Escenario _Escenario;
        /// <summary>
        /// Escenario de la clase
        /// </summary>
        public virtual Escenario Escenario
        {
            get { return _Escenario; }
            set { _Escenario = value; }
        }

        #endregion

        #region Constructor(es)

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public MaquinaEstadosBase(string codigo)
        {
            try
            {
                this._Codigo = codigo;

                // Creación de la lista de estados
                this.ListaEstados = new List<EstadoBase>();

                // Creación de la lista de transiciones
                this.ListaTransiciones = new List<TransicionBase>();
                this.ListaTransicionesSimples = new List<TransicionBase>();
                this.ListaTransicionesUniversales = new List<TransicionBase>();

                // Cargamos valores de la base de datos
                DataTable dtMaquinaEstados = AppBD.GetMaquinaEstados(this._Codigo);
                if (dtMaquinaEstados.Rows.Count == 1)
                {
                    this.Nombre = dtMaquinaEstados.Rows[0]["NombreMaquinaEstados"].ToString();
                    this.Descripcion = dtMaquinaEstados.Rows[0]["DescMaquinaEstados"].ToString();
                    this.Habilitado = (bool)dtMaquinaEstados.Rows[0]["HabilitadoMaquinaEstados"];
                    this.VariableEstadoActual = dtMaquinaEstados.Rows[0]["CodVariableEstadoActual"].ToString();
                    this.CodVista = dtMaquinaEstados.Rows[0]["CodVista"].ToString();
                    this._ColectorBasuraManual = true;

                    if (App.IsNumericInt(dtMaquinaEstados.Rows[0]["CadenciaEjecucion"]))
                    {
                        this.Cadencia = TimeSpan.FromMilliseconds((int)dtMaquinaEstados.Rows[0]["CadenciaEjecucion"]);
                    }

                    // Creación de los cronómetros
                    CronometroRuntime.NuevoCronometro(this._Codigo, "Duración Transito " + this.Nombre, "Duración del transito " + this.Nombre);
                    CronometroRuntime.NuevoCronometro(this._Codigo + ".Ejecucion", "Duración Ejecución " + this.Nombre, "Duración de la ejecución " + this.Nombre);

                    // Creación del escenario
                    this.CrearEscenario();

                    // Creación de los estados
                    DataTable dtEstados = AppBD.GetEstados(this.Codigo);
                    if (dtEstados.Rows.Count > 0)
                    {
                        foreach (DataRow drEstados in dtEstados.Rows)
                        {
                            string codEstado = drEstados["CodEstado"].ToString();
                            string ensambladoClaseImplementadora = drEstados["EnsambladoClaseImplementadora"].ToString();
                            string claseImplementadora = string.Format("{0}.{1}", ensambladoClaseImplementadora, drEstados["ClaseImplementadora"].ToString());

                            object objetoImplementado;
                            if (App.ConstruirClase(ensambladoClaseImplementadora, claseImplementadora, out objetoImplementado, this.Codigo, codEstado, this.Escenario))
                            {
                                EstadoBase estado = (EstadoBase)objetoImplementado;
                                ListaEstados.Add(estado);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("No se ha podido cargar la información de los estados de la máquina de estados " + codigo + " \r\nde la base de datos.");
                    }

                    // Creación de las transiciones
                    DataTable dtTransiciones = AppBD.GetTransiciones(this.Codigo);
                    if (dtTransiciones.Rows.Count > 0)
                    {
                        foreach (DataRow drTransiciones in dtTransiciones.Rows)
                        {
                            string codTransicion = drTransiciones["CodTransicion"].ToString();
                            string ensambladoClaseImplementadora = drTransiciones["EnsambladoClaseImplementadora"].ToString();
                            string claseImplementadora = string.Format("{0}.{1}", ensambladoClaseImplementadora, drTransiciones["ClaseImplementadora"].ToString());

                            object objetoImplementado;
                            if (App.ConstruirClase(ensambladoClaseImplementadora, claseImplementadora, out objetoImplementado, this.Codigo, codTransicion, this.Escenario))
                            {
                                TransicionBase transicion = (TransicionBase)objetoImplementado;
                                ListaTransiciones.Add(transicion);
                                if (transicion is TransicionUniversal)
                                {
                                    this.ListaTransicionesUniversales.Add(transicion);
                                }
                                else
                                {
                                    this.ListaTransicionesSimples.Add(transicion);
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("No se ha podido cargar la información de las transiciones de la máquina de estados " + codigo + " \r\nde la base de datos.");
                    }
                }
                else
                {
                    throw new Exception("No se ha podido cargar la información de la máquina de estados " + codigo + " \r\nde la base de datos.");
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Fatal(ModulosControl.MaquinasEstado, this._Codigo, exception);
                throw new Exception("Imposible iniciar la máquina de estados " + this.Codigo);
            }
        }

        #endregion

        #region Método(s) privado(s)

        /// <summary>
        /// Prepara los estadeos y las transiciones para su ejecución
        /// </summary>
        private void PrepararEjecucion()
        {
            foreach (TransicionBase transicion in this.ListaTransicionesSimples)
            {
                transicion.EstadoOrigen = this.BuscaEstado(transicion.CodigoEstadoOrigen);
                transicion.EstadoDestino = this.BuscaEstado(transicion.CodigoEstadoDestino);

                if ((transicion.EstadoOrigen == null) || (transicion.EstadoDestino == null))
                {
                    throw new Exception("La transición " + transicion.Codigo + " no está correctamente definida");
                }
            }
            foreach (TransicionBase transicion in this.ListaTransicionesUniversales)
            {
                transicion.EstadoDestino = this.BuscaEstado(transicion.CodigoEstadoDestino);

                if (transicion.EstadoDestino == null)
                {
                    throw new Exception("La transición universal " + transicion.Codigo + " no está correctamente definida");
                }
            }
            foreach (EstadoBase estado in this.ListaEstados)
            {
                estado.ListaTransicionesEntrantes = this.BuscaTransicionesEntrantes(estado);
                estado.ListaTransicionesSalientes = this.BuscaTransicionesSalientes(estado);

                if ((estado.ListaTransicionesEntrantes == null) || (estado.ListaTransicionesSalientes == null))
                {
                    throw new Exception("El estado " + estado.Codigo + " no está correctamente definido");
                }
            }
        }

        /// <summary>
        /// Cambia el estado actual de la máquina de estados y realiza su ejecución
        /// </summary>
        /// <param name="siguienteEstado">Nuevo estado actual</param>
        private void EjecutarNuevoEstado(EstadoBase siguienteEstado)
        {
            if (this.ListaEstados.Contains(siguienteEstado))
            {
                if (this.EstadoActual != null)
                {
                    // Eliminamos la suscripcion a las variables necesarias en las transiciones
                    this.EliminarSuscripcionVariables(EstadoActual);

                    // Se finaliza el estado actual
                    this.EstadoActual.FinalizarEjecucion();

                    // Se llama al colector de basura
                    if (this._ColectorBasuraManual && (this.EstadoInicial == siguienteEstado))
                    {
                        //Force garbage collection.
                        GC.Collect();

                        // Wait for all finalizers to complete before continuing.
                        // Without this call to GC.WaitForPendingFinalizers, 
                        // the worker loop below might execute at the same time 
                        // as the finalizers.
                        // With this call, the worker loop executes only after
                        // all finalizers have been called.
                        GC.WaitForPendingFinalizers();
                    }
                }

                // Cronónmetro del ciclo
                if (siguienteEstado == this.EstadoInicial)
                {
                    if (this.EstadoActual != null)
                    {
                        CronometroRuntime.Stop(this._Codigo);
                        CronometroRuntime.Stop(this._Codigo + ".Ejecucion");
                    }
                    CronometroRuntime.Start(this._Codigo);
                }
                if (this.EstadoActual == this.EstadoInicial)
                {
                    CronometroRuntime.Start(this._Codigo + ".Ejecucion");
                }

                // Se establece el nuevo estado actual
                this.EstadoActual = siguienteEstado;

                // Se Modifica la variable del estado
                VariableRuntime.SetValue(this.VariableEstadoActual, this.EstadoActual.Codigo, "MaquinaEstados", this.Codigo);

                // Enviamos el cambio del estado a los suscriptores
                DateTime momentoCambioEstado = DateTime.Now;
                this.LanzarCambioEstado(this.EstadoActual.Codigo, momentoCambioEstado);

                // Enviamos el mensaje a los suscriptores
                if (this.EstadoActual.Monitorizado)
                {
                    this.LanzarMensaje(TipoMensajeMaquinaEstados.CambioEstado, this.EstadoActual.Nombre + ": " + this.EstadoActual.Descripcion, momentoCambioEstado);
                    foreach (TransicionBase transicion in this.EstadoActual.ListaTransicionesSalientes)
                    {
                        if (transicion.Monitorizado)
                        {
                            this.LanzarMensaje(TipoMensajeMaquinaEstados.CondicionesTransicion, transicion.Nombre + ": " + transicion.ExplicacionCondicionEsperada, momentoCambioEstado);
                        }
                    }
                }

                // Se ejecuta el estado actual
                this.EstadoActual.IniciarEjecucion();

                // Creamos la suscripcion a las variables necesarias en las transiciones
                this.CrearSuscripcionVariables(EstadoActual);
            }
        }

        /// <summary>
        /// Enviamos el mensaje a los suscriptores
        /// </summary>
        /// <param name="tipoMensajeMaquinaEstados">Enumerado del tipo de mensaje que la máquina de estados envía a la monitorización</param>
        /// <param name="p"></param>
        /// <param name="momentoCambioEstado"></param>
        private void LanzarMensaje(TipoMensajeMaquinaEstados tipo, string informacion, DateTime momento)
        {
            if (this.OnMensajeMaquinaEstados != null)
            {
                this.OnMensajeMaquinaEstados(this, new EventMessageRaised(tipo, informacion, momento));
            }
        }

        /// <summary>
        /// Enviamos el cambio de estado a los suscriptores
        /// </summary>
        /// <param name="tipoMensajeMaquinaEstados">Enumerado del tipo de mensaje que la máquina de estados envía a la monitorización</param>
        /// <param name="p"></param>
        /// <param name="momentoCambioEstado"></param>
        private void LanzarCambioEstado(string codEstado, DateTime momento)
        {
            if (this.OnEstadoCambiado != null)
            {
                this.OnEstadoCambiado(this, new EventStateChanged(codEstado, momento));
            }
        }

        /// <summary>
        /// Creamos la suscripcion a las variables necesarias en las transiciones
        /// </summary>
        /// <param name="estado">Estado del cuyas transiciones de salidas queremos elminar la suscripción a variables</param>
        private void CrearSuscripcionVariables(EstadoBase estado)
        {
            if (this._Valido)
            {
                foreach (TransicionBase transicion in estado.ListaTransicionesSalientes)
                {
                    foreach (string codigoVariable in transicion.VariablesUtilizadas)
                    {
                        VariableRuntime.CrearSuscripcion(codigoVariable, "MaquinaEstados", this.Codigo, new CambioValorDelegate(this.EjecutarTransiciones));
                    }
                }
            }
        }

        /// <summary>
        /// Eliminamos la suscripcion a las variables necesarias en las transiciones
        /// </summary>
        /// <param name="estado">Estado del cuyas transiciones de salidas queremos elminar la suscripción a variables</param>
        private void EliminarSuscripcionVariables(EstadoBase estado)
        {
            if (this._Valido)
            {
                foreach (TransicionBase transicion in estado.ListaTransicionesSalientes)
                {
                    foreach (string codigoVariable in transicion.VariablesUtilizadas)
                    {
                        VariableRuntime.EliminarSuscripcion(codigoVariable, "MaquinaEstados", this.Codigo, new CambioValorDelegate(this.EjecutarTransiciones));
                    }
                }
            }
        }

        /// <summary>
        /// Se valida que la máquina de estados cumple ciertas restricciones para su correcta ejecución
        /// </summary>
        /// <returns>Verdadero si la máquina de estados es válida</returns>
        private bool Validar()
        {
            // Se valida que exista algún estado
            if (this.ListaEstados.Count == 0)
            {
                return false;
            }

            // Se valida que exista alguna transición
            if (this.ListaTransiciones.Count == 0)
            {
                return false;
            }

            // Se valida que exista un único estado inicial
            int contEstadosInicial = ListaEstados.FindAll(delegate(EstadoBase estado) { return estado.EsEstadoInicial; }).Count;
            if (contEstadosInicial != 1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Devuelve el estado inicial de la máquina de estados
        /// </summary>
        /// <returns>Estado inicial</returns>
        private EstadoBase BuscaEstadoInicial()
        {
            return ListaEstados.Find(delegate(EstadoBase estado) { return estado.EsEstadoInicial; });
        }

        /// <summary>
        /// Busca las transiciones de entrada a un estado
        /// </summary>
        /// <param name="estado">Estado del cual se buscan las transiciones de salida</param>
        /// <returns>Lista de transiciones de salida</returns>
        private List<TransicionBase> BuscaTransicionesSalientes(EstadoBase estado)
        {
            List<TransicionBase> busqueda = new List<TransicionBase>();
            if (this._Valido)
            {
                foreach (TransicionBase transicion in ListaTransicionesSimples)
                {
                    if (transicion.EstadoOrigen == estado)
                    {
                        busqueda.Add(transicion);
                    }
                }
            }

            return busqueda;
        }

        /// <summary>
        /// Busca las transiciones de salida de un estado
        /// </summary>
        /// <param name="estado">Estado del cual se buscan las transiciones de salida</param>
        /// <returns>Lista de transiciones de salida</returns>
        private List<TransicionBase> BuscaTransicionesEntrantes(EstadoBase estado)
        {
            List<TransicionBase> busqueda = new List<TransicionBase>();
            if (this._Valido)
            {
                foreach (TransicionBase transicion in ListaTransiciones)
                {
                    if (transicion.EstadoDestino == estado)
                    {
                        busqueda.Add(transicion);
                    }
                }
            }

            return busqueda;
        }

        /// <summary>
        /// Busca un estado en la lista de estados
        /// </summary>
        /// <param name="codigoEstado">Tipo de estado a buscar</param>
        /// <returns>Estado</returns>
        private EstadoBase BuscaEstado(string codigoEstado)
        {
            return ListaEstados.Find(delegate(EstadoBase estado) { return estado.Codigo == codigoEstado; });
        }

        /// <summary>
        /// Ejecuta la comprobación de transiciones
        /// </summary>
        private void EjecutarTransiciones()
        {
            if (!this.EstadoActual.EnEjecucion) // Si el estado actual está en ejecución no se pueden comprobar sus transiciones
            {
                if (this._Valido)
                {
                    bool continuar;
                    List<TransicionBase> listaTransicionesActuales;

                    do
                    {
                        continuar = false;

                        // Comprobación de las transiciones universales (Deben ser excluyentes, es decir, nunca deben poder ejecutarse varias transiciones simultaneamente)
                        listaTransicionesActuales = this.ListaTransicionesUniversales; // Se extrae la lista de transiciones universales
                        foreach (TransicionBase transicion in listaTransicionesActuales)
                        {
                            if (transicion.EstadoDestino != this.EstadoActual)
                            {
                                if (transicion.IniciarComprobacionCondiciones())
                                {
                                    continuar = true;
                                    this.EjecutarNuevoEstado(transicion.EstadoDestino);
                                    break;
                                }
                            }
                        }

                        // Comprobación de las transiciones universales
                        listaTransicionesActuales = this.EstadoActual.ListaTransicionesSalientes; // Se extrae la lista de transiciones
                        foreach (TransicionBase transicion in listaTransicionesActuales)
                        {
                            if (transicion.IniciarComprobacionCondiciones())
                            {
                                continuar = true;
                                this.EjecutarNuevoEstado(transicion.EstadoDestino);
                                break;
                            }
                        }

                        //Application.DoEvents();
                    } while (continuar);
                }
            }
        }

        #endregion

        #region Método(s) público(s)

        /// <summary>
        /// Inicio de la máquina de estados
        /// </summary>
        internal void IniciarEjecucion()
        {
            if (this._Habilitado && this._Valido)
            {
                // Iniciamos la ejecución del estado incicial
                this.EjecutarNuevoEstado(EstadoInicial);

                // Se ejecutan las transiciones
                this.EjecutarTransiciones();

                // Se ejecuta el timer
                this.TimerEjecucion.Start();
            }
        }

        /// <summary>
        /// Parada de la máquina de estados
        /// </summary>
        internal void PararEjecucion()
        {
            if (this._Habilitado && this._Valido)
            {
                // Se detiene el timer
                this.TimerEjecucion.Stop();

                // Se eliminan las suscripciones a las variables
                this.EliminarSuscripcionVariables(this.EstadoActual);
            }
        }

        /// <summary>
        /// Devuelve el código del estado actual de la máquina de estados
        /// </summary>
        /// <param name="codigo">Código de la máquina de estados</param>
        /// <returns>Código del estado actual</returns>
        public string GetEstadoActual()
        {
            if (this.EstadoActual != null)
            {
                return this.EstadoActual.Codigo;
            }
            return string.Empty;
        }

        /// <summary>
        /// Devuelve el estado con el código buscado
        /// </summary>
        /// <param name="codigo">Código del estado</param>
        /// <returns>Estado buscado</returns>
        public EstadoBase GetEstado(string codigo)
        {
            foreach (EstadoBase estado in this.ListaEstados)
            {
                if (estado.Codigo == codigo)
                {
                    return estado;
                    break;                    
                }
            }
            return null;
        }

        /// <summary>
        /// Devuelve la transición con el código buscado
        /// </summary>
        /// <param name="codigo">Código de la transición</param>
        /// <returns>Transición buscada</returns>
        public TransicionBase GetTransicion(string codigo)
        {
            foreach (TransicionBase transicion in this.ListaTransiciones)
            {
                if (transicion.Codigo == codigo)
                {
                    return transicion;
                    break;
                }
            }
            return null;
        }

        #endregion

        #region Método(s) virtual(es)

        /// <summary>
        /// Método donde se rellenará toda la información de la máquina de estados
        /// </summary>
        public virtual void Inicializar()
        {
            // Comprobación de la validez de la máquina de estados
            this._Valido = this.Validar();

            if (this._Valido)
            {
                this.PrepararEjecucion();

                // Estado inicial
                this.EstadoInicial = this.BuscaEstadoInicial();

                foreach (EstadoBase estado in this.ListaEstados)
                {
                    estado.Inicializar();
                }

                foreach (TransicionBase transicion in this.ListaTransiciones)
                {
                    transicion.Inicializar();
                }

                // Parametrización del timer
                this.TimerEjecucion = new System.Windows.Forms.Timer();
                this.TimerEjecucion.Interval = Convert.ToInt32(Math.Round(this.Cadencia.TotalMilliseconds));
                this.TimerEjecucion.Tick += new EventHandler(EventoTimerEjecucion);
            }
            else
            {
                throw new Exception("La máquina de estados no es válida.");
            }
        }

        /// <summary>
        /// Método donde se finaliza la máquina de estados
        /// </summary>
        public virtual void Finalizar()
        {
            // La máquina de estados ya no es válida
            this._Valido = false;

            foreach (TransicionBase transicion in this.ListaTransiciones)
            {
                transicion.Finalizar();
            }

            foreach (EstadoBase estado in this.ListaEstados)
            {
                estado.Finalizar();
            }

            // Parametrización del timer
            this.TimerEjecucion.Tick -= new EventHandler(EventoTimerEjecucion);
        }

        /// <summary>
        /// Método a heredar para procesar los mensajes producidos por la máquina de estados
        /// </summary>
        public virtual void Mensaje(TipoMensajeMaquinaEstados tipo, string informacion, DateTime hora)
        {
        }

        /// <summary>
        /// Creación del escenario
        /// </summary>
        public virtual void CrearEscenario()
        {
            // Implementado en heredados
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento del timer de ejecución
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void EventoTimerEjecucion(object sender, EventArgs e)
        {
            this.TimerEjecucion.Enabled = false; // Se deshabilita para que no pueda ejecutarse 2 veces simultaneamente

            try
            {
                this.EjecutarTransiciones();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosControl.MaquinasEstado, this._Codigo, exception);
            }

            this.TimerEjecucion.Enabled = true;
        }

        #endregion

        #region Definición de delegado(s)
        /// <summary>
        /// Delegado que indica de la llegada de un mensaje de la máquina de estados para visualizarse en la monitorización
        /// </summary>
        public MensajeMaquinaEstadosLanzado OnMensajeMaquinaEstados;

        /// <summary>
        /// Delegado que indica el cambio de estado
        /// </summary>
        public EstadoCambiado OnEstadoCambiado;
        #endregion
    }

    /// <summary>
    /// Clase base de todos los tipos de estados
    /// </summary>
    public class EstadoBase
    {
        #region Atributo(s)

        /// <summary>
        /// Listado del conjunto de transiciones entrantes
        /// </summary>
        public List<TransicionBase> ListaTransicionesEntrantes;

        /// <summary>
        /// Listado del conjunto de transiciones salientes
        /// </summary>
        public List<TransicionBase> ListaTransicionesSalientes;

        /// <summary>
        /// Código del crónometro asociado a la cuenta del tiempo de activación del estado
        /// </summary>
        protected string CodCronometroAsociadoActivacion;

        /// <summary>
        /// Código del crónometro asociado a la cuenta del tiempo de ejecución del estado
        /// </summary>
        protected string CodCronometroAsociadoEjecucionEntrada;

        /// <summary>
        /// Código del crónometro asociado a la cuenta del tiempo de ejecución de la salida del estado
        /// </summary>
        protected string CodCronometroAsociadoEjecucionSalida;

        /// <summary>
        /// Tipo de Estado
        /// </summary>
        public TipoEstado TipoEstado;

        #endregion

        #region Propiedad(es)

        /// <summary>
        /// Código del estado. Texto que lo identifica inequívocamente.
        /// </summary>
        private string _Codigo;
        /// <summary>
        /// Código del estado. Texto que lo identifica inequívocamente.
        /// </summary>
        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }

        /// <summary>
        /// Código de la máquina de estados. Texto que lo identifica inequívocamente.
        /// </summary>
        private string _CodigoMaquinaEstados;
        /// <summary>
        /// Código de la máquina de estados. Texto que lo identifica inequívocamente.
        /// </summary>
        public string CodigoMaquinaEstados
        {
            get { return _CodigoMaquinaEstados; }
            set { _CodigoMaquinaEstados = value; }
        }

        /// <summary>
        /// Nombre del estado. Texto descriptivo de la funcionalidad del estado.
        /// </summary>
        private string _Nombre;
        /// <summary>
        /// Nombre del estado. Texto descriptivo de la funcionalidad del estado.
        /// </summary>
        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        /// <summary>
        /// Texto explicativo de la funcionalidad del estado
        /// </summary>
        private string _Descripcion;
        /// <summary>
        /// Texto explicativo de la funcionalidad del estado
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
        /// Informa a la monitorización
        /// </summary>
        private bool _Monitorizado;
        /// <summary>
        /// Informa a la monitorización
        /// </summary>
        public bool Monitorizado
        {
            get { return _Monitorizado; }
            set { _Monitorizado = value; }
        }

        /// <summary>
        /// Indica si el estado es el inicial de la máquina de estados
        /// </summary>
        private bool _EsEstadoInicial;
        /// <summary>
        /// Indica si el estado es el inicial de la máquina de estados
        /// </summary>
        public bool EsEstadoInicial
        {
            get { return _EsEstadoInicial; }
            set { _EsEstadoInicial = value; }
        }

        /// <summary>
        /// Tiempo máximo que puede durar el estado
        /// </summary>
        private TimeSpan _TimeOut;
        /// <summary>
        /// Tiempo máximo que puede durar el estado
        /// </summary>
        public TimeSpan TimeOut
        {
            get { return _TimeOut; }
            set { _TimeOut = value; }
        }

        /// <summary>
        /// Indica si el estado actual está en ejecución
        /// </summary>
        private bool _EnEjecucion;
        /// <summary>
        /// Indica si el estado actual está en ejecución
        /// </summary>
        public bool EnEjecucion
        {
            get { return _EnEjecucion; }
            set { _EnEjecucion = value; }
        }

        /// <summary>
        /// Duración de la activación del estado
        /// </summary>
        public TimeSpan DuracionActivacion
        {
            get
            {
                return CronometroRuntime.DuracionUltimaEjecucion(this.CodCronometroAsociadoActivacion);
            }
        }

        /// <summary>
        /// Maquina de estados
        /// </summary>
        protected Escenario _Escenario;
        /// <summary>
        /// Maquina de estados
        /// </summary>
        public Escenario Escenario
        {
            get { return _Escenario; }
            set { _Escenario = value; }
        }

        #endregion

        #region Constructor(es)

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codigoMaquinaEstados">Código de la máquina de estados</param>
        /// <param name="codigo">Código del estado</param>
        public EstadoBase(string codigoMaquinaEstados, string codigo, Escenario escenario)
        {
            try
            {

                // Inicializamos las variables
                this._CodigoMaquinaEstados = codigoMaquinaEstados;
                this._Codigo = codigo;
                this._Escenario = escenario;

                // Cargamos valores de la base de datos
                DataTable dtEstado = AppBD.GetEstado(this.CodigoMaquinaEstados, this.Codigo);
                if (dtEstado.Rows.Count == 1)
                {
                    this.Nombre = dtEstado.Rows[0]["NombreEstado"].ToString();
                    this.Descripcion = dtEstado.Rows[0]["DescEstado"].ToString();
                    this.Habilitado = (bool)dtEstado.Rows[0]["HabilitadoEstado"];
                    this.Monitorizado = (bool)dtEstado.Rows[0]["Monitorizado"];
                    this.EsEstadoInicial = (bool)dtEstado.Rows[0]["EsEstadoInicial"];
                    if (App.IsNumericInt(dtEstado.Rows[0]["TimeOut"]))
                    {
                        this.TimeOut = TimeSpan.FromMilliseconds((int)dtEstado.Rows[0]["TimeOut"]);
                    }

                    this.TipoEstado = (TipoEstado)App.EnumParse(TipoEstado.GetType(), dtEstado.Rows[0]["Tipo"].ToString(), TipoEstado.EstadoSimple);

                    // Creación de los cronómetros
                    this.CodCronometroAsociadoActivacion = this.CodigoMaquinaEstados + "." + this.Codigo + ".Activacion";
                    CronometroRuntime.NuevoCronometro(this.CodCronometroAsociadoActivacion, "Activación Estado " + this.Nombre, "Activación del estado " + this.Nombre);
                    this.CodCronometroAsociadoEjecucionEntrada = this.CodigoMaquinaEstados + "." + this.Codigo + ".Ejecucion";
                    CronometroRuntime.NuevoCronometro(this.CodCronometroAsociadoEjecucionEntrada, "Ejecución de la entrada al Estado " + this.Nombre, "Ejecución de la entrada al estado " + this.Nombre);
                    this.CodCronometroAsociadoEjecucionSalida = this.CodigoMaquinaEstados + "." + this.Codigo + ".Ejecucion";
                    CronometroRuntime.NuevoCronometro(this.CodCronometroAsociadoEjecucionSalida, "Ejecución de la salida del Estado " + this.Nombre, "Ejecución de la salida del estado " + this.Nombre);
                }
                else
                {
                    throw new Exception("No se ha podido cargar la información del estado " + codigo + " \r\nde la base de datos.");
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Fatal(ModulosControl.MaquinasEstado, this._Codigo, exception);
                throw new Exception("Imposible iniciar el estado " + this.Codigo);
            }
        }

        #endregion

        #region Método(s) virtual(es)

        /// <summary>
        /// Método donde se rellenará toda la información del estado
        /// </summary>
        public virtual void Inicializar()
        {

        }

        /// <summary>
        /// Método donde se espera la finalización
        /// </summary>
        public virtual void Finalizar()
        {
            App.Espera(ref this._EnEjecucion, false, 1000);
        }

        /// <summary>
        /// Método que se ejecuta al iniciarse del estado
        /// </summary>
        internal virtual void IniciarEjecucion()
        {
            // Cronometramos la duración del estado como activo
            CronometroRuntime.Start(this.CodCronometroAsociadoActivacion);

            // Guardamos la traza de registros del inicio del estado al entrar
            LogsRuntime.Info(ModulosControl.MaquinasEstado, this._Codigo, "Activación del estado " + this._Codigo);

            this._EnEjecucion = true;
            try
            {
                // Cronometramos la duración de la ejecución
                CronometroRuntime.Start(this.CodCronometroAsociadoEjecucionEntrada);

                // Guardamos la traza de registros del inicio del estado al entrar
                LogsRuntime.Debug(ModulosControl.MaquinasEstado, this._Codigo, "Inicio del método 'al entrar' del estado " + this._Codigo);

                // Ejecutamos el estado
                this.EjecucionAlEntrar();

                // Paramos el cronometro de la duración de la ejecución
                CronometroRuntime.Stop(this.CodCronometroAsociadoEjecucionEntrada);

                // Guardamos la traza de registros del fin del estado al entrar
                LogsRuntime.Debug(ModulosControl.MaquinasEstado, this._Codigo, "Fin del método 'al entrar' del estado " + this._Codigo + ". Duración: " + CronometroRuntime.DuracionUltimaEjecucion(this.CodCronometroAsociadoEjecucionEntrada).ToString());
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosControl.MaquinasEstado, this._Codigo, exception);
            }
            this._EnEjecucion = false;
        }

        /// <summary>
        /// Método que se ha de heredar para realizar las acciones asociadas al estado
        /// </summary>
        internal virtual void FinalizarEjecucion()
        {
            this._EnEjecucion = true;
            try
            {
                // Cronometramos la duración de la ejecución
                CronometroRuntime.Start(this.CodCronometroAsociadoEjecucionSalida);

                // Guardamos la traza de registros del inicio del estado al salir
                LogsRuntime.Debug(ModulosControl.MaquinasEstado, this._Codigo, "Inicio del método 'al salir' del estado " + this._Codigo);

                // Ejecutamos el estado
                this.EjecucionAlSalir();

                // Paramos el cronometro de la duración de la ejecución
                CronometroRuntime.Stop(this.CodCronometroAsociadoEjecucionSalida);

                // Guardamos la traza de registros del fin del estado al salir
                LogsRuntime.Debug(ModulosControl.MaquinasEstado, this._Codigo, "Fin del método 'al salir' del estado " + this._Codigo + ". Duración: " + CronometroRuntime.DuracionUltimaEjecucion(this.CodCronometroAsociadoEjecucionSalida).ToString());
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosControl.MaquinasEstado, this.Codigo, exception);
            }
            this._EnEjecucion = false;

            // Parmos el cronómetro de la duración del estado como activo
            CronometroRuntime.Stop(this.CodCronometroAsociadoActivacion);

            // Guardamos la traza de registros del fin del estado al salir
            LogsRuntime.Info(ModulosControl.MaquinasEstado, this._Codigo, "Fin de la activación del estado " + this._Codigo + ". Duración: " + CronometroRuntime.DuracionUltimaEjecucion(this.CodCronometroAsociadoActivacion).ToString());
        }

        /// <summary>
        /// Método que se ha de heredar para realizar las acciones asociadas a la entrada al estado
        /// </summary>
        protected virtual void EjecucionAlEntrar()
        {
        }

        /// <summary>
        /// Método que se ha de heredar para realizar las acciones asociadas a la salida del estado
        /// </summary>
        protected virtual void EjecucionAlSalir()
        {
        }
        #endregion
    }

    /// <summary>
    /// Clase base de todos los tipos de estados
    /// </summary>
    public class EstadoAsincrono: EstadoBase
    {
        #region Atributo(s)
        /// <summary>
        /// Trabajo en segundo plano
        /// </summary>
        private BackgroundWorker ThreadEjecucion;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Código de la variable asociada a la ejecución del estado.
        /// </summary>
        private string _CodVariableEnEjecucion;
        /// <summary>
        /// Código de la variable asociada a la ejecución del estado.
        /// </summary>
        public string CodVariableEnEjecucion
        {
            get { return _CodVariableEnEjecucion; }
            set { _CodVariableEnEjecucion = value; }
        } 
        #endregion

        #region Constructor(es)

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codigoMaquinaEstados">Código de la máquina de estados</param>
        /// <param name="codigo">Código del estado</param>
        public EstadoAsincrono(string codigoMaquinaEstados, string codigo, Escenario escenario)
            : base(codigoMaquinaEstados, codigo, escenario)
        {
            try
            {
                // Creamos los campos
                this.ThreadEjecucion = new BackgroundWorker();
                this.ThreadEjecucion.WorkerSupportsCancellation = true;
                this.ThreadEjecucion.DoWork += new DoWorkEventHandler(this.EjecucionEnThread);
                this.ThreadEjecucion.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.FinEjecucionThread);

                // Cargamos valores de la base de datos
                DataTable dtEstado = AppBD.GetEstadoAsincrono(this.CodigoMaquinaEstados, this.Codigo);
                if (dtEstado.Rows.Count == 1)
                {
                    this._CodVariableEnEjecucion = dtEstado.Rows[0]["CodVariable"].ToString();
                }
                else
                {
                    throw new Exception("No se ha podido cargar la información del estado asíncrono " + codigo + " \r\nde la base de datos.");
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Fatal(ModulosControl.MaquinasEstado, this.Codigo, exception);
                throw new Exception("Imposible iniciar el estado asíncrono " + this.Codigo);
            }
        }

        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Ejecución del estado a través de otro hilo de ejecución distinto al de la aplicación principal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EjecucionEnThread(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Ejecutamos el estado
                this.EjecucionAlEntrar();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosControl.MaquinasEstado, this.Codigo, exception);
            }
        }

        /// <summary>
        /// Final de la ejecución del estado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FinEjecucionThread(object sender, RunWorkerCompletedEventArgs e)
        {
            this.EnEjecucion = false; // Nuevo

            // Paramos el cronometro de la duración de la ejecución
            CronometroRuntime.Stop(this.CodCronometroAsociadoEjecucionEntrada);

            // Guardamos la traza de registros del fin del estado al entrar
            LogsRuntime.Debug(ModulosControl.MaquinasEstado, this.Codigo, "Fin del método 'al entrar' del estado " + this.Codigo + ". Duración: " + CronometroRuntime.DuracionUltimaEjecucion(this.CodCronometroAsociadoEjecucionEntrada).ToString());

            // Llamada a la variable que indica el estado de ejecución del thread
            VariableRuntime.SetValue(this._CodVariableEnEjecucion, false, "MaquinaEstados", this.Codigo);
        }

        #endregion

        #region Método(s) heredado(s)

        /// <summary>
        /// Método que se ejecuta al iniciarse del estado
        /// </summary>
        internal override void IniciarEjecucion()
        {
            // Cronometramos la duración del estado como activo
            CronometroRuntime.Start(this.CodCronometroAsociadoActivacion);

            // Guardamos la traza de registros del inicio del estado al entrar
            LogsRuntime.Info(ModulosControl.MaquinasEstado, this.Codigo, "Activación del estado " + this.Codigo);
                                                                       
            this.EnEjecucion = true;
            try
            {
                // Cronometramos la duración de la ejecución
                CronometroRuntime.Start(this.CodCronometroAsociadoEjecucionEntrada);

                // Guardamos la traza de registros del inicio del estado al entrar
                LogsRuntime.Debug(ModulosControl.MaquinasEstado, this.Codigo, "Inicio del método 'al entrar' del estado " + this.Codigo);

                // Llamada a la variable que indica el estado de ejecución del thread
                VariableRuntime.SetValue(this._CodVariableEnEjecucion, true, "MaquinaEstados", this.Codigo);

                // Ejecutamos el estado del thread
                this.ThreadEjecucion.RunWorkerAsync();

            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosControl.MaquinasEstado, this.Codigo, exception);
            }
            //this.EnEjecucion = false;
        }

        /// <summary>
        /// Método donde se espera la finalización
        /// </summary>
        public override void Finalizar()
        {
            base.Finalizar();

            this.ThreadEjecucion.CancelAsync();
            this.ThreadEjecucion = null;
        }

        #endregion
    }

    /// <summary>
    /// Clase base de todas las transiciones entre estados
    /// </summary>
    public class TransicionBase
    {
        #region Atributo(s)
        /// <summary>
        /// Código del crónometro asociado a la cuenta del tiempo de ejecución de la transición
        /// </summary>
        protected string CodCronometro;

        /// <summary>
        /// Tipo de Transición
        /// </summary>
        public TipoTransicion TipoTransicion;
        #endregion

        #region Propiedad(es)

        /// <summary>
        /// Código de la transición. Texto que lo identifica inequívocamente.
        /// </summary>
        private string _Codigo;
        /// <summary>
        /// Código de la transición. Texto que lo identifica inequívocamente.
        /// </summary>
        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }

        /// <summary>
        /// Código de la máquina de estados. Texto que lo identifica inequívocamente.
        /// </summary>
        private string _CodigoMaquinaEstados;
        /// <summary>
        /// Código de la máquina de estados. Texto que lo identifica inequívocamente.
        /// </summary>
        public string CodigoMaquinaEstados
        {
            get { return _CodigoMaquinaEstados; }
            set { _CodigoMaquinaEstados = value; }
        }

        /// <summary>
        /// Nombre de la transición. Texto descriptivo de la funcionalidad del estado.
        /// </summary>
        private string _Nombre;
        /// <summary>
        /// Nombre de la transición. Texto descriptivo de la funcionalidad del estado.
        /// </summary>
        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        /// <summary>
        /// Texto explicativo del condiciones esperadas
        /// </summary>
        private string _ExplicacionCondicionEsperada;
        /// <summary>
        /// Texto explicativo del condiciones esperadas
        /// </summary>
        public string ExplicacionCondicionEsperada
        {
            get { return _ExplicacionCondicionEsperada; }
            set { _ExplicacionCondicionEsperada = value; }
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
        /// Informa a la monitorización
        /// </summary>
        private bool _Monitorizado;
        /// <summary>
        /// Informa a la monitorización
        /// </summary>
        public bool Monitorizado
        {
            get { return _Monitorizado; }
            set { _Monitorizado = value; }
        }

        /// <summary>
        /// Tipo del estado origen de la transición
        /// </summary>
        public string CodigoEstadoOrigen;
        /// <summary>
        /// Estado origen de la transición
        /// </summary>
        protected EstadoBase _EstadoOrigen;
        /// <summary>
        /// Estado origen de la transición
        /// </summary>
        public EstadoBase EstadoOrigen
        {
            get { return _EstadoOrigen; }
            set { _EstadoOrigen = value; }
        }

        /// <summary>
        /// Tipo del estado destino de la transición
        /// </summary>
        public string CodigoEstadoDestino;
        /// <summary>
        /// Estado destino de la transición
        /// </summary>
        protected EstadoBase _EstadoDestino;
        /// <summary>
        /// Estado destino de la transición
        /// </summary>
        public EstadoBase EstadoDestino
        {
            get { return _EstadoDestino; }
            set { _EstadoDestino = value; }
        }

        /// <summary>
        /// Lista de códigos de variables utilizadas en la condición de transición
        /// </summary>
        private List<string> _VariablesUtilizadas;
        /// <summary>
        /// Lista de códigos devariables utilizadas en la condición de transición
        /// </summary>
        public List<string> VariablesUtilizadas
        {
            get { return _VariablesUtilizadas; }
            set { _VariablesUtilizadas = value; }
        }

        /// <summary>
        /// Maquina de estados
        /// </summary>
        protected Escenario _Escenario;
        /// <summary>
        /// Maquina de estados
        /// </summary>
        public Escenario Escenario
        {
            get { return _Escenario; }
            set { _Escenario = value; }
        }

        #endregion

        #region Constructor(es)

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codigoMaquinaEstados">Código de la máquina de estados</param>
        /// <param name="codigo">Código de la transición</param>
        public TransicionBase(string codigoMaquinaEstados, string codigo, Escenario escenario)
        {
            try
            {
                // Inicializamos las variables
                this._CodigoMaquinaEstados = codigoMaquinaEstados;
                this._Codigo = codigo;
                this._VariablesUtilizadas = new List<string>();
                this._Escenario = escenario;

                // Cargamos valores de la base de datos
                DataTable dtTransicion = AppBD.GetTransicion(this._CodigoMaquinaEstados, this._Codigo);
                if (dtTransicion.Rows.Count == 1)
                {
                    this.Nombre = dtTransicion.Rows[0]["NombreTransicion"].ToString();
                    this.ExplicacionCondicionEsperada = dtTransicion.Rows[0]["ExplicacionCondicionEsperada"].ToString();
                    this.Habilitado = (bool)dtTransicion.Rows[0]["HabilitadoTransicion"];
                    this.Monitorizado = (bool)dtTransicion.Rows[0]["Monitorizado"];
                    this.CodigoEstadoOrigen = dtTransicion.Rows[0]["CodigoEstadoOrigen"].ToString();
                    this.CodigoEstadoDestino = dtTransicion.Rows[0]["CodigoEstadoDestino"].ToString();

                    this.TipoTransicion = (TipoTransicion)App.EnumParse(TipoTransicion.GetType(), dtTransicion.Rows[0]["Tipo"].ToString(), TipoTransicion.TransicionSimple);

                    // Cargamos el código de las variables que se utilizan en la transición
                    DataTable dtVariables = AppBD.GetVariablesTransicion(this._CodigoMaquinaEstados, this._Codigo);
                    if (dtVariables.Rows.Count > 0)
                    {
                        object codVariableAux;
                        foreach (DataRow dr in dtVariables.Rows)
                        {
                            codVariableAux = dr["CodVariable"];
                            if ((codVariableAux is string) && ((string)codVariableAux != string.Empty))
                            {
                                this.VariablesUtilizadas.Add((string)codVariableAux);
                            }
                        }
                    }

                    // Creación de los cronómetros
                    this.CodCronometro = this.CodigoMaquinaEstados + "." + this.Codigo;
                    CronometroRuntime.NuevoCronometro(this.CodCronometro, "Comprobación transición " + this.Nombre, "Comprobación de la transición " + this.Nombre);
                }
                else
                {
                    throw new Exception("No se ha podido cargar la información de la transición " + codigo + " \r\nde la base de datos.");
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Fatal(ModulosControl.MaquinasEstado, this._Codigo, exception);
                throw new Exception("Imposible iniciar la transición " + this.Codigo);
            }
        }

        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Ejecuta la comprobación de condiciones
        /// </summary>
        /// <returns></returns>
        internal bool IniciarComprobacionCondiciones()
        {
            bool resultado = false;
            try
            {
                // Cronometramos la duración de la ejecución
                CronometroRuntime.Start(this.CodCronometro);

                // Comprobamos las condiciones de la transición
                resultado = this.ComprobarCondiciones();

                // Paramos el cronometro de la duración de la ejecución
                CronometroRuntime.Stop(this.CodCronometro);

                // Guardamos la traza de registros
                if (resultado)
                {
                    LogsRuntime.Info(ModulosControl.MaquinasEstado, this.Codigo, "Transición cumplida: " + this.Codigo + ". Duración: " + CronometroRuntime.DuracionUltimaEjecucion(this.CodCronometro).ToString());
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosControl.MaquinasEstado, this._Codigo, exception);
            }
            return resultado;
        }
        #endregion

        #region Método(s) virtual(es)

        /// <summary>
        /// Método donde se rellenará toda la información del estado
        /// </summary>
        public virtual void Inicializar()
        {
        }

        /// <summary>
        /// Método donde se espera la finalización
        /// </summary>
        public virtual void Finalizar()
        {
        }

        /// <summary>
        /// Método que se ha de heredar para comprobar si se cumple la transición
        /// </summary>
        /// <returns>Verdadero si se ha cumplido las condiciones de transición</returns>
        public virtual bool ComprobarCondiciones()
        {
            return true;
        }

        #endregion
    }

    /// <summary>
    /// Clase base de todas las transiciones de tipo universal.
    /// Se ejecutan independientemente del estado actual de la base de datos.
    /// </summary>
    public class TransicionUniversal : TransicionBase
    {
        #region Constructor(es)

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codigoMaquinaEstados">Código de la máquina de estados</param>
        /// <param name="codigo">Código de la transición</param>
        public TransicionUniversal(string codigoMaquinaEstados, string codigo, Escenario escenario)
            : base(codigoMaquinaEstados, codigo, escenario)
        {
            try
            {
                CodigoEstadoOrigen = "";
            }
            catch (Exception exception)
            {
                LogsRuntime.Fatal(ModulosControl.MaquinasEstado, this.Codigo, exception);
                throw new Exception("Imposible iniciar la transición universal " + this.Codigo);
            }
        }

        #endregion
    }

    /// <summary>
    /// Tipo de estado
    /// </summary>
    public enum TipoEstado
    {
        /// <summary>
        /// Estado simple
        /// </summary>
        EstadoSimple = 1,
        /// <summary>
        /// Estado que se ejecuta en thread
        /// </summary>
        EstadoAsincrono = 2
    }

    /// <summary>
    /// Tipo de transición
    /// </summary>
    public enum TipoTransicion
    {
        /// <summary>
        /// Transicion simple
        /// </summary>
        TransicionSimple = 1,
        /// <summary>
        /// Transicion que se ejecuta siempre, independientemente del estado actual
        /// </summary>
        TransicionUniversal = 2
    }

    #region Definición de delegado(s)
    /// <summary>
    /// Delegado que indica de la llegada de un mensaje de la máquina de estados para visualizarse en la monitorización
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void EstadoCambiado(object sender, EventStateChanged e);

    /// <summary>
    /// Parametros de retorno del evento que indica de la llegada de un mensaje de la máquina de estados para visualizarse en la monitorización
    /// </summary>
    public class EventStateChanged : EventArgs
    {
        #region Atributo(s)
        /// <summary>
        /// Tipo de mensaje
        /// </summary>
        public string CodEstado;
        /// <summary>
        /// Momento en el que se ha producido el mensaje
        /// </summary>
        public DateTime Momento;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="tipo">Tipo de mensaje</param>
        /// <param name="informacion">Texto de la información a visualizar</param>
        /// <param name="hora">Momento en el que se ha producido el mensaje</param>
        public EventStateChanged(string codEstado, DateTime momento)
        {
            this.CodEstado = codEstado;
            this.Momento = momento;
        }
        #endregion
    }

    /// <summary>
    /// Delegado que indica de la llegada de un mensaje de la máquina de estados para visualizarse en la monitorización
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void MensajeMaquinaEstadosLanzado(object sender, EventMessageRaised e);

    /// <summary>
    /// Parametros de retorno del evento que indica de la llegada de un mensaje de la máquina de estados para visualizarse en la monitorización
    /// </summary>
    public class EventMessageRaised : EventArgs
    {
        #region Atributo(s)
        /// <summary>
        /// Tipo de mensaje
        /// </summary>
        public TipoMensajeMaquinaEstados Tipo;
        /// <summary>
        /// Texto de la información a visualizar
        /// </summary>
        public string Informacion;
        /// <summary>
        /// Momento en el que se ha producido el mensaje
        /// </summary>
        public DateTime Momento;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="tipo">Tipo de mensaje</param>
        /// <param name="informacion">Texto de la información a visualizar</param>
        /// <param name="hora">Momento en el que se ha producido el mensaje</param>
        public EventMessageRaised(TipoMensajeMaquinaEstados tipo, string informacion, DateTime momento)
        {
            this.Tipo = tipo;
            this.Informacion = informacion;
            this.Momento = momento;
        }
        #endregion
    }

    /// <summary>
    /// Enumerado del tipo de mensaje que la máquina de estados envía a la monitorización
    /// </summary>
    public enum TipoMensajeMaquinaEstados
    {
        CambioEstado,
        CondicionesTransicion,
        Informacion,
        Warning,
        Parada
    }
    #endregion

}