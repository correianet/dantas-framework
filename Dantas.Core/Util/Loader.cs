using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Dantas.Core.Util
{
    /// <summary>
    /// Runner for auto load infrastructure.
    /// </summary>
    public static class Loader
    {
        /// <summary>
        /// Run all classes with auto load attribute and it is enabled.
        /// </summary>
        /// <param name="assemblyName">Assembly that Loader will run.</param>
        public static void Start(string assemblyName)
        {
            var myAssembly = Assembly.Load(assemblyName);

            var qr = from type in myAssembly.GetTypes()
                     let loadConfig = type.GetCustomAttributes(typeof(AutoLoadAttribute), true).FirstOrDefault() as AutoLoadAttribute
                     where loadConfig != null && loadConfig.IsAutoLoad
                     select type;

            foreach (var item in qr)
            {
                var instance = Activator.CreateInstance(item);

                if (instance is IAutoLoadable)
                    ((IAutoLoadable)instance).Load();
            }

        }
    }
}
