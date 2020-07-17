using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApiUtilities.CrudRequests;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Abstract
{
    public abstract class ChangeableRecord<T, TId, TDbContext> : ReadOnlyRecord<T, TId, TDbContext>, IChangeableRecord
        where T : Entity<TId>
        where TDbContext : DbContext
    {
        public override void RegisterServices(IServiceCollection services)
        {
            base.RegisterServices(services);
        }
    }
}
