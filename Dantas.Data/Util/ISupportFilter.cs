using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dantas.Core;

namespace Dantas.Data.Util
{
    /// <summary>
    /// Enable filters by specifications.
    /// </summary>
    /// <typeparam name="T">Element type.</typeparam>
    public interface ISupportFilter<T>
    {
        /// <summary>
        /// Add criteria specification.
        /// </summary>
        /// <param name="condition">Specification element.</param>
        void AddCriteria(Specification<T> condition);

        /// <summary>
        /// Remove criteria specification.
        /// </summary>
        /// <param name="condition">Specification element.</param>
        void RemoveCriteria(Specification<T> condition);
    }
}
