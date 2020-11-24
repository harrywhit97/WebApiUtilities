using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WebApiUtilities.Identity;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Abstract
{
    public class AuditingDbContext<TId> : IdentityDbContext<User>, IAuditingDbContext
    {
        readonly ITimeService clock;

        public AuditingDbContext(DbContextOptions options, ITimeService clock)
            : base(options)
        {
            this.clock = clock;
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity<TId>>()) //TODO change <long> to find any auditible entity
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
