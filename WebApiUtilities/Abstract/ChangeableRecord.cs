using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApiUtilities.CrudRequests;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Abstract
{
    public abstract class ChangeableRecord<T, TId, TDbContext, TCreateCommand, TUpdateCommand> : ReadOnlyRecord<T, TId, TDbContext>, IChangeableRecord
        where T : Entity<TId>
        where TDbContext : DbContext
        where TCreateCommand : Dto<T, TId>, ICreateCommand<T, TId>
        where TUpdateCommand : Dto<T, TId>, IUpdateCommand<T, TId>
    {
        public override void RegisterServices(IServiceCollection services)
        {
            services.AddTransient(typeof(IRequestHandler<TCreateCommand, T>), typeof(CreateEntityHandler<T, TId, TCreateCommand, TDbContext>));
            services.AddTransient(typeof(IRequestHandler<TUpdateCommand, T>), typeof(UpdateEntityHandler<T, TId, TUpdateCommand, TDbContext>));
            services.AddTransient(typeof(IRequestHandler<DeleteEntity<T, TId>, bool>), typeof(DeleteEntityHandler<T, TId, TDbContext>));
            
            base.RegisterServices(services);
        }
    }
}
