using Microsoft.EntityFrameworkCore;
using Seed;
using System;

namespace Tests.Common
{
    public abstract class AbstractTestBase
    {
        protected SeedDbContext _seedDbContext;

        public AbstractTestBase()
        {
            DbContextOptionsBuilder<SeedDbContext> dbContextOptionsBuilder =
                new DbContextOptionsBuilder<SeedDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());

            _seedDbContext = SeedDbContext.Create();

        }
    }
}
