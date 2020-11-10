using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dantas.Core;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Criterion;
using Spring.Transaction.Interceptor;
using System.Linq.Expressions;

namespace Dantas.Data.NHibernate
{
    /// <summary>
    /// Extends repository funcionality for NHibernate.
    /// </summary>
    /// <typeparam name="T">Type of entity.</typeparam>
    public class NHibernateRepository<T> : RepositoryBase<T> where T : IAggregateRoot
    {
        /// <summary>
        /// Injected NHibernate data component.
        /// </summary>
        private INHibernateDao<T> dao;

        /// <summary>
        /// Overrides this method for add a default fetch strategy on repository.
        /// </summary>
        /// <param name="queryable">This repository.</param>
        /// <returns>Repository with strategy for use on find methods.</returns>
        protected virtual IQueryable<T> AddDefaultFetchStrategy(IQueryable<T> queryable)
        {
            return queryable;
        }

        /// <summary>
        /// Get NHibernate Queryable.
        /// </summary>
        protected override IQueryable<T> RepositoryQueryable
        {
            get { return AddDefaultFetchStrategy(dao.HibernateTemplate.SessionFactory.GetCurrentSession().Query<T>()); }
        }

        /// <summary>
        /// Current NHibernate Session.
        /// </summary>
        protected ISession Session
        {
            get { return dao.HibernateTemplate.SessionFactory.GetCurrentSession(); }
        }

        /// <summary>
        /// Create instance.
        /// </summary>
        public NHibernateRepository() : this(new NHibernateDao<T>())
        {    
        }

        /// <summary>
        /// Create instance.
        /// </summary>
        /// <param name="dao">Data component.</param>
        public NHibernateRepository(INHibernateDao<T> dao)
        {
            this.dao = dao;
        }

        /// <summary>
        /// Save entity.
        /// </summary>
        /// <param name="item">Entity object.</param>
        public override void Save(T item)
        {
            dao.Save(item);
        }

        /// <summary>
        /// Remove entity.
        /// </summary>
        /// <param name="item">Entity object.</param>
        public override void Remove(T item)
        {
            dao.Delete(item);
        }

    }
}
