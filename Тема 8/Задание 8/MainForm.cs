using System;
using System.Drawing;
using System.Windows.Forms;

namespace Задание_8
{
    public partial class MainForm : Form
    {
        private Button[,] cells = new Button[10, 10];
        private int[,] mines = new int[10, 10];
        private int[,] hints = new int[10, 10];
        private bool[,] opened = new bool[10, 10];
        private bool[,] flagged = new bool[10, 10];

        private int mineCount = 10;
        private Color cellColor = Color.LightGray;
        private int flagsPlaced = 0;
        private bool gameActive = false;
        private int startTime;
        private Timer gameTimer;

        public MainForm(string userName)
        {
            InitializeComponent();

            gameTimer = new Timer();
            gameTimer.Interval = 1000;
            gameTimer.Tick += GameTimer_Tick;

            lblUser.Text = $"Игрок: {userName}";
            InitializeGrid();
            StartNewGame();
        }

        private void InitializeGrid()
        {
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowCount = 10;
            tableLayoutPanel1.ColumnCount = 10;
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.ColumnStyles.Clear();

            for (int i = 0; i < 10; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40));
                for (int j = 0; j < 10; j++)
                {
                    var btn = new Button();
                    btn.Size = new Size(40, 40);
                    btn.BackColor = cellColor;
                    btn.Font = new Font("Arial", 12, FontStyle.Bold);
                    btn.Tag = $"{i},{j}";
                    btn.Click += Cell_Click;
                    btn.MouseDown += Cell_MouseDown; // ✅ Обработка нажатия мыши (включая ПКМ)
                    cells[i, j] = btn;
                    tableLayoutPanel1.Controls.Add(btn, j, i);
                }
            }
        }

        private void StartNewGame()
        {
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                {
                    mines[i, j] = 0;
                    hints[i, j] = 0;
                    opened[i, j] = false;
                    flagged[i, j] = false;
                    cells[i, j].Text = "";
                    cells[i, j].BackColor = cellColor;
                    cells[i, j].Enabled = true;
                    cells[i, j].ForeColor = Color.Black;
                }

            flagsPlaced = 0;
            gameActive = true;
            startTime = 0;
            lblTime.Text = "Время: 0";
            lblStatus.Text = "Игра идет";
            lblStatus.ForeColor = Color.Black;

            Random rnd = new Random();
            int placed = 0;
            while (placed < mineCount)
            {
                int r = rnd.Next(10);
                int c = rnd.Next(10);
                if (mines[r, c] == 0)
                {
                    mines[r, c] = 1;
                    placed++;
                }
            }

            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    if (mines[i, j] == 0)
                        hints[i, j] = CountAdjacentMines(i, j);

            gameTimer.Start();
        }

        private int CountAdjacentMines(int r, int c)
        {
            int count = 0;
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {
                    int nr = r + i, nc = c + j;
                    if (nr >= 0 && nr < 10 && nc >= 0 && nc < 10)
                        if (mines[nr, nc] == 1) count++;
                }
            return count;
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            if (!gameActive) return;

            var btn = sender as Button;
            var coords = btn.Tag.ToString().Split(',');
            int r = int.Parse(coords[0]);
            int c = int.Parse(coords[1]);

            // ✅ Если стоит флажок - игнорируем левый клик
            if (flagged[r, c]) return;

            if (opened[r, c]) return;

            if (mines[r, c] == 1)
            {
                GameOver(false);
                btn.BackColor = Color.Red;
                btn.Text = "💣";
            }
            else
            {
                OpenCell(r, c);
                CheckWin();
            }
        }

        // ✅ Обработка нажатия кнопок мыши (для ПКМ)
        private void Cell_MouseDown(object sender, MouseEventArgs e)
        {
            if (!gameActive) return;

            // ✅ Правая кнопка мыши - установка/снятие флажка
            if (e.Button == MouseButtons.Right)
            {
                var btn = sender as Button;
                var coords = btn.Tag.ToString().Split(',');
                int r = int.Parse(coords[0]);
                int c = int.Parse(coords[1]);

                // Нельзя ставить флажок на открытую клетку
                if (opened[r, c]) return;

                if (flagged[r, c])
                {
                    // ✅ Снимаем флажок
                    flagged[r, c] = false;
                    btn.Text = "";
                    btn.BackColor = cellColor;
                    btn.ForeColor = Color.Black;
                    flagsPlaced--;
                }
                else
                {
                    // ✅ Ставим флажок
                    flagged[r, c] = true;
                    btn.Text = "🚩"; // Или "F" если эмодзи не работают
                    btn.ForeColor = Color.Red;
                    btn.BackColor = Color.LightYellow;
                    flagsPlaced++;
                }

                CheckWin();
            }
        }

        private void OpenCell(int r, int c)
        {
            if (r < 0 || r >= 10 || c < 0 || c >= 10) return;
            if (opened[r, c] || flagged[r, c]) return;

            opened[r, c] = true;
            cells[r, c].Enabled = false;
            cells[r, c].BackColor = Color.White;

            if (hints[r, c] > 0)
            {
                cells[r, c].Text = hints[r, c].ToString();
                switch (hints[r, c])
                {
                    case 1: cells[r, c].ForeColor = Color.Blue; break;
                    case 2: cells[r, c].ForeColor = Color.Green; break;
                    case 3: cells[r, c].ForeColor = Color.Red; break;
                    case 4: cells[r, c].ForeColor = Color.DarkBlue; break;
                    default: cells[r, c].ForeColor = Color.Black; break;
                }
            }
            else
            {
                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                        OpenCell(r + i, c + j);
            }
        }

        private void CheckWin()
        {
            int correctFlags = 0;
            int openedCells = 0;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (opened[i, j]) openedCells++;
                    if (flagged[i, j] && mines[i, j] == 1)
                        correctFlags++;
                }
            }

            bool allMinesFlagged = (correctFlags == mineCount);
            bool allSafeOpened = (openedCells == 100 - mineCount);

            if (allMinesFlagged || allSafeOpened)
            {
                GameOver(true);
            }
        }

        private void GameOver(bool win)
        {
            gameActive = false;
            gameTimer.Stop();

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (mines[i, j] == 1)
                    {
                        if (!flagged[i, j])
                        {
                            cells[i, j].Text = "💣";
                            cells[i, j].BackColor = Color.Red;
                        }
                    }
                    else if (flagged[i, j])
                    {
                        cells[i, j].BackColor = Color.Pink;
                        cells[i, j].Text = "❌";
                    }
                }
            }

            if (win)
            {
                lblStatus.Text = "🎉 ПОБЕДА! 🎉";
                lblStatus.ForeColor = Color.Green;
                MessageBox.Show($"Поздравляем! Вы выиграли за {startTime} секунд!",
                    "Победа", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                lblStatus.Text = "💥 ВЗРЫВ! 💥";
                lblStatus.ForeColor = Color.Red;
                MessageBox.Show("Вы подорвались на мине! Игра окончена.",
                    "Поражение", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Storage.SaveResult(new GameResult(CurrentUser.Login, win, startTime, mineCount));
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (gameActive)
            {
                startTime++;
                lblTime.Text = $"Время: {startTime}";
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e) => StartNewGame();

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var frm = new SettingsForm(mineCount, cellColor))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    mineCount = frm.MineCount;
                    cellColor = frm.CellColor;
                    StartNewGame();
                    InitializeGrid();
                }
            }
        }

        private void resultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new ResultsForm();
            frm.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) => Application.Exit();

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Игра 'Минное поле'. Вариант 15.\n\n" +
                           "Управление:\n" +
                           "• ЛКМ - открыть клетку\n" +
                           "• ПКМ - поставить/снять флажок\n\n" +
                           "Цель: найти все мины и отметить их флажками.",
                           "О программе",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information);
        }
    }
}