using Microsoft.AspNetCore.Builder;

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
    }
}
