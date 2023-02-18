// ***********************************************************************
// Assembly         : BingoServer
// Author           : David Ferrandez Molla
// Created          : 24-01-2023
//
// Last Modified By : David Ferrandez Molla
// Last Modified On : 23-01-2023
// Last Modified On : 29-01-2023
// ***********************************************************************
// <copyright file="ServerManager.cs" company="">
//     Copyright ©  2023
// </copyright>
// <summary></summary>
// ***********************************************************************
using BingoClient.Events;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BingoClient.Classes
{
    /// <summary>
    /// Clase para la gestion del Servidor
    /// </summary>
    public class ServerManager : IDisposable
    {
        #region Variables

        /// <summary>
        /// Socket servidor
        /// </summary>
        private TcpClient socket;

        /// <summary>
        /// Ip del Servidor
        /// </summary>
        private readonly string ip;

        /// <summary>
        /// Puerto del Servidor
        /// </summary>
        private readonly int port;

        /// <summary>
        /// Stream para el envio de informacion al servidor
        /// </summary>
        private NetworkStream stream;

        #endregion Variables

        #region Eventos

        /// <summary>
        /// Evento para capturar un mensaje enviado al chat
        /// </summary>
        public event EventHandler<ChatMessageEventArgs> OnChatMessageSend;

        /// <summary>
        /// Evento para capturar cuando el servidor a generado una nueva bola en el juego
        /// </summary>
        public event EventHandler<BallSendEventArgs> OnBallSend;

        /// <summary>
        /// Evento para captura un mensaje enviado al sistema
        /// </summary>
        public event EventHandler<SystemMessageEventArgs> OnSystemMessageSend;

        /// <summary>
        /// Evento para capturar la desconexion de un cliente
        /// </summary>
        public event EventHandler<EventArgs> OnClientDisconnected;

        #endregion Eventos

        #region Constructor

        /// <summary>
        /// Constructor de Clase
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public ServerManager(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        #endregion Constructor

        #region Class Methods

        /// <summary>
        /// Conecta con el servidor y se mantiene a la escucha
        /// </summary>
        public bool ConnectToServer()
        {
            try
            {
                //Construye un nuevo socket
                this.socket = new TcpClient();
                this.socket.Connect(this.ip, this.port);
                this.stream = socket.GetStream();
                //Lanza la escucha
                this.ListenSocket();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Envia un mensaje al servidor, segun un tipo dado y un nombre de cliente
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userName"></param>
        /// <param name="message"></param>
        public void SendMessage(Utils.MessageType type, string userName, string message)
        {
            //Formatea el mensaje y lo envia
            string comMessage = Utils.CastMessage(type, userName, message);
            byte[] data = Encoding.ASCII.GetBytes(comMessage);
            this.stream.Write(data, 0, data.Length);
            this.stream.Flush();
        }

        /// <summary>
        /// Metodo para la escucha de mensajes del servidor
        /// </summary>
        public void ListenSocket()
        {
            //Inicia el hilo de escucha
            new Thread(() =>
            {
                for (; ; )
                {
                    try
                    {
                        //Si el cliente esta conectado escucha a la espera de mensaje
                        if (this.socket.Connected)
                        {
                            byte[] bytesFrom = new byte[1024];
                            this.stream.Read(bytesFrom, 0, 1024);
                            Tuple<Utils.MessageType, string, string> message = Utils.CastMessage(bytesFrom);
                            if(message != null)
                            {
                                this.ManageMessage(message);
                            }
                        }
                    }
                    catch
                    {
                        //Si el cliente desconecta, se emite el evento y si sale del for
                        OnClientDisconnected?.Invoke(null, EventArgs.Empty);
                        break;
                    }
                }
            }).Start();
        }

        /// <summary>
        /// Gestion el mensaje recibido del servidor
        /// </summary>
        /// <param name="message"></param>
        public void ManageMessage(Tuple<Utils.MessageType, string, string> message)
        {
            switch (message.Item1)
            {
                case Utils.MessageType.Chat:
                    {
                        OnChatMessageSend?.Invoke(null, new ChatMessageEventArgs(message.Item2, message.Item3));
                        break;
                    }
                case Utils.MessageType.System:
                    {
                        OnSystemMessageSend?.Invoke(null, new SystemMessageEventArgs(message.Item3));
                        break;
                    }
                case Utils.MessageType.Ball:
                    {
                        OnBallSend?.Invoke(null, new BallSendEventArgs(int.Parse(message.Item3)));
                        break;
                    }
            }
        }

        /// <summary>
        /// Checkea si el socket servidor sigue conectado
        /// </summary>
        /// <returns>true si el socket sigue conectado, false si esta desconectado</returns>
        public bool IsConnected() => this.socket.Connected;

        #endregion Class Methods

        #region IDisposable Implementation

        /// <summary>
        /// Termina la sesion, cerrando el socket y liberando
        /// </summary>
        public void Dispose()
        {
            this.socket.Close();
        }

        #endregion IDisposable Implementation
    }
}