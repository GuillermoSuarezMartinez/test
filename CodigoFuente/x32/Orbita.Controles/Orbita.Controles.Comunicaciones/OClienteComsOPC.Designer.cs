namespace Orbita.Controles.Comunicaciones
{
    partial class OClienteComsOPC
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
            this.txtCadenaPLC = new System.Windows.Forms.TextBox();
            this.lblCadenaPLC = new System.Windows.Forms.Label();
            this.txtCadenaOPC = new System.Windows.Forms.TextBox();
            this.lblCadenaOPC = new System.Windows.Forms.Label();
            this.lblPLC = new System.Windows.Forms.Label();
            this.txtPLC = new System.Windows.Forms.TextBox();
            this.gbDispositivo.SuspendLayout();
            this.gbConfig.SuspendLayout();
            this.pnlDispSup.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDispositivo
            // 
            this.gbDispositivo.Location = new System.Drawing.Point(0, 91);
            this.gbDispositivo.Size = new System.Drawing.Size(807, 452);
            this.gbDispositivo.Text = "Dispositivo OPC";
            // 
            // lblCom
            // 
            this.lblCom.Location = new System.Drawing.Point(64, 20);
            this.lblCom.Size = new System.Drawing.Size(32, 13);
            this.lblCom.Text = "OPC:";
            // 
            // gbConfig
            // 
            this.gbConfig.Controls.Add(this.txtCadenaPLC);
            this.gbConfig.Controls.Add(this.lblCadenaPLC);
            this.gbConfig.Controls.Add(this.txtCadenaOPC);
            this.gbConfig.Controls.Add(this.lblCadenaOPC);
            this.gbConfig.Size = new System.Drawing.Size(807, 91);
            this.gbConfig.Controls.SetChildIndex(this.lblCadenaOPC, 0);
            this.gbConfig.Controls.SetChildIndex(this.txtCadenaOPC, 0);
            this.gbConfig.Controls.SetChildIndex(this.lblCadenaPLC, 0);
            this.gbConfig.Controls.SetChildIndex(this.txtCadenaPLC, 0);
            // 
            // pnlDispSup
            // 
            this.pnlDispSup.Controls.Add(this.lblPLC);
            this.pnlDispSup.Controls.Add(this.txtPLC);
            this.pnlDispSup.Controls.SetChildIndex(this.txtVarLeer, 0);
            this.pnlDispSup.Controls.SetChildIndex(this.txtVarEscribir, 0);
            this.pnlDispSup.Controls.SetChildIndex(this.txtCom, 0);
            this.pnlDispSup.Controls.SetChildIndex(this.lblCom, 0);
            this.pnlDispSup.Controls.SetChildIndex(this.txtPLC, 0);
            this.pnlDispSup.Controls.SetChildIndex(this.lblPLC, 0);
            // 
            // txtCadenaPLC
            // 
            this.txtCadenaPLC.Location = new System.Drawing.Point(319, 55);
            this.txtCadenaPLC.Name = "txtCadenaPLC";
            this.txtCadenaPLC.Size = new System.Drawing.Size(100, 20);
            this.txtCadenaPLC.TabIndex = 31;
            this.txtCadenaPLC.Text = "S7 connection_1";
            // 
            // lblCadenaPLC
            // 
            this.lblCadenaPLC.AutoSize = true;
            this.lblCadenaPLC.Location = new System.Drawing.Point(238, 59);
            this.lblCadenaPLC.Name = "lblCadenaPLC";
            this.lblCadenaPLC.Size = new System.Drawing.Size(75, 13);
            this.lblCadenaPLC.TabIndex = 30;
            this.lblCadenaPLC.Text = "ID Dispositivo:";
            // 
            // txtCadenaOPC
            // 
            this.txtCadenaOPC.Location = new System.Drawing.Point(116, 55);
            this.txtCadenaOPC.Name = "txtCadenaOPC";
            this.txtCadenaOPC.Size = new System.Drawing.Size(100, 20);
            this.txtCadenaOPC.TabIndex = 29;
            this.txtCadenaOPC.Text = "LOCALSERVER";
            // 
            // lblCadenaOPC
            // 
            this.lblCadenaOPC.AutoSize = true;
            this.lblCadenaOPC.Location = new System.Drawing.Point(34, 59);
            this.lblCadenaOPC.Name = "lblCadenaOPC";
            this.lblCadenaOPC.Size = new System.Drawing.Size(72, 13);
            this.lblCadenaOPC.TabIndex = 28;
            this.lblCadenaOPC.Text = "Cadena OPC:";
            // 
            // lblPLC
            // 
            this.lblPLC.AutoSize = true;
            this.lblPLC.Location = new System.Drawing.Point(64, 48);
            this.lblPLC.Name = "lblPLC";
            this.lblPLC.Size = new System.Drawing.Size(30, 13);
            this.lblPLC.TabIndex = 20;
            this.lblPLC.Text = "PLC:";
            // 
            // txtPLC
            // 
            this.txtPLC.Location = new System.Drawing.Point(106, 43);
            this.txtPLC.Name = "txtPLC";
            this.txtPLC.Size = new System.Drawing.Size(99, 20);
            this.txtPLC.TabIndex = 19;
            // 
            // OClienteComsOPC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "OClienteComsOPC";
            this.gbDispositivo.ResumeLayout(false);
            this.gbConfig.ResumeLayout(false);
            this.gbConfig.PerformLayout();
            this.pnlDispSup.ResumeLayout(false);
            this.pnlDispSup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtCadenaPLC;
        private System.Windows.Forms.Label lblCadenaPLC;
        private System.Windows.Forms.TextBox txtCadenaOPC;
        private System.Windows.Forms.Label lblCadenaOPC;
        private System.Windows.Forms.Label lblPLC;
        private System.Windows.Forms.TextBox txtPLC;
    }
}
