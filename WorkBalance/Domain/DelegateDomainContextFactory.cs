using System;

namespace WorkBalance.Domain
{
    public class DelegateDomainContextFactory : IDomainContextFactory
    {
        private readonly Func<IDomainContext> _create;

        public DelegateDomainContextFactory(Func<IDomainContext> create)
        {
            if (create == null) throw new ArgumentNullException("create");
            _create = create;
        }

        public IDomainContext Create()
        {
            return _create.Invoke();
        }
    }
}