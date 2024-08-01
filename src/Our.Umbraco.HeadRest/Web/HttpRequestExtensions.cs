using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Our.Umbraco.HeadRest.Web.Routing;

namespace Our.Umbraco.HeadRest.Web
{
    public static class HttpRequestExtensions
    {
        public static string HeadRestRouteParam(this HttpRequest request, int index, string defaultValue = null)
        {
            var routeParams = request.HeadRestRouteParams();
            return routeParams?[index];
        }

        public static TType HeadRestRouteParam<TType>(this HttpRequest request, int index, TType defaultValue = default(TType))
        {
            var param = request.HeadRestRouteParam(index);
            if (param == null || string.IsNullOrWhiteSpace(param.ToString()))
                return defaultValue;

            var val = (TType)Convert.ChangeType(param, typeof(TType));
            return val = !EqualityComparer<TType>.Default.Equals(val, default(TType))
                ? val
                : defaultValue;
        }

        public static string HeadRestRouteParam(this HttpRequest request, string name, string defaultValue = null)
        {
            var routeParams = request.HeadRestRouteParams();
            return routeParams?[name];
        }

        public static TType HeadRestRouteParam<TType>(this HttpRequest request, string name, TType defaultValue = default(TType))
        {
            var param = request.HeadRestRouteParam(name);
            if (param == null || string.IsNullOrWhiteSpace(param.ToString()))
                return defaultValue;

            var val = (TType)Convert.ChangeType(param, typeof(TType));
            return val = !EqualityComparer<TType>.Default.Equals(val, default(TType))
                ? val
                : defaultValue;
        }

        private static HeadRestRouteParamsCollection HeadRestRouteParams(this HttpRequest request)
        {
            var routeValues = request.HttpContext.Request.RouteValues;
            if (routeValues?.Values == null)
                return null;

            if (!routeValues.ContainsKey(HeadRest.RouteMapMatchKey))
                return null;

            var routeMap = routeValues[HeadRest.RouteMapMatchKey] as HeadRestRouteMapMatch;

            return routeMap?.Params;
        }
    }
}
