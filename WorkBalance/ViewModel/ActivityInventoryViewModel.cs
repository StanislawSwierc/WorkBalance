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
using System.Collections;
using WorkBalance.Utilities;

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
            var selectionNotEmpty = new Func<bool>(() => SelectedActivities != null && SelectedActivities.Count > 0);
            SelectActivityCommand = new RelayCommand(() => SelectActivity(SelectedActivities[0]), selectionNotEmpty);
            DeleteActivityCommand = new RelayCommand(() => SelectedActivities.ForEach(DeleteActivity), selectionNotEmpty);
            ArchiveActivityCommand = new RelayCommand(() => SelectedActivities.ForEach(ArchiveActivity), selectionNotEmpty);
        }

        public IList<Activity> SelectedActivities { get; set; } 
        public ObservableCollection<Activity> Activities { get; private set; }
        public RelayCommand SelectActivityCommand { get; private set; }
        public RelayCommand DeleteActivityCommand { get; private set; }
        public RelayCommand ArchiveActivityCommand { get; private set; }

        private void SelectActivity(Activity activity)
        {
            MessageBus.SendMessage(activity, Notifications.ActivitySelected);
        }

        private void DeleteActivity(Activity activity)
        {
            Activities.Remove(activity);
            ActivityRepository.Delete(activity);    
        }

        private void ArchiveActivity(Activity activity)
        {
            Activities.Remove(activity);
            activity.Archived = true;
            ActivityRepository.Update(activity);
        }


        public void OnImportsSatisfied()
        {
            MessageBus.Listen<Activity>(Notifications.ActivityCreated)
                .ObserveOnDispatcher()
                .Subscribe(Activities.Add);

            ActivityRepository.GetActive().ForEach(Activities.Add);
        }
    }
}
