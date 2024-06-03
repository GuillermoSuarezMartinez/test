namespace Orbita.Controles.VA
{
    partial class FrmGestionVariables
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
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.gridVariables = new Orbita.Controles.OrbitaGridPro();
            this.pnlPanelPrincipalPadre.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlInferiorPadre
            // 
            this.pnlInferiorPadre.Location = new System.Drawing.Point(10, 425);
            this.pnlInferiorPadre.Size = new System.Drawing.Size(970, 43);
            // 
            // pnlPanelPrincipalPadre
            // 
            this.pnlPanelPrincipalPadre.Controls.Add(this.gridVariables);
            this.pnlPanelPrincipalPadre.Size = new System.Drawing.Size(970, 415);
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(970, 433);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(10, 10);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(970, 458);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // gridVariables
            // 
            this.gridVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridVariables.Location = new System.Drawing.Point(0, 0);
            this.gridVariables.Name = "gridVariables";
            this.gridVariables.OrbAutoAjusteColumnas = Infragistics.Win.UltraWinGrid.AutoFitStyle.None;
            this.gridVariables.OrbMostrarBotonAñadirFila = true;
            this.gridVariables.OrbMostrarBotonEliminarFila = true;
            this.gridVariables.OrbMostrarBotonModificarFila = true;
            this.gridVariables.OrbMostrarBotonRefrescar = true;
            this.gridVariables.Size = new System.Drawing.Size(970, 415);
            this.gridVariables.TabIndex = 3;
            this.gridVariables.OrbBotonRefrescarClick += new Orbita.Controles.OrbitaGridPro.orbDelegadoRefrescar(this.gridVariables_OrbBotonRefrescarClick);
            this.gridVariables.OrbBotonEliminarFilaClick += new Orbita.Controles.OrbitaGridPro.orbDelegadoEliminarFila(this.gridVariables_OrbBotonEliminarFilaClick);
            this.gridVariables.OrbBotonModificarClick += new Orbita.Controles.OrbitaGridPro.orbDelegadoModificar(this.gridVariables_OrbBotonModificarClick);
            this.gridVariables.OrbBotonAñadirClick += new Orbita.Controles.OrbitaGridPro.orbDelegadoAñadir(this.gridVariables_OrbBotonAñadirClick);
            // 
            // FrmGestionVariables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 478);
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "FrmGestionVariables";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.Text = "Gestión de variables";
            this.Controls.SetChildIndex(this.toolStripContainer1, 0);
            this.Controls.SetChildIndex(this.pnlInferiorPadre, 0);
            this.Controls.SetChildIndex(this.pnlPanelPrincipalPadre, 0);
            this.pnlPanelPrincipalPadre.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.DataGridViewTextBoxColumn idVariableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn codVariableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombreVariableDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn habilitadoVariableDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn grupoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn guardarTrazabilidadDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descVariableDataGridViewTextBoxColumn;
        private Orbita.Controles.OrbitaGridPro gridVariables;
    }
}