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
using System.ComponentModel;

namespace Orbita.VAComun
{
    internal partial class ScrollControl
    {
        internal class HScrollProperties : PropiedadesScroll
        {
            #region Constructor(es)
            /// <summary>
            /// Constructor de la clase
            /// </summary>
            /// <param name="container"></param>
            public HScrollProperties(ScrollControl container)
                : base(container)
            { } 
            #endregion
        }

        internal abstract class PropiedadesScroll
        {
            #region Atributo(s)
            ScrollControl _container;
            #endregion

            #region Propiedad(es)

            [DefaultValue(true)]
            public bool Enabled { get; set; }

            [DefaultValue(10)]
            public int LargeChange { get; set; }

            [DefaultValue(100)]
            public int Maximum { get; set; }

            [DefaultValue(0)]
            public int Minimum { get; set; }

            [DefaultValue(1)]
            public int SmallChange { get; set; }

            [Bindable(true)]
            [DefaultValue(0)]
            public int Value { get; set; }

            [DefaultValue(false)]
            public bool Visible { get; set; }

            protected ScrollControl ParentControl
            { get { return _container; } }

            #endregion

            #region Constructor(es)
            /// <summary>
            /// Constructor de la clase (abstracta no instancia)
            /// </summary>
            /// <param name="container"></param>
            public PropiedadesScroll(ScrollControl container)
            {
                this._container = container;
            }
            #endregion
        }

        internal class VScrollProperties : PropiedadesScroll
        {
            #region Constructor(es)
            /// <summary>
            /// Constructor de la clase
            /// </summary>
            /// <param name="container"></param>
            public VScrollProperties(ScrollControl container)
                : base(container)
            { }
            #endregion
        }
    }
}

