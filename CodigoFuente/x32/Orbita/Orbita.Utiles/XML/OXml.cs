//***********************************************************************
// Assembly         : OrbitaUtiles
// Author           : crodriguez
// Created          : 03-03-2011
//
// Last Modified By : crodriguez
// Last Modified On : 03-03-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.Globalization;
using System.IO;
using System.Xml;
namespace Orbita.Utiles
{
    /// <summary>
    /// Clase Xml de Orbita.
    /// </summary>
    public class OXml : IDisposable
    {
        //#region Constructor(es)
        ///// <summary>
        ///// Inicializar una nueva instancia de la clase OXML.
        ///// </summary>
        //public OXml() { }
        //#endregion

        //#region Destructor(es)
        ///// <summary>
        ///// Indica si ya se llamo al método Dispose. (default = false)
        ///// </summary>
        //bool disposed = false;
        ///// <summary>
        ///// Implementa IDisposable.
        ///// No  hacer  este  método  virtual.
        ///// Una clase derivada no debería ser
        ///// capaz de  reemplazar este método.
        ///// </summary>
        //public void Dispose()
        //{
        //    // Llamo al método que  contiene la lógica
        //    // para liberar los recursos de esta clase.
        //    Dispose(true);
        //    // Este objeto será limpiado por el método Dispose.
        //    // Llama al método del recolector de basura, GC.SuppressFinalize.
        //    GC.SuppressFinalize(this);
        //}
        ///// <summary>
        ///// Método  sobrecargado de  Dispose que será  el que
        ///// libera los recursos. Controla que solo se ejecute
        ///// dicha lógica una  vez y evita que el GC tenga que
        ///// llamar al destructor de clase.
        ///// </summary>
        ///// <param name="disposing">Indica si llama al método Dispose.</param>
        //protected virtual void Dispose(bool disposing)
        //{
        //    // Preguntar si Dispose ya fue llamado.
        //    if (!this.disposed)
        //    {
        //        // Marcar como desechada ó desechandose,
        //        // de forma que no se puede ejecutar el
        //        // código dos veces.
        //        disposed = true;
        //    }
        //}
        ///// <summary>
        ///// Destructor(es) de clase.
        ///// En caso de que se nos olvide “desechar” la clase,
        ///// el GC llamará al destructor, que tambén ejecuta 
        ///// la lógica anterior para liberar los recursos.
        ///// </summary>
        //~OXml()
        //{
        //    // Llamar a Dispose(false) es óptimo en terminos
        //    // de legibilidad y mantenimiento.
        //    Dispose(false);
        //}
        //#endregion

        //#region Método(s) público(s)
        //#region Método(s) estático(s)
        ///// <summary>
        ///// Cargar el contenido de un fichero XML.
        ///// </summary>
        ///// <param name="fichero">Fichero.</param>
        ///// <returns>Fichero de texto.</returns>
        //public static string Cargar(string fichero)
        //{
        //    string resultado = string.Empty;
        //    if (File.Exists(fichero))
        //    {
        //        XmlDocument xdoc = new XmlDocument();
        //        xdoc.Load(fichero);

        //        // Componer el resultado con el contenido del fichero.
        //        resultado = string.Format(CultureInfo.CurrentCulture, "<{0}>{1}</{0}>", xdoc.DocumentElement.LocalName, xdoc.DocumentElement.InnerXml);
        //    }
        //    return resultado;
        //}
        ///// <summary>
        ///// Leer el contenido de un fichero Xml.
        ///// </summary>
        ///// <param name="documento">Documento Xml.</param>
        ///// <param name="nodo">Nodo buscado en el Xml.</param>
        ///// <returns>El valor de la lista de nodos.</returns>
        //public XmlNodeList Leer(string documento, string nodo)
        //{
        //    // Inicializar el documento.
        //    XmlDocument xmlDocumento = null;

        //    // Crear un objeto de tipo documento Xml.
        //    xmlDocumento = new XmlDocument();

        //    // Cargar el documento Xml a  partir
        //    // de la ruta especificada. La ruta 
        //    // del documento Xml permite  rutas 
        //    // relativas respecto del ejecutable.
        //    xmlDocumento.Load(documento);

        //    // Obtener el nodo a partir del parámetro.
        //    return xmlDocumento.GetElementsByTagName(nodo);
        //}
        ///// <summary>
        ///// Leer el contenido de un fichero Xml.
        ///// </summary>
        ///// <param name="documento">Documento Xml.</param>
        ///// <param name="nodo">Nodo buscado en el Xml.</param>
        ///// <param name="subNodo">Subnodo a partir del nodo.</param>
        ///// <returns>El valor de la lista de nodos.</returns>
        //public static XmlNodeList Leer(string documento, string nodo, string subNodo)
        //{
        //    // Inicializar el documento.
        //    XmlDocument xmlDocumento = null;
        //    XmlNodeList xmlNodo = null;

