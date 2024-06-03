//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 05-11-2012
// Description      : Adaptada a la utilización de los nuevos controles display
//                    Añadido nuevo campo a la BBDD (FrameIntervalMs)
//                    Modificado nombre de campo de la BBDD (MaxFrameIntervalMsVisualizacion)
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Orbita.VAComun;
using Orbita.VAControl;

namespace Orbita.VAHardware
{
	/// <summary>
	/// Clase estática para el acceso a las cámaras
	/// </summary>
	public static class CamaraRuntime
	{
		#region Atributo(s)
		/// <summary>
		/// Lista de las cámaras del sistema
		/// </summary>
		public static List<CamaraBase> ListaCamaras;

		/// <summary>
		/// Formulario de monitorización de cámaras
		/// </summary>
		public static FrmDisplays FrmMonitorizacionCamaras;
		#endregion

		#region Método(s) público(s)

		/// <summary>
		/// Construye los campos de la clase
		/// </summary>
		public static void Constructor()
		{
			ListaCamaras = new List<CamaraBase>();

			// Añadimos las cámaras al formulario
			DataTable dtCamaras = AppBD.GetCamaras();
			if (dtCamaras.Rows.Count > 0)
			{
				// Si hay alguna cámara visualizo el formulario de monitorización de cámaras
				FrmMonitorizacionCamaras = new FrmDisplays("Monitorización de cámaras");
				FrmMonitorizacionCamaras.Show();

				foreach (DataRow dr in dtCamaras.Rows)
				{
					string codCamara = dr["CodHardware"].ToString();
					string claseImplementadora = string.Format("{0}.{1}", Assembly.GetExecutingAssembly().GetName().Name, dr["ClaseImplementadora"].ToString());
					
					object objetoImplementado;
					if (App.ConstruirClase(Assembly.GetExecutingAssembly().GetName().Name, claseImplementadora, out objetoImplementado, codCamara))
					{
						CamaraBase camara = (CamaraBase)objetoImplementado;
						ListaCamaras.Add(camara);
						FrmMonitorizacionCamaras.AddDisplay(camara.Display);
                        
                        // Añado propiedades especificas a los displays para su visualización
                        camara.Display.MostrarBtnMaximinzar = true;
                        camara.Display.MostrarBtnSiguienteAnterior = dtCamaras.Rows.Count > 1;
					}
				}
			}
		}

		/// <summary>
		/// Destruye los campos de la clase
		/// </summary>
		public static void Destructor()
		{
			if (FrmMonitorizacionCamaras != null)
			{
				FrmMonitorizacionCamaras.Close();
				FrmMonitorizacionCamaras = null;
			}

			ListaCamaras = null;
		}

		/// <summary>
		/// Inicializa las propieades de la clase
		/// </summary>
		public static void Inicializar()
		{
			foreach (CamaraBase camara in ListaCamaras)
			{
				camara.Inicializar();
			}
		}

		/// <summary>
		/// Finaliza las propiedades de la clase
		/// </summary>
		public static void Finalizar()
		{
			foreach (CamaraBase camara in ListaCamaras)
			{
				camara.Finalizar();
			}
		}

		/// <summary>
		/// Busca la cámara con el código indicado
		/// </summary>
		/// <param name="codigo">Código de la cámara</param>
		public static CamaraBase GetCamara(string codigo)
		{
			foreach (CamaraBase camara in ListaCamaras)
			{
				if (camara.Codigo == codigo)
				{
					return camara;
				}
			}
			return null;
		}

		/// <summary>
		/// Comienza una reproducción continua de la cámara
		/// </summary>
		/// <returns></returns>
		public static bool Start(string codigo)
		{
			foreach (CamaraBase camara in ListaCamaras)
			{
				if (camara.Codigo == codigo)
				{
					return camara.Start();
				}
			}
			return false;
		}

		/// <summary>
		/// Termina una reproducción continua de la cámara
		/// </summary>
		/// <returns></returns>
		public static bool Stop(string codigo)
		{
			foreach (CamaraBase camara in ListaCamaras)
			{
				if (camara.Codigo == codigo)
				{
					return camara.Stop();
				}
			}
			return false;
		}

		/// <summary>
		/// Comienza una reproducción de todas las cámaras
		/// </summary>
		/// <returns></returns>
		public static bool StartAll()
		{
			bool resultado = true;
			foreach (CamaraBase camara in ListaCamaras)
			{
				resultado &= camara.Start();
			}
			return resultado;
		}

		/// <summary>
		/// Termina la reproducción de todas las cámaras
		/// </summary>
		/// <returns></returns>
		public static bool StopAll()
		{
			foreach (CamaraBase camara in ListaCamaras)
			{
				return camara.Stop();
			}
			return false;
		}

		/// <summary>
		/// Realiza una fotografía de forma sincrona
		/// </summary>
		/// <returns></returns>
		public static bool Snap(string codigo)
		{
			foreach (CamaraBase camara in ListaCamaras)
			{
				if (camara.Codigo == codigo)
				{
					return camara.Snap();
				}
			}
			return false;
		}

		/// <summary>
		/// Establece la cámara en modo simulación de una fotografía
		/// </summary>
		/// <param name="rutaFotografias">Indica la ruta de la fotografía o de la carpeta de fotografías donde están las imágenes a simular</param>
		public static void IniciarModoSimulacionFotografia(string codigo, string rutaFotografias)
		{
			foreach (CamaraBase camara in ListaCamaras)
			{
				if (camara.Codigo == codigo)
				{
					camara.ConfigurarModoSimulacionFotografia(rutaFotografias);
				}
			}
		}

		/// <summary>
		/// Establece la cámara en modo simulación de una carpeta de fotografías
		/// </summary>
		/// <param name="rutaFotografias">Indica la ruta de la fotografía o de la carpeta de fotografías donde están las imágenes a simular</param>
		public static void IniciarModoSimulacionFotografia(string codigo, string rutaFotografias, string filtro, int cadenciaSimulacionEnGrabacionMS)
		{
			foreach (CamaraBase camara in ListaCamaras)
			{
				if (camara.Codigo == codigo)
				{
					camara.ConfigurarModoSimulacionDirectorio(rutaFotografias, filtro, cadenciaSimulacionEnGrabacionMS);
				}
			}
		}

		/// <summary>
		/// Para el modo simulación de la cámara, restableciendo su funcionamiento normal
		/// </summary>
		public static void PararModoSimulacion(string codigo)
		{
			foreach (CamaraBase camara in ListaCamaras)
			{
				if (camara.Codigo == codigo)
				{
					camara.PararModoSimulacion();
				}
			}
		}

