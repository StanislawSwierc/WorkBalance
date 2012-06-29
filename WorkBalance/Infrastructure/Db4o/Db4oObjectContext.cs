using System;
using System.Diagnostics.Contracts;
using Db4objects.Db4o;

namespace WorkBalance.Infrastructure.Db4o
{
    public class Db4oObjectContext : IObjectContext
    {
        public IObjectContainer Container { get; private set; }

        public Db4oObjectContext(IObjectContainer container)
        {
            Contract.Requires<ArgumentNullException>(container != null, "container");

            Container = container;
        }

        #region Implementation of IObjectContext

        public void Commit()
        {
            Container.Commit();
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            Container.Dispose();
        }

        #endregion
    }
}
