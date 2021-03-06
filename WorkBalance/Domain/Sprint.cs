﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkBalance.Domain
{
    public class Sprint : Entity
    {
        public Activity Activity { get; set; }
        public DateTime StartTime {get; set;}
        public DateTime EndTime { get; set; }
        public ICollection<InterruptionRecord> Interruptions { get; set; }

        /// <summary>
        /// Gets the duration of the sprint
        /// </summary>
        public TimeSpan Duration { get { return EndTime - StartTime; } }

        public Sprint()
        {
            StartTime = DateTime.Now;
            EndTime = DateTime.Now;
            Interruptions = new List<InterruptionRecord>();
        }
    }
}
