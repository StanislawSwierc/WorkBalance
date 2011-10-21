using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkBalance.Domain;
using WorkBalance.Utilities;

namespace WorkBalance.Repositories.Design
{
    [DesignTimeExport(typeof(IActivityRepository), DesignTime = true)]
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
                Archived = true,
                ActualEffort = 3,
                Completed = false,
                CreationTime = DateTime.Now,
                ExpectedEffort = 5,
                Name = "Sample Activity",
            };
        }
    }

    [DesignTimeExport(typeof(IActivityTagRepository), DesignTime = true)]
    public class ActivityTagRepository : DesignRepository<ActivityTag>, IActivityTagRepository
    {
        protected override ActivityTag CreateInstance()
        {
            throw new NotImplementedException();
        }
    }

    [DesignTimeExport(typeof(IInterruptionRepository), DesignTime = true)]
    public class InterruptionRepository : DesignRepository<Interruption>, IInterruptionRepository
    {
        protected override Interruption CreateInstance()
        {
            throw new NotImplementedException();
        }
    }

    [DesignTimeExport(typeof(IInterruptionRecordRepository), DesignTime = true)]
    public class InterruptionRecordRepository : DesignRepository<InterruptionRecord>, IInterruptionRecordRepository
    {
        protected override InterruptionRecord CreateInstance()
        {
            throw new NotImplementedException();
        }
    }

    [DesignTimeExport(typeof(IInterruptionTagRepository), DesignTime = true)]
    public class InterruptionTagRepository : DesignRepository<InterruptionTag>, IInterruptionTagRepository
    {
        protected override InterruptionTag CreateInstance()
        {
            throw new NotImplementedException();
        }
    }

    [DesignTimeExport(typeof(ISprintRepository), DesignTime = true)]
    public class SprintRepository : DesignRepository<Sprint>, ISprintRepository
    {
        protected override Sprint CreateInstance()
        {
            throw new NotImplementedException();
        }
    }
}
