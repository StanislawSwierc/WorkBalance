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
using System.Reactive;
using WorkBalance.Contracts;

namespace WorkBalance.ViewModel
{
    [Export]
    public class ActivityInventoryViewModel : ViewModelBase, IPartImportsSatisfiedNotification
    {
        [Import]
        public IActivityRepository ActivityRepository { get; private set; }

        [ImportMany]
        public Lazy<IActivityFormatter, IActivityFormatterMetadata>[] ActivityFormatters{ get; set; }

        public ActivityInventoryViewModel()
        {
            Activities = new ObservableCollection<Activity>();
            var selectedActivitiesNotEmpty = new Func<bool>(() => !EnumerableExtensions.IsNullOrEmpty(SelectedActivities));
            SelectActivityCommand = new RelayCommand(() => SelectActivity(SelectedActivities[0]), selectedActivitiesNotEmpty);
            DeleteActivityCommand = new RelayCommand(() => SelectedActivities.ForEach(DeleteActivity), selectedActivitiesNotEmpty);
            ArchiveActivityCommand = new RelayCommand(() => SelectedActivities.ForEach(ArchiveActivity), selectedActivitiesNotEmpty);
            IncreaseActualEffortCommand = new RelayCommand(() => SelectedActivities.ForEach(IncreaseActualEffort), selectedActivitiesNotEmpty);
            DecreaseActualEffortCommand = new RelayCommand(() => SelectedActivities.ForEach(DecreaseActualEffort), selectedActivitiesNotEmpty);
            EditActivityCommand = new RelayCommand(() => MessageBus.SendMessage(SelectedActivities[0], Notifications.Edit), selectedActivitiesNotEmpty);
        }

        public IList<Activity> SelectedActivities { get; set; }
        public ObservableCollection<Activity> Activities { get; private set; }
        public RelayCommand SelectActivityCommand { get; private set; }
        public RelayCommand DeleteActivityCommand { get; private set; }
        public RelayCommand ArchiveActivityCommand { get; private set; }
        public RelayCommand EditActivityCommand { get; private set; }
        public RelayCommand IncreaseActualEffortCommand { get; private set; }
        public RelayCommand DecreaseActualEffortCommand { get; private set; }

        private void SelectActivity(Activity activity)
        {
            MessageBus.SendMessage(activity, Notifications.ActivitySelected);
        }

        private void IncreaseActualEffort(Activity activity)
        {
            activity.ActualEffort += 1;
            ActivityRepository.Update(activity);
        }

        private void DecreaseActualEffort(Activity activity)
        {
            activity.ActualEffort -= 1;
            ActivityRepository.Update(activity);
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

        private void CopyActivitiesToClipboard(IEnumerable<Activity> activities)
        {
            System.Windows.Clipboard.SetText(
                ActivityFormatters[1].Value.FormatActivities(activities), 
                ActivityFormatters[1].Metadata.Format);
        }


        public void OnImportsSatisfied()
        {
            MessageBus.Listen<Activity>(Notifications.ActivityCreated)
                .ObserveOnDispatcher()
                .Subscribe(Activities.Add);

            MessageBus.Listen<Unit>(Notifications.CopyActivitiesToClipboard)
                .ObserveOnDispatcher()
                .Where(u => !EnumerableExtensions.IsNullOrEmpty(SelectedActivities))
                .Subscribe((u) => CopyActivitiesToClipboard(SelectedActivities));

            ActivityRepository.GetActive().ForEach(Activities.Add);
        }
    }
}
