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
            this.gbOPC = new System.Windows.Forms.GroupBox();
            this.gbOPCVariables = new System.Windows.Forms.GroupBox();
            this.dataGridViewOPC = new System.Windows.Forms.DataGridView();
            this.pnlOPCLecturas = new System.Windows.Forms.Panel();
            this.btnLeerVariablesOPC = new System.Windows.Forms.Button();
            this.btnLeerAlarmasOPC = new System.Windows.Forms.Button();
            this.gbOPCCDato = new System.Windows.Forms.GroupBox();
            this.listViewOPC = new System.Windows.Forms.ListView();
            this.pnlOPCSup = new System.Windows.Forms.Panel();
            this.lblPLC = new System.Windows.Forms.Label();
            this.txtPLC = new System.Windows.Forms.TextBox();
            this.lblOPC = new System.Windows.Forms.Label();
            this.lblEscrituraOPC = new System.Windows.Forms.Label();
            this.txtValEscribirOPC = new System.Windows.Forms.TextBox();
            this.txtOPC = new System.Windows.Forms.TextBox();
            this.txtVarEscribirOPC = new System.Windows.Forms.TextBox();
            this.lblLecturaOPC = new System.Windows.Forms.Label();
            this.btnEscrituraOPC = new System.Windows.Forms.Button();
            this.btnLecturaOPC = new System.Windows.Forms.Button();
            this.txtVaLeerOPC = new System.Windows.Forms.TextBox();
            this.txtVarLeerOPC = new System.Windows.Forms.TextBox();
            this.gbConfig = new System.Windows.Forms.GroupBox();
            this.txtServidorRemoting = new System.Windows.Forms.TextBox();
            this.lblServidorRemoting = new System.Windows.Forms.Label();
            this.txtCadenaPLC = new System.Windows.Forms.TextBox();
            this.lblCadenaPLC = new System.Windows.Forms.Label();
            this.txtCadenaOPC = new System.Windows.Forms.TextBox();
            this.lblCadenaOPC = new System.Windows.Forms.Label();
            this.txtIdDispositivo = new System.Windows.Forms.TextBox();
            this.lblIdDispositivo = new System.Windows.Forms.Label();
            this.btnConectar = new System.Windows.Forms.Button();
            this.txtPuertoRemoting = new System.Windows.Forms.TextBox();
            this.lblPuertoRemoting = new System.Windows.Forms.Label();
            this.gbOPC.SuspendLayout();
            this.gbOPCVariables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOPC)).BeginInit();
            this.pnlOPCLecturas.SuspendLayout();
            this.gbOPCCDato.SuspendLayout();
            this.pnlOPCSup.SuspendLayout();
            this.gbConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbOPC
            // 
            this.gbOPC.Controls.Add(this.gbOPCVariables);
            this.gbOPC.Controls.Add(this.gbOPCCDato);
            this.gbOPC.Controls.Add(this.pnlOPCSup);
            this.gbOPC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbOPC.Location = new System.Drawing.Point(0, 100);
            this.gbOPC.Name = "gbOPC";
            this.gbOPC.Padding = new System.Windows.Forms.Padding(10);
            this.gbOPC.Size = new System.Drawing.Size(807, 443);
            this.gbOPC.TabIndex = 32;
            this.gbOPC.TabStop = false;
            this.gbOPC.Text = "Dispositivo OPC";
            // 
            // gbOPCVariables
            // 
            this.gbOPCVariables.Controls.Add(this.dataGridViewOPC);
            this.gbOPCVariables.Controls.Add(this.pnlOPCLecturas);
            this.gbOPCVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbOPCVariables.Location = new System.Drawing.Point(10, 100);
            this.gbOPCVariables.Name = "gbOPCVariables";
            this.gbOPCVariables.Padding = new System.Windows.Forms.Padding(10);
            this.gbOPCVariables.Size = new System.Drawing.Size(787, 187);
            this.gbOPCVariables.TabIndex = 27;
            this.gbOPCVariables.TabStop = false;
            this.gbOPCVariables.Text = "Lecturas";
            // 
            // dataGridViewOPC
            // 
            this.dataGridViewOPC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOPC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewOPC.Location = new System.Drawing.Point(10, 68);
            this.dataGridViewOPC.Name = "dataGridViewOPC";
            this.dataGridViewOPC.Size = new System.Drawing.Size(767, 109);
            this.dataGridViewOPC.TabIndex = 12;
            // 
            // pnlOPCLecturas
            // 
            this.pnlOPCLecturas.Controls.Add(this.btnLeerVariablesOPC);
            this.pnlOPCLecturas.Controls.Add(this.btnLeerAlarmasOPC);
            this.pnlOPCLecturas.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOPCLecturas.Location = new System.Drawing.Point(10, 23);
            this.pnlOPCLecturas.Name = "pnlOPCLecturas";
            this.pnlOPCLecturas.Size = new System.Drawing.Size(767, 45);
            this.pnlOPCLecturas.TabIndex = 28;
            // 
            // btnLeerVariablesOPC
            // 
            this.btnLeerVariablesOPC.Location = new System.Drawing.Point(15, 14);
            this.btnLeerVariablesOPC.Name = "btnLeerVariablesOPC";
            this.btnLeerVariablesOPC.Size = new System.Drawing.Size(92, 23);
            this.btnLeerVariablesOPC.TabIndex = 14;
            this.btnLeerVariablesOPC.Text = "Leer Variables";
            this.btnLeerVariablesOPC.UseVisualStyleBackColor = true;
            // 
            // btnLeerAlarmasOPC
            // 
            this.btnLeerAlarmasOPC.Location = new System.Drawing.Point(113, 14);
            this.btnLeerAlarmasOPC.Name = "btnLeerAlarmasOPC";
            this.btnLeerAlarmasOPC.Size = new System.Drawing.Size(92, 23);
            this.btnLeerAlarmasOPC.TabIndex = 25;
            this.btnLeerAlarmasOPC.Text = "Leer Alarmas";
            this.btnLeerAlarmasOPC.UseVisualStyleBackColor = true;
            // 
            // gbOPCCDato
            // 
            this.gbOPCCDato.Controls.Add(this.listViewOPC);
            this.gbOPCCDato.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbOPCCDato.Location = new System.Drawing.Point(10, 287);
            this.gbOPCCDato.Name = "gbOPCCDato";
            this.gbOPCCDato.Padding = new System.Windows.Forms.Padding(10);
            this.gbOPCCDato.Size = new System.Drawing.Size(787, 146);
            this.gbOPCCDato.TabIndex = 28;
            this.gbOPCCDato.TabStop = false;
            this.gbOPCCDato.Text = "Cambio Dato";
            // 
            // listViewOPC
            // 
            this.listViewOPC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewOPC.GridLines = true;
            this.listViewOPC.Location = new System.Drawing.Point(10, 23);
            this.listViewOPC.MultiSelect = false;
            this.listViewOPC.Name = "listViewOPC";
            this.listViewOPC.Size = new System.Drawing.Size(767, 113);
            this.listViewOPC.TabIndex = 13;
            this.listViewOPC.UseCompatibleStateImageBehavior = false;
            this.listViewOPC.View = System.Windows.Forms.View.List;
            // 
            // pnlOPCSup
            // 
            this.pnlOPCSup.Controls.Add(this.lblPLC);
            this.pnlOPCSup.Controls.Add(this.txtPLC);
            this.pnlOPCSup.Controls.Add(this.lblOPC);
            this.pnlOPCSup.Controls.Add(this.lblEscrituraOPC);
            this.pnlOPCSup.Controls.Add(this.txtValEscribirOPC);
            this.pnlOPCSup.Controls.Add(this.txtOPC);
            this.pnlOPCSup.Controls.Add(this.txtVarEscribirOPC);
            this.pnlOPCSup.Controls.Add(this.lblLecturaOPC);
            this.pnlOPCSup.Controls.Add(this.btnEscrituraOPC);
            this.pnlOPCSup.Controls.Add(this.btnLecturaOPC);
            this.pnlOPCSup.Controls.Add(this.txtVaLeerOPC);
            this.pnlOPCSup.Controls.Add(this.txtVarLeerOPC);
            this.pnlOPCSup.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOPCSup.Location = new System.Drawing.Point(10, 23);
            this.pnlOPCSup.Name = "pnlOPCSup";
            this.pnlOPCSup.Size = new System.Drawing.Size(787, 77);
            this.pnlOPCSup.TabIndex = 26;
            // 
            // lblPLC
            // 
            this.lblPLC.AutoSize = true;
            this.lblPLC.Location = new System.Drawing.Point(27, 46);
            this.lblPLC.Name = "lblPLC";
            this.lblPLC.Size = new System.Drawing.Size(27, 13);
            this.lblPLC.TabIndex = 18;
            this.lblPLC.Text = "PLC";
            // 
            // txtPLC
            // 
            this.txtPLC.Location = new System.Drawing.Point(68, 43);
            this.txtPLC.Name = "txtPLC";
            this.txtPLC.Size = new System.Drawing.Size(112, 20);
            this.txtPLC.TabIndex = 17;
            // 
            // lblOPC
            // 
            this.lblOPC.AutoSize = true;
            this.lblOPC.Location = new System.Drawing.Point(27, 20);
            this.lblOPC.Name = "lblOPC";
            this.lblOPC.Size = new System.Drawing.Size(29, 13);
            this.lblOPC.TabIndex = 16;
            this.lblOPC.Text = "OPC";
            // 
            // lblEscrituraOPC
            // 
            this.lblEscrituraOPC.AutoSize = true;
            this.lblEscrituraOPC.Location = new System.Drawing.Point(198, 46);
            this.lblEscrituraOPC.Name = "lblEscrituraOPC";
            this.lblEscrituraOPC.Size = new System.Drawing.Size(85, 13);
            this.lblEscrituraOPC.TabIndex = 11;
            this.lblEscrituraOPC.Text = "Prueba Escritura";
            // 
            // txtValEscribirOPC
            // 
            this.txtValEscribirOPC.Location = new System.Drawing.Point(503, 43);
            this.txtValEscribirOPC.Name = "txtValEscribirOPC";
            this.txtValEscribirOPC.Size = new System.Drawing.Size(112, 20);
            this.txtValEscribirOPC.TabIndex = 5;
            // 
            // txtOPC
            // 
            this.txtOPC.Location = new System.Drawing.Point(68, 17);
            this.txtOPC.Name = "txtOPC";
            this.txtOPC.Size = new System.Drawing.Size(112, 20);
            this.txtOPC.TabIndex = 15;
            // 
            // txtVarEscribirOPC
            // 
            this.txtVarEscribirOPC.Location = new System.Drawing.Point(291, 43);
            this.txtVarEscribirOPC.Name = "txtVarEscribirOPC";
            this.txtVarEscribirOPC.Size = new System.Drawing.Size(112, 20);
            this.txtVarEscribirOPC.TabIndex = 4;
            this.txtVarEscribirOPC.Text = "stringprueba";
            // 
            // lblLecturaOPC
            // 
            this.lblLecturaOPC.AutoSize = true;
            this.lblLecturaOPC.Location = new System.Drawing.Point(198, 20);
            this.lblLecturaOPC.Name = "lblLecturaOPC";
            this.lblLecturaOPC.Size = new System.Drawing.Size(80, 13);
            this.lblLecturaOPC.TabIndex = 10;
            this.lblLecturaOPC.Text = "Prueba Lectura";
            // 
            // btnEscrituraOPC
            // 
            this.btnEscrituraOPC.Location = new System.Drawing.Point(415, 41);
            this.btnEscrituraOPC.Name = "btnEscrituraOPC";
            this.btnEscrituraOPC.Size = new System.Drawing.Size(75, 23);
            this.btnEscrituraOPC.TabIndex = 3;
            this.btnEscrituraOPC.Text = "escribir";
            this.btnEscrituraOPC.UseVisualStyleBackColor = true;
            // 
            // btnLecturaOPC
            // 
            this.btnLecturaOPC.Location = new System.Drawing.Point(415, 15);
            this.btnLecturaOPC.Name = "btnLecturaOPC";
            this.btnLecturaOPC.Size = new System.Drawing.Size(75, 23);
            this.btnLecturaOPC.TabIndex = 0;
            this.btnLecturaOPC.Text = "leer";
            this.btnLecturaOPC.UseVisualStyleBackColor = true;
            // 
            // txtVaLeerOPC
            // 
            this.txtVaLeerOPC.Location = new System.Drawing.Point(503, 17);
            this.txtVaLeerOPC.Name = "txtVaLeerOPC";
            this.txtVaLeerOPC.Size = new System.Drawing.Size(112, 20);
            this.txtVaLeerOPC.TabIndex = 2;
            // 
            // txtVarLeerOPC
            // 
            this.txtVarLeerOPC.Location = new System.Drawing.Point(291, 17);
            this.txtVarLeerOPC.Name = "txtVarLeerOPC";
            this.txtVarLeerOPC.Size = new System.Drawing.Size(112, 20);
            this.txtVarLeerOPC.TabIndex = 1;
            this.txtVarLeerOPC.Text = "stringprueba";
            // 
            // gbConfig
            // 
            this.gbConfig.Controls.Add(this.txtServidorRemoting);
            this.gbConfig.Controls.Add(this.lblServidorRemoting);
            this.gbConfig.Controls.Add(this.txtCadenaPLC);
            this.gbConfig.Controls.Add(this.lblCadenaPLC);
            this.gbConfig.Controls.Add(this.txtCadenaOPC);
            this.gbConfig.Controls.Add(this.lblCadenaOPC);
            this.gbConfig.Controls.Add(this.txtIdDispositivo);
            this.gbConfig.Controls.Add(this.lblIdDispositivo);
            this.gbConfig.Controls.Add(this.btnConectar);
            this.gbConfig.Controls.Add(this.txtPuertoRemoting);
            this.gbConfig.Controls.Add(this.lblPuertoRemoting);
            this.gbConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbConfig.Location = new System.Drawing.Point(0, 0);
            this.gbConfig.Name = "gbConfig";
            this.gbConfig.Size = new System.Drawing.Size(807, 100);
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
            // txtCadenaPLC
            // 
            this.txtCadenaPLC.Location = new System.Drawing.Point(311, 61);
            this.txtCadenaPLC.Name = "txtCadenaPLC";
            this.txtCadenaPLC.Size = new System.Drawing.Size(100, 20);
            this.txtCadenaPLC.TabIndex = 25;
            this.txtCadenaPLC.Text = "S7 connection_1";
            // 
            // lblCadenaPLC
            // 
            this.lblCadenaPLC.AutoSize = true;
            this.lblCadenaPLC.Location = new System.Drawing.Point(234, 65);
            this.lblCadenaPLC.Name = "lblCadenaPLC";
            this.lblCadenaPLC.Size = new System.Drawing.Size(75, 13);
            this.lblCadenaPLC.TabIndex = 24;
            this.lblCadenaPLC.Text = "ID Dispositivo:";
            // 
            // txtCadenaOPC
            // 
            this.txtCadenaOPC.Location = new System.Drawing.Point(117, 61);
            this.txtCadenaOPC.Name = "txtCadenaOPC";
            this.txtCadenaOPC.Size = new System.Drawing.Size(100, 20);
            this.txtCadenaOPC.TabIndex = 23;
            this.txtCadenaOPC.Text = "LOCALSERVER";
            // 
            // lblCadenaOPC
            // 
            this.lblCadenaOPC.AutoSize = true;
            this.lblCadenaOPC.Location = new System.Drawing.Point(21, 65);
            this.lblCadenaOPC.Name = "lblCadenaOPC";
            this.lblCadenaOPC.Size = new System.Drawing.Size(72, 13);
            this.lblCadenaOPC.TabIndex = 22;
            this.lblCadenaOPC.Text = "Cadena OPC:";
            // 
            // txtIdDispositivo
            // 
            this.txtIdDispositivo.Location = new System.Drawing.Point(310, 25);
            this.txtIdDispositivo.Name = "txtIdDispositivo";
            this.txtIdDispositivo.Size = new System.Drawing.Size(100, 20);
            this.txtIdDispositivo.TabIndex = 21;
            this.txtIdDispositivo.Text = "1";
            // 
            // lblIdDispositivo
            // 
            this.lblIdDispositivo.AutoSize = true;
            this.lblIdDispositivo.Location = new System.Drawing.Point(233, 29);
            this.lblIdDispositivo.Name = "lblIdDispositivo";
            this.lblIdDispositivo.Size = new System.Drawing.Size(75, 13);
            this.lblIdDispositivo.TabIndex = 20;
            this.lblIdDispositivo.Text = "ID Dispositivo:";
            // 
            // btnConectar
            // 
            this.btnConectar.Location = new System.Drawing.Point(638, 61);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new System.Drawing.Size(75, 23);
            this.btnConectar.TabIndex = 19;
            this.btnConectar.Text = "Conectar";
            this.btnConectar.UseVisualStyleBackColor = true;
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
            this.lblPuertoRemoting.Location = new System.Drawing.Point(20, 29);
            this.lblPuertoRemoting.Name = "lblPuertoRemoting";
            this.lblPuertoRemoting.Size = new System.Drawing.Size(89, 13);
            this.lblPuertoRemoting.TabIndex = 0;
            this.lblPuertoRemoting.Text = "Puerto Remoting:";
            // 
            // OClienteComs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbOPC);
            this.Controls.Add(this.gbConfig);
            this.Name = "OClienteComs";
            this.Size = new System.Drawing.Size(807, 543);
            this.gbOPC.ResumeLayout(false);
            this.gbOPCVariables.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOPC)).EndInit();
            this.pnlOPCLecturas.ResumeLayout(false);
            this.gbOPCCDato.ResumeLayout(false);
            this.pnlOPCSup.ResumeLayout(false);
            this.pnlOPCSup.PerformLayout();
            this.gbConfig.ResumeLayout(false);
            this.gbConfig.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbOPC;
        private System.Windows.Forms.GroupBox gbOPCVariables;
        private System.Windows.Forms.DataGridView dataGridViewOPC;
        private System.Windows.Forms.Panel pnlOPCLecturas;
        private System.Windows.Forms.Button btnLeerVariablesOPC;
        private System.Windows.Forms.Button btnLeerAlarmasOPC;
        private System.Windows.Forms.GroupBox gbOPCCDato;
        private System.Windows.Forms.ListView listViewOPC;
        private System.Windows.Forms.Panel pnlOPCSup;
        private System.Windows.Forms.Label lblPLC;
        private System.Windows.Forms.TextBox txtPLC;
        private System.Windows.Forms.Label lblOPC;
        private System.Windows.Forms.Label lblEscrituraOPC;
        private System.Windows.Forms.TextBox txtValEscribirOPC;
        private System.Windows.Forms.TextBox txtOPC;
        private System.Windows.Forms.TextBox txtVarEscribirOPC;
        private System.Windows.Forms.Label lblLecturaOPC;
        private System.Windows.Forms.Button btnEscrituraOPC;
        private System.Windows.Forms.Button btnLecturaOPC;
        private System.Windows.Forms.TextBox txtVaLeerOPC;
        private System.Windows.Forms.TextBox txtVarLeerOPC;
        private System.Windows.Forms.GroupBox gbConfig;
        private System.Windows.Forms.TextBox txtServidorRemoting;
        private System.Windows.Forms.Label lblServidorRemoting;
        private System.Windows.Forms.TextBox txtCadenaPLC;
        private System.Windows.Forms.Label lblCadenaPLC;
        private System.Windows.Forms.TextBox txtCadenaOPC;
        private System.Windows.Forms.Label lblCadenaOPC;
        private System.Windows.Forms.TextBox txtIdDispositivo;
        private System.Windows.Forms.Label lblIdDispositivo;
        private System.Windows.Forms.Button btnConectar;
        private System.Windows.Forms.TextBox txtPuertoRemoting;
        private System.Windows.Forms.Label lblPuertoRemoting;
    }
}
