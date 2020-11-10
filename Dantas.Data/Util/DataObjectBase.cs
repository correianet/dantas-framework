using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dantas.Core;
using Dantas.Core.Util;
using Dantas.Support;
using System.ComponentModel;
using AutoMapper;
using AutoMapper.Mappers;
using System.Linq.Expressions;

namespace Dantas.Data.Util
{
    /// <summary>
    /// Manipulating an entity to transform, create, read, update and delete.
    /// </summary>
    /// <typeparam name="T">Data transfer object type.</typeparam>
    /// <typeparam name="U">Entity type.</typeparam>
    public abstract class DataObjectBase<T, U> : IDataObject<T>, ISupportFilter<U>
        where T : class, IDataTransferObject
        where U : class, IAggregateRoot
    {
        private static object mapLock = new object();

        private List<Specification<U>> listCriteria = new List<Specification<U>>();

        /// <summary>
        /// Repository associated with aggregate root.
        /// </summary>
        public virtual IRepository<U> Repository { protected get; set; }

        /// <summary>
        /// Application service that enable data object manipulating entity state change.
        /// </summary>
        public virtual IApplicationService<T, U> AppService { protected get; set; }

        /// <summary>
        /// Mapper for transform to aggregate root.
        /// </summary>
        public MapperBase<T, U> Mapper { get; set; }

        /// <summary>
        /// Name of entity property where data object manipulating this when used for first read and when sorter is not specified.
        /// </summary>
        protected virtual string DefaultSorter
        {
            get { return "Id"; }
        }

        /// <summary>
        /// Transform an entity to data transfer object.
        /// </summary>
        /// <param name="entity">Entity used as source.</param>
        /// <returns>Data transfer object transformed by configured mapping expression.</returns>
        protected T CreateTransfer(U entity)
        {
            return Mapper.Convert(entity);
        }

        /// <summary>
        /// Same as <seealso cref="CreateTransfer"/>, but for collection.
        /// </summary>
        /// <param name="entity">Entity used as source.</param>
        /// <returns>Data transfer object transformed by configured mapping expression.</returns>
        /// <typeparam name="TCustom">Custom type for source.</typeparam>
        protected T CreateTransfer<TCustom>(TCustom entity)
        {
            return CreateTransfer<TCustom>(entity, cfg => cfg.CreateMap<TCustom, T>());
        }

        /// <summary>
        /// Transform an entity to data transfer object.
        /// </summary>
        /// <param name="entity">Entity used as source.</param>
        /// <param name="mapperMaker">Caller for function with customized mapping expression.</param>
        /// <returns>Data transfer object transformed by configured mapping expression.</returns>
        /// <typeparam name="TCustom">Tipo customizado para mapeamento.</typeparam>
        protected T CreateTransfer<TCustom>(TCustom entity, Func<IConfiguration, IMappingExpression<TCustom, T>> mapperMaker)
        {
            T result;
            
            var configuration = MapperUtil.CreateDefaultConfiguration();
            mapperMaker(configuration);

            var mappingEngine = new MappingEngine(configuration);
            result = mappingEngine.Map<TCustom, T>(entity);
            
            return result;
        }

        /// <summary>
        /// Same as <seealso cref="CreateTransfer"/>, but for collection.
        /// </summary>
        /// <param name="entities">Collection used as source.</param>
        /// <returns>Transformed collection.</returns>
        protected T[] CreateArrayTransfer(U[] entities)
        {
            return Mapper.ConvertAll(entities);
        }

        /// <summary>
        /// Same as <seealso cref="CreateTransfer"/>, but for collection.
        /// </summary>
        /// <param name="entities">Collection used as source.</param>
        /// <returns>Transformed collection.</returns>
        /// <typeparam name="TCustom">Custom type for source.</typeparam>
        protected T[] CreateArrayTransfer<TCustom>(TCustom[] entities)
        {
            return CreateArrayTransfer<TCustom>(entities, cfg => cfg.CreateMap<TCustom, T>());
        }

        /// <summary>
        /// Same as <seealso cref="CreateTransfer"/>, but for collection.
        /// </summary>
        /// <param name="entities">Collection used as source.</param>
        /// <param name="mapperMaker">Caller for function with customized mapping expression.</param>
        /// <returns>Transformed collection.</returns>
        /// <typeparam name="TCustom">Custom type for source.</typeparam>
        protected T[] CreateArrayTransfer<TCustom>(TCustom[] entities, Func<IConfiguration, IMappingExpression<TCustom, T>> mapperMaker)
        {
            T[] result;

            var configuration = MapperUtil.CreateDefaultConfiguration();
            mapperMaker(configuration);

            var mappingEngine = new MappingEngine(configuration);
            result = mappingEngine.Map<TCustom[], T[]>(entities);
            
            return result;
        }

        /// <summary>
        /// Repository considering filters in criteria list.
        /// </summary>
        /// <returns>Query object having filter expression.</returns>
        protected virtual IQueryable<U> FilteredData
        {
            get
            {
                var result = from e in Repository.FindAll()
                             select e;

                foreach (var criteria in listCriteria)
                {
                    result = result.Where(criteria.Predicate);
                }

                return result;
            }
        }

        /// <summary>
        /// Get entity by id.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <returns>Loaded entity.</returns>
        protected virtual U LoadEntity(object id)
        {
            return Repository.FindOne(id);
        }

        /// <summary>
        /// Call the transformation of entity array to data transfer object array for used to bind UI components.
        /// </summary>
        /// <param name="entities">Entity list.</param>
        /// <returns>Data transfer object set.</returns>
        protected T[] ToDataArray(IQueryable<U> entities)
        {
            return ToDataArray<U>(entities, DefaultSorter, null);
        }

        /// <summary>
        /// Call the transformation of entity array to data transfer object array for used to bind UI components.
        /// </summary>
        /// <param name="entities">Entity list.</param>
        /// <param name="sorter">Property name of entity for order the items.</param>
        /// <returns>Data transfer object set.</returns>
        protected T[] ToDataArray(IQueryable<U> entities, string sorter)
        {
            return ToDataArray<U>(entities, sorter, null);
        }

        /// <summary>
        /// Call the transformation of entity array to data transfer object array for used to bind UI components.
        /// </summary>
        /// <param name="entities">Entity list.</param>
        /// <param name="startIndex">Position for start the rows to return.</param>
        /// <param name="length">Limit size of result.</param>
        /// <returns>Data transfer object set.</returns>
        protected T[] ToDataArray(IQueryable<U> entities, int startIndex, int length)
        {
            return ToDataArray<U>(entities, startIndex, length, null);
        }

        /// <summary>
        /// Call the transformation of entity array to data transfer object array for used to bind UI components.
        /// </summary>
        /// <param name="entities">Entity list.</param>
        /// <param name="startIndex">Position for start the rows to return.</param>
        /// <param name="length">Limit size of result.</param>
        /// <param name="sorter">Property name of entity for order the items.</param>
        /// <returns>Data transfer object set.</returns>
        protected T[] ToDataArray(IQueryable<U> entities, int startIndex, int length, string sorter)
        {
            return ToDataArray<U>(entities, startIndex, length, sorter, null);
        }

        /// <summary>
        /// Call the transformation of entity array to data transfer object array for used to bind UI components.
        /// </summary>
        /// <param name="entities">Entity list.</param>
        /// <param name="mapperMaker">Caller for function with customized mapping expression.</param>
        /// <returns>Data transfer object set.</returns>
        /// <typeparam name="TCustom">Custom type for source.</typeparam>
        protected T[] ToDataArray<TCustom>(IQueryable<TCustom> entities, Func<IConfiguration, IMappingExpression<TCustom, T>> mapperMaker)
        {
            return ToDataArray<TCustom>(entities, DefaultSorter, mapperMaker);
        }

        /// <summary>
        /// Call the transformation of entity array to data transfer object array for used to bind UI components.
        /// </summary>
        /// <param name="entities">Entity list.</param>
        /// <param name="startIndex">Position for start the rows to return.</param>
        /// <param name="length">Limit size of result.</param>
        /// <param name="mapperMaker">Caller for function with customized mapping expression.</param>
        /// <returns>Data transfer object set.</returns>
        /// <typeparam name="TCustom">Custom type for source.</typeparam>
        protected T[] ToDataArray<TCustom>(IQueryable<TCustom> entities, int startIndex, int length, Func<IConfiguration, IMappingExpression<TCustom, T>> mapperMaker)
        {
            return ToDataArray<TCustom>(entities, startIndex, length, this.DefaultSorter, mapperMaker);
        }

        /// <summary>
        /// Call the transformation of entity array to data transfer object array for used to bind UI components.
        /// </summary>
        /// <param name="entities">Entity list.</param>
        /// <param name="sorter">Property name of entity for order the items.</param>
        /// <param name="mapperMaker">Caller for function with customized mapping expression.</param>
        /// <returns>Data transfer object set.</returns>
        /// <typeparam name="TCustom">Custom type for source.</typeparam>
        protected T[] ToDataArray<TCustom>(IQueryable<TCustom> entities, string sorter, Func<IConfiguration, IMappingExpression<TCustom, T>> mapperMaker)
        {
            return ToDataArray(entities, -1, -1, sorter, mapperMaker);
        }

        /// <summary>
        /// Call the transformation of entity array to data transfer object array for used to bind UI components.
        /// </summary>
        /// <param name="entities">Entity list.</param>
        /// <param name="startIndex">Position for start the rows to return.</param>
        /// <param name="length">Limit size of result.</param>
        /// <param name="sorter">Property name of entity for order the items.</param>
        /// <param name="mapperMaker">Caller for function with customized mapping expression.</param>
        /// <returns>Data transfer object set.</returns>
        /// <typeparam name="TCustom">Custom type for source.</typeparam>
        protected T[] ToDataArray<TCustom>(IQueryable<TCustom> entities, int startIndex, int length, string sorter, Func<IConfiguration, IMappingExpression<TCustom, T>> mapperMaker)
        {
            if (String.IsNullOrEmpty(sorter))
                sorter = this.DefaultSorter;

            entities.SortingBy(sorter);

            if (startIndex >= 0 && length > 0)
                entities = entities.Skip(startIndex).Take(length);

            T[] result;
            if (mapperMaker == null)
                result = CreateArrayTransfer(entities.ToArray() as U[]);
            else
                result = CreateArrayTransfer(entities.ToArray(), mapperMaker);

            return result;
        }

        /// <summary>
        /// List all data transfer object.
        /// </summary>
        /// <returns>Transformed array of data transfer objects using mapping expression.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public virtual T[] ReadAll()
        {
            return ToDataArray(this.FilteredData, this.DefaultSorter);
        }

        /// <summary>
        /// List all data transfer object.
        /// </summary>
        /// <param name="startIndex">Position for start the rows to return.</param>
        /// <param name="length">Limit size of result.</param>
        /// <returns>Transformed array of data transfer objects using mapping expression.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public virtual T[] ReadAll(int startIndex, int length)
        {
            return ToDataArray(FilteredData, startIndex, length, this.DefaultSorter);
        }

        /// <summary>
        /// List all data transfer object.
        /// </summary>
        /// <param name="sorter">Property name of entity for order the items.</param>
        /// <returns>Transformed array of data transfer objects using mapping expression.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public virtual T[] ReadAll(string sorter)
        {
            return ToDataArray(FilteredData, sorter);
        }

        /// <summary>
        /// List all data transfer object.
        /// </summary>
        /// <param name="startIndex">Position for start the rows to return.</param>
        /// <param name="length">Limit size of result.</param>
        /// <param name="sorter">Property name of entity for order the items.</param>
        /// <returns>Transformed array of data transfer objects using mapping expression.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public virtual T[] ReadAll(int startIndex, int length, string sorter)
        {
            return ToDataArray(FilteredData, startIndex, length, sorter);
        }

        /// <summary>
        /// List all data transfer object.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <returns>Transformed array of data transfer objects using mapping expression.</returns>
        [DataObjectMethod(DataObjectMethodType.Select)]
        public virtual T[] Read(object id)
        {
            var result = LoadEntity(id);

            if (result != null)
            {
                return new T[] { CreateTransfer(result) };
            }
            else
                return new T[] { };
        }

        /// <summary>
        /// Read an object from source.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <returns>Transformed data transfer object using mapping expression.</returns>
        public virtual T Load(object id)
        {
            var result = LoadEntity(id);

            if (result != null)
                return CreateTransfer(result);
            else
                return default(T);
        }

        /// <summary>
        /// Get total rows at filtered repository data.
        /// </summary>
        /// <returns>Total size.</returns>
        public virtual int Count()
        {
            return FilteredData.Count();
        }

        /// <summary>
        /// Add filter criteria.
        /// </summary>
        /// <param name="condition">Filter criteria.</param>
        public void AddCriteria(Specification<U> condition)
        {
            if (!listCriteria.Contains(condition))
            {
                listCriteria.Add(condition);
            }
        }

        /// <summary>
        /// Create filter criteria by string parameters.
        /// </summary>
        /// <param name="searchField">Property name.</param>
        /// <param name="searchOperator">Operator type.</param>
        /// <param name="searchString">Query value.</param>
        public void AddCriteria(string searchField, string searchOperator, string searchString)
        {
            this.listCriteria.Add(SpecificationUtil.CreateSpecificationBy<U>(searchField, searchOperator, searchString));
        }

        /// <summary>
        /// Remove filter criteria.
        /// </summary>
        /// <param name="condition">Filter criteria.</param>
        public void RemoveCriteria(Specification<U> condition)
        {
            if (listCriteria.Contains(condition))
            {
                listCriteria.Remove(condition);
            }
        }

        /// <summary>
        /// Remove filter criteria by string parameters.
        /// </summary>
        /// <param name="searchField">Property name.</param>
        /// <param name="searchOperator">Operator type.</param>
        /// <param name="searchString">Query value.</param>
        public void RemoveCriteria(string searchField, string searchOperator, string searchString)
        {
            this.listCriteria.Remove(SpecificationUtil.CreateSpecificationBy<U>(searchField, searchOperator, searchString));
        }

        /// <summary>
        /// Clear all criterias.
        /// </summary>
        public void ClearCriteria()
        {
            listCriteria.Clear();
        }

        /// <summary>
        /// Get total of criterias.
        /// </summary>
        /// <returns>Total size.</returns>
        public int CountCriteria()
        {
            return listCriteria.Count;
        }

        /// <summary>
        /// Return an array of all criteria specifications.
        /// </summary>
        /// <returns>Specification set.</returns>
        public Specification<U>[] SelectCriterias()
        {
            return listCriteria.ToArray();
        }

        /// <summary>
        /// Override this to intercept the insert command.
        /// </summary>
        /// <param name="transfer">Data transfer object.</param>
        protected virtual void RunInsertCommand(T transfer)
        {
            AppService.CreateNew(transfer);
        }

        /// <summary>
        /// Override this to intercept the update command.
        /// </summary>
        /// <param name="transfer">Data transfer object.</param>
        protected virtual void RunUpdateCommand(T transfer)
        {
            AppService.UpdateWith(transfer);
        }

        /// <summary>
        /// Override this to intercept the delete command.
        /// </summary>
        /// <param name="transfer">Data transfer object.</param>
        protected virtual void RunDeleteCommand(T transfer)
        {
            AppService.DeleteFor(transfer);
        }

        /// <summary>
        /// Create an entity by data transfer object.
        /// </summary>
        /// <param name="transferObject">Data transfer object.</param>
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public void Create(T transferObject)
        {
            RunInsertCommand(transferObject);
        }

        /// <summary>
        /// Update an entity by data transfer object.
        /// </summary>
        /// <param name="transferObject">Data transfer object.</param>
        [DataObjectMethod(DataObjectMethodType.Update)]
        public void Update(T transferObject)
        {
            RunUpdateCommand(transferObject);
        }

        /// <summary>
        /// Delete an entity by data transfer object.
        /// </summary>
        /// <param name="transferObject">Data transfer object.</param>
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public void Delete(T transferObject)
        {
            RunDeleteCommand(transferObject);
        }
    }
}
