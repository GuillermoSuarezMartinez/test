//***********************************************************************
// Assembly         : Orbita.VAComun
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 16-11-2012
// Description      : Nuevo tipo de Monitorización: MonitorizacionObjetosMemoria
//                     Se controla las clases LargeObects existentes en el sistema
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Orbita.VAComun
{
    /// <summary>
    /// Clase utilizada para la monitorización del estado del PC
    /// </summary>
    public static class MonitorizacionSistemaRuntime
    {
        #region Atributo(s)
        /// <summary>
        /// Listado de las monitorizaciones
        /// </summary>
        public static List<MonitorizacionSistemaBase> ListaMonitorizaciones;
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Construye los objetos
        /// </summary>
        public static void Constructor()
        {
            ListaMonitorizaciones = new List<MonitorizacionSistemaBase>();
        }

        /// <summary>
        /// Destruye los objetos
        /// </summary>
        public static void Destructor()
        {
            ListaMonitorizaciones = null;
        }

        /// <summary>
        /// Se cargan los valores de la clase
        /// </summary>
        public static void Inicializar()
        {
            foreach (MonitorizacionSistemaBase monitorizacionSistema in ListaMonitorizaciones)
            {
                monitorizacionSistema.Inicializar();
            }
        }

        /// <summary>
        /// Se finaliza la ejecución
        /// </summary>
        public static void Finalizar()
        {
        }

        /// <summary>
        /// Añade una nueva monitorización al runtime
        /// </summary>
        /// <param name="tipoMonitorizacion">Tipo de Monitorización a cargar</param>
        public static void NuevaMonitorizacion(TipoMonitorizacion tipoMonitorizacion)
        {
            MonitorizacionSistemaBase monitorizacion;

            switch (tipoMonitorizacion)
            {
                case TipoMonitorizacion.MonitorizacionProcesos:
                default:
                    monitorizacion = new MonitorizacionProcesos();
                    break;
                case TipoMonitorizacion.MonitorizacionCpuRam:
                    monitorizacion = new MonitorizacionCpuRam();
                    break;
                case TipoMonitorizacion.MonitorizacionDiscos:
                    monitorizacion = new MonitorizacionDiscos();
                    break;
                case TipoMonitorizacion.MonitorizacionConexiones:
                    monitorizacion = new MonitorizacionConexiones();
                    break;
            }

            // Añadimos a la lista de monitorizaciones
            ListaMonitorizaciones.Add(monitorizacion);
        }

        /// <summary>
        /// Ejecución de la monitorización del sistema
        /// </summary>
        public static void SiguienteMonitorizacion()
        {
            foreach (MonitorizacionSistemaBase monitorizacionSistema in ListaMonitorizaciones)
            {
                monitorizacionSistema.SiguienteMonitorizacion();
            }
        }

        /// <summary>
        /// Añade nuevas trazas al registro del sistema
        /// </summary>
        public static void Log()
        {
            List<string> listaLogs = new List<string>();
            foreach (MonitorizacionSistemaBase monitorizacionSistema in ListaMonitorizaciones)
            {
                 listaLogs.AddRange(monitorizacionSistema.Resumen());
            }

            foreach (string info in listaLogs)
            {
                LogsRuntime.Info(ModulosSistema.Monitorizacion, "MonitorizaciónSistema", info);
            }
        }
        #endregion
    }

    /// <summary>
    /// Clase padre para la monitorización del sistema
    /// </summary>
    public class MonitorizacionSistemaBase
    {
        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public MonitorizacionSistemaBase()
        {
            // Implementado en hijos
        }
        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Inicialización de la monitorización del sistema
        /// </summary>
        public virtual void Inicializar()
        {
            // Implementado en hijos
        }

        /// <summary>
        /// Ejecución de la monitorización del sistema
        /// </summary>
        public virtual void SiguienteMonitorizacion()
        {
            // Implementado en hijos
        }

        /// <summary>
        /// Resumen de la monitorización
        /// </summary>
        public virtual List<string> Resumen()
        {
            return new List<string>();
        }
        #endregion
    }

    /// <summary>
    /// Clase utilizada para monitorizar los procesos
    /// </summary>
    public class MonitorizacionProcesos: MonitorizacionSistemaBase
    {
        #region Estructuras internas
        /// <summary>
        /// Estructura que guarda información de cada proceso
        /// </summary>
        public struct InformacionProceso
        {
            #region Atributo(s)
            /// <summary>
            /// Id del proceso
            /// </summary>
            public int Id;
            /// <summary>
            /// Nombre del proceso
            /// </summary>
            public string ProcessName;
            /// <summary>
            /// Ejecutable del proceso
            /// </summary>
            public string FileName;
            /// <summary>
            /// Memoria virtual del proceso
            /// </summary>
            public long VirtualMemorySize64;
            /// <summary>
            /// Memoria privada del proceso
            /// </summary>
            public long PrivateMemorySize64;
            /// <summary>
            /// Fecha de inicio del proceso
            /// </summary>
            public DateTime StartTime;
            /// <summary>
            /// Tiempo total del proceso
            /// </summary>
            public TimeSpan TotalProcessorTime;
            /// <summary>
            /// Momento de la monitorización
            /// </summary>
            public DateTime MomentoConsulta;
            /// <summary>
            /// % de uso de CPU del proceso durante el tiempo de monitorización
            /// </summary>
            public double CPUUsage; 
            #endregion

            #region Constructor
            /// <summary>
            /// Constructor de la estructura
            /// </summary>
            public InformacionProceso(Process proceso)
            {
                this.Id = -1;
                this.ProcessName = string.Empty;
                this.FileName = string.Empty;
                this.VirtualMemorySize64 = 0;
                this.PrivateMemorySize64 = 0;
                this.StartTime = DateTime.Now;
                this.TotalProcessorTime = TimeSpan.FromMilliseconds(0);
                this.MomentoConsulta = DateTime.Now;
                this.CPUUsage = 0;

                try
                {
                    this.Id = proceso.Id;
                    this.ProcessName = proceso.ProcessName;
                    this.FileName = proceso.StartInfo.FileName;
                    this.VirtualMemorySize64 = proceso.VirtualMemorySize64;
                    this.PrivateMemorySize64 = proceso.PrivateMemorySize64;
                    this.StartTime = proceso.StartTime;
                    this.TotalProcessorTime = proceso.TotalProcessorTime;
                }
                catch { }
            }
            #endregion

            #region Método(s) público(s)
            /// <summary>
            /// Actualización del proceso
            /// </summary>
            public void ActualizarProceso(InformacionProceso informacionProcesoAntigua)
            {
                TimeSpan intervalo = this.MomentoConsulta - informacionProcesoAntigua.MomentoConsulta;
                TimeSpan usoCPUBruto = this.TotalProcessorTime - informacionProcesoAntigua.TotalProcessorTime;
                this.CPUUsage = (usoCPUBruto.TotalMilliseconds / intervalo.TotalMilliseconds) * 100;
            }
            #endregion
        } 
        #endregion

        #region Atributo(s)
        /// <summary>
        /// Lista de procesos (clave = IdProceso, valor = InformacionProceso)
        /// </summary>
        public Dictionary<int, InformacionProceso> ListaProcesos;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public MonitorizacionProcesos()
        {
            this.ListaProcesos = new Dictionary<int, InformacionProceso>();
        }
        #endregion

        #region Método(s) privado(s)
        private Dictionary<int, InformacionProceso> ObtenerListaProcesos()
        {
            Dictionary<int, InformacionProceso> resultado = new Dictionary<int,InformacionProceso>();

            // Guardo los procesos en un diccionario
            Process[] arrayProccess = Process.GetProcesses();
            foreach (Process pProccess in arrayProccess)
            {
                InformacionProceso infoProceso = new InformacionProceso(pProccess);
                resultado.Add(pProccess.Id, infoProceso);
            }

            return resultado;
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Inicialización de la monitorización del sistema
        /// </summary>
        public override void Inicializar()
        {
            this.ListaProcesos = this.ObtenerListaProcesos();
        }

        /// <summary>
        /// Ejecución de la monitorización del sistema
        /// </summary>
        public override void SiguienteMonitorizacion()
        {
            base.SiguienteMonitorizacion();

            Dictionary<int, InformacionProceso> listaProcesosNueva = this.ObtenerListaProcesos();

            foreach (InformacionProceso infoProcesoAntigua in this.ListaProcesos.Values)
            {
                InformacionProceso infoProcesoNueva;
                if (listaProcesosNueva.TryGetValue(infoProcesoAntigua.Id, out infoProcesoNueva))
                {
                    infoProcesoNueva.ActualizarProceso(infoProcesoAntigua);
                }
            }

            this.ListaProcesos = listaProcesosNueva;
        }

        /// <summary>
        /// Resumen de la monitorización
        /// </summary>
        public override List<string> Resumen()
        {
            List<string> resultado = base.Resumen();

            foreach (InformacionProceso infoProceso in this.ListaProcesos.Values)
            {
                string info = string.Format("Proceso {0}, Fichero {1}, Memoria virtual {2}, Memoria privada {3}, Porcentaje procesador {4}", infoProceso.ProcessName, infoProceso.FileName, infoProceso.VirtualMemorySize64, infoProceso.PrivateMemorySize64, infoProceso.CPUUsage);
                resultado.Add(info);
            }

            return resultado;
        }
        #endregion
    }

    /// <summary>
    /// Clase utilizada para monitorizar la CPU y la RAM
    /// </summary>
    public class MonitorizacionCpuRam : MonitorizacionSistemaBase
    {
        #region Atributo(s)
        /// <summary>
        /// Contador de CPU
        /// </summary>
        private PerformanceCounter cpuCounter; 

        /// <summary>
        /// Contador de RAM libre
        /// </summary>
        private PerformanceCounter ramCounter; 

        /// <summary>
        /// Porcentaje total de CPU utilizada
        /// </summary>
        public double CPU;

        /// <summary>
        /// Cantidad total de memoria RAM libre en MB
        /// </summary>
        public double RAM;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public MonitorizacionCpuRam()
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Inicialización de la monitorización del sistema
        /// </summary>
        public override void Inicializar()
        {
            this.SiguienteMonitorizacion();
        }

        /// <summary>
        /// Ejecución de la monitorización del sistema
        /// </summary>
        public override void SiguienteMonitorizacion()
        {
            this.CPU = cpuCounter.NextValue();
            this.RAM = ramCounter.NextValue();
        }

        /// <summary>
        /// Resumen de la monitorización
        /// </summary>
        public override List<string> Resumen()
        {
            List<string> resultado = base.Resumen();

            string info = String.Format("RAM: {0} MB libres, CPU: {1} %", this.RAM, this.CPU);
            resultado.Add(info);

            return resultado;
        }
        #endregion
    }

    /// <summary>
    /// Clase utilizada para monitorizar los discos duros
    /// </summary>
    public class MonitorizacionDiscos: MonitorizacionSistemaBase
    {
        #region Estructuras internas
        /// <summary>
        /// Estructura que almacena la información de espacio libre de cada disco
        /// </summary>
        public struct InfoDisco
        {
            #region Atributo(s)
            /// <summary>
            /// Etiqueta del volumen
            /// </summary>
            public String VolumeLabel;
            /// <summary>
            /// Espacio libre
            /// </summary>
            public long AvailableFreeSpace;
            #endregion

            #region Constructor
            /// <summary>
            /// Constructor de la estructura
            /// </summary>
            public InfoDisco(DriveInfo driveInfo)
            {
                this.VolumeLabel = string.Empty;
                this.AvailableFreeSpace = 0;
                try
                {
                    if (driveInfo.IsReady)
                    {
                        this.VolumeLabel = driveInfo.VolumeLabel;
                        this.AvailableFreeSpace = driveInfo.AvailableFreeSpace;
                    }
                }
                catch { }
            }
            #endregion
        }
        #endregion

        #region Atributo(s)
        /// <summary>
        /// Lista de la información de los discos
        /// </summary>
        public Dictionary<string, InfoDisco> ListaInfoDisco;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public MonitorizacionDiscos()
        {
            this.ListaInfoDisco = new Dictionary<string, InfoDisco>();
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Inicialización de la monitorización del sistema
        /// </summary>
        public override void Inicializar()
        {
            this.SiguienteMonitorizacion();
        }

        /// <summary>
        /// Ejecución de la monitorización del sistema
        /// </summary>
        public override void SiguienteMonitorizacion()
        {
            try
            {
                DriveInfo[] DI = DriveInfo.GetDrives();
                foreach (DriveInfo drive in DI)
                {
                    if (drive.IsReady)
                    {
                        InfoDisco infoDisco = new InfoDisco(drive);
                        this.ListaInfoDisco.Add(drive.VolumeLabel, infoDisco);
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Resumen de la monitorización
        /// </summary>
        public override List<string> Resumen()
        {
            List<string> resultado = base.Resumen();

            foreach (InfoDisco infoDisco in this.ListaInfoDisco.Values)
            {
                string info = string.Format("Unidad {0}, Espacio libre {1}", infoDisco.VolumeLabel, infoDisco.AvailableFreeSpace);
                resultado.Add(info);
            }

            return resultado;
        }
        #endregion
    }

    /// <summary>
    /// Clase utilizada para monitorizar las conexiones TCP/IP
    /// </summary>
    public class MonitorizacionConexiones : MonitorizacionSistemaBase
    {
        #region Atributo(s)
        /// <summary>
        /// Lista de la información de los discos
        /// </summary>
        public List<string> ListaInfoConexiones;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public MonitorizacionConexiones()
        {
            this.ListaInfoConexiones = new List<string>();
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Inicialización de la monitorización del sistema
        /// </summary>
        public override void Inicializar()
        {
            this.SiguienteMonitorizacion();
        }

        /// <summary>
        /// Ejecución de la monitorización del sistema
        /// </summary>
        public override void SiguienteMonitorizacion()
        {
            try
            {
                string[] infoConexiones = ConnectionsInfo.Connections();
                this.ListaInfoConexiones.Clear();
                this.ListaInfoConexiones.AddRange(infoConexiones);
            }
            catch { }
        }

        /// <summary>
        /// Resumen de la monitorización
        /// </summary>
        public override List<string> Resumen()
        {
            return this.ListaInfoConexiones;
        }
        #endregion
    }

    /// <summary>
    /// Monitorización de los objetos en memoria
    /// </summary>
    public class MonitorizacionObjetosMemoria : MonitorizacionSistemaBase
    {
        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public MonitorizacionObjetosMemoria()
        {
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Inicialización de la monitorización del sistema
        /// </summary>
        public override void Inicializar()
        {
            this.SiguienteMonitorizacion();
        }

        /// <summary>
        /// Ejecución de la monitorización del sistema
        /// </summary>
        public override void SiguienteMonitorizacion()
        {
        }

        /// <summary>
        /// Resumen de la monitorización
        /// </summary>
        public override List<string> Resumen()
        {
            List<string> resultado = new List<string>();
            resultado.Add(string.Format("Número de objetos residentes en memoria: {0}", LargeObjectsRuntime.Count));
            resultado.AddRange(LargeObjectsRuntime.Resumen("Objeto residente en memoria. Código Hash: {0}, Tipo: {1}, Código: {2}"));
            return resultado;
        }
        #endregion
    }

    /// <summary>
    /// Enumerado que describe el tiepo de monitorización
    /// </summary>
    public enum TipoMonitorizacion
    {
        /// <summary>
        /// Monitorización de procesos
        /// </summary>
        MonitorizacionProcesos = 1,
        /// <summary>
        /// Monitorización de CPU y RAM
        /// </summary>
        MonitorizacionCpuRam = 2,
        /// <summary>
        /// Monitorización del espacio libre de discos
        /// </summary>
        MonitorizacionDiscos = 4,
        /// <summary>
        /// Monitorización de conexiones TCP/IP
        /// </summary>
        MonitorizacionConexiones = 5,
        /// <summary>
        /// Monitorización de los objetos en memoria
        /// </summary>
        MonitorizacionObjetosMemoria = 6
    }

    /// <summary>
    /// This is a class for disconnecting TCP connections.
    /// You can get a list of all connections and close by a connection, localIP, 
    /// remoteIP, localPort and remotePort.
    /// </summary>
    internal class ConnectionsInfo
    {
        #region Estructuras privadas
        /// <summary>
        /// Connection info 
        /// </summary>
        private struct MIB_TCPROW
        {
            public int dwState;
            public int dwLocalAddr;
            public int dwLocalPort;
            public int dwRemoteAddr;
            public int dwRemotePort;
        }
        #endregion

        #region Enumeraciones públicas
        /// <summary>
        /// Enumeration of the states 
        /// </summary>
        public enum State
        {
            All = 0,
            Closed = 1,
            Listen = 2,
            Syn_Sent = 3,
            Syn_Rcvd = 4,
            Established = 5,
            Fin_Wait1 = 6,
            Fin_Wait2 = 7,
            Close_Wait = 8,
            Closing = 9,
            Last_Ack = 10,
            Time_Wait = 11,
            Delete_TCB = 12
        } 
        #endregion

        #region Método(s) externos estáticos privados
        /// <summary>
        /// API to get list of connections 
        /// </summary>
        /// <param name="pTcpTable"></param>
        /// <param name="pdwSize"></param>
        /// <param name="bOrder"></param>
        /// <returns></returns>
        [DllImport("iphlpapi.dll")]
        private static extern int GetTcpTable(IntPtr pTcpTable, ref int pdwSize, bool bOrder);

        /// <summary>
        /// API to change status of connection 
        /// </summary>
        /// <param name="pTcprow"></param>
        /// <returns></returns>
        [DllImport("iphlpapi.dll")]
        //private static extern int SetTcpEntry(MIB_TCPROW tcprow);
        private static extern int SetTcpEntry(IntPtr pTcprow);

        /// <summary>
        /// Convert 16-bit value from network to host byte order 
        /// </summary>
        /// <param name="netshort"></param>
        /// <returns></returns>
        [DllImport("wsock32.dll")]
        private static extern int ntohs(int netshort);

        /// <summary>
        /// Convert 16-bit value back again 
        /// </summary>
        /// <param name="netshort"></param>
        /// <returns></returns>
        [DllImport("wsock32.dll")]
        private static extern int htons(int netshort); 
        #endregion

        #region Método(s) estático(s) públicos
        /// <summary>
        /// Close all connection to the remote IP 
        /// </summary>
        /// <param name="IP"></param>
        public static void CloseRemoteIP(string IP)
        {
            MIB_TCPROW[] rows = getTcpTable();
            for (int i = 0;
             i < rows.Length;
             i++)
            {
                if (rows[i].dwRemoteAddr == IPStringToInt(IP))
                {
                    rows[i].dwState = (int)State.Delete_TCB;
                    IntPtr ptr = App.GetPtrToNewObject(rows[i]);
                    int ret = SetTcpEntry(ptr);
                }
            }
        }

        /// <summary>
        /// Close all connections at current local IP 
        /// </summary>
        /// <param name="IP"></param>
        public static void CloseLocalIP(string IP)
        {
            MIB_TCPROW[] rows = getTcpTable();
            for (int i = 0;
             i < rows.Length;
             i++)
            {
                if (rows[i].dwLocalAddr == IPStringToInt(IP))
                {
                    rows[i].dwState = (int)State.Delete_TCB;
                    IntPtr ptr = App.GetPtrToNewObject(rows[i]);
                    int ret = SetTcpEntry(ptr);
                }
            }
        }

        /// <summary>
        /// Closes all connections to the remote port 
        /// </summary>
        /// <param name="port"></param>
        public static void CloseRemotePort(int port)
        {
            MIB_TCPROW[] rows = getTcpTable();
            for (int i = 0;
             i < rows.Length;
             i++)
            {
                if (port == ntohs(rows[i].dwRemotePort))
                {
                    rows[i].dwState = (int)State.Delete_TCB;
                    IntPtr ptr = App.GetPtrToNewObject(rows[i]);
                    int ret = SetTcpEntry(ptr);
                }
            }
        }

        /// <summary>
        /// Closes all connections to the local port 
        /// </summary>
        /// <param name="port"></param>
        public static void CloseLocalPort(int port)
        {
            MIB_TCPROW[] rows = getTcpTable();
            for (int i = 0;
             i < rows.Length;
             i++)
            {
                if (port == ntohs(rows[i].dwLocalPort))
                {
                    rows[i].dwState = (int)State.Delete_TCB;
                    IntPtr ptr = App.GetPtrToNewObject(rows[i]);
                    int ret = SetTcpEntry(ptr);
                }
            }
        }

        /// <summary>
        /// Close a connection by returning the connectionstring 
        /// </summary>
        /// <param name="connectionstring"></param>
        public static void CloseConnection(string connectionstring)
        {
            try
            {
                //Split the string to its subparts 
                string[] parts = connectionstring.Split('-');
                if (parts.Length != 4)
                    throw new Exception("Invalid connectionstring - use the one provided by Connections.");
                string[] loc = parts[0].Split(':');
                string[] rem = parts[1].Split(':');
                string[] locaddr = loc[0].Split('.');
                string[] remaddr = rem[0].Split('.');
                //Fill structure with data 
                MIB_TCPROW row = new MIB_TCPROW();
                row.dwState = 12;
                byte[] bLocAddr = new byte[] { byte.Parse(locaddr[0]), byte.Parse(locaddr[1]), byte.Parse(locaddr[2]), byte.Parse(locaddr[3]) };
                byte[] bRemAddr = new byte[] { byte.Parse(remaddr[0]), byte.Parse(remaddr[1]), byte.Parse(remaddr[2]), byte.Parse(remaddr[3]) };
                row.dwLocalAddr = BitConverter.ToInt32(bLocAddr, 0);
                row.dwRemoteAddr = BitConverter.ToInt32(bRemAddr, 0);
                row.dwLocalPort = htons(int.Parse(loc[1]));
                row.dwRemotePort = htons(int.Parse(rem[1]));
                //Make copy of the structure into memory and use the pointer to call SetTcpEntry 
                IntPtr ptr = App.GetPtrToNewObject(row);
                int ret = SetTcpEntry(ptr);
                if (ret == -1)
                    throw new Exception("Unsuccessful");
                if (ret == 65)
                    throw new Exception("User has no sufficient privilege to execute this API successfully");
                if (ret == 87)
                    throw new Exception("Specified port is not in state to be closed down");
                if (ret != 0)
                    throw new Exception("Unknown error (" + ret + ")");
            }
            catch (Exception ex)
            {
                throw new Exception("CloseConnection failed (" + connectionstring + ")! [" + ex.GetType().ToString() + "," + ex.Message + "]");
            }
        }

        /// <summary>
        /// Gets all connections 
        /// </summary>
        /// <returns></returns>
        public static string[] Connections()
        {
            return Connections(State.All);
        }

        /// <summary>
        /// Gets a connection list of connections with a defined state 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string[] Connections(State state)
        {
            MIB_TCPROW[] rows = getTcpTable();
            ArrayList arr = new ArrayList();
            foreach (MIB_TCPROW row in rows)
            {
                if (state == State.All || state == (State)row.dwState)
                {
                    string localaddress = IPIntToString(row.dwLocalAddr) + ":" + ntohs(row.dwLocalPort);
                    string remoteaddress = IPIntToString(row.dwRemoteAddr) + ":" + ntohs(row.dwRemotePort);
                    arr.Add(localaddress + "-" + remoteaddress + "-" + ((State)row.dwState).ToString() + "-" + row.dwState);
                }
            }
            return (string[])arr.ToArray(typeof(System.String));
        }
        #endregion

        #region Método(s) estático(s) privados
        /// <summary>
        /// The function that fills the MIB_TCPROW array with connectioninfos
        /// </summary>
        /// <returns></returns>
        private static MIB_TCPROW[] getTcpTable()
        {
            IntPtr buffer = IntPtr.Zero;
            bool allocated = false;
            try
            {
                int iBytes = 0;
                GetTcpTable(IntPtr.Zero, ref iBytes, false);
                //Getting size of return data 
                buffer = Marshal.AllocCoTaskMem(iBytes);
                //allocating the datasize 
                allocated = true;
                GetTcpTable(buffer, ref iBytes, false);
                //Run it again to fill the memory with the data 
                int structCount = Marshal.ReadInt32(buffer);
                // Get the number of structures 
                IntPtr buffSubPointer = buffer;
                //Making a pointer that will point into the buffer 
                buffSubPointer = (IntPtr)((int)buffer + 4);
                //Move to the first data (ignoring dwNumEntries from the original MIB_TCPTABLE struct) 
                MIB_TCPROW[] tcpRows = new MIB_TCPROW[structCount];
                //Declaring the array 
                //Get the struct size 
                MIB_TCPROW tmp = new MIB_TCPROW();
                int sizeOfTCPROW = Marshal.SizeOf(tmp);
                //Fill the array 1 by 1 
                for (int i = 0;
                 i < structCount;
                 i++)
                {
                    tcpRows[i] = (MIB_TCPROW)Marshal.PtrToStructure(buffSubPointer, typeof(MIB_TCPROW));
                    //copy struct data 
                    buffSubPointer = (IntPtr)((int)buffSubPointer + sizeOfTCPROW);
                    //move to next structdata 
                }
                return tcpRows;
            }
            catch (Exception ex)
            {
                throw new Exception("getTcpTable failed! [" + ex.GetType().ToString() + "," + ex.Message + "]");
            }
            finally
            {
                if (allocated)
                    Marshal.FreeCoTaskMem(buffer);
                //Free the allocated memory 
            }
        }

        /// <summary>
        /// Convert an IP string to the INT value 
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        private static int IPStringToInt(string IP)
        {
            if (IP.IndexOf(".") < 0)
                throw new Exception("Invalid IP address");
            string[] addr = IP.Split('.');
            if (addr.Length != 4)
                throw new Exception("Invalid IP address");
            byte[] bytes = new byte[] { byte.Parse(addr[0]), byte.Parse(addr[1]), byte.Parse(addr[2]), byte.Parse(addr[3]) };
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Convert an IP integer to IP string 
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        private static string IPIntToString(int IP)
        {
            byte[] addr = System.BitConverter.GetBytes(IP);
            return addr[0] + "." + addr[1] + "." + addr[2] + "." + addr[3];
        } 
        #endregion    
    }
}
