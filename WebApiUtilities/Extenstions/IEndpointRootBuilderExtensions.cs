using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.OData.Edm;

namespace WebApiUtilities.Extenstions
{
    public static class IEndpointRootBuilderExtensions
    {
        public static void AddOdata(this IEndpointRouteBuilder builder, string odataRoutePrefix, IEdmModel EdmModel, int maxTop)
        {
            builder.Select().Filter().OrderBy().Count().MaxTop(maxTop).Expand();
            builder.MapODataRoute(odataRoutePrefix, odataRoutePrefix, EdmModel);
        }
    }
}
