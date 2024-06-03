using System;

namespace Orbita.Utiles.Compresion
{
	/// <summary>
	/// Contiene información acerca del estado de retorno de una función
	/// </summary>
	public class ReturnInfo
	{
		#region Atributo/s
		/// <summary>
		/// Indica si la ejecución ha sido correcta
		/// </summary>
		public bool EjecucionCorrecta;
		/// <summary>
		/// En caso de que la ejecución haya sido incorrecta, se pasa la excepción para que la pueda tratar el código invocante de la función
		/// </summary>
		public Exception Excepcion;
		#endregion Atributo/s

		#region Contructor/es
		/// <summary>
		/// Constructor de la clase por defecto. Crea el objeto adecuado cuando la ejecución es correcta
		/// </summary>
		public ReturnInfo()
		{
			this.EjecucionCorrecta = true;
			this.Excepcion = null;
		}
		/// <summary>
		/// Constructor de la clase con parámetros. Creado especialmente para los casos en los que la ejecución es incorrecta, y pasarle la excpción. Si la ejecución es correcta no se hace caso al argumento de la excepción, siendo éste siempre nulo
		/// </summary>
		/// <param name="ejecucionCorrecta">Indica si la ejecución ha sido correcta o no</param>
		/// <param name="excepcion">La excepción producida en caso que no haya sido correcta la ejecución del método. Si la ejecución es correcta no se hace caso al argumento de la excepción, siendo éste siempre nulo.</param>
		public ReturnInfo(bool ejecucionCorrecta, Exception excepcion)
		{
			this.EjecucionCorrecta = ejecucionCorrecta;

			if (this.EjecucionCorrecta)
			{
				this.Excepcion = null;
			}
			else
			{
				this.Excepcion = excepcion;
			}
		}
		#endregion Contructor/es
	}
}
