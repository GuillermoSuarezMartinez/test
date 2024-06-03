//***********************************************************************
// Assembly         : Orbita.VAComun
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
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using Orbita.Utiles;

namespace Orbita.VAComun
{
    /// <summary>
    /// Clase utilizada para componer XML en memoria
    /// </summary>
    public class OCXML : IDisposable
    {
        #region Atributo/s
        /// <summary>
        /// Variable escrita en el XML.
        /// NOTA: EL SIGUIENTE COMENTARIO SE HA MOVIDO DE OTRA UBICACIÓN, PERO CREO QUE SE REFIERE A ESTE ATRIBUTO (Vnicolau, 07/02/2011)
        /// Este atributo indica 'QUE VARIABLES' se van a escribir en Base de datos,
        /// es decir, contiene cada uno de los registros que se van a insertar.
        /// </summary>
        protected StringWriter m_SW_XML;
        /// <summary>
        /// Variable para escribir el XML
        /// </summary>
        protected XmlTextWriter m_Escritor_XML;
        ///// <summary>
        ///// Variable para contener el XML para lectura
        ///// </summary>
        //public OXml m_XML;
        #endregion Atributo/s

        #region Propiedad/es
        /// <summary>
        /// Propiedad para acceder al Stringwriter
        /// </summary>
        public StringWriter SW_XML
        {
            get { return (this.m_SW_XML); }
            set { this.m_SW_XML = value; }
        }
        /// <summary>
        /// Propiedad para acceder al XmlTextWrite
        /// </summary>
        public XmlTextWriter Escritor_XML
        {
            get { return (this.m_Escritor_XML); }
            set { this.m_Escritor_XML = value; }
        }
        #endregion Propiedad/es

        #region Constructor(es)
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OCXML()
        {
            // Para escritura
            this.m_SW_XML = new StringWriter();
            this.m_Escritor_XML = new XmlTextWriter(this.m_SW_XML);

            //// Para lectura
            //this.m_XML = new OXml();
        }
        #endregion

