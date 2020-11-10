using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dantas.Core
{
    /// <summary>
    /// Information about NotifyChange event.
    /// </summary>
    public class NotifyChangeEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the action for notify.
        /// </summary>
        public ChangeAction Action { get; private set; }

        /// <summary>
        /// Gets the item for notify.
        /// </summary>
        public object ChangedItem { get; private set; }

        /// <summary>
        /// Create instance.
        /// </summary>
        /// <param name="action">Action occurred.</param>
        /// <param name="changedItem">Modified item.</param>
        public NotifyChangeEventArgs(ChangeAction action, object changedItem)
        {
            this.Action = action;
            this.ChangedItem = changedItem;
        }
    }
}
