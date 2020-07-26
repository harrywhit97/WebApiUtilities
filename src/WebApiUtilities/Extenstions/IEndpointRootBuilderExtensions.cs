using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.OData.Edm;

namespace WebApiUtilities.Extenstions
{
    public static class IEndpointRootBuilderExtensions
    {
        public static void AddOdata(this IEndpointRouteBuilder builder, IEdmModel EdmModel, int maxTop, string odataRoutePrefix = "api")
        {
            builder.EnableDependencyInjection();
            builder.MapControllers();
            builder.Select().Filter().OrderBy().Count().MaxTop(maxTop).Expand();
            builder.MapODataRoute(odataRoutePrefix, odataRoutePrefix, EdmModel);
        }
    }
}
