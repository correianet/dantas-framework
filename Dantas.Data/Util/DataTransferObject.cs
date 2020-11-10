using Dantas.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dantas.Data.Util
{
    /// <summary>
    /// Inherit for implement the data transfer class.
    /// </summary>
    [Serializable]
    public class DataTransferObject<T> : IDataTransferObject where T : IComparable, new()
    {
        /// <summary>
        /// Transfer object id.
        /// </summary>
        public virtual T Id { get; set; }

        #region IDataTransferObject Members

        object IDataTransferObject.Id
        {
            get
            {
                return this.Id;
            }
            set
            {
                this.Id = (T)value;
            }
        }

        #endregion
    }
}
