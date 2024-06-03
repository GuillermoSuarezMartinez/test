namespace Orbita.Comunicaciones
{
    /// <summary>
    /// Clase que encapsula información de un puerto de comunicaciones
    /// </summary>
    public class OInformacionPuerto
    {
        #region Atributos
        /// <summary>
        /// Identificador númerico del puerto de comunicaciones
        /// </summary>
        int _idPuerto;
        /// <summary>
        /// Tipo del puerto de comunicaciones
        /// </summary>
        string _tipoPuerto;
        #endregion

        #region Constructores
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="idPuerto">Identificador númerico del puerto de comunicaciones</param>
        /// <param name="tipoPuerto">Tipo del puerto de comunicaciones</param>
        public OInformacionPuerto(int idPuerto, string tipoPuerto)
        {
            this._idPuerto = idPuerto;
            this._tipoPuerto = tipoPuerto;
        }
        #endregion

        #region Propiedades
        /// <summary>
        /// Identificador númerico del puerto de comunicaciones
        /// </summary>
        public int IdPuerto
        {
            get { return this._idPuerto; }
        }
        /// <summary>
        /// Identificador textual del puerto de comunicaciones
        /// </summary>
        public string TipoPuerto
        {
            get { return this._tipoPuerto; }
        }
        #endregion
    }
}
