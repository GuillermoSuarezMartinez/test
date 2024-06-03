//***********************************************************************
// Assembly         : Orbita.Controles.VA
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
using System.Drawing;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinDock;
using Infragistics.Win.UltraWinGrid;
using Orbita.Utiles;
using Orbita.VAComun;

namespace Orbita.Controles.VA
{
    /// <summary>
    /// Constructor de la clase
    /// </summary>
    public partial class FrmBase : OrbitaForm, IOrbitaForm, IDisposable
    {
        #region Atributo(s)
        /// <summary>
        /// Indica si se ha modificado algo
        /// </summary>
        protected bool AlgoModificado;
        /// <summary>
        /// Indica si se está en la fase de inicialización de los controles del formulario
        /// </summary>
        protected bool Inicio;
        /// <summary>
        /// Informa si el motivo del cierre es por decisión el usuario o interna del código
        /// </summary>
        protected bool CierrePorUsuario;
        /// <summary>
        /// Objeto DockAreaPane utilizado para anclar en las ventanas en modo monitorización
        /// </summary>
        private DockAreaPane DockAreaPane;
        /// <summary>
        /// Objeto DockableWindow utilizado para anclar en las ventanas en modo monitorización
        /// </summary>
        private DockableWindow DockableWindow;
        /// <summary>
        /// Objeto DockableControlPane para anclar en la ventanas en modo monitorización
        /// </summary>
        private DockableControlPane DockableControlPane;
        /// <summary>
        /// Estado del formulario por defecto
        /// </summary>
        private FormWindowState DefaultWindowState;
        /// <summary>
        /// Borde del formulario por defecto
        /// </summary>
        private FormBorderStyle DefaultFormBorderStyle;
        /// <summary>
        /// Área de visualización por defecto
        /// </summary>
        private Rectangle DefatulRectangle;
        #endregion Campos

        #region Atributo(s) estáticos
        /// <summary>
        /// Listado de los formularios abiertos y el número de instancias
        /// </summary>
        public static List<string> ListaFormsAbiertos = new List<string>();
        #endregion

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
        /// Si se activa, se está indicando que el propio formulario debe monitorizar la entrada de datos del usuario para saber cuando
        /// han sido modificados, y por lo tanto realizar las tareas apropiadas.
        /// </summary>
        private bool _ControlDatosModificados;
        /// <summary>
        /// Si se activa, se está indicando que el propio formulario debe monitorizar la entrada de datos del usuario para saber cuando
        /// han sido modificados, y por lo tanto realizar las tareas apropiadas.
        /// </summary>
        [Browsable(true),
        DefaultValueAttribute(true),
        Category("Orbita"),
        Description("Indica que la información contenida en el formulario debe ser monitorizada para saber cuando el usuario la ha modificado")]
        public bool ControlDatosModificados
        {
            get { return _ControlDatosModificados; }
            set { _ControlDatosModificados = value; }
        }

        /// <summary>
        /// Se activa a verdadero cuando se han guardado correctamten los datos del formulario,
        /// bien en la base de datos o bien en formulario invocante.
        /// </summary>
        private bool _FormularioModificado;
        /// <summary>
        /// Se activa a verdadero cuando se han guardado correctamten los datos del formulario,
        /// bien en la base de datos o bien en formulario invocante.
        /// </summary>
        [Browsable(false)]
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
        private bool _RecordarPosicion;
        /// <summary>
        /// Indica si se ha de restaurar la posición del formulario a la última guardada
        /// </summary>
        [Browsable(true),
        Category("Orbita"),
        Description("Indica si se ha de restaurar la posición del formulario a la última guardada")]
        public bool RecordarPosicion
        {
            get { return _RecordarPosicion; }
            set { _RecordarPosicion = value; }
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
                bool resultado = false;
                if (this.Anclado && this.IsDockedMDIChild && (this.DockableWindow is DockableWindow) && (this.DockableWindow.ParentForm is MdiChildForm))
                {
                    resultado = this.DockableWindow.ParentForm.WindowState == FormWindowState.Maximized;
                }
                if (!Anclado)
                {
                    resultado = this.WindowState == FormWindowState.Maximized;
                }
                return resultado;
            }
            set
            {
                FormWindowState windowsState = value ? FormWindowState.Maximized : FormWindowState.Normal;

                if (this.Anclado && this.IsDockedMDIChild && (this.DockableWindow is DockableWindow) && (this.DockableWindow.ParentForm is MdiChildForm))
                {
                    this.DockableWindow.ParentForm.WindowState = windowsState;
                }
                if (!Anclado)
                {
                    this.WindowState = windowsState;
                }
            }
        }

