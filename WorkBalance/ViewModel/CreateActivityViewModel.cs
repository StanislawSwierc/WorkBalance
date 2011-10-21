using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using WorkBalance.Repositories;
using System.ComponentModel.Composition;
using WorkBalance.Domain;

namespace WorkBalance.ViewModel
{
    [Export]
    public class CreateActivityViewModel : ViewModelBase
    {
        private IActivityRepository m_ActivityRepository;
        private IActivityTagRepository m_ActivityTagRepository;

        public CreateActivityViewModel(IMessenger messenger, IActivityRepository activityRepository, IActivityTagRepository activityTagRepository)
        {
            m_ActivityRepository = activityRepository;
            m_ActivityTagRepository = activityTagRepository;
        }

        public string Name{ get; set; }
        public int ExpectedEffort { get; set; }
        public string Tags { get; set; }

    }
}
