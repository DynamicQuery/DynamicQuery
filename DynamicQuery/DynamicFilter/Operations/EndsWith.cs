﻿using DynamicFilter.Common;
using DynamicFilter.Common.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace DynamicFilter.Operations
{
    /// <summary>
    /// Operation representing a string "EndsWith" method call.
    /// </summary>
    public class EndsWith : AbstractOperation
    {
        private readonly MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });

        /// <inheritdoc />
        public EndsWith()
            : base("EndsWith", 1, TypeGroup.Text) { }

        /// <inheritdoc />
        public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
        {
            Expression constant = constant1.TrimToLower();

            return Expression.Call(member.TrimToLower(), endsWithMethod, constant)
                   .AddNullCheck(member);
        }
    }
}