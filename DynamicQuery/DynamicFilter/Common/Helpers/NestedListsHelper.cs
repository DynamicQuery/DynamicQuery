using DynamicFilter.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DynamicFilter.Common.Helpers
{
    public class NestedListsHelper : INestedListsHelper
    {
        private const string NON_LIST_KEY = "NonLists";

        
        /// <inheritdoc/>
        public bool PropertyIdsAreAtSameNestedListScope(string firstPropertyId, string secondPropertyId)
        {
            int indexOfFirstPropertyIdInnerOpenBracket = firstPropertyId.LastIndexOf("[");
            int indexOfFirstPropertyIdInnerCloseBracket = firstPropertyId.IndexOf("]");

            int indexOfSecondPropertyIdInnerOpenBracket = secondPropertyId.LastIndexOf("[");
            int indexOfSecondPropertyIdInnerCloseBracket = secondPropertyId.IndexOf("]");

            bool propertyIdLeftIsMatch = firstPropertyId.Substring(0, indexOfFirstPropertyIdInnerOpenBracket) == secondPropertyId.Substring(0, indexOfSecondPropertyIdInnerOpenBracket);
            bool propertyIdRightIsMatch = firstPropertyId.Substring(indexOfFirstPropertyIdInnerCloseBracket + 1) == secondPropertyId.Substring(indexOfSecondPropertyIdInnerCloseBracket + 1);

            return propertyIdLeftIsMatch && propertyIdRightIsMatch;
        }

        public List<List<IDynamicFilterStatement>> GroupFilterStatements(IEnumerable<IDynamicFilterStatement> filterStatementGroup)
        {
            Dictionary<string, List<IDynamicFilterStatement>> keyToListOfStatements = new Dictionary<string, List<IDynamicFilterStatement>>();

            foreach (IDynamicFilterStatement filterStatement in filterStatementGroup)
            {
                if (PropertyIdContainsList(filterStatement.PropertyId))
                {
                    string key = GetKeyForGroupingLists(filterStatement.PropertyId);

                    if (!keyToListOfStatements.ContainsKey(key))
                    {
                        keyToListOfStatements[key] = new List<IDynamicFilterStatement>();
                    }
                    keyToListOfStatements[NON_LIST_KEY].Add(filterStatement);

                }

            }

            return keyToListOfStatements.Select(d => d.Value).ToList();
        }

        private string GetKeyForGroupingLists(string propertyId)
        {
            int indexOfInnerOpenBracketOfPropertyId = propertyId.LastIndexOf("[");
            int indexOfInnerClosedBracketOfPropertyId = propertyId.IndexOf("]");

            return $"{propertyId.Substring(0, indexOfInnerOpenBracketOfPropertyId)}{propertyId.Substring(indexOfInnerClosedBracketOfPropertyId + 1)}";
        }

        public bool IsAnImmediateList(IDynamicFilterStatement statement) => IsAnImmediateList(statement.PropertyId);


        /// <inheritdoc/>
        public bool IsAnImmediateList(string propertyId)
        {
            if (propertyId.Contains("[") && propertyId.Contains("]"))
            {
                if (propertyId.Contains("."))
                {
                    if (propertyId.IndexOf(".") > propertyId.IndexOf("["))
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public bool PropertyIdContainsList(string propertyId) => propertyId.Contains("[") && propertyId.Contains("]");
    }
}
