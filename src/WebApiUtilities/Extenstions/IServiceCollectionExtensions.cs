using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

            //RegisterValidators(services);


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
                options.User.RequireUniqueEmail = false;
            });

            services.AddTransient<AppSettings>();
            services.AddTransient<IUserService, UserService>();

            RegisterRecords(services);
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

        static void RegisterValidators(IServiceCollection services)
        {
            var assembly = Assembly.GetEntryAssembly();

            //var creates = ExtensionHelpers.ExtractTypesFromAssembly(assembly, typeof(ICreateCommand<,>));
            //var updates = ExtensionHelpers.ExtractTypesFromAssembly(assembly, typeof(IUpdateCommand<,>));
            //var validators = ExtensionHelpers.ExtractTypesFromAssembly(assembly, typeof(IValidator<>));

            //foreach (var validator in validators)
            //{
            //    var dtoType = validator.GetInterfaces()
            //        .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IValidate<>))
            //        .GetGenericArguments()
            //        .FirstOrDefault();

            //    MakeAndAddRequestValidatorService(creates, dtoType, validator, services);
            //    MakeAndAddRequestValidatorService(updates, dtoType, validator, services);
            //}
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
    }
}
