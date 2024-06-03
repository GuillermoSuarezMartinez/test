//***********************************************************************
// Assembly         : OrbitaUtiles
// Author           : crodriguez
// Created          : 03-03-2011
//
// Last Modified By : crodriguez
// Last Modified On : 03-03-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Globalization;
using System.Runtime.InteropServices;   // DllImport.
using System.Security.Principal;        // WindowsImpersonationContext.
namespace Orbita.Utiles
{
    /// <summary>
    /// Clase OSeguridad.
    /// </summary>
    public class OSeguridad : IDisposable
    {
        #region Atributo(s)
        /// <summary>
        /// Nombre del servidor.
        /// </summary>
        string _servidor;
        /// <summary>
        /// Usuario del servidor.
        /// </summary>
        string _usuario;
        /// <summary>
        /// Contraseña del servidor.
        /// </summary>
        string _password;
        /// <summary>
        /// Contexto de impersonalización.
        /// </summary>
        WindowsImpersonationContext _impersonalizacionWindows;
        /// <summary>
        /// Identidad Windows.
        /// </summary>
        WindowsIdentity _identidadWindows;
        /// <summary>
        /// Métodos externos de login.
        /// </summary>
        /// <param name="lpszUsuario">Usuario.</param>
        /// <param name="lpszDominio">Dominio.</param>
        /// <param name="lpszPassword">Contraseña.</param>
        /// <param name="dwLogonType">Tipo de login.</param>
        /// <param name="dwLogonProvider">Proveedor.</param>
        /// <param name="phToken"></param>
        /// <returns></returns>
        [DllImport("advapi32.DLL", SetLastError = true)]
        static extern bool LogonUser(string lpszUsuario, string lpszDominio, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Inicializar una nueva instancia de la clase OSeguridad.
        /// </summary>
        public OSeguridad()
        {
            this._servidor = string.Empty;
            this._usuario = string.Empty;
            this._password = string.Empty;
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase OSeguridad.
        /// </summary>
        /// <param name="servidor">Nombre del servidor.</param>
        /// <param name="usuario">Usuario de autenticación.</param>
        /// <param name="password">Password de autenticación.</param>
        public OSeguridad(string servidor, string usuario, string password)
        {
            this._servidor = string.Format(CultureInfo.CurrentCulture, @"\\{0}", servidor);
            this._usuario = usuario;
            this._password = password;
        }
        #endregion

        #region Destructor(es)
        /// <summary>
        /// Indica si ya se llamo al método Dispose. (default = false)
        /// </summary>
        bool disposed = false;
        /// <summary>
        /// Implementa IDisposable.
        /// No  hacer  este  método  virtual.
        /// Una clase derivada no debería ser
        /// capaz de  reemplazar este método.
        /// </summary>
        public void Dispose()
        {
            // Llamo al método que  contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
            // Este objeto será limpiado por el método Dispose.
            // Llama al método del recolector de basura, GC.SuppressFinalize.
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Método  sobrecargado de  Dispose que será  el que
        /// libera los recursos. Controla que solo se ejecute
        /// dicha lógica una  vez y evita que el GC tenga que
        /// llamar al destructor de clase.
        /// </summary>
        /// <param name="disposing">Indica si llama al método Dispose.</param>
        protected virtual void Dispose(bool disposing)
        {
            // Preguntar si Dispose ya fue llamado.
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Llamar a dispose de todos los recursos manejados.
                    this._impersonalizacionWindows.Dispose();
                    this._identidadWindows.Dispose();
                }

                // Finalizar correctamente los recursos no manejados.
                this._servidor = null;
                this._usuario = null;
                this._password = null;

                // Marcar como desechada ó desechandose,
                // de forma que no se puede ejecutar el
                // código dos veces.
                disposed = true;
            }
        }
        /// <summary>
        /// Destructor(es) de clase.
        /// En caso de que se nos olvide “desechar” la clase,
        /// el GC llamará al destructor, que tambén ejecuta 
        /// la lógica anterior para liberar los recursos.
        /// </summary>
        ~OSeguridad()
        {
            // Llamar a Dispose(false) es óptimo en terminos
            // de legibilidad y mantenimiento.
            Dispose(false);
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Establecer la apertura de la impersonalización.
        /// </summary>
        /// <returns></returns>
        public bool Abrir()
        {
            bool resultado = true;
            try
            {
                IntPtr token = IntPtr.Zero;
                LogonUser(this._usuario, this._servidor, this._password, 9, 0, ref token);
                this._identidadWindows = new WindowsIdentity(token);
                this._impersonalizacionWindows = this._identidadWindows.Impersonate();
            }
            catch { resultado = false; }

            return resultado;
        }
        /// <summary>
        /// Cerrar la impersonalización.
        /// </summary>
        public void Cerrar()
        {
            if (this != null)
            {
                if (this._impersonalizacionWindows != null)
                {
                    this._impersonalizacionWindows.Undo();
                }
            }
        }
        /// <summary>
        /// Impersonalizar el acceso al host.
        /// </summary>
        /// <returns></returns>
        public OSeguridad Impersonalizar()
        {
            if (this != null)
            {
                if (this._identidadWindows != null)
                {
                    if (this._identidadWindows.IsAuthenticated)
                    {
                        this._impersonalizacionWindows = this._identidadWindows.Impersonate();
                    }
                }
            }
            return this;
        }
        #endregion
    }
}
