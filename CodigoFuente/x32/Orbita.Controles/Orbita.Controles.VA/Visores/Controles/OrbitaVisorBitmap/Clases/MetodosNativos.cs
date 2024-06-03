//***********************************************************************
// Assembly         : Orbita.Controles.VA
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
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Orbita.Controles.VA
{
    internal partial class MetodosNativos
    {
        #region Constante(s)

        public const int GWL_STYLE = (-16);
        public const int SB_BOTH = 3;
        public const int SB_BOTTOM = 7;
        public const int SB_CTL = 2;
        public const int SB_ENDSCROLL = 8;
        public const int SB_HORZ = 0;
        public const int SB_LEFT = 6;
        public const int SB_LINEDOWN = 1;
        public const int SB_LINELEFT = 0;
        public const int SB_LINERIGHT = 1;
        public const int SB_LINEUP = 0;
        public const int SB_PAGEDOWN = 3;
        public const int SB_PAGELEFT = 2;
        public const int SB_PAGERIGHT = 3;
        public const int SB_PAGEUP = 2;
        public const int SB_RIGHT = 7;
        public const int SB_THUMBPOSITION = 4;
        public const int SB_THUMBTRACK = 5;
        public const int SB_TOP = 6;
        public const int SB_VERT = 1;
        public const int WM_HSCROLL = 0x00000114;
        public const int WM_VSCROLL = 0x00000115;
        public const int WS_BORDER = 0x00800000;
        public const int WS_EX_CLIENTEDGE = 0x200;
        public const int WS_HSCROLL = 0x00100000;

        public const int WS_VSCROLL = 0x00200000;

        #endregion

        #region Constructor(es)

        private MetodosNativos()
        { }

        #endregion

        #region Enumerado(s)

        [Flags]
        public enum SIF
        {
            SIF_RANGE = 0x0001,
            SIF_PAGE = 0x0002,
            SIF_POS = 0x0004,
            SIF_DISABLENOSCROLL = 0x0008,
            SIF_TRACKPOS = 0x0010,
            SIF_ALL = SIF_PAGE | SIF_POS | SIF_RANGE | SIF_TRACKPOS
        }

        #endregion

        #region Llamadas al Sistema

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetScrollInfo(IntPtr hwnd, int bar, [MarshalAs(UnmanagedType.LPStruct)] SCROLLINFO scrollInfo);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        public static extern int SetScrollInfo(IntPtr hwnd, int bar, [MarshalAs(UnmanagedType.LPStruct)] SCROLLINFO scrollInfo, bool redraw);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int index, UInt32 newLong);

        #endregion

        #region Clase ScrollINFO

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class SCROLLINFO
        {
            public int cbSize;
            public SIF fMask;
            public int nMin;
            public int nMax;
            public int nPage;
            public int nPos;
            public int nTrackPos;

            public SCROLLINFO()
            {
                cbSize = Marshal.SizeOf(this);
                nPage = 0;
                nMin = 0;
                nMax = 0;
                nPos = 0;
                nTrackPos = 0;
                fMask = 0;
            }
        }
        #endregion
    }

    internal class BotonRaton
    {
        #region Atributos

        MouseButtons _boton;
        OpcionClickBotones _opcionClick;
        OpcionMantenerClickBotones _opcionArrastrarClick;

        #endregion

        #region Constructor

        public BotonRaton(MouseButtons _boton, OpcionClickBotones _opcionClick, OpcionMantenerClickBotones _opcionArrastrarClick)
        {
            this._boton = _boton;
            this._opcionClick = _opcionClick;
            this._opcionArrastrarClick = _opcionArrastrarClick;
        }

        #endregion

        #region Propiedades

        public MouseButtons Boton
        {
            get { return _boton; }
        }

        public OpcionClickBotones OpcionClick
        {
            get { return _opcionClick; }
            set { _opcionClick = value; }
        }

        public OpcionMantenerClickBotones OpcionArrastrarClick
        {
            get { return _opcionArrastrarClick; }
            set { _opcionArrastrarClick = value; }
        }

        #endregion

    }

    /// <summary>
    /// Clase coleccion (lista) de zooms
    /// </summary>
    internal class ZoomLevelCollection : List<int>
    {
        #region Constructor(es)
        /// <summary>
        /// Constructor sin parámetros
        /// </summary>
        public ZoomLevelCollection()
        {
            this.List = new SortedList<int, int>();
        }
        /// <summary>
        /// Constructor con parámetros
        /// </summary>
        /// <param name="collection"></param>
        public ZoomLevelCollection(IList<int> collection)
            : this()
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            this.AddRange(collection);
        }

        #endregion

        #region propiedad(es)

        public static ZoomLevelCollection Default
        { get { return new ZoomLevelCollection(new int[] { 7, 10, 15, 20, 25, 30, 50, 70, 100, 150, 200, 300, 400, 500, 600, 700, 800, 1200, 1600 }); } }

        public new int Count
        { get { return this.List.Count; } }

        public bool IsReadOnly
        { get { return false; } }

        protected SortedList<int, int> List { get; set; }

        public new int this[int index]
        {
            get
            {
                int resultado = 0;
                try
                {
                    resultado = this.List.Values[index];
                }
                catch { }
                return resultado;
            }
            set
            {
                this.List.RemoveAt(index);
                this.Add(value);
            }
        }

        #endregion

        #region Método(s) público(s)

        public new void Add(int item)
        {
            this.List.Add(item, item);
        }

        public void AddRange(IList<int> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            foreach (int value in collection)
                this.Add(value);
        }

        public new void Clear()
        {
            this.List.Clear();
        }

        public new bool Contains(int item)
        {
            return this.List.ContainsKey(item);
        }

        public new IEnumerator<int> GetEnumerator()
        {
            return this.List.Values.GetEnumerator();
        }

        public new int IndexOf(int item)
        {
            return this.List.IndexOfKey(item);
        }

        public new void Insert(int index, int item)
        {
            throw new NotImplementedException();
        }

        public int NextZoom(int zoomLevel)
        {
            int index;

            index = this.IndexOf(zoomLevel);

            if (index == -1)
            {
                index = this.IndexOf(BuscarMayor(zoomLevel));
            }
            else
            {
                if (index < this.Count - 1)
                    index++;
            }

            return this[index];
        }

        public int PreviousZoom(int zoomLevel)
        {
            int index;

            index = this.IndexOf(zoomLevel);

            if (index == -1)
            {
                index = this.IndexOf(BuscarMenor(zoomLevel));
            }
            else
            {
                if (index > 0)
                    index--;
            }
            return this[index];
        }

        public int BuscarMenor(int zoomLevel)
        {
            int res = this.Count - 1;

            for (int i = this.Count - 1; i > 0; i--)
            {
                //if (this[i] < zoomLevel)
                //{
                //    res = this[i];
                //    break;
                //}
                if (this[i] < res)
                {
                    res = this[i];
                }
                if (this[i] < zoomLevel)
                {
                    res = this[i];
                    break;
                }
            }
            return res;
        }

        public int BuscarMayor(int zoomLevel)
        {
            int res = 0;

            foreach (int i in this)
            {
                //if (this[i] > zoomLevel)
                //{
                //    res = this[i];
                //    break;
                //}
                if (i > res)
                {
                    res = i;
                }
                if (i > zoomLevel)
                {
                    res = i;
                    break;
                }
            }
            return res;
        }

        public new bool Remove(int item)
        {
            return this.List.Remove(item);
        }

        public new void RemoveAt(int index)
        {
            this.List.RemoveAt(index);
        }

        #endregion
    }

    public enum VisorImagenesBorderStyle
    {
        None,
        FixedSingle,
        FixedSingleDropShadow
    }

    public enum VisorImagenesGridDisplayMode
    {
        None,
        Client,
        Image
    }

    public enum VisorImagenesGridScale
    {
        Small,
        Medium,
        Large
    }

    internal enum VisorImagenesSelectionMode
    {
        None,
        Rectangle,
        Zoom
    }

    public enum OpcionClickBotones
    {
        ZoomMas,
        ZoomMenos,
        AutoCenter,
        Nada
    }

    public enum OpcionMantenerClickBotones
    {
        Pan,
        Rectangulo,
        Nada
    }

}
