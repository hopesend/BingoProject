// ***********************************************************************
// Assembly         : BingoServer
// Author           : David Ferrandez Molla
// Created          : 17-01-2023
//
// Last Modified By : David Ferrandez Molla
// Last Modified On : 17-01-2023
// Last Modified On : 24-01-2023
// Last Modified On : 29-01-2023
// ***********************************************************************
// <copyright file="ClienteSocket.cs" company="">
//     Copyright ©  2023
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Net.Sockets;
using System.Threading;

namespace BingoServer.Classes
{
    /// <summary>
    /// Clase que gestiona un cliente conectado a la sesion
    /// </summary>
    public class ClientSocket : IDisposable
    {
        #region Variables

        /// <summary>
        /// Nombre del jugador
        /// </summary>
        private string name;

        public string Name { get => name; set => name = value; }

        /// <summary>
        /// Socket del cliente
        /// </summary>
        private TcpClient socket;

        public TcpClient Socket { get => socket; set => socket = value; }

        private Ranking ranking;
        /// <summary>
        /// Numero de bingos en la partida
        /// </summary>
        //private int Bingo;

        /// <summary>
        /// Numero de lineas en la partida
        /// </summary>
        //private int Line;

        /// <summary>
        /// Hilo para la escucha de mensajes del cliente
        /// </summary>
        private Thread threadListen;

        #endregion Variables

        #region Events

        /// <summary>
        /// Evento para emitir que el cliente se a desconectado
        /// </summary>
        public event EventHandler<ClientDisconnectEventArgs> OnClientDisconnected;

        /// <summary>
        /// Evento para emitir que el cliente a enviado un mensaje
        /// </summary>
        public event EventHandler<ClientMessageReceivedEventArgs> OnClientMessageReceived;

        #endregion Events

        #region Constructor

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="name"></param>
        /// <param name="socket"></param>
        public ClientSocket(string name, TcpClient socket)
        {
            this.Name = name;
            this.Socket = socket;
            this.ranking = new Ranking { Name = name };
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Metodo para la escucha de mensajes emitidos por el cliente al servidor
        /// </summary>
        public void ListenSocket()
        {
            //Creamos el hilo de escucha
            this.threadListen = new Thread(() =>
            {
                for (; ; )
                {
                    try
                    {
                        //Si el socket esta conectado escuchamos
                        if (this.socket.Connected)
                        {
                            //espera hasta la captura de un nuevo mensaje
                            byte[] bytesFrom = new byte[1024];
                            NetworkStream networkStream = this.socket.GetStream();
                            networkStream.Read(bytesFrom, 0, 1024);
                            //Limpia el mensaje y si es un mensaje formateado correctamente emite el evento
                            Tuple<Utils.MessageType, string, string> message = Utils.CastMessage(bytesFrom);
                            if (message != null)
                            {
                                OnClientMessageReceived?.Invoke(null, new ClientMessageReceivedEventArgs(this, message.Item3));
                            }
                        }
                    }
                    catch
                    {
                        //Si se desconecta el cliente rompemos el bucle y emitimos el evento
                        OnClientDisconnected?.Invoke(null, new ClientDisconnectEventArgs(this));
                        break;
                    }
                }
            });
            this.threadListen.Start();
        }

        /// <summary>
        /// Añade un nuevo bingo al usuario
        /// </summary>
        public void SetBingo() => this.ranking.Bingo += 1;

        /// <summary>
        /// Añade una nueva linea al usuario
        /// </summary>
        public void SetLinea() => this.ranking.Line += 1;

        /// <summary>
        /// Obtiene los bingos del usuario en la sesion
        /// </summary>
        /// <returns></returns>
        public int GetBingo() => this.ranking.Bingo;

        /// <summary>
        /// Obtiene todas las lineas del usuario en la sesion
        /// </summary>
        /// <returns></returns>
        public int GetLine() => this.ranking.Line;

        /// <summary>
        /// Obtiene el ranking asociado al socket de usario
        /// </summary>
        /// <returns></returns>
        public Ranking GetRanking() => this.ranking;

        /// <summary>
        /// Inserta un nuevo Ranking al socket de usuario
        /// </summary>
        /// <param name="newRanking"></param>
        public void SetRanking(Ranking newRanking) => this.ranking = newRanking;

        #endregion Methods

        #region IDisposable Implementation

        /// <summary>
        /// Cierra el socket del cliente y libera
        /// </summary>
        public void Dispose()
        {
            //Cerramos el socket
            this.socket.Close();
        }

        #endregion IDisposable Implementation
    }
}