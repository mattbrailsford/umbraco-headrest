using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Our.Umbraco.HeadRest.Web.Routing
{
    internal class UmbracoRoutesConstraint : IRouteConstraint
    {
        private readonly string[] UmbracoReservedPaths = new[]
        {
            "^umbraco/.*",
            "^media/.*"
        };

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return UmbracoReservedPaths.All(x => !Regex.IsMatch(("" + values["path"]), x));
        }
    }
}