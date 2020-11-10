using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dantas.Core.Util
{
    /// <summary>
    /// Call for infrastructure if the class is auto load candidate.
    /// </summary>
    public interface IAutoLoadable
    {
        /// <summary>
        /// Implement this for call the auto load method.
        /// </summary>
        void Load();

        /// <summary>
        /// Implement this for return the auto load result.
        /// </summary>
        /// <returns>Return if auto load is runned.</returns>
        bool IsLoaded();
    }
}
