using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dantas.Core;
using System.Linq.Expressions;
using Spring.Transaction.Interceptor;

namespace Dantas.Data
{
    /// <summary>
    /// Base class for repositories.
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    public abstract class RepositoryBase<T> : IRepository<T> where T : IAggregateRoot
    {
        /// <summary>
        /// Overrides this for give the correct queryable object.
        /// </summary>
        protected abstract IQueryable<T> RepositoryQueryable { get; }

        #region IRepository<T> Members

        /// <summary>
        /// Save entity.
        /// </summary>
        /// <param name="item">Entity object.</param>
        [Transaction]
        public abstract void Save(T item);

        /// <summary>
        /// Remove entity.
        /// </summary>
        /// <param name="item">Entity object.</param>
        [Transaction]
        public abstract void Remove(T item);

        /// <summary>
        /// Find all objects with applied criteria by specification.
        /// </summary>
        /// <param name="spec">Specification element.</param>
        /// <returns>Repository with specification criteria.</returns>
        [Transaction]
        public virtual IQueryable<T> FindAll(Specification<T> spec)
        {
            return RepositoryQueryable.Where(spec.Predicate);
        }

        /// <summary>
        /// Find all objects with applied criteria by expression.
        /// </summary>
        /// <param name="predicate">Expression element.</param>
        /// <returns>Repository with specification criteria.</returns>
        [Transaction]
        public virtual IQueryable<T> FindAll(Expression<Func<T, bool>> predicate)
        {
            return RepositoryQueryable.Where(predicate);
        }

        /// <summary>
        /// Find all objects with no specification or criteria.
        /// </summary>
        /// <returns>Queryable with all elements.</returns>
        [Transaction]
        public virtual IQueryable<T> FindAll()
        {
            return RepositoryQueryable;
        }

        /// <summary>
        /// Get an object by id.
        /// </summary>
        /// <param name="id">Provide id for the object.</param>
        /// <returns>An instance with provided id.</returns>
        [Transaction]
        public virtual T FindOne(object id)
        {
            return FindOne(e => e.Id == id);
        }

        /// <summary>
        /// Get an object by expression.
        /// </summary>
        /// <param name="predicate">Expression element.</param>
        /// <returns>An instance with provided id.</returns>
        [Transaction]
        public virtual T FindOne(Expression<Func<T, bool>> predicate)
        {
            return RepositoryQueryable.Where(predicate).SingleOrDefault();
        }

        /// <summary>
        /// Get an object by specification.
        /// </summary>
        /// <param name="spec">Specification element.</param>
        /// <returns>An instance with provided id.</returns>
        [Transaction]
        public virtual T FindOne(Specification<T> spec)
        {
            return RepositoryQueryable.Where(spec.Predicate).SingleOrDefault();
        }

        /// <summary>
        /// Save entity and validate using IAutoValidable strategy.
        /// </summary>
        /// <param name="item">Entity object.</param>
        [Transaction]
        public virtual void SaveWithValidate(T item)
        {
            SaveWithValidate(item, null);
        }

        /// <summary>
        /// Save entity and validate using IAutoValidable strategy.
        /// </summary>
        /// <param name="item">Entity object.</param>
        /// <param name="ruleSet">Ruleset name.</param>
        [Transaction]
        public virtual void SaveWithValidate(T item, string ruleSet = null)
        {
            var obj = item as IAutoValidable;
            if (obj == null)
                throw new InvalidOperationException("The item needs IAutoValidable interface.");

            obj.Validate(ruleSet);

            Save(item);
        }

        /// <summary>
        /// Get current count of records on repository.
        /// </summary>
        /// <returns>Total integer size of repository.</returns>
        [Transaction]
        public virtual int Count()
        {
            return RepositoryQueryable.Count();
        }

        #endregion
    }
}
