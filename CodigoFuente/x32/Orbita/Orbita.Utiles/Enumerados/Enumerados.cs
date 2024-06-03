namespace Orbita.Utiles
{
    /// <summary>
    /// Enumeración que representa la sobrecarga del
    /// método de tiempo restante de la clase OTimer.
    /// </summary>
    public enum Tiempo
    {
        /// <summary>
        /// Hora.
        /// </summary>
        Hora,
        /// <summary>
        /// Minuto.
        /// </summary>
        Minuto,
        /// <summary>
        /// Segundo.
        /// </summary>
        Segundo,
        /// <summary>
        /// Milisegundo.
        /// </summary>
        Milisegundo
    }
    /// <summary>
    /// Enumeración que representa las posibles 
    /// extensiones que puede tener un fichero.
    /// </summary>
    public enum Extension
    {
        /// <summary>
        /// Extensión .XML.
        /// </summary>
        Xml = 0,
        /// <summary>
        /// Extensión .JPG.
        /// </summary>
        Jpg = 1,
    }
    /// <summary>
    /// Enumeración que representa las acciones
    /// propias correspondientes a hilos.
    /// </summary>
    public enum Accion
    {
        /// <summary>
        /// Hilo inicializado.
        /// </summary>
        Inicializado,
        /// <summary>
        /// Hilo iniciado.
        /// </summary>
        Iniciado,
        /// <summary>
        /// Hilo suspendido.
        /// </summary>
        Suspendido,
        /// <summary>
        /// Hilo reanudado.
        /// </summary>
        Reanudado,
        /// <summary>
        /// Hilo terminado.
        /// </summary>
        Terminado,
        /// <summary>
        /// Join.
        /// </summary>
        EnEspera
    }
}

