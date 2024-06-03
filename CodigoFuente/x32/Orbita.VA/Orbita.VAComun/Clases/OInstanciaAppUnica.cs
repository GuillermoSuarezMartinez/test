using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Orbita.Utiles;
using System.Threading;
using System.IO;

namespace Orbita.VAComun
{
    public static class OInstanciaUnicaAplicacion
    {
        #region Método(s) Componentes
        /// <summary>
        /// Mutex utilizado por el JustOne
        /// </summary>
        private static Mutex MutexJustOne;

        /// <summary>
        /// Funcion que nos indica si ya se esta iniciando una instancia del programa
        /// </summary>
        /// <returns>true si ya esta iniciada y false si no es asi</returns>
        public static bool Execute()
        {
            bool primeraInstancia;
            string exeName = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            MutexJustOne = new Mutex(false, exeName, out primeraInstancia);
            if (!primeraInstancia)
            {
                if (Environment.UserInteractive)
                {
                    OMensajes.MostrarError("La aplicación ya esta en ejecución");
                }
                MutexJustOne.Close();
                MutexJustOne = null;
                return false;
            }
            return true;
        }
        #endregion
    }
}