        //    // Crear un objeto de tipo documento Xml.
        //    xmlDocumento = new XmlDocument();

        //    // Cargar el documento Xml a  partir
        //    // de la ruta especificada. La ruta 
        //    // del documento Xml permite  rutas 
        //    // relativas respecto del ejecutable.
        //    xmlDocumento.Load(documento);

        //    // Obtener el nodo a partir del parámetro.
        //    xmlNodo = xmlDocumento.GetElementsByTagName(nodo.ToUpper());

        //    // Devolver los subnodos a partir del nodo.
        //    return ((XmlElement)xmlNodo[0]).GetElementsByTagName(subNodo.ToUpper());
        //}
        //#endregion
        //#endregion
        
        #region Atributos
        /// <summary>
        /// Variable escrita en el Xml.
		/// NOTA: EL SIGUIENTE COMENTARIO SE HA MOVIDO DE OTRA UBICACIÓN, PERO CREO QUE SE REFIERE A ESTE ATRIBUTO (Vnicolau, 07/02/2011)
		/// Este atributo indica 'QUE VARIABLES' se van a escribir en Base de datos,
        /// es decir, contiene cada uno de los registros que se van a insertar.
        /// </summary>
        StringWriter m_swXml;
        /// <summary>
        /// Variable para escribir el XML
        /// </summary>
        XmlTextWriter m_escritorXml;
        
		#endregion Atributos

