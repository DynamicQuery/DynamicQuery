using System;

namespace DynamicFilter.Common
{
    /// <summary>
    /// Defines how filter statements will be connected to each other.
    /// 
    /// If you have 2 filter statements {F1, F2}, we need to attach a Connector, eg 'AND',
    /// to F2. F2 with its connector 'AND' will now be able to attach to F1, just like a leggo,
    /// to produce F1 'AND' F2
    /// </summary>
    public enum Connector
    {
        /// <summary>
        /// Determines that both the current AND the previous filter statement needs to be satisfied.
        /// </summary>
        And,

        /// <summary>
        /// Determines that the current OR the previous filter statement needs to be satisfied.
        /// </summary>
        Or,

        /// <summary>
        /// Only applied to the first filter statement, as it has no connectors
        /// </summary>
        None
    }

    /// <summary>
    /// Groups types into simple groups and map the supported operations to each group.
    /// </summary>
    [Flags]
    public enum TypeGroup
    {
        /// <summary>
        /// Default type group, only supports EqualTo and NotEqualTo.
        /// </summary>
        Default = -1,

        /// <summary>
        /// Supports all text related operations.
        /// </summary>
        Text = 1,

        /// <summary>
        /// Supports all numeric related operations.
        /// </summary>
        Number = 2,

        /// <summary>
        /// Supports boolean related operations.
        /// </summary>
        Boolean = 4,

        /// <summary>
        /// Supports all date related operations.
        /// </summary>
        Date = 8,

        /// <summary>
        /// Supports nullable related operations.
        /// </summary>
        Nullable = 16
    }
}
