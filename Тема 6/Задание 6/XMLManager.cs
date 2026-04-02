using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;

namespace Задание_6
{
    public static class XMLManager
    {
        private static string xmlPath;

        static XMLManager()
        {
            // Пробуем несколько возможных путей
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            xmlPath = Path.Combine(basePath, "data.xml");

            // Если файл не найден, пробуем относительный путь
            if (!File.Exists(xmlPath))
            {
                xmlPath = "data.xml";
            }
        }

        public static List<Question> LoadQuestions(int levelId)
        {
            List<Question> questions = new List<Question>();

            try
            {
                if (!File.Exists(xmlPath))
                {
                    MessageBox.Show($"Файл data.xml не найден!\nПуть: {Path.GetFullPath(xmlPath)}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return questions;
                }

                XDocument doc = XDocument.Load(xmlPath);

                var level = doc.Descendants("level")
                    .FirstOrDefault(l => l.Attribute("id") != null &&
                                         l.Attribute("id").Value == levelId.ToString());

                if (level == null)
                {
                    MessageBox.Show($"Уровень {levelId} не найден в XML!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return questions;
                }

                foreach (var qElement in level.Descendants("question"))
                {
                    Question q = new Question
                    {
                        ImagePath = GetElementValue(qElement, "image"),
                        Hint = GetElementValue(qElement, "hint"),
                        Text = GetElementValue(qElement, "text"),
                        Answers = new List<string>(),
                        CorrectIndex = 0
                    };

                    var answersElement = qElement.Element("answers");
                    if (answersElement != null)
                    {
                        int index = 0;
                        foreach (var answer in answersElement.Elements("answer"))
                        {
                            q.Answers.Add(answer.Value);
                            if (answer.Attribute("right") != null &&
                                answer.Attribute("right").Value == "yes")
                            {
                                q.CorrectIndex = index;
                            }
                            index++;
                        }
                    }

                    questions.Add(q);
                }

                if (questions.Count == 0)
                {
                    MessageBox.Show($"В уровне {levelId} не найдено вопросов!\n" +
                                  $"Всего уровней в файле: {doc.Descendants("level").Count()}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка чтения XML: {ex.Message}\n\n" +
                              $"Путь к файлу: {Path.GetFullPath(xmlPath)}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return questions;
        }

        private static string GetElementValue(XElement parent, string elementName)
        {
            var element = parent.Element(elementName);
            return element != null ? element.Value : "";
        }

        public static void AddQuestion(int levelId, Question q)
        {
            XDocument doc;

            if (File.Exists(xmlPath))
                doc = XDocument.Load(xmlPath);
            else
            {
                doc = CreateDefaultXML();
            }

            var levelNode = doc.Descendants("level")
                               .FirstOrDefault(l => l.Attribute("id")?.Value == levelId.ToString());

            if (levelNode != null)
            {
                XElement newQuestion = new XElement("question",
                    new XElement("image", q.ImagePath ?? ""),
                    new XElement("hint", q.Hint ?? ""),
                    new XElement("text", q.Text ?? ""),
                    new XElement("answers",
                        q.Answers.Select((ans, idx) =>
                            new XElement("answer",
                                new XAttribute("right", idx == q.CorrectIndex ? "yes" : "no"),
                                ans))
                    )
                );
                levelNode.Add(newQuestion);
                doc.Save(xmlPath);
            }
        }

        private static XDocument CreateDefaultXML()
        {
            XDocument doc = new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                new XElement("quiz",
                    new XElement("topic", new XAttribute("name", "Города и памятники России"),
                        CreateLevelElement(1, "Легкий (Известные)"),
                        CreateLevelElement(2, "Средний (Региональные)"),
                        CreateLevelElement(3, "Сложный (Малоизвестные)")
                    )
                )
            );
            doc.Save(xmlPath);
            return doc;
        }

        private static XElement CreateLevelElement(int id, string name)
        {
            return new XElement("level",
                new XAttribute("id", id),
                new XAttribute("name", name)
            );
        }

        public static bool IsLevelUnlocked(int levelId)
        {
            if (levelId == 1) return true;

            string settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "user_settings.xml");
            if (!File.Exists(settingsPath))
                return false;

            XDocument doc = XDocument.Load(settingsPath);
            var unlocked = doc.Root?.Element("unlocked_levels")?.Element("level_" + (levelId - 1));
            return unlocked?.Value == "true";
        }

        public static void UnlockNextLevel(int levelId)
        {
            string settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "user_settings.xml");
            XDocument doc;

            if (File.Exists(settingsPath))
                doc = XDocument.Load(settingsPath);
            else
            {
                doc = new XDocument(new XElement("settings", new XElement("unlocked_levels")));
            }

            var unlockedNode = doc.Root.Element("unlocked_levels");
            if (unlockedNode == null)
            {
                unlockedNode = new XElement("unlocked_levels");
                doc.Root.Add(unlockedNode);
            }

            var levelElem = unlockedNode.Element("level_" + levelId);
            if (levelElem == null)
                unlockedNode.Add(new XElement("level_" + levelId, "true"));
            else
                levelElem.Value = "true";

            doc.Save(settingsPath);
        }
    }
}