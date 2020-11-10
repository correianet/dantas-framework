using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dantas.Support
{
    /// <summary>
    /// Give comparer with property string name.
    /// </summary>
    /// <typeparam name="T">Comparable type.</typeparam>
    public class ReflectionComparer<T> : IComparer<T>
    {
        private string _prop;
        private bool _reverse;

        /// <summary>
        /// Create instance.
        /// </summary>
        /// <param name="property">Property name.</param>
        /// <param name="reverse">Is descending when compare objects.</param>
        public ReflectionComparer(string property, bool reverse)
        {
            this._prop = property;
            this._reverse = reverse;
        }

        #region IComparer<T> Members

        /// <summary>
        /// Compare two objects by property name configured.
        /// </summary>
        /// <param name="obj1">Object to compare.</param>
        /// <param name="obj2">Object to compare.</param>
        /// <returns>Compare result.</returns>
        public int Compare(T obj1, T obj2)
        {
            var propValue1 = (IComparable)obj1.GetType().GetProperty(_prop).GetValue(obj1, null);
            var propValue2 = (IComparable)obj2.GetType().GetProperty(_prop).GetValue(obj2, null);

            return propValue1.CompareTo(propValue2) * (_reverse ? -1 : 1);
        }

        #endregion
    }
}
