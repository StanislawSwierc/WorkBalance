using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WorkBalance.Infrastructure
{
    public interface IRepository<TEntity>
    {
        IUnitOfWork UnitOfWork { get; }
        IQueryable<TEntity> Get();
        void Add(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity); 
    }
}
