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
using ReactiveUI.Xaml;
using System.Reactive.Concurrency;
using System.Diagnostics.Contracts;

namespace WorkBalance.ViewModel
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class EditActivityViewModel : ViewModelBase
    {
        [Import]
        public IDomainContext DomainContext { get; set; }

        public EditActivityViewModel()
        {
            int result = 0;
            var canSave = this.WhenAny(
                x => x.Name,
                x => x.ExpectedEffort,
                (n, e) => !string.IsNullOrWhiteSpace(n.Value) && int.TryParse(e.Value, out result));
            SaveCommand = new ReactiveCommand(canSave, DispatcherScheduler.Instance);
            SaveCommand.Subscribe(o => Save());

            CancelCommand = new RelayCommand(Cancel);
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { this.RaiseAndSetIfChanged(x => x.Name, value); }
        }

        private string _ExpectedEffort;
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

        private Activity _Activity;
        public Activity Activity
        {
            get { return _Activity; }
            set
            {
                Contract.Requires(value != null);
                if (_Activity != value)
                {
                    _Activity = value;
                    Name = _Activity.Name;
                    ExpectedEffort = _Activity.ExpectedEffort.ToString();
                    Tags = (_Activity.Tags == null) ? string.Empty :
                        string.Join(" ", _Activity.Tags.Select(t => t.ToString()));
                }
            }
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
                var tag = DomainContext.ActivityTags.SingleOrDefault(t => t.Name == name);
                if (tag == null)
                {
                    tag = new ActivityTag() { Name = name };
                }
                return tag;
            }).ToList();

            Activity.Name = Name;
            Activity.ExpectedEffort = int.Parse(ExpectedEffort);
            Activity.Tags = tags;

            // TODO: This should be updated by the caller
            DomainContext.Activities.Update(Activity);
            
            Close();
        }

        public void Cancel()
        {
            Close();
        }

        private void Close()
        {
            CloseRequestSubject.OnNext(Unit.Default);
        }

        public Subject<Unit> CloseRequestSubject = new Subject<Unit>();
        public IObservable<Unit> CloseRequest { get { return CloseRequestSubject; } } 
    }
}
