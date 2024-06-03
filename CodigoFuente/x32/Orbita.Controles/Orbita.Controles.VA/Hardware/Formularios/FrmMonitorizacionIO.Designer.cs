namespace Orbita.Controles.VA
{
    partial class FrmMonitorizacionIO
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.grbTerminales = new Orbita.Controles.OrbitaGroupBox(this.components);
            this.ListTerminales = new Orbita.Controles.OrbitaListView(this.components);
            this.CodVariable = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Valor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DescVariable = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.HabilitadoVariable = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Tipo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageListLarge = new System.Windows.Forms.ImageList(this.components);
            this.imageListSmall = new System.Windows.Forms.ImageList(this.components);
            this.pnlBarraBotones = new Orbita.Controles.OrbitaPanel();
            this.btnVistas = new Infragistics.Win.Misc.UltraDropDownButton();
            this.toolbarsManager = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this.orbitaPanel1 = new Orbita.Controles.OrbitaPanel();
            this.btnMonitorizar = new Orbita.Controles.OrbitaButton(this.components);
            this.menuTerminal = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuMonitorizar = new System.Windows.Forms.ToolStripMenuItem();
            this.timerRefresco = new System.Windows.Forms.Timer(this.components);
            this._FrmBase_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._FrmBase_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._FrmBase_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._FrmBase_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.pnlPanelPrincipalPadre.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grbTerminales)).BeginInit();
            this.grbTerminales.SuspendLayout();
            this.pnlBarraBotones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toolbarsManager)).BeginInit();
            this.orbitaPanel1.SuspendLayout();
            this.menuTerminal.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPanelPrincipalPadre
            // 
            this.pnlPanelPrincipalPadre.Controls.Add(this.grbTerminales);
            // 
            // grbTerminales
            // 
            this.grbTerminales.ContentPadding.Bottom = 2;
            this.grbTerminales.ContentPadding.Left = 2;
            this.grbTerminales.ContentPadding.Right = 2;
            this.grbTerminales.ContentPadding.Top = 2;
            this.grbTerminales.Controls.Add(this.ListTerminales);
            this.grbTerminales.Controls.Add(this.pnlBarraBotones);
            this.grbTerminales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbTerminales.Location = new System.Drawing.Point(0, 0);
            this.grbTerminales.Name = "grbTerminales";
            this.grbTerminales.OrbColorBorde = System.Drawing.Color.Empty;
            this.grbTerminales.OrbColorCabecera = System.Drawing.Color.Empty;
            this.grbTerminales.Size = new System.Drawing.Size(772, 403);
            this.grbTerminales.TabIndex = 16;
            this.grbTerminales.Text = "Listado de terminales del módulo";
            // 
            // ListTerminales
            // 
            this.ListTerminales.AllowColumnReorder = true;
            this.ListTerminales.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.CodVariable,
            this.Valor,
            this.DescVariable,
            this.HabilitadoVariable,
            this.Tipo});
            this.ListTerminales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListTerminales.LargeImageList = this.imageListLarge;
            this.ListTerminales.Location = new System.Drawing.Point(5, 61);
            this.ListTerminales.Name = "ListTerminales";
            this.ListTerminales.ShowItemToolTips = true;
            this.ListTerminales.Size = new System.Drawing.Size(762, 337);
            this.ListTerminales.SmallImageList = this.imageListSmall;
            this.ListTerminales.TabIndex = 15;
            this.ListTerminales.UseCompatibleStateImageBehavior = false;
            this.ListTerminales.View = System.Windows.Forms.View.Details;
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
            // Tipo
            // 
            this.Tipo.Text = "Tipo terminal";
            this.Tipo.Width = 112;
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
            this.pnlBarraBotones.Controls.Add(this.btnVistas);
            this.pnlBarraBotones.Controls.Add(this.orbitaPanel1);
            this.pnlBarraBotones.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBarraBotones.Location = new System.Drawing.Point(5, 18);
            this.pnlBarraBotones.Name = "pnlBarraBotones";
            this.pnlBarraBotones.Size = new System.Drawing.Size(762, 43);
            this.pnlBarraBotones.TabIndex = 16;
            // 
            // btnVistas
            // 
            this.btnVistas.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.Image = global::Orbita.Controles.VA.Properties.Resources.imgVistasVariables24;
            this.btnVistas.Appearance = appearance1;
            this.btnVistas.Location = new System.Drawing.Point(660, 4);
            this.btnVistas.Name = "btnVistas";
            this.btnVistas.PopupItemKey = "Vistas";
            this.btnVistas.PopupItemProvider = this.toolbarsManager;
            this.btnVistas.Size = new System.Drawing.Size(99, 36);
            this.btnVistas.Style = Infragistics.Win.Misc.SplitButtonDisplayStyle.DropDownButtonOnly;
            this.btnVistas.TabIndex = 5;
            this.btnVistas.Text = "Vistas";
            // 
            // toolbarsManager
            // 
            this.toolbarsManager.AlwaysShowMenusExpanded = Infragistics.Win.DefaultableBoolean.True;
            this.toolbarsManager.DesignerFlags = 1;
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
            // orbitaPanel1
            // 
            this.orbitaPanel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.orbitaPanel1.Controls.Add(this.btnMonitorizar);
            this.orbitaPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.orbitaPanel1.Location = new System.Drawing.Point(0, 0);
            this.orbitaPanel1.Name = "orbitaPanel1";
            this.orbitaPanel1.Padding = new System.Windows.Forms.Padding(3);
            this.orbitaPanel1.Size = new System.Drawing.Size(762, 43);
            this.orbitaPanel1.TabIndex = 17;
            // 
            // btnMonitorizar
            // 
            appearance2.Image = global::Orbita.Controles.VA.Properties.Resources.imgMonitorizarVariable24;
            this.btnMonitorizar.Appearance = appearance2;
            this.btnMonitorizar.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnMonitorizar.ImageSize = new System.Drawing.Size(24, 24);
            this.btnMonitorizar.Location = new System.Drawing.Point(3, 3);
            this.btnMonitorizar.Name = "btnMonitorizar";
            this.btnMonitorizar.Size = new System.Drawing.Size(99, 37);
            this.btnMonitorizar.TabIndex = 6;
            this.btnMonitorizar.Text = "Monitorizar";
            // 
            // menuTerminal
            // 
            this.menuTerminal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMonitorizar});
            this.menuTerminal.Name = "menuVariable";
            this.menuTerminal.Size = new System.Drawing.Size(136, 26);
            // 
            // menuMonitorizar
            // 
            this.menuMonitorizar.Name = "menuMonitorizar";
            this.menuMonitorizar.Size = new System.Drawing.Size(171, 22);
            this.menuMonitorizar.Text = "Monitorizar";
            // 
            // timerRefresco
            // 
            this.timerRefresco.Interval = 250;
            this.timerRefresco.Tick += new System.EventHandler(this.timerRefresco_Tick);
            // 
            // _FrmBase_Toolbars_Dock_Area_Top
            // 
            this._FrmBase_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FrmBase_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._FrmBase_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._FrmBase_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FrmBase_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(10, 10);
            this._FrmBase_Toolbars_Dock_Area_Top.Name = "_FrmBase_Toolbars_Dock_Area_Top";
            this._FrmBase_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(772, 0);
            this._FrmBase_Toolbars_Dock_Area_Top.ToolbarsManager = this.toolbarsManager;
            // 
            // _FrmBase_Toolbars_Dock_Area_Bottom
            // 
            this._FrmBase_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FrmBase_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._FrmBase_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._FrmBase_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FrmBase_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(10, 413);
            this._FrmBase_Toolbars_Dock_Area_Bottom.Name = "_FrmBase_Toolbars_Dock_Area_Bottom";
            this._FrmBase_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(772, 0);
            this._FrmBase_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.toolbarsManager;
            // 
            // _FrmBase_Toolbars_Dock_Area_Left
            // 
            this._FrmBase_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FrmBase_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._FrmBase_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._FrmBase_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FrmBase_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(10, 10);
            this._FrmBase_Toolbars_Dock_Area_Left.Name = "_FrmBase_Toolbars_Dock_Area_Left";
            this._FrmBase_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 403);
            this._FrmBase_Toolbars_Dock_Area_Left.ToolbarsManager = this.toolbarsManager;
            // 
            // _FrmBase_Toolbars_Dock_Area_Right
            // 
            this._FrmBase_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FrmBase_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._FrmBase_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._FrmBase_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FrmBase_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(782, 10);
            this._FrmBase_Toolbars_Dock_Area_Right.Name = "_FrmBase_Toolbars_Dock_Area_Right";
            this._FrmBase_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 403);
            this._FrmBase_Toolbars_Dock_Area_Right.ToolbarsManager = this.toolbarsManager;
            // 
            // FrmMonitorizacionIO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 466);
            this.Controls.Add(this._FrmBase_Toolbars_Dock_Area_Top);
            this.Controls.Add(this._FrmBase_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._FrmBase_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._FrmBase_Toolbars_Dock_Area_Right);
            this.ModoAperturaFormulario = Orbita.Controles.VA.ModoAperturaFormulario.Monitorizacion;
            this.MultiplesInstancias = true;
            this.Name = "FrmMonitorizacionIO";
            this.RecordarPosicion = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.Text = "Monitorización de Entradas / Salidas";
            this.Controls.SetChildIndex(this.pnlInferiorPadre, 0);
            this.Controls.SetChildIndex(this.pnlPanelPrincipalPadre, 0);
            this.Controls.SetChildIndex(this._FrmBase_Toolbars_Dock_Area_Right, 0);
            this.Controls.SetChildIndex(this._FrmBase_Toolbars_Dock_Area_Left, 0);
            this.Controls.SetChildIndex(this._FrmBase_Toolbars_Dock_Area_Bottom, 0);
            this.Controls.SetChildIndex(this._FrmBase_Toolbars_Dock_Area_Top, 0);
            this.pnlPanelPrincipalPadre.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grbTerminales)).EndInit();
            this.grbTerminales.ResumeLayout(false);
            this.pnlBarraBotones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.toolbarsManager)).EndInit();
            this.orbitaPanel1.ResumeLayout(false);
            this.menuTerminal.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Orbita.Controles.OrbitaGroupBox grbTerminales;
        private Orbita.Controles.OrbitaListView ListTerminales;
        private System.Windows.Forms.ColumnHeader CodVariable;
        private System.Windows.Forms.ColumnHeader Valor;
        private System.Windows.Forms.ColumnHeader DescVariable;
        private System.Windows.Forms.ColumnHeader HabilitadoVariable;
        private System.Windows.Forms.ColumnHeader Tipo;
        private Orbita.Controles.OrbitaPanel pnlBarraBotones;
        private Orbita.Controles.OrbitaPanel orbitaPanel1;
        private Orbita.Controles.OrbitaButton btnMonitorizar;
        private Infragistics.Win.Misc.UltraDropDownButton btnVistas;
        private System.Windows.Forms.ContextMenuStrip menuTerminal;
        private System.Windows.Forms.ToolStripMenuItem menuMonitorizar;
        private System.Windows.Forms.ImageList imageListSmall;
        private System.Windows.Forms.Timer timerRefresco;
        private System.Windows.Forms.ImageList imageListLarge;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager toolbarsManager;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _FrmBase_Toolbars_Dock_Area_Top;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _FrmBase_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _FrmBase_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _FrmBase_Toolbars_Dock_Area_Right;
    }
}