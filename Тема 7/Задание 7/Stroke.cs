using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Задание_7
{
    /// <summary>
    /// Описание класса хранения свойств для рисования контура фигуры
    /// </summary>
    [Serializable]
    public class Stroke
    {
        /// <summary>
        /// Конструктор без параметров, со свойствами по умолчанию
        /// </summary>
        public Stroke()
        {
            Color = Color.Black;
            Width = 1f;
            DashStyle = DashStyle.Solid;
            FillColor = Color.Transparent;
            IsFilled = false;
        }

        /// <summary>
        /// Цвет линии фигуры
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Ширина линии фигуры
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Стиль линии фигуры
        /// </summary>
        public DashStyle DashStyle { get; set; }

        /// <summary>
        /// Цвет заливки фигуры
        /// </summary>
        public Color FillColor { get; set; }

        /// <summary>
        /// Флаг заливки фигуры
        /// </summary>
        public bool IsFilled { get; set; }

        /// <summary>
        /// Свойство возвращает "карандаш", настроенный по текущим свойствам 
        /// </summary>
        public Pen UpdatePen(Pen pen)
        {
            if (pen == null)
                throw new ArgumentNullException();
            pen.Color = Color;
            pen.Width = Width;
            pen.DashStyle = DashStyle;
            return pen;
        }

        /// <summary>
        /// Свойство возвращает "кисть" для заливки
        /// </summary>
        public Brush GetFillBrush()
        {
            if (IsFilled)
                return new SolidBrush(FillColor);
            return Brushes.Transparent;
        }
    }
}