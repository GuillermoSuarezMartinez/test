using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Orbita.Trazabilidad;

namespace Orbita.Comunicaciones
{
    /// <summary>
    /// Protocolo de comunicación con WinCC
    /// </summary>
    public class OWinCCDataManager
    {
        #region dm constants
        private const Int32 MAX_DM_VAR_NAME = 128;
        private const Int32 _MAX_PATH = 260;
        private const Int32 MAX_DM_DSN_NAME = 32;
        private const Int32 MAX_DM_TYPE_NAME = 128;

        private const Int32 DM_E_NO_RT_PRJ = 0x00000004;
        private const Int32 DM_E_ALREADY_CONNECTED = 0x00000006;

        private const Int32 DM_VARFILTER_TYPE = 0x00000001;
        private const Int32 DM_VARFILTER_NAME = 0x00000004;

        private const Int32 DM_VARKEY_NAME = 0x00000002;

        private const Int32 DM_VARTYPE_BIT = 1;
        private const Int32 DM_VARTYPE_SBYTE = 2;
        private const Int32 DM_VARTYPE_BYTE = 3;
        private const Int32 DM_VARTYPE_SWORD = 4;
        private const Int32 DM_VARTYPE_WORD = 5;
        private const Int32 DM_VARTYPE_SDWORD = 6;
        private const Int32 DM_VARTYPE_DWORD = 7;
        private const Int32 DM_VARTYPE_FLOAT = 8;
        private const Int32 DM_VARTYPE_DOUBLE = 9;
        private const Int32 DM_VARTYPE_TEXT_8 = 10;
        private const Int32 DM_VARTYPE_TEXT_16 = 11;

        private const Int32 VT_BOOL = 11;
        private const Int32 VT_I1 = 16;
        private const Int32 VT_UI1 = 17;
        private const Int32 VT_I2 = 2;
        private const Int32 VT_UI2 = 18;
        private const Int32 VT_I4 = 3;
        private const Int32 VT_UI4 = 19;
        private const Int32 VT_R4 = 4;
        private const Int32 VT_R8 = 5;
        private const Int32 VT_BSTR = 8;

        // * * *  DM-strcuts  * * *

        // Structs need a pack=1 attribute 
        // Attention:
        // *  Structs are valuetypes; they must have the same layout as the unmanaged code:
        //    > [StructLayout(LayoutKind.Sequential/.Explicit, Pack=1)]
        //
        // *  all referenctypes (class) has to marhalled (with [MarshalAs(...)]-attribute)
        //    > Strings(char-Arrays): as UnmanagedType.ByValTStr, SizeConst=<Größe>
        //      (CharSet.Ansi must be a StructLayout)
        //    > Strings (Pointer): as UnmanagedType.LPStr
        //
        // *  Pointer can be defined as pointer (unsafe-Code needed) or as IntPtr.

