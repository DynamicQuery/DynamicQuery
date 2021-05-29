using DynamicFilter.Common.Interfaces;

namespace ExpressionBuilderCore.Interfaces
{
    /// <summary>
    /// Connects to FilterStatement together.
    /// </summary>
	public interface IDynamicFilterStatementConnection
	{
		/// <summary>
		/// Defines that the last filter statement will connect to the next one using the 'AND' logical operator.
		/// </summary>
        IDynamicFilter And { get; }
        /// <summary>
        /// Defines that the last filter statement will connect to the next one using the 'OR' logical operator.
        /// </summary>
        IDynamicFilter Or { get; }
	}
}