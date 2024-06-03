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
namespace Orbita.Utiles
{
    /// <summary>
    /// Queue sincronized .NET.
    /// </summary>
    public class OColaSincronizada : OCola
    {
        #region Atributo(s)
        /// <summary>
        /// Atributo estático de sincronización.
        /// </summary>
        static OResetManual sResetManual;
        #endregion

        #region Constructor(es)
        /// <summary>
        /// Inicializar una nueva instancia de la clase OColaSincronizada.
        /// </summary>
        public OColaSincronizada() { }
        /// <summary>
        /// Inicializar una nueva instancia de la clase OColaSincronizada.
        /// </summary>
        /// <param name="resetManual">Atributo de sincronización.</param>
        public OColaSincronizada(OResetManual resetManual)
            : base()
        {
            sResetManual = resetManual;
        }
        #endregion

        #region Propiedad(es)
        /// <summary>
        /// Atributo de sincronización.
        /// </summary>
        public OResetManual ResetManual
        {
            set { sResetManual = value; }
        }
        #endregion

        #region Método(s) público(s)
        /// <summary>
        /// Método que encola un objeto.
        /// </summary>
        /// <param name="sender">Objeto a encolar.</param>
        public override void Encolar(object sender)
        {
            Encolar(sender, 0);
        }
        /// <summary>
        /// Método que encola un objeto.
        /// </summary>
        /// <param name="sender">Objeto a encolar.</param>
        /// <param name="indice">Identificador de evento
        /// reset manual de sincroniazación.</param>
        public void Encolar(object sender, short indice)
        {
            // Bloquear la cola..
            lock (this.Cola.SyncRoot)
            {
                // Encolar el objeto.
                this.Cola.Enqueue(sender);

                if (sResetManual != null)
                {
                    // Despertar mediante el mandato Set.
                    sResetManual.Despertar(indice);
                }
            }
        }
        /// <summary>
        /// Método que desencola un objeto.
        /// </summary>
        /// <returns>Objeto encolado.</returns>
        public override object Desencolar()
        {
            return Desencolar(0);
        }
        /// <summary>
        /// Método que desencola un objeto.
        /// </summary>
        /// <param name="indice">Indice de evento
        /// reset manual de sincroniazación.</param>
        /// <returns>Objeto encolado.</returns>
        public object Desencolar(short indice)
        {
            // Bloquear la cola.
            lock (this.Cola.SyncRoot)
            {
                object objetoEncolado = null;
                if (this.Cola.Count > 0)
                {
                    // Desencolar el objeto.
                    objetoEncolado = this.Cola.Dequeue();
                }
                else
                {
                    if (sResetManual != null)
                    {
                        // Reset para la siguiente vez que se mande un Set.
                        // Es necesario invocarlo por haber utilizado la 
                        // clase ManualResetEvent.
                        sResetManual.Resetear(indice);
                    }
                }
                return objetoEncolado;
            }
        }
        #endregion
    }
}
