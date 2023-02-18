using System;

namespace BingoClient.Events
{
    /// <summary>
    /// Evento para Emitir que un nuevo mensaje de chat se a capturado
    /// </summary>
    public class ChatMessageEventArgs : EventArgs
    {
        /// <summary>
        /// Usuario del mensaje
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Mensaje
        /// </summary>
        public string Message { get; set; }

        public ChatMessageEventArgs(string user, string message)
        {
            this.User = user;
            this.Message = message;
        }
    }

    /// <summary>
    /// Evento para emitir que una nueva bola a sido generada por el servidor para el juego
    /// </summary>
    public class BallSendEventArgs : EventArgs
    {
        /// <summary>
        /// El numero de la nueva bola emitida
        /// </summary>
        public int Ball { get; set; }

        public BallSendEventArgs(int ball)
        {
            this.Ball = ball;
        }
    }

    /// <summary>
    /// Evento para emitir que una nuevo mensaje de sistema a sido capturado
    /// </summary>
    public class SystemMessageEventArgs : EventArgs
    {
        /// <summary>
        /// Mensaje para el sistema
        /// </summary>
        public string Message { get; set; }

        public SystemMessageEventArgs(string message)
        {
            this.Message = message;
        }
    }
}