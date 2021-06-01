using Microsoft.EntityFrameworkCore;
using System;
using Tests.Common.Persistence;

namespace Tests.Common
{
    public abstract class AbstractTestBase
    {
        protected SeedDbContext _seedDbContext;

        public AbstractTestBase()
        {
            DbContextOptionsBuilder<SeedDbContext> dbContextOptionsBuilder =
                new DbContextOptionsBuilder<SeedDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());

            _seedDbContext = new SeedDbContext(dbContextOptionsBuilder.Options);

            _seedDbContext.Persons.AddRange(SeedDbContext.Seed());
            _seedDbContext.SaveChanges();

        }
    }
}
