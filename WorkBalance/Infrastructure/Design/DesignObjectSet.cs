using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WorkBalance.Infrastructure.Design
{
    public class DesignObjectSet<T> : IObjectSet<T> where T : class
    {
        private readonly Func<T> _entityFactory;

        public DesignObjectSet(Func<T> entityFactory)
        {
            _entityFactory = entityFactory;
        }

        public IEnumerable<T> GetEntities()
        {
            return Enumerable.Range(0, 10)
                .Select(i => _entityFactory());
        }

        #region Implementation of IEnumerable

        public IEnumerator<T> GetEnumerator()
        {
            return GetEntities().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of IQueryable

        public Expression Expression
        {
            get { return GetEntities().AsQueryable().Expression; }
        }

        public Type ElementType
        {
            get { return GetEntities().AsQueryable().ElementType; }
        }

        public IQueryProvider Provider
        {
            get { return GetEntities().AsQueryable().Provider; }
        }

        #endregion

        #region Implementation of IObjectSet<T>

        public void AddObject(T entity)
        {
        }

        public void Attach(T entity)
        {
        }

        public void DeleteObject(T entity)
        {
        }

        public void Detach(T entity)
        {
        }

        #endregion

        #region Implementation of IEntitySet<T>

        public void Add(T entity)
        {
        }

        public void Update(T entity)
        {
        }

        public IQueryable<T> FetchWith<TProperty>(Expression<Func<T, TProperty>> path)
        {
            return this;
        }

        #endregion
    }
}