namespace Orbita.Controles.Comunicaciones
{
    partial class OClienteComs
    {
        /// <summary> 
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar 
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbDispositivo = new System.Windows.Forms.GroupBox();
            this.gbVariables = new System.Windows.Forms.GroupBox();
            this.dataGridViewLecturas = new System.Windows.Forms.DataGridView();
            this.pnlLecturas = new System.Windows.Forms.Panel();
            this.btnLeerVariables = new System.Windows.Forms.Button();
            this.btnLeerAlarmas = new System.Windows.Forms.Button();
            this.gbCDato = new System.Windows.Forms.GroupBox();
            this.listViewCDato = new System.Windows.Forms.ListView();
            this.pnlDispSup = new System.Windows.Forms.Panel();
            this.gbEscrituras = new System.Windows.Forms.GroupBox();
            this.dataGridViewEscrituras = new System.Windows.Forms.DataGridView();
            this.lblCom = new System.Windows.Forms.Label();
            this.txtCom = new System.Windows.Forms.TextBox();
            this.lblVarLectura = new System.Windows.Forms.Label();
            this.btnEscritura = new System.Windows.Forms.Button();
            this.btnLectura = new System.Windows.Forms.Button();
            this.txtValLeer = new System.Windows.Forms.TextBox();
            this.txtVarLeer = new System.Windows.Forms.TextBox();
            this.gbConfig = new System.Windows.Forms.GroupBox();
            this.txtServidorRemoting = new System.Windows.Forms.TextBox();
            this.lblServidorRemoting = new System.Windows.Forms.Label();
            this.txtIdDispositivo = new System.Windows.Forms.TextBox();
            this.lblIdDispositivo = new System.Windows.Forms.Label();
            this.btnConectar = new System.Windows.Forms.Button();
            this.txtPuertoRemoting = new System.Windows.Forms.TextBox();
            this.lblPuertoRemoting = new System.Windows.Forms.Label();
            this.lblTpoLect = new System.Windows.Forms.Label();
            this.lblTpoEsc = new System.Windows.Forms.Label();
            this.gbDispositivo.SuspendLayout();
            this.gbVariables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLecturas)).BeginInit();
            this.pnlLecturas.SuspendLayout();
            this.gbCDato.SuspendLayout();
            this.pnlDispSup.SuspendLayout();
            this.gbEscrituras.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEscrituras)).BeginInit();
            this.gbConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDispositivo
            // 
            this.gbDispositivo.Controls.Add(this.gbVariables);
            this.gbDispositivo.Controls.Add(this.gbCDato);
            this.gbDispositivo.Controls.Add(this.pnlDispSup);
            this.gbDispositivo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbDispositivo.Location = new System.Drawing.Point(0, 65);
            this.gbDispositivo.Name = "gbDispositivo";
            this.gbDispositivo.Padding = new System.Windows.Forms.Padding(10);
            this.gbDispositivo.Size = new System.Drawing.Size(1020, 478);
            this.gbDispositivo.TabIndex = 32;
            this.gbDispositivo.TabStop = false;
            this.gbDispositivo.Text = "Dispositivo";
            // 
            // gbVariables
            // 
            this.gbVariables.Controls.Add(this.dataGridViewLecturas);
            this.gbVariables.Controls.Add(this.pnlLecturas);
            this.gbVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbVariables.Location = new System.Drawing.Point(10, 216);
            this.gbVariables.Name = "gbVariables";
            this.gbVariables.Padding = new System.Windows.Forms.Padding(10);
            this.gbVariables.Size = new System.Drawing.Size(1000, 144);
            this.gbVariables.TabIndex = 27;
            this.gbVariables.TabStop = false;
            this.gbVariables.Text = "Lecturas";
            // 
            // dataGridViewLecturas
            // 
            this.dataGridViewLecturas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLecturas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewLecturas.Location = new System.Drawing.Point(10, 68);
            this.dataGridViewLecturas.Name = "dataGridViewLecturas";
            this.dataGridViewLecturas.Size = new System.Drawing.Size(980, 66);
            this.dataGridViewLecturas.TabIndex = 12;
            // 
            // pnlLecturas
            // 
            this.pnlLecturas.Controls.Add(this.btnLeerVariables);
            this.pnlLecturas.Controls.Add(this.btnLeerAlarmas);
            this.pnlLecturas.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLecturas.Location = new System.Drawing.Point(10, 23);
            this.pnlLecturas.Name = "pnlLecturas";
            this.pnlLecturas.Size = new System.Drawing.Size(980, 45);
            this.pnlLecturas.TabIndex = 28;
            // 
            // btnLeerVariables
            // 
            this.btnLeerVariables.Location = new System.Drawing.Point(15, 14);
            this.btnLeerVariables.Name = "btnLeerVariables";
            this.btnLeerVariables.Size = new System.Drawing.Size(92, 23);
            this.btnLeerVariables.TabIndex = 14;
            this.btnLeerVariables.Text = "Leer Variables";
            this.btnLeerVariables.UseVisualStyleBackColor = true;
            this.btnLeerVariables.Click += new System.EventHandler(this.btnLeerVariables_Click);
            // 
            // btnLeerAlarmas
            // 
            this.btnLeerAlarmas.Location = new System.Drawing.Point(113, 14);
            this.btnLeerAlarmas.Name = "btnLeerAlarmas";
            this.btnLeerAlarmas.Size = new System.Drawing.Size(92, 23);
            this.btnLeerAlarmas.TabIndex = 25;
            this.btnLeerAlarmas.Text = "Leer Alarmas";
            this.btnLeerAlarmas.UseVisualStyleBackColor = true;
            this.btnLeerAlarmas.Click += new System.EventHandler(this.btnLeerAlarmas_Click);
            // 
            // gbCDato
            // 
            this.gbCDato.Controls.Add(this.listViewCDato);
            this.gbCDato.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbCDato.Location = new System.Drawing.Point(10, 360);
            this.gbCDato.Name = "gbCDato";
            this.gbCDato.Padding = new System.Windows.Forms.Padding(10);
            this.gbCDato.Size = new System.Drawing.Size(1000, 108);
            this.gbCDato.TabIndex = 28;
            this.gbCDato.TabStop = false;
            this.gbCDato.Text = "Cambio Dato";
            // 
            // listViewCDato
            // 
            this.listViewCDato.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewCDato.GridLines = true;
            this.listViewCDato.Location = new System.Drawing.Point(10, 23);
            this.listViewCDato.MultiSelect = false;
            this.listViewCDato.Name = "listViewCDato";
            this.listViewCDato.Size = new System.Drawing.Size(980, 75);
            this.listViewCDato.TabIndex = 13;
            this.listViewCDato.UseCompatibleStateImageBehavior = false;
            this.listViewCDato.View = System.Windows.Forms.View.List;
            // 
            // pnlDispSup
            // 
            this.pnlDispSup.Controls.Add(this.lblTpoEsc);
            this.pnlDispSup.Controls.Add(this.lblTpoLect);
            this.pnlDispSup.Controls.Add(this.gbEscrituras);
            this.pnlDispSup.Controls.Add(this.lblCom);
            this.pnlDispSup.Controls.Add(this.txtCom);
            this.pnlDispSup.Controls.Add(this.lblVarLectura);
            this.pnlDispSup.Controls.Add(this.btnEscritura);
            this.pnlDispSup.Controls.Add(this.btnLectura);
            this.pnlDispSup.Controls.Add(this.txtValLeer);
            this.pnlDispSup.Controls.Add(this.txtVarLeer);
            this.pnlDispSup.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDispSup.Location = new System.Drawing.Point(10, 23);
            this.pnlDispSup.Name = "pnlDispSup";
            this.pnlDispSup.Size = new System.Drawing.Size(1000, 193);
            this.pnlDispSup.TabIndex = 26;
            // 
            // gbEscrituras
            // 
            this.gbEscrituras.Controls.Add(this.dataGridViewEscrituras);
            this.gbEscrituras.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbEscrituras.Location = new System.Drawing.Point(0, 146);
            this.gbEscrituras.Name = "gbEscrituras";
            this.gbEscrituras.Size = new System.Drawing.Size(1000, 47);
            this.gbEscrituras.TabIndex = 17;
            this.gbEscrituras.TabStop = false;
            this.gbEscrituras.Text = "Escrituras";
            // 
            // dataGridViewEscrituras
            // 
            this.dataGridViewEscrituras.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEscrituras.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewEscrituras.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewEscrituras.Name = "dataGridViewEscrituras";
            this.dataGridViewEscrituras.Size = new System.Drawing.Size(994, 28);
            this.dataGridViewEscrituras.TabIndex = 0;
            // 
            // lblCom
            // 
            this.lblCom.AutoSize = true;
            this.lblCom.Location = new System.Drawing.Point(12, 20);
            this.lblCom.Name = "lblCom";
            this.lblCom.Size = new System.Drawing.Size(88, 13);
            this.lblCom.TabIndex = 16;
            this.lblCom.Text = "Comunicaciones:";
            // 
            // txtCom
            // 
            this.txtCom.Location = new System.Drawing.Point(106, 17);
            this.txtCom.Name = "txtCom";
            this.txtCom.Size = new System.Drawing.Size(100, 20);
            this.txtCom.TabIndex = 15;
            // 
            // lblVarLectura
            // 
            this.lblVarLectura.AutoSize = true;
            this.lblVarLectura.Location = new System.Drawing.Point(219, 20);
            this.lblVarLectura.Name = "lblVarLectura";
            this.lblVarLectura.Size = new System.Drawing.Size(87, 13);
            this.lblVarLectura.TabIndex = 10;
            this.lblVarLectura.Text = "Variable Lectura:";
            // 
            // btnEscritura
            // 
            this.btnEscritura.Location = new System.Drawing.Point(706, 15);
            this.btnEscritura.Name = "btnEscritura";
            this.btnEscritura.Size = new System.Drawing.Size(75, 23);
            this.btnEscritura.TabIndex = 3;
            this.btnEscritura.Text = "escribir";
            this.btnEscritura.UseVisualStyleBackColor = true;
            this.btnEscritura.Click += new System.EventHandler(this.btnEscritura_Click);
            // 
            // btnLectura
            // 
            this.btnLectura.Location = new System.Drawing.Point(418, 15);
            this.btnLectura.Name = "btnLectura";
            this.btnLectura.Size = new System.Drawing.Size(75, 23);
            this.btnLectura.TabIndex = 0;
            this.btnLectura.Text = "leer";
            this.btnLectura.UseVisualStyleBackColor = true;
            this.btnLectura.Click += new System.EventHandler(this.btnLectura_Click);
            // 
            // txtValLeer
            // 
            this.txtValLeer.Location = new System.Drawing.Point(511, 17);
            this.txtValLeer.Name = "txtValLeer";
            this.txtValLeer.Size = new System.Drawing.Size(100, 20);
            this.txtValLeer.TabIndex = 2;
            // 
            // txtVarLeer
            // 
            this.txtVarLeer.Location = new System.Drawing.Point(309, 17);
            this.txtVarLeer.Name = "txtVarLeer";
            this.txtVarLeer.Size = new System.Drawing.Size(100, 20);
            this.txtVarLeer.TabIndex = 1;
            // 
            // gbConfig
            // 
            this.gbConfig.Controls.Add(this.txtServidorRemoting);
            this.gbConfig.Controls.Add(this.lblServidorRemoting);
            this.gbConfig.Controls.Add(this.txtIdDispositivo);
            this.gbConfig.Controls.Add(this.lblIdDispositivo);
            this.gbConfig.Controls.Add(this.btnConectar);
            this.gbConfig.Controls.Add(this.txtPuertoRemoting);
            this.gbConfig.Controls.Add(this.lblPuertoRemoting);
            this.gbConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbConfig.Location = new System.Drawing.Point(0, 0);
            this.gbConfig.Name = "gbConfig";
            this.gbConfig.Size = new System.Drawing.Size(1020, 65);
            this.gbConfig.TabIndex = 31;
            this.gbConfig.TabStop = false;
            this.gbConfig.Text = "Configuración";
            // 
            // txtServidorRemoting
            // 
            this.txtServidorRemoting.Location = new System.Drawing.Point(521, 25);
            this.txtServidorRemoting.Name = "txtServidorRemoting";
            this.txtServidorRemoting.Size = new System.Drawing.Size(100, 20);
            this.txtServidorRemoting.TabIndex = 27;
            this.txtServidorRemoting.Text = "localhost";
            // 
            // lblServidorRemoting
            // 
            this.lblServidorRemoting.AutoSize = true;
            this.lblServidorRemoting.Location = new System.Drawing.Point(425, 29);
            this.lblServidorRemoting.Name = "lblServidorRemoting";
            this.lblServidorRemoting.Size = new System.Drawing.Size(97, 13);
            this.lblServidorRemoting.TabIndex = 26;
            this.lblServidorRemoting.Text = "Servidor Remoting:";
            // 
            // txtIdDispositivo
            // 
            this.txtIdDispositivo.Location = new System.Drawing.Point(319, 25);
            this.txtIdDispositivo.Name = "txtIdDispositivo";
            this.txtIdDispositivo.Size = new System.Drawing.Size(100, 20);
            this.txtIdDispositivo.TabIndex = 21;
            this.txtIdDispositivo.Text = "1";
            // 
            // lblIdDispositivo
            // 
            this.lblIdDispositivo.AutoSize = true;
            this.lblIdDispositivo.Location = new System.Drawing.Point(241, 29);
            this.lblIdDispositivo.Name = "lblIdDispositivo";
            this.lblIdDispositivo.Size = new System.Drawing.Size(75, 13);
            this.lblIdDispositivo.TabIndex = 20;
            this.lblIdDispositivo.Text = "ID Dispositivo:";
            // 
            // btnConectar
            // 
            this.btnConectar.Location = new System.Drawing.Point(668, 23);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new System.Drawing.Size(75, 23);
            this.btnConectar.TabIndex = 19;
            this.btnConectar.Text = "Conectar";
            this.btnConectar.UseVisualStyleBackColor = true;
            this.btnConectar.Click += new System.EventHandler(this.btnConectar_Click);
            // 
            // txtPuertoRemoting
            // 
            this.txtPuertoRemoting.Location = new System.Drawing.Point(116, 25);
            this.txtPuertoRemoting.Name = "txtPuertoRemoting";
            this.txtPuertoRemoting.Size = new System.Drawing.Size(100, 20);
            this.txtPuertoRemoting.TabIndex = 1;
            this.txtPuertoRemoting.Text = "1852";
            // 
            // lblPuertoRemoting
            // 
            this.lblPuertoRemoting.AutoSize = true;
            this.lblPuertoRemoting.Location = new System.Drawing.Point(22, 28);
            this.lblPuertoRemoting.Name = "lblPuertoRemoting";
            this.lblPuertoRemoting.Size = new System.Drawing.Size(89, 13);
            this.lblPuertoRemoting.TabIndex = 0;
            this.lblPuertoRemoting.Text = "Puerto Remoting:";
            // 
            // lblTpoLect
            // 
            this.lblTpoLect.AutoSize = true;
            this.lblTpoLect.Location = new System.Drawing.Point(617, 20);
            this.lblTpoLect.Name = "lblTpoLect";
            this.lblTpoLect.Size = new System.Drawing.Size(0, 13);
            this.lblTpoLect.TabIndex = 18;
            // 
            // lblTpoEsc
            // 
            this.lblTpoEsc.AutoSize = true;
            this.lblTpoEsc.Location = new System.Drawing.Point(796, 21);
            this.lblTpoEsc.Name = "lblTpoEsc";
            this.lblTpoEsc.Size = new System.Drawing.Size(0, 13);
            this.lblTpoEsc.TabIndex = 19;
            // 
            // OClienteComs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbDispositivo);
            this.Controls.Add(this.gbConfig);
            this.Name = "OClienteComs";
            this.Size = new System.Drawing.Size(1020, 543);
            this.Load += new System.EventHandler(this.OClienteComs_Load);
            this.gbDispositivo.ResumeLayout(false);
            this.gbVariables.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLecturas)).EndInit();
            this.pnlLecturas.ResumeLayout(false);
            this.gbCDato.ResumeLayout(false);
            this.pnlDispSup.ResumeLayout(false);
            this.pnlDispSup.PerformLayout();
            this.gbEscrituras.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEscrituras)).EndInit();
            this.gbConfig.ResumeLayout(false);
            this.gbConfig.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewLecturas;
        private System.Windows.Forms.Panel pnlLecturas;
        private System.Windows.Forms.Button btnLeerVariables;
        private System.Windows.Forms.Button btnLeerAlarmas;
        private System.Windows.Forms.ListView listViewCDato;
        private System.Windows.Forms.Label lblVarLectura;
        private System.Windows.Forms.Button btnEscritura;
        private System.Windows.Forms.Button btnLectura;
        private System.Windows.Forms.TextBox txtValLeer;
        private System.Windows.Forms.Label lblServidorRemoting;
        private System.Windows.Forms.Label lblIdDispositivo;
        private System.Windows.Forms.Button btnConectar;
        private System.Windows.Forms.Label lblPuertoRemoting;
        public System.Windows.Forms.GroupBox gbDispositivo;
        public System.Windows.Forms.Label lblCom;
        public System.Windows.Forms.TextBox txtCom;
        public System.Windows.Forms.GroupBox gbConfig;
        public System.Windows.Forms.TextBox txtVarLeer;
        public System.Windows.Forms.Panel pnlDispSup;
        public System.Windows.Forms.GroupBox gbVariables;
        public System.Windows.Forms.GroupBox gbCDato;
        private System.Windows.Forms.DataGridView dataGridViewEscrituras;
        public System.Windows.Forms.GroupBox gbEscrituras;
        public System.Windows.Forms.TextBox txtServidorRemoting;
        public System.Windows.Forms.TextBox txtIdDispositivo;
        public System.Windows.Forms.TextBox txtPuertoRemoting;
        private System.Windows.Forms.Label lblTpoEsc;
        private System.Windows.Forms.Label lblTpoLect;
    }
}
