using Our.Umbraco.HeadRest.Web.Routing;
using System;
using System.Collections.Generic;
using System.Web;

namespace Our.Umbraco.HeadRest.Web.Extensions
{
    public static class HttpRequestBaseExtensions
    {
        public static string HeadRestRouteParam(this HttpRequestBase request, int index, string defaultValue = null)
        {
            var routeParams = request.HeadRestRouteParams();
            return routeParams?[index];
        }

        public static TType HeadRestRouteParam<TType>(this HttpRequestBase request, int index, TType defaultValue = default(TType))
        {
            var param = request.HeadRestRouteParam(index);
            if (param == null || string.IsNullOrWhiteSpace(param.ToString()))
                return defaultValue;

            var val = (TType)Convert.ChangeType(param, typeof(TType));
            return val = !EqualityComparer<TType>.Default.Equals(val, default(TType))
                ? val
                : defaultValue;
        }

        public static string HeadRestRouteParam(this HttpRequestBase request, string name, string defaultValue = null)
        {
            var routeParams = request.HeadRestRouteParams();
            return routeParams?[name];
        }

        public static TType HeadRestRouteParam<TType>(this HttpRequestBase request, string name, TType defaultValue = default(TType))
        {
            var param = request.HeadRestRouteParam(name);
            if (param == null || string.IsNullOrWhiteSpace(param.ToString()))
                return defaultValue;

            var val = (TType)Convert.ChangeType(param, typeof(TType));
            return val = !EqualityComparer<TType>.Default.Equals(val, default(TType))
                ? val
                : defaultValue;
        }

        private static HeadRestRouteParamsCollection HeadRestRouteParams(this HttpRequestBase request)
        {
            if (request.RequestContext?.RouteData?.Values == null)
                return null;

            if (!request.RequestContext.RouteData.Values.ContainsKey(HeadRest.RouteMapMatchKey))
                return null;

            var routeMap = request.RequestContext.RouteData.Values[HeadRest.RouteMapMatchKey] as HeadRestRouteMapMatch;

            return routeMap?.Params;
        }
    }
}
