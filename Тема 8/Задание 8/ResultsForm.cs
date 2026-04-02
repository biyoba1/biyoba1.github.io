using System;
using System.Windows.Forms;

namespace Задание_8
{
    public partial class ResultsForm : Form
    {
        public ResultsForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            listBoxResults.Items.Clear();
            var results = Storage.LoadResults(CurrentUser.Login);

            if (results.Count == 0)
            {
                listBoxResults.Items.Add("Нет сохраненных игр.");
                return;
            }

            foreach (var r in results)
            {
                string status = r.IsWin ? "Победа" : "Поражение";
                string item = $"{r.Date.ToShortDateString()} | {status} | Время: {r.TimeSeconds}с | Мин: {r.Difficulty}";
                listBoxResults.Items.Add(item);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}