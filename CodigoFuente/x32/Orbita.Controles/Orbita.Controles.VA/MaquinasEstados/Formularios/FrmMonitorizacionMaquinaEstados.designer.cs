using Orbita.VAControl;
namespace Orbita.Controles.VA
{
	partial class FrmMonitorizacionMaquinaEstados
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
            this.splitPrincipal = new Orbita.Controles.OrbitaSplitContainer();
            this.ctrlStateMachineDisplay = new Orbita.Controles.VA.OrbitaVisorMaquinaEstados();
            this.picMaquinaEstados = new Orbita.Controles.OrbitaPictureBox(this.components);
            this.ListMensajes = new Orbita.Controles.OrbitaListView(this.components);
            this.Tipo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Descripcion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Hora = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            this.pnlPanelPrincipalPadre.SuspendLayout();
            this.splitPrincipal.Panel1.SuspendLayout();
            this.splitPrincipal.Panel2.SuspendLayout();
            this.splitPrincipal.SuspendLayout();
            this.ctrlStateMachineDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMaquinaEstados)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlInferiorPadre
            // 
            this.pnlInferiorPadre.Location = new System.Drawing.Point(10, 476);
            this.pnlInferiorPadre.Size = new System.Drawing.Size(851, 43);
            // 
            // pnlPanelPrincipalPadre
            // 
            this.pnlPanelPrincipalPadre.Controls.Add(this.splitPrincipal);
            this.pnlPanelPrincipalPadre.Size = new System.Drawing.Size(851, 466);
            // 
            // splitPrincipal
            // 
            this.splitPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitPrincipal.Location = new System.Drawing.Point(0, 0);
            this.splitPrincipal.Name = "splitPrincipal";
            this.splitPrincipal.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitPrincipal.Panel1
            // 
            this.splitPrincipal.Panel1.Controls.Add(this.ctrlStateMachineDisplay);
            this.splitPrincipal.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitPrincipal.Panel2
            // 
            this.splitPrincipal.Panel2.Controls.Add(this.ListMensajes);
            this.splitPrincipal.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitPrincipal.Panel2MinSize = 50;
            this.splitPrincipal.Size = new System.Drawing.Size(851, 466);
            this.splitPrincipal.SplitterDistance = 357;
            this.splitPrincipal.TabIndex = 13;
            // 
            // ctrlStateMachineDisplay
            // 
            this.ctrlStateMachineDisplay.Controls.Add(this.picMaquinaEstados);
            this.ctrlStateMachineDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlStateMachineDisplay.Location = new System.Drawing.Point(0, 0);
            this.ctrlStateMachineDisplay.Name = "ctrlStateMachineDisplay";
            this.ctrlStateMachineDisplay.Size = new System.Drawing.Size(851, 357);
            this.ctrlStateMachineDisplay.TabIndex = 15;
            this.ctrlStateMachineDisplay.OnMensajeMaquinaEstados += new MensajeMaquinaEstadosLanzado(this.ctrlStateMachineDisplay_OnMensajeMaquinaEstadosRecibido);
            // 
            // picMaquinaEstados
            // 
            this.picMaquinaEstados.BackColor = System.Drawing.SystemColors.Window;
            this.picMaquinaEstados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picMaquinaEstados.Location = new System.Drawing.Point(0, 0);
            this.picMaquinaEstados.Name = "picMaquinaEstados";
            this.picMaquinaEstados.Size = new System.Drawing.Size(851, 357);
            this.picMaquinaEstados.TabIndex = 0;
            this.picMaquinaEstados.TabStop = false;
            // 
            // ListMensajes
            // 
            this.ListMensajes.AllowColumnReorder = true;
            this.ListMensajes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Tipo,
            this.Descripcion,
            this.Hora});
            this.ListMensajes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListMensajes.FullRowSelect = true;
            this.ListMensajes.GridLines = true;
            this.ListMensajes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ListMensajes.LargeImageList = this.ImageList;
            this.ListMensajes.Location = new System.Drawing.Point(0, 0);
            this.ListMensajes.MultiSelect = false;
            this.ListMensajes.Name = "ListMensajes";
            this.ListMensajes.ShowItemToolTips = true;
            this.ListMensajes.Size = new System.Drawing.Size(851, 105);
            this.ListMensajes.SmallImageList = this.ImageList;
            this.ListMensajes.TabIndex = 16;
            this.ListMensajes.UseCompatibleStateImageBehavior = false;
            this.ListMensajes.View = System.Windows.Forms.View.Details;
            // 
            // Tipo
            // 
            this.Tipo.Text = "Tipo";
            this.Tipo.Width = 150;
            // 
            // Descripcion
            // 
            this.Descripcion.Text = "Descripción";
            this.Descripcion.Width = 500;
            // 
            // Hora
            // 
            this.Hora.Text = "Hora";
            this.Hora.Width = 150;
            // 
            // ImageList
            // 
            this.ImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ImageList.ImageSize = new System.Drawing.Size(24, 24);
            this.ImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // FrmMonitorizacionMaquinaEstados
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 529);
            this.ModoAperturaFormulario = Orbita.Controles.VA.ModoAperturaFormulario.Monitorizacion;
            this.MultiplesInstancias = true;
            this.Name = "FrmMonitorizacionMaquinaEstados";
            this.Text = "Monitorización de máquinas de estado";
            this.pnlPanelPrincipalPadre.ResumeLayout(false);
            this.splitPrincipal.Panel1.ResumeLayout(false);
            this.splitPrincipal.Panel2.ResumeLayout(false);
            this.splitPrincipal.ResumeLayout(false);
            this.ctrlStateMachineDisplay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picMaquinaEstados)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

        private Orbita.Controles.OrbitaSplitContainer splitPrincipal;
        private Orbita.Controles.OrbitaListView ListMensajes;
        private System.Windows.Forms.ColumnHeader Tipo;
        private System.Windows.Forms.ColumnHeader Descripcion;
        private System.Windows.Forms.ColumnHeader Hora;
        private Orbita.Controles.VA.OrbitaVisorMaquinaEstados ctrlStateMachineDisplay;
        private Orbita.Controles.OrbitaPictureBox picMaquinaEstados;
        private System.Windows.Forms.ImageList ImageList;


    }
}