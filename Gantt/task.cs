using CrmExpert.ExpressApp.GanttView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmExpert.ExpressApp.GanttView
{
    public enum task_type
    {
        REGULAR,
        GROUP,
        MILESTONE
    };

    public struct task
    {
        public TimeInterval interval;
        public task_type type;

        public task(DateTime start, DateTime end)
        {
            interval.start = start;
            interval.end = end;
            type = task_type.REGULAR;
        }

        public DateTime start
        {
            get
            {
                return interval.start;
            }
            set
            {
                interval.start = value;
            }
        }

        public DateTime end
        {
            get
            {
                return interval.end;
            }
            set
            {
                interval.end = value;
            }
        }
    };
}
