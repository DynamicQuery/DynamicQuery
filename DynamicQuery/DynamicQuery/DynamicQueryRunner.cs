using DynamicFilter;
using DynamicFilter.Common;
using DynamicQuery.QueryBuilder.Models;
using DynamicSelect;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DynamicQuery
{
    public class DynamicQueryRunner
    {
        public IQueryable Build<TEntity>(IQueryable<TEntity> entities, QueryLogic queryLogic) where TEntity : class
        {
            DynamicFilter<TEntity> filter = CreateFilters<TEntity>(queryLogic.QueryGroups);
            return entities.Where(filter).ProjectToDynamic(queryLogic.Projection.Selections);
        }


        public IQueryable Build<TEntity>(DbSet<TEntity> entities, QueryLogic queryLogic) where TEntity : class
        {
            DynamicFilter<TEntity> filter = CreateFilters<TEntity>(queryLogic.QueryGroups);
            return entities.Where(filter).ProjectToDynamic(queryLogic.Projection.Selections);
        }

        private DynamicFilter<TEntity> CreateFilters<TEntity>(List<QueryGroup> queryGroups) where TEntity : class
        {
            DynamicFilter<TEntity> filter = new DynamicFilter<TEntity>();
            for (int queryGroupIndex = 0; queryGroupIndex < queryGroups.Count; queryGroupIndex++)
            {
                QueryGroup queryGroup = queryGroups[queryGroupIndex];

                if (queryGroupIndex != 0)
                {

                    filter.StartGroup();
                }

                foreach (Query query in queryGroup.Queries)
                {
                    Connector connector = TranslateConnector(query.Connector);
                    filter.By(query.PropertyId, query.Operation, query.Value, query.Value2, connector);
                }

            }
            return filter;
        }

        private Connector TranslateConnector(string strConnector)
        {

            Dictionary<string, Connector> connectorMapper = new Dictionary<string, Connector>()
            {
                { "AND", Connector.And},
                { "OR", Connector.Or}
            };

            if (string.IsNullOrEmpty(strConnector) || !connectorMapper.ContainsKey(strConnector.ToUpper()))
            {
                return Connector.None;
            }

            return connectorMapper[strConnector.ToUpper()];
        }
    }
}
