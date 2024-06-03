//***********************************************************************
// Assembly         : Orbita.VAComun
// Author           : aibañez
// Created          : 16-11-2012
//
// Last Modified By : aibañez
// Last Modified On : 12-12-2012
// Description      : Creados objetos para el trabajo con CGI
//
//      Modified By : aibañez
//      Modified On : 16-11-2012
// Description      : Extraido delegado genérico SimpleMethod fuera de App
//                    Añadidos delegados genérico MessageDelegate y ExceptionDelegate
//                    Añadida función ColectorBasura para centralizar las llamadas al GarbageCollector de .Net
//
//      Modified By : aibañez
//      Modified On : 05-11-2012
// Description      : Añadido un método sobrecargado de Espera donde se le pasa como argumento un delegado
//                    Nuevo método para reemplazar strings especificando el tipo de comparación
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Orbita.Utiles;
                                                            
namespace Orbita.VAComun
{
	#region Clase estática App: Contiene métodos genéricos
	/// <summary>
	/// Clase estática con funciones de uso general
	/// </summary>
	public static class App
	{
		#region Atributo(s) de la clase
        /// <summary>
        /// Guarda el vínculo con el formulario principal de la aplicación
        /// </summary>
        public static Form FormularioPrincipal;
		/// <summary>
		/// Lista de parámetros de entrada de la aplicación
		/// </summary>
		public static string[] ListaParametrosEntradaAplicacion;
		#endregion Campos de la clase

		#region Método(s) de trabajo con ficheros
		/// <summary>
		/// Se comprueba la existencia del directorio y si no existe se intenta crear
		/// </summary>
		/// <param name="ruta"></param>
		/// <returns></returns>
		public static bool CreacionDirectorio(string ruta)
		{
			try
			{
				if (Path.IsPathRooted(ruta))
				{
					if (Directory.Exists(ruta))
					{
						return true;
					}
					else
					{
						DirectoryInfo directoryInfo = Directory.CreateDirectory(ruta);
						if (directoryInfo.Exists)
						{
							return true;
						}
					}
				}
			}
			catch (Exception exception)
			{
				OVALogsManager.Error(ModulosSistema.Comun, "CreacionDirectorio", exception);
			}

			return false;
		}

		/// <summary>
		/// Devuelve una cadena de texto identificativa del día actual (utilizada para indexar ficheros)
		/// </summary>
		/// <returns></returns>
		public static string DateString(DateTime fecha)
		{
			string resultado = string.Empty;

			resultado += fecha.Year.ToString("0000");
			resultado += fecha.Month.ToString("00");
			resultado += fecha.Day.ToString("00");

			return resultado;
		}

		/// <summary>
		/// Devuelve una cadena de texto identificativa del momento actual (utilizada para indexar ficheros)
		/// </summary>
		/// <returns></returns>
		public static string DateTimeString(DateTime fecha)
		{
			string resultado = string.Empty;

			resultado += fecha.Year.ToString("0000");
			resultado += fecha.Month.ToString("00");
			resultado += fecha.Day.ToString("00");
			resultado += fecha.Hour.ToString("00");
			resultado += fecha.Minute.ToString("00");
			resultado += fecha.Second.ToString("00");
			resultado += fecha.Millisecond.ToString("000");

			return resultado;
		}

		/// <summary>
		/// Calcula el tamaño de un directorio
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public static long GetDirectorySize(string ruta, SearchOption searchOption)
		{
			// Get array of all file names.
			string[] files = Directory.GetFiles(ruta, "*.*", searchOption);

			// Calculate total bytes of all files in a loop.
			long totalSize = 0;
			foreach (string name in files)
			{
				// Use FileInfo to get length of each file.
				FileInfo info = new FileInfo(name);
				totalSize += info.Length;
			}
			// Return total size
			return totalSize;
		}

		/// <summary>
		/// Copia una carpeta
		/// </summary>
		/// <param name="sourceDirName">Ruta origen</param>
		/// <param name="destDirName">Ruta destino</param>
		/// <param name="copySubDirs">Opciones de la copi</param>
		public static bool DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool overwrite)
		{
			bool resultado = false;
			try
			{
				DirectoryInfo dir = new DirectoryInfo(sourceDirName);
				DirectoryInfo[] dirs = dir.GetDirectories();

				// If the source directory does not exist, throw an exception.
				if (dir.Exists)
				{
					// If the destination directory does not exist, create it.
					if (!Directory.Exists(destDirName))
					{
						Directory.CreateDirectory(destDirName);
					}

					// Get the file contents of the directory to copy.
					FileInfo[] files = dir.GetFiles();

					foreach (FileInfo file in files)
					{
						// Create the path to the new copy of the file.
						string temppath = Path.Combine(destDirName, file.Name);

						// Copy the file.
						if (overwrite || !File.Exists(temppath))
						{
							file.CopyTo(temppath, overwrite);
						}
					}

					// If copySubDirs is true, copy the subdirectories.
					if (copySubDirs)
					{
						foreach (DirectoryInfo subdir in dirs)
						{
							// Create the subdirectory.
							string temppath = Path.Combine(destDirName, subdir.Name);

							// Copy the subdirectories.
							DirectoryCopy(subdir.FullName, temppath, copySubDirs, overwrite);
						}
					}

					resultado = true;
				}
			}
			catch (Exception exception)
			{
				OVALogsManager.Error(ModulosSistema.Comun, "DirectoryCopy", exception);
			}
			return resultado;
		}

