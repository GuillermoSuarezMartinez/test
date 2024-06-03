//***********************************************************************
// Assembly         : OrbitaUtiles
// Author           : ahidalgo
// Created          : 03-03-2011
//
// Last Modified By : ahidalgo
// Last Modified On : 03-03-2011
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
namespace Orbita.Utiles
{
    /// <summary>
    /// Clase OAlmacenXML
    /// </summary>
    [Serializable]
    public class OAlmacenXML: ICloneable
    {
        #region Atributo(s)
        /// <summary>
        /// Ruta del fichero xml.
        /// </summary>
        string _rutaFichero;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Inicializar una nueva instancia de la clase OAlmacenXML.
        /// </summary>
        public OAlmacenXML()
        {
            // Constructor vacío
        }
        /// <summary>
        /// Inicializar una nueva instancia de la clase OAlmacenXML.
        /// </summary>
        /// <param name="rutaFichero">Ruta del fichero de configuracion xml.</param>
        public OAlmacenXML(string rutaFichero)
        {
            this._rutaFichero = rutaFichero;
        }
        #endregion

        #region Destructor(es)
        /// <summary>
        /// Indica si ya se llamo al método Dispose. (default = false)
        /// </summary>
        bool disposed = false;
        /// <summary>
        /// Implementa IDisposable.
        /// No  hacer  este  método  virtual.
        /// Una clase derivada no debería ser
        /// capaz de  reemplazar este método.
        /// </summary>
        public void Dispose()
        {
            // Llamo al método que  contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
            // Este objeto será limpiado por el método Dispose.
            // Llama al método del recolector de basura, GC.SuppressFinalize.
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Método  sobrecargado de  Dispose que será  el que
        /// libera los recursos. Controla que solo se ejecute
        /// dicha lógica una  vez y evita que el GC tenga que
        /// llamar al destructor de clase.
        /// </summary>
        /// <param name="disposing">Indica si llama al método Dispose.</param>
        protected virtual void Dispose(bool disposing)
        {
            // Preguntar si Dispose ya fue llamado.
            if (!this.disposed)
            {
                // Marcar como desechada ó desechandose,
                // de forma que no se puede ejecutar el
                // código dos veces.
                disposed = true;
            }
        }
        /// <summary>
        /// Destructor(es) de clase.
        /// En caso de que se nos olvide “desechar” la clase,
        /// el GC llamará al destructor, que tambén ejecuta 
        /// la lógica anterior para liberar los recursos.
        /// </summary>
        ~OAlmacenXML()
        {
            // Llamar a Dispose(false) es óptimo en terminos
            // de legibilidad y mantenimiento.
            Dispose(false);
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Cargamos los datos del XML
        /// </summary>
        /// <returns>Objeto OAlmacenXML</returns>
        public OAlmacenXML CargarDatos()
        {
            // Si no existe el fichero de configuracion lanzamos una excepción.
            if (!File.Exists(this._rutaFichero))
            {
                throw new FileNotFoundException("No se encuentra el fichero de configuración", this._rutaFichero);
            }
            // Leemos los datos deserializando el objeto OAlmacenXML.
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            using (StreamReader reader = new StreamReader(this._rutaFichero))
            {
                return (OAlmacenXML)serializer.Deserialize(reader);
            }
        }
        /// <summary>
        /// Guardamos los datos en el XML
        /// </summary>
        public virtual void Guardar()
        {
            // Guardamos la serialización del objeto en el fichero.
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            using (StreamWriter writer = new StreamWriter(this._rutaFichero))
            {
                serializer.Serialize(writer, this);
            }
        }
        #endregion

        #region Miembros de ICloneable
        /// <summary>
        /// Clona el objeto OAlmacenXML.
        /// </summary>
        /// <returns>Objeto OAlmacenXML clonado.</returns>
        public object Clone()
        {
            MemoryStream m = new MemoryStream();
            BinaryFormatter b = new BinaryFormatter();

            b.Serialize(m, this);
            m.Position = 0;
            return b.Deserialize(m);
        }
        #endregion
    }
}
