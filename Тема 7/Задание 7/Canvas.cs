using System.Windows.Forms;

namespace Задание_7
{
    /// <summary>
    /// PictureBox с включённой двойной буферизацией для устранения мерцания
    /// </summary>
    public class Canvas : PictureBox
    {
        public Canvas()
        {
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.UserPaint, true);
        }
    }
}