		/// <summary>
		/// Visualiza una imagen en el display
		/// </summary>
		/// <param name="imagen">Imagen a visualizar</param>
		public static void VisualizarImagen(string codigo, OrbitaImage imagen)
		{
			foreach (CamaraBase camara in ListaCamaras)
			{
				if (camara.Codigo == codigo)
				{
					camara.VisualizarImagen(imagen);
				}
			}
		}

		/// <summary>
		/// Visualiza una imagen en el display
		/// </summary>
		/// <param name="imagen">Imagen a visualizar</param>
		/// <param name="graficos">Objeto que contiene los gráficos a visualizar (letras, rectas, circulos, etc)</param>
		public static void VisualizarImagen(string codigo, OrbitaImage imagen, OrbitaGrafico graficos)
		{
			foreach (CamaraBase camara in ListaCamaras)
			{
				if (camara.Codigo == codigo)
				{
					camara.VisualizarImagen(imagen, graficos);
				}
			}
		}

		/// <summary>
		/// Visualiza la última imagen capturada por la cámara
		/// </summary>
		public static void VisualizarUltimaImagen(string codigo)
		{
			foreach (CamaraBase camara in ListaCamaras)
			{
				if (camara.Codigo == codigo)
				{
					camara.VisualizarUltimaImagen();
				}
			}
		}

		/// <summary>
		/// Visualiza la última imagen capturada por la cámara
		/// </summary>
		/// <param name="graficos">Objeto que contiene los gráficos a visualizar (letras, rectas, circulos, etc)</param>
		public static void VisualizarUltimaImagen(string codigo, OrbitaGrafico graficos)
		{
			foreach (CamaraBase camara in ListaCamaras)
			{
				if (camara.Codigo == codigo)
				{
					camara.VisualizarUltimaImagen(graficos);
				}
			}
		}

		#endregion
	}

	/// <summary>
	/// Clase base para todas las cámaras
	/// </summary>
	public class CamaraBase : IHardware
	{
		#region Atributo(s)
		/// <summary>
		/// Objeto de simulación de la cámara
		/// </summary>
		public SimulacionCamara SimulacionCamara;
		/// <summary>
		/// Indica si esta grabando
		/// </summary>
		public bool Recording;
		/// <summary>
		/// Proporciona herramientas para medir la velocidad de adquisición de la cámara
		/// </summary>
		public MedidorVelocidadAdquisicion MedidorVelocidadAdquisicion;
		#endregion

		#region Propiedad(es)
		/// <summary>
		/// Código identificador de la cámara
		/// </summary>
		private string _Codigo;
		/// <summary>
		/// Código identificador de la cámara
		/// </summary>
		public string Codigo
		{
			get { return _Codigo; }
			set { _Codigo = value; }
		}

		/// <summary>
		/// Nombre identificativo de la cámara
		/// </summary>
		private string _Nombre;
		/// <summary>
		/// Nombre identificativo de la cámara
		/// </summary>
		public string Nombre
		{
			get { return _Nombre; }
			set { _Nombre = value; }
		}

		/// <summary>
		/// Descripción de la cámara
		/// </summary>
		private string _Descripcion;
		/// <summary>
		/// Descripción de la cámara
		/// </summary>
		public string Descripcion
		{
			get { return _Descripcion; }
			set { _Descripcion = value; }
		}

		/// <summary>
		/// Habilita o deshabilita el funcionamiento
		/// </summary>
		private bool _Habilitado;
		/// <summary>
		/// Habilita o deshabilita el funcionamiento
		/// </summary>
		public bool Habilitado
		{
			get { return _Habilitado; }
		}

		/// <summary>
		/// Tipo de hardware
		/// </summary>
		public TipoHardware TipoHardware
		{
			get { return TipoHardware.HwCamara; }
		}

		/// <summary>
		/// Tipo de cámara
		/// </summary>
		private TipoCamara _TipoCamara;
		/// <summary>
		/// Tipo de cámara
		/// </summary>
		public TipoCamara TipoCamara
		{
			get { return _TipoCamara; }
			set { _TipoCamara = value; }
		}

		/// <summary>
		/// Código del tipo de cámara
		/// </summary>
		private string _CodigoTipoCamara;
		/// <summary>
		/// Código del tipo de cámara
		/// </summary>
		public string CodigoTipoCamara
		{
			get { return _CodigoTipoCamara; }
			set { _CodigoTipoCamara = value; }
		}

		/// <summary>
		/// Fabricante del tipo de cámara
		/// </summary>
		private string _Fabricante;
		/// <summary>
		/// Fabricante del tipo de cámara
		/// </summary>
		public string Fabricante
		{
			get { return _Fabricante; }
			set { _Fabricante = value; }
		}

		/// <summary>
		/// Modelo del tipo de cámara
		/// </summary>
		private string _Modelo;
		/// <summary>
		/// Modelo del tipo de cámara
		/// </summary>
		public string Modelo
		{
			get { return _Modelo; }
			set { _Modelo = value; }
		}

		/// <summary>
		/// Descripcion del tipo de cámara
		/// </summary>
		private string _DescripcionTipoCamara;
		/// <summary>
		/// Descripcion del tipo de cámara
		/// </summary>
		public string DescripcionTipoCamara
		{
			get { return _DescripcionTipoCamara; }
			set { _DescripcionTipoCamara = value; }
		}

		/// <summary>
		/// Comienza o termina una reproducción de la cámara (sin guardar grabación)
		/// </summary>
		private bool _Grab;
		/// <summary>
		/// Comienza o termina una reproducción de la cámara (sin guardar grabación)
		/// </summary>
		public bool Grab
		{
			get
			{
				return _Grab;
			}
			set
			{
				if (value)
				{
					this.Start();
				}
				else
				{
					this.Stop();
				}
			}
		}

		/// <summary>
		/// Código de la variable que guarda la imagen
		/// </summary>
		private string _CodVariableImagen;
		/// <summary>
		/// Código de la variable que guarda la imagen
		/// </summary>
		public string CodVariableImagen
		{
			get { return _CodVariableImagen; }
			set { _CodVariableImagen = value; }
		}

		/// <summary>
		/// Código de la variable que fuerza el snap
		/// </summary>
		private string _CodVariableSnap;
		/// <summary>
		/// Código de la variable que fuerza el snap
		/// </summary>
		public string CodVariableSnap
		{
			get { return _CodVariableSnap; }
			set { _CodVariableSnap = value; }
		}

		/// <summary>
		/// Resolución de la cámara
		/// </summary>
		private Point _Resolucion;
		/// <summary>
		/// Resolución de la cámara
		/// </summary>
		public Point Resolucion
		{
			get { return _Resolucion; }
			set { _Resolucion = value; }
		}

		/// <summary>
		/// Tipo de cámara dependiendo de si sus imágenes son a color o monocromáticas
		/// </summary>
		private TipoColorPixel _Color;
		/// <summary>
		/// Tipo de cámara dependiendo de si sus imágenes son a color o monocromáticas
		/// </summary>
		public TipoColorPixel Color
		{
			get { return _Color; }
			set { _Color = value; }
		}

