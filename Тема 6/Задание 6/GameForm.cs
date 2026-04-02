using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Задание_6
{
    public partial class GameForm : Form
    {
        private List<Question> _sessionQuestions;
        private int _currentQuestionIndex = 0;
        private int _score = 0;
        private int _sessionTime = 300; // 5 минут в секундах
        private Timer _timer;
        private int _levelId;

        // Публичное свойство для проверки прохождения
        public bool PassedWithHighScore { get; private set; } = false;

        public GameForm(int levelId)
        {
            InitializeComponent();
            _levelId = levelId;
            SetupUI();
            StartGame();
        }

        private void SetupUI()
        {
            this.Text = $"Уровень {_levelId} — Викторина";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1000, 700);  // Увеличили высоту
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            // Панель с таймером и счётом (уменьшили высоту)
            Panel topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,  // Было 60
                BackColor = Color.DarkBlue
            };

            lblTimer = new Label
            {
                Text = "⏱️ Время: 05:00",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),  // Чуть меньше шрифт
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 15)
            };
            topPanel.Controls.Add(lblTimer);

            lblScore = new Label
            {
                Text = "💯 Счёт: 0/100",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.Yellow,
                AutoSize = true,
                Location = new Point(780, 15)
            };
            topPanel.Controls.Add(lblScore);

            this.Controls.Add(topPanel);

            // Основная область вопроса
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(15),
                AutoScroll = true  // Добавили прокрутку на всякий случай
            };

            // Изображение (слева)
            pictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(450, 400),  // Чуть больше
                Location = new Point(15, 15),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.WhiteSmoke
            };
            mainPanel.Controls.Add(pictureBox);

            // Текст вопроса (справа сверху) - ИСПРАВЛЕНО
            lblQuestion = new Label
            {
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = false,
                Size = new Size(480, 80),  // Увеличили высоту
                Location = new Point(480, 15),  // Отступ сверху
                TextAlign = ContentAlignment.TopLeft,
                BackColor = Color.LightYellow,  // Фон для видимости
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(5)
            };
            mainPanel.Controls.Add(lblQuestion);

            // Подсказка (под вопросом)
            lblHint = new Label
            {
                Text = "💡 Подсказка:",
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.DarkGreen,
                AutoSize = false,
                Size = new Size(480, 50),
                Location = new Point(480, 100),  // Под вопросом
                BackColor = Color.White,
                Padding = new Padding(3)
            };
            mainPanel.Controls.Add(lblHint);

            // RadioButton для ответов (в GroupBox)
            GroupBox answersGroup = new GroupBox
            {
                Text = "Выберите правильный ответ:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(480, 180),
                Location = new Point(480, 160),  // Подсказка + отступ
                BackColor = Color.White
            };

            radioAnswers = new RadioButton[4];
            for (int i = 0; i < 4; i++)
            {
                radioAnswers[i] = new RadioButton
                {
                    Text = $"Вариант {i + 1}",
                    Font = new Font("Segoe UI", 10),
                    AutoSize = true,
                    Location = new Point(15, 25 + i * 35),  // Больше места между вариантами
                    TabIndex = i,
                    BackColor = Color.White
                };
                answersGroup.Controls.Add(radioAnswers[i]);
            }
            mainPanel.Controls.Add(answersGroup);

            // Кнопка ответа
            Button btnAnswer = new Button
            {
                Text = "✅ Ответить",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(150, 40),
                Location = new Point(480, 350),  // Под ответами
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Popup
            };
            btnAnswer.Click += BtnAnswer_Click;
            mainPanel.Controls.Add(btnAnswer);

            // Кнопка "В главное меню"
            Button btnMenu = new Button
            {
                Text = "🏠 В меню",
                Font = new Font("Segoe UI", 9),
                Size = new Size(120, 30),
                Location = new Point(800, 355),
                BackColor = Color.LightGray
            };
            btnMenu.Click += (s, e) => { _timer?.Stop(); this.Close(); };
            mainPanel.Controls.Add(btnMenu);

            // Прогресс (под изображением)
            lblProgress = new Label
            {
                Text = "Вопрос 1 из 5",
                Font = new Font("Segoe UI", 9),
                AutoSize = true,
                Location = new Point(15, 425)
            };
            mainPanel.Controls.Add(lblProgress);

            this.Controls.Add(mainPanel);
        }

        private PictureBox pictureBox;
        private Label lblQuestion, lblHint, lblTimer, lblScore, lblProgress;
        private RadioButton[] radioAnswers;

        private void StartGame()
        {
            // Загружаем все вопросы уровня и выбираем 5 случайных
            var allQuestions = XMLManager.LoadQuestions(_levelId);
            var rand = new Random();
            _sessionQuestions = allQuestions.OrderBy(q => rand.Next()).Take(5).ToList();

            if (_sessionQuestions.Count == 0)
            {
                MessageBox.Show("❌ Нет вопросов для этого уровня!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            // Таймер
            _timer = new Timer { Interval = 1000 };
            _timer.Tick += Timer_Tick;
            _timer.Start();

            ShowCurrentQuestion();
        }

        private void ShowCurrentQuestion()
        {
            if (_currentQuestionIndex >= _sessionQuestions.Count)
            {
                EndGame();
                return;
            }

            var q = _sessionQuestions[_currentQuestionIndex];

            // Загружаем изображение
            if (!string.IsNullOrEmpty(q.ImagePath) && System.IO.File.Exists(q.ImagePath))
            {
                try { pictureBox.Image = Image.FromFile(q.ImagePath); }
                catch { pictureBox.Image = null; }
            }
            else
            {
                pictureBox.Image = null;
            }

            lblQuestion.Text = q.Text;
            lblHint.Text = "💡 " + q.Hint;
            lblProgress.Text = $"Вопрос {_currentQuestionIndex + 1} из {_sessionQuestions.Count}";

            // Заполняем варианты ответов
            for (int i = 0; i < 4; i++)
            {
                if (i < q.Answers.Count)
                    radioAnswers[i].Text = q.Answers[i];
                else
                    radioAnswers[i].Text = "";
                radioAnswers[i].Checked = false;
                radioAnswers[i].Enabled = true;
            }
        }

        private void BtnAnswer_Click(object sender, EventArgs e)
        {
            // Проверяем выбор
            int selected = -1;
            for (int i = 0; i < 4; i++)
            {
                if (radioAnswers[i].Checked)
                {
                    selected = i;
                    break;
                }
            }

            if (selected == -1)
            {
                MessageBox.Show("⚠️ Выберите вариант ответа!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверяем правильность
            var q = _sessionQuestions[_currentQuestionIndex];
            if (selected == q.CorrectIndex)
            {
                _score += 20; // 5 вопросов * 20 = 100 баллов
                lblScore.Text = $"💯 Счёт: {_score}/100";
            }

            // Блокируем выбор и переходим к следующему
            for (int i = 0; i < 4; i++)
                radioAnswers[i].Enabled = false;

            _currentQuestionIndex++;

            // Небольшая задержка перед следующим вопросом
            Timer nextTimer = new Timer { Interval = 800 };
            nextTimer.Tick += (s, args) => {
                nextTimer.Stop();
                ShowCurrentQuestion();
            };
            nextTimer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _sessionTime--;
            int mins = _sessionTime / 60;
            int secs = _sessionTime % 60;
            lblTimer.Text = $"⏱️ Время: {mins:00}:{secs:00}";

            if (_sessionTime <= 0)
            {
                _timer.Stop();
                EndGame();
            }
        }

        private void EndGame()
        {
            _timer?.Stop();

            string message = $"🎮 Сеанс окончен!\n\nВаш счёт: {_score}/100\n";

            if (_score >= 80)
            {
                message += "\n✅ Поздравляем! Уровень пройден!\nДоступен следующий уровень сложности.";
                PassedWithHighScore = true;
            }
            else
            {
                message += "\n❌ Для перехода дальше нужно набрать ≥80 баллов.\nПопробуйте ещё раз!";
                PassedWithHighScore = false;
            }

            MessageBox.Show(message, "Результат",
                MessageBoxButtons.OK,
                _score >= 80 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _timer?.Stop();
            base.OnFormClosing(e);
        }
    }
}