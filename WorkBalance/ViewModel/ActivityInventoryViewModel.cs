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

namespace WorkBalance.ViewModel
{
    [Export]
    public class ActivityInventoryViewModel : ViewModelBase, IPartImportsSatisfiedNotification
    {
        [Import]
        public IActivityRepository ActivityRepository { get; private set; }

        public ActivityInventoryViewModel()
        {
            Activities = new ObservableCollection<Activity>();
            var selectedActivitiesNotEmpty = new Func<bool>(() => !EnumerableExtensions.IsNullOrEmpty(SelectedActivities));
            SelectActivityCommand = new RelayCommand(() => SelectActivity(SelectedActivities[0]), selectedActivitiesNotEmpty);
            DeleteActivityCommand = new RelayCommand(() => SelectedActivities.ForEach(DeleteActivity), selectedActivitiesNotEmpty);
            ArchiveActivityCommand = new RelayCommand(() => SelectedActivities.ForEach(ArchiveActivity), selectedActivitiesNotEmpty);
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

        private void CopyActivitiesToClipboard(IEnumerable<Activity> activities)
        {
            var text = activities.Aggregate(
                   new StringBuilder(),
                   (sb, a) =>
                   {
                       sb.AppendLine(string.Format("{0}\t{1}\t{2}\t{3}",
                       a.Name,
                          string.Join(" ", (a.Tags ?? Enumerable.Empty<ActivityTag>()).Select(t => t.Name).ToArray()),
                          a.ExpectedEffort, 
                          a.ActualEffort));
                       return sb;
                   },
                   sb => sb.ToString());

            System.Windows.Clipboard.SetText(text, System.Windows.TextDataFormat.Text);
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
