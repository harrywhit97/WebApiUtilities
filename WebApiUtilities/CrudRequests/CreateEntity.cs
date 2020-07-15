using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WebApiUtilities.Abstract;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.CrudRequests
{
    public interface ICreateCommand<T, TId> : IRequest<T>, IMapFrom<T>
        where T : Entity<TId>
    { }

    public class CreateCommand<T, TId> : ICreateCommand<T, TId> 
        where T : Entity<TId>
    {
    }

    public class CreateEntityHandler<T, TId, TCreateCommand, TDbContext> : IRequestHandler<TCreateCommand, T>
        where T : Entity<TId>
        where TCreateCommand : class, ICreateCommand<T, TId>, IMapFrom<T>
        where TDbContext : DbContext
    {
        readonly protected TDbContext Context;
        readonly protected IMapper Mapper;

        public CreateEntityHandler(TDbContext dbContext, IMapper mapper)
        {
            Context = dbContext;
            Mapper = mapper;
        }

        public async Task<T> Handle(TCreateCommand command, CancellationToken cancellationToken)
        {
            var entity = Mapper.Map<T>(command);
            Context.Set<T>().Add(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
    }
}
