using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkBalance.Domain;

namespace WorkBalance.Repositories
{
    public interface IActivityRepository : IRepository<Activity>
    {
    }

    public interface IActivityTagRepository : IRepository<ActivityTag>
    {
    }

    public interface IInterruptionRepository : IRepository<Interruption>
    {
    }

    public interface IInterruptionRecordRepository : IRepository<InterruptionRecord>
    {
    }

    public interface IInterruptionTagRepository : IRepository<InterruptionTag>
    {
    }

    public interface ISprintRepository : IRepository<Sprint>
    {
    }
}
