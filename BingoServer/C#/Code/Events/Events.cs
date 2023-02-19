// ***********************************************************************
// Assembly         : BingoServer
// Author           : David Ferrandez Molla
// Created          : 17-03-2022
//
// Last Modified By : David Ferrandez Molla
// Last Modified On : 17-01-2023
// Last Modified On : 29-01-2023
// ***********************************************************************
// <copyright file="Events.cs" company="">
//     Copyright ©  2023
// </copyright>
// <summary></summary>
// ***********************************************************************
using BingoServer.Classes;
using System;

namespace BingoServer
{
    /// <summary>
    /// Evento para emitir que un cliente se a desconectado
    /// </summary>
    public class ClientDisconnectEventArgs : EventArgs
    {
        /// <summary>
        /// ClientSocket del cliente desconectado
        /// </summary>
        public ClientSocket socket { get; set; }

        public ClientDisconnectEventArgs(ClientSocket socket)
        {
            this.socket = socket;
        }
    }

    /// <summary>
    /// Evento para emitir que el cliente a enviado un nuevo mensaje
    /// </summary>
    public class ClientMessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// ClientSocket del cliente que a enviado el mensaje
        /// </summary>
        public ClientSocket socket { get; set; }

        /// <summary>
        /// Mensaje Enviado
        /// </summary>
        public MessageSocket Message { get; set; }

        public ClientMessageReceivedEventArgs(ClientSocket socket, MessageSocket message)
        {
            this.socket = socket;
            this.Message = message;
        }
    }
}