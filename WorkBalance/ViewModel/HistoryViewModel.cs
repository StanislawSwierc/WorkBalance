using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using WorkBalance.Repositories;
using WorkBalance.Domain;
using System.Collections.ObjectModel;
using WorkBalance.Utilities;

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
            ActivityRepository.GetAll().Where(a => a.Sprints.Count > 0).ForEach(Activities.Add);
        }

        #endregion

        #region Constructors

        public HistoryViewModel()
        {
            Activities = new ObservableCollection<Activity>();
        }

        #endregion

        public ObservableCollection<Activity> Activities { get; private set; }
    }
}
