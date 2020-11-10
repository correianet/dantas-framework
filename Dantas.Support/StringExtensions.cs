using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace Dantas.Support
{
    /// <summary>
    /// Extends string type.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Create a LINQ expression with string member expression.
        /// </summary>
        /// <typeparam name="T">Type of member expression.</typeparam>
        /// <param name="expression">Expression member.</param>
        /// <returns>LINQ ready-to-use member expression</returns>
        public static MemberExpression ToExpression<T>(this string expression)
        {
            string[] props = expression.Split('.');

            //Should be "e" for compatibility on NH3.1
            ParameterExpression p = Expression.Parameter(typeof(T), "e");

            Type type = p.Type;
            Expression expr = p;
            foreach (string prop in props)
            {
                PropertyInfo pi = type.GetProperty(prop);

                if (pi == null)
                    throw new ArgumentException("The expression contains a invalid property. PropertyName: " + prop, expression);

                expr = Expression.MakeMemberAccess(expr, pi);
                type = pi.PropertyType;
            }

            return (MemberExpression)expr;
        }
    }
}
