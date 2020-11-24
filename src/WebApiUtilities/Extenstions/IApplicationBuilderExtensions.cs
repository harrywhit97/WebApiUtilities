using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.OData.Edm;
using WebApiUtilities.Identity;

namespace WebApiUtilities.Extenstions
{
    public static class IApplicationBuilderExtension
    {
        public static void AddSwagger(this IApplicationBuilder app, string apiName, int apiVersion = 1)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v{apiVersion}/swagger.json", $"{apiName} V{apiVersion}");
            });
        }

        public static void AddWebApiUtilities(this IApplicationBuilder app, IEdmModel edmModel, IUserService userService, string apiName, int maxTop = 10, int apiVersion = 1)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.EnableDependencyInjection();
                endpoints.MapControllers();
                endpoints.Select().Filter().OrderBy().Count().MaxTop(maxTop).Expand();
                endpoints.MapODataRoute("api", "api", edmModel);
            });
            app.AddSwagger(apiName, apiVersion);
            userService.EnsureSystemUserExists().Wait();
        }
    }
}