        [StructLayout(LayoutKind.Sequential,
         Pack = 1, CharSet = CharSet.Ansi)]
        private struct DM_VARKEY
        {
            public UInt32 dwKeyType;
            public UInt32 dwID;
            [MarshalAs(UnmanagedType.ByValTStr,
             SizeConst = MAX_DM_VAR_NAME + 1)]
            public String szName;
            public IntPtr pUserData;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        private struct CMN_ERROR
        {
            public UInt32 dwError1;
            public UInt32 dwError2;
            public UInt32 dwError3;
            public UInt32 dwError4;
            public UInt32 dwError5;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public String szErrorText;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct DM_VARFILTER
        {
            public UInt32 dwFlags;
            public UInt32 dwNumTypes;
            public IntPtr pdwTypes;
            [MarshalAs(UnmanagedType.LPStr)]
            public String pszGroup;
            [MarshalAs(UnmanagedType.LPStr)]
            public String pszName;
            [MarshalAs(UnmanagedType.LPStr)]
            public String pszConn;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
        private struct DM_TYPEREF
        {
            public UInt32 dwType;
            public UInt32 dwSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_DM_TYPE_NAME + 1)]
            public String szTypeName;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct DM_VAR_UPDATE_STRUCT
        {
            public DM_TYPEREF dmTypeRef;
            public DM_VARKEY dmVarKey;
            public VARIANT dmValue;
            public UInt32 dwState;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct DM_PROJECT_INFO
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = _MAX_PATH + 1)]
            public String szProjectFile;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_DM_DSN_NAME + 1)]
            public String szDSNName;
            UInt32 dwDataLocale;
        }

        // VARIANT: von www.pinvoke.net
        //
        // Um unions nachzubilden, wird das Offset der Member in der struct
        // (welche LayoutKind.Explicit) haben muss) über das FieldOffset-Attribut angegeben.
        //
        // Es gibt zwar ein [MarshalAs(UnmanagedType.Bstr)]-Attribut für String. Dieses kann aber nicht
        // verwendet werden, um VT_BSTR (bstrVal) zu marshalen, weil Referenz- und Werttypen
        // sich sinnvollerweise nicht überlappen dürfen.

        [StructLayout(LayoutKind.Explicit, Size = 16)]
        struct VARIANT
        {
            [FieldOffset(0)]
            public UInt16 vt;
            [FieldOffset(8)]
            public Int64 llVal;
            [FieldOffset(8)]
            public Int32 lVal;
            [FieldOffset(8)]
            public Byte bVal;
            [FieldOffset(8)]
            public Int16 iVal;
            [FieldOffset(8)]
            public Single fltVal;
            [FieldOffset(8)]
            public Double dblVal;
            [FieldOffset(8)]
            public Int16 BooleanVal;
            [FieldOffset(8)]
            public Int32 scode;
            [FieldOffset(8)]
            public Double date;
            [FieldOffset(8)]
            public SByte cVal;
            [FieldOffset(8)]
            public UInt32 uiVal;
            [FieldOffset(8)]
            public Int32 intVal;
            [FieldOffset(8)]
            public UInt32 uintVal;
            [FieldOffset(8)]
            public IntPtr bstrVal;
            // die anderen Member werden nicht gebraucht
        }


        //  * * *  Callbacks  * * * 

        // It is important to set the callconvention attribute to Cdecl,
        // the standard value__stdcall will not work with ODK dlls

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Boolean DM_NOTIFY_PROC(UInt32 dwNotifyClass, UInt32 dwNotifyCode, IntPtr pData, UInt32 dwItems, IntPtr pUser);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Boolean DM_ENUM_VAR_PROC(ref DM_VARKEY varkey, ref Object objvars);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Boolean DM_ENUM_OPENED_PROJECTS_PROC(ref DM_PROJECT_INFO pInfo, ref String strProject);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Boolean DM_NOTIFY_VARIABLE_PROC(
            UInt32 dwTAID,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] 
        DM_VAR_UPDATE_STRUCT[] updates,
            UInt32 dwItems,
            ref Object objlist);



        //  * * *  DM-Funktionen  * * *

        // Wo die DM-Funktionen Pointer erwarten, können Referenzen übergeben werden:
        // Referenztypen (class) prinzipiell direkt; bei Werttypen (struct) muss ref
        // oder out verwendet werden. Die Attribute [In] und [Out] sind dort gesetzt,
        // wo das Standardverhalten des Marshalers geändert werden soll (siehe MSDN):
        // class <--> [In]    ref class <--> [In, Out]    ref struct <--> [In, Out]
        //
        // Als Parameter für die Callbacks können Referenztypen nicht direkt
        // übergeben werden, weil der .NET Marshaler hierbei Probleme bekommt.
        // Stattdessen müssen sie mit ref übergeben werden.
        // Generics können ebenfalls nicht gemarshalt werden und müssen als ref Object
        // übergeben werden.
        //
        // Arrays können direkt übergeben werden, benötigen jedoch
        // die Attribute [In, MarshalAs(UnmanagedType.LPArray)] und ggf. [Out].

        [DllImport("dmclient.dll")]
        private static extern Boolean DMEnumVariables(String pszProject, [In] ref DM_VARFILTER pVarFilter, DM_ENUM_VAR_PROC fpEnumProc,
          ref Object objvars, [Out] out CMN_ERROR pError);

        [DllImport("dmclient.dll")]
        private static extern Boolean DMGetRuntimeProject([Out, MarshalAs(UnmanagedType.LPArray)] Byte[] pszProjectFile, UInt32 dwBufSize,
          [Out] out CMN_ERROR pError);

        [DllImport("dmclient.dll")]
        private static extern Boolean DMConnect(
            String pszAppName,
            DM_NOTIFY_PROC fpNotifyProc,
            IntPtr pUser,
            [Out] out CMN_ERROR pError);

        [DllImport("dmclient.dll")]
        private static extern Boolean DMDisConnect([Out] out CMN_ERROR pError);

        [DllImport("dmclient.dll")]
        private static extern Boolean DMSetValue([MarshalAs(UnmanagedType.LPArray)] DM_VARKEY[] varkey, UInt32 dwItems,
          [MarshalAs(UnmanagedType.LPArray)] VARIANT[] value, [Out, MarshalAs(UnmanagedType.LPArray)] UInt32[] pdwVarState,
          [Out] out CMN_ERROR pError);

        [DllImport("dmclient.dll")]
        private static extern Boolean DMEnumOpenedProjects([Out] out UInt32 pdwItems, DM_ENUM_OPENED_PROJECTS_PROC fpEnumProc,
          out String strProjectFile, [Out] out CMN_ERROR pError);

        [DllImport("dmclient.dll")]
        private static extern Boolean DMGetVarType(String pszProjectFile, [MarshalAs(UnmanagedType.LPArray)] DM_VARKEY[] pVarkey,
          UInt32 dwItems, [Out, MarshalAs(UnmanagedType.LPArray)] DM_TYPEREF[] pTyperef, [Out] out CMN_ERROR pError);

        [DllImport("dmclient.dll")]
        private static extern Boolean DMGetValueWait([Out] out UInt32 pdwTAID, [MarshalAs(UnmanagedType.LPArray)] DM_VARKEY[] varkey,
          UInt32 dwItems, Int32 fWaitForCompletion, UInt32 dwTimeout, DM_NOTIFY_VARIABLE_PROC fpNotifyProc, ref Object objlist,
          [Out] out CMN_ERROR pError);

        #endregion

        #region

        private String m_strProject;      // complete wincc projectname
        private CMN_ERROR m_Error;        // global error struct for all function calls 

        //private Boolean m_fIsConnected;   // connectionstate to WinCC V7 ODK

        private DM_NOTIFY_PROC m_fpMyNotifyProc;                        // functionpointer or delegate
        private DM_ENUM_VAR_PROC m_fpMyEnumVarProc;                     // for ODK Callbacks
        private DM_ENUM_OPENED_PROJECTS_PROC m_fpMyEnumProjectsProc;
        private DM_NOTIFY_VARIABLE_PROC m_fpMyGetVarsProc;
        #endregion membervariables

        #region  construction_Destruction
        /// <summary>
        /// Constructor de clase
        /// </summary>
        public OWinCCDataManager()
        {
            // init error structur
            m_Error = new CMN_ERROR();

            // init ODK Callback delegates 
            m_fpMyNotifyProc = new DM_NOTIFY_PROC(MyNotifyProc);
            m_fpMyEnumVarProc = new DM_ENUM_VAR_PROC(MyEnumVarProc);
            m_fpMyEnumProjectsProc = new DM_ENUM_OPENED_PROJECTS_PROC(MyEnumProjectsProc);
            m_fpMyGetVarsProc = new DM_NOTIFY_VARIABLE_PROC(MyGetVarsProc);
        }
        /// <summary>
        /// Destructor de clase
        /// </summary>
        ~OWinCCDataManager()
        {
            // it is not allowed to call disconnect here
            // this would stop all scripts in WinCC !!
            // it is only allowed to call Disconnect if 
            // the ODK function are called from a seperate application

            //Disconnect();
        }
        #endregion  construction_Destruction

        #region StandardODKCalls
        /// <summary>
        /// Conecta con el servidor WinCC
        /// </summary>
        /// <returns></returns>
        public Boolean Connect()
        {
            // Connect
            if (!DMConnect("DemoCtrlApp", m_fpMyNotifyProc, IntPtr.Zero, out m_Error))
            {
                if (m_Error.dwError1 != DM_E_ALREADY_CONNECTED)
                    return false;
            }
            //m_fIsConnected = true;

            // Get RT-Projekt
            UInt32 dwItems = new UInt32();
            if (!DMEnumOpenedProjects(out dwItems, m_fpMyEnumProjectsProc, out m_strProject, out m_Error))
                return false;

            return true;
        }

        /// <summary>
        /// Obtiene las variables con el filtro indicado
        /// </summary>
        /// <param name="strFilter"></param>
        /// <returns></returns>
        public List<string> GetVariables(string strFilter)
        {
            // set tag filter
            DM_VARFILTER varfilter = new DM_VARFILTER();
            varfilter.dwFlags = DM_VARFILTER_NAME;
            varfilter.pszName = strFilter;

            // create list for alls variables
            List<String> vars = new List<String>();

            // ODK Call 
            Object objvars = vars;
            DMEnumVariables(m_strProject, ref varfilter, m_fpMyEnumVarProc, ref objvars, out m_Error);

            return vars;
        }
        /// <summary>
        /// returns a string with the name of the DATAtyp 
        /// </summary>
        /// <param name="strVar"></param>
        /// <returns></returns>
        public String GetVarType(String strVar)
        {
            // fill DM_VARKEY struct
            DM_VARKEY[] varkeys = new DM_VARKEY[1];
            varkeys[0].dwKeyType = DM_VARKEY_NAME;
            varkeys[0].szName = strVar;
            DM_TYPEREF[] typerefs = new DM_TYPEREF[1];

            // ODK-CALL
            DMGetVarType(m_strProject, varkeys, 1, typerefs, out m_Error);

            // check results
            switch (typerefs[0].dwType)
            {
                case DM_VARTYPE_BIT:
                    return "Boolean 1bit";
                case DM_VARTYPE_SBYTE:
                    return "Signed 8bit";
                case DM_VARTYPE_BYTE:
                    return "Unsigned 8bit";
                case DM_VARTYPE_SWORD:
                    return "Signed 16bit";
                case DM_VARTYPE_WORD:
                    return "Unsigned 16bit";
                case DM_VARTYPE_SDWORD:
                    return "Signed 32bit";
                case DM_VARTYPE_DWORD:
                    return "Unsigned 32bit";
                case DM_VARTYPE_FLOAT:
                    return "Real 32bit";
                case DM_VARTYPE_DOUBLE:
                    return "Real 64bit";
                case DM_VARTYPE_TEXT_8:
                    return "Text 8bit";
                case DM_VARTYPE_TEXT_16:
                    return "Text 16bit";
                default:
                    return "<?>";
            }
        }

        /// <summary>
        /// Get current value and Quality from a WinCC Tag
        /// </summary>
        /// <param name="strVar"></param>
        /// <param name="objValue"></param>
        /// <param name="nQuality"></param>
        /// <returns></returns>
        public Boolean GetVarValue(String strVar, out Object objValue, out UInt32 nQuality)
        {
            // init return values 
            objValue = null;
            nQuality = 0;

            // fill DM_VARKEY struct 
            DM_VARKEY[] varkeys = new DM_VARKEY[1];
            varkeys[0].dwKeyType = DM_VARKEY_NAME;
            varkeys[0].szName = strVar;

            // resultlist for Callback function
            List<DM_VAR_UPDATE_STRUCT> updates = new List<DM_VAR_UPDATE_STRUCT>(1);

            // ODK-CALL
            Object objupdates = updates;
            UInt32 dwTAID = new UInt32();
            if (!DMGetValueWait(out dwTAID, varkeys, 1, 1, 10000, m_fpMyGetVarsProc, ref objupdates, out m_Error))
                return false;

            // convert VARIANT to .NET-Object
            if (!VariantToObject(updates[0].dmValue, ref objValue))
                return false;

            nQuality = updates[0].dwState;
            return true;
        }

        /// <summary>
        /// Write value to wincc tag
        /// </summary>
        /// <param name="strVar"></param>
        /// <param name="objValue"></param>
        /// <returns></returns>
        public Boolean SetVarValue(String strVar, Object objValue)
        {
            // fill DM_VARKEY struct 
            DM_VARKEY[] varkey = new DM_VARKEY[1];
            varkey[0].dwKeyType = DM_VARKEY_NAME;
            varkey[0].szName = strVar;
            VARIANT[] value = new VARIANT[1];
            UInt32[] dwState = new UInt32[1];

            // convert object to VARIANT 
            if (!ObjectToVariant(objValue, ref value[0]))
                return false;

            // ODK CALL
            Boolean fRet = DMSetValue(varkey, 1, value, dwState, out m_Error);

            // if VARIANT contains a BSTR, free the string
            if (value[0].vt == VT_BSTR)
                Marshal.FreeBSTR(value[0].bstrVal);

            return fRet;
        }
        #endregion #region StandardODKCalls

        #region Helperfunctions

        // Converts Object to VARIANT 
        private bool ObjectToVariant(Object obj, ref VARIANT var)
        {
            if (obj.GetType() == typeof(Boolean))
            {
                var.vt = VT_BOOL;
                var.BooleanVal = (Int16)(((Boolean)obj) ? 1 : 0);
            }
            else if (obj.GetType() == typeof(SByte))
            {
                var.vt = VT_I1;
                var.cVal = (SByte)obj;
            }
            else if (obj.GetType() == typeof(Byte))
            {
                var.vt = VT_UI1;
                var.bVal = (Byte)obj;
            }
            else if (obj.GetType() == typeof(Int16))
            {
                var.vt = VT_I2;
                var.iVal = (Int16)obj;
            }
            else if (obj.GetType() == typeof(UInt16))
            {
                var.vt = VT_UI2;
                var.uiVal = (UInt16)obj;
            }
            else if (obj.GetType() == typeof(Int32))
            {
                var.vt = VT_I4;
                var.intVal = (Int32)obj;
            }
            else if (obj.GetType() == typeof(UInt32))
            {
                var.vt = VT_UI4;
                var.uintVal = (UInt32)obj;
            }
            else if (obj.GetType() == typeof(Single))
            {
                var.vt = VT_R4;
                var.fltVal = (Single)obj;
            }
            else if (obj.GetType() == typeof(Double))
            {
                var.vt = VT_R8;
                var.dblVal = (Double)obj;
            }
            else if (obj.GetType() == typeof(String))
            {
                var.vt = VT_BSTR;
                // muss eigentlich irgendwann wieder freigegeben werden...
                var.bstrVal = Marshal.StringToBSTR((String)obj);
            }
            else
                return false;

            return true;
        }

        //  Converts a VARIANT to an Object 
        private bool VariantToObject(VARIANT var, ref Object obj)
        {
            switch (var.vt)
            {
                case VT_BOOL:
                    obj = var.BooleanVal;
                    break;
                case VT_I1:
                    obj = var.cVal;
                    break;
                case VT_UI1:
                    obj = var.bVal;
                    break;
                case VT_I2:
                    obj = var.iVal;
                    break;
                case VT_UI2:
                    obj = var.uiVal;
                    break;
                case VT_I4:
                    obj = var.intVal;
                    break;
                case VT_UI4:
                    obj = var.uintVal;
                    break;
                case VT_R4:
                    obj = var.fltVal;
                    break;
                case VT_R8:
                    obj = var.dblVal;
                    break;
                case VT_BSTR:
                    obj = Marshal.PtrToStringBSTR(var.bstrVal);
                    break;
                default:
                    return false;
            }

            return true;
        }
        #endregion Helperfunctions

        #region ODK-Callbacks

        // is used from DMEnumVariables and stroes all Taginformation in a list
        private static Boolean MyEnumVarProc(ref DM_VARKEY varkey, ref Object objvars)
        {
            List<String> vars = (List<String>)objvars;
            vars.Add(varkey.szName);
            return true;
        }


        // is used from DM Connect 
        private static Boolean MyNotifyProc(UInt32 dwNotifyClass, UInt32 dwNotifyCode, IntPtr pData, UInt32 dwItems, IntPtr pUser)
        {
            return true;
        }


        // is used from DMEnumOpenedProjects and stores the porject name;
        private static Boolean MyEnumProjectsProc(ref DM_PROJECT_INFO info, ref String strProject)
        {
            strProject = info.szProjectFile;
            return true;
        }


        // is used from DMGetValueWait and stores all reseived DM_VAR_UPDATE_STRUCT's in one list
        private static Boolean MyGetVarsProc(UInt32 dwTAID, DM_VAR_UPDATE_STRUCT[] updates, UInt32 dwItems, ref Object objlist)
        {
            List<DM_VAR_UPDATE_STRUCT> list = (List<DM_VAR_UPDATE_STRUCT>)objlist;
            for (UInt32 i = 0; i < dwItems; i++)
                list.Add(updates[i]);
            return true;
        }
        #endregion ODK-Callbacks

    }
}
