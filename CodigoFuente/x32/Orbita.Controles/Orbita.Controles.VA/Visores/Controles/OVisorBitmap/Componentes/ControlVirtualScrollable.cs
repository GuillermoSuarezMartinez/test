﻿//***********************************************************************
// Assembly         : Orbita.Controles.VA
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
using System.Drawing;
using System.Windows.Forms;

namespace Orbita.Controles.VA
{
    [ToolboxItem(false)]
    internal class ControlVirtualScrollable : ScrollControl
    {
        #region Atributo(s)
        private bool _autoScroll;

        private Size _autoScrollMargin;

        private Size _autoScrollMinSize;

        private Point _autoScrollPosition; 
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Occurs when the AutoScroll property value changes
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler AutoScrollChanged;

        /// <summary>
        /// Occurs when the AutoScrollMargin property value changes
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler AutoScrollMarginChanged;

        /// <summary>
        /// Occurs when the AutoScrollMinSize property value changes
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler AutoScrollMinSizeChanged;

        /// <summary>
        /// Occurs when the AutoScrollPosition property value changes
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler AutoScrollPositionChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the container enables the user to scroll to any controls placed outside of its visible boundaries.
        /// </summary>
        /// <value><c>true</c> if the container enables auto-scrolling; otherwise, <c>false</c>.</value>
        [Category("Layout"), DefaultValue(true)]
        public virtual bool AutoScroll
        {
            get { return _autoScroll; }
            set
            {
                if (this.AutoScroll != value)
                {
                    _autoScroll = value;

                    this.OnAutoScrollChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the size of the auto-scroll margin.
        /// </summary>
        /// <value>A <see cref="T:System.Drawing.Size"/> that represents the height and width of the auto-scroll margin in pixels.</value>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        [Category("Layout"), DefaultValue(typeof(Size), "0, 0")]
        public virtual Size AutoScrollMargin
        {
            get { return _autoScrollMargin; }
            set
            {
                if (value.Width < 0)
                    throw new ArgumentOutOfRangeException("value", "Width must be a positive integer.");
                else if (value.Height < 0)
                    throw new ArgumentOutOfRangeException("value", "Height must be a positive integer.");

                if (this.AutoScrollMargin != value)
                {
                    _autoScrollMargin = value;

                    this.OnAutoScrollMarginChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the minimum size of the auto-scroll.
        /// </summary>
        /// <value>A <see cref="T:System.Drawing.Size"/> that determines the minimum size of the virtual area through which the user can scroll.</value>
        [Category("Layout"), DefaultValue(typeof(Size), "0, 0")]
        public virtual Size AutoScrollMinSize
        {
            get { return _autoScrollMinSize; }
            set
            {
                if (this.AutoScrollMinSize != value)
                {
                    _autoScrollMinSize = value;

                    this.OnAutoScrollMinSizeChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the location of the auto-scroll position.
        /// </summary>
        /// <value>A <see cref="T:System.Drawing.Point"/> that represents the auto-scroll position in pixels.</value>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Point AutoScrollPosition
        {
            get { return _autoScrollPosition; }
            set
            {
                Point translated;

                translated = this.AdjustPositionToSize(new Point(-value.X, -value.Y));

                if (this.AutoScrollPosition != translated)
                {
                    this.ScrollByOffset(new Size(_autoScrollPosition.X - translated.X, _autoScrollPosition.Y - translated.Y));
                    _autoScrollPosition = translated;

                    this.OnAutoScrollPositionChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Total area of all visible controls which are scrolled with this container
        ///</summary>
        protected Rectangle ScrollArea
        {
            get
            {
                Rectangle area;

                area = Rectangle.Empty;

                foreach (Control child in Controls)
                {
                    if (child.Visible)
                        area = Rectangle.Union(child.Bounds, area);
                }

                return Rectangle.Union(area, new Rectangle(_autoScrollPosition, _autoScrollMinSize));
            }
        }

        /// <summary>
        /// Gets the view port rectangle.
        /// </summary>
        /// <value>The view port rectangle.</value>
        protected Rectangle ViewPortRectangle
        { get { return new Rectangle(-_autoScrollPosition.X, -_autoScrollPosition.Y, this.DisplayRectangle.Width, this.DisplayRectangle.Height); } } 
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ControlVirtualScrollable()
        {
            this.AutoScrollMargin = Size.Empty;
            this.AutoScrollMinSize = Size.Empty;
            this.AutoScrollPosition = Point.Empty;
            this.AutoScroll = true;

            base.SetStyle(ControlStyles.ContainerControl, true);
        }
        #endregion

        #region Método(s) privado(s)
        /// <summary>
        /// Adjusts the given Point according to the scroll size
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        protected Point AdjustPositionToSize(Point position)
        {
            int x;
            int y;

            x = position.X;
            y = position.Y;

            if (x < -(this.AutoScrollMinSize.Width - this.ClientRectangle.Width))
                x = -(this.AutoScrollMinSize.Width - this.ClientRectangle.Width);
            if (y < -(this.AutoScrollMinSize.Height - this.ClientRectangle.Height))
                y = -(this.AutoScrollMinSize.Height - base.ClientRectangle.Height);
            if (x > 0)
                x = 0;
            if (y > 0)
                y = 0;

            return new Point(x, y);
        }

        /// <summary>
        /// Adjusts the scrollbars.
        /// </summary>
        private void AdjustScrollbars()
        {
            Rectangle clientRectangle;
            Size scrollSize;
            Size pageSize;
            int horzAddition;
            int vertAddition;
            bool horizontalScrollVisible;
            bool verticalScrollVisible;

            clientRectangle = this.ClientRectangle;
            scrollSize = new Size();
            pageSize = new Size();
            horzAddition = 0;
            vertAddition = 0;
            horizontalScrollVisible = false;
            verticalScrollVisible = false;

            if (this.AutoScroll && (this.AutoScrollMinSize.Height > clientRectangle.Height || this.AutoScrollMinSize.Width > clientRectangle.Width))
            {
                int i;

                for (i = 0; i < 2; i++)
                {
                    if (this.AutoScrollMinSize.Width > (clientRectangle.Width - vertAddition))
                    {
                        horizontalScrollVisible = true;
                        horzAddition = SystemInformation.VerticalScrollBarWidth;
                    }

                    if (this.AutoScrollMinSize.Height > (clientRectangle.Height - horzAddition))
                    {
                        verticalScrollVisible = true;
                        vertAddition = SystemInformation.HorizontalScrollBarHeight;
                    }
                }
            }

            if (verticalScrollVisible)
            {
                scrollSize.Height = this.AutoScrollMinSize.Height;
                if (clientRectangle.Height > horzAddition)
                    pageSize.Height = clientRectangle.Height - horzAddition;
            }

            if (horizontalScrollVisible)
            {
                scrollSize.Width = this.AutoScrollMinSize.Width;
                if (clientRectangle.Width > vertAddition)
                    pageSize.Width = clientRectangle.Width - vertAddition;
            }

            this.ScrollSize = scrollSize;
            this.PageSize = pageSize;
        }

        /// <summary>
        /// Scrolls child controls by the given offset
        /// </summary>
        /// <param name="offset">The offset.</param>
        private void ScrollByOffset(Size offset)
        {
            if (!offset.IsEmpty)
            {
                this.SuspendLayout();
                foreach (Control child in Controls)
                    child.Location -= offset;

                _autoScrollPosition = new Point(_autoScrollPosition.X - offset.Width, _autoScrollPosition.Y - offset.Height);
                this.ScrollTo(-_autoScrollPosition.X, -_autoScrollPosition.Y);

                this.ResumeLayout();

                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets the image view port without bars.
        /// </summary>
        /// <returns></returns>
        protected Rectangle GetImageViewPortWithoutBars()
        {
            Rectangle clientRectangle;
            Size scrollSize;
            Size pageSize;
            int horzAddition;
            int vertAddition;

            clientRectangle = this.ClientRectangle;
            scrollSize = new Size();
            pageSize = new Size();
            horzAddition = 0;
            vertAddition = 0;

            int i;

            for (i = 0; i < 2; i++)
            {
                if (this.AutoScrollMinSize.Width > (clientRectangle.Width - vertAddition))
                {
                    horzAddition = SystemInformation.VerticalScrollBarWidth;
                }

                if (this.AutoScrollMinSize.Height > (clientRectangle.Height - horzAddition))
                {
                    vertAddition = SystemInformation.HorizontalScrollBarHeight;
                }
            }

            scrollSize.Height = this.AutoScrollMinSize.Height;
            if (clientRectangle.Height > horzAddition)
                pageSize.Height = clientRectangle.Height + horzAddition;

            scrollSize.Width = this.AutoScrollMinSize.Width;
            if (clientRectangle.Width > vertAddition)
                pageSize.Width = clientRectangle.Width + vertAddition;

            return new Rectangle(0, 0, pageSize.Width, pageSize.Height);
        }
        #endregion

        #region Método(s) virtual(es)
        /// <summary>
        /// Raises the <see cref="E:AutoScrollChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnAutoScrollChanged(EventArgs e)
        {
            EventHandler handler;

            handler = this.AutoScrollChanged;

            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:AutoScrollMarginChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnAutoScrollMarginChanged(EventArgs e)
        {
            EventHandler handler;

            handler = this.AutoScrollMarginChanged;

            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:AutoScrollMinSizeChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnAutoScrollMinSizeChanged(EventArgs e)
        {
            EventHandler handler;

            this.AutoScrollPosition = this.AdjustPositionToSize(this.AutoScrollPosition);
            this.AdjustScrollbars();

            handler = this.AutoScrollMinSizeChanged;

            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:AutoScrollPositionChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnAutoScrollPositionChanged(EventArgs e)
        {
            EventHandler handler;

            handler = this.AutoScrollPositionChanged;

            if (handler != null)
                handler(this, e);
        }
        #endregion

        #region Método(s) heredado(s)
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            this.AdjustScrollbars();

            if (this.AutoScroll && !this.AutoScrollPosition.IsEmpty)
            {
                int xOffset;
                int yOffset;
                Rectangle scrollArea;

                scrollArea = this.ScrollArea;
                xOffset = scrollArea.Right - this.DisplayRectangle.Right;
                yOffset = scrollArea.Bottom - this.DisplayRectangle.Bottom;

                if (this.AutoScrollPosition.Y < 0 && yOffset < 0)
                    yOffset = Math.Max(yOffset, this.AutoScrollPosition.Y);
                else
                    yOffset = 0;

                if (this.AutoScrollPosition.X < 0 && xOffset < 0)
                    xOffset = Math.Max(xOffset, this.AutoScrollPosition.X);
                else
                    xOffset = 0;

                this.ScrollByOffset(new Size(xOffset, yOffset));
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Scroll" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ScrollEventArgs" /> instance containing the event data.</param>
        protected override void OnScroll(ScrollEventArgs e)
        {
            if (e.Type != ScrollEventType.EndScroll)
            {
                if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
                    this.ScrollByOffset(new Size(e.NewValue + this.AutoScrollPosition.X, 0));
                else if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
                    this.ScrollByOffset(new Size(0, e.NewValue + this.AutoScrollPosition.Y));
            }

            base.OnScroll(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.VisibleChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnVisibleChanged(EventArgs e)
        {
            if (base.Visible)
                base.PerformLayout();

            base.OnVisibleChanged(e);
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Scrolls the specified child control into view on an auto-scroll enabled control.
        /// </summary>
        /// <param name="activeControl">The child control to scroll into view.</param>
        public void ScrollControlIntoView(Control activeControl)
        {
            if (activeControl.Visible && AutoScroll && (HorizontalScroll.Visible || VerticalScroll.Visible))
            {
                Point position;

                position = this.AdjustPositionToSize(new Point(this.AutoScrollPosition.X + activeControl.Left, this.AutoScrollPosition.Y + activeControl.Top));

                if (position.X != this.AutoScrollPosition.X || position.Y != this.AutoScrollPosition.Y)
                    this.ScrollByOffset(new Size(this.AutoScrollPosition.X - position.X, this.AutoScrollPosition.Y - position.Y));
            }
        }
        #endregion
    };
};
