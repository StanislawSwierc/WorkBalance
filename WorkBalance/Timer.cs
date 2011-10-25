using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Threading;

using System.ComponentModel.Composition;
using WorkBalance.ViewModel;
using ReactiveUI;

namespace WorkBalance
{
    public enum TimerState
    {
        Ready,
        Sprint,
        Break,
        BreakOverrun
    }

    [Export]
    public class Timer : ViewModelBase
    {
        DispatcherTimer m_Timer;
        TimeSpan m_SprintDuration;
        TimeSpan m_BreakDuration;
        ITimerState m_InternalState;
        Dictionary<TimerState, ITimerState> m_InternalStates;

        public Timer()
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                m_SprintDuration = TimeSpan.FromSeconds(10);
                m_BreakDuration = TimeSpan.FromSeconds(5);
            }
            else
            {
                m_SprintDuration = TimeSpan.FromMinutes(25);
                m_BreakDuration = TimeSpan.FromMinutes(5);
            }

            m_InternalStates = new Dictionary<TimerState, ITimerState>();
            m_InternalStates.Add(TimerState.Ready, new ReadyTimerState(this));
            m_InternalStates.Add(TimerState.Sprint, new SprintTimerState(this));
            m_InternalStates.Add(TimerState.Break, new BreakTimerState(this));
            m_InternalStates.Add(TimerState.BreakOverrun, new BreakOverrunTimerState(this));

            _State = TimerState.Ready;
            m_InternalState = m_InternalStates[_State];
            m_InternalState.Activate();

            m_Timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            m_Timer.Tick += HandleTick;
            m_Timer.Start();
        }

        private TimeSpan _Time;
        public TimeSpan Time
        {
            get { return _Time; }
            private set { this.RaiseAndSetIfChanged(x => x.Time, value); }
        }

        private TimerState _State;
        public TimerState State
        {
            get { return _State; }
            private set
            {
                if (_State != value)
                {
                    var oldValue = _State;
                    _State = value;
                    m_InternalState = m_InternalStates[_State];
                    m_InternalState.Activate();
                    this.RaisePropertyChanging(self => self.State);
                    MessageBus.SendMessage<TimerState>(value);
                }
            }
        }

        private void HandleTick(object sender, EventArgs e)
        {
            m_InternalState.HandleSecondElapsed();
        }

        public void ToggleTimer()
        {
            m_InternalState.ToggleTimer();
        }

        #region Internal Types

        internal interface ITimerState
        {
            void Activate();
            void HandleSecondElapsed();
            void ToggleTimer();
        }

        internal abstract class TimerStateBase : ITimerState
        {
            protected Timer m_Timer;

            public TimerStateBase(Timer timer)
            {
                m_Timer = timer;
            }

            public virtual void Activate()
            {
            }

            public virtual void HandleSecondElapsed()
            {
            }

            public virtual void ToggleTimer()
            {
            }
        }

        internal class ReadyTimerState : TimerStateBase
        {
            public ReadyTimerState(Timer timer)
                : base(timer)
            {
            }

            public override void Activate()
            {
                m_Timer.Time = m_Timer.m_SprintDuration;
            }

            public override void ToggleTimer()
            {
                m_Timer.State = TimerState.Sprint;
            }
        }

        internal class SprintTimerState : TimerStateBase
        {
            public SprintTimerState(Timer timer)
                : base(timer)
            {
            }

            public override void HandleSecondElapsed()
            {
                m_Timer.Time = m_Timer.Time.Subtract(TimeSpan.FromSeconds(1));
                if (m_Timer.Time.Ticks == 0)
                {
                    m_Timer.State = TimerState.Break;
                }
            }

            public override void ToggleTimer()
            {
                m_Timer.State = TimerState.Ready;
            }
        }

        internal class BreakTimerState : TimerStateBase
        {
            public BreakTimerState(Timer timer)
                : base(timer)
            {
            }

            public override void HandleSecondElapsed()
            {
                m_Timer.Time = m_Timer.Time.Add(TimeSpan.FromSeconds(1));
                if (m_Timer.Time > m_Timer.m_BreakDuration)
                {
                    m_Timer.State = TimerState.BreakOverrun;
                }
            }

            public override void ToggleTimer()
            {
                m_Timer.State = TimerState.Ready;
            }
        }

        internal class BreakOverrunTimerState : TimerStateBase
        {
            public BreakOverrunTimerState(Timer timer)
                : base(timer)
            {
            }

            public override void HandleSecondElapsed()
            {
                m_Timer.Time = m_Timer.Time.Add(TimeSpan.FromSeconds(1));
            }

            public override void ToggleTimer()
            {
                m_Timer.State = TimerState.Ready;
            }
        }

        #endregion
    }
}