		#region Constructores
		/// <summary>
        /// Constructor de la clase.
        /// </summary>
        public OXml()
        {
            // Para escritura.
            this.m_swXml = new StringWriter(CultureInfo.CurrentCulture);
            this.m_escritorXml = new XmlTextWriter(this.m_swXml);
            // Para lectura.
            //this.m_xml = new Xml();
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Propiedad para acceder al Stringwriter.
        /// </summary>
        public StringWriter SWXml
        {
            get { return this.m_swXml; }
            set { this.m_swXml = value; }
        }
        /// <summary>
        /// Propiedad para acceder al XmlTextWriter.
        /// </summary>
        public XmlTextWriter EscritorXml
        {
            get { return this.m_escritorXml; }
            set { this.m_escritorXml = value; }
        }
        #endregion Propiedades

        #region Métodos públicos escritura
        /// <summary>
        /// Para abrir una nueva etiqueta del Xml
        /// </summary>
        /// <param name="etiqueta"></param>
        public void AbrirEtiqueta(string etiqueta)
        {
            if (!string.IsNullOrEmpty(etiqueta))
            {
                this.m_escritorXml.WriteStartElement(etiqueta.ToString());
            }
        }
        /// <summary>
        /// Funcion para añadir una propiedad tipo string.
        /// </summary>
        /// <param name="etiqueta"></param>
        public void Add(string etiqueta)
        {
            this.m_escritorXml.WriteStartElement(etiqueta.ToString());
        }
        /// <summary>
        /// Funcion para añadir un campo tipo pasandole un balor tipo object.
        /// </summary>
        /// <param name="etiqueta"></param>
        /// <param name="valor"></param>
        public void Add(string etiqueta, object valor)
        {
            if (!string.IsNullOrEmpty(etiqueta))
            {
                this.m_escritorXml.WriteStartElement(etiqueta.ToString());
                this.m_escritorXml.WriteValue(valor);
                this.m_escritorXml.WriteEndElement();
            }
        }
        /// <summary>
        /// Funcion para añadir un campo tipo pasandole un balor tipo int.
        /// </summary>
        /// <param name="etiqueta"></param>
        /// <param name="valor"></param>
        public void Add(string etiqueta, int valor)
        {
            this.m_escritorXml.WriteStartElement(etiqueta.ToString());
            this.m_escritorXml.WriteValue(valor);
            this.m_escritorXml.WriteEndElement();
        }
        /// <summary>
        /// Funcion para añadir un campo tipo pasandole un balor tipo bool.
        /// </summary>
        /// <param name="etiqueta"></param>
        /// <param name="valor"></param>
        public void Add(string etiqueta, bool valor)
        {
            if (!string.IsNullOrEmpty(etiqueta))
            {
                this.m_escritorXml.WriteStartElement(etiqueta.ToString());
                this.m_escritorXml.WriteValue(valor);
                this.m_escritorXml.WriteEndElement();
            }
        }
        /// <summary>
        /// Funcion para añadir un campo tipo pasandole un balor tipo null.
        /// </summary>
        /// <param name="etiqueta"></param>
        /// <param name="valor"></param>
        public void Add(string etiqueta, DBNull valor)
        {
            this.m_escritorXml.WriteStartElement(etiqueta.ToString());
            this.m_escritorXml.WriteValue(null); //No funciona!! Para pasar un valor NULL, hay que utilizar string.Empty como argumento al llamar al método Add
            this.m_escritorXml.WriteEndElement();
        }
        /// <summary>
        /// Funcion para añadir un campo tipo pasandole un balor tipo datetime.
        /// </summary>
        /// <param name="etiqueta"></param>
        /// <param name="valor"></param>
        public void Add(string etiqueta, DateTime valor)
        {
            if (!string.IsNullOrEmpty(etiqueta))
            {
                this.m_escritorXml.WriteStartElement(etiqueta.ToString());
                this.m_escritorXml.WriteValue(valor);
                this.m_escritorXml.WriteEndElement();
            }
        }
        /// <summary>
        /// Funcion para añadir un campo tipo pasandole un balor tipo short.
        /// </summary>
        /// <param name="etiqueta"></param>
        /// <param name="valor"></param>
        public void Add(string etiqueta, short valor)
        {
            if (!string.IsNullOrEmpty(etiqueta))
            {
                this.m_escritorXml.WriteStartElement(etiqueta.ToString());
                this.m_escritorXml.WriteValue(valor);
                this.m_escritorXml.WriteEndElement();
            }
        }
        /// <summary>
        /// Funcion para añadir un campo tipo pasandole un balor tipo double.
        /// </summary>
        /// <param name="etiqueta"></param>
        /// <param name="valor"></param>
        public void Add(string etiqueta, double valor)
        {
            if (!string.IsNullOrEmpty(etiqueta))
            {
                this.m_escritorXml.WriteStartElement(etiqueta.ToString());
                this.m_escritorXml.WriteValue(valor);
                this.m_escritorXml.WriteEndElement();
            }
        }
        /// <summary>
        /// Funcion para añadir un campo tipo pasandole un balor tipo decimal.
        /// </summary>
        /// <param name="etiqueta"></param>
        /// <param name="valor"></param>
        public void Add(string etiqueta, decimal valor)
        {
            if (!string.IsNullOrEmpty(etiqueta))
            {
                this.m_escritorXml.WriteStartElement(etiqueta.ToString());
                this.m_escritorXml.WriteValue(valor);
                this.m_escritorXml.WriteEndElement();
            }
        }
        /// <summary>
        /// Funcion para añadir un campo tipo pasandole un balor tipo string.
        /// </summary>
        /// <param name="etiqueta"></param>
        /// <param name="valor"></param>
        public void Add(string etiqueta, string valor)
        {
            if (!string.IsNullOrEmpty(etiqueta))
            {
                this.m_escritorXml.WriteStartElement(etiqueta.ToString());
                this.m_escritorXml.WriteValue(valor);
                this.m_escritorXml.WriteEndElement();
            }
        }
        /// <summary>
        /// Función para cerrar la etiqueta abierta.
        /// </summary>
        public void CerrarEtiqueta()
        {
            this.m_escritorXml.WriteEndElement();
        }
        /// <summary>
        /// Función para abrir el documento.
        /// </summary>
        public void AbrirDocumento()
        {
            this.m_escritorXml.WriteStartDocument();
        }
        /// <summary>
        /// Función para abrir el documento incluyendole una etiqueta inicial.
        /// </summary>
        /// <param name="etiqueta"></param>
        public void AbrirDocumento(string etiqueta)
        {
            if (!string.IsNullOrEmpty(etiqueta))
            {
                this.m_escritorXml.WriteStartDocument();
                this.m_escritorXml.WriteStartElement(etiqueta.ToString());
            }
        }
        /// <summary>
        /// Función para cerrar el documento abierto.
        /// </summary>
        public void CerrarDocumento()
        {
            this.m_escritorXml.WriteEndElement();
            this.m_escritorXml.Flush();
            this.m_escritorXml.Close();
            this.m_swXml.Flush();
        }
        /// <summary>
        /// Función para leer el documento almacenado. 
        /// </summary>
        /// <returns></returns>
        public System.Xml.XPath.XPathDocument LeerXml()
        {
            StringReader sr = new StringReader(this.m_swXml.ToString());
            System.Xml.XPath.XPathDocument xpathDoc = new System.Xml.XPath.XPathDocument(sr);
            sr.Close();
            return (xpathDoc);
        }
        #endregion

        #region Métodos públicos lectura
        ///// <summary>
        ///// Cargamos el Xml con el string pasado por parámetro.
        ///// </summary>
        ///// <param name="datoXml"></param>
        ///// <returns></returns>
        //public bool CargarXml(string datoXml)
        //{
        //    return this.m_xml.LoadXml(datoXml);
        //}
        ///// <summary>
        ///// Para obtener el valor de un nodo a través de su ruta separadas las etiquetas por |.
        ///// </summary>
        ///// <param name="rutaPath">Relativa a las etiquetas tipo "Presupuesto|Partida|IdPartida", sin añadimos "Presupuesto|Partida[2]|IdPartida iriamos al 3 elemento</param>
        ///// <returns></returns>
        //public string ObtenerNodo(string rutaPathEtiquetas)
        //{
        //    string temp = rutaPathEtiquetas + "|*";
        //    return this.m_xml.ChilkatPath(temp);
        //}
        ///// <summary>
        ///// Para obtener el valor de un nodo en double a través de su ruta separadas las etiquetas por |.
        ///// </summary>
        ///// <param name="rutaPath">Relativa a las etiquetas tipo "Presupuesto|Partida|IdPartida", sin añadimos "Presupuesto|Partida[2]|IdPartida iriamos al 3 elemento</param>
        ///// <returns></returns>
        //public double ObtenerNodoDouble(string rutaPathEtiquetas)
        //{
        //    string temp = rutaPathEtiquetas + "|*";
        //    string cadena = this.m_xml.ChilkatPath(temp);
        //    if (cadena == null)
        //    {
        //        return 0;
        //    }
        //    cadena = cadena.Replace(".", ",");
        //    double aux = Convert.ToDouble(cadena);
        //    return aux;
        //}
        ///// <summary>
        ///// Para obtener el valor de un nodo en boolean a través de su ruta separadas las etiquetas por |.
        ///// </summary>
        ///// <param name="rutaPath">Relativa a las etiquetas tipo "Presupuesto|Partida|IdPartida", sin añadimos "Presupuesto|Partida[2]|IdPartida iriamos al 3 elemento</param>
        ///// <returns></returns>
        //public bool ObtenerNodoBool(string rutaPathEtiquetas)
        //{
        //    string temp = rutaPathEtiquetas + "|*";
        //    if (this.m_xml.ChilkatPath(temp) == "1")
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        ///// <summary>
        ///// Obtenemos el valor de un campo , en relación al valor de otro nodo de la misma rama pasado como argumento
        ///// </summary>
        ///// <param name="etiqueta">Etiqueta del indicador</param>
        ///// <param name="valorIndicador">valor del indicador</param>
        ///// <param name="campoDeseado">campo del que queremos estraer su valor</param>
        ///// <returns></returns>
        //public string ObtenerCampoPorIndicador(string campoDeseado)
        //{
        //    string temp = "/C/" + "etiqueta,+ valorIndicador +|..|" + campoDeseado + "|*";
        //    return this.m_xml.ChilkatPath(temp);
        //}
        ///// <summary>
        ///// Obtenemos el subarbol con la etiqueta pasada por parametro como nodo raiz.
        ///// </summary>
        ///// <param name="etiqueta">etiqueta deseada para el subarbol</param>
        ///// <returns></returns>
        //public Xml ObtenerSubArbol(string etiqueta)
        //{
        //    return this.m_xml.FindChild(etiqueta);
        //}
        ///// <summary>
        ///// Obtenemos el subarbol con el indice pasado por parámetro.
        ///// </summary>
        ///// <param name="etiqueta">etiqueta deseada para el subarbol</param>
        ///// <returns></returns>
        //public Xml ObtenerSubArbol(int id)
        //{
        //    return this.m_xml.GetChild(id);
        //}
        ///// <summary>
        ///// Devuelve el numero de hijos que contiene el árbol.
        ///// </summary>
        ///// <returns></returns>
        //public int NumeroHijos()
        //{
        //    return this.m_xml.NumChildren;
        //}
        ///// <summary>
        ///// Comprobación para saber si tiene un subarbol dentro con la etiqueta pasada por parametro.
        ///// </summary>
        ///// <param name="etiqueta"></param>
        ///// <returns></returns>
        //public bool TieneSubarbol(string etiqueta)
        //{
        //    return this.m_xml.HasChildWithTag(etiqueta);
        //}
        ///// <summary>
        ///// Comprobación para saber si tiene un campo dentro con la etiqueta pasada por parametro.
        ///// </summary>
        ///// <param name="etiqueta"></param>
        ///// <returns></returns>
        //public bool TieneCampo(string valor)
        //{
        //    return this.m_xml.HasAttribute(valor);
        //}
        #endregion

		#region IDisposable Members
		/// <summary>
		/// Elimina los recursos utilizados
		/// </summary>
		public void Dispose()
		{
			if (this.m_escritorXml != null)
			{
				this.m_escritorXml.Close();
				this.m_escritorXml = null;
			}
			if (this.m_swXml != null)
			{
				this.m_swXml.Close();
				this.m_swXml.Dispose();
				this.m_swXml = null;
			}
            //if (this.m_xml != null)
            //{
            //    this.m_xml.Dispose();
            //    this.m_xml = null;
            //}
		}
		#endregion
    }
}
