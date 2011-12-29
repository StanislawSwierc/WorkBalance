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
            m_Activities = new List<Activity>(activityRepository.GetActive());
            m_ActivitiesView = new ListCollectionView(m_Activities);
        }

        IActivityRepository m_ActivityRepository;
        List<Activity> m_Activities;
        ListCollectionView m_ActivitiesView;
        public ICollectionView Activities { get { return m_ActivitiesView; } }
        public List<Activity> DesignActivities { get { return m_Activities; } }
    }
}
