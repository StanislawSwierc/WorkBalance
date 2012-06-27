using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using WorkBalance.Repositories;
using System.ComponentModel.Composition;
using WorkBalance.Domain;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using WorkBalance.Utilities;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI.Xaml;
using System.Reactive.Concurrency;

namespace WorkBalance.ViewModel
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CreateActivityViewModel : ViewModelBase
    {
        [Import]
        public IActivityRepository ActivityRepository;

        [Import]
        private IActivityTagRepository ActivityTagRepository;



        private string _ExpectedEffort;

        [ImportingConstructor]
        public CreateActivityViewModel()
        {
            int result = 0;
            var canSave = this.WhenAny(
                x => x.Name,
                x => x.ExpectedEffort,
                (n, e) => !string.IsNullOrWhiteSpace(n.Value) && int.TryParse(e.Value, out result));
            SaveCommand = new ReactiveCommand(canSave, DispatcherScheduler.Instance);
            SaveCommand.Subscribe(o => Save());

            var prototypeActivity = this.Changed
                .Where(c => c.PropertyName == "Name")
                .Throttle(TimeSpan.FromMilliseconds(500))
                .Select(c => Name)
                .ObserveOnDispatcher()
                // Access to the ActivityRepository synchronized on the main thread... maybe not the best idea
                .Select(n => ActivityRepository.Get().Where(a => a.Name == n).OrderByDescending(a => a.CreationTime).FirstOrDefault())
                .Where(a => a != null);

            prototypeActivity
                .Where(a => string.IsNullOrWhiteSpace(Tags) && a.Tags != null)
                .Select(a => string.Join(" ", a.Tags.Select(t => t.ToString())))
                .ObserveOnDispatcher()
                .Subscribe(t => Tags = t);

            prototypeActivity
                .Where(a => string.IsNullOrWhiteSpace(ExpectedEffort))
                .Select(a => a.ExpectedEffort.ToString())
                .ObserveOnDispatcher()
                .Subscribe(ee => ExpectedEffort = ee);

            CancelCommand = new RelayCommand(Cancel);
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { this.RaiseAndSetIfChanged(x => x.Name, value); }
        }

        public string ExpectedEffort
        {
            get { return _ExpectedEffort; }
            set { this.RaiseAndSetIfChanged(x => x.ExpectedEffort, value); }
        }

        private string _Tags;
        public string Tags 
        {
            get { return _Tags; }
            set { this.RaiseAndSetIfChanged(x => x.Tags, value); }
        }

        public ReactiveCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        public bool CanExecuteSave()
        {
            //return !string.IsNullOrWhiteSpace(Activity.Name);
            return true;
        }

        public void Save()
        {
            string[] tagNames = string.IsNullOrWhiteSpace(Tags) ? new string[0] :
                Tags.ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Convert tag names to real tags stored in the database
            var tags = tagNames.Select(name =>
            {
                var tag = ActivityTagRepository.Get().Where(t => t.Name == name).SingleOrDefault();
                if (tag == null)
                {
                    tag = new ActivityTag() { Name = name };
                    ActivityTagRepository.Add(tag);
                }
                return tag;
            }).ToList();

            var activity = new Activity();
            activity.Name = Name;
            activity.ExpectedEffort = int.Parse(ExpectedEffort);
            activity.Tags = tags;

            ActivityRepository.Add(activity);
            MessageBus.SendMessage(activity, Notifications.ActivityCreated);
            Close();
        }

        public void Cancel()
        {
            Close();
        }

        private void Close()
        {
            MessageBus.SendMessage<Unit>(Unit.Default, Notifications.CreateActivityWindowClose);
        }
    }
}
