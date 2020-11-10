using System;
using NHibernate;
using Spring.Data.NHibernate.Generic;
namespace Dantas.Data.NHibernate
{
    /// <summary>
    /// Implements this for custom Dao.
    /// </summary>
    /// <typeparam name="T">Type of entity.</typeparam>
    public interface INHibernateDao<T>
    {
        /// <summary>
        /// Exposed Template.
        /// </summary>
        HibernateTemplate HibernateTemplate { get; }

        /// <summary>
        /// Delete entity.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        void Delete(T entity);

        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <returns>List of all objects.</returns>
        System.Collections.Generic.IList<T> FindAll();

        /// <summary>
        /// Get object by id.
        /// </summary>
        /// <typeparam name="TId">Id type.</typeparam>
        /// <param name="id">Id object.</param>
        /// <returns>Object loaded.</returns>
        T FindById<TId>(TId id);

        /// <summary>
        /// Get object by criteria.
        /// </summary>
        /// <param name="criteria">Criteria element.</param>
        /// <returns>Object loaded.</returns>
        T FindByCriteria(ICriteria criteria);

        /// <summary>
        /// Save entity.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        void Save(T entity);
    }
}
