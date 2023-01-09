using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NPoco;
using Our.Umbraco.HeadRest.Interfaces;
using Our.Umbraco.HeadRest.Web.Controllers;
using Our.Umbraco.HeadRest.Web.Mapping;
using Our.Umbraco.HeadRest.Web.Routing;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Extensions;

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
            Mapper = (ctx) => 
            {
                var mapper = ctx.HttpContext.RequestServices.GetRequiredService<IUmbracoMapper>();

                // Currently have to call the UmbracoMapper.Map function
                // via reflection as there are no non-generic versions
                // available, and as we only know types at runtime, this
                // is the only way we can call them. We should keep an eye
                // on https://github.com/umbraco/Umbraco-CMS/issues/6250
                // and if this ever changes, we should switch to use the 
                // map methods instead
                var mapFunc = mapper.GetType().GetMethods()
                    .First(m => m.Name == nameof(mapper.Map)
                        && m.GetGenericArguments().Count() == 1
                        && m.GetParameters()
                            .Select(p => p.ParameterType)
                            .SequenceEqual(new[] {
                                typeof(object),
                                typeof(Action<MapperContext>)
                            })
                    )
                    .MakeGenericMethod(ctx.ViewModelType);

                var ctxAction = new Action<MapperContext>(x => x.SetHeadRestMappingContext(ctx));

                return mapFunc.Invoke(mapper, new object[] { ctx.Content, ctxAction });
            };
        }
    }
}
