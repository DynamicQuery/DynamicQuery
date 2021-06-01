using System.Collections.Generic;

namespace Seed.Models
{
    public class Person
    {
        public int Id { get; set; }

        public string Gender { get; set; }

        public PersonName MyName { get; set; }

        public List<Department> Departments { get; set; }
    }
}
