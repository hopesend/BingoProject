
namespace BingoClient.Forms
{
    partial class FormConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TextIP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TextPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TextUserName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TextIP
            // 
            this.TextIP.Location = new System.Drawing.Point(107, 47);
            this.TextIP.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TextIP.Name = "TextIP";
            this.TextIP.Size = new System.Drawing.Size(131, 22);
            this.TextIP.TabIndex = 0;
            this.TextIP.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 50);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Server IP: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 82);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Server Port: ";
            // 
            // TextPort
            // 
            this.TextPort.Location = new System.Drawing.Point(107, 79);
            this.TextPort.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TextPort.Name = "TextPort";
            this.TextPort.Size = new System.Drawing.Size(61, 22);
            this.TextPort.TabIndex = 2;
            this.TextPort.Text = "6666";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 18);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "User Name: ";
            // 
            // TextUserName
            // 
            this.TextUserName.Location = new System.Drawing.Point(107, 15);
            this.TextUserName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TextUserName.Name = "TextUserName";
            this.TextUserName.Size = new System.Drawing.Size(131, 22);
            this.TextUserName.TabIndex = 4;
            // 
            // FormConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(251, 112);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TextUserName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TextPort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TextIP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Server Configuration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox TextIP;
        public System.Windows.Forms.TextBox TextPort;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox TextUserName;
    }
}