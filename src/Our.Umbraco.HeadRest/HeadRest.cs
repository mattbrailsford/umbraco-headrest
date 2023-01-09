using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Our.Umbraco.HeadRest.Interfaces;
using Our.Umbraco.HeadRest.Web.Controllers;
using Our.Umbraco.HeadRest.Web.Models;
using Our.Umbraco.HeadRest.Web.Routing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using Umbraco.Extensions;

namespace Our.Umbraco.HeadRest
{
    public class HeadRest
    {
        internal static string RoutePathKey = "path";
        internal static string ControllerConfigKey = "headRestConfig";
        internal static string RouteMapMatchKey = "HeadRestRouteMapMatch";
        internal static string MappingContextKey = "HeadRestMappingContext";

        internal static ConcurrentDictionary<string, HeadRestConfig> Configs = new ConcurrentDictionary<string, HeadRestConfig>();

        public void ConfigureEndpoint(IUmbracoBuilder builder, HeadRestOptions options)
        {
            ConfigureEndpoint("/", "/root/*[@isDoc][1]", builder, options);
        }

        public void ConfigureEndpoint(string basePath, IUmbracoBuilder builder, HeadRestOptions options)
        {
            ConfigureEndpoint(basePath, "/root/*[@isDoc][1]", builder, options);
        }

        public void ConfigureEndpoint(string basePath, string rootNodeXPath, IUmbracoBuilder builder, HeadRestOptions options)
        {
            var config = new HeadRestConfig(options);
            config.BasePath = basePath;
            config.RootNodeXPath = rootNodeXPath;
            ConfigureEndpoint(builder, config);
        }

        private void ConfigureEndpoint(IUmbracoBuilder builder, HeadRestConfig config)
        {
            ValidateConfig(config);

            if (!Configs.ContainsKey(config.BasePath))
            {
                if (Configs.TryAdd(config.BasePath, config))
                {
                    // from https://docs.umbraco.com/umbraco-cms/reference/routing/custom-routes
                    builder.Services.Configure<UmbracoPipelineOptions>(options =>
                    {
                        var controllerName = config.ControllerType.Name;
                        options.AddFilter(new UmbracoPipelineFilter(controllerName)
                        {
                            Endpoints = app => app.UseEndpoints(endpoints =>
                            {
                                endpoints.MapControllerRoute(
                                    $"HeadRest_{config.BasePath.Trim('/').Replace("/", "_")}",
                                    config.BasePath.EnsureEndsWith("/").TrimStart("/") + "{*" + RoutePathKey + "}",
                                    new
                                    {
                                        controller = config.ControllerType.Name.TrimEnd("Controller"),
                                        action = "Index",
                                        headRestConfig = config
                                    },
                                    constraints: new { path = new UmbracoRoutesConstraint() }
                                ).ForUmbracoPage(FindContent);
                            })
                        });
                    });
                }
            }
        }

        private static void ValidateConfig(HeadRestConfig config)
        {
            if (!typeof(HeadRestController).IsAssignableFrom(config.ControllerType))
            {
                throw new Exception("Supplied controller type must inherit from HeadRestController");
            }

            if (config.ViewModelMappings == null)
            {
                throw new Exception("ViewModelMappings can not be null");
            }
        }

        private static IPublishedContent FindContent(ActionExecutingContext actionExecutingContext)
        {
            var config = actionExecutingContext.RouteData.Values[HeadRest.ControllerConfigKey] as IHeadRestConfig;

            var nodeXPath = config.RootNodeXPath;

            if (actionExecutingContext.RouteData?.Values != null)
            {
                if (actionExecutingContext.RouteData.Values.ContainsKey(HeadRest.RoutePathKey)
                    && actionExecutingContext.RouteData.Values[HeadRest.RoutePathKey] != null)
                {
                    var path = actionExecutingContext.RouteData.Values[HeadRest.RoutePathKey].ToString();

                    // Check for a configured custom route
                    if (config.CustomRouteMappings != null)
                    {
                        var match = config.CustomRouteMappings.GetRouteMapFor(path);
                        if (match != null)
                        {
                            path = match.Target;

                            actionExecutingContext.RouteData.Values.Add(HeadRest.RouteMapMatchKey, match);
                        }
                    }

                    // Construct xpath from path
                    var pathParts = path.Trim('/').Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var pathPart in pathParts)
                    {
                        nodeXPath += $"/*[@urlName='{pathPart}'][1]";
                    }
                }
            }

            var ctx = actionExecutingContext.HttpContext.RequestServices.GetRequiredService<IUmbracoContextAccessor>();

            var node = ctx.GetRequiredUmbracoContext().Content.GetSingleByXPath(nodeXPath);

            return node ?? new NotFoundPublishedContent();
        }
    }
}