using DynamicSelect;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Tests.Common;
using Tests.Common.Model;
using Xunit;

namespace Tests.DynamicSelect
{
    public class DynamicSelectTest : AbstractTestBase
    {


        [Fact]
        public void DynamicSelect_Test()
        {
            List<string> fieldsToSelect = new List<string>()
            {
                "Gender"
            };

            List<dynamic> result = GetResult(fieldsToSelect);
            result.Any().Should().BeTrue();
            result.ForEach(x => ((string)x.Gender).Should().NotBeNullOrEmpty());


            fieldsToSelect = new List<string>()
            {
                "Gender", "MyName.Name"
            };

            result = GetResult(fieldsToSelect);
            result.Any().Should().BeTrue();
            result.ForEach(x => ((string)x.Gender).Should().NotBeNullOrEmpty());
            result.ForEach(x => ((PersonName)x.MyName).Name.Should().NotBeNullOrEmpty());


            fieldsToSelect = new List<string>()
            {
                "Gender", "MyName.Name", "Departments[Name]"
            };

            result = GetResult(fieldsToSelect);
            result.Any().Should().BeTrue();
            result.ForEach(x => ((string)x.Gender).Should().NotBeNullOrEmpty());
            result.ForEach(x => ((PersonName)x.MyName).Name.Should().NotBeNullOrEmpty());
            result.ForEach(x => ((List<Department>)x.Departments).ForEach(d => d.Name.Should().NotBeNullOrEmpty()));


            fieldsToSelect = new List<string>()
            {
                "Gender", "MyName.Name", "Departments[Name]", "Departments[Sections[Name]]"
            };

            result = GetResult(fieldsToSelect);
            result.Any().Should().BeTrue();
            result.ForEach(x => ((string)x.Gender).Should().NotBeNullOrEmpty());
            result.ForEach(x => ((PersonName)x.MyName).Name.Should().NotBeNullOrEmpty());
            result.ForEach(x => ((List<Department>)x.Departments).ForEach(d => d.Name.Should().NotBeNullOrEmpty()));
            result.ForEach(x => ((List<Department>)x.Departments).ForEach(d => d.Sections.ForEach(s => s.Name.Should().NotBeNullOrEmpty())));
        }

        private List<dynamic> GetResult(List<string> fieldsToSelect)
            => _seedDbContext.Persons.ProjectToDynamic(fieldsToSelect).ToDynamicList();
    }

}
