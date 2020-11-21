using Microsoft.EntityFrameworkCore;
using System;
using WebApiUtilities.Abstract;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Testing
{
    public static class DbContextMocker<TDbContext>
        where TDbContext : DbContext, new()
    {
        public static TDbContext GetTestingDbContext()
        {
            var dbContext = new DbContext(DbContextMockerHelpers<TDbContext>.GetOptions());
            dbContext.Database.EnsureCreated();
            return (TDbContext)dbContext;
        }
    }

    public static class AuditingDbContextMocker<TDbContext>
        where TDbContext : AuditingDbContext, new()
    {
        public static TDbContext GetTestingDbContext(IClock clock)
        {
            var dbContext = new AuditingDbContext(DbContextMockerHelpers<TDbContext>.GetOptions(), clock);
            dbContext.Database.EnsureCreated();
            return (TDbContext)dbContext;
        }
    }

    static class DbContextMockerHelpers<TDbContext>
        where TDbContext : DbContext
    {
        public static DbContextOptions GetOptions()
        {
            return new DbContextOptionsBuilder<TDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;
        }
    }
}
