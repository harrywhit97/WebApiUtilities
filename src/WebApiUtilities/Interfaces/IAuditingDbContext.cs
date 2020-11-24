using System.Threading;
using System.Threading.Tasks;

namespace WebApiUtilities.Interfaces
{
    public interface IAuditingDbContext
    {
        public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken);
    }
}
