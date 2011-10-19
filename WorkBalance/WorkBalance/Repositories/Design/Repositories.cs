using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkBalance.Domain;

namespace WorkBalance.Repositories.Design
{
    public class ActivityRepository : DesignRepository<Activity>, IActivityRepository
    {
    }

    public class ActivityTagRepository : DesignRepository<ActivityTag>, IActivityTagRepository
    {
    }

    public class InterruptionRepository : DesignRepository<Interruption>, IInterruptionRepository
    {
    }

    public class InterruptionRecordRepository : DesignRepository<InterruptionRecord>, IInterruptionRecordRepository
    {
    }

    public class InterruptionTagRepository : DesignRepository<InterruptionTag>, IInterruptionTagRepository
    {
    }

    public class SprintRepository : DesignRepository<Sprint>, ISprintRepository
    {
    }
}
