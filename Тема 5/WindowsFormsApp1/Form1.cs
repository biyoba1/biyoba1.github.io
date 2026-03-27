using DictionaryLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Slovar currentDictionary;

        public Form1()
        {
            InitializeComponent();
            chkAnyLetter.Checked = true;
            txtSpecificChar.Enabled = false;
        }

        private void chkAnyLetter_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAnyLetter.Checked)
            {
                txtSpecificChar.Enabled = false;
                txtSpecificChar.Clear();
            }
            else
            {
                txtSpecificChar.Enabled = true;
                txtSpecificChar.Focus();
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text Files|*.txt";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        currentDictionary = new Slovar(ofd.FileName);
                        UpdateListBox(listBoxDictionary, currentDictionary.GetAllWords());
                        MessageBox.Show($"Словарь загружен. Слов: {currentDictionary.Count}", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (currentDictionary == null)
            {
                MessageBox.Show("Сначала загрузите словарь!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (currentDictionary.IsModified)
            {
                var res = MessageBox.Show("Словарь был изменен. Сохранить в новый файл?",
                    "Подтверждение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    using (SaveFileDialog sfd = new SaveFileDialog())
                    {
                        sfd.Filter = "Text Files|*.txt";
                        sfd.FileName = "modified_dictionary.txt";
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            currentDictionary.SaveToFile(sfd.FileName);
                            MessageBox.Show("Изменения сохранены в новый файл.", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Изменений нет.", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (currentDictionary == null)
            {
                MessageBox.Show("Сначала загрузите словарь!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string word = txtNewWord.Text;
            if (currentDictionary.AddWord(word))
            {
                UpdateListBox(listBoxDictionary, currentDictionary.GetAllWords());
                txtNewWord.Clear();
                MessageBox.Show("Слово добавлено.", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Слово уже существует или пустое.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (currentDictionary == null)
            {
                MessageBox.Show("Сначала загрузите словарь!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (listBoxDictionary.SelectedItem != null)
            {
                string word = listBoxDictionary.SelectedItem.ToString();
                if (currentDictionary.RemoveWord(word))
                {
                    UpdateListBox(listBoxDictionary, currentDictionary.GetAllWords());
                }
            }
            else
            {
                MessageBox.Show("Выберите слово в списке для удаления.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSearchVar15_Click(object sender, EventArgs e)
        {
            if (currentDictionary == null)
            {
                MessageBox.Show("Сначала загрузите словарь!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            char? specificChar = null;

            if (!chkAnyLetter.Checked)
            {
                string input = txtSpecificChar.Text?.Trim();
                if (string.IsNullOrEmpty(input))
                {
                    MessageBox.Show("Введите одну букву для поиска!", "Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSpecificChar.Focus();
                    return;
                }
                specificChar = input[0];
            }

            List<string> results = currentDictionary.SearchVariant15(specificChar);
            UpdateListBox(listBoxResults, results);

            if (results.Count == 0)
            {
                string msg = specificChar == null
                    ? "Не найдено слов, начинающихся и заканчивающихся на одну букву."
                    : $"Не найдено слов на букву '{specificChar}', начинающихся и заканчивающихся на неё.";
                MessageBox.Show(msg, "Результат",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Найдено слов: {results.Count}", "Результат",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnFuzzySearch_Click(object sender, EventArgs e)
        {
            if (currentDictionary == null)
            {
                MessageBox.Show("Сначала загрузите словарь!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string pattern = txtFuzzyPattern.Text;
            if (string.IsNullOrEmpty(pattern))
            {
                MessageBox.Show("Введите слово для поиска.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<string> results = currentDictionary.FuzzySearch(pattern, 3);
            UpdateListBox(listBoxResults, results);

            if (results.Count == 0)
            {
                MessageBox.Show("Нечеткий поиск не дал результатов.", "Результат",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Найдено слов: {results.Count}", "Результат",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSaveResults_Click(object sender, EventArgs e)
        {
            if (listBoxResults.Items.Count == 0)
            {
                MessageBox.Show("Нет результатов для сохранения.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Text Files|*.txt";
                sfd.FileName = "search_results.txt";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllLines(sfd.FileName, GetItems(listBoxResults), Encoding.UTF8);
                        MessageBox.Show("Результаты сохранены.", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void UpdateListBox(ListBox lb, List<string> data)
        {
            lb.Items.Clear();
            foreach (var item in data)
            {
                lb.Items.Add(item);
            }
        }

        private string[] GetItems(ListBox lb)
        {
            string[] arr = new string[lb.Items.Count];
            lb.Items.CopyTo(arr, 0);
            return arr;
        }
    }
}