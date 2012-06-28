using System;
using System.Diagnostics.Contracts;
using System.Linq;
using Db4objects.Db4o;
using WorkBalance.Domain;
using WorkBalance.Infrastructure;
using WorkBalance.Repositories.Design;
using WorkBalance.Utilities;

namespace WorkBalance.Repositories.Db4o
{
    [DesignTimeExport(typeof(IDomainFactory), DesignTime = true)]
    public class DesignDomainFactory : IDomainFactory
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

        #region Implementation of IDomainFactory

        IUnitOfWork IDomainFactory.CreateUnitOfWork()
        {
            return new DesignUnitOfWork();
        }

        public IRepository<Activity> CreateActivityRepository()
        {
            return new DesignRepository<Activity>(CreateActivity); 
        }

        public IRepository<Activity> CreateActivityRepository(IUnitOfWork unitOfWork)
        {
            return new DesignRepository<Activity>(CreateActivity); 
        }

        public IRepository<Sprint> CreateSprintRepository()
        {
            return new DesignRepository<Sprint>(CreateSprint); 
        }

        public IRepository<Sprint> CreateSprintRepository(IUnitOfWork unitOfWork)
        {
            return new DesignRepository<Sprint>(CreateSprint); 
        }

        public IRepository<ActivityTag> CreateActivityTagRepository()
        {
            return new DesignRepository<ActivityTag>(CreateActivityTag); 
        }

        public IRepository<ActivityTag> CreateActivityTagRepository(IUnitOfWork unitOfWork)
        {
            return new DesignRepository<ActivityTag>(CreateActivityTag); 
        }

        #endregion
    }
}
