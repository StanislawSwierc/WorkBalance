using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace WorkBalance.Domain
{
    public class Activity : ReactiveObject
    {
        /// <summary>
        /// The name of the Activity
        /// </summary>
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { this.RaiseAndSetIfChanged(self => self.Name, value); }
        }

        /// <summary>
        /// Expected effort measured in sprints
        /// </summary>
        private int _ExpectedEffort;
        public int ExpectedEffort
        {
            get { return _ExpectedEffort; }
            set { this.RaiseAndSetIfChanged(self => self.ExpectedEffort, value); }
        }

        /// <summary>
        /// Actual effort measured in sprints
        /// </summary>
        private int _ActualEffort;
        public int ActualEffort
        {
            get { return _ActualEffort; }
            set { this.RaiseAndSetIfChanged(self => self.ActualEffort, value); }
        }

        /// <summary>
        /// Gets or sets the value indicating wheather the Activity is archived
        /// </summary>
        private bool _Archived;
        public bool Archived
        {
            get { return _Archived; }
            set { this.RaiseAndSetIfChanged(self => self.Archived, value); }
        }

        /// <summary>
        /// Gets or sets the value indicating wheather the Activity is completed
        /// </summary>
        private bool _Completed;
        public bool Completed
        {
            get { return _Completed; }
            set { this.RaiseAndSetIfChanged(self => self.Completed, value); }
        }

        /// <summary>
        /// Gets or sets the time when the Activity was created
        /// </summary>
        private DateTime _CreationTime;
        public DateTime CreationTime
        {
            get { return _CreationTime; }
            set { this.RaiseAndSetIfChanged(self => self.CreationTime, value); }
        }

        /// <summary>
        /// Gets or sets the collection of associated tags
        /// </summary>
        private ICollection<ActivityTag> _Tags;
        public ICollection<ActivityTag> Tags
        {
            get { return _Tags; }
            set { this.RaiseAndSetIfChanged(self => self.Tags, value); }
        }

        /// <summary>
        /// Gets of sets the collection of sprints
        /// </summary>
        private ICollection<Sprint> _Sprints;
        public ICollection<Sprint> Sprints
        {
            get { return _Sprints; }
            set { this.RaiseAndSetIfChanged(self => self.Sprints, value); }
        }

        public Activity()
        {
            Tags = new List<ActivityTag>();
            Sprints = new List<Sprint>();
            CreationTime = DateTime.Now;
        }
    }
}
