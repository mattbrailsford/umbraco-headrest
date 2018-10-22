using Our.Umbraco.HeadRest.Web.Routing;
using System.Web;

namespace Our.Umbraco.HeadRest.Web.Extensions
{
    public static class HttpRequestBaseExtensions
    {
        public static string HeadRestRouteParam(this HttpRequestBase request, int index)
        {
            var routeParams = request.HeadRestRouteParams();
            return routeParams?[index];
        }

        public static string HeadRestRouteParam(this HttpRequestBase request, string name)
        {
            var routeParams = request.HeadRestRouteParams();
            return routeParams?[name];
        }

        private static HeadRestRouteParamsCollection HeadRestRouteParams(this HttpRequestBase request)
        {
            if (request.RequestContext?.RouteData?.Values == null)
                return null;

            if (!request.RequestContext.RouteData.Values.ContainsKey("HeadRestRouteMapMatch"))
                return null;

            var routeMap = request.RequestContext.RouteData.Values["HeadRestRouteMapMatch"] as HeadRestRouteMapMatch;

            return routeMap?.Params;
        }
    }
}
