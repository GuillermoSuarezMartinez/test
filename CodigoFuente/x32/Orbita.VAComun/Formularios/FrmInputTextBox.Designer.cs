namespace Orbita.VAComun
{
    partial class FrmInputTextBox
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
            this.TxtInput = new Orbita.Controles.OrbitaTextBox(this.components);
            this.LblExplicacion = new Orbita.Controles.OrbitaLabel(this.components);
            this.GrpFondo = new Orbita.Controles.OrbitaGroupBox(this.components);
            this.pnlInferiorPadre.SuspendLayout();
            this.pnlPanelPrincipalPadre.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrpFondo)).BeginInit();
            this.GrpFondo.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlInferiorPadre
            // 
            this.pnlInferiorPadre.Location = new System.Drawing.Point(10, 108);
            this.pnlInferiorPadre.Size = new System.Drawing.Size(371, 43);
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            // 
            // pnlPanelPrincipalPadre
            // 
            this.pnlPanelPrincipalPadre.Controls.Add(this.GrpFondo);
            this.pnlPanelPrincipalPadre.Size = new System.Drawing.Size(371, 98);
            // 
            // TxtInput
            // 
            this.TxtInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtInput.Appearance = null;
            this.TxtInput.Font = new System.Drawing.Font("Franklin Gothic Book", 9F);
            this.TxtInput.Location = new System.Drawing.Point(25, 46);
            this.TxtInput.Multiline = false;
            this.TxtInput.Name = "TxtInput";
            this.TxtInput.Size = new System.Drawing.Size(316, 21);
            this.TxtInput.TabIndex = 0;
            // 
            // LblExplicacion
            // 
            this.LblExplicacion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.ForeColor = System.Drawing.Color.Black;
            this.LblExplicacion.Appearance = appearance1;
            this.LblExplicacion.Location = new System.Drawing.Point(25, 22);
            this.LblExplicacion.Name = "LblExplicacion";
            this.LblExplicacion.Size = new System.Drawing.Size(316, 18);
            this.LblExplicacion.TabIndex = 1;
            this.LblExplicacion.Text = "LblExplicacion";
            this.LblExplicacion.UseMnemonic = false;
            // 
            // GrpFondo
            // 
            this.GrpFondo.ContentPadding.Bottom = 2;
            this.GrpFondo.ContentPadding.Left = 2;
            this.GrpFondo.ContentPadding.Right = 2;
            this.GrpFondo.ContentPadding.Top = 2;
            this.GrpFondo.Controls.Add(this.LblExplicacion);
            this.GrpFondo.Controls.Add(this.TxtInput);
            this.GrpFondo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GrpFondo.Location = new System.Drawing.Point(0, 0);
            this.GrpFondo.Name = "GrpFondo";
            this.GrpFondo.OrbColorBorde = System.Drawing.Color.Empty;
            this.GrpFondo.OrbColorCabecera = System.Drawing.Color.Empty;
            this.GrpFondo.Size = new System.Drawing.Size(371, 98);
            this.GrpFondo.TabIndex = 2;
            // 
            // FrmInputTextBox
            // 
            this.AcceptButton = this.btnGuardar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(391, 161);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.ModoAperturaFormulario = Orbita.VAComun.ModoAperturaFormulario.Modificacion;
            this.Name = "FrmInputTextBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmInputTextBox";
            this.Activated += new System.EventHandler(this.FrmInputTextBox_Activated);
            this.pnlInferiorPadre.ResumeLayout(false);
            this.pnlPanelPrincipalPadre.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TxtInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrpFondo)).EndInit();
            this.GrpFondo.ResumeLayout(false);
            this.GrpFondo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Orbita.Controles.OrbitaGroupBox GrpFondo;
        private Orbita.Controles.OrbitaLabel LblExplicacion;
        private Orbita.Controles.OrbitaTextBox TxtInput;
    }
}