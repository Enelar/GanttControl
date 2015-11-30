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
        LINK_ZONE,
        RESIZE_ZONE,
    };

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

    public partial class Gantt
    {
        mouse_state mouse_enter_state, mouse_current_state;

        private void MouseMove(Point2d m)
        {
            var mouse_prev_state = mouse_current_state;
            mouse_current_state = new mouse_state(m);

            mouse_current_state.hovered = RayTrace(m);
            mouse_current_state.UpdateCursor(this);

            if (mouse_current_state.hovered != mouse_prev_state.hovered)
                this.Invalidate();

            if (mouse_enter_state != null)
                EnterStateAction();
        }

        private void OnMouseHold()
        {
            mouse_enter_state = mouse_current_state;
        }

        private void OnMouseUnhold()
        {
            if (mouse_enter_state == null)
                return;

            MouseAction();

            mouse_enter_state = null;
        }

        private void EnterStateAction()
        {
            if (mouse_enter_state.point_mode == active_zone.NONE)
                return;


        }

        private void MouseAction()
        {
            if (mouse_enter_state.point_mode == active_zone.NONE)
                return;
        }

        private int RayTrace(Point2d m)
        {
            var line = YCoordinateToPos((int)m.y);

            if (tasks.Count() <= line)
                return -1;

            var time_interval = tasks[line];

            if (DateTimeToXCoordinate(time_interval.start) > m.x)
                return -1;

            var end_coord = DateTimeToXCoordinate(time_interval.end);
            if (end_coord < m.x)
                return -1;

            if (end_coord - m.x < 5)
                mouse_current_state.point_mode = active_zone.RESIZE_ZONE;
            else
                mouse_current_state.point_mode = active_zone.MOVE_ZONE;

            return line;
        }
    }
}
