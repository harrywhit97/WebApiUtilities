﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApiUtilities.Abstract;

namespace WebApiUtilities.CrudRequests
{
    public interface IGetEntities<T, TId> : IRequest<IQueryable<T>> 
        where T : Entity<TId>
    { }

    public class GetEntities<T, TId> : IGetEntities<T, TId> 
        where T : Entity<TId>
    { }

    public class GetEntitiesHandler<T, TId, TGetEntitiesRequest, TDbContext> : AbstractRequestHandler<TGetEntitiesRequest, IQueryable<T>, TDbContext>
        where T : Entity<TId>
        where TDbContext : DbContext
        where TGetEntitiesRequest : IRequest<IQueryable<T>>
    {
        public GetEntitiesHandler(TDbContext dbContext) 
            : base(dbContext)
        { }

        public override Task<IQueryable<T>> Handle(TGetEntitiesRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(dbContext.Set<T>().AsQueryable());
        }
    }
}