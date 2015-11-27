using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGantt
{
    public struct task
    {
        public TimeInterval interval;
    };

    public partial class Gantt
    {
        private List<TimeInterval> tasks;
        private Dictionary<int, Tuple<int, int>> links;
        private Dictionary<int, int> positions;

        public void UpdateTask(int uid, int pos, DateTime begin, DateTime end)
        {
            if (tasks.Count() < pos)
                throw new Exception("Please add tasks to gantt continiously");
            if (tasks.Count() == pos)
                tasks.Add(new TimeInterval());

            tasks[pos] = new TimeInterval(begin, end);
           
            positions[uid] = pos;
        }

        public void UpdateLink( int uid1, int uid2, bool active )
        {
            var hash = (uid1.ToString() + "delimeter" +  uid2.ToString()).GetHashCode();

            if (active)
            {
                links[hash] = new Tuple<int, int>(uid1, uid2);
                return;
            }

            links.Remove(hash);
        }

        public void SetDrawInterval(DateTime begin, DateTime end)
        {
            draw_interval = new TimeInterval(begin, end);
        }

        private void DrawTasks()
        {           
            var pos = 0;

            foreach (var task in tasks)
            {
                DrawRect(pos, task);

                pos++;
            }
        }

        private void DrawLinks()
        {
            var to_remove = new List<int>();

            foreach (var tuple in links)
            {
                var link = tuple.Value;

                if (!positions.ContainsKey(link.Item1) || !positions.ContainsKey(link.Item2))
                {
                    to_remove.Add(tuple.Key);
                    continue; // skip link, target task unknown
                }

                var row1 = positions[link.Item1];
                var row2 = positions[link.Item2];

                var task1 = tasks[row1];
                var task2 = tasks[row2];

                DrawArrow(row1, task1.end, row2, task2.start);
            }
        }
    }
}
