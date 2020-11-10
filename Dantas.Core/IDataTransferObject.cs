using System;
namespace Dantas.Core
{
    /// <summary>
    /// Implement for data transfer class.
    /// </summary>
    public interface IDataTransferObject
    {
        /// <summary>
        /// Transfer object id.
        /// </summary>
        object Id { get; set; }
    }
}
