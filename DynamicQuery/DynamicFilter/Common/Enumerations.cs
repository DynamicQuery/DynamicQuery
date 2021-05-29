namespace DynamicFilter.Common
{
    /// <summary>
    /// Defines how filter statements will be connected to each other.
    /// 
    /// If you have 2 filter statements {F1, F2}, we need to attache a Connector, eg 'AND',
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
}
