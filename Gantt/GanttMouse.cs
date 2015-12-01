using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrmExpert.ExpressApp.GanttView
{
    using TaskT = TimeInterval;
    using UIDT = Int32;
    using HashT = Int32;
    using LineT = Int32;

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

    public partial class Bicycle
    {
        mouse_state mouse_enter_state, mouse_current_state;

        private void MouseMove(Point2d m)
        {
            var mouse_prev_state = mouse_current_state;
            mouse_current_state = new mouse_state(m);

            mouse_current_state.hovered = RayTrace(m);

            if (mouse_enter_state == null)
                mouse_current_state.UpdateCursor(this);

            if (mouse_current_state.hovered != mouse_prev_state.hovered)
                this.Invalidate();

            if (mouse_enter_state != null)
                this.Invalidate();
        }

        private void OnMouseHold()
        {
            mouse_enter_state = mouse_current_state;

            if (mouse_enter_state.point_mode == active_zone.MOVE_ZONE)
                Cursor = Cursors.NoMoveHoriz;
        }

        private void OnMouseUnhold()
        {
            if (mouse_enter_state == null)
                return;

            MouseAction();

            mouse_enter_state = null;
        }

        private void EnterStateAnimation()
        {
            if (mouse_enter_state == null)
                return;

            if (mouse_enter_state.point_mode == active_zone.NONE)
                return;

            if (mouse_enter_state.point_mode == active_zone.RESIZE_ZONE)
                ResizeAnimation();

            if (mouse_enter_state.point_mode == active_zone.MOVE_ZONE)
                MoveAnimation();
        }

        private Tuple<LineT, TaskT> MouseActiveTask()
        {
            var line = RayTrace(mouse_enter_state.coords);

            return new Tuple<LineT, TaskT>(line, tasks[line]);
        }

        private LineT Resize(out TaskT resized)
        {
            var active = MouseActiveTask();
            var origin_start = active.Item2.start;
            var new_end = XCoordinateToDateTime((int)mouse_current_state.coords.x);

            if (new_end < origin_start)
                new_end = origin_start;

            resized = new TaskT(origin_start, new_end);

            return active.Item1;
        }

        private LineT Move(out TaskT moved)
        {
            var drift = (mouse_current_state.coords.x - mouse_enter_state.coords.x) / hour_to_pixel_ratio;
            var active_task = MouseActiveTask();
            moved = new TaskT();

            moved.start = active_task.Item2.start;
            moved.end = active_task.Item2.end;

            try
            {
                moved.start = active_task.Item2.start.AddHours(drift); 
            }
            catch (System.ArgumentOutOfRangeException)
            {
                if (drift < 0)
                    moved.start = DateTime.MinValue;
                else
                    moved.start = DateTime.MaxValue;
            }

            try
            {
                moved.end = active_task.Item2.end.AddHours(drift);
            }
            catch (System.ArgumentOutOfRangeException)
            {
                if (drift < 0)
                    moved.end = DateTime.MinValue;
                else
                    moved.end = DateTime.MaxValue;
            }

            return active_task.Item1;
        }

        private void ResizeAnimation()
        {
            TaskT updated;
            var line = Resize(out updated);

            g.FillRectangle(new SolidBrush(BackColor), 0, PosToYCoordinate(line), Width, HRow);
            DrawRect(line, updated);
        }

        private void MoveAnimation()
        {
            TaskT updated;
            var line = Move(out updated);

            g.FillRectangle(new SolidBrush(BackColor), 0, PosToYCoordinate(line), Width, HRow);
            DrawRect(line, updated);
        }

        private void MouseAction()
        {
            if (mouse_enter_state.point_mode == active_zone.NONE)
                return;

            TaskT update;

            if (mouse_enter_state.point_mode == active_zone.RESIZE_ZONE)
            {
                var line = Resize(out update);
                var uid = PosToUID(line);
                OnResizeComplete(uid, update.end);
            }

            if (mouse_enter_state.point_mode == active_zone.MOVE_ZONE)
            {
                var line = Move(out update);
                var uid = PosToUID(line);
                OnMoveComplete(uid, update.start);
            }

            this.Invalidate();
        }

        private LineT RayTrace(Point2d m)
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
