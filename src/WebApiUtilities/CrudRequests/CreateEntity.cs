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

    public class CreateCommandHandler<T, TId, TCreateCommand, TDbContext> : AbstractRequestHandler<TCreateCommand, T, TDbContext>
        where T : Entity<TId>
        where TCreateCommand : class, ICreateCommand<T, TId>, IMapFrom<T>
        where TDbContext : DbContext
    {
        readonly protected IMapper mapper;

        public CreateCommandHandler(TDbContext dbContext, IMapper mapper)
            : base(dbContext) => this.mapper = mapper;

        public override async Task<T> Handle(TCreateCommand command, CancellationToken cancellationToken)
        {
            var entity = mapper.Map<T>(command);
            dbContext.Set<T>().Add(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
