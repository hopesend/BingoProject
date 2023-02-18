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
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BingoServer.Classes
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

        //Patron de Encriptacion/Desencriptacion
        private static readonly string patron = "dab=MB=v9/vDGwd-r61voC2Ayhap73unY!h?B?f2iHF6GYM8PKzZFZ2=ATRhUvThfsK0dfe9qKR0KBHcrcOEoVUY6KEs0d!cVh0KNQAn--SPz57l6YauYHtKgxE5LwOtRcAAJEFCOxv5uTC!E8Ln-e/8rwY4ghDhgb0!J9Q0jezwt9X8Jy!O7GbiuGoLqjyRHncccHa!ETX4o!Ot-NIy/RZd6o4!JE/rqO5ky5bttwKhpP8t4MtPUki6H5BTJ20H";

        /// <summary>
        /// Convierte el mensaje y sus parametros en un mensaje legible por el socket tanto en el cliente como en el servidor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userName"></param>
        /// <param name="message"></param>
        /// <returns>mensaje convertido</returns>
        public static string CastMessage(MessageType type, string userName, string message) => Encrypt($"#{type}#:#{userName}#:#{message}#");

        /// <summary>
        /// Desencripta el mensaje y lo secciona para su utilizacion
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Tupla de tipo de mensaje, nombre de usuario y mensaje</returns>
        public static Tuple<MessageType, string, string> CastMessage(string message)
        {
            string messageDecrypt = Decrypt(message);
            return Tuple.Create((MessageType)Enum.Parse(typeof(MessageType), messageDecrypt.Split(':').First().TrimStart('#').TrimEnd('#')),
                                messageDecrypt.Split(':')[1].TrimStart('#').TrimEnd('#'),
                                messageDecrypt.Split(':').Last().TrimStart('#').TrimEnd('#'));
        }

        /// <summary>
        /// Elimina todos los caracteres invalidos en la recepcion del mensaje
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>string limpio para su desencriptacion</returns>
        
        public static Tuple<MessageType, string, string> CastMessage(byte[] bytes)
        {
            string message = Encoding.UTF8.GetString(bytes, 0, 1024).TrimEnd('\0');
            if(string.IsNullOrEmpty(message)) { return null; }
            return CastMessage(message);
        }

        /// <summary>
        /// Encripta un texto mediante el patron de encriptacion
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        private static string Encrypt(string plainText)
        {
            if (plainText == null) { return null; }

            //Obtiene los bytes tanto del texto como del patron de encriptacion
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(patron);

            //Crea un clave Hash a partir del patron con SHA256
            passwordBytes = SHA512.Create().ComputeHash(passwordBytes);

            //Genera la encriptacion del texto con la clave hash
            byte[] bytesEncrypted = Encrypt(bytesToBeEncrypted, passwordBytes);

            //Y lo devuelve como texto plano
            return Convert.ToBase64String(bytesEncrypted);
        }

        /// <summary>
        /// Desencripta un texto mediante el patron de encriptacion
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <returns></returns>
        private static string Decrypt(string encryptedText)
        {
            if (encryptedText == null) { return null; }

            //Obtiene los bytes tanto del texto como del patron de encriptacion
            byte[] bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(patron);

            //Crea un clave Hash a partir del patron con SHA256
            passwordBytes = SHA512.Create().ComputeHash(passwordBytes);

            //Desencripta el texto mediante el patron
            byte[] bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes);

            //devuelve el resultado en texto plano
            return Encoding.UTF8.GetString(bytesDecrypted);
        }

        /// <summary>
        /// Encripta una cadena de bytes mediante patron hash
        /// </summary>
        /// <param name="bytesToBeEncrypted"></param>
        /// <param name="passwordBytes"></param>
        /// <returns></returns>
        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (CryptoStream cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        /// <summary>
        /// Desencripta una cadena de bytes mediante patron hash
        /// </summary>
        /// <param name="bytesToBeDecrypted"></param>
        /// <param name="passwordBytes"></param>
        /// <returns></returns>
        private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;

                    using (CryptoStream cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }

                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
    }
}