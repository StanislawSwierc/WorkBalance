using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkBalance.Domain
{
    public class InterruptionTag : Entity
    {
        public string Name { get; private set; }
        public InterruptionTag Parent { get; private set; }

        /// <summary>
        /// Gets or sets the value indicating wheather the InterruptionTag is archived
        /// </summary>
        public bool Archived { get; set; }
    }
}
