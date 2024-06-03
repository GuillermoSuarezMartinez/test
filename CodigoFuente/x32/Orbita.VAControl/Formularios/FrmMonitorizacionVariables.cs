//***********************************************************************
// Assembly         : Orbita.VAControl
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Infragistics.Win.UltraWinToolbars;
using Orbita.VAComun;

namespace Orbita.VAControl
{
    /// <summary>
    /// Formulario para la monitorización de variables
    /// </summary>
    public partial class FrmMonitorizacionVariables : FrmBase
    {
        #region Atributo(s)
        /// <summary>
        /// Momento en el que se produjo el último refresco de las variables
        /// </summary>
        private DateTime MomentoUltimoRefresco;

        /// <summary>
        /// Código del escenario actual
        /// </summary>
        private string CodEscenarioActual;

        /// <summary>
        /// Listado de las variables que se están visualizando actualmente
        /// </summary>
        private List<string> ListaCodVariablesActuales;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la calse
        /// </summary>
        public FrmMonitorizacionVariables() :
            base()
        {
            InitializeComponent();

            this.imageListLarge.Images.Add("imgVariableInactiva", Orbita.VAControl.Properties.Resources.imgVariableInactiva32);
            this.imageListLarge.Images.Add("imgVariableInactivaForzada", Orbita.VAControl.Properties.Resources.imgVariableInactivaForzada32);
            this.imageListLarge.Images.Add("imgVariableActiva", Orbita.VAControl.Properties.Resources.imgVariableActiva32);
            this.imageListLarge.Images.Add("imgVariableActivaForzada", Orbita.VAControl.Properties.Resources.imgVariableActivaForzada32);
            this.imageListLarge.Images.Add("imgTexto", Orbita.VAControl.Properties.Resources.imgTexto32);
            this.imageListLarge.Images.Add("imgTextoForzada", Orbita.VAControl.Properties.Resources.imgTextoForzada32);
            this.imageListLarge.Images.Add("imgNumero", Orbita.VAControl.Properties.Resources.imgNumero32);
            this.imageListLarge.Images.Add("imgNumeroForzada", Orbita.VAControl.Properties.Resources.imgNumeroForzada32);
            this.imageListLarge.Images.Add("imgFoto", Orbita.VAControl.Properties.Resources.imgFoto32);
            this.imageListLarge.Images.Add("imgFotoForzada", Orbita.VAControl.Properties.Resources.imgFotoForzada32);

            this.imageListSmall.Images.Add("imgVariableInactiva", Orbita.VAControl.Properties.Resources.imgVariableInactiva24);
            this.imageListSmall.Images.Add("imgVariableInactivaForzada", Orbita.VAControl.Properties.Resources.imgVariableInactivaForzada24);
            this.imageListSmall.Images.Add("imgVariableActiva", Orbita.VAControl.Properties.Resources.imgVariableActiva24);
            this.imageListSmall.Images.Add("imgVariableActivaForzada", Orbita.VAControl.Properties.Resources.imgVariableActivaForzada24);
            this.imageListSmall.Images.Add("imgTexto", Orbita.VAControl.Properties.Resources.imgTexto24);
            this.imageListSmall.Images.Add("imgTextoForzada", Orbita.VAControl.Properties.Resources.imgTextoForzada24);
            this.imageListSmall.Images.Add("imgNumero", Orbita.VAControl.Properties.Resources.imgNumero24);
            this.imageListSmall.Images.Add("imgNumeroForzada", Orbita.VAControl.Properties.Resources.imgNumeroForzada24);
            this.imageListSmall.Images.Add("imgFoto", Orbita.VAControl.Properties.Resources.imgFoto24);
            this.imageListSmall.Images.Add("imgFotoForzada", Orbita.VAControl.Properties.Resources.imgFotoForzada24);

            // Inicialización de las variables
            this.CodEscenarioActual = string.Empty;
            this.ListaCodVariablesActuales = new List<string>();

            // Rellenamos el menú de escenarios
            PopupMenuTool menuEscenarios = (PopupMenuTool)this.toolbarsManager.Tools["Escenarios"];

            // Menu de todos los escenarios
            ButtonTool btnVistaTodos = new ButtonTool("Todos");
            btnVistaTodos.SharedProps.Caption = "Todos";
            btnVistaTodos.ToolClick += EscenarioClick;
            this.toolbarsManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] { btnVistaTodos });
            menuEscenarios.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] { btnVistaTodos });

            // Menu de cada escenario
            DataTable dt = Orbita.VAComun.AppBD.GetVistas();
            foreach (DataRow dr in dt.Rows)
            {
                string codVista = dr["CodVista"].ToString();
                string nombreVista = dr["NombreVista"].ToString();
                ButtonTool btnVista = new ButtonTool(codVista);
                btnVista.SharedProps.Caption = nombreVista;
                btnVista.ToolClick += EscenarioClick;
                this.toolbarsManager.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] { btnVista });
                menuEscenarios.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] { btnVista });
            }
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Carga y muestra datos del formulario comunes para los tres modos de funcionamiento
        /// </summary>
        protected override void CargarDatosComunes()
        {
            base.CargarDatosComunes();

            // Inicializamos el timer de refresco
            this.MomentoUltimoRefresco = DateTime.Now;
            this.timerRefresco.Interval = SystemRuntime.Configuracion.CadenciaMonitorizacionMilisegundos;
        }
        /// <summary>
        /// Carga y muestra datos del formulario en modo nuevo. Se cargan todos datos que se muestran en 
        /// el formulario: grids, combos, etc... Cada carga de elementos estará encapsulada en un método
        /// </summary>
        protected override void CargarDatosModoMonitorizacion()
        {
            base.CargarDatosModoMonitorizacion();

            this.CrearItems();
        }
        /// <summary>
        /// Establece la habiliacion adecuada de los controles para el modo visualizacion
        /// </summary>
        protected override void EstablecerModoMonitorizacion()
        {
            base.EstablecerModoMonitorizacion();

            this.btnMonitorizar.Visible = true;
            this.ListVariables.ContextMenuStrip = menuVariable;
        }
        /// <summary>
        ///  Funciones a realizar al salir del formulario
        /// </summary>
        protected override void AccionesSalir()
        {
            base.AccionesSalir();

            this.timerRefresco.Enabled = false;
            this.DestruirItems();
        }
        #endregion

        #region Método(s) privado(s)

        /// <summary>
        /// Crea los items de las variables
        /// </summary>
        private void CrearItems()
        {
            // Creamos la lista de variables
            if (this.CodEscenarioActual == string.Empty)
            {
                // Todas las variables
                foreach (VariableItem variable in VariableRuntime.ListaVariables.Values)
                {
                    this.ListaCodVariablesActuales.Add(variable.Codigo);
                }
            }
            else
            {
                // Variables de una vista
                VistaVariable vistaVariable = VariableRuntime.Vistas[this.CodEscenarioActual]; // Extraigo la vista acutal
                foreach (KeyValuePair<string,string> aliasPair in vistaVariable.ListaAlias) // Para cada alias
                {
                    this.ListaCodVariablesActuales.Add(aliasPair.Value); // Guardo el código de la variable al que hace referencia
                }
            }

            foreach (string codVariable in this.ListaCodVariablesActuales)
            {
                VariableItem variable = VariableRuntime.GetVariable(codVariable);
                this.CrearItem(variable);

                switch (variable.Tipo)
                {
                    case EnumTipoDato.Bit:
                    case EnumTipoDato.Entero:
                    case EnumTipoDato.Texto:
                    case EnumTipoDato.Decimal:
                    case EnumTipoDato.Fecha:
                        VariableRuntime.CrearSuscripcion(variable.Codigo, "Monitorización", this.Name, this.EventoRefrescarVariables);
                        break;
                }
            }
        }

        /// <summary>
        /// Destruye los items de todas las variables
        /// </summary>
        private void DestruirItems()
        {
            foreach (string codVariable in this.ListaCodVariablesActuales)                
            {
                VariableRuntime.EliminarSuscripcion(codVariable, "Monitorización", this.Name, this.EventoRefrescarVariables);
            }

            this.ListaCodVariablesActuales.Clear();
            this.ListVariables.Items.Clear();
        }

        /// <summary>
        /// Dibuja un item en la lista de variables
        /// </summary>
        /// <param name="variable">Variable a visualizar</param>
        private void CrearItem(VariableItem variable)
        {
            string codigo = variable.Codigo.ToString();
            string descripcion = variable.Descripcion.ToString();
            string habilitado = variable.Habilitado.ToString();
            string guardarTrazabilidad = variable.GuardarTrazabilidad.ToString();
            string grupo = variable.Grupo.ToString();

            ListViewItem item = new ListViewItem();

            item.Name = codigo;
            item.Text = codigo;
            item.ToolTipText = descripcion;
            item.Tag = variable;

            if (grupo != string.Empty)
            {
                if (this.ListVariables.Groups[grupo] == null)
                {
                    item.Group = this.ListVariables.Groups.Add(grupo, grupo);
                }
                else
                {
                    item.Group = this.ListVariables.Groups[grupo];
                }
            }

            ListViewItem.ListViewSubItem subItemValor = new ListViewItem.ListViewSubItem();
            subItemValor.Name = "Valor";
            item.SubItems.Add(subItemValor);
            this.PintarItem(codigo, variable.GetValor(), item);

            ListViewItem.ListViewSubItem subItemDescVariable = new ListViewItem.ListViewSubItem();
            subItemDescVariable.Name = "DescVariable";
            subItemDescVariable.Text = descripcion;
            item.SubItems.Add(subItemDescVariable);

            ListViewItem.ListViewSubItem subItemHabilitadoVariable = new ListViewItem.ListViewSubItem();
            subItemHabilitadoVariable.Name = "HabilitadoVariable";
            subItemHabilitadoVariable.Text = habilitado;
            item.SubItems.Add(subItemHabilitadoVariable);

            ListViewItem.ListViewSubItem subItemGuardarTrazabilidad = new ListViewItem.ListViewSubItem();
            subItemGuardarTrazabilidad.Name = "GuardarTrazabilidad";
            subItemGuardarTrazabilidad.Text = guardarTrazabilidad;
            item.SubItems.Add(subItemGuardarTrazabilidad);

            this.ListVariables.Items.Add(item);
        }

        /// <summary>
        /// Extrae el valor del objeto para visualizarlo en la lista
        /// </summary>
        /// <param name="valor">Valor de la variable</param>
        /// <param name="activo">Resultado de tipo bool que indica si la variable está activa o desactiva</param>
        /// <param name="textoValor">Resultado de tipo texto que informa del valor de la variable</param>
        private void ExtraerValorVariable(object valor, ref bool activo, ref string textoValor)
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
            this.ExtraerValorVariable(valor, ref verdadero, ref textoValor);
            VariableItem variable = (VariableItem)item.Tag;
            bool bloqueado = variable.Bloqueado;

            item.SubItems["Valor"].Text = textoValor;

            EnumTipoDato tipo = variable.Tipo;
            switch (tipo)
            {
                case EnumTipoDato.SinDefinir:
                case EnumTipoDato.Flag:
                case EnumTipoDato.Bit:
                    if (verdadero && !bloqueado)
                    {
                        item.ImageKey = "imgVariableActiva";
                    }
                    else if (!verdadero && !bloqueado)
                    {
                        item.ImageKey = "imgVariableInactiva";
                    }
                    else if (verdadero && bloqueado)
                    {
                        item.ImageKey = "imgVariableActivaForzada";
                    }
                    else if (!verdadero && bloqueado)
                    {
                        item.ImageKey = "imgVariableInactivaForzada";
                    }
                    break;
                case EnumTipoDato.Entero:
                case EnumTipoDato.Decimal:
                case EnumTipoDato.Fecha:
                    if (bloqueado)
                    {
                        item.ImageKey = "imgNumeroForzada";
                    }
                    else
                    {
                        item.ImageKey = "imgNumero";
                    }
                    break;
                case EnumTipoDato.Texto:
                    if (bloqueado)
                    {
                        item.ImageKey = "imgTextoForzada";
                    }
                    else
                    {
                        item.ImageKey = "imgTexto";
                    }
                    break;
                case EnumTipoDato.Imagen:
                case EnumTipoDato.Grafico:
                    if (bloqueado)
                    {
                        item.ImageKey = "imgFotoForzada";
                    }
                    else
                    {
                        item.ImageKey = "imgFoto";
                    }
                    break;
            }
        }

        /// <summary>
        /// Fuerza el valor de una variable
        /// </summary>
        /// <param name="variable"></param>
        private void ForzarValor(VariableItem variable)
        {
            if (variable.Bloqueado)
            {
                object objValor = variable.GetValor();
                switch (variable.Tipo)
                {
                    case EnumTipoDato.Entero:
                        break;
                    case EnumTipoDato.Texto:
                        InputTextBox.Show(variable.Codigo, "Forzado de valor", "Escriba el nuevo valor de la variable " + variable.Codigo, App.ToString(objValor), this.TextoImputadoPorUsuario);
                        break;
                    case EnumTipoDato.Decimal:
                        break;
                    case EnumTipoDato.Fecha:
                        break;
                }
            }
        }

        #endregion

        #region Eventos
        /// <summary>
        /// Refresca la visualización de las variables
        /// </summary>
        private void EventoRefrescarVariables(string codigo, object valor)
        {
            try
            {
                TimeSpan tiempoSinRefrescar = DateTime.Now - MomentoUltimoRefresco;
                if (tiempoSinRefrescar > VariableRuntime.CadenciaMonitorizacion)
                {
                    // Si hace más de x tiempo que se refresco, volvemos a referescar
                    ListViewItem[] items = this.ListVariables.Items.Find(codigo, false);

                    if (items.Length > 0)
                    {
                        this.RefrescarVariables(codigo, valor, items[0]);
                    }
                }
                else
                {
                    // Si hace menos de x tiempo que se refresco, ponemos un timer
                    if (!this.timerRefresco.Enabled)
                    {
                        this.timerRefresco.Enabled = true;
                    }
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosControl.MonitorizacionVariables, this.Name, exception);
            }
        }

        /// <summary>
        /// Refresca la visualización de las variables
        /// </summary>
        private void RefrescarVariables(string codigo, object valor, ListViewItem item)
        {
            try
            {
                // Reseteamos el timer para que no se vuelva a refrescar
                this.MomentoUltimoRefresco = DateTime.Now;
                this.timerRefresco.Enabled = false;

                this.PintarItem(codigo, valor, item);
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosControl.MonitorizacionVariables, this.Name, exception);
            }
        }

        /// <summary>
        /// Refresca la visualización de las variables
        /// </summary>
        private void RefrescarTodasVariables()
        {
            try
            {
                foreach (ListViewItem item in this.ListVariables.Items)
                {
                    string codigo = item.Text;
                    object valor = VariableRuntime.GetValue(codigo);
                    VariableItem variable = (VariableItem)item.Tag;

                    switch (variable.Tipo)
                    {
                        case EnumTipoDato.Bit:
                        case EnumTipoDato.Entero:
                        case EnumTipoDato.Texto:
                        case EnumTipoDato.Decimal:
                        case EnumTipoDato.Fecha:
                            this.RefrescarVariables(codigo, valor, item);
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosControl.MonitorizacionVariables, this.Name, exception);
            }
        }

        /// <summary>
        /// Monitorizar la variable seleccionada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMonitorizar_Click(object sender, EventArgs e)
        {
            try
            {
                switch (this._ModoAperturaFormulario)
                {
                    case ModoAperturaFormulario.Monitorizacion:
                        {
                            if (this.ListVariables.SelectedItems.Count > 0)
                            {
                                foreach (ListViewItem item in this.ListVariables.SelectedItems)
                                {
                                    try
                                    {
                                        FrmVariableChart frm = new FrmVariableChart(item.Text);
                                        //App.FormularioPrincipalMDI.OrbMdiEncolarForm(frm);
                                        frm.Show();
                                    }
                                    catch (Exception exception)
                                    {
                                        LogsRuntime.Fatal(ModulosControl.MonitorizacionVariables, this.Name, exception);
                                    }
                                }
                            }
                            break;
                        }
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosControl.MonitorizacionVariables, this.Name, exception);
            }
        }

        /// <summary>
        /// Evento que se ejecuta al abrir el menú popup de las variables
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuVariable_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                if (this.ListVariables.FocusedItem != null)
                {
                    VariableItem variable = (VariableItem)this.ListVariables.FocusedItem.Tag;

                    // Menú de bloqueo
                    this.menuBloquearVariable.Visible = !variable.Bloqueado;
                    this.menuDesbloquearVariable.Visible = variable.Bloqueado;
                    this.menuForzarValor.Enabled = variable.Bloqueado;
                    this.menuForzarValorFalso.Enabled = variable.Bloqueado;
                    this.menuForzarValorVerdadero.Enabled = variable.Bloqueado;
                    this.menuCargarFoto.Enabled = variable.Bloqueado;
                    this.menuGuardarFoto.Enabled = variable.Bloqueado;

                    // Menú de Forzado
                    switch (variable.Tipo)
                    {
                        case EnumTipoDato.Flag:
                            this.menuForzarValor.Visible = true;
                            this.menuForzarValorFalso.Visible = false;
                            this.menuForzarValorVerdadero.Visible = false;
                            this.menuCargarFoto.Visible = false;
                            this.menuGuardarFoto.Visible = false;
                            break;
                        case EnumTipoDato.Bit:
                            bool boolValor = false;
                            object objValor = variable.GetValor();
                            if ((objValor is bool) && ((bool)objValor))
                            {
                                boolValor = true;
                            }

                            this.menuForzarValor.Visible = false;
                            this.menuForzarValorFalso.Visible = boolValor;
                            this.menuForzarValorVerdadero.Visible = !boolValor;
                            this.menuCargarFoto.Visible = false;
                            this.menuGuardarFoto.Visible = false;
                            break;
                        case EnumTipoDato.Entero:
                            this.menuForzarValor.Visible = true;
                            this.menuForzarValorFalso.Visible = false;
                            this.menuForzarValorVerdadero.Visible = false;
                            this.menuCargarFoto.Visible = false;
                            this.menuGuardarFoto.Visible = false;
                            break;
                        case EnumTipoDato.Texto:
                            this.menuForzarValor.Visible = true;
                            this.menuForzarValorFalso.Visible = false;
                            this.menuForzarValorVerdadero.Visible = false;
                            this.menuCargarFoto.Visible = false;
                            this.menuGuardarFoto.Visible = false;
                            break;
                        case EnumTipoDato.Fecha:
                            this.menuForzarValor.Visible = true;
                            this.menuForzarValorFalso.Visible = false;
                            this.menuForzarValorVerdadero.Visible = false;
                            this.menuCargarFoto.Visible = false;
                            this.menuGuardarFoto.Visible = false;
                            break;
                        case EnumTipoDato.Imagen:
                            this.menuForzarValor.Visible = false;
                            this.menuForzarValorFalso.Visible = false;
                            this.menuForzarValorVerdadero.Visible = false;
                            this.menuCargarFoto.Visible = true;
                            this.menuGuardarFoto.Visible = true;
                            break;
                        default:
                            this.menuForzarValor.Visible = false;
                            this.menuForzarValorFalso.Visible = false;
                            this.menuForzarValorVerdadero.Visible = false;
                            this.menuCargarFoto.Visible = false;
                            this.menuGuardarFoto.Visible = false;
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosControl.MonitorizacionVariables, this.Name, exception);
            }
        }

        /// <summary>
        /// Bloquea una variable para que nadie pueda modificarla excepto forzando su valor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuBloquearVariable_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ListVariables.FocusedItem != null)
                {
                    string codigo = this.ListVariables.FocusedItem.Text;
                    VariableRuntime.Bloquear(codigo, "Monitorizacion", this.Name);

                    this.PintarItem(codigo, VariableRuntime.GetValue(codigo), this.ListVariables.FocusedItem);
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosControl.MonitorizacionVariables, this.Name, exception);
            }
        }

        /// <summary>
        /// Desbloquea una variable para que cualquiera pueda modificarla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuDesbloquearVariable_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ListVariables.FocusedItem != null)
                {
                    string codigo = this.ListVariables.FocusedItem.Text;
                    VariableRuntime.Desbloquear(codigo, "Monitorizacion", this.Name);

                    this.PintarItem(codigo, VariableRuntime.GetValue(codigo), this.ListVariables.FocusedItem);
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosControl.MonitorizacionVariables, this.Name, exception);
            }
        }

        /// <summary>
        /// Fuerza el valor de una variable de tipo texto, entero o decimal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuForzarValor_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ListVariables.FocusedItem != null)
                {
                    VariableItem variable = (VariableItem)this.ListVariables.FocusedItem.Tag;
                    string codigo = this.ListVariables.FocusedItem.Text;
                    switch (variable.Tipo)
                    {
                        case EnumTipoDato.Entero:
                        case EnumTipoDato.Texto:
                        case EnumTipoDato.Decimal:
                        case EnumTipoDato.Fecha:
                            this.ForzarValor(variable);
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosControl.MonitorizacionVariables, this.Name, exception);
            }
        }

        /// <summary>
        /// Fuerza el valor de una variable de tipo bool a verdadero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuForzarValorVerdadero_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ListVariables.FocusedItem != null)
                {
                    VariableItem variable = (VariableItem)this.ListVariables.FocusedItem.Tag;
                    if (variable.Tipo == EnumTipoDato.Bit)
                    {
                        string codigo = this.ListVariables.FocusedItem.Text;
                        VariableRuntime.ForzarValor(codigo, true, "Monitorizacion", this.Name);
                    }
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosControl.MonitorizacionVariables, this.Name, exception);
            }
        }

        /// <summary>
        /// Fuerza el valor de una variable de tipo bool a verdadero
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuForzarValorFalso_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ListVariables.FocusedItem != null)
                {
                    VariableItem variable = (VariableItem)this.ListVariables.FocusedItem.Tag;
                    if (variable.Tipo == EnumTipoDato.Bit)
                    {
                        string codigo = this.ListVariables.FocusedItem.Text;
                        VariableRuntime.ForzarValor(codigo, false, "Monitorizacion", this.Name);
                    }
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosControl.MonitorizacionVariables, this.Name, exception);
            }
        }

        /// <summary>
        /// Doble clic sobre una variable fuerza su cambio de valor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListVariables_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (this.ListVariables.FocusedItem != null)
                {
                    VariableItem variable = (VariableItem)this.ListVariables.FocusedItem.Tag;
                    string codigo = this.ListVariables.FocusedItem.Text;
                    object objValor = VariableRuntime.GetValue(codigo);

                    switch (variable.Tipo)
                    {
                        case EnumTipoDato.Flag:
                            VariableRuntime.Dispara(codigo, "Monitorizacion", this.Name);
                            break;
                        case EnumTipoDato.Bit:
                            bool valor = true;
                            if (objValor is bool)
                            {
                                valor = !(bool)objValor;
                            }
                            VariableRuntime.ForzarValor(codigo, valor, "Monitorizacion", this.Name);
                            break;
                        case EnumTipoDato.Entero:
                        case EnumTipoDato.Decimal:
                        case EnumTipoDato.Texto:
                        case EnumTipoDato.Fecha:
                            this.ForzarValor(variable);
                            break;
                    }
                    this.ListVariables.SelectedItems.Clear();
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosControl.MonitorizacionVariables, this.Name, exception);
            }
        }

        /// <summary>
        /// Clic en la lista del botón desplegable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraToolbarsManager_ToolClick(object sender, ToolClickEventArgs e)
        {
            try
            {
                switch (e.Tool.Key)
                {
                    case "KeyIconosGrandes":
                        this.ListVariables.View = View.LargeIcon;
                        break;
                    case "KeyIconosPequeños":
                        this.ListVariables.View = View.SmallIcon;
                        break;
                    case "KeyLista":
                        this.ListVariables.View = View.List;
                        break;
                    case "KeyDetalles":
                        this.ListVariables.View = View.Details;
                        break;
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosControl.MonitorizacionVariables, this.Name, exception);
            }
        }

        /// <summary>
        /// Clic en un escenario
        /// </summary>
        private void EscenarioClick(object sender, ToolClickEventArgs e)
        {
            try
            {
                string codNuevoEscenario = e.Tool.Key;
                codNuevoEscenario = codNuevoEscenario == "Todos" ? string.Empty : codNuevoEscenario;

                if (this.CodEscenarioActual != codNuevoEscenario)
                {
                    this.CodEscenarioActual = codNuevoEscenario;

                    // Recargamos los items
                    this.DestruirItems();
                    this.CrearItems();
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosControl.MonitorizacionVariables, this.Name, exception);
            }
        }

        /// <summary>
        /// Timer de refresco del estado actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerRefresco_Tick(object sender, EventArgs e)
        {
            try
            {
                this.RefrescarTodasVariables();
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosControl.MonitorizacionVariables, this.Name, exception);
            }
        }

        /// <summary>
        /// Se lanza cuando el usuario imputa el texto de una variable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextoImputadoPorUsuario(object sender, EventArgsInputText e)
        {
            try
            {
                VariableRuntime.ForzarValor(e.Codigo, e.TextoImputado, "Monitorizacion", this.Name);
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosControl.MonitorizacionVariables, this.Name, exception);
            }
        }

        /// <summary>
        /// Carga una fotografía de disco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuCargarFoto_Click(object sender, EventArgs e)
        {
            try
            {

                if (this.ListVariables.FocusedItem != null)
                {
                    VariableItem variable = (VariableItem)this.ListVariables.FocusedItem.Tag;
                    if (variable.Tipo == EnumTipoDato.Imagen)
                    {
                        string codigo = this.ListVariables.FocusedItem.Text;
                        string rutaArchivo = RutaParametrizable.AppFolder;
                        bool archivoSeleccionadoOK = App.FormularioSeleccionArchivo(this.openFileDialog, ref rutaArchivo);
                        if (archivoSeleccionadoOK)
                        {
                            OrbitaImage imagen;
                            object objImagen = VariableRuntime.GetValue(codigo);

                            // Por defecto se crea una imagen de tipo BitmapImage
                            if ((objImagen == null) || !(objImagen is OrbitaImage))
                            {
                                imagen = new BitmapImage();
                            }
                            else
                            {
                                imagen = ((OrbitaImage)objImagen).Nueva();
                            }

                            imagen.Cargar(rutaArchivo);
                            VariableRuntime.ForzarValor(codigo, imagen, "Monitorizacion", this.Name);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LogsRuntime.Error(ModulosControl.MonitorizacionVariables, this.Name, exception);
            }
        }

        /// <summary>
        /// Guarda una fotografía a disco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuGuardarFoto_Click(object sender, EventArgs e)
        {
        }
        #endregion
    }
}