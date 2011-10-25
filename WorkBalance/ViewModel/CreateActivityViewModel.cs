using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using WorkBalance.Repositories;
using System.ComponentModel.Composition;
using WorkBalance.Domain;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using WorkBalance.Utilities;

namespace WorkBalance.ViewModel
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CreateActivityViewModel : ViewModelBase
    {
        private IActivityRepository m_ActivityRepository;
        private IActivityTagRepository m_ActivityTagRepository;

        [ImportingConstructor]
        public CreateActivityViewModel(IMessenger messenger, IActivityRepository activityRepository, IActivityTagRepository activityTagRepository)
            : base(messenger)
        {
            m_ActivityRepository = activityRepository;
            m_ActivityTagRepository = activityTagRepository;
            SaveCommand = new RelayCommand(Save, CanExecuteSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        public string Name { get; set; }
        public int ExpectedEffort { get; set; }
        public string Tags { get; set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        public bool CanExecuteSave()
        {
            //return !string.IsNullOrWhiteSpace(Activity.Name);
            return true;
        }

        public void Save()
        {
            string[] tags = string.IsNullOrWhiteSpace(Tags) ? new string[0] :
                Tags.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var activity = new Activity();
            activity.Name = Name;
            activity.ExpectedEffort = ExpectedEffort;
            
            m_ActivityRepository.Add(activity);
            MessengerInstance.Send(Notifications.ActivityCreated, activity);
            MessengerInstance.Send(Notifications.ActivitySelected, activity);
            Close();
        }

        public void Cancel()
        {
            Close();
        }

        private void Close()
        {
            MessengerInstance.Send<NotificationMessage>(new NotificationMessage(Notifications.CreateActivityWindowClose));
        }
    }
}
