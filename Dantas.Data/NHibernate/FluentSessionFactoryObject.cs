using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spring.Data.NHibernate;
using FluentNHibernate.Cfg;
using NHibernate.Cfg;
using System.Reflection;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions.Helpers;
using Dantas.Core;
using FluentNHibernate.Conventions;
using FluentNHibernate;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using NHibernate.Bytecode;
using Spring.Data.NHibernate.Bytecode;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[assembly: InternalsVisibleTo("FluentNHibernate")] 

namespace Dantas.Data.NHibernate
{
    /// <summary>
    /// Extends default local session factory of spring for enable auto mapping NHibernate.
    /// </summary>
    public class FluentSessionFactoryObject : LocalSessionFactoryObject
    {
        private Assembly[] assembliesForMapping = null;
        private Assembly[] assembliesForOverriding = null;
        private bool needCreateConfig = true;
        private string currentPath = String.Empty;

        /// <summary>
        /// Create new session factory object.
        /// </summary>
        public FluentSessionFactoryObject()
        {
            FluentNHibernateMappingAssemblies = new string[0];
            FluentNHibernateOverridingAssemblies = new string[0];
        }

        /// <summary>
        /// Assemblies name of entities.
        /// </summary>
        public string[] FluentNHibernateMappingAssemblies { get; set; }

        /// <summary>
        /// Assemblies name of overrides.
        /// </summary>
        public string[] FluentNHibernateOverridingAssemblies { get; set; }

        /// <summary>
        /// Enable auto mapping.
        /// </summary>
        public bool UsingAutoMap { get; set; }

        /// <summary>
        /// Export HBM.
        /// </summary>
        public bool ExportMappings { get; set; }

        /// <summary>
        /// Custom configuration.
        /// </summary>
        public IAutomappingConfiguration AutoMapConfig { get; set; }

        /// <summary>
        /// Custom setup builder.
        /// </summary>
        public ISetupConvention ConventionBuilder { get; set; }

        /// <summary>
        /// Save NHibernate Configuration only needed.
        /// </summary>
        /// <param name="config">NHibernate Configuration</param>
        protected override void PostProcessConfiguration(Configuration config)
        {
            assembliesForMapping = FluentNHibernateMappingAssemblies.Select(name => Assembly.Load(name)).ToArray();
            assembliesForOverriding = FluentNHibernateOverridingAssemblies.Select(name => Assembly.Load(name)).ToArray();

            var assemblyInfoList = assembliesForMapping.Union(assembliesForOverriding).Select(a => new FileInfo(a.Location)).ToList();
            var assemblyLastModified = assemblyInfoList.OrderByDescending(a => a.LastWriteTime).FirstOrDefault();
            currentPath = assemblyLastModified.DirectoryName;

            if (assemblyLastModified == null)
                throw new DantasException("Assemblies for mapping entities is not configured yet.");

            var formatter = new BinaryFormatter();
            var cache = Path.Combine(assemblyLastModified.DirectoryName, "config.cache");

            if (File.Exists(cache))
            {
                var cacheInfo = new FileInfo(cache);
                needCreateConfig = assemblyLastModified.LastWriteTime > cacheInfo.LastWriteTime;
            }

            if (needCreateConfig)
            {
                Fluently.Configure(config)
                    .Mappings(m =>
                    {
                        m.MergeMappings();

                        if (UsingAutoMap)
                        {
                            AutoPersistenceModel model;

                            if (AutoMapConfig == null)
                                AutoMapConfig = new DantasAutomappingConfiguration();
                            if (ConventionBuilder == null)
                                ConventionBuilder = new ConventionBuilder();

                            m.AutoMappings.Add(
                                model = AutoMap.Assemblies(AutoMapConfig, assembliesForMapping)
                                .Conventions.Setup(c =>
                                    ConventionBuilder.Build(c)
                                )
                            );

                            foreach (var assembly in assembliesForOverriding)
                            {
                                model.UseOverridesFromAssembly(assembly);
                            }

                            if (ExportMappings)
                                m.AutoMappings.ExportTo(assemblyLastModified.DirectoryName);
                        }
                        else
                        {
                            foreach (var assembly in assembliesForMapping)
                            {
                                m.HbmMappings.AddFromAssembly(assembly);

                                m.FluentMappings.AddFromAssembly(assembly);
                            }
                        }
                    }
                ).BuildConfiguration();

                using (var stream = new FileStream(cache, FileMode.Create))
                {
                    formatter.Serialize(stream, config);
                }
            }
        }

        /// <summary>
        /// Create new session factory with new or cached config.
        /// </summary>
        /// <param name="config">Config from Spring</param>
        /// <returns>Session factory</returns>
        protected override global::NHibernate.ISessionFactory NewSessionFactory(Configuration config)
        {
            global::NHibernate.ISessionFactory sf = null;
            if (needCreateConfig)
                sf = base.NewSessionFactory(config);
            else
            {
                var formatter = new BinaryFormatter();
                var cache = Path.Combine(currentPath, "config.cache");
                using (var stream = new FileStream(cache, FileMode.Open))
                {
                    var cachedConfig = (Configuration)formatter.Deserialize(stream);
                    sf = base.NewSessionFactory(cachedConfig);
                }
            }

            return sf;
        }
    }
}
