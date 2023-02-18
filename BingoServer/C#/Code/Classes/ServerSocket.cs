// ***********************************************************************
// Assembly         : BingoServer
// Author           : David Ferrandez Molla
// Created          : 16-01-2023
//
// Last Modified By : David Ferrandez Molla
// Last Modified On : 16-01-2023
// Last Modified On : 17-01-2023
// Last Modified On : 24-01-2023
// Last Modified On : 29-01-2023
// ***********************************************************************
// <copyright file="ServerSocket.cs" company="">
//     Copyright ©  2023
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Net;
using System.Net.Sockets;

namespace BingoServer.Classes
{
    /// <summary>
    /// Clase para la gestion del Socket de Servidor
    /// </summary>
    public class ServerSocket
    {
        #region Properties

        /// <summary>
        /// Ip del servidor
        /// </summary>
        private IPAddress ip = IPAddress.Parse("127.0.0.1");

        public IPAddress Ip { get => ip; set => ip = value; }

        /// <summary>
        /// Puerto del Servidor
        /// </summary>
        private int port = 6666;

        public int Port { get => port; set => port = value; }

        #endregion Properties

        #region Variables

        /// <summary>
        /// Socket Servidor
        /// </summary>
        private TcpListener socket;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Constructor de la Clase
        /// </summary>
        public ServerSocket(int port)
        {
            this.Port = port;
            this.socket = new TcpListener(this.Ip, this.Port);
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Inicia el socket
        /// </summary>
        public void StartSocket() => this.socket.Start();

        /// <summary>
        /// Finaliza el socket
        /// </summary>
        public void StopSocket() => this.socket.Stop();

        /// <summary>
        /// Inicializa la captura de Clientes
        /// </summary>
        /// <returns></returns>
        public TcpClient AcceptClient() => this.socket.AcceptTcpClient();

        #endregion Methods
    }
}