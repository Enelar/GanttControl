using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestGantt
{
    enum active_zone
    {
        NONE,
        MOVE_ZONE,
        RESIZE_ZONE,
    };

    public partial class Gantt
    {
        int hovered = -1;
        active_zone point_mode = active_zone.NONE;

        private void MouseMove(Point2d m)
        {
            var hover = RayTrace(m);

            switch (point_mode)
            {
                case active_zone.RESIZE_ZONE:
                    this.Cursor = Cursors.PanEast;
                    break;
                default:
                    this.Cursor = Cursors.Arrow;
                    break;
            }

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

            point_mode = active_zone.NONE;

            if (tasks.Count() <= line)
                return -1;

            var time_interval = tasks[line];

            if (DateTimeToXCoordinate(time_interval.start) > m.x)
                return -1;

            var end_coord = DateTimeToXCoordinate(time_interval.end);
            if (end_coord < m.x)
                return -1;

            if (end_coord - m.x < 5)
                point_mode = active_zone.RESIZE_ZONE;
            else
                point_mode = active_zone.MOVE_ZONE;

            return line;
        }
    }
}
