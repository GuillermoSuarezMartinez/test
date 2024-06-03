namespace Orbita.Controles.VA
{
    partial class CtrlStateMachineDisplay
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.picMaquinaEstados = new Orbita.Controles.OrbitaPictureBox(this.components);
            this.timerRefresco = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picMaquinaEstados)).BeginInit();
            this.SuspendLayout();
            // 
            // picMaquinaEstados
            // 
            this.picMaquinaEstados.BackColor = System.Drawing.SystemColors.Window;
            this.picMaquinaEstados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picMaquinaEstados.Location = new System.Drawing.Point(0, 0);
            this.picMaquinaEstados.Name = "picMaquinaEstados";
            this.picMaquinaEstados.Size = new System.Drawing.Size(435, 402);
            this.picMaquinaEstados.TabIndex = 0;
            this.picMaquinaEstados.TabStop = false;
            this.picMaquinaEstados.DoubleClick += new System.EventHandler(this.picMaquinaEstados_DoubleClick);
            this.picMaquinaEstados.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picMaquinaEstados_MouseMove);
            this.picMaquinaEstados.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picMaquinaEstados_MouseDown);
            this.picMaquinaEstados.Paint += new System.Windows.Forms.PaintEventHandler(this.picMaquinaEstados_Paint);
            this.picMaquinaEstados.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picMaquinaEstados_MouseUp);
            // 
            // timerRefresco
            // 
            this.timerRefresco.Interval = 250;
            this.timerRefresco.Tick += new System.EventHandler(this.timerRefresco_Tick);
            // 
            // CtrlStateMachineDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.picMaquinaEstados);
            this.Name = "CtrlStateMachineDisplay";
            this.Size = new System.Drawing.Size(435, 402);
            this.Load += new System.EventHandler(this.CtrlStateMachineDisplay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picMaquinaEstados)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Orbita.Controles.OrbitaPictureBox picMaquinaEstados;
        private System.Windows.Forms.Timer timerRefresco;
    }
}
