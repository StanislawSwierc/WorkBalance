using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkBalance.Domain
{
    public class Activity
    {
        public string Name { get; set; }
        public int ExpectedEffort { get; set; }
        public int ActualEffort { get; set; }
        public bool Completed { get; set; }
        public DateTime CreationTime { get; set; }
        public ICollection<ActivityTag> Tags { get; set; }
    }
}
