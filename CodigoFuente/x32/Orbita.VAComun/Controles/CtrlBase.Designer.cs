﻿namespace Orbita.VAComun
{
    partial class CtrlBase
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.pnlInferiorPadre = new Orbita.Controles.OrbitaPanel();
            this.ChkDock = new Orbita.Controles.OrbitaCheckBox();
            this.ChkToolTip = new Orbita.Controles.OrbitaCheckBox();
            this.pnlBotonesPadre = new Orbita.Controles.OrbitaPanel();
            this.btnGuardar = new Orbita.Controles.OrbitaButton(this.components);
            this.btnCancelar = new Orbita.Controles.OrbitaButton(this.components);
            this.pnlPanelPrincipalPadre = new Orbita.Controles.OrbitaPanel();
            this.orbitaTooltipOrbita = new Orbita.Controles.OrbitaTooltip(this.components);
            this.pnlInferiorPadre.SuspendLayout();
            this.pnlBotonesPadre.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlInferiorPadre
            // 
            this.pnlInferiorPadre.Controls.Add(this.ChkDock);
            this.pnlInferiorPadre.Controls.Add(this.ChkToolTip);
            this.pnlInferiorPadre.Controls.Add(this.pnlBotonesPadre);
            this.pnlInferiorPadre.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlInferiorPadre.Location = new System.Drawing.Point(10, 327);
            this.pnlInferiorPadre.Name = "pnlInferiorPadre";
            this.pnlInferiorPadre.Size = new System.Drawing.Size(670, 43);
            this.pnlInferiorPadre.TabIndex = 20;
            // 
            // ChkDock
            // 
            appearance1.Image = global::Orbita.VAComun.Properties.Resources.btnAncla24;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.ChkDock.Appearance = appearance1;
            this.ChkDock.Location = new System.Drawing.Point(39, 10);
            this.ChkDock.Name = "ChkDock";
            this.ChkDock.Size = new System.Drawing.Size(33, 33);
            this.ChkDock.Style = Infragistics.Win.EditCheckStyle.Button;
            this.ChkDock.TabIndex = 29;
            // 
            // ChkToolTip
            // 
            appearance2.Image = global::Orbita.VAComun.Properties.Resources.BtnToolTips24;
            appearance2.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance2.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.ChkToolTip.Appearance = appearance2;
            this.ChkToolTip.Location = new System.Drawing.Point(0, 10);
            this.ChkToolTip.Name = "ChkToolTip";
            this.ChkToolTip.Size = new System.Drawing.Size(33, 33);
            this.ChkToolTip.Style = Infragistics.Win.EditCheckStyle.Button;
            this.ChkToolTip.TabIndex = 28;
            this.ChkToolTip.CheckedChanged += new System.EventHandler(this.ChkToolTip_CheckedChanged);
            // 
            // pnlBotonesPadre
            // 
            this.pnlBotonesPadre.Controls.Add(this.btnGuardar);
            this.pnlBotonesPadre.Controls.Add(this.btnCancelar);
            this.pnlBotonesPadre.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlBotonesPadre.Location = new System.Drawing.Point(468, 0);
            this.pnlBotonesPadre.Name = "pnlBotonesPadre";
            this.pnlBotonesPadre.Size = new System.Drawing.Size(202, 43);
            this.pnlBotonesPadre.TabIndex = 24;
            // 
            // btnGuardar
            // 
            appearance3.Image = global::Orbita.VAComun.Properties.Resources.btnOk24;
            this.btnGuardar.Appearance = appearance3;
            this.btnGuardar.ImageSize = new System.Drawing.Size(24, 24);
            this.btnGuardar.Location = new System.Drawing.Point(0, 10);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(98, 33);
            this.btnGuardar.TabIndex = 22;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnCancelar
            // 
            appearance4.Image = global::Orbita.VAComun.Properties.Resources.BtnNok24;
            this.btnCancelar.Appearance = appearance4;
            this.btnCancelar.ImageSize = new System.Drawing.Size(24, 24);
            this.btnCancelar.Location = new System.Drawing.Point(104, 10);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(98, 33);
            this.btnCancelar.TabIndex = 23;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // pnlPanelPrincipalPadre
            // 
            this.pnlPanelPrincipalPadre.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPanelPrincipalPadre.Location = new System.Drawing.Point(10, 10);
            this.pnlPanelPrincipalPadre.Name = "pnlPanelPrincipalPadre";
            this.pnlPanelPrincipalPadre.Size = new System.Drawing.Size(670, 317);
            this.pnlPanelPrincipalPadre.TabIndex = 22;
            // 
            // orbitaTooltipOrbita
            // 
            this.orbitaTooltipOrbita.AutomaticDelay = 1000;
            this.orbitaTooltipOrbita.ShowAlways = true;
            // 
            // CtrlBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlPanelPrincipalPadre);
            this.Controls.Add(this.pnlInferiorPadre);
            this.Name = "CtrlBase";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(690, 380);
            this.pnlInferiorPadre.ResumeLayout(false);
            this.pnlBotonesPadre.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected Orbita.Controles.OrbitaPanel pnlInferiorPadre;
        protected Orbita.Controles.OrbitaButton btnCancelar;
        protected Orbita.Controles.OrbitaButton btnGuardar;
        private Orbita.Controles.OrbitaPanel pnlBotonesPadre;
        public Orbita.Controles.OrbitaPanel pnlPanelPrincipalPadre;
        private Orbita.Controles.OrbitaCheckBox ChkToolTip;
        private Orbita.Controles.OrbitaCheckBox ChkDock;
        private Orbita.Controles.OrbitaTooltip orbitaTooltipOrbita;
    }
}