		/// <summary>
		/// Mueve una carpeta
		/// </summary>
		/// <param name="sourceDirName">Ruta origen</param>
		/// <param name="destDirName">Ruta destino</param>
		/// <param name="copySubDirs">Opciones de la copi</param>
		public static bool DirectoryMove(string sourceDirName, string destDirName, bool moveSubDirs)
		{
			bool resultado = false;
			try
			{
				DirectoryInfo dir = new DirectoryInfo(sourceDirName);
				DirectoryInfo[] dirs = dir.GetDirectories();

				// If the source directory does not exist, throw an exception.
				if (dir.Exists)
				{
					// Get the file contents of the directory to copy.
					FileInfo[] files = dir.GetFiles();

					foreach (FileInfo file in files)
					{
						if (!Directory.Exists(destDirName))
						{
							Directory.CreateDirectory(destDirName);
						}
						// Create the path to the new copy of the file.
						string temppath = Path.Combine(destDirName, file.Name);

						// Move the file.
						file.MoveTo(temppath);
					}

					// If copySubDirs is true, copy the subdirectories.
					if (moveSubDirs)
					{
						foreach (DirectoryInfo subdir in dirs)
						{
							// Create the subdirectory.
							string temppath = Path.Combine(destDirName, subdir.Name);

							// Move the subdirectories.
							DirectoryMove(subdir.FullName, temppath, moveSubDirs);
						}
					}

					resultado = true;
				}
			}
			catch (Exception exception)
			{
				OVALogsManager.Error(ModulosSistema.Comun, "DirectoryMove", exception);
			}
			return resultado;
		}

		/// <summary>
		/// Copia una carpeta
		/// </summary>
		/// <param name="sourceFileName">Ruta origen</param>
		/// <param name="destFileName">Ruta destino</param>
		/// <param name="ImageFormat">Formato de la imagen</param>
		/// <param name="quality">calidad</param>
		public static bool ConversionFicheroImagen(string sourceFileName, string destFileName, ImageFormat imageFormat, long quality)
		{
			bool resultado = false;
			try
			{
				// Codec
				ImageCodecInfo selectedCodec = null;
				bool codecFound = false;
				ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
				foreach (ImageCodecInfo codec in codecs)
				{
					if (codec.FormatID == imageFormat.Guid)
					{
						selectedCodec = codec;
						codecFound = true;
						break;
					}
				}

				if (codecFound)
				{
					// Codec parameters
					EncoderParameters encoderParameters = new EncoderParameters(1);
					encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
					//encoderParameters.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, (long)EncoderValue.CompressionRle);

					using (Bitmap bmp = new Bitmap(sourceFileName))
					{
						bmp.Save(destFileName, selectedCodec, encoderParameters);
					}

					resultado = true;
				}
			}
			catch (Exception exception)
			{
				OVALogsManager.Error(ModulosSistema.Comun, "ConversionFicheroImagen", exception);
			}
			return resultado;
		}

		/// <summary>
		/// Indica si la ruta es local o remota
		/// </summary>
		/// <param name="ruta">Cadena de texto indicativa de la ruta</param>
		/// <param name="esLocal">Indica si la ruta es local o remota</param>
		/// <returns>Verdadero si se trata de una ruta válida</returns>
		public static bool Ruta2Uri(string ruta, out bool esLocal, out Uri uri)
		{
			bool resultado = false;
			esLocal = true;

			// Se identifica si se trta de una ruta local o remota
			resultado = Uri.TryCreate(ruta, UriKind.Absolute, out uri);

			if (resultado)
			{
				esLocal &= !uri.IsUnc;
			}

			return resultado;
		}
		#endregion

        #region Trabajo con cadenas de texto
        /// <summary>
        /// Case insensitive version of String.Replace().
        /// </summary>
        /// <param name="s">String that contains patterns to replace</param>
        /// <param name="oldValue">Pattern to find</param>
        /// <param name="newValue">New pattern to replaces old</param>
        /// <param name="comparisonType">String comparison type</param>
        /// <returns></returns>
        public static string StringReplace(string s, string oldValue, string newValue, StringComparison comparisonType)
        {
            if (s == null)
                return null;

            if (String.IsNullOrEmpty(oldValue))
                return s;

            StringBuilder result = new StringBuilder(Math.Min(4096, s.Length));
            int pos = 0;

            while (true)
            {
                int i = s.IndexOf(oldValue, pos, comparisonType);
                if (i < 0)
                    break;

                result.Append(s, pos, i - pos);
                result.Append(newValue);

                pos = i + oldValue.Length;
            }
            result.Append(s, pos, s.Length - pos);

            return result.ToString();
        }
        #endregion

		#region Métodos Sistema
		/// <summary>
		/// Obtiene la version del ensamblado actual
		/// </summary>
		/// <param name="asm">Ensamblado del cual se quiere conocer la versión</param>
		/// <returns>Versión del ensamblado</returns>
		public static string ObtenerVersion(System.Reflection.Assembly asm)
		{
			Version v = asm.GetName().Version;
			return (v.Major + "." + v.Minor + "." + v.Build + "." + v.Revision);
		}

		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool GetDiskFreeSpaceEx(string lpDirectoryName,
		out ulong lpFreeBytesAvailable,
		out ulong lpTotalNumberOfBytes,
		out ulong lpTotalNumberOfFreeBytes);

