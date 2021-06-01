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
            Filter = new DynamicQueryGroupBuilderConnector(_screeningLogic.QueryGroups, this);
            Select = new DynamicProjectionBuilder(_screeningLogic.Projection, this);
        }

        public DynamicQueryGroupBuilderConnector Filter { get; set; }
        public DynamicProjectionBuilder Select { get; private set; }
        public QueryLogic Builder() => _screeningLogic;

    }
}
