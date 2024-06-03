//***********************************************************************
// Assembly         : Orbita.Controles.VA
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Windows.Forms;
using Orbita.VAHardware;
using Orbita.VAComun;

namespace Orbita.Controles.VA
{
    /// <summary>
    /// Formulario de monitorización de un módulo de Entradas / Salidas
    /// </summary>
    public partial class FrmMonitorizacionIO : FrmBase
    {
        #region Atributo(s)
        /// <summary>
        /// Código de la tarjeta IO
        /// </summary>
        private string Codigo;

        /// <summary>
        /// Módulo de entradas / salidas
        /// </summary>
        private IHardware Hardware;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codigo">Código del dispositivo IO</param>
        public FrmMonitorizacionIO(string codigo):
            base()
        {
            InitializeComponent();

            this.Codigo = codigo;
            this.Text = @"Monitorización de Entradas / Salidas [" + codigo + "]";

            this.imageListLarge.Images.Add("imgVariableInactiva", Orbita.Controles.VA.Properties.Resources.imgVariableInactiva32);
            this.imageListLarge.Images.Add("imgVariableActiva", Orbita.Controles.VA.Properties.Resources.imgVariableActiva32);

            this.imageListSmall.Images.Add("imgVariableInactiva", Orbita.Controles.VA.Properties.Resources.imgVariableInactiva24);
            this.imageListSmall.Images.Add("imgVariableActiva", Orbita.Controles.VA.Properties.Resources.imgVariableActiva24);
        } 
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Carga y muestra datos del formulario comunes para los tres modos de funcionamiento
        /// </summary>
        protected override void CargarDatosComunes()
        {
            base.CargarDatosComunes();

            this.Hardware = HardwareRuntime.GetHardware(this.Codigo);

            if (this.Hardware != null)
            {
                // Inicializamos el timer de refresco
                this.timerRefresco.Interval = SystemRuntime.Configuracion.CadenciaMonitorizacionMilisegundos;

                foreach (TerminalIOBase terminalIO in this.Hardware.ListaTerminales)
                {
                    this.CrearItem(terminalIO);
                }
            }
        }
        /// <summary>
        /// Establece la habiliacion adecuada de los controles para el modo visualizacion
        /// </summary>
        protected override void EstablecerModoMonitorizacion()
        {
            base.EstablecerModoModificacion();

            this.btnMonitorizar.Visible = true;
            this.ListTerminales.ContextMenuStrip = menuTerminal;
            this.timerRefresco.Enabled = true;
        }
        /// <summary>
        ///  Funciones a realizar al salir del formulario
        /// </summary>
        protected override void AccionesSalir()
        {
            this.timerRefresco.Enabled = false;
        }
        #endregion

        #region Método(s) privado(s)

        /// <summary>
        /// Dibuja un item en la lista de variables
        /// </summary>
        /// <param name="terminalIO">Variable a visualizar</param>
        private void CrearItem(TerminalIOBase terminalIO)
        {
            string codigo = terminalIO.Codigo.ToString();
            string descripcion = terminalIO.Descripcion.ToString();
            string habilitado = terminalIO.Habilitado.ToString();
            string tipo = StringValueAttribute.GetStringValue(terminalIO.TipoTerminalIO);

            ListViewItem item = new ListViewItem();

            item.Name = codigo;
            item.Text = codigo;
            item.ToolTipText = descripcion;
            item.Tag = terminalIO;

            if (tipo != string.Empty)
            {
                if (this.ListTerminales.Groups[tipo] == null)
                {
                    item.Group = this.ListTerminales.Groups.Add(tipo, tipo);
                }
                else
                {
                    item.Group = this.ListTerminales.Groups[tipo];
                }
            }

            ListViewItem.ListViewSubItem subItemValor = new ListViewItem.ListViewSubItem();
            subItemValor.Name = "Valor";
            item.SubItems.Add(subItemValor);
            this.PintarItem(codigo, terminalIO.Valor, item);

            ListViewItem.ListViewSubItem subItemDescTerminal = new ListViewItem.ListViewSubItem();
            subItemDescTerminal.Name = "DescTerminal";
            subItemDescTerminal.Text = descripcion;
            item.SubItems.Add(subItemDescTerminal);

            ListViewItem.ListViewSubItem subItemHabilitadoTerminal = new ListViewItem.ListViewSubItem();
            subItemHabilitadoTerminal.Name = "HabilitadoTerminal";
            subItemHabilitadoTerminal.Text = habilitado;
            item.SubItems.Add(subItemHabilitadoTerminal);

            ListViewItem.ListViewSubItem subItemTipo = new ListViewItem.ListViewSubItem();
            subItemTipo.Name = "Tipo";
            subItemTipo.Text = tipo;
            item.SubItems.Add(subItemTipo);

            this.ListTerminales.Items.Add(item);
        }

