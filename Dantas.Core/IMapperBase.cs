using System;
namespace Dantas.Core
{
    /// <summary>
    /// Interface to implement mapping objets.
    /// </summary>
    /// <typeparam name="TTransfer">Transfer object</typeparam>
    /// <typeparam name="TDomain">Domain object.</typeparam>
    public interface IMapperBase<TTransfer, TDomain>
    {
        /// <summary>
        /// Transform objects.
        /// </summary>
        /// <param name="entity">Domain object to be converted.</param>
        /// <returns>Transfer object from domain.</returns>
        TTransfer Convert(TDomain entity);

        /// <summary>
        /// Transform objects.
        /// </summary>
        /// <param name="transfer">Transfer object to be converted.</param>
        /// <returns>Domain object from transfer.</returns>
        TDomain Convert(TTransfer transfer);

        /// <summary>
        /// Fill destination with source data.
        /// </summary>
        /// <param name="entities">Set of entities used as source.</param>
        TTransfer[] ConvertAll(TDomain[] entities);

        /// <summary>
        /// Fill destination with source data.
        /// </summary>
        /// <param name="transfers">Set of transfers used as source.</param>
        TDomain[] ConvertAll(TTransfer[] transfers);

        /// <summary>
        /// Transform object creating a set.
        /// </summary>
        /// <param name="entity">Entity object to be converted.</param>
        /// <param name="transfer">Transfer object to be converted.</param>
        /// <returns>Transformed set.</returns>
        void Fill(TDomain entity, TTransfer transfer);

        /// <summary>
        /// Transform object creating a set.
        /// </summary>
        /// <param name="transfer">Transfer object to be converted.</param>
        /// <param name="entity">Entity object to be converted.</param>
        /// <returns>Transformed set.</returns>
        void Fill(TTransfer transfer, TDomain entity);
    }
}
