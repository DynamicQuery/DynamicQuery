using DynamicQuery.QueryBuilder.Builders;

namespace DynamicQuery.QueryBuilder.Connectors
{
    public class DynamicProjectionBuilderConnector
    {
        public DynamicProjectionBuilderConnector(DynamicProjectionBuilder projectionBuilder, DynamicQueryBuilder dynamicQueryBuilder)
        {
            And = projectionBuilder;
            Then = dynamicQueryBuilder;
        }

        public DynamicQueryBuilder Then { get; private set; }
        public DynamicProjectionBuilder And { get; set; }
    }
}