		/// <summary>
		/// Devuelve el espacio libre la unidad que contiene la ruta especificada en bytes , aunque sea de RED
		/// </summary>
		/// <param name="ruta"></param>
		/// <returns></returns>
		public static long AvailableFreeSpace(string ruta)
		{
			long space = 0;

			try
			{
				if (Path.IsPathRooted(ruta))
				{
					string root = Path.GetPathRoot(ruta);

					if (!ruta.Contains(@"\\"))
					{
						DriveInfo[] DI = DriveInfo.GetDrives();
						foreach (DriveInfo drive in DI)
						{
							if (drive.Name == root)
							{
								space = drive.AvailableFreeSpace;
							}
						}
					}
					else
					{
						ulong FreeBytesAvailable;
						ulong TotalNumberOfBytes;
						ulong TotalNumberOfFreeBytes;

						bool success = GetDiskFreeSpaceEx(root, out FreeBytesAvailable, out TotalNumberOfBytes,
										   out TotalNumberOfFreeBytes);
						space = (long)FreeBytesAvailable;
					}
				}
			}
			catch (Exception exception)
			{
				OVALogsManager.Error(ModulosSistema.Comun, "GetDiskSpace", exception);
			}

			return space;

		}
		#endregion

		#region Conversion de colecciones
		/// <summary>
		/// Convierte una colección en una cadena de texto
		/// </summary>
		/// <param name="coleccion">Colección que se desea convertir en texto</param>
		/// <param name="separador">Caracter separador de cada uno de los valores de la colección</param>
		/// <returns>Cadena de texto con todos los valores de la colección separados por el carácter separador</returns>
		public static string Colection2String(ICollection coleccion, char separador)
		{
			string resultado = string.Empty;

			foreach (object valor in coleccion)
			{
                resultado += ORobusto.ToString(valor) + separador;
			}

			return resultado;
		}

		/// <summary>
		/// Delegado para la conversión del tipo string a cualquier tipo
		/// </summary>
		/// <typeparam name="T">Tipo que se desea devolver</typeparam>
		/// <param name="text"></param>
		/// <returns></returns>
		public delegate T ConvertFromString<T>(string text);

		/// <summary>
		/// Convierte una cadena de texto en una colección
		/// </summary>
		/// <typeparam name="T">Tipo de colección que se desea devolver</typeparam>
		/// <param name="texto">Texto que se convertira en colección</param>
		/// <param name="separador">Caracter separador de cada uno de los valores de la colección</param>
		/// <returns>Objeto de tipo colección con todos los valores que contiene el texto separados por el carácter separador</returns>
		public static T String2Collection<T>(string texto, char separador, ConvertFromString<T> conversion)
			where T : IList, new()
		{
			T resultado = new T();

			string[] strArray = texto.Split(separador);
			foreach (string strValue in strArray)
			{
				resultado.Add(conversion(strValue));
			}

			return resultado;
		}
		#endregion

		#region Formularios de diálogo
		/// <summary>
		/// Muestra el formulario para la selección de un archivo
		/// </summary>
		/// <param name="p"></param>
		public static bool FormularioSeleccionArchivo(OpenFileDialog openFileDialog, ref string rutaArchivo)
		{
			bool resultado = false;
			bool existeRuta = false;

			if (Path.IsPathRooted(rutaArchivo)) // Si la ruta del archivo es válida
			{
				existeRuta = true;
				string rutaCarpeta = Path.GetDirectoryName(rutaArchivo);
				if (File.Exists(rutaArchivo)) // Si el archivo existe se selecciona por defecto
				{
					openFileDialog.FileName = rutaArchivo;
					openFileDialog.InitialDirectory = rutaCarpeta;
				}
				else // si el archivo no existe pero si su carpeta, esta será la carpeta inicial del formulario
				{
					if (Directory.Exists(rutaCarpeta))
					{
						openFileDialog.FileName = string.Empty;
						openFileDialog.InitialDirectory = rutaCarpeta;
					}
					else
					{
						existeRuta = false;
					}
				}
			}

			if (!existeRuta)
			{
				openFileDialog.FileName = string.Empty;
				openFileDialog.InitialDirectory = ORutaParametrizable.AppFolder;
			}

			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				if (File.Exists(openFileDialog.FileName))
				{
					rutaArchivo = openFileDialog.FileName;
					resultado = true;
				}
			}

