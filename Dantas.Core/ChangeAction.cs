using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dantas.Core
{
    /// <summary>
    /// Signals for NotifyChange event.
    /// </summary>
    public enum ChangeAction
    {
        /// <summary>
        /// Represent add action.
        /// </summary>
        Add,

        /// <summary>
        /// Represent edit action.
        /// </summary>
        Edit,

        /// <summary>
        /// Represent remove action.
        /// </summary>
        Remove,

        /// <summary>
        /// Represent clear action.
        /// </summary>
        Clear
    }
}
