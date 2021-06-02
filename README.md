# DynamicQuery
DynamicQuery is a library aimed at allowing developers to easily filter and shape their data at run time.

<img src="DynamicQuery/Images/DynamicQuery.jpg" width="200" height="160">

DynamicQuery is a library aimed at allowing developers to easily filter and shape their data at run time by specifying the
`field name`, `filter operations`, `values` and `logical connectors` that are used for the filter, and field names used for selecting the columns, all at runtime.
You could also do it via compile time easily like the example given below. 
It would then dynamically translate the filters, perform necessary table joins and pagination, to obtain the required columns.


# Example

```cs
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
```
# Result

```json
[
  {
    "Id": 2,
    "MyName": {
      "Name": "Draco Malfoy"
    },
    "Gender": "M"
  }
]
```

# DynamicFilter

<img src="DynamicQuery/Images/DynamicFilter.jpg" width="200" height="160">

DynamicFilter is a library aimed at allowing developers to easily filter their data at run time by specifying the
`field name`, `filter operations`, `values` and `logical connectors` that are used for the filter.
It would then dynamically translate the filters and construct the sql using an `ORM`

# Example

```cs
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
```

# Result

```json
[
  {
    "Id": 2,
    "Gender": "M",
    "MyName": {
      "Id": 2,
      "Name": "Draco Malfoy"
    },
    "Departments": [
      {
        "Id": 3,
        "Name": "IT",
        "Sections": [
          {
            "Id": 6,
            "Name": "IT-A"
          }
        ]
      },
      {
        "Id": 4,
        "Name": "HR",
        "Sections": [
          {
            "Id": 7,
            "Name": "HR-A"
          }
        ]
      }
    ]
  }
]
```

# DynamicSelect

<img src="DynamicQuery/Images/DynamicSelect.jpg" width="200" height="160">

DynamicSelect is a library aimed at allowing developers to easily shape their data at run time by specifying the
fields that they want. It would then dynamically perform the neccessary joins for you through `EntityFramework Core`.

# Example

```cs
    public static class DynamicSelectExample
    {
        public static void Run()
        {
            SeedDbContext seedDbContext = SeedDbContext.Create();

            
            dynamic result = seedDbContext.Persons.ProjectToDynamic(
                "Id", "Gender", "MyName.Name", "Departments[Name]", "Departments[Sections[Name]]"
                );

            string json = JsonConvert.SerializeObject(result, Formatting.Indented);

            Console.WriteLine(json);
        }
    }
```

# Result

```json
[
  {
    "Id": 1,
    "Gender": "F",
    "MyName": {
      "Name": "Jane Doe"
    },
    "Departments": [
      {
        "Name": "IT",
        "Sections": [
          {
            "Name": "IT-A"
          },
          {
            "Name": "IT-B"
          }
        ]
      },
      {
        "Name": "HR",
        "Sections": [
          {
            "Name": "HR-A"
          },
          {
            "Name": "HR-B"
          },
          {
            "Name": "HR-C"
          }
        ]
      }
    ]
  }]
```
