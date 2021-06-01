using DynamicQuery.QueryBuilder.Builders;
using DynamicQuery.QueryBuilder.Models;
using System.Collections.Generic;

namespace DynamicQuery.QueryBuilder.Connectors
{
    public class QueryGroupBuilderConnector
    {
        private readonly List<QueryGroup> _queryGroups;
        private readonly DynamicQueryBuilder _dynamicQueryBuilder;

        public QueryGroupBuilderConnector(List<QueryGroup> queryGroups, DynamicQueryBuilder dynamicQueryBuilder)
        {
            _queryGroups = queryGroups;
            _dynamicQueryBuilder = dynamicQueryBuilder;
        }

        public QueryGroupBuilder AddGroup()
        {
            return new QueryGroupBuilder(_queryGroups, _dynamicQueryBuilder);
        }
    }
}
