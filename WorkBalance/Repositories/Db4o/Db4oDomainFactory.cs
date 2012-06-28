using System;
using System.Diagnostics.Contracts;
using Db4objects.Db4o;
using WorkBalance.Domain;
using WorkBalance.Infrastructure;

namespace WorkBalance.Repositories.Db4o
{
    public class Db4oDomainFactory : IDomainFactory
    {
        private readonly IObjectServer _server;

        public Db4oDomainFactory(string path)
        {
            Contract.Requires<ArgumentNullException>(path != null, "path");

            _server = Db4oFactory.OpenServer(path, 0);
        }

        public Db4oUnitOfWork CreateUnitOfWork()
        {
            return new Db4oUnitOfWork(_server.OpenClient());
        }

        private static Db4oUnitOfWork ToDb4oUnitOfWork(IUnitOfWork unitOfWork)
        {
            var result = unitOfWork as Db4oUnitOfWork;
            if(result == null)
            {
                throw new ArgumentException("unitOfWork");
            }
            return result;
        }

        #region Implementation of IDomainFactory

        IUnitOfWork IDomainFactory.CreateUnitOfWork()
        {
            return new Db4oUnitOfWork(_server.OpenClient());
        }

        public IRepository<Activity> CreateActivityRepository()
        {
            return new Db4oRepository<Activity>(CreateUnitOfWork());
        }

        public IRepository<Activity> CreateActivityRepository(IUnitOfWork unitOfWork)
        {
            return new Db4oRepository<Activity>(ToDb4oUnitOfWork(unitOfWork));
        }

        public IRepository<Sprint> CreateSprintRepository()
        {
            return new Db4oRepository<Sprint>(CreateUnitOfWork());
        }

        public IRepository<Sprint> CreateSprintRepository(IUnitOfWork unitOfWork)
        {
            return new Db4oRepository<Sprint>(ToDb4oUnitOfWork(unitOfWork));
        }

        public IRepository<ActivityTag> CreateActivityTagRepository()
        {
            return new Db4oRepository<ActivityTag>(CreateUnitOfWork());
        }

        public IRepository<ActivityTag> CreateActivityTagRepository(IUnitOfWork unitOfWork)
        {
            return new Db4oRepository<ActivityTag>(ToDb4oUnitOfWork(unitOfWork));
        }

        #endregion
    }
}
