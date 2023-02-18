// ***********************************************************************
// Assembly         : BingoServer
// Author           : David Ferrandez Molla
// Created          : 23-01-2023
//
// Last Modified By : David Ferrandez Molla
// Last Modified On : 23-01-2023
// Last Modified On : 29-01-2023
// ***********************************************************************
// <copyright file="BingoCardBall.cs" company="">
//     Copyright ©  2023
// </copyright>
// <summary></summary>
// ***********************************************************************
using BingoClient.Interfaces;
using System.Drawing;
using System.Windows.Forms;

namespace BingoClient.Controls
{
    /// <summary>
    /// Simula la bola que sale del bombo en el bingo
    /// </summary>
    public class BingoServerBall : PictureBox, IBallManager
    {
        #region Properties

        /// <summary>
        /// Numero de la bola
        /// </summary>
        public int BallNumber { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructor de la Clase
        /// </summary>
        public BingoServerBall() : base()
        {
            //se limita su tamaño a 80x08
            this.Size = new Size(80, 80);
        }

        #endregion Constructor

        #region Override Methods

        /// <summary>
        /// dibuja la bola segun el numero
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            pe.Graphics.DrawEllipse(new Pen(Color.Black, 2f), 2, 2, 75, 75);

            Size sizeText = TextRenderer.MeasureText(this.BallNumber.ToString(), new Font("Arial", 24));
            pe.Graphics.DrawString(this.BallNumber.ToString(),
                                   new Font("Arial", 24),
                                   Brushes.Black,
                                   new Point((this.Width / 2) - (sizeText.Width / 2) + 2, (this.Size.Height / 2) - (sizeText.Height / 2)));
        }

        #endregion Override Methods

        #region IBallManager Implementation

        /// <summary>
        /// Captura la nueva bola emitida por el servidor y la dibuja
        /// </summary>
        /// <param name="ballNumber"></param>
        public void NewBallActived(int ballNumber)
        {
            this.BallNumber = ballNumber;
            this.Invalidate();
        }

        #endregion IBallManager Implementation
    }
}