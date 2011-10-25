
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
using WorkBalance.Repositories;

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
        public Timer Timer { get; set; }


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


        }

        private bool _Enabled;
        public bool Enabled
        {
            get { return _Enabled; }
            set { this.RaiseAndSetIfChanged(self => self.Enabled, value); }
        }

        private void CreateActivity()
        {
            Enabled = false;
            MessageBus.SendMessage<Action>(() => Enabled = true, Notifications.CreateActivityWindowOpen);
        }


        public RelayCommand CreateActivityCommand { get; set; }
    }
}