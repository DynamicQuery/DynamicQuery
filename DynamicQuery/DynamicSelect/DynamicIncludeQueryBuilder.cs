using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicSelect
{
    public class IncludeQueryBuilder
    {
        public IEnumerable<string> Build(Node node)
        {
            List<string> includesList = new List<string>();

            if (node.Type == Node.NodeType.ROOT)
            {
                foreach (var childNode in node.ChildNodes)
                {
                    IEnumerable<string> builtIncludes = Build(childNode);

                    if (builtIncludes.Any())
                        includesList.AddRange(builtIncludes);
                }

                includesList = includesList.Distinct().ToList();
                includesList.RemoveAll(x => string.IsNullOrEmpty(x));
                includesList = includesList
                    .Select(x => x.EndsWith(".") ? x.Substring(0, x.Length - 1) : x)
                    .ToList();

                return includesList;
            }
            else if (node.Type == Node.NodeType.OBJECT || node.Type == Node.NodeType.LIST)
            {
                foreach (var childNode in node.ChildNodes)
                {
                    IEnumerable<string> builtIncludes = Build(childNode);

                    if (builtIncludes.Any())
                        includesList.AddRange(builtIncludes);
                }
                includesList = includesList.Select(include => $"{node.Projection.DisplayName}.{include}").ToList();

                return includesList.Distinct();
            }
            else
            {
                return new string[] { string.Empty };
            }
        }
    }
}
