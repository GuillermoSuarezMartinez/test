namespace Orbita.Controles.VA
{
    partial class FrmMonitorizacionCronometros
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
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Vistas");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("KeyIconosGrandes");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("KeyIconosPequeños");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("KeyLista");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("KeyDetalles");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("KeyIconosGrandes");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("KeyIconosPequeños");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("KeyLista");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("KeyDetalles");
            this.grbCronometros = new Orbita.Controles.OrbitaGroupBox(this.components);
            this.ListCronometros = new Orbita.Controles.OrbitaListView(this.components);
            this.Codigo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Descripcion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Contador = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.UltimaEjecucion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Promedio = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Total = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuCronometro = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuMonitorizar = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListLarge = new System.Windows.Forms.ImageList(this.components);
            this.imageListSmall = new System.Windows.Forms.ImageList(this.components);
            this.pnlBarraBotones = new Orbita.Controles.OrbitaPanel();
            this.orbitaPanel1 = new Orbita.Controles.OrbitaPanel();
            this.ultraDropDownButton1 = new Infragistics.Win.Misc.UltraDropDownButton();
            this.toolbarsManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this.btnMonitorizar = new Orbita.Controles.OrbitaButton(this.components);
            this.btnVistas = new Infragistics.Win.Misc.UltraDropDownButton();
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
            this.pnlPanelPrincipalPadre.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grbCronometros)).BeginInit();
            this.grbCronometros.SuspendLayout();
            this.menuCronometro.SuspendLayout();
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
            this.pnlPanelPrincipalPadre.Controls.Add(this.grbCronometros);
            this.pnlPanelPrincipalPadre.Size = new System.Drawing.Size(763, 337);
            // 
            // grbCronometros
            // 
            this.grbCronometros.ContentPadding.Bottom = 2;
            this.grbCronometros.ContentPadding.Left = 2;
            this.grbCronometros.ContentPadding.Right = 2;
            this.grbCronometros.ContentPadding.Top = 2;
            this.grbCronometros.Controls.Add(this.ListCronometros);
            this.grbCronometros.Controls.Add(this.pnlBarraBotones);
            this.grbCronometros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbCronometros.Location = new System.Drawing.Point(0, 0);
            this.grbCronometros.Name = "grbCronometros";
            this.grbCronometros.Size = new System.Drawing.Size(763, 337);
            this.grbCronometros.TabIndex = 15;
            this.grbCronometros.Text = "Listado de cronómetros del sistema";
            // 
            // ListCronometros
            // 
            this.ListCronometros.AllowColumnReorder = true;
            this.ListCronometros.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Codigo,
            this.Descripcion,
            this.Contador,
            this.UltimaEjecucion,
            this.Promedio,
            this.Total});
            this.ListCronometros.ContextMenuStrip = this.menuCronometro;
            this.ListCronometros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListCronometros.LargeImageList = this.imageListLarge;
            this.ListCronometros.Location = new System.Drawing.Point(5, 61);
            this.ListCronometros.Name = "ListCronometros";
            this.ListCronometros.ShowItemToolTips = true;
            this.ListCronometros.Size = new System.Drawing.Size(753, 271);
            this.ListCronometros.SmallImageList = this.imageListSmall;
            this.ListCronometros.TabIndex = 15;
            this.ListCronometros.UseCompatibleStateImageBehavior = false;
            this.ListCronometros.View = System.Windows.Forms.View.Details;
            this.ListCronometros.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListCronometros_MouseDoubleClick);
            // 
            // Codigo
            // 
            this.Codigo.Text = "Código";
            this.Codigo.Width = 145;
            // 
            // Descripcion
            // 
            this.Descripcion.Text = "Descripción";
            this.Descripcion.Width = 195;
            // 
            // Contador
            // 
            this.Contador.Text = "Contador";
            this.Contador.Width = 65;
            // 
            // UltimaEjecucion
            // 
            this.UltimaEjecucion.Text = "Última Ejecución";
            this.UltimaEjecucion.Width = 105;
            // 
            // Promedio
            // 
            this.Promedio.Text = "Promedio";
            this.Promedio.Width = 105;
            // 
            // Total
            // 
            this.Total.Text = "Total";
            this.Total.Width = 105;
            // 
            // menuCronometro
            // 
            this.menuCronometro.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMonitorizar});
            this.menuCronometro.Name = "menuVariable";
            this.menuCronometro.Size = new System.Drawing.Size(136, 26);
            this.menuCronometro.Opening += new System.ComponentModel.CancelEventHandler(this.menuVariable_Opening);
            // 
            // menuMonitorizar
            // 
            this.menuMonitorizar.Name = "menuMonitorizar";
            this.menuMonitorizar.Size = new System.Drawing.Size(135, 22);
            this.menuMonitorizar.Text = "Monitorizar";
            this.menuMonitorizar.Click += new System.EventHandler(this.btnMonitorizar_Click);
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
            this.pnlBarraBotones.Controls.Add(this.btnVistas);
            this.pnlBarraBotones.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBarraBotones.Location = new System.Drawing.Point(5, 18);
            this.pnlBarraBotones.Name = "pnlBarraBotones";
            this.pnlBarraBotones.Size = new System.Drawing.Size(753, 43);
            this.pnlBarraBotones.TabIndex = 16;
            // 
            // orbitaPanel1
            // 
            this.orbitaPanel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.orbitaPanel1.Controls.Add(this.ultraDropDownButton1);
            this.orbitaPanel1.Controls.Add(this.btnMonitorizar);
            this.orbitaPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.orbitaPanel1.Location = new System.Drawing.Point(0, 0);
            this.orbitaPanel1.Name = "orbitaPanel1";
            this.orbitaPanel1.Padding = new System.Windows.Forms.Padding(3);
            this.orbitaPanel1.Size = new System.Drawing.Size(753, 43);
            this.orbitaPanel1.TabIndex = 17;
            // 
            // ultraDropDownButton1
            // 
            this.ultraDropDownButton1.Dock = System.Windows.Forms.DockStyle.Right;
            this.ultraDropDownButton1.Location = new System.Drawing.Point(651, 3);
            this.ultraDropDownButton1.Name = "ultraDropDownButton1";
            this.ultraDropDownButton1.PopupItemKey = "Vistas";
            this.ultraDropDownButton1.PopupItemProvider = this.toolbarsManager;
            this.ultraDropDownButton1.Size = new System.Drawing.Size(99, 37);
            this.ultraDropDownButton1.Style = Infragistics.Win.Misc.SplitButtonDisplayStyle.DropDownButtonOnly;
            this.ultraDropDownButton1.TabIndex = 7;
            this.ultraDropDownButton1.Text = "Vistas";
            // 
            // toolbarsManager
            // 
            this.toolbarsManager.AlwaysShowMenusExpanded = Infragistics.Win.DefaultableBoolean.True;
            this.toolbarsManager.DesignerFlags = 1;
            this.toolbarsManager.DockWithinContainer = this;
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
            this.toolbarsManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            popupMenuTool1,
            buttonTool5,
            buttonTool6,
            buttonTool7,
            buttonTool8});
            this.toolbarsManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.ultraToolbarsManager_ToolClick);
            // 
            // btnMonitorizar
            // 
            this.btnMonitorizar.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnMonitorizar.ImageSize = new System.Drawing.Size(24, 24);
            this.btnMonitorizar.Location = new System.Drawing.Point(3, 3);
            this.btnMonitorizar.Name = "btnMonitorizar";
            this.btnMonitorizar.Size = new System.Drawing.Size(99, 37);
            this.btnMonitorizar.TabIndex = 6;
            this.btnMonitorizar.Text = "Monitorizar";
            this.btnMonitorizar.Click += new System.EventHandler(this.btnMonitorizar_Click);
            // 
            // btnVistas
            // 
            this.btnVistas.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVistas.Location = new System.Drawing.Point(651, 4);
            this.btnVistas.Name = "btnVistas";
            this.btnVistas.PopupItemKey = "Vistas";
            this.btnVistas.PopupItemProvider = this.toolbarsManager;
            this.btnVistas.Size = new System.Drawing.Size(99, 36);
            this.btnVistas.Style = Infragistics.Win.Misc.SplitButtonDisplayStyle.DropDownButtonOnly;
            this.btnVistas.TabIndex = 5;
            this.btnVistas.Text = "Vistas";
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
            // FrmMonitorizacionCronometros
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
            this.Name = "FrmMonitorizacionCronometros";
            this.RecordarPosicion = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.Text = "Monitorización de los cronómetros del sistema";
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
            ((System.ComponentModel.ISupportInitialize)(this.grbCronometros)).EndInit();
            this.grbCronometros.ResumeLayout(false);
            this.menuCronometro.ResumeLayout(false);
            this.pnlBarraBotones.ResumeLayout(false);
            this.orbitaPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.toolbarsManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Orbita.Controles.OrbitaGroupBox grbCronometros;
        private Orbita.Controles.OrbitaListView ListCronometros;
        private System.Windows.Forms.ColumnHeader Codigo;
        private System.Windows.Forms.ColumnHeader Descripcion;
        private System.Windows.Forms.ColumnHeader Contador;
        private System.Windows.Forms.ColumnHeader UltimaEjecucion;
        private System.Windows.Forms.ColumnHeader Promedio;
        private System.Windows.Forms.ImageList imageListSmall;
        private Orbita.Controles.OrbitaPanel pnlBarraBotones;
        private System.Windows.Forms.ToolStripMenuItem iconosGrandesToolStripMenuItem;
        private System.Windows.Forms.ImageList imageListLarge;
        private System.Windows.Forms.ContextMenuStrip menuCronometro;
        private System.Windows.Forms.ToolStripMenuItem menuMonitorizar;
        private Infragistics.Win.Misc.UltraDropDownButton btnVistas;
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
        private Infragistics.Win.Misc.UltraDropDownButton ultraDropDownButton1;
        private Orbita.Controles.OrbitaButton btnMonitorizar;
        private System.Windows.Forms.Timer timerRefresco;
        private System.Windows.Forms.ColumnHeader Total;

    }
}