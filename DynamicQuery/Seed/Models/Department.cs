using System.Collections.Generic;

namespace Seed.Models
{
    public class Department
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Section> Sections { get; set; } = new List<Section>();

    }
}
