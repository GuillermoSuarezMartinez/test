//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 12-12-2012
// Description      : Grabación de video
//                    PTZ
//                    Adaptada la forma de conexión desconexión
//
// Last Modified By : aibañez
// Last Modified On : 16-11-2012
// Description      : Eliminadas referencias al formulario de monitorización de cámaras
//                    Eliminadas referencias a los displays
//                    Añadidos eventos de NuevaFotografia, CambioEstado y Mensajes
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
using AForge.Video.FFMPEG;
using Orbita.VAComun;
using Orbita.VAControl;

namespace Orbita.VAHardware
{
	/// <summary>
	/// Clase estática para el acceso a las cámaras
	/// </summary>
	public static class OCamaraManager
	{
		#region Atributo(s)
		/// <summary>
		/// Lista de las cámaras del sistema
		/// </summary>
		public static List<OCamaraBase> ListaCamaras;
		#endregion

		#region Método(s) público(s)

		/// <summary>
		/// Construye los campos de la clase
		/// </summary>
		public static void Constructor()
		{
			ListaCamaras = new List<OCamaraBase>();

			// Añadimos las cámaras al formulario
            DataTable dtCamaras = AppBD.GetCamaras();
            if (dtCamaras.Rows.Count > 0)
            {
                foreach (DataRow dr in dtCamaras.Rows)
                {
                    string codCamara = dr["CodHardware"].ToString();
                    string claseImplementadora = string.Format("{0}.{1}", Assembly.GetExecutingAssembly().GetName().Name, dr["ClaseImplementadora"].ToString());

                    object objetoImplementado;
                    if (App.ConstruirClase(Assembly.GetExecutingAssembly().GetName().Name, claseImplementadora, out objetoImplementado, codCamara))
                    {
                        OCamaraBase camara = (OCamaraBase)objetoImplementado;
                        ListaCamaras.Add(camara);
                    }
                }
            }
		}

		/// <summary>
		/// Destruye los campos de la clase
		/// </summary>
		public static void Destructor()
		{
			ListaCamaras = null;
		}

		/// <summary>
		/// Inicializa las propieades de la clase
		/// </summary>
		public static void Inicializar()
		{
			foreach (OCamaraBase camara in ListaCamaras)
			{
				camara.Inicializar();
			}
		}

		/// <summary>
		/// Finaliza las propiedades de la clase
		/// </summary>
		public static void Finalizar()
		{
			foreach (OCamaraBase camara in ListaCamaras)
			{
				camara.Finalizar();
			}
		}

		/// <summary>
		/// Busca la cámara con el código indicado
		/// </summary>
		/// <param name="codigo">Código de la cámara</param>
		public static OCamaraBase GetCamara(string codigo)
		{
			foreach (OCamaraBase camara in ListaCamaras)
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
			foreach (OCamaraBase camara in ListaCamaras)
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
			foreach (OCamaraBase camara in ListaCamaras)
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
			foreach (OCamaraBase camara in ListaCamaras)
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
			foreach (OCamaraBase camara in ListaCamaras)
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
			foreach (OCamaraBase camara in ListaCamaras)
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
			foreach (OCamaraBase camara in ListaCamaras)
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
			foreach (OCamaraBase camara in ListaCamaras)
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
			foreach (OCamaraBase camara in ListaCamaras)
			{
				if (camara.Codigo == codigo)
				{
					camara.PararModoSimulacion();
				}
			}
		}

        /// <summary>
        /// Suscribe el cambio de fotografía de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir llamadas a cada cambio de fotografía</param>
        public static void CrearSuscripcionNuevaFotografia(string codigo, ODelegadoNuevaFotografiaCamara delegadoNuevaFotografiaCamara)
        {
            OCamaraBase camara = ListaCamaras.Find(delegate(OCamaraBase c) { return (c.Codigo == codigo); });
            if (camara != null)
            {
                camara.CrearSuscripcionNuevaFotografia(delegadoNuevaFotografiaCamara);
            }
        }
        /// <summary>
        /// Suscribe el cambio de fotografía de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir llamadas a cada cambio de fotografía</param>
        public static void CrearSuscripcionNuevaFotografia(string codigo, ODelegadoNuevaFotografiaCamaraAdv delegadoNuevaFotografiaCamara)
        {
            OCamaraBase camara = ListaCamaras.Find(delegate(OCamaraBase c) { return (c.Codigo == codigo); });
            if (camara != null)
            {
                camara.CrearSuscripcionNuevaFotografia(delegadoNuevaFotografiaCamara);
            }
        }

        /// <summary>
        /// Elimina la suscripción del cambio de fotografía de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir llamadas a cada cambio de fotografía</param>
        public static void EliminarSuscripcionNuevaFotografia(string codigo, ODelegadoNuevaFotografiaCamara delegadoNuevaFotografiaCamara)
        {
            OCamaraBase camara = ListaCamaras.Find(delegate(OCamaraBase c) { return (c.Codigo == codigo); });
            if (camara != null)
            {
                camara.EliminarSuscripcionNuevaFotografia(delegadoNuevaFotografiaCamara);
            }
        }
        /// <summary>
        /// Elimina la suscripción del cambio de fotografía de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir llamadas a cada cambio de fotografía</param>
        public static void EliminarSuscripcionNuevaFotografia(string codigo, ODelegadoNuevaFotografiaCamaraAdv delegadoNuevaFotografiaCamara)
        {
            OCamaraBase camara = ListaCamaras.Find(delegate(OCamaraBase c) { return (c.Codigo == codigo); });
            if (camara != null)
            {
                camara.EliminarSuscripcionNuevaFotografia(delegadoNuevaFotografiaCamara);
            }
        }