		/// <summary>
		/// Indica que se ha podido acceder a la cámara con éxito
		/// </summary>
		private bool _Existe;
		/// <summary>
		/// Indica que se ha podido acceder a la cámara con éxito
		/// </summary>
		public bool Existe
		{
			get { return _Existe; }
			set { _Existe = value; }
		}

		/// <summary>
		/// Estado de la conexión
		/// </summary>
		private EstadoConexion _EstadoConexion;
		/// <summary>
		/// Estado de la conexión
		/// </summary>
		public EstadoConexion EstadoConexion
		{
			get { return _EstadoConexion; }
			set
			{
				if (this._EstadoConexion != value)
				{
					// Información extra
					LogsRuntime.Debug(ModulosHardware.Camaras, this.Codigo, "Estado de la conexión: " + value.ToString());

					// La cámara pasa de desconectada a conectada
					if ((this._EstadoConexion == EstadoConexion.Desconectado) && (value == EstadoConexion.Conectado))
					{
						this.ImagenActual = this.NuevaImagen();
						this.ImagenActual.ConvertFromBitmap(global::Orbita.VAHardware.Properties.Resources.CamaraConectada);
						this.VisualizarImagen(this.ImagenActual);
						this.Display.ZoomFull();
                        this.Display.MostrarMensaje("Cámara conectada");
					}

					// La cámara pasa de conectada a desconectada
					if ((this._EstadoConexion == EstadoConexion.Conectado) && (value == EstadoConexion.Desconectado))
					{
						this.ImagenActual = this.NuevaImagen();
						this.ImagenActual.ConvertFromBitmap(global::Orbita.VAHardware.Properties.Resources.CamaraDesConectada);
						this.VisualizarImagen(this.ImagenActual);
                        this.Display.ZoomFull();
                        this.Display.MostrarMensaje("Cámara desconectada");
                    }

					// La cámara pasa de conectada a error de conexión
					if (value == EstadoConexion.ErrorConexion)
					{
                        // Paramos la cámara
                        this.Stop();
                        this.Desconectar();
                        // Mostramos una imagen de error
                        this.ImagenActual = this.NuevaImagen();
						this.ImagenActual.ConvertFromBitmap(global::Orbita.VAHardware.Properties.Resources.CamaraDesConectada);
						this.VisualizarImagen(this.ImagenActual);
                        this.Display.ZoomFull();
                        this.Display.MostrarMensaje("Error de conexión");
                        // Iniciamos el protocolo de reconexión
                        this.IniciarProtocoloReconexion();
                    }

					// La cámara pasa de error de conexión a conectada
					if ((this._EstadoConexion == EstadoConexion.ErrorConexion) && (value == EstadoConexion.Conectado))
					{
						this.ImagenActual = this.NuevaImagen();
						this.ImagenActual.ConvertFromBitmap(global::Orbita.VAHardware.Properties.Resources.CamaraConectada);
						this.VisualizarImagen(this.ImagenActual);
                        this.Display.ZoomFull();
                        this.Display.MostrarMensaje("Cámara conectada");
                    }

					// La cámara pasa de error de conexión a desconectada
					if ((this._EstadoConexion == EstadoConexion.ErrorConexion) && (value == EstadoConexion.Desconectado))
					{
						this.ImagenActual = this.NuevaImagen();
						this.ImagenActual.ConvertFromBitmap(global::Orbita.VAHardware.Properties.Resources.CamaraDesConectada);
						this.VisualizarImagen(this.ImagenActual);
                        this.Display.ZoomFull();
                        this.Display.MostrarMensaje("Cámara desconectada");
                    }

					this._EstadoConexion = value;
				}
			}
		}

		/// <summary>
		/// Última imagen capturada
		/// </summary>
		protected OrbitaImage _ImagenActual;
		/// <summary>
		/// Propieadad a heredar donde se accede a la imagen
		/// </summary>
		public virtual OrbitaImage ImagenActual
		{
			get { return this._ImagenActual; }
			set { this._ImagenActual = value; }
		}

		/// <summary>
		/// Indica que el control ha de visualizar automáticamente la imagen que devuelve la cámara,
		/// de lo contrario se deberá de especificar el momento de la visualización por código.
		/// </summary>
		private bool _VisualizacionEnVivo;
		/// <summary>
		/// Indica que el control ha de visualizar automáticamente la imagen que devuelve la cámara,
		/// de lo contrario se deberá de especificar el momento de la visualización por código.
		/// </summary>
		public bool VisualizacionEnVivo
		{
			get { return _VisualizacionEnVivo; }
			set { _VisualizacionEnVivo = value; }
		}

		/// <summary>
		/// Control display asociado a la cámara
		/// </summary>
		protected CtrlDisplay _Display;
		/// <summary>
		/// Control display asociado a la cámara
		/// </summary>
		public CtrlDisplay Display
		{
			get { return _Display; }
			set { _Display = value; }
		}

		/// <summary>
		/// Lista de todos los terminales de la tarjeta de IO
		/// </summary>              
		protected List<TerminalIOBase> _ListaTerminales;
		/// <summary>
		/// Lista de todos los terminales de la tarjeta de IO
		/// </summary>              
		public List<TerminalIOBase> ListaTerminales
		{
			get { return _ListaTerminales; }
			set { _ListaTerminales = value; }
		}

		/// <summary>
		/// Indica que se ha de cambiar la variable asociada cada vez que se recibe una fotografía
		/// </summary>
		private bool _LanzarEventoAlSnap;
		/// <summary>
		/// Indica que se ha de cambiar la variable asociada cada vez que se recibe una fotografía
		/// </summary>
		public bool LanzarEventoAlSnap
		{
			get { return _LanzarEventoAlSnap; }
			set { _LanzarEventoAlSnap = value; }
		}

        /// <summary>
        /// Indica el intervalo de tiempo en ms. de adquisición de imagenes
        /// </summary>
        private double _FrameInterval;
        /// <summary>
        /// Indica el intervalo de tiempo en ms. de adquisición de imagenes
        /// </summary>
        public double FrameInterval
        {
            get { return _FrameInterval; }
            set { _FrameInterval = value; }
        }
        

        /// <summary>
        /// Indica el límite máximo de visualización de imagenes y que por lo tanto se ha de visualizar de forma retrasada con un timer
        /// </summary>
        private double _MaxFrameIntervalVisualizacion;
        /// <summary>
        /// Indica el límite máximo de visualización de imagenes y que por lo tanto se ha de visualizar de forma retrasada con un timer
        /// </summary>
        public double MaxFrameIntervalVisualizacion
        {
            get { return _MaxFrameIntervalVisualizacion; }
            set { _MaxFrameIntervalVisualizacion = value; }
        }
        
