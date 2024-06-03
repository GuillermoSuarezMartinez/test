using System;
using System.Collections.Generic;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Orbita.Comunicaciones;
using Orbita.Utiles;

namespace Orbita.Controles.Comunicaciones
{    
    public partial class OClienteComs : UserControl
    {
        #region Delegados
        /// <summary>
        /// Delegado para mostar los datos en el formulario
        /// </summary>
        /// <param name="Elemento"></param>
        internal delegate void Delegado(string Elemento);
        /// <summary>
        /// Delegado para el cambio de estado
        /// </summary>
        /// <param name="Elemento"></param>
        internal delegate void DelegadoCambioEstado(OEstadoComms estado);
        /// <summary>
        /// Delegado para el cambio de las ES
        /// </summary>
        /// <param name="estado"></param>
        internal delegate void DelegadoES(OEventArgs estado);
        #endregion

        #region Variables
        /// <summary>
        /// Puerto de comunicación de remoting
        /// </summary>
        private int _remotingPuerto = 1852;
        /// <summary>
        /// Servidor de comunicaciones.
        /// </summary>
        IOCommRemoting _servidor;
        /// <summary>
        /// Identificador de dispositivo
        /// </summary>
        int _idDispositivo = 4;
        /// <summary>
        /// Servidor remoting
        /// </summary>
        string _servidorRemoting = "localhost";
        #endregion

        #region Constructores

        public OClienteComs()
        {
            InitializeComponent();
        }

        #endregion        
    }
}
