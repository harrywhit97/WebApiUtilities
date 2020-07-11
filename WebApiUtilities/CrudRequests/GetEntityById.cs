using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WebApiUtilities.Exceptions;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.CrudRequests
{
    public class GetEntityById<T, TId> : IRequest<T>
        where T : class, IHasId<TId>
    {
        public TId Id { get; set; }

        public GetEntityById(TId id)
        {
            Id = id;
        }
    }

    public abstract class GetEntityByIdHandler<T, TId> : IRequestHandler<GetEntityById<T, TId>, T>
        where T : class, IHasId<TId>
    {
        readonly DbContext Context;

        public GetEntityByIdHandler(DbContext dbContext)
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
