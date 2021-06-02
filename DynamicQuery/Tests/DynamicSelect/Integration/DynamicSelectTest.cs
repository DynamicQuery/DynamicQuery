using DynamicSelect;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Tests.Common;
using Xunit;

namespace Tests.DynamicSelect
{
    public class DynamicSelectTest : AbstractTestBase
    {

        [Fact]
        public void DynamicSelect_Test()
        {
            
            List<dynamic> result = GetResult("Gender");
            result.Any().Should().BeTrue();
            result.ForEach(x => ((string)x.Gender).Should().NotBeNullOrEmpty());



            result = GetResult("Gender", "MyName.Name");
            result.Any().Should().BeTrue();
            result.ForEach(x => ((string)x.Gender).Should().NotBeNullOrEmpty());
            result.ForEach(x => ((string)x.MyName.Name).Should().NotBeNullOrEmpty());


            result = GetResult("Gender", "MyName.Name", "Departments[Name]");
            result.Any().Should().BeTrue();
            result.ForEach(x => ((string)x.Gender).Should().NotBeNullOrEmpty());
            result.ForEach(x => ((string)x.MyName.Name).Should().NotBeNullOrEmpty());
            result.ForEach(x =>
            {
                var departments = (IEnumerable<object>)x.Departments;
                
                foreach(dynamic department in departments)
                {
                    ((string)department.Name).Should().NotBeNullOrEmpty();
                }
            });

            result = GetResult("Gender", "MyName.Name", "Departments[Name]", "Departments[Sections[Name]]");
            
            result.Any().Should().BeTrue();
            result.ForEach(x => ((string)x.Gender).Should().NotBeNullOrEmpty());
            result.ForEach(x => ((string)x.MyName.Name).Should().NotBeNullOrEmpty());
            result.ForEach(x =>
            {
                var departments = (IEnumerable<object>)x.Departments;

                foreach (dynamic department in departments)
                {
                    ((string)department.Name).Should().NotBeNullOrEmpty();

                    var sections = (IEnumerable<object>)department.Sections;
                    foreach(dynamic section in sections)
                    {
                        ((string)section.Name).Should().NotBeNullOrEmpty();
                    }
                }
            });
        }

        private List<dynamic> GetResult(params string[] fieldsToSelect)
            => _seedDbContext.Persons.ProjectToDynamic(fieldsToSelect).ToDynamicList();
    }

}
