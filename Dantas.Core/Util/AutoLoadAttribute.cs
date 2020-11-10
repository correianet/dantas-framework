using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dantas.Core.Util
{
    /// <summary>
    /// Use this for enable auto run methods in a class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class AutoLoadAttribute : Attribute
    {
        readonly bool autoLoad;

        /// <summary>
        /// Create new auto load.
        /// </summary>
        public AutoLoadAttribute()
        {
            this.autoLoad = true;
        }

        /// <summary>
        /// Create new auto load.
        /// </summary>
        /// <param name="isAutoLoad">Enable or disable auto load.</param>
        public AutoLoadAttribute(bool isAutoLoad)
        {
            this.autoLoad = isAutoLoad;
        }

        /// <summary>
        /// Returns if auto load is enabled.
        /// </summary>
        public bool IsAutoLoad
        {
            get { return autoLoad; }
        }
    }
}
