//***********************************************************************
// Assembly         : Orbita.Controles.VA
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 16-11-2012
// Description      : Movido al proyecto Orbita.Controles.VA
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Orbita.VAComun;
using Orbita.VAControl;

namespace Orbita.Controles.VA
{
    /// <summary>
    /// Control para la visualización de una máquina de estados
    /// </summary>
    public partial class OVisorMaquinaEstados : UserControl
    {
        #region Atributo(s)

        /// <summary>
        /// Código de la máquina de estados
        /// </summary>
        public string Codigo;

        /// <summary>
        /// Modo de apertura del control
        /// </summary>
        private ModoAperturaFormulario ModoAperturaFormulario;

        /// <summary>
        /// Variable de la máquina de estados visual
        /// </summary>
        private OMaquinaEstadosVisual MaquinaEstadosVisual;

        /// <summary>
        /// Momento en el que se produjo el último refresco de las variables
        /// </summary>
        DateTime MomentoUltimoRefresco;

        #endregion

        #region Constructor(es)

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OVisorMaquinaEstados()
        {
            InitializeComponent();
        } 

        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Método que inicializa las propiedades internas
        /// </summary>
        public void Inicializar(string codigo, ModoAperturaFormulario modoAperturaFormulario, bool online)
        {
            // Inicializamos el timer de refresco
            this.MomentoUltimoRefresco = DateTime.Now;
            this.timerRefresco.Interval = Convert.ToInt32(OVariablesManager.CadenciaMonitorizacion.TotalMilliseconds);

            // Inicializar variables
            this.MaquinaEstadosVisual = new OMaquinaEstadosVisual(codigo, online);
            this.MaquinaEstadosVisual.OnEstadoMovido = this.ProcEstadoMovido;
            this.MaquinaEstadosVisual.OnEstadoSeleccionado = this.ProcEstadoSeleccionado;
            this.MaquinaEstadosVisual.OnTransicionSeleccionada = this.ProcTransicionSeleccionada;
            this.MaquinaEstadosVisual.OnMensajeMaquinaEstados = this.ProcMensajeMaquinaEstados;
            this.MaquinaEstadosVisual.OnEstadoCambiado = this.ProcEstadoCambiado;

            this.Codigo = codigo;
            this.ModoAperturaFormulario = modoAperturaFormulario;

            this.MaquinaEstadosVisual.Inicializar();

            // Nos suscribimos a la recepción de mensajes
            OMaquinasEstadosManager.CrearSuscripcionMensajes(this.Codigo, this.ProcMensajeMaquinaEstados);

            // Nos suscribimos a los cambios de la variable de estado actual
            OMaquinasEstadosManager.EliminarSuscripcionCambioEstado(this.Codigo, this.RefrescarEstadoActual);
            if (this.ModoAperturaFormulario == ModoAperturaFormulario.Monitorizacion)
            {
                OMaquinasEstadosManager.CrearSuscripcionCambioEstado(this.Codigo, this.RefrescarEstadoActual);

                this.RefrescarEstadoActual();
            }

            this.Refresh();
        }

        /// <summary>
        /// Finaliza el control
        /// </summary>
        public void Finalizar()
        {
            this.timerRefresco.Enabled = false;
            OMaquinasEstadosManager.EliminarSuscripcionCambioEstado(this.Codigo, this.RefrescarEstadoActual);
            OMaquinasEstadosManager.EliminarSuscripcionMensajes(this.Codigo, this.ProcMensajeMaquinaEstados);
        }

        #endregion

