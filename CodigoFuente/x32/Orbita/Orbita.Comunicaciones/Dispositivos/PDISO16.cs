using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Permissions;
using System.Threading;
using MccDaq;
using Orbita.Trazabilidad;
using Orbita.Utiles;
namespace Orbita.Comunicaciones
{
    ///// <summary>
    ///// Comunicación con la placa PDSIO16.
    ///// </summary>
    //[SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "El origen del nombre abreviado.")]
    //public class PDiso16 : ODispositivo
    //{
    //    #region Atributos privados
    //    ODispositivo _dispositivo;
    //    OConfigDispositivo _config;
    //    OTags _tags;
    //    OEventArgs _oEventargs;
    //    static OHilos Hilos;
    //    MccBoard _daqBoard;
    //    OHashtable _salidas;
    //    OHashtable _entradas;
    //    OHashtable _lecturas;
    //    #endregion

    //    #region Constructores
    //    /// <summary>
    //    /// Inicializar una nueva instancia de la clase PDiso16.
    //    /// </summary>
    //    public PDiso16(OTags tags, OHilos hilos, ODispositivo dispositivo)
    //    {
    //        LogManager.GetLogger("wrapper").Info("Creando dispositivo PDISO16");
    //        //Inicio datos de dispositivo
    //        this._dispositivo = dispositivo;
    //        this._config = tags.Config;
    //        // Asignación de las colecciones de datos
    //        this._tags = tags;
    //        this._oEventargs = new OEventArgs();
    //        Hilos = hilos;
    //    }
    //    #endregion

    //    #region Métodos públicos
    //    /// <summary>
    //    /// Arranca la comunicación con el dispositivo
    //    /// </summary>
    //    public void Inicializar()
    //    {
    //        _daqBoard = new MccDaq.MccBoard(this._dispositivo.Identificador);
    //        clsDigitalIO dio = new clsDigitalIO();
    //        _daqBoard.FlashLED(); // Prueba para verificar que el módulo está conectado

    //        _daqBoard.DConfigPort(MccDaq.DigitalPortType.AuxPort, MccDaq.DigitalPortDirection.DigitalOut);
    //        _daqBoard.DConfigPort(MccDaq.DigitalPortType.AuxPort, MccDaq.DigitalPortDirection.DigitalIn);

    //        this.ConfigurarES();

    //        LogManager.GetLogger("wrapper").Info("PDISO16 iniciando hilo de lectura");
    //        this.InicHiloLectura("LecturaMdaq");
    //    }
    //    /// <summary>
    //    /// Escribir el valor en las salidas.
    //    /// </summary>
    //    /// <param name="variables">Colección de variables.</param>
    //    /// <param name="valores">Colección de valores.</param>
    //    /// <returns></returns>
    //    [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
    //    public bool Escribir(string[] variables, object[] valores)
    //    {
    //        bool resultado = true;
    //        try
    //        {
    //            if (variables != null)
    //            {
    //                for (int i = 0; i < variables.Length; i++)
    //                {
    //                    int bit = Convert.ToInt16(this._salidas[variables[i].ToString()]);


    //                    if (valores[i].ToString() == "0")
    //                    {
    //                        _daqBoard.DBitOut(MccDaq.DigitalPortType.AuxPort, bit, MccDaq.DigitalLogicState.Low);
    //                    }
    //                    else
    //                    {
    //                        _daqBoard.DBitOut(MccDaq.DigitalPortType.AuxPort, bit, MccDaq.DigitalLogicState.High);
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            resultado = false;
    //            LogManager.GetLogger("wrapper").Fatal("PDISO16 Error en la escritura: " + ex.ToString());
    //        }
    //        return resultado;
    //    }
    //    /// <summary>
    //    /// Lee el valor de las entradas
    //    /// </summary>
    //    /// <param name="variables">Colección de variables.</param>
    //    /// <returns></returns>
    //    [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
    //    public string[] Leer(string[] variables)
    //    {
    //        string[] resultado = new string[variables.Length];
    //        try
    //        {
    //            if (variables != null)
    //            {
    //                for (int i = 0; i < variables.Length; i++)
    //                {
    //                    int bit = Convert.ToInt16(this._entradas[variables[i].ToString()]);