        /// <summary>
        /// Indica que el formulario puede anclarse o no
        /// </summary>
        [Browsable(false)]
        public bool Anclable
        {
            get { return true; }
        }

        /// <summary>
        /// Indica que el formulario está anclado o no
        /// </summary>
        private bool _Anclado;
        /// <summary>
        /// Indica que el formulario está anclado o no
        /// </summary>
        [Browsable(false)]
        public bool Anclado
        {
            get { return _Anclado; }
        }

        /// <summary>
        /// Is Docked MDIChild
        /// </summary>
        [Browsable(false)]
        public bool IsDockedMDIChild
        {
            get
            {
                bool resultado = false;
                if (this.Anclado && (this.DockableControlPane is DockableControlPane))
                {
                    resultado = this.DockableControlPane.IsMdiChild;
                }
                return resultado;
            }
            set
            {
                if (this.Anclado && (this.DockableControlPane is DockableControlPane))
                {
                    this.DockableControlPane.IsMdiChild = value;
                    this.ChkDock.Checked = !value;
                }
            }
        }

        /// <summary>
        /// Rectangulo de visualización del fomrulario MDI de tipo anclable
        /// </summary>
        [Browsable(false)]
        public Rectangle DockedMDIRectangle
        {
            get
            {
                Rectangle rectangle = new Rectangle();
                if (this.Anclado && this.IsDockedMDIChild && (this.DockableWindow is DockableWindow) && (this.DockableWindow.ParentForm is MdiChildForm))
                {
                    MdiChildForm mdiChildForm = (MdiChildForm)this.DockableWindow.ParentForm;
                    rectangle.X = mdiChildForm.Left;
                    rectangle.Y = mdiChildForm.Top;
                    rectangle.Width = mdiChildForm.Width;
                    rectangle.Height = mdiChildForm.Height;
                }
                return rectangle;
            }
            set
            {
                if (this.Anclado && this.IsDockedMDIChild && (this.DockableWindow is DockableWindow) && (this.DockableWindow.ParentForm is MdiChildForm))
                {
                    MdiChildForm mdiChildForm = (MdiChildForm)this.DockableWindow.ParentForm;
                    mdiChildForm.Left = value.X;
                    mdiChildForm.Top = value.Y;
                    mdiChildForm.Width = value.Width;
                    mdiChildForm.Height = value.Height;
                }
            }
        }

        /// <summary>
        /// Rectangulo de visualización del fomrulario anclado
        /// </summary>
        [Browsable(false)]
        public Size DockedPaneSize
        {
            get
            {
                Size size = new Size();
                if (this.Anclado && !this.IsDockedMDIChild && (this.DockAreaPane is DockAreaPane))
                {
                    size = this.DockableWindow.Size;
                }
                return size;
            }
            set
            {
                if (this.Anclado && !this.IsDockedMDIChild && (this.DockAreaPane is DockAreaPane))
                {
                    this.DockAreaPane.Size = value;
                }
            }
        }

        /// <summary>
        /// Informa si la la ventana anclada está puesta para no oculatarse automáticamente
        /// </summary>
        [Browsable(false)]
        public bool DockedPined
        {
            get
            {
                bool pinned = false;
                if (this.Anclado && !this.IsDockedMDIChild && (this.DockableControlPane is DockableControlPane))
                {
                    pinned = this.DockableControlPane.Pinned;
                }
                return pinned;
            }
            set
            {
                if (this.Anclado && !this.IsDockedMDIChild && (this.DockableControlPane is DockableControlPane))
                {
                    if (value)
                    {
                        this.DockableControlPane.Pin();
                    }
                    else
                    {
                        this.DockableControlPane.Unpin();
                    }
                }
            }
        }

