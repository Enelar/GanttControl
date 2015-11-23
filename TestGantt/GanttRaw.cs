using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestGantt
{
    public partial class Gantt : UserControl
    {
        private Pen current_pen;
        private Graphics g;
        private Dictionary<int, Vector4d> rect, arrows;

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            g = this.CreateGraphics();
            current_pen = new Pen(Color.Black);
          //  current_pen = new SolidBrush(this.
        }

        protected override void OnPaint(PaintEventArgs e) 
        {
            base.OnPaint(e);
        }

        private void DrawRect(Vector4d p)
        {
            Graphics g = this.CreateGraphics();
            g.DrawRectangle(current_pen, p);
        }

        private void DrawArrow(Vector4d p)
        {

        }
    }
}
