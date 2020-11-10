using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dantas.Support
{
    /// <summary>
    /// Some method helpers for enhance enumerable API.
    /// </summary>
    public static class CollectionHelpers
    {
        /// <summary>
        /// Merge a list into collection.
        /// </summary>
        /// <param name="originalItems">Original list items.</param>
        /// <param name="newItems">New list items.</param>
        /// <param name="removeNotFound">Remove items into collection if not found in the original.</param>
        public static void Merge<T>(this ICollection<T> originalItems, IEnumerable<T> newItems, bool removeNotFound = true)
        {
            /* Use any method to allow duplicated items on returned list */

            //Remove if necessary
            if (removeNotFound)
            {
                var removeList = originalItems.Where(original => !newItems.Any(newest => original.Equals(newest))).ToArray();
                foreach (var item in removeList)
                {
                    originalItems.Remove(item);
                }
            }

            //Merge
            var updateList = newItems.Where(newest => originalItems.Any(original => newest.Equals(original))).ToArray();
            foreach (var item in updateList)
            {
                var originalItem = originalItems.Where(e => e.Equals(item)).Single();
                var itemType = originalItem.GetType();
                foreach (var prop in itemType.GetProperties())
                {
                    if (prop.CanWrite)
                    {
                        prop.SetValue(originalItem, prop.GetValue(item, null), null);
                    }
                }
            }

            //News
            var newList = newItems.Where(newest => !originalItems.Any(original => newest.Equals(original))).ToArray();
            foreach (var item in newList)
            {
                originalItems.Add(item);
            }
        }
    }
}
