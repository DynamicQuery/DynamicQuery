using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace DynamicSelect
{
    public static class DynamicSelectExtensions
    {
        public static IQueryable ProjectToDynamic<T>(this IQueryable<T> queryable, IEnumerable<string> propertyIds)
            where T : class
        {
            Node root = new Node();

            foreach (string propertyId in propertyIds)
            {
                root.Add(propertyId);
            }

            DynamicIncludeQueryBuilder includeQueryBuilder = new DynamicIncludeQueryBuilder();
            List<string> includes = includeQueryBuilder.Build(root).ToList();
            includes.ForEach(include =>
            {
                queryable = queryable.Include(include);
            });

            DynamicSelectQueryBuilder selectQueryBuilder = new DynamicSelectQueryBuilder();

            string selectQuery = selectQueryBuilder.Build(root);

            return queryable.Select(selectQuery);
        }
    }
}
