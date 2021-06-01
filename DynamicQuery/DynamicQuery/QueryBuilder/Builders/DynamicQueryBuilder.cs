using DynamicQuery.QueryBuilder.Connectors;
using DynamicQuery.QueryBuilder.Models;

namespace DynamicQuery.QueryBuilder.Builders
{
    public class DynamicQueryBuilder
    {
        private QueryLogic _screeningLogic;

        public DynamicQueryBuilder()
        {
            _screeningLogic = new QueryLogic();
            Filter = new QueryGroupBuilderConnector(_screeningLogic.QueryGroups, this);
            Select = new ProjectionBuilder(_screeningLogic.Projection, this);
        }

        public QueryGroupBuilderConnector Filter { get; set; }
        public ProjectionBuilder Select { get; private set; }
        public QueryLogic Builder() => _screeningLogic;

    }
}
