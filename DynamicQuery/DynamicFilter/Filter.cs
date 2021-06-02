using DynamicFilter.Common;
using DynamicFilter.Common.Interfaces;
using ExpressionBuilderCore.Helpers;
using ExpressionBuilderCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DynamicFilter
{
    /// <summary>
    /// Aggregates <see cref="DynamicFilterStatement{TPropertyType}" /> and build them into a LINQ expression.
    /// </summary>
    /// <typeparam name="TClass"></typeparam>
    public class Filter<TClass> : IDynamicFilter where TClass : class
    {
        private readonly List<List<IDynamicFilterStatement>> _statementGroups;
        private readonly IOperationHelper _operationHelper;

        public Connector Connector { get; set; } = Connector.None;

        public IDynamicFilter Group
        {
            get
            {
                StartGroup();
                return this;
            }
        }

        /// <summary>
        /// List of <see cref="IDynamicFilterStatement" /> groups that will be combined and built into a LINQ expression.
        /// </summary>
        public IEnumerable<IEnumerable<IDynamicFilterStatement>> StatementGroups
        {
            get
            {
                return _statementGroups.ToArray();
            }
        }

        private List<IDynamicFilterStatement> LastStatementGroup
        {
            get
            {
                return _statementGroups.Last();
            }
        }

        /// <summary>
        /// Instantiates a new <see cref="Filter{TClass}" />
        /// </summary>
        public Filter()
        {
            _statementGroups = new List<List<IDynamicFilterStatement>> { new List<IDynamicFilterStatement>() };
            _operationHelper = new OperationHelper();
        }

        /// <summary>
        /// Adds a new <see cref="DynamicFilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
        /// (To be used by <see cref="IOperation" /> that need no values)
        /// </summary>
        /// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
        /// <param name="operation">Operation to be used.</param>
        /// <param name="connector"></param>
        /// <returns></returns>
        public IDynamicFilterStatementConnection By(string propertyId, IOperation operation, Connector connector)
        {
            return By<string>(propertyId, operation, null, null, connector);
        }

        /// <summary>
        /// Adds a new <see cref="DynamicFilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
        /// (To be used by <see cref="IOperation" /> that need no values)
        /// </summary>
        /// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
        /// <param name="operation">Operation to be used.</param>
        /// <returns></returns>
        public IDynamicFilterStatementConnection By(string propertyId, IOperation operation)
        {
            return By<string>(propertyId, operation, null, null, Connector.And);
        }

        /// <summary>
        /// Adds a new <see cref="DynamicFilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
        /// </summary>
        /// <typeparam name="TPropertyType"></typeparam>
        /// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
        /// <param name="operation">Operation to be used.</param>
        /// <param name="value"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
		public IDynamicFilterStatementConnection By<TPropertyType>(string propertyId, IOperation operation, TPropertyType value)
        {
            return By(propertyId, operation, value, default(TPropertyType));
        }

        /// <summary>
        /// Adds a new <see cref="DynamicFilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
        /// </summary>
        /// <typeparam name="TPropertyType"></typeparam>
        /// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
        /// <param name="operation">Operation to be used.</param>
        /// <param name="value"></param>
        /// <param name="value2"></param>
        /// <param name="connector"></param>
        /// <returns></returns>
		public IDynamicFilterStatementConnection By<TPropertyType>(string propertyId, IOperation operation, TPropertyType value, Connector connector)
        {
            return By(propertyId, operation, value, default, connector);
        }

        /// <summary>
        /// Adds a new <see cref="DynamicFilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
        /// </summary>
        /// <typeparam name="TPropertyType"></typeparam>
        /// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
        /// <param name="operation">Operation to be used.</param>
        /// <param name="value"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
		public IDynamicFilterStatementConnection By<TPropertyType>(string propertyId, IOperation operation, TPropertyType value, TPropertyType value2)
        {
            return By(propertyId, operation, value, value2, Connector.And);
        }

        /// <summary>
        /// Adds a new <see cref="DynamicFilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
        /// </summary>
        /// <typeparam name="TPropertyType"></typeparam>
        /// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
        /// <param name="operation">Operation to be used.</param>
        /// <param name="value"></param>
        /// <param name="value2"></param>
        /// <param name="connector"></param>
        /// <returns></returns>
		public IDynamicFilterStatementConnection By<TPropertyType>(string propertyId, IOperation operation, TPropertyType value, TPropertyType value2, Connector connector)
        {
            Connector finalChosenConnector = connector;
            if (Connector != Connector.None)
            {
                finalChosenConnector = Connector;
                Connector = Connector.None;

            }
            IDynamicFilterStatement statement = new DynamicFilterStatement<TPropertyType>(propertyId, operation, value, value2, finalChosenConnector);
            LastStatementGroup.Add(statement);
            return new DynamicFilterStatementConnection(this);
        }

        /// <summary>
        /// Starts a new group denoting that every subsequent filter statement should be grouped together (as if using a parenthesis).
        /// </summary>
        public void StartGroup()
        {
            if (LastStatementGroup.Any())
            {
                _statementGroups.Add(new List<IDynamicFilterStatement>());
            }
        }

        /// <summary>
        /// Removes all <see cref="DynamicFilterStatement{TPropertyType}" />, leaving the <see cref="Filter{TClass}" /> empty.
        /// </summary>
        public void Clear()
        {
            _statementGroups.Clear();
            _statementGroups.Add(new List<IDynamicFilterStatement>());
        }

        /// <summary>
        /// Implicitly converts a <see cref="Filter{TClass}" /> into a <see cref="Func{TClass, TResult}" />.
        /// </summary>
        /// <param name="filter"></param>
        public static implicit operator Func<TClass, bool>(Filter<TClass> filter)
        {
            var builder = new DynamicFilterBuilder();
            return builder.GetExpression<TClass>(filter).Compile();
        }

        /// <summary>
        /// Implicitly converts a <see cref="Filter{TClass}" /> into a <see cref="System.Linq.Expressions.Expression{Func{TClass, TResult}}" />.
        /// </summary>
        /// <param name="filter"></param>
	    public static implicit operator Expression<Func<TClass, bool>>(Filter<TClass> filter)
        {
            var builder = new DynamicFilterBuilder();
            return builder.GetExpression<TClass>(filter);
        }

        /// <summary>
        /// String representation of <see cref="Filter{TClass}" />.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var result = new System.Text.StringBuilder();
            bool hasMoreThanOneStatementGroup = _statementGroups.Count > 1;

            for (int j = 0; j < _statementGroups.Count; j++)
            {
                var filterStatements = _statementGroups[j];
                bool isNotTheFirstStatementGroup = j != 0;

                if (hasMoreThanOneStatementGroup && isNotTheFirstStatementGroup)
                {
                    if (filterStatements.Any())
                        result.Append($"{filterStatements.First().Connector} (");
                }
                else if (hasMoreThanOneStatementGroup)
                {
                    result.Append("(");
                }

                StringBuilder groupResult = new StringBuilder();

                for (int i = 0; i < filterStatements.Count; i++)
                {
                    var filterStatement = filterStatements[i];
                    bool isNotTheFirstFilterStatement = i != 0;

                    if (isNotTheFirstFilterStatement)
                        groupResult.Append($" {filterStatement.Connector} ");

                    groupResult.Append(filterStatement);
                }

                result.Append(groupResult.ToString().Trim());

                if (hasMoreThanOneStatementGroup)
                    result.Append(")");
            }

            return result.ToString();
        }


        public IDynamicFilterStatementConnection By(string propertyId, string operation)
        {
            return By(propertyId, _operationHelper.GetOperationByName(operation));
        }


        public IDynamicFilterStatementConnection By(string propertyId, string operation, Connector connector)
        {
            return By(propertyId, _operationHelper.GetOperationByName(operation), connector);
        }


        public IDynamicFilterStatementConnection By<TPropertyType>(string propertyId, string operation, TPropertyType value, Connector connector)
        {
            return By(propertyId, _operationHelper.GetOperationByName(operation), value, connector);
        }

        public IDynamicFilterStatementConnection By<TPropertyType>(string propertyId, string operation, TPropertyType value, TPropertyType value2)
        {
            return By(propertyId, _operationHelper.GetOperationByName(operation), value, value2);
        }


        public IDynamicFilterStatementConnection By<TPropertyType>(string propertyId, string operation, TPropertyType value, TPropertyType value2, Connector connector)
        {
            return By(propertyId, _operationHelper.GetOperationByName(operation), value, value2, connector);
        }

    }
}
