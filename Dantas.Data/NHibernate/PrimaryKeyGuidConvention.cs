using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dantas.Data.NHibernate
{
    /// <summary>
    /// Conventions for auto mapping.
    /// </summary>
    public class PrimaryKeyGuidConvention : IIdConvention, IIdConventionAcceptance
    {
        /// <summary>
        /// Apply convention.
        /// </summary>
        /// <param name="instance">Instance convention.</param>
        public void Apply(IIdentityInstance instance)
        {
            instance.GeneratedBy.GuidComb();
        }

        /// <summary>
        /// Accept convention.
        /// </summary>
        /// <param name="criteria">Criteria for accept.</param>
        public void Accept(IAcceptanceCriteria<IIdentityInspector> criteria)
        {
            criteria.Expect(i => i.Type.GetUnderlyingSystemType() == typeof(Guid));
        }
    }
}
