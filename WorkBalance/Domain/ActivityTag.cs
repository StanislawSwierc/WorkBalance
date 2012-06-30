using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WorkBalance.Domain
{
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

        public override string ToString()
        {
            return (Parent == null) ? Name : string.Format("{0}{1}{2}", Parent, c_Separator, Name);
        }
    }
}
