//***********************************************************************
// Assembly         : Orbita.VAHardware
// Author           : aibañez
// Created          : 30-11-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Runtime.InteropServices;

namespace Orbita.VAHardware
{
    /// <summary>
    /// Windows API functions and structures
    /// </summary>
    [ComVisible(false)]
    internal class Win32Utils
    {
        #region Método(s) estático(s)
        /// <summary>
        /// Version of AVISaveOptions for one stream only
        ///
        /// I don't find a way to interop AVISaveOptions more likely, so the
        /// usage of original version is not easy. The version makes it more
        /// usefull.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="opts"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static int AVISaveOptions(IntPtr stream, ref AVICOMPRESSOPTIONS opts, IntPtr owner)
        {
            IntPtr[] streams = new IntPtr[1];
            IntPtr[] infPtrs = new IntPtr[1];
            IntPtr mem;
            int ret;

            // alloc unmanaged memory
            mem = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(AVICOMPRESSOPTIONS)));

            // copy from managed structure to unmanaged memory
            Marshal.StructureToPtr(opts, mem, false);

            streams[0] = stream;
            infPtrs[0] = mem;

            // show dialog with a list of available compresors and configuration
            ret = AVISaveOptions(IntPtr.Zero, 0, 1, streams, infPtrs);

            // copy from unmanaged memory to managed structure
            opts = (AVICOMPRESSOPTIONS)Marshal.PtrToStructure(mem, typeof(AVICOMPRESSOPTIONS));

            // free AVI compression options
            AVISaveOptionsFree(1, infPtrs);

            // clear it, because the information already freed by AVISaveOptionsFree
            opts.cbFormat = 0;
            opts.cbParms = 0;
            opts.lpFormat = 0;
            opts.lpParms = 0;

            // free unmanaged memory
            Marshal.FreeHGlobal(mem);

