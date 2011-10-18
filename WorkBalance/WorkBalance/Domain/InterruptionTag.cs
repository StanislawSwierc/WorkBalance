using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkBalance.Domain
{
    public class InterruptionTag
    {
        public string Name { get; private set; }
        public InterruptionTag Parent { get; private set; }
    }
}
