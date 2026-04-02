using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Задание_7
{
    public partial class Form1 : Form
    {
        private List<Figure> figures;
        private Figure selectedFigure;
        private StackMemory undoStack, redoStack;
        private bool isDrawing;
        private bool isDragging;
        private Point startPoint;
        private Point dragOffset;
        private FigureType currentFigureType;
        private ColorDialog colorDialog;
        private const int STACK_DEPTH = 20;
        private Figure clipboardFigure;

        public Form1()
        {
            InitializeComponent();
            InitializeEditor();
        }

        private void InitializeEditor()
        {
            figures = new List<Figure>();
            undoStack = new StackMemory(STACK_DEPTH);
            redoStack = new StackMemory(STACK_DEPTH);
            isDrawing = false;
            isDragging = false;
            currentFigureType = FigureType.Rectangle;
            colorDialog = new ColorDialog { Color = Color.Black };
            SaveState();
        }

        private void SaveState()
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    new BinaryFormatter().Serialize(ms, figures);
                    undoStack.Push(ms);
                    redoStack.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения: " + ex.Message);
            }
        }

        private void Undo()
        {
            if (undoStack.Count <= 0) return;
            using (MemoryStream ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, figures);
                redoStack.Push(ms);
            }
            using (MemoryStream ms = new MemoryStream())
            {
                undoStack.Pop(ms);
                ms.Position = 0;
                figures = (List<Figure>)new BinaryFormatter().Deserialize(ms);
            }
            canvas.Invalidate();
        }

        private void Redo()
        {
            if (redoStack.Count <= 0) return;
            using (MemoryStream ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, figures);
                undoStack.Push(ms);
            }
            using (MemoryStream ms = new MemoryStream())
            {
                redoStack.Pop(ms);
                ms.Position = 0;
                figures = (List<Figure>)new BinaryFormatter().Deserialize(ms);
            }
            canvas.Invalidate();
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(Color.White);

            if (figures != null)
            {
                foreach (var f in figures)
                {
                    if (f != null)
                        f.Draw(e.Graphics);
                }
            }
        }

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            if (currentFigureType == FigureType.Select)
            {
                Figure clickedFigure = null;
                for (int i = figures.Count - 1; i >= 0; i--)
                {
                    if (figures[i] != null && figures[i].Contains(e.Location))
                    {
                        clickedFigure = figures[i];
                        break;
                    }
                }

                if (clickedFigure != null)
                {
                    if (selectedFigure != null)
                        selectedFigure.IsSelected = false;

                    selectedFigure = clickedFigure;
                    selectedFigure.IsSelected = true;

                    isDragging = true;
                    dragOffset = new Point(e.Location.X - selectedFigure.X, e.Location.Y - selectedFigure.Y);
                    canvas.Invalidate();
                }
                else
                {
                    if (selectedFigure != null)
                    {
                        selectedFigure.IsSelected = false;
                        selectedFigure = null;
                        canvas.Invalidate();
                    }
                }
            }
            else
            {
                isDrawing = true;
                startPoint = e.Location;
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedFigure != null && e.Button == MouseButtons.Left)
            {
                int newX = e.Location.X - dragOffset.X;
                int newY = e.Location.Y - dragOffset.Y;
                selectedFigure.MoveTo(newX, newY);
                canvas.Invalidate();
            }
            else if (isDrawing && e.Button == MouseButtons.Left)
            {
                canvas.Invalidate();
            }
        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;
                SaveState();
            }

            if (isDrawing && e.Button == MouseButtons.Left)
            {
                isDrawing = false;
                CreateFigure(startPoint, e.Location);
                SaveState();
                canvas.Invalidate();
            }
        }

        private void CreateFigure(Point start, Point end)
        {
            int x = Math.Min(start.X, end.X);
            int y = Math.Min(start.Y, end.Y);
            int w = Math.Max(5, Math.Abs(end.X - start.X));
            int h = Math.Max(5, Math.Abs(end.Y - start.Y));

            Figure f = null;
            switch (currentFigureType)
            {
                case FigureType.Circle:
                    f = new Circle(x, y, Math.Min(w, h));
                    break;
                case FigureType.Ellipse:
                    f = new Ellipse(x, y, w, h);
                    break;
                case FigureType.Square:
                    f = new Square(x, y, Math.Min(w, h));
                    break;
                case FigureType.Rectangle:
                    f = new RectangleFigure(x, y, w, h);
                    break;
                case FigureType.Rhombus:
                    f = new Rhombus(x, y, w, h);
                    break;
            }

            if (f != null)
            {
                figures.Add(f);
                selectedFigure = f;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (selectedFigure != null)
            {
                int step = e.Shift ? 1 : 5;
                switch (e.KeyCode)
                {
                    case Keys.Up: selectedFigure.Move(0, -step); break;
                    case Keys.Down: selectedFigure.Move(0, step); break;
                    case Keys.Left: selectedFigure.Move(-step, 0); break;
                    case Keys.Right: selectedFigure.Move(step, 0); break;
                    case Keys.Delete:
                        figures.Remove(selectedFigure);
                        selectedFigure = null;
                        SaveState();
                        break;
                }
                canvas.Invalidate();
            }
            if (e.Control)
            {
                if (e.KeyCode == Keys.Z) Undo();
                else if (e.KeyCode == Keys.Y) Redo();
                else if (e.KeyCode == Keys.C) CopyFigure();
                else if (e.KeyCode == Keys.X) CutFigure();
                else if (e.KeyCode == Keys.V) PasteFigure();
            }
        }

        private void CopyFigure()
        {
            if (selectedFigure != null)
                clipboardFigure = selectedFigure.Clone();
        }

        private void CutFigure()
        {
            if (selectedFigure != null)
            {
                clipboardFigure = selectedFigure.Clone();
                figures.Remove(selectedFigure);
                selectedFigure = null;
                SaveState();
                canvas.Invalidate();
            }
        }

        private void PasteFigure()
        {
            if (clipboardFigure != null)
            {
                var f = clipboardFigure.Clone();
                f.Move(20, 20);
                figures.Add(f);
                selectedFigure = f;
                SaveState();
                canvas.Invalidate();
            }
        }

        private void SetFigureType(FigureType type)
        {
            currentFigureType = type;
            UpdateToolbar();
        }

        private void UpdateToolbar()
        {
            circleButton.Checked = currentFigureType == FigureType.Circle;
            ellipseButton.Checked = currentFigureType == FigureType.Ellipse;
            squareButton.Checked = currentFigureType == FigureType.Square;
            rectangleButton.Checked = currentFigureType == FigureType.Rectangle;
            rhombusButton.Checked = currentFigureType == FigureType.Rhombus;
            selectButton.Checked = currentFigureType == FigureType.Select;
        }

        private void ChangeThickness(float t)
        {
            if (selectedFigure != null)
            {
                selectedFigure.Stroke.Width = t;
                SaveState();
                canvas.Invalidate();
            }
            else
                MessageBox.Show("Выберите фигуру");
        }

        // === Menu handlers ===
        private void circleToolStripMenuItem_Click(object sender, EventArgs e) { SetFigureType(FigureType.Circle); }
        private void ellipseToolStripMenuItem_Click(object sender, EventArgs e) { SetFigureType(FigureType.Ellipse); }
        private void squareToolStripMenuItem_Click(object sender, EventArgs e) { SetFigureType(FigureType.Square); }
        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e) { SetFigureType(FigureType.Rectangle); }
        private void rhombusToolStripMenuItem_Click(object sender, EventArgs e) { SetFigureType(FigureType.Rhombus); }
        private void selectToolStripMenuItem_Click(object sender, EventArgs e) { SetFigureType(FigureType.Select); }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedFigure != null && colorDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFigure.Stroke.Color = colorDialog.Color;
                SaveState();
                canvas.Invalidate();
            }
            else
                MessageBox.Show("Выберите фигуру");
        }

        private void fillColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedFigure != null && colorDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFigure.Stroke.FillColor = colorDialog.Color;
                selectedFigure.Stroke.IsFilled = true;
                SaveState();
                canvas.Invalidate();
            }
            else
                MessageBox.Show("Выберите фигуру");
        }

        private void noFillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedFigure != null)
            {
                selectedFigure.Stroke.IsFilled = false;
                SaveState();
                canvas.Invalidate();
            }
            else
                MessageBox.Show("Выберите фигуру");
        }

        private void thickness1ToolStripMenuItem_Click(object sender, EventArgs e) { ChangeThickness(1); }
        private void thickness2ToolStripMenuItem_Click(object sender, EventArgs e) { ChangeThickness(2); }
        private void thickness3ToolStripMenuItem_Click(object sender, EventArgs e) { ChangeThickness(3); }
        private void thickness5ToolStripMenuItem_Click(object sender, EventArgs e) { ChangeThickness(5); }
        private void undoToolStripMenuItem_Click(object sender, EventArgs e) { Undo(); }
        private void redoToolStripMenuItem_Click(object sender, EventArgs e) { Redo(); }

        private void rotate90ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedFigure != null)
            {
                selectedFigure.Rotate(90);
                SaveState();
                canvas.Invalidate();
            }
            else
                MessageBox.Show("Выберите фигуру");
        }

        private void rotate180ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedFigure != null)
            {
                selectedFigure.Rotate(180);
                SaveState();
                canvas.Invalidate();
            }
            else
                MessageBox.Show("Выберите фигуру");
        }

        private void mirrorHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedFigure != null)
            {
                selectedFigure.Mirror(true, selectedFigure.MirrorVertical);
                SaveState();
                canvas.Invalidate();
            }
            else
                MessageBox.Show("Выберите фигуру");
        }

        private void mirrorVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedFigure != null)
            {
                selectedFigure.Mirror(selectedFigure.MirrorHorizontal, true);
                SaveState();
                canvas.Invalidate();
            }
            else
                MessageBox.Show("Выберите фигуру");
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog { Filter = "Vector files|*.vec", Title = "Сохранить" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var fs = new FileStream(dlg.FileName, FileMode.Create))
                        new BinaryFormatter().Serialize(fs, figures);
                    MessageBox.Show("Сохранено");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "Vector files|*.vec", Title = "Загрузить" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var fs = new FileStream(dlg.FileName, FileMode.Open))
                    {
                        figures = (List<Figure>)new BinaryFormatter().Deserialize(fs);
                        selectedFigure = null;
                        SaveState();
                        canvas.Invalidate();
                    }
                    MessageBox.Show("Загружено");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Создать новый?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                figures.Clear();
                selectedFigure = null;
                undoStack.Clear();
                redoStack.Clear();
                SaveState();
                canvas.Invalidate();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) { Application.Exit(); }

        // === Toolbar buttons ===
        private void circleButton_Click(object sender, EventArgs e) { circleToolStripMenuItem_Click(sender, e); }
        private void ellipseButton_Click(object sender, EventArgs e) { ellipseToolStripMenuItem_Click(sender, e); }
        private void squareButton_Click(object sender, EventArgs e) { squareToolStripMenuItem_Click(sender, e); }
        private void rectangleButton_Click(object sender, EventArgs e) { rectangleToolStripMenuItem_Click(sender, e); }
        private void rhombusButton_Click(object sender, EventArgs e) { rhombusToolStripMenuItem_Click(sender, e); }
        private void selectButton_Click(object sender, EventArgs e) { selectToolStripMenuItem_Click(sender, e); }
        private void undoButton_Click(object sender, EventArgs e) { Undo(); }
        private void redoButton_Click(object sender, EventArgs e) { Redo(); }
        private void colorButton_Click(object sender, EventArgs e) { colorToolStripMenuItem_Click(sender, e); }
        private void fillColorButton_Click(object sender, EventArgs e) { fillColorToolStripMenuItem_Click(sender, e); }
        private void rotateButton_Click(object sender, EventArgs e) { rotate90ToolStripMenuItem_Click(sender, e); }
        private void mirrorButton_Click(object sender, EventArgs e) { mirrorHorizontalToolStripMenuItem_Click(sender, e); }
        private void copyButton_Click(object sender, EventArgs e) { CopyFigure(); }
        private void cutButton_Click(object sender, EventArgs e) { CutFigure(); }
        private void pasteButton_Click(object sender, EventArgs e) { PasteFigure(); }
        private void saveButton_Click(object sender, EventArgs e) { saveToolStripMenuItem_Click(sender, e); }
        private void loadButton_Click(object sender, EventArgs e) { loadToolStripMenuItem_Click(sender, e); }
        private void newButton_Click(object sender, EventArgs e) { newToolStripMenuItem_Click(sender, e); }

        // === Menu handlers for copy/cut/paste ===
        private void copyToolStripMenuItem_Click(object sender, EventArgs e) { CopyFigure(); }
        private void cutToolStripMenuItem_Click(object sender, EventArgs e) { CutFigure(); }
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e) { PasteFigure(); }
    }

    public enum FigureType { Select, Circle, Ellipse, Square, Rectangle, Rhombus }
}