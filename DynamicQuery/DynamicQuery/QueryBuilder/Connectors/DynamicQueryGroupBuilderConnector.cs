using DynamicQuery.QueryBuilder.Builders;
using DynamicQuery.QueryBuilder.Models;
using System.Collections.Generic;

namespace DynamicQuery.QueryBuilder.Connectors
{
    public class DynamicQueryGroupBuilderConnector
    {
        private readonly List<QueryGroup> _queryGroups;
        private readonly DynamicQueryBuilder _dynamicQueryBuilder;

        public DynamicQueryGroupBuilderConnector(List<QueryGroup> queryGroups, DynamicQueryBuilder dynamicQueryBuilder)
        {
            _queryGroups = queryGroups;
            _dynamicQueryBuilder = dynamicQueryBuilder;
        }

        public DynamicQueryGroupBuilder AddGroup()
        {
            return new DynamicQueryGroupBuilder(_queryGroups, _dynamicQueryBuilder);
        }
    }
}
