using DynamicQuery.QueryBuilder.Builders;

namespace DynamicQuery.QueryBuilder.Connectors
{
    public class ProjectionBuilderConncetor
    {
        public ProjectionBuilderConncetor(ProjectionBuilder projectionBuilder, DynamicQueryBuilder dynamicQueryBuilder)
        {
            And = projectionBuilder;
            Then = dynamicQueryBuilder;
        }

        public DynamicQueryBuilder Then { get; private set; }
        public ProjectionBuilder And { get; set; }
    }
}
