using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkBalance
{
    public class TimerStateChangedEventArgs : EventArgs
    {
        public TimerStateChangedEventArgs(TimerState previousState, TimerState newState)
        {
            PreviousState = previousState;
            NewState = newState;
        }

        public TimerState PreviousState { get; private set; }
        public TimerState NewState { get; private set; }

        public TimerStateTransition Transition { get { return (TimerStateTransition)(10 * (int)PreviousState + (int)NewState); } }

        internal static void InvokeEventHandler(EventHandler<TimerStateChangedEventArgs> handler, object sender, TimerState previousState, TimerState newState)
        {
            var snapshot = handler;
            if (snapshot != null)
            {
                snapshot.DynamicInvoke(sender, new TimerStateChangedEventArgs(previousState, newState));
            }
        }

    }
}