        /// <summary>
        /// Extrae el valor del objeto para visualizarlo en la lista
        /// </summary>
        /// <param name="valor">Valor de la variable</param>
        /// <param name="activo">Resultado de tipo bool que indica si la variable está activa o desactiva</param>
        /// <param name="textoValor">Resultado de tipo texto que informa del valor de la variable</param>
        private void ExtraerValorTerminal(object valor, ref bool activo, ref string textoValor)
        {
            activo = false;
            textoValor = string.Empty;
            if (valor != null)
            {
                textoValor = valor.ToString();
                activo = true;
                if (valor is bool)
                {
                    activo = (bool)valor;
                }
            }
        }

        /// <summary>
        /// Visualiza el valor del item
        /// </summary>
        /// <param name="codigo">código de la variable</param>
        /// <param name="valor">valor genérico de la variable</param>
        /// <param name="item">item del componente listview</param>
        private void PintarItem(string codigo, object valor, ListViewItem item)
        {
            string textoValor = string.Empty;
            bool verdadero = false;
            this.ExtraerValorTerminal(valor, ref verdadero, ref textoValor);
            TerminalIOBase terminal = (TerminalIOBase)item.Tag;

            item.SubItems["Valor"].Text = textoValor;

            EnumTipoDato tipo = terminal.TipoDato;
            switch (tipo)
            {
                case EnumTipoDato.SinDefinir:
                case EnumTipoDato.Bit:
                case EnumTipoDato.Flag:
                default:
                    if (verdadero)
                    {
                        item.ImageKey = "imgVariableActiva";
                    }
                    else if (!verdadero)
                    {
                        item.ImageKey = "imgVariableInactiva";
                    }
                    break;
                case EnumTipoDato.Entero:
                case EnumTipoDato.Decimal:
                case EnumTipoDato.Fecha:
                    item.ImageKey = "imgNumero";
                    break;
                case EnumTipoDato.Texto:
                    item.ImageKey = "imgTexto";
                    break;
                case EnumTipoDato.Imagen:
                case EnumTipoDato.Grafico:
                    item.ImageKey = "imgFoto";
                    break;
            }
        }

        #endregion

        #region Eventos
        /// <summary>
        /// Refresca la visualización de las terminales
        /// </summary>
        private void RefrescarTerminales(string codigo, object valor, ListViewItem item)
        {
            try
            {
                this.PintarItem(codigo, valor, item);
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.Monitorizacion, this.Name, exception);
            }
        }

        /// <summary>
        /// Refresca la visualización de los terminales
        /// </summary>
        private void RefrescarTodosTerminales()
        {
            try
            {
                foreach (ListViewItem item in this.ListTerminales.Items)
                {
                    string codigo = item.Text;
                    TerminalIOBase terminalIO = (TerminalIOBase)item.Tag;
                    object valor = terminalIO.Valor;

                    //if (terminalIO.TipoVariable == EnumTipoDato.Bit)
                    {
                        this.RefrescarTerminales(codigo, valor, item);
                    }
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.Monitorizacion, this.Name, exception);
            }
        }

        /// <summary>
        /// Monitorizar el terminal seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMonitorizar_Click(object sender, EventArgs e)
        {
            try
            {
            //    switch (this._ModoAperturaFormulario)
            //    {
            //        case ModoAperturaFormulario.Visualizacion:
            //            {
            //                if (this.ListTerminales.SelectedItems.Count > 0)
            //                {
            //                    foreach (ListViewItem item in this.ListTerminales.SelectedItems)
            //                    {
            //                        try
            //                        {
            //                            FrmVariableChart frm = new FrmVariableChart(item.Text);
            //                            //App.FormularioPrincipalMDI.OrbMdiEncolarForm(frm);
            //                            frm.Show();
            //                        }
            //                        catch (Exception exception)
            //                        {
            //                            LogsRuntime.Warning(ModulosHardware.Monitorizacion, this.Name, "Warning: " + exception.ToString());
            //                        }
            //                    }
            //                }
            //                break;
            //            }
            //    }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.Monitorizacion, this.Name, exception);
            }
        }

        /// <summary>
        /// Clic en la lista del botón desplegable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraToolbarsManager_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            try
            {
                switch (e.Tool.Key)
                {
                    case "KeyIconosGrandes":
                        this.ListTerminales.View = View.LargeIcon;
                        break;
                    case "KeyIconosPequeños":
                        this.ListTerminales.View = View.SmallIcon;
                        break;
                    case "KeyLista":
                        this.ListTerminales.View = View.List;
                        break;
                    case "KeyDetalles":
                        this.ListTerminales.View = View.Details;
                        break;
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.Monitorizacion, this.Name, exception);
            }
        }

        /// <summary>
        /// Timer de refresco del estado actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerRefresco_Tick(object sender, EventArgs e)
        {
            this.timerRefresco.Enabled = false;
            try
            {
                this.RefrescarTodosTerminales();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosHardware.Monitorizacion, this.Name, exception);
            }
            this.timerRefresco.Enabled = true;
        }
        #endregion
    }
}
