using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WorkBalance.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }
}
