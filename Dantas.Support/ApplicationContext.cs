using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dantas.Support
{
    /// <summary>
    /// Spring Application Context.
    /// </summary>
    public static class ApplicationContext
    {
        private static IApplicationContext Context { get; set; }

        /// <summary>
        /// Returns a boolean value if the current application context contains an named object.
        /// </summary>
        /// <param name="objectName">Accepts the name of the object to check.</param>
        public static bool Contains(string objectName)
        {
            ApplicationContext.EnsureContext();
            return ApplicationContext.Context.ContainsObject(objectName);
        }

        /// <summary>
        /// Return a instance of an object in the context by the specified name.
        /// </summary>
        /// <param name="objectName">Accepts a string object name.</param>
        public static object Resolve(string objectName)
        {
            ApplicationContext.EnsureContext();
            return ApplicationContext.Context.GetObject(objectName);
        }

        /// <summary>
        /// Return a instance of an object in the context by the specified name and type.
        /// </summary>
        /// <typeparam name="T">Accepts the type of the object to resolve.</typeparam>
        /// <param name="objectName">Accepts a string object name.</param>
        public static T Resolve<T>(string objectName)
        {
            return (T)ApplicationContext.Resolve(objectName);
        }

        /// <summary>
        /// Return a instance of an object in the context by the specified name and type, assumption is the object name equals the type name.
        /// </summary>
        /// <typeparam name="T">Accepts the type of the object to resolve.</typeparam>
        public static T Resolve<T>()
        {
            return (T)ApplicationContext.Resolve(typeof(T).Name);
        }

        #region Private Methods

        private static void EnsureContext()
        {
            if (ApplicationContext.Context == null)
            {
                ApplicationContext.Context = ContextRegistry.GetContext();
            }
        }

        #endregion
    }
}
