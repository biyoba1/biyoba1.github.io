using System;
using System.Drawing;
using System.Windows.Forms;

namespace Задание_8
{
    public partial class SettingsForm : Form
    {
        public int MineCount { get; private set; }
        public Color CellColor { get; private set; }

        public SettingsForm(int currentMines, Color currentColor)
        {
            InitializeComponent();
            MineCount = currentMines;
            CellColor = currentColor;
            numMines.Value = currentMines;
            pnlColor.BackColor = currentColor;
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            using (var dlg = new ColorDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    CellColor = dlg.Color;
                    pnlColor.BackColor = CellColor;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            MineCount = (int)numMines.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}