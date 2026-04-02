using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Задание_6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "🇷🇺 Города и памятники России — Викторина";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.WhiteSmoke;

            // Заголовок
            Label lblTitle = new Label
            {
                Text = "Города и памятники России",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.DarkRed,
                AutoSize = true,
                Location = new Point(250, 30)
            };
            this.Controls.Add(lblTitle);

            // Подзаголовок
            Label lblSubtitle = new Label
            {
                Text = "Выберите уровень сложности:",
                Font = new Font("Segoe UI", 12),
                AutoSize = true,
                Location = new Point(280, 80)
            };
            this.Controls.Add(lblSubtitle);

            // Кнопки уровней
            btnLevel1 = new Button
            {
                Text = "🟢 Уровень 1: Легкий",
                Font = new Font("Segoe UI", 11),
                Size = new Size(300, 50),
                Location = new Point(250, 130),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Popup,
                Tag = 1
            };
            btnLevel1.Click += LevelButton_Click;
            this.Controls.Add(btnLevel1);

            btnLevel2 = new Button
            {
                Text = "🟡 Уровень 2: Средний",
                Font = new Font("Segoe UI", 11),
                Size = new Size(300, 50),
                Location = new Point(250, 190),
                BackColor = Color.LightYellow,
                FlatStyle = FlatStyle.Popup,
                Tag = 2,
                Enabled = XMLManager.IsLevelUnlocked(2)
            };
            btnLevel2.Click += LevelButton_Click;
            this.Controls.Add(btnLevel2);

            btnLevel3 = new Button
            {
                Text = "🔴 Уровень 3: Сложный",
                Font = new Font("Segoe UI", 11),
                Size = new Size(300, 50),
                Location = new Point(250, 250),
                BackColor = Color.LightCoral,
                FlatStyle = FlatStyle.Popup,
                Tag = 3,
                Enabled = XMLManager.IsLevelUnlocked(3)
            };
            btnLevel3.Click += LevelButton_Click;
            this.Controls.Add(btnLevel3);

            // Кнопка администратора
            Button btnAdmin = new Button
            {
                Text = "⚙️ Панель администратора",
                Font = new Font("Segoe UI", 10),
                Size = new Size(200, 35),
                Location = new Point(300, 330),
                BackColor = Color.LightBlue
            };
            btnAdmin.Click += (s, e) => {
                new AdminForm().ShowDialog();
                // Обновить состояние кнопок после закрытия админки
                btnLevel2.Enabled = XMLManager.IsLevelUnlocked(2);
                btnLevel3.Enabled = XMLManager.IsLevelUnlocked(3);
            };
            this.Controls.Add(btnAdmin);

            // Кнопка выхода
            Button btnExit = new Button
            {
                Text = "❌ Выход",
                Font = new Font("Segoe UI", 10),
                Size = new Size(100, 35),
                Location = new Point(350, 380),
                BackColor = Color.LightGray
            };
            btnExit.Click += (s, e) => Application.Exit();
            this.Controls.Add(btnExit);
        }

        private Button btnLevel1, btnLevel2, btnLevel3;

        private void LevelButton_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is int levelId)
            {
                try
                {
                    var questions = XMLManager.LoadQuestions(levelId);
                    if (questions == null || questions.Count == 0)
                    {
                        MessageBox.Show($"В уровне {levelId} нет вопросов!\nПроверьте файл data.xml",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var gameForm = new GameForm(levelId);
                    gameForm.FormClosed += (s, args) => {
                        if (gameForm.PassedWithHighScore)
                        {
                            XMLManager.UnlockNextLevel(levelId);
                            if (levelId == 1) btnLevel2.Enabled = true;
                            if (levelId == 2) btnLevel3.Enabled = true;
                        }
                    };
                    gameForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}\n\nПуть к XML: {Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.xml")}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}