        #region Método(s) público(s) Escritura
        /// <summary>
        /// Para abrir una nueva etiqueta del XML
        /// </summary>
        /// <param name="PEtiqueta"></param>
        public void AbrirEtiqueta(string PEtiqueta)
        {
            this.m_Escritor_XML.WriteStartElement(PEtiqueta.ToString());

            OXml o = new OXml();

        }
        /// <summary>
        /// Funcion para añadir una propiedad tipo string
        /// </summary>
        /// <param name="PEtiqueta"></param>
        public void Add(string PEtiqueta)
        {
            this.m_Escritor_XML.WriteStartElement(PEtiqueta.ToString());
        }
        /// <summary>
        /// Funcion para añadir un campo tipo pasandole un valor tipo object
        /// </summary>
        /// <param name="PEtiqueta"></param>
        /// <param name="PValor"></param>
        public void Add(string PEtiqueta, object PValor)
        {
            if (PValor is DateTime)
            {
                this.Add(PEtiqueta, (DateTime)PValor);
            }
            else if (PValor is TimeSpan)
            {
                this.Add(PEtiqueta, (TimeSpan)PValor);
            }
            else
            {
                this.m_Escritor_XML.WriteStartElement(PEtiqueta.ToString());
                this.m_Escritor_XML.WriteValue(PValor);
                this.m_Escritor_XML.WriteEndElement();
            }
        }
        /// <summary>
        /// Funcion para añadir un campo tipo pasandole un valor tipo int
        /// </summary>
        /// <param name="PEtiqueta"></param>
        /// <param name="PValor"></param>
        public void Add(string PEtiqueta, int PValor)
        {
            this.m_Escritor_XML.WriteStartElement(PEtiqueta.ToString());
            this.m_Escritor_XML.WriteValue(PValor);
            this.m_Escritor_XML.WriteEndElement();
        }
        /// <summary>
        /// Funcion para añadir un campo tipo pasandole un valor tipo bool
        /// </summary>
        /// <param name="PEtiqueta"></param>
        /// <param name="PValor"></param>
        public void Add(string PEtiqueta, bool PValor)
        {
            this.m_Escritor_XML.WriteStartElement(PEtiqueta.ToString());
            this.m_Escritor_XML.WriteValue(PValor);
            this.m_Escritor_XML.WriteEndElement();
        }
        /// <summary>
        /// Funcion para añadir un campo tipo pasandole un valor tipo null
        /// </summary>
        /// <param name="PEtiqueta"></param>
        /// <param name="PValor"></param>
        public void Add(string PEtiqueta, DBNull PValor)
        {
            this.m_Escritor_XML.WriteStartElement(PEtiqueta.ToString());
            this.m_Escritor_XML.WriteValue(null); //No funciona!! Para pasar un valor NULL, hay que utilizar string.Empty como argumento al llamar al método Add
            this.m_Escritor_XML.WriteEndElement();
        }
        /// <summary>
        /// Funcion para añadir un campo tipo pasandole un valor tipo datetime.
        /// En el SQLServer se ha de utilizar la instrucción "convert(fecha, 126)"
        /// </summary>
        /// <param name="PEtiqueta"></param>
        /// <param name="PValor"></param>
        public void Add(string PEtiqueta, DateTime PValor)
        {
            this.m_Escritor_XML.WriteStartElement(PEtiqueta.ToString());
            this.m_Escritor_XML.WriteValue(PValor.ToString("yyyyMMdd' 'HH':'mm':'ss.fff", DateTimeFormatInfo.InvariantInfo));
            this.m_Escritor_XML.WriteEndElement();
        }
        /// <summary>
        /// Funcion para añadir un campo tipo pasandole un valor tipo timespan.
        /// En el SQLServer se ha de utilizar un float puesto que se indica en milisegundos
        /// </summary>
        /// <param name="PEtiqueta"></param>
        /// <param name="PValor"></param>
        public void Add(string PEtiqueta, TimeSpan PValor)
        {
            this.m_Escritor_XML.WriteStartElement(PEtiqueta.ToString());
            this.m_Escritor_XML.WriteValue(PValor.TotalMilliseconds);
            this.m_Escritor_XML.WriteEndElement();
        }
        /// <summary>
        /// Funcion para añadir un campo tipo pasandole un valor tipo short
        /// </summary>
        /// <param name="PEtiqueta"></param>
        /// <param name="PValor"></param>
        public void Add(string PEtiqueta, short PValor)
        {
            this.m_Escritor_XML.WriteStartElement(PEtiqueta.ToString());
            this.m_Escritor_XML.WriteValue(PValor);
            this.m_Escritor_XML.WriteEndElement();
        }
        /// <summary>
        /// Funcion para añadir un campo tipo pasandole un valor tipo double
        /// </summary>
        /// <param name="PEtiqueta"></param>
        /// <param name="PValor"></param>
        public void Add(string PEtiqueta, double PValor)
        {
            this.m_Escritor_XML.WriteStartElement(PEtiqueta.ToString());
            this.m_Escritor_XML.WriteValue(PValor);
            this.m_Escritor_XML.WriteEndElement();
        }
        /// <summary>
        /// Funcion para añadir un campo tipo pasandole un valor tipo decimal
        /// </summary>
        /// <param name="PEtiqueta"></param>
        /// <param name="PValor"></param>
        public void Add(string PEtiqueta, decimal PValor)
        {
            this.m_Escritor_XML.WriteStartElement(PEtiqueta.ToString());
            this.m_Escritor_XML.WriteValue(PValor);
            this.m_Escritor_XML.WriteEndElement();
        }
        /// <summary>
        /// Funcion para añadir un campo tipo pasandole un valor tipo string
        /// </summary>
        /// <param name="PEtiqueta"></param>
        /// <param name="PValor"></param>
        public void Add(string PEtiqueta, string PValor)
        {
            this.m_Escritor_XML.WriteStartElement(PEtiqueta.ToString());
            this.m_Escritor_XML.WriteValue(PValor);
            this.m_Escritor_XML.WriteEndElement();
        }
        /// <summary>
        /// Funcion para añadir un campo tipo pasandole un valor tipo string
        /// </summary>
        /// <param name="PEtiqueta"></param>
        public void AddNull(string PEtiqueta)
        {
            this.m_Escritor_XML.WriteStartElement(PEtiqueta.ToString());
            this.m_Escritor_XML.WriteValue(@"xsi:nil=""true""");
            this.m_Escritor_XML.WriteEndElement();
        }
        /// <summary>
        /// Funcion para cerrar la etiqueta abierta
        /// </summary>
        public void CerrarEtiqueta()
        {
            this.m_Escritor_XML.WriteEndElement();
        }
        /// <summary>
        /// Funcion para abrir el documento
        /// </summary>
        public void AbrirDocumento()
        {
            this.m_Escritor_XML.WriteStartDocument();
        }
        /// <summary>
        /// Funcion para abrir el documento incluyendole una etiqueta inicial
        /// </summary>
        /// <param name="PEtiqueta"></param>
        public void AbrirDocumento(string PEtiqueta)
        {
            this.m_Escritor_XML.WriteStartDocument();
            this.m_Escritor_XML.WriteStartElement(PEtiqueta.ToString());
        }
        /// <summary>
        /// Funcion para cerrar el documento abierto 
        /// </summary>
        public void CerrarDocumento()
        {
            this.m_Escritor_XML.WriteEndElement();
            this.m_Escritor_XML.Flush();
            this.m_Escritor_XML.Close();
            this.m_SW_XML.Flush();
        }
        /// <summary>
        /// Función para leer el documento almacenado 
        /// </summary>
        /// <returns></returns>
        public XPathDocument LeerXml()
        {
            StringReader sr = new StringReader(this.m_SW_XML.ToString());
            XPathDocument xpathDoc = new XPathDocument(sr);
            sr.Close();
            //
            return (xpathDoc);
        }
        #endregion

