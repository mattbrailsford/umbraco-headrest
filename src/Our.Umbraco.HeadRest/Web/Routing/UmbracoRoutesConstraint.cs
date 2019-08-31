using System.Web.Routing;
using System.Web;
using System.Linq;
using System.Text.RegularExpressions;

namespace Our.Umbraco.HeadRest.Web.Routing
{
    internal class UmbracoRoutesConstraint : IRouteConstraint
    {
        private readonly string[] UmbracoReservedPaths = new[]
        {
            "^umbraco/.*",
            "^media/.*"
        };

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return UmbracoReservedPaths.All(x => !Regex.IsMatch(("" + values["path"]), x));
        }
    }
}