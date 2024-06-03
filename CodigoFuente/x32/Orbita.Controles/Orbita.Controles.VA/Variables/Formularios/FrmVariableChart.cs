//***********************************************************************
// Assembly         : Orbita.Controles.VA
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 16-11-2012
// Description      : Movido al proyecto Orbita.Controles.VA
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Windows.Forms;
using Orbita.VAComun;
using Orbita.VAControl;

namespace Orbita.Controles.VA
{
    public partial class FrmVariableChart : FrmBase
    {
        #region Atributo(s)
        /// <summary>
        /// Control de monitorización de la variable
        /// </summary>
        private OrbitaGraficaVariableBase CtrlVariableChart;

        /// <summary>
        /// Código de la variable
        /// </summary>
        private string Codigo;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codigo"></param>
        public FrmVariableChart(string codigo)
        {
            InitializeComponent();

            this.Codigo = codigo;
            this.Text = "Monitorización termporal de variables [" + codigo + "]";

            OVariable variable = OVariablesManager.GetVariable(codigo);
            if (variable != null)
            {
                switch (variable.Tipo)
                {
                    case OEnumTipoDato.SinDefinir:
                    default:
                        throw new Exception("El tipo de variable no está definida");
                        break;
                    case OEnumTipoDato.Bit:
                        this.CtrlVariableChart = new OrbitaGraficaVariableBool();
                        this.CtrlVariableChart.Parent = this.pnlPanelPrincipalPadre;
                        this.CtrlVariableChart.Dock = DockStyle.Fill;
                        break;
                    case OEnumTipoDato.Entero:
                        throw new Exception("No se ha implementado todavía el formulario de monitorización temporal para el tipo de variable Entero");
                        break;
                    case OEnumTipoDato.Texto:
                        throw new Exception("No se ha implementado todavía el formulario de monitorización temporal para el tipo de variable Texto");
                        break;
                    case OEnumTipoDato.Decimal:
                        throw new Exception("No se ha implementado todavía el formulario de monitorización temporal para el tipo de variable decimal");
                        break;
                    case OEnumTipoDato.Fecha:
                        throw new Exception("No se ha implementado todavía el formulario de monitorización temporal para el tipo de variable fecha");
                        break;
                    case OEnumTipoDato.Imagen:
                        throw new Exception("No se ha implementado todavía el formulario de monitorización temporal para el tipo de variable Imagen");
                        break;
                    case OEnumTipoDato.Grafico:
                        throw new Exception("No se ha implementado todavía el formulario de monitorización temporal para el tipo de variable gráfica");
                        break;
                    case OEnumTipoDato.Flag:
                        throw new Exception("No se ha implementado todavía el formulario de monitorización temporal para el tipo de variable Flag");
                        break;
                }
            }
            else
            {
                this.Close();
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

            this.CtrlVariableChart.Inicializar(this.Codigo);
            this.CtrlVariableChart.IniciarEjecucion();
        }
        #endregion
    }
}
