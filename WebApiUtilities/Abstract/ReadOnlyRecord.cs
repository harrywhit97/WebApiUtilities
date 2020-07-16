using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using WebApiUtilities.CrudRequests;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Abstract
{
    public abstract class ReadOnlyRecord<T, TId, TDbContext> : IRecord
        where T : Entity<TId>
        where TDbContext : DbContext
    {
        public virtual void RegisterServices(IServiceCollection services)
        {
            services.AddTransient(typeof(IRequestHandler<GetEntities<T, TId>, IQueryable<T>>), typeof(GetEntitiesHandler<T, TId, IGetEntities<T, TId>, TDbContext>));
            services.AddTransient(typeof(IRequestHandler<GetEntityById<T, TId>, T>), typeof(GetEntityByIdHandler<T, TId, TDbContext>));
        }
    }
}
