using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace Dantas.Data
{
    /// <summary>
    /// Extends LINQ funcionality
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// Apply processing to order by expression in sorter.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="source">Queryable object.</param>
        /// <param name="sorter">Sorter expression.</param>
        /// <returns>Queryable object with order strategy.</returns>
        public static IOrderedQueryable<T> SortingBy<T>(this IQueryable<T> source, string sorter)
        {
            var properties = sorter.Split(',').Select(s => s.Trim());
            bool first = true;

            foreach (var item in properties)
            {
                var property = item;
                var reverse = property.ToUpper().EndsWith(" DESC");
                if (reverse)
                    property = property.Remove(property.Length - 5);
                else if (property.ToUpper().EndsWith(" ASC"))
                    property = property.Remove(property.Length - 4);

                if (!reverse && first)
                {
                    source = source.OrderBy(property);
                    first = false;
                }
                else if (reverse && first)
                {
                    source = source.OrderByDescending(property);
                    first = false;
                }
                else if (!reverse && !first)
                {
                    source = ((IOrderedQueryable<T>)source).ThenBy(property);
                }
                else
                {
                    source = ((IOrderedQueryable<T>)source).ThenByDescending(property);
                }
            }

            return (IOrderedQueryable<T>)source;
        }

        /// <summary>
        /// Order by with string property
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="source">Queryable object.</param>
        /// <param name="property">Property name.</param>
        /// <returns>Queryable object with order strategy.</returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "OrderBy");
        }

        /// <summary>
        /// Order by descending with string property
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="source">Queryable object.</param>
        /// <param name="property">Property name.</param>
        /// <returns>Queryable object with order strategy.</returns>
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "OrderByDescending");
        }

        /// <summary>
        /// Then by with string property
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="source">Queryable object.</param>
        /// <param name="property">Property name.</param>
        /// <returns>Queryable object with order strategy.</returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenBy");
        }

        /// <summary>
        /// Then by descending with string property
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="source">Queryable object.</param>
        /// <param name="property">Property name.</param>
        /// <returns>Queryable object with order strategy.</returns>
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenByDescending");
        }

        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "e");
            Expression expr = arg;
            foreach (string prop in props)
            {
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }
    }
}
