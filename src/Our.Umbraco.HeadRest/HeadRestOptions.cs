using System;
using Our.Umbraco.HeadRest.Interfaces;
using Our.Umbraco.HeadRest.Web.Controllers;
using Our.Umbraco.HeadRest.Web.Mapping;
using Our.Umbraco.HeadRest.Web.Routing;

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
            Mapper = (ctx) => AutoMapper.Mapper.Map(ctx.Content, ctx.ContentType, ctx.ViewModelType, opts =>
            {
                opts.Items[HeadRest.MappingContextKey] = ctx;
            });
		}
    }
}
