namespace Orbita.Controles.VA
{
    partial class FrmUserPsw
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.GrpFondo = new Orbita.Controles.OrbitaGroupBox(this.components);
            this.orbitaPictureBox1 = new Orbita.Controles.OrbitaPictureBox(this.components);
            this.ComboUsuario = new Orbita.Controles.OrbitaComboPro();
            this.LblContraseña = new Orbita.Controles.OrbitaLabel(this.components);
            this.LblUsuario = new Orbita.Controles.OrbitaLabel(this.components);
            this.TxtContraseña = new Orbita.Controles.OrbitaTextBox(this.components);
            this.LblErrorContraseña = new Orbita.Controles.OrbitaLabel(this.components);
            this.pnlInferiorPadre.SuspendLayout();
            this.pnlPanelPrincipalPadre.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrpFondo)).BeginInit();
            this.GrpFondo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.orbitaPictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtContraseña)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlInferiorPadre
            // 
            this.pnlInferiorPadre.Location = new System.Drawing.Point(10, 135);
            this.pnlInferiorPadre.Size = new System.Drawing.Size(408, 43);
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            // 
            // pnlPanelPrincipalPadre
            // 
            this.pnlPanelPrincipalPadre.Controls.Add(this.GrpFondo);
            this.pnlPanelPrincipalPadre.Size = new System.Drawing.Size(408, 125);
            // 
            // GrpFondo
            // 
            this.GrpFondo.ContentPadding.Bottom = 2;
            this.GrpFondo.ContentPadding.Left = 2;
            this.GrpFondo.ContentPadding.Right = 2;
            this.GrpFondo.ContentPadding.Top = 2;
            this.GrpFondo.Controls.Add(this.LblErrorContraseña);
            this.GrpFondo.Controls.Add(this.orbitaPictureBox1);
            this.GrpFondo.Controls.Add(this.ComboUsuario);
            this.GrpFondo.Controls.Add(this.LblContraseña);
            this.GrpFondo.Controls.Add(this.LblUsuario);
            this.GrpFondo.Controls.Add(this.TxtContraseña);
            this.GrpFondo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GrpFondo.Location = new System.Drawing.Point(0, 0);
            this.GrpFondo.Name = "GrpFondo";
            this.GrpFondo.Size = new System.Drawing.Size(408, 125);
            this.GrpFondo.TabIndex = 3;
            // 
            // orbitaPictureBox1
            // 
            this.orbitaPictureBox1.Image = global::Orbita.Controles.VA.Properties.Resources.ImgUsuarios48;
            this.orbitaPictureBox1.Location = new System.Drawing.Point(20, 25);
            this.orbitaPictureBox1.Name = "orbitaPictureBox1";
            this.orbitaPictureBox1.Size = new System.Drawing.Size(48, 48);
            this.orbitaPictureBox1.TabIndex = 3;
            this.orbitaPictureBox1.TabStop = false;
            // 
            // ComboUsuario
            // 
            this.ComboUsuario.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboUsuario.Location = new System.Drawing.Point(159, 25);
            this.ComboUsuario.Margin = new System.Windows.Forms.Padding(0);
            this.ComboUsuario.Name = "ComboUsuario";
            this.ComboUsuario.OrbTexto = "";
            this.ComboUsuario.OrbValor = null;
            this.ComboUsuario.Size = new System.Drawing.Size(224, 21);
            this.ComboUsuario.TabIndex = 2;
            // 
            // LblContraseña
            // 
            this.LblContraseña.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.ForeColor = System.Drawing.Color.Black;
            appearance2.TextHAlignAsString = "Right";
            this.LblContraseña.Appearance = appearance2;
            this.LblContraseña.Location = new System.Drawing.Point(78, 64);
            this.LblContraseña.Name = "LblContraseña";
            this.LblContraseña.Size = new System.Drawing.Size(75, 18);
            this.LblContraseña.TabIndex = 1;
            this.LblContraseña.Text = "Contraseña";
            this.LblContraseña.UseMnemonic = false;
            // 
            // LblUsuario
            // 
            this.LblUsuario.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Right";
            this.LblUsuario.Appearance = appearance3;
            this.LblUsuario.Location = new System.Drawing.Point(78, 28);
            this.LblUsuario.Name = "LblUsuario";
            this.LblUsuario.Size = new System.Drawing.Size(75, 18);
            this.LblUsuario.TabIndex = 1;
            this.LblUsuario.Text = "OUsuario";
            this.LblUsuario.UseMnemonic = false;
            // 
            // TxtContraseña
            // 
            this.TxtContraseña.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtContraseña.Appearance = null;
            this.TxtContraseña.Font = new System.Drawing.Font("Franklin Gothic Book", 9F);
            this.TxtContraseña.Location = new System.Drawing.Point(159, 60);
            this.TxtContraseña.Multiline = false;
            this.TxtContraseña.Name = "TxtContraseña";
            this.TxtContraseña.PasswordChar = '*';
            this.TxtContraseña.Size = new System.Drawing.Size(224, 21);
            this.TxtContraseña.TabIndex = 0;
            // 
            // LblErrorContraseña
            // 
            this.LblErrorContraseña.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.ForeColor = System.Drawing.Color.Red;
            appearance1.TextHAlignAsString = "Center";
            this.LblErrorContraseña.Appearance = appearance1;
            this.LblErrorContraseña.Location = new System.Drawing.Point(20, 93);
            this.LblErrorContraseña.Name = "LblErrorContraseña";
            this.LblErrorContraseña.OrbColorFuente = System.Drawing.Color.Red;
            this.LblErrorContraseña.Size = new System.Drawing.Size(363, 18);
            this.LblErrorContraseña.TabIndex = 4;
            this.LblErrorContraseña.Text = "Contraseña incorrecta";
            this.LblErrorContraseña.UseMnemonic = false;
            this.LblErrorContraseña.Visible = false;
            // 
            // FrmUserPsw
            // 
            this.AcceptButton = this.btnGuardar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(428, 188);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.ModoAperturaFormulario = Orbita.Controles.VA.ModoAperturaFormulario.Modificacion;
            this.Name = "FrmUserPsw";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cambio de usuario";
            this.pnlInferiorPadre.ResumeLayout(false);
            this.pnlPanelPrincipalPadre.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GrpFondo)).EndInit();
            this.GrpFondo.ResumeLayout(false);
            this.GrpFondo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.orbitaPictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtContraseña)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Orbita.Controles.OrbitaGroupBox GrpFondo;
        private Orbita.Controles.OrbitaLabel LblContraseña;
        private Orbita.Controles.OrbitaLabel LblUsuario;
        private Orbita.Controles.OrbitaTextBox TxtContraseña;
        private Orbita.Controles.OrbitaComboPro ComboUsuario;
        private Orbita.Controles.OrbitaPictureBox orbitaPictureBox1;
        private Controles.OrbitaLabel LblErrorContraseña;
    }
}