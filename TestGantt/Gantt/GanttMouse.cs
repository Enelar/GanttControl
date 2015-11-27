using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGantt
{
    public partial class Gantt
    {
        int hovered = -1;

        private void MouseMove(Point2d m)
        {
            var hover = RayTrace(m);

            Rectangle rect = new Rectangle((int)m.x, (int)m.y, 1, 1);
            g.DrawRectangle(current_pen, rect);

            if (hovered == hover)
                return;

            hovered = hover;
            this.Refresh();
        }

        private int RayTrace(Point2d m)
        {
            var line = YCoordinateToPos((int)m.y);

            if (tasks.Count() <= line)
                return -1;

            var time_interval = tasks[line];

            if (DateTimeToXCoordinate(time_interval.start) > m.x)
                return -1;
            if (DateTimeToXCoordinate(time_interval.end) < m.x)
                return -1;

            return line;
        }
    }
}
