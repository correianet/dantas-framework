using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dantas.Core
{
    /// <summary>
    /// Send information about ordering and pagination.
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        /// Configure start read position.
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// Configure size of read.
        /// </summary>
        public int PageLength { get; set; }

        /// <summary>
        /// Configure order on source.
        /// </summary>
        public string Sorter { get; set; }
    }
}
