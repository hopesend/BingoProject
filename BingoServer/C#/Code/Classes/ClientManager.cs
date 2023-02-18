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
// <copyright file="ClientManager.cs" company="">
//     Copyright ©  2023
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace BingoServer.Classes
{
    /// <summary>
    /// Clase gestora de los clientes conectados al servidor
    /// </summary>
    public class ClientManager
    {
        #region Variables

        /// <summary>
        /// Coleccion de clientes conectados al servidor
        /// </summary>
        private List<ClientSocket> sockets = new List<ClientSocket>();

        #endregion Variables

        #region Events

        /// <summary>
        /// Evento para emitir que un cliente se a desconectado
        /// </summary>
        public event EventHandler<ClientDisconnectEventArgs> OnClientDisconnected;

        /// <summary>
        /// Evento para emitir que un cliente a enviado un mensaje
        /// </summary>
        public event EventHandler<ClientMessageReceivedEventArgs> OnClientMessageReceived;

        #endregion Events

        #region Constructor

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public ClientManager()
        {
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Añade un cliente a la coleccion
        /// </summary>
        /// <param name="socket"></param>
        public void AddClient(ClientSocket socket)
        {
            //Nos subscribimos a sus eventos
            socket.OnClientDisconnected += Socket_OnClientDisconnected;
            socket.OnClientMessageReceived += Socket_OnClientMessageReceived;
            //Mantenemos el socket a la escucha
            socket.ListenSocket();
            //Añadimos el socket a la coleccion
            this.sockets.Add(socket);
        }

        /// <summary>
        /// Emite que un cliente a enviado un mensaje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Socket_OnClientMessageReceived(object sender, ClientMessageReceivedEventArgs e)
        {
            OnClientMessageReceived?.Invoke(null, e);
        }

        /// <summary>
        /// Emite que un cliente se a desconectado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Socket_OnClientDisconnected(object sender, ClientDisconnectEventArgs e)
        {
            //eliminamos el socket de la coleccion
            this.sockets.Remove(this.sockets.Find(x => x.Name == e.socket.Name));
            //lo liberamos
            e.socket.Dispose();
            //emitimos el evento
            OnClientDisconnected?.Invoke(null, e);
        }

        /// <summary>
        /// Obtiene el numero de clientes de la coleccion
        /// </summary>
        /// <returns></returns>
        public int GetCount() => this.sockets.Count;

        /// <summary>
        /// Obtiene los sockets de los clientes conectados
        /// </summary>
        /// <returns></returns>
        public List<TcpClient> GetClients() => this.sockets.Select(x => x.Socket).ToList();

        /// <summary>
        /// Envia un mensaje a todos los clientes de la coleccion
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userName"></param>
        /// <param name="message"></param>
        public void SendMessage(Utils.MessageType type, string userName, string message)
        {
            try
            {
                //Iteramos pro todos los sockets de los clientes conectados
                foreach (TcpClient clientSocket in this.GetClients())
                {
                    //Casteamos el mensaje y lo mandamos
                    NetworkStream broadcastStream = clientSocket.GetStream();
                    byte[] broadcastBytes = Encoding.ASCII.GetBytes(Utils.CastMessage(type, userName, message));
                    broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                    broadcastStream.Flush();
                }
            }
            catch
            {
                Console.WriteLine("Error enviando informacion");
            }
        }

        /// <summary>
        /// Para todos los sockets clientes de la sesion
        /// </summary>
        public void Stop()
        {
            this.sockets.ForEach(x => x.Socket.Close());
        }

        /// <summary>
        /// Busca un cliente en la coleccion y le añade un bingo
        /// </summary>
        /// <param name="name"></param>
        public void SetBingo(string name) => this.sockets.Find(x => x.Name == name).SetBingo();

        /// <summary>
        /// Busca un cliente en la coleccion y le añade una linea
        /// </summary>
        /// <param name="name"></param>
        public void SetLine(string name) => this.sockets.Find(x => x.Name == name).SetLinea();

        /// <summary>
        /// Obtiene los rankings de la sesion
        /// </summary>
        /// <returns></returns>
        public List<Ranking> GetRankings()
        {
            //Construimos un nuevo ranking a partir de los clientes conectados
            return this.sockets.Select(x => x.GetRanking()).ToList();
        }

        /// <summary>
        /// Reparte los rankings globales
        /// </summary>
        /// <param name="rankings"></param>
        public void SetRankings(List<Ranking> rankings)
        {
            //Asigna el ranking global a los clientes conectados
            foreach (Ranking ranking in rankings)
            {
                this.sockets.Find(x => x.GetRanking().Name == ranking.Name)?.SetRanking(ranking);
            }
        }

        /// <summary>
        /// Resetea todo los clientes
        /// </summary>
        public void Reset()
        {
            //Liberamos los sockets
            this.sockets.ForEach(x => x.Dispose());
            //Creamos una nueva Coleccion
            this.sockets = new List<ClientSocket>();
        }

        #endregion Methods
    }
}