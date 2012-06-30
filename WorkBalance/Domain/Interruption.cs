using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkBalance.Domain
{
    public class Interruption : Entity
    {
        public string Name { get; private set; }
        public InterruptionType Type { get; private set; }
        public ICollection<InterruptionTag> Tags { get; private set; }

        public Interruption(string name, InterruptionType type)
        {
            Name = name;
            Type = type;
            Tags = new List<InterruptionTag>();
        }

        /// <summary>
        /// Gets or sets the value indicating wheather the Interruption is archived
        /// </summary>
        public bool Archived { get; set; }
    }
}
