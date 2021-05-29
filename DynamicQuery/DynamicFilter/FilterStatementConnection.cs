using DynamicFilter.Common;
using DynamicFilter.Common.Interfaces;
using ExpressionBuilderCore.Interfaces;

namespace DynamicFilter
{
    /// <summary>
    /// Connects to FilterStatement together.
    /// </summary>
	public class DynamicFilterStatementConnection : IDynamicFilterStatementConnection
    {
        private readonly IDynamicFilter _filter;

        internal DynamicFilterStatementConnection(IDynamicFilter filter)
        {
            _filter = filter;
        }

        /// <summary>
		/// Defines that the last filter statement will connect to the next one using the 'AND' logical operator.
		/// </summary>
		public IDynamicFilter And
        {
            get
            {
                _filter.Connector = Connector.And;
                return _filter;
            }
        }

        /// <summary>
        /// Defines that the last filter statement will connect to the next one using the 'OR' logical operator.
        /// </summary>
		public IDynamicFilter Or
        {
            get
            {
                _filter.Connector = Connector.Or;
                return _filter;
            }
        }
    }
}