    //                    DigitalLogicState salida;

    //                    _daqBoard.DBitIn(DigitalPortType.AuxPort, bit, out salida);

    //                    if (salida == DigitalLogicState.High)
    //                    {
    //                        resultado[i] = "1";
    //                    }
    //                    else if (salida == DigitalLogicState.Low)
    //                    {
    //                        resultado[i] = "0";
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            resultado = null;
    //            LogManager.GetLogger("wrapper").Fatal("PDISO16 Error en la lectura: " + ex.ToString());
    //        }
    //        return resultado;
    //    }
    //    #endregion

    //    #region Métodos privados
    //    /// <summary>
    //    /// Configura los campos de E/S
    //    /// </summary>
    //    void ConfigurarES()
    //    {
    //        this._salidas = new OHashtable();
    //        this._entradas = new OHashtable();
    //        this._lecturas = new OHashtable();
    //        foreach (OInfoDato item in this._tags.GetDatos().Values)
    //        {
    //            if (item.Enlace == "S")
    //            {
    //                this._salidas.Add(item.Texto, item.Bit);
    //            }
    //            else if (item.Enlace == "E")
    //            {
    //                this._entradas.Add(item.Texto, item.Bit);
    //            }
    //        }
    //        foreach (OInfoDato item in this._tags.GetLecturas().Values)
    //        {
    //            this._lecturas.Add(item.Bit, item);
    //        }
    //    }
    //    /// <summary>
    //    /// Inicio del hilo de lectura.
    //    /// </summary>
    //    /// <param name="descripcion">Descripción del hilo.</param>
    //    [EnvironmentPermissionAttribute(SecurityAction.Demand, Unrestricted = true)]
    //    void InicHiloLectura(string descripcion)
    //    {
    //        // Crear el objeto Hilo e iniciarlo. El parámetro iniciar indica
    //        // a la colección que una vez añadido el hilo se iniciado.
    //        Hilos.Add(new ThreadStart(ProcesarHiloVida), descripcion, true);
    //    }
    //    /// <summary>
    //    /// Proceso del hilo de vida.
    //    /// </summary>
    //    [EnvironmentPermission(SecurityAction.Demand, Unrestricted = true)]
    //    void ProcesarHiloVida()
    //    {
    //        int valorEntradas = 0;
    //        LogManager.GetLogger("wrapper").Info("PDISO16 hilo de lectura iniciado");
    //        while (true)
    //        {
    //            //LogManager.GetLogger("wrapper").Info("Lectura variables");

    //            try
    //            {
    //                int lectura = this.Leer();
    //                if (lectura != valorEntradas)
    //                {
    //                    for (int i = 0; i < 16; i++)
    //                    {
    //                        int res1 = lectura & Convert.ToInt32(Math.Pow(2, i));
    //                        int res2 = valorEntradas & Convert.ToInt32(Math.Pow(2, i));

