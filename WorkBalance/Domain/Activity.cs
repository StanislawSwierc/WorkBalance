using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkBalance.Aspects;

namespace WorkBalance.Domain
{
    [NotifyPropertyChanged]
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
        /// Gets or sets the value indicating wheather the Activity is archived
        /// </summary>
        public bool Archived { get; set; }

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

        /// <summary>
        /// Gets of sets the collection of sprints
        /// </summary>
        public ICollection<Sprint> Sprints { get; set; }

        public Activity()
        {
            Tags = new List<ActivityTag>();
            Sprints = new List<Sprint>();
        }
    }
}
