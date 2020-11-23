using IdentityServer4.AspNetIdentity;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using System.Collections.Generic;
using TodoExampleApi.Models;
using WebApiUtilities.Concrete;
using WebApiUtilities.Extenstions;
using WebApiUtilities.Identity;

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
            //services.AddIdentity<User, IdentityRole>()
            //    .AddEntityFrameworkStores<TodoListContext>()
            //    .AddDefaultTokenProviders();

            var Config = IdentityConfig.Default;

            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()        //This is for dev only scenarios when you don’t have a certificate to use.
                    .AddInMemoryApiScopes(Config.ApiScopes)
                    .AddInMemoryClients(Config.Clients)
                    .AddInMemoryPersistedGrants()
                    .AddInMemoryIdentityResources(Config.IdentityResources)
                    .AddResourceOwnerValidator<ResourceOwnerPasswordValidator<User>>()
                    .AddInMemoryApiResources(Config.ApiResources);

            //services.AddIdentity<User, IdentityRole>()
            //       .AddEntityFrameworkStores<TodoListContext>()
            //       .AddDefaultTokenProviders();

            services.AddIdentityCore<User>(cfg =>
                    {
                        cfg.User.RequireUniqueEmail = true;
                    })
                .AddEntityFrameworkStores<TodoListContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // base-address of your identityserver
                options.Authority = "https://localhost:5003";

                // name of the API resource
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });

            services.AddTransient<SignInManager<User>>();

            services.AddWebApiServices(ApiTitle);
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
            //odataBuilder.EntitySet<TodoList>(nameof(TodoList));

            return odataBuilder.GetEdmModel();
        }
    }
}
