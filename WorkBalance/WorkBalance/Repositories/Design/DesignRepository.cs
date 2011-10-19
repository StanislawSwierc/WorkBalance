using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace WorkBalance.Repositories.Design
{
    public class DesignRepository<TEntity> : IRepository<TEntity>
    {
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> expression)
        {
            return Enumerable.Empty<TEntity>();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Enumerable.Empty<TEntity>();
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
