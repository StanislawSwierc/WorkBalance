using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using WorkBalance.Repositories;
using WorkBalance.Domain;
using System.Collections.ObjectModel;
using WorkBalance.Utilities;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Collections.Specialized;
using System.Reactive;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace WorkBalance.ViewModel
{
    [Export]
    public class HistoryViewModel : ViewModelBase, IPartImportsSatisfiedNotification
    {
        #region Imports

        [Import]
        public IDomainContext DomainContext { get; set; }

        public void OnImportsSatisfied()
        {
        }

        #endregion

        #region Constructors

        public HistoryViewModel()
        {
            Activities = new ObservableCollection<Activity>();
            _DatesFilterCollectionChanged = new EventHandlerSubject<NotifyCollectionChangedEventArgs>();
            
            var DatesFilterChanged = _DatesFilterCollectionChanged
                .Select(e => Unit.Default)
                // Throttle for a moment to wait for the collection to become stable
                .Throttle(TimeSpan.FromMilliseconds(10));

            var NameFilterChanged = this.ObservableForProperty(self => self.NameFilter)
                .Select(e => Unit.Default)
                .Throttle(TimeSpan.FromMilliseconds(200));

            var dynamicFilterEnabled = this.ObservableForProperty(self => self.DynamicFilterEnabled)
                .Where(e => e.Value == true)
                .Select(e => Unit.Default);

            var filtersChanged = Observable.Merge(
                DatesFilterChanged,
                NameFilterChanged,
                dynamicFilterEnabled);

            filtersChanged
                .Where(a => this.DynamicFilterEnabled)
                .ObserveOnDispatcher()
                .Subscribe(unit => Filter());

            FilterCommand = new ReactiveCommand(
                this.ObservableForProperty(self => self.DynamicFilterEnabled)
                .Select(e => !e.Value), System.Reactive.Concurrency.DispatcherScheduler.Instance);
                
            FilterCommand
                .ObserveOnDispatcher()
                .Subscribe(unit => Filter());
        }

        #endregion

        #region Members
        
        EventHandlerSubject<NotifyCollectionChangedEventArgs> _DatesFilterCollectionChanged;

        #endregion

        #region Properties

        public ReactiveCommand FilterCommand { get; private set; }

        public ObservableCollection<Activity> Activities { get; private set; }

        private bool _DynamicFilterEnabled;
        public bool DynamicFilterEnabled
        {
            get { return _DynamicFilterEnabled; }
            set { this.RaiseAndSetIfChanged(self => self.DynamicFilterEnabled, value); }
        }

        private ObservableCollection<DateTime> _DatesFilter;
        public ObservableCollection<DateTime> DatesFilter {
            get { return _DatesFilter; }
            set
            {
                if (_DatesFilter != value)
                {
                    if(_DatesFilter != null) _DatesFilter.CollectionChanged -= _DatesFilterCollectionChanged.Handler;
                    _DatesFilter = value;
                    _DatesFilter.CollectionChanged += _DatesFilterCollectionChanged.Handler;
                    
                    // Push one item to make the initial selection
                    _DatesFilterCollectionChanged.OnNext(new EventPattern<NotifyCollectionChangedEventArgs>(_DatesFilter, null));
                }
            }
        }

        private string _NameFilter;
        public string NameFilter
        {
            get { return _NameFilter; }
            set { this.RaiseAndSetIfChanged(self => self.NameFilter, value); }
        }

        #endregion

        #region Methods

        private void Filter()
        {
            Activities.Clear();
            if (DatesFilter.Count > 0)
            {
                DomainContext.Activities.Where(
                    a => DatesFilter.Contains(a.CreationTime.Date) &&
                        (string.IsNullOrWhiteSpace(NameFilter) || a.Name.Contains(NameFilter))
                    ).ForEach(Activities.Add);
            }
        }

        #endregion
    }
}
