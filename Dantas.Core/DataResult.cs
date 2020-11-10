using Dantas.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dantas.Core
{
    /// <summary>
    /// Represents a result from application service.
    /// </summary>
    /// <typeparam name="T">Type handled by data result.</typeparam>
    public class DataResult<T>
    {
        /// <summary>
        /// Results from repository.
        /// </summary>
        public T[] Records { get; set; }

        /// <summary>
        /// Current page information.
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Current size information.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Amount of records on repository. It's not the length of Records property.
        /// </summary>
        public int TotalRecords { get; set; }
    }
}
