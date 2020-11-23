using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using TodoExampleApi.Models;
using WebApiUtilities.Extenstions;

namespace TodoExampleApi
{
    public class Startup
    {
        const string ApiTitle = "TodoApi";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("Database");

            services.AddDbContext<TodoListContext>(options =>
                options.UseInMemoryDatabase("Todo"));

            services.AddWebApiServices<TodoListContext>(ApiTitle);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.AddWebApiUtilities(GetEdmModel(), ApiTitle);
        }

        IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();

            odataBuilder.EntitySet<TodoItem>(nameof(TodoItem));

            return odataBuilder.GetEdmModel();
        }
    }
}
