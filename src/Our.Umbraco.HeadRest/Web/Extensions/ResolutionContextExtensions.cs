using AutoMapper;
using Our.Umbraco.HeadRest.Web.Mapping;

namespace Our.Umbraco.HeadRest.Web.Extensions
{
    public static class ResolutionContextExtensions
    {
        public static HeadRestMappingContext GetHeadRestMappingContext(this ResolutionContext context)
        {
            return context?.Options?.Items?[HeadRest.MappingContextKey] as HeadRestMappingContext;
        }
    }
}
