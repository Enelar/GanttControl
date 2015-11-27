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
        private Pen current_pen;
        private Brush current_brush;
        private Graphics g;

        public int HBox = 10;
        public int HRow = 10 + 4 * 2;
        private TimeInterval draw_interval;
        double hour_to_pixel_ratio;

        private void InitDrawGantt()
        {
            g = this.CreateGraphics();
            current_pen = new Pen(Color.Black);
            current_brush = new SolidBrush(Color.Blue);

            tasks = new List<TimeInterval>();
            positions = new Dictionary<int, int>();
            links = new Dictionary<int, Tuple<int, int>>();

            var zero = new DateTime();
            SetDrawInterval(zero.AddHours(0), zero.AddHours(10));
            UpdateTask(0, 0, zero.AddHours(1), zero.AddHours(2));
            UpdateTask(11, 1, zero.AddHours(3), zero.AddHours(4));
            UpdateTask(22, 2, zero.AddHours(5), zero.AddHours(8));
            UpdateTask(33, 3, zero.AddHours(0), zero.AddHours(2));

            UpdateLink(0, 11, true);
            UpdateLink(0, 22, true);
            UpdateLink(33, 22, true);
        }

        private int DateTimeToXCoordinate(DateTime time_point)
        {
            var diff = (time_point - draw_interval.start).TotalHours;
            return (int)(diff * hour_to_pixel_ratio);
        }

        private int PosToYCoordinate(int pos)
        {
            var row_baseline = pos * HRow;
            var centered_offset = (HRow - HBox) / 2;
            return row_baseline + centered_offset;
        }

        private int YCoordinateToPos(int y)
        {
            return y / HRow;
        }

        private void DrawRect(int row, TimeInterval interval)
        {
            var task_draw_interval = interval;

            var x1 = DateTimeToXCoordinate(task_draw_interval.start);
            var x2 = DateTimeToXCoordinate(task_draw_interval.end);

            var y = PosToYCoordinate(row);

            Rectangle rect = new Rectangle(x1, y, x2 - x1, HBox);

            var MicrosoftProjectRectColour = Color.FromArgb(255, 138, 187, 237);

            var brush = new SolidBrush(MicrosoftProjectRectColour);
            g.FillRectangle(brush, rect);

            if (hovered == row)
                g.DrawRectangle(current_pen, rect);
        }

        private void DrawArrow(int row_frow, DateTime date_time_from, int row_to, DateTime date_time_to)
        {
            var y1 = PosToYCoordinate(row_frow) + HBox / 2;
            var y2 = PosToYCoordinate(row_to) - 1;

            if (row_frow > row_to)
                y2 += HBox;
            if (row_frow == row_to)
                y2 = y1;

            var x1 = DateTimeToXCoordinate(date_time_from);
            var x2 = DateTimeToXCoordinate(date_time_to) + 1;

            var delta_x = x2 - x1;
            var delta_y = y2 - y1;

            var smooth_pix = 2;

            var path = new GraphicsPath();

            path.AddLine(x1, y1, x2 - smooth_pix, y1);

            path.AddLine(x2, y1 + Math.Sign(delta_y) * smooth_pix, x2, y2);

            g.DrawPath(current_pen, path);
        }
    }
}