        /// <summary>
        /// Suscribe el cambio de estado de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir llamadas a cada cambio de estado</param>
        public static void CrearSuscripcionCambioEstado(string codigo, ODelegadoCambioEstadoConexionCamara delegadoCambioEstadoConexionCamara)
        {
            OCamaraBase camara = ListaCamaras.Find(delegate(OCamaraBase c) { return (c.Codigo == codigo); });
            if (camara != null)
            {
                camara.CrearSuscripcionCambioEstado(delegadoCambioEstadoConexionCamara);
            }
        }
        /// <summary>
        /// Suscribe el cambio de estado de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir llamadas a cada cambio de estado</param>
        public static void CrearSuscripcionCambioEstado(string codigo, ODelegadoCambioEstadoConexionCamaraAdv delegadoCambioEstadoConexionCamara)
        {
            OCamaraBase camara = ListaCamaras.Find(delegate(OCamaraBase c) { return (c.Codigo == codigo); });
            if (camara != null)
            {
                camara.CrearSuscripcionCambioEstado(delegadoCambioEstadoConexionCamara);
            }
        }

        /// <summary>
        /// Elimina la suscripción del cambio de estado de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir llamadas a cada cambio de estado</param>
        public static void EliminarSuscripcionCambioEstado(string codigo, ODelegadoCambioEstadoConexionCamara delegadoCambioEstadoConexionCamara)
        {
            OCamaraBase camara = ListaCamaras.Find(delegate(OCamaraBase c) { return (c.Codigo == codigo); });
            if (camara != null)
            {
                camara.EliminarSuscripcionCambioEstado(delegadoCambioEstadoConexionCamara);
            }
        }
        /// <summary>
        /// Elimina la suscripción del cambio de estado de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir llamadas a cada cambio de estado</param>
        public static void EliminarSuscripcionCambioEstado(string codigo, ODelegadoCambioEstadoConexionCamaraAdv delegadoCambioEstadoConexionCamara)
        {
            OCamaraBase camara = ListaCamaras.Find(delegate(OCamaraBase c) { return (c.Codigo == codigo); });
            if (camara != null)
            {
                camara.EliminarSuscripcionCambioEstado(delegadoCambioEstadoConexionCamara);
            }
        }

        /// <summary>
        /// Suscribe la recepción de mensajes de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir los mensajes</param>
        public static void CrearSuscripcionMensajes(string codigo, OMessageDelegate messageDelegate)
        {
            OCamaraBase camara = ListaCamaras.Find(delegate(OCamaraBase c) { return (c.Codigo == codigo); });
            if (camara != null)
            {
                camara.CrearSuscripcionMensajes(messageDelegate);
            }
        }
        /// <summary>
        /// Suscribe la recepción de mensajes de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir los mensajes</param>
        public static void CrearSuscripcionMensajes(string codigo, OMessageDelegateAdv messageDelegate)
        {
            OCamaraBase camara = ListaCamaras.Find(delegate(OCamaraBase c) { return (c.Codigo == codigo); });
            if (camara != null)
            {
                camara.CrearSuscripcionMensajes(messageDelegate);
            }
        }

        /// <summary>
        /// Elimina la suscripción de mensajes de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir los mensajes</param>
        public static void EliminarSuscripcionMensajes(string codigo, OMessageDelegate messageDelegate)
        {
            OCamaraBase camara = ListaCamaras.Find(delegate(OCamaraBase c) { return (c.Codigo == codigo); });
            if (camara != null)
            {
                camara.EliminarSuscripcionMensajes(messageDelegate);
            }
        }
        /// <summary>
        /// Elimina la suscripción de mensajes de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir los mensajes</param>
        public static void EliminarSuscripcionMensajes(string codigo, OMessageDelegateAdv messageDelegate)
        {
            OCamaraBase camara = ListaCamaras.Find(delegate(OCamaraBase c) { return (c.Codigo == codigo); });
            if (camara != null)
            {
                camara.EliminarSuscripcionMensajes(messageDelegate);
            }
        }
        #endregion
	}

	/// <summary>
	/// Clase base para todas las cámaras
	/// </summary>
	public class OCamaraBase : IHardware
	{
		#region Atributo(s)
		/// <summary>
		/// Objeto de simulación de la cámara
		/// </summary>
		public OSimulacionCamara SimulacionCamara;
		/// <summary>
		/// Indica si esta grabando
		/// </summary>
        public bool Recording
        {
            get { return this.VideoFile.Estado == OEstadoProductorConsumidor.EnEjecucion; }
        }
		/// <summary>
		/// Proporciona herramientas para medir la velocidad de adquisición de la cámara
		/// </summary>
		public OMedidorVelocidadAdquisicion MedidorVelocidadAdquisicion;
        /// <summary>
        /// Guardado de Fichero de video
        /// </summary>
        private OVideoFile VideoFile;
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
		public OTipoHardware TipoHardware
		{
			get { return OTipoHardware.HwCamara; }
		}

		/// <summary>
		/// Tipo de cámara
		/// </summary>
		private OTipoCamara _TipoCamara;
		/// <summary>
		/// Tipo de cámara
		/// </summary>
		public OTipoCamara TipoCamara
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
		private Size _Resolucion;
		/// <summary>
		/// Resolución de la cámara
		/// </summary>
        public Size Resolucion
		{
			get { return _Resolucion; }
			set { _Resolucion = value; }
		}

