using System.Collections.Generic;

namespace DynamicFilter.Common.Interfaces
{
    public interface INestedListsHelper
    {
        List<List<IDynamicFilterStatement>> GroupFilterStatements(IEnumerable<IDynamicFilterStatement> filterStatementGroup);

        /// <summary>
        /// PersonNames[Type.id]
        /// PersonName.Type[id]
        /// </summary>
        /// <param name="propertyId"></param>
        /// <returns></returns>
        bool IsAnImmediateList(string propertyId);

        bool IsAnImmediateList(IDynamicFilterStatement statement);

        /// <summary>
        /// By DynamicQuery convention, a PropertyId is deemed to contain a list if it has "[" and "]".
        /// These rectangle braces notation represents a List.
        /// </summary>
        /// <example>
        /// PropertyId: PersonName.Name represents a PersonName object property containing a Name property within.
        /// PropertyId: PersonNames[Name] represents a list of PersonName object containing a Name property.
        /// </example>
        /// <param name="propertyId">A propertyId</param>
        /// <returns></returns>
        bool PropertyIdContainsList(string propertyId);


        /// <summary>
        /// Two PropertyIds that are within list(s) are defined to be in same scope when
        /// thier outer substrings match.
        /// </summary>
        /// <example>
        /// PID1: PersonNames[Name]
        /// PID2: PersonNames[Type]
        /// PID1 LEFT SUBSTRING = "PersonNames["
        /// PID2 LEFT SUBSTRING = "PersonNames["
        /// PID1 RIGHT SUBSTRING = "]"
        /// PID2 RIGHT SUBSTRING = "]"
        /// Since they match, they are in same nested list scope.
        /// </example>
        /// <param name="firstPropertyId">The first property id.</param>
        /// <param name="secondPropertyId">The second property id.</param>
        /// <returns></returns>
        bool PropertyIdsAreAtSameNestedListScope(string firstPropertyId, string secondPropertyId);
    }
}