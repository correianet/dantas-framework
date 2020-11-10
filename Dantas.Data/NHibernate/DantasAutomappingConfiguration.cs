using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dantas.Core;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Steps;

namespace Dantas.Data.NHibernate
{
    /// <summary>
    /// Configuration for default auto mapping convention.
    /// </summary>
    public class DantasAutomappingConfiguration : DefaultAutomappingConfiguration
    {
        /// <summary>
        /// Infra method.
        /// </summary>
        /// <param name="type">Type element</param>
        /// <returns>Infra return.</returns>
        public override bool ShouldMap(Type type)
        {
            return type.IsSubclassOf(typeof(EntityBase)) || type.IsSubclassOf(typeof(ValueObject));
        }

        /// <summary>
        /// Infra method.
        /// </summary>
        /// <param name="type">Type element</param>
        /// <returns>Infra return.</returns>
        public override bool IsComponent(Type type)
        {
            return type.IsSubclassOf(typeof(ValueObject));
        }
    }
}
