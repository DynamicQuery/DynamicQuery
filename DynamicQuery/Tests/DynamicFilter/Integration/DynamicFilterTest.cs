using System.Collections.Generic;
using Tests.Common.Model;
using Xunit;
using DynamicFilter;
using DynamicFilter.Operations;
using System.Linq;
using FluentAssertions;
using DynamicFilter.Common;

namespace Tests.DynamicFilter.Integration
{
    public class DynamicFilterTest
    {
        private List<Person> _persons;

        public DynamicFilterTest()
        {
            _persons = Seed();
        }


        [Fact]
        public void SimpleObjectPropertyFilterTest()
        {
            DynamicFilter<Person> dynamicFilter = new DynamicFilter<Person>();
            dynamicFilter.By("Gender", Operations.EqualTo, "F");

            _persons.Where(dynamicFilter).ToList().Count.Should().Equals(1);
        }

        [Fact]
        public void SimpleLevel2ObjectPropertyFilterTest()
        {
            DynamicFilter<Person> dynamicFilter = new DynamicFilter<Person>();
            dynamicFilter.By("MyName.Name", Operations.Contains, "malfoy");

            _persons.Where(dynamicFilter).ToList().Count.Should().Equals(1);
        }

        [Fact]
        public void SimpleLevel2ListPropertyFilterTest()
        {
            DynamicFilter<Person> dynamicFilter = new DynamicFilter<Person>();
            dynamicFilter.By("Departments[Name]", Operations.EqualTo, "HR");

            _persons.Where(dynamicFilter).ToList().Count.Should().Equals(1);
        }

        [Fact]
        public void MultipleLevel2ObjectPropertyFilterTest()
        {
            DynamicFilter<Person> dynamicFilter = new DynamicFilter<Person>();
            dynamicFilter.By("Gender", Operations.EqualTo, "F");
            dynamicFilter.By("Gender", Operations.EqualTo, "M", Connector.And);
            _persons.Where(dynamicFilter).ToList().Count.Should().Equals(0);

            dynamicFilter = new DynamicFilter<Person>();
            dynamicFilter.By("Gender", Operations.EqualTo, "F");
            dynamicFilter.By("Gender", Operations.EqualTo, "M", Connector.Or);
            _persons.Where(dynamicFilter).ToList().Count.Should().Equals(2);
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
