﻿using DynamicFilter.Common;
using DynamicFilter.Common.Interfaces;
using System.Linq.Expressions;

namespace DynamicFilter.Operations
{
    /// <summary>
    /// Operation representing a check for a non-empty string.
    /// </summary>
    public class IsNotEmpty : AbstractOperation
    {
        /// <inheritdoc />
        public IsNotEmpty()
            : base("IsNotEmpty", 0, TypeGroup.Text) { }

        /// <inheritdoc />
        public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
        {
            return Expression.NotEqual(member.TrimToLower(), Expression.Constant(string.Empty))
                   .AddNullCheck(member);
        }
    }
}