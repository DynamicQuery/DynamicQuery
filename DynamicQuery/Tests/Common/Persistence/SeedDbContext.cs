using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Tests.Common.Model;
using Tests.Common.Persistence.Configurations;

namespace Tests.Common.Persistence
{
    public class SeedDbContext : DbContext
    {
        public SeedDbContext(DbContextOptions<SeedDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new PersonConfiguration())
                .ApplyConfiguration(new PersonNameConfiguration())
                .ApplyConfiguration(new DepartmentConfiguration())
                .ApplyConfiguration(new SectionConfiguration());


            base.OnModelCreating(modelBuilder);
        }

        public static List<Person> Seed()
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
                            new Section(){ Id = 6,  Name = "IT-A"}
                        }
                    },
                    new Department()
                    {
                        Id = 4, Name = "HR",
                        Sections = new List<Section>()
                        {
                            new Section(){ Id = 7,  Name = "HR-A"}
                        }
                    },
                }
            };


            return new List<Person>() { jane, draco };
        }
    }
}
