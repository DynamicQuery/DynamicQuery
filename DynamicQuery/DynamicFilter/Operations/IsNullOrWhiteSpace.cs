﻿using DynamicFilter.Common;
using DynamicFilter.Common.Interfaces;
using System.Linq.Expressions;

namespace DynamicFilter.Operations
{
    /// <summary>
    /// Operation representing a "null or whitespace" check.
    /// </summary>
    public class IsNullOrWhiteSpace : AbstractOperation
    {
        /// <inheritdoc />
        public IsNullOrWhiteSpace()
            : base("IsNullOrWhiteSpace", 0, TypeGroup.Text, expectNullValues: true) { }

        /// <inheritdoc />
        public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
        {
            Expression exprNull = Expression.Constant(null);
            Expression exprEmpty = Expression.Constant(string.Empty);
            return Expression.OrElse(
                Expression.Equal(member, exprNull),
                Expression.AndAlso(
                    Expression.NotEqual(member, exprNull),
                    Expression.Equal(member.TrimToLower(), exprEmpty)));
        }
    }
}