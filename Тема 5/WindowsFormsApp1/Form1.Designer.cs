namespace WindowsFormsApp1
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.listBoxDictionary = new System.Windows.Forms.ListBox();
            this.listBoxResults = new System.Windows.Forms.ListBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtNewWord = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkAnyLetter = new System.Windows.Forms.CheckBox();
            this.txtSpecificChar = new System.Windows.Forms.TextBox();
            this.btnSearchVar15 = new System.Windows.Forms.Button();
            this.lblSpecificChar = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtFuzzyPattern = new System.Windows.Forms.TextBox();
            this.btnFuzzySearch = new System.Windows.Forms.Button();
            this.btnSaveResults = new System.Windows.Forms.Button();
            this.lblDictionary = new System.Windows.Forms.Label();
            this.lblResults = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxDictionary
            // 
            this.listBoxDictionary.FormattingEnabled = true;
            this.listBoxDictionary.Location = new System.Drawing.Point(12, 35);
            this.listBoxDictionary.Name = "listBoxDictionary";
            this.listBoxDictionary.Size = new System.Drawing.Size(250, 300);
            this.listBoxDictionary.TabIndex = 0;
            // 
            // listBoxResults
            // 
            this.listBoxResults.FormattingEnabled = true;
            this.listBoxResults.Location = new System.Drawing.Point(280, 35);
            this.listBoxResults.Name = "listBoxResults";
            this.listBoxResults.Size = new System.Drawing.Size(250, 300);
            this.listBoxResults.TabIndex = 1;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(12, 350);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(120, 30);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "Загрузить словарь";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(142, 350);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 30);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Сохранить словарь";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtNewWord
            // 
            this.txtNewWord.Location = new System.Drawing.Point(12, 390);
            this.txtNewWord.Name = "txtNewWord";
            this.txtNewWord.Size = new System.Drawing.Size(150, 20);
            this.txtNewWord.TabIndex = 4;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(168, 388);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(94, 24);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(12, 418);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(250, 30);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Удалить выбранное слово";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkAnyLetter);
            this.groupBox1.Controls.Add(this.txtSpecificChar);
            this.groupBox1.Controls.Add(this.btnSearchVar15);
            this.groupBox1.Controls.Add(this.lblSpecificChar);
            this.groupBox1.Location = new System.Drawing.Point(280, 350);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 100);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Поиск Вариант 15";
            // 
            // chkAnyLetter
            // 
            this.chkAnyLetter.AutoSize = true;
            this.chkAnyLetter.Checked = true;
            this.chkAnyLetter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAnyLetter.Location = new System.Drawing.Point(10, 20);
            this.chkAnyLetter.Name = "chkAnyLetter";
            this.chkAnyLetter.Size = new System.Drawing.Size(90, 17);
            this.chkAnyLetter.TabIndex = 0;
            this.chkAnyLetter.Text = "Любая буква";
            this.chkAnyLetter.UseVisualStyleBackColor = true;
            this.chkAnyLetter.CheckedChanged += new System.EventHandler(this.chkAnyLetter_CheckedChanged);
            // 
            // txtSpecificChar
            // 
            this.txtSpecificChar.Enabled = false;
            this.txtSpecificChar.Location = new System.Drawing.Point(100, 18);
            this.txtSpecificChar.MaxLength = 1;
            this.txtSpecificChar.Name = "txtSpecificChar";
            this.txtSpecificChar.Size = new System.Drawing.Size(30, 20);
            this.txtSpecificChar.TabIndex = 1;
            // 
            // btnSearchVar15
            // 
            this.btnSearchVar15.Location = new System.Drawing.Point(10, 50);
            this.btnSearchVar15.Name = "btnSearchVar15";
            this.btnSearchVar15.Size = new System.Drawing.Size(230, 30);
            this.btnSearchVar15.TabIndex = 2;
            this.btnSearchVar15.Text = "Найти (Вар 15)";
            this.btnSearchVar15.UseVisualStyleBackColor = true;
            this.btnSearchVar15.Click += new System.EventHandler(this.btnSearchVar15_Click);
            // 
            // lblSpecificChar
            // 
            this.lblSpecificChar.AutoSize = true;
            this.lblSpecificChar.Location = new System.Drawing.Point(136, 21);
            this.lblSpecificChar.Name = "lblSpecificChar";
            this.lblSpecificChar.Size = new System.Drawing.Size(80, 13);
            this.lblSpecificChar.TabIndex = 3;
            this.lblSpecificChar.Text = "(введите букву)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtFuzzyPattern);
            this.groupBox2.Controls.Add(this.btnFuzzySearch);
            this.groupBox2.Location = new System.Drawing.Point(280, 460);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(250, 80);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Нечеткий поиск";
            // 
            // txtFuzzyPattern
            // 
            this.txtFuzzyPattern.Location = new System.Drawing.Point(10, 20);
            this.txtFuzzyPattern.Name = "txtFuzzyPattern";
            this.txtFuzzyPattern.Size = new System.Drawing.Size(150, 20);
            this.txtFuzzyPattern.TabIndex = 0;
            // 
            // btnFuzzySearch
            // 
            this.btnFuzzySearch.Location = new System.Drawing.Point(166, 18);
            this.btnFuzzySearch.Name = "btnFuzzySearch";
            this.btnFuzzySearch.Size = new System.Drawing.Size(75, 24);
            this.btnFuzzySearch.TabIndex = 1;
            this.btnFuzzySearch.Text = "Поиск";
            this.btnFuzzySearch.UseVisualStyleBackColor = true;
            this.btnFuzzySearch.Click += new System.EventHandler(this.btnFuzzySearch_Click);
            // 
            // btnSaveResults
            // 
            this.btnSaveResults.Location = new System.Drawing.Point(280, 550);
            this.btnSaveResults.Name = "btnSaveResults";
            this.btnSaveResults.Size = new System.Drawing.Size(250, 30);
            this.btnSaveResults.TabIndex = 9;
            this.btnSaveResults.Text = "Сохранить результаты в файл";
            this.btnSaveResults.UseVisualStyleBackColor = true;
            this.btnSaveResults.Click += new System.EventHandler(this.btnSaveResults_Click);
            // 
            // lblDictionary
            // 
            this.lblDictionary.AutoSize = true;
            this.lblDictionary.Location = new System.Drawing.Point(12, 19);
            this.lblDictionary.Name = "lblDictionary";
            this.lblDictionary.Size = new System.Drawing.Size(100, 13);
            this.lblDictionary.TabIndex = 10;
            this.lblDictionary.Text = "Словарь (весь)";
            // 
            // lblResults
            // 
            this.lblResults.AutoSize = true;
            this.lblResults.Location = new System.Drawing.Point(280, 19);
            this.lblResults.Name = "lblResults";
            this.lblResults.Size = new System.Drawing.Size(120, 13);
            this.lblResults.TabIndex = 11;
            this.lblResults.Text = "Результаты поиска";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 600);
            this.Controls.Add(this.lblResults);
            this.Controls.Add(this.lblDictionary);
            this.Controls.Add(this.btnSaveResults);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtNewWord);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.listBoxResults);
            this.Controls.Add(this.listBoxDictionary);
            this.Name = "Form1";
            this.Text = "Обработка текстовых файлов - Вариант 15";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ListBox listBoxDictionary;
        private System.Windows.Forms.ListBox listBoxResults;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtNewWord;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkAnyLetter;
        private System.Windows.Forms.TextBox txtSpecificChar;
        private System.Windows.Forms.Button btnSearchVar15;
        private System.Windows.Forms.Label lblSpecificChar;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtFuzzyPattern;
        private System.Windows.Forms.Button btnFuzzySearch;
        private System.Windows.Forms.Button btnSaveResults;
        private System.Windows.Forms.Label lblDictionary;
        private System.Windows.Forms.Label lblResults;
    }
}