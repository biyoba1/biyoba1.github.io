using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DollarCourse
{
    public partial class Form1 : Form
    {
        private ArrayCurrencyData currencyData;

        public Form1()
        {
            InitializeComponent();
            currencyData = new ArrayCurrencyData();
            cmbMonth.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtYear.Text))
                {
                    MessageBox.Show("Введите год!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbMonth.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите месяц!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtRate.Text))
                {
                    MessageBox.Show("Введите курс доллара!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                double rate = double.Parse(txtRate.Text);
                if (rate <= 0)
                {
                    MessageBox.Show("Курс должен быть положительным числом!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string month = cmbMonth.SelectedItem.ToString();

                currencyData.Year = txtYear.Text;
                currencyData.Add(new CurrencyData(month, rate));

                UpdateTable();

                MessageBox.Show("Данные добавлены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtRate.Clear();
                if (cmbMonth.SelectedIndex < 11)
                    cmbMonth.SelectedIndex++;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateTable()
        {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < currencyData.Count; i++)
            {
                dataGridView1.Rows.Add(currencyData[i].Month, currencyData[i].Rate.ToString("F2"));
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (currencyData.Count == 0)
            {
                MessageBox.Show("Нет данных для сохранения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveDialog.Title = "Сохранить данные";
            saveDialog.FileName = "currency_data.txt";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                currencyData.SaveToFile(saveDialog.FileName);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openDialog.Title = "Загрузить данные";

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                currencyData.LoadFromFile(openDialog.FileName);
                txtYear.Text = currencyData.Year;
                UpdateTable();
            }
        }

        private void btnBuildChart_Click(object sender, EventArgs e)
        {
            if (currencyData.Count == 0)
            {
                MessageBox.Show("Нет данных для построения диаграммы!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            pnlChart.Invalidate();
        }

        private void pnlChart_Paint(object sender, PaintEventArgs e)
        {
            if (currencyData.Count == 0) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            // Отступы
            int paddingLeft = 60, paddingRight = 20, paddingTop = 50, paddingBottom = 60;
            int chartWidth = pnlChart.ClientRectangle.Width - paddingLeft - paddingRight;
            int chartHeight = pnlChart.ClientRectangle.Height - paddingTop - paddingBottom;

            if (chartWidth <= 0 || chartHeight <= 0) return;

            // Фон панели
            using (var bgBrush = new LinearGradientBrush(
                pnlChart.ClientRectangle,
                Color.WhiteSmoke, Color.LightGray, LinearGradientMode.ForwardDiagonal))
            {
                g.FillRectangle(bgBrush, pnlChart.ClientRectangle);
            }

            // Область построения
            Rectangle chartArea = new Rectangle(paddingLeft, paddingTop, chartWidth, chartHeight);
            g.FillRectangle(Brushes.White, chartArea);
            g.DrawRectangle(Pens.Gray, chartArea);

            // Поиск максимума
            double maxRate = 0;
            for (int i = 0; i < currencyData.Count; i++)
            {
                if (currencyData[i].Rate > maxRate)
                    maxRate = currencyData[i].Rate;
            }
            if (maxRate == 0) maxRate = 1;
            maxRate = maxRate * 1.1; // Запас сверху

            // Заголовок
            using (Font titleFont = new Font("Arial", 12, FontStyle.Bold))
            {
                string title = $"Динамика курса доллара в {currencyData.Year} году";
                SizeF titleSize = g.MeasureString(title, titleFont);
                g.DrawString(title, titleFont, Brushes.DarkBlue,
                    (pnlChart.Width - titleSize.Width) / 2, 10);
            }

            // Подписи осей
            using (Font axisFont = new Font("Arial", 9, FontStyle.Italic))
            {
                // Ось Y
                g.DrawString("Курс (₽)", axisFont, Brushes.Black, 5, paddingTop + chartHeight / 2);
                // Ось X
                string xAxisLabel = "Месяц";
                SizeF xAxisSize = g.MeasureString(xAxisLabel, axisFont);
                g.DrawString(xAxisLabel, axisFont, Brushes.Black,
                    paddingLeft + chartWidth / 2 - xAxisSize.Width / 2,
                    paddingTop + chartHeight + 35);
            }

            // Сетка горизонтальная
            using (Pen gridPen = new Pen(Color.LightGray, 1) { DashStyle = DashStyle.Dot })
            using (Font gridFont = new Font("Arial", 7))
            {
                for (int i = 0; i <= 4; i++)
                {
                    int y = paddingTop + chartHeight - i * chartHeight / 4;
                    g.DrawLine(gridPen, paddingLeft, y, paddingLeft + chartWidth, y);

                    double value = maxRate * i / 4;
                    string label = value.ToString("F0");
                    SizeF labelSize = g.MeasureString(label, gridFont);
                    g.DrawString(label, gridFont, Brushes.Gray,
                        paddingLeft - labelSize.Width - 5, y - labelSize.Height / 2);
                }
            }

            // Отрисовка столбцов
            int barCount = currencyData.Count;
            int totalSpacing = 15;
            int availableWidth = chartWidth - totalSpacing * (barCount + 1);
            int barWidth = Math.Max(15, Math.Min(60, availableWidth / barCount));

            using (Font monthFont = new Font("Arial", 7))
            using (Font valueFont = new Font("Arial", 8, FontStyle.Bold))
            using (StringFormat centerFormat = new StringFormat { Alignment = StringAlignment.Center })
            {
                for (int i = 0; i < barCount; i++)
                {
                    CurrencyData item = currencyData[i];

                    // Позиция столбца
                    int x = paddingLeft + totalSpacing + i * (barWidth + totalSpacing);
                    int barHeight = (int)(item.Rate / maxRate * (chartHeight - 10));
                    if (barHeight < 2) barHeight = 2;
                    int y = paddingTop + chartHeight - barHeight;

                    // Градиент столбца
                    using (Brush barBrush = new LinearGradientBrush(
                        new Rectangle(x, y, barWidth, barHeight),
                        Color.FromArgb(0, 150, 0), Color.FromArgb(0, 80, 0), LinearGradientMode.Vertical))
                    {
                        g.FillRectangle(barBrush, x, y, barWidth, barHeight);
                        g.DrawRectangle(Pens.DarkGreen, x, y, barWidth, barHeight);
                    }

                    // Значение над столбцом
                    string valueText = item.Rate.ToString("F2");
                    SizeF valueSize = g.MeasureString(valueText, valueFont);
                    g.DrawString(valueText, valueFont, Brushes.DarkBlue,
                        x + barWidth / 2 - valueSize.Width / 2, y - valueSize.Height - 2);

                    // Подпись месяца
                    g.DrawString(item.Month, monthFont, Brushes.Black,
                        x + barWidth / 2, paddingTop + chartHeight + 5, centerFormat);
                }
            }

            // Линия нуля
            g.DrawLine(Pens.Black, paddingLeft, paddingTop + chartHeight,
                paddingLeft + chartWidth, paddingTop + chartHeight);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (pnlChart != null)
                pnlChart.Invalidate();
        }
    }
}