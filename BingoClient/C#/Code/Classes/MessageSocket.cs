using System;
using System.Collections.Generic;
using System.Linq;

namespace BingoClient.Classes
{
    /// <summary>
    /// Tipo de mensaje enviado o recibido
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Tipo Chat
        /// </summary>
        Chat,

        /// <summary>
        /// Tipo Sistema
        /// </summary>
        System,

        /// <summary>
        /// Tipo Bola
        /// </summary>
        Ball
    }

    /// <summary>
    /// Clase manejador del mensaje que se transmite/recibe mediante sockets
    /// </summary>
    public class MessageSocket
    {
        // Tipo de Mensaje
        public MessageType Type { get; set; }

        // Usuario que lanza el mensaje
        public string UserName { get; set; }

        // Mensaje que lanza el usuario
        public string Message { get; set; }

        /// <summary>
        /// Constructor para lanzar mensaje
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userName"></param>
        /// <param name="message"></param>
        public MessageSocket(MessageType type, string userName, string message)
        {
            this.Type = type;
            this.UserName = userName;
            this.Message = message;
        }

        /// <summary>
        /// Constructor para recibir mensaje
        /// </summary>
        /// <param name="message"></param>
        public MessageSocket(string message)
        {
            message = message.Replace("(startM)", "").Replace("(endM)", "").Replace("(BingoProject)", "");
            string[] messageSplit = message.Split(new string[] { "(endB)" }, StringSplitOptions.None);
            this.Type = (MessageType)Enum.Parse(typeof(MessageType), messageSplit[0].Replace("(startB)", string.Empty));
            this.UserName = messageSplit[1].Replace("(startB)", string.Empty);
            this.Message = messageSplit[2].Replace("(startB)", string.Empty);
        }

        /// <summary>
        /// Maqueta el mensaje para su envio
        /// </summary>
        /// <returns>cadena de texto maquetada</returns>
        public string ConstructMessage() => $"(BingoProject)(startM)(startB){this.Type}(endB)(startB){this.UserName}(endB)(startB){this.Message}(endB)(endM)";

        /// <summary>
        /// Disecciona el mensaje recibido
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static List<MessageSocket> SatinizeMessage(string message)
        {
            List<MessageSocket> messages = new List<MessageSocket>();
            foreach (string messageClean in message.Split(new string[] { "(endM)" }, StringSplitOptions.None).Where(x => !string.IsNullOrEmpty(x)))
            {
                messages.Add(new MessageSocket(messageClean + "(endM)"));
            }
            return messages;
        }
    }
}