using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WebApiUtilities.Abstract;
using WebApiUtilities.Exceptions;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.CrudRequests
{
    public interface IDeleteEntity<T, TId> : IRequest<bool>, IHasId<TId>
        where T : Entity<TId>
    { }

    public class DeleteEntity<T, TId> : Entity<TId>, IDeleteEntity<T, TId>
        where T : Entity<TId>
    {
        public DeleteEntity(TId id) => Id = id;
    }

    public class DeleteEntityHandler<T, TId, TDbContext> : AbstractRequestHandler<IDeleteEntity<T, TId>, bool, TDbContext>
        where T : Entity<TId>
        where TDbContext : DbContext
    {
        public DeleteEntityHandler(TDbContext dbContext)
            : base(dbContext) 
        { }

        public override async Task<bool> Handle(IDeleteEntity<T, TId> request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.Set<T>().FindAsync(request.Id)
                ?? throw new NotFoundException(typeof(T).Name, request.Id);

            dbContext.Set<T>().Remove(entity);
            dbContext.SaveChanges();
            return true;
        }
    }
}