    //                        try
    //                        {
    //                            if (res1 != res2)
    //                            {
    //                                OInfoDato info = (OInfoDato)this._lecturas[i];
    //                                if (res1 == 0)
    //                                {
    //                                    //MessageBox.Show("bit " + i.ToString() + " cambia a 0");
    //                                    info.Valor = "0";
    //                                }
    //                                else
    //                                {
    //                                    //MessageBox.Show("bit " + i.ToString() + " cambia a 1");
    //                                    info.Valor = "1";
    //                                }
    //                                this.OnCambioDato(new OEventArgs(info));
    //                            }
    //                        }
    //                        catch (Exception ex)
    //                        {
    //                            Console.WriteLine(ex.ToString());
    //                            LogManager.GetLogger("wrapper").Fatal("PDISO16 Error procesando entradas: " + ex.ToString());
    //                        }
    //                    }
    //                    valorEntradas = lectura;
    //                }
    //                Thread.Sleep(this._config.TiempoVida);
    //            }
    //            catch (Exception ex)
    //            {
    //                LogManager.GetLogger("wrapper").Fatal("Error generel en ProcesarHiloVida: " + ex.ToString());
    //                Thread.Sleep(100);
    //            }
    //        }
    //    }
    //    /// <summary>
    //    /// lectura de las entradas en bloque
    //    /// </summary>
    //    short Leer()
    //    {
    //        short resultado = 0;
    //        try
    //        {
    //            _daqBoard.DIn(DigitalPortType.AuxPort, out resultado);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogManager.GetLogger("wrapper").Fatal("PDISO16 Error lectura daqBoard: " + ex.ToString());
    //        }
    //        return resultado;
    //    }
    //    #endregion
    //}
    ///// <summary>
    ///// clsDigitalIO.
    ///// </summary>
    //public class clsDigitalIO
    //{
    //    #region Constantes
    //    public const int PORTOUT = 1;
    //    public const int PORTIN = 2;
    //    public const int PORTOUTSCAN = 5;
    //    public const int PORTINSCAN = 10;
    //    public const int BITOUT = 17;
    //    public const int BITIN = 34;
    //    public const int FIXEDPORT = 0;
    //    public const int PROGPORT = 1;
    //    public const int PROGBIT = 2;
    //    #endregion

    //    #region Atributos públicos
    //    public MccDaq.ErrorReporting ReportError;
    //    public MccDaq.ErrorHandling HandleError;
    //    #endregion

    //    #region Métodos públicos
    //    /// <summary>
    //    /// FindPortsOfType.
    //    /// </summary>
    //    /// <param name="DaqBoard"></param>
    //    /// <param name="PortType"></param>
    //    /// <param name="ProgAbility"></param>
    //    /// <param name="DefaultPort"></param>
    //    /// <param name="DefaultNumBits"></param>
    //    /// <param name="FirstBit"></param>
    //    /// <returns></returns>
    //    public int FindPortsOfType(MccDaq.MccBoard DaqBoard, int PortType, out int ProgAbility, out MccDaq.DigitalPortType DefaultPort, out int DefaultNumBits, out int FirstBit)
    //    {
    //        int ThisType, NumPorts, NumBits;
    //        int DefaultDev, InMask, OutMask;
    //        int PortsFound, curCount, curIndex;
    //        short status;
    //        bool PortIsCompatible;
    //        bool CheckBitProg = false;
    //        MccDaq.DigitalPortType CurPort;
    //        MccDaq.FunctionType DFunction;
    //        MccDaq.ErrorInfo ULStat;

    //        DefaultPort = (MccDaq.DigitalPortType)(-1);
    //        CurPort = DefaultPort;
    //        PortsFound = 0;
    //        FirstBit = 0;
    //        ProgAbility = -1;
    //        DefaultNumBits = 0;
    //        ULStat = DaqBoard.BoardConfig.GetDiNumDevs(out NumPorts);

    //        if ((PortType == BITOUT) || (PortType == BITIN))
    //            CheckBitProg = true;
    //        if ((PortType == PORTOUTSCAN) || (PortType == PORTINSCAN))
    //        {
    //            if (NumPorts > 0)
    //            {
    //                ULStat = MccDaq.MccService.ErrHandling
    //                    (MccDaq.ErrorReporting.DontPrint, MccDaq.ErrorHandling.DontStop);
    //                DFunction = MccDaq.FunctionType.DiFunction;
    //                if (PortType == PORTOUTSCAN)
    //                    DFunction = MccDaq.FunctionType.DoFunction;
    //                ULStat = DaqBoard.GetStatus(out status, out curCount, out curIndex, DFunction);
    //                if (!(ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors))
    //                    NumPorts = 0;
    //                ULStat = MccDaq.MccService.ErrHandling(ReportError, HandleError);
    //            }
    //            PortType = PortType & (PORTOUT | PORTIN);
    //        }

