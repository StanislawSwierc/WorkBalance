using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkBalance.Domain
{
    public class Activity
    {
        /// <summary>
        /// The name of the Activity
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Expected effort measured in sprints
        /// </summary>
        public int ExpectedEffort { get; set; }

        /// <summary>
        /// Actual effort measured in sprints
        /// </summary>
        public int ActualEffort { get; set; }

        /// <summary>
        /// Gets or sets the value indicating wheather the Activity should 
        /// be listed in the ActivityInventory
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the value indicating wheather the Activity is completed
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// Gets or sets the time when the Activity was created
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the collection of associated tags
        /// </summary>
        public ICollection<ActivityTag> Tags { get; set; }
    }
}
