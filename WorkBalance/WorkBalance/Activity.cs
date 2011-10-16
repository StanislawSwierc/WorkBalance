using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkBalance
{
    public class Activity
    {
        public string Name { get; set; }
        public string[] Tags {get; set; }
        public int ExpectedTime { get; set; }
        public int ActualTime { get; set; }
        public bool Completed { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
