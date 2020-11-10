using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Dantas.Core
{
    /// <summary>
    /// Implements this for create application service with default functionality.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public interface IApplicationService<T, U>
        where T : class, IDataTransferObject
        where U : class, IAggregateRoot
    {
        /// <summary>
        /// Get an entity by id.
        /// </summary>
        /// <param name="id">Id parameter value.</param>
        /// <returns>An entity from repository.</returns>
        T FindBy(object id);

        /// <summary>
        /// Get an entity by expression condition.
        /// </summary>
        /// <param name="predicate">Expression to evaluate.</param>
        /// <returns>An entity from repository</returns>
        T FindBy(Expression<Func<U, bool>> predicate);

        /// <summary>
        /// Get an entity by specification.
        /// </summary>
        /// <param name="spec">Specification condition.</param>
        /// <returns>An entity from repository.</returns>
        T FindBy(Specification<U> spec);

        /// <summary>
        /// Read all data from repository.
        /// </summary>
        /// <returns>Resultset with records getted from repository.</returns>
        DataResult<T> ReadAll();

        /// <summary>
        /// Read data from repository with pagination and ordering information.
        /// </summary>
        /// <param name="pageInfo">Paging and ordering information.</param>
        /// <returns>Resultset with records getted from repository.</returns>
        DataResult<T> ReadAll(PageInfo pageInfo);

        /// <summary>
        /// Read data from repository with pagination and ordering information.
        /// </summary>
        /// <param name="expression">Filter criteria.</param>
        /// <param name="pageInfo">Paging and ordering information.</param>
        /// <returns>Resultset with records getted from repository.</returns>
        DataResult<T> ReadAll(Expression<Func<U, bool>> expression, PageInfo pageInfo = null);

        /// <summary>
        /// Read data from repository with pagination and ordering information.
        /// </summary>
        /// <param name="spec">Filter criteria.</param>
        /// <param name="pageInfo">Paging and ordering information.</param>
        /// <returns>Resultset with records getted from repository.</returns>
        DataResult<T> ReadAll(Specification<U> spec, PageInfo pageInfo = null);

        /// <summary>
        /// Read data from repository with pagination and ordering information.
        /// </summary>
        /// <param name="filter">Filter expression.</param>
        /// <param name="filterValues">Filter values in expression.</param>
        /// <param name="pageInfo">Paging and ordering information.</param>
        /// <returns>Resultset with records getted from repository.</returns>
        DataResult<T> ReadAll(string filter, object[] filterValues, PageInfo pageInfo = null);

        /// <summary>
        /// Read data from repository with pagination and ordering information.
        /// </summary>
        /// <param name="expression">Filter criteria.</param>
        /// <param name="filter">Filter expression.</param>
        /// <param name="filterValues">Filter values in expression.</param>
        /// <param name="pageInfo">Paging and ordering information.</param>
        /// <returns>Resultset with records getted from repository.</returns>
        DataResult<T> ReadAll(Expression<Func<U, bool>> expression, string filter, object[] filterValues, PageInfo pageInfo = null);

        /// <summary>
        /// Read data from repository with pagination and ordering information.
        /// </summary>
        /// <param name="spec">Filter criteria.</param>
        /// <param name="filter">Filter expression.</param>
        /// <param name="filterValues">Filter values in expression.</param>
        /// <param name="pageInfo">Paging and ordering information.</param>
        /// <returns>Resultset with records getted from repository.</returns>
        DataResult<T> ReadAll(Specification<U> spec, string filter, object[] filterValues, PageInfo pageInfo = null);

        /// <summary>
        /// Read data from repository with ordering information.
        /// </summary>
        /// <param name="sorter">Sorting column.</param>
        /// <returns>Resultset with records getted from repository.</returns>
        DataResult<T> ReadAll(string sorter);

        /// <summary>
        /// Read data from repository with ordering information.
        /// </summary>
        /// <param name="sorter">Sorting column.</param>
        /// <param name="filter">Filter expression.</param>
        /// <param name="filterValues">Filter values in expression.</param>
        /// <returns>Resultset with records getted from repository.</returns>
        DataResult<T> ReadAll(string sorter, string filter, object[] filterValues);

        /// <summary>
        /// Read data from repository with ordering information.
        /// </summary>
        /// <param name="sorter">Sorting column.</param>
        /// <param name="expression">Filter criteria.</param>
        /// <returns>Resultset with records getted from repository.</returns>
        DataResult<T> ReadAll(string sorter, Expression<Func<U, bool>> expression);

        /// <summary>
        /// Create new entity.
        /// </summary>
        /// <param name="data">Data transfer object.</param>
        /// <param name="ruleSet">Ruleset name for validation code.</param>
        void CreateNew(T data, string ruleSet = null);

        /// <summary>
        /// Update entity.
        /// </summary>
        /// <param name="data">Data transfer object.</param>
        /// <param name="ruleSet">Ruleset name for validation code.</param>
        void UpdateWith(T data, string ruleSet = null);

        /// <summary>
        /// Delete entity.
        /// </summary>
        /// <param name="data">Data transfer object.</param>
        /// <param name="ruleSet">Ruleset name for validation code.</param>
        void DeleteFor(T data, string ruleSet = null);
    }
}
