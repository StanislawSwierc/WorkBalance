using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using System.Diagnostics.Contracts;
using WorkBalance.Infrastructure;


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

        public IQueryable<TEntity> Get()
        {
            return m_Container.Query<TEntity>().AsQueryable();
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
