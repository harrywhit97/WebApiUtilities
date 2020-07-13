using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.CrudRequests
{
    public class GetEntities<T, TId> : IRequest<IQueryable<T>>
            where T : class, IHasId<TId>
    {
    }

    public class GetEntitiesHandler<T, TId> : IRequestHandler<GetEntities<T, TId>, IQueryable<T>>
        where T : class, IHasId<TId>
    {
        readonly DbContext Context;

        public GetEntitiesHandler(DbContext dbContext)
        {
            Context = dbContext;
        }

        public Task<IQueryable<T>> Handle(GetEntities<T, TId> request, CancellationToken cancellationToken)
        {
            return (Task<IQueryable<T>>)Context.Set<T>().AsQueryable();
        }
    }
}
