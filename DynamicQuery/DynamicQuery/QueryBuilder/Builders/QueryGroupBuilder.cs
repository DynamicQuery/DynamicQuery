using DynamicFilter.Common;
using DynamicQuery.QueryBuilder.Models;
using System.Collections.Generic;
using System.Linq;

namespace DynamicQuery.QueryBuilder.Builders
{
    public class QueryGroupBuilder
    {
        private List<QueryGroup> _queryGroups;

        public QueryGroupBuilder(List<QueryGroup> queryGroups, DynamicQueryBuilder queryGroupBuilder)
        {
            Then = queryGroupBuilder;
            _queryGroups = queryGroups;
            AddGroup();

        }

        public QueryGroupBuilder AddGroup()
        {
            _queryGroups.Add(new QueryGroup());
            return this;
        }

        public DynamicQueryBuilder Then { get; private set; }

        public QueryGroupBuilder By(string propertyId, string operation, Connector connector = Connector.None)
        {
            return By(propertyId, operation, null, null, connector);
        }

        public QueryGroupBuilder By(string propertyId, string operation, string value, Connector connector = Connector.None)
        {
            return By(propertyId, operation, value, null, connector);
        }

        public QueryGroupBuilder By(string propertyId, string operation, string value, string value2, Connector connector = Connector.None)
        {
            Query query = new Query(propertyId, operation.ToString(), value, value2, connector);
            _queryGroups.Last().Queries.Add(query);
            return this;
        }

    }
}
