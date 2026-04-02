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
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "⚙️ Панель администратора";
            this.Size = new Size(700, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.WhiteSmoke;

            // Заголовок
            Label lblTitle = new Label
            {
                Text = "Добавление нового вопроса",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                AutoSize = true,
                Location = new Point(20, 15)
            };
            this.Controls.Add(lblTitle);

            // Выбор уровня
            Label lblLevel = new Label { Text = "Уровень сложности:", Location = new Point(30, 60), AutoSize = true };
            this.Controls.Add(lblLevel);

            cmbLevel = new ComboBox
            {
                Location = new Point(30, 85),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbLevel.Items.Add(new { Id = 1, Name = "1 — Легкий (Известные)" });
            cmbLevel.Items.Add(new { Id = 2, Name = "2 — Средний (Региональные)" });
            cmbLevel.Items.Add(new { Id = 3, Name = "3 — Сложный (Малоизвестные)" });
            cmbLevel.DisplayMember = "Name";
            cmbLevel.SelectedIndex = 0;
            this.Controls.Add(cmbLevel);

            // Путь к изображению
            Label lblImage = new Label { Text = "Путь к изображению:", Location = new Point(30, 130), AutoSize = true };
            this.Controls.Add(lblImage);

            txtImagePath = new TextBox { Location = new Point(30, 155), Width = 400 };
            this.Controls.Add(txtImagePath);

            Button btnBrowse = new Button
            {
                Text = "📁 Обзор...",
                Location = new Point(440, 153),
                Width = 100
            };
            btnBrowse.Click += (s, e) => {
                using (OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp";
                    dlg.InitialDirectory = Application.StartupPath + "\\images";
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        // Сохраняем относительный путь
                        string fullPath = dlg.FileName;
                        string relative = fullPath.StartsWith(Application.StartupPath)
                            ? fullPath.Substring(Application.StartupPath.Length + 1)
                            : "images\\" + Path.GetFileName(fullPath);
                        txtImagePath.Text = relative.Replace("\\", "/");
                    }
                }
            };
            this.Controls.Add(btnBrowse);

            // Подсказка
            Label lblHint = new Label { Text = "Подсказка:", Location = new Point(30, 195), AutoSize = true };
            this.Controls.Add(lblHint);

            txtHint = new TextBox { Location = new Point(30, 220), Width = 550, Multiline = true, Height = 50 };
            this.Controls.Add(txtHint);

            // Текст вопроса
            Label lblText = new Label { Text = "Текст вопроса:", Location = new Point(30, 280), AutoSize = true };
            this.Controls.Add(lblText);

            txtQuestion = new TextBox { Location = new Point(30, 305), Width = 550, Multiline = true, Height = 50 };
            this.Controls.Add(txtQuestion);

            // Варианты ответов
            Label lblAnswers = new Label { Text = "Варианты ответов (отметьте правильный):", Location = new Point(30, 370), AutoSize = true };
            this.Controls.Add(lblAnswers);

            txtAnswers = new TextBox[4];
            radCorrect = new RadioButton[4];

            for (int i = 0; i < 4; i++)
            {
                radCorrect[i] = new RadioButton
                {
                    Text = "",
                    Location = new Point(30, 400 + i * 35),
                    AutoSize = true,
                    TabIndex = i
                };
                this.Controls.Add(radCorrect[i]);

                txtAnswers[i] = new TextBox
                {
                    Location = new Point(50, 403 + i * 35),
                    Width = 530
                };
                txtAnswers[i].TextChanged += (s, e) => radCorrect[i].Text = txtAnswers[i].Text;
                this.Controls.Add(txtAnswers[i]);
            }

            // Кнопки
            Button btnSave = new Button
            {
                Text = "💾 Сохранить вопрос",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(180, 40),
                Location = new Point(30, 550),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Popup
            };
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            Button btnClear = new Button
            {
                Text = "🗑️ Очистить",
                Size = new Size(100, 40),
                Location = new Point(220, 550),
                BackColor = Color.LightGray
            };
            btnClear.Click += (s, e) => ClearForm();
            this.Controls.Add(btnClear);

            Button btnClose = new Button
            {
                Text = "❌ Закрыть",
                Size = new Size(100, 40),
                Location = new Point(550, 550),
                BackColor = Color.LightCoral
            };
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);
        }

        private ComboBox cmbLevel;
        private TextBox txtImagePath, txtHint, txtQuestion;
        private TextBox[] txtAnswers;
        private RadioButton[] radCorrect;

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(txtQuestion.Text))
            {
                MessageBox.Show("⚠️ Введите текст вопроса!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedLevel = (dynamic)cmbLevel.SelectedItem;
            int levelId = selectedLevel.Id;

            // Собираем данные
            Question q = new Question
            {
                ImagePath = txtImagePath.Text.Trim(),
                Hint = txtHint.Text.Trim(),
                Text = txtQuestion.Text.Trim(),
                Answers = new List<string>(),
                CorrectIndex = -1
            };

            for (int i = 0; i < 4; i++)
            {
                if (string.IsNullOrWhiteSpace(txtAnswers[i].Text))
                {
                    MessageBox.Show($"⚠️ Заполните вариант ответа #{i + 1}!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                q.Answers.Add(txtAnswers[i].Text.Trim());
                if (radCorrect[i].Checked)
                    q.CorrectIndex = i;
            }

            if (q.CorrectIndex == -1)
            {
                MessageBox.Show("⚠️ Отметьте правильный вариант ответа!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Сохраняем в XML
            try
            {
                XMLManager.AddQuestion(levelId, q);
                MessageBox.Show("✅ Вопрос успешно добавлен!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            txtImagePath.Clear();
            txtHint.Clear();
            txtQuestion.Clear();
            for (int i = 0; i < 4; i++)
            {
                txtAnswers[i].Clear();
                radCorrect[i].Checked = false;
            }
            txtQuestion.Focus();
        }
    }
}