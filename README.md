
# WebApiUtilities
 
 WebApiUtilities is a library that contains tools to facilitate the rapid development of RESTful ASP .Net Core 3.1 Web APIs utilising Entity Framework Core, Identity Server 4, JWT style authentication, Odata and Swagger.
 
 
 ## Getting set up
 
 1. Make a new ASP .Net Core 3.1 Web API project and install the WebApiUtilities nuget package.
 2. For each domain model you wish to expose in the API you need to add the following classes
    1. Model which inherits from `Entity<TId>` **OR** `AuditableEntity<TId>`
    1. DTO which inherits from `Dto<TModel, TId>` and contains the same properties as TModel
    1. A service interface which implemets `IRecordService<TModel, TId>`
    1. A concrete service which inherits from `BaseRecordService<TDbContext, TModel, TId>` and inherits the service interface that you made previously.
    1. A controller which inherits from `RecordController<TModel, TId, TDto>`
 3. Make a DbContext which inherits either `AuditingDbContext` **OR** `IdentityDbContext`
 4. In the ConfigureServices method of Startup.cs configure your DbContext, then add the following line. Also register the services that you made,
 
```C#
services.AddWebApiServices<TDbContext>(ApiTitle);
```
 5. In the Configure method of Startup.cs add IUserService to its signature, remove all of its contents except the use developer exception page if statment and add web api utitities. It should look like the following.
 
 ```C#
public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IUserService userService)
{
    if (env.IsDevelopment())
        app.UseDeveloperExceptionPage();

    app.AddWebApiUtilities(GetEdmModel(), userService, {ApiTitle});
}
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
To add new endpoints for a model add a new method to the service interface then implement it in the concrete service. Then expose this methof in the record controller with the appropriate http verb and path.

 ## App settings
 Add a AppSettings section to appsettings.json then add the following keys and values to it.
 - `JWTKey` - a string of characters which will be used to sign Json Web Tokens
 - `SystemUserName` - the name of the system user for your api service
 - `SystemUserPassword` - the password of the system user
 - `SystemUserEmail` - the email of the system user
 
 The system user name, password and email can be used to access the api via postman
  
 ## Endpoints
 * API endpoints are available at `/api/<TModel>/{id?}`
 * The Swagger UI is available at `/swagger`
 * Odata querying is available at `/api/<TModel>?$<query string>`
 * The identity server endpoint description is available at `/.well-known/openid-configuration`
 * A user registration endpoint is available at `/api/user/register`
 
 ## Other features
 ITimeService an abstraction around DateTime which is by default bound to the Ioc container at startup and can be injected where needed and mocked in tests.
 
 ## Todo:
 - Add entity configurations
 - Add model validations
 - Add RBAC authorization
