using DynamicFilter;
using DynamicFilter.Common;
using DynamicFilter.Operations;
using FluentAssertions;
using Seed.Models;
using System.Collections.Generic;
using System.Linq;
using Tests.Common;
using Xunit;

namespace Tests.DynamicFilter.Integration
{
    public class DynamicFilterTest : AbstractTestBase
    {

        [Fact]
        public void SimpleObjectPropertyFilterTest()
        {
            Filter<Person> dynamicFilter = new Filter<Person>();
            dynamicFilter.By("Gender", Operations.EqualTo, "F");

            _seedDbContext.Persons.Where(dynamicFilter).ToList().Count.Should().Equals(1);
        }

        [Fact]
        public void SimpleLevel2ObjectPropertyFilterTest()
        {
            Filter<Person> dynamicFilter = new Filter<Person>();
            dynamicFilter.By("MyName.Name", Operations.Contains, "malfoy");

            _seedDbContext.Persons.Where(dynamicFilter).ToList().Count.Should().Equals(1);
        }

        [Fact]
        public void SimpleLevel2ListPropertyFilterTest()
        {
            Filter<Person> dynamicFilter = new Filter<Person>();
            dynamicFilter.By("Departments[Name]", Operations.EqualTo, "HR");

            _seedDbContext.Persons.Where(dynamicFilter).ToList().Count.Should().Equals(1);
        }

        [Fact]
        public void MultipleLevel2ObjectPropertyFilterTest()
        {
            Filter<Person> dynamicFilter = new Filter<Person>();
            dynamicFilter.By("Gender", Operations.EqualTo, "F");
            dynamicFilter.By("Gender", Operations.EqualTo, "M", Connector.And);
            _seedDbContext.Persons.Where(dynamicFilter).ToList().Count.Should().Equals(0);

            dynamicFilter = new Filter<Person>();
            dynamicFilter.By("Gender", Operations.EqualTo, "F");
            dynamicFilter.By("Gender", Operations.EqualTo, "M", Connector.Or);
            _seedDbContext.Persons.Where(dynamicFilter).ToList().Count.Should().Equals(2);
        }

        [Fact]
        public void MultipleLevel2ListPropertyFilterTest()
        {
            Filter<Person> dynamicFilter = new Filter<Person>();
            dynamicFilter.By("Departments[Name]", Operations.EqualTo, "HR");
            dynamicFilter.By("Departments[Name]", Operations.EqualTo, "IT", Connector.And);
            _seedDbContext.Persons.Where(dynamicFilter).ToList().Count.Should().Equals(0);

            dynamicFilter = new Filter<Person>();
            dynamicFilter.By("Departments[Name]", Operations.EqualTo, "HR");
            dynamicFilter.By("Departments[Name]", Operations.EqualTo, "IT", Connector.Or);
            _seedDbContext.Persons.Where(dynamicFilter).ToList().Count.Should().Equals(2);
        }


        [Fact]
        public void MultipleLevel2ObjectAndListPropertyFilterTest()
        {
            Filter<Person> dynamicFilter = new Filter<Person>();
            dynamicFilter.By("Departments[Name]", Operations.EqualTo, "HR");
            dynamicFilter.By("MyName.Name", Operations.StartsWith, "Jane", Connector.And);
            _seedDbContext.Persons.Where(dynamicFilter).ToList().Count.Should().Equals(1);

            dynamicFilter = new Filter<Person>();
            dynamicFilter.By("Departments[Name]", Operations.EqualTo, "ABCD");
            dynamicFilter.By("MyName.Name", Operations.StartsWith, "Jane", Connector.And);
            _seedDbContext.Persons.Where(dynamicFilter).ToList().Count.Should().Equals(0);


            dynamicFilter = new Filter<Person>();
            dynamicFilter.By("Departments[Name]", Operations.EqualTo, "ABCD");
            dynamicFilter.By("MyName.Name", Operations.StartsWith, "Jane", Connector.Or);
            _seedDbContext.Persons.Where(dynamicFilter).ToList().Count.Should().Equals(1);


            dynamicFilter = new Filter<Person>();
            dynamicFilter.By("Departments[Name]", Operations.EqualTo, "ABCD");
            dynamicFilter.By("MyName.Name", Operations.StartsWith, "Jane", Connector.Or);
            dynamicFilter.By("MyName.Name", Operations.EndsWith, "Malfoy", Connector.Or);
            _seedDbContext.Persons.Where(dynamicFilter).ToList().Count.Should().Equals(2);
        }


