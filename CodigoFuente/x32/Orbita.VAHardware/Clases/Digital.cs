//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 06-09-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using MccDaq;

namespace DigitalIO
{
    public class clsDigitalIO
    {
        public const int PORTOUT = 1;
        public const int PORTIN = 2;
        public const int PORTOUTSCAN = 5;
        public const int PORTINSCAN = 10;
        public const int BITOUT = 17;
        public const int BITIN = 34;
        public const int FIXEDPORT = 0;
        public const int PROGPORT = 1;
        public const int PROGBIT = 2;

        public MccDaq.ErrorReporting ReportError;
        public MccDaq.ErrorHandling HandleError;

        public int FindPortsOfType(MccDaq.MccBoard DaqBoard, int PortType,
            out int ProgAbility, out MccDaq.DigitalPortType DefaultPort, 
            out int DefaultNumBits, out int FirstBit)

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

            DefaultPort = (MccDaq.DigitalPortType) (-1);
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
                    if (! (ULStat.Value == MccDaq.ErrorInfo.ErrorCode.NoErrors))
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
                if (DefaultPort == (MccDaq.DigitalPortType) (- 1))
                {
                    ULStat = DaqBoard.DioConfig.GetNumBits(DioDev, out NumBits);
                    DefaultNumBits = NumBits;
                    DefaultDev = DioDev;
                    DefaultPort = CurPort;
                }
            }
            return PortsFound;
        }   
        
    }
}
