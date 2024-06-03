namespace Orbita.Controles.VA
{
    partial class FrmDetalleVisor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDetalleVisor));
            this.timerCierre = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timerCierre
            // 
            this.timerCierre.Interval = 10000;
            this.timerCierre.Tick += new System.EventHandler(this.timerCierre_Tick);
            // 
            // FrmDetalleVisor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(535, 231);
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDetalleVisor";
            this.Opacity = 0.8D;
            this.Text = "";
            this.Load += new System.EventHandler(this.FrmDetalleVisor_Load);
            this.Click += new System.EventHandler(this.CerrarVentana);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerCierre;

    }
}