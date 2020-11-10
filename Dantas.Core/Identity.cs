using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dantas.Core
{
    /// <summary>
    /// Extensions for identity.
    /// </summary>
    public static class IdentyExtensionForInt
    {
        /// <summary>
        /// Casting int to identity type.
        /// </summary>
        /// <typeparam name="T">Identity type.</typeparam>
        /// <param name="value">integer value.</param>
        /// <returns>Identity with integer value.</returns>
        public static T AsType<T>(this int value) where T : Identity, new()
        {
            var id = new T();
            id.Value = value;
            return id;
        }
    }

    /// <summary>
    /// Identity type for entities id.
    /// </summary>
    public class Identity : IComparable<Identity>, IComparable, IConvertible
    {
        internal int Value { get; set; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        public Identity() { }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="value">Id source.</param>
        public Identity(int value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="value">Id source.</param>
        public Identity(string value)
        {
            try
            {
                this.Value = Int32.Parse(value);
            }
            catch (Exception)
            {
                throw new InvalidCastException("Formato de string não reconhecido para identidade de objeto.");
            }
        }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="id">Id source.</param>
        public Identity(Identity id)
        {
            this.Value = id.Value;
        }

        /// <summary>
        /// Equals strategy for identity.
        /// </summary>
        /// <param name="obj">Other instance</param>
        /// <returns>Returns true if has the same signature, otherwise false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is int)
                return Value == (int)obj;

            var other = obj as Identity;

            if (other == null)
                return false;

            return this.Value.Equals(other.Value);
        }

        /// <summary>
        /// Get hashcode for identity value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Get string for identity value.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Value.ToString();
        }

        /// <summary>
        /// Get numeric identity value.
        /// </summary>
        /// <returns></returns>
        public int ToNumber()
        {
            return this.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Identity a, Identity b)
        {
            if (Object.ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.GetType() == b.GetType() && a.Value == b.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Identity a, Identity b)
        {
            return !(a == b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static explicit operator int(Identity m)
        {
            return m.ToNumber();
        }

        /// <summary>
        /// Parse element into identity.
        /// </summary>
        /// <param name="typeId">Type.</param>
        /// <param name="value">Value.</param>
        /// <returns>Parsed value.</returns>
        public static Identity Parse(Type typeId, Identity value)
        {
            return (Identity)typeId.GetConstructor(new Type[] { typeof(Identity) }).Invoke(new object[] { value });
        }

        /// <summary>
        /// Parse element into identity.
        /// </summary>
        /// <param name="typeId">Type.</param>
        /// <param name="value">Value.</param>
        /// <returns>Parsed value.</returns>
        public static Identity Parse(Type typeId, int value)
        {
            return (Identity)typeId.GetConstructor(new Type[] { typeof(int) }).Invoke(new object[] { value });
        }

        #region IComparable<Identity> Members

        /// <summary>
        /// Compare this instance with other.
        /// </summary>
        /// <param name="other">Compared object.</param>
        /// <returns>Compare result.</returns>
        public int CompareTo(Identity other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            if (GetType() != other.GetType())
                throw new InvalidOperationException(
                    String.Format("Não é possível comparar identity do tipo '{0}' com identity do tipo {1}", GetType(), other.GetType()));
            return Value.CompareTo(other.Value);
        }

        #endregion

        #region IComparable Members

        int IComparable.CompareTo(object obj)
        {
            if (obj is int)
                return Value.CompareTo(obj);

            var other = obj as Identity;

            return CompareTo(other);
        }

        #endregion

        #region IConvertible Members

        TypeCode IConvertible.GetTypeCode()
        {
            return this.Value.GetTypeCode();
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return (decimal)this.Value;
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return (double)this.Value;
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return this.Value;
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return (long)this.Value;
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return (float)this.Value;
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return this.Value.ToString();
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(this.Value, conversionType);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return (uint)this.Value;
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return (ulong)this.Value;
        }

        #endregion
    }
}
