using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkBalance.Domain
{
    public class InterruptionRecord
    {
        public InterruptionRecord(Interruption interruption)
        {
            Interruption = interruption;
            Timestamp = DateTime.Now;
        }

        public Interruption Interruption { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
