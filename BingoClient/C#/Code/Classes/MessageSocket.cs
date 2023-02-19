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
            message = message.Replace("(startM)", "").Replace("(endM)", "").Replace("(BingoProject)", "");
            string[] messageSplit = message.Split(new string[] { "(endB)" }, StringSplitOptions.None);
            this.Type = (MessageType)Enum.Parse(typeof(MessageType), messageSplit[0].Replace("(startB)", string.Empty));
            this.UserName = messageSplit[1].Replace("(startB)", string.Empty);
            this.Message = messageSplit[2].Replace("(startB)", string.Empty);
        }

        public string ConstructMessage() => $"(BingoProject)(startM)(startB){this.Type}(endB)(startB){this.UserName}(endB)(startB){this.Message}(endB)(endM)";

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
