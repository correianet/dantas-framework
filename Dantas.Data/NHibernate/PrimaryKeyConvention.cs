using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using Dantas.Core;

namespace Dantas.Data.NHibernate
{
    /// <summary>
    /// Conventions for auto mapping.
    /// </summary>
    public class PrimaryKeyConvention : IIdConvention, IIdConventionAcceptance
    {
        /// <summary>
        /// Apply convention.
        /// </summary>
        /// <param name="instance">Instance convention.</param>
        public void Apply(IIdentityInstance instance)
        {
            //TODO: Review this point after upgrade FluentNHibernate to 1.3 or after
            instance.CustomType<IdentityType>();
            instance.Access.LowerCaseField();
            instance.UnsavedValue("null");
            instance.GeneratedBy.Assigned();
        }

        /// <summary>
        /// Accept convention.
        /// </summary>
        /// <param name="criteria">Criteria for accept.</param>
        public void Accept(IAcceptanceCriteria<IIdentityInspector> criteria)
        {
            criteria.Expect(i => i.Type.GetUnderlyingSystemType().IsSubclassOf(typeof(Identity)));
        }
    }
}
