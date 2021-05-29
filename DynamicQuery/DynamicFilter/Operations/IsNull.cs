using DynamicFilter.Common;
using DynamicFilter.Common.Interfaces;
using System.Linq.Expressions;

namespace DynamicFilter.Operations
{
    /// <summary>
    /// Operation representing a null check.
    /// </summary>
    public class IsNull : AbstractOperation
    {
        /// <inheritdoc />
        public IsNull()
            : base("IsNull", 0, TypeGroup.Text | TypeGroup.Nullable, expectNullValues: true) { }

        /// <inheritdoc />
        public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
        {
            return Expression.Equal(member, Expression.Constant(null));
        }
    }
}