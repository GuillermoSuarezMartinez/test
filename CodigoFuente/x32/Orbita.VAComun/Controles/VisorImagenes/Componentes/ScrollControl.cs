//***********************************************************************
// Assembly         : Orbita.VAComun
// Author           : aibañez
// Created          : 05-11-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Orbita.VAComun
{
    [ToolboxItem (false )]
    internal partial class ScrollControl : Control
    {
        #region Atributo(s)

        bool _mostrarHScroll;
        bool _mostrarVScroll;
        BorderStyle _borderStyle;
        HScrollProperties _HorizontalScroll;
        Size _pageSize;
        Size _scrollSize;
        Size _stepSize;

        VScrollProperties _VerticalScroll;

        #endregion

        #region Constructor(es)
        /// <summary>
        /// Constructor del componente
        /// </summary>
        public ScrollControl():base()
        {
            this.BorderStyle = BorderStyle.Fixed3D;
            this.ScrollSize = Size.Empty;
            this.PageSize = Size.Empty;
            this.StepSize = new Size(10, 10);
            this.HorizontalScroll = new HScrollProperties(this);
            this.VerticalScroll = new VScrollProperties(this);
        }

        #endregion

        #region Definición Evento(s)
        /// <summary>
        /// Ocurre cuando el valor de la propiedad BorderStyle cambia
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler BorderStyleChanged;
        /// <summary>
        /// Ocurre cuando el valor de la propiedad PageSize cambia
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler PageSizeChanged;
        /// <summary>
        /// Ocurre cuando se realiza scroll sobre el area cliente
        /// </summary>
        public event ScrollEventHandler Scroll;
        /// <summary>
        /// Ocurre cuando el valor de la propiedad ScrollSize cambia
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler ScrollSizeChanged;
        /// <summary>
        /// Ocurre cuando el valor de la propiedad StepSize cambia
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler StepSizeChanged;
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Propiedad para marcar si el scroll horizontal se ha de mostrar siempre o no
        /// </summary>
        /// <value><c>true</c> si se ha de mostrar el scroll; si no <c>false</c>.</value>
        [Category("Layout"), DefaultValue(false)]
        public bool MostrarSiempreHScroll
        {
            get { return _mostrarHScroll; }
            set
            {
                if (_mostrarHScroll != value)
                {
                    _mostrarHScroll = value;

                    if (value)
                    {
                        MetodosNativos.SCROLLINFO scrollInfo;

                        scrollInfo = new MetodosNativos.SCROLLINFO();
                        scrollInfo.fMask = MetodosNativos.SIF.SIF_RANGE | MetodosNativos.SIF.SIF_DISABLENOSCROLL;
                        scrollInfo.nMin = 0;
                        scrollInfo.nMax = 0;
                        scrollInfo.nPage = 1;
                        this.SetScrollInfo(ScrollOrientation.HorizontalScroll, scrollInfo, false);

                        this.Invalidate();
                    }
                    else
                        this.UpdateHorizontalScrollbar();
                }
            }
        }
        /// <summary>
        /// Propiedad para marcar si el scroll vertical se ha de mostrar siempre o no
        /// </summary>
        /// <value><c>true</c> si se ha de mostrar el scroll; si no <c>false</c>.</value>
        [Category("Layout"), DefaultValue(false)]
        public bool MostrarSiempreVScroll
        {
            get { return _mostrarVScroll; }
            set
            {
                bool shown = VScroll;

                _mostrarVScroll = value;
                if (_mostrarVScroll != shown)
                {
                    if (_mostrarVScroll)
                    {
                        MetodosNativos.SCROLLINFO scrollInfo;

                        scrollInfo = new MetodosNativos.SCROLLINFO();

                        scrollInfo.fMask = MetodosNativos.SIF.SIF_RANGE | MetodosNativos.SIF.SIF_DISABLENOSCROLL;
                        scrollInfo.nMin = 0;
                        scrollInfo.nMax = 0;
                        scrollInfo.nPage = 1;
                        SetScrollInfo(ScrollOrientation.VerticalScroll, scrollInfo, false);

                        this.Invalidate();
                    }
                    else
                    {
                        this.UpdateVerticalScrollbar();
                    }
                }
            }
        }
        /// <summary>
        /// Tipo de Borde del control
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof(BorderStyle), "Fixed3D")]
        public virtual BorderStyle BorderStyle
        {
            get { return _borderStyle; }
            set
            {
                if (this.BorderStyle != value)
                {
                    _borderStyle = value;

                    this.OnBorderStyleChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Obtiene las propiedades del scrollbar horizontal
        /// </summary>
        /// <value>Propiedades del scrollbar horizontal.</value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public HScrollProperties HorizontalScroll
        {
            get { return _HorizontalScroll; }
            protected set { _HorizontalScroll = value; }
        }
        /// <summary>
        /// Obtiene o asigna tamaño de la página de scroll
        /// </summary>
        /// <value>tamaño de la página.</value>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Size PageSize
        {
            get { return _pageSize; }
            set
            {
                if (value.Width < 0)
                    throw new ArgumentOutOfRangeException("valor", "Width debe ser un número positivo.");
                else if (value.Height < 0)
                    throw new ArgumentOutOfRangeException("valor", "Height debe ser un número positivo.");

                if (this.PageSize != value)
                {
                    _pageSize = value;

                    this.OnPageSizeChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Obtiene o asigna el tamaño del área de scroll
        /// </summary>
        /// <value>Tamaño del scroll.</value>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Size ScrollSize
        {
            get { return _scrollSize; }
            set
            {
                if (value.Width < 0)
                    throw new ArgumentOutOfRangeException("valor", "Width debe ser un número positivo.");
                else if (value.Height < 0)
                    throw new ArgumentOutOfRangeException("valor", "Height debe ser un número positivo.");

                if (this.ScrollSize != value)
                {
                    _scrollSize = value;

                    this.OnScrollSizeChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Obtiene o asigna el tamaño del scrollbar stepping.
        /// </summary>
        /// <value>Tamaño del step.</value>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        [Category("Layout"), DefaultValue(typeof(Size), "10, 10")]
        public virtual Size StepSize
        {
            get { return _stepSize; }
            set
            {
                if (value.Width < 0)
                    throw new ArgumentOutOfRangeException("value", "Width debe ser un número positivo.");
                else if (value.Height < 0)
                    throw new ArgumentOutOfRangeException("value", "Height debe ser un número positivo.");

                if (this.StepSize != value)
                {
                    _stepSize = value;

                    this.OnStepSizeChanged(EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// Obtiene las propiedades del scrollbar vertical.
        /// </summary>
        /// <value>Propiedades del scrollbar vertical.</value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public VScrollProperties VerticalScroll
        {
            get { return _VerticalScroll; }
            protected set { _VerticalScroll = value; }
        }
        /// <summary>
        /// Gets the required creation parameters when the control handle is created.
        /// </summary>
        /// <value>The create params.</value>
        /// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams;

                createParams = base.CreateParams;

                switch (_borderStyle)
                {
                    case BorderStyle.FixedSingle:
                        createParams.Style |= MetodosNativos.WS_BORDER;
                        break;

                    case BorderStyle.Fixed3D:
                        createParams.ExStyle |= MetodosNativos.WS_EX_CLIENTEDGE;
                        break;
                }

                return createParams;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether the horizontal scrollbar is displayed
        /// </summary>
        /// <value><c>true</c> if the horizontal scrollbar is displayed; otherwise, <c>false</c>.</value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        protected bool HScroll
        {
            get { return (MetodosNativos.GetWindowLong(this.Handle, MetodosNativos.GWL_STYLE) & MetodosNativos.WS_HSCROLL) == MetodosNativos.WS_HSCROLL; }
            set
            {
                uint longValue = MetodosNativos.GetWindowLong(this.Handle, MetodosNativos.GWL_STYLE);

                if (value)
                {
                    longValue |= MetodosNativos.WS_HSCROLL;
                }
                else
                {
                    unchecked
                    {
                        longValue &= (uint)~MetodosNativos.WS_HSCROLL;
                    }
                }
                MetodosNativos.SetWindowLong(this.Handle, MetodosNativos.GWL_STYLE, longValue);
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether the vertical scrollbar is displayed
        /// </summary>
        /// <value><c>true</c> if the vertical scrollbar is displayed; otherwise, <c>false</c>.</value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        protected bool VScroll
        {
            get { return (MetodosNativos.GetWindowLong(this.Handle, MetodosNativos.GWL_STYLE) & MetodosNativos.WS_VSCROLL) == MetodosNativos.WS_VSCROLL; }
            set
            {
                uint longValue = MetodosNativos.GetWindowLong(this.Handle, MetodosNativos.GWL_STYLE);

                if (value)
                {
                    longValue |= MetodosNativos.WS_VSCROLL;
                }
                else
                {
                    unchecked
                    {
                        longValue &= (uint)~MetodosNativos.WS_VSCROLL;
                    }
                }
                MetodosNativos.SetWindowLong(this.Handle, MetodosNativos.GWL_STYLE, longValue);
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether the control is scrolled when the mouse wheel is spun
        /// </summary>
        /// <value><c>true</c> if the mouse wheel scrolls the control; otherwise, <c>false</c>.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected bool WheelScrollsControl { get; set; }
        /// <summary>
        /// Scrolls both scrollbars to the given values
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void ScrollTo(int x, int y)
        {
            this.ScrollTo(ScrollOrientation.HorizontalScroll, x);
            this.ScrollTo(ScrollOrientation.VerticalScroll, y);
        }
        /// <summary>
        /// Gets the type of scrollbar event.
        /// </summary>
        /// <param name="wparam">The wparam value from a window proc.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        protected ScrollEventType GetEventType(IntPtr wparam)
        {
            switch (wparam.ToInt32() & 0xFFFF)
            {
                case MetodosNativos.SB_BOTTOM:
                    return ScrollEventType.Last;
                case MetodosNativos.SB_ENDSCROLL:
                    return ScrollEventType.EndScroll;
                case MetodosNativos.SB_LINEDOWN:
                    return ScrollEventType.SmallIncrement;
                case MetodosNativos.SB_LINEUP:
                    return ScrollEventType.SmallDecrement;
                case MetodosNativos.SB_PAGEDOWN:
                    return ScrollEventType.LargeIncrement;
                case MetodosNativos.SB_PAGEUP:
                    return ScrollEventType.LargeDecrement;
                case MetodosNativos.SB_THUMBPOSITION:
                    return ScrollEventType.ThumbPosition;
                case MetodosNativos.SB_THUMBTRACK:
                    return ScrollEventType.ThumbTrack;
                case MetodosNativos.SB_TOP:
                    return ScrollEventType.First;
                default:
                    throw new ArgumentException(string.Format("{0} isn't a valid scroll event type.", wparam), "wparam");
            }
        }
        /// <summary>
        /// Raises the <see cref="E:BorderStyleChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnBorderStyleChanged(EventArgs e)
        {
            EventHandler handler;

            base.UpdateStyles();

            handler = this.BorderStyleChanged;

            if (handler != null)
                handler(this, e);
        }
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            this.UpdateScrollbars();
        }
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (!this.Focused)
                this.Focus();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseWheel" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (this.WheelScrollsControl)
            {
                int x;
                int y;
                int delta;

                x = this.HorizontalScroll.Value;
                y = this.VerticalScroll.Value;

                // TODO: Find if we are hovering over a horizontal scrollbar and scroll that instead of the default vertical.
                if (this.VerticalScroll.Visible && this.VerticalScroll.Enabled)
                {
                    if (Control.ModifierKeys == Keys.Control)
                        delta = this.VerticalScroll.LargeChange;
                    else
                        delta = SystemInformation.MouseWheelScrollLines * this.VerticalScroll.SmallChange;

                    y += (e.Delta > 0) ? -delta : delta;
                }
                else if (this.HorizontalScroll.Visible && this.HorizontalScroll.Enabled)
                {
                    if (Control.ModifierKeys == Keys.Control)
                        delta = this.HorizontalScroll.LargeChange;
                    else
                        delta = SystemInformation.MouseWheelScrollLines * this.HorizontalScroll.SmallChange;

                    x += (e.Delta > 0) ? -delta : delta;
                }

                this.ScrollTo(x, y);
            }

            base.OnMouseWheel(e);
        }

        /// <summary>
        /// Raises the <see cref="E:PageSizeChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnPageSizeChanged(EventArgs e)
        {
            EventHandler handler;

            this.UpdateScrollbars();

            handler = this.PageSizeChanged;

            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:Scroll" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ScrollEventArgs" /> instance containing the event data.</param>
        protected virtual void OnScroll(ScrollEventArgs e)
        {
            ScrollEventHandler handler;

            this.UpdateHorizontalScroll();
            this.UpdateVerticalScroll();

            handler = this.Scroll;

            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:ScrollSizeChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnScrollSizeChanged(EventArgs e)
        {
            EventHandler handler;

            this.UpdateScrollbars();

            handler = this.ScrollSizeChanged;

            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:StepSizeChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnStepSizeChanged(EventArgs e)
        {
            EventHandler handler;

            handler = this.StepSizeChanged;

            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Set the given scrollbar's tracking position to the specified value
        /// </summary>
        /// <param name="scrollbar">The scrollbar.</param>
        /// <param name="value">The value.</param>
        protected virtual void ScrollTo(ScrollOrientation scrollbar, int value)
        {
            MetodosNativos.SCROLLINFO oldInfo;

            oldInfo = this.GetScrollInfo(scrollbar);

            if (value > ((oldInfo.nMax - oldInfo.nMin) + 1) - (int)oldInfo.nPage)
                value = ((oldInfo.nMax - oldInfo.nMin) + 1) - (int)oldInfo.nPage;
            if (value < oldInfo.nMin)
                value = oldInfo.nMin;

            if (oldInfo.nPos != value)
            {
                MetodosNativos.SCROLLINFO scrollInfo;

                scrollInfo = new MetodosNativos.SCROLLINFO();
                scrollInfo.fMask = MetodosNativos.SIF.SIF_POS;
                scrollInfo.nPos = value;
                SetScrollInfo(scrollbar, scrollInfo, true);

                this.OnScroll(new ScrollEventArgs(ScrollEventType.ThumbPosition, oldInfo.nPos, value, (ScrollOrientation)scrollbar));
            }
        }

        /// <summary>
        /// Updates the properties of the horizontal scrollbar.
        /// </summary>
        protected virtual void UpdateHorizontalScroll()
        {
            MetodosNativos.SCROLLINFO scrollInfo;

            scrollInfo = this.GetScrollInfo(ScrollOrientation.HorizontalScroll);

            this.HorizontalScroll.Enabled = this.Enabled;
            this.HorizontalScroll.LargeChange = scrollInfo.nPage;
            this.HorizontalScroll.Maximum = scrollInfo.nMax;
            this.HorizontalScroll.Minimum = scrollInfo.nMin;
            this.HorizontalScroll.SmallChange = this.StepSize.Width;
            this.HorizontalScroll.Value = scrollInfo.nPos;
            this.HorizontalScroll.Visible = true;
        }

        protected virtual void UpdateHorizontalScrollbar()
        {
            MetodosNativos.SCROLLINFO scrollInfo;
            int scrollWidth;
            int pageWidth;

            scrollWidth = this.ScrollSize.Width - 1;
            pageWidth = this.PageSize.Width;

            if (scrollWidth < 1)
            {
                scrollWidth = 0;
                pageWidth = 1;
            }

            scrollInfo = new MetodosNativos.SCROLLINFO();
            scrollInfo.fMask = MetodosNativos.SIF.SIF_PAGE | MetodosNativos.SIF.SIF_RANGE;
            if (this.MostrarSiempreHScroll || !this.Enabled)
                scrollInfo.fMask |= MetodosNativos.SIF.SIF_DISABLENOSCROLL;
            scrollInfo.nMin = 0;
            scrollInfo.nMax = scrollWidth;
            scrollInfo.nPage = pageWidth;

            this.SetScrollInfo(ScrollOrientation.HorizontalScroll, scrollInfo, true);
        }

        /// <summary>
        /// Updates the scrollbars.
        /// </summary>
        protected void UpdateScrollbars()
        {
            this.UpdateHorizontalScrollbar();
            this.UpdateVerticalScrollbar();
        }

        /// <summary>
        /// Updates the properties of the vertical scrollbar.
        /// </summary>
        protected virtual void UpdateVerticalScroll()
        {
            VScrollProperties VerticalScrollProperties = new VScrollProperties(this);

            MetodosNativos.SCROLLINFO scrollInfo;

            scrollInfo = this.GetScrollInfo(ScrollOrientation.VerticalScroll);

            this.VerticalScroll.Enabled = this.Enabled;
            this.VerticalScroll.LargeChange = scrollInfo.nPage;
            this.VerticalScroll.Maximum = scrollInfo.nMax;
            this.VerticalScroll.Minimum = scrollInfo.nMin;
            this.VerticalScroll.SmallChange = this.StepSize.Height;
            this.VerticalScroll.Value = scrollInfo.nPos;
            this.VerticalScroll.Visible = true;
        }

        protected virtual void UpdateVerticalScrollbar()
        {
            MetodosNativos.SCROLLINFO scrollInfo;
            int scrollHeight;
            int pageHeight;

            scrollHeight = this.ScrollSize.Height - 1;
            pageHeight = this.PageSize.Height;

            if (scrollHeight < 1)
            {
                scrollHeight = 0;
                pageHeight = 1;
            }

            scrollInfo = new MetodosNativos.SCROLLINFO();
            scrollInfo.fMask = MetodosNativos.SIF.SIF_PAGE | MetodosNativos.SIF.SIF_RANGE;
            if (MostrarSiempreVScroll)
                scrollInfo.fMask |= MetodosNativos.SIF.SIF_DISABLENOSCROLL;
            scrollInfo.nMin = 0;
            scrollInfo.nMax = scrollHeight;
            scrollInfo.nPage = pageHeight;

            this.SetScrollInfo(ScrollOrientation.VerticalScroll, scrollInfo, true);
        }

        /// <summary>
        /// Processes the WM_HSCROLL and WM_VSCROLL Windows messages.
        /// </summary>
        /// <param name="msg">The Windows <see cref="T:System.Windows.Forms.Message"/> to process.</param>
        protected virtual void WmScroll(ref Message msg)
        {
            ScrollOrientation scrollbar;
            int oldValue;
            int newValue;
            ScrollEventType eventType;

            eventType = this.GetEventType(msg.WParam);
            if (msg.Msg == MetodosNativos.WM_HSCROLL)
                scrollbar = ScrollOrientation.HorizontalScroll;
            else
                scrollbar = ScrollOrientation.VerticalScroll;

            if (eventType != ScrollEventType.EndScroll)
            {
                int step;
                MetodosNativos.SCROLLINFO scrollInfo;

                if (scrollbar == ScrollOrientation.HorizontalScroll)
                    step = this.StepSize.Width;
                else
                    step = this.StepSize.Height;

                scrollInfo = this.GetScrollInfo(scrollbar);
                scrollInfo.fMask = MetodosNativos.SIF.SIF_POS;
                oldValue = scrollInfo.nPos;

                switch (eventType)
                {
                    case ScrollEventType.ThumbPosition:
                    case ScrollEventType.ThumbTrack:
                        scrollInfo.nPos = scrollInfo.nTrackPos;
                        break;

                    case ScrollEventType.SmallDecrement:
                        scrollInfo.nPos = oldValue - step;
                        break;

                    case ScrollEventType.SmallIncrement:
                        scrollInfo.nPos = oldValue + step;
                        break;

                    case ScrollEventType.LargeDecrement:
                        scrollInfo.nPos = oldValue - (int)scrollInfo.nPage;
                        break;

                    case ScrollEventType.LargeIncrement:
                        scrollInfo.nPos = oldValue + (int)scrollInfo.nPage;
                        break;

                    case ScrollEventType.First:
                        scrollInfo.nPos = scrollInfo.nMin;
                        break;

                    case ScrollEventType.Last:
                        scrollInfo.nPos = scrollInfo.nMax;
                        break;
                    default:
                        Debug.Assert(false, string.Format("Unknown scroll event type {0}", eventType));
                        break;
                }

                if (scrollInfo.nPos > ((scrollInfo.nMax - scrollInfo.nMin) + 1) - scrollInfo.nPage)
                    scrollInfo.nPos = ((scrollInfo.nMax - scrollInfo.nMin) + 1) - scrollInfo.nPage;

                if (scrollInfo.nPos < scrollInfo.nMin)
                    scrollInfo.nPos = scrollInfo.nMin;

                newValue = scrollInfo.nPos;
                this.SetScrollInfo(scrollbar, scrollInfo, true);
            }
            else
            {
                oldValue = 0;
                newValue = 0;
            }

            this.OnScroll(new ScrollEventArgs(eventType, oldValue, newValue, scrollbar));
        }

        /// <summary>
        /// Processes Windows messages.
        /// </summary>
        /// <param name="msg">The Windows <see cref="T:System.Windows.Forms.Message"/> to process.</param>
        [DebuggerStepThrough]
        protected override void WndProc(ref Message msg)
        {
            switch (msg.Msg)
            {
                case MetodosNativos.WM_HSCROLL:
                case MetodosNativos.WM_VSCROLL:
                    this.WmScroll(ref msg);
                    break;
                default:
                    base.WndProc(ref msg);
                    break;
            }
        }

        /// <summary>
        /// Gets scrollbar properties
        /// </summary>
        /// <param name="scrollbar">The bar.</param>
        /// <returns></returns>
        private MetodosNativos.SCROLLINFO GetScrollInfo(ScrollOrientation scrollbar)
        {
            MetodosNativos.SCROLLINFO info;

            info = new MetodosNativos.SCROLLINFO();
            info.fMask = MetodosNativos.SIF.SIF_ALL;

            MetodosNativos.GetScrollInfo(this.Handle, (int)scrollbar, info);

            return info;
        }

        /// <summary>
        /// Sets scrollbar properties.
        /// </summary>
        /// <param name="scrollbar">The scrollbar.</param>
        /// <param name="scrollInfo">The scrollbar properties.</param>
        /// <param name="refresh">if set to <c>true</c> the scrollbar will be repainted.</param>
        /// <returns></returns>
        private int SetScrollInfo(ScrollOrientation scrollbar, MetodosNativos.SCROLLINFO scrollInfo, bool refresh)
        {
            return MetodosNativos.SetScrollInfo(this.Handle, (int)scrollbar, scrollInfo, refresh);
        }

        #endregion
    }
}
