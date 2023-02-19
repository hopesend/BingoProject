// ***********************************************************************
// Assembly         : BingoServer
// Author           : David Ferrandez Molla
// Created          : 23-01-2023
//
// Last Modified By : David Ferrandez Molla
// Last Modified On : 23-01-2023
// Last Modified On : 29-01-2023
// ***********************************************************************
// <copyright file="FormBingo.cs" company="">
//     Copyright ©  2023
// </copyright>
// <summary></summary>
// ***********************************************************************
using BingoClient.Classes;
using BingoClient.Events;
using System;
using System.Windows.Forms;

namespace BingoClient.Forms
{
    /// <summary>
    /// Interfaz Visual para el Juego
    /// </summary>
    public partial class FormBingo : Form
    {
        #region Properties

        /// <summary>
        /// Gestor del Servidor
        /// </summary>
        public ServerManager managerServer;

        /// <summary>
        /// Interfaz Visual de la Configuracion
        /// </summary>
        private readonly FormConfig config = new FormConfig();

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructor del interfaz
        /// </summary>
        public FormBingo()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Form Events

        /// <summary>
        /// Evento Load para la inicializacion de elementos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormBingo_Load(object sender, EventArgs e)
        {
            //nos subscribimos a los eventos del carton de bingo
            this.CardBingo.OnLineCaptured += CardBingo_OnLineCaptured;
            this.CardBingo.OnBingoCaptured += CardBingo_OnBingoCaptured;
            //Generamos un nombre de usuario aleatorio
            this.config.TextUserName.Text = "User_" + new Random().Next(1, 999).ToString();
            //Marcamos al usuario como no conectado
            this.StatusBarLabel.Text = $"{this.config.TextUserName.Text} Not Connected";
        }

        /// <summary>
        /// Evento para avisar que un nuevo Bingo a sido capturado (Juego automatico)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardBingo_OnBingoCaptured(object sender, EventArgs e)
        {
            //si se captura un bingo se manda un mensaje al servidor
            this.managerServer.SendMessage(new MessageSocket(MessageType.System, this.config.TextUserName.Text, "BINGO"));
        }

        /// <summary>
        /// Evento para avisar que una nueva Linea a sido capturada (Juego automatico)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardBingo_OnLineCaptured(object sender, EventArgs e)
        {
            //si se captura una linea se manda un mensaje al servidor
            this.managerServer.SendMessage(new MessageSocket(MessageType.System, this.config.TextUserName.Text, "LINE"));
        }

        /// <summary>
        /// Abre la configuracion de la aplicacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonServerConfig_Click(object sender, EventArgs e)
        {
            this.config.ShowDialog();
            this.StatusBarLabel.Text = (managerServer != null && managerServer.IsConnected())
                ? $"{this.config.TextUserName.Text} Is Connected"
                : $"{this.config.TextUserName.Text} Not Connected";
        }

        /// <summary>
        /// Conecta con el servidor segun los parametros de la configuracion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btConnect_Click(object sender, EventArgs e)
        {
            if ((sender as Button).Text == "Connect")
            {
                if (string.IsNullOrEmpty(this.config.TextIP.Text))
                {
                    MessageBox.Show("La Ip del Servidor esta incompleta...", "ATENCION");
                    return;
                }

                if (string.IsNullOrEmpty(this.config.TextPort.Text))
                {
                    MessageBox.Show("El Puerto del Servidor esta incompleto...", "ATENCION");
                    return;
                }

                if (string.IsNullOrEmpty(this.config.TextUserName.Text))
                {
                    MessageBox.Show("Se Necesita un Nombre de Usuario para jugar...", "ATENCION");
                    return;
                }

                //Se inicializa un nuevo socket servidor
                this.managerServer = new ServerManager(this.config.TextIP.Text, int.Parse(this.config.TextPort.Text));
                //nos subscribimos a sus eventos
                this.managerServer.OnChatMessageSend += ManagerServer_OnChatMessageSend;
                this.managerServer.OnSystemMessageSend += ManagerServer_OnSystemMessageSend;
                this.managerServer.OnBallSend += ManagerServer_OnBallSend;
                this.managerServer.OnClientDisconnected += ManagerServer_OnClientDisconnected;
                //conectamos con el servidor
                if (this.managerServer.ConnectToServer())
                {
                    //enviamos un mensaje al servidor de que hemos conectado
                    this.managerServer.SendMessage(new MessageSocket(MessageType.System, this.config.TextUserName.Text, this.config.TextUserName.Text));
                    //Marcamos como que hemos conectado
                    this.StatusBarLabel.Text = $"{this.config.TextUserName.Text} Is Connected";
                    //Cambiamos la marca del boton para su desconexion
                    (sender as Button).Text = "Disconnect";
                    //borramos el historial del chat
                    this.ListChat.Items.Clear();
                    //Se genera un nuevo Carton
                    this.CardBingo.GenerateNewCard();
                }
                else
                {
                    MessageBox.Show("El Servidor no esta operativo...", "ATENCION");
                    return;
                }
            }
            else
            {
                //Cancelamos y liberamos el socket del servidor
                this.managerServer.Dispose();
                //Vaciamos el carton
                this.CardBingo.ClearCard();
                //Marcamos como que hemos desconectado
                this.StatusBarLabel.Text = $"{this.config.TextUserName.Text} Not Connected";
                //Cambiamos la marca del carton para su conexion
                (sender as Button).Text = "Connect";
                //borramos el historial del chat
                this.ListChat.Items.Clear();
            }
        }

