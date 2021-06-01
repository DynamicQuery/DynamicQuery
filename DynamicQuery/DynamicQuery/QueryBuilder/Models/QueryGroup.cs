using System.Collections.Generic;

namespace DynamicQuery.QueryBuilder.Models
{
    public class QueryGroup
    {
        public List<Query> Queries { get; set; } = new List<Query>();
    }
}
