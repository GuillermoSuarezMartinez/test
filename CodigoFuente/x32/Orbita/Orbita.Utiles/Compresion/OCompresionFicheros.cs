using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Orbita.Utiles.Compresion.Core;
using Orbita.Utiles.Compresion.Zip;

namespace Orbita.Utiles.Compresion
{
	/// <summary>
	/// Ofrece métodos para comprimir y descomprimir ficheros en distintos formatos, aunque a fecha de marzo de 2011 sólo puede comprimir y descomprimir en el formato .zip
	/// </summary>
	public static class OCompresionFicheros
	{
		#region Enumerados
		/// <summary>
		/// Enumera los tipos de compresión disponibles en OCompresionFicheros. Indica tipo de compresión, no las extensiones utilizadas. Un tipo de compresión puede tener varias extensiones asociadas.
		/// </summary>
		private enum TipoCompresion
		{
			Zip,
			Tar,
			Lzw,
			GZip,
			BZip2
		}
		#endregion Enumerados

		#region Métodos comunes privados
		/// <summary>
		/// Lista en una cadena de texto las extensiones disponibles para el tipo de compresión con el que se está trabajando 
		/// </summary>
		/// <param name="vectorExtensiones">vector con las extensiones disponibles</param>
		/// <returns>Una cadena de texto las extensiones disponibles para el tipo de compresión con el que se está trabajando </returns>
		private static string ObtenerExtensionesDisponibles(string[] vectorExtensiones)
		{
			if (vectorExtensiones.Length > 0)
			{
				string resultado = string.Empty;

				foreach (string s in vectorExtensiones)
				{
					resultado += s + ", ";
				}

				return resultado.Remove(resultado.Length - 2);
			}
			else
			{
				throw new OCompresionFicherosExcepcionBase("No está inciado el vector de extensiones para el tipo de compresión indicado");
			}
		}
		/// <summary>
		/// Devuelve en una cadena de texto la extensión más común para el tipo de compresión con el que se está trabajando, que corresponderá a la extensión de la primera posicón del vector
		/// </summary>
		/// <param name="vectorExtensiones">vector con las extensiones disponibles</param>
		/// <returns>Una cadena de texto la extensión más común para el tipo de compresión con el que se está trabajando, que corresponderá a la extensión de la primera posicón del vector</returns>
		private static string ObtenerExtensionMasComun(string[] vectorExtensiones)
		{
			if (vectorExtensiones.Length > 0)
			{
				return vectorExtensiones[0];
			}
			else
			{
				throw new OCompresionFicherosExcepcionBase("No está inciado el vector de extensiones para el tipo de compresión indicado");
			}
		}
		/// <summary>
		/// Comprueba que la extensión existe o no n el vector de extensiones
		/// </summary>
		/// <param name="extension">Extensión (incluido el punto) de la que se quiere comprobar su exsistencia</param>
		/// <param name="vectorExtensiones">Conjunto de extensiones con las que se va a comprarar</param>
		/// <returns>Ture si la externsión existe en el conjunto de extensiónes; false en caso contrario</returns>
		private static bool ExisteExtension(string extension, string[] vectorExtensiones)
		{
			string extensionConPunto = "." + extension.Replace(".", "");

			if (vectorExtensiones != null && vectorExtensiones.Length > 0)
			{
				foreach (string s in vectorExtensiones)
				{
					if (s.Equals(extensionConPunto))
					{
						//existe
						return true;
					}
				}
			}
			
			return false;
		}
        /// <summary>
        /// Descomprime un fichero comprimido en una de las extensiones admitidas por la librería de compresión
        /// </summary>
        /// <param name="rutaFicheroComprimido">Nombre del fichero .zip a descomprimir</param>
        /// <param name="rutaDirectorioDestino">Ruta con el directorio donde se extraerán el contenido del fichero comprimido</param>
        /// <param name="mostrarInformacionDetallada">Muestra información detallada sobre los archivos y directorios que están siendo descomprimidos. Realmente no se muestra nada, sino que para cada fichero se ejecuta el manejadorFuncionProcesaFicheros y para cada diractorio se ejecuta el manejadorFuncionProcesaDirectorios</param>
        /// <param name="manejadorFuncionProcesaFicheros">Función del tipo "private void ProcesarFichero(object sender, ScanEventArgs e)" que se ejecuta cada vez que se procesa un fichero siempre que mostrarInformacionDetallada sea verdadero</param>
        /// <param name="manejadorFuncionProcesaDirectorios">Función del tipo "private void ProcesarDirectorio(object sender, DirectoryEventArgs e)" que se ejecuta cada vez que se procesa un directorio siempre que mostrarInformacionDetallada sea verdadero</param>
        /// <param name="mostrarProgreso">Indica si debemos mostrar el progreso de la descompresión. Realmente no muestra nada, sino que ejecuta en cada tick la función manejadorFuncionProgreso</param>
        /// <param name="intervaloProgreso">Indica cada cuanto tiempo se invocará la función manejadorFuncionProgreso, en milisegundos</param>
        /// <param name="manejadorFuncionProgreso">Función del tipo "void MostrarProgreso(object sender, ProgressEventArgs e)" que se ejecuta cada vez que hace tick el intervalo del progreso</param>
        /// <param name="sobreescritura">Enumerado del tipo OrbitaZip.Sobreescritura para indicar si sobreescribir siempre, nunca o preguntar si se encuentra un fichero existente en a ruta de descompresión. ATENCIÓN: La sobreescritura de un archivo con sus atributos modificados (por ejemplo, fichero de solo lectura o fichero oculto) da error. No se ha intentado solucionar por falta de tiempo y por no tener la necesidad de hacerlo. Esto ocurre al menos para los archivos .zip</param>
        /// <param name="manejadorFuncionPreguntarSobreescritura">Función del tipo "private bool ConfirmarSobreescritura(string rutaArchivo)" que se ejecuta cada vez que se encuentra un fichero que ya existe en la ruta de descompresión, siempre que el valor de sobreescritura sea OrbitaZip.Sobreescritura.Preguntar ATENCIÓN: La sobreescritura de un archivo con sus atributos modificados (por ejemplo, fichero de solo lectura o fichero oculto) da error. No se ha intentado solucionar por falta de tiempo y por no tener la necesidad de hacerlo. Esto ocurre al menos para los archivos .zip</param>
        /// <param name="restaurarFechas">Las fechas y horas de los ficheros se restaurarán al extraer</param>
        /// <param name="restaurarAtributos">Los atributos de los ficheros se restaurarán al extraer</param>
        /// <param name="descomprimirDirectoriosVacios">Descomprime los directorios vacíos</param>
        /// <param name="contraseña">Contraseña para extraer el fichero comprimido</param>
        /// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
        private static ReturnInfo DescomprimirFicheroSegunExtension(string rutaFicheroComprimido, string rutaDirectorioDestino,
            bool mostrarInformacionDetallada, ProcessFileHandler manejadorFuncionProcesaFicheros, ProcessDirectoryHandler manejadorFuncionProcesaDirectorios,
            bool mostrarProgreso, int intervaloProgreso, ProgressHandler manejadorFuncionProgreso,
            OrbitaZip.Sobreescritura sobreescritura, OrbitaZip.ConfirmOverwriteDelegate manejadorFuncionPreguntarSobreescritura,
            bool restaurarFechas, bool restaurarAtributos, bool descomprimirDirectoriosVacios, string contraseña)
        {
            FileInfo informacionFicheroComprimido;

            informacionFicheroComprimido = new FileInfo(rutaFicheroComprimido);

            if (ExisteExtension(informacionFicheroComprimido.Extension, ExtensionesFicheroZip))
            {
                return DescomprimirFicheroZip(informacionFicheroComprimido.FullName, rutaDirectorioDestino,
                    mostrarInformacionDetallada, manejadorFuncionProcesaFicheros, manejadorFuncionProcesaDirectorios,
                    mostrarProgreso, intervaloProgreso, manejadorFuncionProgreso,
                    sobreescritura, manejadorFuncionPreguntarSobreescritura,
                    restaurarFechas, restaurarAtributos, descomprimirDirectoriosVacios, contraseña);
            }
            else if (ExisteExtension(informacionFicheroComprimido.Extension, ExtensionesFicheroTar))
            {
                return DescomprimirFicheroTar();
            }
            else if (ExisteExtension(informacionFicheroComprimido.Extension, ExtensionesFicheroLzm))
            {
                return DescomprimirFicheroLzm();
            }
            else if (ExisteExtension(informacionFicheroComprimido.Extension, ExtensionesFicheroGzip))
            {
                return DescomprimirFicheroGzip();
            }
            else if (ExisteExtension(informacionFicheroComprimido.Extension, ExtensionesFicheroBZip2))
            {
                return DescomprimirFicheroBZip2();
            }
            else
            {
                return new ReturnInfo(false, new Exception("La librería no puede descomprimir archivos comprimidos del tipo " + informacionFicheroComprimido.Extension));
            }
        }
        /// <summary>
        /// Manejador que procesa la pregunta de sobreescribir la extracción de un fichero comprimido en el caso de indicar que se pregunte (Orbita.Utiles.Compresion.Zip.OrbitaZip.Sobreescritura.Preguntar)
        /// </summary>
        /// <param name="rutaArchivo">Ruta del archivo que se quiere sobreescribir</param>
        private static bool ConfirmarSobreescritura(string rutaArchivo)
        {
            if (OMensajes.MostrarAvisoSiNo("El fichero " + rutaArchivo + " ya existe. ¿Desea sobreescribirlo?") == DialogResult.Yes)
            {
                //Sobreescribimos
                return true;
            }
            return false;
        }
        #endregion Métodos comunes privados

