using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.CrudRequests
{
    public abstract class CreateEntityFromRequestHandler<T, TId, TRequest> : IRequestHandler<TRequest, T>
        where T : class, IHasId<TId>
        where TRequest : IRequest<T>, IMapFrom<T>
    {
        readonly protected DbContext Context;
        readonly protected IMapper Mapper;

        public CreateEntityFromRequestHandler(DbContext dbContext, IMapper mapper)
        {
            Context = dbContext;
            Mapper = mapper;
        }

        public async Task<T> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var entity = Mapper.Map<T>(request);
            Context.Set<T>().Add(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
    }
}
