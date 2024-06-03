namespace Orbita.VAHardware
{
    partial class CtrlDisplayAxis
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlDisplayAxis));
            this.dispAxis = new AxAXISMEDIACONTROLLib.AxAxisMediaControl();
            this.ToolStripTop = new System.Windows.Forms.ToolStrip();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.separadorArchivos = new System.Windows.Forms.ToolStripSeparator();
            this.lblTituloDisplay = new System.Windows.Forms.ToolStripLabel();
            this.btnMaximize = new System.Windows.Forms.ToolStripButton();
            this.btnInfo = new System.Windows.Forms.ToolStripButton();
            this.StatusStripBottom = new System.Windows.Forms.StatusStrip();
            this.lblMensaje = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnPrev = new System.Windows.Forms.ToolStripButton();
            this.btnNext = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.dispAxis)).BeginInit();
            this.ToolStripTop.SuspendLayout();
            this.StatusStripBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // dispAxis
            // 
            this.dispAxis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dispAxis.Enabled = true;
            this.dispAxis.Location = new System.Drawing.Point(0, 25);
            this.dispAxis.Name = "dispAxis";
            this.dispAxis.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("dispAxis.OcxState")));
            this.dispAxis.Size = new System.Drawing.Size(350, 276);
            this.dispAxis.TabIndex = 3;
            // 
            // toolStrip
            // 
            this.ToolStripTop.GripMargin = new System.Windows.Forms.Padding(0);
            this.ToolStripTop.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOpen,
            this.btnSave,
            this.separadorArchivos,
            this.lblTituloDisplay,
            this.btnMaximize,
            this.btnNext,
            this.btnPrev,
            this.btnInfo});
            this.ToolStripTop.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.ToolStripTop.Location = new System.Drawing.Point(0, 0);
            this.ToolStripTop.Name = "toolStrip";
            this.ToolStripTop.Padding = new System.Windows.Forms.Padding(0);
            this.ToolStripTop.Size = new System.Drawing.Size(350, 25);
            this.ToolStripTop.TabIndex = 12;
            // 
            // btnOpen
            // 
            this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnOpen.Image")));
            this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(23, 22);
            this.btnOpen.Text = "Abrir";
            this.btnOpen.ToolTipText = "Abrir imagen";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Image = global::Orbita.VAHardware.Properties.Resources.imgFloppy16;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(23, 22);
            this.btnSave.Text = "Guardar";
            this.btnSave.ToolTipText = "Guardar imagen";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // separadorArchivos
            // 
            this.separadorArchivos.Name = "separadorArchivos";
            this.separadorArchivos.Size = new System.Drawing.Size(6, 25);
            // 
            // lblTituloDisplay
            // 
            this.lblTituloDisplay.Name = "lblTituloDisplay";
            this.lblTituloDisplay.Size = new System.Drawing.Size(33, 22);
            this.lblTituloDisplay.Text = "Visor";
            this.lblTituloDisplay.ToolTipText = "Texto descriptivo de visor";
            // 
            // btnMaximize
            // 
            this.btnMaximize.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnMaximize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMaximize.Image = global::Orbita.VAHardware.Properties.Resources.imgFullScreen16;
            this.btnMaximize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMaximize.Name = "btnMaximize";
            this.btnMaximize.Size = new System.Drawing.Size(23, 22);
            this.btnMaximize.Text = "Maximizar";
            this.btnMaximize.ToolTipText = "Maximizar/Minimizar";
            this.btnMaximize.Click += new System.EventHandler(this.btnMaximize_Click);
            // 
            // btnInfo
            // 
            this.btnInfo.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnInfo.Image = global::Orbita.VAHardware.Properties.Resources.imgInfo16;
            this.btnInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(23, 22);
            this.btnInfo.Text = "Información";
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // statusStrip
            // 
            this.StatusStripBottom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblMensaje});
            this.StatusStripBottom.Location = new System.Drawing.Point(0, 301);
            this.StatusStripBottom.Name = "statusStrip";
            this.StatusStripBottom.ShowItemToolTips = true;
            this.StatusStripBottom.Size = new System.Drawing.Size(350, 22);
            this.StatusStripBottom.SizingGrip = false;
            this.StatusStripBottom.TabIndex = 13;
            // 
            // lblMensaje
            // 
            this.lblMensaje.Image = global::Orbita.VAHardware.Properties.Resources.ImgChat16;
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(16, 17);
            this.lblMensaje.ToolTipText = "Información sobre la imagen visualizada";
            // 
            // btnPrev
            // 
            this.btnPrev.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPrev.Image = global::Orbita.VAHardware.Properties.Resources.ImgPrevDoc16;
            this.btnPrev.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(23, 22);
            this.btnPrev.Visible = false;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNext.Image = global::Orbita.VAHardware.Properties.Resources.ImgNextDoc16;
            this.btnNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(23, 22);
            this.btnNext.Visible = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // CtrlDisplayAxis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dispAxis);
            this.Controls.Add(this.StatusStripBottom);
            this.Controls.Add(this.ToolStripTop);
            this.Name = "CtrlDisplayAxis";
            this.Size = new System.Drawing.Size(350, 323);
            ((System.ComponentModel.ISupportInitialize)(this.dispAxis)).EndInit();
            this.ToolStripTop.ResumeLayout(false);
            this.ToolStripTop.PerformLayout();
            this.StatusStripBottom.ResumeLayout(false);
            this.StatusStripBottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public AxAXISMEDIACONTROLLib.AxAxisMediaControl dispAxis;
        public System.Windows.Forms.ToolStrip ToolStripTop;
        private System.Windows.Forms.ToolStripButton btnOpen;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripSeparator separadorArchivos;
        private System.Windows.Forms.ToolStripLabel lblTituloDisplay;
        private System.Windows.Forms.ToolStripButton btnMaximize;
        private System.Windows.Forms.ToolStripButton btnInfo;
        private System.Windows.Forms.StatusStrip StatusStripBottom;
        private System.Windows.Forms.ToolStripStatusLabel lblMensaje;
        private System.Windows.Forms.ToolStripButton btnNext;
        private System.Windows.Forms.ToolStripButton btnPrev;
    }
}
