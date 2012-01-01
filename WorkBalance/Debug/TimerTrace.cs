using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Diagnostics;
using WorkBalance.Contracts;

namespace WorkBalance.Debug
{
    [Export(typeof(IPlugin))]
    public class TimerTrace : IPlugin, IPartImportsSatisfiedNotification
    {
        [Import]
        ITimer Timer { get; set; }

        public void OnImportsSatisfied()
        {
            // Hook for the lifetime of the application
            Timer.StateChanged += new EventHandler<TimerStateChangedEventArgs>(Timer_StateChanged);
        }

        void Timer_StateChanged(object sender, TimerStateChangedEventArgs e)
        {
            switch (e.Transition)
            {
                case TimerStateTransition.ReadyToSprint:
                    Trace.WriteLine(string.Format("Sprint started with activity: {0}", Timer.CurrentActivity.Name));
                    break;
                case TimerStateTransition.SprintToReady:
                    Trace.WriteLine("Sprint aborted");
                    break;
                case TimerStateTransition.SprintToHomeStraight:
                    Trace.WriteLine("Sprint entered home straight phase");
                    break;
                case TimerStateTransition.HomeStraightToReady:
                    Trace.WriteLine("Sprint aborted in home straight phase");
                    break;
                case TimerStateTransition.HomeStraightToBreak:
                    Trace.WriteLine("Sprint finished");
                    break;
                case TimerStateTransition.BreakToReady:
                    Trace.WriteLine("Break aborted");
                    break;
                case TimerStateTransition.BreakToBreakOverrun:
                    Trace.WriteLine("Break reached its maximum");
                    break;
                case TimerStateTransition.BreakOverrunToReady:
                    Trace.WriteLine("Break finished");
                    break;
                default:
                    break;
            }
        }
    }
}
