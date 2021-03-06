﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dantas.Core
{
    /// <summary>
    /// Base class for delimited domain values object.
    /// </summary>
    /// <typeparam name="T">Custom type of id.</typeparam>
    public abstract class DomainValueObject<T> : EntityBase where T : IComparable, new()
    {
        /// <summary>
        /// Object id.
        /// </summary>
        public virtual T Id
        {
            get
            {
                T result;
                if (id == null)
                    result = default(T);
                else
                    result = (T)id;
                return result;
            }
            protected set
            {
                id = value;
            }
        }

        /// <summary>
        /// Validate id signature for persistent objects.
        /// </summary>
        /// <param name="compareTo">Persistent to compare.</param>
        /// <returns>True if this equals.</returns>
        protected override bool HasSameNonDefaultIdAs(EntityBase compareTo)
        {
            return !IsTransient() &&
                  !compareTo.IsTransient() &&
                  this.Id.Equals(((DomainValueObject<T>)compareTo).Id);
        }
    }
}
