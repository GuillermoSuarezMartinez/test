using System;
using System.Collections;
using System.IO;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Aladdin.HASP;
using Orbita.Trazabilidad;
namespace Orbita.Utiles
{
    /// <summary>
    /// Gestion de llaves
    /// </summary>
    public class OHaspSN
    {
        #region Dlls
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern bool IsDebuggerPresent();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr GetCurrentProcess();
        /// <summary>
        /// CheckRemoteDebuggerPresent
        /// </summary>
        /// <param name="ProcessHandle"></param>
        /// <param name="DebuggerPresent"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CheckRemoteDebuggerPresent(
            [In] IntPtr ProcessHandle,
            [MarshalAs(UnmanagedType.Bool)] ref bool DebuggerPresent
         );
        #endregion

        #region Delegados y eventos
        /// <summary>
        /// Delegado cierre aplicación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void OrbitaCerrarAplicaciontHandler(object sender, OEventArgs e);
        /// <summary>
        /// Evento cierre aplicación
        /// </summary>
        public event OrbitaCerrarAplicaciontHandler OrbitaCerrarAplicacion;
        /// <summary>
        /// Delegado mensaje de aplicación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void OrbitaMensajeAplicaciontHandler(object sender, OEventArgs e);
        /// <summary>
        /// Evento mensaje de aplicación
        /// </summary>
        public event OrbitaMensajeAplicaciontHandler OrbitaMensajeAplicacion;
        #endregion

