﻿using DynamicFilter.Common;
using DynamicFilter.Common.Interfaces;
using System.Linq.Expressions;

namespace DynamicFilter.Operations
{
    /// <summary>
    /// Operation representing a check for an empty string.
    /// </summary>
    public class IsEmpty : AbstractOperation
    {
        /// <inheritdoc />
        public IsEmpty()
            : base("IsEmpty", 0, TypeGroup.Text) { }

        /// <inheritdoc />
        public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
        {
            return Expression.Equal(member.TrimToLower(), Expression.Constant(string.Empty))
                   .AddNullCheck(member);
        }
    }
}