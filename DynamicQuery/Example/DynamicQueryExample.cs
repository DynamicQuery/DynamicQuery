using DynamicFilter.Common;
using DynamicFilter.Operations;
using DynamicQuery;
using DynamicQuery.QueryBuilder.Builders;
using DynamicQuery.QueryBuilder.Models;
using Newtonsoft.Json;
using Seed;
using System;
using System.Linq.Dynamic.Core;

namespace Example
{
    public class DynamicQueryExample
    {
        public static void Run()
        {
            SeedDbContext seedDbContext = SeedDbContext.Create();

            DynamicQueryBuilder dynamicQueryBuilder = new DynamicQueryBuilder();

            QueryLogic queryLogic = dynamicQueryBuilder.Filter
                                                            .AddGroup()
                                                                .By("Gender", Operations.EqualTo, "M")
                                                                .By("MyName.Name", Operations.Contains, "Malfoy", Connector.And)
                                                        .Then
                                                        .Select
                                                            .Fields("Id", "MyName.Name", "Gender")
                                                            .And
                                                            .Paginate(page: 1, pageSize: 1)
                                                        .Then
                                                        .Build();

            dynamic result = new DynamicQueryRunner().Build(seedDbContext.Persons, queryLogic).ToDynamicList();

            string json = JsonConvert.SerializeObject(result, Formatting.Indented);

            Console.WriteLine(json);
        }
    }
}
