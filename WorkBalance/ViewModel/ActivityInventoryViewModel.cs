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
using System.ComponentModel.Composition;
using System.Collections.ObjectModel;

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
            MessengerInstance.Register<NotificationMessage<Activity>>(this, HandleActivityCreated);
        }

        IActivityRepository m_ActivityRepository;
        public ObservableCollection<Activity> Activities { get; private set; }

        public void HandleActivityCreated(NotificationMessage<Activity> notification)
        {
            if (notification.Notification == Notifications.ActivityCreated)
            {
                Activities.Add(notification.Content);
            }
        }
    }
}
