using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using System.Windows.Threading;

namespace WorkBalance
{
    public enum TimerState
    {
        Ready,
        Sprint,
        Break,
        BreakOverrun
    }

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

            m_State = TimerState.Break;
            State = TimerState.Ready;

            m_Timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            m_Timer.Tick += HandleTick;
            m_Timer.Start();
        }

        private TimeSpan m_Time;
        public TimeSpan Time
        {
            get { return m_Time; }
            private set
            {
                if (value != m_Time)
                {
                    m_Time = value;
                    RaisePropertyChanged("Time");
                }
            }
        }

        private TimerState m_State;
        public TimerState State
        {
            get { return m_State; }
            private set
            {
                if (m_State != value)
                {
                    m_State = value;
                    m_InternalState = m_InternalStates[m_State];
                    m_InternalState.Activate();
                    RaisePropertyChanged("State");
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

            public virtual  void Activate()
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