        #region Métodos comunes públicos
        /// <summary>
		/// Descomprime un fichero comprimido en una de las extensiones admitidas por la librería de compresión
		/// </summary>
		/// <param name="rutaFicheroComprimido">Nombre del fichero .zip a descomprimir</param>
		/// <param name="rutaDirectorioDestino">Ruta con el directorio donde se extraerán el contenido del fichero comprimido</param>
		/// <param name="mostrarInformacionDetallada">Muestra información detallada sobre los archivos y directorios que están siendo descomprimidos. Realmente no se muestra nada, sino que para cada fichero se ejecuta el manejadorFuncionProcesaFicheros y para cada diractorio se ejecuta el manejadorFuncionProcesaDirectorios</param>
		/// <param name="manejadorFuncionProcesaFicheros">Función del tipo "private void ProcesarFichero(object sender, ScanEventArgs e)" que se ejecuta cada vez que se procesa un fichero siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <param name="manejadorFuncionProcesaDirectorios">Función del tipo "private void ProcesarDirectorio(object sender, DirectoryEventArgs e)" que se ejecuta cada vez que se procesa un directorio siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <param name="mostrarProgreso">Indica si debemos mostrar el progreso de la descompresión. Realmente no muestra nada, sino que ejecuta en cada tick la función manejadorFuncionProgreso</param>
		/// <param name="intervaloProgreso">Indica cada cuanto tiempo se invocará la función manejadorFuncionProgreso, en milisegundos</param>
		/// <param name="manejadorFuncionProgreso">Función del tipo "void MostrarProgreso(object sender, ProgressEventArgs e)" que se ejecuta cada vez que hace tick el intervalo del progreso</param>
		/// <param name="sobreescritura">Enumerado del tipo OrbitaZip.Sobreescritura para indicar si sobreescribir siempre, nunca o preguntar si se encuentra un fichero existente en a ruta de descompresión. ATENCIÓN: La sobreescritura de un archivo con sus atributos modificados (por ejemplo, fichero de solo lectura o fichero oculto) da error. No se ha intentado solucionar por falta de tiempo y por no tener la necesidad de hacerlo. Esto ocurre al menos para los archivos .zip</param>
		/// <param name="manejadorFuncionPreguntarSobreescritura">Función del tipo "private bool ConfirmarSobreescritura(string rutaArchivo)" que se ejecuta cada vez que se encuentra un fichero que ya existe en la ruta de descompresión, siempre que el valor de sobreescritura sea OrbitaZip.Sobreescritura.Preguntar ATENCIÓN: La sobreescritura de un archivo con sus atributos modificados (por ejemplo, fichero de solo lectura o fichero oculto) da error. No se ha intentado solucionar por falta de tiempo y por no tener la necesidad de hacerlo. Esto ocurre al menos para los archivos .zip</param>
		/// <param name="restaurarFechas">Las fechas y horas de los ficheros se restaurarán al extraer</param>
		/// <param name="restaurarAtributos">Los atributos de los ficheros se restaurarán al extraer</param>
		/// <param name="descomprimirDirectoriosVacios">Descomprime los directorios vacíos</param>
		/// <param name="contraseña">Contraseña para extraer el fichero comprimido</param>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo DescomprimirFichero(string rutaFicheroComprimido, string rutaDirectorioDestino, 
			bool mostrarInformacionDetallada, ProcessFileHandler manejadorFuncionProcesaFicheros, ProcessDirectoryHandler manejadorFuncionProcesaDirectorios, 
			bool mostrarProgreso, int intervaloProgreso, ProgressHandler manejadorFuncionProgreso, 
			OrbitaZip.Sobreescritura sobreescritura, OrbitaZip.ConfirmOverwriteDelegate manejadorFuncionPreguntarSobreescritura,
			bool restaurarFechas, bool restaurarAtributos, bool descomprimirDirectoriosVacios, string contraseña)
		{
            return DescomprimirFicheroSegunExtension(rutaFicheroComprimido, rutaDirectorioDestino,
				mostrarInformacionDetallada, manejadorFuncionProcesaFicheros, manejadorFuncionProcesaDirectorios,
				mostrarProgreso, intervaloProgreso, manejadorFuncionProgreso, 
				sobreescritura, manejadorFuncionPreguntarSobreescritura, 
				restaurarFechas, restaurarAtributos, descomprimirDirectoriosVacios, contraseña);
		}
        /// <summary>
        /// Descomprime un fichero comprimido en una de las extensiones admitidas por la librería de compresión
        /// </summary>
        /// <param name="rutaFicheroComprimido">Nombre del fichero .zip a descomprimir</param>
        /// <param name="rutaDirectorioDestino">Ruta con el directorio donde se extraerán el contenido del fichero comprimido</param>
        /// <param name="mostrarInformacionDetallada">Muestra información detallada sobre los archivos y directorios que están siendo descomprimidos. Realmente no se muestra nada, sino que para cada fichero se ejecuta el manejadorFuncionProcesaFicheros y para cada diractorio se ejecuta el manejadorFuncionProcesaDirectorios</param>
        /// <param name="manejadorFuncionProcesaFicheros">Función del tipo "private void ProcesarFichero(object sender, ScanEventArgs e)" que se ejecuta cada vez que se procesa un fichero siempre que mostrarInformacionDetallada sea verdadero</param>
        /// <param name="manejadorFuncionProcesaDirectorios">Función del tipo "private void ProcesarDirectorio(object sender, DirectoryEventArgs e)" que se ejecuta cada vez que se procesa un directorio siempre que mostrarInformacionDetallada sea verdadero</param>
        /// <param name="mostrarProgreso">Indica si debemos mostrar el progreso de la descompresión. Realmente no muestra nada, sino que ejecuta en cada tick la función manejadorFuncionProgreso</param>
        /// <param name="intervaloProgreso">Indica cada cuanto tiempo se invocará la función manejadorFuncionProgreso, en milisegundos</param>
        /// <param name="manejadorFuncionProgreso">Función del tipo "void MostrarProgreso(object sender, ProgressEventArgs e)" que se ejecuta cada vez que hace tick el intervalo del progreso</param>
        /// <param name="sobreescritura">Enumerado del tipo OrbitaZip.Sobreescritura para indicar si sobreescribir siempre, nunca o preguntar si se encuentra un fichero existente en a ruta de descompresión. ATENCIÓN: La sobreescritura de un archivo con sus atributos modificados (por ejemplo, fichero de solo lectura o fichero oculto) da error. No se ha intentado solucionar por falta de tiempo y por no tener la necesidad de hacerlo. Esto ocurre al menos para los archivos .zip</param>
        /// <param name="manejadorFuncionPreguntarSobreescritura">Función del tipo "private bool ConfirmarSobreescritura(string rutaArchivo)" que se ejecuta cada vez que se encuentra un fichero que ya existe en la ruta de descompresión, siempre que el valor de sobreescritura sea OrbitaZip.Sobreescritura.Preguntar ATENCIÓN: La sobreescritura de un archivo con sus atributos modificados (por ejemplo, fichero de solo lectura o fichero oculto) da error. No se ha intentado solucionar por falta de tiempo y por no tener la necesidad de hacerlo. Esto ocurre al menos para los archivos .zip</param>
        /// <param name="restaurarFechas">Las fechas y horas de los ficheros se restaurarán al extraer</param>
        /// <param name="restaurarAtributos">Los atributos de los ficheros se restaurarán al extraer</param>
        /// <param name="descomprimirDirectoriosVacios">Descomprime los directorios vacíos</param>
        /// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
        public static ReturnInfo DescomprimirFichero(string rutaFicheroComprimido, string rutaDirectorioDestino,
            bool mostrarInformacionDetallada, ProcessFileHandler manejadorFuncionProcesaFicheros, ProcessDirectoryHandler manejadorFuncionProcesaDirectorios,
            bool mostrarProgreso, int intervaloProgreso, ProgressHandler manejadorFuncionProgreso,
            OrbitaZip.Sobreescritura sobreescritura, OrbitaZip.ConfirmOverwriteDelegate manejadorFuncionPreguntarSobreescritura,
            bool restaurarFechas, bool restaurarAtributos, bool descomprimirDirectoriosVacios)
        {
            return DescomprimirFicheroSegunExtension(rutaFicheroComprimido, rutaDirectorioDestino,
                mostrarInformacionDetallada, manejadorFuncionProcesaFicheros, manejadorFuncionProcesaDirectorios,
                mostrarProgreso, intervaloProgreso, manejadorFuncionProgreso,
                sobreescritura, manejadorFuncionPreguntarSobreescritura,
                restaurarFechas, restaurarAtributos, descomprimirDirectoriosVacios, string.Empty);
        }
        /// <summary>
        /// Descomprime un fichero comprimido en una de las extensiones admitidas por la librería de compresión
        /// </summary>
        /// <param name="rutaFicheroComprimido">Nombre del fichero .zip a descomprimir</param>
        /// <param name="rutaDirectorioDestino">Ruta con el directorio donde se extraerán el contenido del fichero comprimido</param>
        /// <param name="mostrarInformacionDetallada">Muestra información detallada sobre los archivos y directorios que están siendo descomprimidos. Realmente no se muestra nada, sino que para cada fichero se ejecuta el manejadorFuncionProcesaFicheros y para cada diractorio se ejecuta el manejadorFuncionProcesaDirectorios</param>
        /// <param name="manejadorFuncionProcesaFicheros">Función del tipo "private void ProcesarFichero(object sender, ScanEventArgs e)" que se ejecuta cada vez que se procesa un fichero siempre que mostrarInformacionDetallada sea verdadero</param>
        /// <param name="manejadorFuncionProcesaDirectorios">Función del tipo "private void ProcesarDirectorio(object sender, DirectoryEventArgs e)" que se ejecuta cada vez que se procesa un directorio siempre que mostrarInformacionDetallada sea verdadero</param>
        /// <param name="mostrarProgreso">Indica si debemos mostrar el progreso de la descompresión. Realmente no muestra nada, sino que ejecuta en cada tick la función manejadorFuncionProgreso</param>
        /// <param name="intervaloProgreso">Indica cada cuanto tiempo se invocará la función manejadorFuncionProgreso, en milisegundos</param>
        /// <param name="manejadorFuncionProgreso">Función del tipo "void MostrarProgreso(object sender, ProgressEventArgs e)" que se ejecuta cada vez que hace tick el intervalo del progreso</param>
        /// <param name="sobreescritura">Enumerado del tipo OrbitaZip.Sobreescritura para indicar si sobreescribir siempre, nunca o preguntar si se encuentra un fichero existente en a ruta de descompresión. ATENCIÓN: La sobreescritura de un archivo con sus atributos modificados (por ejemplo, fichero de solo lectura o fichero oculto) da error. No se ha intentado solucionar por falta de tiempo y por no tener la necesidad de hacerlo. Esto ocurre al menos para los archivos .zip</param>
        /// <param name="manejadorFuncionPreguntarSobreescritura">Función del tipo "private bool ConfirmarSobreescritura(string rutaArchivo)" que se ejecuta cada vez que se encuentra un fichero que ya existe en la ruta de descompresión, siempre que el valor de sobreescritura sea OrbitaZip.Sobreescritura.Preguntar ATENCIÓN: La sobreescritura de un archivo con sus atributos modificados (por ejemplo, fichero de solo lectura o fichero oculto) da error. No se ha intentado solucionar por falta de tiempo y por no tener la necesidad de hacerlo. Esto ocurre al menos para los archivos .zip</param>
        /// <param name="contraseña">Contraseña para extraer el fichero comprimido</param>
        /// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
        public static ReturnInfo DescomprimirFichero(string rutaFicheroComprimido, string rutaDirectorioDestino,
            bool mostrarInformacionDetallada, ProcessFileHandler manejadorFuncionProcesaFicheros, ProcessDirectoryHandler manejadorFuncionProcesaDirectorios,
            bool mostrarProgreso, int intervaloProgreso, ProgressHandler manejadorFuncionProgreso,
            OrbitaZip.Sobreescritura sobreescritura, OrbitaZip.ConfirmOverwriteDelegate manejadorFuncionPreguntarSobreescritura,
            string contraseña)
        {
            return DescomprimirFicheroSegunExtension(rutaFicheroComprimido, rutaDirectorioDestino,
                mostrarInformacionDetallada, manejadorFuncionProcesaFicheros, manejadorFuncionProcesaDirectorios,
                mostrarProgreso, intervaloProgreso, manejadorFuncionProgreso,
                sobreescritura, manejadorFuncionPreguntarSobreescritura,
                true, true, true, contraseña);
        }
        /// <summary>
        /// Descomprime un fichero comprimido en una de las extensiones admitidas por la librería de compresión
        /// </summary>
        /// <param name="rutaFicheroComprimido">Nombre del fichero .zip a descomprimir</param>
        /// <param name="rutaDirectorioDestino">Ruta con el directorio donde se extraerán el contenido del fichero comprimido</param>
        /// <param name="mostrarInformacionDetallada">Muestra información detallada sobre los archivos y directorios que están siendo descomprimidos. Realmente no se muestra nada, sino que para cada fichero se ejecuta el manejadorFuncionProcesaFicheros y para cada diractorio se ejecuta el manejadorFuncionProcesaDirectorios</param>
        /// <param name="manejadorFuncionProcesaFicheros">Función del tipo "private void ProcesarFichero(object sender, ScanEventArgs e)" que se ejecuta cada vez que se procesa un fichero siempre que mostrarInformacionDetallada sea verdadero</param>
        /// <param name="manejadorFuncionProcesaDirectorios">Función del tipo "private void ProcesarDirectorio(object sender, DirectoryEventArgs e)" que se ejecuta cada vez que se procesa un directorio siempre que mostrarInformacionDetallada sea verdadero</param>
        /// <param name="mostrarProgreso">Indica si debemos mostrar el progreso de la descompresión. Realmente no muestra nada, sino que ejecuta en cada tick la función manejadorFuncionProgreso</param>
        /// <param name="intervaloProgreso">Indica cada cuanto tiempo se invocará la función manejadorFuncionProgreso, en milisegundos</param>
        /// <param name="manejadorFuncionProgreso">Función del tipo "void MostrarProgreso(object sender, ProgressEventArgs e)" que se ejecuta cada vez que hace tick el intervalo del progreso</param>
        /// <param name="sobreescritura">Enumerado del tipo OrbitaZip.Sobreescritura para indicar si sobreescribir siempre, nunca o preguntar si se encuentra un fichero existente en a ruta de descompresión. ATENCIÓN: La sobreescritura de un archivo con sus atributos modificados (por ejemplo, fichero de solo lectura o fichero oculto) da error. No se ha intentado solucionar por falta de tiempo y por no tener la necesidad de hacerlo. Esto ocurre al menos para los archivos .zip</param>
        /// <param name="manejadorFuncionPreguntarSobreescritura">Función del tipo "private bool ConfirmarSobreescritura(string rutaArchivo)" que se ejecuta cada vez que se encuentra un fichero que ya existe en la ruta de descompresión, siempre que el valor de sobreescritura sea OrbitaZip.Sobreescritura.Preguntar ATENCIÓN: La sobreescritura de un archivo con sus atributos modificados (por ejemplo, fichero de solo lectura o fichero oculto) da error. No se ha intentado solucionar por falta de tiempo y por no tener la necesidad de hacerlo. Esto ocurre al menos para los archivos .zip</param>
        /// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
        public static ReturnInfo DescomprimirFichero(string rutaFicheroComprimido, string rutaDirectorioDestino,
            bool mostrarInformacionDetallada, ProcessFileHandler manejadorFuncionProcesaFicheros, ProcessDirectoryHandler manejadorFuncionProcesaDirectorios,
            bool mostrarProgreso, int intervaloProgreso, ProgressHandler manejadorFuncionProgreso,
            OrbitaZip.Sobreescritura sobreescritura, OrbitaZip.ConfirmOverwriteDelegate manejadorFuncionPreguntarSobreescritura)
        {
            return DescomprimirFicheroSegunExtension(rutaFicheroComprimido, rutaDirectorioDestino,
                mostrarInformacionDetallada, manejadorFuncionProcesaFicheros, manejadorFuncionProcesaDirectorios,
                mostrarProgreso, intervaloProgreso, manejadorFuncionProgreso,
                sobreescritura, manejadorFuncionPreguntarSobreescritura,
                true, true, true, string.Empty);
        }
        /// <summary>
        /// Descomprime un fichero comprimido en una de las extensiones admitidas por la librería de compresión
        /// </summary>
        /// <param name="rutaFicheroComprimido">Nombre del fichero .zip a descomprimir</param>
        /// <param name="rutaDirectorioDestino">Ruta con el directorio donde se extraerán el contenido del fichero comprimido</param>
        /// <param name="mostrarInformacionDetallada">Muestra información detallada sobre los archivos y directorios que están siendo descomprimidos. Realmente no se muestra nada, sino que para cada fichero se ejecuta el manejadorFuncionProcesaFicheros y para cada diractorio se ejecuta el manejadorFuncionProcesaDirectorios</param>
        /// <param name="manejadorFuncionProcesaFicheros">Función del tipo "private void ProcesarFichero(object sender, ScanEventArgs e)" que se ejecuta cada vez que se procesa un fichero siempre que mostrarInformacionDetallada sea verdadero</param>
        /// <param name="manejadorFuncionProcesaDirectorios">Función del tipo "private void ProcesarDirectorio(object sender, DirectoryEventArgs e)" que se ejecuta cada vez que se procesa un directorio siempre que mostrarInformacionDetallada sea verdadero</param>
        /// <param name="mostrarProgreso">Indica si debemos mostrar el progreso de la descompresión. Realmente no muestra nada, sino que ejecuta en cada tick la función manejadorFuncionProgreso</param>
        /// <param name="intervaloProgreso">Indica cada cuanto tiempo se invocará la función manejadorFuncionProgreso, en milisegundos</param>
        /// <param name="manejadorFuncionProgreso">Función del tipo "void MostrarProgreso(object sender, ProgressEventArgs e)" que se ejecuta cada vez que hace tick el intervalo del progreso</param>
        /// <param name="contraseña">Contraseña para extraer el fichero comprimido</param>
        /// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
        public static ReturnInfo DescomprimirFichero(string rutaFicheroComprimido, string rutaDirectorioDestino,
            bool mostrarInformacionDetallada, ProcessFileHandler manejadorFuncionProcesaFicheros, ProcessDirectoryHandler manejadorFuncionProcesaDirectorios,
            bool mostrarProgreso, int intervaloProgreso, ProgressHandler manejadorFuncionProgreso,
            string contraseña)
        {
            return DescomprimirFicheroSegunExtension(rutaFicheroComprimido, rutaDirectorioDestino,
                mostrarInformacionDetallada, manejadorFuncionProcesaFicheros, manejadorFuncionProcesaDirectorios,
                mostrarProgreso, intervaloProgreso, manejadorFuncionProgreso,
                OrbitaZip.Sobreescritura.Preguntar, ConfirmarSobreescritura,
                true, true, true, contraseña);
        }
        /// <summary>
        /// Descomprime un fichero comprimido en una de las extensiones admitidas por la librería de compresión
        /// </summary>
        /// <param name="rutaFicheroComprimido">Nombre del fichero .zip a descomprimir</param>
        /// <param name="rutaDirectorioDestino">Ruta con el directorio donde se extraerán el contenido del fichero comprimido</param>
        /// <param name="mostrarInformacionDetallada">Muestra información detallada sobre los archivos y directorios que están siendo descomprimidos. Realmente no se muestra nada, sino que para cada fichero se ejecuta el manejadorFuncionProcesaFicheros y para cada diractorio se ejecuta el manejadorFuncionProcesaDirectorios</param>
        /// <param name="manejadorFuncionProcesaFicheros">Función del tipo "private void ProcesarFichero(object sender, ScanEventArgs e)" que se ejecuta cada vez que se procesa un fichero siempre que mostrarInformacionDetallada sea verdadero</param>
        /// <param name="manejadorFuncionProcesaDirectorios">Función del tipo "private void ProcesarDirectorio(object sender, DirectoryEventArgs e)" que se ejecuta cada vez que se procesa un directorio siempre que mostrarInformacionDetallada sea verdadero</param>
        /// <param name="mostrarProgreso">Indica si debemos mostrar el progreso de la descompresión. Realmente no muestra nada, sino que ejecuta en cada tick la función manejadorFuncionProgreso</param>
        /// <param name="intervaloProgreso">Indica cada cuanto tiempo se invocará la función manejadorFuncionProgreso, en milisegundos</param>
        /// <param name="manejadorFuncionProgreso">Función del tipo "void MostrarProgreso(object sender, ProgressEventArgs e)" que se ejecuta cada vez que hace tick el intervalo del progreso</param>
        /// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
        public static ReturnInfo DescomprimirFichero(string rutaFicheroComprimido, string rutaDirectorioDestino,
            bool mostrarInformacionDetallada, ProcessFileHandler manejadorFuncionProcesaFicheros, ProcessDirectoryHandler manejadorFuncionProcesaDirectorios,
            bool mostrarProgreso, int intervaloProgreso, ProgressHandler manejadorFuncionProgreso)
        {
            return DescomprimirFicheroSegunExtension(rutaFicheroComprimido, rutaDirectorioDestino,
                mostrarInformacionDetallada, manejadorFuncionProcesaFicheros, manejadorFuncionProcesaDirectorios,
                mostrarProgreso, intervaloProgreso, manejadorFuncionProgreso,
                OrbitaZip.Sobreescritura.Preguntar, ConfirmarSobreescritura,
                true, true, true, string.Empty);
        }
        /// <summary>
        /// Descomprime un fichero comprimido en una de las extensiones admitidas por la librería de compresión
        /// </summary>
        /// <param name="rutaFicheroComprimido">Nombre del fichero .zip a descomprimir</param>
        /// <param name="rutaDirectorioDestino">Ruta con el directorio donde se extraerán el contenido del fichero comprimido</param>
        /// <param name="mostrarInformacionDetallada">Muestra información detallada sobre los archivos y directorios que están siendo descomprimidos. Realmente no se muestra nada, sino que para cada fichero se ejecuta el manejadorFuncionProcesaFicheros y para cada diractorio se ejecuta el manejadorFuncionProcesaDirectorios</param>
        /// <param name="manejadorFuncionProcesaFicheros">Función del tipo "private void ProcesarFichero(object sender, ScanEventArgs e)" que se ejecuta cada vez que se procesa un fichero siempre que mostrarInformacionDetallada sea verdadero</param>
        /// <param name="manejadorFuncionProcesaDirectorios">Función del tipo "private void ProcesarDirectorio(object sender, DirectoryEventArgs e)" que se ejecuta cada vez que se procesa un directorio siempre que mostrarInformacionDetallada sea verdadero</param>
        /// <param name="contraseña">Contraseña para extraer el fichero comprimido</param>
        /// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
        public static ReturnInfo DescomprimirFichero(string rutaFicheroComprimido, string rutaDirectorioDestino,
            bool mostrarInformacionDetallada, ProcessFileHandler manejadorFuncionProcesaFicheros, ProcessDirectoryHandler manejadorFuncionProcesaDirectorios,
            string contraseña)
        {
            return DescomprimirFicheroSegunExtension(rutaFicheroComprimido, rutaDirectorioDestino,
                mostrarInformacionDetallada, manejadorFuncionProcesaFicheros, manejadorFuncionProcesaDirectorios,
                false, 0, null,
                OrbitaZip.Sobreescritura.Preguntar, ConfirmarSobreescritura,
                true, true, true, contraseña);
        }
        /// <summary>
        /// Descomprime un fichero comprimido en una de las extensiones admitidas por la librería de compresión
        /// </summary>
        /// <param name="rutaFicheroComprimido">Nombre del fichero .zip a descomprimir</param>
        /// <param name="rutaDirectorioDestino">Ruta con el directorio donde se extraerán el contenido del fichero comprimido</param>
        /// <param name="mostrarInformacionDetallada">Muestra información detallada sobre los archivos y directorios que están siendo descomprimidos. Realmente no se muestra nada, sino que para cada fichero se ejecuta el manejadorFuncionProcesaFicheros y para cada diractorio se ejecuta el manejadorFuncionProcesaDirectorios</param>
        /// <param name="manejadorFuncionProcesaFicheros">Función del tipo "private void ProcesarFichero(object sender, ScanEventArgs e)" que se ejecuta cada vez que se procesa un fichero siempre que mostrarInformacionDetallada sea verdadero</param>
        /// <param name="manejadorFuncionProcesaDirectorios">Función del tipo "private void ProcesarDirectorio(object sender, DirectoryEventArgs e)" que se ejecuta cada vez que se procesa un directorio siempre que mostrarInformacionDetallada sea verdadero</param>
        /// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
        public static ReturnInfo DescomprimirFichero(string rutaFicheroComprimido, string rutaDirectorioDestino,
            bool mostrarInformacionDetallada, ProcessFileHandler manejadorFuncionProcesaFicheros, ProcessDirectoryHandler manejadorFuncionProcesaDirectorios)
        {
            return DescomprimirFicheroSegunExtension(rutaFicheroComprimido, rutaDirectorioDestino,
                mostrarInformacionDetallada, manejadorFuncionProcesaFicheros, manejadorFuncionProcesaDirectorios,
                false, 0, null,
                OrbitaZip.Sobreescritura.Preguntar, ConfirmarSobreescritura,
                true, true, true, string.Empty);
        }
        /// <summary>
        /// Descomprime un fichero comprimido en una de las extensiones admitidas por la librería de compresión
        /// </summary>
        /// <param name="rutaFicheroComprimido">Nombre del fichero .zip a descomprimir</param>
        /// <param name="rutaDirectorioDestino">Ruta con el directorio donde se extraerán el contenido del fichero comprimido</param>
        /// <param name="mostrarProgreso">Indica si debemos mostrar el progreso de la descompresión. Realmente no muestra nada, sino que ejecuta en cada tick la función manejadorFuncionProgreso</param>
        /// <param name="intervaloProgreso">Indica cada cuanto tiempo se invocará la función manejadorFuncionProgreso, en milisegundos</param>
        /// <param name="manejadorFuncionProgreso">Función del tipo "void MostrarProgreso(object sender, ProgressEventArgs e)" que se ejecuta cada vez que hace tick el intervalo del progreso</param>
        /// <param name="contraseña">Contraseña para extraer el fichero comprimido</param>
        /// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
        public static ReturnInfo DescomprimirFichero(string rutaFicheroComprimido, string rutaDirectorioDestino,
            bool mostrarProgreso, int intervaloProgreso, ProgressHandler manejadorFuncionProgreso,
            string contraseña)
        {
            return DescomprimirFicheroSegunExtension(rutaFicheroComprimido, rutaDirectorioDestino,
                false, null, null,
                mostrarProgreso, intervaloProgreso, manejadorFuncionProgreso,
                OrbitaZip.Sobreescritura.Preguntar, ConfirmarSobreescritura,
                true, true, true, contraseña);
        }
        /// <summary>
        /// Descomprime un fichero comprimido en una de las extensiones admitidas por la librería de compresión
        /// </summary>
        /// <param name="rutaFicheroComprimido">Nombre del fichero .zip a descomprimir</param>
        /// <param name="rutaDirectorioDestino">Ruta con el directorio donde se extraerán el contenido del fichero comprimido</param>
        /// <param name="mostrarProgreso">Indica si debemos mostrar el progreso de la descompresión. Realmente no muestra nada, sino que ejecuta en cada tick la función manejadorFuncionProgreso</param>
        /// <param name="intervaloProgreso">Indica cada cuanto tiempo se invocará la función manejadorFuncionProgreso, en milisegundos</param>
        /// <param name="manejadorFuncionProgreso">Función del tipo "void MostrarProgreso(object sender, ProgressEventArgs e)" que se ejecuta cada vez que hace tick el intervalo del progreso</param>
        /// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
        public static ReturnInfo DescomprimirFichero(string rutaFicheroComprimido, string rutaDirectorioDestino,
            bool mostrarProgreso, int intervaloProgreso, ProgressHandler manejadorFuncionProgreso)
        {
            return DescomprimirFicheroSegunExtension(rutaFicheroComprimido, rutaDirectorioDestino,
                false, null, null,
                mostrarProgreso, intervaloProgreso, manejadorFuncionProgreso,
                OrbitaZip.Sobreescritura.Preguntar, ConfirmarSobreescritura,
                true, true, true, string.Empty);
        }
        /// <summary>
        /// Descomprime un fichero comprimido en una de las extensiones admitidas por la librería de compresión
        /// </summary>
        /// <param name="rutaFicheroComprimido">Nombre del fichero .zip a descomprimir</param>
        /// <param name="rutaDirectorioDestino">Ruta con el directorio donde se extraerán el contenido del fichero comprimido</param>
        /// <param name="contraseña">Contraseña para extraer el fichero comprimido</param>
        /// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
        public static ReturnInfo DescomprimirFichero(string rutaFicheroComprimido, string rutaDirectorioDestino,
            string contraseña)
        {
            return DescomprimirFicheroSegunExtension(rutaFicheroComprimido, rutaDirectorioDestino,
                false, null, null,
                false, 0, null,
                OrbitaZip.Sobreescritura.Preguntar, ConfirmarSobreescritura,
                true, true, true, contraseña);
        }
        /// <summary>
        /// Descomprime un fichero comprimido en una de las extensiones admitidas por la librería de compresión
        /// </summary>
        /// <param name="rutaFicheroComprimido">Nombre del fichero .zip a descomprimir</param>
        /// <param name="rutaDirectorioDestino">Ruta con el directorio donde se extraerán el contenido del fichero comprimido</param>
        /// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
        public static ReturnInfo DescomprimirFichero(string rutaFicheroComprimido, string rutaDirectorioDestino)
        {
            return DescomprimirFicheroSegunExtension(rutaFicheroComprimido, rutaDirectorioDestino,
                false, null, null,
                false, 0, null,
                OrbitaZip.Sobreescritura.Preguntar, ConfirmarSobreescritura,
                true, true, true, string.Empty);
        }
        #endregion Métodos comunes públicos

