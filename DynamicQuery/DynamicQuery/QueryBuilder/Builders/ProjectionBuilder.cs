using DynamicQuery.QueryBuilder.Connectors;
using DynamicQuery.QueryBuilder.Models;

namespace DynamicQuery.QueryBuilder.Builders
{
    public class ProjectionBuilder
    {
        private readonly ProjectionBuilderConncetor _projectionBuilderConnector;

        private Projection _projection;

        public ProjectionBuilder(Projection projection, DynamicQueryBuilder dynamicQueryBuilder)
        {
            _projection = projection;
            _projectionBuilderConnector = new ProjectionBuilderConncetor(this, dynamicQueryBuilder);
        }

        public ProjectionBuilderConncetor Field(string propertyId)
        {
            return Fields(propertyId);
        }

        public ProjectionBuilderConncetor Fields(params string[] propertyIds)
        {
            _projection.Selections.AddRange(propertyIds);
            return _projectionBuilderConnector;
        }

        public ProjectionBuilderConncetor Paginate(int page = 0, int pageSize = 0)
        {
            _projection.Page = page;
            _projection.PageSize = pageSize;

            return _projectionBuilderConnector;
        }
    }
}

