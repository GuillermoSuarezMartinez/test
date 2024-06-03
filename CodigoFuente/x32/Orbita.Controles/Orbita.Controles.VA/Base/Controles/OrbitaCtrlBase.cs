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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Infragistics.Win.UltraWinDock;
using Infragistics.Win.UltraWinGrid;
using Orbita.Utiles;
using Orbita.VAComun;

namespace Orbita.Controles.VA
{
    /// <summary>
    /// Clase base para controles de formularios
    /// </summary>          
    public partial class OrbitaCtrlBase : UserControl, IOrbitaForm, IDisposable
    {
        #region Atributo(s)
        /// <summary>
        /// Indica si se ha modificado algo
        /// </summary>
        protected bool AlgoModificado;
        /// <summary>
        /// Indica si se está en la fase de inicializacion de los controles del formulario
        /// </summary>
        protected bool Inicio;
        /// <summary>
        /// Informa si el motivo del cierre es por decisión el usuario o interna del código
        /// </summary>
        protected bool CierrePorUsuario;
        #endregion Campos

        #region Propiedad(es)
        /// <summary>
        /// Posibilita la apertura de múltiples instancias del formulario
        /// </summary>
        private bool _MultiplesInstancias;
        /// <summary>
        /// Posibilita la apertura de múltiples instancias del formulario
        /// </summary>
        [Browsable(true),
        Category("Orbita"),
        Description("Posibilita la apertura de múltiples instancias del formulario")]
        public bool MultiplesInstancias
        {
            get { return _MultiplesInstancias; }
            set { _MultiplesInstancias = value; }
        }

        /// <summary>
        /// Se activa a verdadero cuando se han guardado correctamten los datos del formulario,
        /// bien en la base de datos o bien en formulario invocante.
        /// </summary>
        private bool _FormularioModificado;
        /// <summary>
        /// Se activa a verdadero cuando se han guardado correctamente los datos del formulario,
        /// bien en la base de datos o bien en formulario invocante.
        /// </summary>
        [Browsable(true),
        Category("Orbita"),
        Description("Indica que la información contenida en el formulario ha sido modificada o que debe ser modificada")]
        public bool FormularioModificado
        {
            get { return _FormularioModificado; }
        }

        /// <summary>
        /// Almacena el modo con el que se ha abierto el formulario: Nuevo o Modificar
        /// </summary>
        protected ModoAperturaFormulario _ModoAperturaFormulario;
        /// <summary>
        /// Indica el modo de apertura del formulario. Edicion, Visualizacion, Monitorizacion, Sistema
        /// </summary>
        [Browsable(true),
        Category("Orbita"),
        Description("Indica el modo de apertura del formulario. Edicion, Visualizacion, Monitorizacion, Sistema")]
        public ModoAperturaFormulario ModoAperturaFormulario
        {
            get { return _ModoAperturaFormulario; }
            set { _ModoAperturaFormulario = value; }
        }

        /// <summary>
        /// Indica si se ha de restaurar la posición del formulario a la última guardada
        /// </summary>
        [Browsable(true),
        Category("Orbita"),
        Description("Indica si se ha de restaurar la posición del formulario a la última guardada")]
        public bool RecordarPosicion
        {
            get { return false; }
            set { ; }
        }

        /// <summary>
        /// Muestra los botones de acción situados en la parte inferior del formulario
        /// </summary>
        private bool _MostrarBotones = true;

        /// <summary>
        /// Muestra los botones de acción situados en la parte inferior del formulario
        /// </summary>
        [Browsable(true),
        Category("Orbita"),
        Description("Muestra los botones de acción situados en la parte inferior del formulario")]
        public bool MostrarBotones
        {
            get { return _MostrarBotones; }
            set { _MostrarBotones = value; }
        }

