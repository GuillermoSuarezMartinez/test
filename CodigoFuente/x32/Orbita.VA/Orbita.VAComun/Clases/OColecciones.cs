//***********************************************************************
// Assembly         : Orbita.VAComun
// Author           : aibañez
// Created          : 13-12-2012
//
// Last Modified By : 
// Last Modified On : 
// Description      : 
//
// Copyright        : (c) Orbita Ingenieria. All rights reserved.
//***********************************************************************

namespace Orbita.VAComun
{
    /// <summary>
    /// Clase utilizada para almacenar un par de valores
    /// </summary>
    /// <typeparam name="TFirst">Tipo del primer valor</typeparam>
    /// <typeparam name="TSecond">Tipo del segundo valor</typeparam>
    public class OPair<TFirst, TSecond>
    {
        #region Atributo(s)
        /// <summary>
        /// Primer valor
        /// </summary>
        public TFirst First;
        /// <summary>
        /// Segundo valor
        /// </summary>
        public TSecond Second;
        #endregion

        #region Constructor de la clase
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OPair()
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OPair(TFirst first, TSecond second)
        {
            this.First = first;
            this.Second = second;
        }
        #endregion
    }

    /// <summary>
    /// Clase utilizada para almacenar un trío de valores
    /// </summary>
    /// <typeparam name="TFirst">Tipo del primer valor</typeparam>
    /// <typeparam name="TSecond">Tipo del segundo valor</typeparam>
    /// <typeparam name="TThird">Tipo del tercer valor</typeparam>
    public class OTriplet<TFirst, TSecond, TThird>
    {
        #region Atributo(s)
        /// <summary>
        /// Primer valor
        /// </summary>
        public TFirst First;
        /// <summary>
        /// Segundo valor
        /// </summary>
        public TSecond Second;
        /// <summary>
        /// Tercer valor
        /// </summary>
        public TThird Third;
        #endregion

        #region Constructor de la clase
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OTriplet()
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OTriplet(TFirst first, TSecond second, TThird third)
        {
            this.First = first;
            this.Second = second;
            this.Third = third;
        }
        #endregion
    }

    /// <summary>
    /// Clase utilizada para almacenar un trío de valores
    /// </summary>
    /// <typeparam name="TFirst">Tipo del primer valor</typeparam>
    /// <typeparam name="TSecond">Tipo del segundo valor</typeparam>
    /// <typeparam name="TThird">Tipo del tercer valor</typeparam>
    /// <typeparam name="TFourth">Tipo del cuarto valor</typeparam>
    public class OQuartet<TFirst, TSecond, TThird, TFourth>
    {
        #region Atributo(s)
        /// <summary>
        /// Primer valor
        /// </summary>
        public TFirst First;
        /// <summary>
        /// Segundo valor
        /// </summary>
        public TSecond Second;
        /// <summary>
        /// Tercer valor
        /// </summary>
        public TThird Third;
        /// <summary>
        /// Cuarto valor
        /// </summary>
        public TFourth Fourth;
        #endregion

        #region Constructor de la clase
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OQuartet()
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OQuartet(TFirst first, TSecond second, TThird third, TFourth fourth)
        {
            this.First = first;
            this.Second = second;
            this.Third = third;
            this.Fourth = fourth;
        }
        #endregion
    }

    /// <summary>
    /// Clase utilizada para almacenar un trío de valores
    /// </summary>
    /// <typeparam name="TFirst">Tipo del primer valor</typeparam>
    /// <typeparam name="TSecond">Tipo del segundo valor</typeparam>
    /// <typeparam name="TThird">Tipo del tercer valor</typeparam>
    /// <typeparam name="TFourth">Tipo del cuarto valor</typeparam>
    /// <typeparam name="TFifth">Tipo del quinto valor</typeparam>
    public class OQuintet<TFirst, TSecond, TThird, TFourth, TFifth>
    {
        #region Atributo(s)
        /// <summary>
        /// Primer valor
        /// </summary>
        public TFirst First;
        /// <summary>
        /// Segundo valor
        /// </summary>
        public TSecond Second;
        /// <summary>
        /// Tercer valor
        /// </summary>
        public TThird Third;
        /// <summary>
        /// Cuarto valor
        /// </summary>
        public TFourth Fourth;
        /// <summary>
        /// Quinto valor
        /// </summary>
        public TFifth Fifth;
        #endregion

        #region Constructor de la clase
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OQuintet()
        {
        }
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public OQuintet(TFirst first, TSecond second, TThird third, TFourth fourth, TFifth fifth)
        {
            this.First = first;
            this.Second = second;
            this.Third = third;
            this.Fourth = fourth;
            this.Fifth = fifth;
        }
        #endregion
    }
}
