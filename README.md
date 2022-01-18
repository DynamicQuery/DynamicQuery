![DynamicQuery workflow status](https://github.com/wireless90/DynamicQuery/actions/workflows/dotnet.yml/badge.svg)


# DynamicQuery

Are you sick of configuring and teaching your team _GraphQL_?

Or are you puzzled at why _OData_ queries seem unreadable and hard to construct? 

Or are you experiencing the _over-fetching_ and _under-fetching_ problems brought by RESTful APIs?

_DynamicQuery_ is a library aimed at allowing developers to easily filter and shape their data at run time by specifying the
`field name`, `filter operations`, `values` and `logical connectors` that are used for the filter, and field names used for selecting the columns, all at runtime.

**It would then dynamically translate the filters, perform necessary table joins and pagination**, to obtain the required columns.

In its core, it uses 2 modules, `DynamicFilter` and `DynamicSelect`, each solving a problem unique to it, `Filtering` and `Selecting` respectively.

Let's take a look at them!

# DynamicFilter

Let's say we need to filter our collection of data. However, we do not know what is the operation used to filter, or how many filters are used, or whether to `OR` the filters or `AND` the filters.

DynamicFilter is a library aimed at allowing developers to easily filter their data at run time by specifying the `field name`, `filter operations`, `values` and `logical connectors` that are used for the filter.

It would then translate the filters at runtime and construct the necessary expressions and feed it into `EntityFramework Core`.


# Example

Below is an example of filtering a `Person` object. Notice how you can input the `PropertyId`, the `Operation` and the `value`, as strings.

Thus, you can easily obtain these info using a Web API and feed into it as well!

Notice how elegant and readable it is.

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
                .By("MyName.Name", "Contains", "Malfoy");

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


If we don't need the filtering capabilities and simply want to use the selection capabilities, just use DynamicSelect. DynamicSelect is a library aimed at allowing developers to easily shape their data at run time by specifying the
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
## DynamicQueryRunner

Now if we want both filtering and selection capabilities, thats when we use `DynamicQueryRunner`, which is a 1 liner easy set-up solution as compared to `OData` or `GraphQL`.

`DynamicQueryRunner` is our querying engine that is responsible to translate a given `QueryLogic` into sql with the help of `EntityFramework Core`.

A `QueryLogic` is an object that contains the filters that need to be used, and the fields that need to be selected, and how pagination should be done.

It could be populated by the front end and deserialized by the backend using Web Api.

Let's take a look at how a `QueryLogic` looks like!

### How QueryLogic looks like serialized

```json
{
  "QueryGroups": [
    {
      "Queries": [
        {
          "PropertyId": "Gender",
          "Operation": "EqualTo",
          "Value": "M"
        },
        {
          "Connector": "AND",
          "PropertyId": "MyName.Name",
          "Operation": "Contains",
          "Value": "Malfoy"
        }
      ]
    }
  ],
  "Projection": {
    "Selections": [
      "Id",
      "MyName.Name",
      "Gender"
    ],
    "PageSize": 1,
    "Page": 1
  }
}
```


Once we received a `QueryLogic`, we then feed it into our `DynamicQueryRunner`(1 liner solution) and viola! We get the results!

### DynamicQueryRunner Single Liner Example

```cs
dynamic result = new DynamicQueryRunner().Build(seedDbContext.Persons, queryLogic).ToDynamicList();
```


# Example

```cs
public class DynamicQueryExample
    {
        public static void Run()
        {
            SeedDbContext seedDbContext = SeedDbContext.Create();

            DynamicQueryBuilder dynamicQueryBuilder = new DynamicQueryBuilder();
           
            //QueryLogic will prolly come in from front end and deserialized by backend.
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

# Supported Operations

- Default
  - EqualTo
  - NotEqualTo
- Text
  - Contains
  - DoesNotContain
  - EndsWith
  - EqualTo
  - IsEmpty
  - IsNotEmpty
  - IsNotNull
  - IsNotNullNorWhiteSpace
  - IsNull
  - IsNullOrWhiteSpace
  - NotEqualTo
  - StartsWith
- Number
  - Between
  - EqualTo
  - GreaterThan
  - GreaterThanOrEqualTo
  - LessThan
  - LessThanOrEqualTo
  - NotEqualTo
- Boolean
  - EqualTo
  - NotEqualTo
- Date
  - Between
  - EqualTo
  - GreaterThan
  - GreaterThanOrEqualTo
  - LessThan
  - LessThanOrEqualTo
  - NotEqualTo


