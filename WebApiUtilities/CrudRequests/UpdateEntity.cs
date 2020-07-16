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

    public abstract class UpdateCommand<T, TId> : IUpdateCommand<T, TId>
        where T : Entity<TId>
    {
        public TId Id { get; set; }
    }

    public class UpdateEntityHandler<T, TId, TUpdateCommand, TDbContext> : IRequestHandler<TUpdateCommand, T>
        where T : Entity<TId>
        where TUpdateCommand : class, IUpdateCommand<T, TId>, IHasId<TId>
        where TDbContext : DbContext
    {
        readonly protected TDbContext dbContext;
        readonly protected IMapper mapper;

        public UpdateEntityHandler(TDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<T> Handle(TUpdateCommand command, CancellationToken cancellationToken)
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
