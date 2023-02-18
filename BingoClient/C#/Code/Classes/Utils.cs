// ***********************************************************************
// Assembly         : BingoServer
// Author           : David Ferrandez Molla
// Created          : 24-01-2023
//
// Last Modified By : David Ferrandez Molla
// Last Modified On : 24-01-2023
// Last Modified On : 29-01-2023
// ***********************************************************************
// <copyright file="Utils.cs" company="">
//     Copyright ©  2023
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Linq;
using System.Text;

namespace BingoClient.Classes
{
    /// <summary>
    /// Clase para la gestion de mensajes
    /// Formato de Mensaje: #Tipo#:#Usuario#:#Mensaje#
    /// </summary>
    public static class Utils
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
        /// Convierte el mensaje y sus parametros en un mensaje legible por el socket tanto en el cliente como en el servidor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userName"></param>
        /// <param name="message"></param>
        /// <returns>mensaje convertido</returns>
        public static string CastMessage(MessageType type, string userName, string message) => $"#{type}#:#{userName}#:#{message}#";

        /// <summary>
        /// Desencripta el mensaje y lo secciona para su utilizacion
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Tupla de tipo de mensaje, nombre de usuario y mensaje</returns>
        public static Tuple<MessageType, string, string> CastMessage(string message)
        {
            return Tuple.Create((MessageType)Enum.Parse(typeof(MessageType), message.Split(':').First().TrimStart('#').TrimEnd('#')),
                                message.Split(':')[1].TrimStart('#').TrimEnd('#'),
                                message.Split(':').Last().TrimStart('#').TrimEnd('#'));
        }

        /// <summary>
        /// Elimina todos los caracteres invalidos en la recepcion del mensaje
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>string limpio para su desencriptacion</returns>
        public static string SatinizeBytes(byte[] bytes) => Encoding.UTF8.GetString(bytes, 0, 1024).TrimEnd('\0');
    }
}