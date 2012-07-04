using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
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
        public IDomainContext DomainContext { get; set; }

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
                .Select(n => DomainContext.Activities.Where(a => a.Name == n).OrderByDescending(a => a.CreationTime).FirstOrDefault())
                .Where(a => a != null);

            prototypeActivity
                .Select(a => a.Tags.Select(t => t.Name).ToArray())
                .ObserveOnDispatcher()
                .Subscribe(t => Tags = t);

            prototypeActivity
                .Where(a => string.IsNullOrWhiteSpace(ExpectedEffort))
                .Select(a => a.ExpectedEffort.ToString())
                .ObserveOnDispatcher()
                .Subscribe(ee => ExpectedEffort = ee);

            CancelCommand = new RelayCommand(Cancel);
        }

        public Activity Activity { get; set; }

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

        private string[] _Tags;
        public string[] Tags
        {
            get { return _Tags; }
            set { this.RaiseAndSetIfChanged(x => x.Tags, value); }
        }

        public ReactiveCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        public void Save()
        {
            Activity = new Activity();
            Activity.Name = Name;
            Activity.ExpectedEffort = int.Parse(ExpectedEffort);
            Activity.Tags = DomainContext.ActivityTags.GetOrCreate(Tags);

            Close(true);
        }

        public void Cancel()
        {
            Close(false);
        }

        private void Close(bool? result)
        {
            CloseRequestSubject.OnNext(result);
        }

        public Subject<bool?> CloseRequestSubject = new Subject<bool?>();
        public IObservable<bool?> CloseRequest { get { return CloseRequestSubject; } } 
    }
}
