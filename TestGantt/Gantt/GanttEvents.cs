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

            InitDrawGantt();
        }

        protected override void OnPaint(PaintEventArgs e) 
        {
            base.OnPaint(e);
            Draw();
        }
    }
}
