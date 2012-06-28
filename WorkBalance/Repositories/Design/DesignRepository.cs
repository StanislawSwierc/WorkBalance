using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using WorkBalance.Infrastructure;
using WorkBalance.Repositories.Db4o;

namespace WorkBalance.Repositories.Design
{
    public class DesignRepository<TEntity> : IRepository<TEntity>
    {
        private readonly Func<TEntity> _entityFactory;

        public DesignRepository(Func<TEntity> entityFactory)
        {
            _entityFactory = entityFactory;
        }

        protected IEnumerable<TEntity> CreateInstances()
        {
            return Enumerable.Range(0, 10).Select(i => _entityFactory());
        }

        public IUnitOfWork UnitOfWork
        {
            get { return new DesignUnitOfWork();}
        }

        public IQueryable<TEntity> Get()
        {
            return CreateInstances().AsQueryable();
        }

        public void Add(TEntity entity)
        {
        }

        public void Delete(TEntity entity)
        {
        }

        public void Update(TEntity entity)
        {
        }
    }
}
