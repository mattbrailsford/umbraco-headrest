using System;
using System.Web.Routing;
using System.Collections.Concurrent;
using Umbraco.Web;
using Umbraco.Core;
using Our.Umbraco.HeadRest.Web.Routing;
using Our.Umbraco.HeadRest.Web.Controllers;
using Umbraco.Core.Mapping;

namespace Our.Umbraco.HeadRest
{
    public class HeadRest
    {
        internal static string RoutePathKey = "path";
        internal static string ControllerConfigKey = "headRestConfig";
        internal static string RouteMapMatchKey = "HeadRestRouteMapMatch";
        internal static string MappingContextKey = "HeadRestMappingContext";

        internal static ConcurrentDictionary<string, HeadRestConfig> Configs = new ConcurrentDictionary<string, HeadRestConfig>();

        private readonly UmbracoMapper _mapper;

        public HeadRest(UmbracoMapper mapper) 
            => _mapper = mapper;

        public void ConfigureEndpoint(HeadRestOptions options)
        {
            ConfigureEndpoint("/", "/root/*[@isDoc][1]", options);
        }

        public void ConfigureEndpoint(string basePath, HeadRestOptions options)
        {
            ConfigureEndpoint(basePath, "/root/*[@isDoc][1]", options);
        }

        public void ConfigureEndpoint(string basePath, string rootNodeXPath, HeadRestOptions options)
        {
            var config = _mapper.Map<HeadRestConfig>(options);
            config.BasePath = basePath;
            config.RootNodeXPath = rootNodeXPath;
            ConfigureEndpoint(config);
        }

        private void ConfigureEndpoint(HeadRestConfig config)
        {
            ValidateConfig(config);

            if (!Configs.ContainsKey(config.BasePath))
            {
                if (Configs.TryAdd(config.BasePath, config))
                {
                    RouteTable.Routes.MapUmbracoRoute(
                        $"HeadRest_{config.BasePath.Trim('/').Replace("/", "_")}",
                        config.BasePath.EnsureEndsWith("/").TrimStart("/") + "{*"+ RoutePathKey + "}",
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

            if (config.ViewModelMappings == null)
            {
                throw new Exception("ViewModelMappings can not be null");
            }
        }
    }
}