        #region Atributos
        /// <summary>
        /// Descripción de los productos que se van a licenciar
        /// </summary>
        public enum OProductos
        {
            /// <summary>
            /// Basico
            /// </summary>
            Basico = 1,
            /// <summary>
            /// LPR
            /// </summary>
            LPR = 2,
            /// <summary>
            /// OCR
            /// </summary>
            OCR = 3,
            /// <summary>
            /// ADR
            /// </summary>
            ADR = 4,
            /// <summary>
            /// Alarmas Mails
            /// </summary>
            AlarmasMails = 5,
            /// <summary>
            /// Alarmas SMS
            /// </summary>
            AlarmasSMS = 6,
            /// <summary>
            /// Envasado
            /// </summary>
            CCEnvasado = 7,
            /// <summary>
            /// Lector carin
            /// </summary>
            LectorCarin = 8
        }
        /// <summary>
        /// Tecnologías para licencia sw
        /// </summary>
        private enum OLicencias
        {
            HASPSentinel = 1,
            HASPUsb
        }
        /// <summary>
        /// Tecnologías utilizada para licenciar
        /// </summary>
        private OLicencias licencia;
        /// <summary>
        /// Tecnologías utilizada para licenciar
        /// </summary>
        private string nombreAplicacion;
        /// <summary>
        /// Clave del producto
        /// </summary>
        private string vendorCodeString =
        "nDziMIFvJOlsEoaQsgo91ii7QN2rTwS91Fd5n6KsPtAGGUaYE+M7v54JBT7VO7AhETH5yW+Q8SvAMTMQ" +
        "u8Sbw9xkbCRjGA+aDqdCgj2V1Xtos+TB7Z71EfiPOK4tmqZxfHwvK3Ybe2w3cuc/oDzHhBsCBlNcYDNs" +
        "sybWuDanQvmZV6RD9wUg1Nnn5viuoVdn4dlhACKLkn1RUg/buPmBaZp4p3luE6R0dWYMEvU671b3Hkxf" +
        "ZICjxhMRd4fq2QpvEJX9nd5UPfBykbG3oqp12apD+K4x/DBBBt3R+oV3NLNeDBw2jTkbKqBmrFfe2dDU" +
        "QNDDYrYHa8HqhK72dQNvambAEpdVjTFRif7cZWdlvOS+FxHrAw2SXrmC/wohjAMFW+PqFZCj+cqwda3+" +
        "7gF8MyeaP4+843lEer/pqiAiD1elM0BTmE8XXnKEdNIK/4QRDrsJqAfzWhsmMJHedZow75jUGtkg/YGB" +
        "ZTmBGQ4+HoVnOa3rURjQi+5Ij+CBjPsFralg/GQIg6mNpRv6UYJh/Sdv7H01e9G/EZOgtqbSu7Kfw6KE" +
        "fFca+3xE5dDwqFT42/HSTidNLDcK5Q5sKBorFI6uUGC4KUM6D5YfDHdL8/okNcKYNI3g/VVuU3eBZY+7" +
        "67iQwVyppJwejXa6lNWv4zixH7aWjw1/WVyOslyjPxHGCTTSabltvLFQbfLQBSaaKCnEGM0VV26W6nFi" +
        "cZm5VOFs0xmInr7bLloa9PpHiJ54ehOQTs/xESmcN3Z9kEHS9wWxIHrwpnYAzlb4xYM7BhMj2BfZByd0" +
        "qX10XbzIRreQa3/nM8A40H1Ods4O5wBmIZkKK8Q8qZecXP/K/hMNvwuA+iCg1g2LyJ2HUKqrhIGXxib9" +
        "4mV04tPfRKbKjVVhzSxZKOPPjpr/S8HGCtWIDjz7dqyaGl+6RUqMU+1jK1c=";
        /// <summary>
        /// Acceso local
        /// </summary>
        public const string LocalScopeText =
        "<?xml version=\"1.0\" encoding=\"UTF-8\" ?> <haspscope> <license_manager hostname =\"localhost\" /> </haspscope>";
        /// <summary>
        /// Coleccion de productos a licenciar
        /// </summary>
        private ArrayList productos;
        /// <summary>
        /// Hilo de comprobación de llaves
        /// </summary>
        private Thread threadHasp;
        /// <summary>
        /// Hilo de mensajes al usuario
        /// </summary>
        private Thread threadMensajes;
        /// <summary>
        /// Fecha cuando no se detecta la llave
        /// </summary>
        private DateTime FechaError;
        /// <summary>
        /// Minutos cuando empieza a advertir de que no hay llave
        /// </summary>
        private const int minutosAdvertencia = 120;//120;//2 horas
        /// <summary>
        /// Minutos cuando cierra la aplicacion si no hay llave
        /// </summary>
        private const int minutosError = 5760;//5760;//4 días
        /// <summary>
        /// Minutos sin llave
        /// </summary>
        public int minutosSN = 0;
        /// <summary>
        /// Indica si el control de llave se realiza sobre servicio
        /// </summary>
        private bool esServicio;
        /// <summary>
        /// Logger del sistema
        /// </summary>
        ILogger logger;
        /// <summary>
        /// Códigos del sistema
        /// </summary>
        private ArrayList swCodes;
        /// <summary>
        /// Licencia sw
        /// </summary>
        private string swCod;
        /// <summary>
        /// Indica al hilo de mensajes que muestre el mensaje de que no encuentra llave inicial
        /// </summary>
        private bool mensajeLlaveInicial = false;
        /// <summary>
        /// Tiempo de consulta
        /// </summary>
        private int segundosConsulta = 3600;//3600/1hora
        #endregion

        #region Propiedades
        /// <summary>
        /// Indica si hay alguien conectado
        /// </summary>
        private static bool Asociado
        {
            get
            {
                return System.Diagnostics.Debugger.IsAttached;
            }
        }
        /// <summary>
        /// Indica si se ejecuta en local
        /// </summary>
        private static bool Local
        {
            get
            {
                return IsDebuggerPresent();
            }
        }
        /// <summary>
        /// Indica si se ejecuta en remoto
        /// </summary>
        private static bool Remoto
        {
            get
            {
                bool presente = false;
                CheckRemoteDebuggerPresent(GetCurrentProcess(), ref presente);
                return presente;
            }
        }
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor de clase para llaves HASP
        /// </summary>
        public OHaspSN()
        {
            //ImpedirDepurador();
            Assembly assem = Assembly.GetEntryAssembly();
            this.nombreAplicacion = assem.Location;
        }
        #endregion

        #region Métodos

        #region depurador

