using System;
using System.Drawing;
using System.Windows.Forms;

namespace Задание_8
{
    partial class ResultsForm
    {
        private System.ComponentModel.IContainer components = null;
        private ListBox listBoxResults;
        private Button btnClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.listBoxResults = new ListBox();
            this.btnClose = new Button();
            this.SuspendLayout();

            this.listBoxResults.Location = new Point(10, 10);
            this.listBoxResults.Size = new Size(360, 200);
            this.listBoxResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            this.btnClose.Text = "Закрыть";
            this.btnClose.Location = new Point(150, 220);
            this.btnClose.Click += new EventHandler(btnClose_Click);

            this.ClientSize = new Size(380, 250);
            this.Controls.Add(listBoxResults);
            this.Controls.Add(btnClose);
            this.Text = "Результаты игр";
            this.ResumeLayout(false);
        }
    }
}