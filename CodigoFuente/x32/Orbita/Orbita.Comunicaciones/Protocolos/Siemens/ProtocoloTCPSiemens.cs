using System;
using System.Collections.Generic;
using System.Text;

namespace Orbita.Comunicaciones
{
    public class ProtocoloTCPSiemens : IDisposable
    {
        #region Variables
        // Track whether Dispose has been called.
        public bool disposed = false;
        #endregion

        #region Metodos

        /// <summary>
        /// Procesa el mensaje keep alive del PLC
        /// </summary>
        /// <param name="valor">valor recibido por el PLC</param>
        /// <param name="fecha">fecha recibida por el PLC</param>
        /// <returns></returns>
        public virtual bool KeepAliveProcesar(byte[] valor, out byte[] lecturas)
        {
            lecturas = null;
            return false;
        }
        /// <summary>
        /// Prepara el mensaje keep alive de respuesta
        /// </summary>
        /// <returns>mensaje de respuesta</returns>
        public virtual byte[] KeepAliveEnviar()
        {
            return null;
        }
        /// <summary>
        /// Escritura de salidas
        /// </summary>
        /// <param name="salidas"></param>
        /// <returns></returns>
        public virtual bool SalidasProcesar(byte[] valor, byte id)
        {
            return false;
        }
        /// <summary>
        /// Escritura de salidas
        /// </summary>
        /// <param name="salidas"></param>
        /// <returns></returns>
        public virtual byte[] SalidasEnviar(byte salidas, byte idMensaje)
        {
            return null;
        }
        /// <summary>
        /// Destrucción del objeto
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Destrucción del objeto
        /// </summary>
        /// <param name="disposing"></param>
        public virtual void Dispose(bool disposing)
        {
           
        }
        #endregion
    }
}
