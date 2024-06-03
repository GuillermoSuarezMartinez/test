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
using Infragistics.Win.UltraWinDataSource;
using Orbita.VAComun;
using Orbita.VAControl;

namespace Orbita.Controles.VA
{
    /// <summary>
    /// Control para la monitorización en el tiempo de variables de tipo bool
    /// </summary>
    public partial class OrbitaGraficaVariableBool : OrbitaGraficaVariableBase
    {
        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OrbitaGraficaVariableBool()
        {
            InitializeComponent();
        } 
        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Método que inicializa las propiedades internas
        /// </summary>
        public override void Inicializar(string codigo)
        {
            base.Inicializar(codigo);

            this.ChartVariable.TitleTop.Text = this.Descripcion;
        }

        /// <summary>
        /// Inicia la monitorización en el tiempo de la variable
        /// </summary>
        public override void IniciarEjecucion()
        {
            base.IniciarEjecucion();

            // Se cargan los valores acumulados de la varible
            int numeroMonitorizaciones = Convert.ToInt32(Math.Round(OVariablesManager.TiempoPermanenciaTrazasEnMemoria.TotalMilliseconds / OVariablesManager.CadenciaMonitorizacion.TotalMilliseconds));
            DateTime presente = DateTime.Now;
            DateTime ahora = presente - OVariablesManager.TiempoPermanenciaTrazasEnMemoria;
            for (int i = 0; i < numeroMonitorizaciones; i++)
            {
                ahora += OVariablesManager.CadenciaMonitorizacion;
                UltraDataRow dr = this.varDataSource.Rows.Add();
                dr["Valor"] = 0;
                dr["Fecha"] = ahora;
            }
        }

        /// <summary>
        /// Para la monitorización en el tiempo de la variable
        /// </summary>
        public override void PararEjecucion()
        {
            base.PararEjecucion();
        }

        #endregion

        #region Eventos

        /// <summary>
        /// Evento de refresco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void TimerRefresco_Tick(object sender, EventArgs e)
        {
            try
            {
                base.TimerRefresco_Tick(sender, e);


                this.varDataSource.SuspendBindingNotifications();

                UltraDataRow dr = this.varDataSource.Rows.Add();
                dr["Valor"] = Convert.ToInt32(UltimoValor);
                dr["Fecha"] = DateTime.Now;

                this.varDataSource.Rows.RemoveAt(0);


                this.varDataSource.ResumeBindingNotifications();
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosControl.MonitorizacionVariables, this.Name, exception);
            }
        } 
        #endregion
    }
}
