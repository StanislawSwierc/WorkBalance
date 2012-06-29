using System;

namespace WorkBalance.Infrastructure
{
    public interface IObjectContext : IDisposable
    {
        void Commit();
    }
}
