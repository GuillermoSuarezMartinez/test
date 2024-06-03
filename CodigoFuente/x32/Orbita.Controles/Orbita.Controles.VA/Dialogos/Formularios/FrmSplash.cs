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
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Orbita.Controles;
using Orbita.VAComun;

namespace Orbita.Controles.VA
{
    /// <summary>
    /// Formulario que se muestra mientras se abre la aplicación
    /// </summary>
    public partial class FrmSplash : OrbitaDialog
    {
        #region Atributo(s)
        /// <summary>
        /// Contendrá el mensaje a actualizar en el formulario
        /// </summary>
        public string Mensaje = string.Empty;
        /// <summary>
        /// Define si el formulario se ha de cerrar
        /// </summary>
        public bool Cerrar = false;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="milisegundos">Tiempo de refresco en milisegundos</param>
        /// <param name="inicial">Indica si se trata de la visualización inicial o de una recarga de parámetros</param>
        public FrmSplash(int milisegundos, bool inicial)
        {
            InitializeComponent();

            TimerRefresco.Interval = milisegundos;	// pasamos de segundos a milisegundos

            if (!TimerRefresco.Enabled)
                TimerRefresco.Enabled = true;	// Activamos el Timer si no esta Enabled (activado)
            // Si no se trata del Splash del inicio de la aplicación solo mostramos los mensajes
            if (!inicial)
            {
                this.pbLogo.Visible = false;
                this.lblIdioma.Visible = false;
                this.lblMaquina.Visible = false;
                this.lblUsuario.Visible = false;
                this.lblVersion.Visible = false;
                this.lblMensaje.Dock = DockStyle.Fill;
                this.Size = new Size(232, 24);
            }

            //this.BringToFront();
            //this.SendToBack();
            //this.TopLevel = true;
            //this.TopMost = false;
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Modifica los labels del formulario
        /// </summary>
        private void RealizarVisualizaciones()
        {
            if (this.Mensaje != string.Empty)
            {
                this.lblMensaje.Text = this.Mensaje;
                this.Mensaje = string.Empty;
            }
        }
        #endregion

        #region Evento(s)
        /// <summary>
        /// Evento de carga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSplash_Load(object sender, System.EventArgs e)
        {
            this.lblUsuario.Text = "Nombre del equipo: " + Environment.MachineName.ToString();
            this.lblMaquina.Text = "Nombre del usuario: " + Environment.UserName.ToString();
            this.lblIdioma.Text = "Idioma : " + Application.CurrentCulture.ToString();
            this.lblVersion.Text = "Versión " + App.ObtenerVersion(System.Reflection.Assembly.GetExecutingAssembly());
            this.lblProducto.Text = "Producto: " + Application.ProductName;
            this.lblCompañia.Text = "Compañía: " + Application.CompanyName;
        }

        /// <summary>
        /// Evento del Timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerRefresco_Tick(object sender, System.EventArgs e)
        {
            TimerRefresco.Stop();				// Se para el timer.

            SplashRuntime.MostrandoSplash = true;

            // Se realiza la comprobación de la visualización
            this.RealizarVisualizaciones();

            Application.DoEvents();
            if (!this.Cerrar)
            {
                TimerRefresco.Start();
            }
            else
            {
                SplashRuntime.MostrandoSplash = false;
                this.Close();	// Si se ha terminado cerramos.
            }

        }

        /// <summary>
        /// Evento que se ejecutan antes de cerrar el formulario. Se cancela el cierre por el usuario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSplash_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (SplashRuntime.MostrandoSplash)
            {
                e.Cancel = true;
            }
        }
        #endregion
    }

    /// <summary>
    /// Clase estática para el acceso al fomrulario de splash
    /// </summary>
    public static class SplashRuntime
    {
        #region Atributo(s)
        /// <summary>
        /// Formulario splash
        /// </summary>
        private static FrmSplash Splash;

        /// <summary>
        /// Indica que el formulario de splash está mostrandose
        /// </summary>
        public static bool MostrandoSplash = false;

        /// <summary>
        /// Indica si el splash se ha de mostrar de forma completa o reducida
        /// </summary>
        private static bool Inicial = true;
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Inicio del splash en otro hilo de ejecución
        /// </summary>
        private static void IniciarSplash()
        {
            try
            {
                using (Splash = new FrmSplash(50, Inicial))
                {
                    Splash.ShowDialog();
                    Inicial = false;
                }
            }
            catch
            {
            }
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Constructor del splash
        /// </summary>
        public static void Contructor()
        {
            if (App.IsWinForms())
            {
                // Deshabilito el formulario principal
                Application.UseWaitCursor = true;
                App.FormularioPrincipalMDI.UseWaitCursor = true;
                App.FormularioPrincipalMDI.Enabled = false;

                // Mostramos el formulario Splash en un hilo
                Thread tSplash = new Thread(new ThreadStart(IniciarSplash));
                tSplash.Start();

                // Espero el incio
                App.Espera(ref MostrandoSplash, true, 10000);
            }
        }

        /// <summary>
        /// Destrucción del splash
        /// </summary>
        public static void Destructor()
        {
            if (App.IsWinForms() && MostrandoSplash)
            {
                Splash.Cerrar = true;

                // Espero la finalización
                App.Espera(ref MostrandoSplash, false, 10000);

                // Habilito el formulario principal
                Application.UseWaitCursor = false;
                App.FormularioPrincipalMDI.UseWaitCursor = false;
                App.FormularioPrincipalMDI.Enabled = true;
                App.FormularioPrincipalMDI.BringToFront();
            }
        }

        /// <summary>
        /// Visualización de un mensaje en el splash
        /// </summary>
        /// <param name="mensaje"></param>
        public static void Mensaje(string mensaje)
        {
            if (App.IsWinForms() && MostrandoSplash)
            {
                Splash.Mensaje = mensaje;
            }
        }
        #endregion
    }

}
