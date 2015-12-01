using CrmExpert.ExpressApp.GanttView.Gantt.Bind;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmExpert.ExpressApp.GanttView
{
    public struct task
    {
        public TimeInterval interval;
    };

    public partial class Bicycle
    {
        private BindingList<TimeInterval> tasks;
        private Dictionary<int, Tuple<int, int>> links;
        private Dictionary<int, int> positions;
        private Adapter tasks_adapter;

        public void BindToCollections()
        {
            tasks_adapter = new Adapter(tasks, positions);
        }

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

        private int UIDToPos(int uid)
        {
            return positions[uid];
        }

        private int PosToUID(int pos)
        {
            return positions.First(entry => entry.Value == pos).Key;
        }

        protected void OnMoveComplete(int uid, DateTime new_start_date)
        {
            int pos = positions[uid];
            var task = tasks[pos];
            
            var diff = task.end - task.start;
            task.start = new_start_date;
            task.end = task.start + diff;

            tasks[pos] = task;
        }

        protected void OnResizeComplete(int uid, DateTime new_end_date)
        {
            var task = tasks_adapter.BicycleTask(uid);
            task.end = new_end_date;
        }
    }
}
