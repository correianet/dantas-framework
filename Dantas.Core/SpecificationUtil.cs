using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dantas.Support;

namespace Dantas.Core
{
    /// <summary>
    /// Class to help handle <see cref="Dantas.Core.Specification{T}"/>.
    /// </summary>
    public static class SpecificationUtil
    {
        /// <summary>
        /// Create specification by string parameters.
        /// </summary>
        /// <param name="searchField">Property name.</param>
        /// <param name="searchOperator">Operator type.</param>
        /// <param name="searchString">Query value.</param>
        /// <typeparam name="T">Class for specification handle.</typeparam>
        /// <returns>Specification object with predicate based on string parameters.</returns>
        public static Specification<T> CreateSpecificationBy<T>(string searchField, string searchOperator, string searchString)
        {
            Expression exp = null;

            //Should be "e" for compatibility on NH3.1
            var p = Expression.Parameter(typeof(T), "e");

            Expression propertyAccess = searchField.ToExpression<T>();
            var membertype = propertyAccess.Type;
            if (membertype.IsGenericType && membertype.GetGenericTypeDefinition() == typeof(Nullable<>))
                membertype = Nullable.GetUnderlyingType(membertype);

            switch (searchOperator.ToLower())
            {
                case "bw":
                    exp = Expression.Call(propertyAccess, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), Expression.Constant(searchString));
                    break;
                case "cn":
                    exp = Expression.Call(propertyAccess, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), Expression.Constant(searchString));
                    break;
                case "ew":
                    exp = Expression.Call(propertyAccess, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), Expression.Constant(searchString));
                    break;
                case "gt":
                    exp = Expression.GreaterThan(propertyAccess, Expression.Constant(Convert.ChangeType(searchString, membertype), propertyAccess.Type));
                    break;
                case "ge":
                    exp = Expression.GreaterThanOrEqual(propertyAccess, Expression.Constant(Convert.ChangeType(searchString, membertype), propertyAccess.Type));
                    break;
                case "lt":
                    exp = Expression.LessThan(propertyAccess, Expression.Constant(Convert.ChangeType(searchString, membertype), propertyAccess.Type));
                    break;
                case "le":
                    exp = Expression.LessThanOrEqual(propertyAccess, Expression.Constant(Convert.ChangeType(searchString, membertype), propertyAccess.Type));
                    break;
                case "eq":
                    object paramEq;
                    if (membertype.IsEnum)
                        paramEq = Enum.Parse(membertype, searchString);
                    else if (membertype == typeof(Guid))
                        paramEq = Guid.Parse(searchString);
                    else
                        paramEq = Convert.ChangeType(searchString, membertype);
                    exp = Expression.Equal(propertyAccess, Expression.Constant(paramEq, propertyAccess.Type));
                    break;
                case "ne":
                    object paramNe;
                    if (membertype.IsEnum)
                        paramNe = Enum.Parse(membertype, searchString);
                    else if (membertype == typeof(Guid))
                        paramNe = Guid.Parse(searchString);
                    else
                        paramNe = Convert.ChangeType(searchString, membertype);
                    exp = Expression.NotEqual(propertyAccess, Expression.Constant(paramNe, propertyAccess.Type));
                    break;
                default:
                    throw new DantasException("Operator not recognized. Use a correct parameter: eq, ne, bw, cn, ew, gt, ge, lt, le.");
            }

            return new Specification<T>(Expression.Lambda<Func<T, bool>>(exp, p));
        }
    }
}