        [Fact]
        public void MultipleLevel3ListPropertyFilterTest()
        {
            Filter<Person> dynamicFilter = new Filter<Person>();
            dynamicFilter.By("Departments[Sections[Name]]", Operations.StartsWith, "HR");
            dynamicFilter.By("Departments[Sections[Name]]", Operations.StartsWith, "IT", Connector.And);
            _seedDbContext.Persons.Where(dynamicFilter).ToList().Count.Should().Be(0);

            dynamicFilter = new Filter<Person>();
            dynamicFilter.By("Departments[Sections[Name]]", Operations.StartsWith, "HR");
            dynamicFilter.By("Departments[Sections[Name]]", Operations.StartsWith, "IT", Connector.Or);
            _seedDbContext.Persons.Where(dynamicFilter).ToList().Count.Should().Be(2);
        }

        [Fact]
        public void ComplexMultiplePropertyFilterTest()
        {
            Filter<Person> dynamicFilter = new Filter<Person>();
            dynamicFilter.By("Departments[Sections[Name]]", Operations.StartsWith, "HR");
            dynamicFilter.By("Departments[Sections[Name]]", Operations.StartsWith, "IT", Connector.And);
            dynamicFilter.By("MyName.Name", Operations.StartsWith, "Joe", Connector.And);
            _seedDbContext.Persons.Where(dynamicFilter).ToList().Count.Should().Equals(0);


            dynamicFilter = new Filter<Person>();
            dynamicFilter.By("Departments[Sections[Name]]", Operations.StartsWith, "HR");
            dynamicFilter.By("Departments[Sections[Name]]", Operations.StartsWith, "IT", Connector.Or);
            dynamicFilter.By("MyName.Name", Operations.StartsWith, "Joe", Connector.And);
            _seedDbContext.Persons.Where(dynamicFilter).ToList().Count.Should().Equals(2);
        }

        private List<Person> Seed()
        {
            Person jane = new Person()
            {
                Id = 1,
                MyName = new PersonName() { Id = 1, Name = "Jane Doe" },
                Gender = "F",
                Departments = new List<Department>()
                {
                    new Department()
                    {
                        Id = 1, Name = "IT",
                        Sections = new List<Section>()
                        {
                            new Section(){ Id = 1,  Name = "IT-A"},
                            new Section(){ Id = 2,  Name = "IT-B"}
                        }
                    },
                    new Department()
                    {
                        Id = 2, Name = "HR",
                        Sections = new List<Section>()
                        {
                            new Section(){ Id = 3,  Name = "HR-A"},
                            new Section(){ Id = 4,  Name = "HR-B"},
                            new Section(){ Id = 5,  Name = "HR-C"}
                        }
                    },
                }
            };

            Person draco = new Person()
            {
                Id = 2,
                MyName = new PersonName() { Id = 2, Name = "Draco Malfoy" },
                Gender = "M",
                Departments = new List<Department>()
                {
                    new Department()
                    {
                        Id = 3, Name = "IT",
                        Sections = new List<Section>()
                        {
                            new Section(){ Id = 1,  Name = "IT-A"}
                        }
                    },
                    new Department()
                    {
                        Id = 4, Name = "HR",
                        Sections = new List<Section>()
                        {
                            new Section(){ Id = 4,  Name = "HR-A"}
                        }
                    },
                }
            };


            return new List<Person>() { jane, draco };
        }
    }
}
