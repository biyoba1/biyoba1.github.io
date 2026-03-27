using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DictionaryLib
{
    public class Slovar
    {
        private List<string> list = new List<string>();
        private string filename;
        private bool isModified = false;

        public Slovar() { }

        public Slovar(string filename)
        {
            this.filename = filename;
            OpenFile();
        }

        public int Count => list.Count;
        public bool IsModified => isModified;

        private void OpenFile()
        {
            try
            {
                list.Clear();
                if (!File.Exists(filename))
                {
                    throw new FileNotFoundException("Файл словаря не найден!");
                }

                using (StreamReader f = new StreamReader(filename, Encoding.UTF8))
                {
                    while (!f.EndOfStream)
                    {
                        string line = f.ReadLine()?.Trim();
                        if (!string.IsNullOrEmpty(line))
                            list.Add(line);
                    }
                }
                isModified = false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка доступа к файлу: {ex.Message}");
            }
        }

        public List<string> GetAllWords()
        {
            return new List<string>(list);
        }

        public bool AddWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return false;
            word = word.Trim();

            if (list.Any(w => w.Equals(word, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            list.Add(word);
            list.Sort(StringComparer.OrdinalIgnoreCase);
            isModified = true;
            return true;
        }

        public bool RemoveWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word)) return false;
            word = word.Trim();

            var existing = list.FirstOrDefault(w => w.Equals(word, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                list.Remove(existing);
                isModified = true;
                return true;
            }
            return false;
        }

        public void SaveToFile(string path)
        {
            using (StreamWriter f = new StreamWriter(path, false, Encoding.UTF8))
            {
                foreach (var word in list)
                {
                    f.WriteLine(word);
                }
            }
            isModified = false;
            this.filename = path;
        }

        // === ВАРИАНТ 15: Слова, начинающиеся и заканчивающиеся на одну букву ===
        public List<string> SearchVariant15(char? specificChar = null)
        {
            var results = new List<string>();

            foreach (var word in list)
            {
                if (string.IsNullOrEmpty(word) || word.Length < 1)
                    continue;

                char first = char.ToLowerInvariant(word[0]);
                char last = char.ToLowerInvariant(word[word.Length - 1]);

                if (first == last)
                {
                    if (specificChar == null)
                    {
                        results.Add(word);
                    }
                    else
                    {
                        char target = char.ToLowerInvariant(specificChar.Value);
                        if (first == target)
                        {
                            results.Add(word);
                        }
                    }
                }
            }
            return results;
        }

        // === НЕЧЁТКИЙ ПОИСК (Левенштейн) ===
        public List<string> FuzzySearch(string pattern, int maxDistance = 3)
        {
            var results = new List<string>();
            if (string.IsNullOrEmpty(pattern)) return results;

            foreach (var word in list)
            {
                int distance = CalculateLevenshteinDistance(pattern, word);
                if (distance <= maxDistance)
                {
                    results.Add(word);
                }
            }
            return results;
        }

        private int CalculateLevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; d[i, 0] = i++) { }
            for (int j = 0; j <= m; d[0, j] = j++) { }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            return d[n, m];
        }
    }
}