        /// <summary>
        /// Impide la depuración del código
        /// </summary>
        private static void ImpedirDepurador()
        {
            Thread thread = new Thread(new ThreadStart(ThreadImpedirDepurador));
            thread.Start();
        }
        /// <summary>
        /// Impide la depuración del código
        /// </summary>
        private static void ThreadImpedirDepurador()
        {
            while (EsperarDepurador(5000, false))
            {
                //Std.SDE("No se permite asociar depuradores.\nGracias por intentarlo :)", true);
                FinalizarTodo(1);
            }
        }
        /// <summary>
        /// Finaliza el depurador
        /// </summary>
        public static void FinalizarTodo(int codigo = -1)
        {
            System.Environment.Exit(codigo);
        }
        /// <summary>
        /// Espera la ejecución del depurador
        /// </summary>
        private static bool EsperarDepurador(int espera = 1000, bool mensaje = true)
        {
            while (!Asociado && !Local && !Remoto)
            {
                if (mensaje)
                {
                    //Std.SD("Sigo esperando un depurador, siguiente comprobación en " + espera + " ms.", true, false, ConsoleColor.Magenta);
                }
                Thread.Sleep(espera);
            }


            return true;

        }

        #endregion

        /// <summary>
        /// Inicia el hilo para cualquier licenciamiento
        /// </summary>
        /// <param name="listaProductos">Productos sobre los que se realiza la inspeccion</param>
        /// <param name="esServicio">Indica si la aplicacion que realiza la llamada es un servicio</param>
        /// <param name="log">Log de la clase</param>
        public void IniHaspSN(ArrayList listaProductos, bool esServicio, ILogger log)
        {
            try
            {
                this.productos = listaProductos;
                this.esServicio = esServicio;

                if (log == null)
                {
                    log = LogManager.SetDebugLogger("loggerHsp", NivelLog.Debug);
                }

                this.logger = log;
                this.logger.Info("Comprobando lista de productos inicial");

                this.IncluirSerial();

                this.IniciarThreadMensajes();
                //Comprobamos la tecnología para licenciar
                this.ComprobarTecnologia();
                bool existeLlave = this.ComprobarEstadoInicial();

                if (!existeLlave)
                {
                    //esperamos 30 segundos a que den los mensajes iniciales y cerramos
                    mensajeLlaveInicial = true;
                    Thread.Sleep(30000);
                    this.CerrarAplicacion();
                }
                else
                {
                    this.logger.Info("Estado inicial de los productos OK");
                }

                this.IniciarThreadHasp();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //this.logger.Error("Excepción general. Se cerrará la aplicación. " + ex.ToString());
                this.CerrarAplicacion();
            }
        }
        /// <summary>
        /// Inicia el hilo de mensajes
        /// </summary>
        private void ComprobarTecnologia()
        {
            OProductos prodCheck = (OProductos)productos[0];

            this.swCod = "";

            try
            {
                using (StreamReader sr = new StreamReader(Application.StartupPath + @"\orbita.lic"))
                {
                    String line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        this.swCod = this.swCod + line;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            if (ComprobarProductoHASPSL(prodCheck))
            {
                this.licencia = OLicencias.HASPSentinel;
            }
            else if (ComprobarProductoHASPUsb(prodCheck))
            {
                this.licencia = OLicencias.HASPUsb;
            }
            else
            {
                //esperamos 30 segundos a que den los mensajes iniciales y cerramos
                mensajeLlaveInicial = true;
                Thread.Sleep(30000);
                this.CerrarAplicacion();
            }

        }
        /// <summary>
        /// Comprueba si existe licencia HASP Usb
        /// </summary>
        private bool ComprobarProductoHASPUsb(OProductos producto)
        {
            ArrayList inf = new ArrayList();
            bool resultado = false;

            try
            {
                string ret = "";
                ManagementObjectSearcher busq = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                foreach (ManagementObject dispositivo in busq.Get())
                {
                    ret = "";
                    try
                    {
                        ret += dispositivo["Signature"].ToString() + "|";
                        ret += dispositivo["TotalCylinders"].ToString() + "|";
                        ret += dispositivo["TotalHeads"].ToString() + "|";
                        ret += dispositivo["TotalSectors"].ToString() + "|";
                        ret += dispositivo["TotalTracks"].ToString() + "|";
                        ret += dispositivo["TracksPerCylinder"].ToString();
                        inf.Add(ret);
                    }
                    catch (Exception ex)
                    {
                        this.logger.Fatal("ComprobarProductoHASPUsb. Error al comprobar la licencia1: " + ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.Fatal("ComprobarProductoHASPUsb. Error al comprobar la licencia2: " + ex.ToString());
            }

            for (int i = 0; i < inf.Count; i++)
            {
                string text = inf[i].ToString();
                if (ExisteSerial(text))
                {
                    resultado = true;
                }
            }

            return resultado;
        }
        /// <summary>
        /// Comprueba si el producto esta en la llave
        /// </summary>
        /// <param name="cod">Producto a comprobar</param>
        /// <returns>Verdadero si está el producto y falso si no esta</returns>
        private bool ExisteSerial(string cod)
        {
            bool ret = false;

            if (this.swCodes.Contains(cod))
            {
                ret = true;
            }

            return ret;
        }
        /// <summary>
        /// Comprueba si existe licencia HASP Sentinel
        /// </summary>
        private bool ComprobarProductoHASPSL(OProductos producto)
        {
            HaspFeature feature = HaspFeature.FromFeature((int)producto);

            bool resultado = false;

            try
            {
                using (Hasp hasp = new Hasp(feature))
                {
                    HaspStatus status = hasp.Login(vendorCodeString, LocalScopeText);
                    resultado = hasp.IsLoggedIn();

                    if (!resultado)
                    {
                        this.logger.Warn("ComprobarProductoHASPSL. No se encuentra el producto " + producto.ToString());
                        OEventArgs e = new OEventArgs();
                        using (oMsgHasp msg = new oMsgHasp())
                        {
                            msg.Estado = "error";
                            msg.Producto = producto.ToString();
                            msg.Mensaje = status.ToString();
                            e.Argumento = msg;
                            OrbitaMensajeAplicacion(this, e);
                        }
                    }

                    status = hasp.Logout();
                }
            }
            catch (Exception exception)
            {
                this.logger.Fatal("ComprobarProductoHASPSL. Error al comprobar la licencia: " + exception.ToString());
                resultado = false;
            }

            return resultado;
        }
        /// <summary>
        /// Inicia el hilo de mensajes
        /// </summary>
        private void IniciarThreadMensajes()
        {
            this.threadMensajes = new Thread(new ThreadStart(this.TareasMensaje));
            this.threadMensajes.Name = "ThreadMN";
            this.threadMensajes.IsBackground = true;
            this.threadMensajes.Start();
        }
        /// <summary>
        /// Muestra un mensaje si no encuentra licencia si los minutos superan a los minutos de advertencia
        /// </summary>
        private void TareasMensaje()
        {
            while (true)
            {
                if (this.minutosSN > minutosAdvertencia)
                {
                    if (!this.esServicio)
                    {
                        string tareas = "No se ha encontrado la licencia para este producto.";
                        MessageBox.Show(tareas);
                        this.logger.Info(tareas);
                    }
                }
                if (mensajeLlaveInicial)
                {
                    if (!this.esServicio)
                    {
                        string tareas = "No se ha encontrado la licencia para este producto. La aplicación se detendrá";
                        MessageBox.Show(tareas);
                        this.logger.Info(tareas);
                    }
                }
                Thread.Sleep(10000);
            }

        }
        /// <summary>
        /// Realiza la inspeccion inicial y si no encuentra la llave cierra la aplicacion
        /// </summary>
        /// <returns>Verdadero si están los productos y falso si no están</returns>
        private bool ComprobarEstadoInicial()
        {
            bool retorno = false;
            for (int i = 0; i < productos.Count; i++)
            {
                OProductos prodCheck = (OProductos)productos[i];
                retorno = false;
                if (this.licencia == OLicencias.HASPSentinel)
                {
                    retorno = this.ComprobarProductoHASPSL(prodCheck);

                    if (!retorno)
                    {
                        return false;
                    }
                }
                else if (this.licencia == OLicencias.HASPUsb)
                {
                    retorno = this.ComprobarProductoHASPUsb(prodCheck);
                    //Comprobamos los productos
                    if (retorno)
                    {
                        retorno = this.ProcesarXMLProd(prodCheck);

                        if (!retorno)
                        {
                            OEventArgs e = new OEventArgs();
                            using (oMsgHasp msg = new oMsgHasp())
                            {
                                msg.Estado = "error";
                                msg.Producto = prodCheck.ToString();
                                msg.Mensaje = "Error al comprobar los productos usb";
                                e.Argumento = msg;
                                OrbitaMensajeAplicacion(this, e);
                            }
                            return false;
                        }
                    }
                    else
                    {
                        OEventArgs e = new OEventArgs();
                        using (oMsgHasp msg = new oMsgHasp())
                        {
                            msg.Estado = "error";
                            msg.Producto = prodCheck.ToString();
                            msg.Mensaje = "Error al comprobar la llave usb";
                            e.Argumento = msg;
                            OrbitaMensajeAplicacion(this, e);
                        }
                        return false;
                    }
                }

                if (!retorno)
                {
                    this.logger.Info("TareasHasp. No se encuentra la licencia de " + prodCheck.ToString());
                    TimeSpan ts = DateTime.Now - FechaError;
                    minutosSN = (int)ts.TotalMinutes;

                    if (ts.TotalMinutes > minutosError)
                    {
                        this.logger.Info("TareasHasp. Se para la aplicación por falta de licencia de " + prodCheck.ToString());
                        this.CerrarAplicacion();
                    }
                }
                else
                {
                    FechaError = DateTime.Now;
                    minutosSN = 0;
                    OEventArgs e = new OEventArgs();
                    using (oMsgHasp msg = new oMsgHasp())
                    {
                        msg.Estado = "OK";
                        msg.Producto = prodCheck.ToString();
                        msg.Mensaje = "";
                        e.Argumento = msg;
                        OrbitaMensajeAplicacion(this, e);
                    }
                }
            }
            return retorno;
        }
        /// <summary>
        /// Inicia el hilo de comprobacion
        /// </summary>
        private void IniciarThreadHasp()
        {
            this.threadHasp = new Thread(new ThreadStart(this.TareasHasp));
            this.threadHasp.Name = "ThreadSN";
            this.threadHasp.IsBackground = true;
            this.threadHasp.Start();
        }
        /// <summary>
        /// Comprueba si estan las licencias y gestiona los tiempos sin licencia
        /// </summary>   
        private void TareasHasp()
        {
            int tpoEspera = this.segundosConsulta * 1000;
            FechaError = DateTime.Now;
            while (true)
            {
                for (int i = 0; i < productos.Count; i++)
                {
                    OProductos prodCheck = (OProductos)productos[i];

                    bool check = false;

                    if (this.licencia == OLicencias.HASPSentinel)
                    {
                        check = this.ComprobarProductoHASPSL(prodCheck);
                    }
                    else if (this.licencia == OLicencias.HASPUsb)
                    {
                        check = this.ComprobarProductoHASPUsb(prodCheck);
                        //Comprobamos los productos
                        if (check)
                        {
                            check = this.ProcesarXMLProd(prodCheck);

                            if (!check)
                            {
                                OEventArgs e = new OEventArgs();
                                using (oMsgHasp msg = new oMsgHasp())
                                {
                                    msg.Estado = "error";
                                    msg.Producto = prodCheck.ToString();
                                    msg.Mensaje = "Error al comprobar los productos usb";
                                    e.Argumento = msg;
                                    OrbitaMensajeAplicacion(this, e);
                                }
                            }
                        }
                        else
                        {
                            OEventArgs e = new OEventArgs();
                            using (oMsgHasp msg = new oMsgHasp())
                            {
                                msg.Estado = "error";
                                msg.Producto = prodCheck.ToString();
                                msg.Mensaje = "Error al comprobar la llave usb";
                                e.Argumento = msg;
                                OrbitaMensajeAplicacion(this, e);
                            }
                        }
                    }

                    if (!check)
                    {
                        this.logger.Info("TareasHasp. No se encuentra la licencia de " + prodCheck.ToString());
                        TimeSpan ts = DateTime.Now - FechaError;
                        minutosSN = (int)ts.TotalMinutes;

                        if (ts.TotalMinutes > minutosError)
                        {
                            this.logger.Info("TareasHasp. Se para la aplicación por falta de licencia de " + prodCheck.ToString());
                            this.CerrarAplicacion();
                        }
                    }
                    else
                    {
                        FechaError = DateTime.Now;
                        minutosSN = 0;
                        OEventArgs e = new OEventArgs();
                        using (oMsgHasp msg = new oMsgHasp())
                        {
                            msg.Estado = "OK";
                            msg.Producto = prodCheck.ToString();
                            msg.Mensaje = "";
                            e.Argumento = msg;
                            OrbitaMensajeAplicacion(this, e);
                        }
                    }
                }

                Thread.Sleep(tpoEspera);
            }
        }
        /// <summary>
        /// Comprueba si el producto esta en la llave
        /// </summary>
        /// <param name="producto">Producto a comprobar</param>
        /// <returns>Verdadero si está el producto y falso si no esta</returns>
        private bool ProcesarXMLProd(OProductos producto)
        {
            bool ret = false;
            int prod = (int)producto;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.InnerXml = Orbita.Utiles.OCifrado.DesencriptarTexto(this.swCod);
            XmlNamespaceManager ns = new XmlNamespaceManager(xmlDoc.NameTable);
            //obtenemos el identificador de la llave
            string ID = xmlDoc.SelectSingleNode("//ID", ns).InnerText;

            //obtenemos los productos
            XmlNodeList nodeList = xmlDoc.GetElementsByTagName("PROD");

            foreach (XmlNode node in nodeList)
            {
                XmlNamedNodeMap mapAttributes = node.Attributes;
                foreach (XmlNode xnodAttribute in mapAttributes)
                {
                    int val = Convert.ToInt32(xnodAttribute.Value);

                    if (prod == val)
                    {
                        ret = true;
                    }
                }
            }

            nodeList = null;
            ns = null;
            xmlDoc = null;
            GC.Collect();

            return ret;
        }
        /// <summary>
        /// Introduce los códigos de los productos
        /// </summary>
        private void IncluirSerial()
        {
            this.swCodes = new ArrayList();
            swCodes.Add(@"2034811058|486|255|7807590|123930|255");
            swCodes.Add(@"2034811056|486|255|7807590|123930|255");
        }
        /// <summary>
        /// Cierra la aplicación
        /// </summary>
        private void CerrarAplicacion()
        {
            OEventArgs e = new OEventArgs();
            OrbitaCerrarAplicacion(this, e);
            if (this.esServicio)
            {
                string service = "";
                String query = "SELECT Name,PathName FROM Win32_Service";
                using (ManagementObjectSearcher mos = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject mo in mos.Get())
                    {
                        if (mo["PathName"].ToString().ToLower().Contains(this.nombreAplicacion.ToLower()))
                        {
                            service = mo["Name"].ToString();
                        }
                    }
                }

                foreach (ServiceController servicio in ServiceController.GetServices("."))
                {
                    if (servicio.ServiceName.ToLower() == service.ToLower())
                    {
                        servicio.Stop();
                        while (servicio.Status != ServiceControllerStatus.Stopped)
                        {
                            servicio.Stop();
                        }
                    }

                }
            }
            Application.Exit();
        }
        #endregion
    }

    /// <summary>
    /// oOPCComms.
    /// </summary>
    [Serializable]
    public class oMsgHasp:IDisposable
    {
        #region Atributos
        /// <summary>
        /// Producto.
        /// </summary>
        string _producto;
        /// <summary>
        /// Estado.
        /// </summary>
        string _estado;
        /// <summary>
        /// Mensaje.
        /// </summary>
        string _mensaje;
        #endregion

        #region Propiedades
        /// <summary>
        /// Estado.
        /// </summary>
        public string Estado
        {
            get { return _estado; }
            set { _estado = value; }
        }
        /// <summary>
        /// Producto.
        /// </summary>
        public string Producto
        {
            get { return _producto; }
            set { _producto = value; }
        }
        /// <summary>
        /// Mensaje.
        /// </summary>
        public string Mensaje
        {
            get { return _mensaje; }
            set { _mensaje = value; }
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {            
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
