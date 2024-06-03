namespace Orbita.VAComun
{
    partial class CtrlDisplay
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
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.TimerRetrasoVisualizacionUltimaImagen = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Bitmap images (*.BMP)|*.BMP|Jpeg images(*.JPG)|*.JPG|Todos los archivos(*.*)|*.*";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Bitmap images (*.BMP)|*.BMP|Jpeg images(*.JPG)|*.JPG|Todos los archivos(*.*)|*.*";
            // 
            // TimerRetrasoVisualizacionUltimaImagen
            // 
            this.TimerRetrasoVisualizacionUltimaImagen.Interval = 1;
            this.TimerRetrasoVisualizacionUltimaImagen.Tick += new System.EventHandler(this.OnTimerVisualizacionUltimaImagen);
            // 
            // CtrlDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Name = "CtrlDisplay";
            this.Size = new System.Drawing.Size(698, 453);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Timer TimerRetrasoVisualizacionUltimaImagen;

    }
}
