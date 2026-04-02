using System;
using System.Drawing;
using System.Windows.Forms;

namespace Задание_8
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newGameToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem resultsToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblUser;
        private ToolStripStatusLabel lblStatus;
        private ToolStripStatusLabel lblTime;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.menuStrip1 = new MenuStrip();
            this.fileToolStripMenuItem = new ToolStripMenuItem();
            this.newGameToolStripMenuItem = new ToolStripMenuItem();
            this.exitToolStripMenuItem = new ToolStripMenuItem();
            this.settingsToolStripMenuItem = new ToolStripMenuItem();
            this.resultsToolStripMenuItem = new ToolStripMenuItem();
            this.helpToolStripMenuItem = new ToolStripMenuItem();
            this.aboutToolStripMenuItem = new ToolStripMenuItem();
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.statusStrip1 = new StatusStrip();
            this.lblUser = new ToolStripStatusLabel();
            this.lblStatus = new ToolStripStatusLabel();
            this.lblTime = new ToolStripStatusLabel();

            this.fileToolStripMenuItem.Text = "Файл";
            this.newGameToolStripMenuItem.Text = "Новая игра";
            this.newGameToolStripMenuItem.Click += new EventHandler(newGameToolStripMenuItem_Click);
            this.exitToolStripMenuItem.Text = "Выход";
            this.exitToolStripMenuItem.Click += new EventHandler(exitToolStripMenuItem_Click);
            this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.newGameToolStripMenuItem, this.exitToolStripMenuItem });

            this.settingsToolStripMenuItem.Text = "Настройки";
            this.settingsToolStripMenuItem.Click += new EventHandler(settingsToolStripMenuItem_Click);

            this.resultsToolStripMenuItem.Text = "Результаты";
            this.resultsToolStripMenuItem.Click += new EventHandler(resultsToolStripMenuItem_Click);

            this.helpToolStripMenuItem.Text = "Справка";
            this.aboutToolStripMenuItem.Text = "О программе";
            this.aboutToolStripMenuItem.Click += new EventHandler(aboutToolStripMenuItem_Click);
            this.helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.aboutToolStripMenuItem });

            this.menuStrip1.Items.AddRange(new ToolStripItem[] { this.fileToolStripMenuItem, this.settingsToolStripMenuItem, this.resultsToolStripMenuItem, this.helpToolStripMenuItem });

            this.tableLayoutPanel1.Location = new Point(0, 27);
            this.tableLayoutPanel1.Size = new Size(400, 400);
            this.tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            this.lblUser.Text = "Игрок: ";
            this.lblStatus.Text = "Статус";
            this.lblTime.Text = "Время: 0";
            this.statusStrip1.Items.AddRange(new ToolStripItem[] { this.lblUser, this.lblStatus, this.lblTime });

            this.ClientSize = new Size(400, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Text = "Минное поле";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}