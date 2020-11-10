using AutoMapper;
using AutoMapper.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dantas.Support
{
    /// <summary>
    /// Util for automapper engine.
    /// </summary>
    public static class MapperUtil
    {
        /// <summary>
        /// Create a mapping engine.
        /// </summary>
        /// <returns>Default mapping engine for framework use.</returns>
        public static IMappingEngine CreateDefaultEngine()
        {
            var configuration = CreateDefaultConfiguration();
            return new MappingEngine(configuration);
        }

        /// <summary>
        /// Create a configuration object.
        /// </summary>
        /// <returns>Default configuration for framework use.</returns>
        public static ConfigurationStore CreateDefaultConfiguration()
        {
            var configuration = new ConfigurationStore(new TypeMapFactory(), MapperRegistry.AllMappers());
            
            configuration.RecognizeDestinationPostfixes("Text");
            configuration.CreateMap<Enum, string>().ConvertUsing<EnumTypeConverter>();

            return configuration;
        }
    }
}
