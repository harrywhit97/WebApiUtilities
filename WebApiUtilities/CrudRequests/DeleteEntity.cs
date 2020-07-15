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

    public class DeleteEntity<T, TId> : IDeleteEntity<T, TId>
        where T : Entity<TId>
    {
        public TId Id { get; set; }

        public DeleteEntity(TId id)
        {
            Id = id;
        }
    }

    public class DeleteEntityHandler<T, TId, TDbContext> : IRequestHandler<IDeleteEntity<T, TId>, bool>
        where T : Entity<TId>
        where TDbContext : DbContext
    {
        readonly TDbContext Context;

        public DeleteEntityHandler(TDbContext dbContext)
        {
            Context = dbContext;
        }

        public async Task<bool> Handle(IDeleteEntity<T, TId> request, CancellationToken cancellationToken)
        {
            var entity = await Context.Set<T>().FindAsync(request.Id)
                ?? throw new NotFoundException(typeof(T).Name, request.Id);

            Context.Set<T>().Remove(entity);
            Context.SaveChanges();
            return true;
        }
    }
}
