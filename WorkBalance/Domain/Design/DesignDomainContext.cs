using System;
using System.Linq;
using WorkBalance.Domain;
using WorkBalance.Infrastructure;
using WorkBalance.Infrastructure.Design;

namespace WorkBalance.Repositories.Design
{
    public class DesignDomainContext : DesignObjectContext, IDomainContext
    {
        #region Factory methods

        protected Activity CreateActivity()
        {
            var result = new Activity()
            {
                Archived = true,
                ActualEffort = 3,
                Completed = false,
                CreationTime = DateTime.Now,
                ExpectedEffort = 5,
                Name = "Sample Activity",
            };
            result.Sprints = Enumerable.Range(0, 10).Select(i => CreateSprint()).ToList();
            return result;
        }

        protected ActivityTag CreateActivityTag()
        {
            return new ActivityTag()
            {
                Name = "test"
            };
        }

        protected Sprint CreateSprint()
        {
            return new Sprint();
        }

        #endregion

        #region Implementation of IDomainContext

        public IObjectSet<Activity> Activities
        {
            get { return new DesignObjectSet<Activity>(CreateActivity); }
        }

        public IObjectSet<ActivityTag> ActivityTags
        {
            get { return new DesignObjectSet<ActivityTag>(CreateActivityTag); }
        }

        public IObjectSet<Sprint> Sprints
        {
            get { return new DesignObjectSet<Sprint>(CreateSprint); }
        }

        #endregion
    }
}
