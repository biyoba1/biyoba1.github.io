using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form2 : Form
    {
        // Поля для хранения настроек
        public Color SquareColor { get; set; } = Color.Blue;  // значение по умолчанию
        public int Speed { get; set; } = 5;                    // значение по умолчанию

        public Form2()
        {
            InitializeComponent();
        }

        // Загрузка формы: инициализация значений из полей в элементы управления
        private void Form2_Load(object sender, EventArgs e)
        {
            panel1.BackColor = SquareColor;

            // Проверка диапазона перед установкой значения
            if (Speed < trackBar1.Minimum)
                Speed = trackBar1.Minimum;
            if (Speed > trackBar1.Maximum)
                Speed = trackBar1.Maximum;

            trackBar1.Value = Speed;
            UpdateIntervalLabel();
        }

        private void UpdateIntervalLabel()
        {
            int interval = 100 - Speed * 10;
            label3.Text = $"Интервал: {interval} мс";
        }

        
        // Кнопка "Выбрать цвет"
        private void button1_Click(object sender, EventArgs e)
        {
            using (ColorDialog dlg = new ColorDialog())
            {
                dlg.Color = panel1.BackColor;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    SquareColor = dlg.Color;
                    panel1.BackColor = SquareColor; // сразу показываем выбор
                }
            }
        }

        // Ползунок скорости
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Speed = trackBar1.Value;
            UpdateIntervalLabel();
        }

        // Кнопка "OK" — сохраняем и закрываем
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Кнопка "Отмена" — закрываем без сохранения
        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}