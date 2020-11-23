using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using WebApiUtilities.Concrete;
using WebApiUtilities.Identity;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Abstract
{
    public class AuditingDbContext : IdentityDbContext<User>, IAuditingDbContext
    {
        readonly IClock clock;

        public AuditingDbContext(DbContextOptions options, IClock clock)
            : base(options)
        {
            this.clock = clock;
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity<long>>()) //TODO change <long> to find any auditible entity
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = clock.Now;
                        entry.Entity.UpdatedAt = clock.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = clock.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
