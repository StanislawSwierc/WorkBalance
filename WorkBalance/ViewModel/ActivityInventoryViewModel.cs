using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.ComponentModel;
using System.Windows.Data;
using WorkBalance.Domain;
using WorkBalance.Repositories;
using WorkBalance.Utilities;
using System.ComponentModel.Composition;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using ReactiveUI;
using System.Reactive.Linq;

namespace WorkBalance.ViewModel
{
    [Export]
    public class ActivityInventoryViewModel: ViewModelBase, IPartImportsSatisfiedNotification
    {
        [Import]
        public IActivityRepository ActivityRepository { get; private set; }

        public ActivityInventoryViewModel()
        {
            Activities = new ObservableCollection<Activity>();
            SelectActivityCommand = new RelayCommand<Activity>(SelectActivity);
        }

        public ObservableCollection<Activity> Activities { get; private set; }
        public RelayCommand<Activity> SelectActivityCommand { get; private set; }

        private void SelectActivity(Activity activity)
        {
            if (activity != null)
            {
                MessageBus.SendMessage(activity, Notifications.ActivitySelected);
            }
        }

        public void OnImportsSatisfied()
        {
            MessageBus.Listen<Activity>(Notifications.ActivityCreated)
                .ObserveOnDispatcher()
                .Subscribe(Activities.Add);

            foreach (var activity in ActivityRepository.GetActive())
            {
                Activities.Add(activity);
            }
        }
    }
}
