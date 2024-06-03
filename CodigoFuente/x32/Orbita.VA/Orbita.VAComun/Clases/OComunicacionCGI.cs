//***********************************************************************
// Assembly         : Orbita.VAComun
// Author           : aibañez
// Created          : 13-12-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Orbita.VAComun
{
    /// <summary>
    /// Comunicación con aplicaciones CGI
    /// </summary>
    public class OComunicacionCGI
    {
        #region Atritubo(s)
        private HttpWebRequest Request;
        private WebResponse Response;
        public Stream Stream;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Código identificativo de la conexión
        /// </summary>
        private string _Codigo;
        /// <summary>
        /// Código identificativo de la conexión
        /// </summary>
        public string Codigo
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }

        /// <summary>
        /// Url de la conexión CGI
        /// </summary>
        private string _Url;
        /// <summary>
        /// Url de la conexión CGI
        /// </summary>
        public string Url
        {
            get { return _Url; }
            set { _Url = value; }
        }

        /// <summary>
        /// Usuario de la conexión
        /// </summary>
        private string _Usuario;
        /// <summary>
        /// Usuario de la conexión
        /// </summary>
        public string Usuario
        {
            get { return _Usuario; }
            set { _Usuario = value; }
        }

        /// <summary>
        /// Contraseña de la conexión
        /// </summary>
        private string _Contraseña;
        /// <summary>
        /// Contraseña de la conexión
        /// </summary>
        public string Contraseña
        {
            get { return _Contraseña; }
            set { _Contraseña = value; }
        }

        /// <summary>
        /// Indica si se usa un grupo de conexión separado
        /// </summary>
        private bool _UsaGrupoConexionSeparado;
        /// <summary>
        /// Indica si se usa un grupo de conexión separado
        /// </summary>
        public bool UsaGrupoConexionSeparado
        {
            get { return _UsaGrupoConexionSeparado; }
            set { _UsaGrupoConexionSeparado = value; }
        }

        /// <summary>
        /// Tiempo máximo de la duración de la lectura del CGI
        /// </summary>
        private int _TimeOut;
        /// <summary>
        /// Tiempo máximo de la duración de la lectura del CGI
        /// </summary>
        public int TimeOut
        {
            get { return _TimeOut; }
            set { _TimeOut = value; }
        }

        /// <summary>
        /// Indica que se realizado la conexión con el CGI
        /// </summary>
        private bool _Conectado;
        /// <summary>
        /// Indica que se realizado la conexión con el CGI
        /// </summary>
        public bool Conectado
        {
            get { return _Conectado; }
            set { _Conectado = value; }
        }

        /// <summary>
        /// Extrae tidpo del contenido de la respuesta
        /// </summary>
        public TipoContenidoRespuestaCGI TipoContenido
        {
            get
            {
                TipoContenidoRespuestaCGI resultado = TipoContenidoRespuestaCGI.Desconocido;

                try
                {
                    if ((this.Conectado) && (this.Response != null))
                    {
                        if (this.Response.ContentType.IndexOf("text/plain") != -1)
                        {
                            resultado = TipoContenidoRespuestaCGI.TextPlain;
                        }
                        else if (this.Response.ContentType.IndexOf("image/jpeg") != -1)
                        {
                            resultado = TipoContenidoRespuestaCGI.ImageJpeg;
                        }
                        else if (this.Response.ContentType.IndexOf("multipart/x-mixed-replace") != -1)
                        {
                            resultado = TipoContenidoRespuestaCGI.MultipartXMixedReplace;
                        }
                    }
                }
                catch (Exception exception)
                {
                    resultado = TipoContenidoRespuestaCGI.Desconocido;
                    OVALogsManager.Error(OModulosSistema.Comun, "ExtraeTipoContenidoRespuestaComandoCGI", exception);
                }

                return resultado;
            }
        }

        public byte[] Delimitador
        {
            get
            {
                byte[] resultado = new byte[0];

                try
                {
                    if ((this.Conectado) && (this.Response != null))
                    {
                        // get boundary
                        int indiceBoundary = this.Response.ContentType.IndexOf("boundary=", 0);
                        if (indiceBoundary != -1)
                        {
                            ASCIIEncoding encoding = new ASCIIEncoding();
                            resultado = encoding.GetBytes(this.Response.ContentType.Substring(indiceBoundary + 9));
                        }
                    }
                }
                catch (Exception exception)
                {
                    OVALogsManager.Error(OModulosSistema.Comun, "ExtraeLimiteRespuestaComandoCGI", exception);
                }

                return resultado;
            }
        }
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OComunicacionCGI()
        {
            this.Url = string.Empty;
            this.Usuario = string.Empty;
            this.Contraseña = string.Empty;
            this.Codigo = string.Empty;
            this.UsaGrupoConexionSeparado = false;
            this.TimeOut = 15000;
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OComunicacionCGI(string url, string usuario, string contraseña, string codigo, bool usaGrupoConexionSeparado, int timeOut)
        {
            this.Url = url;
            this.Usuario = usuario;
            this.Contraseña = contraseña;
            this.Codigo = codigo;
            this.UsaGrupoConexionSeparado = usaGrupoConexionSeparado;
            this.TimeOut = timeOut;
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Inicia la conexión con el CGI
        /// </summary>
        /// <returns>Verdadero si la conexión se ha establecido con éxito</returns>
        public bool Start()
        {
            bool resultado = false;

            this.Request = null;
            this.Response = null;
            this.Stream = null;

            try
            {
                // Creamos la consulta CGI
                this.Request = (HttpWebRequest)WebRequest.Create(this.Url);

                // set login and password
                if ((this.Usuario != null) && (this.Contraseña != null) && (this.Usuario != ""))
                {
                    this.Request.Credentials = new NetworkCredential(this.Usuario, this.Contraseña);
                }

                // set connection group name
                if (this.UsaGrupoConexionSeparado)
                {
                    this.Request.ConnectionGroupName = this.Codigo;
                }

                this.Request.Method = "GET";
                this.Request.Timeout = this.TimeOut;

                this.Response = this.Request.GetResponse();
                this.Stream = this.Response.GetResponseStream();
                this.Stream.ReadTimeout = this.TimeOut;

                resultado = true;
            }
            catch (Exception exception)
            {
                this.Request = null;
                this.Response = null;
                this.Stream = null;
                OVALogsManager.Info(OModulosSistema.Comun, "StartComandoCGI", exception);
            }

            this.Conectado = resultado;
            return resultado;
        }

        /// <summary>
        /// Finaliza la conexión con el CGI
        /// </summary>
        /// <returns>Verdadero si la conexión se ha finalizado con éxito</returns>
        public bool Stop()
        {
            bool resultado = false;

            try
            {
                if (this.Conectado)
                {
                    if (this.Stream != null)
                    {
                        this.Stream.Close();
                        this.Stream = null;
                    }

                    if (this.Response != null)
                    {
                        this.Response.Close();
                        this.Response = null;
                    }

                    resultado = true;
                    this.Conectado = false;
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosSistema.Comun, "StopComandoCGI", exception);
            }

            return resultado;
        }
        #endregion

        #region Enumerado(s)
        /// <summary>
        /// Indica el tipo de contenido de una consulta CGI
        /// </summary>
        public enum TipoContenidoRespuestaCGI
        {
            /// <summary>
            /// Desconocido
            /// </summary>
            Desconocido = 0,
            /// <summary>
            /// Texto
            /// </summary>
            TextPlain = 1,
            /// <summary>
            /// Imagen  JPG
            /// </summary>
            ImageJpeg = 2,
            /// <summary>
            /// Envio continuo de bytes
            /// </summary>
            MultipartXMixedReplace = 3
        }
        #endregion
    }

    /// <summary>
    /// Comunicación con aplicaciones CGI con respuesta de texto
    /// </summary>
    public class OComunicacionCGITexto : OComunicacionCGI
    {
        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OComunicacionCGITexto() :
            base()
        {
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OComunicacionCGITexto(string url, string usuario, string contraseña, string codigo, bool usaGrupoConexionSeparado, int timeOut) :
            base(url, usuario, contraseña, codigo, usaGrupoConexionSeparado, timeOut)
        {
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Envio de comando CGI para información de texto
        /// </summary>
        /// <returns>Verdadero si la ejecución se ha realizado con éxito</returns>
        public bool Ejecuta(out string respuesta)
        {
            bool resultado = false;
            respuesta = string.Empty;

            try
            {
                resultado = this.Start();

                if (resultado)
                {
                    resultado = this.TipoContenido == TipoContenidoRespuestaCGI.TextPlain;
                }

                if (resultado)
                {
                    StreamReader streamReaderInfo;
                    streamReaderInfo = new StreamReader(this.Stream);
                    respuesta = streamReaderInfo.ReadToEnd();
                    streamReaderInfo.Close();
                }

                this.Stop();
            }
            catch (Exception exception)
            {
                resultado = false;
                respuesta = string.Empty;
                OVALogsManager.Info(OModulosSistema.Comun, "EjecucionComandoCGITexto", exception);
            }

            return resultado;
        }
        #endregion
    }
}
