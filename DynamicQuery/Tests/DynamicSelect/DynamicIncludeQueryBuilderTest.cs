using DynamicSelect;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests.DynamicSelect
{
    public class DynamicIncludeQueryBuilderTest
    {
        private DynamicIncludeQueryBuilder _dynamicIncludeQueryBuilder;

        public DynamicIncludeQueryBuilderTest()
        {
            _dynamicIncludeQueryBuilder = new DynamicIncludeQueryBuilder();
        }


        [Theory]
        [InlineData("Id", "Gender")]
        [InlineData("Gender")]
        [InlineData("Id")]
        public void BuildTest_Level1PropertiesShouldReturnNoIncludes(params string[] propertyIds)
        {
            Node rootNode = new Node();

            foreach (string propertyId in propertyIds)
            {
                rootNode.Add(propertyId);
            }

            _dynamicIncludeQueryBuilder.Build(rootNode).ToList().Count.Should().Be(0);
        }

        [Fact]
        public void BuildTest_Level2PropertiesShouldHaveIncludes()
        {
            Node rootNode = new Node();
            Action<List<string>> addPropertyIdsAction = x => x.ForEach(y => rootNode.Add(y));


            List<string> propertyIds = new List<string>()
            {
                "PersonName.Name", "PersonName.Id",
                "PersonName.Departments[Id]"
            };
            addPropertyIdsAction(propertyIds);

            _dynamicIncludeQueryBuilder.Build(rootNode).ToList().Count.Should().Be(2);


            rootNode = new Node();
            propertyIds = new List<string>()
            {
                "PersonName.Name", "PersonName.Id",
                "PersonName.Departments[Id]", "PersonIdentification.Id",
                "PersonHistories[History]"
            };
            addPropertyIdsAction(propertyIds);

            _dynamicIncludeQueryBuilder.Build(rootNode).ToList().Count.Should().Be(4);


            rootNode = new Node();
            propertyIds = new List<string>()
            {
                "Gender"
            };
            addPropertyIdsAction(propertyIds);

            _dynamicIncludeQueryBuilder.Build(rootNode).ToList().Count.Should().Be(0);
        }
    }
}
