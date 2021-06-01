using System.Collections.Generic;

namespace DynamicQuery.QueryBuilder
{
    public class Projection
    {
        public List<string> Selections { get; set; } = new List<string>();
        public int PageSize { get; set; } = 0;
        public int Page { get; set; } = 0;

    }
}
