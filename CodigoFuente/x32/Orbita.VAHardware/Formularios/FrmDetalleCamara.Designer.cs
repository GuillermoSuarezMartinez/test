namespace Orbita.VAHardware
{
    partial class FrmDetalleCamara
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDetalleCamara));
            this.pnlPadre = new Orbita.Controles.OrbitaPanel();
            this.lblFirmware = new System.Windows.Forms.Label();
            this.lblIP = new System.Windows.Forms.Label();
            this.lblCodigoModelo = new System.Windows.Forms.Label();
            this.lblModelo = new System.Windows.Forms.Label();
            this.lblFabricante = new System.Windows.Forms.Label();
            this.lblColor = new System.Windows.Forms.Label();
            this.lblResolucion = new System.Windows.Forms.Label();
            this.pbCamara = new System.Windows.Forms.PictureBox();
            this.lblSerial = new System.Windows.Forms.Label();
            this.pnlPadre.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCamara)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlPadre
            // 
            this.pnlPadre.Controls.Add(this.lblSerial);
            this.pnlPadre.Controls.Add(this.lblFirmware);
            this.pnlPadre.Controls.Add(this.lblIP);
            this.pnlPadre.Controls.Add(this.lblCodigoModelo);
            this.pnlPadre.Controls.Add(this.lblModelo);
            this.pnlPadre.Controls.Add(this.lblFabricante);
            this.pnlPadre.Controls.Add(this.lblColor);
            this.pnlPadre.Controls.Add(this.lblResolucion);
            this.pnlPadre.Controls.Add(this.pbCamara);
            this.pnlPadre.Location = new System.Drawing.Point(9, 12);
            this.pnlPadre.Name = "pnlPadre";
            this.pnlPadre.Size = new System.Drawing.Size(516, 207);
            this.pnlPadre.TabIndex = 9;
            this.pnlPadre.Click += this.CerrarVentana;
            // 
            // lblFirmware
            // 
            this.lblFirmware.AutoSize = true;
            this.lblFirmware.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirmware.Location = new System.Drawing.Point(316, 155);
            this.lblFirmware.Name = "lblFirmware";
            this.lblFirmware.Size = new System.Drawing.Size(49, 13);
            this.lblFirmware.TabIndex = 15;
            this.lblFirmware.Text = "Firmware";
            this.lblFirmware.Click += this.CerrarVentana;
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(316, 132);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(17, 13);
            this.lblIP.TabIndex = 14;
            this.lblIP.Text = "IP";
            this.lblIP.Click += this.CerrarVentana;
            // 
            // lblCodigoModelo
            // 
            this.lblCodigoModelo.AutoSize = true;
            this.lblCodigoModelo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodigoModelo.Location = new System.Drawing.Point(123, 5);
            this.lblCodigoModelo.Name = "lblCodigoModelo";
            this.lblCodigoModelo.Size = new System.Drawing.Size(59, 20);
            this.lblCodigoModelo.TabIndex = 13;
            this.lblCodigoModelo.Text = "Codigo";
            this.lblCodigoModelo.Click += this.CerrarVentana;
            // 
            // lblModelo
            // 
            this.lblModelo.AutoSize = true;
            this.lblModelo.Location = new System.Drawing.Point(316, 61);
            this.lblModelo.Name = "lblModelo";
            this.lblModelo.Size = new System.Drawing.Size(45, 13);
            this.lblModelo.TabIndex = 12;
            this.lblModelo.Text = "Modelo:";
            this.lblModelo.Click += this.CerrarVentana;
            // 
            // lblFabricante
            // 
            this.lblFabricante.AutoSize = true;
            this.lblFabricante.Location = new System.Drawing.Point(316, 36);
            this.lblFabricante.Name = "lblFabricante";
            this.lblFabricante.Size = new System.Drawing.Size(60, 13);
            this.lblFabricante.TabIndex = 11;
            this.lblFabricante.Text = "Fabricante:";
            this.lblFabricante.Click += this.CerrarVentana;
            // 
            // lblColor
            // 
            this.lblColor.AutoSize = true;
            this.lblColor.Location = new System.Drawing.Point(316, 86);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(31, 13);
            this.lblColor.TabIndex = 10;
            this.lblColor.Text = "Color";
            this.lblColor.Click += this.CerrarVentana;
            // 
            // lblResolucion
            // 
            this.lblResolucion.AutoSize = true;
            this.lblResolucion.Location = new System.Drawing.Point(316, 110);
            this.lblResolucion.Name = "lblResolucion";
            this.lblResolucion.Size = new System.Drawing.Size(60, 13);
            this.lblResolucion.TabIndex = 9;
            this.lblResolucion.Text = "Resolucion";
            this.lblResolucion.Click += this.CerrarVentana;
            // 
            // pbCamara
            // 
            this.pbCamara.Location = new System.Drawing.Point(3, 35);
            this.pbCamara.Name = "pbCamara";
            this.pbCamara.Size = new System.Drawing.Size(295, 158);
            this.pbCamara.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCamara.TabIndex = 8;
            this.pbCamara.TabStop = false;
            this.pbCamara.Click += this.CerrarVentana;
            // 
            // lblSerial
            // 
            this.lblSerial.AutoSize = true;
            this.lblSerial.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSerial.Location = new System.Drawing.Point(316, 180);
            this.lblSerial.Name = "lblSerial";
            this.lblSerial.Size = new System.Drawing.Size(87, 13);
            this.lblSerial.TabIndex = 16;
            this.lblSerial.Text = "Número de serie:";
            this.lblSerial.Click += this.CerrarVentana;
            // 
            // FrmDetalleCamara
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(535, 231);
            this.Controls.Add(this.pnlPadre);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmDetalleCamara";
            this.pnlPadre.ResumeLayout(false);
            this.pnlPadre.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCamara)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Orbita.Controles.OrbitaPanel pnlPadre;
        private System.Windows.Forms.Label lblFirmware;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label lblCodigoModelo;
        private System.Windows.Forms.Label lblModelo;
        private System.Windows.Forms.Label lblFabricante;
        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.Label lblResolucion;
        private System.Windows.Forms.PictureBox pbCamara;
        private System.Windows.Forms.Label lblSerial;

    }
}