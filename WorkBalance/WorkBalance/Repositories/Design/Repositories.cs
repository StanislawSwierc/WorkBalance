using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkBalance.Domain;

namespace WorkBalance.Repositories.Design
{
    public class ActivityRepository : DesignRepository<Activity>, IActivityRepository
    {
        public IEnumerable<Activity> GetActive()
        {
            return CreateInstances();
        }

        protected override Activity CreateInstance()
        {
            return new Activity()
            {
                Active = true,
                ActualEffort = 3,
                Completed = false,
                CreationTime = DateTime.Now,
                ExpectedEffort = 5,
                Name = "Sample Activity",
            };
        }
    }

    public class ActivityTagRepository : DesignRepository<ActivityTag>, IActivityTagRepository
    {
        protected override ActivityTag CreateInstance()
        {
            throw new NotImplementedException();
        }
    }

    public class InterruptionRepository : DesignRepository<Interruption>, IInterruptionRepository
    {
        protected override Interruption CreateInstance()
        {
            throw new NotImplementedException();
        }
    }

    public class InterruptionRecordRepository : DesignRepository<InterruptionRecord>, IInterruptionRecordRepository
    {
        protected override InterruptionRecord CreateInstance()
        {
            throw new NotImplementedException();
        }
    }

    public class InterruptionTagRepository : DesignRepository<InterruptionTag>, IInterruptionTagRepository
    {
        protected override InterruptionTag CreateInstance()
        {
            throw new NotImplementedException();
        }
    }

    public class SprintRepository : DesignRepository<Sprint>, ISprintRepository
    {
        protected override Sprint CreateInstance()
        {
            throw new NotImplementedException();
        }
    }
}
