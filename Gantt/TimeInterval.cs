using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmExpert.ExpressApp.GanttView
{
    public struct TimeInterval
    {
        public DateTime start, end;

        public TimeInterval(DateTime s, DateTime e)
        {
            start = s;
            end = e;
        }

        public TimeInterval CropBy(TimeInterval crop)
        {
            var ret = new TimeInterval();

            // Max between
            ret.start = start > crop.start ? start : crop.start;
            // Min between
            ret.end = end < crop.end ? end : crop.end;

            return ret;
        }

        public TimeSpan Duration()
        {
            return end - start;
        }
    };
}
