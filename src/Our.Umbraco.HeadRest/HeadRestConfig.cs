using Our.Umbraco.HeadRest.Interfaces;

namespace Our.Umbraco.HeadRest
{
    internal class HeadRestConfig : HeadRestOptions, IHeadRestConfig
    {
        public HeadRestConfig() { }

        public HeadRestConfig(HeadRestOptions options) : base()
        {
            Mode = options.Mode;
            ControllerType = options.ControllerType;
            Mapper = options.Mapper;
            ViewModelMappings = options.ViewModelMappings;
            CustomRouteMappings = options.CustomRouteMappings;
        }

        public string BasePath { get; set; }
        public string RootNodeXPath { get; set; }
    }
}
