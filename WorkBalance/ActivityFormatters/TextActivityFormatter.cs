using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkBalance.Contracts;
using System.Windows;
using WorkBalance.Domain;
using WorkBalance.Utilities;

namespace WorkBalance.ActivityFormatters
{
    [ActivityFormatterExport(Name = "Text", Format = TextDataFormat.Text)]
    public class TextActivityFormatter : IActivityFormatter
    {
        private const string c_Header = "Activity\tTags\tExpected\tActual";

        public TextActivityFormatter()
        {
            IncludeHeader = true;
            IncludeSummary = true;
        }

        public bool IncludeHeader { get; set; }
        public bool IncludeSummary { get; set; }

        public virtual string FormatActivity(Domain.Activity activity)
        {
            return FormatActivity(activity, IncludeHeader, IncludeSummary);
        }

        public virtual string FormatActivities(IEnumerable<Activity> activities)
        {
            var sb = new StringBuilder();
            if (IncludeHeader) sb.AppendLine(c_Header);
            activities.OrderBy(a => a.CreationTime).Select(a => FormatActivity(a, false, false)).ForEach(a => sb.AppendLine(a));
            if (IncludeSummary) sb.AppendLine(CreateSummary(activities));
            return sb.ToString();
        }

        public virtual string FormatActivity(Domain.Activity activity, bool includeHeader, bool includeSummary)
        {
            return string.Format("{0}\t{1}\t{2}\t{3}",
                activity.Name,
                string.Join(" ", (activity.Tags ?? Enumerable.Empty<ActivityTag>()).Select(t => t.Name).ToArray()),
                activity.ExpectedEffort,
                activity.ActualEffort);
        }

        private string CreateSummary(IEnumerable<Activity> activities)
        {
            return string.Format("Summary\t\t{0}\t{1}", 
                activities.Select(a => a.ExpectedEffort).Sum(), 
                activities.Select(a => a.ActualEffort).Sum());
        }

    }
}
