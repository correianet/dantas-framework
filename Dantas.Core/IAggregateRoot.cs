using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dantas.Core
{
    /// <summary>
    /// Enable entity to be aggregate root and attachable with repository
    /// </summary>
    public interface IAggregateRoot
    {
        /// <summary>
        /// Exposed id.
        /// </summary>
        object Id { get; }
    }
}
