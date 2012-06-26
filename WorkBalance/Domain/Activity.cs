using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace WorkBalance.Domain
{
    public class Activity : Entity
    {
        /// <summary>
        /// The name of the Activity
        /// </summary>
        private string _name;
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value, "Name"); }
        }

        /// <summary>
        /// Expected effort measured in sprints
        /// </summary>
        private int _expectedEffort;
        public int ExpectedEffort
        {
            get { return _expectedEffort; }
            set { Set(ref _expectedEffort, value, "ExpectedEffort"); }
        }

        /// <summary>
        /// Actual effort measured in sprints
        /// </summary>
        private int _actualEffort;

        public int ActualEffort
        {
            get { return _actualEffort; }
            set { Set(ref _actualEffort, value, "ActualEffort"); }
        }

        /// <summary>
        /// Gets or sets the value indicating wheather the Activity is archived
        /// </summary>
        private bool _archived;
        public bool Archived
        {
            get { return _archived; }
            set { Set(ref _archived, value, "Archived"); }
        }

        /// <summary>
        /// Gets or sets the value indicating wheather the Activity is completed
        /// </summary>
        private bool _completed;
        public bool Completed
        {
            get { return _completed; }
            set { Set(ref _completed, value, "Completed"); }
        }

        /// <summary>
        /// Gets or sets the time when the Activity was created
        /// </summary>
        private DateTime _creationTime;
        public DateTime CreationTime
        {
            get { return _creationTime; }
            set { Set(ref _creationTime, value, "CreationTime"); }
        }

        /// <summary>
        /// Gets or sets the collection of associated tags
        /// </summary>
        private ICollection<ActivityTag> _tags;
        public ICollection<ActivityTag> Tags
        {
            get { return _tags; }
            set { Set(ref _tags, value, "Tags"); }
        }

        /// <summary>
        /// Gets of sets the collection of sprints
        /// </summary>
        private ICollection<Sprint> _sprints;
        public ICollection<Sprint> Sprints
        {
            get { return _sprints; }
            set { Set(ref _sprints, value, "Sprints"); }
        }

        public Activity()
        {
            Tags = new List<ActivityTag>();
            Sprints = new List<Sprint>();
            CreationTime = DateTime.Now;
        }
    }
}