        /// <summary>
        /// Evento para la captura de que el usuario se a desconectado del servidor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManagerServer_OnClientDisconnected(object sender, EventArgs e)
        {
            //Marcamos que hemos desconectado
            this.StatusBarLabel.Text = $"{this.config.TextUserName.Text} Not Connected";
            //LIberamos el socket servidor
            this.managerServer.Dispose();

            Invoke(new Action(() =>
            {
                //Se vacia el Carton
                //this.CardBingo.ClearCard();
                //Cambiamos la marca del carton para su conexion
                btConnect.Text = "Connect";
                //borramos el historial del chat
                //this.ListChat.Items.Clear();
            }));
        }

        /// <summary>
        /// Evento para la captura de que el servidor a emitido una nueva bola para el juego
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManagerServer_OnBallSend(object sender, BallSendEventArgs e)
        {
            //avisamos a los componentes que una nueva bola se a generado en el servidor
            this.BingoBallActived.NewBallActived(e.Ball);
            this.CardBingo.NewBallActived(e.Ball);
            this.CardBingo.Automatic = this.CheckAutomatico.Checked;
        }

        /// <summary>
        /// Evento para la captura de que el servidor a mandado un nuevo mensaje al sistema
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManagerServer_OnSystemMessageSend(object sender, SystemMessageEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Evento para la captura de que el servidor a mandado un nuevo mensaje para el chat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManagerServer_OnChatMessageSend(object sender, ChatMessageEventArgs e)
        {
            //escribimos un nuevo mensaje en el chat tras recibirlo del servidor
            Invoke(new Action(() => 
            {
                this.ListChat.Items.Add(e.User + " says: " + e.Message);
                this.ListChat.TopIndex = this.ListChat.Items.Count - 1;
            }));
        }

        /// <summary>
        /// Envio de mensaje del usuario actual al chat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonChatSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TextChat.Text) && this.managerServer.IsConnected())
            {
                //Enviamos un mensaje al servidor para decirle que hemos escrito en el chat
                this.managerServer.SendMessage(new MessageSocket(MessageType.Chat, this.config.TextUserName.Text, TextChat.Text));
                this.TextChat.Clear();
            }
        }

        private void TextChat_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) { ButtonChatSend.PerformClick(); }
        }

        /// <summary>
        /// Evento para cuando se cierra el interfaz visual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormBingo_FormClosing(object sender, FormClosingEventArgs e) => this.managerServer?.Dispose();

        /// <summary>
        /// Canta una Linea y checkea si es cierto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonLine_Click(object sender, EventArgs e)
        {
            //Busca si la linea es correcta y los muestra en el interfaz
            this.CardBingo.FindLine();
        }

        /// <summary>
        /// Canta un Bingo y checkea si es cierto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBingo_Click(object sender, EventArgs e)
        {
            //Busca si el bingo es correcto y lo muestra en el interfaz
            this.CardBingo.FindBingo();
        }

        /// <summary>
        /// Cambia a juego automatico o manual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckAutomatico_CheckedChanged(object sender, EventArgs e)
        {
            //refresca el estado automatico del juego
            this.CardBingo.Automatic = this.CheckAutomatico.Checked;
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new FormAbout().ShowDialog();
        }

        #endregion Form Events
    }
}