		#endregion

		#region Constructor(es)
		/// <summary>
		/// Constructor de la clase
		/// </summary>
		public CamaraBase(string codigo)
		{
			try
			{
				//Inicializamos los valores por defecto
				this._Codigo = codigo;
				this._ListaTerminales = new List<TerminalIOBase>();
				this._Existe = false;
				this._EstadoConexion = EstadoConexion.Desconectado;
				this._VisualizacionEnVivo = false;
				this.SimulacionCamara = new SimulacionCamara(codigo);
				this.SimulacionCamara.OnSnapSimulado += new DelegadoSnapSimulado(SnapSimulado);
				this.Recording = false;
				this.MedidorVelocidadAdquisicion = new MedidorVelocidadAdquisicion();


				DataTable dt = AppBD.GetCamara(codigo);
				if (dt.Rows.Count == 1)
				{
					this._Nombre = dt.Rows[0]["NombreHardware"].ToString();
					this._Descripcion = dt.Rows[0]["DescHardware"].ToString();
					this._Habilitado = (bool)dt.Rows[0]["HabilitadoHardware"];
					this._TipoCamara = (TipoCamara)App.EnumParse(typeof(TipoCamara), dt.Rows[0]["CodTipoHardware"].ToString(), TipoCamara.VProBasler);
					this._CodigoTipoCamara = dt.Rows[0]["CodTipoHardware"].ToString();
					this._Fabricante = dt.Rows[0]["Fabricante"].ToString();
					this._Modelo = dt.Rows[0]["Modelo"].ToString();
					this._DescripcionTipoCamara = dt.Rows[0]["DescTipoHardware"].ToString();
					this._CodVariableImagen = dt.Rows[0]["CodVariableImagen"].ToString();
					this._LanzarEventoAlSnap = App.EvaluaBooleano(dt.Rows[0]["LanzarEventoAlSnap"], false);
					this._CodVariableSnap = dt.Rows[0]["CodVariableSnap"].ToString();
					this._Resolucion.X = App.EvaluaNumero(dt.Rows[0]["ResolucionX"], 1, 100000, 1024);
					this._Resolucion.Y = App.EvaluaNumero(dt.Rows[0]["ResolucionY"], 1, 100000, 768);
					this._Color = (TipoColorPixel)App.EvaluaNumero(dt.Rows[0]["Color"], 0, 1, 0);
                    this._FrameInterval = App.EvaluaNumero(dt.Rows[0]["FrameIntervalMs"], 0.0, 1000.0, 1.0);
                    this._MaxFrameIntervalVisualizacion = App.EvaluaNumero(dt.Rows[0]["MaxFrameIntervalMsVisualizacion"], 0.0, 1000.0, 0.0);

					string titulo = this._Nombre + " [" + this._Fabricante + "]";

                    string ensambladoClaseImplementadora = dt.Rows[0]["EnsambladoClaseImplementadora"].ToString();
                    string claseImplementadoraDisplay = string.Format("{0}.{1}", ensambladoClaseImplementadora, dt.Rows[0]["ClaseImplementadoraDisplay"].ToString());
					object objetoImplementado;
                    if (App.ConstruirClase(ensambladoClaseImplementadora, claseImplementadoraDisplay, out objetoImplementado, titulo, this._Codigo, this._MaxFrameIntervalVisualizacion, string.Empty, string.Empty))
					{
						this.Display = (CtrlDisplay)objetoImplementado;

                        // Añado propiedades especificas a los displays para su visualización
                        this.Display.MostrarBtnAbrir = false;
                        this.Display.MostrarBtnGuardar = true;
                        this.Display.MostrarBtnInfo = true;
                        this.Display.OnInfoDemandada += this.OnInfoDemandada;
                        this.Display.MostrarStatusMensaje = true;
                    }
				}
				else
				{
					throw new Exception("No se ha podido cargar la información de la cámara " + codigo + " \r\nde la base de datos.");
				}
			}
			catch (Exception exception)
			{
				LogsRuntime.Fatal(ModulosHardware.Camaras, this.Codigo, exception);
				throw new Exception("Imposible iniciar la cámara " + this.Codigo);
			}
		}
		#endregion

		#region Método(s) público(s)
		/// <summary>
		/// Establece la cámara en modo simulación
		/// </summary>
		/// <param name="rutaFotografias">Indica la ruta de la fotografía o de la carpeta de fotografías donde están las imágenes a simular</param>
		public void ConfigurarModoSimulacionFotografia(string rutaFotografias)
		{
			this.SimulacionCamara.ConfigurarModoSimulacionFotografia(rutaFotografias);
		}

		/// <summary>
		/// Establece la cámara en modo simulación
		/// </summary>
		/// <param name="rutaFotografias">Indica la ruta de la fotografía o de la carpeta de fotografías donde están las imágenes a simular</param>
		/// <param name="filtro">Cadena de filtrado de las fotografías situadas en la caperta establecida</param>
		/// <param name="cadenciaSimulacionEnGrabacionMs">Intervalo en milisegundos utilizado para simular la grabación. Se realiza una fotografía según el tiempo indicado.</param>
		public void ConfigurarModoSimulacionDirectorio(string rutaFotografias, string filtro, int cadenciaSimulacionEnGrabacionMs)
		{
			this.SimulacionCamara.ConfigurarModoSimulacionDirectorio(rutaFotografias, filtro, cadenciaSimulacionEnGrabacionMs);
		}

		/// <summary>
		/// Para el modo simulación de la cámara, restableciendo su funcionamiento normal
		/// </summary>
		public void PararModoSimulacion()
		{
			if (this.SimulacionCamara.ConfigurarModoReal())
			{
				this.Stop();
			}
		}

		/// <summary>
		/// Comienza una reproducción continua de la cámara
		/// </summary>
		/// <returns></returns>
		public bool Start()
		{
            if (this.Habilitado && (this.EstadoConexion == VAHardware.EstadoConexion.Conectado))
            {
                // Información extra
                LogsRuntime.Debug(ModulosHardware.Camaras, this.Codigo, "Start de la cámara: " + this.Codigo);

                VariableRuntime.CrearSuscripcion(this._CodVariableSnap, "Camaras", this.Codigo, ComandoSnapPorVariable);

                this._Grab = true;
                if (!this.SimulacionCamara.Simulacion)
                {
                    return this.InternalStart();
                }
                else
                {
                    this.SimulacionCamara.IniciarSimulacionGrabacion();
                    return true;
                }
            }
            return false;
		}

