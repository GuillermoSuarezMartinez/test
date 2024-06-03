namespace Orbita.Controles.VA
{
    partial class FrmSplash
    {
        #region Windows Form Designer generated code

        private System.ComponentModel.IContainer components;
        
        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// Método necesario para admitir el Diseñador, no se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSplash));
            this.TimerRefresco = new System.Windows.Forms.Timer(this.components);
            this.lblUsuario = new System.Windows.Forms.Label();
            this.lblMaquina = new System.Windows.Forms.Label();
            this.lblIdioma = new System.Windows.Forms.Label();
            this.lblMensaje = new Orbita.Controles.OrbitaLabel(this.components);
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblCompañia = new System.Windows.Forms.Label();
            this.lblProducto = new System.Windows.Forms.Label();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // TimerRefresco
            // 
            this.TimerRefresco.Tick += new System.EventHandler(this.TimerRefresco_Tick);
            // 
            // lblUsuario
            // 
            this.lblUsuario.BackColor = System.Drawing.Color.Transparent;
            this.lblUsuario.Font = new System.Drawing.Font("Franklin Gothic Book", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuario.ForeColor = System.Drawing.Color.Black;
            this.lblUsuario.Location = new System.Drawing.Point(22, 170);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(204, 16);
            this.lblUsuario.TabIndex = 2;
            this.lblUsuario.Text = "Usuario";
            // 
            // lblMaquina
            // 
            this.lblMaquina.BackColor = System.Drawing.Color.Transparent;
            this.lblMaquina.Font = new System.Drawing.Font("Franklin Gothic Book", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaquina.ForeColor = System.Drawing.Color.Black;
            this.lblMaquina.Location = new System.Drawing.Point(22, 153);
            this.lblMaquina.Name = "lblMaquina";
            this.lblMaquina.Size = new System.Drawing.Size(204, 16);
            this.lblMaquina.TabIndex = 3;
            this.lblMaquina.Text = "Equipo";
            // 
            // lblIdioma
            // 
            this.lblIdioma.BackColor = System.Drawing.Color.Transparent;
            this.lblIdioma.Font = new System.Drawing.Font("Franklin Gothic Book", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIdioma.ForeColor = System.Drawing.Color.Black;
            this.lblIdioma.Location = new System.Drawing.Point(22, 187);
            this.lblIdioma.Name = "lblIdioma";
            this.lblIdioma.Size = new System.Drawing.Size(204, 16);
            this.lblIdioma.TabIndex = 4;
            this.lblIdioma.Text = "Idioma";
            // 
            // lblMensaje
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.ForeColor = System.Drawing.Color.Black;
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            this.lblMensaje.Appearance = appearance1;
            this.lblMensaje.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMensaje.Location = new System.Drawing.Point(0, 235);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(244, 23);
            this.lblMensaje.TabIndex = 5;
            this.lblMensaje.UseMnemonic = false;
            // 
            // lblVersion
            // 
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.Font = new System.Drawing.Font("Franklin Gothic Book", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.Color.Black;
            this.lblVersion.Location = new System.Drawing.Point(22, 204);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(204, 16);
            this.lblVersion.TabIndex = 6;
            this.lblVersion.Text = "Versión";
            // 
            // lblCompañia
            // 
            this.lblCompañia.BackColor = System.Drawing.Color.Transparent;
            this.lblCompañia.Font = new System.Drawing.Font("Franklin Gothic Book", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompañia.ForeColor = System.Drawing.Color.Black;
            this.lblCompañia.Location = new System.Drawing.Point(22, 136);
            this.lblCompañia.Name = "lblCompañia";
            this.lblCompañia.Size = new System.Drawing.Size(204, 16);
            this.lblCompañia.TabIndex = 7;
            this.lblCompañia.Text = "Comañía";
            // 
            // lblProducto
            // 
            this.lblProducto.BackColor = System.Drawing.Color.Transparent;
            this.lblProducto.Font = new System.Drawing.Font("Franklin Gothic Book", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProducto.ForeColor = System.Drawing.Color.Black;
            this.lblProducto.Location = new System.Drawing.Point(22, 119);
            this.lblProducto.Name = "lblProducto";
            this.lblProducto.Size = new System.Drawing.Size(204, 16);
            this.lblProducto.TabIndex = 8;
            this.lblProducto.Text = "Producto";
            // 
            // pbLogo
            // 
            this.pbLogo.Image = ((System.Drawing.Image)(resources.GetObject("pbLogo.Image")));
            this.pbLogo.Location = new System.Drawing.Point(12, 12);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(220, 93);
            this.pbLogo.TabIndex = 1;
            this.pbLogo.TabStop = false;
            // 
            // FrmSplash
            // 
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(244, 258);
            this.ControlBox = false;
            this.Controls.Add(this.lblProducto);
            this.Controls.Add(this.lblCompañia);
            this.Controls.Add(this.lblMensaje);
            this.Controls.Add(this.lblIdioma);
            this.Controls.Add(this.lblMaquina);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.pbLogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSplash";
            this.Text = "";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSplash_FormClosing);
            this.Load += new System.EventHandler(this.FrmSplash_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Timer TimerRefresco;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label lblMaquina;
        private Orbita.Controles.OrbitaLabel lblMensaje;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblIdioma;

        #endregion
        private System.Windows.Forms.Label lblCompañia;
        private System.Windows.Forms.Label lblProducto;
    }
}
