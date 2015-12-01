using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrmExpert.ExpressApp.GanttView
{
    class mouse_state
    {
        public active_zone point_mode = active_zone.NONE;
        public int hovered = -1;
        public Point2d coords;

        public mouse_state(Point2d m)
        {
            coords = m;
        }

        public void UpdateCursor(UserControl target)
        {
            switch (point_mode)
            {
                case active_zone.RESIZE_ZONE:
                    target.Cursor = Cursors.PanEast;
                    break;
                default:
                    target.Cursor = Cursors.Arrow;
                    break;
            }
        }
    }
}
