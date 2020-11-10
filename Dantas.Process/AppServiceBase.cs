using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dantas.Data;
using Dantas.Core.Util;
using AutoMapper;
using Dantas.Core;
using Dantas.Data.Util;
using Spring.Transaction.Interceptor;
using Dantas.Support;
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace Dantas.Process
{
    /// <summary>
    /// Application service for coordinate transitions of states at domain.
    /// </summary>
    /// <typeparam name="T">Data transfer object.</typeparam>
    /// <typeparam name="U">Entity.</typeparam>
    public abstract class AppServiceBase<T, U> : IApplicationService<T, U>
        where T : class, IDataTransferObject
        where U : class, IAggregateRoot
    {
        private static object mapLock = new object();

        /// <summary>
        /// Enable use auto validation at entities.
        /// </summary>
        protected virtual bool UseAutoValidate { get; set; }

        /// <summary>
        /// Repositório de dados.
        /// </summary>
        public virtual IRepository<U> Repository { protected get; set; }

        /// <summary>
        /// Mapper for transform to aggregate root.
        /// </summary>
        public virtual MapperBase<T, U> Mapper { get; set; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        public AppServiceBase()
        {
            this.UseAutoValidate = true;
        }

        /// <summary>
        /// Use this method for convert a queryable source into <see cref="Dantas.Core.DataResult{T}"/>
        /// </summary>
        /// <param name="source">Source for process by method.</param>
        /// <param name="pageInfo">Pagination and ordering information.</param>
        /// <param name="spec">Filter criteria.</param>
        /// <returns>DataResult from repository.</returns>
        protected virtual DataResult<T> ProcessDataResult(IQueryable<U> source, PageInfo pageInfo = null, Specification<U> spec = null)
        {
            var result = new DataResult<T>();

            if (pageInfo == null)
            {
                var entities = (spec == null ? source : source.Where(spec.Predicate)).ToArray();
                result.Records = Mapper.ConvertAll(entities);
                result.PageSize = 0;
                result.PageIndex = 0;
                result.TotalRecords = result.Records.Length;

                for (int i = 0; i < entities.Count(); i++)
                {
                    OnReadingEntity(result.Records[i], entities[i], fromDataResult: true);
                }
            }
            else
            {
                var entities = (spec == null ? source : source.Where(spec.Predicate))
                                        .SortingBy(pageInfo.Sorter)
                                        .Skip(pageInfo.StartIndex)
                                        .Take(pageInfo.PageLength)
                                        .ToArray();
                result.Records = Mapper.ConvertAll(entities);
                result.PageSize = pageInfo.PageLength;
                double index = pageInfo.StartIndex / pageInfo.PageLength;
                result.PageIndex = (int)Math.Floor(index);
                result.TotalRecords = (spec == null ? source : source.Where(spec.Predicate)).Count();

                for (int i = 0; i < entities.Count(); i++)
                {
                    OnReadingEntity(result.Records[i], entities[i], fromDataResult: true);
                }
            }

            return result;
        }

        /// <summary>
        /// Read all data from repository.
        /// </summary>
        /// <returns>Resultset with records getted from repository.</returns>
        [Transaction]
        public DataResult<T> ReadAll()
        {
            return ReadAll(pageInfo: null, spec: null);
        }

        /// <summary>
        /// Read data from repository with pagination and ordering information.
        /// </summary>
        /// <param name="pageInfo">Paging and ordering information.</param>
        /// <returns>Resultset with records getted from repository.</returns>
        [Transaction]
        public virtual DataResult<T> ReadAll(PageInfo pageInfo)
        {
            return ProcessDataResult(Repository.FindAll(), pageInfo);
        }

        /// <summary>
        /// Read data from repository with pagination and ordering information.
        /// </summary>
        /// <param name="expression">Filter criteria.</param>
        /// <param name="pageInfo">Paging and ordering information.</param>
        /// <returns>Resultset with records getted from repository.</returns>
        [Transaction]
        public virtual DataResult<T> ReadAll(Expression<Func<U, bool>> expression, PageInfo pageInfo = null)
        {
            Specification<U> spec = null;
            if (expression != null)
                spec = new Specification<U>(expression);
            return ProcessDataResult(Repository.FindAll(), pageInfo, spec);
        }

        /// <summary>
        /// Read data from repository with pagination and ordering information.
        /// </summary>
        /// <param name="spec">Filter criteria.</param>
        /// <param name="pageInfo">Paging and ordering information.</param>
        /// <returns>Resultset with records getted from repository.</returns>
        [Transaction]
        public virtual DataResult<T> ReadAll(Specification<U> spec, PageInfo pageInfo = null)
        {
            return ProcessDataResult(Repository.FindAll(), pageInfo, spec);
        }

        /// <summary>
        /// Read data from repository with pagination and ordering information.
        /// </summary>
        /// <param name="filter">Filter expression.</param>
        /// <param name="filterValues">Filter values in expression.</param>
        /// <param name="pageInfo">Paging and ordering information.</param>
        /// <returns>Resultset with records getted from repository.</returns>
        [Transaction]
        public DataResult<T> ReadAll(string filter, object[] filterValues, PageInfo pageInfo = null)
        {
            var query = Repository.FindAll().Where(filter, filterValues);

            return ProcessDataResult(query, pageInfo);
        }

        /// <summary>
        /// Read data from repository with pagination and ordering information.
        /// </summary>
        /// <param name="expression">Filter criteria.</param>
        /// <param name="filter">Filter expression.</param>
        /// <param name="filterValues">Filter values in expression.</param>
        /// <param name="pageInfo">Paging and ordering information.</param>
        /// <returns>Resultset with records getted from repository.</returns>
        [Transaction]
        public DataResult<T> ReadAll(Expression<Func<U, bool>> expression, string filter, object[] filterValues, PageInfo pageInfo = null)
        {
            var query = Repository.FindAll(expression).Where(filter, filterValues);

            return ProcessDataResult(query, pageInfo);
        }

        /// <summary>
        /// Read data from repository with pagination and ordering information.
        /// </summary>
        /// <param name="spec">Filter criteria.</param>
        /// <param name="filter">Filter expression.</param>
        /// <param name="filterValues">Filter values in expression.</param>
        /// <param name="pageInfo">Paging and ordering information.</param>
        /// <returns>Resultset with records getted from repository.</returns>
        [Transaction]
        public DataResult<T> ReadAll(Specification<U> spec, string filter, object[] filterValues, PageInfo pageInfo = null)
        {
            var query = Repository.FindAll(spec).Where(filter, filterValues);

            return ProcessDataResult(query, pageInfo);
        }

        /// <summary>
        /// Read data from repository with ordering information.
        /// </summary>
        /// <param name="sorter">Sorting column.</param>
        /// <returns>Resultset with records getted from repository.</returns>
        [Transaction]
        public DataResult<T> ReadAll(string sorter)
        {
            return ProcessDataResult(Repository.FindAll().SortingBy(sorter));
        }

        /// <summary>
        /// Read data from repository with ordering information.
        /// </summary>
        /// <param name="sorter">Sorting column.</param>
        /// <param name="filter">Filter expression.</param>
        /// <param name="filterValues">Filter values in expression.</param>
        /// <returns>Resultset with records getted from repository.</returns>
        [Transaction]
        public DataResult<T> ReadAll(string sorter, string filter, object[] filterValues)
        {
            return ProcessDataResult(Repository.FindAll().Where(filter, filterValues).SortingBy(sorter));
        }

        /// <summary>
        /// Read data from repository with ordering information.
        /// </summary>
        /// <param name="sorter">Sorting column.</param>
        /// <param name="expression">Filter criteria.</param>
        /// <returns>Resultset with records getted from repository.</returns>
        [Transaction]
        public DataResult<T> ReadAll(string sorter, Expression<Func<U, bool>> expression)
        {
            return ProcessDataResult(Repository.FindAll().Where(expression).SortingBy(sorter));
        }

        /// <summary>
        /// Get an entity by id.
        /// </summary>
        /// <param name="id">Id parameter value.</param>
        /// <returns>An entity from repository.</returns>
        [Transaction]
        public virtual T FindBy(object id)
        {
            var entity = Repository.FindOne(id);
            var transfer = Mapper.Convert(entity);

            OnReadingEntity(transfer, entity);

            return transfer;
        }

        /// <summary>
        /// Get an entity by expression condition.
        /// </summary>
        /// <param name="predicate">Expression to evaluate.</param>
        /// <returns>An entity from repository</returns>
        [Transaction]
        public virtual T FindBy(Expression<Func<U, bool>> predicate)
        {
            var entity = Repository.FindOne(predicate);
            var transfer = Mapper.Convert(entity);

            OnReadingEntity(transfer, entity);

            return transfer;
        }

        /// <summary>
        /// Get an entity by specification.
        /// </summary>
        /// <param name="spec">Specification condition.</param>
        /// <returns>An entity from repository.</returns>
        [Transaction]
        public virtual T FindBy(Specification<U> spec)
        {
            var entity = Repository.FindOne(spec);
            var transfer = Mapper.Convert(entity);

            OnReadingEntity(transfer, entity);

            return transfer;
        }

        /// <summary>
        /// Overrides this method for handle entity before this returned by repository.
        /// </summary>
        /// <param name="data">Data transfer object.</param>
        /// <param name="entity">Entity object.</param>
        /// <param name="fromDataResult">Indicates if this invoke is from ProcessDataResult method.</param>
        protected virtual void OnReadingEntity(T data, U entity, bool fromDataResult = false)
        {
            //This is empty, do nothing on base class
        }

        /// <summary>
        /// Overrides this method for handle entity before this saved by repository.
        /// </summary>
        /// <param name="data">Data transfer object.</param>
        /// <param name="entity">Entity object.</param>
        protected virtual void OnSavingEntity(T data, U entity)
        {
            //This is empty, do nothing on base class
        }

        /// <summary>
        /// Overrides this method for handle entity before this removed by repository.
        /// </summary>
        /// <param name="data">Data transfer object.</param>
        /// <param name="entity">Entity object.</param>
        protected virtual void OnDeletingEntity(T data, U entity)
        {
            //This is empty, do nothing on base class
        }

        /// <summary>
        /// Create new entity.
        /// </summary>
        /// <param name="data">Data transfer object.</param>
        /// <param name="ruleSet">Rule set name for validations.</param>
        [Transaction]
        public virtual void CreateNew(T data, string ruleSet = null)
        {
            U entity = Mapper.Convert(data as T);

            OnSavingEntity(data, entity);

            if (UseAutoValidate)
                Repository.SaveWithValidate(entity, ruleSet);
            else
                Repository.Save(entity);

            //Update id in IDataTransferObject
            (data as IDataTransferObject).Id = entity.Id;
        }

        /// <summary>
        /// Update entity.
        /// </summary>
        /// <param name="data">Data transfer object.</param>
        /// <param name="ruleSet">Rule set name for validations.</param>
        [Transaction]
        public virtual void UpdateWith(T data, string ruleSet = null)
        {
            var castedDto = data as IDataTransferObject;
            if (castedDto == null)
                throw new InvalidOperationException("Cannot use this method with data parameter is not implement IDataTransferObject interface.");

            U entity = Repository.FindOne(castedDto.Id);
            Mapper.Fill(data as T, entity);

            if (entity == null)
                throw new InvalidOperationException(String.Format("The row with id \"{0}\" is not found.", castedDto.Id));

            OnSavingEntity(data, entity);

            if (UseAutoValidate)
                Repository.SaveWithValidate(entity, ruleSet);
            else
                Repository.Save(entity);
        }

        /// <summary>
        /// Delete entity.
        /// </summary>
        /// <param name="data">Data transfer object.</param>
        /// <param name="ruleSet">Ruleset name for validation code.</param>
        [Transaction]
        public virtual void DeleteFor(T data, string ruleSet = null)
        {
            var castedDto = data as IDataTransferObject;
            if (castedDto == null)
                throw new InvalidOperationException("Cannot use this method with data parameter is not implement IDataTransferObject interface.");

            U entity = Repository.FindOne(e => e.Id == castedDto.Id);

            if (entity == null)
                throw new InvalidOperationException(String.Format("The row with id \"{0}\" is not found.", castedDto.Id));

            OnDeletingEntity(data, entity);

            if (UseAutoValidate)
            {
                ((IAutoValidable)entity).Validate(ruleSet);
                Repository.Remove(entity);
            }
            else
                Repository.Remove(entity);
        }
    }
}
