using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Dantas.Core;
using NHibernate.SqlTypes;
using System.Data;
using NHibernate.UserTypes;

namespace Dantas.Data.NHibernate
{
    /// <summary>
    /// Identity translator for id identity type
    /// </summary>
    public class IdentityType : IUserType
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cached"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object Disassemble(object value)
        {
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object DeepCopy(object value)
        {
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsMutable
        {
            get { return false; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rs"></param>
        /// <param name="names"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            object value = NHibernateUtil.Int32.NullSafeGet(rs, names[0]);

            if (value == null)
            {
                return value;
            }
            else
            {
                return new Identity(Convert.ToInt32(value));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value == null)
            {
                NHibernateUtil.Int32.NullSafeSet(cmd, null, index);
            }
            else
            {
                var casted = (Identity)value;
                NHibernateUtil.Int32.NullSafeSet(cmd, casted.ToNumber(), index);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="original"></param>
        /// <param name="target"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        /// <summary>
        /// 
        /// </summary>
        public Type ReturnedType
        {
            get { return typeof(Identity); }
        }

        /// <summary>
        /// 
        /// </summary>
        public SqlType[] SqlTypes
        {
            get { return new SqlType[] { new SqlType(DbType.Int32) }; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public new bool Equals(object x, object y)
        {
            if (x == null)
            {
                return false;
            }
            else
            {
                return x.Equals(y);
            }
        }
    }
}