        #region Eventos
        /// <summary>
        /// Evento de carga del formulario
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void CtrlStateMachineDisplay_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                try
                {

                }
                catch (Exception exception)
                {
                    OVALogsManager.Error(OModulosControl.MonitorizacionMaquinasEstado, this.Name, exception);
                }
            }
        }

        /// <summary>
        /// Refresca la visualización del estado actual
        /// </summary>
        private void RefrescarEstadoActual(object sender, OEventStateChanged e)
        {
            try
            {
                //Llamada al evento de procedimiento cambiado
                this.ProcEstadoCambiado(sender, e);

                // Refrescamos el layout si ha pasado un tiempo prudencial para no saturar la cpu
                TimeSpan tiempoSinRefrescar = DateTime.Now - MomentoUltimoRefresco;
                if (tiempoSinRefrescar > OVariablesManager.CadenciaMonitorizacion)
                {
                    // Si hace más de x tiempo que se refresco, volvemos a referescar
                    this.RefrescarEstadoActual(e.CodEstado);
                }
                else
                {
                    // Si hace menos de x tiempo que se refresco, ponemos un timer
                    if (!this.timerRefresco.Enabled)
                    {
                        this.timerRefresco.Enabled = true;
                    }
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosControl.MonitorizacionMaquinasEstado, this.Name, exception);
            }
        }

        /// <summary>
        /// Refresca la visualización del estado actual
        /// </summary>
        private void RefrescarEstadoActual()
        {
            try
            {
                string codEstadoActual = OMaquinasEstadosManager.GetEstadoActual(this.Codigo);
                this.RefrescarEstadoActual(codEstadoActual);
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosControl.MonitorizacionMaquinasEstado, this.Name, exception);
            }
        }

        /// <summary>
        /// Refresca la visualización del estado actual
        /// </summary>
        private void RefrescarEstadoActual(string codEstado)
        {
            try
            {
                // Reseteamos el timer para que no se vuelva a refrescar
                this.MomentoUltimoRefresco = DateTime.Now;
                this.timerRefresco.Enabled = false;                

                foreach (OEstadoVisual estado in this.MaquinaEstadosVisual.ListaEstados)
                {
                    estado.EnEjecucion = (estado.Codigo == codEstado);
                }
                this.picMaquinaEstados.Refresh();
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosControl.MonitorizacionMaquinasEstado, this.Name, exception);
            }
        }

        /// <summary>
        /// Evento generado al refrescar la visualización
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void picMaquinaEstados_Paint(object sender, PaintEventArgs e)
        {
            if (!this.DesignMode)
            {
                try
                {
                    this.MaquinaEstadosVisual.Pinta(e.Graphics);
                }
                catch (Exception exception)
                {
                    OVALogsManager.Error(OModulosControl.MonitorizacionMaquinasEstado, this.Name, exception);
                }
            }
        }

        /// <summary>
        /// Evento de mouse down para realizar cambios en la disposición del formulario (solo en modo modificar)
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void picMaquinaEstados_MouseDown(object sender, MouseEventArgs e)
        {
            if (!this.DesignMode)
            {
                try
                {
                    if (this.ModoAperturaFormulario == ModoAperturaFormulario.Modificacion)
                    {
                        bool hayMovimiento = this.MaquinaEstadosVisual.MouseDown(e.Location);
                        if (hayMovimiento)
                        {
                            this.Refresh();
                        }
                    }
                }
                catch (Exception exception)
                {
                    OVALogsManager.Error(OModulosControl.MonitorizacionMaquinasEstado, this.Name, exception);
                }
            }
        }

        /// <summary>
        /// Evento de mouse move para realizar cambios en la disposición del formulario (solo en modo modificar)
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void picMaquinaEstados_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.DesignMode)
            {
                try
                {
                    if (this.ModoAperturaFormulario == ModoAperturaFormulario.Modificacion)
                    {
                        bool hayMovimiento = this.MaquinaEstadosVisual.MouseMove(e.Location);
                        if (hayMovimiento)
                        {
                            this.MaquinaEstadosVisual.Inicializar();
                            this.Refresh();
                        }
                    }
                }
                catch (Exception exception)
                {
                    OVALogsManager.Error(OModulosControl.MonitorizacionMaquinasEstado, this.Name, exception);
                }
            }
        }

        /// <summary>
        /// Evento de mouse up para realizar cambios en la disposición del formulario (solo en modo modificar)
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void picMaquinaEstados_MouseUp(object sender, MouseEventArgs e)
        {
            if (!this.DesignMode)
            {
                try
                {

                    if (this.ModoAperturaFormulario == ModoAperturaFormulario.Modificacion)
                    {
                        bool hayMovimiento = this.MaquinaEstadosVisual.MouseUp(e.Location);
                        if (hayMovimiento)
                        {
                            this.MaquinaEstadosVisual.Inicializar();
                            this.Refresh();
                        }
                    }
                }
                catch (Exception exception)
                {
                    OVALogsManager.Error(OModulosControl.MonitorizacionMaquinasEstado, this.Name, exception);
                }
            }
        }

        /// <summary>
        /// Evento de mouse doubleclic para seleccionar una forma (solo en modo edición)
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void picMaquinaEstados_DoubleClick(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                try
                {
                    if (this.ModoAperturaFormulario == ModoAperturaFormulario.Modificacion)
                    {
                        Point pos = picMaquinaEstados.PointToClient(MousePosition);
                        bool hayMovimiento = this.MaquinaEstadosVisual.MouseDoubleClic(pos);
                        if (hayMovimiento)
                        {
                            this.Refresh();
                        }
                    }
                }
                catch (Exception exception)
                {
                    OVALogsManager.Error(OModulosControl.MonitorizacionMaquinasEstado, this.Name, exception);
                }
            }
        }

        /// <summary>
        /// Se dispara cuando un estado ha sido cambiado de posición
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcEstadoMovido(object sender, EventStateMoved e)
        {
            try
            {
                if (ModoAperturaFormulario == ModoAperturaFormulario.Modificacion)
                {
                    if (this.OnEstadoMovido != null)
                    {
                        this.OnEstadoMovido(sender, e);
                    }
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosControl.MonitorizacionMaquinasEstado, this.Name, exception);
            }
        }

        /// <summary>
        /// Se dispara cuando un estado ha sido seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcEstadoSeleccionado(object sender, EventStateSelected e)
        {
            try
            {
                if (ModoAperturaFormulario == ModoAperturaFormulario.Modificacion)
                {
                    if (this.OnEstadoSeleccionado != null)
                    {
                        this.OnEstadoSeleccionado(sender, e);
                    }
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosControl.MonitorizacionMaquinasEstado, this.Name, exception);
            }
        }

        /// <summary>
        /// Se dispara cuando una transición ha sido seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcTransicionSeleccionada(object sender, EventTransitionSelected e)
        {
            try
            {
                if (ModoAperturaFormulario == ModoAperturaFormulario.Modificacion)
                {
                    if (this.OnTransicionSeleccionada != null)
                    {
                        this.OnTransicionSeleccionada(sender, e);
                    }
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosControl.MonitorizacionMaquinasEstado, this.Name, exception);
            }
        }

        /// <summary>
        /// Se ejecuta el evento de cambio de estado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcMensajeMaquinaEstados(object sender, OEventMessageRaised e)
        {
            try
            {
                if (ModoAperturaFormulario == ModoAperturaFormulario.Monitorizacion)
                {
                    if (this.OnMensajeMaquinaEstados != null)
                    {
                        this.OnMensajeMaquinaEstados(sender, e);
                    }
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosControl.MonitorizacionMaquinasEstado, this.Name, exception);
            }
        }

        /// <summary>
        /// Se ejecuta el evento de cambio de estado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcEstadoCambiado(object sender, OEventStateChanged e)
        {
            try
            {
                if (ModoAperturaFormulario == ModoAperturaFormulario.Monitorizacion)
                {
                    if (this.OnEstadoCambiado != null)
                    {
                        this.OnEstadoCambiado(sender, e);
                    }
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosControl.MonitorizacionMaquinasEstado, this.Name, exception);
            }
        }

        /// <summary>
        /// Timer de refresco del estado actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerRefresco_Tick(object sender, EventArgs e)
        {
            try
            {
                this.RefrescarEstadoActual();
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosControl.MonitorizacionMaquinasEstado, this.Name, exception);
            }
        }

        #endregion

        #region Definición de delegado(s)
        /// <summary>
        /// Evento que indica el cambio de posición en un estado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [Category("Orbita")]
        [Description("Se dispara cuando un estado ha sido cambiado de posición.")]
        public event EstadoMovido OnEstadoMovido;

        /// <summary>
        /// Evento que indica la selección de un estado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [Category("Orbita")]
        [Description("Se dispara cuando un estado ha sido seleccionado.")]
        public event EstadoSeleccionado OnEstadoSeleccionado;

        /// <summary>
        /// Evento que indica la selección de una transición
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [Category("Orbita")]
        [Description("Se dispara cuando una transición ha sido seleccionada.")]
        public event TransicionSeleccionada OnTransicionSeleccionada;

        /// <summary>
        /// Evento que indica de la llegada de un mensaje de la máquina de estados para visualizarse en la monitorización
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [Category("Orbita")]
        [Description("Se dispara cuando se recibe un mensaje de la máquina de estados.")]
        public event OMensajeMaquinaEstadosLanzado OnMensajeMaquinaEstados;

        /// <summary>
        /// Evento que indica el cambio de estado en la máquina de estados para visualizarse en la monitorización
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [Category("Orbita")]
        [Description("Se dispara cuando se produce un cambio de estado en la máquina de estados.")]
        public event OEstadoCambiado OnEstadoCambiado; 
        #endregion
    }

    /// <summary>
    /// Máquina de estados heredada de la base con el fin de dotarla de visualización
    /// </summary>
    internal class OMaquinaEstadosVisual
    {
        #region Atributo(s)
        /// <summary>
        /// Código de la clase
        /// </summary>
        public string Codigo;

        /// <summary>
        /// Lista de estados visuales
        /// </summary>
        public List<OEstadoVisual> ListaEstados;

        /// <summary>
        /// Lista de transiciones visuales
        /// </summary>
        public List<OTransicionVisual> ListaTransiciones;

        /// <summary>
        /// Especifica si se ha de trabajar en modo online o offline
        /// </summary>
        public bool Online;

        /// <summary>
        /// Máquina de estados asociada
        /// </summary>
        public OMaquinaEstadosBase MaquinaEstadosAsociada;
        #endregion

        #region Constructor(es)

        public OMaquinaEstadosVisual(string codigo, bool online)
        {
            this.ListaEstados = new List<OEstadoVisual>();
            this.ListaTransiciones = new List<OTransicionVisual>();

            // Cargamos valores de la base de datos
            try
            {
                this.Codigo = codigo;
                this.Online = online;

                this.MaquinaEstadosAsociada = null;
                if (online)
                {
                    this.MaquinaEstadosAsociada = OMaquinasEstadosManager.GetMaquinaEstados(this.Codigo);
                }
                
                // Cargamos los valores de los estados
                DataTable dtEstados = Orbita.VAControl.AppBD.GetEstados(this.Codigo);
                if (dtEstados.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtEstados.Rows)
                    {
                        string codEstado = dr["codEstado"].ToString();
                        OEstadoBase estadoAsociado = null;
                        if (online)
                        {
                            estadoAsociado = this.MaquinaEstadosAsociada.GetEstado(codEstado);
                        }

                        this.ListaEstados.Add(new OEstadoVisual(this.Codigo, codEstado, this.Online, estadoAsociado));
                    }
                }

                // Cargamos los valores de las transiciones
                DataTable dtTransiciones = Orbita.VAControl.AppBD.GetTransiciones(this.Codigo);
                if (dtTransiciones.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTransiciones.Rows)
                    { 
                        string codTransicion = dr["codTransicion"].ToString();
                        OTransicionBase transcionAsociada = null;
                        if (online)
                        {
                            transcionAsociada = this.MaquinaEstadosAsociada.GetTransicion(codTransicion);
                        }

                        this.ListaTransiciones.Add(new OTransicionVisual(this.Codigo, codTransicion, this.Online, transcionAsociada, this.ListaEstados));
                    }
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosControl.MonitorizacionMaquinasEstado, this.Codigo, exception);
            }
        }

        #endregion

        #region Método(s) público(s)
        internal void Pinta(Graphics g)
        {
            foreach (OEstadoVisual estado in this.ListaEstados)
            {
                estado.Pinta(g);
            } 
            foreach (OTransicionVisual transicion in this.ListaTransiciones)
            {
                transicion.Pinta(g);
            }
        }

        /// <summary>
        /// Inicializala máquina de estados
        /// </summary>
        public void Inicializar()
        {
            foreach (OEstadoVisual estado in this.ListaEstados)
            {
                estado.Inicializar();
            }
            foreach (OTransicionVisual transicion in this.ListaTransiciones)
            {
                transicion.Inicializar();
            }
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Evento de mosue down
        /// </summary>
        /// <param name="point"></param>
        internal bool MouseDown(Point position)
        {
            bool clicDentro = false;

            foreach (OEstadoVisual estado in this.ListaEstados)
            {
                if (!clicDentro && estado.PositionInsideShape(position))
                {
                    estado.InicioMovimiento(position);
                    clicDentro = true;
                }
                else
                {
                    estado.FinMovimiento(position);
                }
            }

            if (!clicDentro)
            {
                foreach (OTransicionVisual transicion in this.ListaTransiciones)
                {
                    if (!clicDentro && transicion.PositionInsideShape(position))
                    {
                        //transicion.InicioMovimiento(position);
                        clicDentro = true;
                    }
                    else
                    {
                        //transicion.FinMovimiento(position);
                    }
                }
            }

            return clicDentro;
        }

        /// <summary>
        /// Evento de mouse move para realizar cambios en la disposición del formulario (solo en modo modificar)
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evento</param>
        internal bool MouseMove(Point position)
        {
            bool hayMovmiento = false;

            foreach (OEstadoVisual estado in this.ListaEstados)
            {
                hayMovmiento = estado.SeguirMovimiento(position);
                if (hayMovmiento)
                {
                    return true;
                }
            }

            foreach (OTransicionVisual transicion in this.ListaTransiciones)
            {
                //hayMovmiento = transicion.SeguirMovimiento(position);
                //if (hayMovmiento)
                //{
                //    return true;
                //}
            }

            foreach (OTransicionVisual transicion in this.ListaTransiciones)
            {
                hayMovmiento = transicion.SeguirMovimientoTexto(position);
                if (hayMovmiento)
                {
                    return true;
                }
            }

            return hayMovmiento;
        }

        /// <summary>
        /// Evento de mouse up para realizar cambios en la disposición del formulario (solo en modo modificar)
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evento</param>
        internal bool MouseUp(Point position)
        {
            bool hayMovmiento = false;

            foreach (OEstadoVisual estado in this.ListaEstados)
            {
                hayMovmiento = estado.FinMovimiento(position);
                if (hayMovmiento)
                {
                    if (this.OnEstadoMovido != null)
                    {
                        this.OnEstadoMovido(this, new EventStateMoved(estado.Codigo, estado.Localizacion));
                    }

                    return true;
                }
            }

            return hayMovmiento;
        }

        /// <summary>
        /// Evento de doubleclic
        /// </summary>
        /// <param name="position">Posición del clic</param>
        internal bool MouseDoubleClic(Point position)
        {
            bool hayMovmiento = false;

            foreach (OEstadoVisual estado in this.ListaEstados)
            {
                hayMovmiento = estado.PositionInsideShape(position);
                if (hayMovmiento)
                {
                    if (this.OnEstadoSeleccionado != null)
                    {
                        this.OnEstadoSeleccionado(this, new EventStateSelected(estado.Codigo));
                    }

                    return true;
                }
            }

            foreach (OTransicionVisual transicion in this.ListaTransiciones)
            {
                hayMovmiento = transicion.PositionInsideShape(position);
                if (hayMovmiento)
                {
                    if (this.OnTransicionSeleccionada != null)
                    {
                        this.OnTransicionSeleccionada(this, new EventTransitionSelected(transicion.Codigo));
                    }

                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Definición de delegado(s)
        /// <summary>
        /// Delegado que indica el cambio de posición en un estado
        /// </summary>
        public EstadoMovido OnEstadoMovido;
        /// <summary>
        /// Delegado que indica la selección de un estado
        /// </summary>
        public EstadoSeleccionado OnEstadoSeleccionado;
        /// <summary>
        /// Delegado que indica la selección de una transición
        /// </summary>
        public TransicionSeleccionada OnTransicionSeleccionada;
        /// <summary>
        /// Evento que indica de la llegada de un mensaje de la máquina de estados para visualizarse en la monitorización
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public OMensajeMaquinaEstadosLanzado OnMensajeMaquinaEstados;
        /// <summary>
        /// Evento que indica el cambio de estado en la máquina de estados para visualizarse en la monitorización
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public OEstadoCambiado OnEstadoCambiado; 
        #endregion
    }

    /// <summary>
    /// Clase estado heredada de la base con el fin de dotarla de visualización
        /// </summary>
    internal class OEstadoVisual
    {
        #region Atributo(s)
        /// <summary>
        /// Código de la clase
        /// </summary>
        public string Codigo;

        /// <summary>
        /// Nombre de la clase
        /// </summary>
        public string Nombre;

        /// <summary>
        /// Código de la máquina de estados
        /// </summary>
        public string CodigoMaquinaEstados;

        /// <summary>
        /// Especifica si se ha de trabajar en modo online o offline
        /// </summary>
        public bool Online;

        /// <summary>
        /// Estado asociado
        /// </summary>
        public OEstadoBase EstadoAsociado;

        /// <summary>
        /// Localización de la forma
        /// </summary>
        public Point Localizacion;

        /// <summary>
        /// Posición donde el usuario ha realizado el click
        /// </summary>
        private Point PosicionClick;

        /// <summary>
        /// Indica si el shape está siendo movido por el usuario
        /// </summary>
        private bool EnMovimiento = false;

        /// <summary>
        /// Forma visual
        /// </summary>
        public RoundRectangleShape Shape;

        #endregion

        #region Propiedad(es)
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
        #endregion

        #region Constructor(es)

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="_estadoAsociado">Estado asociado a la visualización</param>
        public OEstadoVisual(string codigoMaquinaEstados, string codigo, bool online, OEstadoBase estadoAsociado)
        {
            this.CodigoMaquinaEstados = codigoMaquinaEstados;
            this.Codigo = codigo;
            this.Online = online;
            this.EstadoAsociado = estadoAsociado;

            // Cargamos valores de la base de datos
            DataTable dtEstado = Orbita.VAControl.AppBD.GetEstado(this.CodigoMaquinaEstados, this.Codigo);
            if (dtEstado.Rows.Count == 1)
            {
                this.Nombre = dtEstado.Rows[0]["NombreEstado"].ToString();

                if (OEnteroRobusto.IsNumericInt(dtEstado.Rows[0]["PosX"]))
                {
                    this.Localizacion.X = (int)dtEstado.Rows[0]["PosX"];
                }
                if (OEnteroRobusto.IsNumericInt(dtEstado.Rows[0]["PosY"]))
                {
                    this.Localizacion.Y = (int)dtEstado.Rows[0]["PosY"];
                }
            }
        } 

        #endregion

        #region Método(s) público(s)
        public void Pinta(Graphics g)
        {
            Shape.Draw(this.EnEjecucion, this.EnMovimiento, g);
        }

        /// <summary>
        /// Informa si una posición XY está dentro del shape
        /// </summary>
        /// <param name="position">Indica la posición que se quiere comprobar</param>
        /// <returns>Verdadero si la posición está dentro del shape</returns>
        internal bool PositionInsideShape(Point position)
        {
            return this.Shape.IsPositionInsideShape(position);
        }

        /// <summary>
        /// Inicia el desplazamiento de la forma
        /// </summary>
        /// <param name="position">Posición del ratón</param>
        internal void InicioMovimiento(Point position)
        {
            if (!this.EnMovimiento)
            {
                this.EnMovimiento = true;
                this.PosicionClick = new Point(position.X - this.Localizacion.X, position.Y - this.Localizacion.Y);
            }
        }

        /// <summary>
        /// Seguir el desplazamiento de la forma
        /// </summary>
        /// <param name="position">Posición del ratón</param>
        internal bool SeguirMovimiento(Point position)
        {
            bool hayMovimiento = false;

            if (this.EnMovimiento)
            {
                Point nuevaPosicionClick = new Point(position.X - this.Localizacion.X, position.Y - this.Localizacion.Y);
                this.Localizacion.X += nuevaPosicionClick.X - this.PosicionClick.X;
                this.Localizacion.Y += nuevaPosicionClick.Y - this.PosicionClick.Y;

                hayMovimiento = true;
            }
            return hayMovimiento;
        }

        /// <summary>
        /// Finaliza el desplazamiento de la forma
        /// </summary>
        /// <param name="position">Posición del ratón</param>
        internal bool FinMovimiento(Point position)
        {
            bool hayMovimiento = false;

            if (this.EnMovimiento)
            {
                Point nuevaPosicionClick = new Point(position.X - this.Localizacion.X, position.Y - this.Localizacion.Y);
                this.Localizacion.X += nuevaPosicionClick.X - this.PosicionClick.X;
                this.Localizacion.Y += nuevaPosicionClick.Y - this.PosicionClick.Y;

                this.EnMovimiento = false;
                hayMovimiento = true;
            }
            return hayMovimiento;
        }

        /// <summary>
        /// Método donde se rellenará toda la información del estado
        /// </summary>
        public void Inicializar()
        {
            // Creamos las variables de visualización
            this.Shape = new RoundRectangleShape(this.Localizacion, this.Nombre, false);
        } 
        #endregion
    }

    /// <summary>
    /// Clase transición heredada de la base con el fin de dotarla de visualización
    /// </summary>
    internal class OTransicionVisual
    {
        #region Atributo(s)

        /// <summary>
        /// Código de la clase
        /// </summary>
        public string Codigo;

        /// <summary>
        /// Nombre de la clase
        /// </summary>
        public string Nombre;

        /// <summary>
        /// Código de la máquina de estados
        /// </summary>
        public string CodigoMaquinaEstados;

        /// <summary>
        /// Especifica si se ha de trabajar en modo online o offline
        /// </summary>
        public bool Online;

        /// <summary>
        /// Transicion asociada
        /// </summary>
        public OTransicionBase TransicionAsociada;

        /// <summary>
        /// Estado origen
        /// </summary>
        public OEstadoVisual EstadoOrigen;

        /// <summary>
        /// Estado origen
        /// </summary>
        public OEstadoVisual EstadoDestino;

        /// <summary>
        /// Forma visual
        /// </summary>
        private ITransitionShape Shape;

        /// <summary>
        /// Posición de la flecha relativa al estado origen
        /// </summary>
        public PosicionFlecha PosicionFlechaEstadoOrigen;

        /// <summary>
        /// Posición de la flecha relativa al estado destino
        /// </summary>
        public PosicionFlecha PosicionFlechaEstadoDestino;

        /// <summary>
        /// Posición origen de la flecha
        /// </summary>
        public Point PosicionOrigen;

        /// <summary>
        /// Posición destino de la flecha
        /// </summary>
        public Point PosicionDestino;

        /// <summary>
        /// Puntos que definen la posición del texto a visualizar
        /// </summary>
        public Point LocalizacionTexto;

        /// <summary>
        /// Posición donde el usuario ha realizado el click
        /// </summary>
        private Point PosicionClick;

        /// <summary>
        /// Indica si el shape está siendo movido por el usuario
        /// </summary>
        private bool EnMovimiento = false;

        /// <summary>
        /// Indica el tipo de movimiento que se está realiando en la forma
        /// </summary>
        private TipoMovimientoArrowShape TipoMovimiento;

        #endregion

        #region Constructor(es)

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="_estadoAsociado">Estado asociado a la visualización</param>
        public OTransicionVisual(string codigoMaquinaEstados, string codigo, bool online, OTransicionBase transicionAsociada, List<OEstadoVisual> listaEstados)
        {
            this.CodigoMaquinaEstados = codigoMaquinaEstados;
            this.Codigo = codigo;
            this.Online = online;
            this.TransicionAsociada = transicionAsociada;

            // Cargamos valores de la base de datos
            DataTable dtTransicion = Orbita.VAControl.AppBD.GetTransicion(this.CodigoMaquinaEstados, this.Codigo);
            if (dtTransicion.Rows.Count == 1)
            {
                this.Nombre = dtTransicion.Rows[0]["NombreTransicion"].ToString();
                
                if (OEnteroRobusto.IsNumericInt(dtTransicion.Rows[0]["PosicionFlechaEstadoOrigen"]) && 
                   (OEnteroRobusto.IsNumericInt(dtTransicion.Rows[0]["PosicionFlechaEstadoDestino"])))
                {
                    // Se busca la posición origen
                    string codigoEstadoOrigen = dtTransicion.Rows[0]["CodigoEstadoOrigen"].ToString();
                    this.EstadoOrigen = listaEstados.Find(delegate(OEstadoVisual e) { return e.Codigo == codigoEstadoOrigen; });
                    this.PosicionFlechaEstadoOrigen = (PosicionFlecha)dtTransicion.Rows[0]["PosicionFlechaEstadoOrigen"];

                    // Se busca la posición destino
                    string codigoEstadoDestino = dtTransicion.Rows[0]["CodigoEstadoDestino"].ToString();
                    this.EstadoDestino = listaEstados.Find(delegate(OEstadoVisual e) { return e.Codigo == codigoEstadoDestino; });
                    this.PosicionFlechaEstadoDestino = (PosicionFlecha)dtTransicion.Rows[0]["PosicionFlechaEstadoDestino"];
                }
            }
        }

        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Calcula posición XY relativa al estado
        /// </summary>
        /// <param name="posicionFlecha"></param>
        /// <param name="estadoAsociado"></param>
        /// <returns></returns>
        private Point CalcularPosicionRelativaAEstado(PosicionFlecha posicionFlecha, OEstadoVisual estadoAsociado)
        {
            Point resultado = new Point();

            switch (posicionFlecha)
            {
                case PosicionFlecha.Superior:
                    resultado.X = estadoAsociado.Shape.Rect.Location.X + estadoAsociado.Shape.Rect.Size.Width / 2;
                    resultado.Y = estadoAsociado.Shape.Rect.Location.Y;
                    break;
                case PosicionFlecha.Inferior:
                    resultado.X = estadoAsociado.Shape.Rect.Location.X + estadoAsociado.Shape.Rect.Size.Width / 2;
                    resultado.Y = estadoAsociado.Shape.Rect.Location.Y + estadoAsociado.Shape.Rect.Size.Height;
                    break;
                case PosicionFlecha.Derecha:
                    resultado.X = estadoAsociado.Shape.Rect.Location.X + estadoAsociado.Shape.Rect.Size.Width;
                    resultado.Y = estadoAsociado.Shape.Rect.Location.Y + estadoAsociado.Shape.Rect.Size.Height / 2;
                    break;
                case PosicionFlecha.Izquierda:
                    resultado.X = estadoAsociado.Shape.Rect.Location.X;
                    resultado.Y = estadoAsociado.Shape.Rect.Location.Y + estadoAsociado.Shape.Rect.Size.Height / 2;
                    break;
            }

            return resultado;
        }
        #endregion

        #region Método(s) público(s)
        public void Pinta(Graphics g)
        {
            Shape.Draw(this.EnMovimiento, g);
        }

        /// <summary>
        /// Informa si una posición XY está dentro del shape
        /// </summary>
        /// <param name="position">Indica la posición que se quiere comprobar</param>
        /// <returns>Verdadero si la posición está dentro del shape</returns>
        internal bool PositionInsideShape(Point position)
        {
            return this.Shape.IsPositionInsideShape(position);
        }

        /// <summary>
        /// Inicia el desplazamiento de la forma
        /// </summary>
        /// <param name="position">Posición del ratón</param>
        internal void InicioMovimientoTexto(Point position)
        {
            if (!this.EnMovimiento)
            {
                this.EnMovimiento = true;
                this.TipoMovimiento = TipoMovimientoArrowShape.MovimientoTexto;
                this.PosicionClick = new Point(position.X - this.LocalizacionTexto.X, position.Y - this.LocalizacionTexto.Y);
            }
        }

        /// <summary>
        /// Seguir el desplazamiento de la forma
        /// </summary>
        /// <param name="position">Posición del ratón</param>
        internal bool SeguirMovimientoTexto(Point position)
        {
            bool hayMovimiento = false;

            if (this.EnMovimiento && (this.TipoMovimiento == TipoMovimientoArrowShape.MovimientoTexto))
            {
                Point nuevaPosicionClick = new Point(position.X - this.LocalizacionTexto.X, position.Y - this.LocalizacionTexto.Y);
                this.LocalizacionTexto.X += nuevaPosicionClick.X - this.PosicionClick.X;
                this.LocalizacionTexto.Y += nuevaPosicionClick.Y - this.PosicionClick.Y;

                hayMovimiento = true;
            }
            return hayMovimiento;
        }

        /// <summary>
        /// Finaliza el desplazamiento de la forma
        /// </summary>
        /// <param name="position">Posición del ratón</param>
        internal bool FinMovimientoTexto(Point position)
        {
            bool hayMovimiento = false;

            if (this.EnMovimiento && (this.TipoMovimiento == TipoMovimientoArrowShape.MovimientoTexto))
            {
                Point nuevaPosicionClick = new Point(position.X - this.LocalizacionTexto.X, position.Y - this.LocalizacionTexto.Y);
                this.LocalizacionTexto.X += nuevaPosicionClick.X - this.PosicionClick.X;
                this.LocalizacionTexto.Y += nuevaPosicionClick.Y - this.PosicionClick.Y;

                this.EnMovimiento = false;
                hayMovimiento = true;
            }
            return hayMovimiento;
        }

        /// <summary>
        /// Método donde se rellenará toda la información del estado
        /// </summary>
        public void Inicializar()
        {
            if ((this.EstadoOrigen == null) && this.EstadoDestino is OEstadoVisual) // Transición universal
            {
                // Creamos las variables de visualización
                this.PosicionDestino.X = this.EstadoDestino.Shape.Rect.Location.X + this.EstadoDestino.Shape.Rect.Size.Width / 2;
                this.PosicionDestino.Y = this.EstadoDestino.Shape.Rect.Location.Y;

                this.PosicionOrigen = Point.Add(this.PosicionDestino, new Size(0, -80));

                this.LocalizacionTexto.X = (this.PosicionOrigen.X + this.PosicionDestino.X) / 2;
                this.LocalizacionTexto.Y = (this.PosicionOrigen.Y + this.PosicionDestino.Y) / 2;

                this.Shape = new ArrowShape(this.PosicionOrigen, this.PosicionDestino, this.LocalizacionTexto, this.Nombre);
            }
            else if ((this.EstadoOrigen is OEstadoVisual) && (this.EstadoDestino is OEstadoVisual)) // Transición de un estado a otro
            {
                if (this.EstadoOrigen.Codigo != this.EstadoDestino.Codigo)
                {
                    // Creamos las variables de visualización
                    this.PosicionOrigen = this.CalcularPosicionRelativaAEstado(this.PosicionFlechaEstadoOrigen, this.EstadoOrigen);
                    this.PosicionDestino = this.CalcularPosicionRelativaAEstado(this.PosicionFlechaEstadoDestino, this.EstadoDestino);

                    this.LocalizacionTexto.X = (this.PosicionOrigen.X + this.PosicionDestino.X) / 2;
                    this.LocalizacionTexto.Y = (this.PosicionOrigen.Y + this.PosicionDestino.Y) / 2;

                    this.Shape = new ArrowShape(this.PosicionOrigen, this.PosicionDestino, this.LocalizacionTexto, this.Nombre);
                }
                else // Transición entre el mismo estado
                {
                    this.PosicionOrigen.X = this.EstadoOrigen.Shape.Rect.Location.X + this.EstadoOrigen.Shape.Rect.Size.Width / 4;
                    this.PosicionOrigen.Y = this.EstadoOrigen.Shape.Rect.Location.Y + this.EstadoOrigen.Shape.Rect.Size.Height;

                    this.PosicionDestino.X = this.EstadoOrigen.Shape.Rect.Location.X + this.EstadoOrigen.Shape.Rect.Size.Width / 4;
                    this.PosicionDestino.Y = this.EstadoOrigen.Shape.Rect.Location.Y;

                    this.LocalizacionTexto.X = this.EstadoOrigen.Shape.Rect.Location.X - this.EstadoOrigen.Shape.Rect.Size.Width / 4;
                    this.LocalizacionTexto.Y = (this.PosicionOrigen.Y + this.PosicionDestino.Y) / 2;

                    this.Shape = new CircleArrowShape(this.PosicionOrigen, this.PosicionDestino, this.LocalizacionTexto, this.Nombre);
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// Visualización gráfica de un estado
    /// </summary>
    internal class RoundRectangleShape
    {
        #region Atributo(s)
        /// <summary>
        /// Texto a mostrar
        /// </summary>
        private string Texto;

        /// <summary>
        /// Constante que define el tamaño del cuadro
        /// </summary>
        private Size Tamaño;

        /// <summary>
        /// Define la posición rectangular de la forma
        /// </summary>
        public Rectangle Rect;

        /// <summary>
        /// Define la forma en la que se visualiza el roundredct del estado
        /// </summary>
        public GraphicsPath ShapePath;

        /// <summary>
        /// Define la forma en la que se visualiza la sombra del roundrect del estado
        /// </summary>
        private GraphicsPath ShadowShapePath;

        /// <summary>
        /// Objecto Pen que define la característica de la línea exterior
        /// </summary>
        private Pen Pen;

        /// <summary>
        /// Objecto Pen que define la característica del rectangulo exterior de selección
        /// </summary>
        private Pen SelectionPen;

        /// <summary>
        /// Objecto Pen que define la característica del rellenado interior de la forma
        /// </summary>        
        private LinearGradientBrush Brush;

        /// <summary>
        /// Objecto Pen que define la característica del rellenado interior de la sombra
        /// </summary>        
        private SolidBrush BrushSombra;

        /// <summary>
        /// Objecto Pen que define la característica del rellenado del rectangulo de selección
        /// </summary>        
        private SolidBrush SelectionBrush;

        /// <summary>
        /// Define el tipo de fuente del texto que se escribirá en la forma
        /// </summary>
        private Font ActiveFont;

        /// <summary>
        /// Define el tipo de fuente del texto que se escribirá en la forma cuando se esté en el modo desactivado
        /// </summary>
        private Font InactiveFont;

        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="localizacion">Posición donde visualizarse</param>
        public RoundRectangleShape(Point localizacion, string texto, bool enMovimiento)
        {
            this.Texto = texto;
            this.Tamaño = new Size(190, 60);
            this.Rect = new Rectangle(Convert.ToInt32(localizacion.X - this.Tamaño.Width / 2),
                               Convert.ToInt32(localizacion.Y - this.Tamaño.Height / 2),
                               this.Tamaño.Width,
                               this.Tamaño.Height);

            this.Brush = new LinearGradientBrush(this.Rect, Color.FromArgb(211, 220, 239), Color.FromArgb(255, 255, 255), 0.0);
            this.BrushSombra = new SolidBrush(Color.FromArgb(216, 216, 216));
            this.ActiveFont = new Font("Arial", 8, FontStyle.Underline);
            this.InactiveFont = new Font("Arial", 8, FontStyle.Regular);
            this.Pen = new Pen(Color.FromArgb(113, 111, 100));
            this.SelectionPen = new Pen(Color.Black, 3);
            this.SelectionBrush = new SolidBrush(Color.FromArgb(128, 128, 128, 128));

            this.ShapePath = this.GetRoundedRect(this.Rect, 15);
            this.ShadowShapePath = this.GetRoundedRect(new Rectangle(this.Rect.X + 5, this.Rect.Y + 5, this.Rect.Width, this.Rect.Height), 15);
        }
        #endregion

        #region Método(s) privado(s)

        /// <summary>
        /// Define la forma roundRectangle a visualizar
        /// </summary>
        /// <param name="baseRect">Área base donde visualizar la forma</param>
        /// <param name="radius">Radio de los bordes</param>
        /// <returns>Objeto GraphicsPath donde se almacen la forma roundRectangle</returns>
        private GraphicsPath GetRoundedRect(RectangleF baseRect, float radius)
        {
            // if corner radius is less than or equal to zero, 
            // return the original rectangle 
            if (radius <= 0.0F)
            {
                GraphicsPath mPath = new GraphicsPath();
                mPath.AddRectangle(baseRect);
                mPath.CloseFigure();
                return mPath;
            }

            // if the corner radius is greater than or equal to 
            // half the width, or height (whichever is shorter) 
            // then return a capsule instead of a lozenge 
            if (radius >= (Math.Min(baseRect.Width, baseRect.Height)) / 2.0)
                return GetCapsule(baseRect);

            // create the arc for the rectangle sides and declare 
            // a graphics path object for the drawing 
            float diameter = radius * 2.0F;
            SizeF sizeF = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(baseRect.Location, sizeF);
            GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();

            // top left arc 
            path.AddArc(arc, 180, 90);

            // top right arc 
            arc.X = baseRect.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc 
            arc.Y = baseRect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc
            arc.X = baseRect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Define la forma roundRectangle a visualizar con las esquinas por defecto
        /// </summary>
        /// <param name="baseRect">Área base donde visualizar la forma</param>
        /// <returns>Objeto GraphicsPath donde se almacen la forma roundRectangle</returns>
        private GraphicsPath GetCapsule(RectangleF baseRect)
        {
            float diameter;
            RectangleF arc;
            GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            try
            {
                if (baseRect.Width > baseRect.Height)
                {
                    // return horizontal capsule 
                    diameter = baseRect.Height;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 90, 180);
                    arc.X = baseRect.Right - diameter;
                    path.AddArc(arc, 270, 180);
                }
                else if (baseRect.Width < baseRect.Height)
                {
                    // return vertical capsule 
                    diameter = baseRect.Width;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 180, 180);
                    arc.Y = baseRect.Bottom - diameter;
                    path.AddArc(arc, 0, 180);
                }
                else
                {
                    // return circle 
                    path.AddEllipse(baseRect);
                }
            }
            catch
            {
                path.AddEllipse(baseRect);
            }
            finally
            {
                path.CloseFigure();
            }
            return path;
        } 
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Dibuja la forma RoundRectangle
        /// </summary>
        /// <param name="active">Indica si hay que dibujar la forma en su estado activo o inactivo</param>
        /// <param name="text">Texto a dibujar en el centro de la forma</param>
        /// <param name="g">Objeto Graphics donde visualizar la forma</param>
        public void Draw(bool active, bool selected, Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            SizeF tamañoTexto;
            Rectangle rectTexto = new Rectangle();

            g.FillPath(this.BrushSombra, this.ShadowShapePath);
            g.FillPath(this.Brush, this.ShapePath);

            if (active)
            {
                tamañoTexto = g.MeasureString(this.Texto, ActiveFont);
                rectTexto.Width = this.Rect.Width;
                rectTexto.Height = this.Rect.Height;
                rectTexto.X = Convert.ToInt32(this.Rect.X + (this.Rect.Width - tamañoTexto.Width) / 2);
                rectTexto.Y = Convert.ToInt32(this.Rect.Y + (this.Rect.Height - tamañoTexto.Height) / 2);
                g.DrawString(this.Texto, this.ActiveFont, Brushes.Black, rectTexto);
                g.FillEllipse(Brushes.Red, this.Rect.X + 10, rectTexto.Y - 10, 12, 12);
                g.DrawEllipse(Pens.Black, this.Rect.X + 10, rectTexto.Y - 10, 12, 12);
            }
            else
            {
                tamañoTexto = g.MeasureString(this.Texto, InactiveFont);
                rectTexto.Width = this.Rect.Width;
                rectTexto.Height = this.Rect.Height;
                rectTexto.X = Convert.ToInt32(this.Rect.X + (this.Rect.Width - tamañoTexto.Width) / 2);
                rectTexto.Y = Convert.ToInt32(this.Rect.Y + (this.Rect.Height - tamañoTexto.Height) / 2);
                g.DrawString(this.Texto, this.InactiveFont, Brushes.Black, rectTexto);
            }

            if (selected)
            {
                g.DrawRectangle(this.SelectionPen, this.Rect);
                g.FillRectangle(this.SelectionBrush, this.Rect);
            }

            this.ShapePath.FillMode = FillMode.Alternate;
            g.DrawPath(this.Pen, this.ShapePath);
        }

        /// <summary>
        /// Informa si una posición XY está dentro del shape
        /// </summary>
        /// <param name="position">Indica la posición que se quiere comprobar</param>
        /// <returns>Verdadero si la posición está dentro del shape</returns>
        public bool IsPositionInsideShape(Point position)
        {
            if (this.ShapePath.IsVisible(position))
            {
                return true;
            }
            return false;
        }

    #endregion
    }

    /// <summary>
    /// Visualización gráfica de una transición
    /// </summary>
    internal class ArrowShape : ITransitionShape
    {
        #region Atributo(s)

        /// <summary>
        /// Texto a mostrar
        /// </summary>
        private string Texto;

        /// <summary>
        /// Posición de la flecha en el estado origen
        /// </summary>
        Point PosicionEstadoOrigen;

        /// <summary>
        /// Posición de la flecha en el estado destino
        /// </summary>
        Point PosicionEstadoDestino;

        /// <summary>
        /// Puntos que definen la posición del texto a visualizar
        /// </summary>
        Point LocalizacionTexto;

        /// <summary>
        /// Línea de la flecha
        /// </summary>
        GraphicsPath RutaFlechaGrafica;

        /// <summary>
        /// Rectangulo que define la posición del texto
        /// </summary>
        GraphicsPath PathTexto;

        /// <summary>
        /// Define el tipo de fuente del texto que se escribirá en la forma
        /// </summary>
        private Font Font;

        /// <summary>
        /// Objecto Pen que define la característica de la línea exterior
        /// </summary>
        private Pen Pen;

        /// <summary>
        /// Objecto Pen que define la característica del rectangulo exterior de selección
        /// </summary>
        private Pen SelectionPen;

        /// <summary>
        /// Objecto Pen que define la característica del rellenado del rectangulo de selección
        /// </summary>        
        private SolidBrush SelectionBrush;

        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="localizacion">Posición donde visualizarse</param>
        public ArrowShape(Point posicionEstadoOrigen, Point posicionEstadoDestino, Point localizacionTexto, string text)
        {
            this.PosicionEstadoOrigen = posicionEstadoOrigen;
            this.PosicionEstadoDestino = posicionEstadoDestino;
            this.LocalizacionTexto = localizacionTexto;
            this.Texto = text;

            this.RutaFlechaGrafica = GetArrowLine(posicionEstadoOrigen, posicionEstadoDestino);

            this.Pen = new Pen(Color.FromArgb(113, 111, 100));
            this.Pen.Width = 1;

            this.Font = new Font("Arial", 8, FontStyle.Regular);

            this.SelectionPen = new Pen(Color.Black, 3);
            this.SelectionBrush = new SolidBrush(Color.FromArgb(128, 128, 128, 128));
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Devuelve una linea con forma de flecha
        /// </summary>
        /// <param name="initialPoint"></param>
        /// <param name="finalPoint"></param>
        /// <returns></returns>
        private GraphicsPath GetArrowLine(Point initialPoint, Point finalPoint)
        {
            int trngSize = 20;
            double angulo = Math.Atan2(initialPoint.Y - finalPoint.Y, initialPoint.X - finalPoint.X);
            Point mp1 = new Point(finalPoint.X + Convert.ToInt32(trngSize * Math.Cos(angulo - 0.2)), finalPoint.Y + Convert.ToInt32(trngSize * Math.Sin(angulo - 0.2)));
            Point mp2 = new Point(finalPoint.X + Convert.ToInt32(trngSize * Math.Cos(angulo + 0.2)), finalPoint.Y + Convert.ToInt32(trngSize * Math.Sin(angulo + 0.2)));

            GraphicsPath path = new GraphicsPath();
            path.AddLine(initialPoint, finalPoint);
            path.AddLine(mp1, finalPoint);
            path.AddLine(mp2, finalPoint);
            return path;
        } 
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Dibuja la forma RoundRectangle
        /// </summary>
        /// <param name="active">Indica si hay que dibujar la forma en su estado activo o inactivo</param>
        /// <param name="text">Texto a dibujar en el centro de la forma</param>
        /// <param name="g">Objeto Graphics donde visualizar la forma</param>
        public void Draw(bool selectedText, Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawPath(this.Pen, this.RutaFlechaGrafica);

            SizeF tamañoTexto = g.MeasureString(this.Texto, this.Font);
            Point localizacionTexto = new Point(this.LocalizacionTexto.X - (int)(tamañoTexto.Width / 2), this.LocalizacionTexto.Y - (int)(tamañoTexto.Height / 2));
            g.DrawString(this.Texto, this.Font, Brushes.Black, localizacionTexto);

            // Guardo la posición del texto
            Rectangle rectTexto = new Rectangle();
            rectTexto.Width = Convert.ToInt32(tamañoTexto.Width);
            rectTexto.Height = Convert.ToInt32(tamañoTexto.Height);
            rectTexto.X = this.LocalizacionTexto.X;
            rectTexto.Y = this.LocalizacionTexto.Y;
            this.PathTexto = new GraphicsPath();
            this.PathTexto.AddRectangle(rectTexto);

            // Dibujamos la selección
            if (selectedText)
            {
                g.DrawRectangle(this.SelectionPen, rectTexto);
                g.FillRectangle(this.SelectionBrush, rectTexto);
            }
        }

        /// <summary>
        /// Informa si una posición XY está dentro del shape
        /// </summary>
        /// <param name="position">Indica la posición que se quiere comprobar</param>
        /// <returns>Verdadero si la posición está dentro del shape</returns>
        public bool IsPositionInsideShape(Point position)
        {
            Pen penClick = new Pen(Color.Black, 5);

            if (this.RutaFlechaGrafica.IsOutlineVisible(position, penClick))
            {
                //MessageBox.Show("dentro transicion");
                return true;
            }
            return false;
        }
        #endregion
    }

    /// <summary>
    /// Visualización gráfica de una transición
    /// </summary>
    internal class CircleArrowShape : ITransitionShape
    {
        #region Atributo(s)

        /// <summary>
        /// Texto a mostrar
        /// </summary>
        private string Texto;

        /// <summary>
        /// Posición de la flecha en el estado origen
        /// </summary>
        Point PosicionEstadoOrigen;

        /// <summary>
        /// Posición de la flecha en el estado destino
        /// </summary>
        Point PosicionEstadoDestino;

        /// <summary>
        /// Puntos que definen la posición del texto a visualizar
        /// </summary>
        Point LocalizacionTexto;

        /// <summary>
        /// Línea de la flecha
        /// </summary>
        GraphicsPath RutaFlechaGrafica;

        /// <summary>
        /// Rectangulo que define la posición del texto
        /// </summary>
        GraphicsPath PathTexto;

        /// <summary>
        /// Define el tipo de fuente del texto que se escribirá en la forma
        /// </summary>
        private Font Font;

        /// <summary>
        /// Objecto Pen que define la característica de la línea exterior
        /// </summary>
        private Pen Pen;

        /// <summary>
        /// Objecto Pen que define la característica del rectangulo exterior de selección
        /// </summary>
        private Pen SelectionPen;

        /// <summary>
        /// Objecto Pen que define la característica del rellenado del rectangulo de selección
        /// </summary>        
        private SolidBrush SelectionBrush;

        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="localizacion">Posición donde visualizarse</param>
        public CircleArrowShape(Point posicionEstadoOrigen, Point posicionEstadoDestino, Point localizacionTexto, string text)
        {
            this.PosicionEstadoOrigen = posicionEstadoOrigen;
            this.PosicionEstadoDestino = posicionEstadoDestino;
            this.LocalizacionTexto = localizacionTexto;
            this.Texto = text;

            this.RutaFlechaGrafica = GetArrowCircle(posicionEstadoOrigen, posicionEstadoDestino);

            this.Pen = new Pen(Color.FromArgb(113, 111, 100));
            this.Pen.Width = 1;

            this.Font = new Font("Arial", 8, FontStyle.Regular);

            this.SelectionPen = new Pen(Color.Black, 3);
            this.SelectionBrush = new SolidBrush(Color.FromArgb(128, 128, 128, 128));
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Devuelve una linea con forma de flecha
        /// </summary>
        /// <param name="initialPoint"></param>
        /// <param name="finalPoint"></param>
        /// <returns></returns>
        private GraphicsPath GetArrowCircle(Point initialPoint, Point finalPoint)
        {
            Point[] puntos = new Point[6];
            puntos[0] = this.PosicionEstadoOrigen;
            puntos[1] = Point.Add(this.PosicionEstadoOrigen, new Size(-8, 30));
            puntos[2] = Point.Add(this.PosicionEstadoOrigen, new Size(-80, 20));
            puntos[3] = Point.Add(this.PosicionEstadoDestino, new Size(-80, -20));
            puntos[4] = Point.Add(this.PosicionEstadoDestino, new Size(-8, -30));
            puntos[5] = this.PosicionEstadoDestino;

            GraphicsPath path = new GraphicsPath();
            path.AddCurve(puntos);


            int trngSize = 20;
            Point arrowInitialPoint = Point.Add(finalPoint, new Size(0, -5));

            double angulo = Math.Atan2(arrowInitialPoint.Y - finalPoint.Y, arrowInitialPoint.X - finalPoint.X);
            Point mp1 = new Point(finalPoint.X + Convert.ToInt32(trngSize * Math.Cos(angulo - 0.2)), finalPoint.Y + Convert.ToInt32(trngSize * Math.Sin(angulo - 0.2)));
            Point mp2 = new Point(finalPoint.X + Convert.ToInt32(trngSize * Math.Cos(angulo + 0.2)), finalPoint.Y + Convert.ToInt32(trngSize * Math.Sin(angulo + 0.2)));
            path.AddLine(mp1, finalPoint);
            path.AddLine(mp2, finalPoint);

            return path;
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Dibuja la forma RoundRectangle
        /// </summary>
        /// <param name="active">Indica si hay que dibujar la forma en su estado activo o inactivo</param>
        /// <param name="text">Texto a dibujar en el centro de la forma</param>
        /// <param name="g">Objeto Graphics donde visualizar la forma</param>
        public void Draw(bool selectedText, Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawPath(this.Pen, this.RutaFlechaGrafica);

            SizeF tamañoTexto = g.MeasureString(this.Texto, this.Font);
            Point localizacionTexto = new Point(this.LocalizacionTexto.X - (int)(tamañoTexto.Width / 2), this.LocalizacionTexto.Y - (int)(tamañoTexto.Height / 2));
            g.DrawString(this.Texto, this.Font, Brushes.Black, localizacionTexto);

            // Guardo la posición del texto
            Rectangle rectTexto = new Rectangle();
            rectTexto.Width = Convert.ToInt32(tamañoTexto.Width);
            rectTexto.Height = Convert.ToInt32(tamañoTexto.Height);
            rectTexto.X = this.LocalizacionTexto.X;
            rectTexto.Y = this.LocalizacionTexto.Y;
            this.PathTexto = new GraphicsPath();
            this.PathTexto.AddRectangle(rectTexto);

            // Dibujamos la selección
            if (selectedText)
            {
                g.DrawRectangle(this.SelectionPen, rectTexto);
                g.FillRectangle(this.SelectionBrush, rectTexto);
            }
        }

        /// <summary>
        /// Informa si una posición XY está dentro del shape
        /// </summary>
        /// <param name="position">Indica la posición que se quiere comprobar</param>
        /// <returns>Verdadero si la posición está dentro del shape</returns>
        public bool IsPositionInsideShape(Point position)
        {
            Pen penClick = new Pen(Color.Black, 5);

            if (this.RutaFlechaGrafica.IsOutlineVisible(position, penClick))
            {
                //MessageBox.Show("dentro transicion");
                return true;
            }
            return false;
        }
        #endregion
    }

    /// <summary>
    /// Interfaz de las formas visualizadas
    /// </summary>
    internal interface ITransitionShape
    {
        #region Método(s) público(s)
        /// <summary>
        /// Dibuja la forma RoundRectangle
        /// </summary>
        /// <param name="active">Indica si hay que dibujar la forma en su estado activo o inactivo</param>
        /// <param name="text">Texto a dibujar en el centro de la forma</param>
        /// <param name="g">Objeto Graphics donde visualizar la forma</param>
        void Draw(bool selectedText, Graphics g);

        /// <summary>
        /// Informa si una posición XY está dentro del shape
        /// </summary>
        /// <param name="position">Indica la posición que se quiere comprobar</param>
        /// <returns>Verdadero si la posición está dentro del shape</returns>
        bool IsPositionInsideShape(Point position);
        #endregion
    }


    /// <summary>
    /// Posición incial de la flecha relativa al estado
    /// </summary>
    internal enum PosicionFlecha
    {
        Superior = 0,
        Inferior = 1,
        Derecha = 2,
        Izquierda = 3
    }

    /// <summary>
    /// Informa del tipo de movimiento de la flecha
    /// </summary>
    internal enum TipoMovimientoArrowShape
    {
        MovimientoFlecha,
        MovimientoTexto
    }

    /// <summary>
    /// Delegado que indica el cambio de posición en un estado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void EstadoMovido(object sender, EventStateMoved e);

    /// <summary>
    /// Parametros de retorno del evento de cambio de posición en un estado
    /// </summary>
    public class EventStateMoved: EventArgs
    {
        #region Atributo(s)
        /// <summary>
        /// Código del estado visual
        /// </summary>
        public string CodEstado;
        /// <summary>
        /// Posición del estado visual
        /// </summary>
        public Point Posicion; 
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codEstado">Código del estado visual</param>
        /// <param name="posicion">Posición del estado visual</param>
        public EventStateMoved(string codEstado, Point posicion)
        {
            this.CodEstado = codEstado;
            this.Posicion = posicion;
        } 
        #endregion
    }

    /// <summary>
    /// Parametros de retorno del evento de cambio de posición en una transición
    /// </summary>
    public class EventTransitionTextMoved : EventArgs
    {
        #region Atributo(s)
        /// <summary>
        /// Código de la transición visual
        /// </summary>
        public string CodTransicion;
        /// <summary>
        /// Posición de la transición visual
        /// </summary>
        public Point Posicion;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codEstado">Código de la transición visual</param>
        /// <param name="posicion">Posición de la transición visual</param>
        public EventTransitionTextMoved(string CodTransicion, Point posicion)
        {
            this.CodTransicion = CodTransicion;
            this.Posicion = posicion;
        }
        #endregion
    }

    /// <summary>
    /// Delegado que indica la selección de un estado
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void EstadoSeleccionado(object sender, EventStateSelected e);

    /// <summary>
    /// Parametros de retorno del evento selección de un estado
    /// </summary>
    public class EventStateSelected : EventArgs
    {
        #region Atributo(s)
        /// <summary>
        /// Código del estado visual
        /// </summary>
        public string CodEstado;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codEstado">Código del estado visual</param>
        public EventStateSelected(string codEstado)
        {
            this.CodEstado = codEstado;
        }
        #endregion
    }

    /// <summary>
    /// Delegado que indica la selección de una transición
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TransicionSeleccionada(object sender, EventTransitionSelected e);

    /// <summary>
    /// Parametros de retorno del evento la selección de una transición
    /// </summary>
    public class EventTransitionSelected : EventArgs
    {
        #region Atributo(s)
        /// <summary>
        /// Código de la transición visual
        /// </summary>
        public string CodTransicion;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codEstado">Código de la transición visual</param>
        public EventTransitionSelected(string CodTransicion)
        {
            this.CodTransicion = CodTransicion;
        }
        #endregion
    }
}
