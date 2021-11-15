using DynamicFilter.Common;
using DynamicFilter.Common.Interfaces;
using System.Linq.Expressions;

namespace DynamicFilter.Operations
{
    /// <summary>
    /// Operation representing an equality comparison.
    /// </summary>
    public class EqualTo : AbstractOperation
    {
        /// <inheritdoc />
        public EqualTo()
            : base("EqualTo", 1, TypeGroup.Default | TypeGroup.Boolean | TypeGroup.Date | TypeGroup.Number | TypeGroup.Text) { }

        /// <inheritdoc />
        public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
        {
            if (member.Type == typeof(string))
            {
                var toLowerExpression = constant1.ConvertConstantToPropertyOrField().TrimToLower();

                return Expression.Equal(member.TrimToLower(), toLowerExpression)
                       .AddNullCheck(member);
            }

            return Expression.Equal(member, constant1.ConvertConstantToPropertyOrField());
        }
    }
}