        #region Método(s) público(s) Lectura
#if CHILLKAT
        /// <summary>
        /// Cargamos el XML con el string pasado por parametro
        /// </summary>
        /// <param name="dataXML"></param>
        /// <returns></returns>
        public bool CargarXML(string dataXML)
        {
            return this.m_XML.LoadXml(dataXML);
        }
        /// <summary>
        /// // Para obtener el valor de un nodo a través de su ruta separadas las etiquetas por |
        /// </summary>
        /// <param name="rutaPath">Relativa a las etiquetas tipo "Presupuesto|Partida|IdPartida", sin añadimos "Presupuesto|Partida[2]|IdPartida iriamos al 3 elemento</param>
        /// <returns></returns>
        public string ObtenerNodo(string rutaPathEtiquetas)
        {
            string temp = rutaPathEtiquetas + "|*";
            return this.m_XML.ChilkatPath(temp);
        }
        /// <summary>
        ///  Para obtener el valor de un nodo en double a través de su ruta separadas las etiquetas por |
        /// </summary>
        /// <param name="rutaPath">Relativa a las etiquetas tipo "Presupuesto|Partida|IdPartida", sin añadimos "Presupuesto|Partida[2]|IdPartida iriamos al 3 elemento</param>
        /// <returns></returns>
        public double ObtenerNodoDouble(string rutaPathEtiquetas)
        {
            string temp = rutaPathEtiquetas + "|*";
            string cadena = this.m_XML.ChilkatPath(temp);
            if (cadena == null)
            {
                return 0;
            }
            cadena = cadena.Replace(".", ",");
            double aux = Convert.ToDouble(cadena);
            return aux;
        }
        /// <summary>
        ///  Para obtener el valor de un nodo en boolean a través de su ruta separadas las etiquetas por |
        /// </summary>
        /// <param name="rutaPath">Relativa a las etiquetas tipo "Presupuesto|Partida|IdPartida", sin añadimos "Presupuesto|Partida[2]|IdPartida iriamos al 3 elemento</param>
        /// <returns></returns>
        public bool ObtenerNodoBool(string rutaPathEtiquetas)
        {
            string temp = rutaPathEtiquetas + "|*";
            if (this.m_XML.ChilkatPath(temp) == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Obtenemos el valor de un campo , en relación al valor de otro nodo de la misma rama pasado como argumento
        /// </summary>
        /// <param name="etiqueta">Etiqueta del indicador</param>
        /// <param name="valorIndicador">valor del indicador</param>
        /// <param name="campoDeseado">campo del que queremos estraer su valor</param>
        /// <returns></returns>
        public string ObtenerCampoPorIndicador(string etiqueta, string valorIndicador, string campoDeseado)
        {
            string temp = "/C/" + "etiqueta,+ valorIndicador +|..|" + campoDeseado + "|*";
            return this.m_XML.ChilkatPath(temp);
        }
        /// <summary>
        /// Obtenemos el subarbol con la etiqueta pasada por parametro como nodo raiz
        /// </summary>
        /// <param name="etiqueta">etiqueta deseada para el subarbol</param>
        /// <returns></returns>
        public OXml ObtenerSubArbol(string etiqueta)
        {
            return this.m_XML.FindChild(etiqueta);
        }
        /// <summary>
        /// Obtenemos el subarbol con el indice pasado por parametro
        /// </summary>
        /// <param name="etiqueta">etiqueta deseada para el subarbol</param>
        /// <returns></returns>
        public OXml ObtenerSubArbol(int id)
        {
            return this.m_XML.GetChild(id);
        }
        /// <summary>
        /// Devuelve el numero de hijos que contiene el árbol
        /// </summary>
        /// <returns></returns>
        public int NumeroHijos()
        {
            return this.m_XML.NumChildren;
        }
        /// <summary>
        /// Comprobación para saber si tiene un subarbol dentro con la etiqueta pasada por parametro
        /// </summary>
        /// <param name="etiqueta"></param>
        /// <returns></returns>
        public bool TieneSubarbol(string etiqueta)
        {
            return this.m_XML.HasChildWithTag(etiqueta);
        }
        /// <summary>
        /// Comprobación para saber si tiene un campo dentro con la etiqueta pasada por parametro
        /// </summary>
        /// <param name="etiqueta"></param>
        /// <returns></returns>
        public bool TieneCampo(string valor)
        {
            return this.m_XML.HasAttribute(valor);
        }
#endif
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Elmina los recursos utilizados
        /// </summary>
        public void Dispose()
        {
            if (this.m_Escritor_XML != null)
            {
                this.m_Escritor_XML.Close();
                this.m_Escritor_XML = null;
            }
            if (this.m_SW_XML != null)
            {
                this.m_SW_XML.Close();
                this.m_SW_XML.Dispose();
                this.m_SW_XML = null;
            }
#if CHILLKAT
            if (this.m_XML != null)
            {
                //this.m_XML.RemoveAllChildren();
                //this.m_XML.RemoveAllAttributes();
                //this.m_XML.Clear();
                this.m_XML.Dispose();
                this.m_XML = null;
            }
#endif
        }
        #endregion
    }
}