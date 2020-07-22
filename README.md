# WebApiUtilities
 
 WebApiUtilities is a library that contains tools to facilitate the rapid development of RESTful ASP .Net Core 3.1 Web APIs utilising Entity Framework Core, Fluent Validation, Odata and Swagger.
 
 
 ## Getting set up
 
 1. Make a new ASP .Net Core 3.1 Web API project and install the WebApiUtilities nuget package.
 2. For each model you need to add the following classes
    1. Model which inherits `Entity<TId>` or `AuditableEntity<TId>`
    1. DTO which inherits `Dto<TModel, TId>` and contains the same properites ad TModel
    1. DTO validator which inherits `DtoValidator<TDto, Dto>` where Dto is the DTO made previously
    1. Model configuration which inherits `IEntityTypeConfiguration<TModel>`
    1. **Optional:** If you want to be able to create the model via the API then a Create command which inherits the DTO made previously and `ICreateCommand<TModel, TId>`
    1. **Optional:** If you want to be able to update (PUT) the model via the API then an Update Command which inherits the DTO made previously and `IUpdateCommand<TModel, TId`
    1. A controller which inherits either `CrudController<TModel, TId, TCreateComman, TUpdateCommand>` **OR** `ReadOnlyController<TModel, TId>`
 3. Make a DbContext which inherits either `AuditingDbContext` **OR** `DbContext`
 4. In the ConfigureServices method of Startup.cs use `services.AddWebApiServices(ApiVersion)` to add all of the required services. Also add your DbContext to the services here.
 5. In the Configure method of Startup.cs add the following
 
 ```C#
      app.UseEndpoints(endpoints =>
            {
                endpoints.EnableDependencyInjection();
                endpoints.MapControllers();
                endpoints.AddOdata("query", GetEdmModel(), MaxTop);
            });

            app.AddSwagger(ApiVersion);
 ```
 6. In Startup.cs add the following method
 
 ```C#
 IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();

            //Duplicate this line for each entity
            odataBuilder.EntitySet<TModel>(nameof(TModel));

            return odataBuilder.GetEdmModel();
        }
 ```
 
 ## Adding endpoints
 WebAppUtilities utilises MediatR to maintain SOLID principles so it is recomended that for each new endpoint a IRequest and IRequest hander is made and then in the controller you give the desired request to the mediatr.
 
 To add new endpoints for a model add a new method to the CrudController/ReadOnlyController, decorate it with the desired HTTP method and send the Mediatr the request you made for the endpoint.
 
 To add non entity related endpoints make a new controller which inherits ApiController. This will give to access to the Mediatr to send your request to. 
 
 ## Endpoints
 API enpoints are avaliable at /api/<Model>/{id}
 The Swagger UI is avaliable at `/swagger`
 Odata querying is avaliable at /query/<Model>?$<query string>
 
 ## Other features
 IClock an abstraction around DateTime.Now which is by default bound to the Ioc container at start up and can be injected where needed and mocked in tests.
 
