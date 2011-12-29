using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkBalance.Domain
{
    public class Sprint
    {
        public Activity Activity { get; private set; }
        public DateTime StartTime {get; set;}
        public DateTime EndTime { get; set; }
        public ICollection<InterruptionRecord> Interruptions { get; set; }

        public Sprint(Activity activity)
        {
            Activity = activity;
        }
    }
}
