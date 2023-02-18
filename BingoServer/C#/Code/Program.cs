// ***********************************************************************
// Assembly         : BingoServer
// Author           : David Ferrandez Molla
// Created          : 16-01-2023
//
// Last Modified By : David Ferrandez Molla
// Last Modified On : 16-01-2023
// Last Modified On : 17-01-2023
// Last Modified On : 29-01-2023
// ***********************************************************************
// <copyright file="Program.cs" company="">
//     Copyright ©  2023
// </copyright>
// <summary></summary>
// ***********************************************************************
using BingoServer.Classes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BingoServer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Rankings de la sesion
            List<Ranking> ranking = new List<Ranking>();

            //Puerto
            int port = 6666;
            int retardo = 5;

            Console.Clear();
            Console.WriteLine("Bingo Server v." + Assembly.GetExecutingAssembly().GetName().Version.ToString());
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Puerto (Enter -> 6666): ");
            string puertoSeleccionado = Console.ReadLine();
            if (!string.IsNullOrEmpty(puertoSeleccionado) &&
               int.TryParse(puertoSeleccionado, out int puertoCast) &&
               (puertoCast >= 0 && puertoCast <= 65535)) { port = puertoCast; }

            Console.Write("Retardo de Bola (Enter -> 5 segundos): ");
            string retardoSeleccionado = Console.ReadLine();
            if (!string.IsNullOrEmpty(retardoSeleccionado) &&
               int.TryParse(retardoSeleccionado, out int retardoCast) &&
               (retardoCast >= 1 && retardoCast <= 10)) { retardo = retardoCast; }

            string opt = "Y";
            while (opt == "Y" || opt == "y")
            {
                //Capturamos el numero de jugadores
                Console.Write("Cantidad de Jugadores: ");
                string select = Console.ReadLine();
                int players = 0;
                while (!int.TryParse(select, out players) || players < 1)
                {
                    Console.WriteLine("Error... (Se necesitan mas de 1 jugador):");
                    Console.Write("Cantidad de Jugadores: ");
                    select = Console.ReadLine();
                }
                Console.WriteLine();
                Console.WriteLine("Se han seleccionado {0} jugadores...", players);
                Console.WriteLine("Se procede a Iniciar el Juego...");
                Console.WriteLine("Montando Servidor...");

                //Inicializamos GameManager
                GameManager game = new GameManager();
                //Creamos el servidor
                game.CreateServer(players, port, retardo);
                //Capturamos los clientes
                game.CaptureClients();
                //Comenzamos el juego
                game.StartGame(ranking);

                //Al campturamos los rankings de la partida y los unimos a los de la sesion
                foreach (Ranking rankingPlayer in game.GetRankingPlayers())
                {
                    if (ranking.Exists(x => x.Name == rankingPlayer.Name))
                    {
                        Ranking listPlayer = ranking.Find(x => x.Name == rankingPlayer.Name);
                        listPlayer.Line += rankingPlayer.Line;
                        listPlayer.Bingo += rankingPlayer.Bingo;
                    }
                    else
                    {
                        ranking.Add(rankingPlayer);
                    }
                }
                //Desconectamos todos los clientes de la sesion
                game.DisconnectClients();

                //Mostramos los rankings
                Console.WriteLine();
                Console.WriteLine("RANKING DE JUGADORES");
                Console.WriteLine("--------------------");
                ranking.ForEach(x => Console.WriteLine(x.ToString()));

                //Pedimos una nueva partida o salir
                Console.WriteLine();
                Console.WriteLine("¿Quiere jugar de nuevo? [Y/N]");
                opt = Console.ReadLine();
            }
        }
    }
}