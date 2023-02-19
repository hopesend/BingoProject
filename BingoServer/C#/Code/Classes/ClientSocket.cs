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
using System.Text;
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

        public string Name
        {
            get => name;
            set
            {
                name = value;
                this.ranking = new Ranking { Name = name };
            }
        }

        /// <summary>
        /// Socket del cliente
        /// </summary>
        private TcpClient socket;

        public TcpClient Socket { get => socket; set => socket = value; }

        private Ranking ranking;

        /// <summary>
        /// Hilo para la escucha de mensajes del cliente
        /// </summary>
        private Thread threadListen;

        private NetworkStream stream;

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
            this.stream = socket.GetStream();
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
                while (true)
                {
                    try
                    {
                        //Hasta que haya informacion en el socket creamos un ciclo infinito
                        while (!this.stream.DataAvailable) ;
                        //Capturamos el mensaje
                        byte[] bytesFrom = new byte[this.socket.Available];
                        stream.Read(bytesFrom, 0, bytesFrom.Length);
                        //Checkea que el mensaje no sea un mensaje vacio
                        if (bytesFrom.Length != 0)
                        {
                            //Desencripta el mensaje y checkea que sea un mensaje para (BingoProject)
                            string decryptMessage = Security.Decrypt(Encoding.UTF8.GetString(bytesFrom));
                            if (!string.IsNullOrEmpty(decryptMessage) && decryptMessage.StartsWith("(BingoProject)"))
                            {
                                MessageSocket.SatinizeMessage(decryptMessage)
                                    .ForEach(x => OnClientMessageReceived?.Invoke(this, new ClientMessageReceivedEventArgs(this, x)));
                            }
                        }
                    }
                    catch (Exception ex)
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