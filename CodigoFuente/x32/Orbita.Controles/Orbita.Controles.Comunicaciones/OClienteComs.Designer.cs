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
            this.lblCom = new System.Windows.Forms.Label();
            this.lblVarEscritura = new System.Windows.Forms.Label();
            this.txtValEscribir = new System.Windows.Forms.TextBox();
            this.txtCom = new System.Windows.Forms.TextBox();
            this.txtVarEscribir = new System.Windows.Forms.TextBox();
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
            this.gbDispositivo.SuspendLayout();
            this.gbVariables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLecturas)).BeginInit();
            this.pnlLecturas.SuspendLayout();
            this.gbCDato.SuspendLayout();
            this.pnlDispSup.SuspendLayout();
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
            this.gbDispositivo.Size = new System.Drawing.Size(807, 478);
            this.gbDispositivo.TabIndex = 32;
            this.gbDispositivo.TabStop = false;
            this.gbDispositivo.Text = "Dispositivo";
            // 
            // gbVariables
            // 
            this.gbVariables.Controls.Add(this.dataGridViewLecturas);
            this.gbVariables.Controls.Add(this.pnlLecturas);
            this.gbVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbVariables.Location = new System.Drawing.Point(10, 100);
            this.gbVariables.Name = "gbVariables";
            this.gbVariables.Padding = new System.Windows.Forms.Padding(10);
            this.gbVariables.Size = new System.Drawing.Size(787, 222);
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
            this.dataGridViewLecturas.Size = new System.Drawing.Size(767, 144);
            this.dataGridViewLecturas.TabIndex = 12;
            // 
            // pnlLecturas
            // 
            this.pnlLecturas.Controls.Add(this.btnLeerVariables);
            this.pnlLecturas.Controls.Add(this.btnLeerAlarmas);
            this.pnlLecturas.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLecturas.Location = new System.Drawing.Point(10, 23);
            this.pnlLecturas.Name = "pnlLecturas";
            this.pnlLecturas.Size = new System.Drawing.Size(767, 45);
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
            this.gbCDato.Location = new System.Drawing.Point(10, 322);
            this.gbCDato.Name = "gbCDato";
            this.gbCDato.Padding = new System.Windows.Forms.Padding(10);
            this.gbCDato.Size = new System.Drawing.Size(787, 146);
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
            this.listViewCDato.Size = new System.Drawing.Size(767, 113);
            this.listViewCDato.TabIndex = 13;
            this.listViewCDato.UseCompatibleStateImageBehavior = false;
            this.listViewCDato.View = System.Windows.Forms.View.List;
            // 
            // pnlDispSup
            // 
            this.pnlDispSup.Controls.Add(this.lblCom);
            this.pnlDispSup.Controls.Add(this.lblVarEscritura);
            this.pnlDispSup.Controls.Add(this.txtValEscribir);
            this.pnlDispSup.Controls.Add(this.txtCom);
            this.pnlDispSup.Controls.Add(this.txtVarEscribir);
            this.pnlDispSup.Controls.Add(this.lblVarLectura);
            this.pnlDispSup.Controls.Add(this.btnEscritura);
            this.pnlDispSup.Controls.Add(this.btnLectura);
            this.pnlDispSup.Controls.Add(this.txtValLeer);
            this.pnlDispSup.Controls.Add(this.txtVarLeer);
            this.pnlDispSup.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDispSup.Location = new System.Drawing.Point(10, 23);
            this.pnlDispSup.Name = "pnlDispSup";
            this.pnlDispSup.Size = new System.Drawing.Size(787, 77);
            this.pnlDispSup.TabIndex = 26;
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
            // lblVarEscritura
            // 
            this.lblVarEscritura.AutoSize = true;
            this.lblVarEscritura.Location = new System.Drawing.Point(214, 46);
            this.lblVarEscritura.Name = "lblVarEscritura";
            this.lblVarEscritura.Size = new System.Drawing.Size(92, 13);
            this.lblVarEscritura.TabIndex = 11;
            this.lblVarEscritura.Text = "Variable Escritura:";
            // 
            // txtValEscribir
            // 
            this.txtValEscribir.Location = new System.Drawing.Point(511, 43);
            this.txtValEscribir.Name = "txtValEscribir";
            this.txtValEscribir.Size = new System.Drawing.Size(100, 20);
            this.txtValEscribir.TabIndex = 5;
            // 
            // txtCom
            // 
            this.txtCom.Location = new System.Drawing.Point(106, 17);
            this.txtCom.Name = "txtCom";
            this.txtCom.Size = new System.Drawing.Size(100, 20);
            this.txtCom.TabIndex = 15;
            // 
            // txtVarEscribir
            // 
            this.txtVarEscribir.Location = new System.Drawing.Point(309, 43);
            this.txtVarEscribir.Name = "txtVarEscribir";
            this.txtVarEscribir.Size = new System.Drawing.Size(100, 20);
            this.txtVarEscribir.TabIndex = 4;
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
            this.btnEscritura.Location = new System.Drawing.Point(418, 43);
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
            this.gbConfig.Size = new System.Drawing.Size(807, 65);
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
            // OClienteComs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbDispositivo);
            this.Controls.Add(this.gbConfig);
            this.Name = "OClienteComs";
            this.Size = new System.Drawing.Size(807, 543);
            this.gbDispositivo.ResumeLayout(false);
            this.gbVariables.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLecturas)).EndInit();
            this.pnlLecturas.ResumeLayout(false);
            this.gbCDato.ResumeLayout(false);
            this.pnlDispSup.ResumeLayout(false);
            this.pnlDispSup.PerformLayout();
            this.gbConfig.ResumeLayout(false);
            this.gbConfig.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbVariables;
        private System.Windows.Forms.DataGridView dataGridViewLecturas;
        private System.Windows.Forms.Panel pnlLecturas;
        private System.Windows.Forms.Button btnLeerVariables;
        private System.Windows.Forms.Button btnLeerAlarmas;
        private System.Windows.Forms.GroupBox gbCDato;
        private System.Windows.Forms.ListView listViewCDato;
        private System.Windows.Forms.Panel pnlDispSup;
        private System.Windows.Forms.Label lblVarEscritura;
        private System.Windows.Forms.TextBox txtValEscribir;
        private System.Windows.Forms.Label lblVarLectura;
        private System.Windows.Forms.Button btnEscritura;
        private System.Windows.Forms.Button btnLectura;
        private System.Windows.Forms.TextBox txtValLeer;
        private System.Windows.Forms.TextBox txtServidorRemoting;
        private System.Windows.Forms.Label lblServidorRemoting;
        private System.Windows.Forms.TextBox txtIdDispositivo;
        private System.Windows.Forms.Label lblIdDispositivo;
        private System.Windows.Forms.Button btnConectar;
        private System.Windows.Forms.TextBox txtPuertoRemoting;
        private System.Windows.Forms.Label lblPuertoRemoting;
        public System.Windows.Forms.GroupBox gbDispositivo;
        public System.Windows.Forms.Label lblCom;
        public System.Windows.Forms.TextBox txtCom;
        public System.Windows.Forms.GroupBox gbConfig;
        public System.Windows.Forms.TextBox txtVarEscribir;
        public System.Windows.Forms.TextBox txtVarLeer;
    }
}
