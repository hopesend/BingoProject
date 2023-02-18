// ***********************************************************************
// Assembly         : BingoServer
// Author           : David Ferrandez Molla
// Created          : 23-01-2023
//
// Last Modified By : David Ferrandez Molla
// Last Modified On : 23-01-2023
// Last Modified On : 29-01-2023
// ***********************************************************************
// <copyright file="BingoCard.cs" company="">
//     Copyright ©  2023
// </copyright>
// <summary></summary>
// ***********************************************************************
using BingoClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BingoClient.Controls
{
    /// <summary>
    /// Clase que simula un carton de bingo
    /// </summary>
    public class BingoCard : Panel, IBallManager
    {
        #region Variables

        /// <summary>
        /// Coleccion con las bolas que contiene el carton
        /// </summary>
        private List<BingoCardBall> balls;

        /// <summary>
        /// Bola actual en el Juego
        /// </summary>
        private int actualBall;

        /// <summary>
        /// Parametro para controlar el tipo de juego, Automatico o Manual
        /// </summary>

        #endregion Variables

        #region Parameters

        public bool Automatic { get; set; }

        #endregion Parameters

        #region Events

        /// <summary>
        /// Evento para Emitir que se a conseguido una linea
        /// </summary>
        public event EventHandler<EventArgs> OnLineCaptured;

        /// <summary>
        /// Evento para Emitir que se a conseguido un Bingo
        /// </summary>
        public event EventHandler<EventArgs> OnBingoCaptured;

        #endregion Events

        #region Constructor

        /// <summary>
        /// Constructor de la Clase
        /// </summary>
        public BingoCard() : base()
        {
            //Se inicializa el carton
            this.Size = new System.Drawing.Size(200, 200);
            this.BorderStyle = BorderStyle.FixedSingle;
            this.balls = new List<BingoCardBall>();
        }

        #endregion Constructor

        #region Control Methods

        /// <summary>
        /// Genera un nuevo Carton
        /// </summary>
        public void GenerateNewCard()
        {
            //Se vacia el carton
            this.ClearCard();
            //Las distribuye en el carton
            this.DistributeBalls();

            //Genera todos los nuemros del bingo
            List<int> numbers = new List<int>();
            for (int i = 0; i < 75; i++)
            {
                numbers.Add(i + 1);
            }

            //Captura los aleatorios hasta las 25 bolas y las asigna
            Random randNum = new Random();
            for (int i = 0; i < 25; i++)
            {
                int index = randNum.Next(0, numbers.Count());
                balls[i].BallNumber = numbers[index];
                balls[i].Invalidate();
                numbers.Remove(numbers[index]);
            }
            this.Invalidate();
        }

        /// <summary>
        /// Distribuye las Bolas en el Carton
        /// </summary>
        private void DistributeBalls()
        {
            int yPosition = -43;
            for (int i = 0; i < 5; i++)
            {
                int xPosition = 2;
                yPosition += 45;
                for (int j = 0; j < 5; j++)
                {
                    BingoCardBall ball = new BingoCardBall
                    {
                        Location = new System.Drawing.Point(xPosition, yPosition),
                        Marked = false,
                        Position = new System.Drawing.Point(j, i)
                    };
                    ball.Click += Ball_Click;
                    this.Controls.Add(ball);
                    this.balls.Add(ball);
                    xPosition += 45;
                }
            }
        }

        /// <summary>
        /// Evento que captura que el usuario a pulsado sobre una bola
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ball_Click(object sender, EventArgs e)
        {
            BingoCardBall ball = (sender as BingoCardBall);
            //Si el numero de bola del servidor coincide con el numero de bola pulsada
            if (ball.BallNumber == this.actualBall)
            {
                //se marca como seleccionada
                ball.Marked = true;
                //se redibuja el control
                ball.Invalidate();
            }
        }

        /// <summary>
        /// Busca una composicion de linea en todo el carton
        /// </summary>
        public void FindLine()
        {
            //itinera por las 5 lineas del carton
            for (int i = 0; i < 5; i++)
            {
                int condemor = this.balls.Where(x => x.Marked).Where(x => x.Position.Y == i).Count();
                //Busca que las 5 bolas de esa linea esten marcadas
                if (this.balls.Where(x => x.Marked).Where(x => x.Position.Y == i).Count() == 5)
                {
                    //itinera por las bolas marcadas
                    this.balls.Where(x => x.Marked).Where(x => x.Position.Y == i).ToList().ForEach(x =>
                    {
                        //quitamos la marca de seleccionada
                        x.Marked = false;
                        //la marca como bloqueadas
                        x.Blocked = true;
                        //las redibuja
                        x.Invalidate();
                    });
                    //emite el evento para el interfaz
                    OnLineCaptured?.Invoke(null, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Busca una composion de Bingo en todo el carton
        /// </summary>
        public void FindBingo()
        {
            //busca que todas las bolas del carton esten marcadas o bloqueadas
            if (this.balls.Where(x => x.Marked || x.Blocked).Count() == 25)
            {
                //itinera por todas la bolas
                this.balls.ForEach(x =>
                {
                    //marca la bola como bloqueada
                    x.Blocked = true;
                    //la redibuja
                    x.Invalidate();
                });
                //emite el evento para el interfaz
                OnBingoCaptured?.Invoke(null, EventArgs.Empty);
            }
        }

        public void ClearCard()
        {
            //Construye las bolas sin valor numerico
            this.balls = new List<BingoCardBall>();
            //Se vacia el Carton
            Invoke(new Action(() => this.Controls.Clear()));
        }

        #endregion Control Methods

        #region IBallManager Implementation

        /// <summary>
        /// Gestiona que el servidor a generado una nueva bola para el juego
        /// </summary>
        /// <param name="ballNumber"></param>
        public void NewBallActived(int ballNumber)
        {
            //captura la bola con la variable
            this.actualBall = ballNumber;
            //si el juego es automatico
            if (this.Automatic)
            {
                //busca la bola actual en la coleccion de bolas del carton
                BingoCardBall ball = this.balls.Find(x => x.BallNumber == this.actualBall);
                //si existe en el carton
                if (ball != null)
                {
                    //la marca como seleccionada
                    ball.Marked = true;
                    //la redibuja
                    ball.Invalidate();
                    //busca una linea en el carton
                    this.FindLine();
                    //busca un bingo en el carton
                    this.FindBingo();
                }
            }
        }

        #endregion IBallManager Implementation
    }
}