using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmExpert.ExpressApp.GanttView
{
    class Vector4d
    {
        public Point2d p1, p2;

        public Vector4d()
        {
            p1 = new Point2d();
            p2 = new Point2d();
        }

        public static implicit operator Rectangle(Vector4d p)
        {
            return new Rectangle((int)p.p1.x, (int)p.p1.y, (int)p.p2.x, (int)p.p2.y);
        }
    }
}
