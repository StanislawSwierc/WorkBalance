using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Threading;

using System.ComponentModel.Composition;
using WorkBalance.ViewModel;
using ReactiveUI;
using WorkBalance.Domain;
using System.Reactive;
using System.Reactive.Linq;
using WorkBalance.Repositories;
using System.Diagnostics.Contracts;
using ReactiveUI.Xaml;
using System.Reactive.Concurrency;
using WorkBalance.Contracts;

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
    public class Timer : ViewModelBase, IPartImportsSatisfiedNotification
    {
        [Import]
        public IActivityRepository ActivityRepository { get; set; }

        [Import]
        public ISprintRepository SprintRepository { get; set; }

        [ImportMany]
        public ISprintListener[] SprintListeners { get; set; }

        DispatcherTimer m_Timer;
        TimeSpan m_SprintDuration;
        TimeSpan m_BreakDuration;
        ITimerState m_InternalState;
        Dictionary<TimerState, ITimerState> m_InternalStates;

        public ReactiveCommand ToggleTimerCommand { get; set; }

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

            m_Timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            m_Timer.Tick += (s, e) => m_InternalState.HandleSecondElapsed();

            _State = TimerState.Ready;
            m_InternalState = m_InternalStates[_State];
            m_InternalState.OnEnter();
        }

        public void OnImportsSatisfied()
        {
            MessageBus.Listen<Activity>(Notifications.ActivitySelected)
                .Where(a => this.State != TimerState.Sprint)
                .ObserveOnDispatcher()
                .Subscribe(a => CurrentActivity = a);

            var canToggleTimerCommand = this.WhenAny(
                x => x.State, 
                x => x.CurrentActivity,
                (state, activity) => !(state.Value == TimerState.Ready && activity.Value == null));

            ToggleTimerCommand = new ReactiveCommand(canToggleTimerCommand, DispatcherScheduler.Instance);
            ToggleTimerCommand.Subscribe(o => m_InternalState.ToggleTimer());
        }

        private TimeSpan _Time;
        public TimeSpan Time
        {
            get { return _Time; }
            private set { this.RaiseAndSetIfChanged(x => x.Time, value); }
        }

        public string ToggleTimerActionName
        {
            get { return m_InternalState.ToggleTimerActionName; }
        }

        private Activity _CurrentActivity;
        public Activity CurrentActivity
        {
            get { return _CurrentActivity; }
            set { this.RaiseAndSetIfChanged(self => self.CurrentActivity, value); }
        }

        private TimerState _State;
        public TimerState State
        {
            get { return _State; }
            private set
            {
                if (_State != value)
                {
                    m_InternalState.OnLeave();
                    _State = value;
                    m_InternalState = m_InternalStates[_State];
                    m_InternalState.OnEnter();
                    this.RaisePropertyChanged(self => self.State);
                    this.RaisePropertyChanged(self => self.ToggleTimerActionName);
                }
            }
        }

        #region Internal Types

        internal interface ITimerState
        {
            void OnEnter();
            void OnLeave();
            void HandleSecondElapsed();
            void ToggleTimer();
            string ToggleTimerActionName { get; }
        }

        internal abstract class TimerStateBase : ITimerState
        {
            protected Timer m_Timer;

            public TimerStateBase(Timer timer)
            {
                m_Timer = timer;
            }

            public virtual void OnEnter()
            {
            }

            public virtual void OnLeave()
            {
            }

            public virtual void HandleSecondElapsed()
            {
            }

            public virtual void ToggleTimer()
            {
            }


            public abstract string ToggleTimerActionName
            {
                get;
            }
        }

        internal class ReadyTimerState : TimerStateBase
        {
            public ReadyTimerState(Timer timer)
                : base(timer)
            {
            }

            public override void OnEnter()
            {
                m_Timer.Time = m_Timer.m_SprintDuration;
                m_Timer.m_Timer.Stop();
            }

            public override void ToggleTimer()
            {
                m_Timer.State = TimerState.Sprint;
            }

            public override string ToggleTimerActionName
            {
                get { return "Start Sprint"; }
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

            public override string ToggleTimerActionName
            {
                get { return "Abort Sprint"; }
            }

            public override void OnEnter()
            {
                m_Timer.m_Timer.Start();
                var sprint = new Sprint(m_Timer.CurrentActivity);
                m_Timer.CurrentActivity.Sprints.Add(sprint);
                m_Timer.SprintRepository.Add(sprint);
                m_Timer.ActivityRepository.Update(m_Timer.CurrentActivity);
                foreach (var listener in m_Timer.SprintListeners)
                {
                    listener.OnSprintStarted(sprint);
                }
            }

            public override void OnLeave()
            {
                var sprint = m_Timer.CurrentActivity.Sprints.Last();
                sprint.EndTime = DateTime.Now;
                // If the sprint was full-length
                if (sprint.Duration >= m_Timer.m_SprintDuration)
                {
                    // Record sprint as actual effort
                    m_Timer.CurrentActivity.ActualEffort++;
                    m_Timer.ActivityRepository.Update(m_Timer.CurrentActivity);
                }
                m_Timer.SprintRepository.Update(sprint);
                foreach (var listener in m_Timer.SprintListeners)
                {
                    listener.OnSprintEnded(sprint);
                }
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

            public override string ToggleTimerActionName
            {
                get { return "Abort Break"; }
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

            public override string ToggleTimerActionName
            {
                get { return "Stop Break"; }
            }
        }

        #endregion
    }
}
