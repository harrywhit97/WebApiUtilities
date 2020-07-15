using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WebApiUtilities.Abstract;
using WebApiUtilities.Exceptions;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.CrudRequests
{
    public interface IGetEntityById<T, TId> : IRequest<T>, IHasId<TId>
        where T : Entity<TId>
    { }

    public class GetEntityById<T, TId> : IGetEntityById<T, TId>
        where T : Entity<TId>
    {
        public TId Id { get; set; }

        public GetEntityById(TId id)
        {
            Id = id;
        }
    }

    public class GetEntityByIdHandler<T, TId, TDbContext> : IRequestHandler<GetEntityById<T, TId>, T>
        where T : Entity<TId>
        where TDbContext : DbContext
    {
        readonly TDbContext Context;

        public GetEntityByIdHandler(TDbContext dbContext)
        {
            Context = dbContext;
        }

        public async Task<T> Handle(GetEntityById<T, TId> request, CancellationToken cancellationToken)
        {
            return await Context.Set<T>().FindAsync(request.Id)
                ?? throw new NotFoundException(typeof(T).Name, request.Id);
        }
    }
}
