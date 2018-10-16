using AutoMapper;
using System;
using System.Web.Routing;
using Umbraco.Web;
using Umbraco.Core;
using Our.Umbraco.HeadRest.Web.Routing;
using Our.Umbraco.HeadRest.Web.Controllers;
using System.Collections.Concurrent;

namespace Our.Umbraco.HeadRest
{
    public static class HeadRest
    {
        internal static ConcurrentDictionary<string, HeadRestConfig> Configs = new ConcurrentDictionary<string, HeadRestConfig>();

        public static void ConfigureEndpoint()
        {
            ConfigureEndpoint(new HeadRestOptions());
        }

        public static void ConfigureEndpoint(HeadRestOptions options)
        {
            ConfigureEndpoint("/", "/root/*[@isDoc][1]", options);
        }

        public static void ConfigureEndpoint(string basePath)
        {
            ConfigureEndpoint(basePath, "/root/*[@isDoc][1]", new HeadRestOptions());
        }

        public static void ConfigureEndpoint(string basePath, HeadRestOptions options)
        {
            ConfigureEndpoint(basePath, "/root/*[@isDoc][1]", options);
        }

        public static void ConfigureEndpoint(string basePath, string rootNodeXPath)
        {
            ConfigureEndpoint(basePath, rootNodeXPath, new HeadRestOptions());
        }

        public static void ConfigureEndpoint(string basePath, string rootNodeXPath, HeadRestOptions options)
        {
            var config = Mapper.Map<HeadRestConfig>(options);
            config.BasePath = basePath;
            config.RootNodeXPath = rootNodeXPath;
            ConfigureEndpoint(config);
        }

        private static void ConfigureEndpoint(HeadRestConfig config)
        {
            ValidateConfig(config);

            if (!Configs.ContainsKey(config.BasePath))
            {
                if (Configs.TryAdd(config.BasePath, config))
                {
                    RouteTable.Routes.MapUmbracoRoute(
                        $"HeadRest_{config.BasePath.Trim('/').Replace("/", "_")}",
                        config.BasePath.EnsureEndsWith("/").TrimStart("/") + "{*path}",
                        new
                        {
                            controller = config.ControllerType.Name.TrimEnd("Controller"),
                            action = "Index",
                            headRestConfig = config
                        },
                        new HeadRestRouteHandler(config));
                }
            }
        }

        private static void ValidateConfig(HeadRestConfig config)
        {
            if (!typeof(HeadRestController).IsAssignableFrom(config.ControllerType))
            {
                throw new Exception("Supplied controller type must inherit from HeadRestController");
            }
        }
    }
}