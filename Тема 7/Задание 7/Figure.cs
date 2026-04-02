using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization.Formatters.Binary;

namespace Задание_7
{
    /// <summary>
    /// Базовый класс для всех фигур
    /// </summary>
    [Serializable]
    public abstract class Figure
    {
        protected int x, y;
        protected int width, height;
        protected Stroke stroke;
        protected bool isSelected;
        protected float rotationAngle;
        protected bool mirrorHorizontal;
        protected bool mirrorVertical;

        public Figure()
        {
            x = 0; y = 0; width = 100; height = 100;
            stroke = new Stroke();
            isSelected = false;
            rotationAngle = 0;
            mirrorHorizontal = false;
            mirrorVertical = false;
        }

        public Figure(int x, int y, int width, int height)
        {
            this.x = x; this.y = y; this.width = width; this.height = height;
            stroke = new Stroke();
            isSelected = false;
            rotationAngle = 0;
            mirrorHorizontal = false;
            mirrorVertical = false;
        }

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public int Width { get { return width; } set { width = value; } }
        public int Height { get { return height; } set { height = value; } }
        public Stroke Stroke { get { return stroke; } set { stroke = value; } }
        public bool IsSelected { get { return isSelected; } set { isSelected = value; } }
        public float RotationAngle { get { return rotationAngle; } set { rotationAngle = value; } }
        public bool MirrorHorizontal { get { return mirrorHorizontal; } set { mirrorHorizontal = value; } }
        public bool MirrorVertical { get { return mirrorVertical; } set { mirrorVertical = value; } }

        public Rectangle GetBounds() => new Rectangle(x, y, width, height);

        public virtual void Draw(Graphics g)
        {
            DrawFigure(g);
            if (isSelected)
                DrawSelectionBorder(g);
        }

        protected abstract void DrawFigure(Graphics g);

        protected virtual void DrawSelectionBorder(Graphics g)
        {
            Matrix original = g.Transform;
            float centerX = x + width / 2f;
            float centerY = y + height / 2f;

            g.TranslateTransform(centerX, centerY);
            g.RotateTransform(rotationAngle);

            if (mirrorHorizontal || mirrorVertical)
            {
                g.ScaleTransform(mirrorHorizontal ? -1 : 1, mirrorVertical ? -1 : 1);
            }

            g.TranslateTransform(-width / 2f, -height / 2f, MatrixOrder.Append);

            Pen selectionPen = new Pen(Color.Blue, 1) { DashStyle = DashStyle.Dash };
            g.DrawRectangle(selectionPen, 0, 0, width, height);

            int m = 6;
            Brush markerBrush = new SolidBrush(Color.Blue);

            g.FillRectangle(markerBrush, -m / 2f, -m / 2f, m, m);
            g.FillRectangle(markerBrush, width - m / 2f, -m / 2f, m, m);
            g.FillRectangle(markerBrush, -m / 2f, height - m / 2f, m, m);
            g.FillRectangle(markerBrush, width - m / 2f, height - m / 2f, m, m);
            g.FillRectangle(markerBrush, width / 2f - m / 2f, -m / 2f, m, m);
            g.FillRectangle(markerBrush, width / 2f - m / 2f, height - m / 2f, m, m);
            g.FillRectangle(markerBrush, -m / 2f, height / 2f - m / 2f, m, m);
            g.FillRectangle(markerBrush, width - m / 2f, height / 2f - m / 2f, m, m);

            g.Transform = original;
        }

        public virtual void Move(int dx, int dy) { x += dx; y += dy; }
        public virtual void MoveTo(int newX, int newY) { x = newX; y = newY; }

        public virtual bool Contains(Point point)
        {
            if (rotationAngle == 0 && !mirrorHorizontal && !mirrorVertical)
            {
                return new Rectangle(x, y, width, height).Contains(point);
            }

            Matrix transform = new Matrix();
            transform.Translate(x + width / 2f, y + height / 2f);
            transform.Rotate(rotationAngle);
            if (mirrorHorizontal || mirrorVertical)
            {
                transform.Scale(mirrorHorizontal ? -1 : 1, mirrorVertical ? -1 : 1);
            }
            transform.Translate(-width / 2f, -height / 2f, MatrixOrder.Append);

            PointF[] pts = new PointF[] { new PointF(point.X, point.Y) };
            transform.Invert();
            transform.TransformPoints(pts);

            Rectangle rect = new Rectangle(0, 0, width, height);
            return rect.Contains((int)pts[0].X, (int)pts[0].Y);
        }

        public virtual Figure Clone()
        {
            using (var ms = new System.IO.MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, this);
                ms.Position = 0;
                return (Figure)formatter.Deserialize(ms);
            }
        }

        public virtual void Rotate(float angle) => rotationAngle += angle;
        public virtual void Mirror(bool horizontal, bool vertical)
        {
            if (horizontal)
                mirrorHorizontal = !mirrorHorizontal;  // Переключаем
            if (vertical)
                mirrorVertical = !mirrorVertical;      // Переключаем
        }

        public abstract string GetTypeName();
    }
}