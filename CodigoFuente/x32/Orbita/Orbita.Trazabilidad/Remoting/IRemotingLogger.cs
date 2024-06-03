//***********************************************************************
// Assembly         : OrbitaTrazabilidad
// Author           : crodriguez
// Created          : 02-17-2011
//
// Last Modified By : crodriguez
// Last Modified On : 02-21-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
namespace Orbita.Trazabilidad
{
    /// <summary>
    /// IRemotingLogger.
    /// </summary>
    public interface IRemotingLogger
    {
        /// <summary>
        /// Crea una entrada de registro nuevo basado en un elemento de inicio de sesión dado.
        /// </summary>
        /// <param name="mensajes">Entradas de registro.</param>
        /// <remarks></remarks>
        [System.Runtime.Remoting.Messaging.OneWay]
        void Log(ItemLog[] mensajes);
    }
}
