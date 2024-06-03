//***********************************************************************
// Assembly         : Orbita.VAComun
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 05-11-2012
// Description      : Añadido un método sobrecargado de Espera donde se le pasa como argumento un delegado
// Description      : Nuevo método para reemplazar strings especificando el tipo de comparación
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.CSharp;
using Orbita.Controles;
using Orbita.Utiles;
using System.Net;
                                                            
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
		public static OrbitaForm FormularioPrincipalMDI;
		/// <summary>
		/// Gestor de anclas de los formularios
		/// </summary>
		public static OrbitaDockManager DockManager;
		/// <summary>
		/// Lista de parámetros de entrada de la aplicación
		/// </summary>
		public static string[] ListaParametrosEntradaAplicacion;
		/// <summary>
		/// Indica si existe algún formulario MDI hijo que este maximizado
		/// </summary>
		/// <returns>Ture si existe algún formulario MDI hijo que este maximizado; false en caso contrario</returns>
		public static bool HayFormulariosMDIHijosMaximizados()
		{
			foreach (Form f in FormularioPrincipalMDI.MdiChildren)
			{
				if (f.WindowState == FormWindowState.Maximized)
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Ajusta el tamaño y posición de la imagen del fondo en función del tamaño y posición del área de cliente del formulario
		/// </summary>
		public static void AjustarFondoAplicacion(Size tamaño, Image imagen)
		{
			FormularioPrincipalMDI.SuspendLayout();
			// Constantes paras tunear la imagen de fondo de la aplicación //
			float valorOpacidad = (float)0.3; //Entre 0 y 1 (0 invisible, 1 totalmente visible)
			//Tamaño del área donde irá la imagen
			Size p = tamaño;

			if (FormularioPrincipalMDI.WindowState != FormWindowState.Minimized)
			{
				if (p.Width > 0 && p.Height > 0)
				{
					//Obtener una subimagen proporcional al tamaño del fondo, alineada desde la derecha, recortando el exceso de imagen por la izquierda
					Image imagenOriginal = imagen;
					Image imagenFondo = new Bitmap(p.Width, p.Height);

					//Tratar la imagen para escalarla y trasladarla, y así que quede alineada por la derecha.
					//Tambien tratamos la opacidad para simular una marca de agua
					float factorEscalaY = (float)imagenFondo.Height / (float)imagenOriginal.Height;
					float factorEscalaX = factorEscalaY;

					using (Graphics g = Graphics.FromImage(imagenFondo))
					{
						g.InterpolationMode = InterpolationMode.HighQualityBicubic;

						//Escalado
						g.ScaleTransform(factorEscalaX, factorEscalaY);

						//Traslacion
						int traslacion = (int)(g.VisibleClipBounds.Width - imagenOriginal.Width);
						g.TranslateTransform(traslacion, 0);

						//Opacidad
						ColorMatrix cm = new ColorMatrix();
						cm.Matrix33 = valorOpacidad;
						ImageAttributes ia = new ImageAttributes();
						ia.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

						//Dibujamos la imagen creada en imagenFondo
						g.DrawImage(imagenOriginal, new Rectangle(0, 0, imagenOriginal.Width, imagenOriginal.Height), 0, 0, imagenOriginal.Width, imagenOriginal.Height, GraphicsUnit.Pixel, ia);

						//Establecemos la imagen obtenida como fondo de la aplicación
						FormularioPrincipalMDI.BackgroundImage = imagenFondo;
					}
				}
			}
			FormularioPrincipalMDI.ResumeLayout();
		}
		#endregion Campos de la clase

		#region Método(s) de evaluación de números
		/// <summary>
		/// Evalúa si el parámetro está dentro del rango determinado
		/// </summary>
		/// <returns>Devuelve verdadero si el parámetro está dentro del rango determinado</returns>
		public static bool InRange(int value, int min, int max)
		{
			return (value >= min) && (value <= max);
		}

		/// <summary>
		/// Evalúa si el parámetro está dentro del rango determinado
		/// </summary>
		/// <returns>Devuelve verdadero si el parámetro está dentro del rango determinado</returns>
		public static bool InRange(double value, double min, double max)
		{
			return (value >= min) && (value <= max);
		}

		/// <summary>
		/// Evalúa si el parámetro está dentro del rango determinado y en caso contrario lo modifica para que cumpla la condición
		/// </summary>
		/// <returns>Devuelve el número obligando a que esté dentro del rango determinado</returns>
		public static int EnsureRange(int value, int min, int max)
		{
			int resultado = value;

			if (value < min)
			{
				resultado = min;
			}
			if (value > max)
			{
				resultado = max;
			}

			return resultado;
		}

		/// <summary>
		/// Evalúa si el parámetro está dentro del rango determinado y en caso contrario lo modifica para que cumpla la condición
		/// </summary>
		/// <returns>Devuelve el número obligando a que esté dentro del rango determinado</returns>
		public static double EnsureRange(double value, double min, double max)
		{
			double resultado = value;

			if (value < min)
			{
				resultado = min;
			}
			if (value > max)
			{
				resultado = max;
			}

			return resultado;
		}

		/// <summary>
		/// Evalúa si el parámetro está dentro del rango determinado
		/// </summary>
		/// <returns>Devuelve verdadero si el parámetro está dentro del rango determinado</returns>
		public static int EvaluaNumero(object value, int min, int max, int defecto)
		{
			object objValue = value;
			if (IsNumericInt(objValue))
			{
				int intValue = (int)objValue;
				if (InRange(intValue, min, max))
				{
					return intValue;
				}
			}
			return defecto;
		}

		/// <summary>
		/// Evalúa si el parámetro está dentro del rango determinado
		/// </summary>
		/// <returns>Devuelve verdadero si el parámetro está dentro del rango determinado</returns>
		public static double EvaluaDecimal(object value, double min, double max, double defecto)
		{
			object objValue = value;
			if (IsNumericFloat(objValue))
			{
				double floatValue = (double)objValue;
				if (InRange(floatValue, min, max))
				{
					return floatValue;
				}
			}
			return defecto;
		}

		/// <summary>
		/// Evalúa si el parámetro está dentro del rango determinado
		/// </summary>
		/// <returns>Devuelve verdadero si el parámetro está dentro del rango determinado</returns>
		public static double EvaluaNumero(object value, double min, double max, double defecto)
		{
			object objValue = value;
			if (IsNumericFloat(objValue))
			{
				double floatValue = (double)objValue;
				if (InRange(floatValue, min, max))
				{
					return floatValue;
				}
			}
			return defecto;
		}

		/// <summary>
		/// Evalúa si el parámetro es booleano
		/// </summary>
		public static bool EvaluaBooleano(object value, bool defecto)
		{
			bool Resulado = defecto;

			if (value is bool)
			{
				Resulado = (bool)value;
			}
			return Resulado;
		}

		/// <summary>
		/// Evalúa si el parámetro es booleano
		/// </summary>
		public static DateTime EvaluaFecha(object value, DateTime defecto)
		{
			DateTime Resulado = defecto;

			if (value is DateTime)
			{
				Resulado = (DateTime)value;
			}
			return Resulado;
		}

		/// <summary>
		/// Función que devuelve si el objeto pertenece a alguno de los tipos listados
		/// </summary>
		/// <param name="o">Objeto que se quiere conocer el tipo</param>
		/// <param name="types">Vector de tipos con lo que se ha de comparar el tipo del objeto</param>
		/// <returns>Verdadero si el tipo del objeto está dentro de la lista de tipos pasados como parámetros</returns>
		public static bool IsTypeOf(object o, params Type[] types)
		{
			foreach (Type t in types)
			{
				if (o.GetType() == t)
					return true;
			}

			return false;
		}

		/// <summary>
		/// Indica si el objeto pasado es de tipo numérico
		/// </summary>
		/// <param name="o">Objeto que se quiere conocer si es de tipo numérico</param>
		/// <returns>Verdadero si el tipo del objeto es numérico</returns>
		public static bool IsNumeric(object o)
		{
			return (o != null) && IsTypeOf(o, typeof(int), typeof(short), typeof(long), typeof(uint), typeof(ushort), typeof(ulong), typeof(byte), typeof(float), typeof(double), typeof(decimal));
		}

		/// <summary>
		/// Indica si el objeto pasado es de tipo entero
		/// </summary>
		/// <param name="o">Objeto que se quiere conocer si es de tipo entero</param>
		/// <returns>Verdadero si el tipo del objeto es entero</returns>
		public static bool IsNumericInt(object o)
		{
			return (o != null) && IsTypeOf(o, typeof(int), typeof(short), typeof(long), typeof(uint), typeof(ushort), typeof(ulong), typeof(byte));
		}

		/// <summary>
		/// Indica si el objeto pasado es de tipo decimal
		/// </summary>
		/// <param name="o">Objeto que se quiere conocer si es de tipo decimal</param>
		/// <returns>Verdadero si el tipo del objeto es decimal</returns>
		public static bool IsNumericFloat(object o)
		{
			return (o != null) && IsTypeOf(o, typeof(float), typeof(double), typeof(decimal));
		}
		#endregion

		#region Método(s) de evaluación de enumerados
		/// <summary>
		/// Se utiliza con enumerados y devuelve verdadero si el enumerado está contenido en el valor
		/// </summary>
		/// <param name="value">Valor del cual se quiere saber si contiene cieto enumerado</param>
		/// <param name="enumerate">Enumerado que deseamos comparar con el valor</param>
		/// <returns>Devuelve verdadero si el enumerado está contenido en el valor</returns>
		public static bool EnumContains(int value, int[] enumerate)
		{
			foreach (int i in enumerate)
			{
				if ((value & i) != 0)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Se utiliza con enumerados para convertir un valor de texto en un enumerado del tipo indicado
		/// </summary>
		/// <param name="enumType">Tipo del enumerado al que deseamos convertir</param>
		/// <param name="value">Texto que queremos convertir a enumerado</param>
		/// <param name="defaultValue">Valor por defecto en el caso que el texto no coincida con ningun elemento del enumerado</param>
		/// <returns>Devuelve el enumerado correspondiente con el texto</returns>
		public static object EnumParse(Type enumType, string value, object defaultValue)
		{
			object resultado;

			try
			{
				resultado = Enum.Parse(enumType, value);
			}
			catch
			{
				resultado = defaultValue;
			}

			return resultado;
		}
		#endregion

		#region Método(s) de trabajo con números binarios
		/// <summary>
		/// Extrae un bit en la posición indicada
		/// </summary>
		/// <param name="numero">Valor al cual queremos extraer el bit</param>
		/// <param name="posicion">Posición del bit a extraer</param>
		/// <returns>Booleano con el valor del bit extraido</returns>
		public static bool GetBit(byte numero, int posicion)
		{
			byte mascara = Convert.ToByte(Math.Pow(2, posicion));
			return (numero & mascara) != 0;
		}

		/// <summary>
		/// Extrae un bit en la posición indicada
		/// </summary>
		/// <param name="numero">Valor al cual queremos extraer el bit</param>
		/// <param name="posicion">Posición del bit a extraer</param>
		/// <returns>Booleano con el valor del bit extraido</returns>
		public static bool GetBit(ushort numero, int posicion)
		{
			UInt16 mascara = Convert.ToUInt16(Math.Pow(2, posicion));
			return (numero & mascara) != 0;
		}

		/// <summary>
		/// Extrae un bit en la posición indicada
		/// </summary>
		/// <param name="numero">Valor al cual queremos extraer el bit</param>
		/// <param name="posicion">Posición del bit a extraer</param>
		/// <returns>Booleano con el valor del bit extraido</returns>
		public static bool GetBit(uint numero, int posicion)
		{
			uint mascara = Convert.ToUInt32(Math.Pow(2, posicion));
			return (numero & mascara) != 0;
		}

		/// <summary>
		/// Establece un bit en la posición indicada
		/// </summary>
		/// <param name="numero">Valor al cual queremos establecer el bit</param>
		/// <param name="posicion">Posición del bit a establecer</param>
		/// <param name="valor">Booleano con el valor del bit a establecer</param>
		public static void SetBit(ref byte numero, int posicion, bool valor)
		{
			byte mascara = Convert.ToByte(Math.Pow(2, posicion));
			if (valor)
			{
				numero = (byte)(numero | mascara);
			}
			else
			{
				numero = (byte)(numero & ~mascara);
			}
		}

		/// <summary>
		/// Establece un bit en la posición indicada
		/// </summary>
		/// <param name="numero">Valor al cual queremos establecer el bit</param>
		/// <param name="posicion">Posición del bit a establecer</param>
		/// <param name="valor">Booleano con el valor del bit a establecer</param>
		public static void SetBit(ref ushort numero, int posicion, bool valor)
		{
			ushort mascara = Convert.ToUInt16(Math.Pow(2, posicion));
			if (valor)
			{
				numero = (ushort)(numero | mascara);
			}
			else
			{
				numero = (ushort)(numero & ~mascara);
			}
		}

		/// <summary>
		/// Establece un bit en la posición indicada
		/// </summary>
		/// <param name="numero">Valor al cual queremos establecer el bit</param>
		/// <param name="posicion">Posición del bit a establecer</param>
		/// <param name="valor">Booleano con el valor del bit a establecer</param>
		public static void SetBit(ref uint numero, int posicion, bool valor)
		{
			uint mascara = Convert.ToUInt32(Math.Pow(2, posicion));
			if (valor)
			{
				numero = (uint)(numero | mascara);
			}
			else
			{
				numero = (uint)(numero & ~mascara);
			}
		}
		#endregion

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
				LogsRuntime.Error(ModulosSistema.Comun, "CreacionDirectorio", exception);
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
				LogsRuntime.Error(ModulosSistema.Comun, "DirectoryCopy", exception);
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
				LogsRuntime.Error(ModulosSistema.Comun, "DirectoryMove", exception);
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
				LogsRuntime.Error(ModulosSistema.Comun, "ConversionFicheroImagen", exception);
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

		#region Trabajo seguro con variables
		/// <summary>
		/// Comprueba que el valor del objeto es correcto
		/// </summary>
		/// <param name="valor">Valor del objeto a comprobar</param>
		/// <returns>Verdadero si el valor es correcto</returns>
		public static bool ValidarTexto(object valor, string codigo, int maxLength, bool admiteVacio, bool lanzarExcepcionSiValorNoValido)
		{
			EstadoTexto estado = EstadoTexto.ResultadoCorrecto;
			bool valido = true;

			if (valido)
			{
				if (!(valor is string))
				{
					estado = EstadoTexto.ValorNoString;
					valido = false;
				}
			}

			if (valido)
			{
				string strValor = (string)valor;
				if (strValor.Length > maxLength)
				{
					estado = EstadoTexto.LongitudSobrepasada;
					valido = false;
				}
			}

			if (valido)
			{
				string strValor = (string)valor;
				if (!admiteVacio && (strValor == string.Empty))
				{
					estado = EstadoTexto.CadenaVacia;
					valido = false;
				}
			}

			if (lanzarExcepcionSiValorNoValido && !valido)
			{
				App.LanzarExcepcionTexto(estado, codigo);
			}

			return valido;
		}
		/// <summary>
		/// Lanza una exepción por no estár permitido el valor especificado
		/// </summary>
		/// <param name="value">valor no permitido</param>
		private static void LanzarExcepcionTexto(EstadoTexto resultado, string codigo)
		{
			switch (resultado)
			{
				case EstadoTexto.ValorNoString:
					throw new Exception("El campo " + codigo + " no es válido.");
					break;
				case EstadoTexto.LongitudSobrepasada:
					throw new Exception("El campo " + codigo + " es demasiado largo.");
					break;
				case EstadoTexto.CadenaVacia:
					throw new Exception("El campo " + codigo + " no puede estar en blanco.");
					break;
			}
		}

		/// <summary>
		/// Comprueba que el valor del objeto es correcto
		/// </summary>
		/// <param name="valor">Valor del objeto a comprobar</param>
		/// <returns>Verdadero si el valor es correcto</returns>
		public static bool ValidarEntero(object valor, string codigo, int minValue, int maxValue, bool lanzarExcepcionSiValorNoValido)
		{
			EstadoEntero estado = EstadoEntero.ResultadoCorrecto;
			bool valido = true;

			if (valido)
			{
				if (!App.IsNumericInt(valor))
				{
					estado = EstadoEntero.ValorNoEntero;
					valido = false;
				}
			}

			if (valido)
			{
				int intValor = Convert.ToInt32(valor);
				if (intValor < minValue)
				{
					estado = EstadoEntero.ValorInferiorMinimo;
					valido = false;
				}
			}

			if (valido)
			{
				int intValor = Convert.ToInt32(valor);
				if (intValor > maxValue)
				{
					estado = EstadoEntero.ValorSuperiorMaximo;
					valido = false;
				}
			}

			if (lanzarExcepcionSiValorNoValido && !valido)
			{
				App.LanzarExcepcionEntero(estado, codigo, minValue, maxValue);
			}

			return valido;
		}
		/// <summary>
		/// Lanza una exepción por no estár permitido el valor especificado
		/// </summary>
		/// <param name="value">valor no permitido</param>
		private static void LanzarExcepcionEntero(EstadoEntero resultado, string codigo, int minValue, int maxValue)
		{
			switch (resultado)
			{
				case EstadoEntero.ValorNoEntero:
					throw new Exception("El campo " + codigo + " no es un número entero.");
					break;
				case EstadoEntero.ValorInferiorMinimo:
					throw new Exception("El campo " + codigo + " es inferior al mínimo " + minValue.ToString() + ".");
					break;
				case EstadoEntero.ValorSuperiorMaximo:
					throw new Exception("El campo " + codigo + " es superior al máximo " + maxValue.ToString() + ".");
					break;
			}
		}

		/// <summary>
		/// Comprueba que el valor del objeto es correcto
		/// </summary>
		/// <param name="valor">Valor del objeto a comprobar</param>
		/// <returns>Verdadero si el valor es correcto</returns>
		public static bool ValidarDecimal(object valor, string codigo, double minValue, double maxValue, bool lanzarExcepcionSiValorNoValido)
		{
			EstadoDecimal estado = EstadoDecimal.ResultadoCorrecto;
			bool valido = true;

			if (valido)
			{
				if (!App.IsNumericFloat(valor))
				{
					estado = EstadoDecimal.ValorNoDecimal;
					valido = false;
				}
			}

			if (valido)
			{
				double intValor = Convert.ToDouble(valor);
				if (intValor < minValue)
				{
					estado = EstadoDecimal.ValorInferiorMinimo;
					valido = false;
				}
			}

			if (valido)
			{
				double intValor = Convert.ToDouble(valor);
				if (intValor > maxValue)
				{
					estado = EstadoDecimal.ValorSuperiorMaximo;
					valido = false;
				}
			}

			if (lanzarExcepcionSiValorNoValido && !valido)
			{
				App.LanzarExcepcionDecimal(estado, codigo, minValue, maxValue);
			}

			return valido;
		}
		/// <summary>
		/// Lanza una exepción por no estár permitido el valor especificado
		/// </summary>
		/// <param name="value">valor no permitido</param>
		private static void LanzarExcepcionDecimal(EstadoDecimal resultado, string codigo, double minValue, double maxValue)
		{
			switch (resultado)
			{
				case EstadoDecimal.ValorNoDecimal:
					throw new Exception("El campo " + codigo + " no es un número decimal.");
					break;
				case EstadoDecimal.ValorInferiorMinimo:
					throw new Exception("El campo " + codigo + " es inferior al mínimo " + minValue.ToString() + ".");
					break;
				case EstadoDecimal.ValorSuperiorMaximo:
					throw new Exception("El campo " + codigo + " es superior al máximo " + maxValue.ToString() + ".");
					break;
			}
		}

		/// <summary>
		/// Comprueba que el valor del objeto es correcto
		/// </summary>
		/// <param name="value">Valor del objeto a comprobar</param>
		/// <returns>Verdadero si el valor es correcto</returns>
		public static bool ValidarEnum<T>(object valor, string codigo, bool lanzarExcepcionSiValorNoValido)
		{
			EstadoEnum estado = EstadoEnum.ValorNoEnumerado;
			bool valido = false;

			if (!valido)
			{
				if ((valor is T) && (typeof(T).IsEnum))
				{
					estado = EstadoEnum.ResultadoCorrecto;
					valido = true;
				}
			}

			if (!valido)
			{
				if (valor is string)
				{
					string strValor = (string)valor;
					try
					{
						T tValor = (T)Enum.Parse(typeof(T), strValor);
						estado = EstadoEnum.ResultadoCorrecto;
						valido = true;
					}
					catch
					{
						estado = EstadoEnum.ValorNoPermitido;
						valido = false;
					}
				}
			}

			if (!valido)
			{
				if (App.IsNumericInt(valor))
				{
					try
					{
						T tValor = (T)valor;
						estado = EstadoEnum.ResultadoCorrecto;
						valido = true;
					}
					catch
					{
						estado = EstadoEnum.ValorNoPermitido;
						valido = false;
					}
				}
			}

			if (lanzarExcepcionSiValorNoValido && !valido)
			{
				App.LanzarExcepcionEnum(estado, codigo);
			}

			return valido;
		}
		/// <summary>
		/// Lanza una exepción por no estár permitido el valor especificado
		/// </summary>
		/// <param name="value">valor no permitido</param>
		private static void LanzarExcepcionEnum(EstadoEnum resultado, string codigo)
		{
			switch (resultado)
			{
				case EstadoEnum.ValorNoEnumerado:
					throw new Exception("El campo " + codigo + " no es válido.");
					break;
				case EstadoEnum.ValorNoPermitido:
					throw new Exception("El campo " + codigo + " no está permitido.");
					break;
			}
		}

		/// <summary>
		/// Comprueba que el valor del objeto es correcto
		/// </summary>
		/// <param name="value">Valor del objeto a comprobar</param>
		/// <returns>Verdadero si el valor es correcto</returns>
		public static bool ValidarFechaHora(object valor, string codigo, DateTime minValue, DateTime maxValue, bool lanzarExcepcionSiValorNoValido)
		{
			EstadoFechaHora estado = EstadoFechaHora.ResultadoCorrecto;
			bool valido = true;

			if (valido)
			{
				if (!(valor is DateTime))
				{
					estado = EstadoFechaHora.ValorNoFecha;
					valido = false;
				}
			}

			if (valido)
			{
				DateTime datetimeValor = (DateTime)valor;
				if (datetimeValor < minValue)
				{
					estado = EstadoFechaHora.ValorInferiorMinimo;
					valido = false;
				}
			}

			if (valido)
			{
				DateTime datetimeValor = (DateTime)valor;
				if (datetimeValor > maxValue)
				{
					estado = EstadoFechaHora.ValorSuperiorMaximo;
					valido = false;
				}
			}

			if (lanzarExcepcionSiValorNoValido && !valido)
			{
				App.LanzarExcepcionFechaHora(estado, codigo, minValue, maxValue);
			}

			return valido;
		}
		/// <summary>
		/// Lanza una exepción por no estár permitido el valor especificado
		/// </summary>
		/// <param name="value">valor no permitido</param>
		private static void LanzarExcepcionFechaHora(EstadoFechaHora resultado, string codigo, DateTime minValue, DateTime maxValue)
		{
			switch (resultado)
			{
				case EstadoFechaHora.ValorNoFecha:
					throw new Exception("El campo " + codigo + " no es una fecha válida.");
					break;
				case EstadoFechaHora.ValorInferiorMinimo:
					throw new Exception("El campo " + codigo + " es inferior al mínimo " + minValue.ToString() + ".");
					break;
				case EstadoFechaHora.ValorSuperiorMaximo:
					throw new Exception("El campo " + codigo + " es superior al máximo " + maxValue.ToString() + ".");
					break;
			}
		}

		/// <summary>
		/// Convierte un objeto a string
		/// </summary>
		/// <param name="valor"></param>
		/// <returns></returns>
		public static string ToString(object valor)
		{
			return valor != null? valor.ToString():string.Empty;
		}

		/// <summary>
		/// Realiza una comparación entre dos objetos
		/// </summary>
		/// <param name="valor1">Primer objeto a comparar</param>
		/// <param name="valor2">Segundo objeto a comparar</param>
		/// <returns></returns>
		public static bool CompararObjetos(object valor1, object valor2)
		{
			bool resultado = false;

			if ((valor1 == null) && (valor2 == null))
			{
				// Ambos son null
				return true;
			}

			if ((valor1 != null) && (valor2 != null))
			{
				if (valor1.GetType() != valor2.GetType())
				{
					// No son del mismo tipo
					return false;
				}

				if (valor1 is byte[])
				{
					byte[] valorByte1 = (byte[])valor1;
					byte[] valorByte2 = (byte[])valor2;

					if (valorByte1.Length != valorByte2.Length)
					{
						// No tienen la misma longitud
						return false;
					}

					for (int i = 0; i < valorByte1.Length; i++)
					{
						if (valorByte1[i] != valorByte2[i])
						{
							// Tienen algun valor distinto
							return false;
						}
					}

					// Los arrays son iguales
					return true;
				}

				return valor1.Equals(valor2);
			}

			return resultado;
		}

		#region Enumerados
		/// <summary>
		/// Resultado de la validación del texto
		/// </summary>
		private enum EstadoTexto
		{
			/// <summary>
			/// Resultado correcto
			/// </summary>
			ResultadoCorrecto = 0,
			/// <summary>
			/// El valor a asignar no es string
			/// </summary>
			ValorNoString = 1,
			/// <summary>
			/// La longitud del texto es demasiado larga
			/// </summary>
			LongitudSobrepasada = 2,
			/// <summary>
			/// El texto no contiene ningun caracter
			/// </summary>
			CadenaVacia = 3
		}
		/// <summary>
		/// Resultado de la validación del Entero
		/// </summary>
		private enum EstadoEntero
		{
			/// <summary>
			/// Resultado correcto
			/// </summary>
			ResultadoCorrecto = 0,
			/// <summary>
			/// El valor a asignar no es entero
			/// </summary>
			ValorNoEntero = 1,
			/// <summary>
			/// El valor a asignar es sueprior al máximo permitido
			/// </summary>
			ValorSuperiorMaximo = 2,
			/// <summary>
			/// El valor a asignar es inferior al mínimo permitido
			/// </summary>
			ValorInferiorMinimo = 3
		}
		/// <summary>
		/// Resultado de la validación del Decimal
		/// </summary>
		private enum EstadoDecimal
		{
			/// <summary>
			/// Resultado correcto
			/// </summary>
			ResultadoCorrecto = 0,
			/// <summary>
			/// El valor a asignar no es entero
			/// </summary>
			ValorNoDecimal = 1,
			/// <summary>
			/// El valor a asignar es sueprior al máximo permitido
			/// </summary>
			ValorSuperiorMaximo = 2,
			/// <summary>
			/// El valor a asignar es inferior al mínimo permitido
			/// </summary>
			ValorInferiorMinimo = 3
		}
		/// <summary>
		/// Resultado de la validación del enumerado
		/// </summary>
		private enum EstadoEnum
		{
			/// <summary>
			/// Resultado correcto
			/// </summary>
			ResultadoCorrecto = 0,
			/// <summary>
			/// El valor a asignar no es enumerado
			/// </summary>
			ValorNoEnumerado = 1,
			/// <summary>
			/// El valor a asignar no está permitido
			/// </summary>
			ValorNoPermitido = 2
		}
		/// <summary>
		/// Resultado de la validación del DateTimne
		/// </summary>
		private enum EstadoFechaHora
		{
			/// <summary>
			/// Resultado correcto
			/// </summary>
			ResultadoCorrecto = 0,
			/// <summary>
			/// El valor a asignar no es entero
			/// </summary>
			ValorNoFecha = 1,
			/// <summary>
			/// El valor a asignar es sueprior al máximo permitido
			/// </summary>
			ValorSuperiorMaximo = 2,
			/// <summary>
			/// El valor a asignar es inferior al mínimo permitido
			/// </summary>
			ValorInferiorMinimo = 3
		}
		#endregion

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

		#region Esperas

		/// <summary>
		/// Método que realiza una espera (sin parar la ejecución del sistema) de cierto tiempo en milisegundos
		/// </summary>
		/// <param name="timeOut">Tiempo de espera en milisegundos</param>
		public static void Espera(int timeOut)
		{
			// Momento en el que finalizará la espera
			DateTime momentoTimeOut = DateTime.Now.AddMilliseconds(timeOut);

			while (DateTime.Now < momentoTimeOut)
			{
				Application.DoEvents();
			}
		}

		/// <summary>
		/// Método que realiza una espera hasta que cierto valor sea verdadero durante un máximo de tiempo
		/// </summary>
		/// <param name="valor">Valor al cual se espera que su estado sea verdadero o falso</param>
		/// <param name="valorEsperado">Valor de comparación</param>
		/// <param name="timeOut">Tiempo máximo de la espera en milisegundos</param>
		/// <returns>Verdadero si el valor a cambiado a verdadero antes de que finalizase el TimeOut</returns>
        public static bool Espera(DelegadoEspera delegadoEspera, int timeOut)
		{
			// Momento en el que finalizará la espera
			DateTime momentoTimeOut = DateTime.Now.AddMilliseconds(timeOut);

            bool valor = delegadoEspera();
            while (!valor && (DateTime.Now < momentoTimeOut))
			{
                Application.DoEvents();
                valor = delegadoEspera();
			}

			return valor;
		}

        /// <summary>
        /// Delegado de la espera
        /// </summary>
        /// <returns></returns>
        public delegate bool DelegadoEspera();

        /// <summary>
        /// Método que realiza una espera hasta que cierto valor sea verdadero durante un máximo de tiempo
        /// </summary>
        /// <param name="valor">Valor al cual se espera que su estado sea verdadero o falso</param>
        /// <param name="valorEsperado">Valor de comparación</param>
        /// <param name="timeOut">Tiempo máximo de la espera en milisegundos</param>
        /// <returns>Verdadero si el valor a cambiado a verdadero antes de que finalizase el TimeOut</returns>
        public static bool Espera(ref bool valor, bool valorEsperado, int timeOut)
        {
            // Momento en el que finalizará la espera
            DateTime momentoTimeOut = DateTime.Now.AddMilliseconds(timeOut);

            while ((valor != valorEsperado) && (DateTime.Now < momentoTimeOut))
            {
                Application.DoEvents();
            }

            return valor == valorEsperado;
        } 

		/// <summary>
		/// Método que realiza una espera hasta que cierto valor sea verdadero durante un máximo de tiempo
		/// </summary>
		/// <param name="valor">Valor al cual se espera que su estado sea verdadero o falso</param>
		/// <param name="valorEsperado">Valor de comparación</param>
		/// <param name="timeOut">Tiempo máximo de la espera en milisegundos</param>
		/// <returns>Verdadero si el valor a cambiado a verdadero antes de que finalizase el TimeOut</returns>
		public static bool Espera(ref object valor, object valorEsperado, int timeOut)
		{
			// Momento en el que finalizará la espera
			DateTime momentoTimeOut = DateTime.Now.AddMilliseconds(timeOut);

			while ((valor != valorEsperado) && (DateTime.Now < momentoTimeOut))
			{
				Application.DoEvents();
			}

			return valor == valorEsperado;
		}
		#endregion

		#region Método(s) Componentes
		/// <summary>
		/// Mutex utilizado por el JustOne
		/// </summary>
		private static Mutex MutexJustOne;

		/// <summary>
		/// Funcion que nos indica si ya se esta iniciando una instancia del programa
		/// </summary>
		/// <returns>true si ya esta iniciada y false si no es asi</returns>
		public static bool JustOne()
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
				LogsRuntime.Error(ModulosSistema.Comun, "GetDiskSpace", exception);
			}

			return space;

		}

		/// <summary>
		/// Mata determinado proceso
		/// </summary>
		/// <param name="nombre">Nombre del proceso</param>
		/// <returns>Verdadero si el proceso ha sido matado</returns>
		public static Process EncontrarProceso(string nombre, string ruta)
		{
			Process resultado = null;

			try
			{
				Process[] asProccess = Process.GetProcessesByName(nombre);
				foreach (Process pProccess in asProccess)
				{
					if (pProccess.MainModule.FileName == ruta)
					{
						resultado = pProccess;
						break;
					}
				}
			}
			catch (Exception exception)
			{
				LogsRuntime.Error(ModulosSistema.Comun, "MatarProceso", exception);
			}

			return resultado;
		}

		/// <summary>
		/// Mata determinado proceso
		/// </summary>
		/// <param name="nombre">Nombre del proceso</param>
		/// <returns>Verdadero si el proceso ha sido matado</returns>
		public static bool MatarProceso(string nombre, string ruta)
		{
			bool resultado = false;

			try
			{
				bool procesoEncontrado;

				// Matamos el proceso de depuración
				procesoEncontrado = true;
				while (procesoEncontrado)
				{
					procesoEncontrado = false;
					Process[] asProccess = Process.GetProcessesByName("dw20");
					foreach (Process pProccess in asProccess)
					{
						pProccess.Kill();
						pProccess.WaitForExit(1000);
						procesoEncontrado = true;
					}
				}

				// Matamos el proceso principal
				procesoEncontrado = true;
				while (procesoEncontrado)
				{
					procesoEncontrado = false;
					Process[] asProccess = Process.GetProcessesByName(nombre);
					foreach (Process pProccess in asProccess)
					{
						if (pProccess.MainModule.FileName == ruta)
						{
							pProccess.Kill();
							pProccess.WaitForExit(1000);
							procesoEncontrado = true;
						}
					}
				}
				resultado = !procesoEncontrado;
			}
			catch (Exception exception)
			{
				LogsRuntime.Error(ModulosSistema.Comun, "MatarProceso", exception);
			}

			return resultado;
		}

		/// <summary>
		/// Inicia una aplicación
		/// </summary>
		/// <param name="ruta">Ruta del ejecutable</param>
		/// <returns>Verdadero si la aplicación se ha ejecutado con éxito</returns>
		public static bool IniciarProceso(string ruta)
		{
			bool resultado = false;

			try
			{
				if (Path.IsPathRooted(ruta) && File.Exists(ruta))
				{
					ProcessStartInfo info = new ProcessStartInfo(ruta);
					Process programa = Process.Start(info);
					resultado = true;
				}
			}
			catch (Exception exception)
			{
				LogsRuntime.Error(ModulosSistema.Comun, "MatarProceso", exception);
			}

			return resultado;
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
				resultado += App.ToString(valor) + separador;
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
				openFileDialog.InitialDirectory = RutaParametrizable.AppFolder;
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
				saveFileDialog.InitialDirectory = RutaParametrizable.AppFolder;
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
				folderBrowserDialog.SelectedPath = RutaParametrizable.AppFolder;
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
			Graphics gr = App.FormularioPrincipalMDI.CreateGraphics();
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

		#region Trabajo con Orbita.Controles
		/// <summary>
		/// Comprueba que hay una fila correctamente activada para lanzar un nuevo formulario
		/// </summary>
		/// <param name="grid">OrbitaGridPro que se desea comprobar</param>
		/// <returns>True si se puede lanzar un nuevo formulario basado en la selección; false en caso contrario</returns>
		public static bool ComprobarGrid(OrbitaGridPro grid)
		{
			return ((grid.OrbGrid.ActiveRow != null) &&
					(grid.OrbGrid.ActiveRow.IsDataRow) &&
					(!grid.OrbGrid.ActiveRow.IsFilteredOut) &&
					(!grid.OrbGrid.ActiveRow.IsAddRow));
		}
		/// <summary>
		/// Comprueba que la fila cumple todos los requisitos para trabajar con los campos de la misma
		/// </summary>
		/// <param name="fila">Fila que se desea comprobar</param>
		/// <returns>True si se puede lanzar un nuevo formulario basado en la selección; false en caso contrario</returns>
		public static bool ComprobarFila(Infragistics.Win.UltraWinGrid.UltraGridRow fila)
		{
			return ((fila != null) &&
					(fila.IsDataRow) &&
					(!fila.IsFilteredOut) &&
					(!fila.IsAddRow));
		}
		/// <summary>
		/// Carga el combo con la lista de módulos de la aplicación
		/// </summary>
		public static void CargarCombo(OrbitaComboPro combo, Type enumType, object valorDefecto)
		{
			// Creación de una nueva tabla.
			DataTable table = new DataTable("DesplegableCombo");

			// Creación de las columnas
			table.Columns.Add(new DataColumn("Indice", enumType));
			table.Columns.Add(new DataColumn("Descripcion", typeof(string)));

			// Se rellena la tabla
			DataRow row;
			foreach (object value in Enum.GetValues(enumType))
			{
				row = table.NewRow();
				row["Indice"] = value;
				row["Descripcion"] = StringValueAttribute.GetStringValue((Enum)value);
				table.Rows.Add(row);
			}

			// Se diseña el grid
			ArrayList cols = new ArrayList();
			cols.Add(new Orbita.Controles.Estilos.CamposEstilos("Descripcion", "Descripción"));

			// Se rellena el grid
			combo.OrbFormatear(table, cols, "Descripcion", "Descripcion");

			// Se establece el valor actual
			combo.OrbCombo.Value = StringValueAttribute.GetStringValue((Enum)valorDefecto);
		}
		/// <summary>
		/// Carga el combo con la lista de módulos de la aplicación
		/// </summary>
		public static void CargarCombo(OrbitaComboPro combo, Dictionary<object, string> valores, Type tipo, object valorDefecto)
		{
			// Creación de una nueva tabla.
			DataTable table = new DataTable("DesplegableCombo");

			// Creación de las columnas
			table.Columns.Add(new DataColumn("Indice", tipo));
			table.Columns.Add(new DataColumn("Descripcion", typeof(string)));

			// Se rellena la tabla
			DataRow row;
			foreach (KeyValuePair<object, string> value in valores)
			{
				row = table.NewRow();
				row["Indice"] = value.Key;
				row["Descripcion"] = value.Value;
				table.Rows.Add(row);
			}

			// Se diseña el grid
			ArrayList cols = new ArrayList();
			cols.Add(new Orbita.Controles.Estilos.CamposEstilos("Descripcion", "Descripción"));
			cols.Add(new Orbita.Controles.Estilos.CamposEstilos("Indice", "Índice"));

			// Se rellena el grid
			combo.OrbFormatear(table, cols, "Descripcion", "Indice");

			// Se establece el valor actual
			combo.OrbCombo.Value = valorDefecto;

			// Se oculta el índice
			combo.OrbCombo.DisplayLayout.Bands[0].Columns["Indice"].Hidden = true;
			combo.OrbCombo.DisplayLayout.Bands[0].ColHeadersVisible = false;
			combo.OrbCombo.DisplayLayout.Bands[0].Columns["Descripcion"].AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows;
		}
		/// <summary>
		/// Carga de un grid de un único campo
		/// </summary>
		/// <param name="grid">Componente sobre el cual se va a trabajar</param>
		/// <param name="valores">Lista de valores a incluir en el grid</param>
		/// <param name="tipo">Tipo de datos de los valores</param>
		/// <param name="estilo">Estilo de la columna</param>
		/// <param name="alinear">Alineación del campo (celdas)</param>
		/// <param name="mascara">Máscara aplicada</param>
		/// <param name="ancho">Ancho de la columna</param>
		/// <param name="editorControl">Control con el cual se modificarán los valores</param>
		public static void CargarGridSimple(OrbitaGridPro grid, List<object> valores, Type tipo, Estilos.EstiloColumna estilo, Estilos.Alineacion alinear, Estilos.Mascara mascara, int ancho, Control editorControl)
		{
			// Bloqueamos el grid
			grid.OrbGrid.BeginUpdate();
			grid.SuspendLayout();

			// Creación de una nueva tabla.
			DataTable table = new DataTable("Table");

			// Creación de las columnas
			table.Columns.Add(new DataColumn("Valor", typeof(object)));
			//table.Columns.Add(new DataColumn("Visualizado", tipo));

			// Se rellena la tabla
			DataRow row;
			foreach (object objeto in valores)
			{
				row = table.NewRow();
				row["Valor"] = objeto;
				table.Rows.Add(row);
			}

			// Se carga el grid
			ArrayList list = new ArrayList();
			list.Add(new Estilos.CamposEstilos("Valor", "Valor", estilo, alinear, mascara, ancho, false));

			// Formateamos las columnas y las rellenamos de datos
			grid.OrbFormatear(table, list);

			// Formato
			grid.OrbGrid.DisplayLayout.Bands[0].ColHeadersVisible = false;
			if (editorControl != null)
			{
				grid.OrbGrid.DisplayLayout.Bands[0].Columns[0].EditorControl = editorControl;
			}

			// Desbloqueamos el grid
			grid.OrbGrid.EndUpdate();
			grid.ResumeLayout();
		}
        /// <summary>
        /// Añade texto a un RichTextBox
        /// </summary>
        /// <param name="box"></param>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public static void RichTextBoxAppendText(RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            Color colorAnterior = box.SelectionColor;
            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = colorAnterior;
        }

        /// <summary>
        /// Añade texto a un RichTextBox
        /// </summary>
        /// <param name="box"></param>
        /// <param name="text"></param>
        /// <param name="font"></param>
        public static void RichTextBoxAppendText(RichTextBox box, string text, Font font)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            Font fuenteAnterior = box.SelectionFont;
            box.SelectionFont = font;
            box.AppendText(text);
            box.SelectionFont = fuenteAnterior;
        }

        /// <summary>
        /// Añade texto a un RichTextBox
        /// </summary>
        /// <param name="box"></param>
        /// <param name="text"></param>
        /// <param name="color"></param>
        /// <param name="font"></param>
        public static void RichTextBoxAppendText(RichTextBox box, string text, Color color, Font font)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            Color colorAnterior = box.SelectionColor;
            Font fuenteAnterior = box.SelectionFont;
            box.SelectionColor = color;
            box.SelectionFont = font;
            box.AppendText(text);
            box.SelectionColor = colorAnterior;
            box.SelectionFont = fuenteAnterior;
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

		#region Definición de delegado(s) genéricos
		/// <summary>
		/// Delegado de método simple
		/// </summary>
		public delegate void SimpleMethod();
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
					LogsRuntime.Fatal(ModulosSistema.Comun, "Constructor clase", "No se encuentra la clase implementadora " + claseImplementadora);
				}
				else
				{
					objetoImplementado = Activator.CreateInstance(tipoClaseImplementadora, args);
					resultado = true;
				}
			}
			catch (Exception exception)
			{
				LogsRuntime.Error(ModulosSistema.Comun, "ConstruirClasePorReflexión", exception, string.Format("Ensamblado: {0}, Clase: {1}", ensamblado, claseImplementadora));
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
				LogsRuntime.Info(ModulosSistema.Comun, "Encriptar fichero", "OK");
			}
			catch (Exception ex)
			{
				LogsRuntime.Error(ModulosSistema.Comun, "Encriptar fichero", ex);
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
				LogsRuntime.Info(ModulosSistema.Comun, "Des-Encriptar fichero", "OK");
			}
			catch (Exception ex)
			{
				LogsRuntime.Error(ModulosSistema.Comun, "Des-Encriptar fichero", ex);
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
				LogsRuntime.Info(ModulosSistema.Comun, "Desencriptar fichero", "OK");
				return inputFileData;
			}
			catch (Exception ex)
			{
				LogsRuntime.Error(ModulosSistema.Comun, "Desencriptar fichero", ex);
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
            catch (WebException wex)
            {
                //set flag if there was a timeout or some other issues
            }
            return resultado;
        }
        #endregion
	}
	#endregion

	#region Clase EnumeracionCombo: Utilizada para visualizar en el componente OrbitaComboPro enumerados
	/// <summary>
	/// Clase destinada a los combos de los formularios databinding que representan un enumerado
	/// </summary>
	public class EnumeracionCombo
	{
		#region Propiedad(es)
		/// <summary>
		/// Enumerado a elegir en el comobo box
		/// </summary>
		private object _Enumerado;
		/// <summary>
		/// Enumerado a elegir en el comobo box
		/// </summary>
		public object Enumerado
		{
			get { return _Enumerado; }
			set { _Enumerado = value; }
		}
		/// <summary>
		/// Descripción a mostrar en el combo box
		/// </summary>
		private string _Descripcion;
		/// <summary>
		/// Descripción a mostrar en el combo box
		/// </summary>
		public string Descripcion
		{
			get { return _Descripcion; }
			set { _Descripcion = value; }
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor de la clase
		/// </summary>
		/// <param name="enumerado">Enumerado a elegir en el comobo box</param>
		/// <param name="descripcion">Descripción a mostrar en el combo box</param>
		public EnumeracionCombo(object enumerado, string descripcion)
		{
			this._Enumerado = enumerado;
			this._Descripcion = descripcion;
		}
		#endregion
	}
	#endregion

	#region Clase StringValueAttribute: Atributo utilizado para convertir enumerados a strings
	/// <summary>
	/// Simple attribute class for storing String Values
	/// </summary>
	public class StringValueAttribute : Attribute
	{
		#region Atributo(s)
		/// <summary>
		/// Valor descriptivo de objeto
		/// </summary>
		private string _value;
		#endregion

		#region Constructor
		/// <summary>
		/// Creates a new <see cref="StringValueAttribute"/> instance.
		/// </summary>
		/// <param name="value">Value.</param>
		public StringValueAttribute(string value)
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
		/// <returns>String Value associated via a <see cref="StringValueAttribute"/> attribute, or null if not found.</returns>
		public static string GetStringValue(Enum value)
		{
			string output = string.Empty;

			//Look for our 'StringValueAttribute' in the field's custom attributes
			Type type = value.GetType();
			FieldInfo fi = type.GetField(value.ToString());
			StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
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

	#region Clase OrbitaControlServicio: Utilizada para instalar/desinstalar o iniciar/detener un servicio
	/// <summary>
	/// Clase utilizada para la instalación/desinstalación de servicios y su inicio y detención
	/// </summary>
	public class OrbitaControlServicio
	{
		#region Atributo(s)
		/// <summary>
		/// Nombre del servicio
		/// </summary>
		string Nombre;
		/// <summary>
		/// Ruta del ejecutable
		/// </summary>
		string Ruta;
		/// <summary>
		/// Controlador del servicio instalado
		/// </summary>
		private ServiceController Controlador;
		#endregion

		#region Propiedad(es)
		/// <summary>
		/// Informa si existe algún servicio con el nombre especificado, en caso contrario se supone que el servicio no está instalado
		/// </summary>
		public bool Instalado
		{
			get
			{
				return this.IsInstalled();
			}
			set
			{
				this.Instalar(value);
			}
		}

		/// <summary>
		/// Estado del servicio
		/// </summary>
		public ServiceControllerStatus Estado
		{
			get { return this.Controlador.Status; }
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor de la clase
		/// </summary>
		/// <param name="nombre">Nombre del servicio</param>
		public OrbitaControlServicio(string nombre, string ruta)
		{
			this.Nombre = nombre;
			this.Ruta = ruta;

			// Se averigua si está instalado
			this.Controlador = null;
			ServiceController[] listaServicios = ServiceController.GetServices();
			foreach (ServiceController servicio in listaServicios)
			{
				if ((servicio.ServiceName == "OrbitaJobService") && (servicio.MachineName == "."))
				{
					this.Controlador = servicio;
				}
			}

			this.Controlador = new ServiceController(this.Nombre);
		}
		#endregion

		#region Método(s) público
		/// <summary>
		/// Instala el servicio
		/// </summary>
		/// <returns>Verdadero si el proceso ha finalizado con éxito</returns>
		public bool Instalar()
		{
			return this.Instalar(true);
		}
		/// <summary>
		/// Desinstala el servicio
		/// </summary>
		/// <returns>Verdadero si el proceso ha finalizado con éxito</returns>
		public bool Desinstalar()
		{
			return this.Instalar(false);
		}
		/// <summary>
		/// Inicia el servicio
		/// </summary>
		/// <returns>Verdadero si se ha iniciado con éxtio</returns>
		public bool Iniciar()
		{
			if (this.Controlador is ServiceController)
			{
				if ((this.Controlador.Status == ServiceControllerStatus.Stopped) || (this.Controlador.Status == ServiceControllerStatus.Paused))
				{
					this.Controlador.Start();
					this.Controlador.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMilliseconds(10000));
				}

			}
			return this.Controlador.Status == ServiceControllerStatus.Running;
		}
		/// <summary>
		/// Inicia el servicio
		/// </summary>
		/// <returns>Verdadero si se ha iniciado con éxtio</returns>
		public bool Detener()
		{
			if (this.Controlador is ServiceController)
			{
				if ((this.Controlador.Status == ServiceControllerStatus.Running) || (this.Controlador.Status == ServiceControllerStatus.Paused))
				{
					this.Controlador.Stop();
					this.Controlador.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds(10000));
				}
			}
			return this.Controlador.Status == ServiceControllerStatus.Stopped;
		}
		#endregion

		#region Método(s) privado(s)
		/// <summary>
		/// Instala o desinstala el servicio
		/// </summary>
		/// <returns>Verdadero si el proceso ha finalizado con éxito</returns>
		private bool Instalar(bool valor)
		{
			Process proceso = new Process();
			string netFolder = RuntimeEnvironment.GetRuntimeDirectory();
			string rutaInstalador = Path.Combine(netFolder, "installutil.exe");
			if (File.Exists(rutaInstalador))
			{
				proceso.StartInfo.FileName = Path.Combine(netFolder, "installutil.exe");
				proceso.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				if (valor)
				{
					proceso.StartInfo.Arguments = this.Ruta;
				}
				else
				{
					proceso.StartInfo.Arguments = "-u " + this.Ruta;
				}
				proceso.Start();
				proceso.WaitForExit();
				return this.IsInstalled();
			}
			return false;
		}

		private bool IsInstalled()
		{
			this.Controlador = null;
			ServiceController[] listaServicios = ServiceController.GetServices();
			foreach (ServiceController servicio in listaServicios)
			{
				if ((servicio.ServiceName == this.Nombre) && (servicio.MachineName == "."))
				{
					this.Controlador = servicio;
					return true;
				}
			}
			return false;
		}
		#endregion
	}
	#endregion

	#region Clase OrbitaScript: Implementa la ejecución de un script (fichero .cs externo a la aplicación)
	/// <summary>
	/// Clase que implementa la ejecución de un script (fichero .cs externo a la aplicación)
	/// </summary>
	public class OrbitaScript
	{
		#region Atributo(s)
		/// <summary>
		/// Nombre de la clase del script
		/// </summary>
		private string NombreClase;
		/// <summary>
		/// Espacio de nombres del script (namespace)
		/// </summary>
		private string EspacioNombres;
		/// <summary>
		/// Ruta del código a compilar
		/// </summary>
		private string RutaScript;
		/// <summary>
		/// Ruta del ensamablado compilado
		/// </summary>
		private string RutaEnsamblado;
		/// <summary>
		/// Referencias a incluir en el ensamblado
		/// </summary>
		private List<string> Referencias;
		/// <summary>
		/// Indica si el script ha sido compilado
		/// </summary>
		public bool Compilado;
		#endregion

		#region Propiedad(es)
		/// <summary>
		/// Código identificativo de la clase.
		/// </summary>
		private string _Codigo;
		/// <summary>
		/// Código identificativo de la clase.
		/// </summary>
		public string Codigo
		{
			get { return _Codigo; }
			set { _Codigo = value; }
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor de la calse
		/// </summary>
		/// <param name="codigoScript">Código identificativo del script (tando del espacio de nombres como de la clase)</param>
		/// <param name="rutaScript">Ruta del código a compilar</param>
		/// <param name="rutaEnsamblado">Ruta del ensamablado compilado</param>
		/// <param name="referencias">Referencias a incluir en el ensamblado</param>
		public OrbitaScript(string nombreClase, string espacioNombres, string rutaScript, string rutaEnsamblado, List<string> referencias)
		{
			this.Codigo = nombreClase;
			this.NombreClase = nombreClase;
			this.EspacioNombres = espacioNombres;
			this.RutaScript = rutaScript;
			this.RutaEnsamblado = rutaEnsamblado;
			this.Referencias = referencias;
			this.Compilado = false;
		}
		#endregion

		#region Método(s) público(s)
		/// <summary>
		/// Compila el script creando un nuevo ensamblado .dll
		/// </summary>
		/// <returns>Verdadero si el método ha funcionado con éxito</returns>
		public bool Compilar()
		{
			bool resultado = false;

			if (this.Validar())
			{
				try
				{
					// Creación del compilador
					CSharpCodeProvider compiladorCSharp = new CSharpCodeProvider();
					// Creación de los parámetros del compilador
					CompilerParameters cp = new CompilerParameters();
					// Generate an executable instead of a class library.
					cp.GenerateExecutable = false;
					// Set the assembly file name to generate.
					cp.OutputAssembly = this.RutaEnsamblado;
					// Generate debug information.
					cp.IncludeDebugInformation = true;
					// Add an assembly reference.
					foreach (string referencia in this.Referencias)
					{
						cp.ReferencedAssemblies.Add(referencia);
					}

					// Save the assembly as a physical file.
					cp.GenerateInMemory = false;
					// Set the level at which the compiler should start displaying warnings.
					cp.WarningLevel = 3;
					// Set whether to treat all warnings as errors.
					cp.TreatWarningsAsErrors = false;
					// Set compiler argument to optimize output.
					cp.CompilerOptions = "/optimize";
					// Set a temporary files collection. The TempFileCollection stores the temporary files generated during a build in the current directory, and does not delete them after compilation. 
					cp.TempFiles = new TempFileCollection(Path.GetTempPath(), false);

					// Compilación
					CompilerResults cr = compiladorCSharp.CompileAssemblyFromFile(cp, this.RutaScript);

					// Verificación
					this.Compilado = (cr.Errors.Count == 0) && (File.Exists(this.RutaEnsamblado));
					resultado = this.Compilado;
				}
				catch (Exception exception)
				{
					LogsRuntime.Error(ModulosSistema.Comun, "Compilar", exception);
				}
			}

			return resultado;
		}

		/// <summary>
		/// Crea una instancia del objeto del script
		/// </summary>
		/// <typeparam name="ClasePadre">Clase padre del objeto creado</typeparam>
		/// <returns>Retorna una instancia de la clase padre del objeto</returns>
		public bool CrearInstancia<ClasePadre>(out ClasePadre instancia)
		{
			instancia = default(ClasePadre);
			if (this.Compilado)
			{
				// Creación de la instancia
				Assembly ensamblado = Assembly.LoadFile(this.RutaEnsamblado);
				Type tipo = ensamblado.GetType(this.EspacioNombres + "." + this.NombreClase);
				if (tipo.IsClass && (tipo.BaseType == typeof(ClasePadre)))
				{
					instancia = (ClasePadre)ensamblado.CreateInstance(this.EspacioNombres + "." + this.NombreClase);
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Se valida que los parámetros de ejecución del script son correctos.
		/// </summary>
		/// <returns></returns>
		public bool Validar()
		{
			bool resultado = true;

			resultado &= NombreClase != string.Empty;
			resultado &= File.Exists(this.RutaScript);
			resultado &= Directory.Exists(Path.GetDirectoryName(this.RutaEnsamblado));

			return resultado;
		}

		#endregion
	}
	#endregion

	#region Clase Enumerado: Implementa un enumerado con posibilidad de herencia
	/// <summary>
	/// Clase que agrupa a un conjunto de enumerados
	/// </summary>
	public class Enumerados
	{
		#region Atributo(s)
		/// <summary>
		/// Lista de los enumerados que contiene
		/// </summary>
		public List<Enumerado> ListaEnumerados;
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor de la clase
		/// </summary>
		public Enumerados()
		{
			this.ListaEnumerados = new List<Enumerado>();

			Type tipo = this.GetType();

			while (tipo != typeof(Enumerados))
			{
				FieldInfo[] fields = tipo.GetFields();
				foreach (FieldInfo fieldInfo in fields)
				{
					if (fieldInfo.IsStatic)
					{
						object valor = fieldInfo.GetValue(null);
						if (valor is Enumerado)
						{
							this.ListaEnumerados.Add((Enumerado)valor);
						}
					}
				}
				tipo = tipo.BaseType;
			}
		}
		#endregion

		#region Método(s) público(s)
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="nombre"></param>
		/// <returns></returns>
		public T Parse<T>(string nombre)
			where T: Enumerado
		{
			T resultado = null;

			foreach (Enumerado enumerado in this.ListaEnumerados)
			{
				if (enumerado.Nombre == nombre)
				{
					return (T)enumerado;
				}
			}

			return resultado;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="codigo"></param>
		/// <returns></returns>
		public T Parse<T>(int valor)
			where T : Enumerado
		{
			T resultado = null;

			foreach (Enumerado enumerado in this.ListaEnumerados)
			{
				if (enumerado.Valor == valor)
				{
					return (T)enumerado;
				}
			}

			return resultado;
		}
		#endregion
	}

	/// <summary>
	/// Clase utilizada para permitir la herencia de enumerados
	/// </summary>
	public class Enumerado
	{
		#region Atributo(s)
		/// <summary>
		/// Nombre del enumerado
		/// </summary>
		public string Nombre;
		/// <summary>
		/// Descripcion
		/// </summary>
		public string Descripcion;
		/// <summary>
		/// Valor del enumerado
		/// </summary>
		public int Valor;
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor de la clase
		/// </summary>
		public Enumerado(string nombre, string descripcion, int valor)
		{
			this.Nombre = nombre;
			this.Descripcion = descripcion;
			this.Valor = valor;
		}
		#endregion
	} 
	#endregion

	#region Clase ComposicionRuta: Compone una ruta permitiendo la utilización de parametros
	/// <summary>
	/// Clase utilizada para componer la ruta de almacenamiento de las imágenes
	/// </summary>
	public static class RutaParametrizable
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
				resultado = resultado.Replace(parametroRuta.Key, App.ToString(parametroRuta.Value));
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
	public enum EnumTipoDato
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
	public static class TipoDato
	{
		/// <summary>
		/// Valor por defecto de un determinado tipo de datos
		/// </summary>
		/// <param name="tipoDato"></param>
		/// <returns></returns>
		public static object DevaultValue(EnumTipoDato tipoDato)
		{
			object resultado = null;

			switch (tipoDato)
			{
				case EnumTipoDato.SinDefinir:
				case EnumTipoDato.Imagen:
				case EnumTipoDato.Grafico:
				case EnumTipoDato.Flag:
					resultado = null;
					break;
				case EnumTipoDato.Bit:
					resultado = false;
					break;
				case EnumTipoDato.Entero:
					resultado = (int)0;
					break;
				case EnumTipoDato.Texto:
					resultado = string.Empty;
					break;
				case EnumTipoDato.Decimal:
					resultado = (double)0.0;
					break;
				case EnumTipoDato.Fecha:
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
	public enum CapitalizacionTexto
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

	#region Trabajo con colecciones
	/// <summary>
	/// Clase utilizada para almacenar un par de valores
	/// </summary>
	/// <typeparam name="TFirst">Tipo del primer valor</typeparam>
	/// <typeparam name="TSecond">Tipo del segundo valor</typeparam>
	public class OPair<TFirst, TSecond>
	{
		#region Atributo(s)
		/// <summary>
		/// Primer valor
		/// </summary>
		public TFirst First;
		/// <summary>
		/// Segundo valor
		/// </summary>
		public TSecond Second;
		#endregion

		#region Constructor de la clase
		/// <summary>
		/// Constructor de la clase
		/// </summary>
		public OPair()
		{
		}
		/// <summary>
		/// Constructor de la clase
		/// </summary>
		public OPair(TFirst first, TSecond second)
		{
			this.First = first;
			this.Second = second;
		}
		#endregion
	}

	/// <summary>
	/// Clase utilizada para almacenar un trío de valores
	/// </summary>
	/// <typeparam name="TFirst">Tipo del primer valor</typeparam>
	/// <typeparam name="TSecond">Tipo del segundo valor</typeparam>
	/// <typeparam name="TThird">Tipo del tercer valor</typeparam>
	public class OTriplet<TFirst, TSecond, TThird>
	{
		#region Atributo(s)
		/// <summary>
		/// Primer valor
		/// </summary>
		public TFirst First;
		/// <summary>
		/// Segundo valor
		/// </summary>
		public TSecond Second;
		/// <summary>
		/// Tercer valor
		/// </summary>
		public TThird Third;
		#endregion

		#region Constructor de la clase
		/// <summary>
		/// Constructor de la clase
		/// </summary>
		public OTriplet()
		{
		}
		/// <summary>
		/// Constructor de la clase
		/// </summary>
		public OTriplet(TFirst first, TSecond second, TThird third)
		{
			this.First = first;
			this.Second = second;
			this.Third = third;
		}
		#endregion
	}

	/// <summary>
	/// Clase utilizada para almacenar un trío de valores
	/// </summary>
	/// <typeparam name="TFirst">Tipo del primer valor</typeparam>
	/// <typeparam name="TSecond">Tipo del segundo valor</typeparam>
	/// <typeparam name="TThird">Tipo del tercer valor</typeparam>
	/// <typeparam name="TFourth">Tipo del cuarto valor</typeparam>
	public class OQuartet<TFirst, TSecond, TThird, TFourth>
	{
		#region Atributo(s)
		/// <summary>
		/// Primer valor
		/// </summary>
		public TFirst First;
		/// <summary>
		/// Segundo valor
		/// </summary>
		public TSecond Second;
		/// <summary>
		/// Tercer valor
		/// </summary>
		public TThird Third;
		/// <summary>
		/// Cuarto valor
		/// </summary>
		public TFourth Fourth;
		#endregion

		#region Constructor de la clase
		/// <summary>
		/// Constructor de la clase
		/// </summary>
		public OQuartet()
		{
		}
		/// <summary>
		/// Constructor de la clase
		/// </summary>
		public OQuartet(TFirst first, TSecond second, TThird third, TFourth fourth)
		{
			this.First = first;
			this.Second = second;
			this.Third = third;
			this.Fourth = fourth;
		}
		#endregion
	}

	/// <summary>
	/// Clase utilizada para almacenar un trío de valores
	/// </summary>
	/// <typeparam name="TFirst">Tipo del primer valor</typeparam>
	/// <typeparam name="TSecond">Tipo del segundo valor</typeparam>
	/// <typeparam name="TThird">Tipo del tercer valor</typeparam>
	/// <typeparam name="TFourth">Tipo del cuarto valor</typeparam>
	/// <typeparam name="TFifth">Tipo del quinto valor</typeparam>
	public class OQuintet<TFirst, TSecond, TThird, TFourth, TFifth>
	{
		#region Atributo(s)
		/// <summary>
		/// Primer valor
		/// </summary>
		public TFirst First;
		/// <summary>
		/// Segundo valor
		/// </summary>
		public TSecond Second;
		/// <summary>
		/// Tercer valor
		/// </summary>
		public TThird Third;
		/// <summary>
		/// Cuarto valor
		/// </summary>
		public TFourth Fourth;
		/// <summary>
		/// Quinto valor
		/// </summary>
		public TFifth Fifth;
		#endregion

		#region Constructor de la clase
		/// <summary>
		/// Constructor de la clase
		/// </summary>
		public OQuintet()
		{
		}
		/// <summary>
		/// Constructor de la clase
		/// </summary>
		public OQuintet(TFirst first, TSecond second, TThird third, TFourth fourth, TFifth fifth)
		{
			this.First = first;
			this.Second = second;
			this.Third = third;
			this.Fourth = fourth;
			this.Fifth = fifth;
		}
		#endregion
	}

	#endregion
}