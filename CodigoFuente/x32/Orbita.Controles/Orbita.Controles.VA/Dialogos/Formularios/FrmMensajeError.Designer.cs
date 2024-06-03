namespace Orbita.Controles.VA
{
    partial class MensajeError
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
            this.btnAceptar = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.btnMasInfo = new System.Windows.Forms.Button();
            this.txtMensaje = new System.Windows.Forms.TextBox();
            this.txtExcepcion = new System.Windows.Forms.TextBox();
            this.txtFichero = new System.Windows.Forms.TextBox();
            this.txtClase = new System.Windows.Forms.TextBox();
            this.txtMetodo = new System.Windows.Forms.TextBox();
            this.txtLinea = new System.Windows.Forms.TextBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtEnsamblado = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAceptar
            // 
            this.btnAceptar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAceptar.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAceptar.Location = new System.Drawing.Point(451, 354);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 0;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(12, 104);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(50, 13);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "Mensaje:";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(12, 180);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(60, 13);
            this.Label2.TabIndex = 1;
            this.Label2.Text = "Excepcion:";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(12, 231);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(45, 13);
            this.Label3.TabIndex = 1;
            this.Label3.Text = "Fichero:";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(12, 269);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(36, 13);
            this.Label4.TabIndex = 1;
            this.Label4.Text = "Clase:";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(12, 293);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(46, 13);
            this.Label5.TabIndex = 1;
            this.Label5.Text = "Método:";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(12, 319);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(38, 13);
            this.Label6.TabIndex = 1;
            this.Label6.Text = "Línea:";
            // 
            // btnMasInfo
            // 
            this.btnMasInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMasInfo.Location = new System.Drawing.Point(13, 354);
            this.btnMasInfo.Name = "btnMasInfo";
            this.btnMasInfo.Size = new System.Drawing.Size(75, 23);
            this.btnMasInfo.TabIndex = 2;
            this.btnMasInfo.Text = "Más Info";
            this.btnMasInfo.UseVisualStyleBackColor = true;
            this.btnMasInfo.Click += new System.EventHandler(this.btnMasInfo_Click);
            // 
            // txtMensaje
            // 
            this.txtMensaje.Enabled = false;
            this.txtMensaje.Location = new System.Drawing.Point(81, 101);
            this.txtMensaje.Multiline = true;
            this.txtMensaje.Name = "txtMensaje";
            this.txtMensaje.Size = new System.Drawing.Size(445, 70);
            this.txtMensaje.TabIndex = 3;
            // 
            // txtExcepcion
            // 
            this.txtExcepcion.Enabled = false;
            this.txtExcepcion.Location = new System.Drawing.Point(81, 177);
            this.txtExcepcion.Name = "txtExcepcion";
            this.txtExcepcion.Size = new System.Drawing.Size(445, 20);
            this.txtExcepcion.TabIndex = 4;
            // 
            // txtFichero
            // 
            this.txtFichero.Enabled = false;
            this.txtFichero.Location = new System.Drawing.Point(81, 228);
            this.txtFichero.Multiline = true;
            this.txtFichero.Name = "txtFichero";
            this.txtFichero.Size = new System.Drawing.Size(445, 32);
            this.txtFichero.TabIndex = 5;
            // 
            // txtClase
            // 
            this.txtClase.Enabled = false;
            this.txtClase.Location = new System.Drawing.Point(81, 266);
            this.txtClase.Name = "txtClase";
            this.txtClase.Size = new System.Drawing.Size(445, 20);
            this.txtClase.TabIndex = 6;
            // 
            // txtMetodo
            // 
            this.txtMetodo.Enabled = false;
            this.txtMetodo.Location = new System.Drawing.Point(81, 290);
            this.txtMetodo.Name = "txtMetodo";
            this.txtMetodo.Size = new System.Drawing.Size(445, 20);
            this.txtMetodo.TabIndex = 7;
            // 
            // txtLinea
            // 
            this.txtLinea.Enabled = false;
            this.txtLinea.Location = new System.Drawing.Point(81, 316);
            this.txtLinea.Name = "txtLinea";
            this.txtLinea.Size = new System.Drawing.Size(445, 20);
            this.txtLinea.TabIndex = 8;
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label7.Location = new System.Drawing.Point(146, 29);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(335, 31);
            this.Label7.TabIndex = 9;
            this.Label7.Text = "Se ha producido un error";
            // 
            // PictureBox1
            // 
            this.PictureBox1.Image = global::Orbita.Controles.VA.Properties.Resources.imgError64;
            this.PictureBox1.Location = new System.Drawing.Point(66, 12);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(64, 64);
            this.PictureBox1.TabIndex = 10;
            this.PictureBox1.TabStop = false;
            // 
            // txtEnsamblado
            // 
            this.txtEnsamblado.Enabled = false;
            this.txtEnsamblado.Location = new System.Drawing.Point(81, 202);
            this.txtEnsamblado.Name = "txtEnsamblado";
            this.txtEnsamblado.Size = new System.Drawing.Size(445, 20);
            this.txtEnsamblado.TabIndex = 12;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 205);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Excepcion:";
            // 
            // MensajeError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 389);
            this.Controls.Add(this.txtEnsamblado);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.PictureBox1);
            this.Controls.Add(this.Label7);
            this.Controls.Add(this.txtLinea);
            this.Controls.Add(this.txtMetodo);
            this.Controls.Add(this.txtClase);
            this.Controls.Add(this.txtFichero);
            this.Controls.Add(this.txtExcepcion);
            this.Controls.Add(this.txtMensaje);
            this.Controls.Add(this.btnMasInfo);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.btnAceptar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MensajeError";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MensajeError_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        internal System.Windows.Forms.Button btnAceptar;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Button btnMasInfo;
        internal System.Windows.Forms.TextBox txtMensaje;
        internal System.Windows.Forms.TextBox txtExcepcion;
        internal System.Windows.Forms.TextBox txtFichero;
        internal System.Windows.Forms.TextBox txtClase;
        internal System.Windows.Forms.TextBox txtMetodo;
        internal System.Windows.Forms.TextBox txtLinea;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.PictureBox PictureBox1;

        #endregion
        internal System.Windows.Forms.TextBox txtEnsamblado;
        internal System.Windows.Forms.Label label8;
    }
}