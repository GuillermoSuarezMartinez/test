namespace Orbita.Controles.Comunicaciones
{
    partial class OClienteOrbita1
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
            this.Desconectar();
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar 
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbConfig = new System.Windows.Forms.GroupBox();
            this.txtServidorRemoting = new System.Windows.Forms.TextBox();
            this.lblServidorRemoting = new System.Windows.Forms.Label();
            this.txtIdDispositivo = new System.Windows.Forms.TextBox();
            this.lblIdDispositivo = new System.Windows.Forms.Label();
            this.btnConectar = new System.Windows.Forms.Button();
            this.txtPuertoRemoting = new System.Windows.Forms.TextBox();
            this.lblPuertoRemoting = new System.Windows.Forms.Label();
            this.gbOrbita = new System.Windows.Forms.GroupBox();
            this.gbOrbitaVariables = new System.Windows.Forms.GroupBox();
            this.dataGridViewOrbita = new System.Windows.Forms.DataGridView();
            this.pnlOrbitaLecturas = new System.Windows.Forms.Panel();
            this.btnLeerVariablesOrbita = new System.Windows.Forms.Button();
            this.btnLeerAlarmasOrbita = new System.Windows.Forms.Button();
            this.gbOrbitaCDato = new System.Windows.Forms.GroupBox();
            this.listViewOrbita = new System.Windows.Forms.ListView();
            this.pnlOrbitaSup = new System.Windows.Forms.Panel();
            this.lblDispositivoOrbita = new System.Windows.Forms.Label();
            this.lblEscrituraOrbita = new System.Windows.Forms.Label();
            this.txtValEscribirOrbita = new System.Windows.Forms.TextBox();
            this.txtOrbita = new System.Windows.Forms.TextBox();
            this.txtVarEscribirOrbita = new System.Windows.Forms.TextBox();
            this.lblLecturaOrbita = new System.Windows.Forms.Label();
            this.btnEscrituraOrbita = new System.Windows.Forms.Button();
            this.btnLecturaOrbita = new System.Windows.Forms.Button();
            this.txtVaLeerOrbita = new System.Windows.Forms.TextBox();
            this.txtVarLeerOrbita = new System.Windows.Forms.TextBox();
            this.gbConfig.SuspendLayout();
            this.gbOrbita.SuspendLayout();
            this.gbOrbitaVariables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOrbita)).BeginInit();
            this.pnlOrbitaLecturas.SuspendLayout();
            this.gbOrbitaCDato.SuspendLayout();
            this.pnlOrbitaSup.SuspendLayout();
            this.SuspendLayout();
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
            this.gbConfig.Location = new System.Drawing.Point(5, 5);
            this.gbConfig.Name = "gbConfig";
            this.gbConfig.Size = new System.Drawing.Size(648, 100);
            this.gbConfig.TabIndex = 1;
            this.gbConfig.TabStop = false;
            this.gbConfig.Text = "Configuración";
            // 
            // txtServidorRemoting
            // 
            this.txtServidorRemoting.Location = new System.Drawing.Point(116, 67);
            this.txtServidorRemoting.Name = "txtServidorRemoting";
            this.txtServidorRemoting.Size = new System.Drawing.Size(100, 20);
            this.txtServidorRemoting.TabIndex = 25;
            this.txtServidorRemoting.Text = "localhost";
            // 
            // lblServidorRemoting
            // 
            this.lblServidorRemoting.AutoSize = true;
            this.lblServidorRemoting.Location = new System.Drawing.Point(20, 71);
            this.lblServidorRemoting.Name = "lblServidorRemoting";
            this.lblServidorRemoting.Size = new System.Drawing.Size(97, 13);
            this.lblServidorRemoting.TabIndex = 24;
            this.lblServidorRemoting.Text = "Servidor Remoting:";
            // 
            // txtIdDispositivo
            // 
            this.txtIdDispositivo.Location = new System.Drawing.Point(313, 40);
            this.txtIdDispositivo.Name = "txtIdDispositivo";
            this.txtIdDispositivo.Size = new System.Drawing.Size(100, 20);
            this.txtIdDispositivo.TabIndex = 23;
            this.txtIdDispositivo.Text = "1";
            // 
            // lblIdDispositivo
            // 
            this.lblIdDispositivo.AutoSize = true;
            this.lblIdDispositivo.Location = new System.Drawing.Point(236, 44);
            this.lblIdDispositivo.Name = "lblIdDispositivo";
            this.lblIdDispositivo.Size = new System.Drawing.Size(75, 13);
            this.lblIdDispositivo.TabIndex = 22;
            this.lblIdDispositivo.Text = "ID Dispositivo:";
            // 
            // btnConectar
            // 
            this.btnConectar.Location = new System.Drawing.Point(513, 39);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new System.Drawing.Size(75, 23);
            this.btnConectar.TabIndex = 19;
            this.btnConectar.Text = "Conectar";
            this.btnConectar.UseVisualStyleBackColor = true;
            this.btnConectar.Click += new System.EventHandler(this.btnConectar_Click);
            // 
            // txtPuertoRemoting
            // 
            this.txtPuertoRemoting.Location = new System.Drawing.Point(116, 40);
            this.txtPuertoRemoting.Name = "txtPuertoRemoting";
            this.txtPuertoRemoting.Size = new System.Drawing.Size(100, 20);
            this.txtPuertoRemoting.TabIndex = 1;
            this.txtPuertoRemoting.Text = "1852";
            // 
            // lblPuertoRemoting
            // 
            this.lblPuertoRemoting.AutoSize = true;
            this.lblPuertoRemoting.Location = new System.Drawing.Point(20, 44);
            this.lblPuertoRemoting.Name = "lblPuertoRemoting";
            this.lblPuertoRemoting.Size = new System.Drawing.Size(89, 13);
            this.lblPuertoRemoting.TabIndex = 0;
            this.lblPuertoRemoting.Text = "Puerto Remoting:";
            // 
            // gbOrbita
            // 
            this.gbOrbita.Controls.Add(this.gbOrbitaVariables);
            this.gbOrbita.Controls.Add(this.gbOrbitaCDato);
            this.gbOrbita.Controls.Add(this.pnlOrbitaSup);
            this.gbOrbita.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbOrbita.Location = new System.Drawing.Point(5, 105);
            this.gbOrbita.Name = "gbOrbita";
            this.gbOrbita.Padding = new System.Windows.Forms.Padding(10);
            this.gbOrbita.Size = new System.Drawing.Size(648, 388);
            this.gbOrbita.TabIndex = 27;
            this.gbOrbita.TabStop = false;
            this.gbOrbita.Text = "Dispositivo Orbita";
            // 
            // gbOrbitaVariables
            // 
            this.gbOrbitaVariables.Controls.Add(this.dataGridViewOrbita);
            this.gbOrbitaVariables.Controls.Add(this.pnlOrbitaLecturas);
            this.gbOrbitaVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbOrbitaVariables.Location = new System.Drawing.Point(10, 100);
            this.gbOrbitaVariables.Name = "gbOrbitaVariables";
            this.gbOrbitaVariables.Padding = new System.Windows.Forms.Padding(10);
            this.gbOrbitaVariables.Size = new System.Drawing.Size(628, 132);
            this.gbOrbitaVariables.TabIndex = 27;
            this.gbOrbitaVariables.TabStop = false;
            this.gbOrbitaVariables.Text = "Lecturas";
            // 
            // dataGridViewOrbita
            // 
            this.dataGridViewOrbita.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOrbita.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewOrbita.Location = new System.Drawing.Point(10, 68);
            this.dataGridViewOrbita.Name = "dataGridViewOrbita";
            this.dataGridViewOrbita.Size = new System.Drawing.Size(608, 54);
            this.dataGridViewOrbita.TabIndex = 12;
            // 
            // pnlOrbitaLecturas
            // 
            this.pnlOrbitaLecturas.Controls.Add(this.btnLeerVariablesOrbita);
            this.pnlOrbitaLecturas.Controls.Add(this.btnLeerAlarmasOrbita);
            this.pnlOrbitaLecturas.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOrbitaLecturas.Location = new System.Drawing.Point(10, 23);
            this.pnlOrbitaLecturas.Name = "pnlOrbitaLecturas";
            this.pnlOrbitaLecturas.Size = new System.Drawing.Size(608, 45);
            this.pnlOrbitaLecturas.TabIndex = 28;
            // 
            // btnLeerVariablesOrbita
            // 
            this.btnLeerVariablesOrbita.Location = new System.Drawing.Point(15, 14);
            this.btnLeerVariablesOrbita.Name = "btnLeerVariablesOrbita";
            this.btnLeerVariablesOrbita.Size = new System.Drawing.Size(92, 23);
            this.btnLeerVariablesOrbita.TabIndex = 14;
            this.btnLeerVariablesOrbita.Text = "Leer Variables";
            this.btnLeerVariablesOrbita.UseVisualStyleBackColor = true;
            this.btnLeerVariablesOrbita.Click += new System.EventHandler(this.btnLeerVariablesOPC_Click);
            // 
            // btnLeerAlarmasOrbita
            // 
            this.btnLeerAlarmasOrbita.Location = new System.Drawing.Point(113, 14);
            this.btnLeerAlarmasOrbita.Name = "btnLeerAlarmasOrbita";
            this.btnLeerAlarmasOrbita.Size = new System.Drawing.Size(92, 23);
            this.btnLeerAlarmasOrbita.TabIndex = 25;
            this.btnLeerAlarmasOrbita.Text = "Leer Alarmas";
            this.btnLeerAlarmasOrbita.UseVisualStyleBackColor = true;
            this.btnLeerAlarmasOrbita.Click += new System.EventHandler(this.btnLeerAlarmasOPC_Click);
            // 
            // gbOrbitaCDato
            // 
            this.gbOrbitaCDato.Controls.Add(this.listViewOrbita);
            this.gbOrbitaCDato.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbOrbitaCDato.Location = new System.Drawing.Point(10, 232);
            this.gbOrbitaCDato.Name = "gbOrbitaCDato";
            this.gbOrbitaCDato.Padding = new System.Windows.Forms.Padding(10);
            this.gbOrbitaCDato.Size = new System.Drawing.Size(628, 146);
            this.gbOrbitaCDato.TabIndex = 28;
            this.gbOrbitaCDato.TabStop = false;
            this.gbOrbitaCDato.Text = "Cambio Dato";
            // 
            // listViewOrbita
            // 
            this.listViewOrbita.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewOrbita.GridLines = true;
            this.listViewOrbita.Location = new System.Drawing.Point(10, 23);
            this.listViewOrbita.MultiSelect = false;
            this.listViewOrbita.Name = "listViewOrbita";
            this.listViewOrbita.Size = new System.Drawing.Size(608, 113);
            this.listViewOrbita.TabIndex = 13;
            this.listViewOrbita.UseCompatibleStateImageBehavior = false;
            this.listViewOrbita.View = System.Windows.Forms.View.List;
            // 
            // pnlOrbitaSup
            // 
            this.pnlOrbitaSup.Controls.Add(this.lblDispositivoOrbita);
            this.pnlOrbitaSup.Controls.Add(this.lblEscrituraOrbita);
            this.pnlOrbitaSup.Controls.Add(this.txtValEscribirOrbita);
            this.pnlOrbitaSup.Controls.Add(this.txtOrbita);
            this.pnlOrbitaSup.Controls.Add(this.txtVarEscribirOrbita);
            this.pnlOrbitaSup.Controls.Add(this.lblLecturaOrbita);
            this.pnlOrbitaSup.Controls.Add(this.btnEscrituraOrbita);
            this.pnlOrbitaSup.Controls.Add(this.btnLecturaOrbita);
            this.pnlOrbitaSup.Controls.Add(this.txtVaLeerOrbita);
            this.pnlOrbitaSup.Controls.Add(this.txtVarLeerOrbita);
            this.pnlOrbitaSup.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOrbitaSup.Location = new System.Drawing.Point(10, 23);
            this.pnlOrbitaSup.Name = "pnlOrbitaSup";
            this.pnlOrbitaSup.Size = new System.Drawing.Size(628, 77);
            this.pnlOrbitaSup.TabIndex = 26;
            // 
            // lblDispositivoOrbita
            // 
            this.lblDispositivoOrbita.AutoSize = true;
            this.lblDispositivoOrbita.Location = new System.Drawing.Point(27, 20);
            this.lblDispositivoOrbita.Name = "lblDispositivoOrbita";
            this.lblDispositivoOrbita.Size = new System.Drawing.Size(35, 13);
            this.lblDispositivoOrbita.TabIndex = 16;
            this.lblDispositivoOrbita.Text = "Orbita";
            // 
            // lblEscrituraOrbita
            // 
            this.lblEscrituraOrbita.AutoSize = true;
            this.lblEscrituraOrbita.Location = new System.Drawing.Point(198, 46);
            this.lblEscrituraOrbita.Name = "lblEscrituraOrbita";
            this.lblEscrituraOrbita.Size = new System.Drawing.Size(85, 13);
            this.lblEscrituraOrbita.TabIndex = 11;
            this.lblEscrituraOrbita.Text = "Prueba Escritura";
            // 
            // txtValEscribirOrbita
            // 
            this.txtValEscribirOrbita.Location = new System.Drawing.Point(503, 43);
            this.txtValEscribirOrbita.Name = "txtValEscribirOrbita";
            this.txtValEscribirOrbita.Size = new System.Drawing.Size(112, 20);
            this.txtValEscribirOrbita.TabIndex = 5;
            // 
            // txtOrbita
            // 
            this.txtOrbita.Location = new System.Drawing.Point(68, 17);
            this.txtOrbita.Name = "txtOrbita";
            this.txtOrbita.Size = new System.Drawing.Size(112, 20);
            this.txtOrbita.TabIndex = 15;
            // 
            // txtVarEscribirOrbita
            // 
            this.txtVarEscribirOrbita.Location = new System.Drawing.Point(291, 43);
            this.txtVarEscribirOrbita.Name = "txtVarEscribirOrbita";
            this.txtVarEscribirOrbita.Size = new System.Drawing.Size(112, 20);
            this.txtVarEscribirOrbita.TabIndex = 4;
            this.txtVarEscribirOrbita.Text = "stringOrbita1";
            // 
            // lblLecturaOrbita
            // 
            this.lblLecturaOrbita.AutoSize = true;
            this.lblLecturaOrbita.Location = new System.Drawing.Point(198, 20);
            this.lblLecturaOrbita.Name = "lblLecturaOrbita";
            this.lblLecturaOrbita.Size = new System.Drawing.Size(80, 13);
            this.lblLecturaOrbita.TabIndex = 10;
            this.lblLecturaOrbita.Text = "Prueba Lectura";
            // 
            // btnEscrituraOrbita
            // 
            this.btnEscrituraOrbita.Location = new System.Drawing.Point(415, 41);
            this.btnEscrituraOrbita.Name = "btnEscrituraOrbita";
            this.btnEscrituraOrbita.Size = new System.Drawing.Size(75, 23);
            this.btnEscrituraOrbita.TabIndex = 3;
            this.btnEscrituraOrbita.Text = "escribir";
            this.btnEscrituraOrbita.UseVisualStyleBackColor = true;
            this.btnEscrituraOrbita.Click += new System.EventHandler(this.btnEscrituraOPC_Click);
            // 
            // btnLecturaOrbita
            // 
            this.btnLecturaOrbita.Location = new System.Drawing.Point(415, 15);
            this.btnLecturaOrbita.Name = "btnLecturaOrbita";
            this.btnLecturaOrbita.Size = new System.Drawing.Size(75, 23);
            this.btnLecturaOrbita.TabIndex = 0;
            this.btnLecturaOrbita.Text = "leer";
            this.btnLecturaOrbita.UseVisualStyleBackColor = true;
            this.btnLecturaOrbita.Click += new System.EventHandler(this.btnLecturaOPC_Click);
            // 
            // txtVaLeerOrbita
            // 
            this.txtVaLeerOrbita.Location = new System.Drawing.Point(503, 17);
            this.txtVaLeerOrbita.Name = "txtVaLeerOrbita";
            this.txtVaLeerOrbita.Size = new System.Drawing.Size(112, 20);
            this.txtVaLeerOrbita.TabIndex = 2;
            // 
            // txtVarLeerOrbita
            // 
            this.txtVarLeerOrbita.Location = new System.Drawing.Point(291, 17);
            this.txtVarLeerOrbita.Name = "txtVarLeerOrbita";
            this.txtVarLeerOrbita.Size = new System.Drawing.Size(112, 20);
            this.txtVarLeerOrbita.TabIndex = 1;
            this.txtVarLeerOrbita.Text = "stringOrbita1";
            // 
            // OClienteOrbita
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbOrbita);
            this.Controls.Add(this.gbConfig);
            this.Name = "OClienteOrbita";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(658, 498);
            this.gbConfig.ResumeLayout(false);
            this.gbConfig.PerformLayout();
            this.gbOrbita.ResumeLayout(false);
            this.gbOrbitaVariables.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOrbita)).EndInit();
            this.pnlOrbitaLecturas.ResumeLayout(false);
            this.gbOrbitaCDato.ResumeLayout(false);
            this.pnlOrbitaSup.ResumeLayout(false);
            this.pnlOrbitaSup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbConfig;
        private System.Windows.Forms.Button btnConectar;
        private System.Windows.Forms.TextBox txtPuertoRemoting;
        private System.Windows.Forms.Label lblPuertoRemoting;
        private System.Windows.Forms.GroupBox gbOrbita;
        private System.Windows.Forms.GroupBox gbOrbitaVariables;
        private System.Windows.Forms.DataGridView dataGridViewOrbita;
        private System.Windows.Forms.Panel pnlOrbitaLecturas;
        private System.Windows.Forms.Button btnLeerVariablesOrbita;
        private System.Windows.Forms.Button btnLeerAlarmasOrbita;
        private System.Windows.Forms.GroupBox gbOrbitaCDato;
        private System.Windows.Forms.ListView listViewOrbita;
        private System.Windows.Forms.Panel pnlOrbitaSup;
        private System.Windows.Forms.Label lblDispositivoOrbita;
        private System.Windows.Forms.Label lblEscrituraOrbita;
        private System.Windows.Forms.TextBox txtValEscribirOrbita;
        private System.Windows.Forms.TextBox txtOrbita;
        private System.Windows.Forms.TextBox txtVarEscribirOrbita;
        private System.Windows.Forms.Label lblLecturaOrbita;
        private System.Windows.Forms.Button btnEscrituraOrbita;
        private System.Windows.Forms.Button btnLecturaOrbita;
        private System.Windows.Forms.TextBox txtVaLeerOrbita;
        private System.Windows.Forms.TextBox txtVarLeerOrbita;
        private System.Windows.Forms.TextBox txtIdDispositivo;
        private System.Windows.Forms.Label lblIdDispositivo;
        private System.Windows.Forms.TextBox txtServidorRemoting;
        private System.Windows.Forms.Label lblServidorRemoting;
    }
}