        #region zip

        #region Atributo/s
        /// <summary>
		/// Vector con las posibles extensiones de ficheros comprimidos con el algoritmo zip. Se incluye el punto que precede a las extensiones de ficheros
		/// </summary>
		private static string[] ExtensionesFicheroZip = { ".zip" };
			#endregion Atributo/s

			#region Compresión en zip de directorios
		/// <summary>
		/// Comprime un directorio en un archivo .zip
		/// </summary>
		/// <param name="rutaFicheroComprimido">Nombre o ruta (absoluta o relativa) del fichero .zip a generar o ruta del mismo, con o sin extensión .zip</param>
		/// <param name="rutaDirectorioOrigen">Ruta con el directorio a comprimir en el fichero rutaFicheroZip.zip</param>
		/// <param name="mostrarInformacionDetallada">Muestra información detallada sobre los archivos y directorios que están siendo comprimidos. Realmente no se muestra nada, sino que para cada fichero se ejecuta el manejadorFuncionProcesaFicheros y para cada diractorio se ejecuta el manejadorFuncionProcesaDirectorios. Si vale false, los manejadores que ejecutan pueden ser nulos (es decir, los dos siguientes parámetros pueden ser nulos)</param>
		/// <param name="manejadorFuncionProcesaFicheros">Función del tipo "private void ProcesarFichero(object sender, ScanEventArgs e)" que se ejecuta cada vez que se procesa un fichero siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <param name="manejadorFuncionProcesaDirectorios">Función del tipo "private void ProcesarDirectorio(object sender, DirectoryEventArgs e)" que se ejecuta cada vez que se procesa un directorio siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <param name="mostrarProgreso">Indica si debemos mostrar el progreso de cada uno de los ficheros comprimidos (no del progreso global de compresión). Realmente no muestra nada, sino que ejecuta en cada tick la función manejadorFuncionProgreso. Si vale false, el manejador que ejecuta puede ser nulo, y el intervalo puede ser cualquier valor, puesto que no se utiliza</param>
		/// <param name="intervaloProgreso">Indica cada cuanto tiempo se invocará la función manejadorFuncionProgreso, en milisegundos</param>
		/// <param name="manejadorFuncionProgreso">Función del tipo "void MostrarProgreso(object sender, ProgressEventArgs e)" que se ejecuta cada vez que hace tick el intervalo del progreso</param>
		/// <param name="comprimirDirectoriosVacios">Comprime los directorios vacíos</param>
		/// <param name="compresionRecursiva">Incluye los subdirectorios y sus ficheros recursivamente en el archivo comprimido</param>
		/// <param name="contraseña">Contraseña para extraer los ficheros comprimidos</param>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirDirectorioZip(string rutaFicheroZip, string rutaDirectorioOrigen, 
			bool mostrarInformacionDetallada, ProcessFileHandler manejadorFuncionProcesaFicheros,
			ProcessDirectoryHandler manejadorFuncionProcesaDirectorios, bool mostrarProgreso,
			int intervaloProgreso, ProgressHandler manejadorFuncionProgreso, bool comprimirDirectoriosVacios,
			bool compresionRecursiva, string contraseña)
		{
			try
			{
				if (rutaFicheroZip.EndsWith(Path.DirectorySeparatorChar.ToString()))
				{
					rutaFicheroZip = rutaFicheroZip.Remove(rutaFicheroZip.Length - 1);
				}

				if (!rutaFicheroZip.ToLower().EndsWith(OCompresionFicheros.ObtenerExtensionMasComun(ExtensionesFicheroZip)))
				{
					rutaFicheroZip += OCompresionFicheros.ObtenerExtensionMasComun(ExtensionesFicheroZip);
				}

				EventosOrbitaZip events = null;

				if (mostrarInformacionDetallada || mostrarProgreso)
				{
					events = new EventosOrbitaZip();
				}

				if (mostrarInformacionDetallada)
				{
					events.ProcessDirectory = new ProcessDirectoryHandler(manejadorFuncionProcesaDirectorios);
					events.ProcessFile = new ProcessFileHandler(manejadorFuncionProcesaFicheros);
				}

				if (mostrarProgreso)
				{
					events.Progress = new ProgressHandler(manejadorFuncionProgreso);
					events.ProgressInterval = TimeSpan.FromMilliseconds(intervaloProgreso);
				}

				using (OrbitaZip compresionZip = new OrbitaZip(events))
				{
					compresionZip.CreateEmptyDirectories = comprimirDirectoriosVacios;
					compresionZip.RestoreAttributesOnExtract = true;

					if (contraseña != null && contraseña != string.Empty)
					{
						compresionZip.Password = contraseña;
					}

					compresionZip.CreateZip(rutaFicheroZip, rutaDirectorioOrigen, compresionRecursiva, null, null);
					
					return new ReturnInfo();
				}
			}
			catch (System.Exception ex)
			{
				return new ReturnInfo(false, ex);
			}
		}
		/// <summary>
		/// Comprime un directorio en un archivo .zip
		/// </summary>
		/// <param name="rutaFicheroComprimido">Nombre o ruta (absoluta o relativa) del fichero .zip a generar o ruta del mismo, con o sin extensión .zip</param>
		/// <param name="rutaDirectorioOrigen">Ruta con el directorio a comprimir en el fichero rutaFicheroZip.zip</param>
		/// <param name="mostrarInformacionDetallada">Muestra información detallada sobre los archivos y directorios que están siendo comprimidos. Realmente no se muestra nada, sino que para cada fichero se ejecuta el manejadorFuncionProcesaFicheros y para cada diractorio se ejecuta el manejadorFuncionProcesaDirectorios. Si vale false, los manejadores que ejecutan pueden ser nulos (es decir, los dos siguientes parámetros pueden ser nulos)</param>
		/// <param name="manejadorFuncionProcesaFicheros">Función del tipo "private void ProcesarFichero(object sender, ScanEventArgs e)" que se ejecuta cada vez que se procesa un fichero siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <param name="manejadorFuncionProcesaDirectorios">Función del tipo "private void ProcesarDirectorio(object sender, DirectoryEventArgs e)" que se ejecuta cada vez que se procesa un directorio siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <param name="mostrarProgreso">Indica si debemos mostrar el progreso de cada uno de los ficheros comprimidos (no del progreso global de compresión). Realmente no muestra nada, sino que ejecuta en cada tick la función manejadorFuncionProgreso. Si vale false, el manejador que ejecuta puede ser nulo, y el intervalo puede ser cualquier valor, puesto que no se utiliza</param>
		/// <param name="intervaloProgreso">Indica cada cuanto tiempo se invocará la función manejadorFuncionProgreso, en milisegundos</param>
		/// <param name="manejadorFuncionProgreso">Función del tipo "void MostrarProgreso(object sender, ProgressEventArgs e)" que se ejecuta cada vez que hace tick el intervalo del progreso</param>
		/// <param name="comprimirDirectoriosVacios">Comprime los directorios vacíos</param>
		/// <param name="compresionRecursiva">Incluye los subdirectorios y sus ficheros recursivamente en el archivo comprimido</param>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirDirectorioZip(string rutaFicheroZip, string rutaDirectorioOrigen,
			bool mostrarInformacionDetallada, ProcessFileHandler manejadorFuncionProcesaFicheros,
			ProcessDirectoryHandler manejadorFuncionProcesaDirectorios, bool mostrarProgreso,
			int intervaloProgreso, ProgressHandler manejadorFuncionProgreso, bool comprimirDirectoriosVacios,
			bool compresionRecursiva)
		{
			return OCompresionFicheros.ComprimirDirectorioZip(rutaFicheroZip, rutaDirectorioOrigen, mostrarInformacionDetallada,
				manejadorFuncionProcesaFicheros, manejadorFuncionProcesaDirectorios, mostrarProgreso, intervaloProgreso,
				manejadorFuncionProgreso, comprimirDirectoriosVacios, compresionRecursiva, null);
		}
		/// <summary>
		/// Comprime un directorio en un archivo .zip
		/// </summary>
		/// <param name="rutaFicheroComprimido">Nombre o ruta (absoluta o relativa) del fichero .zip a generar o ruta del mismo, con o sin extensión .zip</param>
		/// <param name="rutaDirectorioOrigen">Ruta con el directorio a comprimir en el fichero rutaFicheroZip.zip</param>
		/// <param name="mostrarInformacionDetallada">Muestra información detallada sobre los archivos y directorios que están siendo comprimidos. Realmente no se muestra nada, sino que para cada fichero se ejecuta el manejadorFuncionProcesaFicheros y para cada diractorio se ejecuta el manejadorFuncionProcesaDirectorios. Si vale false, los manejadores que ejecutan pueden ser nulos (es decir, los dos siguientes parámetros pueden ser nulos)</param>
		/// <param name="manejadorFuncionProcesaFicheros">Función del tipo "private void ProcesarFichero(object sender, ScanEventArgs e)" que se ejecuta cada vez que se procesa un fichero siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <param name="manejadorFuncionProcesaDirectorios">Función del tipo "private void ProcesarDirectorio(object sender, DirectoryEventArgs e)" que se ejecuta cada vez que se procesa un directorio siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <param name="mostrarProgreso">Indica si debemos mostrar el progreso de cada uno de los ficheros comprimidos (no del progreso global de compresión). Realmente no muestra nada, sino que ejecuta en cada tick la función manejadorFuncionProgreso. Si vale false, el manejador que ejecuta puede ser nulo, y el intervalo puede ser cualquier valor, puesto que no se utiliza</param>
		/// <param name="intervaloProgreso">Indica cada cuanto tiempo se invocará la función manejadorFuncionProgreso, en milisegundos</param>
		/// <param name="manejadorFuncionProgreso">Función del tipo "void MostrarProgreso(object sender, ProgressEventArgs e)" que se ejecuta cada vez que hace tick el intervalo del progreso</param>
		/// <param name="comprimirDirectoriosVacios">Comprime los directorios vacíos</param>
		/// <param name="contraseña">Contraseña para extraer los ficheros comprimidos</param>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirDirectorioZip(string rutaFicheroZip, string rutaDirectorioOrigen,
			bool mostrarInformacionDetallada, ProcessFileHandler manejadorFuncionProcesaFicheros,
			ProcessDirectoryHandler manejadorFuncionProcesaDirectorios, bool mostrarProgreso,
			int intervaloProgreso, ProgressHandler manejadorFuncionProgreso, bool comprimirDirectoriosVacios,
			string contraseña)
		{
			return OCompresionFicheros.ComprimirDirectorioZip(rutaFicheroZip, rutaDirectorioOrigen, mostrarInformacionDetallada,
				manejadorFuncionProcesaFicheros, manejadorFuncionProcesaDirectorios, mostrarProgreso, intervaloProgreso,
				manejadorFuncionProgreso, comprimirDirectoriosVacios, true, contraseña);
		}
		/// <summary>
		/// Comprime un directorio en un archivo .zip
		/// </summary>
		/// <param name="rutaFicheroComprimido">Nombre o ruta (absoluta o relativa) del fichero .zip a generar o ruta del mismo, con o sin extensión .zip</param>
		/// <param name="rutaDirectorioOrigen">Ruta con el directorio a comprimir en el fichero rutaFicheroZip.zip</param>
		/// <param name="mostrarInformacionDetallada">Muestra información detallada sobre los archivos y directorios que están siendo comprimidos. Realmente no se muestra nada, sino que para cada fichero se ejecuta el manejadorFuncionProcesaFicheros y para cada diractorio se ejecuta el manejadorFuncionProcesaDirectorios. Si vale false, los manejadores que ejecutan pueden ser nulos (es decir, los dos siguientes parámetros pueden ser nulos)</param>
		/// <param name="manejadorFuncionProcesaFicheros">Función del tipo "private void ProcesarFichero(object sender, ScanEventArgs e)" que se ejecuta cada vez que se procesa un fichero siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <param name="manejadorFuncionProcesaDirectorios">Función del tipo "private void ProcesarDirectorio(object sender, DirectoryEventArgs e)" que se ejecuta cada vez que se procesa un directorio siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <param name="mostrarProgreso">Indica si debemos mostrar el progreso de cada uno de los ficheros comprimidos (no del progreso global de compresión). Realmente no muestra nada, sino que ejecuta en cada tick la función manejadorFuncionProgreso. Si vale false, el manejador que ejecuta puede ser nulo, y el intervalo puede ser cualquier valor, puesto que no se utiliza</param>
		/// <param name="intervaloProgreso">Indica cada cuanto tiempo se invocará la función manejadorFuncionProgreso, en milisegundos</param>
		/// <param name="manejadorFuncionProgreso">Función del tipo "void MostrarProgreso(object sender, ProgressEventArgs e)" que se ejecuta cada vez que hace tick el intervalo del progreso</param>
		/// <param name="comprimirDirectoriosVacios">Comprime los directorios vacíos</param>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirDirectorioZip(string rutaFicheroZip, string rutaDirectorioOrigen,
			bool mostrarInformacionDetallada, ProcessFileHandler manejadorFuncionProcesaFicheros,
			ProcessDirectoryHandler manejadorFuncionProcesaDirectorios, bool mostrarProgreso,
			int intervaloProgreso, ProgressHandler manejadorFuncionProgreso, bool comprimirDirectoriosVacios)
		{
			return OCompresionFicheros.ComprimirDirectorioZip(rutaFicheroZip, rutaDirectorioOrigen, mostrarInformacionDetallada,
				manejadorFuncionProcesaFicheros, manejadorFuncionProcesaDirectorios, mostrarProgreso, intervaloProgreso,
				manejadorFuncionProgreso, comprimirDirectoriosVacios, true, null);
		}
		/// <summary>
		/// Comprime un directorio en un archivo .zip
		/// </summary>
		/// <param name="rutaFicheroComprimido">Nombre o ruta (absoluta o relativa) del fichero .zip a generar o ruta del mismo, con o sin extensión .zip</param>
		/// <param name="rutaDirectorioOrigen">Ruta con el directorio a comprimir en el fichero rutaFicheroZip.zip</param>
		/// <param name="mostrarInformacionDetallada">Muestra información detallada sobre los archivos y directorios que están siendo comprimidos. Realmente no se muestra nada, sino que para cada fichero se ejecuta el manejadorFuncionProcesaFicheros y para cada diractorio se ejecuta el manejadorFuncionProcesaDirectorios. Si vale false, los manejadores que ejecutan pueden ser nulos (es decir, los dos siguientes parámetros pueden ser nulos)</param>
		/// <param name="manejadorFuncionProcesaFicheros">Función del tipo "private void ProcesarFichero(object sender, ScanEventArgs e)" que se ejecuta cada vez que se procesa un fichero siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <param name="manejadorFuncionProcesaDirectorios">Función del tipo "private void ProcesarDirectorio(object sender, DirectoryEventArgs e)" que se ejecuta cada vez que se procesa un directorio siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <param name="mostrarProgreso">Indica si debemos mostrar el progreso de cada uno de los ficheros comprimidos (no del progreso global de compresión). Realmente no muestra nada, sino que ejecuta en cada tick la función manejadorFuncionProgreso. Si vale false, el manejador que ejecuta puede ser nulo, y el intervalo puede ser cualquier valor, puesto que no se utiliza</param>
		/// <param name="intervaloProgreso">Indica cada cuanto tiempo se invocará la función manejadorFuncionProgreso, en milisegundos</param>
		/// <param name="manejadorFuncionProgreso">Función del tipo "void MostrarProgreso(object sender, ProgressEventArgs e)" que se ejecuta cada vez que hace tick el intervalo del progreso</param>
		/// <param name="contraseña">Contraseña para extraer los ficheros comprimidos</param>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirDirectorioZip(string rutaFicheroZip, string rutaDirectorioOrigen,
			bool mostrarInformacionDetallada, ProcessFileHandler manejadorFuncionProcesaFicheros,
			ProcessDirectoryHandler manejadorFuncionProcesaDirectorios, bool mostrarProgreso,
			int intervaloProgreso, ProgressHandler manejadorFuncionProgreso, string contraseña)
		{
			return OCompresionFicheros.ComprimirDirectorioZip(rutaFicheroZip, rutaDirectorioOrigen, mostrarInformacionDetallada,
				manejadorFuncionProcesaFicheros, manejadorFuncionProcesaDirectorios, mostrarProgreso, intervaloProgreso,
				manejadorFuncionProgreso, true, true, contraseña);
		}
		/// <summary>
		/// Comprime un directorio en un archivo .zip
		/// </summary>
		/// <param name="rutaFicheroComprimido">Nombre o ruta (absoluta o relativa) del fichero .zip a generar o ruta del mismo, con o sin extensión .zip</param>
		/// <param name="rutaDirectorioOrigen">Ruta con el directorio a comprimir en el fichero rutaFicheroZip.zip</param>
		/// <param name="mostrarInformacionDetallada">Muestra información detallada sobre los archivos y directorios que están siendo comprimidos. Realmente no se muestra nada, sino que para cada fichero se ejecuta el manejadorFuncionProcesaFicheros y para cada diractorio se ejecuta el manejadorFuncionProcesaDirectorios. Si vale false, los manejadores que ejecutan pueden ser nulos (es decir, los dos siguientes parámetros pueden ser nulos)</param>
		/// <param name="manejadorFuncionProcesaFicheros">Función del tipo "private void ProcesarFichero(object sender, ScanEventArgs e)" que se ejecuta cada vez que se procesa un fichero siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <param name="manejadorFuncionProcesaDirectorios">Función del tipo "private void ProcesarDirectorio(object sender, DirectoryEventArgs e)" que se ejecuta cada vez que se procesa un directorio siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <param name="mostrarProgreso">Indica si debemos mostrar el progreso de cada uno de los ficheros comprimidos (no del progreso global de compresión). Realmente no muestra nada, sino que ejecuta en cada tick la función manejadorFuncionProgreso. Si vale false, el manejador que ejecuta puede ser nulo, y el intervalo puede ser cualquier valor, puesto que no se utiliza</param>
		/// <param name="intervaloProgreso">Indica cada cuanto tiempo se invocará la función manejadorFuncionProgreso, en milisegundos</param>
		/// <param name="manejadorFuncionProgreso">Función del tipo "void MostrarProgreso(object sender, ProgressEventArgs e)" que se ejecuta cada vez que hace tick el intervalo del progreso</param>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirDirectorioZip(string rutaFicheroZip, string rutaDirectorioOrigen,
			bool mostrarInformacionDetallada, ProcessFileHandler manejadorFuncionProcesaFicheros,
			ProcessDirectoryHandler manejadorFuncionProcesaDirectorios, bool mostrarProgreso,
			int intervaloProgreso, ProgressHandler manejadorFuncionProgreso)
		{
			return OCompresionFicheros.ComprimirDirectorioZip(rutaFicheroZip, rutaDirectorioOrigen, mostrarInformacionDetallada,
				manejadorFuncionProcesaFicheros, manejadorFuncionProcesaDirectorios, mostrarProgreso, intervaloProgreso,
				manejadorFuncionProgreso, true, true, null);
		}
		/// <summary>
		/// Comprime un directorio en un archivo .zip
		/// </summary>
		/// <param name="rutaFicheroComprimido">Nombre o ruta (absoluta o relativa) del fichero .zip a generar o ruta del mismo, con o sin extensión .zip</param>
		/// <param name="rutaDirectorioOrigen">Ruta con el directorio a comprimir en el fichero rutaFicheroZip.zip</param>
		/// <param name="mostrarInformacionDetallada">Muestra información detallada sobre los archivos y directorios que están siendo comprimidos. Realmente no se muestra nada, sino que para cada fichero se ejecuta el manejadorFuncionProcesaFicheros y para cada diractorio se ejecuta el manejadorFuncionProcesaDirectorios. Si vale false, los manejadores que ejecutan pueden ser nulos (es decir, los dos siguientes parámetros pueden ser nulos)</param>
		/// <param name="manejadorFuncionProcesaFicheros">Función del tipo "private void ProcesarFichero(object sender, ScanEventArgs e)" que se ejecuta cada vez que se procesa un fichero siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <param name="manejadorFuncionProcesaDirectorios">Función del tipo "private void ProcesarDirectorio(object sender, DirectoryEventArgs e)" que se ejecuta cada vez que se procesa un directorio siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <param name="contraseña">Contraseña para extraer los ficheros comprimidos</param>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirDirectorioZip(string rutaFicheroZip, string rutaDirectorioOrigen,
			bool mostrarInformacionDetallada, ProcessFileHandler manejadorFuncionProcesaFicheros,
			ProcessDirectoryHandler manejadorFuncionProcesaDirectorios, string contraseña)
		{
			return OCompresionFicheros.ComprimirDirectorioZip(rutaFicheroZip, rutaDirectorioOrigen, mostrarInformacionDetallada,
				manejadorFuncionProcesaFicheros, manejadorFuncionProcesaDirectorios, false, 0, null, true, true, contraseña);
		}
		/// <summary>
		/// Comprime un directorio en un archivo .zip
		/// </summary>
		/// <param name="rutaFicheroComprimido">Nombre o ruta (absoluta o relativa) del fichero .zip a generar o ruta del mismo, con o sin extensión .zip</param>
		/// <param name="rutaDirectorioOrigen">Ruta con el directorio a comprimir en el fichero rutaFicheroZip.zip</param>
		/// <param name="mostrarInformacionDetallada">Muestra información detallada sobre los archivos y directorios que están siendo comprimidos. Realmente no se muestra nada, sino que para cada fichero se ejecuta el manejadorFuncionProcesaFicheros y para cada diractorio se ejecuta el manejadorFuncionProcesaDirectorios. Si vale false, los manejadores que ejecutan pueden ser nulos (es decir, los dos siguientes parámetros pueden ser nulos)</param>
		/// <param name="manejadorFuncionProcesaFicheros">Función del tipo "private void ProcesarFichero(object sender, ScanEventArgs e)" que se ejecuta cada vez que se procesa un fichero siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <param name="manejadorFuncionProcesaDirectorios">Función del tipo "private void ProcesarDirectorio(object sender, DirectoryEventArgs e)" que se ejecuta cada vez que se procesa un directorio siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirDirectorioZip(string rutaFicheroZip, string rutaDirectorioOrigen,
			bool mostrarInformacionDetallada, ProcessFileHandler manejadorFuncionProcesaFicheros,
			ProcessDirectoryHandler manejadorFuncionProcesaDirectorios)
		{
			return OCompresionFicheros.ComprimirDirectorioZip(rutaFicheroZip, rutaDirectorioOrigen, mostrarInformacionDetallada,
				manejadorFuncionProcesaFicheros, manejadorFuncionProcesaDirectorios, false, 0, null, true, true, null);
		}
		/// <summary>
		/// Comprime un directorio en un archivo .zip
		/// </summary>
		/// <param name="rutaFicheroComprimido">Nombre o ruta (absoluta o relativa) del fichero .zip a generar o ruta del mismo, con o sin extensión .zip</param>
		/// <param name="rutaDirectorioOrigen">Ruta con el directorio a comprimir en el fichero rutaFicheroZip.zip</param>
		/// <param name="mostrarProgreso">Indica si debemos mostrar el progreso de cada uno de los ficheros comprimidos (no del progreso global de compresión). Realmente no muestra nada, sino que ejecuta en cada tick la función manejadorFuncionProgreso. Si vale false, el manejador que ejecuta puede ser nulo, y el intervalo puede ser cualquier valor, puesto que no se utiliza</param>
		/// <param name="intervaloProgreso">Indica cada cuanto tiempo se invocará la función manejadorFuncionProgreso, en milisegundos</param>
		/// <param name="manejadorFuncionProgreso">Función del tipo "void MostrarProgreso(object sender, ProgressEventArgs e)" que se ejecuta cada vez que hace tick el intervalo del progreso</param>
		/// <param name="contraseña">Contraseña para extraer los ficheros comprimidos</param>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirDirectorioZip(string rutaFicheroZip, string rutaDirectorioOrigen,
			bool mostrarProgreso, int intervaloProgreso, ProgressHandler manejadorFuncionProgreso, string contraseña)
		{
			return OCompresionFicheros.ComprimirDirectorioZip(rutaFicheroZip, rutaDirectorioOrigen, false,
				null, null, mostrarProgreso, intervaloProgreso, manejadorFuncionProgreso, true, true, contraseña);
		}
		/// <summary>
		/// Comprime un directorio en un archivo .zip
		/// </summary>
		/// <param name="rutaFicheroComprimido">Nombre o ruta (absoluta o relativa) del fichero .zip a generar o ruta del mismo, con o sin extensión .zip</param>
		/// <param name="rutaDirectorioOrigen">Ruta con el directorio a comprimir en el fichero rutaFicheroZip.zip</param>
		/// <param name="mostrarProgreso">Indica si debemos mostrar el progreso de cada uno de los ficheros comprimidos (no del progreso global de compresión). Realmente no muestra nada, sino que ejecuta en cada tick la función manejadorFuncionProgreso. Si vale false, el manejador que ejecuta puede ser nulo, y el intervalo puede ser cualquier valor, puesto que no se utiliza</param>
		/// <param name="intervaloProgreso">Indica cada cuanto tiempo se invocará la función manejadorFuncionProgreso, en milisegundos</param>
		/// <param name="manejadorFuncionProgreso">Función del tipo "void MostrarProgreso(object sender, ProgressEventArgs e)" que se ejecuta cada vez que hace tick el intervalo del progreso</param>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirDirectorioZip(string rutaFicheroZip, string rutaDirectorioOrigen,
			bool mostrarProgreso, int intervaloProgreso, ProgressHandler manejadorFuncionProgreso)
		{
			return OCompresionFicheros.ComprimirDirectorioZip(rutaFicheroZip, rutaDirectorioOrigen, false,
				null, null, mostrarProgreso, intervaloProgreso, manejadorFuncionProgreso, true, true, null);
		}
		/// <summary>
		/// Comprime un directorio en un archivo .zip
		/// </summary>
		/// <param name="rutaFicheroComprimido">Nombre o ruta (absoluta o relativa) del fichero .zip a generar o ruta del mismo, con o sin extensión .zip</param>
		/// <param name="rutaDirectorioOrigen">Ruta con el directorio a comprimir en el fichero rutaFicheroZip.zip</param>
		/// <param name="contraseña">Contraseña para extraer los ficheros comprimidos</param>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirDirectorioZip(string rutaFicheroZip, string rutaDirectorioOrigen, string contraseña)
		{
			return OCompresionFicheros.ComprimirDirectorioZip(rutaFicheroZip, rutaDirectorioOrigen, false,
				null, null, false, 0, null, true, true, contraseña);
		}
		/// <summary>
		/// Comprime un directorio en un archivo .zip
		/// </summary>
		/// <param name="rutaFicheroComprimido">Nombre o ruta (absoluta o relativa) del fichero .zip a generar o ruta del mismo, con o sin extensión .zip</param>
		/// <param name="rutaDirectorioOrigen">Ruta con el directorio a comprimir en el fichero rutaFicheroZip.zip</param>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirDirectorioZip(string rutaFicheroZip, string rutaDirectorioOrigen)
		{
			return OCompresionFicheros.ComprimirDirectorioZip(rutaFicheroZip, rutaDirectorioOrigen, false,
				null, null, false, 0, null, true, true, null);
		}
			#endregion Compresión en zip de directorios

			#region Compresión en zip de ficheros
		/// <summary>
		/// Comprime un directorio en un archivo .zip
		/// </summary>
		/// <param name="rutaFicheroComprimido">Nombre o ruta (absoluta o relativa) del fichero .zip a generar o ruta del mismo, con o sin extensión .zip</param>
		/// <param name="rutaFicheroOrigen">Nombre o ruta (absoluta o relativa) del fichero a comprimir en el fichero rutaFicheroZip.zip</param>
		/// <param name="mostrarInformacionDetallada">Muestra información detallada sobre el archivo que está siendo comprimido. Realmente no se muestra nada, sino que para cada fichero procesado (en este caso, sólo uno) se ejecuta el manejadorFuncionProcesaFicheros. Si vale false, el manejador que ejecuta puede ser nulo (es decir, el siguiente parámetro puede ser nulo)</param>
		/// <param name="manejadorFuncionProcesaFicheros">Función del tipo "private void ProcesarFichero(object sender, ScanEventArgs e)" que se ejecuta cada vez que se procesa un fichero (en este caso, sólo uno) siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <param name="mostrarProgreso">Indica si debemos mostrar el progreso de cada uno de los ficheros comprimidos (en este caso, sólo uno, por lo que se trata del progreso global de compresión). Realmente no muestra nada, sino que ejecuta en cada tick la función manejadorFuncionProgreso. Si vale false, el manejador que ejecuta puede ser nulo, y el intervalo puede ser cualquier valor, puesto que no se utiliza</param>
		/// <param name="intervaloProgreso">Indica cada cuanto tiempo se invocará la función manejadorFuncionProgreso, en milisegundos</param>
		/// <param name="manejadorFuncionProgreso">Función del tipo "void MostrarProgreso(object sender, ProgressEventArgs e)" que se ejecuta cada vez que hace tick el intervalo del progreso</param>
		/// <param name="contraseña">Contraseña para extraer el fichero comprimido</param>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirFicheroZip(string rutaFicheroZip, string rutaFicheroOrigen,
			bool mostrarInformacionDetallada, ProcessFileHandler manejadorFuncionProcesaFicheros, 
			bool mostrarProgreso, int intervaloProgreso, ProgressHandler manejadorFuncionProgreso, string contraseña)
		{
			try
			{
				bool comprimirDirectoriosVacios = false;
				bool compresionRecursiva = false;
				FileInfo informacionFicheroOrigen;
				FileInfo informacionFicheroZip;

				if (rutaFicheroZip.EndsWith(Path.DirectorySeparatorChar.ToString())) 
				{
					rutaFicheroZip = rutaFicheroZip.Remove(rutaFicheroZip.Length - 1);
				}

				if (!rutaFicheroZip.ToLower().EndsWith(OCompresionFicheros.ObtenerExtensionMasComun(ExtensionesFicheroZip)))
				{
					rutaFicheroZip += OCompresionFicheros.ObtenerExtensionMasComun(ExtensionesFicheroZip);
				}

				informacionFicheroOrigen = new FileInfo(rutaFicheroOrigen);
				informacionFicheroZip = new FileInfo(rutaFicheroZip);

				if (informacionFicheroZip.Name == informacionFicheroOrigen.Name //Los nombres de los ficheros son los mismos...
					&& informacionFicheroZip.Directory.FullName == informacionFicheroOrigen.Directory.FullName) //...y los directorios son los mismos
				{
					//No se puede realizar la compresión
					return new ReturnInfo(false, new OCompresionFicherosExcepcionBase("No se puede realizar la compresión porque el nombre del fichero" + Environment.NewLine + "que quiere comprimir coincide con el nombre fichero a comprimir."));
				}

				EventosOrbitaZip events = null;

				if (mostrarInformacionDetallada || mostrarProgreso)
				{
					events = new EventosOrbitaZip();
				}

				if (mostrarInformacionDetallada)
				{
					events.ProcessFile = new ProcessFileHandler(manejadorFuncionProcesaFicheros);
				}

				if (mostrarProgreso)
				{
					events.Progress = new ProgressHandler(manejadorFuncionProgreso);
					events.ProgressInterval = TimeSpan.FromMilliseconds(intervaloProgreso);
				}

				using (OrbitaZip compresionZip = new OrbitaZip(events))
				{
					compresionZip.CreateEmptyDirectories = comprimirDirectoriosVacios;
					compresionZip.RestoreAttributesOnExtract = true;

					if (contraseña != null && contraseña != string.Empty)
					{
						compresionZip.Password = contraseña;
					}

					compresionZip.CreateZip(rutaFicheroZip, informacionFicheroOrigen.Directory.FullName, compresionRecursiva, Regex.Escape(Path.DirectorySeparatorChar + informacionFicheroOrigen.Name));

					return new ReturnInfo();
				}
			}
			catch (System.Exception ex)
			{
				return new ReturnInfo(false, ex);
			}
		}
		/// <summary>
		/// Comprime un directorio en un archivo .zip
		/// </summary>
		/// <param name="rutaFicheroComprimido">Nombre o ruta (absoluta o relativa) del fichero .zip a generar o ruta del mismo, con o sin extensión .zip</param>
		/// <param name="rutaFicheroOrigen">Nombre o ruta (absoluta o relativa) del fichero a comprimir en el fichero rutaFicheroZip.zip</param>
		/// <param name="mostrarInformacionDetallada">Muestra información detallada sobre el archivo que está siendo comprimido. Realmente no se muestra nada, sino que para cada fichero procesado (en este caso, sólo uno) se ejecuta el manejadorFuncionProcesaFicheros. Si vale false, el manejador que ejecuta puede ser nulo (es decir, el siguiente parámetro puede ser nulo)</param>
		/// <param name="manejadorFuncionProcesaFicheros">Función del tipo "private void ProcesarFichero(object sender, ScanEventArgs e)" que se ejecuta cada vez que se procesa un fichero (en este caso, sólo uno) siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <param name="mostrarProgreso">Indica si debemos mostrar el progreso de cada uno de los ficheros comprimidos (en este caso, sólo uno, por lo que se trata del progreso global de compresión). Realmente no muestra nada, sino que ejecuta en cada tick la función manejadorFuncionProgreso. Si vale false, el manejador que ejecuta puede ser nulo, y el intervalo puede ser cualquier valor, puesto que no se utiliza</param>
		/// <param name="intervaloProgreso">Indica cada cuanto tiempo se invocará la función manejadorFuncionProgreso, en milisegundos</param>
		/// <param name="manejadorFuncionProgreso">Función del tipo "void MostrarProgreso(object sender, ProgressEventArgs e)" que se ejecuta cada vez que hace tick el intervalo del progreso</param>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirFicheroZip(string rutaFicheroZip, string rutaFicheroOrigen,
			bool mostrarInformacionDetallada, ProcessFileHandler manejadorFuncionProcesaFicheros,
			bool mostrarProgreso, int intervaloProgreso, ProgressHandler manejadorFuncionProgreso)
		{
			return OCompresionFicheros.ComprimirFicheroZip(rutaFicheroZip, rutaFicheroOrigen, mostrarInformacionDetallada,
				manejadorFuncionProcesaFicheros, mostrarProgreso, intervaloProgreso, manejadorFuncionProgreso, null);
		}
		/// <summary>
		/// Comprime un directorio en un archivo .zip
		/// </summary>
		/// <param name="rutaFicheroComprimido">Nombre o ruta (absoluta o relativa) del fichero .zip a generar o ruta del mismo, con o sin extensión .zip</param>
		/// <param name="rutaFicheroOrigen">Nombre o ruta (absoluta o relativa) del fichero a comprimir en el fichero rutaFicheroZip.zip</param>
		/// <param name="mostrarInformacionDetallada">Muestra información detallada sobre el archivo que está siendo comprimido. Realmente no se muestra nada, sino que para cada fichero procesado (en este caso, sólo uno) se ejecuta el manejadorFuncionProcesaFicheros. Si vale false, el manejador que ejecuta puede ser nulo (es decir, el siguiente parámetro puede ser nulo)</param>
		/// <param name="manejadorFuncionProcesaFicheros">Función del tipo "private void ProcesarFichero(object sender, ScanEventArgs e)" que se ejecuta cada vez que se procesa un fichero (en este caso, sólo uno) siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <param name="contraseña">Contraseña para extraer el fichero comprimido</param>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirFicheroZip(string rutaFicheroZip, string rutaFicheroOrigen,
			bool mostrarInformacionDetallada, ProcessFileHandler manejadorFuncionProcesaFicheros, string contraseña)
		{
			return OCompresionFicheros.ComprimirFicheroZip(rutaFicheroZip, rutaFicheroOrigen, mostrarInformacionDetallada,
				manejadorFuncionProcesaFicheros, false, 0, null, contraseña);
		}
		/// <summary>
		/// Comprime un directorio en un archivo .zip
		/// </summary>
		/// <param name="rutaFicheroComprimido">Nombre o ruta (absoluta o relativa) del fichero .zip a generar o ruta del mismo, con o sin extensión .zip</param>
		/// <param name="rutaFicheroOrigen">Nombre o ruta (absoluta o relativa) del fichero a comprimir en el fichero rutaFicheroZip.zip</param>
		/// <param name="mostrarInformacionDetallada">Muestra información detallada sobre el archivo que está siendo comprimido. Realmente no se muestra nada, sino que para cada fichero procesado (en este caso, sólo uno) se ejecuta el manejadorFuncionProcesaFicheros. Si vale false, el manejador que ejecuta puede ser nulo (es decir, el siguiente parámetro puede ser nulo)</param>
		/// <param name="manejadorFuncionProcesaFicheros">Función del tipo "private void ProcesarFichero(object sender, ScanEventArgs e)" que se ejecuta cada vez que se procesa un fichero (en este caso, sólo uno) siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirFicheroZip(string rutaFicheroZip, string rutaFicheroOrigen,
			bool mostrarInformacionDetallada, ProcessFileHandler manejadorFuncionProcesaFicheros)
		{
			return OCompresionFicheros.ComprimirFicheroZip(rutaFicheroZip, rutaFicheroOrigen, mostrarInformacionDetallada,
				manejadorFuncionProcesaFicheros, false, 0, null, null);
		}
		/// <summary>
		/// Comprime un directorio en un archivo .zip
		/// </summary>
		/// <param name="rutaFicheroComprimido">Nombre o ruta (absoluta o relativa) del fichero .zip a generar o ruta del mismo, con o sin extensión .zip</param>
		/// <param name="rutaFicheroOrigen">Nombre o ruta (absoluta o relativa) del fichero a comprimir en el fichero rutaFicheroZip.zip</param>
		/// <param name="mostrarProgreso">Indica si debemos mostrar el progreso de cada uno de los ficheros comprimidos (en este caso, sólo uno, por lo que se trata del progreso global de compresión). Realmente no muestra nada, sino que ejecuta en cada tick la función manejadorFuncionProgreso. Si vale false, el manejador que ejecuta puede ser nulo, y el intervalo puede ser cualquier valor, puesto que no se utiliza</param>
		/// <param name="intervaloProgreso">Indica cada cuanto tiempo se invocará la función manejadorFuncionProgreso, en milisegundos</param>
		/// <param name="manejadorFuncionProgreso">Función del tipo "void MostrarProgreso(object sender, ProgressEventArgs e)" que se ejecuta cada vez que hace tick el intervalo del progreso</param>
		/// <param name="contraseña">Contraseña para extraer el fichero comprimido</param>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirFicheroZip(string rutaFicheroZip, string rutaFicheroOrigen,
			bool mostrarProgreso, int intervaloProgreso, ProgressHandler manejadorFuncionProgreso, string contraseña)
		{
			return OCompresionFicheros.ComprimirFicheroZip(rutaFicheroZip, rutaFicheroOrigen, false,
				null, mostrarProgreso, intervaloProgreso, manejadorFuncionProgreso, contraseña);
		}
		/// <summary>
		/// Comprime un directorio en un archivo .zip
		/// </summary>
		/// <param name="rutaFicheroComprimido">Nombre o ruta (absoluta o relativa) del fichero .zip a generar o ruta del mismo, con o sin extensión .zip</param>
		/// <param name="rutaFicheroOrigen">Nombre o ruta (absoluta o relativa) del fichero a comprimir en el fichero rutaFicheroZip.zip</param>
		/// <param name="mostrarProgreso">Indica si debemos mostrar el progreso de cada uno de los ficheros comprimidos (en este caso, sólo uno, por lo que se trata del progreso global de compresión). Realmente no muestra nada, sino que ejecuta en cada tick la función manejadorFuncionProgreso. Si vale false, el manejador que ejecuta puede ser nulo, y el intervalo puede ser cualquier valor, puesto que no se utiliza</param>
		/// <param name="intervaloProgreso">Indica cada cuanto tiempo se invocará la función manejadorFuncionProgreso, en milisegundos</param>
		/// <param name="manejadorFuncionProgreso">Función del tipo "void MostrarProgreso(object sender, ProgressEventArgs e)" que se ejecuta cada vez que hace tick el intervalo del progreso</param>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirFicheroZip(string rutaFicheroZip, string rutaFicheroOrigen,
			bool mostrarProgreso, int intervaloProgreso, ProgressHandler manejadorFuncionProgreso)
		{
			return OCompresionFicheros.ComprimirFicheroZip(rutaFicheroZip, rutaFicheroOrigen, false,
				null, mostrarProgreso, intervaloProgreso, manejadorFuncionProgreso, null);
		}
		/// <summary>
		/// Comprime un directorio en un archivo .zip
		/// </summary>
		/// <param name="rutaFicheroComprimido">Nombre o ruta (absoluta o relativa) del fichero .zip a generar o ruta del mismo, con o sin extensión .zip</param>
        /// <param name="rutaFicheroOrigen">Función del tipo "void MostrarProgreso(object sender, ProgressEventArgs e)" que se ejecuta cada vez que hace tick el intervalo del progreso</param>
		/// <param name="contraseña">Contraseña para extraer el fichero comprimido</param>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirFicheroZip(string rutaFicheroZip, string rutaFicheroOrigen, string contraseña)
		{
			return OCompresionFicheros.ComprimirFicheroZip(rutaFicheroZip, rutaFicheroOrigen, false,
				null, false, 0, null, contraseña);
		}
		/// <summary>
		/// Comprime un directorio en un archivo .zip
		/// </summary>
		/// <param name="rutaFicheroComprimido">Nombre o ruta (absoluta o relativa) del fichero .zip a generar o ruta del mismo, con o sin extensión .zip</param>
		/// <param name="rutaFicheroOrigen">Nombre o ruta (absoluta o relativa) del fichero a comprimir en el fichero rutaFicheroZip.zip</param>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirFicheroZip(string rutaFicheroZip, string rutaFicheroOrigen)
		{
			return OCompresionFicheros.ComprimirFicheroZip(rutaFicheroZip, rutaFicheroOrigen, false,
				null, false, 0, null, null);
		}
			#endregion Compresión en zip de ficheros

			#region Descompresión en zip

		/// <summary>
		/// Comprime un directorio en un archivo .zip
		/// </summary>
		/// <param name="rutaFicheroComprimido">Nombre del fichero .zip a descomprimir</param>
		/// <param name="rutaDirectorioDestino">Ruta con el directorio donde se extraerán el contenido del fichero comprimido</param>
		/// <param name="mostrarInformacionDetallada">Muestra información detallada sobre los archivos y directorios que están siendo descomprimidos. Realmente no se muestra nada, sino que para cada fichero se ejecuta el manejadorFuncionProcesaFicheros y para cada diractorio se ejecuta el manejadorFuncionProcesaDirectorios</param>
		/// <param name="manejadorFuncionProcesaFicheros">Función del tipo "private void ProcesarFichero(object sender, ScanEventArgs e)" que se ejecuta cada vez que se procesa un fichero siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <param name="manejadorFuncionProcesaDirectorios">Función del tipo "private void ProcesarDirectorio(object sender, DirectoryEventArgs e)" que se ejecuta cada vez que se procesa un directorio siempre que mostrarInformacionDetallada sea verdadero</param>
		/// <param name="mostrarProgreso">Indica si debemos mostrar el progreso de la descompresión. Realmente no muestra nada, sino que ejecuta en cada tick la función manejadorFuncionProgreso</param>
		/// <param name="intervaloProgreso">Indica cada cuanto tiempo se invocará la función manejadorFuncionProgreso, en milisegundos</param>
		/// <param name="manejadorFuncionProgreso">Función del tipo "void MostrarProgreso(object sender, ProgressEventArgs e)" que se ejecuta cada vez que hace tick el intervalo del progreso</param>
		/// <param name="sobreescritura">Enumerado del tipo OrbitaZip.Sobreescritura para indicar si sobreescribir siempre, nunca o preguntar si se encuentra un fichero existente en a ruta de descompresión. ATENCIÓN: La sobreescritura de un archivo con sus atributos modificados (por ejemplo, fichero de solo lectura o fichero oculto) da error. No se ha intentado solucionar por falta de tiempo y por no tener la necesidad de hacerlo</param>
		/// <param name="manejadorFuncionPreguntarSobreescritura">Función del tipo "private bool ConfirmarSobreescritura(string rutaArchivo)" que se ejecuta cada vez que se encuentra un fichero que ya existe en la ruta de descompresión, siempre que el valor de sobreescritura sea OrbitaZip.Sobreescritura.Preguntar ATENCIÓN: La sobreescritura de un archivo con sus atributos modificados (por ejemplo, fichero de solo lectura o fichero oculto) da error. No se ha intentado solucionar por falta de tiempo y por no tener la necesidad de hacerlo</param>
		/// <param name="restaurarFechas">Las fechas y horas de los ficheros se restaurarán al extraer</param>
		/// <param name="restaurarAtributos">Los atributos de los ficheros se restaurarán al extraer</param>
		/// <param name="descomprimirDirectoriosVacios">Descomprime los directorios vacíos</param>
		/// <param name="contraseña">Contraseña para extraer el fichero comprimido</param>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo DescomprimirFicheroZip(string rutaFicheroZip,  string rutaDirectorioDestino,
			bool mostrarInformacionDetallada, ProcessFileHandler manejadorFuncionProcesaFicheros, ProcessDirectoryHandler manejadorFuncionProcesaDirectorios,
			bool mostrarProgreso, int intervaloProgreso, ProgressHandler manejadorFuncionProgreso,
			OrbitaZip.Sobreescritura sobreescritura, OrbitaZip.ConfirmOverwriteDelegate manejadorFuncionPreguntarSobreescritura,
			bool restaurarFechas, bool restaurarAtributos, bool descomprimirDirectoriosVacios, string contraseña)
		{
			try
			{
				FileInfo informacionFicheroZip;
				DirectoryInfo informaciónDirectorio;

				if (rutaFicheroZip.EndsWith(Path.DirectorySeparatorChar.ToString()))
				{
					rutaFicheroZip = rutaFicheroZip.Remove(rutaFicheroZip.Length - 1);
				}

				if (!rutaFicheroZip.ToLower().EndsWith(OCompresionFicheros.ObtenerExtensionMasComun(ExtensionesFicheroZip)))
				{
					rutaFicheroZip += OCompresionFicheros.ObtenerExtensionMasComun(ExtensionesFicheroZip);
				}

				informacionFicheroZip = new FileInfo(rutaFicheroZip);
				informaciónDirectorio = new DirectoryInfo(rutaDirectorioDestino);

				OrbitaZip.ConfirmOverwriteDelegate delegadoConfirmacionSoobreescritura = null;
				switch (sobreescritura)
				{
					case OrbitaZip.Sobreescritura.Siempre:
						delegadoConfirmacionSoobreescritura = null;
						break;
					case OrbitaZip.Sobreescritura.Nunca:
						delegadoConfirmacionSoobreescritura = null;
						break;
					case OrbitaZip.Sobreescritura.Preguntar:
						delegadoConfirmacionSoobreescritura = new OrbitaZip.ConfirmOverwriteDelegate(manejadorFuncionPreguntarSobreescritura);
						break;
				}

				EventosOrbitaZip events = null;
				if (mostrarInformacionDetallada || mostrarProgreso)
				{
					events = new EventosOrbitaZip();
				}

				if (mostrarInformacionDetallada)
				{
					events.ProcessDirectory = new ProcessDirectoryHandler(manejadorFuncionProcesaDirectorios);
					events.ProcessFile = new ProcessFileHandler(manejadorFuncionProcesaFicheros);
				}

				if (mostrarProgreso)
				{
					events.Progress = new ProgressHandler(manejadorFuncionProgreso);
					events.ProgressInterval = TimeSpan.FromMilliseconds(intervaloProgreso);
				}

				using (OrbitaZip compresionZip = new OrbitaZip(events))
				{
					compresionZip.CreateEmptyDirectories = descomprimirDirectoriosVacios;
					compresionZip.RestoreAttributesOnExtract = restaurarAtributos;
					compresionZip.RestoreDateTimeOnExtract = restaurarFechas;
					compresionZip.Password = contraseña;

					compresionZip.ExtractZip(informacionFicheroZip.FullName, informaciónDirectorio.FullName, sobreescritura, delegadoConfirmacionSoobreescritura, null, null, restaurarFechas);
					
					return new ReturnInfo();
				}
			}
			catch (System.Exception ex)
			{
				return new ReturnInfo(false, ex);
			}
		}
			#endregion Descompresión en zip

		#endregion zip

		#region tar
		//TODO: Ver los ejemplos de compresión y descompresión de SharpZipLib

			#region Atributo/s
		/// <summary>
		/// Vector con las posibles extensiones de ficheros comprimidos con el algoritmo tar. Se incluye el punto que precede a las extensiones de ficheros
		/// </summary>
		private  static string[] ExtensionesFicheroTar = { ".tar" };
			#endregion Atributo/s

			#region Compresión en tar
		/// <summary>
		/// Comprime un directorio en un archivo .tar
		/// Método no implementado. Se tiene capacidad para comprimir ficheros usando el algoritmo .tar, pero por falta de necesidad (y de tiempo), se deja la posibilidad de hacerlo en un futuro
		/// </summary>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirDirectorioTar()
		{
			return new ReturnInfo(false, new OCompresionFicherosExcepcionBase("Orbita.Utiles.Compresion no tiene implementado el método de crear ficheros comprimidos del tipo " + ObtenerExtensionesDisponibles(ExtensionesFicheroTar) + ", aunque si que tiene la capacidad para hacerlo; sólo es necesario disponer de tiempo o que se cree la necesidad de hacerlo :-)"));
		}
		/// <summary>
		/// Comprime un fichero en un archivo .tar
		/// Método no implementado. Se tiene capacidad para comprimir ficheros usando el algoritmo .tar, pero por falta de necesidad (y de tiempo), se deja la posibilidad de hacerlo en un futuro
		/// </summary>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirFicheroTar()
		{
			return new ReturnInfo(false, new OCompresionFicherosExcepcionBase("Orbita.Utiles.Compresion no tiene implementado el método de crear ficheros comprimidos del tipo " + ObtenerExtensionesDisponibles(ExtensionesFicheroTar) + ", aunque si que tiene la capacidad para hacerlo; sólo es necesario disponer de tiempo o que se cree la necesidad de hacerlo :-)"));
		}
			#endregion Compresión en tar

			#region Descompresión en tar
		/// <summary>
		/// Descomprime un fichero en un archivo .tar
		/// Método no implementado. Se tiene capacidad para descomprimir ficheros usando el algoritmo .tar, pero por falta de necesidad (y de tiempo), se deja la posibilidad de hacerlo en un futuro
		/// </summary>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo DescomprimirFicheroTar()
		{
			return new ReturnInfo(false, new OCompresionFicherosExcepcionBase("Orbita.Utiles.Compresion no tiene implementado el método de descomprimir ficheros del tipo " + ObtenerExtensionesDisponibles(ExtensionesFicheroTar) + ", aunque si que tiene la capacidad para hacerlo; sólo es necesario disponer de tiempo o que se cree la necesidad de hacerlo :-)"));
		}
			#endregion Descompresión en tar

		#endregion tar

		#region lzw
		//TODO: Ver los ejemplos de compresión y descompresión de SharpZipLib

			#region Atributo/s
		/// <summary>
		/// Vector con las posibles extensiones de ficheros comprimidos con el algoritmo lzw. Se incluye el punto que precede a las extensiones de ficheros
		/// </summary>
		private static string[] ExtensionesFicheroLzm = { ".z", ".lzw" };
			#endregion Atributo/s

			#region Compresión en lzw
		/// <summary>
		/// Comprime un directorio en un archivo .lzw
		/// Método no implementado. Se tiene capacidad para comprimir ficheros usando el algoritmo .lzw pero por falta de necesidad (y de tiempo), se deja la posibilidad de hacerlo en un futuro
		/// </summary>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirDirectorioLzm()
		{
			return new ReturnInfo(false, new OCompresionFicherosExcepcionBase("Orbita.Utiles.Compresion no tiene implementado el método de crear ficheros comprimidos del tipo " + ObtenerExtensionesDisponibles(ExtensionesFicheroLzm) + ", aunque si que tiene la capacidad para hacerlo; sólo es necesario disponer de tiempo o que se cree la necesidad de hacerlo :-)"));
		}
		/// <summary>
		/// Comprime un fichero en un archivo .lzw
		/// Método no implementado. Se tiene capacidad para comprimir ficheros usando el algoritmo .lzw, pero por falta de necesidad (y de tiempo), se deja la posibilidad de hacerlo en un futuro
		/// </summary>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirFicheroLzm()
		{
			return new ReturnInfo(false, new OCompresionFicherosExcepcionBase("Orbita.Utiles.Compresion no tiene implementado el método de crear ficheros comprimidos del tipo " + ObtenerExtensionesDisponibles(ExtensionesFicheroLzm) + ", aunque si que tiene la capacidad para hacerlo; sólo es necesario disponer de tiempo o que se cree la necesidad de hacerlo :-)"));
		}
			#endregion Compresión en lzw

			#region Descompresión en lzw
		/// <summary>
		/// Descomprime un fichero en un archivo .lzw
		/// Método no implementado. Se tiene capacidad para descomprimir ficheros usando el algoritmo .lzw, pero por falta de necesidad (y de tiempo), se deja la posibilidad de hacerlo en un futuro
		/// </summary>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo DescomprimirFicheroLzm()
		{
			return new ReturnInfo(false, new OCompresionFicherosExcepcionBase("Orbita.Utiles.Compresion no tiene implementado el método de descomprimir ficheros del tipo " + ObtenerExtensionesDisponibles(ExtensionesFicheroLzm) + ", aunque si que tiene la capacidad para hacerlo; sólo es necesario disponer de tiempo o que se cree la necesidad de hacerlo :-)"));
		}
			#endregion Descompresión en lzw

		#endregion lzw

		#region gzip
		//TODO: Ver los ejemplos de compresión y descompresión de SharpZipLib

			#region Atributo/s
		/// <summary>
		/// Vector con las posibles extensiones de ficheros comprimidos con el algoritmo gzip. Se incluye el punto que precede a las extensiones de ficheros
		/// </summary>
		private static string[] ExtensionesFicheroGzip = { ".gz", ".tar.gz", ".tgz" };
			#endregion Atributo/s

			#region Compresión en gzip
		/// <summary>
		/// Comprime un directorio en un archivo .gzip
		/// Método no implementado. Se tiene capacidad para comprimir ficheros usando el algoritmo .gzip pero por falta de necesidad (y de tiempo), se deja la posibilidad de hacerlo en un futuro
		/// </summary>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirDirectorioGzip()
		{
			return new ReturnInfo(false, new OCompresionFicherosExcepcionBase("Orbita.Utiles.Compresion no tiene implementado el método de crear ficheros comprimidos del tipo " + ObtenerExtensionesDisponibles(ExtensionesFicheroGzip) + ", aunque si que tiene la capacidad para hacerlo; sólo es necesario disponer de tiempo o que se cree la necesidad de hacerlo :-)"));
		}
		/// <summary>
		/// Comprime un fichero en un archivo .gzip
		/// Método no implementado. Se tiene capacidad para comprimir ficheros usando el algoritmo .gzip, pero por falta de necesidad (y de tiempo), se deja la posibilidad de hacerlo en un futuro
		/// </summary>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirFicheroGzip()
		{
			return new ReturnInfo(false, new OCompresionFicherosExcepcionBase("Orbita.Utiles.Compresion no tiene implementado el método de crear ficheros comprimidos del tipo " + ObtenerExtensionesDisponibles(ExtensionesFicheroGzip) + ", aunque si que tiene la capacidad para hacerlo; sólo es necesario disponer de tiempo o que se cree la necesidad de hacerlo :-)"));
		}
			#endregion Compresión en gzip

			#region Descompresión en gzip
		/// <summary>
		/// Descomprime un fichero en un archivo .gzip
		/// Método no implementado. Se tiene capacidad para descomprimir ficheros usando el algoritmo .gzip, pero por falta de necesidad (y de tiempo), se deja la posibilidad de hacerlo en un futuro
		/// </summary>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo DescomprimirFicheroGzip()
		{
			return new ReturnInfo(false, new OCompresionFicherosExcepcionBase("Orbita.Utiles.Compresion no tiene implementado el método de descomprimir ficheros del tipo " + ObtenerExtensionesDisponibles(ExtensionesFicheroGzip) + ", aunque si que tiene la capacidad para hacerlo; sólo es necesario disponer de tiempo o que se cree la necesidad de hacerlo :-)"));
		}
			#endregion Descompresión en gzip

		#endregion gzip

		#region bzip2
		//TODO: Ver los ejemplos de compresión y descompresión de SharpZipLib

			#region Atributo/s
		/// <summary>
		/// Vector con las posibles extensiones de ficheros comprimidos con el algoritmo bzip2. Se incluye el punto que precede a las extensiones de ficheros
		/// </summary>
		private static string[] ExtensionesFicheroBZip2 = { ".bz2", ".tar.bz2", ".tbz2", ".tb2", ".bzip2" };
			#endregion Atributo/s

			#region Compresión en bzip2
		/// <summary>
		/// Comprime un directorio en un archivo .bzip2
		/// Método no implementado. Se tiene capacidad para comprimir ficheros usando el algoritmo .bzip2 pero por falta de necesidad (y de tiempo), se deja la posibilidad de hacerlo en un futuro
		/// </summary>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirDirectorioBZip2()
		{
			return new ReturnInfo(false, new OCompresionFicherosExcepcionBase("Orbita.Utiles.Compresion no tiene implementado el método de crear ficheros comprimidos del tipo " + ObtenerExtensionesDisponibles(ExtensionesFicheroBZip2) + ", aunque si que tiene la capacidad para hacerlo; sólo es necesario disponer de tiempo o que se cree la necesidad de hacerlo :-)"));
		}
		/// <summary>
		/// Comprime un fichero en un archivo .bzip2
		/// Método no implementado. Se tiene capacidad para comprimir ficheros usando el algoritmo .bzip2, pero por falta de necesidad (y de tiempo), se deja la posibilidad de hacerlo en un futuro
		/// </summary>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo ComprimirFicheroBZip2()
		{
			return new ReturnInfo(false, new OCompresionFicherosExcepcionBase("Orbita.Utiles.Compresion no tiene implementado el método de crear ficheros comprimidos del tipo " + ObtenerExtensionesDisponibles(ExtensionesFicheroBZip2) + ", aunque si que tiene la capacidad para hacerlo; sólo es necesario disponer de tiempo o que se cree la necesidad de hacerlo :-)"));
		}
			#endregion Compresión en bzip2

			#region Descompresión en bzip2
		/// <summary>
		/// Descomprime un fichero en un archivo .bzip2
		/// Método no implementado. Se tiene capacidad para descomprimir ficheros usando el algoritmo .bzip2, pero por falta de necesidad (y de tiempo), se deja la posibilidad de hacerlo en un futuro
		/// </summary>
		/// <returns>Un objeto Returninfo con la información sobre la ejecución del método</returns>
		public static ReturnInfo DescomprimirFicheroBZip2()
		{
			return new ReturnInfo(false, new OCompresionFicherosExcepcionBase("Orbita.Utiles.Compresion no tiene implementado el método de descomprimir ficheros del tipo " + ObtenerExtensionesDisponibles(ExtensionesFicheroBZip2) + ", aunque si que tiene la capacidad para hacerlo; sólo es necesario disponer de tiempo o que se cree la necesidad de hacerlo :-)"));
		}
			#endregion Descompresión en bzip2

		#endregion bzip2
	}
}

