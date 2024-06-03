namespace Orbita.Controles.VA
{
    partial class FrmMonitorizacionVariables
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Vistas");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("KeyIconosGrandes");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("KeyIconosPequeños");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("KeyLista");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("KeyDetalles");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("KeyIconosGrandes");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("KeyIconosPequeños");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("KeyLista");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("KeyDetalles");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Escenarios");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.grbVariables = new Orbita.Controles.OrbitaGroupBox(this.components);
            this.ListVariables = new Orbita.Controles.OrbitaListView(this.components);
            this.CodVariable = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Valor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DescVariable = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.HabilitadoVariable = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.GuardarTrazabilidad = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuVariable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuMonitorizar = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuBloquearVariable = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDesbloquearVariable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuForzarValor = new System.Windows.Forms.ToolStripMenuItem();
            this.menuForzarValorVerdadero = new System.Windows.Forms.ToolStripMenuItem();
            this.menuForzarValorFalso = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCargarFoto = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGuardarFoto = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListLarge = new System.Windows.Forms.ImageList(this.components);
            this.imageListSmall = new System.Windows.Forms.ImageList(this.components);
            this.pnlBarraBotones = new Orbita.Controles.OrbitaPanel();
            this.orbitaPanel1 = new Orbita.Controles.OrbitaPanel();
            this.DropDownEscenarios = new Infragistics.Win.Misc.UltraDropDownButton();
            this.toolbarsManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this.DropDownVistas = new Infragistics.Win.Misc.UltraDropDownButton();
            this.btnMonitorizar = new Orbita.Controles.OrbitaButton(this.components);
            this.iconosGrandesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._Form1_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._Form1_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._Form1_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._Form1_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._FrmBase_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._FrmBase_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._FrmBase_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._FrmBase_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.timerRefresco = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.pnlPanelPrincipalPadre.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grbVariables)).BeginInit();
            this.grbVariables.SuspendLayout();
            this.menuVariable.SuspendLayout();
            this.pnlBarraBotones.SuspendLayout();
            this.orbitaPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toolbarsManager)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlInferiorPadre
            // 
            this.pnlInferiorPadre.Location = new System.Drawing.Point(10, 347);
            this.pnlInferiorPadre.Size = new System.Drawing.Size(763, 43);
            // 
            // pnlPanelPrincipalPadre
            // 
            this.pnlPanelPrincipalPadre.Controls.Add(this.grbVariables);
            this.pnlPanelPrincipalPadre.Size = new System.Drawing.Size(763, 337);
            // 
            // grbVariables
            // 
            this.grbVariables.ContentPadding.Bottom = 2;
            this.grbVariables.ContentPadding.Left = 2;
            this.grbVariables.ContentPadding.Right = 2;
            this.grbVariables.ContentPadding.Top = 2;
            this.grbVariables.Controls.Add(this.ListVariables);
            this.grbVariables.Controls.Add(this.pnlBarraBotones);
            this.grbVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbVariables.Location = new System.Drawing.Point(0, 0);
            this.grbVariables.Name = "grbVariables";
            this.grbVariables.OrbColorBorde = System.Drawing.Color.Empty;
            this.grbVariables.OrbColorCabecera = System.Drawing.Color.Empty;
            this.grbVariables.Size = new System.Drawing.Size(763, 337);
            this.grbVariables.TabIndex = 15;
            this.grbVariables.Text = "Listado de variables del sistema";
            // 
            // ListVariables
            // 
            this.ListVariables.AllowColumnReorder = true;
            this.ListVariables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.CodVariable,
            this.Valor,
            this.DescVariable,
            this.HabilitadoVariable,
            this.GuardarTrazabilidad});
            this.ListVariables.ContextMenuStrip = this.menuVariable;
            this.ListVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListVariables.LargeImageList = this.imageListLarge;
            this.ListVariables.Location = new System.Drawing.Point(5, 61);
            this.ListVariables.Name = "ListVariables";
            this.ListVariables.ShowItemToolTips = true;
            this.ListVariables.Size = new System.Drawing.Size(753, 271);
            this.ListVariables.SmallImageList = this.imageListSmall;
            this.ListVariables.TabIndex = 15;
            this.ListVariables.UseCompatibleStateImageBehavior = false;
            this.ListVariables.View = System.Windows.Forms.View.Details;
            this.ListVariables.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListVariables_MouseDoubleClick);
            // 
            // CodVariable
            // 
            this.CodVariable.Text = "Código";
            this.CodVariable.Width = 150;
            // 
            // Valor
            // 
            this.Valor.Text = "Valor";
            this.Valor.Width = 181;
            // 
            // DescVariable
            // 
            this.DescVariable.Text = "Descripción";
            this.DescVariable.Width = 475;
            // 
            // HabilitadoVariable
            // 
            this.HabilitadoVariable.Text = "Habilitado";
            this.HabilitadoVariable.Width = 111;
            // 
            // GuardarTrazabilidad
            // 
            this.GuardarTrazabilidad.Text = "Guardar Trazabilidad";
            this.GuardarTrazabilidad.Width = 112;
            // 
            // menuVariable
            // 
            this.menuVariable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMonitorizar,
            this.toolStripMenuItem1,
            this.menuBloquearVariable,
            this.menuDesbloquearVariable,
            this.toolStripMenuItem2,
            this.menuForzarValor,
            this.menuForzarValorVerdadero,
            this.menuForzarValorFalso,
            this.menuCargarFoto,
            this.menuGuardarFoto});
            this.menuVariable.Name = "menuVariable";
            this.menuVariable.Size = new System.Drawing.Size(172, 192);
            this.menuVariable.Opening += new System.ComponentModel.CancelEventHandler(this.menuVariable_Opening);
            // 
            // menuMonitorizar
            // 
            this.menuMonitorizar.Name = "menuMonitorizar";
            this.menuMonitorizar.Size = new System.Drawing.Size(171, 22);
            this.menuMonitorizar.Text = "Monitorizar";
            this.menuMonitorizar.Click += new System.EventHandler(this.btnMonitorizar_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(168, 6);
            // 
            // menuBloquearVariable
            // 
            this.menuBloquearVariable.Name = "menuBloquearVariable";
            this.menuBloquearVariable.Size = new System.Drawing.Size(171, 22);
            this.menuBloquearVariable.Text = "Bloquear";
            this.menuBloquearVariable.Click += new System.EventHandler(this.menuBloquearVariable_Click);
            // 
            // menuDesbloquearVariable
            // 
            this.menuDesbloquearVariable.Name = "menuDesbloquearVariable";
            this.menuDesbloquearVariable.Size = new System.Drawing.Size(171, 22);
            this.menuDesbloquearVariable.Text = "Desbloquear";
            this.menuDesbloquearVariable.Click += new System.EventHandler(this.menuDesbloquearVariable_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(168, 6);
            // 
            // menuForzarValor
            // 
            this.menuForzarValor.Name = "menuForzarValor";
            this.menuForzarValor.Size = new System.Drawing.Size(171, 22);
            this.menuForzarValor.Text = "Forzar valor";
            this.menuForzarValor.Click += new System.EventHandler(this.menuForzarValor_Click);
            // 
            // menuForzarValorVerdadero
            // 
            this.menuForzarValorVerdadero.Name = "menuForzarValorVerdadero";
            this.menuForzarValorVerdadero.Size = new System.Drawing.Size(171, 22);
            this.menuForzarValorVerdadero.Text = "Forzar a verdadero";
            this.menuForzarValorVerdadero.Click += new System.EventHandler(this.menuForzarValorVerdadero_Click);
            // 
            // menuForzarValorFalso
            // 
            this.menuForzarValorFalso.Name = "menuForzarValorFalso";
            this.menuForzarValorFalso.Size = new System.Drawing.Size(171, 22);
            this.menuForzarValorFalso.Text = "Forzar a falso";
            this.menuForzarValorFalso.Click += new System.EventHandler(this.menuForzarValorFalso_Click);
            // 
            // menuCargarFoto
            // 
            this.menuCargarFoto.Name = "menuCargarFoto";
            this.menuCargarFoto.Size = new System.Drawing.Size(171, 22);
            this.menuCargarFoto.Text = "Cargar de disco";
            this.menuCargarFoto.Click += new System.EventHandler(this.menuCargarFoto_Click);
            // 
            // menuGuardarFoto
            // 
            this.menuGuardarFoto.Name = "menuGuardarFoto";
            this.menuGuardarFoto.Size = new System.Drawing.Size(171, 22);
            this.menuGuardarFoto.Text = "Guardar a disco";
            this.menuGuardarFoto.Click += new System.EventHandler(this.menuGuardarFoto_Click);
            // 
            // imageListLarge
            // 
            this.imageListLarge.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListLarge.ImageSize = new System.Drawing.Size(32, 32);
            this.imageListLarge.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imageListSmall
            // 
            this.imageListSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListSmall.ImageSize = new System.Drawing.Size(24, 24);
            this.imageListSmall.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // pnlBarraBotones
            // 
            this.pnlBarraBotones.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pnlBarraBotones.Controls.Add(this.orbitaPanel1);
            this.pnlBarraBotones.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBarraBotones.Location = new System.Drawing.Point(5, 18);
            this.pnlBarraBotones.Name = "pnlBarraBotones";
            this.pnlBarraBotones.Size = new System.Drawing.Size(753, 43);
            this.pnlBarraBotones.TabIndex = 16;
            // 
            // orbitaPanel1
            // 
            this.orbitaPanel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.orbitaPanel1.Controls.Add(this.DropDownEscenarios);
            this.orbitaPanel1.Controls.Add(this.DropDownVistas);
            this.orbitaPanel1.Controls.Add(this.btnMonitorizar);
            this.orbitaPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.orbitaPanel1.Location = new System.Drawing.Point(0, 0);
            this.orbitaPanel1.Name = "orbitaPanel1";
            this.orbitaPanel1.Padding = new System.Windows.Forms.Padding(3);
            this.orbitaPanel1.Size = new System.Drawing.Size(753, 43);
            this.orbitaPanel1.TabIndex = 17;
            // 
            // DropDownEscenarios
            // 
            appearance1.Image = global::Orbita.Controles.VA.Properties.Resources.ImgEscenariosVariables;
            this.DropDownEscenarios.Appearance = appearance1;
            this.DropDownEscenarios.Dock = System.Windows.Forms.DockStyle.Right;
            this.DropDownEscenarios.Location = new System.Drawing.Point(552, 3);
            this.DropDownEscenarios.Name = "DropDownEscenarios";
            this.DropDownEscenarios.PopupItemKey = "Escenarios";
            this.DropDownEscenarios.PopupItemProvider = this.toolbarsManager;
            this.DropDownEscenarios.Size = new System.Drawing.Size(99, 37);
            this.DropDownEscenarios.Style = Infragistics.Win.Misc.SplitButtonDisplayStyle.DropDownButtonOnly;
            this.DropDownEscenarios.TabIndex = 8;
            this.DropDownEscenarios.Text = "Grupos";
            // 
            // toolbarsManager
            // 
            this.toolbarsManager.AlwaysShowMenusExpanded = Infragistics.Win.DefaultableBoolean.True;
            this.toolbarsManager.DesignerFlags = 1;
            this.toolbarsManager.DockWithinContainer = this;
            this.toolbarsManager.MenuAnimationStyle = Infragistics.Win.UltraWinToolbars.MenuAnimationStyle.Slide;
            this.toolbarsManager.ShowFullMenusDelay = 500;
            popupMenuTool1.SharedProps.Caption = "Vistas";
            popupMenuTool1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool2,
            buttonTool3,
            buttonTool4});
            buttonTool5.SharedProps.Caption = "Iconos grandes";
            buttonTool6.SharedProps.Caption = "Iconos pequeños";
            buttonTool7.SharedProps.Caption = "Lista";
            buttonTool8.SharedProps.Caption = "Detalles";
            popupMenuTool2.SharedProps.Caption = "Escenarios";
            this.toolbarsManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            popupMenuTool1,
            buttonTool5,
            buttonTool6,
            buttonTool7,
            buttonTool8,
            popupMenuTool2});
            this.toolbarsManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager_ToolClick);
            // 
            // DropDownVistas
            // 
            appearance2.Image = global::Orbita.Controles.VA.Properties.Resources.imgVistasVariables24;
            this.DropDownVistas.Appearance = appearance2;
            this.DropDownVistas.Dock = System.Windows.Forms.DockStyle.Right;
            this.DropDownVistas.Location = new System.Drawing.Point(651, 3);
            this.DropDownVistas.Name = "DropDownVistas";
            this.DropDownVistas.PopupItemKey = "Vistas";
            this.DropDownVistas.PopupItemProvider = this.toolbarsManager;
            this.DropDownVistas.Size = new System.Drawing.Size(99, 37);
            this.DropDownVistas.Style = Infragistics.Win.Misc.SplitButtonDisplayStyle.DropDownButtonOnly;
            this.DropDownVistas.TabIndex = 7;
            this.DropDownVistas.Text = "Vistas";
            // 
            // btnMonitorizar
            // 
            appearance3.Image = global::Orbita.Controles.VA.Properties.Resources.imgMonitorizarVariable24;
            this.btnMonitorizar.Appearance = appearance3;
            this.btnMonitorizar.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnMonitorizar.ImageSize = new System.Drawing.Size(24, 24);
            this.btnMonitorizar.Location = new System.Drawing.Point(3, 3);
            this.btnMonitorizar.Name = "btnMonitorizar";
            this.btnMonitorizar.Size = new System.Drawing.Size(99, 37);
            this.btnMonitorizar.TabIndex = 6;
            this.btnMonitorizar.Text = "Monitorizar";
            this.btnMonitorizar.Click += new System.EventHandler(this.btnMonitorizar_Click);
            // 
            // iconosGrandesToolStripMenuItem
            // 
            this.iconosGrandesToolStripMenuItem.Name = "iconosGrandesToolStripMenuItem";
            this.iconosGrandesToolStripMenuItem.Size = new System.Drawing.Size(99, 20);
            this.iconosGrandesToolStripMenuItem.Text = "Iconos grandes";
            // 
            // _Form1_Toolbars_Dock_Area_Top
            // 
            this._Form1_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Form1_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._Form1_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._Form1_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Form1_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(10, 10);
            this._Form1_Toolbars_Dock_Area_Top.Name = "_Form1_Toolbars_Dock_Area_Top";
            this._Form1_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(763, 0);
            this._Form1_Toolbars_Dock_Area_Top.ToolbarsManager = this.toolbarsManager;
            // 
            // _Form1_Toolbars_Dock_Area_Bottom
            // 
            this._Form1_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Form1_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._Form1_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._Form1_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Form1_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(10, 390);
            this._Form1_Toolbars_Dock_Area_Bottom.Name = "_Form1_Toolbars_Dock_Area_Bottom";
            this._Form1_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(763, 0);
            this._Form1_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.toolbarsManager;
            // 
            // _Form1_Toolbars_Dock_Area_Left
            // 
            this._Form1_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Form1_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._Form1_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._Form1_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Form1_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(10, 10);
            this._Form1_Toolbars_Dock_Area_Left.Name = "_Form1_Toolbars_Dock_Area_Left";
            this._Form1_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 380);
            this._Form1_Toolbars_Dock_Area_Left.ToolbarsManager = this.toolbarsManager;
            // 
            // _Form1_Toolbars_Dock_Area_Right
            // 
            this._Form1_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Form1_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._Form1_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._Form1_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Form1_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(773, 10);
            this._Form1_Toolbars_Dock_Area_Right.Name = "_Form1_Toolbars_Dock_Area_Right";
            this._Form1_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 380);
            this._Form1_Toolbars_Dock_Area_Right.ToolbarsManager = this.toolbarsManager;
            // 
            // _FrmBase_Toolbars_Dock_Area_Left
            // 
            this._FrmBase_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FrmBase_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._FrmBase_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._FrmBase_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FrmBase_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(10, 10);
            this._FrmBase_Toolbars_Dock_Area_Left.Name = "_FrmBase_Toolbars_Dock_Area_Left";
            this._FrmBase_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 380);
            this._FrmBase_Toolbars_Dock_Area_Left.ToolbarsManager = this.toolbarsManager;
            // 
            // _FrmBase_Toolbars_Dock_Area_Right
            // 
            this._FrmBase_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FrmBase_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._FrmBase_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._FrmBase_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FrmBase_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(773, 10);
            this._FrmBase_Toolbars_Dock_Area_Right.Name = "_FrmBase_Toolbars_Dock_Area_Right";
            this._FrmBase_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 380);
            this._FrmBase_Toolbars_Dock_Area_Right.ToolbarsManager = this.toolbarsManager;
            // 
            // _FrmBase_Toolbars_Dock_Area_Top
            // 
            this._FrmBase_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FrmBase_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._FrmBase_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._FrmBase_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FrmBase_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(10, 10);
            this._FrmBase_Toolbars_Dock_Area_Top.Name = "_FrmBase_Toolbars_Dock_Area_Top";
            this._FrmBase_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(763, 0);
            this._FrmBase_Toolbars_Dock_Area_Top.ToolbarsManager = this.toolbarsManager;
            // 
            // _FrmBase_Toolbars_Dock_Area_Bottom
            // 
            this._FrmBase_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FrmBase_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._FrmBase_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._FrmBase_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FrmBase_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(10, 390);
            this._FrmBase_Toolbars_Dock_Area_Bottom.Name = "_FrmBase_Toolbars_Dock_Area_Bottom";
            this._FrmBase_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(763, 0);
            this._FrmBase_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.toolbarsManager;
            // 
            // timerRefresco
            // 
            this.timerRefresco.Interval = 250;
            this.timerRefresco.Tick += new System.EventHandler(this.timerRefresco_Tick);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Bitmap images (*.BMP)|*.BMP|Jpeg images(*.JPG)|*.JPG|Todos los archivos(*.*)|*.*";
            // 
            // FrmMonitorizacionVariables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 400);
            this.Controls.Add(this._Form1_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._Form1_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._FrmBase_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._FrmBase_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._Form1_Toolbars_Dock_Area_Top);
            this.Controls.Add(this._Form1_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._FrmBase_Toolbars_Dock_Area_Top);
            this.Controls.Add(this._FrmBase_Toolbars_Dock_Area_Bottom);
            this.ModoAperturaFormulario = Orbita.Controles.VA.ModoAperturaFormulario.Monitorizacion;
            this.Name = "FrmMonitorizacionVariables";
            this.RecordarPosicion = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.Text = "Monitorizacion de las variables del sistema";
            this.Controls.SetChildIndex(this._FrmBase_Toolbars_Dock_Area_Bottom, 0);
            this.Controls.SetChildIndex(this._FrmBase_Toolbars_Dock_Area_Top, 0);
            this.Controls.SetChildIndex(this._Form1_Toolbars_Dock_Area_Bottom, 0);
            this.Controls.SetChildIndex(this._Form1_Toolbars_Dock_Area_Top, 0);
            this.Controls.SetChildIndex(this._FrmBase_Toolbars_Dock_Area_Right, 0);
            this.Controls.SetChildIndex(this._FrmBase_Toolbars_Dock_Area_Left, 0);
            this.Controls.SetChildIndex(this._Form1_Toolbars_Dock_Area_Right, 0);
            this.Controls.SetChildIndex(this._Form1_Toolbars_Dock_Area_Left, 0);
            this.Controls.SetChildIndex(this.pnlInferiorPadre, 0);
            this.Controls.SetChildIndex(this.pnlPanelPrincipalPadre, 0);
            this.pnlPanelPrincipalPadre.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grbVariables)).EndInit();
            this.grbVariables.ResumeLayout(false);
            this.menuVariable.ResumeLayout(false);
            this.pnlBarraBotones.ResumeLayout(false);
            this.orbitaPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.toolbarsManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Orbita.Controles.OrbitaGroupBox grbVariables;
        private Orbita.Controles.OrbitaListView ListVariables;
        private System.Windows.Forms.ColumnHeader CodVariable;
        private System.Windows.Forms.ColumnHeader Valor;
        private System.Windows.Forms.ColumnHeader DescVariable;
        private System.Windows.Forms.ColumnHeader HabilitadoVariable;
        private System.Windows.Forms.ColumnHeader GuardarTrazabilidad;
        private System.Windows.Forms.ImageList imageListSmall;
        private Orbita.Controles.OrbitaPanel pnlBarraBotones;
        private System.Windows.Forms.ToolStripMenuItem iconosGrandesToolStripMenuItem;
        private System.Windows.Forms.ImageList imageListLarge;
        private System.Windows.Forms.ContextMenuStrip menuVariable;
        private System.Windows.Forms.ToolStripMenuItem menuMonitorizar;
        private System.Windows.Forms.ToolStripMenuItem menuForzarValor;
        private System.Windows.Forms.ToolStripMenuItem menuBloquearVariable;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuDesbloquearVariable;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem menuForzarValorVerdadero;
        private System.Windows.Forms.ToolStripMenuItem menuForzarValorFalso;
        private System.Windows.Forms.ToolStripMenuItem menuCargarFoto;
        private System.Windows.Forms.ToolStripMenuItem menuGuardarFoto;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager toolbarsManager;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _Form1_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _Form1_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _FrmBase_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _FrmBase_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _Form1_Toolbars_Dock_Area_Top;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _Form1_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _FrmBase_Toolbars_Dock_Area_Top;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _FrmBase_Toolbars_Dock_Area_Bottom;
        private Orbita.Controles.OrbitaPanel orbitaPanel1;
        private Orbita.Controles.OrbitaButton btnMonitorizar;
        private System.Windows.Forms.Timer timerRefresco;
        private Infragistics.Win.Misc.UltraDropDownButton DropDownEscenarios;
        private Infragistics.Win.Misc.UltraDropDownButton DropDownVistas;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}