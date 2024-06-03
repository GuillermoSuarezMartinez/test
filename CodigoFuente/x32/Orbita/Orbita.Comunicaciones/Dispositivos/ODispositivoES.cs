using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Security.Permissions;
using System.Threading;
using System.Data;
using Orbita.Trazabilidad;
using Orbita.Utiles;
using Orbita.Winsock;


namespace Orbita.Comunicaciones
{
    /// <summary>
    /// Clase base para los dispositivos de ES
    /// </summary>
    public class ODispositivoES : ODispositivo
    {

        #region Atributos
        /// <summary>
        /// Atributo que indica las  colecciones
        /// de tags de datos, lecturas y alarmas.
        /// </summary>
        public OTags Tags;
        /// <summary>
        /// Colección de hilos.
        /// </summary>
        public static OHilos Hilos;
        /// <summary>
        /// Datos de configuración del canal
        /// </summary>
        public OConfigDispositivo Config;
        /// <summary>
        /// Argumentos para generar los eventos
        /// </summary>
        public OEventArgs OEventargs;
        /// <summary>
        /// Objeto para establecer el canal TCP
        /// </summary>
        public OWinsockBase Winsock;
        /// <summary>
        /// Segundos para la reconexión con el dispositivo
        /// </summary>
        public decimal SegReconexion = 1;
        /// <summary>
        /// Valor devuelto para las entradas
        /// </summary>
        public byte[] Entradas;
        /// <summary>
        /// Valor devuelto para las salidas
        /// </summary>
        public byte[] Salidas;
        #endregion

        #region Constructores
        /// <summary>
        /// Inicializar una nueva instancia de la clase de ES.
        /// </summary>
        public ODispositivoES(OTags tags, OHilos hilos, ODispositivo dispositivo)
        {
            LogManager.GetLogger("wrapper").Info("Creando dispositivo ODispositivoES");

            try
            {
                // Asignación de las colecciones de datos, lecturas y alarmas.
                this.Tags = tags;
                this.Config = tags.Config;
                this.OEventargs = new OEventArgs();
                //Actualizando las variables de dispositivo
                this.Identificador = dispositivo.Identificador;
                this.Nombre = dispositivo.Nombre;
                this.Tipo = dispositivo.Tipo;
                this.Direccion = dispositivo.Direccion;
                this.Local = dispositivo.Local;
                this.Puerto = dispositivo.Puerto;
                this.Protocolo = dispositivo.Protocolo;
                Hilos = hilos;
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("wrapper").Error("Error en ODispositivoTCP. ", ex);
                throw ex;
            }

        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Colección de datos.
        /// </summary>
        public OHashtable Datos
        {
            get { return this.Tags.GetDatos(); }
        }
        /// <summary>
        /// Colección de lecturas.
        /// </summary>
        public OHashtable Lecturas
        {
            get { return this.Tags.GetLecturas(); }
        }
        /// <summary>
        /// Colección de alarmas.
        /// </summary>
        public OHashtable Alarmas
        {
            get { return this.Tags.GetAlarmas(); }
        }
        /// <summary>
        /// Colección de alarmas activas.
        /// </summary>
        public ArrayList AlarmasActivas
        {
            get { return this.Tags.GetAlarmasActivas(); }
        }
        #endregion

        #region Métodos públicos
        /// <summary>
        /// Inicializar grupos, punteros e items.
        /// </summary>
        public override void Iniciar()
        {
            this.InicHiloVida();
        }
        /// <summary>
        /// Devuelva las alarmas activas del sistema
        /// </summary>
        /// <returns></returns>
        public override ArrayList GetAlarmasActivas()
        {
            return this.Tags.GetAlarmasActivas();
        }
        /// <summary>
        /// Devuelve los datos del dipositivo y su valor
        /// </summary>
        /// <returns></returns>
        public override OHashtable GetDatos()
        {
            return this.Tags.GetDatos();
        }
        /// <summary>
        /// Devuelve las lectuas del dipositivo y su valor
        /// </summary>
        /// <returns></returns>
        public override OHashtable GetLecturas()
        {
            return this.Tags.GetLecturas();
        }
        /// <summary>
        /// Devuelve las alarmas del dipositivo y su valor
        /// </summary>
        /// <returns></returns>
        public override OHashtable GetAlarmas()
        {
            return this.Tags.GetAlarmas();
        }
        /// <summary>
        /// Proceso del hilo de vida.
        /// </summary>
        public virtual void ProcesarHiloVida()
        {

        }
        /// <summary>
        /// Limpia los objetos en memoria
        /// </summary>
        /// <param name="disposing"></param>
        public override void Dispose(bool disposing)
        {
            this.Tags.Dispose();
            Hilos.Destruir();
        }
        #endregion

        #region Métodos privados
        /// <summary>
        /// Inicia hilo de vida.
        /// </summary>
        private void InicHiloVida()
        {
            // Crear el objeto Hilo e iniciarlo. El parámetro iniciar indica
            // a la colección que una vez añadido el hilo se iniciado.
            Hilos.Add(new ThreadStart(ProcesarHiloVida), true);
        }

        #endregion

    }
}