using System;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using WebApiUtilities.Abstract;
using WebApiUtilities.CrudRequests;
using WebApiUtilities.Interfaces;
using WebApiUtilities.PipelineBehaviours;

namespace WebApiUtilities.Concrete
{
    public static class ConfigureWebApiServices
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddOData();

            services.AddMvc()
                .AddControllersAsServices()
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });

            //Work around to enable swagger with Odata
            services.AddMvcCore(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            services.AddLogging();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient<IClock, Clock>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        }

        public static void RegisterCrudActionsForType<T, TId, TDto, TDbContext, TCreateCommand, TUpdateCommand>(IServiceCollection services)
            where T : Entity<TId>
            where TDto : class
            where TDbContext : DbContext
            where TCreateCommand : class, TDto, ICreateCommand<T, TId>
            where TUpdateCommand : class, TDto, IUpdateCommand<T, TId>
        {
            services.AddTransient(typeof(IRequestHandler<TCreateCommand, T>), typeof(CreateEntityHandler<T, TId, TCreateCommand, TDbContext>));
            services.AddTransient(typeof(IRequestHandler<TUpdateCommand, T>), typeof(UpdateEntityHandler<T, TId, TUpdateCommand, TDbContext>));
            services.AddTransient(typeof(IRequestHandler<GetEntities<T, TId>, IQueryable<T>>), typeof(GetEntitiesHandler<T, TId, IGetEntities<T, TId>, TDbContext>));
            services.AddTransient(typeof(IRequestHandler<GetEntityById<T, TId>, T>), typeof(GetEntityByIdHandler<T, TId, TDbContext>));
            services.AddTransient(typeof(IRequestHandler<DeleteEntity<T, TId>, bool>), typeof(DeleteEntityHandler<T, TId, TDbContext>));
        }
    }
}
