using DynamicSelect;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests.DynamicSelect
{
    public class DynamicSelectQueryBuilderTest
    {
        private DynamicSelectQueryBuilder _dynamicSelectQueryBuilder;

        public DynamicSelectQueryBuilderTest()
        {
            _dynamicSelectQueryBuilder = new DynamicSelectQueryBuilder();
        }

        [Fact]
        public void BuildTest()
        {
            Node rootNode = new Node();
            Action<List<string>> addPropertyIdsAction = x => x.ForEach(y => rootNode.Add(y));


            List<string> propertyIds = new List<string>()
            {
                "Id"
            };
            addPropertyIdsAction(propertyIds);
            _dynamicSelectQueryBuilder.Build(rootNode).Should().Be("new { Id as Id }");


            rootNode = new Node();
            propertyIds = new List<string>()
            {
                "Id", "Gender"
            };

            addPropertyIdsAction(propertyIds);
            _dynamicSelectQueryBuilder.Build(rootNode).Should().Be("new { Id as Id, Gender as Gender }");

            rootNode = new Node();
            propertyIds = new List<string>()
            {
                "Id", "Gender", "MyName.Name"
            };

            addPropertyIdsAction(propertyIds);
            _dynamicSelectQueryBuilder.Build(rootNode).Should().Be("new { Id as Id, Gender as Gender, new { MyName.Name as Name } as MyName }");


            rootNode = new Node();
            propertyIds = new List<string>()
            {
                "Id", "Gender", "Departments[Name]"
            };

            addPropertyIdsAction(propertyIds);
            _dynamicSelectQueryBuilder.Build(rootNode).Should().Be("new { Id as Id, Gender as Gender, Departments.Select(new { Name as Name }) as Departments }");


            rootNode = new Node();
            propertyIds = new List<string>()
            {
                "Id", "Gender", "Departments[Sections[Name]]"
            };

            addPropertyIdsAction(propertyIds);
            _dynamicSelectQueryBuilder.Build(rootNode).Should().Be("new { Id as Id, Gender as Gender, Departments.Select(new { Sections.Select(new { Name as Name }) as Sections }) as Departments }");


            rootNode = new Node();
            propertyIds = new List<string>()
            {
                "Id", "Gender", "MyName.Name","Departments[Sections[Name]]", "Departments[Name]"
            };

            addPropertyIdsAction(propertyIds);
            _dynamicSelectQueryBuilder.Build(rootNode).Should().Be("new { Id as Id, Gender as Gender, new { MyName.Name as Name } as MyName, Departments.Select(new { Sections.Select(new { Name as Name }) as Sections, Name as Name }) as Departments }");

        }
    }
}
