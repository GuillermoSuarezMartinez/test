﻿//***********************************************************************
// Assembly         : Orbita.VAComun
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 27-09-2012
// Description      : Si la contraseña introducida no es correcta se visualiza un aviso
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System.Collections.Generic;

namespace Orbita.VAComun
{
    /// <summary>
    /// Formulario de entrada de usuario y contraseña
    /// </summary>
    public partial class FrmUserPsw : FrmDialogoBase
    {
        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public FrmUserPsw()
            : base()
        {
            InitializeComponent();
        }
        #endregion

        #region Método(s) herdados
        /// <summary>
        /// Carga y muestra datos del formulario en modo Modificación. Se cargan todos datos que se muestran en 
        /// el formulario: grids, combos, etc... Cada carga de elementos estará encapsulada en un método
        /// </summary>
        protected override void CargarDatosModoModificacion()
        {
            base.CargarDatosModoModificacion();

            Dictionary<object, string> valores = new Dictionary<object, string>();
            foreach (Usuario usuario in UsuariosRuntime.ListaUsuarios)
            {
                valores.Add(usuario, usuario.Codigo);
            }
            App.CargarCombo(this.ComboUsuario, valores, typeof(Usuario), UsuariosRuntime.UsuarioActual);
        }

        /// <summary>
        /// Guarda los datos cuando el formulario está abierto en modo Modificación
        /// </summary>
        /// <returns>True si la operación de guardado de datos ha tenido éxito; false en caso contrario</returns>
        protected override bool GuardarDatosModoModificacion()
        {
            bool resultado = base.GuardarDatosModoModificacion();

            resultado &= UsuariosRuntime.Registrar(this.ComboUsuario.OrbTexto, this.TxtContraseña.Text);
            if (!resultado)
            {
                this.LblErrorContraseña.Visible = true;
                this.TxtContraseña.Clear();
            }

            return resultado;
        }
        #endregion
    }
}
