namespace Orbita.Controles.VA
{
    partial class FrmDisplays
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
            this.layFondoVisores = new Orbita.Controles.OrbitaTableLayoutPanel();
            this.pnlPanelPrincipalPadre.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlInferiorPadre
            // 
            this.pnlInferiorPadre.Location = new System.Drawing.Point(10, 197);
            this.pnlInferiorPadre.Size = new System.Drawing.Size(514, 43);
            // 
            // pnlPanelPrincipalPadre
            // 
            this.pnlPanelPrincipalPadre.Controls.Add(this.layFondoVisores);
            this.pnlPanelPrincipalPadre.Size = new System.Drawing.Size(514, 187);
            // 
            // layFondoCamaras
            // 
            this.layFondoVisores.ColumnCount = 1;
            this.layFondoVisores.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layFondoVisores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layFondoVisores.Location = new System.Drawing.Point(0, 0);
            this.layFondoVisores.Name = "layFondoCamaras";
            this.layFondoVisores.RowCount = 1;
            this.layFondoVisores.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layFondoVisores.Size = new System.Drawing.Size(514, 187);
            this.layFondoVisores.TabIndex = 0;
            // 
            // FrmMonitorizacionCamaras
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 250);
            this.ModoAperturaFormulario = Orbita.Controles.VA.ModoAperturaFormulario.Sistema;
            this.MostrarBotones = false;
            this.Name = "FrmMonitorizacionCamaras";
            this.RecordarPosicion = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Monitorización de cámaras";
            this.pnlPanelPrincipalPadre.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Orbita.Controles.OrbitaTableLayoutPanel layFondoVisores;
    }
}