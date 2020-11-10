using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Data.NHibernate.Generic.Support;
using Dantas.Core;
using Spring.Transaction;
using Spring.Transaction.Interceptor;
using NHibernate;

namespace Dantas.Data.NHibernate
{
    /// <summary>
    /// Implements data component for NHibernate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NHibernateDao<T> : HibernateDaoSupport, INHibernateDao<T>
    {
        /// <summary>
        /// Get object by id.
        /// </summary>
        /// <typeparam name="TId">Id type.</typeparam>
        /// <param name="id">Id object.</param>
        /// <returns>Object loaded.</returns>
        public T FindById<TId>(TId id)
        {
            return HibernateTemplate.Load<T>(id);
        }

        /// <summary>
        /// Get object by criteria.
        /// </summary>
        /// <param name="criteria">Criteria element.</param>
        /// <returns>Object loaded.</returns>
        public T FindByCriteria(ICriteria criteria)
        {
            HibernateTemplate.PrepareCriteria(criteria);
            return criteria.UniqueResult<T>();
        }

        /// <summary>
        /// Get all objects.
        /// </summary>
        /// <returns>List of all objects.</returns>
        public IList<T> FindAll()
        {
            return HibernateTemplate.LoadAll<T>();
        }

        /// <summary>
        /// Save entity.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        [Transaction]
        public void Save(T entity)
        {
            HibernateTemplate.SaveOrUpdate(entity);
        }

        /// <summary>
        /// Delete entity.
        /// </summary>
        /// <param name="entity">Entity object.</param>
        [Transaction]
        public void Delete(T entity)
        {
            HibernateTemplate.Delete(entity);
        }
    }
}
