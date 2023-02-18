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
using System.Drawing;
using System.Windows.Forms;

namespace BingoClient.Controls
{
    /// <summary>
    /// Clase que simula una bola del carton
    /// </summary>
    public class BingoCardBall : PictureBox
    {
        #region Properties

        /// <summary>
        /// Numero de la bola
        /// </summary>
        public int BallNumber { get; set; }

        /// <summary>
        /// Cuando la bola es marcada por que a salido en el juego, se pinta de verde.
        /// </summary>
        public bool Marked { get; set; }

        /// <summary>
        /// Cuando la bola forma parte de una linea o de un bingo, se pinta de naranja.
        /// </summary>
        public bool Blocked { get; set; }

        /// <summary>
        /// La posicion que ocupa en el carton
        /// </summary>
        public Point Position { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        public BingoCardBall() : base()
        {
            //se fuerza su tamañao a 40x40
            this.Size = new Size(40, 40);
        }

        #endregion Constructor

        #region Override Methods

        /// <summary>
        /// Redibuja el control teniendo en cuenta si esta marcado o bloqueado
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            //si esta marcada pintar de verde
            if (this.Marked)
            {
                pe.Graphics.DrawEllipse(new Pen(Color.Black, 2f), 2, 2, 35, 35);
                pe.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(128, 255, 128)), 3, 3, 33, 33);
            }
            //si esta bloqueada pintar de naranja
            else if (this.Blocked)
            {
                pe.Graphics.DrawEllipse(new Pen(Color.Black, 2f), 2, 2, 35, 35);
                pe.Graphics.FillEllipse(new SolidBrush(Color.FromArgb(255, 128, 0)), 3, 3, 33, 33);
            }
            //si no mantenerla como esta
            else
            {
                pe.Graphics.DrawEllipse(new Pen(Color.Black, 2f), 2, 2, 35, 35);
            }

            //Se dibuja el numero de la bola
            Size sizeText = TextRenderer.MeasureText(this.BallNumber.ToString(), new Font("Arial", 14));
            pe.Graphics.DrawString(this.BallNumber.ToString(),
                                   new Font("Arial", 14),
                                   Brushes.Black,
                                   new Point((this.Width / 2) - (sizeText.Width / 2) + 2, (this.Size.Height / 2) - (sizeText.Height / 2)));
        }

        #endregion Override Methods
    }
}