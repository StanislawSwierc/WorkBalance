using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using Db4objects.Db4o;
using WorkBalance.Domain;
using WorkBalance.Infrastructure;

namespace WorkBalance.Repositories.Db4o
{
    public class Db4oObjectSet<T> : IObjectSet<T> where T : class
    {
        private readonly IObjectContainer _container;

        public Db4oObjectSet(Db4objects.Db4o.IObjectContainer container)
        {
            Contract.Requires<ArgumentNullException>(container != null, "container");

            _container = container;
        }

        #region Implementation of IEnumerable

        public IEnumerator<T> GetEnumerator()
        {
            return _container.Query<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of IQueryable

        public Expression Expression
        {
            get { return _container.Query<T>().AsQueryable().Expression; }
        }

        public Type ElementType
        {
            get { return _container.Query<T>().AsQueryable().ElementType; }
        }

        public IQueryProvider Provider
        {
            get { return _container.Query<T>().AsQueryable().Provider; }
        }

        #endregion

        #region Implementation of IObjectSet<T>

        public void AddObject(T entity)
        {
            _container.Store(entity);
        }

        public void Attach(T entity)
        {
            _container.Store(entity);
        }

        public void DeleteObject(T entity)
        {
            _container.Delete(entity);
        }

        public void Detach(T entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Implementation of IEntitySet<T>

        public void Add(T entity)
        {
            _container.Store(entity);
        }

        public void Update(T entity)
        {
            _container.Store(entity);
        }

        /// <remarks>
        /// In Db4o the object graph is five levels deep by default.
        /// </remarks>
        public IQueryable<T> FetchWith<TProperty>(Expression<Func<T, TProperty>> path)
        {
            return _container.Query<T>().AsQueryable();
        }

        #endregion
    }
}