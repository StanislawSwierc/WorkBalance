using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using WorkBalance.Infrastructure;

namespace WorkBalance.Repositories.Design
{
    public abstract class DesignRepository<TEntity> : IRepository<TEntity>
    {
        protected abstract TEntity CreateInstance();
        protected IEnumerable<TEntity> CreateInstances()
        {
            return Enumerable.Range(0, 10).Select(i => CreateInstance());
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
