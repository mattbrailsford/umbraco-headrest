using Umbraco.Cms.Core.Mapping;

namespace Our.Umbraco.HeadRest.Mapping
{
    public class HeadRestMapDefinition : IMapDefinition
    {
        public void DefineMaps(IUmbracoMapper mapper)
        {
            mapper.Define<HeadRestOptions, HeadRestConfig>(
                (src, ctx) => new HeadRestConfig(),
                (src, dst, ctx) =>
                {
                    dst.Mode = src.Mode;
                    dst.ControllerType = src.ControllerType;
                    dst.Mapper = src.Mapper;
                    dst.ViewModelMappings = src.ViewModelMappings;
                    dst.CustomRouteMappings = src.CustomRouteMappings;
                });
        }
    }
}
