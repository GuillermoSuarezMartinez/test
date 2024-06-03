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
using System.Collections;
using Orbita.Utiles;


namespace Orbita.Comunicaciones
{
    /// <summary>
    /// Interfaz para la generación de comunicaciones remotas
    /// </summary>
    public interface IOCommRemoting
    {
        #region Evento(s)

        /// <summary>
        /// Evento de cambio de dato.
        /// </summary>
        event OManejadorEventoComm OrbitaCambioDato;
        /// <summary>
        /// Evento de alarma.
        /// </summary>
        event OManejadorEventoComm OrbitaAlarma;
        /// <summary>
        /// Evento de comunicaciones.
        /// </summary>
        event OManejadorEventoComm OrbitaComm;

        #endregion

        #region Método(s)

        /// <summary>
        /// Método de conexión entre procesos.
        /// </summary>
        /// <param name="ip">Dirección Ip de conexión.</param>
        /// <param name="estado">Estado de la máquina; conectado, desconectado.</param>
        void OrbitaConectar(string ip, bool estado);
        /// <summary>
        /// Método de escritura de valores.
        /// </summary>
        /// <param name="dispositivo">Identificador de dispositivo de escritura.</param>
        /// <param name="variables">Colección de variables.</param>
        /// <param name="valores">Colección de valores.</param>
        /// <returns>Estado correcto o incorrecto de escritura.</returns>
        bool OrbitaEscribir(int dispositivo, string[] variables, object[] valores);
        /// <summary>
        /// Método de lectura de variables.
        /// </summary>
        /// <param name="dispositivo">Identificador de dispositivo de lectura.</param>
        /// <param name="variables">Colección de variables.</param>
        /// <param name="demanda">Lectura contra dispositivo</param>
        /// <returns>Colección de resultados.</returns>
        object[] OrbitaLeer(int dispositivo, string[] variables,bool demanda);  
        /// <summary>
        /// Obtener la colección de datos, lecturas y alarmas.
        /// </summary>
        /// <returns>Colección de datos, lecturas y alarmas.</returns>
        OHashtable OrbitaGetDatos(int dispositivo);
        /// <summary>
        /// Obtener la colección de lecturas.
        /// </summary>
        /// <returns>Colección de lecturas.</returns>
        OHashtable OrbitaGetLecturas(int dispositivo);
        /// <summary>
        /// Obtener la colección de alarmas.
        /// </summary>
        /// <returns>Colección de alarmas.</returns>
        OHashtable OrbitaGetAlarmas(int dispositivo);
        /// <summary>
        /// Obtener la colección de alarmas.
        /// </summary>
        /// <returns>Colección de alarmas.</returns>
        ArrayList OrbitaGetAlarmasActivas(int dispositivo);
        #endregion
    }
}