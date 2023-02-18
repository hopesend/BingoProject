// ***********************************************************************
// Assembly         : BingoServer
// Author           : David Ferrandez Molla
// Created          : 24-01-2023
//
// Last Modified By : David Ferrandez Molla
// Last Modified On : 24-01-2023
// Last Modified On : 29-01-2023
// ***********************************************************************
// <copyright file="Ranking.cs" company="">
//     Copyright ©  2023
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace BingoServer.Classes
{
    /// <summary>
    /// Clase que simula un Ranking de juegador en la sesion
    /// </summary>
    public class Ranking
    {
        #region Properties

        /// <summary>
        /// Nombre del Jugador
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Numero de veces que a cantado Bingo
        /// </summary>
        public int Bingo { get; set; }

        /// <summary>
        /// Numero de veces que a cantado Linea
        /// </summary>
        public int Line { get; set; }

        #endregion Properties

        #region Override Methods

        /// <summary>
        /// reformatea el objeto a cadena de texto legible
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Jugador: {this.Name} - Bingos: {this.Bingo} - Lineas: {this.Line}";
        }

        /// <summary>
        /// reformatea el objeto para el envio del mensaje encriptado legible por el servidor
        /// </summary>
        /// <returns></returns>
        public string ToStringMessageSatinized()
        {
            return $"Jugador= {this.Name} - Bingos= {this.Bingo} - Lineas= {this.Line}";
        }

        #endregion Override Methods
    }
}