		/// <summary>
		/// Tipo de cámara dependiendo de si sus imágenes son a color o monocromáticas
		/// </summary>
		private OTipoColorPixel _Color;
		/// <summary>
		/// Tipo de cámara dependiendo de si sus imágenes son a color o monocromáticas
		/// </summary>
		public OTipoColorPixel Color
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
		/// Lista de todos los terminales de la tarjeta de IO
		/// </summary>              
		protected List<OTerminalIOBase> _ListaTerminales;
		/// <summary>
		/// Lista de todos los terminales de la tarjeta de IO
		/// </summary>              
		public List<OTerminalIOBase> ListaTerminales
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
        private double _ExpectedFrameInterval;
        /// <summary>
        /// Indica el intervalo de tiempo en ms. de adquisición de imagenes
        /// </summary>
        public double ExpectedFrameInterval
        {
            get { return _ExpectedFrameInterval; }
            set { _ExpectedFrameInterval = value; }
        }

        /// <summary>
        /// Indica la tasa de adquisición esperada
        /// </summary>
        private double _ExpectedFrameRate;
        /// <summary>
        /// Indica la tasa de adquisición esperada
        /// </summary>
        public double ExpectedFrameRate
        {
            get { return _ExpectedFrameRate; }
            set { _ExpectedFrameRate = value; }
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

        /// <summary>
        /// Nombre del ensamblado de la clase que implementa el display asociado a este tipo de cámara
        /// </summary>
        private string _EnsambladoClaseImplementadoraDisplay;
        /// <summary>
        /// Nombre del ensamblado de la clase que implementa el display asociado a este tipo de cámara
        /// </summary>
        public string EnsambladoClaseImplementadoraDisplay
        {
            get { return _EnsambladoClaseImplementadoraDisplay; }
            set { _EnsambladoClaseImplementadoraDisplay = value; }
        }

        /// <summary>
        /// Nombre de la clase que implementa el display asociado a este tipo de cámara
        /// </summary>                                        
        private string _ClaseImplementadoraDisplay;
        /// <summary>
        /// Nombre de la clase que implementa el display asociado a este tipo de cámara
        /// </summary>
        public string ClaseImplementadoraDisplay
        {
            get { return _ClaseImplementadoraDisplay; }
            set { _ClaseImplementadoraDisplay = value; }
        }

        /// <summary>
        /// Nombre del ensamblado de la clase que implementa el display asociado a este tipo de cámara
        /// </summary>
        private string _EnsambladoClaseImplementadoraPTZ;
        /// <summary>
        /// Nombre del ensamblado de la clase que implementa el display asociado a este tipo de cámara
        /// </summary>
        public string EnsambladoClaseImplementadoraPTZ
        {
            get { return _EnsambladoClaseImplementadoraPTZ; }
            set { _EnsambladoClaseImplementadoraPTZ = value; }
        }

        /// <summary>
        /// Nombre de la clase que implementa el display asociado a este tipo de cámara
        /// </summary>                                        
        private string _ClaseImplementadoraPTZ;
        /// <summary>
        /// Nombre de la clase que implementa el display asociado a este tipo de cámara
        /// </summary>
        public string ClaseImplementadoraPTZ
        {
            get { return _ClaseImplementadoraPTZ; }
            set { _ClaseImplementadoraPTZ = value; }
        }

        /// <summary>
        /// Clase encargada de comprobar la conectividad con un dispositivo
        /// </summary>
        private OConectividad _Conectividad;
        /// <summary>
        /// Clase encargada de comprobar la conectividad con un dispositivo
        /// </summary>
        public OConectividad Conectividad
        {
            get { return _Conectividad; }
            set { _Conectividad = value; }
        }

        /// <summary>
        /// Control PTZ
        /// </summary>
        private OPTZBase _PTZ;
        /// <summary>
        /// Control PTZ
        /// </summary>
        public OPTZBase PTZ
        {
            get { return _PTZ; }
            set { _PTZ = value; }
        }

        /// <summary>
        /// Porcentaje de la reducción de imagenes en el video grabado: 100 => Tamaño original, 50 => Reducción a la mitad...
        /// </summary>
        public Size ResolucionGrabacion
        {
            get { return this.VideoFile.Resolucion; }
            set { this.VideoFile.Resolucion = value; }
        }

        /// <summary>
        /// Tasa de captura
        /// </summary>
        public int FrameRateGrabacion
        {
            get { return this.VideoFile.FrameRate; }
        }

        /// <summary>
        /// Indica el intervalo de tiempo en ms. de grabación de imagenes
        /// </summary>
        public double FrameIntervalGrabacion
        {
            get { return this.VideoFile.FrameInterval.TotalMilliseconds; }
        }

        /// <summary>
        /// Tasa de transferencia
        /// </summary>
        public int BitRateGrabacion
        {
            get { return this.VideoFile.BitRate; }
            set { this.VideoFile.BitRate = value; }
        }

        /// <summary>
        /// Códec de grabación
        /// </summary>
        public VideoCodec CodecGarbacion
        {
            get { return this.VideoFile.Codec; }
            set { this.VideoFile.Codec = value; }
        }

        /// <summary>
        /// Tiempo máximo de duración de la grabación
        /// </summary>
        public TimeSpan TiempoMaxGrabacion
        {
            get { return this.VideoFile.TiempoMaxGrabacion; }
            set { this.VideoFile.TiempoMaxGrabacion = value; }
        }
        #endregion

        #region Propiedad(es) virtual(es)
        /// <summary>
        /// Última imagen capturada
        /// </summary>
        protected OImage _ImagenActual;
        /// <summary>
        /// Propieadad a heredar donde se accede a la imagen
        /// </summary>
        public virtual OImage ImagenActual
        {
            get { return this._ImagenActual; }
            set { this._ImagenActual = value; }
        }

        /// <summary>
        /// Estado de la conexión
        /// </summary>
        public virtual OEstadoConexion EstadoConexion
        {
            get
            {
                return this.Conectividad != null ? this.Conectividad.EstadoConexion : OEstadoConexion.Desconectado;
            }
            set
            {
                if (this.Conectividad != null)
                {
                    this.Conectividad.EstadoConexion = value;
                }
            }
        }
        #endregion

        #region Declaración(es) de evento(s)
        /// <summary>
        /// Delegado de nueva fotografía
        /// </summary>
        /// <param name="estadoConexion"></param>
        public ODelegadoNuevaFotografiaCamara OnNuevaFotografiaCamara;
        /// <summary>
        /// Delegado de nueva fotografía
        /// </summary>
        /// <param name="estadoConexion"></param>
        public ODelegadoNuevaFotografiaCamaraAdv OnNuevaFotografiaCamaraAdv;
        /// <summary>
        /// Delegado de cambio de estaco de conexión de la cámara
        /// </summary>
        /// <param name="estadoConexion"></param>
        public ODelegadoCambioEstadoConexionCamara OnCambioEstadoConexionCamara;
        /// <summary>
        /// Delegado de cambio de estaco de conexión de la cámara
        /// </summary>
        /// <param name="estadoConexion"></param>
        public ODelegadoCambioEstadoConexionCamaraAdv OnCambioEstadoConexionCamaraAdv;
        /// <summary>
        /// Delegado de mensaje de la cámara
        /// </summary>
        /// <param name="estadoConexion"></param>
        public OMessageDelegate OnMensajeCamara;
        /// <summary>
        /// Delegado de mensaje de la cámara
        /// </summary>
        /// <param name="estadoConexion"></param>
        public OMessageDelegateAdv OnMensajeCamaraAdv;
        /// <summary>
        /// Delegado de mensaje de cambio de estado de reproducción
        /// </summary>
        public ODelegadoCambioEstadoReproduccionCamara OnCambioReproduccionCamara;
        /// <summary>
        /// Delegado de mensaje de cambio de estado de reproducción
        /// </summary>
        public ODelegadoCambioEstadoReproduccionCamaraAdv OnCambioReproduccionCamaraAdv;
        #endregion

		#region Constructor(es)
		/// <summary>
		/// Constructor de la clase
		/// </summary>
		public OCamaraBase(string codigo)
		{
			try
			{
				//Inicializamos los valores por defecto
				this._Codigo = codigo;
				this._ListaTerminales = new List<OTerminalIOBase>();
				this._Existe = false;
				this._VisualizacionEnVivo = false;
				this.SimulacionCamara = new OSimulacionCamara(codigo);
				this.SimulacionCamara.OnSnapSimulado += new ODelegadoSnapSimulado(SnapSimulado);
				//this.Recording = false;
				this.MedidorVelocidadAdquisicion = new OMedidorVelocidadAdquisicion();

				DataTable dt = AppBD.GetCamara(codigo);
				if (dt.Rows.Count == 1)
				{
					this._Nombre = dt.Rows[0]["NombreHardware"].ToString();
					this._Descripcion = dt.Rows[0]["DescHardware"].ToString();
					this._Habilitado = (bool)dt.Rows[0]["HabilitadoHardware"];
					this._TipoCamara = (OTipoCamara)App.EnumParse(typeof(OTipoCamara), dt.Rows[0]["CodTipoHardware"].ToString(), OTipoCamara.VProBasler);
					this._CodigoTipoCamara = dt.Rows[0]["CodTipoHardware"].ToString();
					this._Fabricante = dt.Rows[0]["Fabricante"].ToString();
					this._Modelo = dt.Rows[0]["Modelo"].ToString();
					this._DescripcionTipoCamara = dt.Rows[0]["DescTipoHardware"].ToString();
					this._CodVariableImagen = dt.Rows[0]["CodVariableImagen"].ToString();
					this._LanzarEventoAlSnap = App.EvaluaBooleano(dt.Rows[0]["LanzarEventoAlSnap"], false);
					this._CodVariableSnap = dt.Rows[0]["CodVariableSnap"].ToString();
					this._Resolucion.Width = App.EvaluaNumero(dt.Rows[0]["ResolucionX"], 1, 100000, 1024);
					this._Resolucion.Height = App.EvaluaNumero(dt.Rows[0]["ResolucionY"], 1, 100000, 768);
					this._Color = (OTipoColorPixel)App.EvaluaNumero(dt.Rows[0]["Color"], 0, 1, 0);
                    this._ExpectedFrameInterval = App.EvaluaDecimal(dt.Rows[0]["FrameIntervalMs"], 0.0, 1000.0, 1.0);
                    this._ExpectedFrameRate = this._ExpectedFrameInterval > 0 ? 1000 / this._ExpectedFrameInterval : 25;
                    this._MaxFrameIntervalVisualizacion = App.EvaluaDecimal(dt.Rows[0]["MaxFrameIntervalMsVisualizacion"], 0.0, 1000.0, 0.0);
                    this._EnsambladoClaseImplementadoraDisplay = dt.Rows[0]["EnsambladoClaseImplementadoraDisplay"].ToString();
                    this._ClaseImplementadoraDisplay = string.Format("{0}.{1}", this._EnsambladoClaseImplementadoraDisplay, dt.Rows[0]["ClaseImplementadoraDisplay"].ToString());

                    // Construcción del PTZ
                    this._EnsambladoClaseImplementadoraPTZ = App.EvaluaTexto(dt.Rows[0]["EnsambladoClaseImplementadoraPTZ"], 100, false, Assembly.GetExecutingAssembly().GetName().Name);
                    this._ClaseImplementadoraPTZ = string.Format("{0}.{1}", this._EnsambladoClaseImplementadoraPTZ, App.EvaluaTexto(dt.Rows[0]["ClaseImplementadoraPTZ"], 100, false, typeof(OPTZBase).Name));
                    object objetoImplementado;
                    if (App.ConstruirClase(this._EnsambladoClaseImplementadoraPTZ, this._ClaseImplementadoraPTZ, out objetoImplementado, this._Codigo))
                    {
                        this.PTZ = (OPTZBase)objetoImplementado;
                    }
                    else
                    {
                        this.PTZ = new OPTZBase(this.Codigo);
                    }

                    // Construcción del Grabador de videos
                    TimeSpan tiempoMaxGrabacion = TimeSpan.FromMilliseconds(App.EvaluaNumero(dt.Rows[0]["GrabacionTiempoMaxMs"], 1, int.MaxValue, 60));
                    Size resolucionGrabacion = new Size();
                    resolucionGrabacion.Width = App.EvaluaNumero(dt.Rows[0]["GrabacionResolucionX"], 1, 100000, 1024);
                    resolucionGrabacion.Height = App.EvaluaNumero(dt.Rows[0]["GrabacionResolucionY"], 1, 100000, 768);
                    VideoCodec codecGrabacion = (VideoCodec)App.EnumParse(typeof(VideoCodec), dt.Rows[0]["GrabacionCodec"].ToString(), VideoCodec.MPEG4);
                    int bitRateGrabacion = App.EvaluaNumero(dt.Rows[0]["GrabacionBitRate"], 1, int.MaxValue, 1000);
                    double grabacionFrameIntervalMs = App.EvaluaNumero(dt.Rows[0]["GrabacionFrameIntervalMs"], 0.0, 1000.0, 1.0);
                    this.VideoFile = new OVideoFile(this.Codigo, resolucionGrabacion, tiempoMaxGrabacion, grabacionFrameIntervalMs, bitRateGrabacion, codecGrabacion);
				}
				else
				{
					throw new Exception("No se ha podido cargar la información de la cámara " + codigo + " de la base de datos.");
				}
			}
			catch (Exception exception)
			{
				OVALogsManager.Fatal(OModulosHardware.Camaras, this.Codigo, exception);
				throw new Exception("Imposible iniciar la cámara " + this.Codigo);
			}
		}
		#endregion

		#region Método(s) público(s)
        /// <summary>
        /// Se conecta a la cámara
        /// </summary>
        public bool Conectar()
        {
            return this.Conectar(false);
        }

        /// <summary>
        /// Se desconecta a la cámara
        /// </summary>
        public bool Desconectar()
        {
            return this.Desconectar(false);
        }

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
            bool resultado = false;
            if (this.Habilitado && (this.EstadoConexion == VAHardware.OEstadoConexion.Conectado))
            {
                // Información extra
                OVALogsManager.Debug(OModulosHardware.Camaras, this.Codigo, "Start de la cámara: " + this.Codigo);

                OVariablesManager.CrearSuscripcion(this._CodVariableSnap, "Camaras", this.Codigo, ComandoSnapPorVariable);

                this._Grab = true;
                if (!this.SimulacionCamara.Simulacion)
                {
                    resultado = this.InternalStart();
                }
                else
                {
                    this.SimulacionCamara.IniciarSimulacionGrabacion();
                    resultado = true;
                }

                if (this.OnCambioReproduccionCamara != null)
                {
                    this.OnCambioReproduccionCamara(this._Grab);
                }
                if (this.OnCambioReproduccionCamaraAdv != null)
                {
                    this.OnCambioReproduccionCamaraAdv(this.Codigo, this._Grab);
                }
            }
            return resultado;
		}

