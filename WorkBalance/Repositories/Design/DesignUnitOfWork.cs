using WorkBalance.Infrastructure;

namespace WorkBalance.Repositories.Db4o
{
    public class DesignUnitOfWork : IUnitOfWork
    {
        #region Implementation of IUnitOfWork

        public void Dispose()
        {
        }

        public void Commit()
        {
        }

        #endregion
    }
}