		/// <summary>
		/// Termina una reproducción continua de la cámara
		/// </summary>
		/// <returns></returns>
		public bool Stop()
		{
            if (this.Habilitado)
            {
                // Información extra
                LogsRuntime.Debug(ModulosHardware.Camaras, this.Codigo, "Stop de la cámara: " + this.Codigo);

                VariableRuntime.EliminarSuscripcion(this._CodVariableSnap, "Camaras", this.Codigo, ComandoSnapPorVariable);

                this._Grab = false;
                if (!this.SimulacionCamara.Simulacion)
                {
                    return this.InternalStop();
                }
                else
                {
                    this.SimulacionCamara.ParaSimulacionGrabacion();
                    return true;
                }
            }
            return false;
        }

		/// <summary>
		/// Realiza una fotografía de forma sincrona
		/// </summary>
		/// <returns></returns>
		public virtual bool Snap()
		{
			bool resultado = false;

            if (this.Habilitado && (this.EstadoConexion == VAHardware.EstadoConexion.Conectado))
            {
                if (!this.SimulacionCamara.Simulacion)
                { // Modo de funcionamiento normal
                    resultado = this.InternalSnap();
                }
                else
                { // Modo de funcionamiento en modo simulación (por ahora de 1 sola fotografía)
                    this.SimulacionCamara.EjecutarSimulacion();
                    resultado = true;
                }

                // Información extra
                LogsRuntime.Debug(ModulosHardware.Camaras, this.Codigo, "Snap de la cámara: " + this.Codigo + ". Resultado: " + resultado.ToString());
            }

			return resultado;
		}

		/// <summary>
		/// Visualiza una imagen en el display
		/// </summary>
		/// <param name="imagen">Imagen a visualizar</param>        
		public void VisualizarImagen(OrbitaImage imagen)
		{
			this.VisualizarImagen(imagen, null);
		}

		/// <summary>
		/// Visualiza la última imagen capturada por la cámara
		/// </summary>
		public void VisualizarUltimaImagen()
		{
			this.VisualizarImagen(this.ImagenActual, null);
		}

		/// <summary>
		/// Visualiza la última imagen capturada por la cámara
		/// </summary>
		/// <param name="graficos">Objeto que contiene los gráficos a visualizar (letras, rectas, circulos, etc)</param>
		public void VisualizarUltimaImagen(OrbitaGrafico graficos)
		{
			this.VisualizarImagen(this.ImagenActual, graficos);
		}