    //        for (int DioDev = 0; DioDev < NumPorts; ++DioDev)
    //        {
    //            ProgAbility = -1;
    //            ULStat = DaqBoard.DioConfig.GetDInMask(DioDev, out InMask);
    //            ULStat = DaqBoard.DioConfig.GetDOutMask(DioDev, out OutMask);
    //            if ((InMask & OutMask) > 0)
    //                ProgAbility = FIXEDPORT;
    //            ULStat = DaqBoard.DioConfig.GetDevType(DioDev, out ThisType);
    //            if (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors)
    //                CurPort = (DigitalPortType)Enum.Parse(typeof(DigitalPortType),
    //                    ThisType.ToString());
    //            if ((DioDev == 0) && (CurPort == MccDaq.DigitalPortType.FirstPortCL))
    //                //a few devices (USB-SSR08 for example)
    //                //start at FIRSTPORTCL and number the bits
    //                //as if FIRSTPORTA and FIRSTPORTB exist for
    //                //compatibiliry with older digital peripherals
    //                FirstBit = 16;

    //            //check if port is set for requested direction 
    //            //or can be programmed for requested direction
    //            PortIsCompatible = false;
    //            switch (PortType)
    //            {
    //                case (PORTOUT):
    //                    if (OutMask > 0)
    //                        PortIsCompatible = true;
    //                    break;
    //                case (PORTIN):
    //                    if (InMask > 0)
    //                        PortIsCompatible = true;
    //                    break;
    //                default:
    //                    PortIsCompatible = false;
    //                    break;
    //            }
    //            PortType = (PortType & (PORTOUT | PORTIN));
    //            if (!PortIsCompatible)
    //            {
    //                if (ProgAbility != FIXEDPORT)
    //                {
    //                    ULStat = MccDaq.MccService.ErrHandling
    //                    (MccDaq.ErrorReporting.DontPrint, MccDaq.ErrorHandling.DontStop);
    //                    MccDaq.DigitalPortDirection ConfigDirection;
    //                    ConfigDirection = DigitalPortDirection.DigitalOut;
    //                    if (PortType == PORTIN)
    //                        ConfigDirection = DigitalPortDirection.DigitalIn;
    //                    if ((CurPort == MccDaq.DigitalPortType.AuxPort) && CheckBitProg)
    //                    {
    //                        //if it's an AuxPort, check bit programmability
    //                        ULStat = DaqBoard.DConfigBit(MccDaq.DigitalPortType.AuxPort,
    //                            FirstBit, ConfigDirection);
    //                        if (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors)
    //                            ProgAbility = PROGBIT;
    //                    }
    //                    if (ProgAbility == -1)
    //                    {
    //                        //check port programmability
    //                        ULStat = DaqBoard.DConfigPort(CurPort, ConfigDirection);
    //                        if (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors)
    //                            ProgAbility = PROGPORT;
    //                    }
    //                }
    //                ULStat = MccDaq.MccService.ErrHandling(ReportError, HandleError);
    //                PortIsCompatible = !(ProgAbility == -1);
    //            }

    //            if (PortIsCompatible)
    //                PortsFound = PortsFound + 1;
    //            if (DefaultPort == (MccDaq.DigitalPortType)(-1))
    //            {
    //                ULStat = DaqBoard.DioConfig.GetNumBits(DioDev, out NumBits);
    //                DefaultNumBits = NumBits;
    //                DefaultDev = DioDev;
    //                DefaultPort = CurPort;
    //            }
    //        }
    //        return PortsFound;
    //    }
    //    #endregion
    //}
}
