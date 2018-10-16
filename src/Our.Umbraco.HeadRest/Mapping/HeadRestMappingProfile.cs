using AutoMapper;

namespace Our.Umbraco.HeadRest.Mapping
{
    internal class HeadRestMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<HeadRestOptions, HeadRestConfig>();
        }
    }
}
