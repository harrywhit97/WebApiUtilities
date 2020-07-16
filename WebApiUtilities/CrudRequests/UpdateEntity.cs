using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WebApiUtilities.Abstract;
using WebApiUtilities.Exceptions;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.CrudRequests
{
    public interface IUpdateCommand<T, TId> :  IRequest<T>, IHasId<TId>
        where T : Entity<TId>
    {
    }

    public class UpdateEntityHandler<T, TId, TUpdateCommand, TDbContext> : AbstractRequestHandler<TUpdateCommand, T, TDbContext>
        where T : Entity<TId>
        where TUpdateCommand : class, IUpdateCommand<T, TId>, IHasId<TId>
        where TDbContext : DbContext
    {
        readonly protected IMapper mapper;

        public UpdateEntityHandler(TDbContext dbContext, IMapper mapper)
            : base(dbContext) => this.mapper = mapper;

        public override async Task<T> Handle(TUpdateCommand command, CancellationToken cancellationToken)
        {
            var existingEntity = await dbContext.Set<T>().FindAsync(command.Id)
                ?? throw new NotFoundException(typeof(T).Name, command.Id);

            dbContext.Entry(existingEntity).State = EntityState.Detached;

            var entity = mapper.Map<T>(command);

            dbContext.Entry(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
