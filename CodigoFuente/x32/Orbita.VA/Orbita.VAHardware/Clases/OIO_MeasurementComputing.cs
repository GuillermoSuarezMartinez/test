//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : aibañez
// Last Modified On : 27-09-2012
// Description      : Bug solucionado: Al escribir una salida reseteaba el resto a falso
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MccDaq;
using Orbita.Utiles;
using Orbita.VAComun;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Clase que implementa las funciones para el control del módulo de Entradas/Salidas USB-1024HLS de Measurement Computing
    /// </summary>
    class OIO_MeasurementComputingUniversal : OTarjetaIOBase
    {
        #region Atributo(s)
        /// <summary>
        /// Driver de la tarjeta
        /// </summary>
        protected MccBoard DaqBoard;

        /// <summary>
        /// Timer de escaneo de las entradas
        /// </summary>
        protected Timer TimerScan;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Número identificativo del módulo
        /// </summary>
        protected int _BoardNum;
        /// <summary>
        /// Número identificativo del módulo
        /// </summary>
        protected int BoardNum
        {
            get { return _BoardNum; }
            set { _BoardNum = value; }
        }
        #endregion

        #region Contructores
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codHardware">Código del hardware</param>
        public OIO_MeasurementComputingUniversal(string codHardware)
            : base(codHardware)
        {
            // Creamos los campos
            this.TimerScan = new Timer();
            this.TimerScan.Interval = 1;
            this.TimerScan.Enabled = false;
            this.TimerScan.Tick += new EventHandler(EventoScan);

            // Cargamos valores de la base de datos
            DataTable dtTarjetaIO = AppBD.GetTarjetaIO(this.Codigo);
            if (dtTarjetaIO.Rows.Count == 1)
            {
                this._BoardNum = OEnteroRobusto.Validar(dtTarjetaIO.Rows[0]["DAQ_NumBoard"], 0, 1000, 0);

                this.CrearTerminales();
            }
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Rellenamos la lista genérica de terminales del puerto correspondiente
        /// </summary>
        /// <param name="terminalPuerto"></param>
        protected void RellenarListaTerminales(OTerminalIOMeasurementComputingInt terminalPuerto)
        {
            this.ListaTerminales.Add(terminalPuerto);
            foreach (OTerminalIOBase terminal in terminalPuerto.ListaTerminalesBit)
            {
                this.ListaTerminales.Add(terminal);
            }
        }
        #endregion

        #region Método(s) estático(s)
        /// <summary>
        /// Realiza las acciones oportunas en caso de error de operación de Entradas/Salida
        /// </summary>
        /// <param name="error">Valor devuelto por la operación de Entradas/Salidas</param>
        internal static void ProcesarError(ErrorInfo error, string codigo)
        {
            if ((error != null) && (error.Value != 0))
            {
                OVALogsManager.Error(OModulosHardware.ES_MeasurementComputing, codigo, "Error: " + error.Message);
            }
        }
        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Crea los terminales de la tarjeta
        /// </summary>
        protected virtual void CrearTerminales()
        {
            // Implementado en los hijos
        }

        #endregion

        #region Método(s) heredado(s)

        /// <summary>
        /// Método a heredar donde se conecta y se configura la tarjeta de IO
        /// </summary>
        public override void Inicializar()
        {
            try
            {
                base.Inicializar();
                if (this.Habilitado)
                {
                    // Se crea la tarjeta
                    DaqBoard = new MccDaq.MccBoard(_BoardNum);                    
                    DigitalIO dio = new DigitalIO();
                    DaqBoard.FlashLED(); // Prueba para verificar que el módulo está conectado

                    foreach (OTerminalIOBase terminal in this.ListaTerminales)
                    {
                        if (terminal is OTerminalIOMeasurementComputingInt)
                        {
                            ((OTerminalIOMeasurementComputingInt)terminal).Inicializar(this.DaqBoard);                            
                        }
                    }

                    // Ponemos en marcha el timer de escaneo
                    this.TimerScan.Enabled = true;
                }
                this.Existe = true;
            }
            catch (AccessViolationException exception)
            {
                OMensajes.MostrarError("El módulo de entradas/salidas con identificador " + this.Codigo + "\n no se encuetra o está actualmente en uso.");
                OVALogsManager.Info(OModulosHardware.CamaraBaslerVPro, this.Codigo, exception);
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.ES_MeasurementComputing, this.Codigo, exception);
            }
        }

        /// <summary>
        /// Finaliza la cámara
        /// </summary>
        public override void Finalizar()
        {
            base.Finalizar();

            this.TimerScan.Stop();
        }
        #endregion

        #region Eventos

        /// <summary>
        /// Evento del timer de ejecución
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        public void EventoScan(object sender, EventArgs e)
        {
            this.TimerScan.Stop();
            try
            {
                foreach (OTerminalIOBase terminal in this.ListaTerminales)
                {
                    if (terminal is OTerminalIOMeasurementComputingInt)
                    {
                        ((OTerminalIOMeasurementComputingInt)terminal).LeerEntrada();
                    }
                }
            }
            catch (Exception exception)
            {
                OVALogsManager.Error(OModulosHardware.ES_MeasurementComputing, this.Codigo, exception);
            }
            this.TimerScan.Start();
        }

        #endregion
    }

    /// <summary>
    /// Clase que implementa las funciones para el control del módulo de Entradas/Salidas USB-1024HLS de Measurement Computing
    /// </summary>
    class OIO_USB1024HLS : OIO_MeasurementComputingUniversal
    {
        #region Atributo(s)
        /// <summary>
        /// PuertoA
        /// </summary>
        private OTerminalIOMeasurementComputingInt PuertoA;

        /// <summary>
        /// PuertoB
        /// </summary>
        private OTerminalIOMeasurementComputingInt PuertoB;

        /// <summary>
        /// PuertoCL
        /// </summary>
        private OTerminalIOMeasurementComputingInt PuertoCL;

        /// <summary>
        /// PuertoCH
        /// </summary>
        private OTerminalIOMeasurementComputingInt PuertoCH;
        #endregion

        #region Contructores
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codHardware">Código del hardware</param>
        public OIO_USB1024HLS(string codHardware)
            : base(codHardware)
        {
        }
        #endregion

        #region Método(s) heredado(s)

        /// <summary>
        /// Crea los terminales de la tarjeta
        /// </summary>
        protected override void CrearTerminales()
        {
            // Cargamos los terminales del puerto A
            this.PuertoA = new OTerminalIOMeasurementComputingInt(this.Codigo, "FirstPortA", DigitalPortType.FirstPortA, 8);
            this.RellenarListaTerminales(this.PuertoA);

            // Cargamos los terminales del puerto B
            this.PuertoB = new OTerminalIOMeasurementComputingInt(this.Codigo, "FirstPortB", DigitalPortType.FirstPortB, 8);
            this.RellenarListaTerminales(this.PuertoB);

            // Cargamos los terminales del puerto CL
            this.PuertoCL = new OTerminalIOMeasurementComputingInt(this.Codigo, "FirstPortCL", DigitalPortType.FirstPortCL, 4);
            this.RellenarListaTerminales(this.PuertoCL);

            // Cargamos los terminales del puerto CH
            this.PuertoCH = new OTerminalIOMeasurementComputingInt(this.Codigo, "FirstPortCH", DigitalPortType.FirstPortCH, 4);
            this.RellenarListaTerminales(this.PuertoCH);
        }

        #endregion
    }

    /// <summary>
    /// Clase que implementa las funciones para el control del módulo de Entradas/Salidas E-PDISO16 de Measurement Computing
    /// </summary>
    class OIO_EPDISO16 : OIO_MeasurementComputingUniversal
    {
        #region Atributo(s)
        /// <summary>
        /// PuertoA
        /// </summary>
        private OTerminalIOMeasurementComputingInt PuertoI;

        /// <summary>
        /// PuertoB
        /// </summary>
        private OTerminalIOMeasurementComputingInt PuertoO;

        #endregion

        #region Contructores
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="codHardware">Código del hardware</param>
        public OIO_EPDISO16(string codHardware)
            : base(codHardware)
        {
        }
        #endregion

        #region Método(s) heredado(s)

        /// <summary>
        /// Crea los terminales de la tarjeta
        /// </summary>
        protected override void CrearTerminales()
        {
            // Cargamos los terminales del puerto de entrada
            this.PuertoI = new OTerminalIOMeasurementComputingInt(this.Codigo, "FirstPortI", DigitalPortType.AuxPort, 16);
            this.RellenarListaTerminales(this.PuertoI);

            // Cargamos los terminales del puerto de salida
            this.PuertoO = new OTerminalIOMeasurementComputingInt(this.Codigo, "FirstPortO", DigitalPortType.AuxPort, 16);
            this.RellenarListaTerminales(this.PuertoO);
        }

        #endregion
    }

    /// <summary>
    /// Terminal de tipo ushort que simboliza un puerto entero
    /// </summary>
    internal class OTerminalIOMeasurementComputingInt : OTerminalIOBase
    {
        #region Atributo(s)
        /// <summary>
        /// Puerto físico al que hace referencia
        /// </summary>
        public DigitalPortType PortType;

        /// <summary>
        /// Tipo de puerto según su acción de entrada o de salida
        /// </summary>
        private DigitalPortDirection PortDirection;

        /// <summary>
        /// Número de bits que contiene el puerto
        /// </summary>
        public short NumBits;

        /// <summary>
        /// Driver de la tarjeta
        /// </summary>
        private MccBoard DaqBoard;

        public List<OTerminalIOMeasurementComputingBit> ListaTerminalesBit;

        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Valor del terminal
        /// </summary>
        public new ushort Valor
        {
            get
            {
                ushort ushortValor;
                if (this.ComprobarValor(base.Valor, out ushortValor))
                {
                    return ushortValor;
                }
                return 0;
            }
            set
            {
                base.Valor = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public OTerminalIOMeasurementComputingInt(string codTarjetaIO, string codTerminalIO, DigitalPortType portType, short numBits)
            : base(codTarjetaIO, codTerminalIO)
        {
            // Inicialiamos los valores
            this.PortType = portType;
            this.NumBits = numBits;

            switch (this.TipoTerminalIO)
            {
                case OTipoTerminalIO.EntradaDigital:
                    this.PortDirection = DigitalPortDirection.DigitalIn;
                    break;
                case OTipoTerminalIO.SalidaDigital:
                    this.PortDirection = DigitalPortDirection.DigitalOut;
                    break;
            }

            this.ListaTerminalesBit = new List<OTerminalIOMeasurementComputingBit>();

            // Creación de los bits asociados
            for (int i = 0; i < numBits; i++)
            {
                OTerminalIOMeasurementComputingBit terminalBit = new OTerminalIOMeasurementComputingBit(this.CodTarjeta, this.Codigo + i.ToString(), this);
                this.ListaTerminalesBit.Add(terminalBit);
            }
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Método a heredar donde se inicializan los terminales
        /// </summary>
        public new void Inicializar(MccBoard daqBoard)
        {
            base.Inicializar();

            this.DaqBoard = daqBoard;

            ErrorInfo error = DaqBoard.DConfigPort(this.PortType, this.PortDirection);
            OIO_MeasurementComputingUniversal.ProcesarError(error, this.Codigo);

            foreach (OTerminalIOMeasurementComputingBit terminalBit in this.ListaTerminalesBit)
            {
                terminalBit.Inicializar(daqBoard);
            }

            this.Existe = true;
        }

        /// <summary>
        /// Lectura de la entrada física
        /// </summary>
        public override void LeerEntrada()
        {
            if (this.Habilitado && this.Existe)
            {
                if (this.TipoTerminalIO == OTipoTerminalIO.EntradaDigital)
                {
                    // Leo la entrada propia
                    ushort valor;
                    ErrorInfo error = DaqBoard.DIn(this.PortType, out valor);
                    OIO_MeasurementComputingUniversal.ProcesarError(error, this.Codigo);

                    if (error.Value == 0)
                    {
                        ushort ushortValor = (ushort)valor;

                        if (this.Valor != ushortValor)
                        {
                            this.Valor = ushortValor;
                            this.EstablecerValorVariable();
                            //VariableRuntime.SetValue(this.CodVariable, this.Valor, "IO", this.Codigo);

                            // Actualizo las entradas de tipo bit asociadas
                            foreach (OTerminalIOMeasurementComputingBit terminalBit in this.ListaTerminalesBit)
                            {
                                terminalBit.LeerEntrada(this.Valor);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Escritura de la salida física
        /// </summary>
        public override void EscribirSalida(string codigoVariable, object valor)
        {
            if (this.TipoTerminalIO == OTipoTerminalIO.SalidaDigital)
            {
                base.EscribirSalida(codigoVariable, valor);

                // Se comprueba que el valor a escribir sea correcto
                ushort ushortValor;
                if (this.ComprobarValor(valor, out ushortValor))
                {
                    // Si el valor es correcto se escribe la salida física
                    if (this.Habilitado && this.Existe)
                    {
                        // Se escribe la entrada propia
                        ErrorInfo error = DaqBoard.DOut(this.PortType, (ushort)ushortValor);
                        OIO_MeasurementComputingUniversal.ProcesarError(error, this.Codigo);
                    }
                    this.Valor = ushortValor;

                    // Se actualizan los valores de las salidas de tipo bit asociadas
                    foreach (OTerminalIOMeasurementComputingBit terminalBit in this.ListaTerminalesBit)
                    {
                        terminalBit.EscribirSalida(ushortValor);
                    }
                }
            }
        }

        /// <summary>
        /// Se comprueba que el valor a escribir sea del tipo correcto
        /// </summary>
        /// <param name="valor">Valor a escribir</param>
        /// <param name="ushortValor">Valor a escribir del tipo correcto</param>
        /// <returns>Devuelve verdadero si el valor a escribir es válido</returns>
        private bool ComprobarValor(object valor, out ushort ushortValor)
        {
            ushortValor = 0;

            // Se comprueba que el valor sea correcto
            bool valorOK = false;
            if (OEnteroRobusto.IsNumericInt(valor))
            {
                int intValor = Convert.ToInt32(valor);
                int maxvalor = (int)Math.Pow(2, this.NumBits) - 1;

                if (OEnteroRobusto.InRange(intValor, 0x00, maxvalor))
                {
                    ushortValor = (ushort)intValor;
                    valorOK = true;
                }
            }
            return valorOK;
        }

        #endregion
    }

    /// <summary>
    /// Terminal de tipo bit que simboliza un bit de un puerto
    /// </summary>
    internal class OTerminalIOMeasurementComputingBit : OTerminalIOBase
    {
        #region Atributo(s)
        /// <summary>
        /// Puerto físico al que hace referencia
        /// </summary>
        public DigitalPortType PortType;

        /// <summary>
        /// Driver de la tarjeta
        /// </summary>
        private MccBoard DaqBoard;

        /// <summary>
        /// Terminal de tipo ushort al que pertenece
        /// </summary>
        private OTerminalIOMeasurementComputingInt TerminalPerteneciente;

        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Valor del terminal
        /// </summary>
        public new bool Valor
        {
            get
            {
                bool boolValor;
                if (this.ComprobarValor(base.Valor, out boolValor))
                {
                    return boolValor;
                }
                return false;
            }
            set
            {
                base.Valor = value;
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Contructor de la clase
        /// </summary>
        public OTerminalIOMeasurementComputingBit(string codTarjetaIO, string codTerminalIO, OTerminalIOMeasurementComputingInt terminalPerteneciente)
            : base(codTarjetaIO, codTerminalIO)
        {
            // Inicialiamos los valores
            this.PortType = terminalPerteneciente.PortType;
            this.TerminalPerteneciente = terminalPerteneciente;
        }
        #endregion

        #region Método(s) heredado(s)

        /// <summary>
        /// Método a heredar donde se inicializan los terminales
        /// </summary>
        public new void Inicializar(MccBoard daqBoard)
        {
            base.Inicializar();

            this.DaqBoard = daqBoard;
            this.Existe = true;
        }
                                                            
        /// <summary>
        /// Lectura de la entrada física
        /// </summary>
        public override void LeerEntrada()
        {
            if (this.Habilitado)
            {
                DigitalLogicState estado = new DigitalLogicState();
                ErrorInfo error = DaqBoard.DBitIn(this.PortType, this.Numero, out estado);

                OIO_MeasurementComputingUniversal.ProcesarError(error, this.Codigo);
                
                if (error.Value == 0)
                {
                    bool lectura = false;
                    switch (estado)
                    {
                        case DigitalLogicState.High:
                            lectura = true;
                            break;
                        case DigitalLogicState.Low:
                            lectura = false;
                            break;
                    }
                    this.LeerEntrada(lectura);
                }
            }
        }

        /// <summary>
        /// Posteriormente a la lectura del entero del puerto se acutaliza el valor de la entrada del bit
        /// </summary>
        public void LeerEntrada(ushort ushortValor)
        {
            if (this.Habilitado && this.Existe)
            {
                bool boolValor = OBinario.GetBit(ushortValor, this.Numero);
                if (this.Valor != boolValor)
                {
                    this.Valor = boolValor;
                    this.EstablecerValorVariable();
                    //VariableRuntime.SetValue(this.CodVariable, this.Valor, "IO", this.Codigo);
                }
            }
        }
        /// <summary>
        /// Posteriormente a la lectura del bit del puerto se acutaliza el valor a la variable
        /// </summary>
        public void LeerEntrada(bool boolValor)
        {
            if (this.Habilitado && this.Existe)
            {
                if (this.Valor != boolValor)
                {
                    this.Valor = boolValor;
                    this.EstablecerValorVariable();
                    //VariableRuntime.SetValue(this.CodVariable, this.Valor, "IO", this.Codigo);
                }
            }
        }

        /// <summary>
        /// Escritura de la salida física
        /// </summary>
        public override void EscribirSalida(string codigoVariable, object valor)
        {
            base.EscribirSalida(codigoVariable, valor);

            // Se comprueba que el valor a escribir sea correcto
            bool boolValor;
            if (this.ComprobarValor(valor, out boolValor))
            {
                // Si el valor es correcto se escribe la salida física
                if (this.Habilitado && this.Existe)
                {
                    ushort ushortValor = this.TerminalPerteneciente.Valor;
                    OBinario.SetBit(ref ushortValor, this.Numero, boolValor);

                    //this.TerminalPerteneciente.EscribirSalida(codigoVariable, ushortValor);

                    // Se escribe la entrada propia
                    ErrorInfo error = DaqBoard.DOut(this.PortType, (ushort)ushortValor);
                    OIO_MeasurementComputingUniversal.ProcesarError(error, this.Codigo);
                    this.TerminalPerteneciente.Valor = ushortValor;
                }
                this.Valor = boolValor;
            }
        }

        /// <summary>
        /// Escritura de la salida física
        /// </summary>
        public void EscribirSalida(ushort valor)
        {
            // Si el valor es correcto se escribe la salida física
            this.Valor = OBinario.GetBit(valor, this.Numero);
        }

        /// <summary>
        /// Se comprueba que el valor a escribir sea del tipo correcto
        /// </summary>
        /// <param name="valor">Valor a escribir</param>
        /// <param name="boolValor">Valor a escribir del tipo correcto</param>
        /// <returns>Devuelve verdadero si el valor a escribir es válido</returns>
        private bool ComprobarValor(object valor, out bool boolValor)
        {
            boolValor = false;

            // Se comprueba que el valor sea correcto
            bool valorOK = false;
            if (valor is bool)
            {
                boolValor = (bool)valor;
                valorOK = true;
            }
            return valorOK;
        }
        #endregion
    }

    /// <summary>
    /// Clase para el manejo de los módulos de Measurement Computing
    /// </summary>
    internal class DigitalIO
    {
        #region Constante(s)
        public const int PORTOUT = 1;
        public const int PORTIN = 2;
        public const int PORTOUTSCAN = 5;
        public const int PORTINSCAN = 10;
        public const int BITOUT = 17;
        public const int BITIN = 34;
        public const int FIXEDPORT = 0;
        public const int PROGPORT = 1;
        public const int PROGBIT = 2;
        #endregion

        #region Declaracion(es) de eventos
        public MccDaq.ErrorReporting ReportError;
        public MccDaq.ErrorHandling HandleError;
        #endregion

        #region Método(s) público(s)
        public int FindPortsOfType(MccDaq.MccBoard DaqBoard, int PortType, out int ProgAbility, out MccDaq.DigitalPortType DefaultPort, out int DefaultNumBits, out int FirstBit)
        {
            int ThisType, NumPorts, NumBits;
            int DefaultDev, InMask, OutMask;
            int PortsFound, curCount, curIndex;
            short status;
            bool PortIsCompatible;
            bool CheckBitProg = false;
            MccDaq.DigitalPortType CurPort;
            MccDaq.FunctionType DFunction;
            MccDaq.ErrorInfo ULStat;

            DefaultPort = (MccDaq.DigitalPortType)(-1);
            CurPort = DefaultPort;
            PortsFound = 0;
            FirstBit = 0;
            ProgAbility = -1;
            DefaultNumBits = 0;
            ULStat = DaqBoard.BoardConfig.GetDiNumDevs(out NumPorts);

            if ((PortType == BITOUT) || (PortType == BITIN))
                CheckBitProg = true;
            if ((PortType == PORTOUTSCAN) || (PortType == PORTINSCAN))
            {
                if (NumPorts > 0)
                {
                    ULStat = MccDaq.MccService.ErrHandling
                        (MccDaq.ErrorReporting.DontPrint, MccDaq.ErrorHandling.DontStop);
                    DFunction = MccDaq.FunctionType.DiFunction;
                    if (PortType == PORTOUTSCAN)
                        DFunction = MccDaq.FunctionType.DoFunction;
                    ULStat = DaqBoard.GetStatus(out status, out curCount, out curIndex, DFunction);
                    if (!(ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors))
                        NumPorts = 0;
                    ULStat = MccDaq.MccService.ErrHandling(ReportError, HandleError);
                }
                PortType = PortType & (PORTOUT | PORTIN);
            }

            for (int DioDev = 0; DioDev < NumPorts; ++DioDev)
            {
                ProgAbility = -1;
                ULStat = DaqBoard.DioConfig.GetDInMask(DioDev, out InMask);
                ULStat = DaqBoard.DioConfig.GetDOutMask(DioDev, out OutMask);
                if ((InMask & OutMask) > 0)
                    ProgAbility = FIXEDPORT;
                ULStat = DaqBoard.DioConfig.GetDevType(DioDev, out ThisType);
                if (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors)
                    CurPort = (DigitalPortType)Enum.Parse(typeof(DigitalPortType),
                        ThisType.ToString());
                if ((DioDev == 0) && (CurPort == MccDaq.DigitalPortType.FirstPortCL))
                    //a few devices (USB-SSR08 for example)
                    //start at FIRSTPORTCL and number the bits
                    //as if FIRSTPORTA and FIRSTPORTB exist for
                    //compatibiliry with older digital peripherals
                    FirstBit = 16;

                //check if port is set for requested direction 
                //or can be programmed for requested direction
                PortIsCompatible = false;
                switch (PortType)
                {
                    case (PORTOUT):
                        if (OutMask > 0)
                            PortIsCompatible = true;
                        break;
                    case (PORTIN):
                        if (InMask > 0)
                            PortIsCompatible = true;
                        break;
                    default:
                        PortIsCompatible = false;
                        break;
                }
                PortType = (PortType & (PORTOUT | PORTIN));
                if (!PortIsCompatible)
                {
                    if (ProgAbility != FIXEDPORT)
                    {
                        ULStat = MccDaq.MccService.ErrHandling
                        (MccDaq.ErrorReporting.DontPrint, MccDaq.ErrorHandling.DontStop);
                        MccDaq.DigitalPortDirection ConfigDirection;
                        ConfigDirection = DigitalPortDirection.DigitalOut;
                        if (PortType == PORTIN)
                            ConfigDirection = DigitalPortDirection.DigitalIn;
                        if ((CurPort == MccDaq.DigitalPortType.AuxPort) && CheckBitProg)
                        {
                            //if it's an AuxPort, check bit programmability
                            ULStat = DaqBoard.DConfigBit(MccDaq.DigitalPortType.AuxPort,
                                FirstBit, ConfigDirection);
                            if (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors)
                                ProgAbility = PROGBIT;
                        }
                        if (ProgAbility == -1)
                        {
                            //check port programmability
                            ULStat = DaqBoard.DConfigPort(CurPort, ConfigDirection);
                            if (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors)
                                ProgAbility = PROGPORT;
                        }
                    }
                    ULStat = MccDaq.MccService.ErrHandling(ReportError, HandleError);
                    PortIsCompatible = !(ProgAbility == -1);
                }

                if (PortIsCompatible)
                    PortsFound = PortsFound + 1;
                if (DefaultPort == (MccDaq.DigitalPortType)(-1))
                {
                    ULStat = DaqBoard.DioConfig.GetNumBits(DioDev, out NumBits);
                    DefaultNumBits = NumBits;
                    DefaultDev = DioDev;
                    DefaultPort = CurPort;
                }
            }
            return PortsFound;
        }
        #endregion
    }
}
