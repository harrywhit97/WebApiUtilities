using System;
using System.Linq;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using TodoExampleApi.Models;
using WebApiUtilities.Concrete;
using WebApiUtilities.Extenstions;

namespace TodoExampleApi
{
    public class Startup
    {
        const int ApiVersion = 1;
        const int MaxTop = 10;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("Database");

            services.AddDbContext<TodoListContext>(options =>
                options.UseInMemoryDatabase("Todo"));

            services.AddWebApiServices(ApiVersion);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.EnableDependencyInjection();
                endpoints.MapControllers();
                endpoints.AddOdata("query", GetEdmModel(), MaxTop);
            });

            app.AddSwagger(ApiVersion);
        }
        IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();

            odataBuilder.EntitySet<TodoItem>(nameof(TodoItem));
            odataBuilder.EntitySet<TodoList>(nameof(TodoList));

            return odataBuilder.GetEdmModel();
        }
    }
}
