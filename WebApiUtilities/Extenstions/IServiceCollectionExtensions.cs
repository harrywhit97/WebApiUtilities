using System;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using WebApiUtilities.Concrete;
using WebApiUtilities.Interfaces;
using WebApiUtilities.PipelineBehaviours;

namespace WebApiUtilities.Extenstions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddWebApiServices(this IServiceCollection services, int apiVersion)
        {
            services.AddOData();
            services.AddControllers();

            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });

            services.AddMvcCore(options =>
            {
                //Work around to enable swagger with Odata
                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));

                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc($"v{apiVersion}", new OpenApiInfo { Title = "My API", Version = $"v{apiVersion}" });
            });

            services.AddLogging();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient<IClock, Clock>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));

            RegisterCrudActionsForRecords(services);
        }

        static void RegisterCrudActionsForRecords(IServiceCollection services)
        {
            var assembly = Assembly.GetEntryAssembly();

            var types = assembly.GetExportedTypes()
                .Where(x => x.GetInterfaces().Any(i => i.Name == nameof(IRecord)));

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type) as IRecord;
                instance.RegisterServices(services);
            }
        }
    }
}
