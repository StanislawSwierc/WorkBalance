using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Linq;
using System.Text;
using WorkBalance.Infrastructure.Ef;

namespace WorkBalance.Domain.Ef
{
    public class EfDomainContext : EfObjectContext, IDomainContext
    {
        public DbSet<Activity> ActivitiesDbSet { get; set; }

        public DbSet<ActivityTag> ActivityTagsDbSet { get; set; }

        public DbSet<Sprint> SprintsDbSet { get; set; }

        #region Implementation of IDomainContext

        public Infrastructure.IObjectSet<Activity> Activities { get { return new EfObjectSet<Activity>(ActivitiesDbSet); } }

        public Infrastructure.IObjectSet<ActivityTag> ActivityTags { get { return new EfObjectSet<ActivityTag>(ActivityTagsDbSet); } }

        public Infrastructure.IObjectSet<Sprint> Sprints { get { return new EfObjectSet<Sprint>(SprintsDbSet); } }

        #endregion
    }
}
