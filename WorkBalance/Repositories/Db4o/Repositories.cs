using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkBalance.Domain;
using Db4objects.Db4o;
using System.ComponentModel.Composition;

namespace WorkBalance.Repositories.Db4o
{
    [Export(typeof(IActivityRepository))]
    public class ActivityRepository : Db4oRepository<Activity>, IActivityRepository
    {
        [ImportingConstructor]
        public ActivityRepository(IObjectContainer container)
            : base(container)
        {
        }

        public IEnumerable<Activity> GetActive()
        {
            return Get().Where(a => !a.Archived);
        }
    }

    [Export(typeof(IActivityTagRepository))]
    public class ActivityTagRepository : Db4oRepository<ActivityTag>, IActivityTagRepository
    {
        [ImportingConstructor]
        public ActivityTagRepository(IObjectContainer container)
            : base(container)
        {
        }
    }

    [Export(typeof(IInterruptionRepository))]
    public class InterruptionRepository : Db4oRepository<Interruption>, IInterruptionRepository
    {
        [ImportingConstructor]
        public InterruptionRepository(IObjectContainer container)
            : base(container)
        {
        }
    }

    [Export(typeof(IInterruptionRecordRepository))]
    public class InterruptionRecordRepository : Db4oRepository<InterruptionRecord>, IInterruptionRecordRepository
    {
        [ImportingConstructor]
        public InterruptionRecordRepository(IObjectContainer container)
            : base(container)
        {
        }
    }

    [Export(typeof(IInterruptionTagRepository))]
    public class InterruptionTagRepository : Db4oRepository<InterruptionTag>, IInterruptionTagRepository
    {
        [ImportingConstructor]
        public InterruptionTagRepository(IObjectContainer container)
            : base(container)
        {
        }
    }

    [Export(typeof(ISprintRepository))]
    public class SprintRepository : Db4oRepository<Sprint>, ISprintRepository
    {
        [ImportingConstructor]
        public SprintRepository(IObjectContainer container)
            : base(container)
        {
        }
    }
}
