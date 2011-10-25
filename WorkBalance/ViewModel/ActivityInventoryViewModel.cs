using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.ComponentModel;
using System.Windows.Data;
using WorkBalance.Domain;
using WorkBalance.Repositories;
using WorkBalance.Utilities;
using System.ComponentModel.Composition;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;

namespace WorkBalance.ViewModel
{
    [Export]
    public class ActivityInventoryViewModel: ViewModelBase
    {
        [ImportingConstructor]
        public ActivityInventoryViewModel(IMessenger messenger, IActivityRepository activityRepository)
            : base(messenger)
        {
            m_ActivityRepository = activityRepository;
            Activities = new ObservableCollection<Activity>(activityRepository.GetActive());
            MessengerInstance.Register<Activity>(this, Notifications.ActivityCreated, Activities.Add);
            SelectActivityCommand = new RelayCommand<Activity>(SelectActivity);
        }

        IActivityRepository m_ActivityRepository;
        public ObservableCollection<Activity> Activities { get; private set; }
        public RelayCommand<Activity> SelectActivityCommand { get; private set; }

        private void SelectActivity(Activity activity)
        {
            if (activity != null)
            {
                MessengerInstance.Send(Notifications.ActivitySelected, activity);
            }
        }
    }
}
