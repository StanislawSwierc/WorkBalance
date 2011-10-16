using GalaSoft.MvvmLight;
using System;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace WorkBalance.ViewModel
{
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
        private static readonly TimeSpan c_Interval = TimeSpan.FromMinutes(25);

        DispatcherTimer m_Timer;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
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

            m_ToggleTimerCommand = new RelayCommand(ToggleCommand);
        }

        private void ToggleCommand()
        {
            m_Timer.IsEnabled ^= true;
            if (!m_Timer.IsEnabled)
            {
                Clock = c_Interval;
            }
        }

        void HandleTick(object sender, EventArgs e)
        {
            Clock = Clock.Subtract(TimeSpan.FromSeconds(1));
        }


        private TimeSpan m_Clock;
        public TimeSpan Clock { 
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


        private RelayCommand m_ToggleTimerCommand;
        public ICommand ToggleTimerCommand { get { return m_ToggleTimerCommand; } }
    }
}