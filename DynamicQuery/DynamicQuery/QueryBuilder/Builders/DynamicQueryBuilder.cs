using DynamicQuery.QueryBuilder.Models;

namespace DynamicQuery.QueryBuilder.Builders
{
    public class DynamicQueryBuilder
    {
        private QueryLogic _screeningLogic;

        public LogicBuilder()
        {
            _screeningLogic = new QueryLogic();
            Filter = new QueryGroupBuilderTransiter(_screeningLogic.QueryGroups, this);
            Select = new ProjectionBuilder(_screeningLogic.Projection, this);
        }

        public QueryGroupBuilderTransiter Filter { get; set; }
        public ProjectionBuilder Select { get; private set; }
        public QueryLogic Builder() => _screeningLogic;

    }
}
