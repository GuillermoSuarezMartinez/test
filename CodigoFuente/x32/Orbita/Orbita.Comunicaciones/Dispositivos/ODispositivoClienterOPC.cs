//***********************************************************************
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using OpcRcw.Comn;
using OpcRcw.Da;
using Orbita.Trazabilidad;
using Orbita.Utiles;
namespace Orbita.Comunicaciones
{
    /// <summary>
    /// OLE for Process Control de Siemens.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "El origen del nombre abreviado.")]
    public class ODispositivoClienteOPC : ODispositivo, IOPCDataCallback
    {
        #region Constantes
        /// <summary>
        /// Máximo número de reintentos.
        /// </summary>
        internal const int MAXREINTENTOSERRORCOMM = 3;
        /// <summary>
        /// Local para inglés.
        /// </summary>
        internal const int LOCALE_ID = 0x407;
        #endregion

        #region Atributos privados estáticos
        /// <summary>
        /// Colección de hilos.
        /// </summary>
        static OHilos Hilos;
        #endregion

        #region Atributos privados
        /// <summary>
        /// Atributo de sincronización en las
        /// lecturas y escrituras asíncronas.
        /// </summary>
        OResetManual _sincronizacion;
        /// <summary>
        /// Atributo que indica las  colecciones
        /// de tags de datos, lecturas y alarmas.
        /// </summary>
        OTags _tags;
        /// <summary>
        /// Enlaces
        /// </summary>
        Hashtable oEnlaces;

        #region OPC
        /// <summary>
        /// OPC.
        /// </summary>
        IOPCServer pIOPCServer;
        int pRevUpdateRate;
        /// <summary>
        /// Puntero para IO asincrono de datos.
        /// </summary>
        IOPCAsyncIO2 pIOPCAsyncIO2Datos = null;
        IConnectionPointContainer pIConnectionPointContainerDatos = null;
        IConnectionPoint pIConnectionPointDatos = null;
        Object pobjGroupDatos = null;
        int pSvrGroupHandleDatos = 0;
        int hClientGroupDatos = 1;
        /// <summary>
        /// Puntero para IO asincrono de lecturas.
        /// </summary>      
        //IOPCAsyncIO2 pIOPCAsyncIO2Lecturas = null;
        //IOPCGroupStateMgt pIOPCGroupStateMgtLecturas = null;
        IConnectionPointContainer pIConnectionPointContainerLecturas = null;
        IConnectionPoint pIConnectionPointLecturas = null;
        Object pobjGroupLecturas = null;
        int pSvrGroupHandleLecturas = 0;
        int hClientGroupLecturas = 2;
        /// <summary>
        /// Puntero para IO asincrono de alarmas.
        /// </summary>            
        //IOPCAsyncIO2 pIOPCAsyncIO2Alarmas = null;
        //IOPCGroupStateMgt pIOPCGroupStateMgtAlarmas = null;
        IConnectionPointContainer pIConnectionPointContainerAlarmas = null;
        IConnectionPoint pIConnectionPointAlarmas = null;
        Object pobjGroupAlarmas = null;
        int pSvrGroupHandleAlarmas = 0;
        int hClientGroupAlarmas = 3;
        int[] itemSvrHandleArray;
        int dwCookie = 0;
        //int nTransactionID = 0;
        Type svrComponenttyp;
        // Propiedad de grupos.
        int dwRequestedUpdateRate = 50;
        float deadband = 0;
        int TimeBias = 0;
        GCHandle hTimeBias, hDeadband;
        //Dispositivo _dispositivo;
        OEventArgs _oEventargs;
        OEstadoComms[] _oOPCComms;
        OConfigDispositivo _config;
        #endregion
        
        #endregion

