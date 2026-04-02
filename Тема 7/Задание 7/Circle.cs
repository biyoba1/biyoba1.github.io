using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Задание_7
{
    [Serializable]
    public class Circle : Figure
    {
        public Circle() : base() { width = height = 100; }
        public Circle(int x, int y, int diameter) : base(x, y, diameter, diameter) { }

        protected override void DrawFigure(Graphics g)
        {
            Pen pen = new Pen(stroke.Color, stroke.Width) { DashStyle = stroke.DashStyle };
            ApplyTransform(g, () =>
            {
                int diameter = Math.Min(width, height);
                Rectangle rect = new Rectangle(0, 0, diameter, diameter);
                g.DrawEllipse(pen, rect);

                if (stroke.IsFilled)
                {
                    using (Brush brush = new SolidBrush(stroke.FillColor))
                    {
                        g.FillEllipse(brush, rect);
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

        public override string GetTypeName() => "Круг";
    }
}