using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public class MessageSocket
    {
        public MessageType Type { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }

        public MessageSocket(MessageType type, string userName, string message)
        {
            this.Type = type;
            this.UserName = userName;
            this.Message = message;
        }

        public MessageSocket(string message)
        {
            message = message.TrimStart('(').TrimEnd(')');
            this.Type = (MessageType)Enum.Parse(typeof(MessageType), message.Split(':').First().TrimStart('#').TrimEnd('#'));
            this.UserName = message.Split(':')[1].TrimStart('#').TrimEnd('#');
            this.Message = message.Split(':').Last().TrimStart('#').TrimEnd('#');
        }

        public string ConstructMessage() => $"(#{this.Type}#:#{this.UserName}#:#{this.Message}#)";

        public static List<MessageSocket> SatinizeMessage(string message)
        {
            List<MessageSocket> messages = new List<MessageSocket>();
            foreach (string messageClean in message.Split(')').Where(x => !string.IsNullOrEmpty(x)))
            {
                messages.Add(new MessageSocket(messageClean + ")"));
            }
            return messages;
        }
    }
}
