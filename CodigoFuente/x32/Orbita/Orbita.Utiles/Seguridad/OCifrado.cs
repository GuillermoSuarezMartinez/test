namespace Orbita.Utiles
{
    /// <summary>
    /// Clase estática para encriptar y desencriptar textos con la clave y el vector de incio de la librería.
    /// </summary>
    public static class OCifrado
    {
        #region Método(s) estático(s) público(s) encriptación
        /// <summary>
        /// Clave de la librería para encriptar y desencriptar.
        /// Está puesto como método porque si se pone como atributo o propiedad, a pesar de estar ofuscado, se desemambla y se obtiene su valor.
        /// Además, si se devuelve directamente el string, también se desensambla y se obtiene su valor
        /// ¡No se debe modificar nunca!
        /// </summary>
        public static string ObtenerContraseñaLibreria()
        {
            string s = "46208#O%r(b-i&t*a=I?n?g!e{n¿i¡e]r}i[a93175"; //Puede ser cualquier cadena. No se debe modificar nunca!
            return s;
        }
        /// <summary>
        /// Vector de inicio de la librería para encriptar y desencriptar.
        /// Está puesto como método porque si se pone como atributo o propiedad, a pesar de estar ofuscado, se desemambla y se obtiene su valor.
        /// Además, si se devuelve directamente el string, también se desensambla y se obtiene su valor
        /// ¡No se debe modificar nunca!
        /// </summary>
        public static string ObtenerVectorInicioLibreria()
        {
            string s = "5@O,r:b72_i,t}a3"; // Debe ser de 16 bytes (carácteres ASCII). No se debe modificar nunca!
            return s;
        }
        /// <summary>
        /// Encripta un texto con la contraseña y el vector de incio de la librería
        /// </summary>
        /// <param name="textoPlano">Texto a encriptar</param>
        /// <returns>Texto encriptado</returns>
        public static string EncriptarTexto(string textoPlano)
        {
            string textoCifrado = "";                 // encrypted text

            // Before encrypting data, we will append plain text to a random
            // salt value, which will be between 4 and 8 bytes long (implicitly
            // used defaults).
            OEncriptacion rijndaelKey = new OEncriptacion(ObtenerContraseñaLibreria(), ObtenerVectorInicioLibreria());

            //Console.WriteLine(String.Format("Plaintext   : {0}\n", textoPlano));

            //No hace falta encriptar tantas veces
            //// Encrypt the same plain text data 10 time (using the same key,
            //// initialization vector, etc) and see the resulting cipher text;
            //// encrypted values will be different.
            //for (int i = 0; i < 10; i++)
            //{
            //    cipherText = rijndaelKey.Encrypt(textoPlano);
            //    Console.WriteLine(String.Format("Encrypted #{0}: {1}", i, textoPlano));
            //    //textoPlano = rijndaelKey.Decrypt(textoPlano);
            //}
            textoCifrado = rijndaelKey.Encrypt(textoPlano);

            // Make sure we got decryption working correctly.
            //Console.WriteLine(String.Format("\nDecrypted   :{0}", textoPlano));

            return textoCifrado;
        }
        /// <summary>
        /// Realiza una encriptación personalizada con la contraseña y el vector de incio pasados por argumento
        /// </summary>
        /// <param name="textoPlano">Texto a encriptar</param>
        /// <param name="clave">Clave para la encriptación</param>
        /// <param name="vectorInicio">Vector de incio para la encriptación. Debe ser una cadena ASCII de 16 bytes de longitud</param>
        /// <returns>El texto encriptado</returns>
        public static string EncriptarTexto(string textoPlano, string clave, string vectorInicio)
        {
            if (clave.Trim() != string.Empty)
            {
                if (vectorInicio.Trim().Length != 16)
                {
                    OMensajes.MostrarAviso("El vector de inicio debe estar compuesto por una cadena de 16 carácteres ASCII");
                    return string.Empty;
                }
                else
                {
                    string resultado = string.Empty;

                    OEncriptacion rijndaelKey = new OEncriptacion(clave, vectorInicio);
                    resultado = rijndaelKey.Encrypt(textoPlano);

                    return resultado;
                }
            }
            else
            {
                OMensajes.MostrarAviso("La clave de encrptación no puede ser vacía");
                return string.Empty;
            }
        }
        #endregion

        #region Método(s) estático(s) público(s) desencriptación
        /// <summary>
        /// Desencripta un texto con la contraseña y el vector de incio de la librería
        /// </summary>
        /// <param name="plainText">Texto a desencriptar</param>
        /// <returns>Texto desencriptado</returns>
        public static string DesencriptarTexto(string textoCifrado)
        {
            try
            {
                string textoPlano = "";                      // encrypted text

                // Before encrypting data, we will append plain text to a random
                // salt value, which will be between 4 and 8 bytes long (implicitly
                // used defaults).
                OEncriptacion rijndaelKey = new OEncriptacion(ObtenerContraseñaLibreria(), ObtenerVectorInicioLibreria());

                //Console.WriteLine(String.Format("Plaintext   : {0}\n", textoPlano));

                //No hace falta desencriptar tantas veces
                //// Encrypt the same plain text data 10 time (using the same key,
                //// initialization vector, etc) and see the resulting cipher text;
                //// encrypted values will be different.
                //for (int i = 0; i < 10; i++)
                //{
                //    //cipherText = rijndaelKey.Encrypt(plainText);
                //    //Console.WriteLine(
                //    //    String.Format("Encrypted #{0}: {1}", i, cipherText));
                //    textoPlano = rijndaelKey.Decrypt(textoCifrado);
                //}
                textoPlano = rijndaelKey.Decrypt(textoCifrado);
                // //Make sure we got decryption working correctly.
                //Console.WriteLine(String.Format("\nDecrypted   :{0}", textoPlano));

                return textoPlano;
            }
            catch (System.Security.Cryptography.CryptographicException)
            {
                OMensajes.MostrarAviso("No se ha podido desencriptar el texto. Probablamente la clave y/o el vector de inicio utilizados para la encriptación del texto no se corresponden con los utilizados para la desencriptación.");
                return string.Empty;
            }
        }
        /// <summary>
        /// Realiza una desencriptación personalizada con la contraseña y el vector de incio pasados por argumento
        /// </summary>
        /// <param name="textoCifrado">Texto a desencriptar</param>
        /// <param name="clave">Clave para la desencriptación</param>
        /// <param name="vectorInicio">Vector de incio para la desencriptación. Debe ser una cadena ASCII de 16 bytes de longitud</param>
        /// <returns>El texto desencriptado</returns>
        public static string DesencriptarTexto(string textoCifrado, string clave, string vectorInicio)
        {
            try
            {
                if (clave.Trim() != string.Empty)
                {
                    if (vectorInicio.Trim().Length != 16)
                    {
                        OMensajes.MostrarAviso("El vector de inicio debe estar compuesto por una cadena de 16 carácteres ASCII");
                        return string.Empty;
                    }
                    else
                    {
                        string resultado = string.Empty;

                        OEncriptacion rijndaelKey = new OEncriptacion(clave, vectorInicio);
                        resultado = rijndaelKey.Decrypt(textoCifrado);

                        return resultado;
                    }
                }
                else
                {
                    OMensajes.MostrarAviso("La clave de encrptación no puede ser vacía");
                    return string.Empty;
                }
            }
            catch (System.Security.Cryptography.CryptographicException)
            {
                OMensajes.MostrarAviso("No se ha podido desencriptar el texto. Probablamente la clave y/o el vector de inicio utilizados para la encriptación del texto no se corresponden con los utilizados para la desencriptación.");
                return string.Empty;
            }
        }
        #endregion
    }
}