            return ret;
        }
        #endregion

        #region Estructuras
        /// <summary>
        /// Define the coordinates of the upper-left and
        /// lower-right corners of a rectangle
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RECT
        {
            [MarshalAs(UnmanagedType.I4)]
            public int left;
            [MarshalAs(UnmanagedType.I4)]
            public int top;
            [MarshalAs(UnmanagedType.I4)]
            public int right;
            [MarshalAs(UnmanagedType.I4)]
            public int bottom;
        }

        /// <summary>
        /// Contains information for a single stream
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        public struct AVISTREAMINFO
        {
            [MarshalAs(UnmanagedType.I4)]
            public int fccType;
            [MarshalAs(UnmanagedType.I4)]
            public int fccHandler;
            [MarshalAs(UnmanagedType.I4)]
            public int dwFlags;
            [MarshalAs(UnmanagedType.I4)]
            public int dwCaps;
            [MarshalAs(UnmanagedType.I2)]
            public short wPriority;
            [MarshalAs(UnmanagedType.I2)]
            public short wLanguage;
            [MarshalAs(UnmanagedType.I4)]
            public int dwScale;
            [MarshalAs(UnmanagedType.I4)]
            public int dwRate;		// dwRate / dwScale == samples/second
            [MarshalAs(UnmanagedType.I4)]
            public int dwStart;
            [MarshalAs(UnmanagedType.I4)]
            public int dwLength;
            [MarshalAs(UnmanagedType.I4)]
            public int dwInitialFrames;
            [MarshalAs(UnmanagedType.I4)]
            public int dwSuggestedBufferSize;
            [MarshalAs(UnmanagedType.I4)]
            public int dwQuality;
            [MarshalAs(UnmanagedType.I4)]
            public int dwSampleSize;
            [MarshalAs(UnmanagedType.Struct, SizeConst = 16)]
            public RECT rcFrame;
            [MarshalAs(UnmanagedType.I4)]
            public int dwEditCount;
            [MarshalAs(UnmanagedType.I4)]
            public int dwFormatChangeCount;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string szName;
        }

        /// <summary>
        /// Contains information about the dimensions and color format of a DIB
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BITMAPINFOHEADER
        {
            [MarshalAs(UnmanagedType.I4)]
            public int biSize;
            [MarshalAs(UnmanagedType.I4)]
            public int biWidth;
            [MarshalAs(UnmanagedType.I4)]
            public int biHeight;
            [MarshalAs(UnmanagedType.I2)]
            public short biPlanes;
            [MarshalAs(UnmanagedType.I2)]
            public short biBitCount;
            [MarshalAs(UnmanagedType.I4)]
            public int biCompression;
            [MarshalAs(UnmanagedType.I4)]
            public int biSizeImage;
            [MarshalAs(UnmanagedType.I4)]
            public int biXPelsPerMeter;
            [MarshalAs(UnmanagedType.I4)]
            public int biYPelsPerMeter;
            [MarshalAs(UnmanagedType.I4)]
            public int biClrUsed;
            [MarshalAs(UnmanagedType.I4)]
            public int biClrImportant;
        }

        /// <summary>
        /// Contains information about a stream and how it is compressed and saved
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct AVICOMPRESSOPTIONS
        {
            [MarshalAs(UnmanagedType.I4)]
            public int fccType;
            [MarshalAs(UnmanagedType.I4)]
            public int fccHandler;
            [MarshalAs(UnmanagedType.I4)]
            public int dwKeyFrameEvery;
            [MarshalAs(UnmanagedType.I4)]
            public int dwQuality;
            [MarshalAs(UnmanagedType.I4)]
            public int dwBytesPerSecond;
            [MarshalAs(UnmanagedType.I4)]
            public int dwFlags;
            [MarshalAs(UnmanagedType.I4)]
            public int lpFormat;
            [MarshalAs(UnmanagedType.I4)]
            public int cbFormat;
            [MarshalAs(UnmanagedType.I4)]
            public int lpParms;
            [MarshalAs(UnmanagedType.I4)]
            public int cbParms;
            [MarshalAs(UnmanagedType.I4)]
            public int dwInterleaveEvery;
        }
        #endregion

        #region Enumerados
        /// <summary>
        /// File access modes
        /// </summary>
        [Flags]
        public enum OpenFileMode
        {
            Read = 0x00000000,
            Write = 0x00000001,
            ReadWrite = 0x00000002,
            ShareCompat = 0x00000000,
            ShareExclusive = 0x00000010,
            ShareDenyWrite = 0x00000020,
            ShareDenyRead = 0x00000030,
            ShareDenyNone = 0x00000040,
            Parse = 0x00000100,
            Delete = 0x00000200,
            Verify = 0x00000400,
            Cancel = 0x00000800,
            Create = 0x00001000,
            Prompt = 0x00002000,
            Exist = 0x00004000,
            Reopen = 0x00008000
        }

        /// <summary>
        /// window styles
        /// </summary>
        [Flags]
        public enum WS
        {
            CHILD = 0x40000000,
            VISIBLE = 0x10000000
        }
        #endregion

        #region Métodos AVI
        /// <summary>
        /// Initialize the AVIFile library
        /// </summary>
        [DllImport("avifil32.dll")]
        public static extern void AVIFileInit();

        /// <summary>
        /// Exit the AVIFile library 
        /// </summary>
        [DllImport("avifil32.dll")]
        public static extern void AVIFileExit();

        /// <summary>
        /// Open an AVI file
        /// </summary>
        /// <param name="ppfile"></param>
        /// <param name="szFile"></param>
        /// <param name="mode"></param>
        /// <param name="pclsidHandler"></param>
        /// <returns></returns>
        [DllImport("avifil32.dll", CharSet = CharSet.Unicode)]
        public static extern int AVIFileOpen(
            out IntPtr ppfile,
            String szFile,
            OpenFileMode mode,
            IntPtr pclsidHandler);

        /// <summary>
        /// Release an open AVI stream
        /// </summary>
        /// <param name="pfile"></param>
        /// <returns></returns>
        [DllImport("avifil32.dll")]
        public static extern int AVIFileRelease(
            IntPtr pfile);

        /// <summary>
        /// Get address of a stream interface that is associated with a specified AVI file
        /// </summary>
        /// <param name="pfile"></param>
        /// <param name="ppavi"></param>
        /// <param name="fccType"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("avifil32.dll")]
        public static extern int AVIFileGetStream(
            IntPtr pfile,
            out IntPtr ppavi,
            int fccType,
            int lParam);

        /// <summary>
        /// Create a new stream in an existing file and creates an interface to the new stream
        /// </summary>
        /// <param name="pfile"></param>
        /// <param name="ppavi"></param>
        /// <param name="psi"></param>
        /// <returns></returns>
        [DllImport("avifil32.dll")]
        public static extern int AVIFileCreateStream(
            IntPtr pfile,
            out IntPtr ppavi,
            ref AVISTREAMINFO psi);

        /// <summary>
        /// Release an open AVI stream
        /// </summary>
        /// <param name="pavi"></param>
        /// <returns></returns>
        [DllImport("avifil32.dll")]
        public static extern int AVIStreamRelease(
            IntPtr pavi);

        /// <summary>
        /// Set the format of a stream at the specified position
        /// </summary>
        /// <param name="pavi"></param>
        /// <param name="lPos"></param>
        /// <param name="lpFormat"></param>
        /// <param name="cbFormat"></param>
        /// <returns></returns>
        [DllImport("avifil32.dll")]
        public static extern int AVIStreamSetFormat(
            IntPtr pavi,
            int lPos,
            ref BITMAPINFOHEADER lpFormat,
            int cbFormat);

        /// <summary>
        /// Get the starting sample number for the stream
        /// </summary>
        /// <param name="pavi"></param>
        /// <returns></returns>
        [DllImport("avifil32.dll")]
        public static extern int AVIStreamStart(
            IntPtr pavi);

        /// <summary>
        /// Get the length of the stream
        /// </summary>
        /// <param name="pavi"></param>
        /// <returns></returns>
        [DllImport("avifil32.dll")]
        public static extern int AVIStreamLength(
            IntPtr pavi);

        /// <summary>
        /// Obtain stream header information
        /// </summary>
        /// <param name="pavi"></param>
        /// <param name="psi"></param>
        /// <param name="lSize"></param>
        /// <returns></returns>
        [DllImport("avifil32.dll", CharSet = CharSet.Unicode)]
        public static extern int AVIStreamInfo(
            IntPtr pavi,
            ref AVISTREAMINFO psi,
            int lSize);

        /// <summary>
        /// Prepare to decompress video frames from the specified video stream
        /// </summary>
        /// <param name="pavi"></param>
        /// <param name="lpbiWanted"></param>
        /// <returns></returns>
        [DllImport("avifil32.dll")]
        public static extern IntPtr AVIStreamGetFrameOpen(
            IntPtr pavi,
            ref BITMAPINFOHEADER lpbiWanted);

        /// <summary>
        /// Prepare to decompress video frames from the specified video stream
        /// </summary>
        /// <param name="pavi"></param>
        /// <param name="lpbiWanted"></param>
        /// <returns></returns>
        [DllImport("avifil32.dll")]
        public static extern IntPtr AVIStreamGetFrameOpen(
            IntPtr pavi,
            int lpbiWanted);

        /// <summary>
        /// Releases resources used to decompress video frames
        /// </summary>
        /// <param name="pget"></param>
        /// <returns></returns>
        [DllImport("avifil32.dll")]
        public static extern int AVIStreamGetFrameClose(
            IntPtr pget);

        /// <summary>
        /// Return the address of a decompressed video frame
        /// </summary>
        /// <param name="pget"></param>
        /// <param name="lPos"></param>
        /// <returns></returns>
        [DllImport("avifil32.dll")]
        public static extern IntPtr AVIStreamGetFrame(
            IntPtr pget,
            int lPos);

        /// <summary>
        /// Write data to a stream
        /// </summary>
        /// <param name="pavi"></param>
        /// <param name="lStart"></param>
        /// <param name="lSamples"></param>
        /// <param name="lpBuffer"></param>
        /// <param name="cbBuffer"></param>
        /// <param name="dwFlags"></param>
        /// <param name="plSampWritten"></param>
        /// <param name="plBytesWritten"></param>
        /// <returns></returns>
        [DllImport("avifil32.dll")]
        public static extern int AVIStreamWrite(
            IntPtr pavi,
            int lStart,
            int lSamples,
            IntPtr lpBuffer,
            int cbBuffer,
            int dwFlags,
            IntPtr plSampWritten,
            IntPtr plBytesWritten);

        /// <summary>
        /// Retrieve the save options for a file and returns them in a buffer
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="flags"></param>
        /// <param name="streams"></param>
        /// <param name="ppavi"></param>
        /// <param name="plpOptions"></param>
        /// <returns></returns>
        [DllImport("avifil32.dll")]
        public static extern int AVISaveOptions(
            IntPtr hwnd,
            int flags,
            int streams,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] IntPtr[] ppavi,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] IntPtr[] plpOptions);

        /// <summary>
        /// Free the resources allocated by the AVISaveOptions function
        /// </summary>
        /// <param name="streams"></param>
        /// <param name="plpOptions"></param>
        /// <returns></returns>
        [DllImport("avifil32.dll")]
        public static extern int AVISaveOptionsFree(
            int streams,
            [In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] IntPtr[] plpOptions);

        /// <summary>
        /// Create a compressed stream from an uncompressed stream and a
        /// compression filter, and returns the address of a pointer to
        /// the compressed stream
        /// </summary>
        /// <param name="ppsCompressed"></param>
        /// <param name="psSource"></param>
        /// <param name="lpOptions"></param>
        /// <param name="pclsidHandler"></param>
        /// <returns></returns>
        [DllImport("avifil32.dll")]
        public static extern int AVIMakeCompressedStream(
            out IntPtr ppsCompressed,
            IntPtr psSource,
            ref AVICOMPRESSOPTIONS lpOptions,
            IntPtr pclsidHandler);
        #endregion

        #region Métodos manejo de memoria
        /// <summary>
        /// memcpy - copy a block of memery
        /// </summary>
        /// <param name="dst"></param>
        /// <param name="src"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [DllImport("ntdll.dll")]
        public static extern IntPtr memcpy(
            IntPtr dst,
            IntPtr src,
            int count);
        /// <summary>
        /// memcpy - copy a block of memery
        /// </summary>
        /// <param name="dst"></param>
        /// <param name="src"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [DllImport("ntdll.dll")]
        public static extern int memcpy(
            int dst,
            int src,
            int count);
        #endregion

        #region Métodos multimedia
        /// <summary>
        /// Supplies a pointer to an implementation of IBindCtx
        /// </summary>
        /// <param name="reserved"></param>
        /// <param name="ppbc"></param>
        /// <returns></returns>
        [DllImport("ole32.dll")]
        public static extern
        int CreateBindCtx(
            int reserved,
            out UCOMIBindCtx ppbc);

        /// <summary>
        /// Converts a string into a moniker that identifies
        /// the object named by the string
        /// </summary>
        /// <param name="pbc"></param>
        /// <param name="szUserName"></param>
        /// <param name="pchEaten"></param>
        /// <param name="ppmk"></param>
        /// <returns></returns>
        [DllImport("ole32.dll", CharSet = CharSet.Unicode)]
        public static extern
        int MkParseDisplayName(
            UCOMIBindCtx pbc,
            string szUserName,
            ref int pchEaten,
            out UCOMIMoniker ppmk);
        #endregion
    }

    /// <summary>
    /// Clase utilizada para la enumración de los tipos de codecs
    /// </summary>
    public class FOURCC
    {
        #region Constante(s)
        public static FOURCC VIDS = new FOURCC("vids");
        public static FOURCC VIDC = new FOURCC("vidc");
        public static FOURCC FRMR = new FOURCC("FrmR");
        public static FOURCC KEYR = new FOURCC("KeyR");
        public static FOURCC AUDS = new FOURCC("auds");
        public static FOURCC AUDC = new FOURCC("audc");
        public static FOURCC MIDS = new FOURCC("mids");
        public static FOURCC TXTS = new FOURCC("txts");
        public static FOURCC DIB  = new FOURCC("DIB "); // Microsoft Device independent Bitmap
        public static FOURCC MRLE = new FOURCC("MRLE"); // Microsoft RLE
        public static FOURCC CVID = new FOURCC("cvid"); // Cinepak Codec by Radius
        public static FOURCC MSVC = new FOURCC("MSVC"); // Microsoft Video 1
        public static FOURCC M261 = new FOURCC("M261"); // Microsoft H.261 Video Codec
        public static FOURCC M263 = new FOURCC("M263"); // Microsoft H.263 Video Codec
        public static FOURCC I420 = new FOURCC("I420"); // Intel 4:2:0 Video Codec (same as M263)
        public static FOURCC IV32 = new FOURCC("IV32"); // Intel Indeo Video Codec 3.2
        public static FOURCC IV41 = new FOURCC("IV41"); // Intel Indeo Video Codec 4.5
        public static FOURCC IV50 = new FOURCC("IV50"); // Intel Indeo Video 5.1
        public static FOURCC IYUV = new FOURCC("IYUV"); // Intel IYUV Codec
        public static FOURCC MPG4 = new FOURCC("MPG4"); // Microsoft MPEG-4 Video Codec V1
        public static FOURCC MP42 = new FOURCC("MP42"); // Microsoft MPEG-4 Video Codec V2
        public static FOURCC DIVX = new FOURCC("DIVX"); // DivX 5.0.4 Codec (must be installed)
        public static FOURCC MJPG = new FOURCC("MJPG"); // Motion JPG
        public static FOURCC WMV1 = new FOURCC("WMV1"); // Windows media video
        public static FOURCC WMV2 = new FOURCC("WMV2"); // Windows media video
        public static FOURCC WMV3 = new FOURCC("WMV3"); // Windows media video
        #endregion

        #region Atributo(s)
        /// <summary>
        /// Valor entero del código
        /// </summary>
        public int IntCode;
        /// <summary>
        /// Cadena de texto del código
        /// </summary>
        public string StrCode;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="str"></param>
        private FOURCC(string str)
        {
            this.StrCode = str;
            this.IntCode = mmioFOURCC(str);
        }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="str"></param>
        private FOURCC(int code)
        {
            this.StrCode = decode_mmioFOURCC(code);
            this.IntCode = code;
        } 
        #endregion

        #region Método(s) estático(s)
        /// <summary>
        /// Replacement of mmioFOURCC macros
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int mmioFOURCC(string str)
        {
            return (
                ((int)(byte)(str[0])) |
                ((int)(byte)(str[1]) << 8) |
                ((int)(byte)(str[2]) << 16) |
                ((int)(byte)(str[3]) << 24));
        }

        /// <summary>
        /// Inverse of mmioFOURCC
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string decode_mmioFOURCC(int code)
        {
            char[] chs = new char[4];

            for (int i = 0; i < 4; i++)
            {
                chs[i] = (char)(byte)((code >> (i << 3)) & 0xFF);
                if (!char.IsLetterOrDigit(chs[i]))
                    chs[i] = ' ';
            }
            return new string(chs);
        } 
        #endregion
    }
}


