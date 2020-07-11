using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WebApiUtilities.Exceptions;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.CrudRequests
{
    public abstract class UpdateEntityFromRequestHandler<T, TId, TRequest> : IRequestHandler<TRequest, T>
            where T : class, IHasId<TId>
            where TRequest : IRequest<T>, IMapFrom<T>, IHasId<TId>
    {
        readonly protected DbContext dbcontext;
        readonly protected IMapper mapper;

        public UpdateEntityFromRequestHandler(DbContext dbContext, IMapper mapper)
        {
            this.dbcontext = dbContext;
            this.mapper = mapper;
        }

        public async Task<T> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var existingEntity = await dbcontext.Set<T>().FindAsync(request.Id)
                ?? throw new NotFoundException(typeof(T).Name, request.Id);

            dbcontext.Entry(existingEntity).State = EntityState.Detached;

            var entity = mapper.Map<T>(request);

            dbcontext.Entry(entity).State = EntityState.Modified;
            await dbcontext.SaveChangesAsync();
            return entity;
        }
    }
}
