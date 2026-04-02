using System;
using System.Drawing;
using System.Windows.Forms;

namespace Задание_8
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblMines;
        private NumericUpDown numMines;
        private Label lblColor;
        private Panel pnlColor;
        private Button btnColor;
        private Button btnSave;
        private Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblMines = new Label();
            this.numMines = new NumericUpDown();
            this.lblColor = new Label();
            this.pnlColor = new Panel();
            this.btnColor = new Button();
            this.btnSave = new Button();
            this.btnCancel = new Button();
            this.SuspendLayout();

            this.lblMines.Text = "Количество мин (10-90):";
            this.lblMines.Location = new Point(10, 10);

            this.numMines.Location = new Point(10, 30);
            this.numMines.Minimum = 10;
            this.numMines.Maximum = 90;
            this.numMines.Value = 10;

            this.lblColor.Text = "Цвет ячеек:";
            this.lblColor.Location = new Point(10, 60);

            this.pnlColor.Location = new Point(10, 80);
            this.pnlColor.Size = new Size(50, 30);
            this.pnlColor.BorderStyle = BorderStyle.FixedSingle;

            this.btnColor.Text = "Выбрать";
            this.btnColor.Location = new Point(70, 80);
            this.btnColor.Click += new EventHandler(btnColor_Click);

            this.btnSave.Text = "Сохранить";
            this.btnSave.Location = new Point(10, 120);
            this.btnSave.Click += new EventHandler(btnSave_Click);

            this.btnCancel.Text = "Отмена";
            this.btnCancel.Location = new Point(100, 120);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);

            this.ClientSize = new Size(200, 160);
            this.Controls.Add(lblMines);
            this.Controls.Add(numMines);
            this.Controls.Add(lblColor);
            this.Controls.Add(pnlColor);
            this.Controls.Add(btnColor);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
            this.Text = "Настройки";
            this.ResumeLayout(false);
        }
    }
}