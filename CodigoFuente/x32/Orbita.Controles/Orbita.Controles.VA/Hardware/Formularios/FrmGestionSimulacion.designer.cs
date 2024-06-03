using Orbita.VAComun;
using Orbita.VAHardware;
namespace Orbita.Controles.VA
{
    partial class FrmGestionSimulacion
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.pageGestion = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.pnlFormulario = new Orbita.Controles.OrbitaPanel();
            this.lblFiltro = new System.Windows.Forms.Label();
            this.txtFiltro = new System.Windows.Forms.TextBox();
            this.BindingSimulacionCamara = new System.Windows.Forms.BindingSource(this.components);
            this.lblIntervaloEntreSnaps = new Orbita.Controles.OrbitaLabel(this.components);
            this.lblRutaFotografias = new Orbita.Controles.OrbitaLabel(this.components);
            this.lblTipoSimulacion = new Orbita.Controles.OrbitaLabel(this.components);
            this.lblMsIntervaloEntreSnaps = new Orbita.Controles.OrbitaLabel(this.components);
            this.lblSimulacion = new Orbita.Controles.OrbitaLabel(this.components);
            this.checkSimulacion = new Orbita.Controles.OrbitaCheckBox();
            this.txtRutaFotografias = new Orbita.Controles.OrbitaTextBox(this.components);
            this.txtIntervaloEntreSnaps = new Orbita.Controles.OrbitaTextBox(this.components);
            this.btnDialogoRuta = new Orbita.Controles.OrbitaButton(this.components);
            this.cboTipoSimulacion = new System.Windows.Forms.ComboBox();
            this.bindingTipoSimulacion = new System.Windows.Forms.BindingSource(this.components);
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.gridMaster = new System.Windows.Forms.DataGridView();
            this.codigoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.simulacionDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tipoSimulacionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.intervaloEntreSnapsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rutaFotografiasDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabGestion = new Orbita.Controles.OrbitaTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.pnlPanelPrincipalPadre.SuspendLayout();
            this.pageGestion.SuspendLayout();
            this.pnlFormulario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSimulacionCamara)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRutaFotografias)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIntervaloEntreSnaps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingTipoSimulacion)).BeginInit();
            this.ultraTabPageControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMaster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabGestion)).BeginInit();
            this.tabGestion.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlInferiorPadre
            // 
            this.pnlInferiorPadre.Location = new System.Drawing.Point(10, 232);
            this.pnlInferiorPadre.Size = new System.Drawing.Size(652, 43);
            // 
            // pnlPanelPrincipalPadre
            // 
            this.pnlPanelPrincipalPadre.Controls.Add(this.tabGestion);
            this.pnlPanelPrincipalPadre.Size = new System.Drawing.Size(652, 222);
            // 
            // pageGestion
            // 
            this.pageGestion.Controls.Add(this.pnlFormulario);
            this.pageGestion.Location = new System.Drawing.Point(1, 23);
            this.pageGestion.Name = "pageGestion";
            this.pageGestion.Padding = new System.Windows.Forms.Padding(5);
            this.pageGestion.Size = new System.Drawing.Size(648, 196);
            // 
            // pnlFormulario
            // 
            this.pnlFormulario.AutoScroll = true;
            this.pnlFormulario.Controls.Add(this.lblFiltro);
            this.pnlFormulario.Controls.Add(this.txtFiltro);
            this.pnlFormulario.Controls.Add(this.lblIntervaloEntreSnaps);
            this.pnlFormulario.Controls.Add(this.lblRutaFotografias);
            this.pnlFormulario.Controls.Add(this.lblTipoSimulacion);
            this.pnlFormulario.Controls.Add(this.lblMsIntervaloEntreSnaps);
            this.pnlFormulario.Controls.Add(this.lblSimulacion);
            this.pnlFormulario.Controls.Add(this.checkSimulacion);
            this.pnlFormulario.Controls.Add(this.txtRutaFotografias);
            this.pnlFormulario.Controls.Add(this.txtIntervaloEntreSnaps);
            this.pnlFormulario.Controls.Add(this.btnDialogoRuta);
            this.pnlFormulario.Controls.Add(this.cboTipoSimulacion);
            this.pnlFormulario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFormulario.Location = new System.Drawing.Point(5, 5);
            this.pnlFormulario.Name = "pnlFormulario";
            this.pnlFormulario.Size = new System.Drawing.Size(638, 186);
            this.pnlFormulario.TabIndex = 11;
            // 
            // lblFiltro
            // 
            this.lblFiltro.AutoSize = true;
            this.lblFiltro.Location = new System.Drawing.Point(106, 123);
            this.lblFiltro.Name = "lblFiltro";
            this.lblFiltro.Size = new System.Drawing.Size(32, 13);
            this.lblFiltro.TabIndex = 28;
            this.lblFiltro.Text = "Filtro:";
            this.lblFiltro.Visible = false;
            // 
            // txtFiltro
            // 
            this.txtFiltro.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.BindingSimulacionCamara, "Filtro", true));
            this.txtFiltro.Location = new System.Drawing.Point(144, 120);
            this.txtFiltro.Name = "txtFiltro";
            this.txtFiltro.Size = new System.Drawing.Size(100, 20);
            this.txtFiltro.TabIndex = 27;
            this.txtFiltro.Visible = false;
            // 
            // BindingSimulacionCamara
            // 
            this.BindingSimulacionCamara.DataSource = typeof(SimulacionCamara);
            // 
            // lblIntervaloEntreSnaps
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.ForeColor = System.Drawing.Color.Black;
            appearance1.TextHAlignAsString = "Right";
            this.lblIntervaloEntreSnaps.Appearance = appearance1;
            this.lblIntervaloEntreSnaps.Location = new System.Drawing.Point(16, 149);
            this.lblIntervaloEntreSnaps.Name = "lblIntervaloEntreSnaps";
            this.lblIntervaloEntreSnaps.Size = new System.Drawing.Size(122, 15);
            this.lblIntervaloEntreSnaps.TabIndex = 26;
            this.lblIntervaloEntreSnaps.Text = "Intervalo Entre Snaps:";
            this.lblIntervaloEntreSnaps.UseMnemonic = false;
            this.lblIntervaloEntreSnaps.Visible = false;
            // 
            // lblRutaFotografias
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.ForeColor = System.Drawing.Color.Black;
            appearance2.TextHAlignAsString = "Right";
            this.lblRutaFotografias.Appearance = appearance2;
            this.lblRutaFotografias.Location = new System.Drawing.Point(16, 80);
            this.lblRutaFotografias.Name = "lblRutaFotografias";
            this.lblRutaFotografias.Size = new System.Drawing.Size(122, 15);
            this.lblRutaFotografias.TabIndex = 25;
            this.lblRutaFotografias.Text = "Ruta Fotografias:";
            this.lblRutaFotografias.UseMnemonic = false;
            this.lblRutaFotografias.Visible = false;
            // 
            // lblTipoSimulacion
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Right";
            this.lblTipoSimulacion.Appearance = appearance3;
            this.lblTipoSimulacion.Location = new System.Drawing.Point(16, 53);
            this.lblTipoSimulacion.Name = "lblTipoSimulacion";
            this.lblTipoSimulacion.Size = new System.Drawing.Size(122, 15);
            this.lblTipoSimulacion.TabIndex = 24;
            this.lblTipoSimulacion.Text = "Tipo Simulacion:";
            this.lblTipoSimulacion.UseMnemonic = false;
            this.lblTipoSimulacion.Visible = false;
            // 
            // lblMsIntervaloEntreSnaps
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.ForeColor = System.Drawing.Color.Black;
            appearance4.TextHAlignAsString = "Left";
            this.lblMsIntervaloEntreSnaps.Appearance = appearance4;
            this.lblMsIntervaloEntreSnaps.Location = new System.Drawing.Point(250, 149);
            this.lblMsIntervaloEntreSnaps.Name = "lblMsIntervaloEntreSnaps";
            this.lblMsIntervaloEntreSnaps.Size = new System.Drawing.Size(122, 15);
            this.lblMsIntervaloEntreSnaps.TabIndex = 23;
            this.lblMsIntervaloEntreSnaps.Text = "ms";
            this.lblMsIntervaloEntreSnaps.UseMnemonic = false;
            this.lblMsIntervaloEntreSnaps.Visible = false;
            // 
            // lblSimulacion
            // 
            appearance5.BackColor = System.Drawing.Color.Transparent;
            appearance5.ForeColor = System.Drawing.Color.Black;
            appearance5.TextHAlignAsString = "Right";
            this.lblSimulacion.Appearance = appearance5;
            this.lblSimulacion.Location = new System.Drawing.Point(16, 26);
            this.lblSimulacion.Name = "lblSimulacion";
            this.lblSimulacion.Size = new System.Drawing.Size(122, 15);
            this.lblSimulacion.TabIndex = 23;
            this.lblSimulacion.Text = "Simulación:";
            this.lblSimulacion.UseMnemonic = false;
            // 
            // checkSimulacion
            // 
            this.checkSimulacion.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.BindingSimulacionCamara, "Simulacion", true));
            this.checkSimulacion.Location = new System.Drawing.Point(144, 24);
            this.checkSimulacion.Name = "checkSimulacion";
            this.checkSimulacion.Size = new System.Drawing.Size(120, 20);
            this.checkSimulacion.TabIndex = 22;
            this.checkSimulacion.CheckedChanged += new System.EventHandler(this.Simulacion_StateChanged);
            // 
            // txtRutaFotografias
            // 
            this.txtRutaFotografias.Appearance = null;
            this.txtRutaFotografias.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.BindingSimulacionCamara, "RutaFotografias", true));
            this.txtRutaFotografias.Font = new System.Drawing.Font("Franklin Gothic Book", 9F);
            this.txtRutaFotografias.Location = new System.Drawing.Point(144, 77);
            this.txtRutaFotografias.Name = "txtRutaFotografias";
            this.txtRutaFotografias.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRutaFotografias.Size = new System.Drawing.Size(439, 37);
            this.txtRutaFotografias.TabIndex = 21;
            this.txtRutaFotografias.Visible = false;
            // 
            // txtIntervaloEntreSnaps
            // 
            this.txtIntervaloEntreSnaps.Appearance = null;
            this.txtIntervaloEntreSnaps.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.BindingSimulacionCamara, "IntervaloEntreSnaps", true));
            this.txtIntervaloEntreSnaps.Font = new System.Drawing.Font("Franklin Gothic Book", 9F);
            this.txtIntervaloEntreSnaps.Location = new System.Drawing.Point(144, 146);
            this.txtIntervaloEntreSnaps.Name = "txtIntervaloEntreSnaps";
            this.txtIntervaloEntreSnaps.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtIntervaloEntreSnaps.Size = new System.Drawing.Size(100, 21);
            this.txtIntervaloEntreSnaps.TabIndex = 20;
            this.txtIntervaloEntreSnaps.Visible = false;
            // 
            // btnDialogoRuta
            // 
            this.btnDialogoRuta.Location = new System.Drawing.Point(589, 76);
            this.btnDialogoRuta.Name = "btnDialogoRuta";
            this.btnDialogoRuta.Size = new System.Drawing.Size(31, 23);
            this.btnDialogoRuta.TabIndex = 18;
            this.btnDialogoRuta.Text = "...";
            this.btnDialogoRuta.Visible = false;
            this.btnDialogoRuta.Click += new System.EventHandler(this.btnDialogoRuta_Click);
            // 
            // cboTipoSimulacion
            // 
            this.cboTipoSimulacion.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.BindingSimulacionCamara, "TipoSimulacion", true));
            this.cboTipoSimulacion.DataSource = this.bindingTipoSimulacion;
            this.cboTipoSimulacion.DisplayMember = "Descripcion";
            this.cboTipoSimulacion.FormattingEnabled = true;
            this.cboTipoSimulacion.Location = new System.Drawing.Point(144, 50);
            this.cboTipoSimulacion.Name = "cboTipoSimulacion";
            this.cboTipoSimulacion.Size = new System.Drawing.Size(216, 21);
            this.cboTipoSimulacion.TabIndex = 15;
            this.cboTipoSimulacion.ValueMember = "Enumerado";
            this.cboTipoSimulacion.Visible = false;
            this.cboTipoSimulacion.SelectedIndexChanged += new System.EventHandler(this.Simulacion_StateChanged);
            // 
            // bindingTipoSimulacion
            // 
            this.bindingTipoSimulacion.DataSource = typeof(EnumeracionCombo);
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.gridMaster);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Padding = new System.Windows.Forms.Padding(5);
            this.ultraTabPageControl2.Size = new System.Drawing.Size(648, 196);
            // 
            // gridMaster
            // 
            this.gridMaster.AutoGenerateColumns = false;
            this.gridMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMaster.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.codigoDataGridViewTextBoxColumn,
            this.simulacionDataGridViewCheckBoxColumn,
            this.tipoSimulacionDataGridViewTextBoxColumn,
            this.intervaloEntreSnapsDataGridViewTextBoxColumn,
            this.rutaFotografiasDataGridViewTextBoxColumn});
            this.gridMaster.DataSource = this.BindingSimulacionCamara;
            this.gridMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMaster.Location = new System.Drawing.Point(5, 5);
            this.gridMaster.Name = "gridMaster";
            this.gridMaster.Size = new System.Drawing.Size(638, 186);
            this.gridMaster.TabIndex = 0;
            // 
            // codigoDataGridViewTextBoxColumn
            // 
            this.codigoDataGridViewTextBoxColumn.DataPropertyName = "Codigo";
            this.codigoDataGridViewTextBoxColumn.HeaderText = "Código de la cámara";
            this.codigoDataGridViewTextBoxColumn.Name = "codigoDataGridViewTextBoxColumn";
            this.codigoDataGridViewTextBoxColumn.ReadOnly = true;
            this.codigoDataGridViewTextBoxColumn.Width = 150;
            // 
            // simulacionDataGridViewCheckBoxColumn
            // 
            this.simulacionDataGridViewCheckBoxColumn.DataPropertyName = "Simulacion";
            this.simulacionDataGridViewCheckBoxColumn.HeaderText = "Habilitar simulación";
            this.simulacionDataGridViewCheckBoxColumn.Name = "simulacionDataGridViewCheckBoxColumn";
            this.simulacionDataGridViewCheckBoxColumn.Width = 120;
            // 
            // tipoSimulacionDataGridViewTextBoxColumn
            // 
            this.tipoSimulacionDataGridViewTextBoxColumn.DataPropertyName = "TipoSimulacion";
            this.tipoSimulacionDataGridViewTextBoxColumn.DataSource = this.bindingTipoSimulacion;
            this.tipoSimulacionDataGridViewTextBoxColumn.DisplayMember = "Descripcion";
            this.tipoSimulacionDataGridViewTextBoxColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.tipoSimulacionDataGridViewTextBoxColumn.HeaderText = "Tipo";
            this.tipoSimulacionDataGridViewTextBoxColumn.Name = "tipoSimulacionDataGridViewTextBoxColumn";
            this.tipoSimulacionDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.tipoSimulacionDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.tipoSimulacionDataGridViewTextBoxColumn.ValueMember = "Enumerado";
            this.tipoSimulacionDataGridViewTextBoxColumn.Width = 170;
            // 
            // intervaloEntreSnapsDataGridViewTextBoxColumn
            // 
            this.intervaloEntreSnapsDataGridViewTextBoxColumn.DataPropertyName = "IntervaloEntreSnaps";
            this.intervaloEntreSnapsDataGridViewTextBoxColumn.HeaderText = "Intervalo entre snaps";
            this.intervaloEntreSnapsDataGridViewTextBoxColumn.Name = "intervaloEntreSnapsDataGridViewTextBoxColumn";
            this.intervaloEntreSnapsDataGridViewTextBoxColumn.Width = 130;
            // 
            // rutaFotografiasDataGridViewTextBoxColumn
            // 
            this.rutaFotografiasDataGridViewTextBoxColumn.DataPropertyName = "RutaFotografias";
            this.rutaFotografiasDataGridViewTextBoxColumn.HeaderText = "Ruta";
            this.rutaFotografiasDataGridViewTextBoxColumn.Name = "rutaFotografiasDataGridViewTextBoxColumn";
            this.rutaFotografiasDataGridViewTextBoxColumn.Width = 270;
            // 
            // tabGestion
            // 
            this.tabGestion.Controls.Add(this.ultraTabSharedControlsPage1);
            this.tabGestion.Controls.Add(this.pageGestion);
            this.tabGestion.Controls.Add(this.ultraTabPageControl2);
            this.tabGestion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabGestion.Location = new System.Drawing.Point(0, 0);
            this.tabGestion.Name = "tabGestion";
            this.tabGestion.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.tabGestion.Size = new System.Drawing.Size(652, 222);
            this.tabGestion.TabIndex = 2;
            ultraTab1.Key = "Formulario";
            ultraTab1.TabPage = this.pageGestion;
            ultraTab1.Text = "Formulario";
            ultraTab2.Key = "Tabla";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Tabla";
            this.tabGestion.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(648, 196);
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(652, 240);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(10, 10);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(652, 265);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Bitmap images (*.BMP)|*.BMP|Jpeg images(*.JPG)|*.JPG|Todos los archivos(*.*)|*.*";
            // 
            // FrmGestionSimulacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 285);
            this.Controls.Add(this.toolStripContainer1);
            this.ModoAperturaFormulario = Orbita.Controles.VA.ModoAperturaFormulario.Modificacion;
            this.Name = "FrmGestionSimulacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.Text = "Simulación de cámaras";
            this.Controls.SetChildIndex(this.toolStripContainer1, 0);
            this.Controls.SetChildIndex(this.pnlInferiorPadre, 0);
            this.Controls.SetChildIndex(this.pnlPanelPrincipalPadre, 0);
            this.pnlPanelPrincipalPadre.ResumeLayout(false);
            this.pageGestion.ResumeLayout(false);
            this.pnlFormulario.ResumeLayout(false);
            this.pnlFormulario.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BindingSimulacionCamara)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRutaFotografias)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIntervaloEntreSnaps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingTipoSimulacion)).EndInit();
            this.ultraTabPageControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridMaster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabGestion)).EndInit();
            this.tabGestion.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Orbita.Controles.OrbitaTabControl tabGestion;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl pageGestion;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private Orbita.Controles.OrbitaPanel pnlFormulario;
        private System.Windows.Forms.DataGridView gridMaster;
        private System.Windows.Forms.DataGridViewTextBoxColumn idVariableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn codVariableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombreVariableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn habilitadoVariableDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn grupoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn guardarTrazabilidadDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descVariableDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource BindingSimulacionCamara;
        private System.Windows.Forms.ComboBox cboTipoSimulacion;
        private Orbita.Controles.OrbitaButton btnDialogoRuta;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.BindingSource bindingTipoSimulacion;
        private Orbita.Controles.OrbitaTextBox txtIntervaloEntreSnaps;
        private Orbita.Controles.OrbitaTextBox txtRutaFotografias;
        private Orbita.Controles.OrbitaCheckBox checkSimulacion;
        private Orbita.Controles.OrbitaLabel lblSimulacion;
        private Orbita.Controles.OrbitaLabel lblIntervaloEntreSnaps;
        private Orbita.Controles.OrbitaLabel lblRutaFotografias;
        private Orbita.Controles.OrbitaLabel lblTipoSimulacion;
        private Orbita.Controles.OrbitaLabel lblMsIntervaloEntreSnaps;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn simulacionDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn tipoSimulacionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn intervaloEntreSnapsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rutaFotografiasDataGridViewTextBoxColumn;
        private System.Windows.Forms.TextBox txtFiltro;
        private System.Windows.Forms.Label lblFiltro;

    }
}