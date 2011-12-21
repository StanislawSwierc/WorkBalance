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

namespace WorkBalance.ViewModel
{
    [Export]
    public class HistoryViewModel : ViewModelBase, IPartImportsSatisfiedNotification
    {
        #region Imports

        [Import]
        public IActivityRepository ActivityRepository { get; set; }

        public void OnImportsSatisfied()
        {
        }

        #endregion

        #region Constructors

        public HistoryViewModel()
        {
            Activities = new ObservableCollection<Activity>();
            _DatesFilterCollectionChanged = new EventHandlerSubject<NotifyCollectionChangedEventArgs>();
            _DatesFilterChanged = _DatesFilterCollectionChanged.Select(e => (Collection<DateTime>)e.Sender);
            _DatesFilterChanged
                // Throttle for a moment to wait for the collection to become stable
                .Throttle(TimeSpan.FromMilliseconds(10))
                .ObserveOnDispatcher()
                .Subscribe(c => 
                {
                    Activities.Clear();
                    if (c.Count > 0) ActivityRepository.Get(a => c.Contains(a.CreationTime.Date)).ForEach(Activities.Add); 
                });
        }

        #endregion

        EventHandlerSubject<NotifyCollectionChangedEventArgs> _DatesFilterCollectionChanged;
        private IObservable<Collection<DateTime>> _DatesFilterChanged;

        public ObservableCollection<Activity> Activities { get; private set; }

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
    }
}
