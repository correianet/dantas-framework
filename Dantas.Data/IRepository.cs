using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dantas.Core;
using System.Linq.Expressions;

namespace Dantas.Data
{
    /// <summary>
    /// Implements this for repositories.
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Save entity.
        /// </summary>
        /// <param name="item">Entity object.</param>
        void Save(T item);

        /// <summary>
        /// Save entity and validate using IAutoValidable strategy.
        /// </summary>
        /// <param name="item">Entity object.</param>
        void SaveWithValidate(T item);

        /// <summary>
        /// Save entity and validate using IAutoValidable strategy.
        /// </summary>
        /// <param name="item">Entity object.</param>
        /// <param name="ruleSet">Ruleset name.</param>
        void SaveWithValidate(T item, string ruleSet=null);

        /// <summary>
        /// Remove entity.
        /// </summary>
        /// <param name="item">Entity object.</param>
        void Remove(T item);

        /// <summary>
        /// Find all objects with applied criteria by specification.
        /// </summary>
        /// <param name="spec">Specification element.</param>
        /// <returns>Queryable with specification criteria.</returns>
        IQueryable<T> FindAll(Specification<T> spec);

        /// <summary>
        /// Find all objects with applied criteria by expression.
        /// </summary>
        /// <param name="predicate">Expression element.</param>
        /// <returns>Queryable with specification criteria.</returns>
        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Find all objects with no specification or criteria.
        /// </summary>
        /// <returns>Queryable with all elements.</returns>
        IQueryable<T> FindAll();

        /// <summary>
        /// Get an object by id.
        /// </summary>
        /// <param name="id">Provide id for the object.</param>
        /// <returns>An instance with provided id.</returns>
        T FindOne(object id);

        /// <summary>
        /// Get an object by expression.
        /// </summary>
        /// <param name="predicate">Expression element.</param>
        /// <returns>An instance with provided id.</returns>
        T FindOne(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Get an object by specification.
        /// </summary>
        /// <param name="spec">Specification element.</param>
        /// <returns>An instance with provided id.</returns>
        T FindOne(Specification<T> spec);

        /// <summary>
        /// Get current count of records on repository.
        /// </summary>
        /// <returns>Total integer size of repository.</returns>
        int Count();
    }
}
