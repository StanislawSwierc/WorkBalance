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
using System.Reactive.Subjects;
using System.Collections.Specialized;

namespace WorkBalance.ViewModel
{
    [Export]
    public class ActivityInventoryViewModel : ViewModelBase, IPartImportsSatisfiedNotification
    {
        [Import]
        public IActivityRepository ActivityRepository { get; private set; }

        [ImportMany]
        public Lazy<IActivityFormatter, IActivityFormatterMetadata>[] ActivityFormatters { get; set; }

        public ActivityInventoryViewModel()
        {
            Activities = new ObservableCollection<Activity>();

            var activityPropertyChanged = new ObservableEventHandler<Activity, PropertyChangedEventArgs>();
            var activitiesCollectionChanged = new Utilities.ObservableEventHandler<NotifyCollectionChangedEventArgs>();
            
            Activities.CollectionChanged += activitiesCollectionChanged.Handler;

            activitiesCollectionChanged.Where(e => e.EventArgs.Action == NotifyCollectionChangedAction.Add)
                .SelectMany(e => e.EventArgs.NewItems.Cast<Activity>())
                .Subscribe(a => a.PropertyChanged += activityPropertyChanged.Handler);

            activitiesCollectionChanged.Where(e => e.EventArgs.Action == NotifyCollectionChangedAction.Remove)
                .SelectMany(e => e.EventArgs.OldItems.Cast<Activity>())
                .Subscribe(a => a.PropertyChanged -= activityPropertyChanged.Handler);

            activitiesCollectionChanged.Select(e => Unit.Default)
                .Merge(activityPropertyChanged.Where(e => e.EventArgs.PropertyName == "ExpectedEffort").Select(e => Unit.Default))
                .Subscribe(u => ExpectedEffortSum = Activities.Select(a => a.ExpectedEffort).Sum());

            activitiesCollectionChanged.Select(e => Unit.Default)
                .Merge(activityPropertyChanged.Where(e => e.EventArgs.PropertyName == "ActualEffort").Select(e => Unit.Default))
                .Subscribe(u => ActualEffortSum = Activities.Select(a => a.ActualEffort).Sum());

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

        private int _ExpectedEffortSum;
        public int ExpectedEffortSum
        {
            get { return _ExpectedEffortSum; }
            set { this.RaiseAndSetIfChanged(self => self.ExpectedEffortSum, value); }
        }

        private int _ActualEffortSum;
        public int ActualEffortSum
        {
            get { return _ActualEffortSum; }
            set { this.RaiseAndSetIfChanged(self => self.ActualEffortSum, value); }
        }

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

        private void AddActivity(Activity activity)
        {
            Activities.Add(activity);
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
                ActivityFormatters[0].Value.FormatActivities(activities),
                ActivityFormatters[0].Metadata.Format);
        }

        public void OnImportsSatisfied()
        {
            MessageBus.Listen<Activity>(Notifications.ActivityCreated)
                .ObserveOnDispatcher()
                .Subscribe(AddActivity);

            MessageBus.Listen<Unit>(Notifications.CopyActivitiesToClipboard)
                .ObserveOnDispatcher()
                .Where(u => !EnumerableExtensions.IsNullOrEmpty(SelectedActivities))
                .Subscribe((u) => CopyActivitiesToClipboard(SelectedActivities));

            ActivityRepository.GetActive().ForEach(AddActivity);
        }
    }
}
