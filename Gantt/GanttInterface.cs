using CrmExpert.ExpressApp.GanttView.Gantt.Bind;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmExpert.ExpressApp.GanttView
{
    /* Do same changes to each file */
    using TaskT = task;
    using UIDT = String;
    using HashT = Int32;
    using LineT = Int32;

    public partial class Bicycle
    {
        private BindingList<TaskT> tasks;
        private Dictionary<HashT, Tuple<UIDT, UIDT>> links;
        private Dictionary<UIDT, LineT> positions;

        public void BindToCollections()
        {
        }

        public void UpdateTask(UIDT uid, LineT pos, DateTime begin, DateTime end)
        {
            if (tasks.Count() < pos)
                throw new Exception("Please add tasks to gantt continiously");
            if (tasks.Count() == pos)
                tasks.Add(new TaskT());

            tasks[pos] = new TaskT(begin, end);
           
            positions[uid] = pos;
        }

        public void UpdateLink( UIDT uid1, UIDT uid2, bool active )
        {
            var hash = (uid1.ToString() + "delimeter" +  uid2.ToString()).GetHashCode();

            if (active)
            {
                links[hash] = new Tuple<UIDT, UIDT>(uid1, uid2);
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
            var to_remove = new List<HashT>();

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

        private int UIDToPos(UIDT uid)
        {
            return positions[uid];
        }

        private UIDT PosToUID(LineT pos)
        {
            return positions.First(entry => entry.Value == pos).Key;
        }

        protected void OnMoveComplete(UIDT uid, DateTime new_start_date)
        {
        }

        protected void OnResizeComplete(UIDT uid, DateTime new_end_date)
        {
        }
    }
}
