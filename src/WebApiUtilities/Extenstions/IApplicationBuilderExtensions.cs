using Microsoft.AspNetCore.Builder;
using Microsoft.OData.Edm;

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

        public static void AddWebApiUtilities(this IApplicationBuilder app, IEdmModel edmModel, string apiName, int maxTop = 10, int apiVersion = 1)
        {
            app.UseEndpoints(endpoints => endpoints.AddOdata(edmModel, maxTop));
            app.AddSwagger(apiName, apiVersion);
            app.UseAuthentication();
        }
    }
}
