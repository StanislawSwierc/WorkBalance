using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace WorkBalance.Infrastructure.Ef
{
    public class EfObjectSet<T> : IObjectSet<T> where T : class
    {
        private readonly DbSet<T> _internal;

        public EfObjectSet(DbSet<T> dbSet)
        {
            Contract.Requires<ArgumentNullException>(dbSet != null, "dbSet");

            _internal = dbSet;
        }

        #region Implementation of IEnumerable

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_internal).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_internal).GetEnumerator();
        }

        #endregion

        #region Implementation of IQueryable

        public Expression Expression
        {
            get { return ((IQueryable<T>)_internal).Expression; }
        }

        public Type ElementType
        {
            get { return ((IQueryable<T>)_internal).ElementType; }
        }

        public IQueryProvider Provider
        {
            get { return ((IQueryable<T>)_internal).Provider; }
        }

        #endregion

        #region Implementation of IObjectSet<T>

        public void AddObject(T entity)
        {
            _internal.Add(entity);
        }

        public void Attach(T entity)
        {
            _internal.Attach(entity);
        }

        public void DeleteObject(T entity)
        {
            _internal.Remove(entity);
        }

        public void Detach(T entity)
        {
            throw new InvalidOperationException();
        }

        #endregion

        #region Implementation of IObjectSet<T>

        public void Add(T entity)
        {
            AddObject(entity);
        }

        public void Update(T entity)
        {
        }

        #endregion
    }
}