			return resultado;
		}

		/// <summary>
		/// Muestra el formulario para guardar un archivo
		/// </summary>
		/// <param name="p"></param>
		public static bool FormularioGuardarArchivo(SaveFileDialog saveFileDialog, ref string rutaArchivo)
		{
			bool resultado = false;
			bool existeRuta = false;

			if (Path.IsPathRooted(rutaArchivo)) // Si la ruta del archivo es válida
			{
				existeRuta = true;
				string rutaCarpeta = Path.GetDirectoryName(rutaArchivo);
				if (File.Exists(rutaArchivo)) // Si el archivo existe se selecciona por defecto
				{
					saveFileDialog.FileName = rutaArchivo;
					saveFileDialog.InitialDirectory = rutaCarpeta;
				}
				else // si el archivo no existe pero si su carpeta, esta será la carpeta inicial del formulario
				{
					if (Directory.Exists(rutaCarpeta))
					{
						saveFileDialog.FileName = string.Empty;
						saveFileDialog.InitialDirectory = rutaCarpeta;
					}
					else
					{
						existeRuta = false;
					}
				}
			}

			if (!existeRuta)
			{
				saveFileDialog.FileName = string.Empty;
				saveFileDialog.InitialDirectory = ORutaParametrizable.AppFolder;
			}

			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				rutaArchivo = saveFileDialog.FileName;
				resultado = true;
			}

			return resultado;
		}

		/// <summary>
		/// Muestra el formulario para la selección de una carpeta
		/// </summary>
		/// <param name="p"></param>
		public static bool FormularioSeleccionCarpeta(FolderBrowserDialog folderBrowserDialog, ref string rutaCarpeta)
		{
			bool resultado = false;
			bool existeRuta = false;

			if (Path.IsPathRooted(rutaCarpeta)) // Si la ruta de la carpeta es válida
			{
				existeRuta = true;
				string rutaCarpetaAnterior = Path.GetDirectoryName(rutaCarpeta);
				if (Directory.Exists(rutaCarpeta)) // Si la carpeta existe se selecciona por defecto
				{
					folderBrowserDialog.SelectedPath = rutaCarpeta;
				}
				else // si el archivo no existe pero si su carpeta, esta será la carpeta inicial del formulario
				{
					if (Directory.Exists(rutaCarpetaAnterior))
					{
						folderBrowserDialog.SelectedPath = rutaCarpetaAnterior;
					}
					else
					{
						existeRuta = false;
					}
				}
			}

			if (!existeRuta)
			{
				folderBrowserDialog.SelectedPath = ORutaParametrizable.AppFolder;
			}

			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				if (Directory.Exists(folderBrowserDialog.SelectedPath))
				{
					rutaCarpeta = folderBrowserDialog.SelectedPath;
					resultado = true;
				}
			}

			return resultado;
		}
		#endregion

		#region Arrays
		/// <summary>
		/// Función para redimiendionar un Array
		/// </summary>
		/// <param name="orgArray">Array</param>
		/// <param name="tamaño">Tamaño</param>
		/// <returns>Array redimensionado</returns>

		public static Array aRedimensionar(Array orgArray, Int32 tamaño)
		{
			Type t = orgArray.GetType().GetElementType();
			Array nArray = Array.CreateInstance(t, tamaño);
			Array.Copy(orgArray, 0, nArray, 0, Math.Min(orgArray.Length, tamaño));
			return nArray;
		}
		#endregion

		#region Captura de pantalla
		/// <summary>
		/// Captura de toda la pantalla
		/// </summary>
		public static Bitmap CapturaPantalla()
		{
			Graphics gr = App.FormularioPrincipal.CreateGraphics();
			// Tamaño de lo que queremos copiar En este caso el tamaño de la pantalla principal
			Size fSize = Screen.PrimaryScreen.Bounds.Size;
			// Creamos el bitmap con el área que vamos a capturar
			Bitmap bm = new Bitmap(fSize.Width, fSize.Height, gr);
			// Un objeto Graphics a partir del bitmap
			Graphics gr2 = Graphics.FromImage(bm);
			// Copiar todo el área de la pantalla
			gr2.CopyFromScreen(0, 0, 0, 0, fSize);

			return bm;
		}

		#endregion

		#region Matemáticas
		/// <summary>
		/// Convierte un ángulo de grados a radianes
		/// </summary>
		/// <param name="angle">Ángulo en grados</param>
		/// <returns>Ángulo en radianes</returns>
		public static double DegreeToRadian(double angle)
		{
			return Math.PI * angle / 180.0;
		}
		/// <summary>
		/// Convierte un ángulo de radianes a grados
		/// </summary>
		/// <param name="angle">Ángulo en radianes</param>
		/// <returns>Ángulo en grados</returns>
		public static double RadianToDegree(double angle)
		{
			return angle * (180.0 / Math.PI);
		}
		#endregion

		#region Punteros
		/// <summary>
		/// Obtiene el puntero a partir del objeto
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static IntPtr GetPtrToNewObject(object obj)
		{
			IntPtr ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(obj));
			Marshal.StructureToPtr(obj, ptr, false);
			return ptr;
		}
		#endregion

		#region Depuración
		/// <summary>
		/// Permite la depuración del sistema
		/// </summary>
		//[Conditional("DEBUG")]
		public static void DebugMode()
		{
			if (Environment.UserInteractive)
			{
				if (!Debugger.IsAttached)
				{
					OMensajes.MostrarInfo("Esperando la conexión de depuración remota");
				}
			}
			else
			{
				Debugger.Break();
				while (!Debugger.IsAttached)
				{
					Thread.Sleep(1);
				}
			}
		}

		/// <summary>
		/// Devuelve información sobre el método que efectua la llamada
		/// </summary>
		/// <param name="ordenLlamada">0 es la información del método GetCallingMethod, 1 la del método que ha llamado, 2 la del metodo que ha llamado al que ha llamado al actual, etc....</param>
		public static void GetCallingMethod(int ordenLlamada, out string assembly, out string file, out string className, out string methode, out int line, out string pilaLlamadas)
		{
			assembly = string.Empty;
			file = string.Empty;
			className = string.Empty;
			methode = string.Empty;
			pilaLlamadas = string.Empty;
			line = 0;

			StackTrace stackTrace = new StackTrace(true);
			if (stackTrace.FrameCount > ordenLlamada)
			{
				StackFrame stackFrame = stackTrace.GetFrame(ordenLlamada);
				MethodBase methodeBase = stackFrame.GetMethod();

				pilaLlamadas = stackFrame.ToString();
				assembly = methodeBase.Module.Assembly.GetName().Name;
				file = Path.GetFileName(stackFrame.GetFileName());
				className = methodeBase.DeclaringType.ToString();
				methode = methodeBase.Name;
				line = stackFrame.GetFileLineNumber();
			}
		}

		/// <summary>
		/// Extrae la información de la excepción
		/// </summary>
		/// <param name="exception"></param>
		/// <param name="assembly"></param>
		/// <param name="file"></param>
		/// <param name="className"></param>
		/// <param name="methode"></param>
		/// <param name="line"></param>
		/// <param name="pilaLlamadas"></param>
		public static void GetExceptionInfo(Exception exception, out string assembly, out string file, out string className, out string methode, out int line, out string pilaLlamadas)
		{
			assembly = string.Empty;
			file = string.Empty;
			className = string.Empty;
			methode = string.Empty;
			pilaLlamadas = string.Empty;
			line = 0;

			StackTrace st = new StackTrace(exception, true);
			foreach (StackFrame sf in st.GetFrames())
			{
				if (!string.IsNullOrEmpty(sf.GetFileName()))
				{
					assembly = sf.GetMethod().Module.Assembly.GetName().Name;
					file = sf.GetFileName();
					className = sf.GetMethod().ReflectedType.FullName;
					methode = sf.GetMethod().Name;
					line = sf.GetFileLineNumber();
					break;
				}
			}
			pilaLlamadas = exception.StackTrace;
		}

		/// <summary>
		/// Determina si la aplicación es de tipo consola
		/// </summary>
		/// <returns></returns>
		public static bool IsConsole()
		{
			return Console.In != StreamReader.Null;
		}

		public static bool IsWinForms()
		{
			return !IsConsole() && Environment.UserInteractive && (Application.OpenForms.Count != 0);
		}
		#endregion

		#region Trabajo con fechas
		/// <summary>
		/// Conversión de día gregocriano a juliano
		/// </summary>
		/// <param name="dia">Día</param>
		/// <param name="mes">Mes</param>
		/// <param name="anno">Año</param>
		/// <returns></returns>
		public static long GregorianoAJuliano(int dia, int mes, int anno)
		{
			//dada una fecha del calendario gregoriano, obtiene
			//un entero que la representa
			long tmes, tanno;
			long jdia;
			//marzo es el mes 0 del año
			if (mes > 2)
			{
				tmes = mes - 3;
				tanno = anno;
			}
			else
			//febrero es el mes 11 del año anterior.
			{
				tmes = mes + 9;
				tanno = anno - 1;
			}
			jdia = (tanno / 4000) * 1460969;
			tanno = (tanno % 4000);
			jdia = jdia +
			   (((tanno / 100) * 146097) / 4) +
			   (((tanno % 100) * 1461) / 4) +
			   (((153 * tmes) + 2) / 5) +
			   dia +
			   1721119;
			return jdia;
		}

		/// <summary>
		/// Conversión de dia juliano a gregoriano
		/// </summary>
		/// <param name="jdia">Día juliano</param>
		/// <returns>Fecha gregoriana</returns>
		public static DateTime JulianoAGregoriano(long jdia)
		{
			long anno, mes, dia;
			long tmp1, tmp2;
			tmp1 = jdia - 1721119;
			anno = ((tmp1 - 1) / 1460969) * 4000;
			tmp1 = ((tmp1 - 1) % 1460969) + 1;
			tmp1 = (4 * tmp1) - 1;
			tmp2 = (4 * ((tmp1 % 146097) / 4)) + 3;
			anno = (100 * (tmp1 / 146097)) + (tmp2 / 1461) + anno;
			tmp1 = (5 * (((tmp2 % 1461) + 4) / 4)) - 3;
			mes = tmp1 / 153;
			dia = ((tmp1 % 153) + 5) / 5;
			if (mes < 10)
				mes = mes + 3;
			else
			{
				mes = mes - 9;
				anno = anno + 1;
			}
			return new DateTime((int)anno, (int)mes, (int)dia);
		}
		#endregion

		#region Reflexión
		/// <summary>
		/// Función capaz de construir una clase dinámicamente
		/// </summary>
		/// <param name="ensamblado">Nombre del ensamblado al que pertenece la clase</param>
		/// <param name="claseImplementadora">Nombre de la clase a construir</param>
		/// <param name="objetoImplementado">Objeto resultado de la implementación</param>
		/// <param name="args">Argumentos</param>
		/// <returns></returns>
		public static bool ConstruirClase(string ensamblado, string claseImplementadora, out object objetoImplementado, params object[] args)
		{
			objetoImplementado = null;
			bool resultado = false;
			try
			{
				Assembly assembly = Assembly.Load(ensamblado);
				Type tipoClaseImplementadora = assembly.GetType(claseImplementadora);
				if (tipoClaseImplementadora == null)
				{
					OVALogsManager.Fatal(ModulosSistema.Comun, "Constructor clase", "No se encuentra la clase implementadora " + claseImplementadora);
				}
				else
				{
					objetoImplementado = Activator.CreateInstance(tipoClaseImplementadora, args);
					resultado = true;
				}
			}
			catch (Exception exception)
			{
				OVALogsManager.Error(ModulosSistema.Comun, "ConstruirClasePorReflexión", exception, string.Format("Ensamblado: {0}, Clase: {1}", ensamblado, claseImplementadora));
			}
			return resultado;
		}
		#endregion

		#region Encriptación
		/// <summary>
		/// Encripta el fichero pasado
		/// </summary>
		/// <param name="inputFile"></param>
		/// <param name="outputFile"></param>
		/// <param name="key"></param>
		public static void EncryptFile(string inputFile, string outputFile, string key)
		{
			try
			{
				byte[] keyBytes; keyBytes = Encoding.Unicode.GetBytes(key);
				Rfc2898DeriveBytes derivedKey = new Rfc2898DeriveBytes(key, keyBytes);
				RijndaelManaged rijndaelCSP = new RijndaelManaged();
				rijndaelCSP.Key = derivedKey.GetBytes(rijndaelCSP.KeySize / 8);
				rijndaelCSP.IV = derivedKey.GetBytes(rijndaelCSP.BlockSize / 8);
				ICryptoTransform encryptor = rijndaelCSP.CreateEncryptor();
				FileStream inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
				byte[] inputFileData = new byte[(int)inputFileStream.Length];
				inputFileStream.Read(inputFileData, 0, (int)inputFileStream.Length);
				FileStream outputFileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
				CryptoStream encryptStream = new CryptoStream(outputFileStream, encryptor, CryptoStreamMode.Write);
				encryptStream.Write(inputFileData, 0, (int)inputFileStream.Length);
				encryptStream.FlushFinalBlock();
				rijndaelCSP.Clear();
				encryptStream.Close();
				inputFileStream.Close();
				outputFileStream.Close();
				OVALogsManager.Info(ModulosSistema.Comun, "Encriptar fichero", "OK");
			}
			catch (Exception ex)
			{
				OVALogsManager.Error(ModulosSistema.Comun, "Encriptar fichero", ex);
				return;
			}
		}
		/// <summary>
		/// Desencripta el fichero pasado
		/// </summary>
		/// <param name="inputFile"></param>
		/// <param name="outputFile"></param>
		/// <param name="key"></param>
		public static void DecryptFile(string inputFile, string outputFile, string key)
		{
			try
			{
				byte[] keyBytes = Encoding.Unicode.GetBytes(key);
				Rfc2898DeriveBytes derivedKey = new Rfc2898DeriveBytes(key, keyBytes);
				RijndaelManaged rijndaelCSP = new RijndaelManaged(); rijndaelCSP.Key = derivedKey.GetBytes(rijndaelCSP.KeySize / 8);
				rijndaelCSP.IV = derivedKey.GetBytes(rijndaelCSP.BlockSize / 8);
				ICryptoTransform decryptor = rijndaelCSP.CreateDecryptor();
				FileStream inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
				CryptoStream decryptStream = new CryptoStream(inputFileStream, decryptor, CryptoStreamMode.Read);
				byte[] inputFileData = new byte[(int)inputFileStream.Length];
				decryptStream.Read(inputFileData, 0, (int)inputFileStream.Length);
				FileStream outputFileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write);
				outputFileStream.Write(inputFileData, 0, inputFileData.Length);
				outputFileStream.Flush();
				rijndaelCSP.Clear();
				decryptStream.Close();
				inputFileStream.Close();
				outputFileStream.Close();
				OVALogsManager.Info(ModulosSistema.Comun, "Des-Encriptar fichero", "OK");
			}
			catch (Exception ex)
			{
				OVALogsManager.Error(ModulosSistema.Comun, "Des-Encriptar fichero", ex);
				return;
			}
		}
		/// <summary>
		/// Desencripta el fichero pasado
		/// </summary>
		/// <param name="inputFile"></param>
		/// <param name="outputFile"></param>
		/// <param name="key"></param>
		public static byte[] DecryptFile(string inputFile, string key)
		{
			try
			{
				byte[] keyBytes = Encoding.Unicode.GetBytes(key);
				Rfc2898DeriveBytes derivedKey = new Rfc2898DeriveBytes(key, keyBytes);
				RijndaelManaged rijndaelCSP = new RijndaelManaged(); rijndaelCSP.Key = derivedKey.GetBytes(rijndaelCSP.KeySize / 8);
				rijndaelCSP.IV = derivedKey.GetBytes(rijndaelCSP.BlockSize / 8);
				ICryptoTransform decryptor = rijndaelCSP.CreateDecryptor();
				FileStream inputFileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
				CryptoStream decryptStream = new CryptoStream(inputFileStream, decryptor, CryptoStreamMode.Read);
				byte[] inputFileData = new byte[(int)inputFileStream.Length];
				decryptStream.Read(inputFileData, 0, (int)inputFileStream.Length);
				rijndaelCSP.Clear();
				decryptStream.Close();
				inputFileStream.Close();
				OVALogsManager.Info(ModulosSistema.Comun, "Desencriptar fichero", "OK");
				return inputFileData;
			}
			catch (Exception ex)
			{
				OVALogsManager.Error(ModulosSistema.Comun, "Desencriptar fichero", ex);
				return null;
			}
		}
		#endregion

        #region Red
        public static bool HttpPing(string url)
        {
            bool resultado = false;
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.AllowAutoRedirect = false; // find out if this site is up and don't follow a redirector
                request.Method = "HEAD";
                WebResponse webResponse = request.GetResponse();
                resultado = webResponse.ContentLength > 0;
                // do something with response.Headers to find out information about the request
            }
            catch (WebException ex)
            {
                //set flag if there was a timeout or some other issues
            }
            return resultado;
        }
        #endregion
	}
	#endregion

	#region Clase StringValueAttribute: Atributo utilizado para convertir enumerados a strings
	/// <summary>
	/// Simple attribute class for storing String Values
	/// </summary>
	public class OStringValueAttribute : Attribute
	{
		#region Atributo(s)
		/// <summary>
		/// Valor descriptivo de objeto
		/// </summary>
		private string _value;
		#endregion

		#region Constructor
		/// <summary>
		/// Creates a new <see cref="OStringValueAttribute"/> instance.
		/// </summary>
		/// <param name="value">Value.</param>
		public OStringValueAttribute(string value)
		{
			_value = value;
		}
		#endregion

		#region Método(s) público(s)
		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value></value>
		public string Value
		{
			get { return _value; }
		}
		#endregion

		#region Método(s) estático(s)
		/// <summary>
		/// Gets a string value for a particular enum value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>String Value associated via a <see cref="OStringValueAttribute"/> attribute, or null if not found.</returns>
		public static string GetStringValue(Enum value)
		{
			string output = string.Empty;

			//Look for our 'StringValueAttribute' in the field's custom attributes
			Type type = value.GetType();
			FieldInfo fi = type.GetField(value.ToString());
			OStringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(OStringValueAttribute), false) as OStringValueAttribute[];
			if (attrs.Length > 0)
			{
				output = attrs[0].Value;
			}

			return output;
		}

		/// <summary>
		/// Devuelve una lista con los valores de texto de los enumerados
		/// </summary>
		/// <param name="enumType"></param>
		/// <returns></returns>
		public static Dictionary<int, string> GetListStringValue(Type enumType)
		{
			Dictionary<int, string> resultado = new Dictionary<int, string>();

			foreach (object value in Enum.GetValues(enumType))
			{
				resultado.Add((int)value, GetStringValue((Enum)value));
			}

			return resultado;
		}

		/// <summary>
		/// Busca si el texto coincide con un enumerado
		/// </summary>
		/// <param name="stringValue"></param>
		/// <returns></returns>
		public static Enum FindStringValue(Type enumType, string stringValue, Enum DefaultValue)
		{
			Enum resultado = DefaultValue;

			foreach (object value in Enum.GetValues(enumType))
			{
				string strAux = GetStringValue((Enum)value);
				if (strAux == stringValue)
				{
					resultado = (Enum)value;
					break;
				}
			}

			return resultado;
		}

		#endregion
	}
	#endregion

	#region Clase ComposicionRuta: Compone una ruta permitiendo la utilización de parametros
	/// <summary>
	/// Clase utilizada para componer la ruta de almacenamiento de las imágenes
	/// </summary>
	public static class ORutaParametrizable
	{
		#region Constante(s)
		/// <summary>
		/// Ruta de la aplicación
		/// </summary>
		public const string RutaApp = @"%APP_PATH%";
		/// <summary>
		/// Fecha actual
		/// </summary>
		public const string Fecha = @"%FECHA%";
		/// <summary>
		/// Fecha y hora actual
		/// </summary>
		public const string FechaHora = @"%FECHA_HORA%";
		/// <summary>
		/// Año actual
		/// </summary>
		public const string Año = @"%AÑO%";
		/// <summary>
		/// Mes actual
		/// </summary>
		public const string Mes = @"%MES%";
		/// <summary>
		/// Día actual
		/// </summary>
		public const string Dia = @"%DIA%";
		/// <summary>
		/// Día juliano actual
		/// </summary>
		public const string DiaJuliano = @"%DIA_JULIANO%";
		/// <summary>
		/// Hora actual
		/// </summary>
		public const string Hora = @"%HORA%";
		/// <summary>
		/// Minuto actual
		/// </summary>
		public const string Minuto = @"%MINUTO%";
		/// <summary>
		/// Segundo actual
		/// </summary>
		public const string Segundo = @"%SEGUNDO%";
		/// <summary>
		/// Milisegundo actual
		/// </summary>
		public const string Milisegundo = @"%MILISEGUNDO%";
		#endregion

		#region Propiedad(es)
		/// <summary>
		/// Devuelve la ruta de la aplicación
		/// </summary>
		public static string AppPath
		{
			get
			{
				return Path.GetFileNameWithoutExtension(Application.ExecutablePath);
			}
		}

		/// <summary>
		/// Devuelve la carpeta de la aplicación
		/// </summary>
		public static string AppFolder
		{
			get
			{
				return Path.GetDirectoryName(Application.ExecutablePath);
			}
		}
		#endregion

		#region Método(s) público(s)
		/// <summary>
		/// Devuelve una cadena de texto identificativa del día actual (utilizada para indexar ficheros)
		/// </summary>
		/// <returns></returns>
		public static string DateString(DateTime fecha)
		{
			string resultado = string.Empty;

			resultado += fecha.Year.ToString("0000");
			resultado += fecha.Month.ToString("00");
			resultado += fecha.Day.ToString("00");

			return resultado;
		}

		/// <summary>
		/// Devuelve una cadena de texto identificativa del momento actual (utilizada para indexar ficheros)
		/// </summary>
		/// <returns></returns>
		public static string DateTimeString(DateTime fecha)
		{
			string resultado = string.Empty;

			resultado += fecha.Year.ToString("0000");
			resultado += fecha.Month.ToString("00");
			resultado += fecha.Day.ToString("00");
			resultado += fecha.Hour.ToString("00");
			resultado += fecha.Minute.ToString("00");
			resultado += fecha.Second.ToString("00");
			resultado += fecha.Millisecond.ToString("000");

			return resultado;
		}

		/// <summary>
		/// Composición de rutas a partir de parámetros
		/// </summary>
		/// <returns></returns>
		public static string ComponerRuta(string ruta, Dictionary<string, object> listaParametrosRuta)
		{
			return ComponerRuta(ruta, listaParametrosRuta, DateTime.Now);
		}

		/// <summary>
		/// Composición de rutas a partir de parámetros
		/// </summary>
		/// <returns></returns>
		public static string ComponerRuta(string ruta, Dictionary<string, object> listaParametrosRuta, DateTime momento)
		{
			string resultado = ruta;

			// Sustitución de parámetros propios
			resultado = resultado.Replace(RutaApp, AppFolder);
			resultado = resultado.Replace(Fecha, DateString(momento));
			resultado = resultado.Replace(FechaHora, DateTimeString(momento));
			resultado = resultado.Replace(Año, momento.Year.ToString("0000"));
			resultado = resultado.Replace(Mes, momento.Month.ToString("00"));
			resultado = resultado.Replace(Dia, momento.Day.ToString("00"));
			resultado = resultado.Replace(DiaJuliano, App.GregorianoAJuliano(momento.Year, momento.Month, momento.Day).ToString("0000000"));
			resultado = resultado.Replace(Hora, momento.Hour.ToString("00"));
			resultado = resultado.Replace(Minuto, momento.Minute.ToString("00"));
			resultado = resultado.Replace(Segundo, momento.Second.ToString("00"));
			resultado = resultado.Replace(Milisegundo, momento.Millisecond.ToString("000"));

			// Sustitución de parámetros a medida
			foreach (KeyValuePair<string, object> parametroRuta in listaParametrosRuta)
			{
                resultado = resultado.Replace(parametroRuta.Key, ORobusto.ToString(parametroRuta.Value));
			}

			return resultado;
		}
		#endregion
	}
	#endregion

	#region Tipos de datos provinientes de la base de datos
	/// <summary>
	/// Enumerado que implementa el enumerado de los módulos del sistema
	/// </summary>
	public enum OEnumTipoDato
	{
		/// <summary>
		/// Tipo no definido
		/// </summary>
		SinDefinir = 0,
		/// <summary>
		/// Tipo booleano o bit
		/// </summary>
		Bit = 1,
		/// <summary>
		/// Tipo entero
		/// </summary>
		Entero = 2,
		/// <summary>
		/// Tipo texto
		/// </summary>
		Texto = 3,
		/// <summary>
		/// Tipo decimal
		/// </summary>
		Decimal = 4,
		/// <summary>
		/// Tipo fecha
		/// </summary>
		Fecha = 5,
		/// <summary>
		/// Tipo imagen
		/// </summary>
		Imagen = 6,
		/// <summary>
		/// Tipo gráfico
		/// </summary>
		Grafico = 7,
		/// <summary>
		/// Tipo Evento
		/// </summary>
		Flag = 8,
	}

	/// <summary>
	/// Clase estática encargada de devolver el valor por defecto de un determinado tipo de dato
	/// </summary>
	public static class OTipoDato
	{
		/// <summary>
		/// Valor por defecto de un determinado tipo de datos
		/// </summary>
		/// <param name="tipoDato"></param>
		/// <returns></returns>
		public static object DevaultValue(OEnumTipoDato tipoDato)
		{
			object resultado = null;

			switch (tipoDato)
			{
				case OEnumTipoDato.SinDefinir:
				case OEnumTipoDato.Imagen:
				case OEnumTipoDato.Grafico:
				case OEnumTipoDato.Flag:
					resultado = null;
					break;
				case OEnumTipoDato.Bit:
					resultado = false;
					break;
				case OEnumTipoDato.Entero:
					resultado = (int)0;
					break;
				case OEnumTipoDato.Texto:
					resultado = string.Empty;
					break;
				case OEnumTipoDato.Decimal:
					resultado = (double)0.0;
					break;
				case OEnumTipoDato.Fecha:
					resultado = DateTime.Now;
					break;
			}

			return resultado;
		}
	}
	#endregion

	#region Enumerado de capitalización del texto
	/// <summary>
	/// Modifica la capitalización de un texto
	/// </summary>
	public enum OCapitalizacionTexto
	{
		/// <summary>
		/// No realiza ninguna acción
		/// </summary>
		Nada,
		/// <summary>
		/// Convierte el texto a mayusculas
		/// </summary>
		Mayusculas,
		/// <summary>
		/// Convierte el texto a minusculas
		/// </summary>
		Minusculas
	}
	#endregion

    #region Definición de delegado(s) genéricos
    /// <summary>
    /// Delegado de método simple
    /// </summary>
    public delegate void OSimpleMethod();
    /// <summary>
    /// Delegado de método simple con devolución de booleano
    /// </summary>
    public delegate bool OSimpleReturnMethod();
    /// <summary>
    /// Delegado utilizado para devolver mensajes
    /// </summary>
    /// <param name="message"></param>
    public delegate void OMessageDelegate(string mensaje);
    /// <summary>
    /// Delegado utilizado para devolver mensajes
    /// </summary>
    /// <param name="codigo"></param>
    /// <param name="message"></param>
    public delegate void OMessageDelegateAdv(string codigo, string mensaje);
    /// <summary>
    /// Delegado utilizado para devolver una excepción
    /// </summary>
    /// <param name="message"></param>
    public delegate void OExceptionDelegate(Exception exception);
    /// <summary>
    /// Delegado utilizado para devolver una excepción
    /// </summary>
    /// <param name="codigo"></param>
    /// <param name="message"></param>
    public delegate void OExceptionDelegateAdv(string codigo, Exception exception);
    #endregion
}