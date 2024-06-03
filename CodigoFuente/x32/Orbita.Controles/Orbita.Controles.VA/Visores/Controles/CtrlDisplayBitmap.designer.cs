namespace Orbita.Controles.VA
{
    partial class CtrlDisplayBitmap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlDisplayBitmap));
            this.TimerUpdateAutoScrollPosition = new System.Windows.Forms.Timer(this.components);
            this.TimerUpdateCursorPosition = new System.Windows.Forms.Timer(this.components);
            this.TimerUpdateZoomLevel = new System.Windows.Forms.Timer(this.components);
            this.TimerUpdateViewSize = new System.Windows.Forms.Timer(this.components);
            this.TimerUpdateSelecction = new System.Windows.Forms.Timer(this.components);
            this.VisorImagenes = new Orbita.Controles.VA.VisorImagenes();
            this.ToolStripTop = new System.Windows.Forms.ToolStrip();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.separadorArchivos = new System.Windows.Forms.ToolStripSeparator();
            this.btnPointer = new System.Windows.Forms.ToolStripButton();
            this.btnHand = new System.Windows.Forms.ToolStripButton();
            this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
            this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
            this.BtnFit = new System.Windows.Forms.ToolStripButton();
            this.separadorZoom = new System.Windows.Forms.ToolStripSeparator();
            this.lblTituloDisplay = new System.Windows.Forms.ToolStripLabel();
            this.btnMaximize = new System.Windows.Forms.ToolStripButton();
            this.btnNext = new System.Windows.Forms.ToolStripButton();
            this.btnPrev = new System.Windows.Forms.ToolStripButton();
            this.btnInfo = new System.Windows.Forms.ToolStripButton();
            this.StatusStripBottom = new System.Windows.Forms.StatusStrip();
            this.lblMensaje = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblPixelValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblAutoScrollPosition = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblViewArea = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblZoom = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCursor = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSelection = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripTop.SuspendLayout();
            this.StatusStripBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // TimerUpdateAutoScrollPosition
            // 
            this.TimerUpdateAutoScrollPosition.Tick += new System.EventHandler(this.TimerUpdateAutoScrollPosition_Tick);
            // 
            // TimerUpdateCursorPosition
            // 
            this.TimerUpdateCursorPosition.Tick += new System.EventHandler(this.TimerUpdateCursorPosition_Tick);
            // 
            // TimerUpdateZoomLevel
            // 
            this.TimerUpdateZoomLevel.Tick += new System.EventHandler(this.TimerUpdateZoomLevel_Tick);
            // 
            // TimerUpdateViewSize
            // 
            this.TimerUpdateViewSize.Tick += new System.EventHandler(this.TimerUpdateViewSize_Tick);
            // 
            // TimerUpdateSelecction
            // 
            this.TimerUpdateSelecction.Tick += new System.EventHandler(this.TimerUpdateSelecction_Tick);
            // 
            // VisorImagenes
            // 
            this.VisorImagenes.AutoSize = false;
            this.VisorImagenes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VisorImagenes.Location = new System.Drawing.Point(0, 25);
            this.VisorImagenes.Name = "VisorImagenes";
            this.VisorImagenes.Size = new System.Drawing.Size(578, 322);
            this.VisorImagenes.TabIndex = 6;
            this.VisorImagenes.SelectionRegionChanged += new System.EventHandler(this.VisorImagenes_SelectionRegionChanged);
            this.VisorImagenes.ZoomChanged += new System.EventHandler(this.VisorImagenes_ZoomChanged);
            this.VisorImagenes.Scroll += new System.Windows.Forms.ScrollEventHandler(this.VisorImagenes_Scroll);
            this.VisorImagenes.MouseLeave += new System.EventHandler(this.VisorImagenes_MouseLeave);
            this.VisorImagenes.MouseMove += new System.Windows.Forms.MouseEventHandler(this.VisorImagenes_MouseMove);
            this.VisorImagenes.Resize += new System.EventHandler(this.VisorImagenes_Resize);
            // 
            // toolStrip1
            // 
            this.ToolStripTop.GripMargin = new System.Windows.Forms.Padding(0);
            this.ToolStripTop.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOpen,
            this.btnSave,
            this.separadorArchivos,
            this.btnPointer,
            this.btnHand,
            this.btnZoomIn,
            this.btnZoomOut,
            this.BtnFit,
            this.separadorZoom,
            this.lblTituloDisplay,
            this.btnMaximize,
            this.btnNext,
            this.btnPrev,
            this.btnInfo});
            this.ToolStripTop.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.ToolStripTop.Location = new System.Drawing.Point(0, 0);
            this.ToolStripTop.Name = "toolStrip1";
            this.ToolStripTop.Padding = new System.Windows.Forms.Padding(0);
            this.ToolStripTop.Size = new System.Drawing.Size(578, 25);
            this.ToolStripTop.TabIndex = 11;
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
            this.btnSave.Image = global::Orbita.Controles.VA.Properties.Resources.imgFloppy16;
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
            // btnPointer
            // 
            this.btnPointer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPointer.Image = ((System.Drawing.Image)(resources.GetObject("btnPointer.Image")));
            this.btnPointer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPointer.Name = "btnPointer";
            this.btnPointer.Size = new System.Drawing.Size(23, 22);
            this.btnPointer.Text = "Puntero";
            this.btnPointer.ToolTipText = "Cursor en modo puntero";
            // 
            // btnHand
            // 
            this.btnHand.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnHand.Image = global::Orbita.Controles.VA.Properties.Resources.ImgHand16;
            this.btnHand.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnHand.Name = "btnHand";
            this.btnHand.Size = new System.Drawing.Size(23, 22);
            this.btnHand.Text = "Deslizamiento";
            this.btnHand.ToolTipText = "Cursor en modo deslizamiento";
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomIn.Image = global::Orbita.Controles.VA.Properties.Resources.ImgZoomIn16;
            this.btnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(23, 22);
            this.btnZoomIn.Text = "Zoom +";
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomOut.Image = global::Orbita.Controles.VA.Properties.Resources.ImgZoomOut16;
            this.btnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(23, 22);
            this.btnZoomOut.Text = "Zoom -";
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // BtnFit
            // 
            this.BtnFit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnFit.Image = global::Orbita.Controles.VA.Properties.Resources.ImgZoomFit16;
            this.BtnFit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnFit.Name = "BtnFit";
            this.BtnFit.Size = new System.Drawing.Size(23, 22);
            this.BtnFit.Text = "Ajuste";
            this.BtnFit.ToolTipText = "Ajustar imagen";
            this.BtnFit.Click += new System.EventHandler(this.btnZoomFit_Click);
            // 
            // separadorZoom
            // 
            this.separadorZoom.Name = "separadorZoom";
            this.separadorZoom.Size = new System.Drawing.Size(6, 25);
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
            this.btnMaximize.Image = global::Orbita.Controles.VA.Properties.Resources.imgFullScreen16;
            this.btnMaximize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMaximize.Name = "btnMaximize";
            this.btnMaximize.Size = new System.Drawing.Size(23, 22);
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
            this.btnNext.Size = new System.Drawing.Size(23, 22);
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
            this.btnPrev.Size = new System.Drawing.Size(23, 22);
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
            this.btnInfo.Size = new System.Drawing.Size(23, 22);
            this.btnInfo.Text = "Información";
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // statusStrip1
            // 
            this.StatusStripBottom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblMensaje,
            this.toolStripStatusLabel1,
            this.lblPixelValue,
            this.lblAutoScrollPosition,
            this.lblViewArea,
            this.lblZoom,
            this.lblCursor,
            this.lblSelection});
            this.StatusStripBottom.Location = new System.Drawing.Point(0, 347);
            this.StatusStripBottom.Name = "statusStrip1";
            this.StatusStripBottom.ShowItemToolTips = true;
            this.StatusStripBottom.Size = new System.Drawing.Size(578, 22);
            this.StatusStripBottom.SizingGrip = false;
            this.StatusStripBottom.TabIndex = 12;
            // 
            // lblMensaje
            // 
            this.lblMensaje.Image = global::Orbita.Controles.VA.Properties.Resources.ImgChat16;
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(16, 17);
            this.lblMensaje.ToolTipText = "Información sobre la imagen visualizada";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(245, 17);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // lblPixelValue
            // 
            this.lblPixelValue.Image = global::Orbita.Controles.VA.Properties.Resources.ImgTarget16;
            this.lblPixelValue.Name = "lblPixelValue";
            this.lblPixelValue.Size = new System.Drawing.Size(47, 17);
            this.lblPixelValue.Text = "0,0,0";
            // 
            // lblAutoScrollPosition
            // 
            this.lblAutoScrollPosition.Image = global::Orbita.Controles.VA.Properties.Resources.ImgPosicion16;
            this.lblAutoScrollPosition.Name = "lblAutoScrollPosition";
            this.lblAutoScrollPosition.Size = new System.Drawing.Size(61, 17);
            this.lblAutoScrollPosition.Text = "X:0, Y:0";
            this.lblAutoScrollPosition.ToolTipText = "Posición del desplazamiento";
            // 
            // lblViewArea
            // 
            this.lblViewArea.Image = global::Orbita.Controles.VA.Properties.Resources.ImgTamaño16;
            this.lblViewArea.Name = "lblViewArea";
            this.lblViewArea.Size = new System.Drawing.Size(111, 17);
            this.lblViewArea.Text = "X:0, Y:0, W:0, H:0";
            this.lblViewArea.ToolTipText = "Tamaño de la imagen";
            // 
            // lblZoom
            // 
            this.lblZoom.Image = global::Orbita.Controles.VA.Properties.Resources.ImgZoom16;
            this.lblZoom.Name = "lblZoom";
            this.lblZoom.Size = new System.Drawing.Size(51, 17);
            this.lblZoom.Text = "100%";
            this.lblZoom.ToolTipText = "Zoom";
            // 
            // lblCursor
            // 
            this.lblCursor.Image = ((System.Drawing.Image)(resources.GetObject("lblCursor.Image")));
            this.lblCursor.Name = "lblCursor";
            this.lblCursor.Size = new System.Drawing.Size(16, 17);
            this.lblCursor.ToolTipText = "Posición actual del cursor";
            // 
            // lblSelection
            // 
            this.lblSelection.Image = global::Orbita.Controles.VA.Properties.Resources.ImgSeleccion16;
            this.lblSelection.Name = "lblSelection";
            this.lblSelection.Size = new System.Drawing.Size(16, 17);
            // 
            // CtrlDisplayBitmap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.VisorImagenes);
            this.Controls.Add(this.StatusStripBottom);
            this.Controls.Add(this.ToolStripTop);
            this.Name = "CtrlDisplayBitmap";
            this.Size = new System.Drawing.Size(578, 369);
            this.Load += new System.EventHandler(this.CtrlDisplayBitmap_Load);
            this.ToolStripTop.ResumeLayout(false);
            this.ToolStripTop.PerformLayout();
            this.StatusStripBottom.ResumeLayout(false);
            this.StatusStripBottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer TimerUpdateAutoScrollPosition;
        private System.Windows.Forms.Timer TimerUpdateCursorPosition;
        private System.Windows.Forms.Timer TimerUpdateZoomLevel;
        private System.Windows.Forms.Timer TimerUpdateViewSize;
        private System.Windows.Forms.Timer TimerUpdateSelecction;
        private VisorImagenes VisorImagenes;
        public System.Windows.Forms.ToolStrip ToolStripTop;
        private System.Windows.Forms.ToolStripButton btnOpen;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripSeparator separadorArchivos;
        private System.Windows.Forms.ToolStripButton btnPointer;
        private System.Windows.Forms.ToolStripButton btnHand;
        private System.Windows.Forms.ToolStripButton btnZoomIn;
        private System.Windows.Forms.ToolStripButton btnZoomOut;
        private System.Windows.Forms.ToolStripButton BtnFit;
        private System.Windows.Forms.ToolStripSeparator separadorZoom;
        private System.Windows.Forms.ToolStripLabel lblTituloDisplay;
        private System.Windows.Forms.ToolStripButton btnMaximize;
        private System.Windows.Forms.ToolStripButton btnInfo;
        private System.Windows.Forms.StatusStrip StatusStripBottom;
        private System.Windows.Forms.ToolStripStatusLabel lblMensaje;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lblPixelValue;
        private System.Windows.Forms.ToolStripStatusLabel lblAutoScrollPosition;
        private System.Windows.Forms.ToolStripStatusLabel lblViewArea;
        private System.Windows.Forms.ToolStripStatusLabel lblZoom;
        private System.Windows.Forms.ToolStripStatusLabel lblCursor;
        private System.Windows.Forms.ToolStripStatusLabel lblSelection;
        private System.Windows.Forms.ToolStripButton btnNext;
        private System.Windows.Forms.ToolStripButton btnPrev;



    }
}
