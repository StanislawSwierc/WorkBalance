using System;
using System.Diagnostics.Contracts;
using Db4objects.Db4o;
using WorkBalance.Infrastructure;

namespace WorkBalance.Repositories.Db4o
{
    public class Db4oUnitOfWork : IUnitOfWork
    {
        private readonly IObjectContainer _container;

        public Db4oUnitOfWork(IObjectContainer container)
        {
            Contract.Requires<ArgumentNullException>(container != null, "container");

            _container = container;
        }

        public IObjectContainer Container
        {
            get { return _container; }
        }

        #region Implementation of IUnitOfWork

        public void Commit()
        {
            Container.Commit();
        }

        public void Dispose()
        {
            if (Container != null)
            {
                Container.Dispose();    
            }
        }

        #endregion
    }
}