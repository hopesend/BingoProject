namespace BingoClient.Forms
{
    partial class FormBingo
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBingo));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ButtonServerConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusBarLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ButtonLine = new System.Windows.Forms.Button();
            this.ButtonBingo = new System.Windows.Forms.Button();
            this.btConnect = new System.Windows.Forms.Button();
            this.ListChat = new System.Windows.Forms.ListBox();
            this.TextChat = new System.Windows.Forms.TextBox();
            this.ButtonChatSend = new System.Windows.Forms.Button();
            this.CheckAutomatico = new System.Windows.Forms.CheckBox();
            this.BingoBallActived = new BingoClient.Controls.BingoServerBall();
            this.CardBingo = new BingoClient.Controls.BingoCard();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BingoBallActived)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(684, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonServerConfig});
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.configToolStripMenuItem.Text = "Config";
            // 
            // ButtonServerConfig
            // 
            this.ButtonServerConfig.Name = "ButtonServerConfig";
            this.ButtonServerConfig.Size = new System.Drawing.Size(145, 22);
            this.ButtonServerConfig.Text = "Server Config";
            this.ButtonServerConfig.Click += new System.EventHandler(this.ButtonServerConfig_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem1});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem1.Text = "About...";
            this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.aboutToolStripMenuItem1_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusBarLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 272);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(684, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusBarLabel
            // 
            this.StatusBarLabel.Name = "StatusBarLabel";
            this.StatusBarLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // ButtonLine
            // 
            this.ButtonLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonLine.Location = new System.Drawing.Point(246, 118);
            this.ButtonLine.Name = "ButtonLine";
            this.ButtonLine.Size = new System.Drawing.Size(80, 23);
            this.ButtonLine.TabIndex = 5;
            this.ButtonLine.Text = "Line";
            this.ButtonLine.UseVisualStyleBackColor = true;
            this.ButtonLine.Click += new System.EventHandler(this.ButtonLine_Click);
            // 
            // ButtonBingo
            // 
            this.ButtonBingo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonBingo.Location = new System.Drawing.Point(246, 147);
            this.ButtonBingo.Name = "ButtonBingo";
            this.ButtonBingo.Size = new System.Drawing.Size(80, 23);
            this.ButtonBingo.TabIndex = 6;
            this.ButtonBingo.Text = "Bingo";
            this.ButtonBingo.UseVisualStyleBackColor = true;
            this.ButtonBingo.Click += new System.EventHandler(this.ButtonBingo_Click);
            // 
            // btConnect
            // 
            this.btConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btConnect.Location = new System.Drawing.Point(246, 238);
            this.btConnect.Name = "btConnect";
            this.btConnect.Size = new System.Drawing.Size(80, 23);
            this.btConnect.TabIndex = 7;
            this.btConnect.Text = "Connect";
            this.btConnect.UseVisualStyleBackColor = true;
            this.btConnect.Click += new System.EventHandler(this.btConnect_Click);
            // 
            // ListChat
            // 
            this.ListChat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ListChat.Location = new System.Drawing.Point(332, 32);
            this.ListChat.Name = "ListChat";
            this.ListChat.Size = new System.Drawing.Size(345, 200);
            this.ListChat.TabIndex = 8;
            // 
            // TextChat
            // 
            this.TextChat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextChat.Location = new System.Drawing.Point(332, 240);
            this.TextChat.Name = "TextChat";
            this.TextChat.Size = new System.Drawing.Size(288, 21);
            this.TextChat.TabIndex = 9;
            this.TextChat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextChat_KeyPress);
            // 
            // ButtonChatSend
            // 
            this.ButtonChatSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonChatSend.Location = new System.Drawing.Point(626, 239);
            this.ButtonChatSend.Name = "ButtonChatSend";
            this.ButtonChatSend.Size = new System.Drawing.Size(51, 22);
            this.ButtonChatSend.TabIndex = 10;
            this.ButtonChatSend.Text = "Send";
            this.ButtonChatSend.UseVisualStyleBackColor = true;
            this.ButtonChatSend.Click += new System.EventHandler(this.ButtonChatSend_Click);
            // 
            // CheckAutomatico
            // 
            this.CheckAutomatico.AutoSize = true;
            this.CheckAutomatico.Location = new System.Drawing.Point(248, 215);
            this.CheckAutomatico.Name = "CheckAutomatico";
            this.CheckAutomatico.Size = new System.Drawing.Size(73, 17);
            this.CheckAutomatico.TabIndex = 11;
            this.CheckAutomatico.Text = "Automatic";
            this.CheckAutomatico.UseVisualStyleBackColor = true;
            this.CheckAutomatico.CheckedChanged += new System.EventHandler(this.CheckAutomatico_CheckedChanged);
            // 
            // BingoBallActived
            // 
            this.BingoBallActived.BallNumber = 0;
            this.BingoBallActived.Location = new System.Drawing.Point(246, 32);
            this.BingoBallActived.Name = "BingoBallActived";
            this.BingoBallActived.Size = new System.Drawing.Size(80, 80);
            this.BingoBallActived.TabIndex = 4;
            this.BingoBallActived.TabStop = false;
            // 
            // CardBingo
            // 
            this.CardBingo.Automatic = false;
            this.CardBingo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CardBingo.Location = new System.Drawing.Point(12, 32);
            this.CardBingo.Name = "CardBingo";
            this.CardBingo.Size = new System.Drawing.Size(228, 229);
            this.CardBingo.TabIndex = 1;
            // 
            // FormBingo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 294);
            this.Controls.Add(this.CheckAutomatico);
            this.Controls.Add(this.ButtonChatSend);
            this.Controls.Add(this.TextChat);
            this.Controls.Add(this.ListChat);
            this.Controls.Add(this.btConnect);
            this.Controls.Add(this.ButtonBingo);
            this.Controls.Add(this.ButtonLine);
            this.Controls.Add(this.BingoBallActived);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.CardBingo);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(700, 333);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 333);
            this.Name = "FormBingo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bingo Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBingo_FormClosing);
            this.Load += new System.EventHandler(this.FormBingo_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BingoBallActived)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BingoClient.Controls.BingoCard CardBingo;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusBarLabel;
        private BingoClient.Controls.BingoServerBall BingoBallActived;
        private System.Windows.Forms.Button ButtonLine;
        private System.Windows.Forms.Button ButtonBingo;
        private System.Windows.Forms.Button btConnect;
        private System.Windows.Forms.ListBox ListChat;
        private System.Windows.Forms.TextBox TextChat;
        private System.Windows.Forms.Button ButtonChatSend;
        private System.Windows.Forms.ToolStripMenuItem ButtonServerConfig;
        private System.Windows.Forms.CheckBox CheckAutomatico;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
    }
}

