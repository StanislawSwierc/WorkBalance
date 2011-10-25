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

        [ImportingConstructor]
        public CreateActivityViewModel()
        {
            SaveCommand = new RelayCommand(Save, CanExecuteSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        public string Name { get; set; }
        public string ExpectedEffort { get; set; }
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
            string[] tagNames = string.IsNullOrWhiteSpace(Tags) ? new string[0] :
                Tags.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Convert tag names to real tags stored in the database
            var tags = tagNames.Select(name =>
            {
                var tag = ActivityTagRepository.Get(t => t.Name == name).SingleOrDefault();
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
            MessageBus.SendMessage(activity, Notifications.ActivitySelected);
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
