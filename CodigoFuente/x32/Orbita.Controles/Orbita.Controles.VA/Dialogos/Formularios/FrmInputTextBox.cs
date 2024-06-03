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

namespace Orbita.Controles.VA
{
    /// <summary>
    /// Formulario de entrada de texto por parte del usuario
    /// </summary>
    public partial class FrmInputTextBox : FrmDialogoBase
    {
        #region Atributo(s)
        /// <summary>
        /// Código identificativo
        /// </summary>
        public string Codigo;
        /// <summary>
        /// Título del formulario
        /// </summary>
        public string Titulo;
        /// <summary>
        /// Explicación de la entrada de texto
        /// </summary>
        public string Explicacion;
        /// <summary>
        /// Texto escrito
        /// </summary>
        public string TextoInputado;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public FrmInputTextBox(string codigo, string titulo, string explicacion, string textoDefecto)
            :base()
        {
            InitializeComponent();

            this.Codigo = codigo;
            this.Titulo = titulo;
            this.Explicacion = explicacion;
            this.TextoInputado = textoDefecto;
        } 
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Muestra la ventana de diálogo
        /// </summary>
        /// <param name="texto"></param>
        public new bool ShowDialog(out string texto)
        {
            DialogResult dialogResult = base.ShowDialog();
            texto = this.TextoInputado;

            return dialogResult == DialogResult.OK;
        }
        #endregion

        #region Método(s) heredado(s)

        /// <summary>
        /// Carga y muestra datos del formulario en modo Modificación. Se cargan todos datos que se muestran en 
        /// el formulario: grids, combos, etc... Cada carga de elementos estará encapsulada en un método
        /// </summary>
        protected override void CargarDatosModoModificacion()
        {
            base.CargarDatosModoModificacion();

            this.Text = this.Titulo;
            this.LblExplicacion.Text = this.Explicacion;
            this.TxtInput.Text = this.TextoInputado;
        }

        /// <summary>
        /// Guarda los datos cuando el formulario está abierto en modo Modificación
        /// </summary>
        /// <returns>True si la operación de guardado de datos ha tenido éxito; false en caso contrario</returns>
        protected override bool GuardarDatosModoModificacion()
        {
            bool resultado = base.GuardarDatosModoModificacion();

            this.TextoInputado = this.TxtInput.Text;

            if (this.OnInputText != null)
            {
                EventArgsInputText e = new EventArgsInputText(this.Codigo, this.TextoInputado);
                this.OnInputText(this, e);
            }

            return resultado;
        }
        #endregion

        #region Evento(s)
        /// <summary>
        /// Se ejecuta cuando se activa el formulario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmInputTextBox_Activated(object sender, EventArgs e)
        {
            this.TxtInput.Focus();
        }

        /// <summary>
        /// Evento que se lanza cuando el usuario ha aceptado la imputació del texto
        /// </summary>
        public EventHandlerInputText OnInputText;
        #endregion
    }

    /// <summary>
    /// Clase estática para la imputación de datos de texto
    /// </summary>
    public static class InputTextBox
    {
        #region Método(s) público(s)
        /// <summary>
        /// Imputación de texto por parte del usuario
        /// </summary>
        /// <param name="titulo">Título de la ventana de diálogo</param>
        /// <param name="explicacion">Explicación del dato a imputar</param>
        /// <param name="texto">Texto por defecto</param>
        /// <returns>Verdadero si el usuario ha aceptado la imputación</returns>
        public static bool ShowModal(string codigo, string titulo, string explicacion, ref string texto)
        {
            FrmInputTextBox frm = new FrmInputTextBox(codigo, titulo, explicacion, texto);
            return frm.ShowDialog(out texto);
        }

        /// <summary>
        /// Imputación de texto por parte del usuario
        /// </summary>
        /// <param name="titulo">Título de la ventana de diálogo</param>
        /// <param name="explicacion">Explicación del dato a imputar</param>
        /// <param name="texto">Texto por defecto</param>
        /// <returns>Verdadero si el usuario ha aceptado la imputación</returns>
        public static void Show(string codigo, string titulo, string explicacion, string texto, EventHandlerInputText onInputText)
        {
            FrmInputTextBox frm = new FrmInputTextBox(codigo, titulo, explicacion, texto);
            frm.OnInputText = onInputText;
            frm.Show();
        }
        #endregion
    }

    #region Definición de delegado(s)
    /// <summary>
    /// Parametros de retorno del evento que indica de la llegada de un mensaje actualizacion
    /// </summary>
    public class EventArgsInputText : EventArgs
    {
        #region Atributo(s)
        /// <summary>
        /// Código identificativo
        /// </summary>
        public string Codigo;

        /// <summary>
        /// Texto imputado por el usuario
        /// </summary>
        public string TextoImputado;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public EventArgsInputText()
        {
            this.TextoImputado = string.Empty;
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public EventArgsInputText(string codigo, string textoImputado)
        {
            this.Codigo = codigo;
            this.TextoImputado = textoImputado;
        }
        #endregion
    }

    /// <summary>
    /// Delegado que indica de la llegada de un nuevo resultado parcial
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void EventHandlerInputText(object sender, EventArgsInputText e);
    #endregion
}
