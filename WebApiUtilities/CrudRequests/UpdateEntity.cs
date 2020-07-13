using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WebApiUtilities.Exceptions;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.CrudRequests
{
    public class UpdateEntity<T, TId, TDto> :  IRequest<T>, IHasId<TId>
    {
        public TId Id { get; set; }
        public TDto Entity { get; set; }
    }

    public class UpdateEntityHandler<T, TId, TDto> : IRequestHandler<UpdateEntity<T, TId, TDto>, T>
            where T : class, IHasId<TId>
    {
        readonly protected DbContext dbContext;
        readonly protected IMapper mapper;

        public UpdateEntityHandler(DbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<T> Handle(UpdateEntity<T, TId, TDto> request, CancellationToken cancellationToken)
        {
            var existingEntity = await dbContext.Set<T>().FindAsync(request.Id)
                ?? throw new NotFoundException(typeof(T).Name, request.Id);

            dbContext.Entry(existingEntity).State = EntityState.Detached;

            var entity = mapper.Map<T>(request);

            dbContext.Entry(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
