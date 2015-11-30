using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace TestGantt
{
    public partial class Gantt
    {
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
        }

        protected override void OnPaint(PaintEventArgs e) 
        {
            base.OnPaint(e);
            Draw();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (g == null)
                InitDrawGantt();
            else
                Resize();

            this.Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            MouseMove(new Point2d(e.X, e.Y));
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            OnMouseHold();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            OnMouseUnhold();
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
        }
    }
}
