using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;

namespace Dantas.Data.NHibernate
{
    /// <summary>
    /// Implements this for container the infrastructure of conventions.
    /// </summary>
    public interface ISetupConvention
    {
        /// <summary>
        /// Builder method
        /// </summary>
        /// <param name="finder">The finder convention.</param>
        void Build(IConventionFinder finder);
    }
}
