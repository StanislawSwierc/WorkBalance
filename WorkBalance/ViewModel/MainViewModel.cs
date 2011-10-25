using GalaSoft.MvvmLight;
using System;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.ComponentModel.Composition;
using WorkBalance.Utilities;
using WorkBalance.Domain;

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
    [Export]
    public class MainViewModel : ViewModelBase
    {
        private bool m_Enabled;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        [ImportingConstructor]
        public MainViewModel(IMessenger messenger)
            : base(messenger)
        {
            Contract.Requires(messenger != null);


            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                //MessengerInstance.Register<PropertyChangedMessage<TimerState>>(this, m => System.Windows.MessageBox.Show(m.NewValue.ToString()));
            }
            Timer = new WorkBalance.Timer(MessengerInstance);
            // Translate state change notification and propagate it to the user interface
            Timer.PropertyChanged += new PropertyChangedEventHandler(
                CreatePropertyChangedHandler("State", s => RaisePropertyChanged("ToggleTimerActionName")));
            ToggleTimerCommand = new RelayCommand(Timer.ToggleTimer);
            CreateActivityCommand = new RelayCommand(CreateActivity);
            m_Enabled = true;

            MessengerInstance.Register<Activity>(this, Notifications.ActivitySelected, HandleActivitySelected);
        }

        private void HandleActivitySelected(Activity activity)
        {
            if (Timer.State != TimerState.Sprint)
            {
                CurrentActivity = activity;
            }
        }

        public Timer Timer { get; private set; }

        public string ToggleTimerActionName
        {
            get
            {
                string result = null;
                switch (Timer.State)
                {
                    case TimerState.Ready:
                        result = "Start Sprint";
                        break;
                    case TimerState.Sprint:
                        result = "Abort Sprint";
                        break;
                    case TimerState.Break:
                        result = "Abort Break";
                        break;
                    case TimerState.BreakOverrun:
                        result = "Stop Break";
                        break;
                }
                return result;
            }
        }

        public bool Enabled
        {
            get
            {
                return m_Enabled;
            }
            set
            {
                if (m_Enabled != value)
                {
                    m_Enabled = value;
                    RaisePropertyChanged("Enabled");
                }
            }
        }

        private Activity m_CurrentActivity;
        public Activity CurrentActivity
        {
            get { return m_CurrentActivity; }
            set
            {
                if (m_CurrentActivity != value)
                {
                    m_CurrentActivity = value;
                    RaisePropertyChanged("CurrentActivity");
                }
            }
        }

        private void CreateActivity()
        {
            Enabled = false;
            MessengerInstance.Send<Action>(Notifications.CreateActivityWindowOpen, () => Enabled = true);
        }

        private Action<object, PropertyChangedEventArgs> CreatePropertyChangedHandler(string property, Action<object> handler)
        {
            return new Action<object, PropertyChangedEventArgs>(delegate(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName == "State")
                {
                    handler(sender);
                }
            });
        }

        public RelayCommand ToggleTimerCommand { get; set; }
        public RelayCommand CreateActivityCommand { get; set; }
    }
}