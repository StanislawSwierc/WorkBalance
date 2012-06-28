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
        private readonly Db4oUnitOfWork _unitOfWork;

        public Db4oRepository(Db4oUnitOfWork unitOfWork)
        {
            Contract.Requires<ArgumentNullException>(unitOfWork != null, "unitOfWork");

            _unitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        public IQueryable<TEntity> Get()
        {
            return _unitOfWork.Container.Query<TEntity>().AsQueryable();
        }

        public void Add(TEntity entity)
        {
            _unitOfWork.Container.Store(entity);
        }

        public void Delete(TEntity entity)
        {
            _unitOfWork.Container.Delete(entity);
        }

        public void Update(TEntity entity)
        {
            _unitOfWork.Container.Store(entity);
        }
    }
}
