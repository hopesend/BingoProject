// ***********************************************************************
// Assembly         : BingoServer
// Author           : David Ferrandez Molla
// Created          : 24-01-2023
//
// Last Modified By : David Ferrandez Molla
// Last Modified On : 24-01-2023
// Last Modified On : 29-01-2023
// ***********************************************************************
// <copyright file="IBallManager.cs" company="">
//     Copyright ©  2023
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace BingoClient.Interfaces
{
    /// <summary>
    /// Inteface compartido para Evento de nueva bola generada
    /// </summary>
    public interface IBallManager
    {
        /// <summary>
        /// Nueva bola generada
        /// </summary>
        /// <param name="ballNumber"></param>
        void NewBallActived(int ballNumber);
    }
}