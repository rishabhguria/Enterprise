using Prana.LogManager;
// ***********************************************************************
// Assembly         : Prana.Utilities
// Author           : dewashish
// Created          : 09-10-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-10-2014
// ***********************************************************************
// <copyright file="PredicateBuilder.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Linq.Expressions;

/// <summary>
/// The PredicateHelper namespace.
/// </summary>
namespace Prana.Utilities.PredicateHelper
{
    /// <summary>
    /// Class PredicateBuilder.
    /// </summary>
    public static class PredicateBuilder
    {



        /// <summary>
        /// Ands the specified first expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="firstExpression">The first expression.</param>
        /// <param name="secondExpression">The second expression.</param>
        /// <returns>Expression&lt;Func&lt;T, System.Boolean&gt;&gt;.</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> firstExpression, Expression<Func<T, bool>> secondExpression)
        {

            try
            {
                ParameterExpression p = firstExpression.Parameters[0];

                SubsetExpressionVisitor visitor = new SubsetExpressionVisitor();
                visitor.subset[secondExpression.Parameters[0]] = p;

                Expression body = Expression.AndAlso(firstExpression.Body, visitor.Visit(secondExpression.Body));
                return Expression.Lambda<Func<T, bool>>(body, p);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }


        /// <summary>
        /// Ors the specified first expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="firstExpression">The first expression.</param>
        /// <param name="secondExpression">The second expression.</param>
        /// <returns>Expression&lt;Func&lt;T, System.Boolean&gt;&gt;.</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> firstExpression, Expression<Func<T, bool>> secondExpression)
        {

            try
            {
                ParameterExpression p = firstExpression.Parameters[0];

                SubsetExpressionVisitor visitor = new SubsetExpressionVisitor();
                visitor.subset[secondExpression.Parameters[0]] = p;

                Expression body = Expression.OrElse(firstExpression.Body, visitor.Visit(secondExpression.Body));
                return Expression.Lambda<Func<T, bool>>(body, p);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }
    }
}