        /// <summary>
        /// Localización del anclaje
        /// </summary>
        [Browsable(false)]
        public DockedLocation DockedLocation
        {
            get
            {
                DockedLocation resultado = DockedLocation.DockedRight;
                if (this.Anclado && (this.DockableControlPane is DockableControlPane) && (this.DockableControlPane.Parent is DockAreaPane))
                {
                    resultado = ((DockAreaPane)this.DockableControlPane.Parent).DockedLocation;
                }
                return resultado;
            }
            set
            {
                if (this.Anclado && !this.IsDockedMDIChild && (this.DockableControlPane is DockableControlPane) && (this.DockableControlPane.Parent is DockAreaPane))
                {
                    this.RehacerAnclaje(value);
                }
            }
        }
        #endregion Propiedades

        #region Constructor(es)
        /// <summary>
        /// Constructor vacio de la clase (es necesario para que el diseñador construya el formulario heredado en tiempo de diseño)
        /// </summary>		
        public FrmBase()
        {
            InitializeComponent();

            // Inicialiación de campos
            this.AlgoModificado = false;
            this._FormularioModificado = false;
            this.CierrePorUsuario = false;
            this.DefaultWindowState = this.WindowState;
            this.DefaultFormBorderStyle = this.FormBorderStyle;
        }
        /// <summary>
        /// Constructor de la clase (No existe bloqueo de registro)
        /// </summary>
        /// <param name="modoFormulario">Modo de apertura del formulario (Nuevo, Visualizacion, Modificacion, Ninguno)</param>
        public FrmBase(ModoAperturaFormulario modoFormulario)
        {
            InitializeComponent();
            this._ModoAperturaFormulario = modoFormulario;

            // Inicialiación de campos
            this.AlgoModificado = false;
            this._FormularioModificado = false;
            this.CierrePorUsuario = false;
            this.DefaultWindowState = this.WindowState;
            this.DefaultFormBorderStyle = this.FormBorderStyle;
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

        /// <summary>
        /// Permite que el formulario pueda ser manejado por OrbitaDockManager
        /// </summary>
        private void CrearAnclaje()
        {
            if (OEscritoriosManager.PermiteAnclajes)
            {
                this.SuspendLayout();
                OTrabajoControles.DockManager.SuspendLayout();

                FormWindowState windowState = this.WindowState;

                // Creación del área de anclaje
                this.DockAreaPane = OTrabajoControles.DockManager.DockControls(new Control[] { this }, DockedLocation.DockedRight, ChildPaneStyle.TabGroup);
                this.DockAreaPane.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width / 2, Screen.PrimaryScreen.WorkingArea.Height / 2);
                this.DockAreaPane.Key = this.Handle.ToString();
                this.DockAreaPane.Text = this.Text;
                this.DockAreaPane.Settings.AllowDockLeft = DefaultableBoolean.False;
                this.DockAreaPane.Settings.AllowDockTop = DefaultableBoolean.False;

                // Creación del furmulario MDI de anclaje
                this.DockableControlPane = (DockableControlPane)this.DockAreaPane.Panes[0];
                this.DockableControlPane.Text = this.Text;
                this.DockableControlPane.IsMdiChild = true;

                // Personalización del formulario MDI de anclaje
                this.DockableControlPane.MdiChildIcon = this.Icon;
                this.DockableWindow = (DockableWindow)this.Parent;
                if (this.DockableWindow.ParentForm is MdiChildForm)
                {
                    ((MdiChildForm)this.DockableWindow.ParentForm).Text = this.Text;
                    ((MdiChildForm)this.DockableWindow.ParentForm).Left = this.DefatulRectangle.Left;
                    ((MdiChildForm)this.DockableWindow.ParentForm).Width = this.DefatulRectangle.Width;
                    ((MdiChildForm)this.DockableWindow.ParentForm).Top = this.DefatulRectangle.Top;
                    ((MdiChildForm)this.DockableWindow.ParentForm).Height = this.DefatulRectangle.Height;
                    ((MdiChildForm)this.DockableWindow.ParentForm).FormBorderStyle = this.DefaultFormBorderStyle;
                    ((MdiChildForm)this.DockableWindow.ParentForm).WindowState = this.DefaultWindowState;
                    ((MdiChildForm)this.DockableWindow.ParentForm).FormClosing += this.FrmBase_FormClosing;
                    ((MdiChildForm)this.DockableWindow.ParentForm).FormClosed += this.FrmBase_FormClosed;
                    ((MdiChildForm)this.DockableWindow.ParentForm).WindowState = windowState;
                    this.DockAreaPane.DefaultPaneSettings.AllowClose = Infragistics.Win.DefaultableBoolean.True;
                }

                // Eventos para controlar la salida del formulario
                OTrabajoControles.DockManager.AfterPaneButtonClick += this.FrmBase_AfterPaneButtonClick;
                OTrabajoControles.DockManager.AfterDockChange += this.FrmBase_AfterDockChange;

                // Se establece el formulario para ser visualizado como un control
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.Dock = DockStyle.Fill;
                this._Anclado = true;

                OTrabajoControles.DockManager.ResumeLayout();
                this.ResumeLayout();
            }
        }

