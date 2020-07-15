using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApiUtilities.Abstract;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.CrudRequests
{
    public interface IGetEntities<T, TId> : IRequest<IQueryable<T>> 
        where T : Entity<TId>
    {
    }

    public class GetEntities<T, TId> : IGetEntities<T, TId> 
        where T : Entity<TId>
    { 
    }


    public class GetEntitiesHandler<T, TId, TRequest, TDbContext> : IRequestHandler<TRequest, IQueryable<T>>
        where T : class, IHasId<TId>
        where TDbContext : DbContext
        where TRequest : IRequest<IQueryable<T>>
    {
        readonly TDbContext Context;

        public GetEntitiesHandler(TDbContext dbContext)
        {
            Context = dbContext;
        }

        public Task<IQueryable<T>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Context.Set<T>().AsQueryable());
        }
    }
}
