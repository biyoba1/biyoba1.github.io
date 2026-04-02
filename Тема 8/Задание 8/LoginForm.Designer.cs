using System;
using System.Drawing;
using System.Windows.Forms;

namespace Задание_8
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtLogin;
        private Button btnLogin;
        private Button btnExit;
        private Label lblTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtLogin = new TextBox();
            this.btnLogin = new Button();
            this.btnExit = new Button();
            this.lblTitle = new Label();
            this.SuspendLayout();

            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new Point(50, 20);
            this.lblTitle.Text = "Введите логин:";

            this.txtLogin.Location = new Point(50, 50);
            this.txtLogin.Size = new Size(200, 20);

            this.btnLogin.Location = new Point(50, 90);
            this.btnLogin.Text = "Войти";
            this.btnLogin.Click += new EventHandler(this.btnLogin_Click);

            this.btnExit.Location = new Point(150, 90);
            this.btnExit.Text = "Выход";
            this.btnExit.Click += new EventHandler(this.btnExit_Click);

            this.ClientSize = new Size(300, 150);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtLogin);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnExit);
            this.Text = "Авторизация";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}