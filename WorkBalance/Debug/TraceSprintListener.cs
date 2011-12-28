using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkBalance.Contracts;
using System.ComponentModel.Composition;

namespace WorkBalance.Debug
{
    [Export(typeof(ISprintListener))]
    class TraceSprintListener : ISprintListener
    {
        public void OnSprintStarted(Domain.Sprint sprint)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("Sprint started :{0}", sprint.Activity.Name));
        }

        public void OnSprintEnded(Domain.Sprint sprint)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("Sprint ended :{0}", sprint.Activity.Name));
        }
    }
}
