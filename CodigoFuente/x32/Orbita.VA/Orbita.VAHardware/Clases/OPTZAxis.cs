using System;
using System.Collections.Generic;
using System.Text;
using Orbita.VAComun;
using System.Net;
using System.Data;
using System.Threading;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Clase base para todos los controladores ptz
    /// </summary>
    public class OPTZAxis: OPTZBase
    {
        #region Atributo(s)
        /// <summary>
        /// Dirección URL original
        /// </summary>
        private string URLOriginal;
        /// <summary>
        /// Dirección URL del video
        /// </summary>
        private string URL;
        /// <summary>
        /// Dirección IP de la cámara
        /// </summary>
        private IPAddress IP;
        /// <summary>
        /// Puerto de conexión con la cámara
        /// </summary>
        private int Puerto;
        /// <summary>
        /// Usuario para el acceso a la cámara
        /// </summary>
        private string Usuario;
        /// <summary>
        /// Contraseña para el acceso a la cámara
        /// </summary>
        private string Contraseña;
        /// <summary>
        /// TimeOut de envio de comando
        /// </summary>
        private int TimeOutMs;
        /// <summary>
        /// El método de actualización de la posición actual es mediante pooling
        /// </summary>
        private bool Pooling;
        /// <summary>
        /// Intervalo de tiempo entre actualizaciones de la posición actual (Expresado en milisegundos)
        /// </summary>
        private int IntervaloPoolingMs;
        /// <summary>
        /// Thread de consulta
        /// </summary>
        private OThread Thread;
        #endregion

        #region Propiedad(es) Virtual(es)
        /// <summary>
        /// Obtiene la información de los comandos permitidos por el dispositivo PTZ
        /// </summary>
        public override OMovimientosPTZ ComandosPermitidos
        {
            get 
            {
                return new OMovimientosPTZ( 
                new OMovimientoPTZ(OEnumTipoMovimientoPTZ.Pan, OEnumModoMovimientoPTZ.Absoluto),
                new OMovimientoPTZ(OEnumTipoMovimientoPTZ.Pan, OEnumModoMovimientoPTZ.Relativo),
                new OMovimientoPTZ(OEnumTipoMovimientoPTZ.Tilt, OEnumModoMovimientoPTZ.Absoluto),
                new OMovimientoPTZ(OEnumTipoMovimientoPTZ.Tilt, OEnumModoMovimientoPTZ.Relativo),
                new OMovimientoPTZ(OEnumTipoMovimientoPTZ.Zoom, OEnumModoMovimientoPTZ.Absoluto),
                new OMovimientoPTZ(OEnumTipoMovimientoPTZ.Zoom, OEnumModoMovimientoPTZ.Relativo),
                new OMovimientoPTZ(OEnumTipoMovimientoPTZ.Focus, OEnumModoMovimientoPTZ.Relativo));
            }
        }

        /// <summary>
        /// Obtiene la información de los movimientos permitidos por el dispositivo PTZ
        /// </summary>
        public override OTiposMovimientosPTZ MovimientosPermitidos
        {
            get
            {
                return new OTiposMovimientosPTZ( 
                    OEnumTipoMovimientoPTZ.Pan,
                    OEnumTipoMovimientoPTZ.Tilt,
                    OEnumTipoMovimientoPTZ.Zoom,
                    OEnumTipoMovimientoPTZ.Focus);
            }
        }
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OPTZAxis(string codigo) :
            base(codigo)
        {
            try
            {
                DataTable dt = AppBD.GetCamara(codigo);
                if (dt.Rows.Count == 1)
                {
                    this.IP = IPAddress.Parse(dt.Rows[0]["IPCam_IP"].ToString());
                    this.URLOriginal = OTextoRobusto.Validar(dt.Rows[0]["PTZAxis_URL"], 200, false, false, "http:\\%IP%");
                    this.Puerto = OEnteroRobusto.Validar(dt.Rows[0]["IPCam_Puerto"], 0, int.MaxValue, 80);
                    this.Usuario = dt.Rows[0]["IPCam_Usuario"].ToString();
                    this.Contraseña = dt.Rows[0]["IPCam_Contraseña"].ToString();
                    this.TimeOutMs = OEnteroRobusto.Validar(dt.Rows[0]["PTZAxis_TimeOutMs"], 1, int.MaxValue, 1000);
                    this.Pooling = OBoolRobusto.Validar(dt.Rows[0]["PTZAxis_Pooling"], false);
                    this.IntervaloPoolingMs = OEnteroRobusto.Validar(dt.Rows[0]["PTZAxis_Intervalo_PoolingMs"], 1, int.MaxValue, 250);

                    // Construcción de la url
                    string url = this.URLOriginal;
                    url = App.StringReplace(url, @"%IPCam_IP%", this.IP.ToString(), StringComparison.OrdinalIgnoreCase);
                    url = App.StringReplace(url, @"%IPCam_Puerto%", this.Puerto.ToString(), StringComparison.OrdinalIgnoreCase);
                    url = App.StringReplace(url, @"%IPCam_Usuario%", this.Usuario, StringComparison.OrdinalIgnoreCase);
                    url = App.StringReplace(url, @"%IPCam_Contraseña%", this.Contraseña, StringComparison.OrdinalIgnoreCase);
                    this.URL = url;
                }

                this.Thread = new OThread("PTZ_" + this.Codigo, this.IntervaloPoolingMs, ThreadPriority.BelowNormal);
            }
            catch (Exception exception)
            {
                OVALogsManager.Fatal(OModulosHardware.Camaras, this.Codigo, exception);
                throw new Exception("Imposible iniciar el PTZ " + this.Codigo);
            }
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Devuelve el string utilizado por el CGI para cada movimiento PTZ
        /// </summary>
        /// <param name="tipo">Movimiento a convertir a string</param>
        /// <returns>String del movimiento que será utilizado construir la consulta PTZ</returns>
        private string TipoMovimientoToString(OEnumTipoMovimientoPTZ tipo, OEnumModoMovimientoPTZ modo)
        {
            string resultado = string.Empty;

            switch (tipo)
            {
                case OEnumTipoMovimientoPTZ.Pan:
                    resultado = "pan";
                    break;
                case OEnumTipoMovimientoPTZ.Tilt:
                    resultado = "tilt";
                    break;
                case OEnumTipoMovimientoPTZ.Zoom:
                    resultado = "zoom";
                    break;
                case OEnumTipoMovimientoPTZ.Iris:
                    resultado = "iris";
                    break;
                case OEnumTipoMovimientoPTZ.Focus:
                    resultado = "focus";
                    break;
            }

            switch (modo)
            {
                case OEnumModoMovimientoPTZ.Absoluto:
                    break;
                case OEnumModoMovimientoPTZ.Relativo:
                    resultado = "r" + resultado;
                    break;
            }

            return resultado;
        }

        /// <summary>
        /// Devuelve el string del valor del movimiento utilizado por el CGI para cada movimiento PTZ
        /// </summary>
        /// <param name="tipo">Valor de un movimiento a convertir a string</param>
        /// <returns>String del valor de un movimiento que será utilizado construir la consulta PTZ</returns>
        private string ValorMovimientoToString(OEnumTipoMovimientoPTZ tipo, OEnumModoMovimientoPTZ modo, double valor)
        {
            string resultado = string.Empty;

            switch (tipo)
            {
                case OEnumTipoMovimientoPTZ.Pan:
                    switch (modo)
	                {
                        case OEnumModoMovimientoPTZ.Absoluto:
                            float valorPanA = (float)ODecimalRobusto.Validar(valor, -180, 180, 0);
                            resultado = valorPanA.ToString("{0:0.0}"); //float
                            break;
                        case OEnumModoMovimientoPTZ.Relativo:
                            float valorPanR = (float)ODecimalRobusto.Validar(valor, -360, 360, 0);
                            resultado = valorPanR.ToString("{0:0.0}"); //float
                            break;
	                }
                    break;
                case OEnumTipoMovimientoPTZ.Tilt:
                    switch (modo)
	                {
                        case OEnumModoMovimientoPTZ.Absoluto:
                            float valorTiltA = (float)ODecimalRobusto.Validar(valor, -180, 180, 0);
                            resultado = valorTiltA.ToString("{0:0.0}"); //float
                            break;
                        case OEnumModoMovimientoPTZ.Relativo:
                            float valorTiltR = (float)ODecimalRobusto.Validar(valor, -360, 360, 0);
                            resultado = valorTiltR.ToString("{0:0.0}"); //float
                            break;
	                }
                    break;
                case OEnumTipoMovimientoPTZ.Zoom:
                    switch (modo)
	                {
                        case OEnumModoMovimientoPTZ.Absoluto:
                            int valorZoomA = (int)ODecimalRobusto.Validar(valor, 1, 9999, 0);
                            resultado = valorZoomA.ToString(); //int
                            break;
                        case OEnumModoMovimientoPTZ.Relativo:
                            int valorZoomR = (int)ODecimalRobusto.Validar(valor, -9999, 9999, 0);
                            resultado = valorZoomR.ToString(); //int
                            break;
	                }
                    break;
                case OEnumTipoMovimientoPTZ.Iris:
                    switch (modo)
	                {
                        case OEnumModoMovimientoPTZ.Absoluto:
                            int valorIrisA = (int)ODecimalRobusto.Validar(valor, 1, 9999, 0);
                            resultado = valorIrisA.ToString(); //int
                            break;
                        case OEnumModoMovimientoPTZ.Relativo:
                            int valorIrisR = (int)ODecimalRobusto.Validar(valor, -9999, 9999, 0);
                            resultado = valorIrisR.ToString(); //int
                            break;
	                }
                    break;
                case OEnumTipoMovimientoPTZ.Focus:
                    switch (modo)
	                {
                        case OEnumModoMovimientoPTZ.Absoluto:
                            int valorFocusA = (int)ODecimalRobusto.Validar(valor, 1, 9999, 0);
                            resultado = valorFocusA.ToString(); //int
                            break;
                        case OEnumModoMovimientoPTZ.Relativo:
                            int valorFocusR = (int)ODecimalRobusto.Validar(valor, -9999, 9999, 0);
                            resultado = valorFocusR.ToString(); //int
                            break;
	                }
                    break;
                default:
                    resultado = string.Empty;
                    break;
            }

            return resultado;
        }

        /// <summary>
        /// Devuelve la cadena de texto conjunta del tipo de movimiento y el valor utilizado por el CGI para cada movimiento PTZ
        /// </summary>
        /// <param name="movimientoPTZ">Valor de un movimiento a convertir a string</param>
        /// <returns>String del valor de un movimiento que será utilizado construir la consulta PTZ</returns>
        private string TipoMovimientoYValorToString(OEnumTipoMovimientoPTZ tipo, OEnumModoMovimientoPTZ modo, double valor)
        {
            return string.Format("{0}={1}", this.TipoMovimientoToString(tipo, modo), this.ValorMovimientoToString(tipo, modo, valor));
        }

        /// <summary>
        /// Devuelve una posiciones a partir de la cadena devuelta por el CGI
        /// </summary>
        /// <param name="strTipo">Cadena del tipo devuelta por el CGI</param>
        /// <param name="strValor">Cadena del valor devuelta por el CGI</param>
        /// <returns>Posiciones</returns>
        private OPosicionPTZ StringToPosicion(string strTipo, string strValor)
        {
            // Inicialización de resultado
            OPosicionPTZ resultado = new OPosicionPTZ();

            OEnumRobusto<OEnumTipoMovimientoPTZ> tipo = new OEnumRobusto<OEnumTipoMovimientoPTZ>("TipoPosicion", OEnumTipoMovimientoPTZ.Focus, false);
            tipo.ValorGenerico = strTipo;
            if (tipo.Valido)
            {
                resultado.Tipo = tipo.Valor;
            }

            switch (resultado.Tipo)
            {
                case OEnumTipoMovimientoPTZ.Pan:
                case OEnumTipoMovimientoPTZ.Tilt:
                    ODecimalRobusto valorDouble = new ODecimalRobusto("ValorDouble", -180, 180, 0, false);
                    valorDouble.ValorGenerico = strValor;
                    if (valorDouble.Valido)
                    {
                        resultado.Valor = valorDouble.Valor;
                    }
                    break;
                case OEnumTipoMovimientoPTZ.Zoom:
                case OEnumTipoMovimientoPTZ.Iris:
                case OEnumTipoMovimientoPTZ.Focus:
                    OEnteroRobusto valorEntero = new OEnteroRobusto("ValorEntero", 1, 9999, 1, false);
                    valorEntero.ValorGenerico = strValor;
                    if (valorEntero.Valido)
                    {
                        resultado.Valor = valorEntero.Valor;
                    }
                    break;
            }
            
            // Devolución de resultados
            return resultado;
        }

        /// <summary>
        /// Devuelve una lista de posiciones a partir de la cadena devuelta por el CGI
        /// </summary>
        /// <param name="strPosiciones">Cadena devuelta por el CGI</param>
        /// <returns>lista de posiciones</returns>
        private OPosicionesPTZ StringToPosiciones(string strPosiciones)
        {
            // Inicialización de resultado
            OPosicionesPTZ resultado = new OPosicionesPTZ();

            // Verificación de la cadena de entrada
            if ((strPosiciones.Length <= 3) || (strPosiciones.IndexOf('=') == -1))
            {
                throw new Exception("cadena devuelta por CGI incorrecta");
            }

            // Extracción de información
            string[] strSplit = strPosiciones.Split('\n', '\r');
            foreach (string strPosicion in strSplit)
            {
                int indice = strPosicion.IndexOf('=');
                if (indice != -1)
                {
                    string strTipo = strPosicion.Substring(0, indice);
                    string strValor = strPosicion.Substring(indice + 1);

                    OPosicionPTZ posicion = this.StringToPosicion(strTipo, strValor);
                    resultado.Add(posicion);
                }
            }

            // Devolución de resultados
            return resultado;
        }

        /// <summary>
        /// Método de la ejecución del thread
        /// </summary>
        private void ThreadRun(ref bool finalize)
        {
            finalize = false;
            this.ConsultaPosicion();
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Carga los valores de la cámara
        /// </summary>
        public override void Inicializar()
        {
            //this.Thread.OnEjecucion += this.ThreadRun;
            this.Thread.CrearSuscripcionRun(this.ThreadRun, true);
            if (this.Pooling)
            {
                this.Thread.Start();
            }
        }

        /// <summary>
        /// Finaliza la cámara
        /// </summary>
        public override void Finalizar()
        {
            //this.Thread.OnEjecucion -= this.ThreadRun;
            if (this.Pooling)
            {
                this.Thread.Stop(this.TimeOutMs);
            }
        }

        /// <summary>
        /// Ejecuta un movimiento compuesto de ptz
        /// </summary>
        /// <param name="valores">Tipos de movimientos y valores</param>
        /// <returns>Verdadero si se ha ejecutado correctamente</returns>
        public override bool EjecutaMovimiento(OComandosPTZ valores)
        {
            bool resultado = false;
            try
            {
                resultado = base.EjecutaMovimiento(valores);

                if (resultado)
                {
                    string urlComando = this.URL + "?";
                    foreach (OComandoPTZ comando in valores)
                    {
                        urlComando += this.TipoMovimientoYValorToString(comando.Movimiento.Tipo, comando.Movimiento.Modo, comando.Valor) + "&";
                    }

                    OComunicacionCGITexto comunicacionCGITexto = new OComunicacionCGITexto(urlComando, this.Usuario, this.Contraseña, this.Codigo, true, this.TimeOutMs);
                    string respuesta;
                    if (comunicacionCGITexto.Ejecuta(out respuesta))
                    {
                        resultado = respuesta != string.Empty;
                    }
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.PTZ, this.Codigo, exception);
            }

            return resultado;
        }

        /// <summary>
        /// Actualiza la posición actual de un determinado movimiento PTZ
        /// </summary>
        /// <returns>Listado de posiciones actuales</returns>
        public override OPosicionesPTZ ConsultaPosicion()
        {
            OPosicionesPTZ resultado = this._Posicion;

            try
            {
                string urlComando = this.URL + "?query=position";
                OComunicacionCGITexto comunicacionCGITexto = new OComunicacionCGITexto(urlComando, this.Usuario, this.Contraseña, this.Codigo, true, this.TimeOutMs);
                string respuesta;
                if (comunicacionCGITexto.Ejecuta(out respuesta))
                {
                    // Interpretación de la respuesta
                    resultado = this.StringToPosiciones(respuesta);
                    this._Posicion = resultado;
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Info(OModulosHardware.PTZ, this.Codigo, exception);
            }

            return resultado;
        }

        /// <summary>
        /// Actualiza la posición actual de un determinado movimiento PTZ
        /// </summary>
        /// <returns>Posición actuales</returns>
        public override OPosicionPTZ ConsultaPosicion(OEnumTipoMovimientoPTZ movimiento)
        {
            OPosicionPTZ resultado = new OPosicionPTZ();

            try
            {
                this.ConsultaPosicion();

                if (this._Posicion.Exists(movimiento))
                {
                    resultado = this._Posicion[movimiento];
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.PTZ, this.Codigo, exception);
            }

            return resultado;
        }
        #endregion
    }
}