using GalaSoft.MvvmLight;
using System;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using System.Diagnostics.Contracts;

namespace WorkBalance.ViewModel
{
    public enum TimerState
    {
        Ready,
        Running,
        Break
    }

    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        //private static readonly TimeSpan c_Interval = TimeSpan.FromMinutes(25);
        private static readonly TimeSpan c_Interval = TimeSpan.FromSeconds(10);
        private static readonly string c_StartTimerText = "Start Timer";
        private static readonly string c_StopTimerText = "Stop Timer";
        private static readonly string c_StopBreakText = "Stop Break";

        DispatcherTimer m_Timer;
        IMessenger m_Messenger; 

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IMessenger messenger)
        {
            Contract.Requires(messenger != null);

            m_Messenger = messenger;
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            m_Clock = c_Interval;

            m_Timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            m_Timer.Tick += HandleTick;
            m_ToggleTimerActionName = c_StartTimerText;
            State = TimerState.Ready;
            m_ToggleTimerCommand = new RelayCommand(ToggleCommand);
            m_CloseCommand = new RelayCommand(() => App.Current.Shutdown());            
        }

        private void ToggleCommand()
        {
            switch (State)
            {
                case TimerState.Ready:
                    State = TimerState.Running;
                    break;
                case TimerState.Running:
                    State = TimerState.Ready;
                    break;
                case TimerState.Break:
                    State = TimerState.Ready;
                    break;
                default:
                    break;
            }
        }

        void HandleTick(object sender, EventArgs e)
        {
            switch (State)
            {
                case TimerState.Ready:
                    break;
                case TimerState.Running:
                    Clock = Clock.Subtract(TimeSpan.FromSeconds(1));
                    break;
                case TimerState.Break:
                    Clock = Clock.Add(TimeSpan.FromSeconds(1));
                    break;
                default:
                    break;
            }
            if (State == TimerState.Running && Clock.Ticks == 0) State = TimerState.Break;
            if (State == TimerState.Break && Clock == c_Interval) State = TimerState.Ready;
        }


        private TimeSpan m_Clock;
        public TimeSpan Clock 
        { 
            get { return m_Clock; }
            private set 
            {
                if (value != m_Clock)
                {
                    m_Clock = value;
                    RaisePropertyChanged("Clock");
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
                    switch (m_State)
                    {
                        case TimerState.Ready:
                            m_Timer.IsEnabled = false;
                            Clock = c_Interval;
                            ToggleTimerActionName = c_StartTimerText;
                            break;
                        case TimerState.Running:
                            m_Timer.IsEnabled = true;
                            ToggleTimerActionName = c_StopTimerText;
                            break;
                        case TimerState.Break:
                            ToggleTimerActionName = c_StopBreakText;
                            break;
                        default:
                            break;
                    }
                }
            }
        }


        private string m_ToggleTimerActionName;
        public string ToggleTimerActionName
        {
            get { return m_ToggleTimerActionName; }
            private set
            {
                if (m_ToggleTimerActionName != value)
                {
                    m_ToggleTimerActionName = value;
                    RaisePropertyChanged("ToggleTimerActionName");
                }
            }
        }
        

        private RelayCommand m_ToggleTimerCommand;
        public ICommand ToggleTimerCommand { get { return m_ToggleTimerCommand; } }

        private RelayCommand m_CloseCommand;
        public ICommand CloseCommand { get { return m_CloseCommand; } }
    }
}