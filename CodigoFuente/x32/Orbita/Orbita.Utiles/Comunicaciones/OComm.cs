//***********************************************************************
// Assembly         : OrbitaUtiles
// Author           : crodriguez
// Created          : 10-03-2011
//
// Last Modified By : crodriguez
// Last Modified On : 10-03-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System.Net;
using System.Net.NetworkInformation;
namespace Orbita.Utiles
{
    /// <summary>
    /// Gestión de comunicaciones
    /// </summary>
    public static class OComm
    {
        #region Método(s) público(s)
        /// <summary>
        /// Obtener la disponibilidad de un puerto.
        /// </summary>
        /// <param name="puerto">Número de puerto.</param>
        /// <returns>Si es no disponible.</returns>
        public static bool GetDisponibilidadPuerto(int puerto)
        {
            bool disponible = true;

            if (puerto > 0)
            {
                // Evaluar el sistema actual de conexiones Tcp.
                // Mirar a través de la lista si el puerto que se
                // desea utilizar en nuestro TcpClient está ocupado.
                IPGlobalProperties ipPropiedadesGlobales = IPGlobalProperties.GetIPGlobalProperties();
                IPEndPoint[] tcpConInfoArray = ipPropiedadesGlobales.GetActiveTcpListeners();

                foreach (IPEndPoint endpoint in tcpConInfoArray)
                {
                    if (endpoint.Port == puerto)
                    {
                        disponible = false;
                        break;
                    }
                }
            }

            return disponible;
        }
        #endregion
    }
}
