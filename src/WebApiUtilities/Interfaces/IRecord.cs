using Microsoft.Extensions.DependencyInjection;

namespace WebApiUtilities.Interfaces
{
    public interface IRecord
    {
        public void RegisterServices(IServiceCollection services);
    }
}
