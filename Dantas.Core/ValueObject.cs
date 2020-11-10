using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dantas.Core
{
    /// <summary>
    /// Base class for objects value.
    /// </summary>
    public abstract class ValueObject
    {
        /// <summary>
        /// Domain value.
        /// </summary>
        public virtual event EventHandler<DomainStateChangedEventArgs> StateChanged;

        /// <summary>
        /// Raise event.
        /// </summary>
        /// <param name="propertyName">Property for used.</param>
        /// <param name="newValue">newValue for used.</param>
        protected virtual void NotifyStateChanged(string propertyName, object newValue)
        {
            if (StateChanged != null)
                StateChanged(this, new DomainStateChangedEventArgs(propertyName, newValue));
        }

        /// <summary>
        /// Validate someone can perform validation.
        /// </summary>
        /// <returns>Returns true if has listener, otherwise false.</returns>
        public virtual bool HasListener()
        {
            return StateChanged != null;
        }
    }
}