		/// <summary>
		/// Comienza una grabación continua de la cámara
		/// </summary>
		/// <param name="fichero">Ruta y nombre del fichero que contendra el video</param>
		/// <returns></returns>
		public bool REC(string fichero)
		{
            if (this.Habilitado && (this.EstadoConexion == VAHardware.EstadoConexion.Conectado))
            {
                // Información extra
                LogsRuntime.Debug(ModulosHardware.Camaras, this.Codigo, "REC de la cámara: " + this.Codigo);

                if (!this.SimulacionCamara.Simulacion)
                {
                    this.Recording = true;
                    return this.InternalREC(fichero);
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

		/// <summary>
		/// Termina una grabación continua de la cámara
		/// </summary>
		/// <returns></returns>
        public bool StopREC()
        {
            if (this.Habilitado && (this.EstadoConexion == VAHardware.EstadoConexion.Conectado))
            {
                // Información extra
                LogsRuntime.Debug(ModulosHardware.Camaras, this.Codigo, "StopREC de la cámara: " + this.Codigo);

                if (!this.SimulacionCamara.Simulacion)
                {
                    return this.InternalStopREC();
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
		#endregion

		#region Método(s) privado(s)
		/// <summary>
		/// Establece el valor de la imagen a la variable asociada
		/// </summary>
		/// <param name="imagen">Imagen a establecer en la variable</param>
		protected void EstablecerVariableAsociada(OrbitaImage imagen)
		{
			//OrbitaImage nuevaImagen = imagen).Clone(); // Tal vez no sea necesario!!

			// Se asigna el valor de la variable asociada
			VariableRuntime.SetValue(this.CodVariableImagen, imagen, "Camaras", this.Codigo);
		}

		/// <summary>
		/// Simulación de la realización de un snap
		/// </summary>
		protected bool SnapSimulado(string rutaFotografia)
		{
			OrbitaImage imagen;

			// Se carga la fotografía de disco
			bool resultado = this.CargarImagenDeDisco(out imagen, rutaFotografia);

			// Se asigna el valor de la variable asociada
			if (resultado)
			{
				this.EstablecerVariableAsociada(imagen);
			}

			return resultado;
		}
		#endregion

		#region Método(s) virtual(es)
		/// <summary>
		/// Carga los valores de la cámara
		/// </summary>
		public virtual void Inicializar()
		{
			if (this.Habilitado)
			{
				if (!this.Conectar())
				{
					LogsRuntime.Error(ModulosHardware.Camaras, "Inicialización", "Ha sido imposible realizar la conexión de la cámara " + this.Nombre);
				}
			}
		}

		/// <summary>
		/// Finaliza la cámara
		/// </summary>
		public virtual void Finalizar()
		{
			if (this.Habilitado)
			{
				if (this.Desconectar())
				{
					if (this.ImagenActual != null)
					{
						this.ImagenActual = null;
					}
				}
				else
				{
					LogsRuntime.Error(ModulosHardware.Camaras, "Finalización", "Ha sido imposible realizar la desconexión de la cámara " + this.Nombre);
				}
			}
		}

		/// <summary>
		/// Se toma el control de la cámara
		/// </summary>
		/// <returns>Verdadero si la operación ha funcionado correctamente</returns>
		public virtual bool Conectar()
		{
			// Información extra
			LogsRuntime.Debug(ModulosHardware.Camaras, this.Codigo, "Conexión de la cámara: " + this.Codigo);

			return false;
		}

		/// <summary>
		/// Se deja el control de la cámara
		/// </summary>
		/// <returns>Verdadero si la operación ha funcionado correctamente</returns>
		public virtual bool Desconectar()
		{
			// Información extra
			LogsRuntime.Debug(ModulosHardware.Camaras, this.Codigo, "Desconexión de la cámara: " + this.Codigo);

			return false;
		}

		/// <summary>
		/// Comienza una reproducción continua de la cámara
		/// </summary>
		/// <returns></returns>
		protected virtual bool InternalStart()
		{
			this._Grab = true;

			this.MedidorVelocidadAdquisicion.ResetearMediciones();

			// Método implementado en las clases hijas
			return true;
		}

		/// <summary>
		/// Termina una reproducción continua de la cámara
		/// </summary>
		/// <returns></returns>
		protected virtual bool InternalStop()
		{
			this._Grab = false;

			// Método implementado en las clases hijas
			return true;
		}

		/// <summary>
		/// Realiza una fotografía de forma sincrona
		/// </summary>
		/// <returns></returns>
		protected virtual bool InternalSnap()
		{
			this.MedidorVelocidadAdquisicion.ResetearMediciones();

			// Método implementado en las clases hijas
			return true;
		}

		/// <summary>
		/// Carga una imagen de disco
		/// </summary>
		/// <param name="ruta">Indica la ruta donde se encuentra la fotografía</param>
		/// <returns>Devuelve verdadero si la operación se ha realizado con éxito</returns>
		public virtual bool CargarImagenDeDisco(out OrbitaImage imagen, string ruta)
		{
			// Método implementado en las clases hijas
			imagen = null;
			return false;
		}

		/// <summary>
		/// Guarda una imagen en disco
		/// </summary>
		/// <param name="ruta">Indica la ruta donde se ha de guardar la fotografía</param>
		/// <returns>Devuelve verdadero si la operación se ha realizado con éxito</returns>
		public virtual bool GuardarImagenADisco(string ruta)
		{
			// Método implementado en las clases hijas
			return false;
		}

		/// <summary>
		/// Carga un grafico de disco
		/// </summary>
		/// <param name="ruta">Indica la ruta donde se encuentra el grafico</param>
		/// <returns>Devuelve verdadero si la operación se ha realizado con éxito</returns>
		public virtual bool CargarGraficoDeDisco(out OrbitaGrafico grafico, string ruta)
		{
			// Método implementado en las clases hijas
			grafico = null;
			return false;
		}

		/// <summary>
		/// Guarda un objeto gráfico en disco
		/// </summary>
		/// <param name="ruta">Indica la ruta donde se ha de guardar el objeto gráfico</param>
		/// <returns>Devuelve verdadero si la operación se ha realizado con éxito</returns>
		public virtual bool GuardarGraficoADisco(OrbitaGrafico graficos, string ruta)
		{
			// Método implementado en las clases hijas
			return false;
		}

		/// <summary>
		/// Devuelve una nueva imagen del tipo adecuado al trabajo con la cámara
		/// </summary>
		/// <returns>Imagen del tipo adecuado al trabajo con la cámara</returns>
		public virtual OrbitaImage NuevaImagen()
		{
			return null;
		}

		/// <summary>
		/// Devuelve un nuevo gráfico del tipo adecuado al trabajo con el display
		/// </summary>
		/// <returns>Grafico del tipo adecuado al trabajo con el display</returns>
		public virtual OrbitaGrafico NuevoGrafico()
		{
			return null;
		}

		/// <summary>
		/// Visualiza una imagen en el display
		/// </summary>
		/// <param name="imagen">Imagen a visualizar</param>
		/// <param name="graficos">Objeto que contiene los gráficos a visualizar (letras, rectas, circulos, etc)</param>
		public virtual void VisualizarImagen(OrbitaImage imagen, OrbitaGrafico graficos)
		{
		}

		/// <summary>
		/// Se inicia el protocolo de reconexión
		/// </summary>
		public virtual void IniciarProtocoloReconexion()
		{
		}

		/// <summary>
		/// Comienza una grabacion continua de la cámara
		/// </summary>
		/// <param name="fichero">Ruta y nombre del fichero que contendra el video</param>
		/// <returns></returns>
		protected virtual bool InternalREC(string fichero)
		{
			// Método implementado en las clases hijas
			return true;
		}

		/// <summary>
		/// Termina una grabación continua de la cámara
		/// </summary>
		/// <returns></returns>
		protected virtual bool InternalStopREC()
		{
			this.Recording = false;

			// Método implementado en las clases hijas
			return true;
		}

        /// <summary>
        /// Muestra la ventana de información del dispositivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void OnInfoDemandada(object sender, EventArgs e)
        {
            FrmDetalleCamara frmDetalleCam = new FrmDetalleCamara(this.Codigo);
            frmDetalleCam.Show();
        }
		#endregion

		#region Eventos
		/// <summary>
		/// Evento de snap realizado por una variable
		/// </summary>
		protected void ComandoSnapPorVariable()
		{
			this.Snap();
		}
		#endregion
	}

	/// <summary>
	/// Enumerado que identifica a los tipos de cámaras
	/// </summary>
	public enum TipoCamara
	{
		/// <summary>
		/// Cámara tipo Axis  con firmware 4
		/// </summary>
		Axis = 1,

		/// <summary>
		/// Cámara tipo Pilot de Basler. Mediante driver de Vision Pro
		/// </summary>
		VProBasler = 2
	}

	/// <summary>
	/// Tipo de la cámara (mono o RGB)
	/// </summary>
	public enum TipoColorPixel
	{
		/// <summary>
		/// Cámara monocromática
		/// </summary>
		Monocromatica = 0,
		/// <summary>
		/// Cámara a color RGB
		/// </summary>
		RGB = 1
	}

	#region Acceso a los parámeteros internos de las cámaras
	/// <summary>
	/// Acceso a una característica de la cámara de tipo string
	/// </summary>
	public interface iCamFeature
	{
		#region Método(s)
		/// <summary>
		/// Aplica el valor de memoria a la cámara
		/// </summary>
		bool Send();
		/// <summary>
		/// Consulta el valor de la cámara y lo guarda en memoria
		/// </summary>
		bool Receive();
		#endregion
	}

	#endregion

	#region Clases para la simulación de las cámaras
	/// <summary>
	/// Clase que guarda las propiedades de la simulación de una cámara
	/// </summary>
	public class SimulacionCamara
	{
		#region Atributo(s)
		/// <summary>
		/// Listado de las fotografías situadas en el directorio
		/// </summary>
		private List<string> ListaRutaFotografias;
		/// <summary>
		/// Indica la fotografía actual en la lista de fotografías
		/// </summary>
		private int IndiceFotografia;
		/// <summary>
		/// Timer que se utiliza para realizar la simulación de la grabación
		/// </summary>
		private Timer TimerSimulacionGrab;
		#endregion

		#region Propiedad(es)
		/// <summary>
		/// Código identificador de la cámara
		/// </summary>
		private string _Codigo;
		/// <summary>
		/// Código identificador de la cámara
		/// </summary>
		public string Codigo
		{
			get { return _Codigo; }
			set { _Codigo = value; }
		}

		/// <summary>
		/// Indica si la cámara está en modo simulación
		/// </summary>
		private bool _Simulacion;
		/// <summary>
		/// Indica si la cámara está en modo simulación
		/// </summary>
		public bool Simulacion
		{
			get { return _Simulacion; }
			set { _Simulacion = value; }
		}

		/// <summary>
		/// Indica el tipo de simulación que se realizará en la cámara (con una única foto o con toda una carpeta de fotos)
		/// </summary>
		private TipoSimulacionCamara _TipoSimulacion;
		/// <summary>
		/// Indica el tipo de simulación que se realizará en la cámara (con una única foto o con toda una carpeta de fotos)
		/// </summary>
		public TipoSimulacionCamara TipoSimulacion
		{
			get { return _TipoSimulacion; }
			set { _TipoSimulacion = value; }
		}

		/// <summary>
		/// Indica la ruta de la fotografía o de la carpeta de fotografías donde están las imágenes a simular
		/// </summary>
		private string _RutaFotografias;
		/// <summary>
		/// Indica la ruta de la fotografía o de la carpeta de fotografías donde están las imágenes a simular
		/// </summary>
		public string RutaFotografias
		{
			get { return _RutaFotografias; }
			set { _RutaFotografias = value; }
		}

		/// <summary>
		/// Intervalo de tiempo entre snaps de grabación
		/// </summary>
		private int _IntervaloEntreSnaps;
		/// <summary>
		/// Intervalo de tiempo entre snaps de grabación
		/// </summary>
		public int IntervaloEntreSnaps
		{
			get { return _IntervaloEntreSnaps; }
			set { _IntervaloEntreSnaps = value; }
		}

		/// <summary>
		/// Filtro de las imagenes a simular
		/// </summary>
		private string _Filtro;
		/// <summary>
		/// Filtro de las imagenes a simular
		/// </summary>
		public string Filtro
		{
			get { return _Filtro; }
			set { _Filtro = value; }
		}

		/// <summary>
		/// Evento de simluación de la grabación
		/// </summary>
		private DelegadoSnapSimulado _OnSnapSimulado;
		/// <summary>
		/// Evento de simluación de la grabación
		/// </summary>
		internal DelegadoSnapSimulado OnSnapSimulado
		{
			get { return _OnSnapSimulado; }
			set { _OnSnapSimulado = value; }
		}

		#endregion

		#region Constructor(es)
		/// <summary>
		/// Constructor de la clase
		/// </summary>
		public SimulacionCamara(string codigo)
		{
			this._Codigo = codigo;
			this._Simulacion = false;
			this._TipoSimulacion = TipoSimulacionCamara.FotografiaSimple;
			this._RutaFotografias = Path.GetDirectoryName(Application.ExecutablePath);
			this._IntervaloEntreSnaps = 2000;
			this._Filtro = "*.bmp";
			this.IndiceFotografia = -1;
			this.ListaRutaFotografias = new List<string>();

			this.TimerSimulacionGrab = new Timer();
			this.TimerSimulacionGrab.Interval = this._IntervaloEntreSnaps;
			this.TimerSimulacionGrab.Enabled = false;
			this.TimerSimulacionGrab.Tick += EventoGrabSimulado;
		}
		#endregion

		#region Método(s) público(s)

		/// <summary>
		/// Verifica que el modo simulación es correcto
		/// </summary>
		public bool ConfigurarModoSimulacion()
		{
			if (this._Simulacion)
			{
				try
				{
					switch (this._TipoSimulacion)
					{
						case TipoSimulacionCamara.FotografiaSimple:
							if (File.Exists(this.RutaFotografias))
							{
								this.Simulacion = true;
							}
							break;
						case TipoSimulacionCamara.DirectorioFotografias:
							this.ListaRutaFotografias = new List<string>();
							this.IndiceFotografia = -1;

							if (Path.IsPathRooted(this.RutaFotografias) && Directory.Exists(this.RutaFotografias))
							{
								string[] arrayFotografias = Directory.GetFiles(this.RutaFotografias, this.Filtro, SearchOption.TopDirectoryOnly);
								this.ListaRutaFotografias.AddRange(arrayFotografias);

								this.TimerSimulacionGrab.Interval = this._IntervaloEntreSnaps;

								this.Simulacion = this.ListaRutaFotografias.Count > 0;
							}
							break;
					}
				}
				catch (Exception exception)
				{
					LogsRuntime.Error(ModulosHardware.Camaras, this.Codigo, exception);
				}
			}

			return this._Simulacion;
		}

		/// <summary>
		/// Establece la cámara en modo simulación
		/// </summary>
		/// <param name="rutaFotografias">Indica la ruta de la fotografía o de la carpeta de fotografías donde están las imágenes a simular</param>
		public void ConfigurarModoSimulacionFotografia(string rutaFotografias)
		{
			if (!this.Simulacion)
			{
				this.TipoSimulacion = TipoSimulacionCamara.FotografiaSimple;
				this.RutaFotografias = rutaFotografias;
				if (File.Exists(rutaFotografias))
				{
					this.Simulacion = true;
				}
			}
		}

		/// <summary>
		/// Establece la cámara en modo simulación
		/// </summary>
		/// <param name="rutaFotografias">Indica la ruta de la fotografía o de la carpeta de fotografías donde están las imágenes a simular</param>
		/// <param name="filtro">Cadena de filtrado de las fotografías situadas en la caperta establecida</param>
		/// <param name="cadenciaSimulacionEnGrabacionMs">Intervalo en milisegundos utilizado para simular la grabación. Se realiza una fotografía según el tiempo indicado.</param>
		public void ConfigurarModoSimulacionDirectorio(string rutaFotografias, string filtro, int cadenciaSimulacionEnGrabacionMs)
		{
			if (!this.Simulacion)
			{
				this.TipoSimulacion = TipoSimulacionCamara.DirectorioFotografias;
				this.RutaFotografias = rutaFotografias;
				this.ListaRutaFotografias = new List<string>();
				this.IndiceFotografia = -1;

				if (Path.IsPathRooted(rutaFotografias) && Directory.Exists(rutaFotografias))
				{
					string[] arrayFotografias = Directory.GetFiles(rutaFotografias, filtro, SearchOption.TopDirectoryOnly);
					this.ListaRutaFotografias.AddRange(arrayFotografias);

					this.TimerSimulacionGrab.Interval = cadenciaSimulacionEnGrabacionMs;

					this.Simulacion = this.ListaRutaFotografias.Count > 0;
				}
			}
		}

		/// <summary>
		/// Para el modo simulación de la cámara, restableciendo su funcionamiento normal
		/// </summary>
		public bool ConfigurarModoReal()
		{
			if (this.Simulacion)
			{
				this.Simulacion = false;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Inicia la simulación de la grabación
		/// </summary>
		internal void IniciarSimulacionGrabacion()
		{
			if (this.Simulacion)
			{
				this.TimerSimulacionGrab.Enabled = true;
			}
		}

		/// <summary>
		/// Inicia la simulación de la grabación
		/// </summary>
		internal void ParaSimulacionGrabacion()
		{
			if (this.Simulacion)
			{
				this.TimerSimulacionGrab.Enabled = false;
			}
		}
		#endregion

		#region Eventos

		/// <summary>
		/// Evento del timer de simulación de fotografía
		/// </summary>
		/// <param name="source"></param>
		/// <param name="e"></param>
		public void EventoGrabSimulado(object sender, EventArgs e)
		{
			try
			{
				this.EjecutarSimulacion();
			}
			catch (Exception exception)
			{
				LogsRuntime.Error(ModulosHardware.Camaras, this.Codigo, exception);
			}
		}

		/// <summary>
		/// Ejecuta la simulación de un snap
		/// </summary>
		/// <returns>Verdadero si la simulación se ha ejecutado con éxito</returns>
		public bool EjecutarSimulacion()
		{
			bool resultado = false;

			try
			{
				if (this.Simulacion)
				{
					string rutaFotografiaActual = "";

					switch (this.TipoSimulacion)
					{
						case TipoSimulacionCamara.FotografiaSimple:
							rutaFotografiaActual = this.RutaFotografias;
							resultado = true;
							break;
						case TipoSimulacionCamara.DirectorioFotografias:
							if (this.ListaRutaFotografias.Count > 0)
							{
								this.IndiceFotografia++;
								resultado = App.InRange(this.IndiceFotografia, 0, this.ListaRutaFotografias.Count - 1);
								//this.IndiceFotografia = App.EvaluaNumero(this.IndiceFotografia, 0, this.ListaRutaFotografias.Count - 1, 0); // Si se sobrepasa la última fotografía se vuelve a la primera
								//if (IndiceFotografia == 0)
								//{
								//    MessageBox.Show("Fin de la reproducción.");
								//}
								if (resultado)
								{
									rutaFotografiaActual = this.ListaRutaFotografias[this.IndiceFotografia];
								}
							}
							break;
					}

					if (resultado && (OnSnapSimulado != null))
					{
						resultado = this.OnSnapSimulado(rutaFotografiaActual);
					}
				}

			}
			catch (Exception exception)
			{
				LogsRuntime.Error(ModulosHardware.Camaras, this.Codigo, exception);
			}

			return resultado;
		}
		#endregion
	}

	/// <summary>
	/// Enumerado que informa sobre el tipo de simulación que se está realizando en la cámara
	/// </summary>
	public enum TipoSimulacionCamara
	{
		/// <summary>
		/// Se simula siempre con la misma fotografía
		/// </summary>
		FotografiaSimple,

		/// <summary>
		/// Se simula con todo un directorio de fotografías
		/// </summary>
		DirectorioFotografias
	}

	/// <summary>
	/// Delegado que se dispara cuando se simula un snap
	/// </summary>
	/// <param name="rutaFotografia">Ruta de la fotografía de disco</param>
	/// <returns>Devuelve verdadero si la simulación se ha ejecutado correctamente</returns>
	internal delegate bool DelegadoSnapSimulado(string rutaFotografia);
	#endregion

	#region Reconexión con las cámaras
	/// <summary>
	/// Excepción que se lanza al detectar un problema de conexión con las cámaras
	/// </summary>
	[Serializable]
	public class CameraConectionException : Exception
	{
		#region Constructor
		/// <summary>
		/// Constructor de la clase
		/// </summary>
		public CameraConectionException()
			: base("La cámara se encuentra desconectada")
		{
		}
		#endregion
	}

	/// <summary>
	/// Estado de la conexión de la cámara
	/// </summary>
	public enum EstadoConexion
	{
		/// <summary>
		/// Cámara desconectada
		/// </summary>
		Desconectado = 0,
		/// <summary>
		/// Cámara conectada correctamente
		/// </summary>
		Conectado = 1,
		/// <summary>
		/// La cámara estaba conectada pero ha aparecido un error de conexión
		/// </summary>
		ErrorConexion = 2
	}

	#endregion

	#region Medición de la tasa de adquisición
	/// <summary>
	/// Se utiliza para medir la tasa de adquisición de la cámara
	/// </summary>
	public class MedidorVelocidadAdquisicion
	{
		#region Constante(s)
		/// <summary>
		/// Constante que indica el número de valores utilizados en el promedio de las medidas
		/// </summary>
		private const int NumeroValoresPromediados = 10;
		#endregion

		#region Atributo(s)
		private Stopwatch Cronometro;
		/// <summary>
		/// Duración de la última adquisición. En el caso de ser la primera es 0.
		/// </summary>
		public TimeSpan UltimaDuracion;
		/// <summary>
		/// Capturas por sergundo tomada de la última adquisición. En el caso de ser la primera es 0.
		/// </summary>
		public double UltimaTasa;
		/// <summary>
		/// Promedio de duración de las últimas "NumeroValoresPromediados". En el caso de ser la primera es 0.
		/// </summary>
		public TimeSpan PromedioDuracion;
		/// <summary>
		/// Capturas por sergundo tomada de las últimas "NumeroValoresPromediados". En el caso de ser la primera es 0.
		/// </summary>
		public double PromedioTasa;
		#endregion

		#region Constructor de la clase
		/// <summary>
		/// Constructor de la clase
		/// </summary>
		public MedidorVelocidadAdquisicion()
		{
			this.ResetearMediciones();
		}
		#endregion

		#region Método(s) público(s)
		/// <summary>
		/// Se resetean las mediciones
		/// </summary>
		public void ResetearMediciones()
		{
			this.Cronometro = new Stopwatch();
			this.UltimaDuracion = new TimeSpan();
			this.UltimaTasa = 0;
			this.PromedioDuracion = new TimeSpan();
			this.PromedioTasa = 0;        
		}

		/// <summary>
		/// Se ha capturado una nueva foto, por lo que se recalcula la tasa de adquisición
		/// </summary>
		public void NuevaCaptura()
		{
			bool running = this.Cronometro.IsRunning;

			this.Cronometro.Stop();

			if (running)
			{
				this.UltimaDuracion = this.Cronometro.Elapsed;
				this.UltimaTasa = 1 / this.UltimaDuracion.TotalSeconds;
				if (this.PromedioDuracion.TotalSeconds == 0.0)
				{
					this.PromedioDuracion = this.UltimaDuracion;
					this.PromedioTasa = 1 / this.UltimaDuracion.TotalSeconds;
				}
				else
				{
					double factorValorAnterior = ((double)NumeroValoresPromediados - 1.0) / (double)NumeroValoresPromediados;
					double factorValorNuevo = 1.0 / (double)NumeroValoresPromediados;
					this.PromedioDuracion = TimeSpan.FromMilliseconds((this.PromedioDuracion.TotalMilliseconds * factorValorAnterior) + (this.UltimaDuracion.TotalMilliseconds * factorValorNuevo));
					this.PromedioTasa = 1 / this.PromedioDuracion.TotalSeconds;
				}
			}
			else
			{
				this.ResetearMediciones();
			}
			
			this.Cronometro.Reset();
			this.Cronometro.Start();
		}
		#endregion
	}
	#endregion
}
