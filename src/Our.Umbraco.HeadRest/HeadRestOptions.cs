using System;
using Our.Umbraco.HeadRest.Interfaces;
using Our.Umbraco.HeadRest.Web.Controllers;
using Our.Umbraco.HeadRest.Web.Extensions;
using Our.Umbraco.HeadRest.Web.Mapping;
using Our.Umbraco.HeadRest.Web.Routing;
using Umbraco.Core.Composing;
using Umbraco.Core.Mapping;

namespace Our.Umbraco.HeadRest
{
    public class HeadRestOptions : IHeadRestOptions
    {
        public HeadRestEndpointMode Mode { get; set; }
        public Type ControllerType { get; set; }
        public Func<HeadRestMappingContext, object> Mapper { get; set; }
        public HeadRestViewModelMap ViewModelMappings { get; set; }
        public HeadRestRouteMap CustomRouteMappings { get; set; }

        public HeadRestOptions()
        {
            Mode = HeadRestEndpointMode.Dedicated;
            ControllerType = typeof(HeadRestController);
            Mapper = (ctx) => {
                
                var mapFunc = typeof(UmbracoMapper)
                    .GetMethod("Map")
                    .MakeGenericMethod(ctx.ViewModelType);

                var ctxAction = new Action<MapperContext>(x => x.SetHeadRestMappingContext(ctx));

                return mapFunc.Invoke(Current.Mapper, new object[] { ctx.Content, ctxAction });
            };
        }
    }
}
