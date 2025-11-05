// ***********************************************************************
// Assembly         : Prana.Utilities
// Author           : dewashish
// Created          : 09-10-2014
//
// Last Modified By : dewashish
// Last Modified On : 09-10-2014
// ***********************************************************************
// <copyright file="SubsetExpressionVisitor.cs" company="Nirvana">
//     Copyright (c) Nirvana. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.Linq.Expressions;

/// <summary>
/// The PredicateHelper namespace.
/// </summary>
namespace Prana.Utilities.PredicateHelper
{
    /// <summary>
    /// Class SubstExpressionVisitor.
    /// </summary>
    public class SubsetExpressionVisitor : System.Linq.Expressions.ExpressionVisitor
    {
        /// <summary>
        /// The subst
        /// </summary>
        public Dictionary<Expression, Expression> subset = new Dictionary<Expression, Expression>();

        /// <summary>
        /// Visits the <see cref="T:System.Linq.Expressions.ParameterExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            Expression newValue;
            if (subset.TryGetValue(node, out newValue))
            {
                return newValue;
            }
            return node;
        }
    }
}
