//***********************************************************************
// Assembly         : Orbita.VAComun
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 16-11-2012
// Description      : Cambiadas referencias a eventos genéricos de App.cs
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections.Generic;
using System.Data;

namespace Orbita.VAComun
{
    /// <summary>
    /// Proporciona información sobre los usuarios del sistema
    /// </summary>
    public static class OUsuariosManager
    {
        #region Atributo(s)
        /// <summary>
        /// Lista de los usuarios del sistema
        /// </summary>
        public static List<OUsuario> ListaUsuarios;

        /// <summary>
        /// Indica el permiso del usuario registrado
        /// </summary>
        public static OPermisos PermisoActual;

        /// <summary>
        /// Indica usuario registrado
        /// </summary>
        public static OUsuario UsuarioActual;
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Construye los objetos
        /// </summary>
        public static void Constructor()
        {
            // Construyo la lista de usuarios
            ListaUsuarios = new List<OUsuario>();

            // Consulta a la base de datos
            DataTable dt = AppBD.GetUsuarios();
            if (dt.Rows.Count > 0)
            {
                // Cargamos todos los usuarios
                foreach (DataRow dr in dt.Rows)
                {
                    string codUsuario = dr["CodUsuario"].ToString();
                    OUsuario usuario = new OUsuario(codUsuario);
                    ListaUsuarios.Add(usuario);
                }
            }

            // Por defecto se accede con permisos de operador
            PermisoActual = OPermisos.Operador;
            UsuarioActual = null;
        }

        /// <summary>
        /// Destruye los objetos
        /// </summary>
        public static void Destructor()
        {
            // Destruyo la lista de máquinas de estados
            ListaUsuarios = null;
        }

        /// <summary>
        /// Carga las propiedades de la base de datos
        /// </summary>
        public static void Inicializar()
        {
        }

        /// <summary>
        /// Finaliza la ejecución
        /// </summary>
        public static void Finalizar()
        {
        }

        /// <summary>
        /// Registra a un usuario en el sistema
        /// </summary>
        /// <param name="codUsuario">Codigo identificativo del usuario</param>
        /// <param name="contraseña">Contraseña del usuario</param>
        /// <returns>Verdadero si el usuario se ha registrado con éxito</returns>
        public static bool Registrar(string codUsuario, string contraseña)
        {
            bool resultado = false;

            OUsuario usuario = ListaUsuarios.Find(delegate(OUsuario u) { return string.Equals(u.Codigo, codUsuario, StringComparison.InvariantCultureIgnoreCase) && (u.Contraseña == contraseña); });

            if (usuario != null)
            {
                resultado = true;
                PermisoActual = usuario.Permisos;
                UsuarioActual = usuario;
                if (OnRegistroUsuario != null)
                {
                    OnRegistroUsuario();
                }
            }

            return resultado;
        }

        /// <summary>
        /// Crea una suscripción para recibir un evento cada vez que se registra un usuario
        /// </summary>
        /// <param name="onRegistroUsuario"></param>
        public static void SuscribirAlRegistrarUsuario(OSimpleMethod onRegistroUsuario)
        {
            OnRegistroUsuario += onRegistroUsuario;
        }

        /// <summary>
        /// Elimina la suscripción para recibir un evento cada vez que se registra un usuario
        /// </summary>
        /// <param name="onRegistroUsuario"></param>
        public static void DesSuscribirAlRegistrarUsuario(OSimpleMethod onRegistroUsuario)
        {
            OnRegistroUsuario -= onRegistroUsuario;
        }
        #endregion

        #region Evento(s)
        /// <summary>
        /// Evento de registro de usuario
        /// </summary>
        private static OSimpleMethod OnRegistroUsuario;
        #endregion
    }

    /// <summary>
    /// Usuario del sistema
    /// </summary>
    public class OUsuario
    {
        #region Atributo(s)
        /// <summary>
        /// Identificador del usuario
        /// </summary>
        public string Codigo;

        /// <summary>
        /// Contraseña del usuario
        /// </summary>
        public string Contraseña;

        /// <summary>
        /// Permisos del usuario
        /// </summary>
        public OPermisos Permisos;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OUsuario(string codigo)
        {
            this.Codigo = codigo;

            // Cargamos valores de la base de datos
            DataTable dtFuncionVision = AppBD.GetUsuario(this.Codigo);
            if (dtFuncionVision.Rows.Count == 1)
            {
                this.Contraseña = dtFuncionVision.Rows[0]["Contraseña"].ToString();
                this.Permisos = (OPermisos)App.EnumParse(typeof(OPermisos), dtFuncionVision.Rows[0]["Permiso"].ToString(), OPermisos.Operador);
            }            
        }
        #endregion
    }

    /// <summary>
    /// Lista de permisos
    /// </summary>
    public enum OPermisos
    {
        /// <summary>
        /// Permisos de operador
        /// </summary>
        Operador = 0,
        /// <summary>
        /// Permisos de administrador
        /// </summary>
        Administrador = 1
    }
}
