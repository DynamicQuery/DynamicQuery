﻿using DynamicFilter.Common;
using DynamicFilter.Common.Interfaces;
using System.Linq.Expressions;

namespace DynamicFilter.Operations
{
    /// <summary>
    /// Operation representing a "not null nor whitespace" check.
    /// </summary>
    public class IsNotNullNorWhiteSpace : AbstractOperation
    {
        /// <inheritdoc />
        public IsNotNullNorWhiteSpace()
            : base("IsNotNullNorWhiteSpace", 0, TypeGroup.Text) { }

        /// <inheritdoc />
        public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
        {
            Expression exprNull = Expression.Constant(null);
            Expression exprEmpty = Expression.Constant(string.Empty);
            return Expression.AndAlso(
                Expression.NotEqual(member, exprNull),
                Expression.NotEqual(member.TrimToLower(), exprEmpty));
        }
    }
}