		/// <summary>
		/// Termina una reproducción continua de la cámara
		/// </summary>
		/// <returns></returns>
		public bool Stop()
		{
            bool resultado = false;
            if (this.Habilitado)
            {
                // Información extra
                OVALogsManager.Debug(OModulosHardware.Camaras, this.Codigo, "Stop de la cámara: " + this.Codigo);

                OVariablesManager.EliminarSuscripcion(this._CodVariableSnap, "Camaras", this.Codigo, ComandoSnapPorVariable);

                this._Grab = false;
                if (!this.SimulacionCamara.Simulacion)
                {
                    resultado = this.InternalStop();
                }
                else
                {
                    this.SimulacionCamara.ParaSimulacionGrabacion();
                    resultado = true;
                }

                if (this.OnCambioReproduccionCamara != null)
                {
                    this.OnCambioReproduccionCamara(this._Grab);
                }
                if (this.OnCambioReproduccionCamaraAdv != null)
                {
                    this.OnCambioReproduccionCamaraAdv(this.Codigo, this._Grab);
                }
            }
            return resultado;
        }

		/// <summary>
		/// Realiza una fotografía de forma sincrona
		/// </summary>
		/// <returns></returns>
		public virtual bool Snap()
		{
			bool resultado = false;

            if (this.Habilitado && (this.EstadoConexion == VAHardware.OEstadoConexion.Conectado))
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
                OVALogsManager.Debug(OModulosHardware.Camaras, this.Codigo, "Snap de la cámara: " + this.Codigo + ". Resultado: " + resultado.ToString());
            }

