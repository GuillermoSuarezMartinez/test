using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Data;
using System.Reflection;
using Orbita.VAComun;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Orbita.Controles.VA
{
    #region Clase OTrabajoControles: Destinada a alojar métodos comnes para el trabajo con los controles
    /// <summary>
    /// Clase estática destinada a alojar métodos comnes para el trabajo con los controles
    /// </summary>
    public static class OTrabajoControles
    {
        #region Atributo(s) estático(s)
        /// <summary>
        /// Formulario principal de tipo MDI de la aplicación
        /// </summary>
        public static OrbitaForm FormularioPrincipalMDI
        {
            get { return (OrbitaForm)OTrabajoControles.FormularioPrincipalMDI; }
            set { OTrabajoControles.FormularioPrincipalMDI = value; }
        }

        /// <summary>
        /// Gestor de anclas de los formularios
        /// </summary>
        public static OrbitaDockManager DockManager; 
        #endregion

        #region Método(s) estático(s)
        /// <summary>
        /// Indica si existe algún formulario MDI hijo que este maximizado
        /// </summary>
        /// <returns>Ture si existe algún formulario MDI hijo que este maximizado; false en caso contrario</returns>
        public static bool HayFormulariosMDIHijosMaximizados()
        {
            foreach (Form f in OTrabajoControles.FormularioPrincipalMDI.MdiChildren)
            {
                if (f.WindowState == FormWindowState.Maximized)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Ajusta el tamaño y posición de la imagen del fondo en función del tamaño y posición del área de cliente del formulario
        /// </summary>
        public static void AjustarFondoAplicacion(Size tamaño, Image imagen)
        {
            OTrabajoControles.FormularioPrincipalMDI.SuspendLayout();
            // Constantes paras tunear la imagen de fondo de la aplicación //
            float valorOpacidad = (float)0.3; //Entre 0 y 1 (0 invisible, 1 totalmente visible)
            //Tamaño del área donde irá la imagen
            Size p = tamaño;

            if (OTrabajoControles.FormularioPrincipalMDI.WindowState != FormWindowState.Minimized)
            {
                if (p.Width > 0 && p.Height > 0)
                {
                    //Obtener una subimagen proporcional al tamaño del fondo, alineada desde la derecha, recortando el exceso de imagen por la izquierda
                    Image imagenOriginal = imagen;
                    Image imagenFondo = new Bitmap(p.Width, p.Height);

                    //Tratar la imagen para escalarla y trasladarla, y así que quede alineada por la derecha.
                    //Tambien tratamos la opacidad para simular una marca de agua
                    float factorEscalaY = (float)imagenFondo.Height / (float)imagenOriginal.Height;
                    float factorEscalaX = factorEscalaY;

                    using (Graphics g = Graphics.FromImage(imagenFondo))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        //Escalado
                        g.ScaleTransform(factorEscalaX, factorEscalaY);

                        //Traslacion
                        int traslacion = (int)(g.VisibleClipBounds.Width - imagenOriginal.Width);
                        g.TranslateTransform(traslacion, 0);

                        //Opacidad
                        ColorMatrix cm = new ColorMatrix();
                        cm.Matrix33 = valorOpacidad;
                        ImageAttributes ia = new ImageAttributes();
                        ia.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                        //Dibujamos la imagen creada en imagenFondo
                        g.DrawImage(imagenOriginal, new Rectangle(0, 0, imagenOriginal.Width, imagenOriginal.Height), 0, 0, imagenOriginal.Width, imagenOriginal.Height, GraphicsUnit.Pixel, ia);

                        //Establecemos la imagen obtenida como fondo de la aplicación
                        OTrabajoControles.FormularioPrincipalMDI.BackgroundImage = imagenFondo;
                    }
                }
            }
            OTrabajoControles.FormularioPrincipalMDI.ResumeLayout();
        }
        /// <summary>
        /// Comprueba que hay una fila correctamente activada para lanzar un nuevo formulario
        /// </summary>
        /// <param name="grid">OrbitaGridPro que se desea comprobar</param>
        /// <returns>True si se puede lanzar un nuevo formulario basado en la selección; false en caso contrario</returns>
        public static bool ComprobarGrid(OrbitaGridPro grid)
        {
            return ((grid.OrbGrid.ActiveRow != null) &&
                    (grid.OrbGrid.ActiveRow.IsDataRow) &&
                    (!grid.OrbGrid.ActiveRow.IsFilteredOut) &&
                    (!grid.OrbGrid.ActiveRow.IsAddRow));
        }
        /// <summary>
        /// Comprueba que la fila cumple todos los requisitos para trabajar con los campos de la misma
        /// </summary>
        /// <param name="fila">Fila que se desea comprobar</param>
        /// <returns>True si se puede lanzar un nuevo formulario basado en la selección; false en caso contrario</returns>
        public static bool ComprobarFila(Infragistics.Win.UltraWinGrid.UltraGridRow fila)
        {
            return ((fila != null) &&
                    (fila.IsDataRow) &&
                    (!fila.IsFilteredOut) &&
                    (!fila.IsAddRow));
        }
        /// <summary>
        /// Carga el combo con la lista de módulos de la aplicación
        /// </summary>
        public static void CargarCombo(OrbitaComboPro combo, Type enumType, object valorDefecto)
        {
            // Creación de una nueva tabla.
            DataTable table = new DataTable("DesplegableCombo");

            // Creación de las columnas
            table.Columns.Add(new DataColumn("Indice", enumType));
            table.Columns.Add(new DataColumn("Descripcion", typeof(string)));

            // Se rellena la tabla
            DataRow row;
            foreach (object value in Enum.GetValues(enumType))
            {
                row = table.NewRow();
                row["Indice"] = value;
                row["Descripcion"] = OStringValueAttribute.GetStringValue((Enum)value);
                table.Rows.Add(row);
            }

            // Se diseña el grid
            ArrayList cols = new ArrayList();
            cols.Add(new Orbita.Controles.Estilos.CamposEstilos("Descripcion", "Descripción"));

            // Se rellena el grid
            combo.OrbFormatear(table, cols, "Descripcion", "Descripcion");

            // Se establece el valor actual
            combo.OrbCombo.Value = OStringValueAttribute.GetStringValue((Enum)valorDefecto);
        }
        /// <summary>
        /// Carga el combo con la lista de módulos de la aplicación
        /// </summary>
        public static void CargarCombo(OrbitaComboPro combo, Dictionary<object, string> valores, Type tipo, object valorDefecto)
        {
            // Creación de una nueva tabla.
            DataTable table = new DataTable("DesplegableCombo");

            // Creación de las columnas
            table.Columns.Add(new DataColumn("Indice", tipo));
            table.Columns.Add(new DataColumn("Descripcion", typeof(string)));

            // Se rellena la tabla
            DataRow row;
            foreach (KeyValuePair<object, string> value in valores)
            {
                row = table.NewRow();
                row["Indice"] = value.Key;
                row["Descripcion"] = value.Value;
                table.Rows.Add(row);
            }

            // Se diseña el grid
            ArrayList cols = new ArrayList();
            cols.Add(new Orbita.Controles.Estilos.CamposEstilos("Descripcion", "Descripción"));
            cols.Add(new Orbita.Controles.Estilos.CamposEstilos("Indice", "Índice"));

            // Se rellena el grid
            combo.OrbFormatear(table, cols, "Descripcion", "Indice");

            // Se establece el valor actual
            combo.OrbCombo.Value = valorDefecto;

            // Se oculta el índice
            combo.OrbCombo.DisplayLayout.Bands[0].Columns["Indice"].Hidden = true;
            combo.OrbCombo.DisplayLayout.Bands[0].ColHeadersVisible = false;
            combo.OrbCombo.DisplayLayout.Bands[0].Columns["Descripcion"].AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.VisibleRows;
        }
        /// <summary>
        /// Carga de un grid de un único campo
        /// </summary>
        /// <param name="grid">Componente sobre el cual se va a trabajar</param>
        /// <param name="valores">Lista de valores a incluir en el grid</param>
        /// <param name="tipo">Tipo de datos de los valores</param>
        /// <param name="estilo">Estilo de la columna</param>
        /// <param name="alinear">Alineación del campo (celdas)</param>
        /// <param name="mascara">Máscara aplicada</param>
        /// <param name="ancho">Ancho de la columna</param>
        /// <param name="editorControl">Control con el cual se modificarán los valores</param>
        public static void CargarGridSimple(OrbitaGridPro grid, List<object> valores, Type tipo, Estilos.EstiloColumna estilo, Estilos.Alineacion alinear, Estilos.Mascara mascara, int ancho, Control editorControl)
        {
            // Bloqueamos el grid
            grid.OrbGrid.BeginUpdate();
            grid.SuspendLayout();

            // Creación de una nueva tabla.
            DataTable table = new DataTable("Table");

            // Creación de las columnas
            table.Columns.Add(new DataColumn("Valor", typeof(object)));
            //table.Columns.Add(new DataColumn("Visualizado", tipo));

            // Se rellena la tabla
            DataRow row;
            foreach (object objeto in valores)
            {
                row = table.NewRow();
                row["Valor"] = objeto;
                table.Rows.Add(row);
            }

            // Se carga el grid
            ArrayList list = new ArrayList();
            list.Add(new Estilos.CamposEstilos("Valor", "Valor", estilo, alinear, mascara, ancho, false));

            // Formateamos las columnas y las rellenamos de datos
            grid.OrbFormatear(table, list);

            // Formato
            grid.OrbGrid.DisplayLayout.Bands[0].ColHeadersVisible = false;
            if (editorControl != null)
            {
                grid.OrbGrid.DisplayLayout.Bands[0].Columns[0].EditorControl = editorControl;
            }

            // Desbloqueamos el grid
            grid.OrbGrid.EndUpdate();
            grid.ResumeLayout();
        }
        /// <summary>
        /// Añade texto a un RichTextBox
        /// </summary>
        /// <param name="box"></param>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public static void RichTextBoxAppendText(RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            Color colorAnterior = box.SelectionColor;
            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = colorAnterior;
        }
        /// <summary>
        /// Añade texto a un RichTextBox
        /// </summary>
        /// <param name="box"></param>
        /// <param name="text"></param>
        /// <param name="font"></param>
        public static void RichTextBoxAppendText(RichTextBox box, string text, Font font)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            Font fuenteAnterior = box.SelectionFont;
            box.SelectionFont = font;
            box.AppendText(text);
            box.SelectionFont = fuenteAnterior;
        }
        /// <summary>
        /// Añade texto a un RichTextBox
        /// </summary>
        /// <param name="box"></param>
        /// <param name="text"></param>
        /// <param name="color"></param>
        /// <param name="font"></param>
        public static void RichTextBoxAppendText(RichTextBox box, string text, Color color, Font font)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            Color colorAnterior = box.SelectionColor;
            Font fuenteAnterior = box.SelectionFont;
            box.SelectionColor = color;
            box.SelectionFont = font;
            box.AppendText(text);
            box.SelectionColor = colorAnterior;
            box.SelectionFont = fuenteAnterior;
        }
        #endregion
    } 
    #endregion

    #region Clase EnumeracionCombo: Utilizada para visualizar en el componente OrbitaComboPro enumerados
    /// <summary>
    /// Clase destinada a los combos de los formularios databinding que representan un enumerado
    /// </summary>
    public class OEnumeracionCombo
    {
        #region Propiedad(es)
        /// <summary>
        /// Enumerado a elegir en el comobo box
        /// </summary>
        private object _Enumerado;
        /// <summary>
        /// Enumerado a elegir en el comobo box
        /// </summary>
        public object Enumerado
        {
            get { return _Enumerado; }
            set { _Enumerado = value; }
        }
        /// <summary>
        /// Descripción a mostrar en el combo box
        /// </summary>
        private string _Descripcion;
        /// <summary>
        /// Descripción a mostrar en el combo box
        /// </summary>
        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="enumerado">Enumerado a elegir en el comobo box</param>
        /// <param name="descripcion">Descripción a mostrar en el combo box</param>
        public OEnumeracionCombo(object enumerado, string descripcion)
        {
            this._Enumerado = enumerado;
            this._Descripcion = descripcion;
        }
        #endregion
    }
    #endregion
}
