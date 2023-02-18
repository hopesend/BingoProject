// ***********************************************************************
// Assembly         : BingoServer
// Author           : David Ferrandez Molla
// Created          : 24-01-2023
//
// Last Modified By : David Ferrandez Molla
// Last Modified On : 24-01-2023
// Last Modified On : 29-01-2023
// ***********************************************************************
// <copyright file="GameManager.cs" company="">
//     Copyright ©  2023
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace BingoServer.Classes
{
    /// <summary>
    /// Clase que gestiona el juego del bingo
    /// </summary>
    public class GameManager
    {
        #region Variables

        /// <summary>
        /// Gestor de Servidor
        /// </summary>
        private ServerSocket serverSocket;

        /// <summary>
        /// Gestor de Clientes
        /// </summary>
        private ClientManager clientManager;

        /// <summary>
        /// Numero de jugadores en la sesion
        /// </summary>
        private int players;

        /// <summary>
        /// hilo de captura de clientes
        /// </summary>
        private Thread threadClients;

        /// <summary>
        /// Hilo del juego
        /// </summary>
        private Thread threadGame;

        /// <summary>
        /// Retardo de bola
        /// </summary>
        private int delayTime;

        #endregion Variables

        #region Constructor

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public GameManager()
        { }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Inicializa el Servidor segun los juegadores
        /// </summary>
        /// <param name="players"></param>
        public void CreateServer(int players, int port, int delay)
        {
            this.players = players;
            this.delayTime = delay;
            //Inicializamos el servidor
            this.serverSocket = new ServerSocket(port);
            //Inicializamos el Manager de clientes
            this.clientManager = new ClientManager();
            //Nos subscribimos a sus eventso
            this.clientManager.OnClientMessageReceived += ClientManager_OnClientMessageReceived;
            this.clientManager.OnClientDisconnected += ClientManager_OnClientDisconnected;
        }

        /// <summary>
        /// Captura la desconexion de uno de los clientes del juego
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientManager_OnClientDisconnected(object sender, ClientDisconnectEventArgs e)
        {
            //Cuando se desconecta el cliente mandamos un mensaje a la consola
            Console.WriteLine("El Cliente " + e.socket.Name + " Se a desconectado del Juego");
            //Mandamos un mensaje a los demas clientes de que el cliente se a desconectado
            this.clientManager.SendMessage(Utils.MessageType.Chat, "Bingo", e.socket.Name + " a abandonado la partida");
            //Si se han desconectado todos los clientes se aborta la partida
            if (this.clientManager.GetCount() == 0)
            {
                this.threadGame.Abort();
                Console.WriteLine("Se han desconectado todos los jugadores...");
            }
        }

        /// <summary>
        /// Captura el envio de un mensaje al caht
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientManager_OnClientMessageReceived(object sender, ClientMessageReceivedEventArgs e)
        {
            //Formateamos el mensaje recibido del cliente
            Tuple<Utils.MessageType, string, string> splitMessage = Utils.CastMessage(e.Message);

            //Repartimos el mensaje segun el tipo de mensaje
            switch (splitMessage.Item1)
            {
                case Utils.MessageType.Chat:
                    {
                        //si el mensaje contiene la palabra ranking se muestra el ranking actual de la sesion en al ventana de chat
                        if (splitMessage.Item3.ToLower().Contains("ranking"))
                        {
                            this.clientManager.SendMessage(Utils.MessageType.Chat, "Bingo", "RAKINGS");
                            Thread.Sleep(50);
                            this.clientManager.SendMessage(Utils.MessageType.Chat, "Bingo", "-------");
                            Thread.Sleep(50);
                            this.clientManager.GetRankings().ForEach(x =>
                            {
                                this.clientManager.SendMessage(Utils.MessageType.Chat, "Bingo", x.ToStringMessageSatinized());
                                Thread.Sleep(50);
                            });
                        }
                        else { this.clientManager.SendMessage(splitMessage.Item1, splitMessage.Item2, splitMessage.Item3); }
                        break;
                    }
                case Utils.MessageType.System:
                    {
                        switch (splitMessage.Item3)
                        {
                            case "BINGO":
                                {
                                    //Si el mensaje es un mensaje de bingo, se avisa a los clientes y se acaba el juego
                                    this.clientManager.SendMessage(Utils.MessageType.Chat, "Bingo", $"El Jugador {splitMessage.Item2} es el Ganador");
                                    this.clientManager.SetBingo(splitMessage.Item2);
                                    this.threadGame.Abort();
                                    Console.WriteLine("Se han desconectado todos los jugadores...");
                                    break;
                                }
                            case "LINE":
                                {
                                    //Si el mensaje es un mensaje de linea, se avisa a los clientes
                                    this.clientManager.SendMessage(Utils.MessageType.Chat, "Bingo", $"El Jugador {splitMessage.Item2} a cantado Linea");
                                    this.clientManager.SetLine(splitMessage.Item2);
                                    break;
                                }
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// Inicia la captura de clientes al juego
        /// </summary>
        public void CaptureClients()
        {
            Console.WriteLine("Se ha iniciado el servidor, Escuchando en : {0}:{1}", this.serverSocket.Ip, this.serverSocket.Port);

            // Se inicia el hilo de captura de clientes
            this.threadClients = new Thread(() =>
            {
                try
                {
                    //Inicializa el socket servidor
                    this.serverSocket.StartSocket();
                    //Mantiene el bucle hasta que la cantidad de jugadores conectados sea igual al numero de jugadores seleccionados
                    while (this.clientManager.GetCount() != this.players)
                    {
                        try
                        {
                            //Se mantiene a la espera de la conexion de un Cliente
                            TcpClient clientSocket = this.serverSocket.AcceptClient();
                            byte[] bytesFrom = new byte[1024];

                            //Obtiene el mensaje del cliente al conectar
                            NetworkStream networkStream = clientSocket.GetStream();
                            networkStream.Read(bytesFrom, 0, 1024);
                            string info = Utils.SatinizeBytes(bytesFrom);
                            string infoCliente = info.Split(':').Last().TrimStart('#').TrimEnd('#');

                            //añade el cliente a la coleccion del gestor
                            this.clientManager.AddClient(new ClientSocket(infoCliente, clientSocket));
                            Console.WriteLine("Jugador " + this.clientManager.GetCount() + " Se unio al juego...");
                            //se envian mensajes de que un nuevo jugador se a conectado
                            this.clientManager.SendMessage(Utils.MessageType.Chat, "Bingo", infoCliente + " Se a unido al juego...");
                            this.clientManager.SendMessage(Utils.MessageType.Chat,
                                                          "Bingo",
                                                          $"Faltan {this.players - this.clientManager.GetCount()} jugadores para empezar el BINGO");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error capturando el nuevo Cliente");
                            Console.WriteLine(e.Message);
                            Console.WriteLine(e.Source);
                        }
                    }

                    //se cancela el socket de escucha
                    this.serverSocket.StopSocket();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error....");
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.Source);
                    this.serverSocket.StopSocket();
                    this.clientManager.Stop();
                }
            });
            this.threadClients.Start();
            this.threadClients.Join();
        }

        /// <summary>
        /// Inicializa el juego
        /// </summary>
        public void StartGame(List<Ranking> rankings)
        {
            this.clientManager.SetRankings(rankings);

            //Se envian mensajes de que el juego va a comenzar
            this.clientManager.SendMessage(Utils.MessageType.Chat, "Bingo", "Todos los jugadores se han unido al juego...");
            this.clientManager.SendMessage(Utils.MessageType.Chat, "Bingo", "El juego comenzara en 10 segundos...");
            Thread.Sleep(10000);

            this.clientManager.SendMessage(Utils.MessageType.Chat, "Bingo", "Comienza el BINGO, Suerte a todos los participantes...");
            //se inicia el hilo de juego
            this.threadGame = new Thread(() =>
            {
                try
                {
                    //Se genera toda la numeracion
                    List<int> numbers = new List<int>();
                    List<int> balls = new List<int>();
                    for (int i = 0; i < 75; i++)
                    {
                        numbers.Add(i + 1);
                    }

                    //Se mezcla esa numera de manera aleatoria
                    Random randNum = new Random();
                    while (numbers.Count > 0)
                    {
                        int index = randNum.Next(0, numbers.Count());
                        balls.Add(numbers[index]);
                        numbers.Remove(numbers[index]);
                    }

                    //Se itinera por los numeros aleatorios y se manda un mensaje a los clientes
                    for (int i = 0; i < 75; i++)
                    {
                        Thread.Sleep(this.delayTime * 1000);
                        this.clientManager.SendMessage(Utils.MessageType.Ball, "Bingo", balls[i].ToString());
                    }
                    Console.WriteLine("Ya hay ganador ");
                }
                catch { }
            });
            this.threadGame.Start();
            this.threadGame.Join();

            Console.WriteLine("El juego a finalizado");
            this.clientManager.SendMessage(Utils.MessageType.Chat, "Bingo", "El BINGO a finalizado....");
        }

        /// <summary>
        /// Obtiene el ranking de jugadores de la sesion actual
        /// </summary>
        /// <returns></returns>
        public List<Ranking> GetRankingPlayers()
        {
            return this.clientManager.GetRankings();
        }

        /// <summary>
        /// Desconecta todos los clientes de la sesion
        /// </summary>
        public void DisconnectClients()
        {
            this.clientManager.Reset();
        }

        #endregion Methods
    }
}