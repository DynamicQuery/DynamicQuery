using Newtonsoft.Json;
using System.Collections.Generic;

namespace DynamicQuery.QueryBuilder.Models
{
    public class QueryLogic
    {
        public List<QueryGroup> QueryGroups { get; set; } = new List<QueryGroup>();

        public Projection Projection { get; set; } = new Projection();

        public string AsJson(bool indented = true)
            => JsonConvert.SerializeObject(this, indented ? Formatting.Indented : Formatting.None);
    }
}
