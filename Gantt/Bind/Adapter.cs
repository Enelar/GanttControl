using CrmExpert.ExpressApp.GanttView.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrmExpert.ExpressApp.GanttView.Gantt.Bind
{
    struct IGanttAppointment
    {

    };

    class Adapter
    {
        BindingList<TimeInterval> BicycleTasks;
        Dictionary<int, int> Positions;
        BindingList<IGanttAppointment> OriginTasks;


        public Adapter(BindingList<TimeInterval> _BicycleTasks, Dictionary<int, int> _Positions)
        {
            Positions = _Positions;
            BicycleTasks = _BicycleTasks;
            BicycleTasks.ListChanged += new ListChangedEventHandler(SyncFromBicycleToOrigin);
        }

        public void BindSource(BindingList<IGanttAppointment> source)
        {
            OriginTasks = source;
            OriginTasks.ListChanged += new ListChangedEventHandler(SyncFromOriginToBicycle);
        }

        public TimeInterval BicycleTask(int uid)
        {
            int pos = Positions[uid];

            return BicycleTasks[pos];
        }

        void SyncFromBicycleToOrigin(object sender, ListChangedEventArgs e)
        {
            MessageBox.Show(e.ListChangedType.ToString());
        }

        void SyncFromOriginToBicycle(object sender, ListChangedEventArgs e)
        {
            MessageBox.Show(e.ListChangedType.ToString());
        }
    }
}
