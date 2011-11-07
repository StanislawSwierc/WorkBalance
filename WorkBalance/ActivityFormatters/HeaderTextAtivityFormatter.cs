using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkBalance.Contracts;
using System.Windows;

namespace WorkBalance.ActivityFormatters
{
    [ActivityFormatterExport(Name="Text with headers", Format=TextDataFormat.Text)]
    public class HeaderTextAtivityFormatter: TextActivityFormatter
    {
        public override string FormatActivities(IEnumerable<Domain.Activity> activities)
        {
            string text = base.FormatActivities(activities);
            return string.Format("Activity\tTags\tExpected\tActual{0}{1}", Environment.NewLine, text);
        }
    }
}
