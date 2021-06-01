using DynamicFilter.Common;
using DynamicFilter.Common.Exceptions;
using DynamicFilter.Common.Helpers;
using DynamicFilter.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DynamicFilter
{
    internal class DynamicFilterBuilder
    {
        private readonly INestedListsHelper _nestedListHelper;

        internal DynamicFilterBuilder()
        {
            _nestedListHelper = new NestedListsHelper();
        }

        public Expression<Func<T, bool>> GetExpression<T>(IDynamicFilter filter) where T : class
        {
            var outerMostParam = Expression.Parameter(typeof(T), "x");
            Expression expression = null;

            //StatementGroups are statements enclosed within brackets
            foreach (var statementGroup in filter.StatementGroups)
            {
                Connector statementGroupConnector = Connector.And;

                if (statementGroup.Any())
                {
                    statementGroupConnector = statementGroup.First().Connector == Connector.None ? Connector.And : statementGroup.First().Connector;
                }

                Expression partialExpr = GetPartialExpression(outerMostParam, statementGroupConnector, statementGroup);

                expression = expression == null ? partialExpr : CombineExpressions(expression, partialExpr, statementGroupConnector);
                //connector = statementGroupConnector;
            }

            expression = expression ?? Expression.Constant(true);

            return Expression.Lambda<Func<T, bool>>(expression, outerMostParam);
        }

        private Expression GetExpressionFromStatement(ParameterExpression param, IDynamicFilterStatement statement, string propertyName = null)
        {
            Expression resultExpr = null;
            var memberName = propertyName ?? statement.PropertyId;

            if (_nestedListHelper.IsAnImmediateList(memberName))
            {
                statement.PropertyId = memberName;
                return ProcessListStatements(param, new List<IDynamicFilterStatement>() { statement });
            }
            else
            {
                if (_nestedListHelper.PropertyIdContainsList(memberName))
                {
                    statement.PropertyId = memberName;
                    return ProcessListStatements(param, new List<IDynamicFilterStatement>() { statement });
                }
                else
                {
                    MemberExpression member = param.GetMemberExpression(memberName);
                    if (Nullable.GetUnderlyingType(member.Type) != null && statement.Value != null)
                    {
                        resultExpr = Expression.Property(member, "HasValue");
                        member = Expression.Property(member, "Value");
                    }

                    var constant1 = Expression.Constant(statement.Value);
                    var constant2 = Expression.Constant(statement.Value2);

                    CheckPropertyValueMismatch(member, constant1);

                    var safeStringExpression = statement.Operation.GetExpression(member, constant1, constant2);
                    resultExpr = resultExpr != null ? Expression.AndAlso(resultExpr, safeStringExpression) : safeStringExpression;
                    resultExpr = GetSafePropertyMember(param, memberName, resultExpr);

                    if (statement.Operation.ExpectNullValues && memberName.Contains("."))
                    {
                        resultExpr = Expression.OrElse(CheckIfParentIsNull(param, memberName), resultExpr);

                        return resultExpr;
                    }

                    return resultExpr;
                }
            }
        }
        private Expression GetPartialExpression(ParameterExpression param, Connector connector, IEnumerable<IDynamicFilterStatement> statementGroup)
        {
            List<List<IDynamicFilterStatement>> statementSets = _nestedListHelper.GroupFilterStatements(statementGroup);
            Expression expression = null;

            foreach (List<IDynamicFilterStatement> statementSet in statementSets)
            {
                connector = statementSet.First().Connector;

                Expression expr = null;
                bool doesStatementSetDealWithLists = _nestedListHelper.PropertyIdContainsList(statementSet.First().PropertyId);

                if (doesStatementSetDealWithLists)
                {
                    expr = ProcessListStatements(param, statementSet);
                    expression = expression == null ? expr : CombineExpressions(expression, expr, connector);
                }
                else
                {
                    foreach (IDynamicFilterStatement statement in statementSet)
                    {
                        connector = statement.Connector;
                        expr = GetExpressionFromStatement(param, statement);
                        expression = expression == null ? expr : CombineExpressions(expression, expr, connector);
                    }
                }
            }

            return expression;
        }
        /*
                private bool IsList(IFilterStatement statement)
                {
                    return statement.PropertyId.Contains("[") && statement.PropertyId.Contains("]");
                }
        */

        private Expression CombineExpressions(Expression expr1, Expression expr2, Connector connector)
        {
            return connector == Connector.And ? Expression.AndAlso(expr1, expr2) : Expression.OrElse(expr1, expr2);
        }

        private Expression ProcessListStatements(ParameterExpression param, List<IDynamicFilterStatement> statements)
        {
            string firstStatementPropertyId = statements.First().PropertyId;
            var basePropertyListName = firstStatementPropertyId.Substring(0, statements.First().PropertyId.IndexOf("["));
            MemberExpression memberExpression = param.GetMemberExpression(basePropertyListName);
            Type genericType = null;

            foreach (var p in basePropertyListName.Split(".", StringSplitOptions.RemoveEmptyEntries))
            {
                if (genericType == null)
                {
                    genericType = param.Type.GetProperty(p).PropertyType;
                }
                else
                {
                    genericType = genericType.GetProperty(p).PropertyType;
                }
            }

            genericType = genericType.GetGenericArguments().First();
            ParameterExpression listItemParam = Expression.Parameter(genericType, "i" + new Random().Next(999).ToString());
            List<Tuple<Connector, Expression>> innerExpressionsWithConnectors = new List<Tuple<Connector, Expression>>();

            foreach (var statement in statements)
            {
                string propertyId = statement.PropertyId;
                string remainingPropertyId = propertyId.Substring(statement.PropertyId.IndexOf("[") + 1);
                remainingPropertyId = remainingPropertyId.Remove(remainingPropertyId.Length - 1);
                Expression innerExpression = GetExpressionFromStatement(listItemParam, statement, remainingPropertyId);
                Tuple<Connector, Expression> tuple = new Tuple<Connector, Expression>(statement.Connector, innerExpression);
                innerExpressionsWithConnectors.Add(tuple);
            }

            BinaryExpression binaryExpression = Expression.AndAlso(Expression.Constant(true), Expression.Constant(true));
            for (int i = 0; i < innerExpressionsWithConnectors.Count; i++)
            {
                var tuple = innerExpressionsWithConnectors[i];
                if (i == 0 || tuple.Item1 == Connector.And)
                {
                    binaryExpression = Expression.AndAlso(binaryExpression, tuple.Item2);
                }
                else
                {
                    binaryExpression = Expression.OrElse(binaryExpression, tuple.Item2);
                }
            }

            LambdaExpression lambdaExpression = Expression.Lambda(binaryExpression, listItemParam);
            Type enumerableType = typeof(Enumerable);
            MethodInfo anyInfo = enumerableType.GetMethods(BindingFlags.Static | BindingFlags.Public).First(m => m.Name == "Any" && m.GetParameters().Count() == 2);
            anyInfo = anyInfo.MakeGenericMethod(genericType);
            return Expression.Call(anyInfo, memberExpression, lambdaExpression);
        }


        private void CheckPropertyValueMismatch(MemberExpression member, ConstantExpression constant1)
        {
            var memberType = member.Member.MemberType == MemberTypes.Property ? (member.Member as PropertyInfo).PropertyType : (member.Member as FieldInfo).FieldType;

            var constant1Type = GetConstantType(constant1);
            var nullableType = constant1Type != null ? Nullable.GetUnderlyingType(constant1Type) : null;

            var constantValueIsNotNull = constant1.Value != null;
            var memberAndConstantTypeDoNotMatch = nullableType == null && memberType != constant1Type;
            var memberAndNullableUnderlyingTypeDoNotMatch = nullableType != null && memberType != nullableType;

            if (constantValueIsNotNull && (memberAndConstantTypeDoNotMatch || memberAndNullableUnderlyingTypeDoNotMatch))
            {
                throw new PropertyValueTypeMismatchException(member.Member.Name, memberType.Name, constant1.Type.Name);
            }
        }

        private Type GetConstantType(ConstantExpression constant)
        {
            if (constant != null && constant.Value != null && constant.Value.IsGenericList())
            {
                return constant.Value.GetType().GenericTypeArguments[0];
            }

            return constant != null && constant.Value != null ? constant.Value.GetType() : null;
        }

        public Expression GetSafePropertyMember(ParameterExpression param, string memberName, Expression expr)
        {
            if (!memberName.Contains("."))
            {
                return expr;
            }

            var index = memberName.LastIndexOf(".", StringComparison.InvariantCulture);
            var parentName = memberName.Substring(0, index);
            var subParam = param.GetMemberExpression(parentName);
            var resultExpr = Expression.AndAlso(Expression.NotEqual(subParam, Expression.Constant(null)), expr);
            return GetSafePropertyMember(param, parentName, resultExpr);
        }

        protected Expression CheckIfParentIsNull(ParameterExpression param, string memberName)
        {
            var parentMember = GetParentMember(param, memberName);
            return Expression.Equal(parentMember, Expression.Constant(null));
        }

        private MemberExpression GetParentMember(ParameterExpression param, string memberName)
        {
            var parentName = memberName.Substring(0, memberName.IndexOf("."));
            return param.GetMemberExpression(parentName);
        }
    }
}
