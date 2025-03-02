﻿using DynamicFilter.Common;
using DynamicFilter.Common.Interfaces;
using System;
using System.Collections;
using System.Linq.Expressions;

namespace DynamicFilter.Operations
{
    /// <summary>
    /// Operation representing the inverse of a list "Contains" method call.
    /// </summary>
    public class NotIn : AbstractOperation
    {
        /// <inheritdoc />
        public NotIn()
            : base("NotIn", 1, TypeGroup.Default | TypeGroup.Boolean | TypeGroup.Date | TypeGroup.Number | TypeGroup.Text, true, true) { }

        /// <inheritdoc />
        public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
        {
            if (!(constant1.Value is IList) || !constant1.Value.GetType().IsGenericType)
            {
                throw new ArgumentException("The 'NotIn' operation only supports lists as parameters.");
            }

            var type = constant1.Value.GetType();
            var inInfo = type.GetMethod("Contains", new[] { type.GetGenericArguments()[0] });
            var contains = Expression.Call(constant1, inInfo, member);
            return Expression.Not(contains);
        }
    }
}