			return resultado;
		}

		/// <summary>
		/// Visualiza una imagen en el display
		/// </summary>
		/// <param name="imagen">Imagen a visualizar</param>        
		public void VisualizarImagen(OImage imagen)
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
		public void VisualizarUltimaImagen(OGrafico graficos)
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
            bool resultado = false;
            if (this.Habilitado && (this.EstadoConexion == VAHardware.OEstadoConexion.Conectado))
            {
                // Información extra
                OVALogsManager.Debug(OModulosHardware.Camaras, this.Codigo, "REC de la cámara: " + this.Codigo);

                if (!this.SimulacionCamara.Simulacion)
                {
                    resultado = this.InternalREC(fichero);
                }
            }
            //this.Recording = resultado;
            return resultado;
        }

		/// <summary>
		/// Termina una grabación continua de la cámara
		/// </summary>
		/// <returns></returns>
        public bool StopREC()
        {
            bool resultado = false;
            if (this.Habilitado && (this.EstadoConexion == VAHardware.OEstadoConexion.Conectado))
            {
                // Información extra
                OVALogsManager.Debug(OModulosHardware.Camaras, this.Codigo, "StopREC de la cámara: " + this.Codigo);

                if (!this.SimulacionCamara.Simulacion)
                {
                    resultado = this.InternalStopREC();
                }
            }
            //this.Recording = false;
            return resultado;
        }

        /// <summary>
        /// Suscribe el cambio de fotografía de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir llamadas a cada cambio de fotografía</param>
        public void CrearSuscripcionNuevaFotografia(ODelegadoNuevaFotografiaCamara delegadoNuevaFotografiaCamara)
        {
            this.OnNuevaFotografiaCamara += delegadoNuevaFotografiaCamara;
        }
        /// <summary>
        /// Suscribe el cambio de fotografía de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir llamadas a cada cambio de fotografía</param>
        public void CrearSuscripcionNuevaFotografia(ODelegadoNuevaFotografiaCamaraAdv delegadoNuevaFotografiaCamara)
        {
            this.OnNuevaFotografiaCamaraAdv += delegadoNuevaFotografiaCamara;
        }

        /// <summary>
        /// Elimina la suscripción del cambio de fotografía de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir llamadas a cada cambio de fotografía</param>
        public void EliminarSuscripcionNuevaFotografia(ODelegadoNuevaFotografiaCamara delegadoNuevaFotografiaCamara)
        {
            this.OnNuevaFotografiaCamara -= delegadoNuevaFotografiaCamara;
        }
        /// <summary>
        /// Elimina la suscripción del cambio de fotografía de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir llamadas a cada cambio de fotografía</param>
        public void EliminarSuscripcionNuevaFotografia(ODelegadoNuevaFotografiaCamaraAdv delegadoNuevaFotografiaCamara)
        {
            this.OnNuevaFotografiaCamaraAdv -= delegadoNuevaFotografiaCamara;
        }

        /// <summary>
        /// Suscribe el cambio de estado de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir llamadas a cada cambio de estado</param>
        public void CrearSuscripcionCambioEstado(ODelegadoCambioEstadoConexionCamara delegadoCambioEstadoConexionCamara)
        {
            this.OnCambioEstadoConexionCamara += delegadoCambioEstadoConexionCamara;
        }
        /// <summary>
        /// Suscribe el cambio de estado de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir llamadas a cada cambio de estado</param>
        public void CrearSuscripcionCambioEstado(ODelegadoCambioEstadoConexionCamaraAdv delegadoCambioEstadoConexionCamara)
        {
            this.OnCambioEstadoConexionCamaraAdv += delegadoCambioEstadoConexionCamara;
        }

        /// <summary>
        /// Elimina la suscripción del cambio de estado de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir llamadas a cada cambio de estado</param>
        public void EliminarSuscripcionCambioEstado(ODelegadoCambioEstadoConexionCamara delegadoCambioEstadoConexionCamara)
        {
            this.OnCambioEstadoConexionCamara -= delegadoCambioEstadoConexionCamara;
        }
        /// <summary>
        /// Elimina la suscripción del cambio de estado de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir llamadas a cada cambio de estado</param>
        public void EliminarSuscripcionCambioEstado(ODelegadoCambioEstadoConexionCamaraAdv delegadoCambioEstadoConexionCamara)
        {
            this.OnCambioEstadoConexionCamaraAdv -= delegadoCambioEstadoConexionCamara;
        }

        /// <summary>
        /// Suscribe la recepción de mensajes de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir los mensajes</param>
        public void CrearSuscripcionMensajes(OMessageDelegate messageDelegate)
        {
            this.OnMensajeCamara += messageDelegate;
        }
        /// <summary>
        /// Suscribe la recepción de mensajes de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir los mensajes</param>
        public void CrearSuscripcionMensajes(OMessageDelegateAdv messageDelegate)
        {
            this.OnMensajeCamaraAdv += messageDelegate;
        }

        /// <summary>
        /// Elimina la suscripción de mensajes de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir los mensajes</param>
        public void EliminarSuscripcionMensajes(OMessageDelegate messageDelegate)
        {
            this.OnMensajeCamara -= messageDelegate;
        }
        /// <summary>
        /// Elimina la suscripción de mensajes de una determinada cámara
        /// </summary>
        /// <param name="codigo">Código de la cámara</param>
        /// <param name="delegadoNuevaFotografiaCamara">Delegado donde recibir los mensajes</param>
        public void EliminarSuscripcionMensajes(OMessageDelegateAdv messageDelegate)
        {
            this.OnMensajeCamaraAdv -= messageDelegate;
        }
		#endregion

		#region Método(s) privado(s)
        /// <summary>
        /// Se ejecuta cuando se ha completado la adquisición de una imagen
        /// </summary>
        protected virtual void AdquisicionCompletada(OImage imagen)
        {
            // Se añade la foto al video
            if (this.Recording)
            {
                this.VideoFile.Add(this.ImagenActual);
            }

            // Se dispara el delegado de nueva fotografía
            if (this.OnNuevaFotografiaCamara != null)
            {
                this.OnNuevaFotografiaCamara(imagen);
            }
            if (this.OnNuevaFotografiaCamaraAdv != null)
            {
                this.OnNuevaFotografiaCamaraAdv(this.Codigo, imagen, DateTime.Now, this.MedidorVelocidadAdquisicion.UltimaTasa);
            }
        }

		/// <summary>
		/// Establece el valor de la imagen a la variable asociada
		/// </summary>
		/// <param name="imagen">Imagen a establecer en la variable</param>
		protected void EstablecerVariableAsociada(OImage imagen)
		{
			// Se asigna el valor de la variable asociada
			OVariablesManager.SetValue(this.CodVariableImagen, imagen, "Camaras", this.Codigo);
		}

		/// <summary>
		/// Simulación de la realización de un snap
		/// </summary>
		protected bool SnapSimulado(string rutaFotografia)
		{
			OImage imagen;

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
					OVALogsManager.Error(OModulosHardware.Camaras, "Inicialización", "Ha sido imposible realizar la conexión de la cámara " + this.Nombre);
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
                    this.Stop();

					if (this.ImagenActual != null)
					{
						this.ImagenActual = null;
					}
				}
				else
				{
					OVALogsManager.Error(OModulosHardware.Camaras, "Finalización", "Ha sido imposible realizar la desconexión de la cámara " + this.Nombre);
				}
			}
		}

		/// <summary>
		/// Se toma el control de la cámara
		/// </summary>
		/// <returns>Verdadero si la operación ha funcionado correctamente</returns>
		protected virtual bool Conectar(bool reconexion)
		{
			// Información extra
			OVALogsManager.Debug(OModulosHardware.Camaras, this.Codigo, "Conexión de la cámara: " + this.Codigo);

			return false;
		}

		/// <summary>
		/// Se deja el control de la cámara
		/// </summary>
		/// <returns>Verdadero si la operación ha funcionado correctamente</returns>
		protected virtual bool Desconectar(bool errorConexion)
		{
			// Información extra
			OVALogsManager.Debug(OModulosHardware.Camaras, this.Codigo, "Desconexión de la cámara: " + this.Codigo);

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
		public virtual bool CargarImagenDeDisco(out OImage imagen, string ruta)
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
		public virtual bool CargarGraficoDeDisco(out OGrafico grafico, string ruta)
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
		public virtual bool GuardarGraficoADisco(OGrafico graficos, string ruta)
		{
			// Método implementado en las clases hijas
			return false;
		}

		/// <summary>
		/// Devuelve una nueva imagen del tipo adecuado al trabajo con la cámara
		/// </summary>
		/// <returns>Imagen del tipo adecuado al trabajo con la cámara</returns>
		public virtual OImage NuevaImagen()
		{
			return null;
		}

		/// <summary>
		/// Devuelve un nuevo gráfico del tipo adecuado al trabajo con el display
		/// </summary>
		/// <returns>Grafico del tipo adecuado al trabajo con el display</returns>
		public virtual OGrafico NuevoGrafico()
		{
			return null;
		}

		/// <summary>
		/// Visualiza una imagen en el display
		/// </summary>
		/// <param name="imagen">Imagen a visualizar</param>
		/// <param name="graficos">Objeto que contiene los gráficos a visualizar (letras, rectas, circulos, etc)</param>
		public virtual void VisualizarImagen(OImage imagen, OGrafico graficos)
		{
		}

		/// <summary>
		/// Comienza una grabacion continua de la cámara
		/// </summary>
		/// <param name="fichero">Ruta y nombre del fichero que contendra el video</param>
		/// <returns></returns>
		protected virtual bool InternalREC(string fichero)
		{
            bool resultado = false;

            this.VideoFile.Ruta = fichero;
            if (this.VideoFile.Valido)
            {
                resultado = this.VideoFile.Start();
            }

            return resultado;
		}

		/// <summary>
		/// Termina una grabación continua de la cámara
		/// </summary>
		/// <returns></returns>
		protected virtual bool InternalStopREC()
		{
            bool resultado = false;

            if (this.Recording)
            {
                this.VideoFile.Stop();
                resultado = true;
            }

            return resultado;
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

        #region Evento(s) virtual(es)
        /// <summary>
        /// Evento de cambio en la conexión con la cámara
        /// </summary>
        /// <param name="estadoConexion"></param>
        protected virtual void OnCambioEstadoConectividadCamara(string codigo, OEstadoConexion estadoConexionActal, OEstadoConexion estadoConexionAnterior)
        {
            // Lanzamos el evento de cambio de estado
            if (this.OnCambioEstadoConexionCamara != null)
            {
                this.OnCambioEstadoConexionCamara(estadoConexionActal);
            }
            if (this.OnCambioEstadoConexionCamaraAdv != null)
            {
                this.OnCambioEstadoConexionCamaraAdv(this.Codigo, estadoConexionActal, estadoConexionAnterior);
            }
        }
        #endregion
	}

	/// <summary>
	/// Enumerado que identifica a los tipos de cámaras
	/// </summary>
	public enum OTipoCamara
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
	public enum OTipoColorPixel
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
	public class OSimulacionCamara
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
		private OTipoSimulacionCamara _TipoSimulacion;
		/// <summary>
		/// Indica el tipo de simulación que se realizará en la cámara (con una única foto o con toda una carpeta de fotos)
		/// </summary>
		public OTipoSimulacionCamara TipoSimulacion
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
		private ODelegadoSnapSimulado _OnSnapSimulado;
		/// <summary>
		/// Evento de simluación de la grabación
		/// </summary>
		internal ODelegadoSnapSimulado OnSnapSimulado
		{
			get { return _OnSnapSimulado; }
			set { _OnSnapSimulado = value; }
		}

		#endregion

		#region Constructor(es)
		/// <summary>
		/// Constructor de la clase
		/// </summary>
		public OSimulacionCamara(string codigo)
		{
			this._Codigo = codigo;
			this._Simulacion = false;
			this._TipoSimulacion = OTipoSimulacionCamara.FotografiaSimple;
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
						case OTipoSimulacionCamara.FotografiaSimple:
							if (File.Exists(this.RutaFotografias))
							{
								this.Simulacion = true;
							}
							break;
						case OTipoSimulacionCamara.DirectorioFotografias:
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
					OVALogsManager.Error(OModulosHardware.Camaras, this.Codigo, exception);
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
				this.TipoSimulacion = OTipoSimulacionCamara.FotografiaSimple;
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
				this.TipoSimulacion = OTipoSimulacionCamara.DirectorioFotografias;
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
				OVALogsManager.Error(OModulosHardware.Camaras, this.Codigo, exception);
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
						case OTipoSimulacionCamara.FotografiaSimple:
							rutaFotografiaActual = this.RutaFotografias;
							resultado = true;
							break;
						case OTipoSimulacionCamara.DirectorioFotografias:
							if (this.ListaRutaFotografias.Count > 0)
							{
								this.IndiceFotografia++;
								resultado = App.InRange(this.IndiceFotografia, 0, this.ListaRutaFotografias.Count - 1);
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
				OVALogsManager.Error(OModulosHardware.Camaras, this.Codigo, exception);
			}

			return resultado;
		}
		#endregion
	}

	/// <summary>
	/// Enumerado que informa sobre el tipo de simulación que se está realizando en la cámara
	/// </summary>
	public enum OTipoSimulacionCamara
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
	internal delegate bool ODelegadoSnapSimulado(string rutaFotografia);
	#endregion

	#region Reconexión con las cámaras
	/// <summary>
	/// Excepción que se lanza al detectar un problema de conexión con las cámaras
	/// </summary>
	[Serializable]
	public class OCameraConectionException : Exception
	{
		#region Constructor
		/// <summary>
		/// Constructor de la clase
		/// </summary>
		public OCameraConectionException()
			: base("La cámara se encuentra desconectada")
		{
		}
		#endregion
	}

	/// <summary>
	/// Estado de la conexión de la cámara
	/// </summary>
	public enum OEstadoConexion
	{
		/// <summary>
		/// Cámara desconectada
		/// </summary>
		Desconectado = 0,
        /// <summary>
        /// Cámara en proceso de desconexión
        /// </summary>
        Desconectando = 1,
        /// <summary>
		/// Cámara conectada correctamente
		/// </summary>
		Conectado = 2,
        /// <summary>
        /// Cámara en proceso de conexión
        /// </summary>
        Conectando = 3,
        /// <summary>
		/// La cámara estaba conectada pero ha aparecido un error de conexión
		/// </summary>
		ErrorConexion = 4,
        /// <summary>
		/// La cámara tiene un error de conexión y está intentando reconectarse
		/// </summary>
		Reconectando = 5
	}
	#endregion

    #region Delegados de las cámaras
    /// <summary>
    /// Delegado de nueva fotografía
    /// </summary>
    /// <param name="estadoConexion"></param>
    public delegate void ODelegadoNuevaFotografiaCamara(OImage imagen);
    /// <summary>
    /// Delegado de nueva fotografía
    /// </summary>
    /// <param name="codigo"></param>
    /// <param name="estadoConexion"></param>
    public delegate void ODelegadoNuevaFotografiaCamaraAdv(string codigo, OImage imagen, DateTime momentoAdquisicion, double velocidadAdquisicion);
    /// <summary>
    /// Delegado de cambio de estaco de conexión de la cámara
    /// </summary>
    /// <param name="estadoConexion"></param>
    public delegate void ODelegadoCambioEstadoConexionCamara(OEstadoConexion estadoConexion);
    /// <summary>
    /// Delegado de cambio de estaco de conexión de la cámara
    /// </summary>
    /// <param name="codigo"></param>
    /// <param name="estadoConexion"></param>
    public delegate void ODelegadoCambioEstadoConexionCamaraAdv(string codigo, OEstadoConexion estadoConexionActual, OEstadoConexion estadoConexionAnterior);
    /// <summary>
    /// Delegado de cambio de estaco de reproducción de la cámara
    /// </summary>
    /// <param name="estadoConexion"></param>
    public delegate void ODelegadoCambioEstadoReproduccionCamara(bool modoReproduccionContinua);
    /// <summary>
    /// Delegado de cambio de estaco de reproducción de la cámara
    /// </summary>
    /// <param name="codigo"></param>
    /// <param name="estadoConexion"></param>
    public delegate void ODelegadoCambioEstadoReproduccionCamaraAdv(string codigo, bool modoReproduccionContinua);
    #endregion

	#region Medición de la tasa de adquisición
	/// <summary>
	/// Se utiliza para medir la tasa de adquisición de la cámara
	/// </summary>
	public class OMedidorVelocidadAdquisicion
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
		public OMedidorVelocidadAdquisicion()
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
