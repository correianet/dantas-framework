using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dantas.Core;
using AutoMapper;
using AutoMapper.Mappers;
using Dantas.Support;

namespace Dantas.Core
{
    /// <summary>
    /// Class base for mapping objects.
    /// </summary>
    /// <typeparam name="TTransfer">Transfer object.</typeparam>
    /// <typeparam name="TDomain">Domain object.</typeparam>
    public class MapperBase<TTransfer, TDomain> : Dantas.Core.IMapperBase<TTransfer,TDomain>
    {
        /// <summary>
        /// Configuration for AutoMapper.
        /// </summary>
        protected readonly ConfigurationStore configuration;

        /// <summary>
        /// Mapping for AutoMapper.
        /// </summary>
        protected readonly IMappingEngine mappingEngine;
        
        /// <summary>
        /// Create instance.
        /// </summary>
        public MapperBase()
        {
            configuration = MapperUtil.CreateDefaultConfiguration();

            OnConfiguring(configuration);

            TransferMap(configuration);
            DomainMap(configuration);
            
            mappingEngine = new MappingEngine(configuration);
        }

        protected virtual void OnConfiguring(ConfigurationStore configuration)
        {
            //Do nothing, is virtual only because is not a mandatory implementation
        }

        /// <summary>
        /// Convert domain object to transfer object.
        /// </summary>
        /// <param name="configuration">AutoMapper configuration</param>
        protected virtual void TransferMap(IConfiguration configuration)
        {
            configuration.CreateMap<TDomain, TTransfer>();
        }

        /// <summary>
        /// Convert transfer object to domain object.
        /// </summary>
        /// <param name="configuration">AutoMapper configuration</param>
        protected virtual void DomainMap(IConfiguration configuration)
        {
            configuration.CreateMap<TTransfer, TDomain>();
        }

        /// <summary>
        /// Transform objects.
        /// </summary>
        /// <param name="entity">Domain object to be converted.</param>
        /// <returns>Transfer object from domain.</returns>
        public TTransfer Convert(TDomain entity)
        {
            return mappingEngine.Map<TDomain, TTransfer>(entity);
        }

        /// <summary>
        /// Transform objects.
        /// </summary>
        /// <param name="transfer">Transfer object to be converted.</param>
        /// <returns>Domain object from transfer.</returns>
        public TDomain Convert(TTransfer transfer)
        {
            return mappingEngine.Map<TTransfer, TDomain>(transfer);
        }

        /// <summary>
        /// Fill destination with source data.
        /// </summary>
        /// <param name="entity">Entity used as source.</param>
        /// <param name="transfer">Transfer object destination.</param>
        public void Fill(TDomain entity, TTransfer transfer)
        {
            mappingEngine.Map<TDomain, TTransfer>(entity, transfer);
        }

        /// <summary>
        /// Fill destination with source data.
        /// </summary>
        /// <param name="transfer">Transfer used as source.</param>
        /// <param name="entity">Entity object destination</param>
        public void Fill(TTransfer transfer, TDomain entity)
        {
            mappingEngine.Map<TTransfer, TDomain>(transfer, entity);
        }

        /// <summary>
        /// Transform object creating a set.
        /// </summary>
        /// <param name="entities">Set to transform.</param>
        /// <returns>Transformed set.</returns>
        public TTransfer[] ConvertAll(TDomain[] entities)
        {
            return mappingEngine.Map<TDomain[], TTransfer[]>(entities);
        }

        /// <summary>
        /// Transform object creating a set.
        /// </summary>
        /// <param name="transfers">Set to transform.</param>
        /// <returns>Transformed set.</returns>
        public TDomain[] ConvertAll(TTransfer[] transfers)
        {
            return mappingEngine.Map<TTransfer[], TDomain[]>(transfers);
        }
    }
}
