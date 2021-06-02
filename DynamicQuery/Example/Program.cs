using DynamicSelect;
using Newtonsoft.Json;
using Seed;
using System;
using System.Collections.Generic;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            SeedDbContext seedDbContext = SeedDbContext.Create();

            List<string> fieldsToSelect = new List<string>()
            {
                "Id", "Gender", "MyName.Name", "Departments[Name]", "Departments[Sections[Name]]"
            };

            var result = seedDbContext.Persons.ProjectToDynamic(fieldsToSelect);

            string json = JsonConvert.SerializeObject(result, Formatting.Indented);

            Console.WriteLine(json);
        }
    }
}
