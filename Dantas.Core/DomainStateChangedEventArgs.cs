using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dantas.Core
{
    /// <summary>
    /// Information about DomainState event.
    /// </summary>
    public sealed class DomainStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets new value for notify.
        /// </summary>
        public object NewValue { get; private set; }

        /// <summary>
        /// Gets property name for notify.
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Create instance.
        /// </summary>
        public DomainStateChangedEventArgs() { }

        /// <summary>
        /// Create instance.
        /// </summary>
        /// <param name="propertyName">Property name was changed.</param>
        /// <param name="newValue">New property value.</param>
        public DomainStateChangedEventArgs(string propertyName, object newValue)
        {
            this.PropertyName = propertyName;
            this.NewValue = newValue;
        }
    }
}
