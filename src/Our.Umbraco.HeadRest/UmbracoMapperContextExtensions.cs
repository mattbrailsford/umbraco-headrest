using Our.Umbraco.HeadRest.Web.Mapping;
using Umbraco.Cms.Core.Mapping;

namespace Our.Umbraco.HeadRest
{
    public static class MapperContextExtensions
    {
        public static void SetHeadRestMappingContext(this MapperContext context, HeadRestMappingContext headRestContext)
        {
            context.Items[HeadRest.MappingContextKey] = headRestContext;
        }

        public static HeadRestMappingContext GetHeadRestMappingContext(this MapperContext context)
        {
            return context.HasItems 
                && context.Items.TryGetValue(HeadRest.MappingContextKey, out var obj) 
                && obj is HeadRestMappingContext ctx
                    ? ctx
                    : null;
        }
    }
}
