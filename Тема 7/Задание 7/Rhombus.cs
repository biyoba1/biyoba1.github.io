using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Задание_7
{
    [Serializable]
    public class Rhombus : Figure
    {
        public Rhombus() : base() { }
        public Rhombus(int x, int y, int width, int height) : base(x, y, width, height) { }

        protected override void DrawFigure(Graphics g)
        {
            Pen pen = new Pen(stroke.Color, stroke.Width) { DashStyle = stroke.DashStyle };
            ApplyTransform(g, () =>
            {
                // Ромб - 4 точки: верх, право, низ, лево
                PointF[] points = new PointF[]
                {
                    new PointF(width / 2f, 0),           // Верх
                    new PointF(width, height / 2f),      // Право
                    new PointF(width / 2f, height),      // Низ
                    new PointF(0, height / 2f),          // Лево
                    new PointF(width / 2f, 0)            // Замыкаем
                };

                g.DrawPolygon(pen, points);

                if (stroke.IsFilled)
                {
                    using (Brush brush = new SolidBrush(stroke.FillColor))
                    {
                        g.FillPolygon(brush, points);
                    }
                }
            });
        }

        private void ApplyTransform(Graphics g, Action drawAction)
        {
            Matrix original = g.Transform;
            float centerX = x + width / 2f;
            float centerY = y + height / 2f;

            g.TranslateTransform(centerX, centerY);
            g.RotateTransform(rotationAngle);
            if (mirrorHorizontal || mirrorVertical)
                g.ScaleTransform(mirrorHorizontal ? -1 : 1, mirrorVertical ? -1 : 1);
            g.TranslateTransform(-width / 2f, -height / 2f, MatrixOrder.Append);
            drawAction();
            g.Transform = original;
        }

        public override string GetTypeName() => "Ромб";
    }
}