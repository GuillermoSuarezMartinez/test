//***********************************************************************
// Assembly         : Orbita.Comunicaciones
// Author           : crodriguez
// Created          : 03-11-2011
//
// Last Modified By : crodriguez
// Last Modified On : 03-12-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using Orbita.Utiles;

namespace Orbita.Comunicaciones
{
    /// <summary>
    /// Elemento para la generación de eventos por remoting
    /// </summary>
    [Serializable]
    public class OBroadcastEventWrapper : MarshalByRefObject
    {
        #region Evento(s)

        /// <summary>
        /// Evento de cambio de dato.
        /// </summary>
        public event OManejadorEventoComm OrbitaCambioDato;
        /// <summary>
        /// Evento de alarma.
        /// </summary>
        public event OManejadorEventoComm OrbitaAlarma;
        /// <summary>
        /// Evento de comunicaciones.
        /// </summary>
        public event OManejadorEventoComm OrbitaComm;

        #endregion

        #region Método(s) público(s)

        /// <summary>
        /// Evento de cambio de dato.
        /// </summary>
        /// <param name="e">Argumento que puede ser utilizado en el manejador de evento.</param>
        public void OnCambioDato(OEventArgs e)
        {
            OrbitaCambioDato(e);
        }

        /// <summary>
        /// Evento de alarma.
        /// </summary>
        /// <param name="e">Argumento que puede ser utilizado en el manejador de evento.</param>
        public void OnAlarma(OEventArgs e)
        {
            OrbitaAlarma(e);
        }

        /// <summary>
        /// Evento de comunicaciones.
        /// </summary>
        /// <param name="e">Argumento que puede ser utilizado en el manejador de evento.</param>
        public void OnComm(OEventArgs e)
        {
            OrbitaComm(e);
        }

        #endregion
    }
}