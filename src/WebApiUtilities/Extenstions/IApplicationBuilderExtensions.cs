using Microsoft.AspNetCore.Builder;
using Microsoft.OData.Edm;

namespace WebApiUtilities.Extenstions
{
    public static class IApplicationBuilderExtension
    {
        public static void AddSwagger(this IApplicationBuilder app, int apiVersion)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v{apiVersion}/swagger.json", $"My API V{apiVersion}");
            });
        }

        public static void AddWebApiUtilities(this IApplicationBuilder app, IEdmModel edmModel, int maxTop, int apiVersion)
        {
            app.UseEndpoints(endpoints => endpoints.AddOdata(edmModel, maxTop));

            app.AddSwagger(apiVersion);
        }
    }
}
