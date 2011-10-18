using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkBalance.Domain;
using Db4objects.Db4o;

namespace WorkBalance.Repositories.Db4o
{
    public class ActivityRepository : Db4oRepository<Activity>, IActivityRepository
    {
        public ActivityRepository(IObjectContainer container)
            :base(container)
        {
        }
    }

    public class ActivityTagRepository : Db4oRepository<ActivityTag>
    {
        public ActivityTagRepository(IObjectContainer container)
            :base(container)
        {
        }
    }

    public class InterruptionRepository : Db4oRepository<Interruption>
    {
        public InterruptionRepository(IObjectContainer container)
            : base(container)
        {
        }
    }

    public class InterruptionRecordRepository : Db4oRepository<InterruptionRecord>
    {
        public InterruptionRecordRepository(IObjectContainer container)
            : base(container)
        {
        }
    }

    public class InterruptionTagRepository : Db4oRepository<InterruptionTag>
    {
        public InterruptionTagRepository(IObjectContainer container)
            : base(container)
        {
        }
    }

    public class SprintRepository : Db4oRepository<Sprint>
    {
        public SprintRepository(IObjectContainer container)
            : base(container)
        {
        }
    }
}
