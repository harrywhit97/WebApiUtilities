﻿using AutoMapper;
using FluentValidation;
using IdentityServer4.AspNetIdentity;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using WebApiUtilities.Abstract;
using WebApiUtilities.Concrete;
using WebApiUtilities.Identity;
using WebApiUtilities.Interfaces;

namespace WebApiUtilities.Extenstions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddWebApiServices<TDbContext>(this IServiceCollection services, IConfiguration configuration, string apiTitle, int apiVersion = 1)
            where TDbContext : IdentityDbContext<User>
        {
            services.AddOData();
            services.AddControllers();

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });

            AddOdataFormatters(services);
            services.AddLogging();

# if DEBUG
            AddSwagger(services, apiVersion, apiTitle);
#endif

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddTransient<ITimeService, TimeService>();

            services.AddSingleton<AppSettings>();
            RegisterIdentity<TDbContext>(services, configuration);
            RegisterRecords(services);
        }

        private static void AddOdataFormatters(IServiceCollection services)
        {
            services.AddMvcCore(options =>
            {
                //Work around to enable swagger with Odata
                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));

                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
            });
        }

        private static void AddSwagger(IServiceCollection services, int apiVersion, string apiTitle)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc($"v{apiVersion}", new OpenApiInfo { Title = apiTitle, Version = $"v{apiVersion}" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
        }

        private static void RegisterIdentity<TDbContext>(IServiceCollection services, IConfiguration configuration)
            where TDbContext : IdentityDbContext<User>
        {
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            var appSettings = new AppSettings(configuration);
            var Config = IdentityConfig.GetDefault(appSettings);

            services.AddIdentityServer()
                    .AddDeveloperSigningCredential() //This is for dev only scenarios when you don’t have a certificate to use.
                    .AddInMemoryApiScopes(Config.ApiScopes)
                    .AddInMemoryClients(Config.Clients)
                    .AddInMemoryPersistedGrants()
                    .AddInMemoryIdentityResources(Config.IdentityResources)
                    .AddResourceOwnerValidator<ResourceOwnerPasswordValidator<User>>()
                    .AddInMemoryApiResources(Config.ApiResources);

            services.AddIdentityCore<User>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<TDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // base-address of your identityserver
                options.Authority = appSettings.HttpsUrl;

                // name of the API resource
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });
            services.AddTransient<SignInManager<User>>();
            services.AddTransient<IUserService, UserService>();
        }

        static void RegisterRecords(IServiceCollection services)
        {
            var assembly = Assembly.GetEntryAssembly();
            var entities = ExtensionHelpers.GetEntities(assembly);
            var dbContext = assembly.DefinedTypes.Where(t => typeof(DbContext).IsAssignableFrom(t))
                                       .FirstOrDefault();

            var iRecordService = typeof(IRecordService<,>);
            var baseRecordService = typeof(BaseRecordService<,,>);

            foreach (var entity in entities)
            {
                var id = GetIdTypeOfEntity(entity);
                var serviceType = iRecordService.MakeGenericType(entity, id);
                var handlerType = baseRecordService.MakeGenericType(dbContext, entity, id);
                services.AddTransient(serviceType, handlerType);
            }
        }

        static Type GetIdTypeOfEntity(Type entityType)
        {
            return entityType.GetInterfaces()
                        .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntity<>))
                        .GetGenericArguments()
                        .FirstOrDefault();
        }
    }
}
