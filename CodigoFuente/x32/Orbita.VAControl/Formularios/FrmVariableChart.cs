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
using System.Windows.Forms;
using Orbita.VAComun;

namespace Orbita.VAControl
{
    public partial class FrmVariableChart : FrmBase
    {
        #region Atributo(s)
        /// <summary>
        /// Control de monitorización de la variable
        /// </summary>
        private CtrlVariableChart CtrlVariableChart;

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

            VariableItem variable = VariableRuntime.GetVariable(codigo);
            if (variable != null)
            {
                switch (variable.Tipo)
                {
                    case EnumTipoDato.SinDefinir:
                    default:
                        throw new Exception("El tipo de variable no está definida");
                        break;
                    case EnumTipoDato.Bit:
                        this.CtrlVariableChart = new CtrlVariableChartBool();
                        this.CtrlVariableChart.Parent = this.pnlPanelPrincipalPadre;
                        this.CtrlVariableChart.Dock = DockStyle.Fill;
                        break;
                    case EnumTipoDato.Entero:
                        throw new Exception("No se ha implementado todavía el formulario de monitorización temporal para el tipo de variable Entero");
                        break;
                    case EnumTipoDato.Texto:
                        throw new Exception("No se ha implementado todavía el formulario de monitorización temporal para el tipo de variable Texto");
                        break;
                    case EnumTipoDato.Decimal:
                        throw new Exception("No se ha implementado todavía el formulario de monitorización temporal para el tipo de variable decimal");
                        break;
                    case EnumTipoDato.Fecha:
                        throw new Exception("No se ha implementado todavía el formulario de monitorización temporal para el tipo de variable fecha");
                        break;
                    case EnumTipoDato.Imagen:
                        throw new Exception("No se ha implementado todavía el formulario de monitorización temporal para el tipo de variable Imagen");
                        break;
                    case EnumTipoDato.Grafico:
                        throw new Exception("No se ha implementado todavía el formulario de monitorización temporal para el tipo de variable gráfica");
                        break;
                    case EnumTipoDato.Flag:
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
