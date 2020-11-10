using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

namespace Dantas.Core
{
    /// <summary>
    /// Exception for validation results in entity analysis.
    /// </summary>
    [Serializable]
    public class DomainPolicyException : DantasException
    {
        const string messageDefault = "An error occurred while performing a domain operation.";

        /// <summary>
        /// Validation results in exception.
        /// </summary>
        public ReadOnlyCollection<ValidationResult> ValidationResults { get; private set; }

        /// <summary>
        /// New exception for domain policy constraint.
        /// </summary>
        public DomainPolicyException() { }

        /// <summary>
        /// New exception for domain policy constraint.
        /// </summary>
        /// <param name="results"></param>
        public DomainPolicyException(ValidationResults results)
            : base(messageDefault)
        {
            this.ValidationResults = results.AsQueryable().ToList().AsReadOnly();
        }

        /// <summary>
        /// New exception for domain policy constraint.
        /// </summary>
        /// <param name="results"></param>
        /// <param name="inner"></param>
        public DomainPolicyException(ValidationResults results, Exception inner)
            : base(messageDefault, inner)
        {
            this.ValidationResults = results.AsQueryable().ToList().AsReadOnly();
        }

        /// <summary>
        /// New exception for domain policy constraint.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected DomainPolicyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
