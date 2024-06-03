namespace Orbita.Controles.Comunicaciones
{
    partial class OClienteMCUSB
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
            this.gbConfig.SuspendLayout();
            this.SuspendLayout();            
            // 
            // gbOPC
            // 
            this.gbDispositivo.Location = new System.Drawing.Point(0, 60);
            this.gbDispositivo.Size = new System.Drawing.Size(918, 535);
            this.gbDispositivo.Text = "Dispositivo MCC USB";
            // 
            // lblOPC
            // 
            this.lblCom.Size = new System.Drawing.Size(30, 13);
            this.lblCom.Text = "MCC";
            // 
            // gbConfig
            // 
            this.gbConfig.Size = new System.Drawing.Size(918, 60);
            // 
            // OClienteMCUSB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "OClienteMCUSB";
            this.Size = new System.Drawing.Size(918, 595);
            this.gbConfig.ResumeLayout(false);
            this.gbConfig.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
    }
}
