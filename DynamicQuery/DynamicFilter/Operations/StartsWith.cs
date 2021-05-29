using DynamicFilter.Common;
using DynamicFilter.Common.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace DynamicFilter.Operations
{
    /// <summary>
    /// Operation representing a string "StartsWith" method call.
    /// </summary>
    public class StartsWith : AbstractOperation
    {
        private readonly MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });

        /// <inheritdoc />
        public StartsWith()
            : base("StartsWith", 1, TypeGroup.Text) { }

        /// <inheritdoc />
        public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
        {
            Expression constant = constant1.TrimToLower();

            return Expression.Call(member.TrimToLower(), startsWithMethod, constant)
                   .AddNullCheck(member);
        }
    }
}