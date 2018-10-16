namespace Our.Umbraco.HeadRest.Interfaces
{
    internal interface IHeadRestConfig : IHeadRestOptions
    {
        string BasePath { get; }
        string RootNodeXPath { get; }
    }
}
