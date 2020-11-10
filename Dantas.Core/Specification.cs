using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Dantas.Core
{
    /// <summary>
    /// Specification rule for an entity.
    /// </summary>
    /// <typeparam name="T">Type of entity.</typeparam>
    public class Specification<T>
    {
        /// <summary>
        /// Linq expression.
        /// </summary>
        public Expression<Func<T, bool>> Predicate { get; internal set; }

        /// <summary>
        /// Lambda expression to use in lambda
        /// </summary>
        /// <param name="predicate"></param>
        public Specification(Expression<Func<T, bool>> predicate)
        {
            this.Predicate = predicate;
        }

        /// <summary>
        /// Validate if the avaliable instance is not invalid state.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool IsSatisfiedBy(T entity)
        {
            return Predicate.Compile().Invoke(entity);
        }

        #region Operator overloads

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftHand"></param>
        /// <param name="rightHand"></param>
        /// <returns></returns>
        public static Specification<T> operator &(Specification<T> leftHand, Specification<T> rightHand)
        {
            InvocationExpression rightInvoke = Expression.Invoke(rightHand.Predicate,
                                                                 leftHand.Predicate.Parameters.Cast<Expression>());
            BinaryExpression newExpression = Expression.MakeBinary(ExpressionType.AndAlso, leftHand.Predicate.Body,
                                                                   rightInvoke);
            return new Specification<T>(
                Expression.Lambda<Func<T, bool>>(newExpression, leftHand.Predicate.Parameters)
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftHand"></param>
        /// <param name="rightHand"></param>
        /// <returns></returns>
        public static Specification<T> operator |(Specification<T> leftHand, Specification<T> rightHand)
        {
            InvocationExpression rightInvoke = Expression.Invoke(rightHand.Predicate,
                                                                 leftHand.Predicate.Parameters.Cast<Expression>());
            BinaryExpression newExpression = Expression.MakeBinary(ExpressionType.OrElse, leftHand.Predicate.Body,
                                                                   rightInvoke);
            return new Specification<T>(
                Expression.Lambda<Func<T, bool>>(newExpression, leftHand.Predicate.Parameters)
                );
        }

        #endregion
    }
}
