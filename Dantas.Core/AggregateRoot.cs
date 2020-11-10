using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dantas.Core
{
    /// <summary>
    /// Entity aggregate root enabled.
    /// </summary>
    public abstract class AggregateRoot<T> : Entity<T>, IAggregateRoot where T : IComparable, new()
    {
        #region IAggregateRoot Members

        object IAggregateRoot.Id
        {
            get { return this.Id; }
        }

        #endregion
    }
}
