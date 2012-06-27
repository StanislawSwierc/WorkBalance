using System;
using System.Collections.Generic;
using System.Linq;
using WorkBalance.Infrastructure;

namespace WorkBalance.Domain
{
    public interface IDomainFactory
    {
        IUnitOfWork CreateUnitOfWork();

        IRepository<Activity> CreateActivityRepository();
        IRepository<Activity> CreateActivityRepository(IUnitOfWork unitOfWork);

        IRepository<Sprint> CreateSprintRepository();
        IRepository<Sprint> CreateSprintRepository(IUnitOfWork unitOfWork);

        IRepository<ActivityTag> CreateActivityTagRepository();
        IRepository<ActivityTag> CreateActivityTagRepository(IUnitOfWork unitOfWork);
    }
}
