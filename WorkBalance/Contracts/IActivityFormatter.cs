using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkBalance.Domain;
using System.ComponentModel.Composition;
using System.Windows;

namespace WorkBalance.Contracts
{
    public interface IActivityFormatter
    {
        string FormatActivity(Activity activity);
        string FormatActivities(IEnumerable<Activity> activities);
    }

    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class ActivityFormatterExportAttribute : ExportAttribute, IActivityFormatterMetadata
    {
        public ActivityFormatterExportAttribute()
            :base(typeof(IActivityFormatter))
        {
        }

        public TextDataFormat Format { get; set; }
        public string Name { get; set; }
    }

    public interface IActivityFormatterMetadata
    {
        TextDataFormat Format { get; }
        string Name { get; }
    }
}
