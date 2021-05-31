using DynamicSelect;
using FluentAssertions;
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
    }
}
