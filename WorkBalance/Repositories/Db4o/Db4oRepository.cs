using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using System.Diagnostics.Contracts;


namespace WorkBalance.Repositories.Db4o
{
    public class Db4oRepository<TEntity> :IRepository<TEntity>
    {
        IObjectContainer m_Container;

        public Db4oRepository(IObjectContainer container)
        {
            Contract.Requires(container != null);

            m_Container = container;
        }

        public IEnumerable<TEntity> Get(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return m_Container.AsQueryable<TEntity>().Where(predicate);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return m_Container.Query<TEntity>();
        }

        public void Add(TEntity entity)
        {
            m_Container.Store(entity);
            m_Container.Commit();
        }

        public void Delete(TEntity entity)
        {
            m_Container.Delete(entity);
            m_Container.Commit();
        }

        public void Update(TEntity entity)
        {
            m_Container.Store(entity);
            m_Container.Commit();
        }
    }
}
