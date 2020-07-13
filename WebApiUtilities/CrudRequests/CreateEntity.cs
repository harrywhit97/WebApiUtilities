using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.CrudRequests
{
    public class CreateEntity<T, TDto> : IRequest<T>
    {
        public TDto Entity { get; set; }
    }

    public class CreateEntityHandler<T, TId, TDto> : IRequestHandler<CreateEntity<T, TDto>, T>
        where T : class, IHasId<TId>
        where TDto : class
    {
        readonly protected DbContext Context;
        readonly protected IMapper Mapper;

        public CreateEntityHandler(DbContext dbContext, IMapper mapper)
        {
            Context = dbContext;
            Mapper = mapper;
        }

        public async Task<T> Handle(CreateEntity<T, TDto> request, CancellationToken cancellationToken)
        {
            var entity = Mapper.Map<T>(request.Entity);
            Context.Set<T>().Add(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
    }
}