        #region Constructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase SiemensOPC.
        /// </summary>
        public ODispositivoClienteOPC(OTags tags, OHilos hilos, ODispositivo dispositivo)
        {
            this.Identificador = dispositivo.Identificador;
            this.Nombre = dispositivo.Nombre;
            this.Tipo = dispositivo.Tipo;
            this.Direccion = dispositivo.Direccion;
            this.Local = dispositivo.Local;

            LogManager.GetLogger("wrapper").Info("Creando OPC");
            //Inicio datos de dispositivo
            this.Identificador = dispositivo.Identificador;
            this.Nombre = dispositivo.Nombre;
            this.Tipo = dispositivo.Tipo;
            this.Direccion = dispositivo.Direccion;
            this.Local = dispositivo.Local;
            this._config = tags.Config;

            // Asignación de las colecciones de datos, lecturas y alarmas.
            this._tags = tags;

            //Inicio eventargs
            OInfoOPCvida info = this._tags.HtVida;

            this._oEventargs = new OEventArgs();
            this._oOPCComms = new OEstadoComms[info.Enlaces.Length];
            this.oEnlaces = new Hashtable();

            for (int i = 0; i < info.Enlaces.Length; i++)
            {
                this._oOPCComms[i] = new OEstadoComms();
                this._oOPCComms[i].Enlace = info.Enlaces[i];
                this._oOPCComms[i].Nombre = this.Nombre;
                this._oOPCComms[i].Id = this.Identificador;
                OOPCEnlaces enlace = new OOPCEnlaces(this._oOPCComms[i].Enlace);
                this.oEnlaces.Add(enlace.Nombre, enlace);
            }
            // Atributos de sincronización de lecturas y escrituras.
            this._sincronizacion = new OResetManual(3);

            // Asignación de la colección estática de hilos.
            Hilos = hilos;
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Colección de datos.
        /// </summary>
        private OHashtable Datos
        {
            get { return this._tags.GetDatos(); }
        }
        /// <summary>
        /// Colección de lecturas.
        /// </summary>
        private OHashtable Lecturas
        {
            get { return this._tags.GetLecturas(); }
        }
        /// <summary>
        /// Colección de alarmas.
        /// </summary>
        private OHashtable Alarmas
        {
            get { return this._tags.GetAlarmas(); }
        }
        /// <summary>
        /// Colección de alarmas activas.
        /// </summary>
        private ArrayList AlarmasActivas
        {
            get { return this._tags.GetAlarmasActivas(); }
        }
        #endregion

        #region Métodos públicos
        /// <summary>
        /// Inicializar grupos, punteros e items.
        /// </summary>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public override void Iniciar()
        {
            // Inicializar grupo de datos.
            InicGrupoDatosOPC();
            LogManager.GetLogger("wrapper").Info("Grupo de datos iniciado");
            // Inicializar grupo de lecturas.
            InicGrupoLecturasOPC();
            LogManager.GetLogger("wrapper").Info("Grupo de lecturas iniciado");
            // Inicializar grupo de alarmas.
            InicGrupoAlarmasOPC();
            LogManager.GetLogger("wrapper").Info("Grupo de alarmas iniciado");

            // Inicializar punteros de datos.
            InicReqIOInterfacesDatos();
            LogManager.GetLogger("wrapper").Info("Puntero de datos iniciado");
            // Inicializar punteros de lecturas.
            InicReqIOInterfacesLecturas();
            LogManager.GetLogger("wrapper").Info("Puntero de lecturas iniciado");
            // Inicializar punteros de alarmas.
            InicReqIOInterfacesAlarmas();
            LogManager.GetLogger("wrapper").Info("Puntero de alarmas iniciado");

            this.SetItems();
            this.InicHiloVida();
            this.InicTareasEstado();
            this.IniciaStringsLocales();
        }              
        /// <summary>
        /// Leer el valor de las descripciones de variables de la colección bajo demanda.
        /// </summary>
        /// <param name="variables">Colección de variables.</param>
        /// <param name="demanda">Indica que la demanda de datos leidos es directa.</param>
        /// <returns>Colección de resultados.</returns>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public override object[] Leer(string[] variables, bool demanda)
        {
            if (!demanda)
            {
                return this.Leer(variables);
            }
            string[] resultado = null;
            if (variables != null)
            {
                try
                {
                    // Inicializar contador de variables.
                    int contador = variables.Length;

                    // Inicializar colección de identificadores de lectura.
                    int[] items = new int[contador];
                    int[] identificadores = new int[contador];

                    for (int i = 0; i < contador; i++)
                    {
                        int identificador = this._tags.GetDB(variables[i]).Identificador;
                        items[i] = this._tags.GetDatos(identificador).IdLectura;
                        identificadores[i] = this._tags.GetDatos(identificador).Identificador;
                    }

                    // Leer asincronamente el valor del grupo
                    // de identificadores del PLC.
                    this.Read(items, 1);

                    // Inicializar resultado de valores asociados 
                    // a la posición del identificador de lectura.
                    resultado = new string[contador];

                    // Letargo del método hasta t-tiempo, o bien, hasta
                    // el término de  la  lectura   asíncrona  de datos.
                    if (this._sincronizacion.Dormir(1, TimeSpan.FromSeconds(this._config.TiempoEsperaLectura)))
                    {
                        // Actualización de la colección de resultados.
                        for (int i = 0; i < contador; i++)
                        {
                            //CAMBIADO REVISAR
                            resultado[i] = this._tags.GetDatos(identificadores[i]).Valor.ToString();
                        }
                    }
                    else
                    {
                        this.OnComm(null);
                    }
                    // Resetear el evento.
                    this._sincronizacion.Resetear(1);
                }
                catch (Exception ex)
                {
                    this.ActualizarCalidadVariables();
                    LogManager.GetLogger("wrapper").Error("Leer string: " + ex.ToString());
                }
            }
            return resultado;
        }
        /// <summary>
        /// Escribir el valor de los identificadores de variables de la colección.
        /// </summary>
        /// <param name="variables">Colección de variables.</param>
        /// <param name="valores">Colección de valores.</param>
        /// <returns></returns>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public override bool Escribir(string[] variables, object[] valores)
        {
            bool resultado = true;
            if (variables != null)
            {
                try
                {
                    // Inicializar contador de variables.
                    int contador = variables.Length;

                    // Inicializar colecciones de identificadores.
                    int[] items = new int[contador];
                    int[] datos = new int[contador];

                    for (int i = 0; i < contador; i++)
                    {
                        OInfoDato infoDBdato = this._tags.GetDB(variables[i]);
                        items[i] = infoDBdato.IdLectura;
                        datos[i] = infoDBdato.Identificador;
                        infoDBdato.Error = 1;
                    }

                    // Escribir el grupo de valores en los items.
                    this.Write(items, valores);

                    // Letargo del método hasta t-tiempo, o bien, hasta
                    // el término de  la  lectura   asíncrona  de datos.
                    if (this._sincronizacion.Dormir(2, TimeSpan.FromSeconds(this._config.TiempoEsperaEscritura)))
                    {
                        for (int i = 0; i < contador; i++)
                        {
                            if (this._tags.GetDatos(datos[i]).Error != 0)
                            {
                                resultado = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        this.OnComm(null);
                    }
                    // Resetear el evento.
                    this._sincronizacion.Resetear(2);
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger("wrapper").Error("Escribir: " + ex.ToString());
                }
            }
            return resultado;
        }        
        /// <summary>
        /// Devuelva las alarmas alctivas del sistemas
        /// </summary>
        /// <returns></returns>
        public override ArrayList GetAlarmasActivas()
        {
            this.ActualizarAlarmas();
            return this._tags.GetAlarmasActivas();
        }
        /// <summary>
        /// Devuelve los datos del dipositivo y su valor
        /// </summary>
        /// <returns></returns>
        public override OHashtable GetDatos()
        {
            return this._tags.GetDatos();
        }
        /// <summary>
        /// Devuelve las lectuas del dipositivo y su valor
        /// </summary>
        /// <returns></returns>
        public override OHashtable GetLecturas()
        {
            return this._tags.GetLecturas();
        }
        /// <summary>
        /// Devuelve las alarmas del dipositivo y su valor
        /// </summary>
        /// <returns></returns>
        public override OHashtable GetAlarmas()
        {
            return this._tags.GetAlarmas();
        }
        /// <summary>
        /// Elimina los elementos de memoria
        /// </summary>
        /// <param name="disposing"></param>
        public override void Dispose(bool disposing)
        {
            this._tags.Dispose();
            Hilos.Destruir();
        }
        #endregion

        #region Métodos privados
        // Obtener el tipo de ProgID.
        Guid iidRequiredInterface;
        /// <summary>
        /// Inicialización de grupo de datos OPC.
        /// </summary>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        private void InicGrupoDatosOPC()
        {
            try
            {
                this.hTimeBias = GCHandle.Alloc(TimeBias, GCHandleType.Pinned);
                this.hDeadband = GCHandle.Alloc(deadband, GCHandleType.Pinned);

                // Obtener el tipo de ProgID y crear la instancia del componente COM de servidor OPC.
                iidRequiredInterface = typeof(IOPCItemMgt).GUID;

                if (this.Local)
                {
                    // Conectar al servidor local.
                    svrComponenttyp = Type.GetTypeFromProgID(this.Tipo);
                }
                else
                {
                    // Conectar al servidor remoto.
                    svrComponenttyp = Type.GetTypeFromProgID(this.Tipo, this.Direccion.ToString());
                }

                pIOPCServer = (IOPCServer)Activator.CreateInstance(svrComponenttyp);

                #region Comentarios

                //primero agregamos los datos
                /* 2. Add a new group
                        Add a group object and querry for interface IOPCItemMgt
                        Parameter as following:
                        [in] not active, so no OnDataChange callback
                        [in] Request this Update Rate from Server
                        [in] Client Handle, not necessary in this sample
                        [in] No time interval to system UTC time
                        [in] No Deadband, so all data changes are reported
                        [in] Server uses english language to for text values
                        [out] Server handle to identify this group in later calls
                        [out] The answer from Server to the requested Update Rate
                        [in] requested interface type of the group object
                        [out] pointer to the requested interface
                    */

                #endregion

                pIOPCServer.AddGroup(OComunicacionesConstantes.OInfoOPC.Dato, 0, dwRequestedUpdateRate, hClientGroupDatos,
                    hTimeBias.AddrOfPinnedObject(), hDeadband.AddrOfPinnedObject(), LOCALE_ID, out pSvrGroupHandleDatos,
                    out pRevUpdateRate, ref iidRequiredInterface, out pobjGroupDatos);
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("wrapper").Error("InicGrupoDatosOPC " + ex.ToString());
            }
        }
        /// <summary>
        /// Inicialización de grupo de lecturas OPC.
        /// </summary>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        private void InicGrupoLecturasOPC()
        {
            pIOPCServer.AddGroup(OComunicacionesConstantes.OInfoOPC.Lectura, 1, dwRequestedUpdateRate, hClientGroupLecturas,
                hTimeBias.AddrOfPinnedObject(), hDeadband.AddrOfPinnedObject(), LOCALE_ID, out pSvrGroupHandleLecturas,
                out pRevUpdateRate, ref iidRequiredInterface, out pobjGroupLecturas);

        }
        /// <summary>
        /// Inicialización de grupo de alarmas OPC.
        /// </summary>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        private void InicGrupoAlarmasOPC()
        {
            pIOPCServer.AddGroup(OComunicacionesConstantes.OInfoOPC.Alarma, 1, dwRequestedUpdateRate, hClientGroupAlarmas,
                hTimeBias.AddrOfPinnedObject(), hDeadband.AddrOfPinnedObject(), LOCALE_ID, out pSvrGroupHandleAlarmas,
                out pRevUpdateRate, ref iidRequiredInterface, out pobjGroupAlarmas);
        }
        /// <summary>
        /// Inicialización del puntero de datos.
        /// </summary>
        private void InicReqIOInterfacesDatos()
        {
            // Interfaz de consulta para las llamadas asíncronas en los objetos del grupo.
            pIOPCAsyncIO2Datos = pobjGroupDatos as IOPCAsyncIO2;

            // Consulta Callback de interfaz para el objeto de grupo.
            pIConnectionPointContainerDatos = pobjGroupDatos as IConnectionPointContainer;

            // Establecer Callback para todas las operaciones asíncronas.
            Guid iid = typeof(IOPCDataCallback).GUID;
            pIConnectionPointContainerDatos.FindConnectionPoint(ref iid, out pIConnectionPointDatos);

            // Crear una conexión entre el punto de conexión de los servidores OPC y
            // sumidero de este cliente (el objeto Callback).
            pIConnectionPointDatos.Advise(this, out dwCookie);

        }
        /// <summary>
        /// Inicialización del puntero de lecturas.
        /// </summary>
        private void InicReqIOInterfacesLecturas()
        {
            // Interfaz de consulta para las llamadas asíncronas en los objetos del grupo.
            // pIOPCAsyncIO2Lecturas = (IOPCAsyncIO2)pobjGroupLecturas;
            // pIOPCGroupStateMgtLecturas = (IOPCGroupStateMgt)pobjGroupLecturas;

            // Consulta Callback de interfaz para el objeto de grupo.
            pIConnectionPointContainerLecturas = pobjGroupLecturas as IConnectionPointContainer;

            // Establecer Callback para todas las operaciones asíncronas.
            Guid iid = typeof(IOPCDataCallback).GUID;
            pIConnectionPointContainerLecturas.FindConnectionPoint(ref iid, out pIConnectionPointLecturas);

            // Crear una conexión entre el punto de conexión de los servidores OPC y
            // sumidero de este cliente (el objeto Callback).
            pIConnectionPointLecturas.Advise(this, out dwCookie);

        }
        /// <summary>
        /// Inicialización del puntero de alarmas.
        /// </summary>
        private void InicReqIOInterfacesAlarmas()
        {
            // Interfaz de consulta para las llamadas asíncronas en los objetos del grupo.
            // pIOPCAsyncIO2Alarmas = (IOPCAsyncIO2)pobjGroupAlarmas;
            // pIOPCGroupStateMgtAlarmas = (IOPCGroupStateMgt)pobjGroupAlarmas;

            // Consulta Callback de interfaz para el objeto de grupo.
            pIConnectionPointContainerAlarmas = pobjGroupAlarmas as IConnectionPointContainer;

            // Establecer Callback para todas las operaciones asíncronas.
            Guid iid = typeof(IOPCDataCallback).GUID;
            pIConnectionPointContainerAlarmas.FindConnectionPoint(ref iid, out pIConnectionPointAlarmas);

            // Crear una conexión entre el punto de conexión de los servidores OPC y
            // sumidero de este cliente (el objeto Callback).
            pIConnectionPointAlarmas.Advise(this, out dwCookie);
        }
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        private void SetItems()
        {
            // Crear items de datos.
            SetItemsDatos();
            LogManager.GetLogger("wrapper").Info("Items de datos iniciado");
            // Crear items de lecturas.
            SetItemsLecturas();
            LogManager.GetLogger("wrapper").Info("Items de lecturas iniciado");
            // Crear items de alarmas.
            SetItemsAlarmas();
            LogManager.GetLogger("wrapper").Info("Items de alarmas iniciado");
        }
        /// <summary>
        /// Asignar items de datos al grupo de datos OPC.
        /// </summary>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        private void SetItemsDatos()
        {
            int longitud = this.Datos.Contar();
            if (longitud > 0)
            {
                OPCITEMDEF[] itemDeffArrayDatos = new OPCITEMDEF[longitud];
                int j = 0;
                foreach (OInfoDato dato in this.Datos.Values)
                {
                    itemDeffArrayDatos[j].szItemID = dato.Conexion;	        // cadena de conexión.
                    itemDeffArrayDatos[j].bActive = 0;			            // item es activo.
                    itemDeffArrayDatos[j].hClient = dato.Identificador;	    // manejador cliente.
                    itemDeffArrayDatos[j].dwBlobSize = 0;                   // tamaño de blob.
                    itemDeffArrayDatos[j].pBlob = IntPtr.Zero;              // puntero a blob.
                    itemDeffArrayDatos[j].vtRequestedDataType = 8;	        // retorna el valor con el tipo nativo.
                    j++;
                }

                IntPtr pResultados = IntPtr.Zero;
                IntPtr pErr = IntPtr.Zero;
                try
                {
                    // Añadir items al grupo.
                    ((IOPCItemMgt)pobjGroupDatos).AddItems(longitud, itemDeffArrayDatos, out pResultados, out pErr);

                    // Deserializar para obtener el manejador del servidor, después chequea los errores.
                    int[] err = new int[longitud];
                    IntPtr posicion = pResultados;

                    this.itemSvrHandleArray = new int[longitud];
                    Marshal.Copy(pErr, err, 0, longitud);

                    for (int i = 0; i < longitud; i++)
                    {
                        if (err[i] == 0)
                        {
                            if (i != 0)
                            {
                                posicion = new IntPtr(posicion.ToInt32() + Marshal.SizeOf(typeof(OPCITEMRESULT)));
                            }
                            OPCITEMRESULT resultado = (OPCITEMRESULT)Marshal.PtrToStructure(posicion, typeof(OPCITEMRESULT));
                            this.itemSvrHandleArray[i] = resultado.hServer;

                            int item = itemDeffArrayDatos[i].hClient;

                            // Modificar el identificar de lectura  de la 
                            // colección de datos y de la colección de db.
                            // Datos.
                            OInfoDato InfoDato = this._tags.GetDatos(item);
                            InfoDato.IdLectura = resultado.hServer;
                            // Db.
                            this._tags.GetDB(InfoDato.Texto).IdLectura = resultado.hServer;
                        }
                        else
                        {
                            string strError;
                            pIOPCServer.GetErrorString(err[0], LOCALE_ID, out strError);
                        }
                    }
                    //this.OnSetItems(new OEventArgs(err));
                }
                finally
                {
                    // Liberar la memoria.
                    if (pResultados != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(pResultados);
                        pResultados = IntPtr.Zero;
                    }
                    if (pErr != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(pErr);
                        pErr = IntPtr.Zero;
                    }
                }
            }
        }
        /// <summary>
        /// Asignar items de lecturas al grupo de lecturas OPC.
        /// </summary>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        private void SetItemsLecturas()
        {
            int longitud = this.Lecturas.Contar();
            if (longitud > 0)
            {
                OPCITEMDEF[] itemDeffArrayDatos = new OPCITEMDEF[longitud];
                int j = 0;
                foreach (OInfoDato lectura in this.Lecturas.Values)
                {
                    itemDeffArrayDatos[j].szItemID = lectura.Conexion;      // cadena de conexión.
                    itemDeffArrayDatos[j].bActive = 1;			            // item es activo.
                    itemDeffArrayDatos[j].hClient = lectura.Identificador;  // manejador cliente.	
                    itemDeffArrayDatos[j].dwBlobSize = 0;                   // tamaño de blob.
                    itemDeffArrayDatos[j].pBlob = IntPtr.Zero;              // puntero a blob.
                    itemDeffArrayDatos[j].vtRequestedDataType = 8;	        // retorna el valor con el tipo nativo.
                    j++;
                }

                IntPtr pResultados = IntPtr.Zero;
                IntPtr pErr = IntPtr.Zero;
                try
                {
                    // Añadir item al grupo.
                    ((IOPCItemMgt)pobjGroupLecturas).AddItems(longitud, itemDeffArrayDatos, out pResultados, out pErr);

                    // Deserializar para obtener el manejador del servidor, después chequea los errores.
                    int[] err = new int[longitud];
                    IntPtr posicion = pResultados;
                    this.itemSvrHandleArray = new int[longitud];
                    Marshal.Copy(pErr, err, 0, longitud);
                    for (int i = 0; i < longitud; i++)
                    {
                        if (err[i] == 0)
                        {
                            if (i != 0)
                            {
                                posicion = new IntPtr(posicion.ToInt32() + Marshal.SizeOf(typeof(OPCITEMRESULT)));
                            }
                            OPCITEMRESULT resultado = (OPCITEMRESULT)Marshal.PtrToStructure(posicion, typeof(OPCITEMRESULT));
                            this.itemSvrHandleArray[i] = resultado.hServer;
                            int item = itemDeffArrayDatos[i].hClient;

                            // Lecturas.
                            this._tags.GetLecturas(item).IdLectura = resultado.hServer;
                        }
                        else
                        {
                            string strError;
                            pIOPCServer.GetErrorString(err[0], LOCALE_ID, out strError);
                        }
                    }
                    //this.OnSetItems(new OEventArgs(err));
                }
                finally
                {
                    // Liberar memoria.
                    if (pResultados != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(pResultados);
                        pResultados = IntPtr.Zero;
                    }
                    if (pErr != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(pErr);
                        pErr = IntPtr.Zero;
                    }
                }
            }
        }
        /// <summary>
        /// Asignar items de alarmas al grupo de alarmas OPC.
        /// </summary>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        private void SetItemsAlarmas()
        {
            int longitud = this.Alarmas.Contar();
            if (longitud > 0)
            {
                OPCITEMDEF[] itemDeffArrayAlarmas = new OPCITEMDEF[longitud];
                int j = 0;
                foreach (OInfoDato alarma in this.Alarmas.Values)
                {
                    itemDeffArrayAlarmas[j].szItemID = alarma.Conexion;       // cadena de conexión.
                    itemDeffArrayAlarmas[j].bActive = 1;			          // item es activo.
                    itemDeffArrayAlarmas[j].hClient = alarma.Identificador;	  // manejador cliente.
                    itemDeffArrayAlarmas[j].dwBlobSize = 0;                   // tamaño de blob.
                    itemDeffArrayAlarmas[j].pBlob = IntPtr.Zero;              // puntero a blob.
                    itemDeffArrayAlarmas[j].vtRequestedDataType = 8;	      // retorna el valor con el tipo nativo.
                    j++;
                }

                IntPtr pResultados = IntPtr.Zero;
                IntPtr pErr = IntPtr.Zero;
                try
                {
                    // Añadir item al grupo.
                    ((IOPCItemMgt)pobjGroupAlarmas).AddItems(longitud, itemDeffArrayAlarmas, out pResultados, out pErr);

                    // Deserializar para obtener el manejador del servidor, después chequea los errores.
                    int[] err = new int[longitud];
                    IntPtr posicion = pResultados;

                    this.itemSvrHandleArray = new int[longitud];
                    Marshal.Copy(pErr, err, 0, longitud);

                    for (int i = 0; i < longitud; i++)
                    {
                        if (err[i] == 0)
                        {
                            if (i != 0)
                            {
                                posicion = new IntPtr(posicion.ToInt32() + Marshal.SizeOf(typeof(OPCITEMRESULT)));
                            }
                            OPCITEMRESULT resultado = (OPCITEMRESULT)Marshal.PtrToStructure(posicion, typeof(OPCITEMRESULT));
                            this.itemSvrHandleArray[i] = resultado.hServer;
                            int item = itemDeffArrayAlarmas[i].hClient;

                            // Alarmas.
                            this._tags.GetAlarmas(item).IdLectura = resultado.hServer;
                        }
                        else
                        {
                            string strError;
                            pIOPCServer.GetErrorString(err[0], LOCALE_ID, out strError);
                        }
                    }
                    //this.OnSetItems(new OEventArgs(err));
                }
                finally
                {
                    // Liberar memoria.
                    if (pResultados != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(pResultados);
                        pResultados = IntPtr.Zero;
                    }
                    if (pErr != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(pErr);
                        pErr = IntPtr.Zero;
                    }
                }
            }
        }
        /// <summary>
        /// Inicialización del hilo de vida.
        /// </summary>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        private void InicHiloVida()
        {
            // Crear el objeto Hilo e iniciarlo. El parámetro iniciar indica
            // a la colección que una vez añadido el hilo se iniciado.
            Hilos.Add(new ThreadStart(ProcesarHiloVida), true);
        }
        /// <summary>
        /// Inicialización del hilo de vida.
        /// </summary>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        private void InicTareasEstado()
        {
            // Crear el objeto Hilo e iniciarlo. El parámetro iniciar indica
            // a la colección que una vez añadido el hilo se iniciado.
            Hilos.Add(new ThreadStart(ProcesarHiloEstado), true);
        }               
        /// <summary>
        /// Proceso del hilo de vida.
        /// </summary>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        private void ProcesarHiloVida()
        {
            //OInfoOPCvida infoVida = (OInfoOPCvida)this._tags.HtVida;
            while (true)
            {
                ////LogManager.GetLogger("wrapper").Info("Lectura variables");
                this._tags.HtVida.Valores = this.Leer(this._tags.HtVida.Variables);
                Thread.Sleep(this._config.TiempoVida);
            }
        }
        /// <summary>
        /// Proceso del hilo de vida.
        /// </summary>
        [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
        void ProcesarHiloEstado()
        {
            while (true)
            {
                // Procesar datos de lectura de valores en la colección
                // de vida, si se hubiera producido un error de lectura
                // y, por tanto, el tiempo  de espera  lectura  agotado,
                // la colección de valores devolverá null.
                // En la colección de datos se respetará el último valor
                // conocido correcto y se asignará el valor obtenido.
                // Inicializar contador de variables.
                OEstadoComms oopcComs = null;
                foreach (OOPCEnlaces enlace in oEnlaces.Values)
                {
                    bool error = false;

                    foreach (OInfoDato dato in this.Datos.Values)
                    {
                        if (dato.Calidad == GetQuality(0) && dato.Enlace == enlace.Nombre)
                        {
                            error = true;

                            if (enlace.Reintento <= MAXREINTENTOSERRORCOMM)
                            {
                                if (dato.UltimoValor != null)
                                {
                                    dato.Valor = dato.UltimoValor;
                                }
                                else
                                {
                                    dato.Valor = null;
                                }
                            }
                            else
                            {
                                dato.Valor = null;
                                dato.UltimoValor = null;
                            }
                        }
                    }
                    for (int i = 0; i < this._oOPCComms.Length; i++)
                    {
                        if (this._oOPCComms[i].Enlace == enlace.Nombre)
                        {
                            oopcComs = this._oOPCComms[i];

                        }
                    }
                    if (!error)
                    {
                        enlace.Reintento = 0;
                        oopcComs.Estado = "OK";
                        this._oEventargs.Argumento = oopcComs;
                    }
                    else
                    {
                        enlace.Reintento++;
                        if (enlace.Reintento > MAXREINTENTOSERRORCOMM)
                        {
                            oopcComs.Estado = "Error";
                            this._oEventargs.Argumento = oopcComs;
                        }
                        else
                        {
                            //Sirve para que en los microcortes no haya errores de comunicación
                            oopcComs.Estado = "OK";
                            this._oEventargs.Argumento = oopcComs;
                        }
                    }
                    this.OnComm(this._oEventargs);
                }
                Thread.Sleep(this._config.TiempoVida);
            }
        }
        /// <summary>
        /// Pone a cero la calidad de todas las variables. Se llama desde los catch de las lecturas y escrituras
        /// </summary>
        void ActualizarCalidadVariables()
        {
            foreach (OInfoDato dato in this.Datos.Values)
            {
                dato.Calidad = GetQuality(0);
            }
        }        
        /// <summary>
        /// Lecturas asíncronas.
        /// </summary>
        /// <param name="variables">Colección de variables.</param>
        /// <param name="trans">Id de transacción.</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        void Read(int[] variables, int trans)
        {
            lock (this)
            {
                int nCancelid;
                IntPtr pErr = IntPtr.Zero;
                if (pIOPCAsyncIO2Datos != null)
                {
                    try
                    {   // Lectura asíncrona de variables.
                        pIOPCAsyncIO2Datos.Read(variables.Length, variables, trans, out nCancelid, out pErr);
                        int[] err = new int[1];
                        Marshal.Copy(pErr, err, 0, 1);
                        if (err[0] != 0)
                        {
                            string pstrError;
                            pIOPCServer.GetErrorString(err[0], LOCALE_ID, out pstrError);
                        }
                    }
                    finally
                    {
                        // Liberar memoria.
                        if (pErr != IntPtr.Zero)
                        {
                            Marshal.FreeCoTaskMem(pErr);
                            pErr = IntPtr.Zero;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Escritura asíncrona.
        /// </summary>
        /// <param name="variables">Colección de variables.</param>
        /// <param name="valores">Colección de valores.</param>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        void Write(int[] variables, object[] valores)
        {
            int nCancelid;
            IntPtr pErr = IntPtr.Zero;
            if (pIOPCAsyncIO2Datos != null)
            {
                try
                {	// Escritura asíncrona.
                    pIOPCAsyncIO2Datos.Write(variables.Length, variables, valores, 2, out nCancelid, out pErr);
                    int[] errors = new int[1];
                    Marshal.Copy(pErr, errors, 0, 1);
                    if (errors[0] != 0)
                    {
                        Marshal.FreeCoTaskMem(pErr);
                        pErr = IntPtr.Zero;
                    }
                }
                catch (Exception) { }
                finally
                {
                    // Liberar la memoria.
                    if (pErr != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(pErr);
                        pErr = IntPtr.Zero;
                    }
                }
            }
        }                
        /// <summary>
        /// Leer el valor de los identificadores de variables de la colección.
        /// </summary>
        /// <param name="variables">Colección de variables.</param>
        /// <returns>Colección de resultados.</returns>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        private string[] Leer(int[] variables)
        {
            string[] resultado = null;
            if (variables != null)
            {
                try
                {
                    // Inicializar contador de variables.
                    int contador = variables.Length;

                    // Inicializar colección de identificadores de lectura.
                    int[] items = new int[contador];
                    for (int i = 0; i < contador; i++)
                    {
                        items[i] = this._tags.GetDatos(variables[i]).IdLectura;
                    }

                    // Leer asincronamente el valor del grupo
                    // de identificadores del PLC.

                    this.Read(items, 0);

                    // Inicializar resultado de valores asociados 
                    // a la posición del identificador de lectura.
                    resultado = new string[contador];

                    // Letargo del método hasta t-tiempo, o bien, hasta
                    // el término de  la  lectura   asíncrona  de datos.
                    if (this._sincronizacion.Dormir(0, TimeSpan.FromSeconds(this._config.TiempoEsperaLectura)))
                    {
                        // Actualizar la colección de resultados.
                        for (int i = 0; i < contador; i++)
                        {
                            resultado[i] = this._tags.GetDatos(variables[i]).Valor.ToString();
                        }
                    }
                    else
                    {
                        this.OnComm(null);
                    }
                    // Resetear el evento.
                    this._sincronizacion.Resetear(0);
                }
                catch (Exception ex)
                {
                    this.ActualizarCalidadVariables();
                    LogManager.GetLogger("wrapper").Error("Leer int:" + ex.ToString());
                }
            }
            return resultado;
        }
        /// <summary>
        /// Leer el valor de las descripciones de variables de la colección
        /// a partir del valor de la colección de datos DB actualiza  en el
        /// proceso del hilo vida.
        /// </summary>
        /// <param name="variables">Colección de variables.</param>
        /// <returns>Colección de resultados.</returns>
        private string[] Leer(string[] variables)
        {
            string[] resultado = null;
            if (variables != null)
            {
                // Inicializar contador de variables.
                int contador = variables.Length;

                // Asignar a la colección resultado el número
                // de variables de la colección de variables.
                resultado = new string[contador];
                for (int i = 0; i < contador; i++)
                {
                    resultado[i] = this._tags.GetDB(variables[i]).Valor.ToString();
                }
            }
            return resultado;
        }
        /// <summary>
        /// inicializa los string locales
        /// </summary>
        private void IniciaStringsLocales()
        {

            ArrayList variablesString = new ArrayList();

            foreach (DictionaryEntry item in this.GetDatos())
            {
                OInfoDato dato = (OInfoDato)item.Value;
                if (dato.Conexion.ToLower().Contains("localserver") && dato.Conexion.ToLower().Contains("string"))
                {
                    variablesString.Add(dato.Texto);
                }
            }

            if (variablesString.Count > 0)
            {
                string[] variables = new string[variablesString.Count];
                string[] valores = new string[variablesString.Count];

                for (int i = 0; i < variablesString.Count; i++)
                {
                    variables[i] = (string)variablesString[i];
                    valores[i] = "";
                }

                this.Escribir(variables, valores);
            }


        }
        /// <summary>
        /// Actualizar de la colección de datos los tipo alarmas.
        /// </summary>
        /// <returns></returns>
        private ArrayList ActualizarAlarmas()
        {
            ArrayList alarmasActivas = null;           
            foreach (DictionaryEntry item in this.Datos)
            {
                OInfoDato dato = (OInfoDato)item.Value;
                if (dato.ESAlarma)
                {
                    if (dato.Valor.ToString().ToUpperInvariant() == bool.TrueString.ToUpperInvariant())
                    {
                        if (!this.AlarmasActivas.Contains(dato.Texto))
                        {
                            this.AlarmasActivas.Add(dato.Texto);
                        }                            
                    }
                    else
                    {
                        if (this.AlarmasActivas.Contains(dato.Texto))
                        {
                            this.AlarmasActivas.Remove(dato.Texto);
                        }
                    }
                }
            }                
            
            return alarmasActivas;
        }      
        #region Estáticos
        /// <summary>
        /// Obtener calidad.
        /// </summary>
        /// <param name="wQuality">Calidad.</param>
        /// <returns></returns>
        String GetQuality(long wQuality)
        {
            String strQuality = "";
            switch (wQuality)
            {
                case Qualities.OPC_QUALITY_GOOD:
                    strQuality = "Good";
                    break;
                case Qualities.OPC_QUALITY_BAD:
                    strQuality = "Bad";
                    break;
                case Qualities.OPC_QUALITY_CONFIG_ERROR:
                    strQuality = "BadConfigurationError";
                    break;
                case Qualities.OPC_QUALITY_NOT_CONNECTED:
                    strQuality = "BadNotConnected";
                    break;
                case Qualities.OPC_QUALITY_DEVICE_FAILURE:
                    strQuality = "BadDeviceFailure";
                    break;
                case Qualities.OPC_QUALITY_SENSOR_FAILURE:
                    strQuality = "BadSensorFailure";
                    break;
                case Qualities.OPC_QUALITY_COMM_FAILURE:
                    strQuality = "BadCommFailure";
                    break;
                case Qualities.OPC_QUALITY_OUT_OF_SERVICE:
                    strQuality = "BadOutOfService";
                    break;
                case Qualities.OPC_QUALITY_WAITING_FOR_INITIAL_DATA:
                    strQuality = "BadWaitingForInitialData";
                    break;
                case Qualities.OPC_QUALITY_EGU_EXCEEDED:
                    strQuality = "UncertainEGUExceeded";
                    break;
                case Qualities.OPC_QUALITY_SUB_NORMAL:
                    strQuality = "UncertainSubNormal";
                    break;
                default:
                    strQuality = "Not handled";
                    break;
            }
            return strQuality;
        }
        #endregion
        #endregion

        #region Manejadors de eventos

        #region IOPCDataCallback
        /// <summary>
        /// Callback IOPCDataCallback OnDataChange   implementación del
        /// manejador de evento. Este evento es llamado por el servidor
        /// OPC cuando un dato cambio de valor.
        /// </summary>
        [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
        public virtual void OnDataChange(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, OpcRcw.Da.FILETIME[] pftTimeStamps, int[] pErrors)
        {
            if (phClientItems != null && pvValues != null)
            {
                switch (hGroup)
                {
                    // Cambio de dato.
                    case 2:
                        if (pwQualities != null)
                        {
                            try
                            {
                                OInfoDato infoOPClectura = null;
                                for (int i = 0; i < phClientItems.Length; i++)
                                {
                                    infoOPClectura = this._tags.GetLecturas(phClientItems[i]);
                                    infoOPClectura.Valor = pvValues[i] as string;
                                    infoOPClectura.Calidad = this.GetQuality(pwQualities[i]);
                                    this.OnCambioDato(new OEventArgs(infoOPClectura));
                                }
                            }
                            catch (Exception ex)
                            {
                                LogManager.GetLogger("wrapper").Error("Error en cambio dato:" + ex.ToString());
                            }
                        }
                        break;
                    // Alarmas.
                    case 3:
                        if (pwQualities != null)
                        {
                            try
                            {
                                OInfoDato infoOPCalarma = null;
                                for (int i = 0; i < phClientItems.Length; i++)
                                {
                                    infoOPCalarma = this._tags.GetAlarmas(phClientItems[i]);
                                    infoOPCalarma.Valor = pvValues[i] as string;
                                    infoOPCalarma.Calidad = this.GetQuality(pwQualities[i]);
                                    this.OnAlarma(new OEventArgs(infoOPCalarma));                                  
                                }
                            }
                            catch (Exception ex)
                            {
                                LogManager.GetLogger("wrapper").Error("Error en alarma:" + ex.ToString());
                            }
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// Callback IOPCDataCallback OnReadComplete implementación del
        /// manejador de evento. Este evento es llamado por el servidor
        /// OPC cuando se completa la lectura asíncrona.
        /// </summary>
        public virtual void OnReadComplete(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, OpcRcw.Da.FILETIME[] pftTimeStamps, int[] pErrors)
        {
            if (phClientItems != null && pErrors != null && pvValues != null && pwQualities != null)
            {
                // Recorrer el número de identificadores leidos de la colección.
                for (int i = 0; i < phClientItems.Length; i++)
                {
                    // Obtener el objeto InfoDato de la colección de datos
                    // en función de la clave del  identificador  de variable.
                    OInfoDato InfoDato = this._tags.GetDatos(phClientItems[i]);
                    if (pErrors[i] == 0)
                    {
                        // Si no se ha producido un error  en  la lectura del dato
                        // asignar los valores adecuados al objeto de la colección.
                        InfoDato.UltimoValor = InfoDato.Valor; // Último valor.
                        InfoDato.Valor = pvValues[i] as string;   // Valor.                                   
                        InfoDato.Calidad = this.GetQuality(pwQualities[i]);
                    }
                    else
                    {

                        LogManager.GetLogger("wrapper").Error("Error de lectura en la variable:" + InfoDato.Conexion);
                        // Se ha producido un error en la lectura del dato.                        
                        InfoDato.Calidad = this.GetQuality(0);
                    }
                }
            }
            // Despertar el método de lectura.
            this._sincronizacion.Despertar((short)dwTransid);
        }
        /// <summary>
        /// Callback IOPCDataCallback OnWriteComplete implementación del
        /// manejador de evento. Este evento es llamado por el servidor
        /// OPC cuando se completa la escritura asincrona.
        /// </summary>
        public virtual void OnWriteComplete(int dwTransid, int hGroup, int hrMastererr, int dwCount, int[] pClienthandles, int[] pErrors)
        {
            if (pErrors != null && pClienthandles != null)
            {
                for (int i = 0; i < pErrors.Length; i++)
                {
                    this._tags.GetDatos(pClienthandles[i]).Error = pErrors[i];
                }
                // Despertar el método de escritura.
                this._sincronizacion.Despertar(2);
            }
        }
        /// <summary>
        /// Callback IOPCDataCallback OnCancelComplete implementación del
        /// manejador de evento.
        /// </summary>
        /// <param name="dwTransid"></param>
        /// <param name="hGroup"></param>
        public virtual void OnCancelComplete(int dwTransid, int hGroup) { }
        #endregion

        #endregion
    }
    
}
