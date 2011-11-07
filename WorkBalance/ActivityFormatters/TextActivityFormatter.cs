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
            throw new NotImplementedException();
        }

        public virtual string FormatActivities(IEnumerable<Activity> activities)
        {
            return activities
                .OrderBy(a => a.CreationTime)
                .Aggregate(
                    new StringBuilder(),
                    (sb, a) =>
                    {
                        sb.AppendLine(string.Format("{0}\t{1}\t{2}\t{3}",
                        a.Name,
                            string.Join(" ", (a.Tags ?? Enumerable.Empty<ActivityTag>()).Select(t => t.Name).ToArray()),
                            a.ExpectedEffort,
                            a.ActualEffort));
                        return sb;
                    },
                    sb => sb.ToString());
        }
    }
}
