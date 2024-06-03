using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Orbita.Trazabilidad;

namespace Orbita.Comunicaciones
{
    /// <summary>
    /// Protocolo pra comunicar con WinCC. Es necesario que este dispositivo esté en la misma máquina que WinCC.
    /// </summary>
    public class OWinCC:ODispositivo
    {
        #region Atributo(s)
        /// <summary>
        /// Objeto para comunicar con WinCC
        /// </summary>
        private OWinCCDataManager oDataManager = null;
        /// <summary>
        /// Tipo de variable para escribir
        /// </summary>
        private Type m_CurVarType;
        /// <summary>
        /// Logger del sistema
        /// </summary>
        private ILogger logger;

        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de clase
        /// </summary>
        /// <param name="logger">Log de la clase</param>
        public OWinCC(ILogger logger)
        {
            this.logger = logger;
            oDataManager = new OWinCCDataManager();
            oDataManager.Connect();
        }

        /// <summary>
        /// Destruye el objeto
        /// </summary>
        ~OWinCC()
        {
            // Do not re-create Dispose clean-up code here. 
            // Calling Dispose(false) is optimal in terms of 
            // readability and maintainability.
            Dispose(false);
        }
        #endregion

        #region Metodo(s) publicos

        /// <summary>
        /// Inicia el dispositivo
        /// </summary>
        public override void Iniciar()
        {

        }
        /// <summary>
        /// limpia los objetos en memoria
        /// </summary>
        /// <param name="disposing"></param>
        public override void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called. 
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if (disposing)
                {
                    //// Dispose managed resources.
                    //component.Dispose();
                }

                // Call the appropriate methods to clean up 
                // unmanaged resources here. 
                // If disposing is false, 
                // only the following code is executed.
                //CloseHandle(handle);
                //handle = IntPtr.Zero;

                // Note disposing has been done.
                oDataManager = null;
                disposed = true;
            }
        }
        /// <summary>
        /// Leer las variables de WinCC. El método de lectura es variable a variable.
        /// </summary>
        /// <param name="variables">nombre de las variables</param>
        /// <param name="demanda">indica que la llamada al dispositivo se hace de forma inmediata</param>
        /// <returns></returns>
        public override object[] Leer(string[] variables, bool demanda)
        {
               object[] retorno = new object[variables.Length];
               demanda = true;

               if (demanda)
               {
                   for (int i = 0; i < retorno.Length; i++)
                   {
                       object valor = null;
                       if (this.Leer(variables[i], out  valor))
                       {
                           retorno[i] = valor;
                       }
                   }
               }
               
               return retorno;
        }
        /// <summary>
        /// Escribe las variables en WinCC. El método de escritura es variable a variable
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="valores"></param>
        /// <returns></returns>
        public override bool Escribir(string[] variables, object[] valores)
        {
            bool retorno = true;

            for (int i = 0; i < variables.Length; i++)
            {
                if (!this.Escribir(variables[i],valores[i]))
                {
                    retorno = false;
                }                
            }

            return retorno;
        }
        #endregion  

        #region Metodo(s) privados
        /// <summary>
        /// Lee la variable de wincc
        /// </summary>
        /// <param name="variable">Variable que se quiere leer</param>
        /// <param name="valor">Valor leído</param>
        /// <returns>Verdadero si se lee y falso si no se lee</returns>
        private bool Leer(string variable, out object valor)
        {
            bool retorno = false;

            valor = null;
            UInt32 calidad = 0;

            try
            {
                retorno = oDataManager.GetVarValue(variable, out valor, out calidad);
            }
            catch (Exception ex)
            {
                this.logger.Fatal("Error al leer: ", ex);
            }

            return retorno;
        }
        /// <summary>
        /// Escribe en la variable de wincc
        /// </summary>
        /// <param name="variable">Variable que se quiere escribir</param>
        /// <param name="valor">Valor que se quiere escribir</param>
        /// <returns>Verdadero si se escribe y falso si no se escribe</returns>
        private bool Escribir(string variable, object valor)
        {
            bool retorno = false;

            try
            {
                m_CurVarType = valor.GetType();
                Object objNewValue = Convert.ChangeType(valor, m_CurVarType);
                retorno = oDataManager.SetVarValue(variable, objNewValue);
            }
            catch (Exception ex)
            {
                this.logger.Fatal("Error al escribir: ", ex);
            }

            return retorno;
        }
        #endregion
    }

    
}
