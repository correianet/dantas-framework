using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dantas.Core
{
    /// <summary>
    /// Base class for exceptions in this framework.
    /// </summary>
    [Serializable]
    public class DantasException : Exception
    {
        /// <summary>
        /// New exception.
        /// </summary>
        public DantasException() { }

        /// <summary>
        /// New exception.
        /// </summary>
        public DantasException(string message) : base(message) { }

        /// <summary>
        /// New exception.
        /// </summary>
        public DantasException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// New exception.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected DantasException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
