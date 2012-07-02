using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WorkBalance.Domain
{
    /// <summary>
    /// Tag for the activity
    /// </summary>
    /// <remarks>
    /// There are few different ways one could implement tags.
    /// A good comparison of solutions can be found at:
    /// http://www.pui.ch/phred/archives/2005/04/tags-database-schemas.html
    ///
    ///  I decided that creating and entity for tag will be the best option
    /// because it is possible to add some metadata to tags like description
    /// or parent to organize them into hierarchies.
    /// </remarks>
    public class ActivityTag : Entity
    {
        private const char c_Separator = '\\';
        private static readonly Regex c_NameValidator = new Regex(@"^[^\s]+");

        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set
            {
                if (m_Name != value)
                {
                    if (!c_NameValidator.IsMatch(value))
                    {
                        throw new FormatException("Tag Name cannot contain any whitespaces");
                    }
                    m_Name = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the value indicating wheather the ActivityTag is archived
        /// </summary>
        public bool Archived { get; set; }

        public ActivityTag Parent { get; set; }

        private ICollection<Activity> _activities;
        public ICollection<Activity> Activities
        {
            get { return _activities; }
            set { Set(ref _activities, value, "Activities"); }
        }

        public override string ToString()
        {
            return (Parent == null) ? Name : string.Format("{0}{1}{2}", Parent, c_Separator, Name);
        }
    }
}
