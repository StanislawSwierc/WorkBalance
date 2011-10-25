
using System;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

using System.Diagnostics.Contracts;
using System.ComponentModel;
using System.ComponentModel.Composition;
using WorkBalance.Utilities;
using WorkBalance.Domain;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI.Xaml;
using System.Reactive.Concurrency;

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
    public class MainViewModel : ViewModelBase, IPartImportsSatisfiedNotification

    {
        [Import]
        public Timer Timer { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            // Translate state change notification and propagate it to the user interface

            CreateActivityCommand = new RelayCommand(CreateActivity);
            _Enabled = true;
        }

        public void OnImportsSatisfied()
        {
            var canToggleTimerCommand = Observable.CombineLatest(
                this.WhenAny(x => x.CurrentActivity, e => e.Value),
                Timer.WhenAny(x => x.State, e => e.Value),
                (a, s) => (s != TimerState.Ready) || (a != null)
                );
            ToggleTimerCommand = new ReactiveCommand(canToggleTimerCommand, DispatcherScheduler.Instance);
            ToggleTimerCommand.Subscribe(o => Timer.ToggleTimer());

            MessageBus.Listen<Activity>(Notifications.ActivitySelected)
                .Where(a => Timer.State != TimerState.Sprint)
                .ObserveOnDispatcher()
                .Subscribe(a => CurrentActivity = a);

            MessageBus.Listen<TimerState>()
                .ObserveOnDispatcher()
                .Subscribe(s => this.RaisePropertyChanged(x => x.ToggleTimerActionName));
        }

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

        private bool _Enabled;
        public bool Enabled
        {
            get { return _Enabled; }
            set { this.RaiseAndSetIfChanged(self => self.Enabled, value); }
        }

        private Activity _CurrentActivity;
        public Activity CurrentActivity
        {
            get { return _CurrentActivity; }
            set { this.RaiseAndSetIfChanged(self => self.CurrentActivity, value); }
        }

        private void CreateActivity()
        {
            Enabled = false;
            MessageBus.SendMessage<Action>(() => Enabled = true, Notifications.CreateActivityWindowOpen);
        }

        public ReactiveCommand ToggleTimerCommand { get; set; }
        public RelayCommand CreateActivityCommand { get; set; }
    }
}