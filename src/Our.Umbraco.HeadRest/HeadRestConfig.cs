using Our.Umbraco.HeadRest.Interfaces;

namespace Our.Umbraco.HeadRest
{
    internal class HeadRestConfig : HeadRestOptions, IHeadRestConfig
    {
        public string BasePath { get; set; }
        public string RootNodeXPath { get; set; }
    }
}
