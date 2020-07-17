using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using AutoMapper;
using AutoMapper.Internal;
using FluentValidation;
using MediatR;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using WebApiUtilities.Abstract;
using WebApiUtilities.Concrete;
using WebApiUtilities.CrudRequests;
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

            RegisterValidators(services);
            RegisterCrudActionsForRecords(services);
            RegisterCommandHandlers(services, typeof(ICreateCommand<,>), typeof(CreateEntityHandler<,,,>));
            RegisterCommandHandlers(services, typeof(IUpdateCommand<,>), typeof(UpdateEntityHandler<,,,>));
            RegisterCommandHandlers(services, typeof(IDeleteEntity<,>), typeof(DeleteEntityHandler<,,>));

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

        static void RegisterDeleteCommand(Type entity, Type id, Type dbContext, IServiceCollection services)
        {
            var iRequestHandler = typeof(IRequestHandler<,>);
            var emptyDeleteRequest = typeof(DeleteEntity<,>);
            var deleteRequest = emptyDeleteRequest.MakeGenericType(entity, id);
            var serviceType = iRequestHandler.MakeGenericType(deleteRequest, typeof(bool));
            var emptyDeleteHandler = typeof(DeleteEntityHandler<,,>); 
            var handler = emptyDeleteHandler.MakeGenericType(entity, id, dbContext);
            services.AddTransient(serviceType, handler);
        }

        static void RegisterCommandHandlers(IServiceCollection services, Type commandType, Type emptycommandHandler)
        {
            var assembly = Assembly.GetEntryAssembly();

            var commands = ExtractTypesFromAssembly(assembly, commandType);

            var dbContext = assembly.DefinedTypes.Where(t => typeof(DbContext).IsAssignableFrom(t))
                                        .FirstOrDefault();

            foreach (var command in commands)
            {
                var iCommandGenericArguments = command.GetInterfaces()
                                                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == commandType)
                                                .FirstOrDefault()
                                                .GetGenericArguments();

                var entityType = iCommandGenericArguments
                        .Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntity<>)))
                        .FirstOrDefault();

                var idType = GetIdTypeOfEntity(entityType);

                var iRequestHandler = typeof(IRequestHandler<,>);
                var serviceType = iRequestHandler.MakeGenericType(command, entityType);
                var handler = emptycommandHandler.MakeGenericType(entityType, idType, command, dbContext );

                services.AddTransient(serviceType, handler);
                RegisterDeleteCommand(entityType, idType, dbContext, services);
            }
        }

        static Type GetIdTypeOfEntity(Type entityType)
        {
            return entityType.GetInterfaces()
                        .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntity<>))
                        .GetGenericArguments()
                        .FirstOrDefault();
        }

        static void RegisterValidators(IServiceCollection services)
        {
            var assembly = Assembly.GetEntryAssembly();

            var creates = ExtractTypesFromAssembly(assembly, typeof(ICreateCommand<,>));
            var updates = ExtractTypesFromAssembly(assembly, typeof(IUpdateCommand<,>));
            var validators = ExtractTypesFromAssembly(assembly, typeof(IValidator<>));

            foreach (var validator in validators)
            {
                var dtoType = validator.GetInterfaces()
                    .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IValidate<>))
                    .GetGenericArguments()
                    .FirstOrDefault();

                MakeAndAddRequestValidatorService(creates, dtoType, validator, services);
                MakeAndAddRequestValidatorService(updates, dtoType, validator, services);
            }
        }

        static List<Type> ExtractTypesFromAssembly(Assembly assembly, Type genericInterface)
        {
            return assembly.GetExportedTypes()
                            .Where(t => t.GetInterfaces().Any(i =>
                                i.IsGenericType && i.GetGenericTypeDefinition() == genericInterface))
                            .ToList();
        }

        static void MakeAndAddRequestValidatorService(List<Type> requests, Type dto, Type validator, IServiceCollection services)
        {
            var request = requests.Where(x => x.BaseType?.Equals(dto) ?? false)
                    .FirstOrDefault();

            var requestValidator = validator.MakeGenericType(request);
            RegisterValidatorService(request, requestValidator, services);
        }

        static void RegisterValidatorService(Type request, Type validator, IServiceCollection services)
        {
            var iValidator = typeof(IValidator<>);
            var serviceType = iValidator.MakeGenericType(request);
            services.AddTransient(serviceType, validator);
        }
    }
}
