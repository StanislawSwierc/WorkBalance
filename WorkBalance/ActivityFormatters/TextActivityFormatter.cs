using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkBalance.Contracts;
using System.Windows;
using WorkBalance.Domain;

namespace WorkBalance.ActivityFormatters
{
    [ActivityFormatterExport(Name = "Text", Format = TextDataFormat.Text)]
    public class TextActivityFormatter : IActivityFormatter
    {
        public virtual string FormatActivity(Domain.Activity activity)
        {
            return string.Format("{0}\t{1}\t{2}\t{3}",
                activity.Name,
                string.Join(" ", (activity.Tags ?? Enumerable.Empty<ActivityTag>()).Select(t => t.Name).ToArray()),
                activity.ExpectedEffort,
                activity.ActualEffort);
        }

        public virtual string FormatActivities(IEnumerable<Activity> activities)
        {
            return string.Join(Environment.NewLine,
                activities.OrderBy(a => a.CreationTime).Select(FormatActivity));
        }
    }
}
