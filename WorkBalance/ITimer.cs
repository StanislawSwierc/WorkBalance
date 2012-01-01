using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkBalance.Domain;

namespace WorkBalance
{
    public interface ITimer
    {
        TimeSpan Time { get; }

        Activity CurrentActivity { get; }

        TimerState State { get; }

        event EventHandler<TimerStateChangedEventArgs> StateChanged;
    }
}
