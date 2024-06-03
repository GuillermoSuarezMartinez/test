//***********************************************************************
// Assembly         : Orbita.Controles.VA
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 16-11-2012
// Description      : Movido al proyecto Orbita.Controles.VA
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

namespace Orbita.Controles.VA
{
    /// <summary>
    /// Formulario de visualización de error
    /// </summary>
    public partial class MensajeError : Form
    {
        #region Atributo(s)
        /// <summary>
        /// Pila de llamadas
        /// </summary>
        private string PilaLlamadas;
        /// <summary>
        /// Mensaje a mostrar
        /// </summary>
        private string Mensaje;
        /// <summary>
        /// Excepción
        /// </summary>
        private string Excepcion;
        /// <summary>
        /// Ensamblado
        /// </summary>
        private string Assembly;
        /// <summary>
        /// Fichero
        /// </summary>
        private string File;
        /// <summary>
        /// Clase
        /// </summary>
        private string ClassName;
        /// <summary>
        /// Método
        /// </summary>
        private string Methode;
        /// <summary>
        /// Linea
        /// </summary>
        private int Line;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public MensajeError()
        {
            InitializeComponent();
        }
        #endregion

        #region Método(s) estático(s)
        /// <summary>
        /// Muestra el mensaje de error
        /// </summary>
        /// <param name="Excepcion">Excepción a mostrar</param>
        public static void MostrarExcepcion(bool dialogo, Exception excepcion)
        {
            using (MensajeError frmExcepcion = new MensajeError())
            {
                frmExcepcion.MostrarExcepcionInterna(dialogo, excepcion);
            }
        }

        /// <summary>
        /// Muestra el mensaje de error
        /// </summary>
        /// <param name="informacion">Informacion a mostrar</param>
        public static void MostrarInfo(bool dialogo, string informacion)
        {
            using (MensajeError frmExcepcion = new MensajeError())
            {
                frmExcepcion.MostrarInfoInterna(dialogo, informacion);
            }
        }

        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Muestra el mensaje de error
        /// </summary>
        /// <param name="Excepcion">Excepción a mostrar</param>
        private void MostrarExcepcionInterna(bool dialogo, Exception excepcion)
        {
            this.Mensaje = excepcion.Message;
            this.Excepcion = excepcion.GetType().FullName;
            App.GetExceptionInfo(excepcion, out this.Assembly, out this.File, out this.ClassName, out this.Methode, out this.Line, out this.PilaLlamadas);

            this.Mostrar(dialogo);
        }

        /// <summary>
        /// Muestra el mensaje de error
        /// </summary>
        /// <param name="Excepcion">Excepción a mostrar</param>
        private void MostrarInfoInterna(bool dialogo, string informacion)
        {
                this.Mensaje = informacion;
                this.Excepcion = "";
                App.GetCallingMethod(6, out this.Assembly, out this.File, out this.ClassName, out this.Methode, out this.Line, out this.PilaLlamadas);

                this.Mostrar(dialogo);
        }

        /// <summary>
        /// Muestra el formulario
        /// </summary>
        /// <param name="dialogo">Indica si se ha de parar la ejecución hasta que se cierre o ha de continuar ejecutandose</param>
        private void Mostrar(bool dialogo)
        {
            try
            {
                if (!ThreadRuntime.EjecucionEnTrheadPrincipal())
                {
                    ThreadRuntime.SincronizarConThreadPrincipal(new DelegadoMostrar(this.Mostrar), new object[] { dialogo });
                    return;
                }

                if (dialogo)
                {
                    this.ShowDialog();
                }
                else
                {
                    this.Show();
                }
            }
            catch { }
        }
        #endregion

        #region Evento(s)
        /// <summary>
        /// Carga del formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MensajeError_Load(System.Object sender, System.EventArgs e)
        {
            this.txtMensaje.Text = this.Mensaje;
            this.txtExcepcion.Text = this.Excepcion;
            this.txtFichero.Text = this.File;
            this.txtClase.Text = this.ClassName;
            this.txtMetodo.Text = this.Methode;
            this.txtEnsamblado.Text = this.Assembly;
            this.txtLinea.Text = this.Line.ToString();
        }

        /// <summary>
        /// Click en el botón de más información
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMasInfo_Click(System.Object sender, System.EventArgs e)
        {
            MessageBox.Show(this.PilaLlamadas, "Pila de llamadas", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Definición de delegado(s)
        /// <summary>
        /// Delegado que muestra la ventana de excepción
        /// </summary>
        /// <param name="dialogo"></param>
        private delegate void DelegadoMostrar(bool dialogo);

        #endregion
    }
}
