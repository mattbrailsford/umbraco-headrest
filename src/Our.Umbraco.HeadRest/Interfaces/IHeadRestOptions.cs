using System;
using Our.Umbraco.HeadRest.Web.Mapping;
using Our.Umbraco.HeadRest.Web.Routing;

namespace Our.Umbraco.HeadRest.Interfaces
{
    public interface IHeadRestOptions
    {
        HeadRestEndpointMode Mode { get; }
        Type ControllerType { get; }
        Func<HeadRestMappingContext, object> Mapper { get; }
        HeadRestViewModelMap ViewModelMappings { get; }
        HeadRestRouteMap CustomRouteMappings { get; }
    }
}