        /// <summary>
        /// Permite resituar un formulario anclado
        /// </summary>
        private void RehacerAnclaje(DockedLocation dockedLocation)
        {
            if (OEscritoriosManager.PermiteAnclajes)
            {
                this.SuspendLayout();
                OTrabajoControles.DockManager.SuspendLayout();

                DockAreaPane dockAreaPane = new DockAreaPane(dockedLocation);
                dockAreaPane.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width / 2, Screen.PrimaryScreen.WorkingArea.Height / 2);
                dockAreaPane.Key = this.Text;
                dockAreaPane.Settings.AllowDockLeft = DefaultableBoolean.False;
                dockAreaPane.Settings.AllowDockTop = DefaultableBoolean.False;

                this.DockableControlPane.Pin();

                switch (dockedLocation)
                {
                    case DockedLocation.DockedBottom:
                        this.DockableControlPane.Dock(DockedSide.Bottom);
                        break;
                    case DockedLocation.DockedLeft:
                        this.DockableControlPane.Dock(DockedSide.Left);
                        break;
                    case DockedLocation.DockedRight:
                        this.DockableControlPane.Dock(DockedSide.Right);
                        break;
                    case DockedLocation.DockedTop:
                        this.DockableControlPane.Dock(DockedSide.Top);
                        break;
                }

                this.DestroyClosedPanes();
                this.DockAreaPane = this.DockableControlPane.DockAreaPane;

                OTrabajoControles.DockManager.ResumeLayout();
                this.ResumeLayout();
            }
        }

        /// <summary>
        /// Permite que el formulario pueda ser manejado por OrbitaDockManager
        /// </summary>
        private void EliminarAnclaje()
        {
            if (OEscritoriosManager.PermiteAnclajes)
            {
                this.SuspendLayout();
                OTrabajoControles.DockManager.SuspendLayout();

                // Eventos para controlar la salida del formulario
                OTrabajoControles.DockManager.AfterPaneButtonClick -= this.FrmBase_AfterPaneButtonClick;
                OTrabajoControles.DockManager.AfterDockChange -= this.FrmBase_AfterDockChange;

                if (this.DockableWindow.ParentForm is MdiChildForm)
                {
                    ((MdiChildForm)this.DockableWindow.ParentForm).FormClosing -= this.FrmBase_FormClosing;
                    ((MdiChildForm)this.DockableWindow.ParentForm).FormClosed -= this.FrmBase_FormClosed;
                }

                this.DockableControlPane.Close();
                this.DestroyClosedPanes();

                OTrabajoControles.DockManager.ResumeLayout();
                this.ResumeLayout();
            }
        }

        /// <summary>
        /// Destruye los anclajes cerrados
        /// </summary>
        private void DestroyClosedPanes()
        {
            foreach (DockableControlPane pane in OTrabajoControles.DockManager.ControlPanes)
            {
                if (pane.Closed)
                {
                    if (pane.Control != null)
                        pane.Control.Dispose(); //removes from DockManager collections
                    pane.Dispose();
                }
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
            this.ChkDock.Checked = false;
            this.ChkDock.Visible = false;
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
            this.ChkDock.Checked = false;
            this.ChkDock.Visible = false;
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
            this.ChkDock.Checked = false;
            this.ChkDock.Visible = false;
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
            this.ChkDock.Checked = false;
            this.ChkDock.Visible = OEscritoriosManager.PermiteAnclajes && this.Anclable;
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
            this.ChkDock.Checked = false;
            this.ChkDock.Visible = OEscritoriosManager.PermiteAnclajes && this.Anclable;
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
                    OTrabajoControles.FormularioPrincipalMDI.OrbMdiEncolarForm(this);

                    // Posición por defecto del formulario
                    this.DefatulRectangle = new Rectangle(this.Left, this.Top, this.Width, this.Height);

                    // Situa la posición del formulario
                    IOrbitaForm frmBase = this;

                    FrmBase.ListaFormsAbiertos.Add(this.Name);

                    //base.Show();
                    this.Visible = true;

                    // Dock para los formularios de monitorización
                    this._Anclado = false;
                    if ((this.ModoAperturaFormulario == ModoAperturaFormulario.Monitorizacion) ||
                        ((this.ModoAperturaFormulario == ModoAperturaFormulario.Sistema) &&
                        (this.MostrarBotones)))
                    {
                        this.CrearAnclaje();
                    }

                    OEscritoriosManager.SituarFormulario(ref frmBase);
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
        public new void Close()
        {
            this.CierrePorUsuario = true;
            base.Close();
        }
        #endregion

        #region Manejadores de eventos
        /// <summary>
        /// Evento que se produce al cargar el formulario, antes de aparecer
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void FrmBase_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.DesignMode)
                {
                    this.IniciarFormulario();
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosSistema.Escritorios, "Carga de formulario", exception);
            }
        }
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
        /// Evento que se produce cuando se cierra el formulario, antes de cerrarlo
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void FrmBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if ((this.ModoAperturaFormulario == ModoAperturaFormulario.Sistema) && (e.CloseReason == CloseReason.UserClosing) && (!this.CierrePorUsuario))
                {
                    e.Cancel = true;
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
                                    e.Cancel = true;
                                    return;
                                }
                                break;
                            case DialogResult.No:
                                this.AccionesNoGuardar();
                                //Cerrar el formulario, es decir, seguir con la ejecucion
                                break;
                            case DialogResult.Cancel:
                                e.Cancel = true;
                                break;
                        }
                    }
                }
            }
            catch (System.Exception exception)
            {
                OVALogsManager.Error(OModulosSistema.Escritorios, "Cierre de formulario", exception);
            }
        }
        /// <summary>
        /// Evento que se produce cuando se cierra el formulario, despues de cerrarlo
        /// </summary>
        /// <param name="sender">Objeto que envía el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void FrmBase_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.FinalizarMonitorizarModificaciones();
                this.AccionesSalir();

                // Eliminamos el formulario de la lista de formularios abiertos
                FrmBase.ListaFormsAbiertos.Remove(this.Name);

                if ((this.ModoAperturaFormulario == ModoAperturaFormulario.Monitorizacion) ||
                    ((this.ModoAperturaFormulario == ModoAperturaFormulario.Sistema) &&
                    (this.MostrarBotones)))
                {
                    this.EliminarAnclaje();
                }
            }
            catch (System.Exception exception)
            {
                OVALogsManager.Error(OModulosSistema.Escritorios, "Cierre de formulario", exception);
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
                OVALogsManager.Error(OModulosSistema.Escritorios, "Activación de formulario", exception);
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
                if (!this.Inicio && this.ControlDatosModificados)
                {
                    this.AlgoModificado = true;
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosSistema.Escritorios, "Cambio de valor", exception);
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
        /// <summary>
        /// Maneja el anclaje del formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChkDock_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.DockableControlPane.IsMdiChild = !this.ChkDock.Checked;
                this.FrmBase_AfterDockChange(sender, new PaneEventArgs(this.DockableControlPane));
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosSistema.Escritorios, "Clic en botón de anclaje", exception);
            }
        }
        /// <summary>
        /// Evento de clic sobre los botones del control de anclaje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmBase_AfterPaneButtonClick(object sender, PaneButtonEventArgs e)
        {
            try
            {
                if (e.Pane is DockableControlPane)
                {
                    Control c = ((DockableControlPane)e.Pane).Control;
                    if (((DockableControlPane)e.Pane).Control == this)
                    {
                        if (e.Button == PaneButton.Close)
                        {
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosSistema.Escritorios, "Clic en botón cerrar del anclaje", exception);
            }
        }
        /// <summary>
        /// Evento de cambio de anclaje sobre el control de anclaje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmBase_AfterDockChange(object sender, PaneEventArgs e)
        {
            try
            {
                if (this.DockableControlPane.Control == this)
                {
                    if (this.DockableControlPane.IsMdiChild)
                    {
                        ((MdiChildForm)this.DockableWindow.ParentForm).Text = this.Text;
                        ((MdiChildForm)this.DockableWindow.ParentForm).Left = this.DefatulRectangle.Left;
                        ((MdiChildForm)this.DockableWindow.ParentForm).Width = this.DefatulRectangle.Width;
                        ((MdiChildForm)this.DockableWindow.ParentForm).Top = this.DefatulRectangle.Top;
                        ((MdiChildForm)this.DockableWindow.ParentForm).Height = this.DefatulRectangle.Height;
                        ((MdiChildForm)this.DockableWindow.ParentForm).FormBorderStyle = this.DefaultFormBorderStyle;
                        ((MdiChildForm)this.DockableWindow.ParentForm).WindowState = this.DefaultWindowState;
                        ((MdiChildForm)this.DockableWindow.ParentForm).FormClosing += this.FrmBase_FormClosing;
                        ((MdiChildForm)this.DockableWindow.ParentForm).FormClosed += this.FrmBase_FormClosed;
                        this.DockAreaPane.DefaultPaneSettings.AllowClose = Infragistics.Win.DefaultableBoolean.True;
                    }
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosSistema.Escritorios, "Formulario cambiado de anclaje", exception);
            }
        }
        #endregion Manejadores de eventos
    }

    /// <summary>
    /// Enumerado que representa el objetivo de la apetura del formulario
    /// </summary>
    public enum ModoAperturaFormulario
    {
        /// <summary>
        /// El formulario no se abre en ningun modo
        /// </summary>
        Ninguno = 0,
        /// <summary>
        /// El formulario se abre en modo creación de registro
        /// </summary>        
        Nuevo = 1,
        /// <summary>
        /// El formulario se abre en modo visualizacion
        /// </summary>
        Visualizacion = 2,
        /// <summary>
        /// El formulario se abre en modo modificación
        /// </summary>        
        Modificacion = 4,
        /// <summary>
        /// El formulario se abre en modo monitorización
        /// </summary>
        Monitorizacion = 8,
        /// <summary>
        /// El formulario se abre en modo sistema, por lo que no se puede cerrar por el usuario
        /// </summary>
        Sistema = 16
    }

    /// <summary>
    /// Intefaz para unificar las referencias a obetos de tipo OrbitaForm y OrbitaDialog
    /// </summary>
    public interface IOrbitaForm
    {
        #region Propiedad(es)
        /// <summary>
        /// Nombre del formulario
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Localización del formulario
        /// </summary>
        Point Location { get; set; }

        /// <summary>
        /// Tamaño del formulario
        /// </summary>
        Size Size { get; set; }

        /// <summary>
        /// Indica que el formulario está maximizado
        /// </summary>
        bool Maximized { get; set; }

        /// <summary>
        /// Indica si se ha de restaurar la posición del formulario a la última guardada
        /// </summary>
        bool RecordarPosicion { get; set; }

        /// <summary>
        /// Indica que el formulario puede anclarse o no
        /// </summary>
        bool Anclable { get; }

        /// <summary>
        /// Indica que el formulario está anclado o no
        /// </summary>
        bool Anclado { get; }

        /// <summary>
        /// Is Docked MDIChild
        /// </summary>
        bool IsDockedMDIChild { get; set; }

        /// <summary>
        /// Rectangulo de visualización del fomrulario MDI de tipo anclable
        /// </summary>
        Rectangle DockedMDIRectangle { get; set; }

        /// <summary>
        /// Rectangulo de visualización del fomrulario anclado
        /// </summary>
        Size DockedPaneSize { get; set; }

        /// <summary>
        /// Informa si la la ventana anclada está puesta para no oculatarse automáticamente
        /// </summary>
        bool DockedPined { get; set; }

        /// <summary>
        /// Localización del anclaje
        /// </summary>
        DockedLocation DockedLocation { get; set; }
        #endregion

        #region Método(s)
        /// <summary>
        /// Displays the control to the user.
        /// </summary>
        void Show();
        #endregion
    }
}