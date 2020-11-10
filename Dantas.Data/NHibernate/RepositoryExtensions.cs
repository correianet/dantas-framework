using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;
using System.Linq.Expressions;
using Dantas.Core;

namespace Dantas.Data.NHibernate
{
    /// <summary>
    /// Extensions for IRepository.
    /// </summary>
    public static class RepositoryExtensions
    {
        /// <summary>
        /// Enable eager loading for property.
        /// </summary>
        /// <typeparam name="TOriginating">Entity type.</typeparam>
        /// <typeparam name="TRelated">Property type.</typeparam>
        /// <param name="query">Repository.</param>
        /// <param name="expression">Property to eager loading.</param>
        /// <returns>Repository with fecth strategy.</returns>
        public static INhFetchRequest<TOriginating, TRelated> Fetch<TOriginating, TRelated>(this IRepository<TOriginating> query, Expression<Func<TOriginating, TRelated>> expression) where TOriginating : IAggregateRoot
        {
            return ((IQueryable<TOriginating>)query).Fetch(expression);
        }

        /// <summary>
        /// Enable eager loading for property.
        /// </summary>
        /// <typeparam name="TOriginating">Entity type.</typeparam>
        /// <typeparam name="TRelated">Property type.</typeparam>
        /// <param name="query">Repository.</param>
        /// <param name="expression">Property to eager loading.</param>
        /// <returns>Repository with fecth strategy.</returns>
        public static INhFetchRequest<TOriginating, TRelated> FetchMany<TOriginating, TRelated>(this IRepository<TOriginating> query, Expression<Func<TOriginating, IEnumerable<TRelated>>> expression) where TOriginating : IAggregateRoot
        {
            return ((IQueryable<TOriginating>)query).FetchMany(expression);
        }
    }
}
