using DynamicFilter.Common;
using DynamicFilter.Common.Interfaces;
using DynamicQuery.QueryBuilder.Models;
using System.Collections.Generic;
using System.Linq;

namespace DynamicQuery.QueryBuilder.Builders
{
    public class DynamicQueryGroupBuilder
    {
        private List<QueryGroup> _queryGroups;

        public DynamicQueryGroupBuilder(List<QueryGroup> queryGroups, DynamicQueryBuilder queryGroupBuilder)
        {
            Then = queryGroupBuilder;
            _queryGroups = queryGroups;
            AddGroup();

        }

        public DynamicQueryGroupBuilder AddGroup()
        {
            _queryGroups.Add(new QueryGroup());
            return this;
        }

        public DynamicQueryBuilder Then { get; private set; }

        public DynamicQueryGroupBuilder By(string propertyId, IOperation operation, Connector connector = Connector.None)
             => By(propertyId, operation.ToString(), connector);

        public DynamicQueryGroupBuilder By(string propertyId, string operation, Connector connector = Connector.None)
        {
            return By(propertyId, operation, null, null, connector);
        }

        public DynamicQueryGroupBuilder By(string propertyId, IOperation operation, string value, Connector connector = Connector.None)
             => By(propertyId, operation.ToString(), value, connector);


        public DynamicQueryGroupBuilder By(string propertyId, string operation, string value, Connector connector = Connector.None)
        {
            return By(propertyId, operation, value, null, connector);
        }

        public DynamicQueryGroupBuilder By(string propertyId, IOperation operation, string value, string value2, Connector connector = Connector.None)
             => By(propertyId, operation.ToString(), value, value2, connector);

        public DynamicQueryGroupBuilder By(string propertyId, string operation, string value, string value2, Connector connector = Connector.None)
        {
            Query query = new Query(propertyId, operation.ToString(), value, value2, connector);
            _queryGroups.Last().Queries.Add(query);
            return this;
        }

    }
}
