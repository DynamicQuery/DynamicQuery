using DynamicFilter;
using DynamicFilter.Operations;
using Newtonsoft.Json;
using Seed;
using Seed.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Example
{
    public static class DynamicFilterExample
    {
        public static void Run()
        {
            SeedDbContext seedDbContext = SeedDbContext.Create();

            Filter<Person> filter = new Filter<Person>();

            filter
                .By("Gender", Operations.EqualTo, "M")
                .And
                .By("MyName.Name", Operations.Contains, "Malfoy");

            List<Person> result = seedDbContext.Persons.Where(filter).ToList();

            string json = JsonConvert.SerializeObject(result, Formatting.Indented);

            Console.WriteLine(json);
        }
    }
}
