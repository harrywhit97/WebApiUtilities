using System;
using System.Collections.Generic;
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
using WebApiUtilities.Concrete;
using WebApiUtilities.CrudRequests;
using WebApiUtilities.Interfaces;
using WebApiUtilities.PipelineBehaviours;

namespace WebApiUtilities.Extenstions
{
    public static class IServiceCollectionExtensions
    {
        static readonly Type iRequestHandler = typeof(IRequestHandler<,>);
        
        public static void AddWebApiServices(this IServiceCollection services, string apiTitle, int apiVersion = 1)
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
                c.SwaggerDoc($"v{apiVersion}", new OpenApiInfo { Title = apiTitle, Version = $"v{apiVersion}" });
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
            RegisterReadActions(services);
            RegisterCUDHandlers(services);
        }

        static void RegisterReadActions(IServiceCollection services)
        {
            var assembly = Assembly.GetEntryAssembly();

            var entities = ExtensionHelpers.GetEntities(assembly);

            var dbContext = assembly.DefinedTypes.Where(t => typeof(DbContext).IsAssignableFrom(t))
                                        .FirstOrDefault();

            foreach (var entity in entities)
            {
                var id = GetIdTypeOfEntity(entity);

                var getEntitiesRequest = BaseRequests.GetEntitiesRequest.MakeGenericType(entity, id);

                var getEntitiesResponse = typeof(IQueryable<>).MakeGenericType(entity);

                Register(services, BaseRequests.GetEntitiesRequest, getEntitiesResponse, 
                    BaseRequests.GetEntitiesHandler, entity, id, dbContext);

                Register(services, BaseRequests.GetEntityByIdRequest, entity,
                    BaseRequests.GetEntityByIdHandler, entity, id, dbContext);
            }
        }

        static void Register(IServiceCollection services, Type request, Type response, Type handler, Type entity, Type id, Type dbContext)
        {
            var requestType = request.MakeGenericType(entity, id);
            var serviceType = iRequestHandler.MakeGenericType(requestType, response);
            var handlerType = handler.MakeGenericType(entity, id, requestType, dbContext);
            services.AddTransient(serviceType, handlerType);
        }

        static void RegisterInstantiatedRequest(IServiceCollection services, Type request, Type response, Type handler, Type entity, Type id, Type dbContext)
        {
            var serviceType = iRequestHandler.MakeGenericType(request, response);
            var handlerType = handler.MakeGenericType(entity, id, request, dbContext);
            services.AddTransient(serviceType, handlerType);
        }

        static void RegisterCUDHandlers(IServiceCollection services)
        {
            var assembly = Assembly.GetEntryAssembly();

            var createCommands = ExtensionHelpers.ExtractTypesFromAssembly(assembly, BaseRequests.CreateCommand);
            var updateCommands = ExtensionHelpers.ExtractTypesFromAssembly(assembly, BaseRequests.UpdateCommand);

            var dbContext = assembly.DefinedTypes.Where(t => typeof(DbContext).IsAssignableFrom(t))
                                        .FirstOrDefault();

            foreach (var createCommand in createCommands)
            {
                var createCommandGenericArguments = GetCommandGenericArguments(createCommand, BaseRequests.CreateCommand);

                Type entity = GetEnityFromGenericArguments(createCommandGenericArguments);
                var id = GetIdTypeOfEntity(entity);

                var updateCommand = GetUpdateCommandForEntity(updateCommands, entity);

                RegisterInstantiatedRequest(services, createCommand, entity, 
                    BaseRequests.CreateHandler, entity, id, dbContext);

                if (updateCommand != null)
                    RegisterInstantiatedRequest(services, updateCommand, entity,
                        BaseRequests.UpdateHandler, entity, id, dbContext);
                
                Register(services, BaseRequests.DeleteCommand, typeof(bool), 
                    BaseRequests.DeleteHandler, entity, id, dbContext);
            }
        }
        
        static Type GetEnityFromGenericArguments(Type[] commandGenericArguments)
        {
            return commandGenericArguments
                    .Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntity<>)))
                    .FirstOrDefault();
        }

        static Type GetUpdateCommandForEntity(IEnumerable<Type> updates, Type entity)
        {
            foreach (var update in updates)
            {
                var args = GetCommandGenericArguments(update, BaseRequests.UpdateCommand);
                var updateEntity = GetEnityFromGenericArguments(args);

                if (updateEntity is null)
                    continue;

                if (entity == updateEntity)
                    return update;
            }
            return null;
        }

        static Type[] GetCommandGenericArguments(Type command, Type commandType)
        {
            return command.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == commandType)
                .FirstOrDefault()
                .GetGenericArguments();
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

            var creates = ExtensionHelpers.ExtractTypesFromAssembly(assembly, typeof(ICreateCommand<,>));
            var updates = ExtensionHelpers.ExtractTypesFromAssembly(assembly, typeof(IUpdateCommand<,>));
            var validators = ExtensionHelpers.ExtractTypesFromAssembly(assembly, typeof(IValidator<>));

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

        static void MakeAndAddRequestValidatorService(IEnumerable<Type> requests, Type dto, Type validator, IServiceCollection services)
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

        static class BaseRequests
        {
            public static Type CreateCommand { get => typeof(ICreateCommand<,>); }
            public static Type UpdateCommand { get => typeof(IUpdateCommand<,>); }
            public static Type DeleteCommand { get => typeof(DeleteCommand<,>); }

            public static Type GetEntitiesRequest { get => typeof(GetEntities<,>); }
            public static Type GetEntityByIdRequest { get => typeof(GetEntityById<,>); }

            public static Type CreateHandler { get => typeof(CreateCommandHandler<,,,>); }
            public static Type UpdateHandler { get => typeof(UpdateCommandHandler<,,,>); }
            public static Type DeleteHandler { get => typeof(DeleteEntityHandler<,,,>); }

            public static Type GetEntitiesHandler { get => typeof(GetEntitiesHandler<,,,>); }
            public static Type GetEntityByIdHandler { get => typeof(GetEntityByIdHandler<,,,>); }
        }
    }
}
