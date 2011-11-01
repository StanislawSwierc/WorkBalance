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
            DeleteActivityCommand = new RelayCommand<Activity>(DeleteActivity);
            ArchiveActivityCommand = new RelayCommand<Activity>(ArchiveActivity);
        }

        public ObservableCollection<Activity> Activities { get; private set; }
        public RelayCommand<Activity> SelectActivityCommand { get; private set; }
        public RelayCommand<Activity> DeleteActivityCommand { get; private set; }
        public RelayCommand<Activity> ArchiveActivityCommand { get; private set; }

        private void SelectActivity(Activity activity)
        {
            if (activity != null)
            {
                MessageBus.SendMessage(activity, Notifications.ActivitySelected);
            }
        }

        private void DeleteActivity(Activity activity)
        {
            // TODO: Remove this check
            if (activity != null)
            {
                Activities.Remove(activity);
                ActivityRepository.Delete(activity);
            }
        }

        private void ArchiveActivity(Activity activity)
        {
            // TODO: Remove this check
            if (activity != null)
            {
                Activities.Remove(activity);
                activity.Archived = true;
                ActivityRepository.Update(activity);
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
