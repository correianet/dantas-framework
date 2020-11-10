using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Helpers;

namespace Dantas.Data.NHibernate
{
    /// <summary>
    /// Builder configuration for auto mapping entities
    /// </summary>
    public class ConventionBuilder : ISetupConvention
    {
        /// <summary>
        /// Map the convention.
        /// </summary>
        /// <param name="finder">Convention finder.</param>
        public virtual void Build(IConventionFinder finder)
        {
            finder.Add<PrimaryKeyConvention>();
            finder.Add<PrimaryKeyGuidConvention>();
            finder.Add(DefaultCascade.None());
            finder.Add(ForeignKey.EndsWith("Id"));
        }
    }
}
