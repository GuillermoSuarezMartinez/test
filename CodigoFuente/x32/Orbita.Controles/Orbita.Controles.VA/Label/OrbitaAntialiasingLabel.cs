using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Orbita.Controles.VA
{
    /// <summary>
    /// Label con la posibilidad de elegir el método de renderizado
    /// </summary>
    public class OrbitaAntialiasingLabel: OrbitaLabel
    {
        #region Propiedad(es)
        /// <summary>
        /// Indica el método para renderizar el texto. Por defecto se utiliza el antialiasing
        /// </summary>
        private TextRenderingHint _hint = TextRenderingHint.SystemDefault;
        /// <summary>
        /// Indica el método para renderizar el texto. Por defecto se utiliza el antialiasing
        /// </summary>
        [Browsable(true),
        Category("Orbita"),
        Description("Indica el método para renderizar el texto. Por defecto se utiliza el antialiasing"),
        DefaultValue(true)]
        public TextRenderingHint TextRenderingHint
        {
            get { return this._hint; }
            set { this._hint = value; }
        }         
        #endregion

        #region Método(s) heredado(s)
		/// <summary>
        /// Método de repintado
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {            
            pe.Graphics.TextRenderingHint = TextRenderingHint;
            base.OnPaint(pe);
        }
	    #endregion    
    }
}