        /// <summary>
        /// Nombre del formulario
        /// </summary>
        [Browsable(false)]
        public new string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                base.Name = value;
            }
        }

        /// <summary>
        /// Localización del formulario
        /// </summary>
        [Browsable(false)]
        public new Point Location
        {
            get
            {
                return base.Location;
            }
            set
            {
                base.Location = value;
            }
        }
        /// <summary>
        /// Tamaño del formulario
        /// </summary>
        [Browsable(false)]
        public new Size Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                base.Size = value;
            }
        }

        /// <summary>
        /// Indica que el formulario está maximizado
        /// </summary>
        [Browsable(false)]
        public bool Maximized
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        /// <summary>
        /// Indica que el formulario puede anclarse o no
        /// </summary>
        [Browsable(false)]
        public bool Anclable
        {
            get { return false; }
        }

        /// <summary>
        /// Indica que el formulario está anclado o no
        /// </summary>
        [Browsable(false)]
        public bool Anclado
        {
            get { return false; }
        }

        /// <summary>
        /// Código identificativo del cronómetro.
        /// </summary>
        [Browsable(false)]
        public bool IsDockedMDIChild
        {
            get { return false; }
            set { ;}
        }

        /// <summary>
        /// Rectangulo de visualización del fomrulario MDI de tipo anclable
        /// </summary>
        [Browsable(false)]
        public Rectangle DockedMDIRectangle
        {
            get { return new Rectangle(); }
            set { ;}
        }

        /// <summary>
        /// Rectangulo de visualización del fomrulario anclado
        /// </summary>
        [Browsable(false)]
        public Size DockedPaneSize
        {
            get { return new Size(); }
            set { ;}
        }

        /// <summary>
        /// Informa si la la ventana anclada está puesta para no oculatarse automáticamente
        /// </summary>
        [Browsable(false)]
        public bool DockedPined
        {
            get { return false; }
            set { ;}
        }

        /// <summary>
        /// Código identificativo del cronómetro.
        /// </summary>
        [Browsable(false)]
        public DockedLocation DockedLocation
        {
            get { return DockedLocation.DockedRight; }
            set { ;}
        }
        #endregion Propiedades

        #region Constructor(es)
        /// <summary>
        /// Constructor vacio de la clase (es necesario para que el diseñador construya el formulario heredado en tiempo de diseño)
        /// </summary>		
        public OrbitaCtrlBase()
        {
            InitializeComponent();

            // Inicialiación de campos
            this.AlgoModificado = false;
            this._FormularioModificado = false;
            this.CierrePorUsuario = false;
        }
        /// <summary>
        /// Constructor de la clase (No existe bloqueo de registro)
        /// </summary>
        /// <param name="modoFormulario">Modo de apertura del formulario (Nuevo, Visualizacion, Modificacion, Ninguno)</param>
        public OrbitaCtrlBase(Control contendor, ModoAperturaFormulario modoFormulario)
        {
            InitializeComponent();
            this._ModoAperturaFormulario = modoFormulario;
            this.Parent = contendor;

            // Inicialiación de campos
            this.AlgoModificado = false;
            this._FormularioModificado = false;
            this.CierrePorUsuario = false;
        }
        #endregion Constructores

        #region Destructores
        /// <summary>
        /// Destructor de la clase
        /// </summary>
        public new void Dispose()
        {
            base.Dispose();
            //GC.Collect();
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Informa de que el formulario puede abrirse, bien porque se permiten multiples intancias o bien porque no existe niguna instancia abierta
        /// </summary>
        /// <returns>Verdadero si el formulario puede abrirse</returns>
        private bool VisualizacionPermitida()
        {
            bool puedeAbrirse = true;

            // Si no permite abrir multiples instancias...
            if (!this._MultiplesInstancias)
            {
                foreach (string nombreForm in FrmBase.ListaFormsAbiertos)
                {
                    if (nombreForm == this.Name)
                    {
                        puedeAbrirse = false;
                        break;
                    }
                }
            }

            return puedeAbrirse;
        }

        /// <summary>
        /// Se añade el evento de monitorización a los controles del formulario
        /// </summary>
        private void InternoIniciarMonitorizarModificaciones(Control control)
        {
            foreach (Control controlInterno in control.Controls)
            {
                if (controlInterno != this.pnlInferiorPadre)
                {
                if (controlInterno is OrbitaComboPro)
                {
                    ((OrbitaComboPro)controlInterno).OrbCambiaValor += this.EventoCambioValor;
                }
                else if (controlInterno is OrbitaTextBox)
                {
                    ((OrbitaTextBox)controlInterno).TextChanged += this.EventoCambioValor;
                }
                else if (controlInterno is OrbitaNumberEditor)
                {
                    ((OrbitaNumberEditor)controlInterno).ValueChanged += this.EventoCambioValor;
                }
                else if (controlInterno is OrbitaCheckBox)
                {
                    ((OrbitaCheckBox)controlInterno).CheckedChanged += this.EventoCambioValor;
                }
                else if (controlInterno is OrbitaGridPro)
                {
                    ((OrbitaGridPro)controlInterno).OrbCeldaCambiaValor += this.EventoCeldaCambioValor;
                }
                else if (controlInterno is OrbitaDateTime)
                {
                    ((OrbitaDateTime)controlInterno).ValueChanged += this.EventoCambioValor;
                }

                // Recursivo
                this.InternoIniciarMonitorizarModificaciones(controlInterno);
            }
        }
        }

        /// <summary>
        /// Se elimina el evento de monitorización a los controles del formulario
        /// </summary>
        private void InternoFinalizarMonitorizarModificaciones(Control control)
        {
            foreach (Control controlInterno in control.Controls)
            {
                if (controlInterno != this.pnlInferiorPadre)
                {
                if (controlInterno is OrbitaComboPro)
                {
                    ((OrbitaComboPro)controlInterno).OrbCambiaValor -= this.EventoCambioValor;
                }
                else if (controlInterno is OrbitaTextBox)
                {
                    ((OrbitaTextBox)controlInterno).TextChanged -= this.EventoCambioValor;
                }
                else if (controlInterno is OrbitaNumberEditor)
                {
                    ((OrbitaNumberEditor)controlInterno).ValueChanged -= this.EventoCambioValor;
                }
                else if (controlInterno is OrbitaCheckBox)
                {
                    ((OrbitaCheckBox)controlInterno).CheckedChanged -= this.EventoCambioValor;
                }
                else if (controlInterno is OrbitaGridPro)
                {
                    ((OrbitaGridPro)controlInterno).OrbCeldaCambiaValor -= this.EventoCeldaCambioValor;
                }
                else if (controlInterno is OrbitaDateTime)
                {
                    ((OrbitaDateTime)controlInterno).ValueChanged -= this.EventoCambioValor;
                }

                // Recursivo
                this.InternoFinalizarMonitorizarModificaciones(controlInterno);
            }
        }
        }

        /// <summary>
        /// Método usado para solucionar el problemas de los componentes orbita con los tooltips
        /// </summary>
        private void SolucionarToolTips(Control control)
        {
            foreach (Control controlInterno in control.Controls)
            {
                if (controlInterno is OrbitaComboPro)
                {
                    this.OrbTooltip.SetToolTip(((OrbitaComboPro)controlInterno).OrbCombo, this.OrbTooltip.GetToolTip(((OrbitaComboPro)controlInterno)));
                }
                if (controlInterno is OrbitaGridPro)
                {
                    this.OrbTooltip.SetToolTip(((OrbitaGridPro)controlInterno).OrbGrid, this.OrbTooltip.GetToolTip(((OrbitaGridPro)controlInterno)));
                }

                // Recursivo
                this.SolucionarToolTips(controlInterno);
            }
        }
        /// <summary>
        /// Se ejecuta cuando se cierra el formulario, antes de cerrarlo
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private bool IntentaCerrar()
        {
            bool resultado = true;
            try
            {
                if ((this.ModoAperturaFormulario == ModoAperturaFormulario.Sistema) && (!this.CierrePorUsuario))
                {
                    resultado = false;
                }
                else
                {
                    this.btnCancelar.Focus();

                    if (this.ComprobarDatosModificados())
                    {
                        //Si hay datos modificados en el formulario, avisar y preguntar qué hacer
                        switch (OMensajes.MostrarPreguntaSiNoCancelar("¿Desea guardar los cambios realizados?", MessageBoxDefaultButton.Button3))
                        {
                            case DialogResult.Yes:
                                if (!this.GuardarDatos())
                                {
                                    resultado = false;
                                }
                                break;
                            case DialogResult.No:
                                this.AccionesNoGuardar();
                                //Cerrar el formulario, es decir, seguir con la ejecucion
                                break;
                            case DialogResult.Cancel:
                                resultado = false;
                                break;
                        }
                    }
                }
            }
            catch (System.Exception exception)
            {
                OVALogsManager.Error(OModulosSistema.Escritorios, "Cierre de formulario", exception);
            }

            return resultado;
        }

        /// <summary>
        /// Evento que se produce cuando se cierra el formulario, despues de cerrarlo
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void Salir()
        {
            this.FinalizarMonitorizarModificaciones();
            this.AccionesSalir();
            this.Visible = false;

            // Eliminamos el formulario de la lista de formularios abiertos
            FrmBase.ListaFormsAbiertos.Remove(this.Name);

            //this.Parent.Controls.Remove(this);
            //this.Parent = null;
        }
        /// <summary>
        /// Se establecen los controles como accesibles por el usuario dependiendo del modo
        /// </summary>
        private void InternoEstablecerModo(Control control)
        {
            foreach (Control controlInterno in control.Controls)
            {
                if (controlInterno is OrbitaComboPro)
                {
                    ((OrbitaComboPro)controlInterno).OrbCombo.ReadOnly = this.ModoAperturaFormulario == ModoAperturaFormulario.Visualizacion;
                }
                else if (controlInterno is OrbitaTextBox)
                {
                    ((OrbitaTextBox)controlInterno).ReadOnly = this.ModoAperturaFormulario == ModoAperturaFormulario.Visualizacion;
                }
                else if (controlInterno is OrbitaNumberEditor)
                {
                    ((OrbitaNumberEditor)controlInterno).ReadOnly = this.ModoAperturaFormulario == ModoAperturaFormulario.Visualizacion;
                }
                else if (controlInterno is OrbitaCheckBox)
                {
                    ((OrbitaCheckBox)controlInterno).Enabled = this.ModoAperturaFormulario != ModoAperturaFormulario.Visualizacion;
                }
                else if (controlInterno is OrbitaGridPro)
                {
                    ((OrbitaGridPro)controlInterno).OrbCeldaEditable = this.ModoAperturaFormulario != ModoAperturaFormulario.Visualizacion;
                }
                else if (controlInterno is OrbitaDateTime)
                {
                    ((OrbitaDateTime)controlInterno).ReadOnly = this.ModoAperturaFormulario == ModoAperturaFormulario.Visualizacion;
                }
                else if (controlInterno is OrbitaButton)
                {
                    ((OrbitaButton)controlInterno).Enabled = this.ModoAperturaFormulario != ModoAperturaFormulario.Visualizacion;
                }

                // Recursivo
                this.InternoEstablecerModo(controlInterno);
            }
        }
        #endregion Métodos privados

        #region Método(s) protegidos
        /// <summary>
        /// Inicializa todos los componentes del formulario, cargando los datos si procede
        /// </summary>
        protected void IniciarFormulario()
        {
            this.Inicio = true;
            this.btnCancelar.Select();

            this.CargarDatosComunes();

            switch (this._ModoAperturaFormulario)
            {
                case ModoAperturaFormulario.Nuevo:
                    this.CargarDatosModoNuevo();
                    this.EstablecerModoNuevo();
                    break;
                case ModoAperturaFormulario.Modificacion:
                    this.CargarDatosModoModificacion();
                    this.EstablecerModoModificacion();
                    break;
                case ModoAperturaFormulario.Visualizacion:
                    this.OcultarBotonCancelar();
                    this.CargarDatosModoVisualizacion();
                    this.EstablecerModoVisualizacion();
                    break;
                case ModoAperturaFormulario.Monitorizacion:
                    this.OcultarBotonAceptar();
                    this.CargarDatosModoMonitorizacion();
                    this.EstablecerModoMonitorizacion();
                    break;
                case ModoAperturaFormulario.Sistema:
                    this.OcultarBotones();
                    this.CargarDatosModoSistema();
                    this.EstablecerModoSistema();
                    break;
                default:
                    OVALogsManager.Error(OModulosSistema.Escritorios, "Inicio de formulario", "La ejecucion no debería pasar por este punto del código: switch/default");
                    break;
            }

            this.IniciarMonitorizarModificaciones();
            this.SolucionarToolTips(this);
            this.ResetDeteccionModificaciones();
            this.Inicio = false;
        }
        /// <summary>
        /// Guarda los datos
        /// </summary>
        protected bool GuardarDatos()
        {
            if (this.ComprobacionesDeCampos())
            {
                switch (this._ModoAperturaFormulario)
                {
                    case ModoAperturaFormulario.Nuevo:
                        //Llamada a la funcion de guardado de datos
                        if (!this.GuardarDatosModoNuevo())
                        {
                            //Si han habido errores...
                            return false;
                        }
                        //Si no han habido errores...
                        this.ResetDeteccionModificaciones();
                        return true;
                    case ModoAperturaFormulario.Modificacion:
                        //Llamada a la funcion de guardado de datos
                        if (!this.GuardarDatosModoModificacion())
                        {
                            //Si han habido errores...
                            return false;
                        }
                        //Si no han habido errores...
                        this.ResetDeteccionModificaciones();
                        return true;
                    case ModoAperturaFormulario.Visualizacion:
                        //Llamada a la funcion de guardado de datos
                        if (!this.GuardarDatosModoVisualizacion())
                        {
                            //Si han habido errores...
                            return false;
                        }
                        //Si no han habido errores...
                        this.ResetDeteccionModificaciones();
                        return true;
                    case ModoAperturaFormulario.Monitorizacion:
                        //Llamada a la funcion de guardado de datos
                        if (!this.GuardarDatosModoMonitorizacion())
                        {
                            //Si han habido errores...
                            return false;
                        }
                        //Si no han habido errores...
                        this.ResetDeteccionModificaciones();
                        return true;
                    case ModoAperturaFormulario.Sistema:
                        //Llamada a la funcion de guardado de datos
                        if (!this.GuardarDatosModoSistema())
                        {
                            //Si han habido errores...
                            return false;
                        }
                        //Si no han habido errores...
                        this.ResetDeteccionModificaciones();
                        return true;
                    default:
                        OVALogsManager.Error(OModulosSistema.Escritorios, "Inicio de formulario", "La ejecucion no debería pasar por este punto del código: switch/default");
                        return false;
                }
            }
            else //No se pueden guardar los datos por las restricciones impuestas
            {
                return false;
            }
        }
        /// <summary>
        /// Cambia el texto del botón aceptar
        /// </summary>
        /// <param name="nuevoTexto">Nuevo texto que tendrá el botón aceptar</param>
        protected void CambiarTextoBotonGuardar(string nuevoTexto)
        {
            this.btnGuardar.Text = nuevoTexto;
        }
        /// <summary>
        /// Cambia el texto del botón cancelar
        /// </summary>
        /// <param name="nuevoTexto">Nuevo texto que tendrá el botón cancelar</param>
        protected void CambiarTextoBotonCancelar(string nuevoTexto)
        {
            this.btnCancelar.Text = nuevoTexto;
        }
        /// <summary>
        /// Cambia la propiedad Visible del botón aceptar
        /// </summary>
        protected void OcultarBotonAceptar()
        {
            this.CambiarTextoBotonCancelar("Cerrar");
            this.btnGuardar.Visible = false;
        }
        /// <summary>
        /// Cambia la propiedad Visible del botón cancelar
        /// </summary>
        protected void OcultarBotonCancelar()
        {
            this.CambiarTextoBotonGuardar("Aceptar");
            this.btnCancelar.Visible = false;
                this.btnGuardar.Location = this.btnCancelar.Location;
                this.btnGuardar.Select();
            }
        /// <summary>
        /// Cambia la propiedad Visible de la barra de botones
        /// </summary>
        protected void OcultarBotones()
        {
            this.pnlInferiorPadre.Visible = this._MostrarBotones;
            this.OcultarBotonCancelar();
            this.OcultarBotonAceptar();
        }
        /// <summary>
        /// Establece el estilo de los botones de la barra de título del formulario
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                if (this._ModoAperturaFormulario == ModoAperturaFormulario.Sistema)
                {
                    myCp.ClassStyle = myCp.ClassStyle | 0x200;
                }
                return myCp;
            }
        }
        #endregion Métodos protegidos

        #region Método(s) virtual(es)
        /// <summary>
        /// Carga y muestra datos del formulario comunes para los modos de funcionamiento
        /// </summary>
        protected virtual void CargarDatosComunes()
        {
        }
        /// <summary>
        /// Carga y muestra datos del formulario en modo edición. Se cargan todos datos que se muestran en 
        /// el formulario: grids, combos, etc... Cada carga de elementos estará encapsulada en un método
        /// </summary>
        protected virtual void CargarDatosModoNuevo()
        {
        }
        /// <summary>
        /// Carga y muestra datos del formulario en modo Modificación. Se cargan todos datos que se muestran en 
        /// el formulario: grids, combos, etc... Cada carga de elementos estará encapsulada en un método
        /// </summary>
        protected virtual void CargarDatosModoModificacion()
        {
        }
        /// <summary>
        /// Carga y muestra datos del formulario en modo visualización. Se cargan todos datos que se muestran en 
        /// el formulario: grids, combos, etc... Cada carga de elementos estará encapsulada en un método
        /// </summary>
        protected virtual void CargarDatosModoVisualizacion()
        {
        }
        /// <summary>
        /// Carga y muestra datos del formulario en modo Monitorización. Se cargan todos datos que se muestran en 
        /// el formulario: grids, combos, etc... Cada carga de elementos estará encapsulada en un método
        /// </summary>
        protected virtual void CargarDatosModoMonitorizacion()
        {
        }
        /// <summary>
        /// Carga y muestra datos del formulario en modo sistema. Se cargan todos datos que se muestran en 
        /// el formulario: grids, combos, etc... Cada carga de elementos estará encapsulada en un método
        /// </summary>
        protected virtual void CargarDatosModoSistema()
        {
        }
        /// <summary>
        /// Establece la habiliacion adecuada de los controles para el modo Nuevo
        /// </summary>
        protected virtual void EstablecerModoNuevo()
        {
            this.SuspendLayout();
            this.ChkToolTip.Checked = false;
            this.OrbTooltip.Active = false;
            this.InternoEstablecerModo(this.pnlPanelPrincipalPadre);
            this.ResumeLayout();
        }
        /// <summary>
        /// Establece la habiliacion adecuada de los controles para el modo Modificacion
        /// </summary>
        protected virtual void EstablecerModoModificacion()
        {
            this.SuspendLayout();
            this.ChkToolTip.Checked = false;
            this.OrbTooltip.Active = false;
            this.InternoEstablecerModo(this.pnlPanelPrincipalPadre);
            this.ResumeLayout();
        }
        /// <summary>
        /// Establece la habiliacion adecuada de los controles para el modo visualizacion
        /// </summary>
        protected virtual void EstablecerModoVisualizacion()
        {
            this.SuspendLayout();
            this.ChkToolTip.Checked = false;
            this.OrbTooltip.Active = false;
            this.InternoEstablecerModo(this.pnlPanelPrincipalPadre);
            this.ResumeLayout();
        }
        /// <summary>
        /// Establece la habiliacion adecuada de los controles para el modo monitorización
        /// </summary>
        protected virtual void EstablecerModoMonitorizacion()
        {
            this.SuspendLayout();
            this.ChkToolTip.Checked = false;
            this.OrbTooltip.Active = false;
            this.InternoEstablecerModo(this.pnlPanelPrincipalPadre);
            this.ResumeLayout();
        }
        /// <summary>
        /// Establece la habiliacion adecuada de los controles para el modo sistema
        /// </summary>
        protected virtual void EstablecerModoSistema()
        {
            this.SuspendLayout();
            this.ChkToolTip.Checked = false;
            this.OrbTooltip.Active = false;
            this.InternoEstablecerModo(this.pnlPanelPrincipalPadre);
            this.ResumeLayout();
        }
        /// <summary>
        /// Guarda los datos cuando el formulario está abierto en modo Creación
        /// </summary>
        /// <returns>True si la operación de guardado de datos ha tenido éxito; false en caso contrario</returns>
        protected virtual bool GuardarDatosModoNuevo()
        {
            this._FormularioModificado = true;
            return true;
        }
        /// <summary>
        /// Guarda los datos cuando el formulario está abierto en modo Modificación
        /// </summary>
        /// <returns>True si la operación de guardado de datos ha tenido éxito; false en caso contrario</returns>
        protected virtual bool GuardarDatosModoModificacion()
        {
            this._FormularioModificado = true;
            return true;
        }
        /// <summary>
        /// Guarda los datos cuando el formulario está abierto en modo visualizacion
        /// </summary>
        /// <returns>True si la operación de guardado de datos ha tenido éxito; false en caso contrario</returns>
        protected virtual bool GuardarDatosModoVisualizacion()
        {
            return true;
        }
        /// <summary>
        /// Guarda los datos cuando el formulario está abierto en modo monitorización
        /// </summary>
        /// <returns>True si la operación de guardado de datos ha tenido éxito; false en caso contrario</returns>
        protected virtual bool GuardarDatosModoMonitorizacion()
        {
            return true;
        }
        /// <summary>
        /// Guarda los datos cuando el formulario está abierto en modo sistema
        /// </summary>
        /// <returns>True si la operación de guardado de datos ha tenido éxito; false en caso contrario</returns>
        protected virtual bool GuardarDatosModoSistema()
        {
            return true;
        }
        /// <summary>
        /// Se añade el evento de monitorización a los controles del formulario
        /// </summary>
        protected virtual void IniciarMonitorizarModificaciones()
        {
            this.InternoIniciarMonitorizarModificaciones(this);
        }
        /// <summary>
        /// Se elimina el evento de monitorización a los controles del formulario
        /// </summary>
        protected virtual void FinalizarMonitorizarModificaciones()
        {
            this.InternoFinalizarMonitorizarModificaciones(this);
        }
        /// <summary>
        /// Comprueba si se ha modificado algún dato en algún campo del formulario
        /// </summary>
        /// <returns>True si se han modificado datos; false en caso contrario</returns>
        protected virtual bool ComprobarDatosModificados()
        {
            if (this.AlgoModificado)
                return true;

            return false;
        }
        /// <summary>
        /// Resetea el mecanismo de modificaciones indicando que se han procesado los cambios
        /// </summary>
        protected virtual void ResetDeteccionModificaciones()
        {
            this.AlgoModificado = false;
        }
        /// <summary>
        /// Realiza las comprobaciones pertinentes antes de realizar un guardado de los datos. Se usa para el caso en que hayan restricciones en el momento de guardar los datos
        /// </summary>
        /// <returns>True si todo está correcto para ser guardado; false en caso contrario</returns>
        protected virtual bool ComprobacionesDeCampos()
        {
            return true;
        }
        /// <summary>
        ///  Método para que sea sobreescrito en el hijo y realizar funciones si el usuario no desea guardar los cambios 
        /// </summary>
        protected virtual void AccionesNoGuardar()
        {

        }
        /// <summary>
        ///  Método  para que sea sobreescrito en el hijo y realizar funciones al salir del formulario
        /// </summary>
        protected virtual void AccionesSalir()
        {
        }
        #endregion Métodos virtuales

        #region Método(s) heredado(s)
        /// <summary>
        /// Displays the control to the user.
        /// </summary>
        public new void Show()
        {
            if (this.VisualizacionPermitida())
            {
                try
                {
                    // Apertura del formulario
                    this.CierrePorUsuario = false;

                    IOrbitaForm frm = this;
                    FrmBase.ListaFormsAbiertos.Add(this.Name);

                    this.Visible = true;
                    this.Dock = DockStyle.Fill;
                    this.BringToFront();

                    if (!this.DesignMode)
                    {
                        this.IniciarFormulario();
                    }
                }
                catch (Exception exception)
                {
                    OVALogsManager.Error(OModulosSistema.Escritorios, "Apertura de formulario", exception);
                }
            }
        }

        /// <summary>
        /// Closes the form.
        /// </summary>
        public new bool Close()
        {
            this.CierrePorUsuario = true;

            bool cerrar = this.IntentaCerrar();
            if (cerrar)
            {
                this.Salir();
            }
            return cerrar;
        }
        #endregion

        #region Manejadores de eventos
        /// <summary>
        /// Manejador de evento click del boton btnCancelar
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evento</param>
        protected virtual void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (System.Exception exception)
            {
                OVALogsManager.Error(OModulosSistema.Escritorios, "Cancelar formulario", exception);
            }
        }
        /// <summary>
        /// Manejador de evento click del boton btnAceptar
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evento</param>
        protected virtual void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.GuardarDatos())
                {
                    //Si se han guardado los datos correctamente, cerramos el formulario
                    this.Close();
                }
            }
            catch (System.Exception exception)
            {
                OVALogsManager.Error(OModulosSistema.Escritorios, "Guardar datos", exception);

            }
        }
        /// <summary>
        /// Evento que se produce cuando se activa el formulario, despues de mostrarse
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void FrmBase_Activated(object sender, EventArgs e)
        {
            try
            {
            }
            catch (System.Exception exception)
            {
                OVALogsManager.Error(OModulosSistema.Escritorios, "Activación del control", exception);
            }
        }
        /// <summary>
        /// Cambio de valor en un componente
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evento</param>
        public void EventoCambioValor(object sender, EventArgs e)
        {
            try
            {
                if (!this.Inicio)
                {
                    this.AlgoModificado = true;
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosSistema.Escritorios, "Activación del control", exception);
            }
        }
        /// <summary>
        /// Cambio de valor en un componente
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="celda">Argumentos del evento</param>
        private void EventoCeldaCambioValor(object sender, UltraGridCell celda)
        {
            this.EventoCambioValor(sender, new EventArgs());
        }
        /// <summary>
        /// Muestra o oculta los tooltips
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void ChkToolTip_CheckedChanged(object sender, EventArgs e)
        {    
            try
            {
                this.OrbTooltip.Active = ChkToolTip.Checked;
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosSistema.Escritorios, "Clic en botón de ToolTips", exception);
            }
        }
        #endregion Manejadores de eventos
    }
}
