using System.Collections.Generic;

namespace Tests.Common.Model
{
    public class Department
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Section> Sections { get; set; } = new List<Section>();

    }
}
