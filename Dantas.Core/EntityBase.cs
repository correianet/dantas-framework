using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Dantas.Core
{
    /// <summary>
    /// Infrastructure base class for generic persistent entities, with equals and comparable strategy.
    /// </summary>
    public class EntityBase : IComparable<EntityBase>
    {
        private int? cachedHashcode;
        private const int HASH_MULTIPLIER = 31;

        /// <summary>
        /// ORM only.
        /// </summary>
        protected object id;

        /// <summary>
        /// Type without proxy generation of ORM.
        /// </summary>
        /// <returns>Original type.</returns>
        protected virtual Type GetTypeUnproxied()
        {
            return GetType();
        }

        /// <summary>
        /// Verify if instances are same.
        /// </summary>
        /// <param name="obj">Other instance.</param>
        /// <returns>True for valid.</returns>
        public override bool Equals(object obj)
        {
            var compareTo = obj as EntityBase;

            if (ReferenceEquals(this, compareTo))
                return true;

            if (compareTo == null || !GetType().Equals(compareTo.GetTypeUnproxied()))
                return false;

            if (HasSameNonDefaultIdAs(compareTo))
                return true;

            return IsTransient() && compareTo.IsTransient() &&
                HasSameObjectSignatureAs(compareTo);
        }

        /// <summary>
        /// Indicates if entity is persisted or run-time associated.
        /// </summary>
        /// <returns>True for run-time object, otherwise false.</returns>
        public virtual bool IsTransient()
        {
            return id == null;
        }

        /// <summary>
        /// Hash for current instance.
        /// </summary>
        /// <returns>Hashcode.</returns>
        public override int GetHashCode()
        {
            if (cachedHashcode.HasValue)
                return cachedHashcode.Value;

            if (IsTransient())
            {
                cachedHashcode = base.GetHashCode();
            }
            else
            {
                unchecked
                {
                    int hashCode = GetType().GetHashCode();
                    cachedHashcode = (hashCode * HASH_MULTIPLIER) ^ id.GetHashCode();
                }
            }

            return cachedHashcode.Value;
        }

        /// <summary>
        /// Validate hash code signature for transient objects.
        /// </summary>
        /// <param name="compareTo">Transient to compare.</param>
        /// <returns>True if this equals.</returns>
        protected virtual bool HasSameObjectSignatureAs(EntityBase compareTo)
        {
            if (compareTo == null)
                throw new ArgumentNullException("compareTo");

            return GetHashCode().Equals(compareTo.GetHashCode());
        }

        /// <summary>
        /// Validate id signature for persistent objects.
        /// </summary>
        /// <param name="compareTo">Persistent to compare.</param>
        /// <returns>True if this equals.</returns>
        protected virtual bool HasSameNonDefaultIdAs(EntityBase compareTo)
        {
            return !IsTransient() &&
                  !compareTo.IsTransient() &&
                  id.Equals(compareTo.id);
        }

        /// <summary>
        /// Perform validation.
        /// </summary>
        /// <returns>Return validation result set.</returns>
        protected virtual ValidationResults Validate()
        {
            return Validate(String.Empty);
        }

        /// <summary>
        /// Perform validation.
        /// </summary>
        /// <param name="ruleSetName">Name of rule set where want perform validation.</param>
        /// <returns>Return validation result set.</returns>
        protected virtual ValidationResults Validate(string ruleSetName)
        {
            var results = new ValidationResults();

            results.AddAllResults(ValidationFactory.CreateValidator(GetTypeUnproxied()).Validate(this));

            if (!String.IsNullOrEmpty(ruleSetName))
                results.AddAllResults(ValidationFactory.CreateValidator(GetTypeUnproxied(), ruleSetName).Validate(this));

            return results;
        }

        /// <summary>
        /// Perform validation.
        /// </summary>
        /// <exception cref="Dantas.Core.DomainPolicyException">Exception for invalid state.</exception>
        protected virtual void ValidateAndThrow()
        {
            ValidateAndThrow(String.Empty);
        }

        /// <summary>
        /// Perform validation.
        /// </summary>
        /// <exception cref="Dantas.Core.DomainPolicyException">Exception for invalid state.</exception>
        protected virtual void ValidateAndThrow(string ruleSetName)
        {
            var result = this.Validate(ruleSetName);

            if (!result.IsValid)
                throw new DomainPolicyException(result);
        }

        /// <summary>
        /// Enable validate a value object when this is changed by external elements and raise an event.
        /// </summary>
        /// <param name="vo"></param>
        protected virtual void ConfigureListener(ValueObject vo)
        {
            vo.StateChanged += new EventHandler<DomainStateChangedEventArgs>(ComponentStateChanged);
        }

        /// <summary>
        /// Validation by DomainState event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event args.</param>
        protected virtual void ComponentStateChanged(object sender, DomainStateChangedEventArgs e)
        {
            ValidateAndThrow();
        }

        #region IComparable<EntityBase> Members

        /// <summary>
        /// Compare this instance with other.
        /// </summary>
        /// <param name="other">Compared object.</param>
        /// <returns>Compare result.</returns>
        public virtual int CompareTo(EntityBase other)
        {
            if (other == null)
                throw new ArgumentNullException("compareTo");

            var otherEntity = other as EntityBase;
            if (otherEntity == null)
                throw new ArgumentException("compareTo");

            if (!(id is IComparable))
                throw new NotSupportedException("Id not cast to IComparable");

            return ((IComparable)id).CompareTo(otherEntity.id);
        }

        #endregion
    }
}
