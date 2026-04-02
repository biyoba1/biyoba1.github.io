using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Задание_6
{
    /// <summary>
    /// Класс, представляющий один вопрос викторины
    /// </summary>
    public class Question
    {
        public string ImagePath { get; set; }      // Путь к изображению
        public string Hint { get; set; }           // Текст подсказки
        public string Text { get; set; }           // Текст вопроса
        public List<string> Answers { get; set; }  // Список вариантов ответов (4 шт)
        public int CorrectIndex { get; set; }      // Индекс правильного ответа (0-3)

        public Question()
        {
            Answers = new List<string>();
        }
    }
}