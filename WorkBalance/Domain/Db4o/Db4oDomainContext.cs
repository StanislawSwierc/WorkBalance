using System;
using System.Diagnostics.Contracts;
using Db4objects.Db4o;
using WorkBalance.Infrastructure;
using WorkBalance.Infrastructure.Db4o;
using WorkBalance.Repositories.Db4o;

namespace WorkBalance.Domain.Db4o
{
    public class Db4oDomainContext : Db4oObjectContext, IDomainContext
    {
        public Db4oDomainContext(IObjectContainer container)
            :base(container)
        {
        }
        
        #region Implementation of IDomainContext

        private IObjectSet<Activity> _activities;
        public IObjectSet<Activity> Activities
        {
            get
            {
                if (_activities == null)
                {
                    _activities = new Db4oObjectSet<Activity>(Container);
                }
                return _activities;
            }
        }

        private IObjectSet<ActivityTag> _activityTags;
        public IObjectSet<ActivityTag> ActivityTags
        {
            get
            {
                if (_activityTags == null)
                {
                    _activityTags = new Db4oObjectSet<ActivityTag>(Container);
                }
                return _activityTags;
            }
        }

        private IObjectSet<Sprint> _sprints;
        public IObjectSet<Sprint> Sprints
        {
            get
            {
                if (_sprints == null)
                {
                    _sprints = new Db4oObjectSet<Sprint>(Container);
                }
                return _sprints;
            }
        }

        #endregion
    }
}
