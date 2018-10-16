using System;
using Our.Umbraco.HeadRest.Web.Mapping;

namespace Our.Umbraco.HeadRest.Interfaces
{
    public interface IHeadRestOptions
    {
        Type ControllerType { get; }
        Func<HeadRestMappingContext, object> Mapper { get; }
        string RoutesListPath { get; }
    }
}
