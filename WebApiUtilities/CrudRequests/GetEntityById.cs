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

    public class GetEntityById<T, TId> : Entity<TId>, IGetEntityById<T, TId>
        where T : Entity<TId>
    {
        public GetEntityById(TId id) => Id = id;
    }

    public class GetEntityByIdHandler<T, TId, TDbContext> : AbstractRequestHandler<GetEntityById<T, TId>, T, TDbContext>
        where T : Entity<TId>
        where TDbContext : DbContext
    {
        public GetEntityByIdHandler(TDbContext dbContext)
            : base(dbContext)
        { }

        public override async Task<T> Handle(GetEntityById<T, TId> request, CancellationToken cancellationToken)
        {
            return await dbContext.Set<T>().FindAsync(request.Id)
                ?? throw new NotFoundException(typeof(T).Name, request.Id);
        }
    }
}
