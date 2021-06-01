using DynamicQuery.QueryBuilder.Connectors;
using DynamicQuery.QueryBuilder.Models;

namespace DynamicQuery.QueryBuilder.Builders
{
    public class DynamicProjectionBuilder
    {
        private readonly DynamicProjectionBuilderConnector _projectionBuilderConnector;

        private Projection _projection;

        public DynamicProjectionBuilder(Projection projection, DynamicQueryBuilder dynamicQueryBuilder)
        {
            _projection = projection;
            _projectionBuilderConnector = new DynamicProjectionBuilderConnector(this, dynamicQueryBuilder);
        }

        public DynamicProjectionBuilderConnector Field(string propertyId)
        {
            return Fields(propertyId);
        }

        public DynamicProjectionBuilderConnector Fields(params string[] propertyIds)
        {
            _projection.Selections.AddRange(propertyIds);
            return _projectionBuilderConnector;
        }

        public DynamicProjectionBuilderConnector Paginate(int page = 0, int pageSize = 0)
        {
            _projection.Page = page;
            _projection.PageSize = pageSize;

            return _projectionBuilderConnector;
        }
    }
}

