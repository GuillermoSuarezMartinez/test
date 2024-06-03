namespace Orbita.Controles.VA
{
    partial class CtrlDisplayVPro
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlDisplayVPro));
            this.Display = new Cognex.VisionPro.Display.CogDisplay();
            this.PnlButtonsTop = new System.Windows.Forms.Panel();
            this.toolStripCenter = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.lblTituloDisplay = new System.Windows.Forms.ToolStripLabel();
            this.DisplayToolbar = new Cognex.VisionPro.CogDisplayToolbarV2();
            this.toolStripRight = new System.Windows.Forms.ToolStrip();
            this.btnMaximize = new System.Windows.Forms.ToolStripButton();
            this.btnNext = new System.Windows.Forms.ToolStripButton();
            this.btnPrev = new System.Windows.Forms.ToolStripButton();
            this.btnInfo = new System.Windows.Forms.ToolStripButton();
            this.toolStripLeft = new System.Windows.Forms.ToolStrip();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.separadorArchivos = new System.Windows.Forms.ToolStripSeparator();
            this.PnlStatusBottom = new System.Windows.Forms.Panel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblMensaje = new System.Windows.Forms.ToolStripStatusLabel();
            this.PnlStatusVPro = new Orbita.Controles.OrbitaPanel();
            this.DisplayStatusBar = new Cognex.VisionPro.CogDisplayStatusBarV2();
            ((System.ComponentModel.ISupportInitialize)(this.Display)).BeginInit();
            this.PnlButtonsTop.SuspendLayout();
            this.toolStripCenter.SuspendLayout();
            this.toolStripRight.SuspendLayout();
            this.toolStripLeft.SuspendLayout();
            this.PnlStatusBottom.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.PnlStatusVPro.SuspendLayout();
            this.SuspendLayout();
            // 
            // Display
            // 
            this.Display.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Display.Location = new System.Drawing.Point(0, 28);
            this.Display.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.Display.MouseWheelSensitivity = 1D;
            this.Display.Name = "Display";
            this.Display.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("Display.OcxState")));
            this.Display.Size = new System.Drawing.Size(926, 477);
            this.Display.TabIndex = 7;
            // 
            // PnlButtonsTop
            // 
            this.PnlButtonsTop.Controls.Add(this.toolStripCenter);
            this.PnlButtonsTop.Controls.Add(this.DisplayToolbar);
            this.PnlButtonsTop.Controls.Add(this.toolStripRight);
            this.PnlButtonsTop.Controls.Add(this.toolStripLeft);
            this.PnlButtonsTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.PnlButtonsTop.Location = new System.Drawing.Point(0, 0);
            this.PnlButtonsTop.Name = "PnlButtonsTop";
            this.PnlButtonsTop.Size = new System.Drawing.Size(926, 28);
            this.PnlButtonsTop.TabIndex = 13;
            // 
            // toolStripCenter
            // 
            this.toolStripCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripCenter.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStripCenter.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripCenter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator2,
            this.lblTituloDisplay});
            this.toolStripCenter.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStripCenter.Location = new System.Drawing.Point(257, 0);
            this.toolStripCenter.Name = "toolStripCenter";
            this.toolStripCenter.Padding = new System.Windows.Forms.Padding(0);
            this.toolStripCenter.Size = new System.Drawing.Size(621, 28);
            this.toolStripCenter.TabIndex = 15;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 28);
            // 
            // lblTituloDisplay
            // 
            this.lblTituloDisplay.Name = "lblTituloDisplay";
            this.lblTituloDisplay.Size = new System.Drawing.Size(33, 25);
            this.lblTituloDisplay.Text = "Visor";
            this.lblTituloDisplay.ToolTipText = "Texto descriptivo de visor";
            // 
            // DisplayToolbar
            // 
            this.DisplayToolbar.Dock = System.Windows.Forms.DockStyle.Left;
            this.DisplayToolbar.Location = new System.Drawing.Point(54, 0);
            this.DisplayToolbar.Name = "DisplayToolbar";
            this.DisplayToolbar.Size = new System.Drawing.Size(203, 28);
            this.DisplayToolbar.TabIndex = 13;
            // 
            // toolStripRight
            // 
            this.toolStripRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStripRight.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStripRight.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripRight.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnMaximize,
            this.btnNext,
            this.btnPrev,
            this.btnInfo});
            this.toolStripRight.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStripRight.Location = new System.Drawing.Point(878, 0);
            this.toolStripRight.Name = "toolStripRight";
            this.toolStripRight.Padding = new System.Windows.Forms.Padding(0);
            this.toolStripRight.Size = new System.Drawing.Size(48, 28);
            this.toolStripRight.TabIndex = 14;
            // 
            // btnMaximize
            // 
            this.btnMaximize.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnMaximize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMaximize.Image = global::Orbita.Controles.VA.Properties.Resources.imgFullScreen16;
            this.btnMaximize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMaximize.Name = "btnMaximize";
            this.btnMaximize.Size = new System.Drawing.Size(23, 25);
            this.btnMaximize.Text = "Maximizar";
            this.btnMaximize.ToolTipText = "Maximizar/Minimizar";
            this.btnMaximize.Click += new System.EventHandler(this.btnMaximize_Click);
            // 
            // btnNext
            // 
            this.btnNext.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNext.Image = global::Orbita.Controles.VA.Properties.Resources.ImgNextDoc16;
            this.btnNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(23, 25);
            this.btnNext.Text = "toolStripButton1";
            this.btnNext.Visible = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPrev.Image = global::Orbita.Controles.VA.Properties.Resources.ImgPrevDoc16;
            this.btnPrev.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(23, 25);
            this.btnPrev.Text = "toolStripButton2";
            this.btnPrev.Visible = false;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnInfo
            // 
            this.btnInfo.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnInfo.Image = global::Orbita.Controles.VA.Properties.Resources.imgInfo16;
            this.btnInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(23, 25);
            this.btnInfo.Text = "Información";
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // toolStripLeft
            // 
            this.toolStripLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStripLeft.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStripLeft.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripLeft.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOpen,
            this.btnSave,
            this.separadorArchivos});
            this.toolStripLeft.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStripLeft.Location = new System.Drawing.Point(0, 0);
            this.toolStripLeft.Name = "toolStripLeft";
            this.toolStripLeft.Padding = new System.Windows.Forms.Padding(0);
            this.toolStripLeft.Size = new System.Drawing.Size(54, 28);
            this.toolStripLeft.TabIndex = 12;
            // 
            // btnOpen
            // 
            this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnOpen.Image")));
            this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(23, 25);
            this.btnOpen.Text = "Abrir";
            this.btnOpen.ToolTipText = "Abrir imagen";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Image = global::Orbita.Controles.VA.Properties.Resources.imgFloppy16;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(23, 25);
            this.btnSave.Text = "Guardar";
            this.btnSave.ToolTipText = "Guardar imagen";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // separadorArchivos
            // 
            this.separadorArchivos.Name = "separadorArchivos";
            this.separadorArchivos.Size = new System.Drawing.Size(6, 28);
            // 
            // PnlStatusBottom
            // 
            this.PnlStatusBottom.Controls.Add(this.statusStrip);
            this.PnlStatusBottom.Controls.Add(this.PnlStatusVPro);
            this.PnlStatusBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PnlStatusBottom.Location = new System.Drawing.Point(0, 505);
            this.PnlStatusBottom.Name = "PnlStatusBottom";
            this.PnlStatusBottom.Size = new System.Drawing.Size(926, 20);
            this.PnlStatusBottom.TabIndex = 14;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblMensaje});
            this.statusStrip.Location = new System.Drawing.Point(0, -2);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.ShowItemToolTips = true;
            this.statusStrip.Size = new System.Drawing.Size(522, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 13;
            // 
            // lblMensaje
            // 
            this.lblMensaje.Image = global::Orbita.Controles.VA.Properties.Resources.ImgChat16;
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(16, 17);
            this.lblMensaje.ToolTipText = "Información sobre la imagen visualizada";
            // 
            // PnlStatusVPro
            // 
            this.PnlStatusVPro.Controls.Add(this.DisplayStatusBar);
            this.PnlStatusVPro.Dock = System.Windows.Forms.DockStyle.Right;
            this.PnlStatusVPro.Location = new System.Drawing.Point(522, 0);
            this.PnlStatusVPro.Name = "PnlStatusVPro";
            this.PnlStatusVPro.Size = new System.Drawing.Size(404, 20);
            this.PnlStatusVPro.TabIndex = 12;
            // 
            // DisplayStatusBar
            // 
            this.DisplayStatusBar.CoordinateSpaceName = "*\\#";
            this.DisplayStatusBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DisplayStatusBar.Location = new System.Drawing.Point(0, 0);
            this.DisplayStatusBar.Name = "DisplayStatusBar";
            this.DisplayStatusBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.DisplayStatusBar.ShowToolTips = true;
            this.DisplayStatusBar.Size = new System.Drawing.Size(404, 20);
            this.DisplayStatusBar.TabIndex = 11;
            // 
            // CtrlDisplayVPro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Display);
            this.Controls.Add(this.PnlStatusBottom);
            this.Controls.Add(this.PnlButtonsTop);
            this.Name = "CtrlDisplayVPro";
            this.Size = new System.Drawing.Size(926, 525);
            ((System.ComponentModel.ISupportInitialize)(this.Display)).EndInit();
            this.PnlButtonsTop.ResumeLayout(false);
            this.PnlButtonsTop.PerformLayout();
            this.toolStripCenter.ResumeLayout(false);
            this.toolStripCenter.PerformLayout();
            this.toolStripRight.ResumeLayout(false);
            this.toolStripRight.PerformLayout();
            this.toolStripLeft.ResumeLayout(false);
            this.toolStripLeft.PerformLayout();
            this.PnlStatusBottom.ResumeLayout(false);
            this.PnlStatusBottom.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.PnlStatusVPro.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Cognex.VisionPro.Display.CogDisplay Display;
        private System.Windows.Forms.Panel PnlButtonsTop;
        public System.Windows.Forms.ToolStrip toolStripLeft;
        private System.Windows.Forms.ToolStripButton btnOpen;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripSeparator separadorArchivos;
        public System.Windows.Forms.ToolStrip toolStripCenter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel lblTituloDisplay;
        private Cognex.VisionPro.CogDisplayToolbarV2 DisplayToolbar;
        public System.Windows.Forms.ToolStrip toolStripRight;
        private System.Windows.Forms.ToolStripButton btnMaximize;
        private System.Windows.Forms.ToolStripButton btnInfo;
        private System.Windows.Forms.Panel PnlStatusBottom;
        private Controles.OrbitaPanel PnlStatusVPro;
        private Cognex.VisionPro.CogDisplayStatusBarV2 DisplayStatusBar;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblMensaje;
        private System.Windows.Forms.ToolStripButton btnNext;
        private System.Windows.Forms.ToolStripButton btnPrev;
    }
}
